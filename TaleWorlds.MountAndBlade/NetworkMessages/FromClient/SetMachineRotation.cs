using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000037 RID: 55
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SetMachineRotation : GameNetworkMessage
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000040B5 File Offset: 0x000022B5
		// (set) Token: 0x060001AE RID: 430 RVA: 0x000040BD File Offset: 0x000022BD
		public MissionObjectId UsableMachineId { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001AF RID: 431 RVA: 0x000040C6 File Offset: 0x000022C6
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x000040CE File Offset: 0x000022CE
		public float HorizontalRotation { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000040D7 File Offset: 0x000022D7
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x000040DF File Offset: 0x000022DF
		public float VerticalRotation { get; private set; }

		// Token: 0x060001B3 RID: 435 RVA: 0x000040E8 File Offset: 0x000022E8
		public SetMachineRotation(MissionObjectId missionObjectId, float horizontalRotation, float verticalRotation)
		{
			this.UsableMachineId = missionObjectId;
			this.HorizontalRotation = horizontalRotation;
			this.VerticalRotation = verticalRotation;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00004105 File Offset: 0x00002305
		public SetMachineRotation()
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00004110 File Offset: 0x00002310
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMachineId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.HorizontalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			this.VerticalRotation = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.HighResRadianCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00004151 File Offset: 0x00002351
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMachineId);
			GameNetworkMessage.WriteFloatToPacket(this.HorizontalRotation, CompressionBasic.HighResRadianCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.VerticalRotation, CompressionBasic.HighResRadianCompressionInfo);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000417E File Offset: 0x0000237E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00004186 File Offset: 0x00002386
		protected override string OnGetLogFormat()
		{
			return "Set rotation of UsableMachine with ID: " + this.UsableMachineId;
		}
	}
}
