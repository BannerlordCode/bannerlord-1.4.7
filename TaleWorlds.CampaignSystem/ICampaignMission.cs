using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005A RID: 90
	public interface ICampaignMission
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060008EC RID: 2284
		GameState State { get; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060008ED RID: 2285
		IMissionTroopSupplier AgentSupplier { get; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060008EE RID: 2286
		// (set) Token: 0x060008EF RID: 2287
		Location Location { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060008F0 RID: 2288
		// (set) Token: 0x060008F1 RID: 2289
		Alley LastVisitedAlley { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060008F2 RID: 2290
		MissionMode Mode { get; }

		// Token: 0x060008F3 RID: 2291
		void SetMissionMode(MissionMode newMode, bool atStart);

		// Token: 0x060008F4 RID: 2292
		void OnCloseEncounterMenu();

		// Token: 0x060008F5 RID: 2293
		bool AgentLookingAtAgent(IAgent agent1, IAgent agent2);

		// Token: 0x060008F6 RID: 2294
		void OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation);

		// Token: 0x060008F7 RID: 2295
		void OnProcessSentence();

		// Token: 0x060008F8 RID: 2296
		void OnConversationContinue();

		// Token: 0x060008F9 RID: 2297
		bool CheckIfAgentCanFollow(IAgent agent);

		// Token: 0x060008FA RID: 2298
		void AddAgentFollowing(IAgent agent);

		// Token: 0x060008FB RID: 2299
		bool CheckIfAgentCanUnFollow(IAgent agent);

		// Token: 0x060008FC RID: 2300
		void RemoveAgentFollowing(IAgent agent);

		// Token: 0x060008FD RID: 2301
		void OnConversationPlay(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath);

		// Token: 0x060008FE RID: 2302
		void OnConversationStart(IAgent agent, bool setActionsInstantly);

		// Token: 0x060008FF RID: 2303
		void OnConversationEnd(IAgent agent);

		// Token: 0x06000900 RID: 2304
		void EndMission();

		// Token: 0x06000901 RID: 2305
		void FadeOutCharacter(CharacterObject characterObject);

		// Token: 0x06000902 RID: 2306
		void OnGameStateChanged();
	}
}
