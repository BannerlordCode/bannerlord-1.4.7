using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009C RID: 156
	public interface IViewDataTracker
	{
		// Token: 0x060012D0 RID: 4816
		void SetInventoryLocks(IEnumerable<string> locks);

		// Token: 0x060012D1 RID: 4817
		IEnumerable<string> GetInventoryLocks();

		// Token: 0x060012D2 RID: 4818
		bool GetMapBarExtendedState();

		// Token: 0x060012D3 RID: 4819
		void SetMapBarExtendedState(bool value);

		// Token: 0x060012D4 RID: 4820
		void SetPartyTroopLocks(IEnumerable<string> locks);

		// Token: 0x060012D5 RID: 4821
		void SetPartyPrisonerLocks(IEnumerable<string> locks);

		// Token: 0x060012D6 RID: 4822
		void SetPartySortType(int sortType);

		// Token: 0x060012D7 RID: 4823
		void SetIsPartySortAscending(bool isAscending);

		// Token: 0x060012D8 RID: 4824
		IEnumerable<string> GetPartyTroopLocks();

		// Token: 0x060012D9 RID: 4825
		IEnumerable<string> GetPartyPrisonerLocks();

		// Token: 0x060012DA RID: 4826
		int GetPartySortType();

		// Token: 0x060012DB RID: 4827
		bool GetIsPartySortAscending();

		// Token: 0x060012DC RID: 4828
		void AddEncyclopediaBookmarkToItem(Concept concept);

		// Token: 0x060012DD RID: 4829
		void AddEncyclopediaBookmarkToItem(Kingdom kingdom);

		// Token: 0x060012DE RID: 4830
		void AddEncyclopediaBookmarkToItem(Settlement settlement);

		// Token: 0x060012DF RID: 4831
		void AddEncyclopediaBookmarkToItem(CharacterObject unit);

		// Token: 0x060012E0 RID: 4832
		void AddEncyclopediaBookmarkToItem(Hero item);

		// Token: 0x060012E1 RID: 4833
		void AddEncyclopediaBookmarkToItem(ShipHull shipHull);

		// Token: 0x060012E2 RID: 4834
		void AddEncyclopediaBookmarkToItem(Clan clan);

		// Token: 0x060012E3 RID: 4835
		void RemoveEncyclopediaBookmarkFromItem(Hero hero);

		// Token: 0x060012E4 RID: 4836
		void RemoveEncyclopediaBookmarkFromItem(ShipHull shipHull);

		// Token: 0x060012E5 RID: 4837
		void RemoveEncyclopediaBookmarkFromItem(Clan clan);

		// Token: 0x060012E6 RID: 4838
		void RemoveEncyclopediaBookmarkFromItem(Concept concept);

		// Token: 0x060012E7 RID: 4839
		void RemoveEncyclopediaBookmarkFromItem(Kingdom kingdom);

		// Token: 0x060012E8 RID: 4840
		void RemoveEncyclopediaBookmarkFromItem(Settlement settlement);

		// Token: 0x060012E9 RID: 4841
		void RemoveEncyclopediaBookmarkFromItem(CharacterObject unit);

		// Token: 0x060012EA RID: 4842
		bool IsEncyclopediaBookmarked(Hero hero);

		// Token: 0x060012EB RID: 4843
		bool IsEncyclopediaBookmarked(ShipHull shipHull);

		// Token: 0x060012EC RID: 4844
		bool IsEncyclopediaBookmarked(Clan clan);

		// Token: 0x060012ED RID: 4845
		bool IsEncyclopediaBookmarked(Concept concept);

		// Token: 0x060012EE RID: 4846
		bool IsEncyclopediaBookmarked(Kingdom kingdom);

		// Token: 0x060012EF RID: 4847
		bool IsEncyclopediaBookmarked(Settlement settlement);

		// Token: 0x060012F0 RID: 4848
		bool IsEncyclopediaBookmarked(CharacterObject unit);

		// Token: 0x060012F1 RID: 4849
		void SetQuestSelection(QuestBase selection);

		// Token: 0x060012F2 RID: 4850
		QuestBase GetQuestSelection();

		// Token: 0x060012F3 RID: 4851
		void SetQuestSortTypeSelection(int questSortTypeSelection);

		// Token: 0x060012F4 RID: 4852
		int GetQuestSortTypeSelection();

		// Token: 0x060012F5 RID: 4853
		void InventorySetSortPreference(int inventoryMode, int sortOption, int sortState);

		// Token: 0x060012F6 RID: 4854
		Tuple<int, int> InventoryGetSortPreference(int inventoryMode);

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060012F7 RID: 4855
		bool IsPartyNotificationActive { get; }

		// Token: 0x060012F8 RID: 4856
		TextObject GetPartyNotificationText();

		// Token: 0x060012F9 RID: 4857
		void ClearPartyNotification();

		// Token: 0x060012FA RID: 4858
		void UpdatePartyNotification();

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060012FB RID: 4859
		bool IsQuestNotificationActive { get; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060012FC RID: 4860
		IReadOnlyList<JournalLog> UnExaminedQuestLogs { get; }

		// Token: 0x060012FD RID: 4861
		TextObject GetQuestNotificationText();

		// Token: 0x060012FE RID: 4862
		void OnQuestLogExamined(JournalLog log);

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060012FF RID: 4863
		List<Army> UnExaminedArmies { get; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001300 RID: 4864
		int NumOfKingdomArmyNotifications { get; }

		// Token: 0x06001301 RID: 4865
		void OnArmyExamined(Army army);

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001302 RID: 4866
		bool IsCharacterNotificationActive { get; }

		// Token: 0x06001303 RID: 4867
		void ClearCharacterNotification();

		// Token: 0x06001304 RID: 4868
		TextObject GetCharacterNotificationText();

		// Token: 0x06001305 RID: 4869
		MBReadOnlyList<ItemRosterElement> GetPlunderItems();

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001306 RID: 4870
		IReadOnlyList<Figurehead> UnexaminedFigureheads { get; }

		// Token: 0x06001307 RID: 4871
		void OnFigureheadExamined(Figurehead figurehead);
	}
}
