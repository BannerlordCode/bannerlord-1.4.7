using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000034 RID: 52
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectFormation : GameNetworkMessage
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00003F68 File Offset: 0x00002168
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00003F70 File Offset: 0x00002170
		public int FormationIndex { get; private set; }

		// Token: 0x06000197 RID: 407 RVA: 0x00003F79 File Offset: 0x00002179
		public SelectFormation(int formationIndex)
		{
			this.FormationIndex = formationIndex;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00003F88 File Offset: 0x00002188
		public SelectFormation()
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00003F90 File Offset: 0x00002190
		protected override bool OnRead()
		{
			bool flag = true;
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00003FB2 File Offset: 0x000021B2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00003FC4 File Offset: 0x000021C4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00003FC9 File Offset: 0x000021C9
		protected override string OnGetLogFormat()
		{
			return "Select Formation with ID: " + this.FormationIndex;
		}
	}
}
