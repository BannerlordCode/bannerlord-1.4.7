using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003BA RID: 954
	public class SiegeTowerSpawner : SpawnerBase
	{
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600355F RID: 13663 RVA: 0x000DB74D File Offset: 0x000D994D
		public float RampRotationRadian
		{
			get
			{
				return this.RampRotationDegree * 0.017453292f;
			}
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000DB75C File Offset: 0x000D995C
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._spawnerEditorHelper = new SpawnerEntityEditorHelper(this);
			this._spawnerEditorHelper.LockGhostParent = false;
			if (this._spawnerEditorHelper.IsValid)
			{
				this._spawnerEditorHelper.SetupGhostMovement(this.PathEntityName);
				this._spawnerEditorHelper.GivePermission("ramp", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.rotation, SpawnerEntityEditorHelper.Axis.x), new Action<float>(this.SetRampRotation));
				this._spawnerEditorHelper.GivePermission("ai_barrier_r", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.scale, SpawnerEntityEditorHelper.Axis.z), new Action<float>(this.SetAIBarrierRight));
				this._spawnerEditorHelper.GivePermission("ai_barrier_l", new SpawnerEntityEditorHelper.Permission(SpawnerEntityEditorHelper.PermissionType.scale, SpawnerEntityEditorHelper.Axis.z), new Action<float>(this.SetAIBarrierLeft));
			}
			this.OnEditorVariableChanged("RampRotationDegree");
			this.OnEditorVariableChanged("BarrierLength");
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000DB824 File Offset: 0x000D9A24
		private void SetRampRotation(float unusedArgument)
		{
			MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ramp").GetFrame();
			Vec3 vec = new Vec3(-frame.rotation.u.y, frame.rotation.u.x, 0f, -1f);
			float z = frame.rotation.u.z;
			float num = MathF.Atan2(vec.Length, z);
			if ((double)vec.x < 0.0)
			{
				num = -num;
				num += 6.2831855f;
			}
			float num2 = num;
			this.RampRotationDegree = num2 * 57.29578f;
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000DB8C8 File Offset: 0x000D9AC8
		private void SetAIBarrierRight(float barrierScale)
		{
			this.BarrierLength = barrierScale;
			MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_l").GetFrame();
			MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_r").GetFrame();
			frame.rotation.u = frame2.rotation.u;
			this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_l", frame, false);
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000DB934 File Offset: 0x000D9B34
		private void SetAIBarrierLeft(float barrierScale)
		{
			this.BarrierLength = barrierScale;
			MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_l").GetFrame();
			MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_r").GetFrame();
			frame2.rotation.u = frame.rotation.u;
			this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_r", frame2, false);
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000DB99D File Offset: 0x000D9B9D
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._spawnerEditorHelper.Tick(dt);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000DB9B4 File Offset: 0x000D9BB4
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "PathEntityName")
			{
				this._spawnerEditorHelper.SetupGhostMovement(this.PathEntityName);
				return;
			}
			if (variableName == "EnableAutoGhostMovement")
			{
				this._spawnerEditorHelper.SetEnableAutoGhostMovement(this.EnableAutoGhostMovement);
				return;
			}
			if (variableName == "RampRotationDegree")
			{
				MatrixFrame frame = this._spawnerEditorHelper.GetGhostEntityOrChild("ramp").GetFrame();
				frame.rotation = Mat3.Identity;
				frame.rotation.RotateAboutSide(this.RampRotationRadian);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ramp", frame, true);
				return;
			}
			if (variableName == "BarrierLength")
			{
				MatrixFrame frame2 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_l").GetFrame();
				frame2.rotation.u.Normalize();
				frame2.rotation.u = frame2.rotation.u * MathF.Max(0.01f, MathF.Abs(this.BarrierLength));
				MatrixFrame frame3 = this._spawnerEditorHelper.GetGhostEntityOrChild("ai_barrier_r").GetFrame();
				frame3.rotation.u = frame2.rotation.u;
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_l", frame2, true);
				this._spawnerEditorHelper.ChangeStableChildMatrixFrameAndApply("ai_barrier_r", frame3, true);
				return;
			}
			if (variableName == "SpeedModifierFactor")
			{
				this.SpeedModifierFactor = MathF.Clamp(this.SpeedModifierFactor, 0.8f, 1.2f);
			}
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000DBB3D File Offset: 0x000D9D3D
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000DBB54 File Offset: 0x000D9D54
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			SiegeTower firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<SiegeTower>();
			firstScriptOfType.AddOnDeployTag = this.AddOnDeployTag;
			firstScriptOfType.RemoveOnDeployTag = this.RemoveOnDeployTag;
			firstScriptOfType.MaxSpeed *= this.SpeedModifierFactor;
			firstScriptOfType.MinSpeed *= this.SpeedModifierFactor;
			Mat3 identity = Mat3.Identity;
			identity.RotateAboutSide(this.RampRotationRadian);
			firstScriptOfType.AssignParametersFromSpawner(this.PathEntityName, this.TargetWallSegmentTag, this.SideTag, this.SoilNavMeshID1, this.SoilNavMeshID2, this.DitchNavMeshID1, this.DitchNavMeshID2, this.GroundToSoilNavMeshID1, this.GroundToSoilNavMeshID2, this.SoilGenericNavMeshID, this.GroundGenericNavMeshID, identity, this.BarrierTagToRemove);
		}

		// Token: 0x040016CC RID: 5836
		private const float _modifierFactorUpperLimit = 1.2f;

		// Token: 0x040016CD RID: 5837
		private const float _modifierFactorLowerLimit = 0.8f;

		// Token: 0x040016CE RID: 5838
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame wait_pos_ground = MatrixFrame.Zero;

		// Token: 0x040016CF RID: 5839
		[EditorVisibleScriptComponentVariable(true)]
		public string SideTag;

		// Token: 0x040016D0 RID: 5840
		[EditorVisibleScriptComponentVariable(true)]
		public string TargetWallSegmentTag = "";

		// Token: 0x040016D1 RID: 5841
		[EditorVisibleScriptComponentVariable(true)]
		public string PathEntityName = "Path";

		// Token: 0x040016D2 RID: 5842
		[EditorVisibleScriptComponentVariable(true)]
		public int SoilNavMeshID1 = -1;

		// Token: 0x040016D3 RID: 5843
		[EditorVisibleScriptComponentVariable(true)]
		public int SoilNavMeshID2 = -1;

		// Token: 0x040016D4 RID: 5844
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID1 = -1;

		// Token: 0x040016D5 RID: 5845
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID2 = -1;

		// Token: 0x040016D6 RID: 5846
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToSoilNavMeshID1 = -1;

		// Token: 0x040016D7 RID: 5847
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToSoilNavMeshID2 = -1;

		// Token: 0x040016D8 RID: 5848
		[EditorVisibleScriptComponentVariable(true)]
		public int SoilGenericNavMeshID = -1;

		// Token: 0x040016D9 RID: 5849
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundGenericNavMeshID = -1;

		// Token: 0x040016DA RID: 5850
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x040016DB RID: 5851
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x040016DC RID: 5852
		[EditorVisibleScriptComponentVariable(true)]
		public float RampRotationDegree;

		// Token: 0x040016DD RID: 5853
		[EditorVisibleScriptComponentVariable(true)]
		public float BarrierLength = 1f;

		// Token: 0x040016DE RID: 5854
		[EditorVisibleScriptComponentVariable(true)]
		public float SpeedModifierFactor = 1f;

		// Token: 0x040016DF RID: 5855
		public bool EnableAutoGhostMovement;

		// Token: 0x040016E0 RID: 5856
		[SpawnerBase.SpawnerPermissionField]
		[RestrictedAccess]
		public MatrixFrame ai_barrier_l = MatrixFrame.Zero;

		// Token: 0x040016E1 RID: 5857
		[SpawnerBase.SpawnerPermissionField]
		[RestrictedAccess]
		public MatrixFrame ai_barrier_r = MatrixFrame.Zero;

		// Token: 0x040016E2 RID: 5858
		[EditorVisibleScriptComponentVariable(true)]
		public string BarrierTagToRemove = string.Empty;
	}
}
