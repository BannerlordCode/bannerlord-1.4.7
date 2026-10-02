using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Arena
{
	// Token: 0x0200009B RID: 155
	public class ArenaPracticeFightMissionController : MissionLogic
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0002B2E1 File Offset: 0x000294E1
		private int AISpawnIndex
		{
			get
			{
				return this._spawnedOpponentAgentCount;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x0002B2E9 File Offset: 0x000294E9
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x0002B2F1 File Offset: 0x000294F1
		public int RemainingOpponentCountFromLastPractice { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0002B2FA File Offset: 0x000294FA
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x0002B302 File Offset: 0x00029502
		public bool IsPlayerPracticing { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0002B30B File Offset: 0x0002950B
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0002B313 File Offset: 0x00029513
		public int OpponentCountBeatenByPlayer { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0002B31C File Offset: 0x0002951C
		public int RemainingOpponentCount
		{
			get
			{
				return 30 - this._spawnedOpponentAgentCount + this._aliveOpponentCount;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0002B32E File Offset: 0x0002952E
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x0002B336 File Offset: 0x00029536
		public bool IsPlayerSurvived { get; private set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0002B33F File Offset: 0x0002953F
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x0002B347 File Offset: 0x00029547
		public bool AfterPractice { get; set; }

		// Token: 0x0600066D RID: 1645 RVA: 0x0002B350 File Offset: 0x00029550
		public override void AfterStart()
		{
			this._settlement = PlayerEncounter.LocationEncounter.Settlement;
			this.InitializeTeams();
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("tournament_practice") ?? base.Mission.Scene.FindEntityWithTag("tournament_fight");
			List<GameEntity> list = Mission.Current.Scene.FindEntitiesWithTag("arena_set").ToList<GameEntity>();
			list.Remove(gameEntity);
			foreach (GameEntity gameEntity2 in list)
			{
				gameEntity2.Remove(88);
			}
			this._initialSpawnFrames = (from e in base.Mission.Scene.FindEntitiesWithTag("sp_arena")
				select e.GetGlobalFrame()).ToList<MatrixFrame>();
			this._spawnFrames = (from e in base.Mission.Scene.FindEntitiesWithTag("sp_arena_respawn")
				select e.GetGlobalFrame()).ToList<MatrixFrame>();
			for (int i = 0; i < this._initialSpawnFrames.Count; i++)
			{
				MatrixFrame matrixFrame = this._initialSpawnFrames[i];
				matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				this._initialSpawnFrames[i] = matrixFrame;
			}
			for (int j = 0; j < this._spawnFrames.Count; j++)
			{
				MatrixFrame matrixFrame2 = this._spawnFrames[j];
				matrixFrame2.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
				this._spawnFrames[j] = matrixFrame2;
			}
			this.IsPlayerPracticing = false;
			this._participantAgents = new List<Agent>();
			this.StartPractice();
			MissionAgentHandler missionBehavior = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			SandBoxHelpers.MissionHelper.SpawnPlayer(true, true, false, false, "");
			missionBehavior.SpawnLocationCharacters(null);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0002B544 File Offset: 0x00029744
		private void SpawnPlayerNearTournamentMaster()
		{
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("sp_player_near_arena_master");
			base.Mission.SpawnAgent(new AgentBuildData(CharacterObject.PlayerCharacter).Team(base.Mission.PlayerTeam).InitialFrameFromSpawnPointEntity(gameEntity).NoHorses(true)
				.CivilianEquipment(true)
				.TroopOrigin(new SimpleAgentOrigin(CharacterObject.PlayerCharacter, -1, null, default(UniqueTroopDescriptor)))
				.Controller(AgentControllerType.Player), false);
			Mission.Current.SetMissionMode(MissionMode.StartUp, false);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0002B5CC File Offset: 0x000297CC
		private Agent SpawnArenaAgent(Team team, MatrixFrame frame)
		{
			CharacterObject characterObject;
			int num;
			if (team == base.Mission.PlayerTeam)
			{
				characterObject = CharacterObject.PlayerCharacter;
				num = 0;
			}
			else
			{
				characterObject = this._participantCharacters[this.AISpawnIndex];
				num = this.AISpawnIndex;
			}
			Equipment equipment = new Equipment();
			this.AddRandomWeapons(equipment, num);
			this.AddRandomClothes(characterObject, equipment);
			Mission mission = base.Mission;
			AgentBuildData agentBuildData = new AgentBuildData(characterObject).Team(team).InitialPosition(in frame.origin);
			Vec2 vec = frame.rotation.f.AsVec2;
			vec = vec.Normalized();
			Agent agent = mission.SpawnAgent(agentBuildData.InitialDirection(in vec).NoHorses(true).Equipment(equipment)
				.TroopOrigin(new SimpleAgentOrigin(characterObject, -1, null, default(UniqueTroopDescriptor)))
				.Controller((characterObject == CharacterObject.PlayerCharacter) ? AgentControllerType.Player : AgentControllerType.AI), false);
			agent.FadeIn();
			if (characterObject != CharacterObject.PlayerCharacter)
			{
				this._aliveOpponentCount++;
				this._spawnedOpponentAgentCount++;
			}
			if (agent.IsAIControlled)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			return agent;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002B6D8 File Offset: 0x000298D8
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent == null)
			{
				return;
			}
			if (affectorAgent.IsMount && affectorAgent.RiderAgent != null)
			{
				affectorAgent = affectorAgent.RiderAgent;
			}
			if (affectorAgent.Character == null || affectedAgent.Character == null)
			{
				return;
			}
			float num = (float)blow.InflictedDamage;
			if (num > affectedAgent.HealthLimit)
			{
				num = affectedAgent.HealthLimit;
			}
			float num2 = num / affectedAgent.HealthLimit;
			this.EnemyHitReward(affectedAgent, affectorAgent, blow.MovementSpeedDamageModifier, shotDifficulty, attackerWeapon, blow.AttackType, 0.5f * num2, num, collisionData.IsSneakAttack);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002B75C File Offset: 0x0002995C
		private void EnemyHitReward(Agent affectedAgent, Agent affectorAgent, float lastSpeedBonus, float lastShotDifficulty, WeaponComponentData attackerWeapon, AgentAttackType attackType, float hitpointRatio, float damageAmount, bool isSneakAttack)
		{
			CharacterObject characterObject = (CharacterObject)affectedAgent.Character;
			CharacterObject characterObject2 = (CharacterObject)affectorAgent.Character;
			if (affectedAgent.Origin != null && affectorAgent != null && affectorAgent.Origin != null)
			{
				bool flag = affectorAgent.MountAgent != null;
				bool flag2 = flag && attackType == AgentAttackType.Collision;
				SkillLevelingManager.OnCombatHit(characterObject2, characterObject, null, null, lastSpeedBonus, lastShotDifficulty, attackerWeapon, hitpointRatio, CombatXpModel.MissionTypeEnum.PracticeFight, flag, affectorAgent.Team == affectedAgent.Team, false, damageAmount, affectedAgent.Health < 1f, false, flag2, isSneakAttack);
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002B7E0 File Offset: 0x000299E0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._aliveOpponentCount < 6 && this._spawnedOpponentAgentCount < 30 && (this._aliveOpponentCount == 2 || this._nextSpawnTime < base.Mission.CurrentTime))
			{
				Team team = this.SelectRandomAiTeam();
				Agent agent = this.SpawnArenaAgent(team, this.GetSpawnFrame(true, false));
				this._participantAgents.Add(agent);
				this._nextSpawnTime = base.Mission.CurrentTime + 14f - (float)this._spawnedOpponentAgentCount / 3f;
				if (this._spawnedOpponentAgentCount == 30 && !this.IsPlayerPracticing)
				{
					this._spawnedOpponentAgentCount = 0;
				}
			}
			if (this._teleportTimer == null && this.IsPlayerPracticing && this.CheckPracticeEndedForPlayer())
			{
				this._teleportTimer = new BasicMissionTimer();
				this.IsPlayerSurvived = base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive();
				if (this.IsPlayerSurvived)
				{
					MBInformationManager.AddQuickInformation(new TextObject("{=seyti8xR}Victory!", null), 0, null, null, "event:/ui/mission/arena_victory");
				}
				this.AfterPractice = true;
			}
			if (this._teleportTimer != null && this._teleportTimer.ElapsedTime > (float)this.TeleportTime)
			{
				this._teleportTimer = null;
				this.RemainingOpponentCountFromLastPractice = this.RemainingOpponentCount;
				this.IsPlayerPracticing = false;
				this.StartPractice();
				this.SpawnPlayerNearTournamentMaster();
				Agent agent2 = base.Mission.Agents.FirstOrDefault<Agent>((Agent x) => x.Character != null && ((CharacterObject)x.Character).Occupation == Occupation.ArenaMaster);
				MissionConversationLogic.Current.StartConversation(agent2, true, false);
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0002B978 File Offset: 0x00029B78
		private Team SelectRandomAiTeam()
		{
			Team team = null;
			foreach (Team team2 in this._AIParticipantTeams)
			{
				if (!team2.HasBots)
				{
					team = team2;
					break;
				}
			}
			if (team == null)
			{
				team = this._AIParticipantTeams[MBRandom.RandomInt(this._AIParticipantTeams.Count - 1) + 1];
			}
			return team;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0002B9F8 File Offset: 0x00029BF8
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectedAgent != null && affectedAgent.IsHuman)
			{
				if (affectedAgent != Agent.Main)
				{
					this._aliveOpponentCount--;
				}
				if (affectorAgent != null && affectorAgent.IsHuman && affectorAgent == Agent.Main && affectedAgent != Agent.Main)
				{
					int opponentCountBeatenByPlayer = this.OpponentCountBeatenByPlayer;
					this.OpponentCountBeatenByPlayer = opponentCountBeatenByPlayer + 1;
				}
			}
			if (this._participantAgents.Contains(affectedAgent))
			{
				this._participantAgents.Remove(affectedAgent);
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0002BA6C File Offset: 0x00029C6C
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return false;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002BA70 File Offset: 0x00029C70
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (!this.IsPlayerPracticing)
			{
				return null;
			}
			return new InquiryData(new TextObject("{=zv49qE35}Practice Fight", null).ToString(), GameTexts.FindText("str_give_up_fight", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002BAF0 File Offset: 0x00029CF0
		public void StartPlayerPractice()
		{
			this.IsPlayerPracticing = true;
			this.AfterPractice = false;
			this.StartPractice();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002BB08 File Offset: 0x00029D08
		private void StartPractice()
		{
			this.InitializeParticipantCharacters();
			SandBoxHelpers.MissionHelper.FadeOutAgents(base.Mission.Agents.Where<Agent>((Agent agent) => this._participantAgents.Contains(agent) || agent.IsMount || agent.IsPlayerControlled), true, false);
			this._spawnedOpponentAgentCount = 0;
			this._aliveOpponentCount = 0;
			this._participantAgents.Clear();
			Mission.Current.ClearCorpses(false);
			base.Mission.RemoveSpawnedItemsAndMissiles();
			this.ArrangePlayerTeamEnmity();
			if (this.IsPlayerPracticing)
			{
				Agent agent2 = this.SpawnArenaAgent(base.Mission.PlayerTeam, this.GetSpawnFrame(false, true));
				agent2.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				this.OpponentCountBeatenByPlayer = 0;
				this._participantAgents.Add(agent2);
			}
			int count = this._AIParticipantTeams.Count;
			int num = 0;
			while (this._spawnedOpponentAgentCount < 6)
			{
				this._participantAgents.Add(this.SpawnArenaAgent(this._AIParticipantTeams[num % count], this.GetSpawnFrame(false, true)));
				num++;
			}
			this._nextSpawnTime = base.Mission.CurrentTime + 14f;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0002BC0B File Offset: 0x00029E0B
		private bool CheckPracticeEndedForPlayer()
		{
			return base.Mission.MainAgent == null || !base.Mission.MainAgent.IsActive() || this.RemainingOpponentCount == 0;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0002BC38 File Offset: 0x00029E38
		private void AddRandomWeapons(Equipment equipment, int spawnIndex)
		{
			int num = 1 + spawnIndex * 3 / 30;
			List<Equipment> list = (Game.Current.ObjectManager.GetObject<CharacterObject>(string.Concat(new object[]
			{
				"weapon_practice_stage_",
				num,
				"_",
				this._settlement.MapFaction.Culture.StringId
			})) ?? Game.Current.ObjectManager.GetObject<CharacterObject>("weapon_practice_stage_" + num + "_empire")).BattleEquipments.ToList<Equipment>();
			int num2 = MBRandom.RandomInt(list.Count);
			for (int i = 0; i <= 3; i++)
			{
				EquipmentElement equipmentFromSlot = list[num2].GetEquipmentFromSlot((EquipmentIndex)i);
				if (equipmentFromSlot.Item != null)
				{
					equipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
				}
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0002BD08 File Offset: 0x00029F08
		private void AddRandomClothes(CharacterObject troop, Equipment equipment)
		{
			Equipment participantArmor = Campaign.Current.Models.TournamentModel.GetParticipantArmor(troop);
			for (int i = 0; i < 12; i++)
			{
				if (i > 4 && i != 10 && i != 11)
				{
					EquipmentElement equipmentFromSlot = participantArmor.GetEquipmentFromSlot((EquipmentIndex)i);
					if (equipmentFromSlot.Item != null)
					{
						equipment.AddEquipmentToSlotWithoutAgent((EquipmentIndex)i, equipmentFromSlot);
					}
				}
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0002BD60 File Offset: 0x00029F60
		private void InitializeTeams()
		{
			this._AIParticipantTeams = new List<Team>();
			base.Mission.Teams.Add(BattleSideEnum.Defender, Hero.MainHero.MapFaction.Color, Hero.MainHero.MapFaction.Color2, null, true, false, true);
			base.Mission.PlayerTeam = base.Mission.DefenderTeam;
			this._tournamentMasterTeam = base.Mission.Teams.Add(BattleSideEnum.None, this._settlement.MapFaction.Color, this._settlement.MapFaction.Color2, null, true, false, true);
			while (this._AIParticipantTeams.Count < 6)
			{
				this._AIParticipantTeams.Add(base.Mission.Teams.Add(BattleSideEnum.Attacker, uint.MaxValue, uint.MaxValue, null, true, false, true));
			}
			for (int i = 0; i < this._AIParticipantTeams.Count; i++)
			{
				this._AIParticipantTeams[i].SetIsEnemyOf(this._tournamentMasterTeam, false);
				for (int j = i + 1; j < this._AIParticipantTeams.Count; j++)
				{
					this._AIParticipantTeams[i].SetIsEnemyOf(this._AIParticipantTeams[j], true);
				}
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002BE94 File Offset: 0x0002A094
		private void InitializeParticipantCharacters()
		{
			List<CharacterObject> participantCharacters = ArenaPracticeFightMissionController.GetParticipantCharacters(this._settlement);
			this._participantCharacters = participantCharacters.OrderBy<CharacterObject, int>((CharacterObject x) => x.Level).ToList<CharacterObject>();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0002BEE0 File Offset: 0x0002A0E0
		public static List<CharacterObject> GetParticipantCharacters(Settlement settlement)
		{
			int num = 30;
			List<CharacterObject> list = new List<CharacterObject>();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (list.Count < num && settlement.Town.GarrisonParty != null)
			{
				foreach (TroopRosterElement troopRosterElement in settlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					int num5 = num - list.Count;
					if (!list.Contains(troopRosterElement.Character) && troopRosterElement.Character.Tier == 3 && (float)num5 * 0.4f > (float)num2)
					{
						list.Add(troopRosterElement.Character);
						num2++;
					}
					else if (!list.Contains(troopRosterElement.Character) && troopRosterElement.Character.Tier == 4 && (float)num5 * 0.4f > (float)num3)
					{
						list.Add(troopRosterElement.Character);
						num3++;
					}
					else if (!list.Contains(troopRosterElement.Character) && troopRosterElement.Character.Tier == 5 && (float)num5 * 0.2f > (float)num4)
					{
						list.Add(troopRosterElement.Character);
						num4++;
					}
					if (list.Count >= num)
					{
						break;
					}
				}
			}
			if (list.Count < num)
			{
				List<CharacterObject> list2 = new List<CharacterObject>();
				ArenaPracticeFightMissionController.GetUpgradeTargets(((settlement != null) ? settlement.Culture : Game.Current.ObjectManager.GetObject<CultureObject>("empire")).BasicTroop, ref list2);
				int num6 = num - list.Count;
				using (List<CharacterObject>.Enumerator enumerator2 = list2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CharacterObject characterObject = enumerator2.Current;
						if (!list.Contains(characterObject) && characterObject.Tier == 3 && (float)num6 * 0.4f > (float)num2)
						{
							list.Add(characterObject);
							num2++;
						}
						else if (!list.Contains(characterObject) && characterObject.Tier == 4 && (float)num6 * 0.4f > (float)num3)
						{
							list.Add(characterObject);
							num3++;
						}
						else if (!list.Contains(characterObject) && characterObject.Tier == 5 && (float)num6 * 0.2f > (float)num4)
						{
							list.Add(characterObject);
							num4++;
						}
						if (list.Count >= num)
						{
							break;
						}
					}
					goto IL_0284;
				}
				IL_0256:
				int num7 = 0;
				while (num7 < list2.Count && list.Count < num)
				{
					list.Add(list2[num7]);
					num7++;
				}
				IL_0284:
				if (list.Count < num)
				{
					goto IL_0256;
				}
			}
			return list;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0002C198 File Offset: 0x0002A398
		private static void GetUpgradeTargets(CharacterObject troop, ref List<CharacterObject> list)
		{
			if (!list.Contains(troop) && troop.Tier >= 3)
			{
				list.Add(troop);
			}
			CharacterObject[] upgradeTargets = troop.UpgradeTargets;
			for (int i = 0; i < upgradeTargets.Length; i++)
			{
				ArenaPracticeFightMissionController.GetUpgradeTargets(upgradeTargets[i], ref list);
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0002C1E0 File Offset: 0x0002A3E0
		private void ArrangePlayerTeamEnmity()
		{
			foreach (Team team in this._AIParticipantTeams)
			{
				team.SetIsEnemyOf(base.Mission.PlayerTeam, this.IsPlayerPracticing);
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002C244 File Offset: 0x0002A444
		private Team GetStrongestTeamExceptPlayerTeam()
		{
			Team team = null;
			int num = -1;
			foreach (Team team2 in this._AIParticipantTeams)
			{
				int num2 = this.CalculateTeamPower(team2);
				if (num2 > num)
				{
					team = team2;
					num = num2;
				}
			}
			return team;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002C2A8 File Offset: 0x0002A4A8
		private int CalculateTeamPower(Team team)
		{
			int num = 0;
			foreach (Agent agent in team.ActiveAgents)
			{
				num += agent.Character.Level * agent.KillCount + (int)MathF.Sqrt(agent.Health);
			}
			return num;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0002C31C File Offset: 0x0002A51C
		private MatrixFrame GetSpawnFrame(bool considerPlayerDistance, bool isInitialSpawn)
		{
			List<MatrixFrame> list = ((isInitialSpawn || this._spawnFrames.IsEmpty<MatrixFrame>()) ? this._initialSpawnFrames : this._spawnFrames);
			if (list.Count == 1)
			{
				Debug.FailedAssert("Spawn point count is wrong! Arena practice spawn point set should be used in arena scenes.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Arena\\ArenaPracticeFightMissionController.cs", "GetSpawnFrame", 616);
				return list[0];
			}
			MatrixFrame matrixFrame;
			if (considerPlayerDistance && Agent.Main != null && Agent.Main.IsActive())
			{
				int num = MBRandom.RandomInt(list.Count);
				matrixFrame = list[num];
				float num2 = float.MinValue;
				for (int i = num + 1; i < num + list.Count; i++)
				{
					MatrixFrame matrixFrame2 = list[i % list.Count];
					float num3 = this.CalculateLocationScore(matrixFrame2);
					if (num3 >= 100f)
					{
						matrixFrame = matrixFrame2;
						break;
					}
					if (num3 > num2)
					{
						matrixFrame = matrixFrame2;
						num2 = num3;
					}
				}
			}
			else
			{
				int num4 = this._spawnedOpponentAgentCount;
				if (this.IsPlayerPracticing && Agent.Main != null)
				{
					num4++;
				}
				matrixFrame = list[num4 % list.Count];
			}
			return matrixFrame;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0002C420 File Offset: 0x0002A620
		private float CalculateLocationScore(MatrixFrame matrixFrame)
		{
			float num = 100f;
			float num2 = 0.25f;
			float num3 = 0.75f;
			if (matrixFrame.origin.DistanceSquared(Agent.Main.Position) < 144f)
			{
				num *= num2;
			}
			for (int i = 0; i < this._participantAgents.Count; i++)
			{
				if (this._participantAgents[i].Position.DistanceSquared(matrixFrame.origin) < 144f)
				{
					num *= num3;
				}
			}
			return num;
		}

		// Token: 0x04000373 RID: 883
		private const int AIParticipantCount = 30;

		// Token: 0x04000374 RID: 884
		private const int MaxAliveAgentCount = 6;

		// Token: 0x04000375 RID: 885
		private const int MaxSpawnInterval = 14;

		// Token: 0x04000376 RID: 886
		private const int MinSpawnDistanceSquared = 144;

		// Token: 0x04000377 RID: 887
		private const int TotalStageCount = 3;

		// Token: 0x04000378 RID: 888
		private const int PracticeFightTroopTierLimit = 3;

		// Token: 0x04000379 RID: 889
		public int TeleportTime = 5;

		// Token: 0x0400037A RID: 890
		private Settlement _settlement;

		// Token: 0x0400037B RID: 891
		private int _spawnedOpponentAgentCount;

		// Token: 0x0400037C RID: 892
		private int _aliveOpponentCount;

		// Token: 0x0400037D RID: 893
		private float _nextSpawnTime;

		// Token: 0x0400037E RID: 894
		private List<MatrixFrame> _initialSpawnFrames;

		// Token: 0x0400037F RID: 895
		private List<MatrixFrame> _spawnFrames;

		// Token: 0x04000380 RID: 896
		private List<Team> _AIParticipantTeams;

		// Token: 0x04000381 RID: 897
		private List<Agent> _participantAgents;

		// Token: 0x04000382 RID: 898
		private Team _tournamentMasterTeam;

		// Token: 0x04000383 RID: 899
		private BasicMissionTimer _teleportTimer;

		// Token: 0x04000384 RID: 900
		private List<CharacterObject> _participantCharacters;

		// Token: 0x0400038A RID: 906
		private const float XpShareForKill = 0.5f;

		// Token: 0x0400038B RID: 907
		private const float XpShareForDamage = 0.5f;
	}
}
