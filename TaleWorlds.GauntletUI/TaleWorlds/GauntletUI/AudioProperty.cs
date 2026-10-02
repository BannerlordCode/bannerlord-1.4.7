using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000030 RID: 48
	public class AudioProperty
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		[Editor(false)]
		public string AudioName { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000EFB9 File Offset: 0x0000D1B9
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000EFC1 File Offset: 0x0000D1C1
		[Editor(false)]
		public bool Delay { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000EFCA File Offset: 0x0000D1CA
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000EFD2 File Offset: 0x0000D1D2
		[Editor(false)]
		public float DelaySeconds { get; set; }

		// Token: 0x0600035B RID: 859 RVA: 0x0000EFDB File Offset: 0x0000D1DB
		public void FillFrom(AudioProperty audioProperty)
		{
			this.AudioName = audioProperty.AudioName;
			this.Delay = audioProperty.Delay;
			this.DelaySeconds = audioProperty.DelaySeconds;
		}
	}
}
