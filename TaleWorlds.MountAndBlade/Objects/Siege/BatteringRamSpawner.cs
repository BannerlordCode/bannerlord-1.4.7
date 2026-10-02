using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Objects.Siege
{
	// Token: 0x020003AD RID: 941
	public class BatteringRamSpawner : SpawnerBase
	{
		// Token: 0x06003535 RID: 13621 RVA: 0x000DAC5A File Offset: 0x000D8E5A
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._spawnerEditorHelper = new SpawnerEntityEditorHelper(this);
			this._spawnerEditorHelper.LockGhostParent = false;
			if (this._spawnerEditorHelper.IsValid)
			{
				this._spawnerEditorHelper.SetupGhostMovement(this.PathEntityName);
			}
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x000DAC98 File Offset: 0x000D8E98
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._spawnerEditorHelper.Tick(dt);
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000DACB0 File Offset: 0x000D8EB0
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
			if (variableName == "SpeedModifierFactor")
			{
				this.SpeedModifierFactor = MathF.Clamp(this.SpeedModifierFactor, 0.8f, 1.2f);
			}
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000DAD2C File Offset: 0x000D8F2C
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (!base.Scene.IsMultiplayerScene() && base.Scene.FindWeakEntitiesWithTag("ditch_filler").FirstOrDefault<WeakGameEntity>((WeakGameEntity df) => df.HasTag(this.SideTag)) != null)
			{
				if (this.DitchNavMeshID_1 >= 0 && !base.Scene.IsAnyFaceWithId(this.DitchNavMeshID_1))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'DitchNavMeshID_1' id.");
					flag = true;
				}
				if (this.DitchNavMeshID_2 >= 0 && !base.Scene.IsAnyFaceWithId(this.DitchNavMeshID_2))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'DitchNavMeshID_2' id.");
					flag = true;
				}
				if (this.GroundToBridgeNavMeshID_1 >= 0 && !base.Scene.IsAnyFaceWithId(this.GroundToBridgeNavMeshID_1))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'GroundToBridgeNavMeshID_1' id.");
					flag = true;
				}
				if (this.GroundToBridgeNavMeshID_2 >= 0 && !base.Scene.IsAnyFaceWithId(this.GroundToBridgeNavMeshID_2))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'GroundToBridgeNavMeshID_1' id.");
					flag = true;
				}
				if (this.BridgeNavMeshID_1 >= 0 && !base.Scene.IsAnyFaceWithId(this.BridgeNavMeshID_1))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'BridgeNavMeshID_1' id.");
					flag = true;
				}
				if (this.BridgeNavMeshID_2 >= 0 && !base.Scene.IsAnyFaceWithId(this.BridgeNavMeshID_2))
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Couldn't find any face with 'BridgeNavMeshID_2' id.");
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000DAE91 File Offset: 0x000D9091
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			this._spawnerMissionHelper = new SpawnerEntityMissionHelper(this, false);
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000DAEA8 File Offset: 0x000D90A8
		public override void AssignParameters(SpawnerEntityMissionHelper _spawnerMissionHelper)
		{
			BatteringRam firstScriptOfType = _spawnerMissionHelper.SpawnedEntity.GetFirstScriptOfType<BatteringRam>();
			firstScriptOfType.AddOnDeployTag = this.AddOnDeployTag;
			firstScriptOfType.RemoveOnDeployTag = this.RemoveOnDeployTag;
			firstScriptOfType.MaxSpeed *= this.SpeedModifierFactor;
			firstScriptOfType.MinSpeed *= this.SpeedModifierFactor;
			firstScriptOfType.AssignParametersFromSpawner(this.GateTag, this.SideTag, this.BridgeNavMeshID_1, this.BridgeNavMeshID_2, this.DitchNavMeshID_1, this.DitchNavMeshID_2, this.GroundToBridgeNavMeshID_1, this.GroundToBridgeNavMeshID_2, this.PathEntityName);
		}

		// Token: 0x0400169C RID: 5788
		private const float _modifierFactorUpperLimit = 1.2f;

		// Token: 0x0400169D RID: 5789
		private const float _modifierFactorLowerLimit = 0.8f;

		// Token: 0x0400169E RID: 5790
		[SpawnerBase.SpawnerPermissionField]
		public MatrixFrame wait_pos_ground = MatrixFrame.Zero;

		// Token: 0x0400169F RID: 5791
		[EditorVisibleScriptComponentVariable(true)]
		public string SideTag;

		// Token: 0x040016A0 RID: 5792
		[EditorVisibleScriptComponentVariable(true)]
		public string GateTag = "";

		// Token: 0x040016A1 RID: 5793
		[EditorVisibleScriptComponentVariable(true)]
		public string PathEntityName = "Path";

		// Token: 0x040016A2 RID: 5794
		[EditorVisibleScriptComponentVariable(true)]
		public int BridgeNavMeshID_1 = 8;

		// Token: 0x040016A3 RID: 5795
		[EditorVisibleScriptComponentVariable(true)]
		public int BridgeNavMeshID_2 = 8;

		// Token: 0x040016A4 RID: 5796
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID_1 = 9;

		// Token: 0x040016A5 RID: 5797
		[EditorVisibleScriptComponentVariable(true)]
		public int DitchNavMeshID_2 = 10;

		// Token: 0x040016A6 RID: 5798
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToBridgeNavMeshID_1 = 12;

		// Token: 0x040016A7 RID: 5799
		[EditorVisibleScriptComponentVariable(true)]
		public int GroundToBridgeNavMeshID_2 = 13;

		// Token: 0x040016A8 RID: 5800
		[EditorVisibleScriptComponentVariable(true)]
		public string AddOnDeployTag = "";

		// Token: 0x040016A9 RID: 5801
		[EditorVisibleScriptComponentVariable(true)]
		public string RemoveOnDeployTag = "";

		// Token: 0x040016AA RID: 5802
		[EditorVisibleScriptComponentVariable(true)]
		public float SpeedModifierFactor = 1f;

		// Token: 0x040016AB RID: 5803
		public bool EnableAutoGhostMovement;
	}
}
