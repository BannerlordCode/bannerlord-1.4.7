using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000366 RID: 870
	public class TacticalRegion : MissionObject
	{
		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x000CB8C4 File Offset: 0x000C9AC4
		// (set) Token: 0x060031EF RID: 12783 RVA: 0x000CB8CC File Offset: 0x000C9ACC
		public WorldPosition Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x060031F0 RID: 12784 RVA: 0x000CB8D5 File Offset: 0x000C9AD5
		// (set) Token: 0x060031F1 RID: 12785 RVA: 0x000CB8DD File Offset: 0x000C9ADD
		public List<TacticalPosition> LinkedTacticalPositions
		{
			get
			{
				return this._linkedTacticalPositions;
			}
			set
			{
				this._linkedTacticalPositions = value;
			}
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000CB8E8 File Offset: 0x000C9AE8
		protected internal override void OnInit()
		{
			base.OnInit();
			this._position = new WorldPosition(base.GameEntity.GetScenePointer(), base.GameEntity.GlobalPosition);
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000CB924 File Offset: 0x000C9B24
		public override void AfterMissionStart()
		{
			base.AfterMissionStart();
			this._linkedTacticalPositions = new List<TacticalPosition>();
			this._linkedTacticalPositions = (from c in base.GameEntity.GetChildren()
				where c.HasScriptOfType<TacticalPosition>()
				select c.GetFirstScriptOfType<TacticalPosition>()).ToList<TacticalPosition>();
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000CB9A4 File Offset: 0x000C9BA4
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._position = new WorldPosition(base.GameEntity.GetScenePointer(), UIntPtr.Zero, base.GameEntity.GlobalPosition, false);
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000CB9E4 File Offset: 0x000C9BE4
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._position.SetVec3(UIntPtr.Zero, base.GameEntity.GlobalPosition, false);
			if (this.IsEditorDebugRingVisible)
			{
				MBEditor.HelpersEnabled();
			}
		}

		// Token: 0x0400151A RID: 5402
		public bool IsEditorDebugRingVisible;

		// Token: 0x0400151B RID: 5403
		private WorldPosition _position;

		// Token: 0x0400151C RID: 5404
		public float radius = 1f;

		// Token: 0x0400151D RID: 5405
		private List<TacticalPosition> _linkedTacticalPositions;

		// Token: 0x0400151E RID: 5406
		public TacticalRegion.TacticalRegionTypeEnum tacticalRegionType;

		// Token: 0x02000643 RID: 1603
		public enum TacticalRegionTypeEnum
		{
			// Token: 0x04002124 RID: 8484
			Forest,
			// Token: 0x04002125 RID: 8485
			DifficultTerrain,
			// Token: 0x04002126 RID: 8486
			Opening
		}
	}
}
