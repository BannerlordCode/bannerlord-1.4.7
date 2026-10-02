using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C7 RID: 199
	public class NavalDeathSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x0005E400 File Offset: 0x0005C600
		public Hero DeadHero { get; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0005E408 File Offset: 0x0005C608
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_naval_battle_death";
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x0005E40F File Offset: 0x0005C60F
		// (set) Token: 0x06001438 RID: 5176 RVA: 0x0005E417 File Offset: 0x0005C617
		public KillCharacterAction.KillCharacterActionDetail KillDetail { get; private set; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x0005E420 File Offset: 0x0005C620
		public override SceneNotificationData.NotificationSceneProperties SceneProperties
		{
			get
			{
				return new SceneNotificationData.NotificationSceneProperties
				{
					InitializePhysics = true,
					DisableStaticShadows = true,
					OverriddenWaterStrength = null
				};
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x0005E454 File Offset: 0x0005C654
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
				{
					return GameTexts.FindText("str_main_hero_battle_death", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor)
				{
					return GameTexts.FindText("str_main_hero_battle_death_in_labor", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.Executed || this.KillDetail == KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent)
				{
					return GameTexts.FindText("str_main_hero_battle_executed", null);
				}
				if (this.KillDetail == KillCharacterAction.KillCharacterActionDetail.Murdered)
				{
					return GameTexts.FindText("str_main_hero_battle_murdered", null);
				}
				return GameTexts.FindText("str_family_member_death", null);
			}
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0005E50B File Offset: 0x0005C70B
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationCharacter>();
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0005E512 File Offset: 0x0005C712
		public override SceneNotificationData.SceneNotificationShip[] GetShips()
		{
			return Array.Empty<SceneNotificationData.SceneNotificationShip>();
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0005E519 File Offset: 0x0005C719
		public NavalDeathSceneNotificationItem(Hero deadHero, CampaignTime creationTime, KillCharacterAction.KillCharacterActionDetail killDetail)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = creationTime;
			this.KillDetail = killDetail;
		}

		// Token: 0x040006BD RID: 1725
		private readonly CampaignTime _creationCampaignTime;
	}
}
