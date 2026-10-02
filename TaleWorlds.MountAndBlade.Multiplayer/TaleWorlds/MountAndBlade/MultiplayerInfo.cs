using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000021 RID: 33
	public class MultiplayerInfo
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007C44 File Offset: 0x00005E44
		public MultiplayerData MultiplayerDataValues
		{
			get
			{
				return this.multiplayerDataValues;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007C4C File Offset: 0x00005E4C
		public MultiplayerInfo()
		{
			this.multiplayerDataValues = new MultiplayerData();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007C5F File Offset: 0x00005E5F
		public bool IsMultiplayerTeamAvailable(int peerNo, int teamNo)
		{
			return true;
		}

		// Token: 0x04000064 RID: 100
		protected MultiplayerData multiplayerDataValues;
	}
}
