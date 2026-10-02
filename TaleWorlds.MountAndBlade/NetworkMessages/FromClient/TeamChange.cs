using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200001E RID: 30
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class TeamChange : GameNetworkMessage
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000338F File Offset: 0x0000158F
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00003397 File Offset: 0x00001597
		public bool AutoAssign { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000033A0 File Offset: 0x000015A0
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000033A8 File Offset: 0x000015A8
		public int TeamIndex { get; private set; }

		// Token: 0x060000E6 RID: 230 RVA: 0x000033B1 File Offset: 0x000015B1
		public TeamChange(bool autoAssign, int teamIndex)
		{
			this.AutoAssign = autoAssign;
			this.TeamIndex = teamIndex;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000033C7 File Offset: 0x000015C7
		public TeamChange()
		{
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000033D0 File Offset: 0x000015D0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AutoAssign = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (!this.AutoAssign)
			{
				this.TeamIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003407 File Offset: 0x00001607
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.AutoAssign);
			if (!this.AutoAssign)
			{
				GameNetworkMessage.WriteIntToPacket(this.TeamIndex, CompressionMission.TeamCompressionInfo);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000342C File Offset: 0x0000162C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003434 File Offset: 0x00001634
		protected override string OnGetLogFormat()
		{
			return "Changed team to: " + this.TeamIndex;
		}
	}
}
