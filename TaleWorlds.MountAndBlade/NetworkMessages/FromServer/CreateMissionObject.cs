using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000086 RID: 134
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateMissionObject : GameNetworkMessage
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00009A09 File Offset: 0x00007C09
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00009A11 File Offset: 0x00007C11
		public MissionObjectId ObjectId { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00009A1A File Offset: 0x00007C1A
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x00009A22 File Offset: 0x00007C22
		public string Prefab { get; private set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00009A2B File Offset: 0x00007C2B
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x00009A33 File Offset: 0x00007C33
		public MatrixFrame Frame { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00009A3C File Offset: 0x00007C3C
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00009A44 File Offset: 0x00007C44
		public List<MissionObjectId> ChildObjectIds { get; private set; }

		// Token: 0x0600053D RID: 1341 RVA: 0x00009A4D File Offset: 0x00007C4D
		public CreateMissionObject(MissionObjectId objectId, string prefab, MatrixFrame frame, List<MissionObjectId> childObjectIds)
		{
			this.ObjectId = objectId;
			this.Prefab = prefab;
			this.Frame = frame;
			this.ChildObjectIds = childObjectIds;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00009A72 File Offset: 0x00007C72
		public CreateMissionObject()
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00009A7C File Offset: 0x00007C7C
		protected override bool OnRead()
		{
			bool flag = true;
			this.ObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Prefab = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.EntityChildCountCompressionInfo, ref flag);
			if (flag)
			{
				this.ChildObjectIds = new List<MissionObjectId>(num);
				for (int i = 0; i < num; i++)
				{
					if (flag)
					{
						this.ChildObjectIds.Add(GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag));
					}
				}
			}
			return flag;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00009AF0 File Offset: 0x00007CF0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.ObjectId);
			GameNetworkMessage.WriteStringToPacket(this.Prefab);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
			GameNetworkMessage.WriteIntToPacket(this.ChildObjectIds.Count, CompressionBasic.EntityChildCountCompressionInfo);
			foreach (MissionObjectId missionObjectId in this.ChildObjectIds)
			{
				GameNetworkMessage.WriteMissionObjectIdToPacket(missionObjectId);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00009B78 File Offset: 0x00007D78
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00009B80 File Offset: 0x00007D80
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Create a MissionObject with index: ", this.ObjectId, " from prefab: ", this.Prefab, " at frame: ", this.Frame });
		}
	}
}
