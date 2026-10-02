using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D7 RID: 215
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class NotificationMessage : GameNetworkMessage
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0000EC37 File Offset: 0x0000CE37
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x0000EC3F File Offset: 0x0000CE3F
		public int Message { get; private set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0000EC48 File Offset: 0x0000CE48
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0000EC50 File Offset: 0x0000CE50
		public int ParameterOne { get; private set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0000EC59 File Offset: 0x0000CE59
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0000EC61 File Offset: 0x0000CE61
		public int ParameterTwo { get; private set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0000EC6A File Offset: 0x0000CE6A
		private bool HasParameterOne
		{
			get
			{
				return this.ParameterOne != -1;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0000EC78 File Offset: 0x0000CE78
		private bool HasParameterTwo
		{
			get
			{
				return this.ParameterOne != -1;
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0000EC86 File Offset: 0x0000CE86
		public NotificationMessage(int message, int param1, int param2)
		{
			this.Message = message;
			this.ParameterOne = param1;
			this.ParameterTwo = param2;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0000ECA3 File Offset: 0x0000CEA3
		public NotificationMessage()
		{
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0000ECAC File Offset: 0x0000CEAC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Message, CompressionMission.MultiplayerNotificationCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.HasParameterOne);
			if (this.HasParameterOne)
			{
				GameNetworkMessage.WriteIntToPacket(this.ParameterOne, CompressionMission.MultiplayerNotificationParameterCompressionInfo);
				GameNetworkMessage.WriteBoolToPacket(this.HasParameterTwo);
				if (this.HasParameterTwo)
				{
					GameNetworkMessage.WriteIntToPacket(this.ParameterTwo, CompressionMission.MultiplayerNotificationParameterCompressionInfo);
				}
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0000ED10 File Offset: 0x0000CF10
		protected override bool OnRead()
		{
			bool flag = true;
			this.ParameterOne = (this.ParameterTwo = -1);
			this.Message = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerNotificationCompressionInfo, ref flag);
			if (GameNetworkMessage.ReadBoolFromPacket(ref flag))
			{
				this.ParameterOne = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerNotificationParameterCompressionInfo, ref flag);
				if (GameNetworkMessage.ReadBoolFromPacket(ref flag))
				{
					this.ParameterTwo = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerNotificationParameterCompressionInfo, ref flag);
				}
			}
			return flag;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000ED78 File Offset: 0x0000CF78
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0000ED7C File Offset: 0x0000CF7C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Receiving message: ",
				this.Message,
				this.HasParameterOne ? (" With first parameter: " + this.ParameterOne) : "",
				this.HasParameterTwo ? (" and second parameter: " + this.ParameterTwo) : ""
			});
		}
	}
}
