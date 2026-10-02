using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000039 RID: 57
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class TauntSelected : GameNetworkMessage
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00004255 File Offset: 0x00002455
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000425D File Offset: 0x0000245D
		public int IndexOfTaunt { get; private set; }

		// Token: 0x060001C5 RID: 453 RVA: 0x00004266 File Offset: 0x00002466
		public TauntSelected(int indexOfTaunt)
		{
			this.IndexOfTaunt = indexOfTaunt;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00004275 File Offset: 0x00002475
		public TauntSelected()
		{
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00004280 File Offset: 0x00002480
		protected override bool OnRead()
		{
			bool flag = true;
			this.IndexOfTaunt = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TauntIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000042A2 File Offset: 0x000024A2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.IndexOfTaunt, CompressionMission.TauntIndexCompressionInfo);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000042B4 File Offset: 0x000024B4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000042B8 File Offset: 0x000024B8
		protected override string OnGetLogFormat()
		{
			return "FromClient.CheerSelected: " + this.IndexOfTaunt;
		}
	}
}
