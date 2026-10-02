using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D6 RID: 470
	public struct MBMusicTrack
	{
		// Token: 0x06001C02 RID: 7170 RVA: 0x00060D88 File Offset: 0x0005EF88
		public MBMusicTrack(MBMusicTrack obj)
		{
			this.index = obj.index;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x00060D96 File Offset: 0x0005EF96
		internal MBMusicTrack(int i)
		{
			this.index = i;
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x00060D9F File Offset: 0x0005EF9F
		private bool IsValid
		{
			get
			{
				return this.index >= 0;
			}
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x00060DAD File Offset: 0x0005EFAD
		public bool Equals(MBMusicTrack obj)
		{
			return this.index == obj.index;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x00060DBD File Offset: 0x0005EFBD
		public override int GetHashCode()
		{
			return this.index;
		}

		// Token: 0x0400096E RID: 2414
		private int index;
	}
}
