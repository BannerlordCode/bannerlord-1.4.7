using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AA RID: 426
	[ScriptingInterfaceBase]
	internal interface IMBEditor
	{
		// Token: 0x060017CE RID: 6094
		[EngineMethod("is_edit_mode", false, null, false)]
		bool IsEditMode();

		// Token: 0x060017CF RID: 6095
		[EngineMethod("is_edit_mode_enabled", false, null, false)]
		bool IsEditModeEnabled();

		// Token: 0x060017D0 RID: 6096
		[EngineMethod("update_scene_tree", false, null, false)]
		void UpdateSceneTree(bool do_next_frame);

		// Token: 0x060017D1 RID: 6097
		[EngineMethod("is_entity_selected", false, null, false)]
		bool IsEntitySelected(UIntPtr entityId);

		// Token: 0x060017D2 RID: 6098
		[EngineMethod("add_editor_warning", false, null, false)]
		void AddEditorWarning(string msg);

		// Token: 0x060017D3 RID: 6099
		[EngineMethod("render_editor_mesh", false, null, false)]
		void RenderEditorMesh(UIntPtr metaMeshId, ref MatrixFrame frame);

		// Token: 0x060017D4 RID: 6100
		[EngineMethod("apply_delta_to_editor_camera", false, null, false)]
		void ApplyDeltaToEditorCamera(in Vec3 delta);

		// Token: 0x060017D5 RID: 6101
		[EngineMethod("enter_edit_mode", false, null, false)]
		void EnterEditMode(UIntPtr sceneWidgetPointer, ref MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing);

		// Token: 0x060017D6 RID: 6102
		[EngineMethod("tick_edit_mode", false, null, false)]
		void TickEditMode(float dt);

		// Token: 0x060017D7 RID: 6103
		[EngineMethod("leave_edit_mode", false, null, false)]
		void LeaveEditMode();

		// Token: 0x060017D8 RID: 6104
		[EngineMethod("enter_edit_mission_mode", false, null, false)]
		void EnterEditMissionMode(UIntPtr missionPointer);

		// Token: 0x060017D9 RID: 6105
		[EngineMethod("leave_edit_mission_mode", false, null, false)]
		void LeaveEditMissionMode();

		// Token: 0x060017DA RID: 6106
		[EngineMethod("activate_scene_editor_presentation", false, null, false)]
		void ActivateSceneEditorPresentation();

		// Token: 0x060017DB RID: 6107
		[EngineMethod("deactivate_scene_editor_presentation", false, null, false)]
		void DeactivateSceneEditorPresentation();

		// Token: 0x060017DC RID: 6108
		[EngineMethod("tick_scene_editor_presentation", false, null, false)]
		void TickSceneEditorPresentation(float dt);

		// Token: 0x060017DD RID: 6109
		[EngineMethod("get_editor_scene_view", false, null, false)]
		SceneView GetEditorSceneView();

		// Token: 0x060017DE RID: 6110
		[EngineMethod("helpers_enabled", false, null, false)]
		bool HelpersEnabled();

		// Token: 0x060017DF RID: 6111
		[EngineMethod("border_helpers_enabled", false, null, false)]
		bool BorderHelpersEnabled();

		// Token: 0x060017E0 RID: 6112
		[EngineMethod("zoom_to_position", false, null, false)]
		void ZoomToPosition(Vec3 pos);

		// Token: 0x060017E1 RID: 6113
		[EngineMethod("add_entity_warning", false, null, false)]
		void AddEntityWarning(UIntPtr entityId, string msg);

		// Token: 0x060017E2 RID: 6114
		[EngineMethod("add_nav_mesh_warning", false, null, false)]
		void AddNavMeshWarning(UIntPtr sceneId, in PathFaceRecord record, string msg);

		// Token: 0x060017E3 RID: 6115
		[EngineMethod("get_all_prefabs_and_child_with_tag", false, null, false)]
		string GetAllPrefabsAndChildWithTag(string tag);

		// Token: 0x060017E4 RID: 6116
		[EngineMethod("set_upgrade_level_visibility", false, null, false)]
		void SetUpgradeLevelVisibility(string cumulated_string);

		// Token: 0x060017E5 RID: 6117
		[EngineMethod("set_level_visibility", false, null, false)]
		void SetLevelVisibility(string cumulated_string);

		// Token: 0x060017E6 RID: 6118
		[EngineMethod("toggle_enable_editor_physics", false, null, false)]
		void ToggleEnableEditorPhysics();

		// Token: 0x060017E7 RID: 6119
		[EngineMethod("exit_edit_mode", false, null, false)]
		void ExitEditMode();

		// Token: 0x060017E8 RID: 6120
		[EngineMethod("is_replay_manager_recording", false, null, false)]
		bool IsReplayManagerRecording();

		// Token: 0x060017E9 RID: 6121
		[EngineMethod("is_replay_manager_rendering", false, null, false)]
		bool IsReplayManagerRendering();

		// Token: 0x060017EA RID: 6122
		[EngineMethod("is_replay_manager_replaying", false, null, false)]
		bool IsReplayManagerReplaying();
	}
}
