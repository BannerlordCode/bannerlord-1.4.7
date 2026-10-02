using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000127 RID: 295
	[Serializable]
	public class AnotherPlayerData
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0000B85B File Offset: 0x00009A5B
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0000B863 File Offset: 0x00009A63
		public AnotherPlayerState PlayerState { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0000B86C File Offset: 0x00009A6C
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x0000B874 File Offset: 0x00009A74
		public int Experience { get; set; }

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000B87D File Offset: 0x00009A7D
		public AnotherPlayerData()
		{
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0000B885 File Offset: 0x00009A85
		public AnotherPlayerData(AnotherPlayerState anotherPlayerState, int anotherPlayerExperience)
		{
			this.PlayerState = anotherPlayerState;
			this.Experience = anotherPlayerExperience;
		}
	}
}
