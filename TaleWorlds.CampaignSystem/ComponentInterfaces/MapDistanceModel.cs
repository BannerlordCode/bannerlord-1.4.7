using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000192 RID: 402
	public abstract class MapDistanceModel : MBGameModel<MapDistanceModel>
	{
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001C3E RID: 7230
		public abstract int RegionSwitchCostFromLandToSea { get; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001C3F RID: 7231
		public abstract int RegionSwitchCostFromSeaToLand { get; }

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001C40 RID: 7232
		public abstract float MaximumSpawnDistanceForCompanionsAfterDisband { get; }

		// Token: 0x06001C41 RID: 7233
		public abstract float GetMaximumDistanceBetweenTwoConnectedSettlements(MobileParty.NavigationType navigationType);

		// Token: 0x06001C42 RID: 7234
		public abstract float GetLandRatioOfPathBetweenSettlements(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort);

		// Token: 0x06001C43 RID: 7235
		public abstract float GetDistance(MobileParty fromMobileParty, Settlement toSettlement, bool isTargetingPort, MobileParty.NavigationType customCapability, out float estimatedLandRatio);

		// Token: 0x06001C44 RID: 7236
		public abstract float GetDistance(MobileParty fromMobileParty, MobileParty toMobileParty, MobileParty.NavigationType customCapability, out float landRatio);

		// Token: 0x06001C45 RID: 7237
		public abstract bool GetDistance(MobileParty fromMobileParty, MobileParty toMobileParty, MobileParty.NavigationType customCapability, float maxDistance, out float distance, out float landRatio);

		// Token: 0x06001C46 RID: 7238
		public abstract float GetDistance(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort, MobileParty.NavigationType navigationCapability);

		// Token: 0x06001C47 RID: 7239
		public abstract float GetDistance(Settlement fromSettlement, Settlement toSettlement, bool isFromPort, bool isTargetingPort, MobileParty.NavigationType navigationCapability, out float landRatio);

		// Token: 0x06001C48 RID: 7240
		public abstract float GetDistance(MobileParty fromMobileParty, in CampaignVec2 toPoint, MobileParty.NavigationType navigationType, out float landRatio);

		// Token: 0x06001C49 RID: 7241
		public abstract float GetDistance(Settlement fromSettlement, in CampaignVec2 toPoint, bool isFromPort, MobileParty.NavigationType navigationType);

		// Token: 0x06001C4A RID: 7242
		public abstract float GetPortToGateDistanceForSettlement(Settlement settlement);

		// Token: 0x06001C4B RID: 7243
		public abstract bool PathExistBetweenPoints(in CampaignVec2 fromPoint, in CampaignVec2 toPoint, MobileParty.NavigationType navigationType);

		// Token: 0x06001C4C RID: 7244
		public abstract void RegisterDistanceCache(MobileParty.NavigationType navigationCapability, MapDistanceModel.INavigationCache cacheToRegister);

		// Token: 0x06001C4D RID: 7245
		public abstract ValueTuple<Settlement, bool> GetClosestEntranceToFace(PathFaceRecord face, MobileParty.NavigationType navigationCapabilities);

		// Token: 0x06001C4E RID: 7246
		public abstract MBReadOnlyList<Settlement> GetNeighborsOfFortification(Town town, MobileParty.NavigationType navigationCapabilities);

		// Token: 0x06001C4F RID: 7247
		public abstract float GetTransitionCostAdjustment(Settlement settlement1, bool isFromPort, Settlement settlement2, bool isTargetingPort, bool fromIsCurrentlyAtSea, bool toIsCurrentlyAtSea);

		// Token: 0x04000950 RID: 2384
		public const float PossibleMaximumMapBoundary = 100000000f;

		// Token: 0x020005FE RID: 1534
		public interface INavigationCache
		{
			// Token: 0x17000F3B RID: 3899
			// (get) Token: 0x06005016 RID: 20502
			float MaximumDistanceBetweenTwoConnectedSettlements { get; }

			// Token: 0x06005017 RID: 20503
			float GetSettlementToSettlementDistanceWithLandRatio(Settlement settlement1, bool isAtSea1, Settlement settlement2, bool isAtSea2, out float landRatio);

			// Token: 0x06005018 RID: 20504
			MBReadOnlyList<Settlement> GetNeighbors(Settlement settlement);

			// Token: 0x06005019 RID: 20505
			Settlement GetClosestSettlementToFaceIndex(int faceId, out bool isAtSea);

			// Token: 0x0600501A RID: 20506
			void FinalizeInitialization();
		}
	}
}
