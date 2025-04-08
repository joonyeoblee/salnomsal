using Equipment;

namespace Spawn
{
    public class SpawnSlot : CharacterSlot
    {
        protected override string SaveKey => $"Spawn_{transform.GetSiblingIndex()}";
    }
}
