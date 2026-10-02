using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D2 RID: 210
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.DebugFromServer)]
	internal sealed class DebugAgentScaleOnNetworkTest : GameNetworkMessage
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0000EA0E File Offset: 0x0000CC0E
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x0000EA16 File Offset: 0x0000CC16
		internal int AgentToTestIndex { get; private set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0000EA1F File Offset: 0x0000CC1F
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0000EA27 File Offset: 0x0000CC27
		internal float ScaleToTest { get; private set; }

		// Token: 0x060008A3 RID: 2211 RVA: 0x0000EA30 File Offset: 0x0000CC30
		public DebugAgentScaleOnNetworkTest()
		{
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0000EA38 File Offset: 0x0000CC38
		internal DebugAgentScaleOnNetworkTest(int agentToTestIndex, float scale)
		{
			this.AgentToTestIndex = agentToTestIndex;
			this.ScaleToTest = scale;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0000EA50 File Offset: 0x0000CC50
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentToTestIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.ScaleToTest = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.DebugScaleValueCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0000EA7F File Offset: 0x0000CC7F
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentToTestIndex);
			GameNetworkMessage.WriteFloatToPacket(this.ScaleToTest, CompressionMission.DebugScaleValueCompressionInfo);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000EA9C File Offset: 0x0000CC9C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000EAA4 File Offset: 0x0000CCA4
		protected override string OnGetLogFormat()
		{
			return "DebugAgentScaleOnNetworkTest";
		}
	}
}
