using System;

namespace TaleWorlds.MountAndBlade.DividableTasks
{
	// Token: 0x02000403 RID: 1027
	public class FormationSearchThreatTask : DividableTask
	{
		// Token: 0x060037BF RID: 14271 RVA: 0x000E5300 File Offset: 0x000E3500
		protected override bool UpdateExtra()
		{
			this._result = this._formation.HasUnitWithConditionLimitedRandom((Agent agent) => this._weapon.CanShootAtAgent(agent, 5), this._storedIndex, this._checkCountPerTick, out this._targetAgent);
			this._storedIndex += this._checkCountPerTick;
			return this._storedIndex >= this._formation.CountOfUnits || this._result;
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x000E536A File Offset: 0x000E356A
		public void Prepare(Formation formation, RangedSiegeWeapon weapon)
		{
			base.ResetTaskStatus();
			this._formation = formation;
			this._weapon = weapon;
			this._storedIndex = 0;
			this._checkCountPerTick = (int)((float)this._formation.CountOfUnits * 0.1f) + 1;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x000E53A2 File Offset: 0x000E35A2
		public bool GetResult(out Agent targetAgent)
		{
			targetAgent = this._targetAgent;
			return this._result;
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000E53B2 File Offset: 0x000E35B2
		public FormationSearchThreatTask()
			: base(null)
		{
		}

		// Token: 0x040017E1 RID: 6113
		private Agent _targetAgent;

		// Token: 0x040017E2 RID: 6114
		private const float CheckCountRatio = 0.1f;

		// Token: 0x040017E3 RID: 6115
		private RangedSiegeWeapon _weapon;

		// Token: 0x040017E4 RID: 6116
		private Formation _formation;

		// Token: 0x040017E5 RID: 6117
		private int _storedIndex;

		// Token: 0x040017E6 RID: 6118
		private int _checkCountPerTick;

		// Token: 0x040017E7 RID: 6119
		private bool _result;
	}
}
