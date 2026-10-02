using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000045 RID: 69
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagRaisingStatus : GameNetworkMessage
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00004B29 File Offset: 0x00002D29
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00004B31 File Offset: 0x00002D31
		public float Progress { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00004B3A File Offset: 0x00002D3A
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00004B42 File Offset: 0x00002D42
		public CaptureTheFlagFlagDirection Direction { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00004B4B File Offset: 0x00002D4B
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00004B53 File Offset: 0x00002D53
		public float Speed { get; private set; }

		// Token: 0x0600023D RID: 573 RVA: 0x00004B5C File Offset: 0x00002D5C
		public FlagRaisingStatus()
		{
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00004B64 File Offset: 0x00002D64
		public FlagRaisingStatus(float currProgress, CaptureTheFlagFlagDirection direction, float speed)
		{
			this.Progress = currProgress;
			this.Direction = direction;
			this.Speed = speed;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00004B84 File Offset: 0x00002D84
		protected override bool OnRead()
		{
			bool flag = true;
			this.Progress = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagClassicProgressCompressionInfo, ref flag);
			this.Direction = (CaptureTheFlagFlagDirection)GameNetworkMessage.ReadIntFromPacket(CompressionMission.FlagDirectionEnumCompressionInfo, ref flag);
			if (flag && this.Direction != CaptureTheFlagFlagDirection.None && this.Direction != CaptureTheFlagFlagDirection.Static)
			{
				this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagSpeedCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00004BE0 File Offset: 0x00002DE0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteFloatToPacket(this.Progress, CompressionMission.FlagClassicProgressCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.Direction, CompressionMission.FlagDirectionEnumCompressionInfo);
			if (this.Direction != CaptureTheFlagFlagDirection.None && this.Direction != CaptureTheFlagFlagDirection.Static)
			{
				GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionMission.FlagSpeedCompressionInfo);
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00004C2F File Offset: 0x00002E2F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00004C38 File Offset: 0x00002E38
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Updating flag movement: Progress: ", this.Progress, ", Direction: ", this.Direction, ", Speed: ", this.Speed });
		}
	}
}
