using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002DF RID: 735
	public static class PlayerSiege
	{
		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x000A8264 File Offset: 0x000A6464
		public static SiegeEvent PlayerSiegeEvent
		{
			get
			{
				SiegeEvent siegeEvent;
				if ((siegeEvent = MobileParty.MainParty.SiegeEvent) == null)
				{
					Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
					if (currentSettlement == null)
					{
						return null;
					}
					siegeEvent = currentSettlement.SiegeEvent;
				}
				return siegeEvent;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x000A8289 File Offset: 0x000A6489
		public static Settlement BesiegedSettlement
		{
			get
			{
				SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
				if (playerSiegeEvent == null)
				{
					return null;
				}
				return playerSiegeEvent.BesiegedSettlement;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06002848 RID: 10312 RVA: 0x000A829B File Offset: 0x000A649B
		public static BattleSideEnum PlayerSide
		{
			get
			{
				if (MobileParty.MainParty.BesiegerCamp == null)
				{
					return BattleSideEnum.Defender;
				}
				return BattleSideEnum.Attacker;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06002849 RID: 10313 RVA: 0x000A82AC File Offset: 0x000A64AC
		public static bool IsRebellion
		{
			get
			{
				return PlayerSiege.BesiegedSettlement != null && PlayerSiege.BesiegedSettlement.IsUnderRebellionAttack();
			}
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000A82C1 File Offset: 0x000A64C1
		private static void SetPlayerSiegeEvent()
		{
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000A82C3 File Offset: 0x000A64C3
		public static void StartSiegePreparation()
		{
			if (Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.ExitToLast();
			}
			GameMenu.ActivateGameMenu("menu_siege_strategies");
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000A82E0 File Offset: 0x000A64E0
		public static void OnSiegeEventFinalized(bool besiegerPartyDefeated)
		{
			MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
			if (PlayerSiege.IsRebellion)
			{
				if (mapState != null && mapState.AtMenu)
				{
					GameMenu.ExitToLast();
					return;
				}
			}
			else if (PlayerSiege.PlayerSide == BattleSideEnum.Defender && !PlayerSiege.IsRebellion)
			{
				if (Settlement.CurrentSettlement != null)
				{
					if (mapState != null && !mapState.AtMenu)
					{
						GameMenu.ActivateGameMenu(besiegerPartyDefeated ? "siege_attacker_defeated" : "siege_attacker_left");
						return;
					}
					GameMenu.SwitchToMenu(besiegerPartyDefeated ? "siege_attacker_defeated" : "siege_attacker_left");
					return;
				}
			}
			else if (Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Army != null && Hero.MainHero.PartyBelongedTo.Army.LeaderParty != MobileParty.MainParty)
			{
				if (MobileParty.MainParty.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
				}
				if (PlayerEncounter.Battle == null)
				{
					if (mapState != null)
					{
						if (mapState.AtMenu)
						{
							GameMenu.SwitchToMenu("army_wait");
							return;
						}
						GameMenu.ActivateGameMenu("army_wait");
						return;
					}
					else
					{
						Campaign.Current.GameMenuManager.SetNextMenu("army_wait");
					}
				}
			}
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000A83F8 File Offset: 0x000A65F8
		public static void StartPlayerSiege(BattleSideEnum playerSide, bool isSimulation = false, Settlement settlement = null)
		{
			if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				MobileParty.MainParty.SetMoveModeHold();
			}
			PlayerSiege.SetPlayerSiegeEvent();
			if (!isSimulation)
			{
				GameState gameState = Game.Current.GameStateManager.GameStates.FirstOrDefault<GameState>((GameState s) => s is MapState);
				if (gameState != null)
				{
					MapState mapState = gameState as MapState;
					if (mapState != null)
					{
						mapState.OnPlayerSiegeActivated();
					}
				}
			}
			CampaignEventDispatcher.Instance.OnPlayerSiegeStarted();
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000A848C File Offset: 0x000A668C
		public static void FinalizePlayerSiege()
		{
			if (PlayerSiege.PlayerSiegeEvent == null)
			{
				return;
			}
			PlayerSiege.BesiegedSettlement.Party.SetVisualAsDirty();
			MobileParty.MainParty.SetMoveModeHold();
			GameState gameState = Game.Current.GameStateManager.GameStates.FirstOrDefault<GameState>((GameState s) => s is MapState);
			if (gameState != null)
			{
				MapState mapState = gameState as MapState;
				if (mapState == null)
				{
					return;
				}
				mapState.OnPlayerSiegeDeactivated();
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000A8504 File Offset: 0x000A6704
		public static void StartSiegeMission(Settlement settlement = null)
		{
			Settlement besiegedSettlement = PlayerSiege.BesiegedSettlement;
			Settlement.SiegeState currentSiegeState = besiegedSettlement.CurrentSiegeState;
			if (currentSiegeState == Settlement.SiegeState.OnTheWalls)
			{
				List<MissionSiegeWeapon> preparedAndActiveSiegeEngines = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker));
				List<MissionSiegeWeapon> preparedAndActiveSiegeEngines2 = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender));
				bool flag = preparedAndActiveSiegeEngines.Exists((MissionSiegeWeapon data) => data.Type == DefaultSiegeEngineTypes.SiegeTower);
				int wallLevel = besiegedSettlement.Town.GetWallLevel();
				CampaignMission.OpenSiegeMissionWithDeployment(besiegedSettlement.LocationComplex.GetLocationWithId("center").GetSceneName(wallLevel), besiegedSettlement.SettlementWallSectionHitPointsRatioList.ToArray(), flag, preparedAndActiveSiegeEngines, preparedAndActiveSiegeEngines2, PlayerEncounter.Current.PlayerSide == BattleSideEnum.Attacker, wallLevel, false, false);
				return;
			}
			if (currentSiegeState != Settlement.SiegeState.Invalid)
			{
				return;
			}
			Debug.FailedAssert("Siege state is invalid!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Siege\\PlayerSiege.cs", "StartSiegeMission", 181);
		}
	}
}
