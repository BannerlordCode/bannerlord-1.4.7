using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x02000078 RID: 120
	public class MPArmoryCosmeticsVM : ViewModel
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000BD2 RID: 3026 RVA: 0x00023698 File Offset: 0x00021898
		// (remove) Token: 0x06000BD3 RID: 3027 RVA: 0x000236CC File Offset: 0x000218CC
		public static event Action<MPArmoryCosmeticItemBaseVM> OnCosmeticPreview;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000BD4 RID: 3028 RVA: 0x00023700 File Offset: 0x00021900
		// (remove) Token: 0x06000BD5 RID: 3029 RVA: 0x00023734 File Offset: 0x00021934
		public static event Action<MPArmoryCosmeticItemBaseVM> OnRemoveCosmeticFromPreview;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000BD6 RID: 3030 RVA: 0x00023768 File Offset: 0x00021968
		// (remove) Token: 0x06000BD7 RID: 3031 RVA: 0x0002379C File Offset: 0x0002199C
		public static event Action<List<EquipmentElement>> OnEquipmentRefreshed;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000BD8 RID: 3032 RVA: 0x000237D0 File Offset: 0x000219D0
		// (remove) Token: 0x06000BD9 RID: 3033 RVA: 0x00023804 File Offset: 0x00021A04
		public static event Action OnTauntAssignmentRefresh;

		// Token: 0x06000BDA RID: 3034 RVA: 0x00023838 File Offset: 0x00021A38
		public MPArmoryCosmeticsVM(Func<List<IReadOnlyPerkObject>> getSelectedPerks)
		{
			this._getSelectedPerks = getSelectedPerks;
			this._usedCosmetics = new Dictionary<string, List<string>>();
			this._ownedCosmetics = new List<string>();
			this._clothingCategoriesLookup = new Dictionary<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM>();
			this._tauntCategoriesLookup = new Dictionary<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM>();
			this._cosmeticItemsLookup = new Dictionary<string, MPArmoryCosmeticItemBaseVM>();
			this.AvailableCategories = new MBBindingList<MPArmoryCosmeticCategoryBaseVM>();
			this.SortCategories = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnSortCategoryUpdated));
			this.SortOrders = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnSortOrderUpdated));
			this.TauntSlots = new MBBindingList<MPArmoryCosmeticTauntSlotVM>();
			this.InitializeCosmeticItemComparers();
			this.InitializeAllCosmetics();
			this.InitializeCallbacks();
			this.IsLoading = true;
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=J2wEawTl}Category", null)));
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=ebUrBmHK}Price", null)));
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=bD8nTS86}Rarity", null)));
			this.SortCategories.AddItem(new SelectorItemVM(new TextObject("{=PDdh1sBj}Name", null)));
			this.SortCategories.SelectedIndex = 0;
			this.SortOrders.AddItem(new SelectorItemVM(new TextObject("{=mOmFzU78}Ascending", null)));
			this.SortOrders.AddItem(new SelectorItemVM(new TextObject("{=FgFUsncP}Descending", null)));
			this.SortOrders.SelectedIndex = 0;
			this.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Clothing);
			this.RefreshValues();
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000239B0 File Offset: 0x00021BB0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SortCategories.RefreshValues();
			this.SortOrders.RefreshValues();
			this.CosmeticInfoErrorText = new TextObject("{=ehkVpzpa}Unable to get cosmetic information", null).ToString();
			this.AllCategoriesHint = new HintViewModel(new TextObject("{=yfa7tpbK}All", null), null);
			this.BodyCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_13", null), null);
			this.HeadCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_12", null), null);
			this.ShoulderCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_22", null), null);
			this.HandCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_15", null), null);
			this.LegCategoryHint = new HintViewModel(GameTexts.FindText("str_inventory_type_14", null), null);
			this.ResetPreviewHint = new HintViewModel(new TextObject("{=imUnCFgZ}Reset preview", null), null);
			this._allCosmetics.ForEach(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
			this.AvailableCategories.ApplyActionOnAllItems(delegate(MPArmoryCosmeticCategoryBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00023AE4 File Offset: 0x00021CE4
		private void InitializeCallbacks()
		{
			MPArmoryClothingCosmeticCategoryVM.OnSelected += this.OnClothingCosmeticCategorySelected;
			MPArmoryTauntCosmeticCategoryVM.OnSelected += this.OnTauntCosmeticCategorySelected;
			MPArmoryCosmeticItemBaseVM.OnPreviewed += this.EquipItemOnHeroPreview;
			MPArmoryCosmeticItemBaseVM.OnEquipped += this.OnCosmeticEquipRequested;
			MPArmoryCosmeticTauntSlotVM.OnFocusChanged += this.OnTauntSlotFocusChanged;
			MPArmoryCosmeticTauntSlotVM.OnSelected += this.OnTauntSlotSelected;
			MPArmoryCosmeticTauntSlotVM.OnPreview += this.OnTauntSlotPreview;
			MPArmoryCosmeticTauntSlotVM.OnTauntEquipped += this.OnTauntItemEquipped;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00023B7C File Offset: 0x00021D7C
		private void FinalizeCallbacks()
		{
			MPArmoryClothingCosmeticCategoryVM.OnSelected -= this.OnClothingCosmeticCategorySelected;
			MPArmoryTauntCosmeticCategoryVM.OnSelected -= this.OnTauntCosmeticCategorySelected;
			MPArmoryCosmeticItemBaseVM.OnPreviewed -= this.EquipItemOnHeroPreview;
			MPArmoryCosmeticItemBaseVM.OnEquipped -= this.OnCosmeticEquipRequested;
			MPArmoryCosmeticTauntSlotVM.OnFocusChanged -= this.OnTauntSlotFocusChanged;
			MPArmoryCosmeticTauntSlotVM.OnSelected -= this.OnTauntSlotSelected;
			MPArmoryCosmeticTauntSlotVM.OnPreview -= this.OnTauntSlotPreview;
			MPArmoryCosmeticTauntSlotVM.OnTauntEquipped -= this.OnTauntItemEquipped;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00023C11 File Offset: 0x00021E11
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.FinalizeCallbacks();
			this.AvailableCategories.ApplyActionOnAllItems(delegate(MPArmoryCosmeticCategoryBaseVM c)
			{
				c.OnFinalize();
			});
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00023C4C File Offset: 0x00021E4C
		public async void OnTick(float dt)
		{
			if (NetworkMain.GameClient == null)
			{
				this._isNetworkCosmeticsDirty = false;
				this._isLocalCosmeticsDirty = false;
			}
			if (!this._isSendingCosmeticData && !this._isRetrievingCosmeticData)
			{
				if (this._isNetworkCosmeticsDirty)
				{
					this.RefreshCosmeticInfoFromNetworkAux();
					this._isNetworkCosmeticsDirty = false;
				}
				if (this._isLocalCosmeticsDirty)
				{
					await this.UpdateUsedCosmeticsAux();
					this._isLocalCosmeticsDirty = false;
				}
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00023C88 File Offset: 0x00021E88
		private void InitializeCosmeticItemComparers()
		{
			this._itemComparers = new List<MPArmoryCosmeticsVM.CosmeticItemComparer>
			{
				new MPArmoryCosmeticsVM.CosmeticItemCategoryComparer(),
				new MPArmoryCosmeticsVM.CosmeticItemCostComparer(),
				new MPArmoryCosmeticsVM.CosmeticItemRarityComparer(),
				new MPArmoryCosmeticsVM.CosmeticItemNameComparer()
			};
			this._currentItemComparer = this._itemComparers[0];
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00023CE0 File Offset: 0x00021EE0
		private void InitializeAllCosmetics()
		{
			this._tauntCategoriesLookup.Clear();
			this._tauntCategoriesLookup.Add(MPArmoryCosmeticsVM.TauntCategoryFlag.All, new MPArmoryTauntCosmeticCategoryVM(MPArmoryCosmeticsVM.TauntCategoryFlag.All));
			foreach (object obj in Enum.GetValues(typeof(MPArmoryCosmeticsVM.TauntCategoryFlag)))
			{
				MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag = (MPArmoryCosmeticsVM.TauntCategoryFlag)obj;
				if (tauntCategoryFlag > MPArmoryCosmeticsVM.TauntCategoryFlag.None && tauntCategoryFlag < MPArmoryCosmeticsVM.TauntCategoryFlag.All)
				{
					this._tauntCategoriesLookup.Add(tauntCategoryFlag, new MPArmoryTauntCosmeticCategoryVM(tauntCategoryFlag));
				}
			}
			this._clothingCategoriesLookup.Clear();
			for (MPArmoryCosmeticsVM.ClothingCategory clothingCategory = MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesBegin; clothingCategory < MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesEnd; clothingCategory++)
			{
				this._clothingCategoriesLookup.Add(clothingCategory, new MPArmoryClothingCosmeticCategoryVM(clothingCategory));
			}
			this._allCosmetics = new List<MPArmoryCosmeticItemBaseVM>();
			List<CosmeticElement> list = CosmeticsManager.CosmeticElementsList.ToList<CosmeticElement>();
			for (int i = 0; i < list.Count; i++)
			{
				ClothingCosmeticElement clothingCosmeticElement;
				TauntCosmeticElement tauntCosmeticElement;
				if (list[i].Type == CosmeticsManager.CosmeticType.Clothing && (clothingCosmeticElement = list[i] as ClothingCosmeticElement) != null)
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM = new MPArmoryCosmeticClothingItemVM(clothingCosmeticElement, clothingCosmeticElement.Id);
					mparmoryCosmeticClothingItemVM.IsUnlocked = clothingCosmeticElement.IsFree;
					mparmoryCosmeticClothingItemVM.IsSelectable = true;
					this._allCosmetics.Add(mparmoryCosmeticClothingItemVM);
					this._cosmeticItemsLookup.Add(clothingCosmeticElement.Id, mparmoryCosmeticClothingItemVM);
					this._clothingCategoriesLookup[mparmoryCosmeticClothingItemVM.ClothingCategory].AvailableCosmetics.Add(mparmoryCosmeticClothingItemVM);
					this._clothingCategoriesLookup[MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesBegin].AvailableCosmetics.Add(mparmoryCosmeticClothingItemVM);
				}
				else if (list[i].Type == CosmeticsManager.CosmeticType.Taunt && (tauntCosmeticElement = list[i] as TauntCosmeticElement) != null)
				{
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM = new MPArmoryCosmeticTauntItemVM(tauntCosmeticElement.Id, tauntCosmeticElement, tauntCosmeticElement.Id);
					mparmoryCosmeticTauntItemVM.IsUnlocked = tauntCosmeticElement.IsFree;
					mparmoryCosmeticTauntItemVM.IsSelectable = true;
					this._allCosmetics.Add(mparmoryCosmeticTauntItemVM);
					this._cosmeticItemsLookup.Add(tauntCosmeticElement.Id, mparmoryCosmeticTauntItemVM);
					foreach (object obj2 in Enum.GetValues(typeof(MPArmoryCosmeticsVM.TauntCategoryFlag)))
					{
						MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag2 = (MPArmoryCosmeticsVM.TauntCategoryFlag)obj2;
						if (tauntCategoryFlag2 > MPArmoryCosmeticsVM.TauntCategoryFlag.None && tauntCategoryFlag2 <= MPArmoryCosmeticsVM.TauntCategoryFlag.All && (mparmoryCosmeticTauntItemVM.TauntCategory & tauntCategoryFlag2) != MPArmoryCosmeticsVM.TauntCategoryFlag.None)
						{
							this._tauntCategoriesLookup[tauntCategoryFlag2].AvailableCosmetics.Add(mparmoryCosmeticTauntItemVM);
						}
					}
				}
			}
			for (int j = 0; j < TauntCosmeticElement.MaxNumberOfTaunts; j++)
			{
				this.TauntSlots.Add(new MPArmoryCosmeticTauntSlotVM(j));
			}
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00023F98 File Offset: 0x00022198
		private void OnClothingCosmeticCategorySelected(MPArmoryClothingCosmeticCategoryVM selectedCosmetic)
		{
			this.FilterClothingsByCategory(selectedCosmetic);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00023FA1 File Offset: 0x000221A1
		private void OnTauntCosmeticCategorySelected(MPArmoryTauntCosmeticCategoryVM selectedCosmetic)
		{
			this.FilterTauntsByCategory(selectedCosmetic);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00023FAC File Offset: 0x000221AC
		public void RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType type)
		{
			this._currentCosmeticType = type;
			this.AvailableCategories.Clear();
			if (type == CosmeticsManager.CosmeticType.Clothing)
			{
				using (Dictionary<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM>.Enumerator enumerator = this._clothingCategoriesLookup.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM> keyValuePair = enumerator.Current;
						this.AvailableCategories.Add(keyValuePair.Value);
					}
					goto IL_009B;
				}
			}
			if (type == CosmeticsManager.CosmeticType.Taunt)
			{
				foreach (KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> keyValuePair2 in this._tauntCategoriesLookup)
				{
					this.AvailableCategories.Add(keyValuePair2.Value);
				}
			}
			IL_009B:
			if (this.AvailableCategories.Count > 0)
			{
				if (type == CosmeticsManager.CosmeticType.Clothing && this._currentClothingCategory != MPArmoryCosmeticsVM.ClothingCategory.Invalid)
				{
					this.FilterClothingsByCategory(this._clothingCategoriesLookup[this._currentClothingCategory]);
					return;
				}
				if (type == CosmeticsManager.CosmeticType.Taunt)
				{
					MPArmoryCosmeticsVM.TauntCategoryFlag tauntCategoryFlag = ((this._currentTauntCategory != MPArmoryCosmeticsVM.TauntCategoryFlag.None) ? this._currentTauntCategory : MPArmoryCosmeticsVM.TauntCategoryFlag.All);
					this.FilterTauntsByCategory(this._tauntCategoriesLookup[tauntCategoryFlag]);
				}
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000240D0 File Offset: 0x000222D0
		public void RefreshPlayerData(PlayerData playerData)
		{
			this.Loot = playerData.Gold;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x000240DE File Offset: 0x000222DE
		public void RefreshCosmeticInfoFromNetwork()
		{
			this._isNetworkCosmeticsDirty = true;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x000240E8 File Offset: 0x000222E8
		private void RefreshCosmeticInfoFromNetworkAux()
		{
			this._isRetrievingCosmeticData = true;
			if (NetworkMain.GameClient.PlayerData == null)
			{
				this._isRetrievingCosmeticData = false;
				return;
			}
			this.IsLoading = true;
			this.HasCosmeticInfoReceived = true;
			this.IsLoading = false;
			LobbyClient gameClient = NetworkMain.GameClient;
			string text;
			if (gameClient == null)
			{
				text = null;
			}
			else
			{
				PlayerData playerData = gameClient.PlayerData;
				text = ((playerData != null) ? playerData.UserId.ToString() : null);
			}
			string text2 = text;
			LobbyClient gameClient2 = NetworkMain.GameClient;
			IReadOnlyDictionary<string, List<string>> readOnlyDictionary = ((gameClient2 != null) ? gameClient2.UsedCosmetics : null);
			LobbyClient gameClient3 = NetworkMain.GameClient;
			List<string> list;
			if (gameClient3 == null)
			{
				list = null;
			}
			else
			{
				IReadOnlyList<string> ownedCosmetics = gameClient3.OwnedCosmetics;
				list = ((ownedCosmetics != null) ? ownedCosmetics.ToList<string>() : null);
			}
			List<string> list2 = list;
			if (text2 == null || readOnlyDictionary == null || list2 == null)
			{
				this._isRetrievingCosmeticData = false;
				return;
			}
			this._ownedCosmetics = list2;
			MBReadOnlyList<TauntIndexData> tauntIndicesForPlayer = MultiplayerLocalDataManager.Instance.TauntSlotData.GetTauntIndicesForPlayer(text2);
			this.RefreshTaunts(text2, tauntIndicesForPlayer);
			this._usedCosmetics = new Dictionary<string, List<string>>();
			foreach (KeyValuePair<string, List<string>> keyValuePair in readOnlyDictionary)
			{
				this._usedCosmetics.Add(keyValuePair.Key, new List<string>());
				foreach (string text3 in readOnlyDictionary[keyValuePair.Key])
				{
					this._usedCosmetics[keyValuePair.Key].Add(text3);
				}
			}
			this.RefreshSelectedClass(this._selectedClass, this._getSelectedPerks());
			this._isRetrievingCosmeticData = false;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00024284 File Offset: 0x00022484
		private async Task<bool> UpdateUsedCosmeticsAux()
		{
			this._isSendingCosmeticData = true;
			IReadOnlyDictionary<string, List<string>> usedCosmetics = NetworkMain.GameClient.UsedCosmetics;
			Dictionary<string, List<ValueTuple<string, bool>>> dictionary = new Dictionary<string, List<ValueTuple<string, bool>>>();
			foreach (string text in this._usedCosmetics.Keys)
			{
				dictionary.Add(text, new List<ValueTuple<string, bool>>());
			}
			foreach (KeyValuePair<string, List<string>> keyValuePair in usedCosmetics)
			{
				foreach (string text2 in keyValuePair.Value)
				{
					if (!this._usedCosmetics[keyValuePair.Key].Contains(text2))
					{
						dictionary[keyValuePair.Key].Add(new ValueTuple<string, bool>(text2, false));
					}
				}
			}
			foreach (KeyValuePair<string, List<string>> keyValuePair2 in this._usedCosmetics)
			{
				if (!usedCosmetics.ContainsKey(keyValuePair2.Key))
				{
					using (List<string>.Enumerator enumerator3 = keyValuePair2.Value.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string text3 = enumerator3.Current;
							dictionary[keyValuePair2.Key].Add(new ValueTuple<string, bool>(text3, true));
						}
						continue;
					}
				}
				foreach (string text4 in keyValuePair2.Value)
				{
					if (!usedCosmetics[keyValuePair2.Key].Contains(text4))
					{
						dictionary[keyValuePair2.Key].Add(new ValueTuple<string, bool>(text4, true));
					}
				}
			}
			foreach (KeyValuePair<string, List<ValueTuple<string, bool>>> keyValuePair3 in dictionary)
			{
				List<ItemObject.ItemTypeEnum> list = new List<ItemObject.ItemTypeEnum>();
				foreach (ValueTuple<string, bool> valueTuple in keyValuePair3.Value)
				{
					string item = valueTuple.Item1;
					MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM;
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					if (valueTuple.Item2 && this._cosmeticItemsLookup.TryGetValue(item, out mparmoryCosmeticItemBaseVM) && (mparmoryCosmeticClothingItemVM = mparmoryCosmeticItemBaseVM as MPArmoryCosmeticClothingItemVM) != null)
					{
						ItemObject.ItemTypeEnum itemType = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType;
						list.Add(itemType);
					}
				}
			}
			List<TauntIndexData> list2 = new List<TauntIndexData>();
			for (int i = 0; i < this.TauntSlots.Count; i++)
			{
				MPArmoryCosmeticTauntItemVM assignedTauntItem = this.TauntSlots[i].AssignedTauntItem;
				if (assignedTauntItem != null)
				{
					TauntIndexData tauntIndexData = new TauntIndexData(assignedTauntItem.TauntID, i);
					list2.Add(tauntIndexData);
				}
			}
			bool flag = false;
			LobbyClient gameClient = NetworkMain.GameClient;
			string text5;
			if (gameClient == null)
			{
				text5 = null;
			}
			else
			{
				PlayerData playerData = gameClient.PlayerData;
				text5 = ((playerData != null) ? playerData.UserId.ToString() : null);
			}
			string text6 = text5;
			if (text6 != null)
			{
				MultiplayerLocalDataManager.Instance.TauntSlotData.SetTauntIndicesForPlayer(text6, list2);
				flag = await NetworkMain.GameClient.UpdateUsedCosmeticItems(dictionary);
			}
			this._isSendingCosmeticData = false;
			return flag;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x000242CC File Offset: 0x000224CC
		public void RefreshSelectedClass(MultiplayerClassDivisions.MPHeroClass selectedClass, List<IReadOnlyPerkObject> selectedPerks)
		{
			this._selectedClass = selectedClass;
			if (this._selectedClass == null)
			{
				return;
			}
			this._selectedClassDefaultEquipment = this._selectedClass.HeroCharacter.Equipment.Clone(false);
			if (selectedPerks != null)
			{
				MPArmoryVM.ApplyPerkEffectsToEquipment(ref this._selectedClassDefaultEquipment, selectedPerks);
			}
			this._selectedTroopID = this._selectedClass.StringId;
			MPArmoryCosmeticCategoryBaseVM activeCategory = this.ActiveCategory;
			if (activeCategory != null)
			{
				activeCategory.Sort(this._currentItemComparer);
			}
			if (this._ownedCosmetics != null)
			{
				using (List<string>.Enumerator enumerator = this._ownedCosmetics.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string ownedCosmeticID = enumerator.Current;
						MPArmoryCosmeticCategoryBaseVM activeCategory2 = this.ActiveCategory;
						MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM = ((activeCategory2 != null) ? activeCategory2.AvailableCosmetics.FirstOrDefault<MPArmoryCosmeticItemBaseVM>((MPArmoryCosmeticItemBaseVM c) => c.CosmeticID == ownedCosmeticID) : null);
						if (mparmoryCosmeticItemBaseVM != null)
						{
							mparmoryCosmeticItemBaseVM.IsUnlocked = true;
						}
					}
				}
			}
			this.RefreshFilters();
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000243C4 File Offset: 0x000225C4
		private void EquipItemOnHeroPreview(MPArmoryCosmeticItemBaseVM itemVM)
		{
			if (itemVM == null)
			{
				Debug.FailedAssert("Previewing null item", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "EquipItemOnHeroPreview", 529);
				return;
			}
			Action<MPArmoryCosmeticItemBaseVM> onCosmeticPreview = MPArmoryCosmeticsVM.OnCosmeticPreview;
			if (onCosmeticPreview == null)
			{
				return;
			}
			onCosmeticPreview(itemVM);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x000243F3 File Offset: 0x000225F3
		private void OnCosmeticEquipRequested(MPArmoryCosmeticItemBaseVM cosmeticItemVM)
		{
			if (cosmeticItemVM.CosmeticType == CosmeticsManager.CosmeticType.Clothing)
			{
				this.OnItemEquipRequested((MPArmoryCosmeticClothingItemVM)cosmeticItemVM);
				return;
			}
			if (cosmeticItemVM.CosmeticType == CosmeticsManager.CosmeticType.Taunt)
			{
				this.OnTauntEquipRequested((MPArmoryCosmeticTauntItemVM)cosmeticItemVM);
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00024420 File Offset: 0x00022620
		private void OnItemEquipRequested(MPArmoryCosmeticClothingItemVM itemVM)
		{
			if (itemVM.IsUsed && !itemVM.Cosmetic.IsFree && this.ActiveCategory != null && this.ActiveCategory.CosmeticType == CosmeticsManager.CosmeticType.Clothing && itemVM.ClothingCosmeticElement.ReplaceItemsId.Count > 0 && this._selectedClassDefaultEquipment != null)
			{
				for (int i = 0; i < itemVM.ClothingCosmeticElement.ReplaceItemsId.Count; i++)
				{
					string replacedItemId = itemVM.ClothingCosmeticElement.ReplaceItemsId[i];
					Func<MPArmoryCosmeticItemBaseVM, bool> <>9__1;
					for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
					{
						ItemObject item = this._selectedClassDefaultEquipment[equipmentIndex].Item;
						if (((item != null) ? item.StringId : null) == replacedItemId)
						{
							IEnumerable<MPArmoryCosmeticItemBaseVM> availableCosmetics = this.ActiveCategory.AvailableCosmetics;
							Func<MPArmoryCosmeticItemBaseVM, bool> func;
							if ((func = <>9__1) == null)
							{
								func = (<>9__1 = (MPArmoryCosmeticItemBaseVM c) => c.Cosmetic.Id == replacedItemId);
							}
							MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM = availableCosmetics.FirstOrDefault<MPArmoryCosmeticItemBaseVM>(func) as MPArmoryCosmeticClothingItemVM;
							if (mparmoryCosmeticClothingItemVM != null)
							{
								this.OnClothingItemEquipped(mparmoryCosmeticClothingItemVM, true);
								this._isLocalCosmeticsDirty = true;
								return;
							}
						}
					}
				}
			}
			if (itemVM.ClothingCosmeticElement.ReplaceItemless.Any<Tuple<string, string>>((Tuple<string, string> r) => r.Item1 == this._selectedClass.StringId))
			{
				if (itemVM.IsUsed)
				{
					itemVM.IsUsed = false;
					Dictionary<string, List<string>> usedCosmetics = this._usedCosmetics;
					if (usedCosmetics != null)
					{
						usedCosmetics[this._selectedTroopID].Remove(itemVM.CosmeticID);
					}
					Action<MPArmoryCosmeticItemBaseVM> onRemoveCosmeticFromPreview = MPArmoryCosmeticsVM.OnRemoveCosmeticFromPreview;
					if (onRemoveCosmeticFromPreview != null)
					{
						onRemoveCosmeticFromPreview(itemVM);
					}
				}
				else
				{
					itemVM.ActionText = itemVM.UnequipText;
					this.OnClothingItemEquipped(itemVM, true);
				}
			}
			else
			{
				this.OnClothingItemEquipped(itemVM, true);
			}
			this._isLocalCosmeticsDirty = true;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x000245D4 File Offset: 0x000227D4
		private void OnClothingItemEquipped(MPArmoryCosmeticClothingItemVM itemVM, bool forceRemove = true)
		{
			this.EquipItemOnHeroPreview(itemVM);
			if (!this._usedCosmetics.ContainsKey(this._selectedTroopID))
			{
				this._usedCosmetics.Add(this._selectedTroopID, new List<string>());
			}
			if (itemVM.CosmeticID != string.Empty && !this._usedCosmetics[this._selectedTroopID].Contains(itemVM.CosmeticID))
			{
				this._usedCosmetics[this._selectedTroopID].Add(itemVM.CosmeticID);
			}
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in this.ActiveCategory.AvailableCosmetics)
			{
				if (((MPArmoryCosmeticClothingItemVM)mparmoryCosmeticItemBaseVM).EquipmentElement.Item.ItemType == itemVM.EquipmentElement.Item.ItemType)
				{
					mparmoryCosmeticItemBaseVM.IsUsed = false;
					if (itemVM.Cosmetic.Id != mparmoryCosmeticItemBaseVM.Cosmetic.Id && forceRemove)
					{
						List<string> list = this._usedCosmetics[this._selectedTroopID];
						if (list != null)
						{
							list.Remove(mparmoryCosmeticItemBaseVM.CosmeticID);
						}
					}
				}
			}
			itemVM.IsUsed = true;
			if (this.ActiveCategory != null)
			{
				this.UpdateKeyBindingsForCategory(this.ActiveCategory);
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00024730 File Offset: 0x00022930
		public void ClearTauntSelections()
		{
			if (this.SelectedTauntItem == null && this.SelectedTauntSlot == null)
			{
				return;
			}
			this.OnTauntEquipRequested(null);
			this.OnTauntSlotSelected(null);
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.TauntSlots)
			{
				mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts = false;
				mparmoryCosmeticTauntSlotVM.IsFocused = false;
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x000247A4 File Offset: 0x000229A4
		private void OnTauntEquipRequested(MPArmoryCosmeticTauntItemVM tauntItem)
		{
			if (this.SelectedTauntItem != null)
			{
				if (this.SelectedTauntItem == tauntItem)
				{
					this.ClearTauntSelections();
					return;
				}
				this.SelectedTauntItem.IsSelected = false;
			}
			this.SelectedTauntItem = tauntItem;
			if (this.SelectedTauntItem != null)
			{
				MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM = null;
				for (int i = 0; i < this.TauntSlots.Count; i++)
				{
					MPArmoryCosmeticTauntItemVM assignedTauntItem = this.TauntSlots[i].AssignedTauntItem;
					if (((assignedTauntItem != null) ? assignedTauntItem.CosmeticID : null) == tauntItem.CosmeticID)
					{
						mparmoryCosmeticTauntSlotVM = this.TauntSlots[i];
						break;
					}
				}
				if (mparmoryCosmeticTauntSlotVM != null)
				{
					this.SelectedTauntItem = null;
					mparmoryCosmeticTauntSlotVM.AssignTauntItem(null, false);
					this.ClearTauntSelections();
					this._isLocalCosmeticsDirty = true;
					return;
				}
				this.SelectedTauntItem.IsSelected = true;
				this.SelectedTauntItem.ActionText = this.SelectedTauntItem.CancelEquipText;
				using (IEnumerator<MPArmoryCosmeticItemBaseVM> enumerator = this.ActiveCategory.AvailableCosmetics.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM = enumerator.Current;
						mparmoryCosmeticItemBaseVM.IsSelectable = mparmoryCosmeticItemBaseVM == this.SelectedTauntItem;
					}
					goto IL_0137;
				}
			}
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM2 in this.ActiveCategory.AvailableCosmetics)
			{
				mparmoryCosmeticItemBaseVM2.IsSelectable = true;
			}
			IL_0137:
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM2 in this.TauntSlots)
			{
				mparmoryCosmeticTauntSlotVM2.IsAcceptingTaunts = mparmoryCosmeticTauntSlotVM2.AssignedTauntItem != tauntItem;
			}
			Action onTauntAssignmentRefresh = MPArmoryCosmeticsVM.OnTauntAssignmentRefresh;
			if (onTauntAssignmentRefresh == null)
			{
				return;
			}
			onTauntAssignmentRefresh();
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00024958 File Offset: 0x00022B58
		private void OnTauntSlotFocusChanged(MPArmoryCosmeticTauntSlotVM changedSlot, bool isFocused)
		{
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.TauntSlots)
			{
				mparmoryCosmeticTauntSlotVM.IsFocused = isFocused && changedSlot == mparmoryCosmeticTauntSlotVM;
				if (mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts)
				{
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(new bool?(false));
					mparmoryCosmeticTauntSlotVM.SelectKeyVisual.SetForcedVisibility(null);
				}
				else if (mparmoryCosmeticTauntSlotVM.AssignedTauntItem != null)
				{
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(null);
					mparmoryCosmeticTauntSlotVM.SelectKeyVisual.SetForcedVisibility(new bool?(false));
				}
				else
				{
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(new bool?(false));
				}
				bool? flag = ((!mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts && mparmoryCosmeticTauntSlotVM.AssignedTauntItem != null) ? null : new bool?(false));
				bool? flag2 = ((mparmoryCosmeticTauntSlotVM.AssignedTauntItem != null || mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts) ? null : new bool?(false));
				InputKeyItemVM emptySlotKeyVisual = mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual;
				if (emptySlotKeyVisual != null)
				{
					emptySlotKeyVisual.SetForcedVisibility(flag);
				}
				InputKeyItemVM selectKeyVisual = mparmoryCosmeticTauntSlotVM.SelectKeyVisual;
				if (selectKeyVisual != null)
				{
					selectKeyVisual.SetForcedVisibility(flag2);
				}
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00024A9C File Offset: 0x00022C9C
		private void OnTauntSlotPreview(MPArmoryCosmeticTauntSlotVM previewSlot)
		{
			if (previewSlot != null)
			{
				MPArmoryCosmeticTauntItemVM assignedTauntItem = previewSlot.AssignedTauntItem;
				if (assignedTauntItem == null)
				{
					return;
				}
				assignedTauntItem.ExecutePreview();
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00024AB4 File Offset: 0x00022CB4
		private void OnTauntSlotSelected(MPArmoryCosmeticTauntSlotVM selectedSlot)
		{
			if (this.SelectedTauntSlot == null && this.SelectedTauntItem == null && selectedSlot != null && selectedSlot.IsEmpty)
			{
				return;
			}
			MPArmoryCosmeticTauntSlotVM selectedTauntSlot = this.SelectedTauntSlot;
			this.SelectedTauntSlot = selectedSlot;
			if (selectedTauntSlot != null)
			{
				selectedTauntSlot.IsSelected = false;
			}
			if (this.SelectedTauntSlot != null)
			{
				this.SelectedTauntSlot.IsSelected = true;
			}
			if (((selectedSlot != null) ? selectedSlot.AssignedTauntItem : null) != null)
			{
				bool flag = false;
				for (int i = 0; i < this.ActiveCategory.AvailableCosmetics.Count; i++)
				{
					if (this.ActiveCategory.AvailableCosmetics[i] == selectedSlot.AssignedTauntItem)
					{
						flag = true;
						break;
					}
				}
				MPArmoryTauntCosmeticCategoryVM mparmoryTauntCosmeticCategoryVM;
				if (!flag && this._tauntCategoriesLookup.TryGetValue(MPArmoryCosmeticsVM.TauntCategoryFlag.All, out mparmoryTauntCosmeticCategoryVM))
				{
					this.FilterTauntsByCategory(mparmoryTauntCosmeticCategoryVM);
				}
			}
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in this.ActiveCategory.AvailableCosmetics)
			{
				mparmoryCosmeticItemBaseVM.IsSelectable = selectedSlot == null || mparmoryCosmeticItemBaseVM == ((selectedSlot != null) ? selectedSlot.AssignedTauntItem : null);
			}
			if (this.SelectedTauntItem == null)
			{
				MPArmoryCosmeticTauntSlotVM selectedTauntSlot2 = this.SelectedTauntSlot;
				if (selectedTauntSlot2 != null && !selectedTauntSlot2.IsEmpty)
				{
					foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.TauntSlots)
					{
						mparmoryCosmeticTauntSlotVM.IsAcceptingTaunts = mparmoryCosmeticTauntSlotVM != selectedSlot;
					}
				}
			}
			if (this.SelectedTauntSlot != null)
			{
				bool flag2 = false;
				if (this.SelectedTauntItem != null && this.SelectedTauntSlot.AssignedTauntItem != this.SelectedTauntItem)
				{
					MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM2 = null;
					for (int j = 0; j < this.TauntSlots.Count; j++)
					{
						if (this.TauntSlots[j].AssignedTauntItem == this.SelectedTauntItem)
						{
							mparmoryCosmeticTauntSlotVM2 = this.TauntSlots[j];
							break;
						}
					}
					if (mparmoryCosmeticTauntSlotVM2 != null)
					{
						MPArmoryCosmeticTauntItemVM assignedTauntItem = this.SelectedTauntSlot.AssignedTauntItem;
						MPArmoryCosmeticTauntItemVM assignedTauntItem2 = mparmoryCosmeticTauntSlotVM2.AssignedTauntItem;
						this.SelectedTauntSlot.AssignTauntItem(assignedTauntItem2, true);
						mparmoryCosmeticTauntSlotVM2.AssignTauntItem(assignedTauntItem, true);
					}
					else
					{
						this.SelectedTauntSlot.AssignTauntItem(this.SelectedTauntItem, false);
					}
					flag2 = true;
					this.ClearTauntSelections();
				}
				else if (selectedTauntSlot != null && !selectedTauntSlot.IsEmpty && this.SelectedTauntSlot != selectedTauntSlot)
				{
					MPArmoryCosmeticTauntItemVM assignedTauntItem3 = selectedTauntSlot.AssignedTauntItem;
					MPArmoryCosmeticTauntItemVM assignedTauntItem4 = this.SelectedTauntSlot.AssignedTauntItem;
					this.SelectedTauntSlot.AssignTauntItem(assignedTauntItem3, true);
					selectedTauntSlot.AssignTauntItem(assignedTauntItem4, true);
					flag2 = true;
					this.ClearTauntSelections();
				}
				if (flag2)
				{
					this._isLocalCosmeticsDirty = true;
				}
			}
			Action onTauntAssignmentRefresh = MPArmoryCosmeticsVM.OnTauntAssignmentRefresh;
			if (onTauntAssignmentRefresh == null)
			{
				return;
			}
			onTauntAssignmentRefresh();
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00024D5C File Offset: 0x00022F5C
		private void OnTauntItemEquipped(MPArmoryCosmeticTauntSlotVM equippedSlot, MPArmoryCosmeticTauntItemVM previousTauntItem, bool isSwapping)
		{
			for (int i = 0; i < this.TauntSlots.Count; i++)
			{
				MPArmoryCosmeticTauntItemVM assignedTauntItem = this.TauntSlots[i].AssignedTauntItem;
				if (assignedTauntItem != null && !assignedTauntItem.IsUnlocked)
				{
					Debug.FailedAssert("Assigned a taunt without ownership: " + assignedTauntItem.TauntID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "OnTauntItemEquipped", 872);
				}
			}
			this._isLocalCosmeticsDirty = true;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00024DC7 File Offset: 0x00022FC7
		public void OnItemObtained(string cosmeticID, int finalLoot)
		{
			this._ownedCosmetics.Add(cosmeticID);
			this.RefreshCosmeticInfoFromNetwork();
			this.Loot = finalLoot;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00024DE4 File Offset: 0x00022FE4
		private void OnSortCategoryUpdated(SelectorVM<SelectorItemVM> selector)
		{
			if (this.SortCategories.SelectedIndex == -1)
			{
				this.SortCategories.SelectedIndex = 0;
			}
			this._currentItemComparer = this._itemComparers[selector.SelectedIndex];
			MPArmoryCosmeticCategoryBaseVM activeCategory = this.ActiveCategory;
			if (activeCategory == null)
			{
				return;
			}
			activeCategory.Sort(this._currentItemComparer);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00024E38 File Offset: 0x00023038
		private void OnSortOrderUpdated(SelectorVM<SelectorItemVM> selector)
		{
			if (this.SortOrders.SelectedIndex == -1)
			{
				this.SortOrders.SelectedIndex = 0;
			}
			foreach (MPArmoryCosmeticsVM.CosmeticItemComparer cosmeticItemComparer in this._itemComparers)
			{
				cosmeticItemComparer.SetSortMode(selector.SelectedIndex == 0);
			}
			MPArmoryCosmeticCategoryBaseVM activeCategory = this.ActiveCategory;
			if (activeCategory == null)
			{
				return;
			}
			activeCategory.Sort(this._currentItemComparer);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00024EC4 File Offset: 0x000230C4
		private void RefreshFilters()
		{
			MPArmoryClothingCosmeticCategoryVM mparmoryClothingCosmeticCategoryVM;
			if (this._currentCosmeticType == CosmeticsManager.CosmeticType.Clothing && this._clothingCategoriesLookup.TryGetValue(this._currentClothingCategory, out mparmoryClothingCosmeticCategoryVM))
			{
				this.FilterClothingsByCategory(mparmoryClothingCosmeticCategoryVM);
				return;
			}
			MPArmoryTauntCosmeticCategoryVM mparmoryTauntCosmeticCategoryVM;
			if (this._currentCosmeticType == CosmeticsManager.CosmeticType.Taunt && this._tauntCategoriesLookup.TryGetValue(this._currentTauntCategory, out mparmoryTauntCosmeticCategoryVM))
			{
				this.FilterTauntsByCategory(mparmoryTauntCosmeticCategoryVM);
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00024F1C File Offset: 0x0002311C
		private void FilterClothingsByCategory(MPArmoryClothingCosmeticCategoryVM clothingCategory)
		{
			if (this._currentCosmeticType != CosmeticsManager.CosmeticType.Clothing)
			{
				this.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Clothing);
				return;
			}
			if (clothingCategory == null)
			{
				Debug.FailedAssert("Trying to filter by null clothing category", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "FilterClothingsByCategory", 935);
				return;
			}
			this._currentClothingCategory = clothingCategory.ClothingCategory;
			foreach (KeyValuePair<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM> keyValuePair in this._clothingCategoriesLookup)
			{
				keyValuePair.Value.IsSelected = false;
			}
			clothingCategory.SetDefaultEquipments(this._selectedClassDefaultEquipment);
			this.ActiveCategory = clothingCategory;
			if (this._selectedClass != null)
			{
				foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in this._allCosmetics)
				{
					if (mparmoryCosmeticItemBaseVM.CosmeticType == CosmeticsManager.CosmeticType.Clothing)
					{
						clothingCategory.ReplaceCosmeticWithDefaultItem((MPArmoryCosmeticClothingItemVM)mparmoryCosmeticItemBaseVM, clothingCategory.ClothingCategory, this._selectedClass, this._ownedCosmetics);
					}
				}
			}
			this.ActiveCategory.Sort(this._currentItemComparer);
			this.RefreshEquipment();
			if (this.ActiveCategory != null)
			{
				this.ActiveCategory.IsSelected = true;
				this.UpdateKeyBindingsForCategory(this.ActiveCategory);
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00025064 File Offset: 0x00023264
		private void FilterTauntsByCategory(MPArmoryTauntCosmeticCategoryVM tauntCategory)
		{
			if (this._currentCosmeticType != CosmeticsManager.CosmeticType.Taunt)
			{
				this.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Taunt);
			}
			this._currentTauntCategory = tauntCategory.TauntCategory;
			foreach (KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> keyValuePair in this._tauntCategoriesLookup)
			{
				keyValuePair.Value.IsSelected = false;
			}
			this.ActiveCategory = tauntCategory;
			if (this.ActiveCategory != null)
			{
				this.ActiveCategory.IsSelected = true;
				this.UpdateKeyBindingsForCategory(this.ActiveCategory);
			}
			this.ActiveCategory.Sort(this._currentItemComparer);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00025114 File Offset: 0x00023314
		private void RefreshEquipment()
		{
			Dictionary<EquipmentIndex, bool> dictionary = new Dictionary<EquipmentIndex, bool>();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
			{
				dictionary.Add(equipmentIndex, false);
			}
			List<EquipmentElement> list = new List<EquipmentElement>();
			using (IEnumerator<MPArmoryCosmeticItemBaseVM> enumerator = this.ActiveCategory.AvailableCosmetics.Where<MPArmoryCosmeticItemBaseVM>((MPArmoryCosmeticItemBaseVM c) => c.Cosmetic.Rarity == CosmeticsManager.CosmeticRarity.Default).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					if ((mparmoryCosmeticClothingItemVM = enumerator.Current as MPArmoryCosmeticClothingItemVM) != null)
					{
						this.OnClothingItemEquipped(mparmoryCosmeticClothingItemVM, false);
						dictionary[mparmoryCosmeticClothingItemVM.EquipmentElement.Item.GetCosmeticEquipmentIndex()] = true;
						list.Add(mparmoryCosmeticClothingItemVM.EquipmentElement);
					}
				}
			}
			if (!string.IsNullOrEmpty(this._selectedTroopID))
			{
				Dictionary<string, List<string>> usedCosmetics = this._usedCosmetics;
				if (usedCosmetics != null && usedCosmetics.ContainsKey(this._selectedTroopID))
				{
					Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
					foreach (string text in this._usedCosmetics.Keys)
					{
						List<string> list2 = new List<string>();
						foreach (string text2 in this._usedCosmetics[text])
						{
							list2.Add(text2);
						}
						dictionary2.Add(text, list2);
					}
					using (List<string>.Enumerator enumerator3 = dictionary2[this._selectedTroopID].GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string cosmeticID = enumerator3.Current;
							MPArmoryCosmeticClothingItemVM cosmeticItem = (MPArmoryCosmeticClothingItemVM)this._allCosmetics.First<MPArmoryCosmeticItemBaseVM>((MPArmoryCosmeticItemBaseVM c) => c.CosmeticID == cosmeticID);
							if (cosmeticItem != null)
							{
								EquipmentIndex cosmeticEquipmentIndex = cosmeticItem.EquipmentElement.Item.GetCosmeticEquipmentIndex();
								if (!(cosmeticItem.Cosmetic as ClothingCosmeticElement).ReplaceItemless.IsEmpty<Tuple<string, string>>() || !this._selectedClassDefaultEquipment[cosmeticEquipmentIndex].IsEmpty)
								{
									EquipmentElement equipmentElement = list.FirstOrDefault<EquipmentElement>((EquipmentElement i) => i.Item.GetCosmeticEquipmentIndex() == cosmeticItem.EquipmentElement.Item.GetCosmeticEquipmentIndex());
									if (!equipmentElement.IsEmpty)
									{
										list.Remove(equipmentElement);
										list.Add(cosmeticItem.EquipmentElement);
									}
									this.OnClothingItemEquipped(cosmeticItem, true);
									dictionary[cosmeticEquipmentIndex] = true;
								}
							}
						}
					}
				}
			}
			foreach (EquipmentIndex equipmentIndex2 in dictionary.Keys)
			{
				if (!dictionary[equipmentIndex2])
				{
					MPArmoryClothingCosmeticCategoryVM mparmoryClothingCosmeticCategoryVM = (MPArmoryClothingCosmeticCategoryVM)this.ActiveCategory;
					if (mparmoryClothingCosmeticCategoryVM != null)
					{
						mparmoryClothingCosmeticCategoryVM.OnEquipmentRefreshed(equipmentIndex2);
					}
				}
			}
			Action<List<EquipmentElement>> onEquipmentRefreshed = MPArmoryCosmeticsVM.OnEquipmentRefreshed;
			if (onEquipmentRefreshed == null)
			{
				return;
			}
			onEquipmentRefreshed(list);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00025490 File Offset: 0x00023690
		private void RefreshTaunts(string playerId, MBReadOnlyList<TauntIndexData> registeredTaunts)
		{
			List<TauntIndexData> list = ((registeredTaunts != null) ? registeredTaunts.ToList<TauntIndexData>() : null);
			if (list == null)
			{
				list = new List<TauntIndexData>();
				foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in (from c in this._tauntCategoriesLookup.SelectMany<KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM>, MPArmoryCosmeticItemBaseVM>((KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> c) => c.Value.AvailableCosmetics)
					where c.Cosmetic.IsFree
					select c).Distinct<MPArmoryCosmeticItemBaseVM>())
				{
					TauntIndexData tauntIndexData = new TauntIndexData(mparmoryCosmeticItemBaseVM.CosmeticID, list.Count);
					list.Add(tauntIndexData);
				}
			}
			foreach (KeyValuePair<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> keyValuePair in this._tauntCategoriesLookup)
			{
				foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM2 in keyValuePair.Value.AvailableCosmetics)
				{
					mparmoryCosmeticItemBaseVM2.IsUnlocked = mparmoryCosmeticItemBaseVM2.Cosmetic.IsFree || this._ownedCosmetics.Contains(mparmoryCosmeticItemBaseVM2.CosmeticID);
				}
			}
			for (int i = 0; i < this.TauntSlots.Count; i++)
			{
				this.TauntSlots[i].AssignTauntItem(null, false);
			}
			for (int j = 0; j < list.Count; j++)
			{
				string tauntId = list[j].TauntId;
				int tauntIndex = list[j].TauntIndex;
				MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM3;
				MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
				if (this._cosmeticItemsLookup.TryGetValue(tauntId, out mparmoryCosmeticItemBaseVM3) && (mparmoryCosmeticTauntItemVM = mparmoryCosmeticItemBaseVM3 as MPArmoryCosmeticTauntItemVM) != null)
				{
					if (!mparmoryCosmeticTauntItemVM.IsUnlocked)
					{
						Debug.FailedAssert("Trying to add non-owned cosmetic to taunt slot: " + mparmoryCosmeticTauntItemVM.TauntID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Armory\\MPArmoryCosmeticsVM.cs", "RefreshTaunts", 1113);
					}
					else if (tauntIndex >= 0 && tauntIndex < this.TauntSlots.Count)
					{
						this.TauntSlots[tauntIndex].AssignTauntItem(mparmoryCosmeticTauntItemVM, false);
					}
				}
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000256E0 File Offset: 0x000238E0
		private void UpdateTauntAssignmentState()
		{
			this.IsTauntAssignmentActive = this.SelectedTauntItem != null || this.SelectedTauntSlot != null;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000256FC File Offset: 0x000238FC
		private void ExecuteRefreshCosmeticInfo()
		{
			this.RefreshCosmeticInfoFromNetwork();
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00025704 File Offset: 0x00023904
		private void ExecuteResetPreview()
		{
			this.RefreshSelectedClass(this._selectedClass, this._getSelectedPerks());
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00025720 File Offset: 0x00023920
		public void RefreshKeyBindings(HotKey actionKey, HotKey previewKey)
		{
			this.ActionInputKey = InputKeyItemVM.CreateFromHotKey(actionKey, false);
			this.PreviewInputKey = InputKeyItemVM.CreateFromHotKey(previewKey, false);
			for (int i = 0; i < this.AvailableCategories.Count; i++)
			{
				this.UpdateKeyBindingsForCategory(this.AvailableCategories[i]);
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00025770 File Offset: 0x00023970
		private void UpdateKeyBindingsForCategory(MPArmoryCosmeticCategoryBaseVM categoryVM)
		{
			if (this.ActionInputKey != null && this.PreviewInputKey != null)
			{
				for (int i = 0; i < categoryVM.AvailableCosmetics.Count; i++)
				{
					categoryVM.AvailableCosmetics[i].RefreshKeyBindings(this.ActionInputKey.HotKey, this.PreviewInputKey.HotKey);
				}
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x000257CA File Offset: 0x000239CA
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x000257D2 File Offset: 0x000239D2
		[DataSourceProperty]
		public InputKeyItemVM ActionInputKey
		{
			get
			{
				return this._actionInputKey;
			}
			set
			{
				if (value != this._actionInputKey)
				{
					this._actionInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ActionInputKey");
				}
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x000257F0 File Offset: 0x000239F0
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x000257F8 File Offset: 0x000239F8
		[DataSourceProperty]
		public InputKeyItemVM PreviewInputKey
		{
			get
			{
				return this._previewInputKey;
			}
			set
			{
				if (value != this._previewInputKey)
				{
					this._previewInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviewInputKey");
				}
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00025816 File Offset: 0x00023A16
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x0002581E File Offset: 0x00023A1E
		[DataSourceProperty]
		public int Loot
		{
			get
			{
				return this._loot;
			}
			set
			{
				if (value != this._loot)
				{
					this._loot = value;
					base.OnPropertyChangedWithValue(value, "Loot");
				}
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0002583C File Offset: 0x00023A3C
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x00025844 File Offset: 0x00023A44
		[DataSourceProperty]
		public bool IsLoading
		{
			get
			{
				return this._isLoading;
			}
			set
			{
				if (value != this._isLoading)
				{
					this._isLoading = value;
					base.OnPropertyChangedWithValue(value, "IsLoading");
				}
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x00025862 File Offset: 0x00023A62
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x0002586A File Offset: 0x00023A6A
		[DataSourceProperty]
		public bool HasCosmeticInfoReceived
		{
			get
			{
				return this._hasCosmeticInfoReceived;
			}
			set
			{
				if (value != this._hasCosmeticInfoReceived)
				{
					this._hasCosmeticInfoReceived = value;
					base.OnPropertyChangedWithValue(value, "HasCosmeticInfoReceived");
				}
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00025888 File Offset: 0x00023A88
		// (set) Token: 0x06000C0C RID: 3084 RVA: 0x00025890 File Offset: 0x00023A90
		[DataSourceProperty]
		public bool IsManagingTaunts
		{
			get
			{
				return this._isManagingTaunts;
			}
			set
			{
				if (value != this._isManagingTaunts)
				{
					this._isManagingTaunts = value;
					base.OnPropertyChangedWithValue(value, "IsManagingTaunts");
				}
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x000258AE File Offset: 0x00023AAE
		// (set) Token: 0x06000C0E RID: 3086 RVA: 0x000258B6 File Offset: 0x00023AB6
		[DataSourceProperty]
		public bool IsTauntAssignmentActive
		{
			get
			{
				return this._isTauntAssignmentActive;
			}
			set
			{
				if (value != this._isTauntAssignmentActive)
				{
					this._isTauntAssignmentActive = value;
					base.OnPropertyChangedWithValue(value, "IsTauntAssignmentActive");
				}
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000C0F RID: 3087 RVA: 0x000258D4 File Offset: 0x00023AD4
		// (set) Token: 0x06000C10 RID: 3088 RVA: 0x000258DC File Offset: 0x00023ADC
		[DataSourceProperty]
		public string CosmeticInfoErrorText
		{
			get
			{
				return this._cosmeticInfoErrorText;
			}
			set
			{
				if (value != this._cosmeticInfoErrorText)
				{
					this._cosmeticInfoErrorText = value;
					base.OnPropertyChangedWithValue<string>(value, "CosmeticInfoErrorText");
				}
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x000258FF File Offset: 0x00023AFF
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00025907 File Offset: 0x00023B07
		[DataSourceProperty]
		public HintViewModel AllCategoriesHint
		{
			get
			{
				return this._allCategoriesHint;
			}
			set
			{
				if (value != this._allCategoriesHint)
				{
					this._allCategoriesHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AllCategoriesHint");
				}
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00025925 File Offset: 0x00023B25
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x0002592D File Offset: 0x00023B2D
		[DataSourceProperty]
		public HintViewModel BodyCategoryHint
		{
			get
			{
				return this._bodyCategoryHint;
			}
			set
			{
				if (value != this._bodyCategoryHint)
				{
					this._bodyCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BodyCategoryHint");
				}
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x0002594B File Offset: 0x00023B4B
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x00025953 File Offset: 0x00023B53
		[DataSourceProperty]
		public HintViewModel HeadCategoryHint
		{
			get
			{
				return this._headCategoryHint;
			}
			set
			{
				if (value != this._headCategoryHint)
				{
					this._headCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HeadCategoryHint");
				}
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00025971 File Offset: 0x00023B71
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x00025979 File Offset: 0x00023B79
		[DataSourceProperty]
		public HintViewModel ShoulderCategoryHint
		{
			get
			{
				return this._shoulderCategoryHint;
			}
			set
			{
				if (value != this._shoulderCategoryHint)
				{
					this._shoulderCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShoulderCategoryHint");
				}
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00025997 File Offset: 0x00023B97
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x0002599F File Offset: 0x00023B9F
		[DataSourceProperty]
		public HintViewModel HandCategoryHint
		{
			get
			{
				return this._handCategoryHint;
			}
			set
			{
				if (value != this._handCategoryHint)
				{
					this._handCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HandCategoryHint");
				}
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x000259BD File Offset: 0x00023BBD
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x000259C5 File Offset: 0x00023BC5
		[DataSourceProperty]
		public HintViewModel LegCategoryHint
		{
			get
			{
				return this._legCategoryHint;
			}
			set
			{
				if (value != this._legCategoryHint)
				{
					this._legCategoryHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LegCategoryHint");
				}
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x000259E3 File Offset: 0x00023BE3
		// (set) Token: 0x06000C1E RID: 3102 RVA: 0x000259EB File Offset: 0x00023BEB
		[DataSourceProperty]
		public HintViewModel ResetPreviewHint
		{
			get
			{
				return this._resetPreviewHint;
			}
			set
			{
				if (value != this._resetPreviewHint)
				{
					this._resetPreviewHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetPreviewHint");
				}
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00025A09 File Offset: 0x00023C09
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x00025A11 File Offset: 0x00023C11
		[DataSourceProperty]
		public MPArmoryCosmeticCategoryBaseVM ActiveCategory
		{
			get
			{
				return this._activeCategory;
			}
			set
			{
				if (value != this._activeCategory)
				{
					this._activeCategory = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticCategoryBaseVM>(value, "ActiveCategory");
				}
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00025A2F File Offset: 0x00023C2F
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x00025A37 File Offset: 0x00023C37
		[DataSourceProperty]
		public MPArmoryCosmeticTauntSlotVM SelectedTauntSlot
		{
			get
			{
				return this._selectedTauntSlot;
			}
			set
			{
				if (value != this._selectedTauntSlot)
				{
					this._selectedTauntSlot = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticTauntSlotVM>(value, "SelectedTauntSlot");
					this.UpdateTauntAssignmentState();
				}
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00025A5B File Offset: 0x00023C5B
		// (set) Token: 0x06000C24 RID: 3108 RVA: 0x00025A63 File Offset: 0x00023C63
		[DataSourceProperty]
		public MPArmoryCosmeticTauntItemVM SelectedTauntItem
		{
			get
			{
				return this._selectedTauntItem;
			}
			set
			{
				if (value != this._selectedTauntItem)
				{
					this._selectedTauntItem = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticTauntItemVM>(value, "SelectedTauntItem");
					this.UpdateTauntAssignmentState();
				}
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00025A87 File Offset: 0x00023C87
		// (set) Token: 0x06000C26 RID: 3110 RVA: 0x00025A8F File Offset: 0x00023C8F
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> SortCategories
		{
			get
			{
				return this._sortCategories;
			}
			set
			{
				if (value != this._sortCategories)
				{
					this._sortCategories = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "SortCategories");
				}
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00025AAD File Offset: 0x00023CAD
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x00025AB5 File Offset: 0x00023CB5
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> SortOrders
		{
			get
			{
				return this._sortOrders;
			}
			set
			{
				if (value != this._sortOrders)
				{
					this._sortOrders = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "SortOrders");
				}
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00025AD3 File Offset: 0x00023CD3
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x00025ADB File Offset: 0x00023CDB
		[DataSourceProperty]
		public MBBindingList<MPArmoryCosmeticTauntSlotVM> TauntSlots
		{
			get
			{
				return this._tauntSlots;
			}
			set
			{
				if (value != this._tauntSlots)
				{
					this._tauntSlots = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPArmoryCosmeticTauntSlotVM>>(value, "TauntSlots");
				}
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00025AF9 File Offset: 0x00023CF9
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x00025B01 File Offset: 0x00023D01
		[DataSourceProperty]
		public MBBindingList<MPArmoryCosmeticCategoryBaseVM> AvailableCategories
		{
			get
			{
				return this._availableCategories;
			}
			set
			{
				if (value != this._availableCategories)
				{
					this._availableCategories = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPArmoryCosmeticCategoryBaseVM>>(value, "AvailableCategories");
				}
			}
		}

		// Token: 0x04000561 RID: 1377
		private readonly Func<List<IReadOnlyPerkObject>> _getSelectedPerks;

		// Token: 0x04000562 RID: 1378
		private List<MPArmoryCosmeticItemBaseVM> _allCosmetics;

		// Token: 0x04000563 RID: 1379
		private List<string> _ownedCosmetics;

		// Token: 0x04000564 RID: 1380
		private Dictionary<string, List<string>> _usedCosmetics;

		// Token: 0x04000565 RID: 1381
		private Equipment _selectedClassDefaultEquipment;

		// Token: 0x04000566 RID: 1382
		private MPArmoryCosmeticsVM.CosmeticItemComparer _currentItemComparer;

		// Token: 0x04000567 RID: 1383
		private List<MPArmoryCosmeticsVM.CosmeticItemComparer> _itemComparers;

		// Token: 0x04000568 RID: 1384
		private Dictionary<MPArmoryCosmeticsVM.ClothingCategory, MPArmoryClothingCosmeticCategoryVM> _clothingCategoriesLookup;

		// Token: 0x04000569 RID: 1385
		private Dictionary<MPArmoryCosmeticsVM.TauntCategoryFlag, MPArmoryTauntCosmeticCategoryVM> _tauntCategoriesLookup;

		// Token: 0x0400056A RID: 1386
		private Dictionary<string, MPArmoryCosmeticItemBaseVM> _cosmeticItemsLookup;

		// Token: 0x0400056B RID: 1387
		private MultiplayerClassDivisions.MPHeroClass _selectedClass;

		// Token: 0x0400056C RID: 1388
		private string _selectedTroopID;

		// Token: 0x0400056D RID: 1389
		private bool _isLocalCosmeticsDirty;

		// Token: 0x0400056E RID: 1390
		private bool _isNetworkCosmeticsDirty;

		// Token: 0x0400056F RID: 1391
		private bool _isSendingCosmeticData;

		// Token: 0x04000570 RID: 1392
		private bool _isRetrievingCosmeticData;

		// Token: 0x04000571 RID: 1393
		private CosmeticsManager.CosmeticType _currentCosmeticType;

		// Token: 0x04000572 RID: 1394
		private MPArmoryCosmeticsVM.ClothingCategory _currentClothingCategory;

		// Token: 0x04000573 RID: 1395
		private MPArmoryCosmeticsVM.TauntCategoryFlag _currentTauntCategory;

		// Token: 0x04000574 RID: 1396
		private InputKeyItemVM _actionInputKey;

		// Token: 0x04000575 RID: 1397
		private InputKeyItemVM _previewInputKey;

		// Token: 0x04000576 RID: 1398
		private int _loot;

		// Token: 0x04000577 RID: 1399
		private bool _isLoading;

		// Token: 0x04000578 RID: 1400
		private bool _hasCosmeticInfoReceived;

		// Token: 0x04000579 RID: 1401
		private bool _isManagingTaunts;

		// Token: 0x0400057A RID: 1402
		private bool _isTauntAssignmentActive;

		// Token: 0x0400057B RID: 1403
		private string _cosmeticInfoErrorText;

		// Token: 0x0400057C RID: 1404
		private HintViewModel _allCategoriesHint;

		// Token: 0x0400057D RID: 1405
		private HintViewModel _bodyCategoryHint;

		// Token: 0x0400057E RID: 1406
		private HintViewModel _headCategoryHint;

		// Token: 0x0400057F RID: 1407
		private HintViewModel _shoulderCategoryHint;

		// Token: 0x04000580 RID: 1408
		private HintViewModel _handCategoryHint;

		// Token: 0x04000581 RID: 1409
		private HintViewModel _legCategoryHint;

		// Token: 0x04000582 RID: 1410
		private HintViewModel _resetPreviewHint;

		// Token: 0x04000583 RID: 1411
		private MPArmoryCosmeticCategoryBaseVM _activeCategory;

		// Token: 0x04000584 RID: 1412
		private MPArmoryCosmeticTauntSlotVM _selectedTauntSlot;

		// Token: 0x04000585 RID: 1413
		private MPArmoryCosmeticTauntItemVM _selectedTauntItem;

		// Token: 0x04000586 RID: 1414
		private SelectorVM<SelectorItemVM> _sortCategories;

		// Token: 0x04000587 RID: 1415
		private SelectorVM<SelectorItemVM> _sortOrders;

		// Token: 0x04000588 RID: 1416
		private MBBindingList<MPArmoryCosmeticTauntSlotVM> _tauntSlots;

		// Token: 0x04000589 RID: 1417
		private MBBindingList<MPArmoryCosmeticCategoryBaseVM> _availableCategories;

		// Token: 0x0200015D RID: 349
		public enum ClothingCategory
		{
			// Token: 0x040009CD RID: 2509
			Invalid = -1,
			// Token: 0x040009CE RID: 2510
			ClothingCategoriesBegin,
			// Token: 0x040009CF RID: 2511
			All = 0,
			// Token: 0x040009D0 RID: 2512
			HeadArmor,
			// Token: 0x040009D1 RID: 2513
			Cape,
			// Token: 0x040009D2 RID: 2514
			BodyArmor,
			// Token: 0x040009D3 RID: 2515
			HandArmor,
			// Token: 0x040009D4 RID: 2516
			LegArmor,
			// Token: 0x040009D5 RID: 2517
			ClothingCategoriesEnd
		}

		// Token: 0x0200015E RID: 350
		[Flags]
		public enum TauntCategoryFlag
		{
			// Token: 0x040009D7 RID: 2519
			None = 0,
			// Token: 0x040009D8 RID: 2520
			UsableWithMount = 1,
			// Token: 0x040009D9 RID: 2521
			UsableWithOneHanded = 2,
			// Token: 0x040009DA RID: 2522
			UsableWithTwoHanded = 4,
			// Token: 0x040009DB RID: 2523
			UsableWithBow = 8,
			// Token: 0x040009DC RID: 2524
			UsableWithCrossbow = 16,
			// Token: 0x040009DD RID: 2525
			UsableWithShield = 32,
			// Token: 0x040009DE RID: 2526
			All = 63
		}

		// Token: 0x0200015F RID: 351
		public abstract class CosmeticItemComparer : IComparer<MPArmoryCosmeticItemBaseVM>
		{
			// Token: 0x170005A8 RID: 1448
			// (get) Token: 0x06001272 RID: 4722 RVA: 0x0003A0EA File Offset: 0x000382EA
			protected int _sortMultiplier
			{
				get
				{
					if (!this._isAscending)
					{
						return -1;
					}
					return 1;
				}
			}

			// Token: 0x06001273 RID: 4723 RVA: 0x0003A0F7 File Offset: 0x000382F7
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06001274 RID: 4724
			public abstract int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y);

			// Token: 0x040009DF RID: 2527
			private bool _isAscending;
		}

		// Token: 0x02000160 RID: 352
		private class CosmeticItemNameComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x06001276 RID: 4726 RVA: 0x0003A108 File Offset: 0x00038308
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				return x.Name.CompareTo(y.Name) * base._sortMultiplier;
			}
		}

		// Token: 0x02000161 RID: 353
		private class CosmeticItemCostComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x06001278 RID: 4728 RVA: 0x0003A12C File Offset: 0x0003832C
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				int num = x.Cost.CompareTo(y.Cost);
				if (num == 0)
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM2;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM2;
					if ((mparmoryCosmeticClothingItemVM = x as MPArmoryCosmeticClothingItemVM) != null && (mparmoryCosmeticClothingItemVM2 = y as MPArmoryCosmeticClothingItemVM) != null)
					{
						num = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType.CompareTo(mparmoryCosmeticClothingItemVM2.EquipmentElement.Item.ItemType);
					}
					else if ((mparmoryCosmeticTauntItemVM = x as MPArmoryCosmeticTauntItemVM) != null && (mparmoryCosmeticTauntItemVM2 = y as MPArmoryCosmeticTauntItemVM) != null)
					{
						num = mparmoryCosmeticTauntItemVM.Name.CompareTo(mparmoryCosmeticTauntItemVM2.Name);
					}
				}
				return num * base._sortMultiplier;
			}
		}

		// Token: 0x02000162 RID: 354
		private class CosmeticItemRarityComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x0600127A RID: 4730 RVA: 0x0003A1E0 File Offset: 0x000383E0
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				int num = x.Cosmetic.Rarity.CompareTo(y.Cosmetic.Rarity);
				if (num == 0)
				{
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
					MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM2;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
					MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM2;
					if ((mparmoryCosmeticClothingItemVM = x as MPArmoryCosmeticClothingItemVM) != null && (mparmoryCosmeticClothingItemVM2 = y as MPArmoryCosmeticClothingItemVM) != null)
					{
						num = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType.CompareTo(mparmoryCosmeticClothingItemVM2.EquipmentElement.Item.ItemType);
					}
					else if ((mparmoryCosmeticTauntItemVM = x as MPArmoryCosmeticTauntItemVM) != null && (mparmoryCosmeticTauntItemVM2 = y as MPArmoryCosmeticTauntItemVM) != null)
					{
						num = mparmoryCosmeticTauntItemVM.Name.CompareTo(mparmoryCosmeticTauntItemVM2.Name);
					}
				}
				return num * base._sortMultiplier;
			}
		}

		// Token: 0x02000163 RID: 355
		private class CosmeticItemCategoryComparer : MPArmoryCosmeticsVM.CosmeticItemComparer
		{
			// Token: 0x0600127C RID: 4732 RVA: 0x0003A2A4 File Offset: 0x000384A4
			public override int Compare(MPArmoryCosmeticItemBaseVM x, MPArmoryCosmeticItemBaseVM y)
			{
				MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
				MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM2;
				if ((mparmoryCosmeticClothingItemVM = x as MPArmoryCosmeticClothingItemVM) != null && (mparmoryCosmeticClothingItemVM2 = y as MPArmoryCosmeticClothingItemVM) != null)
				{
					return mparmoryCosmeticClothingItemVM.EquipmentElement.Item.ItemType.CompareTo(mparmoryCosmeticClothingItemVM2.EquipmentElement.Item.ItemType) * base._sortMultiplier;
				}
				return 0;
			}
		}
	}
}
