using Equipment.RefactoringSlot;

namespace Team
{
    public class TeamSlot : CharacterSlotR
    {

        // protected override void Start()
        // {
        //     base.Start();
        //     if (currentCharacterPortrait == null) return;
        //     GameManager.Instance.TeamSlots[transform.GetSiblingIndex()] = this;
        //     GameManager.Instance.Characters[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>().portrait.Character;
        //     GameManager.Instance.PortraitItems[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>();
        // }
        //
        // protected override void Save()
        // {
        //     base.Save();
        //     GameManager.Instance.TeamSlots[transform.GetSiblingIndex()] = this;
        //     GameManager.Instance.Characters[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>().portrait.Character;
        //     GameManager.Instance.PortraitItems[transform.GetSiblingIndex()] = currentCharacterPortrait.GetComponent<PortraitItem>();
        // }
        // public override void DeleteItem()
        // {
        //     GameManager.Instance.PortraitItems[transform.GetSiblingIndex()] = null;
        //     base.DeleteItem();
        // }
        //
        // public override string SaveKey => $"TeamSlot_{transform.GetSiblingIndex()}";
     
    }
}
