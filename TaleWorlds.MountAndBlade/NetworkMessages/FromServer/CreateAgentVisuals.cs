using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000082 RID: 130
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateAgentVisuals : GameNetworkMessage
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00009178 File Offset: 0x00007378
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x00009180 File Offset: 0x00007380
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00009189 File Offset: 0x00007389
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x00009191 File Offset: 0x00007391
		public int VisualsIndex { get; private set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000919A File Offset: 0x0000739A
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x000091A2 File Offset: 0x000073A2
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000091AB File Offset: 0x000073AB
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x000091B3 File Offset: 0x000073B3
		public Equipment Equipment { get; private set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x000091BC File Offset: 0x000073BC
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x000091C4 File Offset: 0x000073C4
		public int BodyPropertiesSeed { get; private set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x000091CD File Offset: 0x000073CD
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x000091D5 File Offset: 0x000073D5
		public bool IsFemale { get; private set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x000091DE File Offset: 0x000073DE
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x000091E6 File Offset: 0x000073E6
		public int SelectedEquipmentSetIndex { get; private set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x000091EF File Offset: 0x000073EF
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x000091F7 File Offset: 0x000073F7
		public int TroopCountInFormation { get; private set; }

		// Token: 0x060004F9 RID: 1273 RVA: 0x00009200 File Offset: 0x00007400
		public CreateAgentVisuals(NetworkCommunicator peer, AgentBuildData agentBuildData, int selectedEquipmentSetIndex, int troopCountInFormation = 0)
		{
			this.Peer = peer;
			this.VisualsIndex = agentBuildData.AgentVisualsIndex;
			this.Character = agentBuildData.AgentCharacter;
			this.BodyPropertiesSeed = agentBuildData.AgentEquipmentSeed;
			this.IsFemale = agentBuildData.AgentIsFemale;
			this.Equipment = new Equipment();
			this.Equipment.FillFrom(agentBuildData.AgentOverridenSpawnEquipment, true);
			this.SelectedEquipmentSetIndex = selectedEquipmentSetIndex;
			this.TroopCountInFormation = troopCountInFormation;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00009276 File Offset: 0x00007476
		public CreateAgentVisuals()
		{
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00009280 File Offset: 0x00007480
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.VisualsIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentOffsetCompressionInfo, ref flag);
			this.Character = (BasicCharacterObject)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			this.Equipment = new Equipment();
			bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < (flag2 ? EquipmentIndex.NumEquipmentSetSlots : EquipmentIndex.ArmorItemEndSlot); equipmentIndex++)
			{
				EquipmentElement equipmentElement = ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag);
				if (!flag)
				{
					break;
				}
				this.Equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, equipmentElement);
			}
			this.BodyPropertiesSeed = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RandomSeedCompressionInfo, ref flag);
			this.IsFemale = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.SelectedEquipmentSetIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			this.TroopCountInFormation = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00009354 File Offset: 0x00007554
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket(this.VisualsIndex, CompressionMission.AgentOffsetCompressionInfo);
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Character, CompressionBasic.GUIDCompressionInfo);
			bool flag = this.Equipment[EquipmentIndex.ArmorItemEndSlot].Item != null;
			GameNetworkMessage.WriteBoolToPacket(flag);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < (flag ? EquipmentIndex.NumEquipmentSetSlots : EquipmentIndex.ArmorItemEndSlot); equipmentIndex++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.Equipment.GetEquipmentFromSlot(equipmentIndex));
			}
			GameNetworkMessage.WriteIntToPacket(this.BodyPropertiesSeed, CompressionBasic.RandomSeedCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsFemale);
			GameNetworkMessage.WriteIntToPacket(this.SelectedEquipmentSetIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.TroopCountInFormation, CompressionBasic.PlayerCompressionInfo);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000940B File Offset: 0x0000760B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00009413 File Offset: 0x00007613
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create AgentVisuals for peer: ",
				this.Peer.UserName,
				", and with Index: ",
				this.VisualsIndex
			});
		}
	}
}
