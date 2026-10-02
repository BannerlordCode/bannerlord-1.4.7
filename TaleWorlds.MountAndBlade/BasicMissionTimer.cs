using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E5 RID: 485
	public class BasicMissionTimer
	{
		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x00061423 File Offset: 0x0005F623
		public float ElapsedTime
		{
			get
			{
				return MBCommon.GetTotalMissionTime() - this._startTime;
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x00061431 File Offset: 0x0005F631
		public BasicMissionTimer()
		{
			this._startTime = MBCommon.GetTotalMissionTime();
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x00061444 File Offset: 0x0005F644
		public void Reset()
		{
			this._startTime = MBCommon.GetTotalMissionTime();
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00061451 File Offset: 0x0005F651
		public void Set(float newElapsedTime)
		{
			this._startTime = MBCommon.GetTotalMissionTime() - newElapsedTime;
		}

		// Token: 0x0400098A RID: 2442
		private float _startTime;
	}
}
