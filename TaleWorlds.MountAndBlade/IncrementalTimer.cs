using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000249 RID: 585
	public class IncrementalTimer
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x00075716 File Offset: 0x00073916
		// (set) Token: 0x06002196 RID: 8598 RVA: 0x0007571E File Offset: 0x0007391E
		public float TimerCounter { get; private set; }

		// Token: 0x06002197 RID: 8599 RVA: 0x00075728 File Offset: 0x00073928
		public IncrementalTimer(float totalDuration, float tickInterval)
		{
			this._tickInterval = MathF.Max(tickInterval, 0.01f);
			this._totalDuration = MathF.Max(totalDuration, 0.01f);
			this.TimerCounter = 0f;
			this._timer = new Timer(MBCommon.GetTotalMissionTime(), this._tickInterval, true);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x0007577F File Offset: 0x0007397F
		public bool Check()
		{
			if (this._timer.Check(MBCommon.GetTotalMissionTime()))
			{
				this.TimerCounter += this._tickInterval / this._totalDuration;
				return true;
			}
			return false;
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x000757B0 File Offset: 0x000739B0
		public bool HasEnded()
		{
			return this.TimerCounter >= 1f;
		}

		// Token: 0x04000CE6 RID: 3302
		private readonly float _totalDuration;

		// Token: 0x04000CE7 RID: 3303
		private readonly float _tickInterval;

		// Token: 0x04000CE8 RID: 3304
		private readonly Timer _timer;
	}
}
