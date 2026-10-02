using System;
using System.Collections.Generic;
using SandBox.View.Map;
using SandBox.ViewModelCollection.MapSiege;
using SandBox.ViewModelCollection.Missions.NameMarker;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;

namespace SandBox.GauntletUI.Tutorial
{
	// Token: 0x02000017 RID: 23
	public abstract class TutorialItemBase
	{
		// Token: 0x0600013C RID: 316
		public abstract bool IsConditionsMetForCompletion();

		// Token: 0x0600013D RID: 317
		public abstract bool IsConditionsMetForActivation();

		// Token: 0x0600013E RID: 318
		public abstract TutorialContexts GetTutorialsRelevantContext();

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000A35D File Offset: 0x0000855D
		// (set) Token: 0x0600013F RID: 319 RVA: 0x0000A354 File Offset: 0x00008554
		public TutorialItemVM.ItemPlacements Placement { get; protected set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000A36E File Offset: 0x0000856E
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000A365 File Offset: 0x00008565
		public bool MouseRequired { get; protected set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000A37F File Offset: 0x0000857F
		// (set) Token: 0x06000143 RID: 323 RVA: 0x0000A376 File Offset: 0x00008576
		public string HighlightedVisualElementID { get; protected set; }

		// Token: 0x06000145 RID: 325 RVA: 0x0000A387 File Offset: 0x00008587
		protected virtual string GetCustomTutorialElementHighlightID()
		{
			return "";
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000A38E File Offset: 0x0000858E
		public virtual void OnDeactivate()
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000A390 File Offset: 0x00008590
		public virtual bool IsConditionsMetForVisibility()
		{
			return this.GetTutorialsRelevantContext() != TutorialContexts.Mission || !BannerlordConfig.HideBattleUI;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000A3A5 File Offset: 0x000085A5
		public virtual void OnInventoryTransferItem(InventoryTransferItemEvent obj)
		{
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000A3A7 File Offset: 0x000085A7
		public virtual void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000A3A9 File Offset: 0x000085A9
		public virtual void OnInventoryFilterChanged(InventoryFilterChangedEvent obj)
		{
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000A3AB File Offset: 0x000085AB
		public virtual void OnPerkSelectedByPlayer(PerkSelectedByPlayerEvent obj)
		{
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A3AD File Offset: 0x000085AD
		public virtual void OnFocusAddedByPlayer(FocusAddedByPlayerEvent obj)
		{
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A3AF File Offset: 0x000085AF
		public virtual void OnGameMenuOpened(MenuCallbackArgs obj)
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A3B1 File Offset: 0x000085B1
		public virtual void OnMainMapCameraMove(MapScreen.MainMapCameraMoveEvent obj)
		{
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A3B3 File Offset: 0x000085B3
		public virtual void OnCharacterPortraitPopUpOpened(CharacterObject obj)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A3B5 File Offset: 0x000085B5
		public virtual void OnPlayerStartTalkFromMenuOverlay(Hero obj)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A3B7 File Offset: 0x000085B7
		public virtual void OnGameMenuOptionSelected(GameMenuOption obj)
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A3B9 File Offset: 0x000085B9
		public virtual void OnPlayerStartRecruitment(CharacterObject obj)
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A3BB File Offset: 0x000085BB
		public virtual void OnNewCompanionAdded(Hero obj)
		{
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000A3BD File Offset: 0x000085BD
		public virtual void OnPlayerRecruitedUnit(CharacterObject obj, int count)
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A3BF File Offset: 0x000085BF
		public virtual void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A3C1 File Offset: 0x000085C1
		public virtual void OnMissionNameMarkerToggled(MissionNameMarkerToggleEvent obj)
		{
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000A3C3 File Offset: 0x000085C3
		public virtual void OnPlayerToggleTrackSettlementFromEncyclopedia(PlayerToggleTrackSettlementFromEncyclopediaEvent obj)
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000A3C5 File Offset: 0x000085C5
		public virtual void OnInventoryEquipmentTypeChange(InventoryEquipmentTypeChangedEvent obj)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000A3C7 File Offset: 0x000085C7
		public virtual void OnArmyCohesionByPlayerBoosted(ArmyCohesionBoostedByPlayerEvent obj)
		{
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000A3C9 File Offset: 0x000085C9
		public virtual void OnPartyAddedToArmyByPlayer(PartyAddedToArmyByPlayerEvent obj)
		{
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000A3CB File Offset: 0x000085CB
		public virtual void OnPlayerStartEngineConstruction(PlayerStartEngineConstructionEvent obj)
		{
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000A3CD File Offset: 0x000085CD
		public virtual void OnPlayerUpgradeTroop(CharacterObject arg1, CharacterObject arg2, int arg3)
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000A3CF File Offset: 0x000085CF
		public virtual void OnPlayerMoveTroop(PlayerMoveTroopEvent obj)
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000A3D1 File Offset: 0x000085D1
		public virtual void OnPerkSelectionToggle(PerkSelectionToggleEvent obj)
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000A3D3 File Offset: 0x000085D3
		public virtual void OnPlayerInspectedPartySpeed(PlayerInspectedPartySpeedEvent obj)
		{
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A3D5 File Offset: 0x000085D5
		public virtual void OnPlayerMovementFlagChanged(MissionPlayerMovementFlagsChangeEvent obj)
		{
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A3D7 File Offset: 0x000085D7
		public virtual void OnPlayerToggledUpgradePopup(PlayerToggledUpgradePopupEvent obj)
		{
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000A3D9 File Offset: 0x000085D9
		public virtual void OnOrderOfBattleHeroAssignedToFormation(OrderOfBattleHeroAssignedToFormationEvent obj)
		{
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000A3DB File Offset: 0x000085DB
		public virtual void OnOrderOfBattleFormationClassChanged(OrderOfBattleFormationClassChangedEvent obj)
		{
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000A3DD File Offset: 0x000085DD
		public virtual void OnOrderOfBattleFormationWeightChanged(OrderOfBattleFormationWeightChangedEvent obj)
		{
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000A3DF File Offset: 0x000085DF
		public virtual void OnCraftingWeaponClassSelectionOpened(CraftingWeaponClassSelectionOpenedEvent obj)
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000A3E1 File Offset: 0x000085E1
		public virtual void OnCraftingOnWeaponResultPopupOpened(CraftingWeaponResultPopupToggledEvent obj)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000A3E3 File Offset: 0x000085E3
		public virtual void OnCraftingOrderTabOpened(CraftingOrderTabOpenedEvent obj)
		{
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000A3E5 File Offset: 0x000085E5
		public virtual void OnCraftingOrderSelectionOpened(CraftingOrderSelectionOpenedEvent obj)
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A3E7 File Offset: 0x000085E7
		public virtual void OnInventoryItemInspected(InventoryItemInspectedEvent obj)
		{
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000A3E9 File Offset: 0x000085E9
		public virtual void OnCrimeValueInspectedInSettlementOverlay(CrimeValueInspectedInSettlementOverlayEvent obj)
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A3EB File Offset: 0x000085EB
		public virtual void OnClanRoleAssignedThroughClanScreen(ClanRoleAssignedThroughClanScreenEvent obj)
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A3ED File Offset: 0x000085ED
		public virtual void OnPlayerSelectedAKingdomDecisionOption(PlayerSelectedAKingdomDecisionOptionEvent obj)
		{
		}
	}
}
