using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D7 RID: 471
	public struct MBParticleSystem
	{
		// Token: 0x06001C07 RID: 7175 RVA: 0x00060DC5 File Offset: 0x0005EFC5
		internal MBParticleSystem(int i)
		{
			this.index = i;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x00060DCE File Offset: 0x0005EFCE
		public bool Equals(MBParticleSystem a)
		{
			return this.index == a.index;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x00060DDE File Offset: 0x0005EFDE
		public override int GetHashCode()
		{
			return this.index;
		}

		// Token: 0x0400096F RID: 2415
		private int index;
	}
}
