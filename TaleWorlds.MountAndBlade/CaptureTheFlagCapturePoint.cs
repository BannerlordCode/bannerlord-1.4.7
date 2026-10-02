using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032C RID: 812
	public class CaptureTheFlagCapturePoint
	{
		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x000B1061 File Offset: 0x000AF261
		// (set) Token: 0x06002DD6 RID: 11734 RVA: 0x000B1069 File Offset: 0x000AF269
		public float Progress { get; set; }

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002DD7 RID: 11735 RVA: 0x000B1072 File Offset: 0x000AF272
		// (set) Token: 0x06002DD8 RID: 11736 RVA: 0x000B107A File Offset: 0x000AF27A
		public CaptureTheFlagFlagDirection Direction { get; set; }

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002DD9 RID: 11737 RVA: 0x000B1083 File Offset: 0x000AF283
		// (set) Token: 0x06002DDA RID: 11738 RVA: 0x000B108B File Offset: 0x000AF28B
		public float Speed { get; set; }

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002DDB RID: 11739 RVA: 0x000B1094 File Offset: 0x000AF294
		// (set) Token: 0x06002DDC RID: 11740 RVA: 0x000B109C File Offset: 0x000AF29C
		public MatrixFrame InitialFlagFrame { get; private set; }

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002DDD RID: 11741 RVA: 0x000B10A5 File Offset: 0x000AF2A5
		// (set) Token: 0x06002DDE RID: 11742 RVA: 0x000B10AD File Offset: 0x000AF2AD
		public GameEntity FlagEntity { get; private set; }

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002DDF RID: 11743 RVA: 0x000B10B6 File Offset: 0x000AF2B6
		// (set) Token: 0x06002DE0 RID: 11744 RVA: 0x000B10BE File Offset: 0x000AF2BE
		public SynchedMissionObject FlagHolder { get; private set; }

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002DE1 RID: 11745 RVA: 0x000B10C7 File Offset: 0x000AF2C7
		// (set) Token: 0x06002DE2 RID: 11746 RVA: 0x000B10CF File Offset: 0x000AF2CF
		public GameEntity FlagBottomBoundary { get; private set; }

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x000B10D8 File Offset: 0x000AF2D8
		// (set) Token: 0x06002DE4 RID: 11748 RVA: 0x000B10E0 File Offset: 0x000AF2E0
		public GameEntity FlagTopBoundary { get; private set; }

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x000B10E9 File Offset: 0x000AF2E9
		public BattleSideEnum BattleSide { get; }

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x000B10F1 File Offset: 0x000AF2F1
		public int Index { get; }

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x000B10F9 File Offset: 0x000AF2F9
		// (set) Token: 0x06002DE8 RID: 11752 RVA: 0x000B1101 File Offset: 0x000AF301
		public bool UpdateFlag { get; set; }

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000B110C File Offset: 0x000AF30C
		public CaptureTheFlagCapturePoint(GameEntity flagPole, BattleSideEnum battleSide, int index)
		{
			this.Reset();
			this.BattleSide = battleSide;
			this.Index = index;
			this.FlagHolder = flagPole.CollectChildrenEntitiesWithTag("score_stand").SingleOrDefault<GameEntity>().GetFirstScriptOfType<SynchedMissionObject>();
			this.FlagEntity = GameEntity.CreateFromWeakEntity(this.FlagHolder.GameEntity.GetChildren().Single<WeakGameEntity>((WeakGameEntity q) => q.HasTag("flag")));
			this.FlagHolder.GameEntity.SetEntityFlags(this.FlagHolder.GameEntity.EntityFlags | EntityFlags.NoOcclusionCulling);
			this.FlagEntity.EntityFlags |= EntityFlags.NoOcclusionCulling;
			this.FlagBottomBoundary = flagPole.GetChildren().Single<GameEntity>((GameEntity q) => q.HasTag("flag_raising_bottom"));
			this.FlagTopBoundary = flagPole.GetChildren().Single<GameEntity>((GameEntity q) => q.HasTag("flag_raising_top"));
			MatrixFrame globalFrame = this.FlagHolder.GameEntity.GetGlobalFrame();
			globalFrame.origin.z = this.FlagBottomBoundary.GetGlobalFrame().origin.z;
			this.InitialFlagFrame = globalFrame;
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000B126F File Offset: 0x000AF46F
		public void Reset()
		{
			this.Progress = 0f;
			this.Direction = CaptureTheFlagFlagDirection.None;
			this.Speed = 0f;
			this.UpdateFlag = false;
		}
	}
}
