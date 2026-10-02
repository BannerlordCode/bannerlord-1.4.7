using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017E RID: 382
	public class AttackEntityOrderSecondaryDetachment
	{
		// Token: 0x0600145A RID: 5210 RVA: 0x0004B6B2 File Offset: 0x000498B2
		public AttackEntityOrderSecondaryDetachment(GameEntity targetEntity)
		{
			this._targetEntity = targetEntity;
			this._surroundEntity = this._targetEntity.GetFirstScriptOfType<CastleGate>() == null;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0004B6D8 File Offset: 0x000498D8
		public void TickOccasionally(Formation formation)
		{
			foreach (IFormationUnit formationUnit in formation.Arrangement.GetAllUnits())
			{
				((Agent)formationUnit).SetScriptedTargetEntity(this._targetEntity.WeakEntity, this._surroundEntity ? Agent.AISpecialCombatModeFlags.SurroundAttackEntity : Agent.AISpecialCombatModeFlags.None, true);
			}
			foreach (Agent agent in formation.DetachedUnits)
			{
				if (agent.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent.DisableScriptedCombatMovement();
				}
			}
			foreach (Agent agent2 in formation.LooseDetachedUnits)
			{
				if (agent2.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent2.DisableScriptedCombatMovement();
				}
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0004B7E8 File Offset: 0x000499E8
		public void Disband(Formation formation)
		{
			foreach (IFormationUnit formationUnit in formation.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				if (agent.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent.DisableScriptedCombatMovement();
				}
			}
			foreach (Agent agent2 in formation.DetachedUnits)
			{
				if (agent2.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent2.DisableScriptedCombatMovement();
				}
			}
			foreach (Agent agent3 in formation.LooseDetachedUnits)
			{
				if (agent3.GetScriptedCombatFlags().HasAnyFlag(Agent.AISpecialCombatModeFlags.AttackEntity))
				{
					agent3.DisableScriptedCombatMovement();
				}
			}
		}

		// Token: 0x04000570 RID: 1392
		private readonly GameEntity _targetEntity;

		// Token: 0x04000571 RID: 1393
		private readonly bool _surroundEntity;
	}
}
