using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x0200021E RID: 542
	public interface IMapScene
	{
		// Token: 0x060020B0 RID: 8368
		void Load();

		// Token: 0x060020B1 RID: 8369
		void AfterLoad();

		// Token: 0x060020B2 RID: 8370
		void Destroy();

		// Token: 0x060020B3 RID: 8371
		PathFaceRecord GetFaceIndex(in CampaignVec2 vec2);

		// Token: 0x060020B4 RID: 8372
		TerrainType GetTerrainTypeAtPosition(in CampaignVec2 vec2);

		// Token: 0x060020B5 RID: 8373
		List<TerrainType> GetEnvironmentTerrainTypes(in CampaignVec2 vec2);

		// Token: 0x060020B6 RID: 8374
		List<TerrainType> GetEnvironmentTerrainTypesCount(in CampaignVec2 vec2, out TerrainType currentPositionTerrainType);

		// Token: 0x060020B7 RID: 8375
		MapPatchData GetMapPatchAtPosition(in CampaignVec2 position);

		// Token: 0x060020B8 RID: 8376
		TerrainType GetFaceTerrainType(PathFaceRecord faceIndex);

		// Token: 0x060020B9 RID: 8377
		CampaignVec2 GetNearestFaceCenterForPosition(in CampaignVec2 vec2, int[] excludedFaceIds);

		// Token: 0x060020BA RID: 8378
		CampaignVec2 GetNearestFaceCenterForPositionWithPath(PathFaceRecord pathFaceRecord, bool targetIsLand, float maxDist, int[] excludedFaceIds);

		// Token: 0x060020BB RID: 8379
		CampaignVec2 GetAccessiblePointNearPosition(in CampaignVec2 vec2, float radius);

		// Token: 0x060020BC RID: 8380
		bool GetPathBetweenAIFaces(PathFaceRecord startingFace, PathFaceRecord endingFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, NavigationPath path, int[] excludedFaceIds, float extraCostMultiplier, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand);

		// Token: 0x060020BD RID: 8381
		bool GetPathDistanceBetweenAIFaces(PathFaceRecord startingAiFace, PathFaceRecord endingAiFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, float distanceLimit, out float distance, int[] excludedFaceIds, int regionSwitchCostFromLandToSea, int regionSwitchCostFromSeaToLand);

		// Token: 0x060020BE RID: 8382
		bool IsLineToPointClear(PathFaceRecord startingFace, Vec2 position, Vec2 destination, float agentRadius);

		// Token: 0x060020BF RID: 8383
		Vec2 GetLastPointOnNavigationMeshFromPositionToDestination(PathFaceRecord startingFace, Vec2 position, Vec2 destination, int[] excludedFaceIds = null);

		// Token: 0x060020C0 RID: 8384
		Vec2 GetLastPositionOnNavMeshFaceForPointAndDirection(PathFaceRecord startingFace, Vec2 position, Vec2 destination);

		// Token: 0x060020C1 RID: 8385
		Vec2 GetNavigationMeshCenterPosition(PathFaceRecord face);

		// Token: 0x060020C2 RID: 8386
		Vec2 GetNavigationMeshCenterPosition(int faceIndex);

		// Token: 0x060020C3 RID: 8387
		PathFaceRecord GetFaceAtIndex(int faceIndex);

		// Token: 0x060020C4 RID: 8388
		int GetNumberOfNavigationMeshFaces();

		// Token: 0x060020C5 RID: 8389
		bool GetHeightAtPoint(in CampaignVec2 point, ref float height);

		// Token: 0x060020C6 RID: 8390
		float GetWinterTimeFactor();

		// Token: 0x060020C7 RID: 8391
		void GetTerrainHeightAndNormal(Vec2 position, out float height, out Vec3 normal);

		// Token: 0x060020C8 RID: 8392
		float GetFaceVertexZ(PathFaceRecord navMeshFace);

		// Token: 0x060020C9 RID: 8393
		Vec3 GetGroundNormal(Vec2 position);

		// Token: 0x060020CA RID: 8394
		void GetSiegeCampFrames(Settlement settlement, out List<MatrixFrame> siegeCamp1GlobalFrames, out List<MatrixFrame> siegeCamp2GlobalFrames);

		// Token: 0x060020CB RID: 8395
		string GetTerrainTypeName(TerrainType type);

		// Token: 0x060020CC RID: 8396
		Vec2 GetTerrainSize();

		// Token: 0x060020CD RID: 8397
		uint GetSceneLevel(string name);

		// Token: 0x060020CE RID: 8398
		void SetSceneLevels(List<string> levels);

		// Token: 0x060020CF RID: 8399
		List<AtmosphereState> GetAtmosphereStates();

		// Token: 0x060020D0 RID: 8400
		void SetAtmosphereColorgrade(TerrainType terrainType);

		// Token: 0x060020D1 RID: 8401
		void AddNewEntityToMapScene(string entityId, in CampaignVec2 position);

		// Token: 0x060020D2 RID: 8402
		void GetMapBorders(out Vec2 minimumPosition, out Vec2 maximumPosition, out float maximumHeight);

		// Token: 0x060020D3 RID: 8403
		uint GetSceneXmlCrc();

		// Token: 0x060020D4 RID: 8404
		uint GetSceneNavigationMeshCrc();

		// Token: 0x060020D5 RID: 8405
		float GetSnowAmountAtPosition(Vec2 position);

		// Token: 0x060020D6 RID: 8406
		float GetRainAmountAtPosition(Vec2 position);
	}
}
