using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x02000025 RID: 37
	public static class NavigationHelper
	{
		// Token: 0x06000163 RID: 355 RVA: 0x000103DF File Offset: 0x0000E5DF
		public static bool IsPositionValidForNavigationType(CampaignVec2 vec2, MobileParty.NavigationType navigationType)
		{
			return vec2.IsValid() && NavigationHelper.IsPositionValidForNavigationType(vec2.Face, navigationType);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000103FC File Offset: 0x0000E5FC
		public static bool IsPositionValidForNavigationType(PathFaceRecord face, MobileParty.NavigationType navigationType)
		{
			bool flag = false;
			if (face.IsValid())
			{
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(face);
				flag = Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(faceTerrainType, navigationType);
			}
			return flag;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0001043D File Offset: 0x0000E63D
		public static bool CanPlayerNavigateToPosition(CampaignVec2 vec2, out MobileParty.NavigationType navigationType)
		{
			return Campaign.Current.Models.PartyNavigationModel.CanPlayerNavigateToPosition(vec2, out navigationType);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00010455 File Offset: 0x0000E655
		public static CampaignVec2 GetClosestNavMeshFaceCenterPositionForPosition(CampaignVec2 vec2, int[] excludedFaceIds)
		{
			return Campaign.Current.MapSceneWrapper.GetNearestFaceCenterForPosition(in vec2, excludedFaceIds);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0001046C File Offset: 0x0000E66C
		public static NavigationHelper.EmbarkDisembarkData GetEmbarkDisembarkDataForTick(CampaignVec2 position, Vec2 direction)
		{
			CampaignVec2 campaignVec;
			CampaignVec2 campaignVec2;
			Vec2 vec;
			NavigationHelper.CalculateTransitionStartAndEndPosition(position, direction, out campaignVec, out campaignVec2, out vec);
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			if (campaignVec2.IsValid())
			{
				CampaignVec2 campaignVec3 = (campaignVec.IsValid() ? campaignVec : position);
				return new NavigationHelper.EmbarkDisembarkData(true, new CampaignVec2(vec, position.IsOnLand), campaignVec3, campaignVec2, false, false);
			}
			return NavigationHelper.EmbarkDisembarkData.Invalid;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000104C4 File Offset: 0x0000E6C4
		public static NavigationHelper.EmbarkDisembarkData GetEmbarkAndDisembarkDataForPlayer(CampaignVec2 position, Vec2 direction, CampaignVec2 moveTargetPointOfTheParty, bool isMoveTargetOnLand)
		{
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			NavigationHelper.EmbarkDisembarkData embarkDisembarkData = NavigationHelper.GetEmbarkDisembarkDataForTick(position, direction);
			if (embarkDisembarkData.IsValidTransition)
			{
				PathFaceRecord pathFaceRecord = position.Face;
				PathFaceRecord pathFaceRecord2 = embarkDisembarkData.TransitionStartPosition.Face;
				bool flag = pathFaceRecord2.IsValid() && Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(mapSceneWrapper.GetFaceTerrainType(pathFaceRecord2), MobileParty.NavigationType.Default);
				PathFaceRecord pathFaceRecord3 = embarkDisembarkData.TransitionEndPosition.Face;
				bool flag2 = pathFaceRecord3.IsValid() && Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(mapSceneWrapper.GetFaceTerrainType(pathFaceRecord3), MobileParty.NavigationType.Default);
				if (flag == flag2)
				{
					PathFaceRecord face = moveTargetPointOfTheParty.Face;
					Vec2 navigationMeshCenterPosition = mapSceneWrapper.GetNavigationMeshCenterPosition(face);
					direction = moveTargetPointOfTheParty.ToVec2() + navigationMeshCenterPosition;
					direction.Normalize();
					CampaignVec2 campaignVec = new CampaignVec2(navigationMeshCenterPosition, moveTargetPointOfTheParty.IsOnLand);
					embarkDisembarkData = NavigationHelper.GetEmbarkDisembarkDataForTick(campaignVec, direction);
					if (embarkDisembarkData.IsValidTransition)
					{
						pathFaceRecord = campaignVec.Face;
						pathFaceRecord2 = embarkDisembarkData.TransitionStartPosition.Face;
						flag = pathFaceRecord2.IsValid() && Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(mapSceneWrapper.GetFaceTerrainType(pathFaceRecord2), MobileParty.NavigationType.Default);
						pathFaceRecord3 = embarkDisembarkData.TransitionEndPosition.Face;
						flag2 = pathFaceRecord3.IsValid() && Campaign.Current.Models.PartyNavigationModel.IsTerrainTypeValidForNavigationType(mapSceneWrapper.GetFaceTerrainType(pathFaceRecord3), MobileParty.NavigationType.Default);
					}
				}
				if (embarkDisembarkData.IsValidTransition)
				{
					PathFaceRecord face2 = embarkDisembarkData.TransitionStartPosition.Face;
					Vec2 navigationMeshCenterPosition2 = mapSceneWrapper.GetNavigationMeshCenterPosition(face2);
					Vec2 lastPositionOnNavMeshFaceForPointAndDirection = mapSceneWrapper.GetLastPositionOnNavMeshFaceForPointAndDirection(pathFaceRecord, navigationMeshCenterPosition2, moveTargetPointOfTheParty.ToVec2() + navigationMeshCenterPosition2 * 10f);
					float num = moveTargetPointOfTheParty.Distance(lastPositionOnNavMeshFaceForPointAndDirection);
					float num2 = embarkDisembarkData.TransitionStartPosition.Distance(embarkDisembarkData.NavMeshEdgePosition);
					if (num < num2)
					{
						embarkDisembarkData.IsTargetingTheDeadZone = flag != flag2;
						PathFaceRecord face3 = moveTargetPointOfTheParty.Face;
						embarkDisembarkData.IsTargetingOwnSideOfTheDeadZone = embarkDisembarkData.IsTargetingTheDeadZone && face3.FaceIndex == face2.FaceIndex;
					}
				}
			}
			return embarkDisembarkData;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000106D0 File Offset: 0x0000E8D0
		private static void CalculateTransitionStartAndEndPosition(CampaignVec2 position, Vec2 direction, out CampaignVec2 transitionStartPosition, out CampaignVec2 transitionEndPosition, out Vec2 originalEdge)
		{
			Vec2 vec = direction;
			Vec2 vec2 = direction;
			vec.RotateCCW(0.05f);
			vec2.RotateCCW(-0.05f);
			int[] invalidTerrainTypesForNavigationType = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(position.IsOnLand ? MobileParty.NavigationType.Default : MobileParty.NavigationType.Naval);
			PathFaceRecord face = position.Face;
			originalEdge = Campaign.Current.MapSceneWrapper.GetLastPointOnNavigationMeshFromPositionToDestination(face, position.ToVec2(), position.ToVec2() + direction, invalidTerrainTypesForNavigationType);
			Vec2 lastPointOnNavigationMeshFromPositionToDestination = Campaign.Current.MapSceneWrapper.GetLastPointOnNavigationMeshFromPositionToDestination(face, position.ToVec2(), position.ToVec2() + vec, invalidTerrainTypesForNavigationType);
			Vec2 vec3 = Campaign.Current.MapSceneWrapper.GetLastPointOnNavigationMeshFromPositionToDestination(face, position.ToVec2(), position.ToVec2() + vec2, invalidTerrainTypesForNavigationType) - lastPointOnNavigationMeshFromPositionToDestination;
			Vec2 vec4 = vec3.LeftVec();
			Vec2 vec5 = vec3.RightVec();
			vec4.Normalize();
			vec4 *= Campaign.Current.Models.PartyNavigationModel.GetEmbarkDisembarkThresholdDistance();
			vec5.Normalize();
			vec5 *= Campaign.Current.Models.PartyNavigationModel.GetEmbarkDisembarkThresholdDistance();
			transitionStartPosition = new CampaignVec2(vec5 + originalEdge, position.IsOnLand);
			transitionEndPosition = new CampaignVec2(vec4 + originalEdge, position.IsOnLand);
			if (transitionStartPosition.Face.IsValid() && transitionEndPosition.Face.IsValid())
			{
				transitionStartPosition = CampaignVec2.Invalid;
				transitionEndPosition = CampaignVec2.Invalid;
				return;
			}
			transitionStartPosition = new CampaignVec2(vec5 + originalEdge, position.IsOnLand);
			transitionEndPosition = new CampaignVec2(vec4 + originalEdge, !position.IsOnLand);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000108C0 File Offset: 0x0000EAC0
		public static CampaignVec2 FindPointAroundPosition(CampaignVec2 centerPosition, MobileParty.NavigationType navigationCapability, float maxDistance, float minDistance = 0f, bool requirePath = true, bool useUniformDistribution = false)
		{
			PathFaceRecord face = centerPosition.Face;
			int[] invalidTerrainTypesForNavigationType = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(navigationCapability);
			CampaignVec2 campaignVec = centerPosition;
			if (maxDistance > 0f)
			{
				Vec2 vec;
				Vec2 vec2;
				float num;
				Campaign.Current.MapSceneWrapper.GetMapBorders(out vec, out vec2, out num);
				Vec2 vec3 = new Vec2(MathF.Max(centerPosition.X - maxDistance, vec.x), MathF.Max(centerPosition.Y - maxDistance, vec.y));
				Vec2 vec4 = new Vec2(MathF.Min(centerPosition.X + maxDistance, vec2.x), MathF.Min(centerPosition.Y + maxDistance, vec2.y));
				maxDistance = MathF.Min(vec4.x - vec3.x, vec4.y - vec3.y) * 0.5f;
				for (int i = 0; i < 250; i++)
				{
					CampaignVec2 campaignVec2 = NavigationHelper.FindPointInCircle(centerPosition, minDistance, maxDistance, useUniformDistribution);
					if (campaignVec2 != centerPosition)
					{
						PathFaceRecord face2 = campaignVec2.Face;
						if (face2.IsValid())
						{
							int regionSwitchCostFromLandToSea = Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromLandToSea;
							int regionSwitchCostFromSeaToLand = Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromSeaToLand;
							if ((!requirePath || Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(face, face2, centerPosition.ToVec2(), campaignVec2.ToVec2(), 0.3f, maxDistance, out num, invalidTerrainTypesForNavigationType, regionSwitchCostFromLandToSea, regionSwitchCostFromSeaToLand)) && NavigationHelper.IsPositionValidForNavigationType(campaignVec2, navigationCapability))
							{
								campaignVec = campaignVec2;
								break;
							}
						}
					}
				}
			}
			return campaignVec;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00010A50 File Offset: 0x0000EC50
		public static CampaignVec2 FindReachablePointAroundPosition(CampaignVec2 center, int[] excludedFaceIds, float maxDistance, float minDistance = 0f, bool useUniformDistribution = false)
		{
			CampaignVec2 campaignVec = center;
			if (maxDistance > 0f)
			{
				Vec2 vec;
				Vec2 vec2;
				float num;
				Campaign.Current.MapSceneWrapper.GetMapBorders(out vec, out vec2, out num);
				Vec2 vec3 = new Vec2(MathF.Max(center.X - maxDistance, vec.x), MathF.Max(center.Y - maxDistance, vec.y));
				Vec2 vec4 = new Vec2(MathF.Min(center.X + maxDistance, vec2.x), MathF.Min(center.Y + maxDistance, vec2.y));
				maxDistance = MathF.Min(vec4.x - vec3.x, vec4.y - vec3.y) * 0.5f;
				for (int i = 0; i < 250; i++)
				{
					CampaignVec2 campaignVec2 = NavigationHelper.FindPointInCircle(center, minDistance, maxDistance, useUniformDistribution);
					if (campaignVec2 != center && campaignVec2.Face.IsValid() && Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(center.Face, campaignVec2.Face, center.ToVec2(), campaignVec2.ToVec2(), 0.3f, maxDistance, out num, excludedFaceIds, Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromLandToSea, Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromSeaToLand))
					{
						campaignVec = campaignVec2;
						break;
					}
				}
			}
			return campaignVec;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00010BAC File Offset: 0x0000EDAC
		public static CampaignVec2 FindReachablePointAroundPosition(CampaignVec2 center, MobileParty.NavigationType navigationCapability, float maxDistance, float minDistance = 0f, bool useUniformDistribution = false)
		{
			int[] invalidTerrainTypesForNavigationType = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(navigationCapability);
			return NavigationHelper.FindReachablePointAroundPosition(center, invalidTerrainTypesForNavigationType, maxDistance, minDistance, useUniformDistribution);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00010BDC File Offset: 0x0000EDDC
		public static CampaignVec2 FindPointInsideArea(Vec2 minBorder, Vec2 maxBorder, MobileParty.NavigationType navigationCapability)
		{
			Vec2 vec;
			Vec2 vec2;
			float num;
			Campaign.Current.MapSceneWrapper.GetMapBorders(out vec, out vec2, out num);
			CampaignVec2 campaignVec = CampaignVec2.Invalid;
			bool flag = false;
			for (int i = 0; i < 250; i++)
			{
				CampaignVec2 campaignVec2 = new CampaignVec2(new Vec2(MBRandom.RandomFloatRanged(minBorder.x, maxBorder.x), MBRandom.RandomFloatRanged(minBorder.y, maxBorder.y)), true);
				if (NavigationHelper.IsPositionValidForNavigationType(campaignVec2, navigationCapability))
				{
					flag = true;
				}
				if (flag)
				{
					campaignVec = campaignVec2;
					break;
				}
			}
			return campaignVec;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00010C5E File Offset: 0x0000EE5E
		public static bool IsPointInsideBorders(Vec2 point, Vec2 minBorders, Vec2 maxBorders)
		{
			return point.x < maxBorders.x && point.y < maxBorders.y && point.x > minBorders.x && point.y > minBorders.y;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00010C9C File Offset: 0x0000EE9C
		public static CampaignVec2 FindPointInsideArea(Vec2 minBorders, Vec2 maxBorders, CampaignVec2 center, MobileParty.NavigationType navigationCapability, float maxDistance, float minDistance = 0f, bool requirePathFromCenter = false)
		{
			Vec2 vec;
			Vec2 vec2;
			float num;
			Campaign.Current.MapSceneWrapper.GetMapBorders(out vec, out vec2, out num);
			CampaignVec2 campaignVec = CampaignVec2.Invalid;
			float num2 = MathF.Max(minBorders.x, maxBorders.x);
			float num3 = MathF.Min(minBorders.x, maxBorders.x);
			float num4 = MathF.Max(minBorders.y, maxBorders.y);
			float num5 = MathF.Min(minBorders.y, maxBorders.y);
			Vec2 vec3 = new Vec2(num3, num5);
			Vec2 vec4 = new Vec2(num2, num5);
			Vec2 vec5 = new Vec2(num2, num4);
			Vec2 vec6 = new Vec2(num3, num4);
			float num6 = MathF.Max(MathF.Max(center.Distance(vec3), center.Distance(vec4), center.Distance(vec6)), center.Distance(vec5));
			maxDistance = MathF.Min(maxDistance, num6);
			bool flag = false;
			CampaignVec2 campaignVec2 = CampaignVec2.Invalid;
			for (int i = 0; i < 250; i++)
			{
				campaignVec2 = NavigationHelper.FindPointInCircle(center, minDistance, maxDistance, false);
				if (NavigationHelper.IsPositionValidForNavigationType(campaignVec2, navigationCapability) && NavigationHelper.IsPointInsideBorders(campaignVec2.ToVec2(), minBorders, maxBorders))
				{
					flag = true;
				}
				if (flag && requirePathFromCenter)
				{
					int[] invalidTerrainTypesForNavigationType = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(navigationCapability);
					if (!Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(center.Face, campaignVec2.Face, center.ToVec2(), campaignVec2.ToVec2(), 0.3f, maxDistance, out num, invalidTerrainTypesForNavigationType, Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromLandToSea, Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromSeaToLand))
					{
						flag = false;
					}
				}
				if (flag)
				{
					campaignVec = campaignVec2;
					break;
				}
			}
			if (campaignVec.ToVec2() == Vec2.Invalid)
			{
				Debug.FailedAssert("Point should not be invalid!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "FindPointInsideArea", 9911);
				return NavigationHelper.FindPointInsideArea(minBorders, maxBorders, navigationCapability);
			}
			return campaignVec;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00010E84 File Offset: 0x0000F084
		private static CampaignVec2 FindPointInCircle(CampaignVec2 center, float min, float max, bool useUniformDistribution)
		{
			float num = MBRandom.RandomFloatRanged(0f, 6.2831855f);
			Vec2 vec = Vec2.One.Normalized();
			vec.RotateCCW(num);
			if (useUniformDistribution)
			{
				vec *= MathF.Sqrt(MBRandom.RandomFloat) * (max - min);
			}
			else
			{
				vec *= MBRandom.RandomFloatRanged(min, max);
			}
			return center + vec;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00010EE8 File Offset: 0x0000F0E8
		public static void GetInteractionDataForMainParty(Settlement settlement, out bool canNavigate, out MobileParty.NavigationType bestNavigationType, out bool isTargetingPort)
		{
			CampaignVec2 campaignVec;
			if (MobileParty.MainParty.IsCurrentlyAtSea && settlement.HasPort)
			{
				isTargetingPort = true;
				campaignVec = settlement.PortPosition;
			}
			else
			{
				isTargetingPort = false;
				campaignVec = settlement.GatePosition;
			}
			canNavigate = NavigationHelper.CanPlayerNavigateToPosition(campaignVec, out bestNavigationType);
		}

		// Token: 0x020004F1 RID: 1265
		public class EmbarkDisembarkData
		{
			// Token: 0x06004B9F RID: 19359 RVA: 0x0017D36E File Offset: 0x0017B56E
			public EmbarkDisembarkData(bool isValid, CampaignVec2 navMeshEdgePosition, CampaignVec2 transitionStartPosition, CampaignVec2 transitionEndPosition, bool isTargetingTheDeadZone, bool isTargetingOwnSideOfTheDeadZone)
			{
				this.IsValidTransition = isValid;
				this.NavMeshEdgePosition = navMeshEdgePosition;
				this.TransitionStartPosition = transitionStartPosition;
				this.TransitionEndPosition = transitionEndPosition;
				this.IsTargetingTheDeadZone = isTargetingTheDeadZone;
				this.IsTargetingOwnSideOfTheDeadZone = isTargetingOwnSideOfTheDeadZone;
			}

			// Token: 0x04001540 RID: 5440
			public static readonly NavigationHelper.EmbarkDisembarkData Invalid = new NavigationHelper.EmbarkDisembarkData(false, CampaignVec2.Invalid, CampaignVec2.Invalid, CampaignVec2.Invalid, false, false);

			// Token: 0x04001541 RID: 5441
			public bool IsValidTransition;

			// Token: 0x04001542 RID: 5442
			public CampaignVec2 NavMeshEdgePosition;

			// Token: 0x04001543 RID: 5443
			public CampaignVec2 TransitionStartPosition;

			// Token: 0x04001544 RID: 5444
			public CampaignVec2 TransitionEndPosition;

			// Token: 0x04001545 RID: 5445
			public bool IsTargetingTheDeadZone;

			// Token: 0x04001546 RID: 5446
			public bool IsTargetingOwnSideOfTheDeadZone;
		}
	}
}
