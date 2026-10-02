using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002ED RID: 749
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefineGameNetworkMessageTypeForMod : Attribute
	{
		// Token: 0x06002AE4 RID: 10980 RVA: 0x000A5091 File Offset: 0x000A3291
		public DefineGameNetworkMessageTypeForMod(GameNetworkMessageSendType sendType)
		{
			this.SendType = sendType;
		}

		// Token: 0x040010BD RID: 4285
		public readonly GameNetworkMessageSendType SendType;
	}
}
