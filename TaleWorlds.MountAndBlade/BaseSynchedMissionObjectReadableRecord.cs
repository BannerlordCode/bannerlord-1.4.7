using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000363 RID: 867
	[DefineSynchedMissionObjectType(typeof(SynchedMissionObject))]
	public struct BaseSynchedMissionObjectReadableRecord
	{
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060031B5 RID: 12725 RVA: 0x000CB395 File Offset: 0x000C9595
		// (set) Token: 0x060031B6 RID: 12726 RVA: 0x000CB39D File Offset: 0x000C959D
		public bool SetVisibilityExcludeParents { get; private set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060031B7 RID: 12727 RVA: 0x000CB3A6 File Offset: 0x000C95A6
		// (set) Token: 0x060031B8 RID: 12728 RVA: 0x000CB3AE File Offset: 0x000C95AE
		public bool SynchTransform { get; private set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x000CB3B7 File Offset: 0x000C95B7
		// (set) Token: 0x060031BA RID: 12730 RVA: 0x000CB3BF File Offset: 0x000C95BF
		public MatrixFrame GameObjectFrame { get; private set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060031BB RID: 12731 RVA: 0x000CB3C8 File Offset: 0x000C95C8
		// (set) Token: 0x060031BC RID: 12732 RVA: 0x000CB3D0 File Offset: 0x000C95D0
		public bool SynchronizeFrameOverTime { get; private set; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060031BD RID: 12733 RVA: 0x000CB3D9 File Offset: 0x000C95D9
		// (set) Token: 0x060031BE RID: 12734 RVA: 0x000CB3E1 File Offset: 0x000C95E1
		public MatrixFrame LastSynchedFrame { get; private set; }

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x000CB3EA File Offset: 0x000C95EA
		// (set) Token: 0x060031C0 RID: 12736 RVA: 0x000CB3F2 File Offset: 0x000C95F2
		public float Duration { get; private set; }

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060031C1 RID: 12737 RVA: 0x000CB3FB File Offset: 0x000C95FB
		// (set) Token: 0x060031C2 RID: 12738 RVA: 0x000CB403 File Offset: 0x000C9603
		public bool HasSkeleton { get; private set; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x000CB40C File Offset: 0x000C960C
		// (set) Token: 0x060031C4 RID: 12740 RVA: 0x000CB414 File Offset: 0x000C9614
		public bool SynchAnimation { get; private set; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060031C5 RID: 12741 RVA: 0x000CB41D File Offset: 0x000C961D
		// (set) Token: 0x060031C6 RID: 12742 RVA: 0x000CB425 File Offset: 0x000C9625
		public int AnimationIndex { get; private set; }

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x000CB42E File Offset: 0x000C962E
		// (set) Token: 0x060031C8 RID: 12744 RVA: 0x000CB436 File Offset: 0x000C9636
		public float AnimationSpeed { get; private set; }

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x000CB43F File Offset: 0x000C963F
		// (set) Token: 0x060031CA RID: 12746 RVA: 0x000CB447 File Offset: 0x000C9647
		public float AnimationParameter { get; private set; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060031CB RID: 12747 RVA: 0x000CB450 File Offset: 0x000C9650
		// (set) Token: 0x060031CC RID: 12748 RVA: 0x000CB458 File Offset: 0x000C9658
		public bool IsSkeletonAnimationPaused { get; private set; }

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060031CD RID: 12749 RVA: 0x000CB461 File Offset: 0x000C9661
		// (set) Token: 0x060031CE RID: 12750 RVA: 0x000CB469 File Offset: 0x000C9669
		public bool SynchColors { get; private set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060031CF RID: 12751 RVA: 0x000CB472 File Offset: 0x000C9672
		// (set) Token: 0x060031D0 RID: 12752 RVA: 0x000CB47A File Offset: 0x000C967A
		public uint Color { get; private set; }

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060031D1 RID: 12753 RVA: 0x000CB483 File Offset: 0x000C9683
		// (set) Token: 0x060031D2 RID: 12754 RVA: 0x000CB48B File Offset: 0x000C968B
		public uint Color2 { get; private set; }

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060031D3 RID: 12755 RVA: 0x000CB494 File Offset: 0x000C9694
		// (set) Token: 0x060031D4 RID: 12756 RVA: 0x000CB49C File Offset: 0x000C969C
		public bool IsDisabled { get; private set; }

		// Token: 0x060031D5 RID: 12757 RVA: 0x000CB4A8 File Offset: 0x000C96A8
		public bool ReadFromNetwork(ref bool bufferReadValid)
		{
			this.SetVisibilityExcludeParents = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			this.SynchTransform = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if (this.SynchTransform)
			{
				this.GameObjectFrame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref bufferReadValid);
				this.SynchronizeFrameOverTime = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				if (this.SynchronizeFrameOverTime)
				{
					this.LastSynchedFrame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref bufferReadValid);
					this.Duration = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagCapturePointDurationCompressionInfo, ref bufferReadValid);
				}
			}
			this.HasSkeleton = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if (this.HasSkeleton)
			{
				this.SynchAnimation = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				if (this.SynchAnimation)
				{
					this.AnimationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.AnimationIndexCompressionInfo, ref bufferReadValid);
					this.AnimationSpeed = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationSpeedCompressionInfo, ref bufferReadValid);
					this.AnimationParameter = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AnimationProgressCompressionInfo, ref bufferReadValid);
					this.IsSkeletonAnimationPaused = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				}
			}
			this.SynchColors = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if (this.SynchColors)
			{
				this.Color = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref bufferReadValid);
				this.Color2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref bufferReadValid);
			}
			this.IsDisabled = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			return bufferReadValid;
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000CB5BD File Offset: 0x000C97BD
		public void SetSetVisibilityExcludeParents(bool visible)
		{
			this.SetVisibilityExcludeParents = visible;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000CB5C8 File Offset: 0x000C97C8
		public static ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> CreateFromNetworkWithTypeIndex(int typeIndex)
		{
			bool flag = true;
			BaseSynchedMissionObjectReadableRecord baseSynchedMissionObjectReadableRecord = default(BaseSynchedMissionObjectReadableRecord);
			baseSynchedMissionObjectReadableRecord.ReadFromNetwork(ref flag);
			ISynchedMissionObjectReadableRecord synchedMissionObjectReadableRecord = null;
			if (typeIndex >= 0)
			{
				synchedMissionObjectReadableRecord = Activator.CreateInstance(GameNetwork.GetSynchedMissionObjectReadableRecordTypeFromIndex(typeIndex)) as ISynchedMissionObjectReadableRecord;
				synchedMissionObjectReadableRecord.ReadFromNetwork(ref flag);
			}
			return new ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord>(baseSynchedMissionObjectReadableRecord, synchedMissionObjectReadableRecord);
		}
	}
}
