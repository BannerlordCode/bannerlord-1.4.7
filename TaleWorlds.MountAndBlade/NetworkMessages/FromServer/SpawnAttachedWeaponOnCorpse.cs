using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C1 RID: 193
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnAttachedWeaponOnCorpse : GameNetworkMessage
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0000D5EF File Offset: 0x0000B7EF
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x0000D5F7 File Offset: 0x0000B7F7
		public int AgentIndex { get; private set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x0000D600 File Offset: 0x0000B800
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x0000D608 File Offset: 0x0000B808
		public int AttachedIndex { get; private set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0000D611 File Offset: 0x0000B811
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0000D619 File Offset: 0x0000B819
		public int ForcedIndex { get; private set; }

		// Token: 0x060007D1 RID: 2001 RVA: 0x0000D622 File Offset: 0x0000B822
		public SpawnAttachedWeaponOnCorpse(int agentIndex, int attachedIndex, int forcedIndex)
		{
			this.AgentIndex = agentIndex;
			this.AttachedIndex = attachedIndex;
			this.ForcedIndex = forcedIndex;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000D63F File Offset: 0x0000B83F
		public SpawnAttachedWeaponOnCorpse()
		{
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000D648 File Offset: 0x0000B848
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.AttachedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponAttachmentIndexCompressionInfo, ref flag);
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0000D689 File Offset: 0x0000B889
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.AttachedIndex, CompressionMission.WeaponAttachmentIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0000D6B6 File Offset: 0x0000B8B6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0000D6BA File Offset: 0x0000B8BA
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "SpawnAttachedWeaponOnCorpse with agent-index: ", this.AgentIndex, ", and with ID: ", this.ForcedIndex });
		}
	}
}
