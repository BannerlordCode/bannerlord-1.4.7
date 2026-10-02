using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000299 RID: 665
	public class SallyOutReinforcementSpawnTimer : ICustomReinforcementSpawnTimer
	{
		// Token: 0x060024CF RID: 9423 RVA: 0x00085BE1 File Offset: 0x00083DE1
		public SallyOutReinforcementSpawnTimer(float besiegedInterval, float besiegerInterval, float besiegerIntervalChange, int besiegerIntervalChangeCount)
		{
			this._besiegedSideTimer = new BasicMissionTimer();
			this._besiegedInterval = besiegedInterval;
			this._besiegerSideTimer = new BasicMissionTimer();
			this._besiegerInterval = besiegerInterval;
			this._besiegerIntervalChange = besiegerIntervalChange;
			this._besiegerRemainingIntervalChanges = besiegerIntervalChangeCount;
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x00085C1C File Offset: 0x00083E1C
		public bool Check(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Attacker)
			{
				if (this._besiegerSideTimer.ElapsedTime >= this._besiegerInterval)
				{
					if (this._besiegerRemainingIntervalChanges > 0)
					{
						this._besiegerInterval -= this._besiegerIntervalChange;
						this._besiegerRemainingIntervalChanges--;
					}
					this._besiegerSideTimer.Reset();
					return true;
				}
			}
			else if (side == BattleSideEnum.Defender && this._besiegedSideTimer.ElapsedTime >= this._besiegedInterval)
			{
				this._besiegedSideTimer.Reset();
				return true;
			}
			return false;
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00085C9B File Offset: 0x00083E9B
		public void ResetTimer(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Attacker)
			{
				this._besiegerSideTimer.Reset();
				return;
			}
			if (side == BattleSideEnum.Defender)
			{
				this._besiegedSideTimer.Reset();
			}
		}

		// Token: 0x04000E45 RID: 3653
		private BasicMissionTimer _besiegedSideTimer;

		// Token: 0x04000E46 RID: 3654
		private BasicMissionTimer _besiegerSideTimer;

		// Token: 0x04000E47 RID: 3655
		private float _besiegedInterval;

		// Token: 0x04000E48 RID: 3656
		private float _besiegerInterval;

		// Token: 0x04000E49 RID: 3657
		private float _besiegerIntervalChange;

		// Token: 0x04000E4A RID: 3658
		private int _besiegerRemainingIntervalChanges;
	}
}
