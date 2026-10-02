using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200003C RID: 60
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SendVoiceRecord : GameNetworkMessage
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000043B9 File Offset: 0x000025B9
		// (set) Token: 0x060001DC RID: 476 RVA: 0x000043C1 File Offset: 0x000025C1
		public byte[] Buffer { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000043CA File Offset: 0x000025CA
		// (set) Token: 0x060001DE RID: 478 RVA: 0x000043D2 File Offset: 0x000025D2
		public int BufferLength { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000043DB File Offset: 0x000025DB
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x000043E3 File Offset: 0x000025E3
		public List<VirtualPlayer> ReceiverList { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000043EC File Offset: 0x000025EC
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x000043F4 File Offset: 0x000025F4
		public bool HasReceiverList { get; private set; }

		// Token: 0x060001E3 RID: 483 RVA: 0x000043FD File Offset: 0x000025FD
		public SendVoiceRecord()
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00004405 File Offset: 0x00002605
		public SendVoiceRecord(byte[] buffer, int bufferLength)
		{
			this.Buffer = buffer;
			this.BufferLength = bufferLength;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000441B File Offset: 0x0000261B
		public SendVoiceRecord(byte[] buffer, int bufferLength, List<VirtualPlayer> receiverList)
		{
			this.Buffer = buffer;
			this.BufferLength = bufferLength;
			this.ReceiverList = receiverList;
			this.HasReceiverList = true;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00004440 File Offset: 0x00002640
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteByteArrayToPacket(this.Buffer, 0, this.BufferLength);
			int num = 0;
			if (this.ReceiverList != null)
			{
				num = this.ReceiverList.Count;
			}
			GameNetworkMessage.WriteBoolToPacket(this.HasReceiverList);
			GameNetworkMessage.WriteIntToPacket(num, CompressionBasic.PlayerCompressionInfo);
			for (int i = 0; i < num; i++)
			{
				GameNetworkMessage.WriteVirtualPlayerReferenceToPacket(this.ReceiverList[i]);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000044A8 File Offset: 0x000026A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.Buffer = new byte[1440];
			this.BufferLength = GameNetworkMessage.ReadByteArrayFromPacket(this.Buffer, 0, 1440, ref flag);
			this.HasReceiverList = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			if (this.HasReceiverList)
			{
				this.ReceiverList = new List<VirtualPlayer>();
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						VirtualPlayer virtualPlayer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref flag, false);
						this.ReceiverList.Add(virtualPlayer);
					}
				}
			}
			return flag;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00004533 File Offset: 0x00002733
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00004537 File Offset: 0x00002737
		protected override string OnGetLogFormat()
		{
			return string.Empty;
		}
	}
}
