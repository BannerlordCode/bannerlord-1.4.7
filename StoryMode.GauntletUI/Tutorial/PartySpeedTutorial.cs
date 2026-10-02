using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200001B RID: 27
	[Tutorial("PartySpeed")]
	public class PartySpeedTutorial : TutorialItemBase
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00002F22 File Offset: 0x00001122
		public PartySpeedTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "PartySpeedLabel";
			base.MouseRequired = true;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002F43 File Offset: 0x00001143
		public override bool IsConditionsMetForCompletion()
		{
			return this._isPlayerInspectedPartySpeed;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002F4B File Offset: 0x0000114B
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002F4E File Offset: 0x0000114E
		public override void OnPlayerInspectedPartySpeed(PlayerInspectedPartySpeedEvent obj)
		{
			if (this._isActivated)
			{
				this._isPlayerInspectedPartySpeed = true;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002F60 File Offset: 0x00001160
		public override bool IsConditionsMetForActivation()
		{
			this._isActivated = TutorialHelper.CurrentContext == TutorialContexts.MapWindow && Campaign.Current.CurrentMenuContext == null && MobileParty.MainParty.PartyMoveMode != MoveModeType.Hold && MobileParty.MainParty.IsActive && MobileParty.MainParty.Speed < TutorialHelper.MaximumSpeedForPartyForSpeedTutorial && (float)MobileParty.MainParty.InventoryCapacity < MobileParty.MainParty.TotalWeightCarried;
			return this._isActivated;
		}

		// Token: 0x04000021 RID: 33
		private bool _isPlayerInspectedPartySpeed;

		// Token: 0x04000022 RID: 34
		private bool _isActivated;
	}
}
