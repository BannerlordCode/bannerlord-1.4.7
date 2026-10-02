using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A1 RID: 673
	public class MissionTimer
	{
		// Token: 0x0600253C RID: 9532 RVA: 0x000871D5 File Offset: 0x000853D5
		private MissionTimer()
		{
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000871DD File Offset: 0x000853DD
		public MissionTimer(float duration)
		{
			this._startTime = MissionTime.Now;
			this._duration = duration;
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000871F7 File Offset: 0x000853F7
		public MissionTime GetStartTime()
		{
			return this._startTime;
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000871FF File Offset: 0x000853FF
		public float GetTimerDuration()
		{
			return this._duration;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x00087208 File Offset: 0x00085408
		public float GetRemainingTimeInSeconds(bool synched = false)
		{
			if (this._duration < 0f)
			{
				return 0f;
			}
			float num = this._duration - this._startTime.ElapsedSeconds;
			if (synched && GameNetwork.IsClientOrReplay)
			{
				num -= Mission.Current.MissionTimeTracker.GetLastSyncDifference();
			}
			if (num <= 0f)
			{
				return 0f;
			}
			return num;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x00087266 File Offset: 0x00085466
		public bool Check(bool reset = false)
		{
			bool flag = this.GetRemainingTimeInSeconds(false) <= 0f;
			if (flag && reset)
			{
				this._startTime = MissionTime.Now;
			}
			return flag;
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x00087289 File Offset: 0x00085489
		public void Reset()
		{
			this._startTime = MissionTime.Now;
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x00087296 File Offset: 0x00085496
		public void Set(float timeInSeconds)
		{
			this._startTime = new MissionTime(Mission.Current.MissionTimeTracker.NumberOfTicks + (long)(timeInSeconds * 10000000f));
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000872BB File Offset: 0x000854BB
		public void SetDuration(float duration)
		{
			this._duration = duration;
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000872C4 File Offset: 0x000854C4
		public static MissionTimer CreateSynchedTimerClient(float startTimeInSeconds, float duration)
		{
			return new MissionTimer
			{
				_startTime = new MissionTime((long)(startTimeInSeconds * 10000000f)),
				_duration = duration
			};
		}

		// Token: 0x04000E5F RID: 3679
		private MissionTime _startTime;

		// Token: 0x04000E60 RID: 3680
		private float _duration;
	}
}
