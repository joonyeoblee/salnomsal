using Equipment;
using Portrait;

namespace Team
{
    public class TeamSlot : CharacterSlot
    {

        protected override void Start()
        {
            base.Start();
            if (currentCharacterPortrait == null) return;
            GameManager.Instance.TeamSlots[transform.GetSiblingIndex()] = this;
            GameManager.Instance.Characters[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>().portrait.Character;
            GameManager.Instance.PortraitItems[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>();
        }

        protected override void Save()
        {
            base.Save();
            GameManager.Instance.TeamSlots[transform.GetSiblingIndex()] = this;
            GameManager.Instance.Characters[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>().portrait.Character;
            GameManager.Instance.PortraitItems[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>();
        }

        public override string SaveKey => $"TeamSlot_{transform.GetSiblingIndex()}";
     
    }
}
