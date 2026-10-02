using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000059 RID: 89
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerIntermissionCultureItemAdded : GameNetworkMessage
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000061F5 File Offset: 0x000043F5
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000061FD File Offset: 0x000043FD
		public string CultureId { get; private set; }

		// Token: 0x06000321 RID: 801 RVA: 0x00006206 File Offset: 0x00004406
		public MultiplayerIntermissionCultureItemAdded()
		{
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000620E File Offset: 0x0000440E
		public MultiplayerIntermissionCultureItemAdded(string cultureId)
		{
			this.CultureId = cultureId;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00006220 File Offset: 0x00004420
		protected override bool OnRead()
		{
			bool flag = true;
			this.CultureId = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000623D File Offset: 0x0000443D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.CultureId);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000624A File Offset: 0x0000444A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00006252 File Offset: 0x00004452
		protected override string OnGetLogFormat()
		{
			return "Adding culture for voting with id: " + this.CultureId + ".";
		}
	}
}
