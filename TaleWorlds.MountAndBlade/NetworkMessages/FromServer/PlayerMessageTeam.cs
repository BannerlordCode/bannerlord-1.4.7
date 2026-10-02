using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200003F RID: 63
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PlayerMessageTeam : GameNetworkMessage
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000046C9 File Offset: 0x000028C9
		// (set) Token: 0x060001FF RID: 511 RVA: 0x000046D1 File Offset: 0x000028D1
		public string Message { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000046DA File Offset: 0x000028DA
		// (set) Token: 0x06000201 RID: 513 RVA: 0x000046E2 File Offset: 0x000028E2
		public NetworkCommunicator Player { get; private set; }

		// Token: 0x06000202 RID: 514 RVA: 0x000046EB File Offset: 0x000028EB
		public PlayerMessageTeam(NetworkCommunicator player, string message)
		{
			this.Player = player;
			this.Message = message;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00004701 File Offset: 0x00002901
		public PlayerMessageTeam()
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00004709 File Offset: 0x00002909
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Player);
			GameNetworkMessage.WriteStringToPacket(this.Message);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00004724 File Offset: 0x00002924
		protected override bool OnRead()
		{
			bool flag = true;
			this.Player = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000474F File Offset: 0x0000294F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00004754 File Offset: 0x00002954
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Receiving team message: ",
				this.Message,
				" from peer: ",
				this.Player.UserName,
				" index: ",
				this.Player.Index
			});
		}
	}
}
