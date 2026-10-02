using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B0 RID: 176
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectVisibility : GameNetworkMessage
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0000C67D File Offset: 0x0000A87D
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0000C685 File Offset: 0x0000A885
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0000C68E File Offset: 0x0000A88E
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0000C696 File Offset: 0x0000A896
		public bool Visible { get; private set; }

		// Token: 0x06000719 RID: 1817 RVA: 0x0000C69F File Offset: 0x0000A89F
		public SetMissionObjectVisibility(MissionObjectId missionObjectId, bool visible)
		{
			this.MissionObjectId = missionObjectId;
			this.Visible = visible;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0000C6B5 File Offset: 0x0000A8B5
		public SetMissionObjectVisibility()
		{
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Visible = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0000C6EA File Offset: 0x0000A8EA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.Visible);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0000C702 File Offset: 0x0000A902
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0000C70C File Offset: 0x0000A90C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set Visibility of MissionObject with ID: ",
				this.MissionObjectId,
				" to: ",
				this.Visible ? "True" : "False"
			});
		}
	}
}
