using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000045 RID: 69
	[Tutorial("InventoryBannerItemTutorial")]
	public class InventoryBannerItemTutorial : TutorialItemBase
	{
		// Token: 0x0600014A RID: 330 RVA: 0x000046F5 File Offset: 0x000028F5
		public InventoryBannerItemTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Center;
			base.HighlightedVisualElementID = "InventoryOtherBannerItems";
			base.MouseRequired = false;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00004716 File Offset: 0x00002916
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.InventoryScreen;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000471C File Offset: 0x0000291C
		public override void OnInventoryItemInspected(InventoryItemInspectedEvent obj)
		{
			if (obj.Item.EquipmentElement.Item.IsBannerItem && obj.ItemSide == InventoryLogic.InventorySide.OtherInventory)
			{
				this._inspectedOtherBannerItem = true;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00004755 File Offset: 0x00002955
		public override bool IsConditionsMetForActivation()
		{
			return TutorialPhase.Instance.IsCompleted && TutorialHelper.CurrentContext == TutorialContexts.InventoryScreen && TutorialHelper.CurrentInventoryScreenIncludesBannerItem;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00004772 File Offset: 0x00002972
		public override bool IsConditionsMetForCompletion()
		{
			return this._inspectedOtherBannerItem;
		}

		// Token: 0x0400005B RID: 91
		private bool _inspectedOtherBannerItem;
	}
}
