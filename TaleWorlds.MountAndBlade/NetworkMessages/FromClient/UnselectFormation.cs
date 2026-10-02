using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200003A RID: 58
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class UnselectFormation : GameNetworkMessage
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000042CF File Offset: 0x000024CF
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000042D7 File Offset: 0x000024D7
		public int FormationIndex { get; private set; }

		// Token: 0x060001CD RID: 461 RVA: 0x000042E0 File Offset: 0x000024E0
		public UnselectFormation(int formationIndex)
		{
			this.FormationIndex = formationIndex;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000042EF File Offset: 0x000024EF
		public UnselectFormation()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000042F8 File Offset: 0x000024F8
		protected override bool OnRead()
		{
			bool flag = true;
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000431A File Offset: 0x0000251A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000432C File Offset: 0x0000252C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00004331 File Offset: 0x00002531
		protected override string OnGetLogFormat()
		{
			return "Deselect Formation with index: " + this.FormationIndex;
		}
	}
}
