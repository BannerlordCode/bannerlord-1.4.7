using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000050 RID: 80
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ChangeClassRestrictions : GameNetworkMessage
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00005712 File Offset: 0x00003912
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000571A File Offset: 0x0000391A
		public FormationClass ClassToChangeRestriction { get; private set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00005723 File Offset: 0x00003923
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000572B File Offset: 0x0000392B
		public bool NewValue { get; private set; }

		// Token: 0x060002B7 RID: 695 RVA: 0x00005734 File Offset: 0x00003934
		public ChangeClassRestrictions()
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000573C File Offset: 0x0000393C
		public ChangeClassRestrictions(FormationClass classToChangeRestriction, bool newValue)
		{
			this.ClassToChangeRestriction = classToChangeRestriction;
			this.NewValue = newValue;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00005754 File Offset: 0x00003954
		protected override bool OnRead()
		{
			bool flag = true;
			this.ClassToChangeRestriction = (FormationClass)GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.NewValue = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00005783 File Offset: 0x00003983
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.ClassToChangeRestriction, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.NewValue);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000057A0 File Offset: 0x000039A0
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000057A8 File Offset: 0x000039A8
		protected override string OnGetLogFormat()
		{
			return string.Format("ChangeClassRestrictions for {0} to be {1}", this.ClassToChangeRestriction, this.NewValue);
		}
	}
}
