using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.DividableTasks
{
	// Token: 0x02000402 RID: 1026
	public class FindMostDangerousThreat : DividableTask
	{
		// Token: 0x060037BA RID: 14266 RVA: 0x000E505E File Offset: 0x000E325E
		public FindMostDangerousThreat(DividableTask continueToTask = null)
			: base(continueToTask)
		{
			base.SetTaskFinished(false);
			this._formationSearchThreatTask = new FormationSearchThreatTask();
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000E507C File Offset: 0x000E327C
		protected override bool UpdateExtra()
		{
			bool flag = false;
			if (this._hasOngoingThreatTask)
			{
				if (this._formationSearchThreatTask.Update())
				{
					this._hasOngoingThreatTask = false;
					if (!(flag = this._formationSearchThreatTask.GetResult(out this._targetAgent)))
					{
						this._threats.Remove(this._currentThreat);
						this._currentThreat = null;
					}
				}
			}
			else
			{
				int num = 5;
				for (;;)
				{
					num--;
					flag = true;
					int num2 = -1;
					float num3 = float.MinValue;
					bool flag2 = false;
					for (int i = 0; i < this._threats.Count; i++)
					{
						Threat threat = this._threats[i];
						if (!flag2 || threat.ForceTarget)
						{
							if (!flag2 && threat.ForceTarget)
							{
								flag2 = true;
								num3 = threat.ThreatValue;
								num2 = i;
							}
							else if (threat.ThreatValue > num3)
							{
								num3 = threat.ThreatValue;
								num2 = i;
							}
						}
					}
					if (num2 >= 0)
					{
						this._currentThreat = this._threats[num2];
						if (this._currentThreat.Formation != null)
						{
							break;
						}
						if ((this._currentThreat.TargetableObject == null && this._currentThreat.Agent == null) || !this._weapon.CanShootAtThreat(this._currentThreat, 5))
						{
							if (!this._currentThreat.ForceTarget)
							{
								this._threats.RemoveAt(num2);
							}
							this._currentThreat = null;
							flag = false;
						}
					}
					if (flag || num <= 0)
					{
						goto IL_0179;
					}
				}
				this._formationSearchThreatTask.Prepare(this._currentThreat.Formation, this._weapon);
				this._hasOngoingThreatTask = true;
				flag = false;
			}
			IL_0179:
			return flag || this._threats.Count == 0;
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x000E5218 File Offset: 0x000E3418
		public void Prepare(List<Threat> threats, RangedSiegeWeapon weapon)
		{
			base.ResetTaskStatus();
			this._hasOngoingThreatTask = false;
			this._weapon = weapon;
			this._threats = threats;
			foreach (Threat threat in this._threats)
			{
				threat.ThreatValue *= 0.9f + MBRandom.RandomFloat * 0.2f;
			}
			if (this._currentThreat != null)
			{
				this._currentThreat = this._threats.SingleOrDefault<Threat>((Threat t) => t.Equals(this._currentThreat));
				if (this._currentThreat != null)
				{
					this._currentThreat.ThreatValue *= 2f;
				}
			}
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x000E52E0 File Offset: 0x000E34E0
		public Threat GetResult(out Agent targetAgent)
		{
			targetAgent = this._targetAgent;
			return this._currentThreat;
		}

		// Token: 0x040017DB RID: 6107
		private Agent _targetAgent;

		// Token: 0x040017DC RID: 6108
		private FormationSearchThreatTask _formationSearchThreatTask;

		// Token: 0x040017DD RID: 6109
		private List<Threat> _threats;

		// Token: 0x040017DE RID: 6110
		private RangedSiegeWeapon _weapon;

		// Token: 0x040017DF RID: 6111
		private Threat _currentThreat;

		// Token: 0x040017E0 RID: 6112
		private bool _hasOngoingThreatTask;
	}
}
