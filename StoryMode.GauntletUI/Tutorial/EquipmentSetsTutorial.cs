using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200001A RID: 26
	[Tutorial("EquipmentSets")]
	public class EquipmentSetsTutorial : TutorialItemBase
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00002ECD File Offset: 0x000010CD
		public EquipmentSetsTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "EquipmentSetFilters";
			base.MouseRequired = true;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002EEE File Offset: 0x000010EE
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerFilteredToDifferentEquipment;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002EF6 File Offset: 0x000010F6
		public override void OnInventoryEquipmentTypeChange(InventoryEquipmentTypeChangedEvent obj)
		{
			this._playerFilteredToDifferentEquipment = !obj.IsCurrentlyWarSet;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002F07 File Offset: 0x00001107
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.InventoryScreen;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002F0A File Offset: 0x0000110A
		public override bool IsConditionsMetForActivation()
		{
			return TutorialPhase.Instance.IsCompleted && TutorialHelper.CurrentContext == TutorialContexts.InventoryScreen;
		}

		// Token: 0x04000020 RID: 32
		private bool _playerFilteredToDifferentEquipment;
	}
}
