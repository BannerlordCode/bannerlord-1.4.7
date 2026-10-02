using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003B9 RID: 953
	public class SiegeLadderSpawner : SpawnerBase
	{
		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06003554 RID: 13652 RVA: 0x000DB2A8 File Offset: 0x000D94A8
		public float UpperStateRotationRadian
		{
			get
			{
				return this.UpperStateRotationDegree * 0.017453292f;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003555 RID: 13653 RVA: 0x000DB2B6 File Offset: 0x000D94B6
		public float DownStateRotationRadian
		{
			get
			{
				return this.DownStateRotationDegree * 0.017453292f;
			}
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000DB2C4 File Offset: 0x000D94C4
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._spawnerEditorHelper = new SpawnerEntityEditorHelper(this);
			if (this._spawnerEditorHelper.IsValid)
			{
				this._spawnerEditorHelper.GivePermission("ladder_up_state", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.rotation, SpawnerEntityEditorHelper.Axis.x), new Action<float>(this.OnLadderUpStateChange));
				this._spawnerEditorHelper.GivePermission("ladder_down_state", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.rotation, SpawnerEntityEditorHelper.Axis.x), new Action<float>(this.OnLadderDownStateChange));
			}
			this.OnEditorVariableChanged("UpperStateRotationDegree");
			this.OnEditorVariableChanged("DownStateRotationDegree");
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000DB34C File Offset: 0x000D954C
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._spawnerEditorHelper.Tick(dt);
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000DB361 File Offset: 0x000D9561
		private void OnLadderUpStateChange(float rotation)
		{
			if (rotation > -0.20135832f)
			{
				rotation = -0.20135832f;
				this.UpperStateRotationDegree = rotation * 57.29578f;
				this.OnEditorVariableChanged("UpperStateRotationDegree");
				return;
			}
			this.UpperStateRotationDegree = rotation * 57.29578f;
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000DB398 File Offset: 0x000D9598
		private void OnLadderDownStateChange(float unusedArgument)
		{
			GameEntity ghostEntityOrChild = this._spawnerEditorHelper.GetGhostEntityOrChild("ladder_down_state");
			this.DownStateRotationDegree = Vec3.AngleBetweenTwoVectors(Vec3.Up, ghostEntityOrChild.GetFrame().rotation.u) * 57.29578f;
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000DB3DC File Offset: 0x000D95DC
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "UpperStateRotationDegree")
			{
				if (this.UpperStateRotationDegree > -11.536982f)
				{
					this.UpperStateRotationDegree = -11.536982f;
				}
				MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ladder_up_state").GetFrame();
				frame.rotation = Mat3.Identity;
				frame.rotation.RotateAboutSide(this.UpperStateRotationRadian);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ladder_up_state", frame, true);
				return;
			}
			if (variableName == "DownStateRotationDegree")
			{
				MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ladder_down_state").GetFrame();
				frame2.rotation = Mat3.Identity;
				frame2.rotation.RotateAboutUp(1.5707964f);
				frame2.rotation.RotateAboutSide(this.DownStateRotationRadian);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ladder_down_state", frame2, true);
			}
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000DB4C0 File Offset: 0x000D96C0
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (base.Scene.IsMultiplayerScene())
			{
				if (this.OnWallNavMeshId == 0 || this.OnWallNavMeshId % 10 == 1)
				{
					MBEditor.AddEntityWarning(base.GameEntity, "OnWallNavMeshId's ones digit cannot be 1 and OnWallNavMeshId cannot be 0 in a multiplayer scene.");
					flag = true;
				}
			}
			else if (this.OnWallNavMeshId == -1 || this.OnWallNavMeshId == 0 || this.OnWallNavMeshId % 10 == 1)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "OnWallNavMeshId's ones digit cannot be 1 and OnWallNavMeshId cannot be -1 or 0 in a singleplayer scene.");
				flag = true;
			}
			if (this.OnWallNavMeshId != -1)
			{
				List<GameEntity> list = new List<GameEntity>();
				base.Scene.GetEntities(ref list);
				foreach (GameEntity gameEntity in list)
				{
					SiegeLadderSpawner firstScriptOfType = gameEntity.GetFirstScriptOfType<SiegeLadderSpawner>();
					if (firstScriptOfType != null && gameEntity != base.GameEntity && this.OnWallNavMeshId == firstScriptOfType.OnWallNavMeshId && base.GameEntity.GetVisibilityLevelMaskIncludingParents() == gameEntity.GetVisibilityLevelMaskIncludingParents())
					{
						MBEditor.AddEntityWarning(base.GameEntity, "OnWallNavMeshId must not be shared with any other siege ladder.");
					}
				}
			}
			return flag;
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000DB5E4 File Offset: 0x000D97E4
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000DB5FC File Offset: 0x000D97FC
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			SiegeLadder firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<SiegeLadder>();
			firstScriptOfType.AddOnDeployTag = this.AddOnDeployTag;
			firstScriptOfType.RemoveOnDeployTag = this.RemoveOnDeployTag;
			firstScriptOfType.AssignParametersFromSpawner(this.SideTag, this.TargetWallSegmentTag, this.OnWallNavMeshId, this.DownStateRotationRadian, this.UpperStateRotationRadian, this.BarrierTagToRemove, this.IndestructibleMerlonsTag);
			List<GameEntity> list = new List<GameEntity>();
			_spawnerMissionHelper.SpawnedEntity.GetChildrenRecursive(ref list);
			list.Find((GameEntity x) => x.Name == "initial_wait_pos").GetFirstScriptOfType<TacticalPosition>().SetWidth(this.TacticalPositionWidth);
		}

		// Token: 0x040016BC RID: 5820
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame fork_holder = MatrixFrame.Zero;

		// Token: 0x040016BD RID: 5821
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame initial_wait_pos = MatrixFrame.Zero;

		// Token: 0x040016BE RID: 5822
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame use_push = MatrixFrame.Zero;

		// Token: 0x040016BF RID: 5823
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame stand_position_wall_push = MatrixFrame.Zero;

		// Token: 0x040016C0 RID: 5824
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame distance_holder = MatrixFrame.Zero;

		// Token: 0x040016C1 RID: 5825
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame stand_position_ground_wait = MatrixFrame.Zero;

		// Token: 0x040016C2 RID: 5826
		[EditorVisibleScriptComponentVariable(true)]
		public string SideTag;

		// Token: 0x040016C3 RID: 5827
		[EditorVisibleScriptComponentVariable(true)]
		public string TargetWallSegmentTag = "";

		// Token: 0x040016C4 RID: 5828
		[EditorVisibleScriptComponentVariable(true)]
		public int OnWallNavMeshId = -1;

		// Token: 0x040016C5 RID: 5829
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x040016C6 RID: 5830
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x040016C7 RID: 5831
		[EditorVisibleScriptComponentVariable(true)]
		public float UpperStateRotationDegree;

		// Token: 0x040016C8 RID: 5832
		[EditorVisibleScriptComponentVariable(true)]
		public float DownStateRotationDegree = 90f;

		// Token: 0x040016C9 RID: 5833
		public float TacticalPositionWidth = 1f;

		// Token: 0x040016CA RID: 5834
		[EditorVisibleScriptComponentVariable(true)]
		public string BarrierTagToRemove = string.Empty;

		// Token: 0x040016CB RID: 5835
		[EditorVisibleScriptComponentVariable(true)]
		public string IndestructibleMerlonsTag = string.Empty;
	}
}
