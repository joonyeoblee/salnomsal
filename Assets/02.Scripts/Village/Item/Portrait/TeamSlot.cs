using Equipment;

namespace Team
{
    public class TeamSlot : CharacterSlot
    {
        public override string SaveKey => $"TeamSlot_{transform.GetSiblingIndex()}";
     
    }
}
