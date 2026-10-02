using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027D RID: 637
	public class BattleReinforcementsSpawnController : MissionLogic
	{
		// Token: 0x0600236A RID: 9066 RVA: 0x0007E124 File Offset: 0x0007C324
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<IMissionAgentSpawnLogic>();
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0007E140 File Offset: 0x0007C340
		public override void AfterStart()
		{
			foreach (Team team in base.Mission.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingEmpty)
				{
					formation.OnBeforeMovementOrderApplied += this.OnBeforeFormationMovementOrderApplied;
				}
			}
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0007E1DC File Offset: 0x0007C3DC
		public override void OnMissionTick(float dt)
		{
			for (int i = 0; i < 2; i++)
			{
				if (this._sideRequiresUpdate[i])
				{
					this.UpdateSide((BattleSideEnum)i);
					this._sideRequiresUpdate[i] = false;
				}
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0007E210 File Offset: 0x0007C410
		protected override void OnEndMission()
		{
			foreach (Team team in base.Mission.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingEmpty)
				{
					formation.OnBeforeMovementOrderApplied -= this.OnBeforeFormationMovementOrderApplied;
				}
			}
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0007E2AC File Offset: 0x0007C4AC
		private void UpdateSide(BattleSideEnum side)
		{
			if (this.IsBattleSideRetreating(side))
			{
				if (!this._sideReinforcementSuspended[(int)side] && this._missionAgentSpawnLogic.IsSideSpawnEnabled(side))
				{
					this._missionAgentSpawnLogic.StopSpawner(side);
					this._sideReinforcementSuspended[(int)side] = true;
					return;
				}
			}
			else if (this._sideReinforcementSuspended[(int)side])
			{
				this._missionAgentSpawnLogic.StartSpawner(side);
				this._sideReinforcementSuspended[(int)side] = false;
			}
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0007E314 File Offset: 0x0007C514
		private bool IsBattleSideRetreating(BattleSideEnum side)
		{
			bool flag = true;
			foreach (Team team in base.Mission.Teams)
			{
				if (team.Side == side)
				{
					foreach (Formation formation in team.FormationsIncludingEmpty)
					{
						if (formation.CountOfUnits > 0 && formation.GetReadonlyMovementOrderReference().OrderEnum != MovementOrder.MovementOrderEnum.Retreat)
						{
							flag = false;
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0007E3CC File Offset: 0x0007C5CC
		private unsafe void OnBeforeFormationMovementOrderApplied(Formation formation, MovementOrder.MovementOrderEnum orderEnum)
		{
			if (formation.GetReadonlyMovementOrderReference()->OrderEnum == MovementOrder.MovementOrderEnum.Retreat || orderEnum == MovementOrder.MovementOrderEnum.Retreat)
			{
				int side = (int)formation.Team.Side;
				this._sideRequiresUpdate[side] = true;
			}
		}

		// Token: 0x04000D9E RID: 3486
		private IMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x04000D9F RID: 3487
		private bool[] _sideReinforcementSuspended = new bool[2];

		// Token: 0x04000DA0 RID: 3488
		private bool[] _sideRequiresUpdate = new bool[2];
	}
}
