using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000101 RID: 257
	[Serializable]
	public class ClanCreationProgress
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00006F08 File Offset: 0x00005108
		public Progress Progress
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (ClanCreationPlayerData clanCreationPlayerData2 in this.ClanCreationPlayerData)
				{
					if (clanCreationPlayerData2.ClanCreationAnswer == ClanCreationAnswer.Accepted)
					{
						num++;
					}
					else if (clanCreationPlayerData2.ClanCreationAnswer == ClanCreationAnswer.Declined)
					{
						num2++;
					}
				}
				if (num == this.ClanCreationPlayerData.Length)
				{
					return Progress.Success;
				}
				if (num2 > 0)
				{
					return Progress.Fail;
				}
				return Progress.Undecided;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00006F65 File Offset: 0x00005165
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x00006F6D File Offset: 0x0000516D
		public ClanCreationPlayerData[] ClanCreationPlayerData { get; private set; }

		// Token: 0x06000578 RID: 1400 RVA: 0x00006F76 File Offset: 0x00005176
		public ClanCreationProgress(ClanCreationPlayerData[] clanCreationPlayerData)
		{
			this.ClanCreationPlayerData = clanCreationPlayerData;
		}
	}
}
