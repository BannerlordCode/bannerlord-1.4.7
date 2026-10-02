using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BC RID: 188
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetUsableMissionObjectIsDisabledForPlayers : GameNetworkMessage
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0000CFB9 File Offset: 0x0000B1B9
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0000CFC1 File Offset: 0x0000B1C1
		public MissionObjectId UsableGameObjectId { get; private set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0000CFCA File Offset: 0x0000B1CA
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0000CFD2 File Offset: 0x0000B1D2
		public bool IsDisabledForPlayers { get; private set; }

		// Token: 0x0600078D RID: 1933 RVA: 0x0000CFDB File Offset: 0x0000B1DB
		public SetUsableMissionObjectIsDisabledForPlayers(MissionObjectId usableGameObjectId, bool isDisabledForPlayers)
		{
			this.UsableGameObjectId = usableGameObjectId;
			this.IsDisabledForPlayers = isDisabledForPlayers;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0000CFF1 File Offset: 0x0000B1F1
		public SetUsableMissionObjectIsDisabledForPlayers()
		{
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0000CFFC File Offset: 0x0000B1FC
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableGameObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsDisabledForPlayers = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0000D026 File Offset: 0x0000B226
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableGameObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.IsDisabledForPlayers);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0000D03E File Offset: 0x0000B23E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000D048 File Offset: 0x0000B248
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set IsDisabled for player: ",
				this.IsDisabledForPlayers ? "True" : "False",
				" on UsableMissionObject with ID: ",
				this.UsableGameObjectId
			});
		}
	}
}
