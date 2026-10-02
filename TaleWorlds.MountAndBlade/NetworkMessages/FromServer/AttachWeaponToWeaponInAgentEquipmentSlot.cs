using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000079 RID: 121
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AttachWeaponToWeaponInAgentEquipmentSlot : GameNetworkMessage
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00008247 File Offset: 0x00006447
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0000824F File Offset: 0x0000644F
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00008258 File Offset: 0x00006458
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x00008260 File Offset: 0x00006460
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00008269 File Offset: 0x00006469
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x00008271 File Offset: 0x00006471
		public int AgentIndex { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0000827A File Offset: 0x0000647A
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00008282 File Offset: 0x00006482
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x06000460 RID: 1120 RVA: 0x0000828B File Offset: 0x0000648B
		public AttachWeaponToWeaponInAgentEquipmentSlot(MissionWeapon weapon, int agentIndex, EquipmentIndex slot, MatrixFrame attachLocalFrame)
		{
			this.Weapon = weapon;
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
			this.AttachLocalFrame = attachLocalFrame;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000082B0 File Offset: 0x000064B0
		public AttachWeaponToWeaponInAgentEquipmentSlot()
		{
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000082B8 File Offset: 0x000064B8
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.AttachLocalFrame.origin, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(this.AttachLocalFrame.rotation);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00008310 File Offset: 0x00006510
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
			if (flag)
			{
				this.AttachLocalFrame = new MatrixFrame(in mat, in vec);
			}
			return flag;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00008378 File Offset: 0x00006578
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00008380 File Offset: 0x00006580
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"AttachWeaponToWeaponInAgentEquipmentSlot with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" to SlotIndex: ",
				this.SlotIndex,
				" on agent-index: ",
				this.AgentIndex
			});
		}
	}
}
