using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000081 RID: 129
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateAgent : GameNetworkMessage
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00008BBA File Offset: 0x00006DBA
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x00008BC2 File Offset: 0x00006DC2
		public int AgentIndex { get; private set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00008BCB File Offset: 0x00006DCB
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x00008BD3 File Offset: 0x00006DD3
		public int MountAgentIndex { get; private set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00008BDC File Offset: 0x00006DDC
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00008BE4 File Offset: 0x00006DE4
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00008BED File Offset: 0x00006DED
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00008BF5 File Offset: 0x00006DF5
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00008BFE File Offset: 0x00006DFE
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00008C06 File Offset: 0x00006E06
		public Monster Monster { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00008C0F File Offset: 0x00006E0F
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00008C17 File Offset: 0x00006E17
		public MissionEquipment MissionEquipment { get; private set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00008C20 File Offset: 0x00006E20
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00008C28 File Offset: 0x00006E28
		public Equipment SpawnEquipment { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00008C31 File Offset: 0x00006E31
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00008C39 File Offset: 0x00006E39
		public BodyProperties BodyPropertiesValue { get; private set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00008C42 File Offset: 0x00006E42
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00008C4A File Offset: 0x00006E4A
		public int BodyPropertiesSeed { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00008C53 File Offset: 0x00006E53
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00008C5B File Offset: 0x00006E5B
		public bool IsFemale { get; private set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00008C64 File Offset: 0x00006E64
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00008C6C File Offset: 0x00006E6C
		public int TeamIndex { get; private set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00008C75 File Offset: 0x00006E75
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00008C7D File Offset: 0x00006E7D
		public Vec3 Position { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00008C86 File Offset: 0x00006E86
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00008C8E File Offset: 0x00006E8E
		public Vec2 Direction { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00008C97 File Offset: 0x00006E97
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00008C9F File Offset: 0x00006E9F
		public int FormationIndex { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00008CA8 File Offset: 0x00006EA8
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00008CB0 File Offset: 0x00006EB0
		public bool IsPlayerAgent { get; private set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00008CB9 File Offset: 0x00006EB9
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00008CC1 File Offset: 0x00006EC1
		public uint ClothingColor1 { get; private set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00008CCA File Offset: 0x00006ECA
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00008CD2 File Offset: 0x00006ED2
		public uint ClothingColor2 { get; private set; }

		// Token: 0x060004E3 RID: 1251 RVA: 0x00008CDC File Offset: 0x00006EDC
		public CreateAgent(int agentIndex, BasicCharacterObject character, Monster monster, Equipment spawnEquipment, MissionEquipment missionEquipment, BodyProperties bodyPropertiesValue, int bodyPropertiesSeed, bool isFemale, int agentTeamIndex, int agentFormationIndex, uint clothingColor1, uint clothingColor2, int mountAgentIndex, Equipment mountAgentSpawnEquipment, bool isPlayerAgent, Vec3 position, Vec2 direction, NetworkCommunicator peer)
		{
			this.AgentIndex = agentIndex;
			this.MountAgentIndex = mountAgentIndex;
			this.Peer = peer;
			this.Character = character;
			this.Monster = monster;
			this.SpawnEquipment = new Equipment();
			this.MissionEquipment = new MissionEquipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this.MissionEquipment[equipmentIndex] = missionEquipment[equipmentIndex];
			}
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.NumAllWeaponSlots; equipmentIndex2 < EquipmentIndex.ArmorItemEndSlot; equipmentIndex2++)
			{
				this.SpawnEquipment[equipmentIndex2] = spawnEquipment.GetEquipmentFromSlot(equipmentIndex2);
			}
			if (this.MountAgentIndex >= 0)
			{
				this.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot] = mountAgentSpawnEquipment[EquipmentIndex.ArmorItemEndSlot];
				this.SpawnEquipment[EquipmentIndex.HorseHarness] = mountAgentSpawnEquipment[EquipmentIndex.HorseHarness];
			}
			else
			{
				this.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot] = default(EquipmentElement);
				this.SpawnEquipment[EquipmentIndex.HorseHarness] = default(EquipmentElement);
			}
			this.BodyPropertiesValue = bodyPropertiesValue;
			this.BodyPropertiesSeed = bodyPropertiesSeed;
			this.IsFemale = isFemale;
			this.TeamIndex = agentTeamIndex;
			this.Position = position;
			this.Direction = direction;
			this.FormationIndex = agentFormationIndex;
			this.ClothingColor1 = clothingColor1;
			this.ClothingColor2 = clothingColor2;
			this.IsPlayerAgent = isPlayerAgent;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00008E1E File Offset: 0x0000701E
		public CreateAgent()
		{
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00008E28 File Offset: 0x00007028
		protected override bool OnRead()
		{
			bool flag = true;
			this.Character = (BasicCharacterObject)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			this.Monster = (Monster)GameNetworkMessage.ReadObjectReferenceFromPacket(MBObjectManager.Instance, CompressionBasic.GUIDCompressionInfo, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.MountAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.SpawnEquipment = new Equipment();
			this.MissionEquipment = new MissionEquipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this.MissionEquipment[equipmentIndex] = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			}
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.NumAllWeaponSlots; equipmentIndex2 < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex2++)
			{
				this.SpawnEquipment.AddEquipmentToSlotWithoutAgent(equipmentIndex2, ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag));
			}
			this.IsPlayerAgent = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.BodyPropertiesSeed = ((!this.IsPlayerAgent) ? GameNetworkMessage.ReadIntFromPacket(CompressionBasic.RandomSeedCompressionInfo, ref flag) : 0);
			this.BodyPropertiesValue = GameNetworkMessage.ReadBodyPropertiesFromPacket(ref flag);
			this.IsFemale = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag).Normalized();
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.ClothingColor1 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.ClothingColor2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00008FAC File Offset: 0x000071AC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Character, CompressionBasic.GUIDCompressionInfo);
			GameNetworkMessage.WriteObjectReferenceToPacket(this.Monster, CompressionBasic.GUIDCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteAgentIndexToPacket(this.MountAgentIndex);
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				ModuleNetworkData.WriteWeaponReferenceToPacket(this.MissionEquipment[equipmentIndex]);
			}
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.NumAllWeaponSlots; equipmentIndex2 < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex2++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.SpawnEquipment.GetEquipmentFromSlot(equipmentIndex2));
			}
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayerAgent);
			if (!this.IsPlayerAgent)
			{
				GameNetworkMessage.WriteIntToPacket(this.BodyPropertiesSeed, CompressionBasic.RandomSeedCompressionInfo);
			}
			GameNetworkMessage.WriteBodyPropertiesToPacket(this.BodyPropertiesValue);
			GameNetworkMessage.WriteBoolToPacket(this.IsFemale);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec2ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.ClothingColor1, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.ClothingColor2, CompressionBasic.ColorCompressionInfo);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000090C9 File Offset: 0x000072C9
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000090D4 File Offset: 0x000072D4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Create an agent with index: ",
				this.AgentIndex,
				(this.Peer != null) ? string.Concat(new object[]
				{
					", belonging to peer with Name: ",
					this.Peer.UserName,
					", and peer-index: ",
					this.Peer.Index
				}) : "",
				(this.MountAgentIndex == -1) ? "" : (", owning a mount with index: " + this.MountAgentIndex)
			});
		}
	}
}
