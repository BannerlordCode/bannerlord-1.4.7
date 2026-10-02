using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EE RID: 750
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class DefineGameNetworkMessageType : Attribute
	{
		// Token: 0x06002AE5 RID: 10981 RVA: 0x000A50A0 File Offset: 0x000A32A0
		public DefineGameNetworkMessageType(GameNetworkMessageSendType sendType)
		{
			this.SendType = sendType;
		}

		// Token: 0x040010BE RID: 4286
		public readonly GameNetworkMessageSendType SendType;
	}
}
