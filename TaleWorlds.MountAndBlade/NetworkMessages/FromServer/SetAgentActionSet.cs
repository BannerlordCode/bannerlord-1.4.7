using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000099 RID: 153
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentActionSet : GameNetworkMessage
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0000AE3A File Offset: 0x0000903A
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0000AE42 File Offset: 0x00009042
		public int AgentIndex { get; private set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0000AE4B File Offset: 0x0000904B
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x0000AE53 File Offset: 0x00009053
		public MBActionSet ActionSet { get; private set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0000AE5C File Offset: 0x0000905C
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x0000AE64 File Offset: 0x00009064
		public int NumPaces { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0000AE6D File Offset: 0x0000906D
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x0000AE75 File Offset: 0x00009075
		public int MonsterUsageSetIndex { get; private set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0000AE7E File Offset: 0x0000907E
		// (set) Token: 0x06000619 RID: 1561 RVA: 0x0000AE86 File Offset: 0x00009086
		public float WalkingSpeedLimit { get; private set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0000AE8F File Offset: 0x0000908F
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x0000AE97 File Offset: 0x00009097
		public float CrouchWalkingSpeedLimit { get; private set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0000AEA0 File Offset: 0x000090A0
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public float StepSize { get; private set; }

		// Token: 0x0600061E RID: 1566 RVA: 0x0000AEB4 File Offset: 0x000090B4
		public SetAgentActionSet(int agentIndex, AnimationSystemData animationSystemData)
		{
			this.AgentIndex = agentIndex;
			this.ActionSet = animationSystemData.ActionSet;
			this.NumPaces = animationSystemData.NumPaces;
			this.MonsterUsageSetIndex = animationSystemData.MonsterUsageSetIndex;
			this.WalkingSpeedLimit = animationSystemData.WalkingSpeedLimit;
			this.CrouchWalkingSpeedLimit = animationSystemData.CrouchWalkingSpeedLimit;
			this.StepSize = animationSystemData.StepSize;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0000AF16 File Offset: 0x00009116
		public SetAgentActionSet()
		{
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0000AF20 File Offset: 0x00009120
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.ActionSet = GameNetworkMessage.ReadActionSetReferenceFromPacket(CompressionMission.ActionSetCompressionInfo, ref flag);
			this.NumPaces = GameNetworkMessage.ReadIntFromPacket(CompressionMission.NumberOfPacesCompressionInfo, ref flag);
			this.MonsterUsageSetIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MonsterUsageSetCompressionInfo, ref flag);
			this.WalkingSpeedLimit = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.WalkingSpeedLimitCompressionInfo, ref flag);
			this.CrouchWalkingSpeedLimit = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.WalkingSpeedLimitCompressionInfo, ref flag);
			this.StepSize = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.StepSizeCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0000AFAC File Offset: 0x000091AC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteActionSetReferenceToPacket(this.ActionSet, CompressionMission.ActionSetCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.NumPaces, CompressionMission.NumberOfPacesCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.MonsterUsageSetIndex, CompressionMission.MonsterUsageSetCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.WalkingSpeedLimit, CompressionMission.WalkingSpeedLimitCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.CrouchWalkingSpeedLimit, CompressionMission.WalkingSpeedLimitCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.StepSize, CompressionMission.StepSizeCompressionInfo);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0000B024 File Offset: 0x00009224
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentAnimations;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0000B02C File Offset: 0x0000922C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set ActionSet: ", this.ActionSet, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
