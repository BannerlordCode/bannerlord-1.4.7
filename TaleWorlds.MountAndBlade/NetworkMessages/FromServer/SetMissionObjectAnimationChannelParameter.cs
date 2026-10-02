using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A5 RID: 165
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectAnimationChannelParameter : GameNetworkMessage
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0000BA92 File Offset: 0x00009C92
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x0000BA9A File Offset: 0x00009C9A
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0000BAA3 File Offset: 0x00009CA3
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x0000BAAB File Offset: 0x00009CAB
		public int ChannelNo { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0000BAB4 File Offset: 0x00009CB4
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0000BABC File Offset: 0x00009CBC
		public float Parameter { get; private set; }

		// Token: 0x060006A1 RID: 1697 RVA: 0x0000BAC5 File Offset: 0x00009CC5
		public SetMissionObjectAnimationChannelParameter(MissionObjectId missionObjectId, int channelNo, float parameter)
		{
			this.MissionObjectId = missionObjectId;
			this.ChannelNo = channelNo;
			this.Parameter = parameter;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0000BAE2 File Offset: 0x00009CE2
		public SetMissionObjectAnimationChannelParameter()
		{
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0000BAEC File Offset: 0x00009CEC
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			if (flag)
			{
				this.ChannelNo = (flag2 ? 1 : 0);
			}
			this.Parameter = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0000BB33 File Offset: 0x00009D33
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.ChannelNo == 1);
			GameNetworkMessage.WriteFloatToPacket(this.Parameter, CompressionBasic.AnimationProgressCompressionInfo);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0000BB5E File Offset: 0x00009D5E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0000BB68 File Offset: 0x00009D68
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set animation parameter: ", this.Parameter, " on channel: ", this.ChannelNo, " of MissionObject with ID: ", this.MissionObjectId });
		}
	}
}
