using MoreMountains.Feedbacks;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public enum FloatingTextType
{
    Damage,
    CriticalDamage,
    Heal,
    Buff,
    Debuff,

    Count
}

public class FloatingTextDisplay : MonoBehaviour
{
    public static FloatingTextDisplay Instance;

    private MMF_Player _mmfPlayer;
    private List<Color> _textColors = new List<Color>((int)FloatingTextType.Count);
    private Gradient _gradient = new Gradient();
    private GradientColorKey[] _colorKey = new GradientColorKey[2];
    private GradientAlphaKey[] _alphaKey = new GradientAlphaKey[2];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _mmfPlayer = GetComponent<MMF_Player>();

        for (int i = 0; i < (int)FloatingTextType.Count; ++i)
        {
            _textColors.Add(Color.white);
        }
        _textColors[(int)FloatingTextType.Damage] = Color.yellow;
        _textColors[(int)FloatingTextType.CriticalDamage] = Color.red;
        _textColors[(int)FloatingTextType.Heal] = Color.green;
        _textColors[(int)FloatingTextType.Buff] = Color.blue;
        _textColors[(int)FloatingTextType.Debuff] = Color.magenta;
    }

    public void ShowFloatingText(Vector3 position, string text, FloatingTextType type)
    {
        if (_mmfPlayer == null) return;

        MMF_FloatingText floatingText = _mmfPlayer.GetFeedbackOfType<MMF_FloatingText>();

        floatingText.Value = text;

        _colorKey[0].color = _textColors[(int)type];
        _colorKey[0].time = 0.0f;
        _colorKey[1].color = _textColors[(int)type];
        _colorKey[1].time = 1.0f;

        _alphaKey[0].alpha = 1.0f;
        _alphaKey[0].time = 0.0f;
        _alphaKey[1].alpha = 0.0f;
        _alphaKey[1].time = 1.0f;

        _gradient.SetKeys(_colorKey, _alphaKey);

        floatingText.ForceColor = true;
        floatingText.AnimateColorGradient = _gradient;

        _mmfPlayer.PlayFeedbacks(position);
    }
}
