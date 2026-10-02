using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000028 RID: 40
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ApplySiegeWeaponOrder : GameNetworkMessage
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00003B76 File Offset: 0x00001D76
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00003B7E File Offset: 0x00001D7E
		public SiegeWeaponOrderType OrderType { get; private set; }

		// Token: 0x06000145 RID: 325 RVA: 0x00003B87 File Offset: 0x00001D87
		public ApplySiegeWeaponOrder(SiegeWeaponOrderType orderType)
		{
			this.OrderType = orderType;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00003B96 File Offset: 0x00001D96
		public ApplySiegeWeaponOrder()
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00003BA0 File Offset: 0x00001DA0
		protected override bool OnRead()
		{
			bool flag = true;
			this.OrderType = (SiegeWeaponOrderType)GameNetworkMessage.ReadIntFromPacket(CompressionMission.OrderTypeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00003BC2 File Offset: 0x00001DC2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.OrderType, CompressionMission.OrderTypeCompressionInfo);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00003BD4 File Offset: 0x00001DD4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed | MultiplayerMessageFilter.Orders;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00003BDC File Offset: 0x00001DDC
		protected override string OnGetLogFormat()
		{
			return "Apply siege order: " + this.OrderType;
		}
	}
}
