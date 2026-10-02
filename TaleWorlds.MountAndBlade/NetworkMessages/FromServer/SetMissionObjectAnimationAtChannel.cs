using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A4 RID: 164
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectAnimationAtChannel : GameNetworkMessage
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0000B8ED File Offset: 0x00009AED
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x0000B8F5 File Offset: 0x00009AF5
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0000B8FE File Offset: 0x00009AFE
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x0000B906 File Offset: 0x00009B06
		public int ChannelNo { get; private set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0000B90F File Offset: 0x00009B0F
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0000B917 File Offset: 0x00009B17
		public int AnimationIndex { get; private set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0000B920 File Offset: 0x00009B20
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0000B928 File Offset: 0x00009B28
		public float AnimationSpeed { get; private set; }

		// Token: 0x06000695 RID: 1685 RVA: 0x0000B931 File Offset: 0x00009B31
		public SetMissionObjectAnimationAtChannel(MissionObjectId missionObjectId, int channelNo, int animationIndex, float animationSpeed)
		{
			this.MissionObjectId = missionObjectId;
			this.ChannelNo = channelNo;
			this.AnimationIndex = animationIndex;
			this.AnimationSpeed = animationSpeed;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0000B956 File Offset: 0x00009B56
		public SetMissionObjectAnimationAtChannel()
		{
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0000B960 File Offset: 0x00009B60
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.ChannelNo = (GameNetworkMessage.ReadBoolFromPacket(ref flag) ? 1 : 0);
			this.AnimationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationIndexCompressionInfo, ref flag);
			this.AnimationSpeed = (GameNetworkMessage.ReadBoolFromPacket(ref flag) ? GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationSpeedCompressionInfo, ref flag) : 1f);
			return flag;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.ChannelNo == 1);
			GameNetworkMessage.WriteIntToPacket(this.AnimationIndex, CompressionBasic.AnimationIndexCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.AnimationSpeed != 1f);
			if (this.AnimationSpeed != 1f)
			{
				GameNetworkMessage.WriteFloatToPacket(this.AnimationSpeed, CompressionBasic.AnimationSpeedCompressionInfo);
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0000BA30 File Offset: 0x00009C30
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0000BA38 File Offset: 0x00009C38
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set animation: ", this.AnimationIndex, " on channel: ", this.ChannelNo, " of MissionObject with ID: ", this.MissionObjectId });
		}
	}
}
