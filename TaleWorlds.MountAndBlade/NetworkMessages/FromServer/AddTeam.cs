using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000072 RID: 114
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddTeam : GameNetworkMessage
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x000079B1 File Offset: 0x00005BB1
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x000079B9 File Offset: 0x00005BB9
		public int TeamIndex { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x000079C2 File Offset: 0x00005BC2
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x000079CA File Offset: 0x00005BCA
		public BattleSideEnum Side { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x000079D3 File Offset: 0x00005BD3
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x000079DB File Offset: 0x00005BDB
		public uint Color { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x000079E4 File Offset: 0x00005BE4
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x000079EC File Offset: 0x00005BEC
		public uint Color2 { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x000079F5 File Offset: 0x00005BF5
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x000079FD File Offset: 0x00005BFD
		public string BannerCode { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00007A06 File Offset: 0x00005C06
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00007A0E File Offset: 0x00005C0E
		public bool IsPlayerGeneral { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00007A17 File Offset: 0x00005C17
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x00007A1F File Offset: 0x00005C1F
		public bool IsPlayerSergeant { get; private set; }

		// Token: 0x0600040E RID: 1038 RVA: 0x00007A28 File Offset: 0x00005C28
		public AddTeam(int teamIndex, BattleSideEnum side, uint color, uint color2, string bannerCode, bool isPlayerGeneral, bool isPlayerSergeant)
		{
			this.TeamIndex = teamIndex;
			this.Side = side;
			this.Color = color;
			this.Color2 = color2;
			this.BannerCode = bannerCode;
			this.IsPlayerGeneral = isPlayerGeneral;
			this.IsPlayerSergeant = isPlayerSergeant;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00007A65 File Offset: 0x00005C65
		public AddTeam()
		{
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00007A70 File Offset: 0x00005C70
		protected override bool OnRead()
		{
			bool flag = true;
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.Side = (BattleSideEnum)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			this.Color = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.Color2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsPlayerGeneral = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsPlayerSergeant = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00007AEC File Offset: 0x00005CEC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.Side, CompressionMission.TeamSideCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.Color, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.Color2, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayerGeneral);
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayerSergeant);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00007B55 File Offset: 0x00005D55
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00007B5D File Offset: 0x00005D5D
		protected override string OnGetLogFormat()
		{
			return "Add team with side: " + this.Side;
		}
	}
}
