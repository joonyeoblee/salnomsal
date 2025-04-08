using Equipment;

namespace Team
{
    public class TeamSlot : CharacterSlot
    {
        protected override string SaveKey => $"TeamSlot_{transform.GetSiblingIndex()}";
    }
}
