using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox
{
	// Token: 0x0200001F RID: 31
	public class CampaignMissionManager : CampaignMission.ICampaignMissionManager
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00006008 File Offset: 0x00004208
		IMission CampaignMission.ICampaignMissionManager.OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointsPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int upgradeLevel, bool isSallyOut, bool isReliefForceAttack)
		{
			return SandBoxMissions.OpenSiegeMissionWithDeployment(scene, wallHitPointsPercentages, hasAnySiegeTower, siegeWeaponsOfAttackers, siegeWeaponsOfDefenders, isPlayerAttacker, upgradeLevel, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006029 File Offset: 0x00004229
		IMission CampaignMission.ICampaignMissionManager.OpenSiegeMissionNoDeployment(string scene, bool isSallyOut, bool isReliefForceAttack)
		{
			return SandBoxMissions.OpenSiegeMissionNoDeployment(scene, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006033 File Offset: 0x00004233
		IMission CampaignMission.ICampaignMissionManager.OpenSiegeLordsHallFightMission(string scene, FlattenedTroopRoster attackerPriorityList)
		{
			return SandBoxMissions.OpenSiegeLordsHallFightMission(scene, attackerPriorityList);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000603C File Offset: 0x0000423C
		IMission CampaignMission.ICampaignMissionManager.OpenBattleMission(MissionInitializerRecord rec)
		{
			return SandBoxMissions.OpenBattleMission(rec);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006044 File Offset: 0x00004244
		IMission CampaignMission.ICampaignMissionManager.OpenCaravanBattleMission(MissionInitializerRecord rec, bool isCaravan)
		{
			return SandBoxMissions.OpenCaravanBattleMission(rec, isCaravan);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000604D File Offset: 0x0000424D
		IMission CampaignMission.ICampaignMissionManager.OpenBattleMission(string scene, bool usesTownDecalAtlas, string sceneLevels)
		{
			return SandBoxMissions.OpenBattleMission(scene, usesTownDecalAtlas, sceneLevels);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006057 File Offset: 0x00004257
		IMission CampaignMission.ICampaignMissionManager.OpenAlleyFightMission(string scene, int upgradeLevel, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops)
		{
			return SandBoxMissions.OpenAlleyFightMission(scene, upgradeLevel, location, playerSideTroops, rivalSideTroops);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006065 File Offset: 0x00004265
		IMission CampaignMission.ICampaignMissionManager.OpenCombatMissionWithDialogue(string scene, CharacterObject characterToTalkTo, int upgradeLevel)
		{
			return SandBoxMissions.OpenCombatMissionWithDialogue(scene, characterToTalkTo, upgradeLevel);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000606F File Offset: 0x0000426F
		IMission CampaignMission.ICampaignMissionManager.OpenBattleMissionWhileEnteringSettlement(string scene, int upgradeLevel, int numberOfMaxTroopToBeSpawnedForPlayer, int numberOfMaxTroopToBeSpawnedForOpponent)
		{
			return SandBoxMissions.OpenBattleMissionWhileEnteringSettlement(scene, upgradeLevel, numberOfMaxTroopToBeSpawnedForPlayer, numberOfMaxTroopToBeSpawnedForOpponent);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000607B File Offset: 0x0000427B
		IMission CampaignMission.ICampaignMissionManager.OpenHideoutBattleMission(string scene, FlattenedTroopRoster playerTroops, bool isTutorial)
		{
			return SandBoxMissions.OpenHideoutBattleMission(scene, playerTroops, isTutorial);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006085 File Offset: 0x00004285
		IMission CampaignMission.ICampaignMissionManager.OpenTownCenterMission(string scene, int townUpgradeLevel, Location location, CharacterObject talkToChar, string playerSpawnTag)
		{
			return SandBoxMissions.OpenTownCenterMission(scene, townUpgradeLevel, location, talkToChar, playerSpawnTag);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006093 File Offset: 0x00004293
		IMission CampaignMission.ICampaignMissionManager.OpenCastleCourtyardMission(string scene, int castleUpgradeLevel, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenCastleCourtyardMission(scene, castleUpgradeLevel, location, talkToChar);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000609F File Offset: 0x0000429F
		IMission CampaignMission.ICampaignMissionManager.OpenVillageMission(string scene, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenVillageMission(scene, location, talkToChar, null);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000060AA File Offset: 0x000042AA
		IMission CampaignMission.ICampaignMissionManager.OpenIndoorMission(string scene, int upgradeLevel, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenIndoorMission(scene, upgradeLevel, location, talkToChar);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000060B6 File Offset: 0x000042B6
		IMission CampaignMission.ICampaignMissionManager.OpenPrisonBreakMission(string scene, Location location, CharacterObject prisonerCharacter)
		{
			return SandBoxMissions.OpenPrisonBreakMission(scene, location, prisonerCharacter);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000060C0 File Offset: 0x000042C0
		IMission CampaignMission.ICampaignMissionManager.OpenArenaStartMission(string scene, Location location, CharacterObject talkToChar)
		{
			return SandBoxMissions.OpenArenaStartMission(scene, location, talkToChar, "");
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000060CF File Offset: 0x000042CF
		public IMission OpenArenaDuelMission(string scene, Location location, CharacterObject duelCharacter, bool requireCivilianEquipment, bool spawnBOthSidesWithHorse, Action<CharacterObject> onDuelEndAction, float customAgentHealth)
		{
			return SandBoxMissions.OpenArenaDuelMission(scene, location, duelCharacter, requireCivilianEquipment, spawnBOthSidesWithHorse, onDuelEndAction, customAgentHealth, "");
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000060E6 File Offset: 0x000042E6
		IMission CampaignMission.ICampaignMissionManager.OpenConversationMission(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, string specialScene, string sceneLevels, bool isMultiAgentConversation)
		{
			return SandBoxMissions.OpenConversationMission(playerCharacterData, conversationPartnerData, specialScene, sceneLevels, isMultiAgentConversation);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000060F4 File Offset: 0x000042F4
		IMission CampaignMission.ICampaignMissionManager.OpenMeetingMission(string scene, CharacterObject character)
		{
			return SandBoxMissions.OpenMeetingMission(scene, character);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000060FD File Offset: 0x000042FD
		IMission CampaignMission.ICampaignMissionManager.OpenRetirementMission(string scene, Location location, CharacterObject talkToChar, string sceneLevels, string unconsciousMenuId)
		{
			return SandBoxMissions.OpenRetirementMission(scene, location, talkToChar, sceneLevels, unconsciousMenuId);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000610B File Offset: 0x0000430B
		IMission CampaignMission.ICampaignMissionManager.OpenHideoutAmbushMission(string sceneName, FlattenedTroopRoster playerTroops, Location location)
		{
			return SandBoxMissions.OpenHideoutAmbushMission(sceneName, playerTroops, location);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006115 File Offset: 0x00004315
		public IMission OpenDisguiseMission(string scene, bool willSetUpContact, string sceneLevels, Location fromLocation)
		{
			return SandBoxMissions.OpenDisguiseMission(scene, willSetUpContact, fromLocation, sceneLevels);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006121 File Offset: 0x00004321
		public IMission OpenNavalRaidMission(TroopRoster navalRaidTroops, BattleSideEnum navalSide, List<Ship> allShips)
		{
			return null;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006124 File Offset: 0x00004324
		public IMission OpenNavalBattleMission(MissionInitializerRecord rec)
		{
			return null;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006127 File Offset: 0x00004327
		public IMission OpenNavalSetPieceBattleMission(MissionInitializerRecord rec, MBList<IShipOrigin> playerShips, MBList<IShipOrigin> playerAllyShips, MBList<IShipOrigin> enemyShips)
		{
			return null;
		}
	}
}
