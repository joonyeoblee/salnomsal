using Equipment;

namespace Team
{
    public class TeamSlot : CharacterSlot
    {

        void Start()
        {
            GameManager.Instance.TeamSlots.Add(this);
        }
        public override string SaveKey => $"TeamSlot_{transform.GetSiblingIndex()}";
     
    }
}
