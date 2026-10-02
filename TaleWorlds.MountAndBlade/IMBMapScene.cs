using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BA RID: 442
	[ScriptingInterfaceBase]
	internal interface IMBMapScene
	{
		// Token: 0x060018EB RID: 6379
		[EngineMethod("get_accessible_point_near_position", false, null, false)]
		Vec3 GetAccessiblePointNearPosition(UIntPtr scenePointer, Vec2 position, bool isRegionMap0, float radius);

		// Token: 0x060018EC RID: 6380
		[EngineMethod("get_nearest_nav_mesh_face_center_position_for_position", false, null, false)]
		Vec2 GetNearestFaceCenterPositionForPosition(UIntPtr scenePointer, Vec3 position, bool isRegionMap0, int[] excludedFaceIds, int excludedFaceIdCount);

		// Token: 0x060018ED RID: 6381
		[EngineMethod("get_nearest_nav_mesh_face_center_position_between_regions_using_path", false, null, false)]
		Vec2 GetNearestFaceCenterForPositionWithPath(UIntPtr scenePointer, int startFaceIndex, bool targetRegionMap0, float distMax, int[] excludedFaceIds, int excludedFaceIdCount);

		// Token: 0x060018EE RID: 6382
		[EngineMethod("remove_zero_corner_bodies", false, null, false)]
		void RemoveZeroCornerBodies(UIntPtr scenePointer);

		// Token: 0x060018EF RID: 6383
		[EngineMethod("load_atmosphere_data", false, null, false)]
		void LoadAtmosphereData(UIntPtr scenePointer);

		// Token: 0x060018F0 RID: 6384
		[EngineMethod("tick_step_sound", false, null, false)]
		void TickStepSound(UIntPtr scenePointer, UIntPtr visualsPointer, int faceIndexTerrainType, TerrainTypeSoundSlot soundType, int partySize);

		// Token: 0x060018F1 RID: 6385
		[EngineMethod("tick_ambient_sounds", false, null, false)]
		void TickAmbientSounds(UIntPtr scenePointer, int terrainType);

		// Token: 0x060018F2 RID: 6386
		[EngineMethod("tick_visuals", false, null, false)]
		void TickVisuals(UIntPtr scenePointer, float tod, UIntPtr[] ticked_map_meshes, int tickedMapMeshesCount);

		// Token: 0x060018F3 RID: 6387
		[EngineMethod("validate_terrain_sound_ids", false, null, false)]
		void ValidateTerrainSoundIds();

		// Token: 0x060018F4 RID: 6388
		[EngineMethod("set_political_color", false, null, false)]
		void SetPoliticalColor(UIntPtr scenePointer, string value);

		// Token: 0x060018F5 RID: 6389
		[EngineMethod("set_frame_for_atmosphere", false, null, false)]
		void SetFrameForAtmosphere(UIntPtr scenePointer, float tod, float cameraElevation, bool forceLoadTextures);

		// Token: 0x060018F6 RID: 6390
		[EngineMethod("get_color_grade_grid_data", false, null, false)]
		void GetColorGradeGridData(UIntPtr scenePointer, byte[] snowData, string textureName);

		// Token: 0x060018F7 RID: 6391
		[EngineMethod("get_battle_scene_index_map_resolution", false, null, false)]
		void GetBattleSceneIndexMapResolution(UIntPtr scenePointer, ref int width, ref int height);

		// Token: 0x060018F8 RID: 6392
		[EngineMethod("get_battle_scene_index_map", false, null, false)]
		void GetBattleSceneIndexMap(UIntPtr scenePointer, byte[] indexData);

		// Token: 0x060018F9 RID: 6393
		[EngineMethod("set_terrain_dynamic_params", false, null, false)]
		void SetTerrainDynamicParams(UIntPtr scenePointer, Vec3 dynamic_params);

		// Token: 0x060018FA RID: 6394
		[EngineMethod("set_season_time_factor", false, null, false)]
		void SetSeasonTimeFactor(UIntPtr scenePointer, float seasonTimeFactor);

		// Token: 0x060018FB RID: 6395
		[EngineMethod("get_season_time_factor", false, null, false)]
		float GetSeasonTimeFactor(UIntPtr scenePointer);

		// Token: 0x060018FC RID: 6396
		[EngineMethod("get_mouse_visible", false, null, false)]
		bool GetMouseVisible();

		// Token: 0x060018FD RID: 6397
		[EngineMethod("send_mouse_key_down_event", false, null, false)]
		void SendMouseKeyEvent(int keyId, bool isDown);

		// Token: 0x060018FE RID: 6398
		[EngineMethod("set_mouse_visible", false, null, false)]
		void SetMouseVisible(bool value);

		// Token: 0x060018FF RID: 6399
		[EngineMethod("set_mouse_pos", false, null, false)]
		void SetMousePos(int posX, int posY);
	}
}
