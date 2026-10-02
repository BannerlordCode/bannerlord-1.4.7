using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x02000228 RID: 552
	public class SandBoxNavigationCache : NavigationCache<Settlement>, MapDistanceModel.INavigationCache
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x000933BF File Offset: 0x000915BF
		private IMapScene MapSceneWrapper
		{
			get
			{
				return Campaign.Current.MapSceneWrapper;
			}
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000933CC File Offset: 0x000915CC
		public SandBoxNavigationCache(MobileParty.NavigationType navigationType)
			: base(navigationType)
		{
			this._excludedFaceIds = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(base._navigationType);
			this._regionSwitchCostTo0 = Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromLandToSea;
			this._regionSwitchCostTo1 = Campaign.Current.Models.MapDistanceModel.RegionSwitchCostFromSeaToLand;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x00093434 File Offset: 0x00091634
		protected override Settlement GetCacheElement(string settlementId)
		{
			return Settlement.Find(settlementId);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x0009343C File Offset: 0x0009163C
		protected override NavigationCacheElement<Settlement> GetCacheElement(Settlement settlement, bool isPortUsed)
		{
			return new NavigationCacheElement<Settlement>(settlement, isPortUsed);
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x00093448 File Offset: 0x00091648
		float MapDistanceModel.INavigationCache.GetSettlementToSettlementDistanceWithLandRatio(Settlement settlement1, bool isAtSea1, Settlement settlement2, bool isAtSea2, out float landRatio)
		{
			NavigationCacheElement<Settlement> cacheElement = this.GetCacheElement(settlement1, isAtSea1);
			NavigationCacheElement<Settlement> cacheElement2 = this.GetCacheElement(settlement2, isAtSea2);
			return base.GetSettlementToSettlementDistanceWithLandRatio(cacheElement, cacheElement2, out landRatio);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00093472 File Offset: 0x00091672
		public override void GetSceneXmlCrcValues(out uint sceneXmlCrc, out uint sceneNavigationMeshCrc)
		{
			sceneXmlCrc = this.MapSceneWrapper.GetSceneXmlCrc();
			sceneNavigationMeshCrc = this.MapSceneWrapper.GetSceneNavigationMeshCrc();
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x0009348E File Offset: 0x0009168E
		protected override int GetNavMeshFaceCount()
		{
			return this.MapSceneWrapper.GetNumberOfNavigationMeshFaces();
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x0009349B File Offset: 0x0009169B
		protected override Vec2 GetNavMeshFaceCenterPosition(int faceIndex)
		{
			return this.MapSceneWrapper.GetNavigationMeshCenterPosition(faceIndex);
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000934A9 File Offset: 0x000916A9
		protected override PathFaceRecord GetFaceRecordAtIndex(int faceIndex)
		{
			return this.MapSceneWrapper.GetFaceAtIndex(faceIndex);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000934B7 File Offset: 0x000916B7
		protected override int GetRegionSwitchCostTo0()
		{
			return this._regionSwitchCostTo0;
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000934BF File Offset: 0x000916BF
		protected override int GetRegionSwitchCostTo1()
		{
			return this._regionSwitchCostTo1;
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000934C7 File Offset: 0x000916C7
		protected override int[] GetExcludedFaceIds()
		{
			return this._excludedFaceIds;
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000934D0 File Offset: 0x000916D0
		protected override float GetRealDistanceAndLandRatioBetweenSettlements(NavigationCacheElement<Settlement> settlement1, NavigationCacheElement<Settlement> settlement2, out float landRatio)
		{
			landRatio = 1f;
			float num = (float)Campaign.PathFindingMaxCostLimit;
			CampaignVec2 campaignVec = (settlement1.IsPortUsed ? settlement1.PortPosition : settlement1.GatePosition);
			CampaignVec2 campaignVec2 = (settlement2.IsPortUsed ? settlement2.PortPosition : settlement2.GatePosition);
			NavigationPath navigationPath = new NavigationPath();
			Campaign.Current.MapSceneWrapper.GetPathBetweenAIFaces(campaignVec.Face, campaignVec2.Face, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, navigationPath, this.GetExcludedFaceIds(), 1f, this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			float num2;
			Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(campaignVec.Face, campaignVec2.Face, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, num, out num2, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			float num3;
			Campaign.Current.MapSceneWrapper.GetPathDistanceBetweenAIFaces(campaignVec2.Face, campaignVec.Face, campaignVec2.ToVec2(), campaignVec.ToVec2(), 0.3f, num, out num3, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			float num4 = (num2 + num3) * 0.5f;
			if (num4 > 0f)
			{
				if (base._navigationType == MobileParty.NavigationType.Naval)
				{
					landRatio = 0f;
				}
				else if (base._navigationType == MobileParty.NavigationType.All)
				{
					landRatio = base.GetLandRatioOfPath(navigationPath, campaignVec.ToVec2());
				}
				bool flag;
				NavigationCacheElement<Settlement>.Sort(ref settlement1, ref settlement2, out flag);
				return num4;
			}
			return 0f;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x00093650 File Offset: 0x00091850
		protected override void GetFaceRecordForPoint(Vec2 position, out bool isOnRegion1)
		{
			isOnRegion1 = true;
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			CampaignVec2 campaignVec = new CampaignVec2(position, true);
			PathFaceRecord pathFaceRecord = mapSceneWrapper.GetFaceIndex(in campaignVec);
			if (!pathFaceRecord.IsValid())
			{
				isOnRegion1 = false;
				IMapScene mapSceneWrapper2 = Campaign.Current.MapSceneWrapper;
				campaignVec = new CampaignVec2(position, false);
				pathFaceRecord = mapSceneWrapper2.GetFaceIndex(in campaignVec);
			}
			if (!pathFaceRecord.IsValid())
			{
				Debug.Print(string.Format("{0} has no region data.", position), 0, Debug.DebugColor.Red, 17592186044416UL);
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000936CC File Offset: 0x000918CC
		protected override bool CheckBeingNeighbor(List<Settlement> settlementsToConsider, Settlement settlement1, Settlement settlement2, bool useGate1, bool useGate2, out float distance)
		{
			CampaignVec2 campaignVec = (useGate1 ? settlement1.GatePosition : settlement1.PortPosition);
			CampaignVec2 campaignVec2 = (useGate2 ? settlement2.GatePosition : settlement2.PortPosition);
			PathFaceRecord faceIndex = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
			PathFaceRecord faceIndex2 = this.MapSceneWrapper.GetFaceIndex(in campaignVec2);
			if (!faceIndex.IsValid() || !faceIndex2.IsValid())
			{
				Debug.FailedAssert("Settlement navFace index should not be -1, check here", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\DistanceCache\\SandboxNavigationCache.cs", "CheckBeingNeighbor", 193);
			}
			NavigationPath navigationPath = new NavigationPath();
			this.MapSceneWrapper.GetPathBetweenAIFaces(faceIndex, faceIndex2, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, navigationPath, this.GetExcludedFaceIds(), 2f, this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1());
			bool flag = navigationPath.Size > 0 || faceIndex.FaceIndex == faceIndex2.FaceIndex;
			bool flag2 = useGate1;
			if (!this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(faceIndex, faceIndex2, campaignVec.ToVec2(), campaignVec2.ToVec2(), 0.3f, Campaign.MapDiagonalSquared, out distance, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
			{
				distance = Campaign.MapDiagonalSquared;
			}
			int num = 0;
			while (num < navigationPath.Size && flag)
			{
				Vec2 vec = navigationPath[num] - ((num == 0) ? campaignVec.ToVec2() : navigationPath[num - 1]);
				float num2 = vec.Length / 1f;
				vec.Normalize();
				int num3 = 0;
				while ((float)num3 < num2)
				{
					Vec2 vec2 = ((num == 0) ? campaignVec.ToVec2() : navigationPath[num - 1]) + vec * 1f * (float)num3;
					if (vec2 != campaignVec.ToVec2() && vec2 != campaignVec2.ToVec2())
					{
						CampaignVec2 campaignVec3 = new CampaignVec2(vec2, flag2);
						PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec3);
						if (pathFaceRecord.FaceIndex == -1)
						{
							flag2 = !flag2;
							campaignVec3 = new CampaignVec2(vec2, flag2);
							pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec3);
						}
						bool flag3;
						float realPathDistanceFromPositionToSettlement = this.GetRealPathDistanceFromPositionToSettlement(vec2, pathFaceRecord, distance, settlement1, out flag3);
						float realPathDistanceFromPositionToSettlement2 = this.GetRealPathDistanceFromPositionToSettlement(vec2, pathFaceRecord, distance, settlement2, out flag3);
						float num4 = ((realPathDistanceFromPositionToSettlement < realPathDistanceFromPositionToSettlement2) ? realPathDistanceFromPositionToSettlement : realPathDistanceFromPositionToSettlement2);
						if (pathFaceRecord.FaceIndex != -1)
						{
							Settlement closestSettlementToPosition = base.GetClosestSettlementToPosition(vec2, pathFaceRecord, this.GetExcludedFaceIds(), settlementsToConsider, this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1(), num4 * 0.8f, out flag3);
							if (closestSettlementToPosition != null && closestSettlementToPosition != settlement1 && closestSettlementToPosition != settlement2)
							{
								flag = false;
								break;
							}
						}
					}
					num3++;
				}
				num++;
			}
			return flag;
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0009397C File Offset: 0x00091B7C
		protected override float GetRealPathDistanceFromPositionToSettlement(Vec2 checkPosition, PathFaceRecord currentFaceRecord, float maxDistanceToLookForPathDetection, Settlement currentSettlementToLook, out bool isPort)
		{
			float num = float.MaxValue;
			isPort = false;
			switch (base._navigationType)
			{
			case MobileParty.NavigationType.Default:
			{
				CampaignVec2 campaignVec = currentSettlementToLook.GatePosition;
				PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
				float num2;
				if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.GatePosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num2, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
				{
					num = num2;
				}
				break;
			}
			case MobileParty.NavigationType.Naval:
			{
				CampaignVec2 campaignVec = currentSettlementToLook.PortPosition;
				PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
				float num3;
				if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.PortPosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num3, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
				{
					num = num3;
					isPort = true;
				}
				break;
			}
			case MobileParty.NavigationType.All:
			{
				CampaignVec2 campaignVec = currentSettlementToLook.GatePosition;
				PathFaceRecord pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
				float num4;
				if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.GatePosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num4, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()))
				{
					num = num4;
				}
				if (currentSettlementToLook.HasPort)
				{
					campaignVec = currentSettlementToLook.PortPosition;
					pathFaceRecord = this.MapSceneWrapper.GetFaceIndex(in campaignVec);
					float num5;
					if (this.MapSceneWrapper.GetPathDistanceBetweenAIFaces(currentFaceRecord, pathFaceRecord, checkPosition, currentSettlementToLook.PortPosition.ToVec2(), 0.3f, maxDistanceToLookForPathDetection, out num5, this.GetExcludedFaceIds(), this.GetRegionSwitchCostTo0(), this.GetRegionSwitchCostTo1()) && num5 < num4)
					{
						num = num5;
						isPort = true;
					}
				}
				break;
			}
			}
			return num;
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00093B28 File Offset: 0x00091D28
		protected override IEnumerable<Settlement> GetClosestSettlementsToPositionInCache(Vec2 checkPosition, List<Settlement> settlements)
		{
			if (base._navigationType == MobileParty.NavigationType.Naval)
			{
				return from x in settlements
					where x.HasPort
					orderby checkPosition.DistanceSquared(x.PortPosition.ToVec2())
					select x;
			}
			return settlements.OrderBy<Settlement, float>((Settlement x) => checkPosition.DistanceSquared(x.GatePosition.ToVec2()));
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00093B94 File Offset: 0x00091D94
		protected override List<Settlement> GetAllRegisteredSettlements()
		{
			return Settlement.All.ToList<Settlement>();
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x00093BA0 File Offset: 0x00091DA0
		public void FinalizeInitialization()
		{
			base.FinalizeCacheInitialization();
		}

		// Token: 0x040009C1 RID: 2497
		private readonly int[] _excludedFaceIds;

		// Token: 0x040009C2 RID: 2498
		private readonly int _regionSwitchCostTo0;

		// Token: 0x040009C3 RID: 2499
		private readonly int _regionSwitchCostTo1;
	}
}
