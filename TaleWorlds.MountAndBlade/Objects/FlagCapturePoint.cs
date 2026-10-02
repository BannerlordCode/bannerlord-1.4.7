using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003A0 RID: 928
	public class FlagCapturePoint : SynchedMissionObject
	{
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060034D9 RID: 13529 RVA: 0x000D97A8 File Offset: 0x000D79A8
		[EditableScriptComponentVariable(false, "")]
		public Vec3 Position
		{
			get
			{
				return base.GameEntity.GlobalPosition;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060034DA RID: 13530 RVA: 0x000D97C3 File Offset: 0x000D79C3
		public int FlagChar
		{
			get
			{
				return 65 + this.FlagIndex;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060034DB RID: 13531 RVA: 0x000D97CE File Offset: 0x000D79CE
		public bool IsContested
		{
			get
			{
				return this._currentDirection == CaptureTheFlagFlagDirection.Down;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060034DC RID: 13532 RVA: 0x000D97D9 File Offset: 0x000D79D9
		public bool IsFullyRaised
		{
			get
			{
				return this._currentDirection == CaptureTheFlagFlagDirection.None;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060034DD RID: 13533 RVA: 0x000D97E4 File Offset: 0x000D79E4
		public bool IsDeactivated
		{
			get
			{
				return !base.GameEntity.IsVisibleIncludeParents();
			}
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x000D9802 File Offset: 0x000D7A02
		protected internal override void OnMissionReset()
		{
			this._currentDirection = CaptureTheFlagFlagDirection.None;
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x000D980C File Offset: 0x000D7A0C
		public void ResetPointAsServer(uint defaultColor, uint defaultColor2)
		{
			MatrixFrame globalFrame = this._flagTopBoundary.GetGlobalFrame();
			this._flagHolder.SetGlobalFrameSynched(ref globalFrame, false);
			this.SetTeamColorsWithAllSynched(defaultColor, defaultColor2);
			this.SetVisibleWithAllSynched(true, false);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x000D9843 File Offset: 0x000D7A43
		public void RemovePointAsServer()
		{
			this.SetVisibleWithAllSynched(false, false);
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x000D9850 File Offset: 0x000D7A50
		protected internal override void OnInit()
		{
			this._flagHolder = base.GameEntity.GetFirstChildEntityWithTag("score_stand").GetFirstScriptOfType<SynchedMissionObject>();
			this._theFlag = this._flagHolder.GameEntity.GetFirstChildEntityWithTag("flag_white").GetFirstScriptOfType<SynchedMissionObject>();
			this._flagBottomBoundary = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("flag_raising_bottom"));
			this._flagTopBoundary = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("flag_raising_top"));
			MatrixFrame globalFrame = this._flagTopBoundary.GetGlobalFrame();
			this._flagHolder.GameEntity.SetGlobalFrame(in globalFrame, true);
			this._flagDependentObjects = new List<SynchedMissionObject>();
			foreach (WeakGameEntity weakGameEntity in Mission.Current.Scene.FindWeakEntitiesWithTag("depends_flag_" + this.FlagIndex).ToList<WeakGameEntity>())
			{
				SynchedMissionObject firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SynchedMissionObject>();
				this._flagDependentObjects.Add(firstScriptOfType);
			}
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x000D9988 File Offset: 0x000D7B88
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				DebugExtensions.RenderDebugCircleOnTerrain(base.Scene, base.GameEntity.GetGlobalFrame(), 4f, 2852192000U, true, false);
				DebugExtensions.RenderDebugCircleOnTerrain(base.Scene, base.GameEntity.GetGlobalFrame(), 6f, 2868838400U, true, false);
			}
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000D99F4 File Offset: 0x000D7BF4
		public void OnAfterTick(bool canOwnershipChange, out bool ownerTeamChanged)
		{
			ownerTeamChanged = false;
			if (this._flagHolder.SynchronizeCompleted)
			{
				bool flag = this._flagHolder.GameEntity.GlobalPosition.DistanceSquared(this._flagTopBoundary.GlobalPosition).ApproximatelyEqualsTo(0f, 1E-05f);
				if (canOwnershipChange)
				{
					if (!flag)
					{
						ownerTeamChanged = true;
						return;
					}
					this._currentDirection = CaptureTheFlagFlagDirection.None;
					return;
				}
				else if (flag)
				{
					this._currentDirection = CaptureTheFlagFlagDirection.None;
				}
			}
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x000D9A64 File Offset: 0x000D7C64
		public void SetMoveFlag(CaptureTheFlagFlagDirection directionTo, float speedMultiplier = 1f)
		{
			float flagProgress = this.GetFlagProgress();
			float num = 1f / speedMultiplier;
			float num2 = ((directionTo == CaptureTheFlagFlagDirection.Up) ? (1f - flagProgress) : flagProgress);
			float num3 = 10f * num;
			float num4 = num2 * num3;
			this._currentDirection = directionTo;
			MatrixFrame matrixFrame;
			if (directionTo != CaptureTheFlagFlagDirection.Up)
			{
				if (directionTo != CaptureTheFlagFlagDirection.Down)
				{
					throw new ArgumentOutOfRangeException("directionTo", directionTo, null);
				}
				matrixFrame = this._flagBottomBoundary.GetFrame();
			}
			else
			{
				matrixFrame = this._flagTopBoundary.GetFrame();
			}
			this._flagHolder.SetFrameSynchedOverTime(ref matrixFrame, num4, false);
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000D9AE7 File Offset: 0x000D7CE7
		public void ChangeMovementSpeed(float speedMultiplier)
		{
			if (this._currentDirection != CaptureTheFlagFlagDirection.None)
			{
				this.SetMoveFlag(this._currentDirection, speedMultiplier);
			}
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000D9B00 File Offset: 0x000D7D00
		public void SetMoveNone()
		{
			this._currentDirection = CaptureTheFlagFlagDirection.None;
			MatrixFrame frame = this._flagHolder.GameEntity.GetFrame();
			this._flagHolder.SetFrameSynched(ref frame, false);
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000D9B38 File Offset: 0x000D7D38
		public void SetVisibleWithAllSynched(bool value, bool forceChildrenVisible = false)
		{
			this.SetVisibleSynched(value, forceChildrenVisible);
			foreach (SynchedMissionObject synchedMissionObject in this._flagDependentObjects)
			{
				synchedMissionObject.SetVisibleSynched(value, false);
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000D9B94 File Offset: 0x000D7D94
		public void SetTeamColorsWithAllSynched(uint color, uint color2)
		{
			this._theFlag.SetTeamColorsSynched(color, color2);
			foreach (SynchedMissionObject synchedMissionObject in this._flagDependentObjects)
			{
				synchedMissionObject.SetTeamColorsSynched(color, color2);
			}
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000D9BF4 File Offset: 0x000D7DF4
		public uint GetFlagColor()
		{
			return this._theFlag.Color;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000D9C01 File Offset: 0x000D7E01
		public uint GetFlagColor2()
		{
			return this._theFlag.Color2;
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000D9C10 File Offset: 0x000D7E10
		public float GetFlagProgress()
		{
			return MathF.Clamp((this._theFlag.GameEntity.GlobalPosition.z - this._flagBottomBoundary.GlobalPosition.z) / (this._flagTopBoundary.GlobalPosition.z - this._flagBottomBoundary.GlobalPosition.z), 0f, 1f);
		}

		// Token: 0x0400166C RID: 5740
		public const float PointRadius = 4f;

		// Token: 0x0400166D RID: 5741
		public const float RadiusMultiplierForContestedArea = 1.5f;

		// Token: 0x0400166E RID: 5742
		private const float TimeToTravelBetweenBoundaries = 10f;

		// Token: 0x0400166F RID: 5743
		public int FlagIndex;

		// Token: 0x04001670 RID: 5744
		private SynchedMissionObject _theFlag;

		// Token: 0x04001671 RID: 5745
		private SynchedMissionObject _flagHolder;

		// Token: 0x04001672 RID: 5746
		private GameEntity _flagBottomBoundary;

		// Token: 0x04001673 RID: 5747
		private GameEntity _flagTopBoundary;

		// Token: 0x04001674 RID: 5748
		private List<SynchedMissionObject> _flagDependentObjects;

		// Token: 0x04001675 RID: 5749
		private CaptureTheFlagFlagDirection _currentDirection = CaptureTheFlagFlagDirection.None;
	}
}
