using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200001E RID: 30
	public class MultiplayerData : MBMultiplayerData
	{
		// Token: 0x06000199 RID: 409 RVA: 0x000076FA File Offset: 0x000058FA
		public MultiplayerData()
		{
			new List<NetworkCommunicator>();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007710 File Offset: 0x00005910
		public bool IsMultiplayerTeamAvailable(int peerNo, int teamNo)
		{
			return false;
		}

		// Token: 0x04000063 RID: 99
		public readonly int AutoTeamBalanceLimit = 50;
	}
}
