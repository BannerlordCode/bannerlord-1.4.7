using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000071 RID: 113
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddPrefabComponentToAgentBone : GameNetworkMessage
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00007893 File Offset: 0x00005A93
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000789B File Offset: 0x00005A9B
		public int AgentIndex { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000078A4 File Offset: 0x00005AA4
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x000078AC File Offset: 0x00005AAC
		public string PrefabName { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x000078B5 File Offset: 0x00005AB5
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x000078BD File Offset: 0x00005ABD
		public sbyte BoneIndex { get; private set; }

		// Token: 0x060003FA RID: 1018 RVA: 0x000078C6 File Offset: 0x00005AC6
		public AddPrefabComponentToAgentBone(int agentIndex, string prefabName, sbyte boneIndex)
		{
			this.AgentIndex = agentIndex;
			this.PrefabName = prefabName;
			this.BoneIndex = boneIndex;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000078E3 File Offset: 0x00005AE3
		public AddPrefabComponentToAgentBone()
		{
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000078EC File Offset: 0x00005AEC
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.PrefabName = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.BoneIndex = (sbyte)GameNetworkMessage.ReadIntFromPacket(CompressionMission.BoneIndexCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00007929 File Offset: 0x00005B29
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteStringToPacket(this.PrefabName);
			GameNetworkMessage.WriteIntToPacket((int)this.BoneIndex, CompressionMission.BoneIndexCompressionInfo);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00007951 File Offset: 0x00005B51
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000795C File Offset: 0x00005B5C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Add prefab component: ", this.PrefabName, " on bone with index: ", this.BoneIndex, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
