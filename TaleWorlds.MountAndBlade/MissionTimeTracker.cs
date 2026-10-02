using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A2 RID: 674
	public class MissionTimeTracker
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x000872E5 File Offset: 0x000854E5
		// (set) Token: 0x06002547 RID: 9543 RVA: 0x000872ED File Offset: 0x000854ED
		public long NumberOfTicks { get; private set; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x000872F6 File Offset: 0x000854F6
		// (set) Token: 0x06002549 RID: 9545 RVA: 0x000872FE File Offset: 0x000854FE
		public long DeltaTimeInTicks { get; private set; }

		// Token: 0x0600254A RID: 9546 RVA: 0x00087307 File Offset: 0x00085507
		public MissionTimeTracker(MissionTime initialMapTime)
		{
			this.NumberOfTicks = initialMapTime.NumberOfTicks;
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x0008731C File Offset: 0x0008551C
		public MissionTimeTracker()
		{
			this.NumberOfTicks = 0L;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x0008732C File Offset: 0x0008552C
		public void Tick(float seconds)
		{
			this.DeltaTimeInTicks = (long)(seconds * 10000000f);
			this.NumberOfTicks += this.DeltaTimeInTicks;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x00087350 File Offset: 0x00085550
		public void UpdateSync(float newValue)
		{
			long num = (long)(newValue * 10000000f);
			this._lastSyncDifference = num - this.NumberOfTicks;
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x00087374 File Offset: 0x00085574
		public float GetLastSyncDifference()
		{
			return (float)this._lastSyncDifference / 10000000f;
		}

		// Token: 0x04000E63 RID: 3683
		private long _lastSyncDifference;
	}
}
