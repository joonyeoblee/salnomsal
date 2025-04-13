using Equipment;

namespace Team
{
    public class TeamSlot : CharacterSlot
    {

        protected override void Start()
        {
            base.Start();
            GameManager.Instance.TeamSlots[transform.GetSiblingIndex()] = this;
        }
        public override string SaveKey => $"TeamSlot_{transform.GetSiblingIndex()}";
     
    }
}
