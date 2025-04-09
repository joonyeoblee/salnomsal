Shader "Custom/2D_Dissolve_FromTop"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "gray" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.0
        _EdgeColor ("Edge Color", Color) = (1, 0.5, 0, 1)
        _EdgeWidth ("Edge Width", Range(0.001, 0.5)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _DissolveAmount;
            float4 _EdgeColor;
            float _EdgeWidth;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.screenPos = o.vertex.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 noiseUV = i.uv * 4.0; // 노이즈 세기 조절
                float noise = tex2D(_NoiseTex, noiseUV).r;

                // dissolve 기준: y값 기준으로 사라지기
                float yFactor = 1.0 - i.uv.y;
                float dissolve = noise + yFactor;

                float4 texColor = tex2D(_MainTex, i.uv);

                if (dissolve < _DissolveAmount)
                    discard;

                // Edge 처리
                float edge = smoothstep(_DissolveAmount, _DissolveAmount + _EdgeWidth, dissolve);
                float4 edgeCol = lerp(_EdgeColor, texColor, edge);

                edgeCol.a *= texColor.a; // 알파 유지
                return edgeCol;
            }
            ENDCG
        }
    }
}
