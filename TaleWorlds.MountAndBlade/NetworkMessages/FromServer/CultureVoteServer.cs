using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000044 RID: 68
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CultureVoteServer : GameNetworkMessage
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600022B RID: 555 RVA: 0x000049A9 File Offset: 0x00002BA9
		// (set) Token: 0x0600022C RID: 556 RVA: 0x000049B1 File Offset: 0x00002BB1
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600022D RID: 557 RVA: 0x000049BA File Offset: 0x00002BBA
		// (set) Token: 0x0600022E RID: 558 RVA: 0x000049C2 File Offset: 0x00002BC2
		public BasicCultureObject VotedCulture { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600022F RID: 559 RVA: 0x000049CB File Offset: 0x00002BCB
		// (set) Token: 0x06000230 RID: 560 RVA: 0x000049D3 File Offset: 0x00002BD3
		public CultureVoteTypes VotedType { get; private set; }

		// Token: 0x06000231 RID: 561 RVA: 0x000049DC File Offset: 0x00002BDC
		public CultureVoteServer()
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000049E4 File Offset: 0x00002BE4
		public CultureVoteServer(NetworkCommunicator peer, CultureVoteTypes type, BasicCultureObject culture)
		{
			this.Peer = peer;
			this.VotedType = type;
			this.VotedCulture = culture;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00004A04 File Offset: 0x00002C04
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket((int)this.VotedType, CompressionMission.TeamSideCompressionInfo);
			MBReadOnlyList<BasicCultureObject> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>();
			GameNetworkMessage.WriteIntToPacket((this.VotedCulture == null) ? (-1) : objectTypeList.IndexOf(this.VotedCulture), CompressionBasic.CultureIndexCompressionInfo);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00004A58 File Offset: 0x00002C58
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.VotedType = (CultureVoteTypes)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.CultureIndexCompressionInfo, ref flag);
			if (flag)
			{
				MBReadOnlyList<BasicCultureObject> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>();
				this.VotedCulture = ((num < 0) ? null : objectTypeList[num]);
			}
			return flag;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00004AB7 File Offset: 0x00002CB7
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00004AC0 File Offset: 0x00002CC0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Culture ",
				this.VotedCulture.Name,
				" has been ",
				this.VotedType.ToString().ToLower(),
				(this.VotedType == CultureVoteTypes.Ban) ? "ned." : "ed."
			});
		}
	}
}
