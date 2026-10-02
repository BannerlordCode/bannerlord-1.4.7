using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007F RID: 127
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CombatLogNetworkMessage : GameNetworkMessage
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0000866D File Offset: 0x0000686D
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x00008675 File Offset: 0x00006875
		public int AttackerAgentIndex { get; private set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0000867E File Offset: 0x0000687E
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x00008686 File Offset: 0x00006886
		public int VictimAgentIndex { get; private set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000868F File Offset: 0x0000688F
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x00008697 File Offset: 0x00006897
		public MissionObjectId MissionObjectHitId { get; private set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x000086A0 File Offset: 0x000068A0
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x000086A8 File Offset: 0x000068A8
		public DamageTypes DamageType { get; private set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x000086B1 File Offset: 0x000068B1
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x000086B9 File Offset: 0x000068B9
		public bool CrushedThrough { get; private set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x000086C2 File Offset: 0x000068C2
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x000086CA File Offset: 0x000068CA
		public bool Chamber { get; private set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x000086D3 File Offset: 0x000068D3
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x000086DB File Offset: 0x000068DB
		public bool IsRangedAttack { get; private set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x000086E4 File Offset: 0x000068E4
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x000086EC File Offset: 0x000068EC
		public bool IsFriendlyFire { get; private set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x000086F5 File Offset: 0x000068F5
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x000086FD File Offset: 0x000068FD
		public bool IsFatalDamage { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00008706 File Offset: 0x00006906
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0000870E File Offset: 0x0000690E
		public bool IsSpecialDamage { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00008717 File Offset: 0x00006917
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0000871F File Offset: 0x0000691F
		public BoneBodyPartType BodyPartHit { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00008728 File Offset: 0x00006928
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x00008730 File Offset: 0x00006930
		public float HitSpeed { get; private set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00008739 File Offset: 0x00006939
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x00008741 File Offset: 0x00006941
		public float Distance { get; private set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0000874A File Offset: 0x0000694A
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00008752 File Offset: 0x00006952
		public int InflictedDamage { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0000875B File Offset: 0x0000695B
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x00008763 File Offset: 0x00006963
		public int AbsorbedDamage { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0000876C File Offset: 0x0000696C
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x00008774 File Offset: 0x00006974
		public int ModifiedDamage { get; private set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0000877D File Offset: 0x0000697D
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x00008785 File Offset: 0x00006985
		public int ReflectedDamage { get; private set; }

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000878E File Offset: 0x0000698E
		public CombatLogNetworkMessage()
		{
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00008798 File Offset: 0x00006998
		public CombatLogNetworkMessage(int attackerAgentIndex, int victimAgentIndex, MissionObjectId missionObjectHitId, CombatLogData combatLogData)
		{
			this.AttackerAgentIndex = attackerAgentIndex;
			this.VictimAgentIndex = victimAgentIndex;
			this.MissionObjectHitId = missionObjectHitId;
			this.DamageType = combatLogData.DamageType;
			this.CrushedThrough = combatLogData.CrushedThrough;
			this.Chamber = combatLogData.Chamber;
			this.IsRangedAttack = combatLogData.IsRangedAttack;
			this.IsFriendlyFire = combatLogData.IsFriendlyFire;
			this.IsFatalDamage = combatLogData.IsFatalDamage;
			this.IsSpecialDamage = combatLogData.IsSpecialDamage;
			this.BodyPartHit = combatLogData.BodyPartHit;
			this.HitSpeed = combatLogData.HitSpeed;
			this.Distance = combatLogData.Distance;
			this.InflictedDamage = combatLogData.InflictedDamage;
			this.AbsorbedDamage = combatLogData.AbsorbedDamage;
			this.ModifiedDamage = combatLogData.ModifiedDamage;
			this.ReflectedDamage = combatLogData.ReflectedDamage;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00008878 File Offset: 0x00006A78
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AttackerAgentIndex);
			GameNetworkMessage.WriteAgentIndexToPacket(this.VictimAgentIndex);
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectHitId);
			GameNetworkMessage.WriteIntToPacket((int)this.DamageType, CompressionBasic.AgentHitDamageTypeCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.CrushedThrough);
			GameNetworkMessage.WriteBoolToPacket(this.Chamber);
			GameNetworkMessage.WriteBoolToPacket(this.IsRangedAttack);
			GameNetworkMessage.WriteBoolToPacket(this.IsFriendlyFire);
			GameNetworkMessage.WriteBoolToPacket(this.IsFatalDamage);
			GameNetworkMessage.WriteBoolToPacket(this.IsSpecialDamage);
			GameNetworkMessage.WriteIntToPacket((int)this.BodyPartHit, CompressionBasic.AgentHitBodyPartCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.HitSpeed, CompressionBasic.AgentHitRelativeSpeedCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.Distance, CompressionBasic.AgentHitRelativeSpeedCompressionInfo);
			this.AbsorbedDamage = MBMath.ClampInt(this.AbsorbedDamage, 0, 2000);
			this.InflictedDamage = MBMath.ClampInt(this.InflictedDamage, 0, 2000);
			this.ModifiedDamage = MBMath.ClampInt(this.ModifiedDamage, -2000, 2000);
			this.ReflectedDamage = MBMath.ClampInt(this.ReflectedDamage, 0, 2000);
			GameNetworkMessage.WriteIntToPacket(this.AbsorbedDamage, CompressionBasic.AgentHitDamageCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.InflictedDamage, CompressionBasic.AgentHitDamageCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ModifiedDamage, CompressionBasic.AgentHitModifiedDamageCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ReflectedDamage, CompressionBasic.AgentHitDamageCompressionInfo);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000089C8 File Offset: 0x00006BC8
		protected override bool OnRead()
		{
			bool flag = true;
			this.AttackerAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.VictimAgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.MissionObjectHitId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.DamageType = (DamageTypes)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageTypeCompressionInfo, ref flag);
			this.CrushedThrough = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.Chamber = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsRangedAttack = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsFriendlyFire = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsFatalDamage = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsSpecialDamage = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.BodyPartHit = (BoneBodyPartType)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitBodyPartCompressionInfo, ref flag);
			this.HitSpeed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AgentHitRelativeSpeedCompressionInfo, ref flag);
			this.Distance = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AgentHitRelativeSpeedCompressionInfo, ref flag);
			this.AbsorbedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageCompressionInfo, ref flag);
			this.InflictedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageCompressionInfo, ref flag);
			this.ModifiedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitModifiedDamageCompressionInfo, ref flag);
			this.ReflectedDamage = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AgentHitDamageCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00008ADE File Offset: 0x00006CDE
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00008AE6 File Offset: 0x00006CE6
		protected override string OnGetLogFormat()
		{
			return "Agent got hit.";
		}
	}
}
