using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200008C RID: 140
	public class WhileEnteringSettlementBattleMissionController : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00023F0E File Offset: 0x0002210E
		public BattleSideEnum PlayerSide
		{
			get
			{
				return BattleSideEnum.None;
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00023F11 File Offset: 0x00022111
		public WhileEnteringSettlementBattleMissionController(IMissionTroopSupplier[] suppliers, int numberOfMaxTroopForPlayer, int numberOfMaxTroopForEnemy)
		{
			this._troopSuppliers = suppliers;
			this._numberOfMaxTroopForPlayer = numberOfMaxTroopForPlayer;
			this._numberOfMaxTroopForEnemy = numberOfMaxTroopForEnemy;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00023F2E File Offset: 0x0002212E
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._battleAgentLogic = Mission.Current.GetMissionBehavior<BattleAgentLogic>();
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00023F48 File Offset: 0x00022148
		public override void OnMissionTick(float dt)
		{
			if (!this._isMissionInitialized)
			{
				this.SpawnAgents();
				this._isMissionInitialized = true;
				return;
			}
			if (!this._troopsInitialized)
			{
				this._troopsInitialized = true;
				foreach (Agent agent in base.Mission.Agents)
				{
					this._battleAgentLogic.OnAgentBuild(agent, null);
				}
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00023FCC File Offset: 0x000221CC
		private void SpawnAgents()
		{
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("sp_outside_near_town_main_gate");
			IMissionTroopSupplier[] troopSuppliers = this._troopSuppliers;
			for (int i = 0; i < troopSuppliers.Length; i++)
			{
				foreach (IAgentOriginBase agentOriginBase in troopSuppliers[i].SupplyTroops(this._numberOfMaxTroopForPlayer + this._numberOfMaxTroopForEnemy).ToList<IAgentOriginBase>())
				{
					bool flag = agentOriginBase.IsUnderPlayersCommand || agentOriginBase.Troop.IsPlayerCharacter;
					if ((!flag || this._numberOfMaxTroopForPlayer >= this._playerSideSpawnedTroopCount) && (flag || this._numberOfMaxTroopForEnemy >= this._otherSideSpawnedTroopCount))
					{
						WorldFrame worldFrame = new WorldFrame(gameEntity.GetGlobalFrame().rotation, new WorldPosition(base.Mission.Scene, gameEntity.GetGlobalFrame().origin));
						if (!flag)
						{
							worldFrame.Origin.SetVec2(worldFrame.Origin.AsVec2 + worldFrame.Rotation.f.AsVec2 * 20f);
							worldFrame.Rotation.f = (gameEntity.GetGlobalFrame().origin.AsVec2 - worldFrame.Origin.AsVec2).ToVec3(0f);
							worldFrame.Origin.SetVec2(base.Mission.GetRandomPositionAroundPoint(worldFrame.Origin.GetNavMeshVec3(), 0f, 2.5f, false).AsVec2);
						}
						worldFrame.Rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
						bool isPlayerCharacter = agentOriginBase.Troop.IsPlayerCharacter;
						base.Mission.SpawnTroop(agentOriginBase, flag, false, isPlayerCharacter, false, 0, 0, true, false, new Vec3?(worldFrame.Origin.GetGroundVec3()), new Vec2?(worldFrame.Rotation.f.AsVec2), null, null, FormationClass.NumberOfAllFormations, false).Defensiveness = 1f;
						if (flag)
						{
							this._playerSideSpawnedTroopCount++;
						}
						else
						{
							this._otherSideSpawnedTroopCount++;
						}
					}
				}
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00024228 File Offset: 0x00022428
		public void StartSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0002422A File Offset: 0x0002242A
		public void StopSpawner(BattleSideEnum side)
		{
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0002422C File Offset: 0x0002242C
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return false;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0002422F File Offset: 0x0002242F
		public float GetReinforcementInterval(BattleSideEnum side = BattleSideEnum.None)
		{
			return 0f;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00024236 File Offset: 0x00022436
		public bool IsSideDepleted(BattleSideEnum side)
		{
			if (side == base.Mission.PlayerTeam.Side)
			{
				return this._troopSuppliers[(int)side].NumRemovedTroops == this._playerSideSpawnedTroopCount;
			}
			return this._troopSuppliers[(int)side].NumRemovedTroops == this._otherSideSpawnedTroopCount;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00024276 File Offset: 0x00022476
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0002427D File Offset: 0x0002247D
		public int GetNumberOfPlayerControllableTroops()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00024284 File Offset: 0x00022484
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			return false;
		}

		// Token: 0x040002D3 RID: 723
		private const int GuardSpawnPointAndPlayerSpawnPointPositionDelta = 20;

		// Token: 0x040002D4 RID: 724
		private BattleAgentLogic _battleAgentLogic;

		// Token: 0x040002D5 RID: 725
		private bool _isMissionInitialized;

		// Token: 0x040002D6 RID: 726
		private bool _troopsInitialized;

		// Token: 0x040002D7 RID: 727
		private int _numberOfMaxTroopForPlayer;

		// Token: 0x040002D8 RID: 728
		private int _numberOfMaxTroopForEnemy;

		// Token: 0x040002D9 RID: 729
		private int _playerSideSpawnedTroopCount;

		// Token: 0x040002DA RID: 730
		private int _otherSideSpawnedTroopCount;

		// Token: 0x040002DB RID: 731
		private readonly IMissionTroopSupplier[] _troopSuppliers;
	}
}
