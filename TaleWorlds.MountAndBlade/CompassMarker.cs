using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000255 RID: 597
	public class CompassMarker
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x000767F6 File Offset: 0x000749F6
		// (set) Token: 0x060021C9 RID: 8649 RVA: 0x000767FE File Offset: 0x000749FE
		public string Id { get; private set; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x00076807 File Offset: 0x00074A07
		// (set) Token: 0x060021CB RID: 8651 RVA: 0x0007680F File Offset: 0x00074A0F
		public float Angle { get; private set; }

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x00076818 File Offset: 0x00074A18
		// (set) Token: 0x060021CD RID: 8653 RVA: 0x00076820 File Offset: 0x00074A20
		public bool IsPrimary { get; private set; }

		// Token: 0x060021CE RID: 8654 RVA: 0x00076829 File Offset: 0x00074A29
		public CompassMarker(string id, float angle, bool isPrimary)
		{
			this.Id = id;
			this.Angle = angle % 360f;
			this.IsPrimary = isPrimary;
		}
	}
}
