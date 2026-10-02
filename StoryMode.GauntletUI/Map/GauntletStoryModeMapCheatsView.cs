using System;
using SandBox.GauntletUI.Map;
using SandBox.View.Map;
using StoryMode.GameComponents.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace StoryMode.GauntletUI.Map
{
	// Token: 0x0200004A RID: 74
	[OverrideView(typeof(MapCheatsView))]
	internal class GauntletStoryModeMapCheatsView : GauntletMapCheatsView
	{
		// Token: 0x06000167 RID: 359 RVA: 0x00004A54 File Offset: 0x00002C54
		protected override void CreateLayout()
		{
			base.CreateLayout();
			AchievementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<AchievementsCampaignBehavior>();
			TextObject textObject;
			if (campaignBehavior == null || !campaignBehavior.CheckAchievementSystemActivity(out textObject))
			{
				this.EnableCheatMenu();
				return;
			}
			this._layerAsGauntletLayer.UIContext.ContextAlpha = 0f;
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=4Ygn4OGE}Enable Cheats", null).ToString(), new TextObject("{=YkbOfPRU}Enabling cheats will disable the achievements this game. Do you want to proceed?", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.EnableCheatMenu), new Action(this.RemoveCheatMenu), "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004B14 File Offset: 0x00002D14
		private void EnableCheatMenu()
		{
			this._layerAsGauntletLayer.UIContext.ContextAlpha = 1f;
			AchievementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<AchievementsCampaignBehavior>();
			TextObject textObject;
			if (campaignBehavior != null && campaignBehavior.CheckAchievementSystemActivity(out textObject) && campaignBehavior != null)
			{
				campaignBehavior.DeactivateAchievements(new TextObject("{=sO8Zh3ZH}Achievements are disabled due to cheat usage.", null), true, false);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004B64 File Offset: 0x00002D64
		private void RemoveCheatMenu()
		{
			base.MapScreen.CloseGameplayCheats();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004B71 File Offset: 0x00002D71
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004B8D File Offset: 0x00002D8D
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}
	}
}
