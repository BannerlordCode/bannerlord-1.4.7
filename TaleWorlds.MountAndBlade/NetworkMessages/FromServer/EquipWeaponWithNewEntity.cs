using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000088 RID: 136
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class EquipWeaponWithNewEntity : GameNetworkMessage
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00009D63 File Offset: 0x00007F63
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x00009D6B File Offset: 0x00007F6B
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00009D74 File Offset: 0x00007F74
		// (set) Token: 0x06000554 RID: 1364 RVA: 0x00009D7C File Offset: 0x00007F7C
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00009D85 File Offset: 0x00007F85
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x00009D8D File Offset: 0x00007F8D
		public int AgentIndex { get; private set; }

		// Token: 0x06000557 RID: 1367 RVA: 0x00009D96 File Offset: 0x00007F96
		public EquipWeaponWithNewEntity(int agentIndex, EquipmentIndex slot, MissionWeapon weapon)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
			this.Weapon = weapon;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00009DB3 File Offset: 0x00007FB3
		public EquipWeaponWithNewEntity()
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00009DBB File Offset: 0x00007FBB
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00009DE4 File Offset: 0x00007FE4
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00009E25 File Offset: 0x00008025
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00009E30 File Offset: 0x00008030
		protected override string OnGetLogFormat()
		{
			if (this.AgentIndex < 0)
			{
				return "Not equipping weapon because there is no agent to equip it to,";
			}
			return string.Concat(new object[]
			{
				"Equip weapon with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" from SlotIndex: ",
				this.SlotIndex,
				" on agent with agent-index: ",
				this.AgentIndex
			});
		}
	}
}
