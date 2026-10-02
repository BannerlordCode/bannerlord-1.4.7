using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000076 RID: 118
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AssignFormationToPlayer : GameNetworkMessage
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00007E3A File Offset: 0x0000603A
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x00007E42 File Offset: 0x00006042
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00007E4B File Offset: 0x0000604B
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x00007E53 File Offset: 0x00006053
		public FormationClass FormationClass { get; private set; }

		// Token: 0x06000438 RID: 1080 RVA: 0x00007E5C File Offset: 0x0000605C
		public AssignFormationToPlayer(NetworkCommunicator peer, FormationClass formationClass)
		{
			this.Peer = peer;
			this.FormationClass = formationClass;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00007E72 File Offset: 0x00006072
		public AssignFormationToPlayer()
		{
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00007E7C File Offset: 0x0000607C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.FormationClass = (FormationClass)GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00007EAC File Offset: 0x000060AC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket((int)this.FormationClass, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00007EC9 File Offset: 0x000060C9
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00007ED0 File Offset: 0x000060D0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Assign formation with index: ",
				(int)this.FormationClass,
				" to NetworkPeer with name: ",
				this.Peer.UserName,
				" and peer-index",
				this.Peer.Index,
				" and make him captain."
			});
		}
	}
}
