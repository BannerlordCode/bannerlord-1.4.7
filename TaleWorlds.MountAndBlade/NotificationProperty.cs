using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000320 RID: 800
	[AttributeUsage(AttributeTargets.Field)]
	public class NotificationProperty : Attribute
	{
		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002D85 RID: 11653 RVA: 0x000AFF25 File Offset: 0x000AE125
		// (set) Token: 0x06002D86 RID: 11654 RVA: 0x000AFF2D File Offset: 0x000AE12D
		public string StringId { get; private set; }

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002D87 RID: 11655 RVA: 0x000AFF36 File Offset: 0x000AE136
		// (set) Token: 0x06002D88 RID: 11656 RVA: 0x000AFF3E File Offset: 0x000AE13E
		public string SoundIdOne { get; private set; }

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002D89 RID: 11657 RVA: 0x000AFF47 File Offset: 0x000AE147
		// (set) Token: 0x06002D8A RID: 11658 RVA: 0x000AFF4F File Offset: 0x000AE14F
		public string SoundIdTwo { get; private set; }

		// Token: 0x06002D8B RID: 11659 RVA: 0x000AFF58 File Offset: 0x000AE158
		public NotificationProperty(string stringId, string soundIdOne, string soundIdTwo = "")
		{
			this.StringId = stringId;
			this.SoundIdOne = soundIdOne;
			this.SoundIdTwo = soundIdTwo;
		}
	}
}
