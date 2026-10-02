using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000038 RID: 56
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SyncRelevantGameOptionsToServer : GameNetworkMessage
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000419D File Offset: 0x0000239D
		// (set) Token: 0x060001BA RID: 442 RVA: 0x000041A5 File Offset: 0x000023A5
		public bool SendMeBloodEvents { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000041AE File Offset: 0x000023AE
		// (set) Token: 0x060001BC RID: 444 RVA: 0x000041B6 File Offset: 0x000023B6
		public bool SendMeSoundEvents { get; private set; }

		// Token: 0x060001BD RID: 445 RVA: 0x000041BF File Offset: 0x000023BF
		public SyncRelevantGameOptionsToServer()
		{
			this.SendMeBloodEvents = true;
			this.SendMeSoundEvents = true;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000041D5 File Offset: 0x000023D5
		public void InitializeOptions()
		{
			this.SendMeBloodEvents = BannerlordConfig.ShowBlood;
			this.SendMeSoundEvents = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.SoundVolume) > 0.01f && NativeOptions.GetConfig(NativeOptions.NativeOptionsType.MasterVolume) > 0.01f;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00004208 File Offset: 0x00002408
		protected override bool OnRead()
		{
			bool flag = true;
			this.SendMeBloodEvents = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.SendMeSoundEvents = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00004232 File Offset: 0x00002432
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.SendMeBloodEvents);
			GameNetworkMessage.WriteBoolToPacket(this.SendMeSoundEvents);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000424A File Offset: 0x0000244A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000424E File Offset: 0x0000244E
		protected override string OnGetLogFormat()
		{
			return "SyncRelevantGameOptionsToServer";
		}
	}
}
