using Equipment;

namespace Spawn
{
    public class SpawnSlot : CharacterSlot
    {
        public override string SaveKey => $"Spawn_{transform.GetSiblingIndex()}";
        
    }
}
