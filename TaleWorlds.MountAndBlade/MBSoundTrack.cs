using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DD RID: 477
	public struct MBSoundTrack
	{
		// Token: 0x06001C1E RID: 7198 RVA: 0x00061005 File Offset: 0x0005F205
		internal MBSoundTrack(int i)
		{
			this.index = i;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0006100E File Offset: 0x0005F20E
		public bool Equals(MBSoundTrack a)
		{
			return this.index == a.index;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0006101E File Offset: 0x0005F21E
		public override int GetHashCode()
		{
			return this.index;
		}

		// Token: 0x04000986 RID: 2438
		private int index;
	}
}
