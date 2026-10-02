using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000054 RID: 84
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class InitializeLobbyPeer : GameNetworkMessage
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00005A54 File Offset: 0x00003C54
		// (set) Token: 0x060002DA RID: 730 RVA: 0x00005A5C File Offset: 0x00003C5C
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00005A65 File Offset: 0x00003C65
		// (set) Token: 0x060002DC RID: 732 RVA: 0x00005A6D File Offset: 0x00003C6D
		public PlayerId ProvidedId { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00005A76 File Offset: 0x00003C76
		// (set) Token: 0x060002DE RID: 734 RVA: 0x00005A7E File Offset: 0x00003C7E
		public string BannerCode { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00005A87 File Offset: 0x00003C87
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x00005A8F File Offset: 0x00003C8F
		public BodyProperties BodyProperties { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00005A98 File Offset: 0x00003C98
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x00005AA0 File Offset: 0x00003CA0
		public int ChosenBadgeIndex { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00005AA9 File Offset: 0x00003CA9
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00005AB1 File Offset: 0x00003CB1
		public int ForcedAvatarIndex { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00005ABA File Offset: 0x00003CBA
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x00005AC2 File Offset: 0x00003CC2
		public bool IsFemale { get; private set; }

		// Token: 0x060002E7 RID: 743 RVA: 0x00005ACC File Offset: 0x00003CCC
		public InitializeLobbyPeer(NetworkCommunicator peer, VirtualPlayer virtualPlayer, int forcedAvatarIndex)
		{
			this.Peer = peer;
			this.ProvidedId = virtualPlayer.Id;
			this.BannerCode = ((virtualPlayer.BannerCode != null) ? virtualPlayer.BannerCode : string.Empty);
			this.BodyProperties = virtualPlayer.BodyProperties;
			this.ChosenBadgeIndex = virtualPlayer.ChosenBadgeIndex;
			this.IsFemale = virtualPlayer.IsFemale;
			this.ForcedAvatarIndex = forcedAvatarIndex;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00005B38 File Offset: 0x00003D38
		public InitializeLobbyPeer()
		{
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00005B40 File Offset: 0x00003D40
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			ulong num = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num2 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num3 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			string text = GameNetworkMessage.ReadStringFromPacket(ref flag);
			if (flag)
			{
				this.ProvidedId = new PlayerId(num, num2, num3, num4);
				BodyProperties bodyProperties;
				if (BodyProperties.FromString(text, out bodyProperties))
				{
					this.BodyProperties = bodyProperties;
				}
				else
				{
					flag = false;
				}
			}
			this.ChosenBadgeIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerChosenBadgeCompressionInfo, ref flag);
			this.ForcedAvatarIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ForcedAvatarIndexCompressionInfo, ref flag);
			this.IsFemale = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00005C04 File Offset: 0x00003E04
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part1, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part2, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part3, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part4, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
			GameNetworkMessage.WriteStringToPacket(this.BodyProperties.ToString());
			GameNetworkMessage.WriteIntToPacket(this.ChosenBadgeIndex, CompressionBasic.PlayerChosenBadgeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedAvatarIndex, CompressionBasic.ForcedAvatarIndexCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsFemale);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00005CCB File Offset: 0x00003ECB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00005CCF File Offset: 0x00003ECF
		protected override string OnGetLogFormat()
		{
			return "Initialize LobbyPeer from Peer: " + this.Peer.UserName;
		}
	}
}
