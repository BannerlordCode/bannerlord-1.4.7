using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006F RID: 111
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SiegeMoraleChangeMessage : GameNetworkMessage
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00007623 File Offset: 0x00005823
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0000762B File Offset: 0x0000582B
		public int AttackerMorale { get; private set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00007634 File Offset: 0x00005834
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0000763C File Offset: 0x0000583C
		public int DefenderMorale { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00007645 File Offset: 0x00005845
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x0000764D File Offset: 0x0000584D
		public int[] CapturePointRemainingMoraleGains { get; private set; }

		// Token: 0x060003E2 RID: 994 RVA: 0x00007656 File Offset: 0x00005856
		public SiegeMoraleChangeMessage()
		{
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000765E File Offset: 0x0000585E
		public SiegeMoraleChangeMessage(int attackerMorale, int defenderMorale, int[] capturePointRemainingMoraleGains)
		{
			this.AttackerMorale = attackerMorale;
			this.DefenderMorale = defenderMorale;
			this.CapturePointRemainingMoraleGains = capturePointRemainingMoraleGains;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000767C File Offset: 0x0000587C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.AttackerMorale, CompressionMission.SiegeMoraleCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DefenderMorale, CompressionMission.SiegeMoraleCompressionInfo);
			int[] capturePointRemainingMoraleGains = this.CapturePointRemainingMoraleGains;
			for (int i = 0; i < capturePointRemainingMoraleGains.Length; i++)
			{
				GameNetworkMessage.WriteIntToPacket(capturePointRemainingMoraleGains[i], CompressionMission.SiegeMoralePerFlagCompressionInfo);
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000076CC File Offset: 0x000058CC
		protected override bool OnRead()
		{
			bool flag = true;
			this.AttackerMorale = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeMoraleCompressionInfo, ref flag);
			this.DefenderMorale = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeMoraleCompressionInfo, ref flag);
			this.CapturePointRemainingMoraleGains = new int[7];
			for (int i = 0; i < this.CapturePointRemainingMoraleGains.Length; i++)
			{
				this.CapturePointRemainingMoraleGains[i] = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeMoralePerFlagCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00007733 File Offset: 0x00005933
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000773B File Offset: 0x0000593B
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Morale synched. A: ", this.AttackerMorale, " D: ", this.DefenderMorale });
		}
	}
}
