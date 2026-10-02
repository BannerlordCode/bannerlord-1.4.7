using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000077 RID: 119
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AttachWeaponToAgent : GameNetworkMessage
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x00007F37 File Offset: 0x00006137
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x00007F3F File Offset: 0x0000613F
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00007F48 File Offset: 0x00006148
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00007F50 File Offset: 0x00006150
		public int AgentIndex { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00007F59 File Offset: 0x00006159
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00007F61 File Offset: 0x00006161
		public sbyte BoneIndex { get; private set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00007F6A File Offset: 0x0000616A
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00007F72 File Offset: 0x00006172
		public MatrixFrame AttachLocalFrame { get; private set; }

		// Token: 0x06000446 RID: 1094 RVA: 0x00007F7B File Offset: 0x0000617B
		public AttachWeaponToAgent(MissionWeapon weapon, int agentIndex, sbyte boneIndex, MatrixFrame attachLocalFrame)
		{
			this.Weapon = weapon;
			this.AgentIndex = agentIndex;
			this.BoneIndex = boneIndex;
			this.AttachLocalFrame = attachLocalFrame;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00007FA0 File Offset: 0x000061A0
		public AttachWeaponToAgent()
		{
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00007FA8 File Offset: 0x000061A8
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.BoneIndex, CompressionMission.BoneIndexCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(this.AttachLocalFrame.origin, CompressionBasic.LocalPositionCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(this.AttachLocalFrame.rotation);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00008000 File Offset: 0x00006200
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.BoneIndex = (sbyte)GameNetworkMessage.ReadIntFromPacket(CompressionMission.BoneIndexCompressionInfo, ref flag);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.LocalPositionCompressionInfo, ref flag);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref flag);
			if (flag)
			{
				this.AttachLocalFrame = new MatrixFrame(in mat, in vec);
			}
			return flag;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00008069 File Offset: 0x00006269
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00008074 File Offset: 0x00006274
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"AttachWeaponToAgent with name: ",
				(!this.Weapon.IsEmpty) ? this.Weapon.Item.Name : TextObject.GetEmpty(),
				" to bone index: ",
				this.BoneIndex,
				" on agent agent-index: ",
				this.AgentIndex
			});
		}
	}
}
