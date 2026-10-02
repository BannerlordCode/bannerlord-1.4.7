using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Library;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000055 RID: 85
	public class ThirdPhaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600053B RID: 1339 RVA: 0x0001E154 File Offset: 0x0001C354
		public override void RegisterEvents()
		{
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
			CampaignEvents.CanKingdomBeDiscontinuedEvent.AddNonSerializedListener(this, new ReferenceAction<Kingdom, bool>(this.CanKingdomBeDiscontinued));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001E1BD File Offset: 0x0001C3BD
		private void OnSessionLaunched(CampaignGameStarter starter)
		{
			this.AddGameMenus(starter);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001E1C8 File Offset: 0x0001C3C8
		private void AddGameMenus(CampaignGameStarter starter)
		{
			starter.AddGameMenu("siege_ended_by_last_conspiracy_kingdom_defeat", "{=3pEDvftb} The conspiracy has collapsed. The defenders of their final stronghold send out a delegation under flag of truce and agree to surrender. Your men take possession of their fortress.", new OnInitDelegate(this.game_menu_last_conspiracy_kingdom_defeated_when_player_besiege_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			starter.AddGameMenuOption("siege_ended_by_last_conspiracy_kingdom_defeat", "leave_from_besieged_last_conspiracy_settlement", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.siege_ended_by_last_conspiracy_kingdom_defeat_condition), new GameMenuOption.OnConsequenceDelegate(this.siege_ended_by_last_conspiracy_kingdom_defeat_consequence), true, -1, false, null);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001E228 File Offset: 0x0001C428
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			Kingdom kingdom;
			Kingdom kingdom2;
			if ((kingdom = faction1 as Kingdom) != null && (kingdom2 = faction2 as Kingdom) != null && StoryModeManager.Current.MainStoryLine.ThirdPhase != null)
			{
				MBReadOnlyList<Kingdom> oppositionKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms;
				MBReadOnlyList<Kingdom> allyKingdoms = StoryModeManager.Current.MainStoryLine.ThirdPhase.AllyKingdoms;
				if ((oppositionKingdoms.IndexOf(kingdom) >= 0 && oppositionKingdoms.IndexOf(kingdom2) >= 0) || (allyKingdoms.IndexOf(kingdom) >= 0 && allyKingdoms.IndexOf(kingdom2) >= 0))
				{
					this._warsToEnforcePeaceNextWeek.Add(new Tuple<Kingdom, Kingdom>(kingdom, kingdom2));
				}
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001E2C0 File Offset: 0x0001C4C0
		private void WeeklyTick()
		{
			foreach (Tuple<Kingdom, Kingdom> tuple in new List<Tuple<Kingdom, Kingdom>>(this._warsToEnforcePeaceNextWeek))
			{
				MakePeaceAction.Apply(tuple.Item1, tuple.Item2);
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001E324 File Offset: 0x0001C524
		private void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
			if (StoryModeManager.Current.MainStoryLine.ThirdPhase != null && StoryModeManager.Current.MainStoryLine.ThirdPhase.OppositionKingdoms.Contains(kingdom))
			{
				result = false;
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001E356 File Offset: 0x0001C556
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Tuple<Kingdom, Kingdom>>>("_warsToEnforcePeaceNextWeek", ref this._warsToEnforcePeaceNextWeek);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001E36A File Offset: 0x0001C56A
		private void siege_ended_by_last_conspiracy_kingdom_defeat_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001E371 File Offset: 0x0001C571
		private bool siege_ended_by_last_conspiracy_kingdom_defeat_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001E37C File Offset: 0x0001C57C
		private void game_menu_last_conspiracy_kingdom_defeated_when_player_besiege_menu_on_init(MenuCallbackArgs args)
		{
			Debug.Print("Game loaded when the player siege is left on last conspiracy kingdom is defeated by some other reasons", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x040001DB RID: 475
		private List<Tuple<Kingdom, Kingdom>> _warsToEnforcePeaceNextWeek = new List<Tuple<Kingdom, Kingdom>>();
	}
}
