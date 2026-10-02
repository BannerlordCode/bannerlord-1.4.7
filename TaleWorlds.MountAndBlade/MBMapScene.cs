using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001CD RID: 461
	public static class MBMapScene
	{
		// Token: 0x06001B9F RID: 7071 RVA: 0x000602DF File Offset: 0x0005E4DF
		public static Vec2 GetNearestFaceCenterForPosition(Scene mapScene, Vec2 position, bool isRegionMap0, int[] excludedFaceIds)
		{
			return MBAPI.IMBMapScene.GetNearestFaceCenterPositionForPosition(mapScene.Pointer, position.ToVec3(0f), isRegionMap0, excludedFaceIds, excludedFaceIds.Length);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00060302 File Offset: 0x0005E502
		public static Vec2 GetNearestFaceCenterForPositionWithPath(Scene mapScene, PathFaceRecord pathFaceRecord, bool targetRegionMap0, float maxDist, int[] excludedFaceIds)
		{
			return MBAPI.IMBMapScene.GetNearestFaceCenterForPositionWithPath(mapScene.Pointer, pathFaceRecord.FaceIndex, targetRegionMap0, maxDist, excludedFaceIds, excludedFaceIds.Length);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00060324 File Offset: 0x0005E524
		public static Vec2 GetAccessiblePointNearPosition(Scene mapScene, Vec2 position, bool isRegionMap1, float radius)
		{
			return MBAPI.IMBMapScene.GetAccessiblePointNearPosition(mapScene.Pointer, position, isRegionMap1, radius).AsVec2;
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0006034C File Offset: 0x0005E54C
		public static void RemoveZeroCornerBodies(Scene mapScene)
		{
			MBAPI.IMBMapScene.RemoveZeroCornerBodies(mapScene.Pointer);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0006035E File Offset: 0x0005E55E
		public static void LoadAtmosphereData(Scene mapScene)
		{
			MBAPI.IMBMapScene.LoadAtmosphereData(mapScene.Pointer);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x00060370 File Offset: 0x0005E570
		public static void TickStepSound(Scene mapScene, MBAgentVisuals visuals, int terrainType, TerrainTypeSoundSlot soundType, int partySize)
		{
			MBAPI.IMBMapScene.TickStepSound(mapScene.Pointer, visuals.Pointer, terrainType, soundType, partySize);
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0006038C File Offset: 0x0005E58C
		public static void TickAmbientSounds(Scene mapScene, int terrainType)
		{
			MBAPI.IMBMapScene.TickAmbientSounds(mapScene.Pointer, terrainType);
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0006039F File Offset: 0x0005E59F
		public static bool GetMouseVisible()
		{
			return MBAPI.IMBMapScene.GetMouseVisible();
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000603AB File Offset: 0x0005E5AB
		public static bool GetApplyRainColorGrade()
		{
			return MBMapScene.ApplyRainColorGrade;
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x000603B2 File Offset: 0x0005E5B2
		public static void SendMouseKeyEvent(int mouseKeyId, bool isDown)
		{
			MBAPI.IMBMapScene.SendMouseKeyEvent(mouseKeyId, isDown);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000603C0 File Offset: 0x0005E5C0
		public static void SetMousePos(int posX, int posY)
		{
			MBAPI.IMBMapScene.SetMousePos(posX, posY);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000603D0 File Offset: 0x0005E5D0
		public static void TickVisuals(Scene mapScene, float tod, Mesh[] tickedMapMeshes)
		{
			for (int i = 0; i < tickedMapMeshes.Length; i++)
			{
				MBMapScene._tickedMapMeshesCachedArray[i] = tickedMapMeshes[i].Pointer;
			}
			MBAPI.IMBMapScene.TickVisuals(mapScene.Pointer, tod, MBMapScene._tickedMapMeshesCachedArray, tickedMapMeshes.Length);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x00060413 File Offset: 0x0005E613
		public static void ValidateTerrainSoundIds()
		{
			MBAPI.IMBMapScene.ValidateTerrainSoundIds();
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0006041F File Offset: 0x0005E61F
		public static void GetGlobalIlluminationOfString(Scene mapScene, string value)
		{
			MBAPI.IMBMapScene.SetPoliticalColor(mapScene.Pointer, value);
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00060432 File Offset: 0x0005E632
		public static void GetColorGradeGridData(Scene mapScene, byte[] gridData, string textureName)
		{
			MBAPI.IMBMapScene.GetColorGradeGridData(mapScene.Pointer, gridData, textureName);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x00060448 File Offset: 0x0005E648
		public static void GetBattleSceneIndexMap(Scene mapScene, ref byte[] indexData, ref int width, ref int height)
		{
			MBAPI.IMBMapScene.GetBattleSceneIndexMapResolution(mapScene.Pointer, ref width, ref height);
			int num = width * height * 2;
			if (indexData == null || indexData.Length != num)
			{
				indexData = new byte[num];
			}
			MBAPI.IMBMapScene.GetBattleSceneIndexMap(mapScene.Pointer, indexData);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x00060494 File Offset: 0x0005E694
		public static void SetFrameForAtmosphere(Scene mapScene, float tod, float cameraElevation, bool forceLoadTextures)
		{
			MBAPI.IMBMapScene.SetFrameForAtmosphere(mapScene.Pointer, tod, cameraElevation, forceLoadTextures);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x000604A9 File Offset: 0x0005E6A9
		public static void SetTerrainDynamicParams(Scene mapScene, Vec3 dynamic_params)
		{
			MBAPI.IMBMapScene.SetTerrainDynamicParams(mapScene.Pointer, dynamic_params);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000604BC File Offset: 0x0005E6BC
		public static void SetSeasonTimeFactor(Scene mapScene, float seasonTimeFactor)
		{
			MBAPI.IMBMapScene.SetSeasonTimeFactor(mapScene.Pointer, seasonTimeFactor);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x000604CF File Offset: 0x0005E6CF
		public static float GetSeasonTimeFactor(Scene mapScene)
		{
			return MBAPI.IMBMapScene.GetSeasonTimeFactor(mapScene.Pointer);
		}

		// Token: 0x0400090E RID: 2318
		public static bool ApplyRainColorGrade;

		// Token: 0x0400090F RID: 2319
		private static UIntPtr[] _tickedMapMeshesCachedArray = new UIntPtr[1024];
	}
}
