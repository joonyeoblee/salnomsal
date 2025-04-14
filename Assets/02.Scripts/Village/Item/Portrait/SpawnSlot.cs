using Equipment;

namespace Spawn
{
    public class SpawnSlot : CharacterSlot
    {
        public override string SaveKey => $"Spawn_{transform.GetSiblingIndex()}";

        public override void DeleteItem()
        {
            Destroy(currentCharacterPortrait);
            base.DeleteItem();
        }
    }
}
