using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035D RID: 861
	public class TrajectoryVisualizer : ScriptComponentBehavior
	{
		// Token: 0x0600314B RID: 12619 RVA: 0x000C8138 File Offset: 0x000C6338
		public void SetTrajectoryParams(Vec3 missileShootingPositionOffset, float missileSpeed, float verticalAngleMinInDegrees, float verticalAngleMaxInDegrees, float horizontalAngleRangeInDegrees, float airFrictionConstant)
		{
			this._trajectoryParams.MissileShootingPositionOffset = missileShootingPositionOffset;
			this._trajectoryParams.MissileSpeed = missileSpeed;
			this._trajectoryParams.VerticalAngleMinInDegrees = verticalAngleMinInDegrees;
			this._trajectoryParams.VerticalAngleMaxInDegrees = verticalAngleMaxInDegrees;
			this._trajectoryParams.HorizontalAngleRangeInDegrees = horizontalAngleRangeInDegrees;
			this._trajectoryParams.AirFrictionConstant = airFrictionConstant;
			this._trajectoryParams.IsValid = true;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000C819C File Offset: 0x000C639C
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000C81A4 File Offset: 0x000C63A4
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "ShowTrajectory")
			{
				if (this.ShowTrajectory && this._trajectoryMeshHolder == null && !base.GameEntity.IsGhostObject() && this._trajectoryParams.IsValid)
				{
					this._trajectoryMeshHolder = TaleWorlds.Engine.GameEntity.CreateEmpty(base.Scene, false, true, true);
					if (this._trajectoryMeshHolder != null)
					{
						this._trajectoryMeshHolder.EntityFlags |= EntityFlags.DontSaveToScene;
						MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
						Vec3 vec = globalFrame.origin + (globalFrame.rotation.s * this._trajectoryParams.MissileShootingPositionOffset.x + globalFrame.rotation.f * this._trajectoryParams.MissileShootingPositionOffset.y + globalFrame.rotation.u * this._trajectoryParams.MissileShootingPositionOffset.z);
						globalFrame.origin = vec;
						this._trajectoryMeshHolder.SetGlobalFrame(in globalFrame, true);
						this._trajectoryMeshHolder.ComputeTrajectoryVolume(this._trajectoryParams.MissileSpeed, this._trajectoryParams.VerticalAngleMaxInDegrees, this._trajectoryParams.VerticalAngleMinInDegrees, this._trajectoryParams.HorizontalAngleRangeInDegrees, this._trajectoryParams.AirFrictionConstant);
						base.GameEntity.AddChild(this._trajectoryMeshHolder.WeakEntity, true);
						this._trajectoryMeshHolder.SetVisibilityExcludeParents(false);
					}
				}
				if (this._trajectoryMeshHolder != null)
				{
					this._trajectoryMeshHolder.SetVisibilityExcludeParents(this.ShowTrajectory);
				}
			}
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x000C835E File Offset: 0x000C655E
		protected override void OnRemoved(int removeReason)
		{
			if (this._trajectoryMeshHolder != null)
			{
				this._trajectoryMeshHolder.Remove(removeReason);
			}
		}

		// Token: 0x040014AD RID: 5293
		public bool ShowTrajectory;

		// Token: 0x040014AE RID: 5294
		private GameEntity _trajectoryMeshHolder;

		// Token: 0x040014AF RID: 5295
		private TrajectoryVisualizer.TrajectoryParams _trajectoryParams;

		// Token: 0x0200063D RID: 1597
		private struct TrajectoryParams
		{
			// Token: 0x040020FD RID: 8445
			public Vec3 MissileShootingPositionOffset;

			// Token: 0x040020FE RID: 8446
			public float MissileSpeed;

			// Token: 0x040020FF RID: 8447
			public float VerticalAngleMinInDegrees;

			// Token: 0x04002100 RID: 8448
			public float VerticalAngleMaxInDegrees;

			// Token: 0x04002101 RID: 8449
			public float HorizontalAngleRangeInDegrees;

			// Token: 0x04002102 RID: 8450
			public float AirFrictionConstant;

			// Token: 0x04002103 RID: 8451
			public bool IsValid;
		}
	}
}
