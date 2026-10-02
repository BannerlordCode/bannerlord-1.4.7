using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000189 RID: 393
	public class TeamAISiegeAttacker : TeamAISiegeComponent
	{
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0004DEBA File Offset: 0x0004C0BA
		public MBReadOnlyList<ArcherPosition> ArcherPositions
		{
			get
			{
				return this._archerPositions;
			}
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0004DEC4 File Offset: 0x0004C0C4
		public TeamAISiegeAttacker(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
			IEnumerable<GameEntity> enumerable = currentMission.Scene.FindEntitiesWithTag("archer_position_attacker");
			this._archerPositions = enumerable.Select<GameEntity, ArcherPosition>((GameEntity ap) => new ArcherPosition(ap, TeamAISiegeComponent.QuerySystem, BattleSideEnum.Attacker)).ToMBList<ArcherPosition>();
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0004DF20 File Offset: 0x0004C120
		public override void OnUnitAddedToFormationForTheFirstTime(Formation formation)
		{
			if (formation.AI.GetBehavior<BehaviorCharge>() == null)
			{
				formation.ForceCalculateCaches();
				if (formation.FormationIndex == FormationClass.NumberOfRegularFormations)
				{
					formation.AI.AddAiBehavior(new BehaviorGeneral(formation));
				}
				else if (formation.FormationIndex == FormationClass.Bodyguard)
				{
					formation.AI.AddAiBehavior(new BehaviorProtectGeneral(formation));
				}
				formation.AI.AddAiBehavior(new BehaviorCharge(formation));
				formation.AI.AddAiBehavior(new BehaviorPullBack(formation));
				formation.AI.AddAiBehavior(new BehaviorRegroup(formation));
				formation.AI.AddAiBehavior(new BehaviorReserve(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreat(formation));
				formation.AI.AddAiBehavior(new BehaviorStop(formation));
				formation.AI.AddAiBehavior(new BehaviorTacticalCharge(formation));
				formation.AI.AddAiBehavior(new BehaviorAssaultWalls(formation));
				formation.AI.AddAiBehavior(new BehaviorShootFromSiegeTower(formation));
				formation.AI.AddAiBehavior(new BehaviorUseSiegeMachines(formation));
				formation.AI.AddAiBehavior(new BehaviorWaitForLadders(formation));
				formation.AI.AddAiBehavior(new BehaviorSparseSkirmish(formation));
				formation.AI.AddAiBehavior(new BehaviorSkirmish(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreatToKeep(formation));
			}
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0004E068 File Offset: 0x0004C268
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			foreach (SiegeTower siegeTower in this.SiegeTowers)
			{
				base.DifficultNavmeshIDs.AddRange(siegeTower.CollectGetDifficultNavmeshIDsForAttackers());
			}
			foreach (ArcherPosition archerPosition in this._archerPositions)
			{
				archerPosition.OnDeploymentFinished(TeamAISiegeComponent.QuerySystem, BattleSideEnum.Attacker);
			}
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0004E110 File Offset: 0x0004C310
		public override void OnFormationFrameChanged(Agent agent, bool isFrameEnabled, WorldPosition frame)
		{
			base.OnFormationFrameChanged(agent, isFrameEnabled, frame);
			foreach (SiegeTower siegeTower in this.SiegeTowers)
			{
				if (agent.IsInLadderQueue || siegeTower.HasCompletedAction())
				{
					siegeTower.OnFormationFrameChanged(agent, isFrameEnabled, frame);
				}
			}
			foreach (SiegeLadder siegeLadder in base.Ladders)
			{
				if (agent.IsInLadderQueue || siegeLadder.State == SiegeLadder.LadderState.OnWall)
				{
					siegeLadder.OnFormationFrameChanged(agent, isFrameEnabled, frame);
				}
			}
		}

		// Token: 0x040005AB RID: 1451
		private readonly MBList<ArcherPosition> _archerPositions;
	}
}
