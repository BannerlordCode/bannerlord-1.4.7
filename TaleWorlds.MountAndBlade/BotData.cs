using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002AE RID: 686
	public class BotData
	{
		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x0008E8F9 File Offset: 0x0008CAF9
		public int Score
		{
			get
			{
				return this.KillCount * 3 + this.AssistCount;
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0008E90A File Offset: 0x0008CB0A
		public BotData()
		{
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0008E912 File Offset: 0x0008CB12
		public BotData(int kill, int assist, int death, int alive)
		{
			this.KillCount = kill;
			this.DeathCount = death;
			this.AssistCount = assist;
			this.AliveCount = alive;
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002687 RID: 9863 RVA: 0x0008E937 File Offset: 0x0008CB37
		public bool IsAnyValid
		{
			get
			{
				return this.KillCount != 0 || this.DeathCount != 0 || this.AssistCount != 0 || this.AliveCount != 0;
			}
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0008E95C File Offset: 0x0008CB5C
		public void ResetKillDeathAssist()
		{
			this.KillCount = 0;
			this.DeathCount = 0;
			this.AssistCount = 0;
		}

		// Token: 0x04000E9E RID: 3742
		public int AliveCount;

		// Token: 0x04000E9F RID: 3743
		public int KillCount;

		// Token: 0x04000EA0 RID: 3744
		public int DeathCount;

		// Token: 0x04000EA1 RID: 3745
		public int AssistCount;
	}
}
