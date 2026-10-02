using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A2 RID: 162
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetBoundariesState : GameNetworkMessage
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0000B73E File Offset: 0x0000993E
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x0000B746 File Offset: 0x00009946
		public bool IsOutside { get; private set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0000B74F File Offset: 0x0000994F
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x0000B757 File Offset: 0x00009957
		public float StateStartTimeInSeconds { get; private set; }

		// Token: 0x0600067A RID: 1658 RVA: 0x0000B760 File Offset: 0x00009960
		public SetBoundariesState()
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0000B768 File Offset: 0x00009968
		public SetBoundariesState(bool isOutside)
		{
			this.IsOutside = isOutside;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0000B777 File Offset: 0x00009977
		public SetBoundariesState(bool isOutside, long stateStartTimeInTicks)
			: this(isOutside)
		{
			this.StateStartTimeInSeconds = (float)stateStartTimeInTicks / 10000000f;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0000B78E File Offset: 0x0000998E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.IsOutside);
			if (this.IsOutside)
			{
				GameNetworkMessage.WriteFloatToPacket(this.StateStartTimeInSeconds, CompressionMatchmaker.MissionTimeCompressionInfo);
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0000B7B4 File Offset: 0x000099B4
		protected override bool OnRead()
		{
			bool flag = true;
			this.IsOutside = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (this.IsOutside)
			{
				this.StateStartTimeInSeconds = GameNetworkMessage.ReadFloatFromPacket(CompressionMatchmaker.MissionTimeCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0000B7EB File Offset: 0x000099EB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0000B7F3 File Offset: 0x000099F3
		protected override string OnGetLogFormat()
		{
			if (!this.IsOutside)
			{
				return "I am now inside the level boundaries";
			}
			return "I am now outside of the level boundaries";
		}
	}
}
