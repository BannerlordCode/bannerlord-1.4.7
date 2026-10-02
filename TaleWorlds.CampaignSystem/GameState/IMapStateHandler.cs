using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200039A RID: 922
	public interface IMapStateHandler
	{
		// Token: 0x0600355B RID: 13659
		void OnRefreshState();

		// Token: 0x0600355C RID: 13660
		void OnMainPartyEncounter();

		// Token: 0x0600355D RID: 13661
		void OnIncidentStarted(Incident incident);

		// Token: 0x0600355E RID: 13662
		void BeforeTick(float dt);

		// Token: 0x0600355F RID: 13663
		void Tick(float dt);

		// Token: 0x06003560 RID: 13664
		void AfterTick(float dt);

		// Token: 0x06003561 RID: 13665
		void AfterWaitTick(float dt);

		// Token: 0x06003562 RID: 13666
		void OnIdleTick(float dt);

		// Token: 0x06003563 RID: 13667
		void OnSignalPeriodicEvents();

		// Token: 0x06003564 RID: 13668
		void OnExit();

		// Token: 0x06003565 RID: 13669
		void ResetCamera(bool resetDistance, bool teleportToMainParty);

		// Token: 0x06003566 RID: 13670
		void TeleportCameraToMainParty();

		// Token: 0x06003567 RID: 13671
		void FastMoveCameraToMainParty();

		// Token: 0x06003568 RID: 13672
		bool IsCameraLockedToPlayerParty();

		// Token: 0x06003569 RID: 13673
		void StartCameraAnimation(CampaignVec2 targetPosition, float animationStopDuration);

		// Token: 0x0600356A RID: 13674
		void OnHourlyTick();

		// Token: 0x0600356B RID: 13675
		void OnMenuModeTick(float dt);

		// Token: 0x0600356C RID: 13676
		void OnEnteringMenuMode(MenuContext menuContext);

		// Token: 0x0600356D RID: 13677
		void OnExitingMenuMode();

		// Token: 0x0600356E RID: 13678
		void OnBattleSimulationStarted(BattleSimulation battleSimulation);

		// Token: 0x0600356F RID: 13679
		void OnBattleSimulationEnded();

		// Token: 0x06003570 RID: 13680
		void OnGameplayCheatsEnabled();

		// Token: 0x06003571 RID: 13681
		void OnMapConversationStarts(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData);

		// Token: 0x06003572 RID: 13682
		void OnMapConversationOver();

		// Token: 0x06003573 RID: 13683
		void OnPlayerSiegeActivated();

		// Token: 0x06003574 RID: 13684
		void OnPlayerSiegeDeactivated();

		// Token: 0x06003575 RID: 13685
		void OnSiegeEngineClick(MatrixFrame siegeEngineFrame);

		// Token: 0x06003576 RID: 13686
		void OnGameLoadFinished();
	}
}
