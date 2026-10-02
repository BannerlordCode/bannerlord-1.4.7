using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CF RID: 207
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UseObject : GameNetworkMessage
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0000E703 File Offset: 0x0000C903
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x0000E70B File Offset: 0x0000C90B
		public int AgentIndex { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0000E714 File Offset: 0x0000C914
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x0000E71C File Offset: 0x0000C91C
		public MissionObjectId UsableGameObjectId { get; private set; }

		// Token: 0x06000883 RID: 2179 RVA: 0x0000E725 File Offset: 0x0000C925
		public UseObject(int agentIndex, MissionObjectId usableGameObjectId)
		{
			this.AgentIndex = agentIndex;
			this.UsableGameObjectId = usableGameObjectId;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0000E73B File Offset: 0x0000C93B
		public UseObject()
		{
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0000E744 File Offset: 0x0000C944
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.UsableGameObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000E76E File Offset: 0x0000C96E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.UsableGameObjectId.Id >= 0) ? this.UsableGameObjectId : MissionObjectId.Invalid);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0000E79B File Offset: 0x0000C99B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents | MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
		protected override string OnGetLogFormat()
		{
			string text = "Use UsableMissionObject with ID: ";
			if (this.UsableGameObjectId != MissionObjectId.Invalid)
			{
				text += this.UsableGameObjectId;
			}
			else
			{
				text += "null";
			}
			text += " by Agent with name: ";
			if (this.AgentIndex >= 0)
			{
				text = text + "agent-index: " + this.AgentIndex;
			}
			else
			{
				text += "null";
			}
			return text;
		}
	}
}
