using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Roster;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000394 RID: 916
	public interface IMenuContextHandler
	{
		// Token: 0x06003508 RID: 13576
		void OnBackgroundMeshNameSet(string name);

		// Token: 0x06003509 RID: 13577
		void OnOpenTownManagement();

		// Token: 0x0600350A RID: 13578
		void OnOpenRecruitVolunteers();

		// Token: 0x0600350B RID: 13579
		void OnOpenTournamentLeaderboard();

		// Token: 0x0600350C RID: 13580
		void OnOpenTroopSelection(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount);

		// Token: 0x0600350D RID: 13581
		void OnOpenNavalTroopSelection(TroopRoster fullRoster, TroopRoster initialTroopSelections, List<Ship> eligibleShips, List<Ship> initialShipSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster, List<Ship>> onDone, int minSelectableTroopCount, int minSelectableShipCount, int maxSelectableShipCount, bool anyOtherPartiesOnPlayerSide);

		// Token: 0x0600350E RID: 13582
		void OnMenuCreate();

		// Token: 0x0600350F RID: 13583
		void OnMenuActivate();

		// Token: 0x06003510 RID: 13584
		void OnMenuRefresh();

		// Token: 0x06003511 RID: 13585
		void OnHourlyTick();

		// Token: 0x06003512 RID: 13586
		void OnPanelSoundIDSet(string panelSoundID);

		// Token: 0x06003513 RID: 13587
		void OnAmbientSoundIDSet(string ambientSoundID);
	}
}
