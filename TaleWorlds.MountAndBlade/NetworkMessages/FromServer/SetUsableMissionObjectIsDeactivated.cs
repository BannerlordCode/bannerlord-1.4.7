using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BB RID: 187
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetUsableMissionObjectIsDeactivated : GameNetworkMessage
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0000CEDD File Offset: 0x0000B0DD
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x0000CEE5 File Offset: 0x0000B0E5
		public MissionObjectId UsableGameObjectId { get; private set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0000CEEE File Offset: 0x0000B0EE
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x0000CEF6 File Offset: 0x0000B0F6
		public bool IsDeactivated { get; private set; }

		// Token: 0x06000783 RID: 1923 RVA: 0x0000CEFF File Offset: 0x0000B0FF
		public SetUsableMissionObjectIsDeactivated(MissionObjectId usableGameObjectId, bool isDeactivated)
		{
			this.UsableGameObjectId = usableGameObjectId;
			this.IsDeactivated = isDeactivated;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0000CF15 File Offset: 0x0000B115
		public SetUsableMissionObjectIsDeactivated()
		{
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0000CF20 File Offset: 0x0000B120
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableGameObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsDeactivated = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0000CF4A File Offset: 0x0000B14A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableGameObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.IsDeactivated);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0000CF62 File Offset: 0x0000B162
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0000CF6C File Offset: 0x0000B16C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set IsDeactivated: ",
				this.IsDeactivated ? "True" : "False",
				" on UsableMissionObject with ID: ",
				this.UsableGameObjectId
			});
		}
	}
}
