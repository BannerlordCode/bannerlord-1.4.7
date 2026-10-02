using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000187 RID: 391
	public class TeamAISallyOutAttacker : TeamAISiegeComponent
	{
		// Token: 0x060014E0 RID: 5344 RVA: 0x0004D87C File Offset: 0x0004BA7C
		public TeamAISallyOutAttacker(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
			this.ArcherPositions = currentMission.Scene.FindEntitiesWithTag("archer_position").ToMBList<GameEntity>();
			this.BesiegerRangedSiegeWeapons = new List<UsableMachine>(from w in currentMission.ActiveMissionObjects.FindAllWithType<RangedSiegeWeapon>()
				where w.Side == BattleSideEnum.Attacker && !w.IsDisabled
				select w);
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0004D8EC File Offset: 0x0004BAEC
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
				formation.AI.AddAiBehavior(new BehaviorShootFromCastleWalls(formation));
				formation.AI.AddAiBehavior(new BehaviorDestroySiegeWeapons(formation));
				formation.AI.AddAiBehavior(new BehaviorSparseSkirmish(formation));
				formation.AI.AddAiBehavior(new BehaviorDefend(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreatToCastle(formation));
				formation.AI.AddAiBehavior(new BehaviorRetreatToKeep(formation));
				formation.AI.AddAiBehavior(new BehaviorDefendCastleKeyPosition(formation));
			}
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0004DA34 File Offset: 0x0004BC34
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			if (base.CurrentTactic != null)
			{
				base.CurrentTactic.ResetTactic();
			}
		}

		// Token: 0x040005A7 RID: 1447
		public MBList<GameEntity> ArcherPositions;

		// Token: 0x040005A8 RID: 1448
		public readonly List<UsableMachine> BesiegerRangedSiegeWeapons;
	}
}
