using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000021 RID: 33
	[Tutorial("RaidVillageStep1")]
	public class RaidVillageStep1Tutorial : TutorialItemBase
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x000032BB File Offset: 0x000014BB
		public RaidVillageStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Top;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = true;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000032DC File Offset: 0x000014DC
		public override bool IsConditionsMetForCompletion()
		{
			return this._gameMenuChanged;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000032E4 File Offset: 0x000014E4
		public override void OnGameMenuOpened(MenuCallbackArgs obj)
		{
			if (this._villageRaidMenuOpened && obj.MenuContext.GameMenu.StringId != TutorialHelper.ActiveVillageRaidGameMenuID)
			{
				this._gameMenuChanged = true;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003314 File Offset: 0x00001514
		public override void OnGameMenuOptionSelected(GameMenuOption obj)
		{
			base.OnGameMenuOptionSelected(obj);
			if (this._villageRaidMenuOpened)
			{
				Campaign campaign = Campaign.Current;
				string text;
				if (campaign == null)
				{
					text = null;
				}
				else
				{
					MenuContext currentMenuContext = campaign.CurrentMenuContext;
					if (currentMenuContext == null)
					{
						text = null;
					}
					else
					{
						GameMenu gameMenu = currentMenuContext.GameMenu;
						text = ((gameMenu != null) ? gameMenu.StringId : null);
					}
				}
				if (text == TutorialHelper.ActiveVillageRaidGameMenuID)
				{
					this._gameMenuChanged = true;
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000336C File Offset: 0x0000156C
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000336F File Offset: 0x0000156F
		public override bool IsConditionsMetForActivation()
		{
			this._villageRaidMenuOpened = TutorialHelper.IsActiveVillageRaidGameMenuOpen;
			return this._villageRaidMenuOpened;
		}

		// Token: 0x04000029 RID: 41
		private bool _gameMenuChanged;

		// Token: 0x0400002A RID: 42
		private bool _villageRaidMenuOpened;
	}
}
