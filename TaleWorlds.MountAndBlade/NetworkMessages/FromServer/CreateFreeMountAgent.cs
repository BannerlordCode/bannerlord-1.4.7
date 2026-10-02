using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000084 RID: 132
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateFreeMountAgent : GameNetworkMessage
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00009515 File Offset: 0x00007715
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0000951D File Offset: 0x0000771D
		public int AgentIndex { get; private set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00009526 File Offset: 0x00007726
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0000952E File Offset: 0x0000772E
		public EquipmentElement HorseItem { get; private set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00009537 File Offset: 0x00007737
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0000953F File Offset: 0x0000773F
		public EquipmentElement HorseHarnessItem { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00009548 File Offset: 0x00007748
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x00009550 File Offset: 0x00007750
		public Vec3 Position { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00009559 File Offset: 0x00007759
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x00009561 File Offset: 0x00007761
		public Vec2 Direction { get; private set; }

		// Token: 0x06000513 RID: 1299 RVA: 0x0000956C File Offset: 0x0000776C
		public CreateFreeMountAgent(Agent agent, Vec3 position, Vec2 direction)
		{
			this.AgentIndex = agent.Index;
			this.HorseItem = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.ArmorItemEndSlot);
			this.HorseHarnessItem = agent.SpawnEquipment.GetEquipmentFromSlot(EquipmentIndex.HorseHarness);
			this.Position = position;
			this.Direction = direction.Normalized();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000095C5 File Offset: 0x000077C5
		public CreateFreeMountAgent()
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000095D0 File Offset: 0x000077D0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.HorseItem = ModuleNetworkData.ReadItemReferenceFromPacket(Game.Current.ObjectManager, ref flag);
			this.HorseHarnessItem = ModuleNetworkData.ReadItemReferenceFromPacket(Game.Current.ObjectManager, ref flag);
			this.Position = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref flag);
			this.Direction = GameNetworkMessage.ReadVec2FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00009640 File Offset: 0x00007840
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			ModuleNetworkData.WriteItemReferenceToPacket(this.HorseItem);
			ModuleNetworkData.WriteItemReferenceToPacket(this.HorseHarnessItem);
			GameNetworkMessage.WriteVec3ToPacket(this.Position, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec2ToPacket(this.Direction, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000968E File Offset: 0x0000788E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00009696 File Offset: 0x00007896
		protected override string OnGetLogFormat()
		{
			return "Create a mount-agent with index: " + this.AgentIndex;
		}
	}
}
