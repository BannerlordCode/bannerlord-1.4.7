using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.Order;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010E RID: 270
	public class WeaponDesignVM : ViewModel
	{
		// Token: 0x0600181B RID: 6171 RVA: 0x0005BD60 File Offset: 0x00059F60
		public WeaponDesignVM(Crafting crafting, ICraftingCampaignBehavior craftingBehavior, Action onRefresh, Action onWeaponCrafted, Func<CraftingAvailableHeroItemVM> getCurrentCraftingHero, Action<CraftingOrder> refreshHeroAvailabilities, Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> getItemUsageSetFlags)
		{
			this._crafting = crafting;
			this._craftingBehavior = craftingBehavior;
			this._onRefresh = onRefresh;
			this._onWeaponCrafted = onWeaponCrafted;
			this._getCurrentCraftingHero = getCurrentCraftingHero;
			this._getItemUsageSetFlags = getItemUsageSetFlags;
			this._refreshHeroAvailabilities = refreshHeroAvailabilities;
			this.MaxDifficulty = 300;
			this._currentCraftingSkillText = new TextObject("{=LEiZWuZm}{SKILL_NAME}: {SKILL_VALUE}", null);
			this.PrimaryPropertyList = new MBBindingList<CraftingListPropertyItem>();
			this.DesignResultPropertyList = new MBBindingList<WeaponDesignResultPropertyItemVM>();
			this._newlyUnlockedPieces = new List<CraftingPiece>();
			this._pieceTierComparer = new WeaponDesignVM.PieceTierComparer();
			this.BladePieceList = new CraftingPieceListVM(new MBBindingList<CraftingPieceVM>(), CraftingPiece.PieceTypes.Blade, new Action<CraftingPiece.PieceTypes, bool>(this.OnSelectPieceType));
			this.GuardPieceList = new CraftingPieceListVM(new MBBindingList<CraftingPieceVM>(), CraftingPiece.PieceTypes.Guard, new Action<CraftingPiece.PieceTypes, bool>(this.OnSelectPieceType));
			this.HandlePieceList = new CraftingPieceListVM(new MBBindingList<CraftingPieceVM>(), CraftingPiece.PieceTypes.Handle, new Action<CraftingPiece.PieceTypes, bool>(this.OnSelectPieceType));
			this.PommelPieceList = new CraftingPieceListVM(new MBBindingList<CraftingPieceVM>(), CraftingPiece.PieceTypes.Pommel, new Action<CraftingPiece.PieceTypes, bool>(this.OnSelectPieceType));
			this.PieceLists = new MBBindingList<CraftingPieceListVM> { this.BladePieceList, this.GuardPieceList, this.HandlePieceList, this.PommelPieceList };
			this._pieceListsDictionary = new Dictionary<CraftingPiece.PieceTypes, CraftingPieceListVM>
			{
				{
					CraftingPiece.PieceTypes.Blade,
					this.BladePieceList
				},
				{
					CraftingPiece.PieceTypes.Guard,
					this.GuardPieceList
				},
				{
					CraftingPiece.PieceTypes.Handle,
					this.HandlePieceList
				},
				{
					CraftingPiece.PieceTypes.Pommel,
					this.PommelPieceList
				}
			};
			this._pieceVMs = new Dictionary<CraftingPiece, CraftingPieceVM>();
			this.TierFilters = new MBBindingList<TierFilterTypeVM>
			{
				new TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter.All, new Action<WeaponDesignVM.CraftingPieceTierFilter>(this.OnSelectPieceTierFilter), GameTexts.FindText("str_crafting_tier_filter_all", null).ToString()),
				new TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter.Tier1, new Action<WeaponDesignVM.CraftingPieceTierFilter>(this.OnSelectPieceTierFilter), GameTexts.FindText("str_tier_one", null).ToString()),
				new TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter.Tier2, new Action<WeaponDesignVM.CraftingPieceTierFilter>(this.OnSelectPieceTierFilter), GameTexts.FindText("str_tier_two", null).ToString()),
				new TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter.Tier3, new Action<WeaponDesignVM.CraftingPieceTierFilter>(this.OnSelectPieceTierFilter), GameTexts.FindText("str_tier_three", null).ToString()),
				new TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter.Tier4, new Action<WeaponDesignVM.CraftingPieceTierFilter>(this.OnSelectPieceTierFilter), GameTexts.FindText("str_tier_four", null).ToString()),
				new TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter.Tier5, new Action<WeaponDesignVM.CraftingPieceTierFilter>(this.OnSelectPieceTierFilter), GameTexts.FindText("str_tier_five", null).ToString())
			};
			this._templateComparer = new WeaponDesignVM.TemplateComparer();
			this._primaryUsages = CraftingTemplate.All.ToList<CraftingTemplate>();
			this._primaryUsages.Sort(this._templateComparer);
			this.SecondaryUsageSelector = new SelectorVM<CraftingSecondaryUsageItemVM>(new List<string>(), 0, null);
			this.CraftingOrderPopup = new CraftingOrderPopupVM(new Action<CraftingOrderItemVM>(this.OnCraftingOrderSelected), this._getCurrentCraftingHero, new Func<CraftingOrder, IEnumerable<CraftingStatData>>(this.GetOrderStatDatas));
			this.WeaponClassSelectionPopup = new WeaponClassSelectionPopupVM(this._craftingBehavior, this._primaryUsages, delegate(int x)
			{
				this.RefreshWeaponDesignMode(null, x, false);
			}, new Func<CraftingTemplate, int>(this.GetUnlockedPartsCount));
			this.WeaponFlagIconsList = new MBBindingList<ItemFlagVM>();
			this.CraftedItemVisual = new ItemCollectionElementViewModel();
			CampaignEvents.CraftingPartUnlockedEvent.AddNonSerializedListener(this, new Action<CraftingPiece>(this.OnNewPieceUnlocked));
			this.CraftingHistory = new CraftingHistoryVM(this._crafting, this._craftingBehavior, delegate
			{
				CraftingOrderItemVM activeCraftingOrder = this.ActiveCraftingOrder;
				if (activeCraftingOrder == null)
				{
					return null;
				}
				return activeCraftingOrder.CraftingOrder;
			}, new Action<WeaponDesignSelectorVM>(this.OnSelectItemFromHistory));
			this.RefreshWeaponDesignMode(null, -1, false);
			this._selectedWeaponClassIndex = this._primaryUsages.IndexOf(this._crafting.CurrentCraftingTemplate);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0005C130 File Offset: 0x0005A330
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ShowOnlyUnlockedPiecesHint = new HintViewModel(new TextObject("{=dOa7frHR}Show only unlocked pieces", null), null);
			this.ComponentSizeLbl = new TextObject("{=OkWLI5C8}Size:", null).ToString();
			this.AlternativeUsageText = new TextObject("{=13wo3QQB}Secondary", null).ToString();
			this.DefaultUsageText = new TextObject("{=ta4R2RR7}Primary", null).ToString();
			this.DifficultyText = GameTexts.FindText("str_difficulty", null).ToString();
			this.ScabbardHint = new HintViewModel(GameTexts.FindText("str_toggle_scabbard", null), null);
			this.RandomizeHint = new HintViewModel(GameTexts.FindText("str_randomize", null), null);
			this.UndoHint = new HintViewModel(GameTexts.FindText("str_undo", null), null);
			this.RedoHint = new HintViewModel(GameTexts.FindText("str_redo", null), null);
			this.DifficultyExplanationHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetSmithingDifficultyTooltip());
			this.OrderDisabledReasonHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetOrdersDisabledReasonTooltip(this.CraftingOrderPopup.CraftingOrders, this._getCurrentCraftingHero().Hero));
			this._primaryPropertyList.ApplyActionOnAllItems(delegate(CraftingListPropertyItem x)
			{
				x.RefreshValues();
			});
			CraftingPieceVM selectedBladePiece = this._selectedBladePiece;
			if (selectedBladePiece != null)
			{
				selectedBladePiece.RefreshValues();
			}
			CraftingPieceVM selectedGuardPiece = this._selectedGuardPiece;
			if (selectedGuardPiece != null)
			{
				selectedGuardPiece.RefreshValues();
			}
			CraftingPieceVM selectedHandlePiece = this._selectedHandlePiece;
			if (selectedHandlePiece != null)
			{
				selectedHandlePiece.RefreshValues();
			}
			CraftingPieceVM selectedPommelPiece = this._selectedPommelPiece;
			if (selectedPommelPiece != null)
			{
				selectedPommelPiece.RefreshValues();
			}
			this._secondaryUsageSelector.RefreshValues();
			this._craftingOrderPopup.RefreshValues();
			this.ChooseOrderText = this.CraftingOrderPopup.OrderCountText;
			this.ChooseWeaponTypeText = new TextObject("{=Gd6zuUwh}Free Build", null).ToString();
			this.CurrentCraftedWeaponTypeText = this._crafting.CurrentCraftingTemplate.TemplateName.ToString();
			this.CurrentCraftedWeaponTemplateId = this._crafting.CurrentCraftingTemplate.StringId;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0005C32C File Offset: 0x0005A52C
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.CraftingPartUnlockedEvent.ClearListeners(this);
			CraftingHistoryVM craftingHistory = this.CraftingHistory;
			if (craftingHistory != null)
			{
				craftingHistory.OnFinalize();
			}
			ItemCollectionElementViewModel craftedItemVisual = this.CraftedItemVisual;
			if (craftedItemVisual != null)
			{
				craftedItemVisual.OnFinalize();
			}
			WeaponDesignResultPopupVM craftingResultPopup = this.CraftingResultPopup;
			if (craftingResultPopup != null)
			{
				craftingResultPopup.OnFinalize();
			}
			this.CraftedItemVisual = null;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0005C384 File Offset: 0x0005A584
		internal void OnCraftingLogicRefreshed(Crafting newCraftingLogic)
		{
			this._crafting = newCraftingLogic;
			this.InitializeDefaultFromLogic();
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0005C394 File Offset: 0x0005A594
		private void FilterPieces(WeaponDesignVM.CraftingPieceTierFilter filter)
		{
			List<int> list = new List<int>();
			switch (filter)
			{
			case WeaponDesignVM.CraftingPieceTierFilter.None:
				goto IL_009B;
			case WeaponDesignVM.CraftingPieceTierFilter.Tier1:
				list.Add(1);
				goto IL_009B;
			case WeaponDesignVM.CraftingPieceTierFilter.Tier2:
				list.Add(2);
				goto IL_009B;
			case WeaponDesignVM.CraftingPieceTierFilter.Tier1 | WeaponDesignVM.CraftingPieceTierFilter.Tier2:
			case WeaponDesignVM.CraftingPieceTierFilter.Tier1 | WeaponDesignVM.CraftingPieceTierFilter.Tier3:
			case WeaponDesignVM.CraftingPieceTierFilter.Tier2 | WeaponDesignVM.CraftingPieceTierFilter.Tier3:
			case WeaponDesignVM.CraftingPieceTierFilter.Tier1 | WeaponDesignVM.CraftingPieceTierFilter.Tier2 | WeaponDesignVM.CraftingPieceTierFilter.Tier3:
				break;
			case WeaponDesignVM.CraftingPieceTierFilter.Tier3:
				list.Add(3);
				goto IL_009B;
			case WeaponDesignVM.CraftingPieceTierFilter.Tier4:
				list.Add(4);
				goto IL_009B;
			default:
				if (filter == WeaponDesignVM.CraftingPieceTierFilter.Tier5)
				{
					list.Add(5);
					goto IL_009B;
				}
				if (filter == WeaponDesignVM.CraftingPieceTierFilter.All)
				{
					list.AddRange(new int[] { 1, 2, 3, 4, 5 });
					goto IL_009B;
				}
				break;
			}
			Debug.FailedAssert("Invalid tier filter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Crafting\\WeaponDesign\\WeaponDesignVM.cs", "FilterPieces", 218);
			IL_009B:
			foreach (TierFilterTypeVM tierFilterTypeVM in this.TierFilters)
			{
				tierFilterTypeVM.IsSelected = filter.HasAllFlags(tierFilterTypeVM.FilterType);
			}
			foreach (CraftingPieceListVM craftingPieceListVM in this.PieceLists)
			{
				foreach (CraftingPieceVM craftingPieceVM in craftingPieceListVM.Pieces)
				{
					bool flag = list.Contains(craftingPieceVM.CraftingPiece.CraftingPiece.PieceTier);
					bool flag2 = this.ShowOnlyUnlockedPieces && !craftingPieceVM.PlayerHasPiece;
					craftingPieceVM.IsFilteredOut = !flag || flag2;
				}
			}
			this._currentTierFilter = filter;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0005C53C File Offset: 0x0005A73C
		private void OnNewPieceUnlocked(CraftingPiece piece)
		{
			if (piece.IsValid && !piece.IsHiddenOnDesigner)
			{
				this.SetPieceNewlyUnlocked(piece);
				CraftingPieceVM craftingPieceVM;
				if (this._pieceVMs.TryGetValue(piece, out craftingPieceVM))
				{
					craftingPieceVM.PlayerHasPiece = true;
					craftingPieceVM.IsNewlyUnlocked = true;
				}
			}
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0005C580 File Offset: 0x0005A780
		private int GetUnlockedPartsCount(CraftingTemplate template)
		{
			return template.Pieces.Count<CraftingPiece>((CraftingPiece piece) => this._craftingBehavior.IsOpened(piece, template) && !string.IsNullOrEmpty(piece.MeshName));
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0005C5BD File Offset: 0x0005A7BD
		private WeaponClassVM GetCurrentWeaponClass()
		{
			if (this._selectedWeaponClassIndex >= 0 && this._selectedWeaponClassIndex < this.WeaponClassSelectionPopup.WeaponClasses.Count)
			{
				return this.WeaponClassSelectionPopup.WeaponClasses[this._selectedWeaponClassIndex];
			}
			return null;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0005C5F8 File Offset: 0x0005A7F8
		private void OnSelectItemFromHistory(WeaponDesignSelectorVM selector)
		{
			WeaponDesign design = selector.Design;
			if (design == null)
			{
				Debug.FailedAssert("History design returned null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Crafting\\WeaponDesign\\WeaponDesignVM.cs", "OnSelectItemFromHistory", 284);
				return;
			}
			ValueTuple<CraftingPiece, int>[] array = new ValueTuple<CraftingPiece, int>[design.UsedPieces.Length];
			for (int i = 0; i < design.UsedPieces.Length; i++)
			{
				array[i] = new ValueTuple<CraftingPiece, int>(design.UsedPieces[i].CraftingPiece, design.UsedPieces[i].ScalePercentage);
			}
			this.SetDesignManually(design.Template, array, true);
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0005C684 File Offset: 0x0005A884
		public void SetPieceNewlyUnlocked(CraftingPiece piece)
		{
			if (!this._newlyUnlockedPieces.Contains(piece))
			{
				this._newlyUnlockedPieces.Add(piece);
			}
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0005C6A0 File Offset: 0x0005A8A0
		private void UnsetPieceNewlyUnlocked(CraftingPieceVM pieceVM)
		{
			CraftingPiece craftingPiece = pieceVM.CraftingPiece.CraftingPiece;
			if (this._newlyUnlockedPieces.Contains(craftingPiece))
			{
				this._newlyUnlockedPieces.Remove(craftingPiece);
				pieceVM.IsNewlyUnlocked = false;
			}
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0005C6DB File Offset: 0x0005A8DB
		private void OnSelectPieceTierFilter(WeaponDesignVM.CraftingPieceTierFilter filter)
		{
			if (this._currentTierFilter != filter)
			{
				this.FilterPieces(filter);
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0005C6F0 File Offset: 0x0005A8F0
		private void OnSelectPieceType(CraftingPiece.PieceTypes pieceType, bool fromClick = false)
		{
			CraftingPieceListVM craftingPieceListVM = this.PieceLists.ElementAt<CraftingPieceListVM>(this.SelectedPieceTypeIndex);
			if (craftingPieceListVM != null && fromClick)
			{
				foreach (CraftingPieceVM craftingPieceVM in craftingPieceListVM.Pieces)
				{
					if (craftingPieceVM.IsNewlyUnlocked)
					{
						this.UnsetPieceNewlyUnlocked(craftingPieceVM);
					}
				}
			}
			foreach (CraftingPieceListVM craftingPieceListVM2 in this.PieceLists)
			{
				craftingPieceListVM2.Refresh();
				if (craftingPieceListVM2.PieceType == pieceType)
				{
					craftingPieceListVM2.IsSelected = true;
					this.ActivePieceList = craftingPieceListVM2;
				}
				else
				{
					craftingPieceListVM2.IsSelected = false;
				}
			}
			this.SelectedPieceTypeIndex = (int)pieceType;
			base.OnPropertyChanged("ActivePieceSize");
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0005C7D0 File Offset: 0x0005A9D0
		private void SelectDefaultPiecesForCurrentTemplate()
		{
			CraftingOrderItemVM activeCraftingOrder = this.ActiveCraftingOrder;
			string text = ((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder.GetStatWeapon().WeaponDescriptionId : null);
			WeaponDescription statWeaponUsage = ((text != null) ? MBObjectManager.Instance.GetObject<WeaponDescription>(text) : null);
			WeaponClassVM currentWeaponClass = this.GetCurrentWeaponClass();
			this._shouldRecordHistory = false;
			this._isAutoSelectingPieces = true;
			Func<CraftingPieceVM, bool> <>9__3;
			foreach (CraftingPieceListVM craftingPieceListVM in this.PieceLists)
			{
				if (this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(craftingPieceListVM.PieceType))
				{
					CraftingPieceVM craftingPieceVM = null;
					if (this.IsInFreeMode && currentWeaponClass != null)
					{
						string selectedPieceID = currentWeaponClass.GetSelectedPieceData(craftingPieceListVM.PieceType);
						craftingPieceVM = craftingPieceListVM.Pieces.FirstOrDefault<CraftingPieceVM>((CraftingPieceVM p) => p.CraftingPiece.CraftingPiece.StringId == selectedPieceID);
					}
					if (craftingPieceVM == null)
					{
						IOrderedEnumerable<CraftingPieceVM> orderedEnumerable = from p in craftingPieceListVM.Pieces
							orderby p.PlayerHasPiece descending, !p.IsNewlyUnlocked descending
							select p;
						Func<CraftingPieceVM, bool> func;
						if ((func = <>9__3) == null)
						{
							func = (<>9__3 = (CraftingPieceVM p) => statWeaponUsage == null || statWeaponUsage.AvailablePieces.Any<CraftingPiece>((CraftingPiece x) => x.StringId == p.CraftingPiece.CraftingPiece.StringId));
						}
						craftingPieceVM = orderedEnumerable.ThenByDescending<CraftingPieceVM, bool>(func).FirstOrDefault<CraftingPieceVM>();
					}
					if (craftingPieceVM != null)
					{
						craftingPieceVM.ExecuteSelect();
					}
				}
			}
			this._shouldRecordHistory = true;
			this._isAutoSelectingPieces = false;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0005C974 File Offset: 0x0005AB74
		private void InitializeDefaultFromLogic()
		{
			this.PrimaryPropertyList.Clear();
			this.BladePieceList.Pieces.Clear();
			this.GuardPieceList.Pieces.Clear();
			this.HandlePieceList.Pieces.Clear();
			this.PommelPieceList.Pieces.Clear();
			this.SelectedBladePiece = new CraftingPieceVM();
			this.SelectedGuardPiece = new CraftingPieceVM();
			this.SelectedHandlePiece = new CraftingPieceVM();
			this.SelectedPommelPiece = new CraftingPieceVM();
			this._pieceVMs.Clear();
			bool flag = Campaign.Current.GameMode == CampaignGameMode.Tutorial;
			foreach (CraftingPieceListVM craftingPieceListVM in this.PieceLists)
			{
				if (this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(craftingPieceListVM.PieceType))
				{
					int pieceType = (int)craftingPieceListVM.PieceType;
					for (int i = 0; i < this._crafting.UsablePiecesList[pieceType].Count; i++)
					{
						WeaponDesignElement weaponDesignElement = this._crafting.UsablePiecesList[pieceType][i];
						if (flag || !weaponDesignElement.CraftingPiece.IsHiddenOnDesigner)
						{
							bool flag2 = this._craftingBehavior.IsOpened(weaponDesignElement.CraftingPiece, this._crafting.CurrentCraftingTemplate);
							CraftingPieceVM craftingPieceVM = new CraftingPieceVM(new Action<CraftingPieceVM>(this.OnSetItemPieceManually), this._crafting.CurrentCraftingTemplate.StringId, this._crafting.UsablePiecesList[pieceType][i], pieceType, i, flag2);
							craftingPieceListVM.Pieces.Add(craftingPieceVM);
							craftingPieceVM.IsNewlyUnlocked = flag2 && this._newlyUnlockedPieces.Contains(weaponDesignElement.CraftingPiece);
							if (this._crafting.SelectedPieces[pieceType].CraftingPiece == craftingPieceVM.CraftingPiece.CraftingPiece)
							{
								craftingPieceListVM.SelectedPiece = craftingPieceVM;
								craftingPieceVM.IsSelected = true;
							}
							this._pieceVMs.Add(this._crafting.UsablePiecesList[pieceType][i].CraftingPiece, craftingPieceVM);
						}
					}
					craftingPieceListVM.Pieces.Sort(this._pieceTierComparer);
				}
			}
			CraftingPieceListVM craftingPieceListVM2 = this.PieceLists.FirstOrDefault<CraftingPieceListVM>((CraftingPieceListVM x) => x.PieceType == CraftingPiece.PieceTypes.Blade);
			this.SelectedBladePiece = ((craftingPieceListVM2 != null) ? craftingPieceListVM2.SelectedPiece : null);
			CraftingPieceListVM craftingPieceListVM3 = this.PieceLists.FirstOrDefault<CraftingPieceListVM>((CraftingPieceListVM x) => x.PieceType == CraftingPiece.PieceTypes.Guard);
			this.SelectedGuardPiece = ((craftingPieceListVM3 != null) ? craftingPieceListVM3.SelectedPiece : null);
			CraftingPieceListVM craftingPieceListVM4 = this.PieceLists.FirstOrDefault<CraftingPieceListVM>((CraftingPieceListVM x) => x.PieceType == CraftingPiece.PieceTypes.Handle);
			this.SelectedHandlePiece = ((craftingPieceListVM4 != null) ? craftingPieceListVM4.SelectedPiece : null);
			CraftingPieceListVM craftingPieceListVM5 = this.PieceLists.FirstOrDefault<CraftingPieceListVM>((CraftingPieceListVM x) => x.PieceType == CraftingPiece.PieceTypes.Pommel);
			this.SelectedPommelPiece = ((craftingPieceListVM5 != null) ? craftingPieceListVM5.SelectedPiece : null);
			this.ItemName = this._crafting.CraftedWeaponName.ToString();
			this.PommelSize = 0;
			this.GuardSize = 0;
			this.HandleSize = 0;
			this.BladeSize = 0;
			this.RefreshPieceFlags();
			this.RefreshItem();
			this.RefreshAlternativeUsageList();
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0005CCF4 File Offset: 0x0005AEF4
		private void RefreshPieceFlags()
		{
			foreach (CraftingPieceListVM craftingPieceListVM in this.PieceLists)
			{
				craftingPieceListVM.IsEnabled = this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(craftingPieceListVM.PieceType);
				foreach (CraftingPieceVM craftingPieceVM in craftingPieceListVM.Pieces)
				{
					craftingPieceVM.RefreshFlagIcons();
					if (craftingPieceListVM.PieceType == CraftingPiece.PieceTypes.Blade)
					{
						this.AddClassFlagsToPiece(craftingPieceVM);
					}
				}
			}
			this.RefreshWeaponFlags();
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0005CDA8 File Offset: 0x0005AFA8
		private void AddClassFlagsToPiece(CraftingPieceVM piece)
		{
			WeaponComponentData weaponWithUsageIndex = this._crafting.GetCurrentCraftedItemObject(false, null).GetWeaponWithUsageIndex(this.SecondaryUsageSelector.SelectedIndex);
			int indexOfUsageDataWithId = this._crafting.CurrentCraftingTemplate.GetIndexOfUsageDataWithId(weaponWithUsageIndex.WeaponDescriptionId);
			WeaponDescription weaponDescription = this._crafting.CurrentCraftingTemplate.WeaponDescriptions.ElementAtOrDefault<WeaponDescription>(indexOfUsageDataWithId);
			if (weaponDescription != null)
			{
				using (List<ValueTuple<string, TextObject>>.Enumerator enumerator = CampaignUIHelper.GetWeaponFlagDetails(weaponDescription.WeaponFlags, null).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<string, TextObject> flagPath = enumerator.Current;
						if (!piece.ItemAttributeIcons.Any<CraftingItemFlagVM>((CraftingItemFlagVM x) => x.Icon.Contains(flagPath.Item1)))
						{
							piece.ItemAttributeIcons.Add(new CraftingItemFlagVM(flagPath.Item1, flagPath.Item2, false));
						}
					}
				}
			}
			using (List<ValueTuple<string, TextObject>>.Enumerator enumerator = CampaignUIHelper.GetFlagDetailsForWeapon(weaponWithUsageIndex, this._getItemUsageSetFlags(weaponWithUsageIndex), null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ValueTuple<string, TextObject> usageFlag = enumerator.Current;
					if (!piece.ItemAttributeIcons.Any<CraftingItemFlagVM>((CraftingItemFlagVM x) => x.Icon.Contains(usageFlag.Item1)))
					{
						piece.ItemAttributeIcons.Add(new CraftingItemFlagVM(usageFlag.Item1, usageFlag.Item2, false));
					}
				}
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0005CF34 File Offset: 0x0005B134
		private void UpdateSecondaryUsageIndex(SelectorVM<CraftingSecondaryUsageItemVM> selector)
		{
			if (selector.SelectedIndex != -1)
			{
				this.RefreshStats();
				this.RefreshPieceFlags();
			}
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0005CF4C File Offset: 0x0005B14C
		private MBBindingList<WeaponDesignResultPropertyItemVM> GetResultPropertyList(CraftingSecondaryUsageItemVM usageItem)
		{
			MBBindingList<WeaponDesignResultPropertyItemVM> mbbindingList = new MBBindingList<WeaponDesignResultPropertyItemVM>();
			if (usageItem == null)
			{
				return mbbindingList;
			}
			int usageIndex = usageItem.UsageIndex;
			this.TrySetSecondaryUsageIndex(usageIndex);
			this.RefreshStats();
			ItemModifier currentItemModifier = this._craftingBehavior.GetCurrentItemModifier();
			foreach (CraftingListPropertyItem craftingListPropertyItem in this.PrimaryPropertyList)
			{
				float num = 0f;
				bool flag = craftingListPropertyItem.Type == CraftingTemplate.CraftingStatTypes.Weight;
				if (currentItemModifier != null)
				{
					float num2 = craftingListPropertyItem.PropertyValue;
					if (craftingListPropertyItem.Type == CraftingTemplate.CraftingStatTypes.SwingDamage)
					{
						num2 = (float)currentItemModifier.ModifyDamage((int)craftingListPropertyItem.PropertyValue);
					}
					else if (craftingListPropertyItem.Type == CraftingTemplate.CraftingStatTypes.SwingSpeed)
					{
						num2 = (float)currentItemModifier.ModifySpeed((int)craftingListPropertyItem.PropertyValue);
					}
					else if (craftingListPropertyItem.Type == CraftingTemplate.CraftingStatTypes.ThrustDamage)
					{
						num2 = (float)currentItemModifier.ModifyDamage((int)craftingListPropertyItem.PropertyValue);
					}
					else if (craftingListPropertyItem.Type == CraftingTemplate.CraftingStatTypes.ThrustSpeed)
					{
						num2 = (float)currentItemModifier.ModifySpeed((int)craftingListPropertyItem.PropertyValue);
					}
					else if (craftingListPropertyItem.Type == CraftingTemplate.CraftingStatTypes.Handling)
					{
						num2 = (float)currentItemModifier.ModifySpeed((int)craftingListPropertyItem.PropertyValue);
					}
					if (num2 != craftingListPropertyItem.PropertyValue)
					{
						num = num2 - craftingListPropertyItem.PropertyValue;
					}
				}
				if (this.IsInOrderMode)
				{
					mbbindingList.Add(new WeaponDesignResultPropertyItemVM(craftingListPropertyItem.Description, craftingListPropertyItem.PropertyValue, craftingListPropertyItem.TargetValue, num, flag, craftingListPropertyItem.IsExceedingBeneficial, true));
				}
				else
				{
					mbbindingList.Add(new WeaponDesignResultPropertyItemVM(craftingListPropertyItem.Description, craftingListPropertyItem.PropertyValue, num, flag));
				}
			}
			return mbbindingList;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0005D0EC File Offset: 0x0005B2EC
		public void SelectPrimaryWeaponClass(CraftingTemplate template)
		{
			int num = this._primaryUsages.IndexOf(template);
			this._selectedWeaponClassIndex = num;
			if (this._crafting.CurrentCraftingTemplate != template)
			{
				CraftingHelper.ChangeCurrentCraftingTemplate(template);
				return;
			}
			this.AddHistoryKey();
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0005D128 File Offset: 0x0005B328
		private void RefreshWeaponDesignMode(CraftingOrderItemVM orderToSelect, int classIndex = -1, bool doNotAutoSelectPieces = false)
		{
			bool flag = false;
			CraftingTemplate selectedCraftingTemplate = null;
			this.SecondaryUsageSelector.SelectedIndex = 0;
			if (orderToSelect != null)
			{
				this.IsInOrderMode = true;
				this.ActiveCraftingOrder = orderToSelect;
				selectedCraftingTemplate = orderToSelect.CraftingOrder.PreCraftedWeaponDesignItem.WeaponDesign.Template;
				this.SelectPrimaryWeaponClass(selectedCraftingTemplate);
				flag = true;
			}
			else
			{
				this.IsInOrderMode = false;
				this.ActiveCraftingOrder = null;
				if (classIndex >= 0)
				{
					selectedCraftingTemplate = this._primaryUsages[classIndex];
					this.SelectPrimaryWeaponClass(selectedCraftingTemplate);
					flag = true;
				}
			}
			WeaponClassVM weaponClassVM = this.WeaponClassSelectionPopup.WeaponClasses.FirstOrDefault<WeaponClassVM>((WeaponClassVM x) => x.Template == selectedCraftingTemplate);
			if (weaponClassVM != null)
			{
				weaponClassVM.NewlyUnlockedPieceCount = 0;
			}
			this.CraftingOrderPopup.RefreshOrders();
			this.CraftingHistory.RefreshAvailability();
			this.IsOrderButtonActive = this.CraftingOrderPopup.HasEnabledOrders;
			Action onRefresh = this._onRefresh;
			if (onRefresh != null)
			{
				onRefresh();
			}
			Action<CraftingOrder> refreshHeroAvailabilities = this._refreshHeroAvailabilities;
			if (refreshHeroAvailabilities != null)
			{
				CraftingOrderItemVM activeCraftingOrder = this.ActiveCraftingOrder;
				refreshHeroAvailabilities((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder : null);
			}
			if (!flag)
			{
				this.InitializeDefaultFromLogic();
			}
			this.RefreshValues();
			this.RefreshItem();
			this.OnSelectPieceType(CraftingPiece.PieceTypes.Blade, false);
			this.FilterPieces(this._currentTierFilter);
			this.RefreshCurrentHeroSkillLevel();
			if (!doNotAutoSelectPieces)
			{
				this.SelectDefaultPiecesForCurrentTemplate();
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0005D278 File Offset: 0x0005B478
		private void OnCraftingOrderSelected(CraftingOrderItemVM selectedOrder)
		{
			this.RefreshWeaponDesignMode(selectedOrder, -1, false);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0005D284 File Offset: 0x0005B484
		public void ExecuteOpenOrderPopup()
		{
			this.CraftingOrderPopup.ExecuteOpenPopup();
			MBBindingList<CraftingOrderItemVM> craftingOrders = this.CraftingOrderPopup.CraftingOrders;
			CraftingOrderItemVM craftingOrderItemVM = ((craftingOrders != null) ? craftingOrders.FirstOrDefault<CraftingOrderItemVM>(delegate(CraftingOrderItemVM x)
			{
				CraftingOrder craftingOrder = x.CraftingOrder;
				CraftingOrderItemVM activeCraftingOrder = this.ActiveCraftingOrder;
				return craftingOrder == ((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder : null);
			}) : null);
			if (craftingOrderItemVM != null)
			{
				craftingOrderItemVM.IsSelected = true;
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0005D2CA File Offset: 0x0005B4CA
		public void ExecuteCloseOrderPopup()
		{
			this.CraftingOrderPopup.IsVisible = false;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0005D2D8 File Offset: 0x0005B4D8
		public void ExecuteOpenOrdersTab()
		{
			if (this.IsInFreeMode)
			{
				MBBindingList<CraftingOrderItemVM> craftingOrders = this.CraftingOrderPopup.CraftingOrders;
				CraftingOrderItemVM craftingOrderItemVM;
				if (craftingOrders == null)
				{
					craftingOrderItemVM = null;
				}
				else
				{
					craftingOrderItemVM = craftingOrders.FirstOrDefault<CraftingOrderItemVM>((CraftingOrderItemVM x) => x.IsEnabled);
				}
				CraftingOrderItemVM craftingOrderItemVM2 = craftingOrderItemVM;
				if (craftingOrderItemVM2 != null)
				{
					this.CraftingOrderPopup.SelectOrder(craftingOrderItemVM2);
				}
				else
				{
					this.CraftingOrderPopup.ExecuteOpenPopup();
				}
				Game game = Game.Current;
				if (game == null)
				{
					return;
				}
				game.EventManager.TriggerEvent<CraftingOrderTabOpenedEvent>(new CraftingOrderTabOpenedEvent(true));
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0005D35A File Offset: 0x0005B55A
		public void ExecuteOpenWeaponClassSelectionPopup()
		{
			this.WeaponClassSelectionPopup.UpdateNewlyUnlockedPiecesCount(this._newlyUnlockedPieces);
			this.WeaponClassSelectionPopup.WeaponClasses.ApplyActionOnAllItems(delegate(WeaponClassVM x)
			{
				x.IsSelected = x.SelectionIndex == this._selectedWeaponClassIndex;
			});
			this.WeaponClassSelectionPopup.ExecuteOpenPopup();
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0005D394 File Offset: 0x0005B594
		public void ExecuteOpenFreeBuildTab()
		{
			if (this.IsInOrderMode)
			{
				this.WeaponClassSelectionPopup.UpdateNewlyUnlockedPiecesCount(this._newlyUnlockedPieces);
				this.WeaponClassSelectionPopup.WeaponClasses.ApplyActionOnAllItems(delegate(WeaponClassVM x)
				{
					x.IsSelected = false;
				});
				this.WeaponClassSelectionPopup.ExecuteSelectWeaponClass(0);
				Game game = Game.Current;
				if (game == null)
				{
					return;
				}
				game.EventManager.TriggerEvent<CraftingOrderTabOpenedEvent>(new CraftingOrderTabOpenedEvent(false));
			}
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0005D410 File Offset: 0x0005B610
		public void CreateCraftingResultPopup()
		{
			this.CraftedItemVisual.StringId = this.CraftedItemObject.StringId;
			this.IsWeaponCivilian = this.CraftedItemObject.IsCivilian;
			WeaponDesignResultPopupVM craftingResultPopup = this.CraftingResultPopup;
			if (craftingResultPopup != null)
			{
				craftingResultPopup.OnFinalize();
			}
			ItemObject craftedItemObject = this.CraftedItemObject;
			TextObject craftedWeaponName = this._crafting.CraftedWeaponName;
			Action action = new Action(this.ExecuteFinalizeCrafting);
			Crafting crafting = this._crafting;
			CraftingOrderItemVM activeCraftingOrder = this.ActiveCraftingOrder;
			this.CraftingResultPopup = new WeaponDesignResultPopupVM(craftedItemObject, craftedWeaponName, action, crafting, (activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder : null, this._craftedItemVisual, this.WeaponFlagIconsList, new Func<CraftingSecondaryUsageItemVM, MBBindingList<WeaponDesignResultPropertyItemVM>>(this.GetResultPropertyList), new Action<CraftingSecondaryUsageItemVM>(this.OnSecondaryUsageChangedFromPopup));
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x0005D4BC File Offset: 0x0005B6BC
		private void OnSecondaryUsageChangedFromPopup(CraftingSecondaryUsageItemVM usage)
		{
			for (int i = 0; i < this.SecondaryUsageSelector.ItemList.Count; i++)
			{
				if (this.SecondaryUsageSelector.ItemList[i].UsageIndex == usage.UsageIndex)
				{
					this.SecondaryUsageSelector.SelectedIndex = i;
					return;
				}
			}
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x0005D50F File Offset: 0x0005B70F
		public void ExecuteToggleShowOnlyUnlockedPieces()
		{
			this.ShowOnlyUnlockedPieces = !this.ShowOnlyUnlockedPieces;
			this.FilterPieces(this._currentTierFilter);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x0005D52C File Offset: 0x0005B72C
		public void ExecuteUndo()
		{
			if (this._crafting.Undo())
			{
				Action onRefresh = this._onRefresh;
				if (onRefresh != null)
				{
					onRefresh();
				}
				this._updatePiece = false;
				int i2;
				int i;
				for (i = 0; i < 4; i = i2 + 1)
				{
					CraftingPiece.PieceTypes j = (CraftingPiece.PieceTypes)i;
					if (this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(j))
					{
						CraftingPieceVM craftingPieceVM = this._pieceListsDictionary[j].Pieces.First<CraftingPieceVM>((CraftingPieceVM piece) => piece.CraftingPiece.CraftingPiece == this._crafting.SelectedPieces[i].CraftingPiece);
						this.OnSetItemPiece(craftingPieceVM, 0, true, false);
					}
					i2 = i;
				}
				this.RefreshItem();
				this._updatePiece = true;
			}
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x0005D5E4 File Offset: 0x0005B7E4
		public void ExecuteRedo()
		{
			if (this._crafting.Redo())
			{
				Action onRefresh = this._onRefresh;
				if (onRefresh != null)
				{
					onRefresh();
				}
				this._updatePiece = false;
				int i2;
				int i;
				for (i = 0; i < 4; i = i2 + 1)
				{
					CraftingPiece.PieceTypes j = (CraftingPiece.PieceTypes)i;
					if (this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(j))
					{
						CraftingPieceVM craftingPieceVM = this._pieceListsDictionary[j].Pieces.First<CraftingPieceVM>((CraftingPieceVM piece) => piece.CraftingPiece.CraftingPiece == this._crafting.SelectedPieces[i].CraftingPiece);
						this.OnSetItemPiece(craftingPieceVM, 0, true, false);
					}
					i2 = i;
				}
				this.RefreshItem();
				this._updatePiece = true;
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0005D69C File Offset: 0x0005B89C
		internal void OnCraftingHeroChanged(CraftingAvailableHeroItemVM newHero)
		{
			this.RefreshCurrentHeroSkillLevel();
			this.RefreshDifficulty();
			this.CraftingOrderPopup.RefreshOrders();
			this.IsOrderButtonActive = this.CraftingOrderPopup.HasEnabledOrders;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0005D6C8 File Offset: 0x0005B8C8
		public void ChangeModeIfHeroIsUnavailable()
		{
			CraftingAvailableHeroItemVM craftingAvailableHeroItemVM = this._getCurrentCraftingHero();
			if (this.IsInOrderMode && craftingAvailableHeroItemVM.IsDisabled)
			{
				this.RefreshWeaponDesignMode(null, -1, false);
			}
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0005D6FC File Offset: 0x0005B8FC
		public void ExecuteBeginHeroHint()
		{
			CraftingOrderItemVM activeCraftingOrder = this._activeCraftingOrder;
			if (((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder.OrderOwner : null) != null)
			{
				InformationManager.ShowTooltip(typeof(Hero), new object[]
				{
					this._activeCraftingOrder.CraftingOrder.OrderOwner,
					false
				});
			}
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0005D753 File Offset: 0x0005B953
		public void ExecuteEndHeroHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0005D75C File Offset: 0x0005B95C
		public void ExecuteRandomize()
		{
			for (int i = 0; i < 4; i++)
			{
				CraftingPiece.PieceTypes pieceTypes = (CraftingPiece.PieceTypes)i;
				if (this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(pieceTypes))
				{
					CraftingPieceVM randomElementWithPredicate = this._pieceListsDictionary[pieceTypes].Pieces.GetRandomElementWithPredicate<CraftingPieceVM>((CraftingPieceVM p) => p.PlayerHasPiece);
					if (randomElementWithPredicate != null)
					{
						this.OnSetItemPiece(randomElementWithPredicate, (int)(90f + MBRandom.RandomFloat * 20f), false, true);
					}
				}
			}
			this._updatePiece = false;
			this.RefreshItem();
			this.AddHistoryKey();
			this._updatePiece = true;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0005D7F8 File Offset: 0x0005B9F8
		public void ExecuteChangeScabbardVisibility()
		{
			if (!this._crafting.CurrentCraftingTemplate.UseWeaponAsHolsterMesh)
			{
				this.IsScabbardVisible = !this.IsScabbardVisible;
			}
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0005D81C File Offset: 0x0005BA1C
		public void SelectWeapon(ItemObject itemObject)
		{
			this._crafting.SwitchToCraftedItem(itemObject);
			Action onRefresh = this._onRefresh;
			if (onRefresh != null)
			{
				onRefresh();
			}
			this._updatePiece = false;
			int i;
			int i2;
			for (i = 0; i < 4; i = i2 + 1)
			{
				CraftingPiece.PieceTypes j = (CraftingPiece.PieceTypes)i;
				if (this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(j))
				{
					CraftingPieceVM craftingPieceVM = this._pieceListsDictionary[j].Pieces.First<CraftingPieceVM>((CraftingPieceVM piece) => piece.CraftingPiece.CraftingPiece == this._crafting.CurrentWeaponDesign.UsedPieces[i].CraftingPiece);
					this.OnSetItemPiece(craftingPieceVM, this._crafting.CurrentWeaponDesign.UsedPieces[i].ScalePercentage, true, false);
				}
				i2 = i;
			}
			this.RefreshItem();
			this.AddHistoryKey();
			this._updatePiece = true;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0005D8F4 File Offset: 0x0005BAF4
		public bool CanCompleteOrder()
		{
			bool flag = true;
			if (this.IsInOrderMode)
			{
				ItemObject currentCraftedItemObject = this._crafting.GetCurrentCraftedItemObject(false, null);
				flag = this.ActiveCraftingOrder.CraftingOrder.CanHeroCompleteOrder(this._getCurrentCraftingHero().Hero, currentCraftedItemObject);
			}
			return flag;
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0005D93C File Offset: 0x0005BB3C
		public void ExecuteFinalizeCrafting()
		{
			if (this._craftingBehavior != null && Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				if (GameStateManager.Current.ActiveState is CraftingState)
				{
					if (this.IsInOrderMode)
					{
						this._craftingBehavior.CompleteOrder(Settlement.CurrentSettlement.Town, this.ActiveCraftingOrder.CraftingOrder, this.CraftedItemObject, this._getCurrentCraftingHero().Hero);
						this.CraftedItemObject = null;
						this.CraftingOrderPopup.RefreshOrders();
						CraftingOrderItemVM craftingOrderItemVM = this.CraftingOrderPopup.CraftingOrders.FirstOrDefault<CraftingOrderItemVM>((CraftingOrderItemVM x) => x.IsEnabled);
						if (craftingOrderItemVM != null)
						{
							this.CraftingOrderPopup.SelectOrder(craftingOrderItemVM);
						}
						else
						{
							this.ExecuteOpenFreeBuildTab();
						}
					}
					else
					{
						int bladeSize = this.BladeSize;
						int guardSize = this.GuardSize;
						int handleSize = this.HandleSize;
						int pommelSize = this.PommelSize;
						this.RefreshWeaponDesignMode(null, this._selectedWeaponClassIndex, false);
						this.BladeSize = bladeSize;
						this.GuardSize = guardSize;
						this.HandleSize = handleSize;
						this.PommelSize = pommelSize;
					}
				}
				this.IsInFinalCraftingStage = false;
			}
			TextObject textObject = new TextObject("{=uZhHh7pm}Crafted {CURR_TEMPLATE_NAME}", null);
			textObject.SetTextVariable("CURR_TEMPLATE_NAME", this._crafting.CurrentCraftingTemplate.TemplateName);
			this._crafting.SetCraftedWeaponName(textObject);
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x0005DA99 File Offset: 0x0005BC99
		private bool DoesCurrentItemHaveSecondaryUsage(int usageIndex)
		{
			return usageIndex >= 0 && usageIndex < this._crafting.GetCurrentCraftedItemObject(false, null).Weapons.Count;
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0005DABC File Offset: 0x0005BCBC
		private void TrySetSecondaryUsageIndex(int usageIndex)
		{
			int num = 0;
			if (this.DoesCurrentItemHaveSecondaryUsage(usageIndex))
			{
				CraftingSecondaryUsageItemVM craftingSecondaryUsageItemVM = this.SecondaryUsageSelector.ItemList.FirstOrDefault<CraftingSecondaryUsageItemVM>((CraftingSecondaryUsageItemVM x) => x.UsageIndex == usageIndex);
				if (craftingSecondaryUsageItemVM != null)
				{
					num = craftingSecondaryUsageItemVM.SelectorIndex;
				}
			}
			if (num >= 0 && num < this.SecondaryUsageSelector.ItemList.Count)
			{
				this.SecondaryUsageSelector.SelectedIndex = num;
				this.SecondaryUsageSelector.ItemList[num].IsSelected = true;
			}
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0005DB48 File Offset: 0x0005BD48
		private void RefreshAlternativeUsageList()
		{
			int num = this.SecondaryUsageSelector.SelectedIndex;
			this.SecondaryUsageSelector.Refresh(new List<string>(), 0, new Action<SelectorVM<CraftingSecondaryUsageItemVM>>(this.UpdateSecondaryUsageIndex));
			MBReadOnlyList<WeaponComponentData> weapons = this._crafting.GetCurrentCraftedItemObject(false, null).Weapons;
			int num2 = 0;
			for (int i = 0; i < weapons.Count; i++)
			{
				if (CampaignUIHelper.IsItemUsageApplicable(weapons[i]))
				{
					TextObject textObject = GameTexts.FindText("str_weapon_usage", weapons[i].WeaponDescriptionId);
					this.SecondaryUsageSelector.AddItem(new CraftingSecondaryUsageItemVM(textObject, num2, i, this.SecondaryUsageSelector));
					CraftingOrderItemVM activeCraftingOrder = this.ActiveCraftingOrder;
					if (((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder.GetStatWeapon().WeaponDescriptionId : null) == weapons[i].WeaponDescriptionId)
					{
						num = num2;
					}
					num2++;
				}
			}
			this.TrySetSecondaryUsageIndex(num);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0005DC24 File Offset: 0x0005BE24
		private void RefreshStats()
		{
			if (!this.DoesCurrentItemHaveSecondaryUsage(this.SecondaryUsageSelector.SelectedIndex))
			{
				this.TrySetSecondaryUsageIndex(0);
			}
			List<CraftingStatData> list = this._crafting.GetStatDatas(this.SecondaryUsageSelector.SelectedIndex).ToList<CraftingStatData>();
			WeaponComponentData weaponComponentData = (this.IsInOrderMode ? this.ActiveCraftingOrder.CraftingOrder.GetStatWeapon() : null);
			IEnumerable<CraftingStatData> enumerable = (this.IsInOrderMode ? this.GetOrderStatDatas(this.ActiveCraftingOrder.CraftingOrder) : null);
			ItemObject currentCraftedItemObject = this._crafting.GetCurrentCraftedItemObject(false, null);
			WeaponComponentData weaponWithUsageIndex = currentCraftedItemObject.GetWeaponWithUsageIndex(this.SecondaryUsageSelector.SelectedIndex);
			bool flag = weaponComponentData == null || weaponComponentData.WeaponDescriptionId == weaponWithUsageIndex.WeaponDescriptionId;
			if (enumerable != null)
			{
				using (IEnumerator<CraftingStatData> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CraftingStatData orderStatData = enumerator.Current;
						if (!list.Any<CraftingStatData>((CraftingStatData x) => x.Type == orderStatData.Type && x.DamageType == orderStatData.DamageType))
						{
							if ((orderStatData.Type == CraftingTemplate.CraftingStatTypes.SwingDamage && orderStatData.DamageType != weaponWithUsageIndex.SwingDamageType) || (orderStatData.Type == CraftingTemplate.CraftingStatTypes.ThrustDamage && orderStatData.DamageType != weaponWithUsageIndex.ThrustDamageType))
							{
								list.Add(new CraftingStatData(orderStatData.DescriptionText, 0f, orderStatData.MaxValue, orderStatData.Type, orderStatData.DamageType));
							}
							else
							{
								list.Add(orderStatData);
							}
						}
					}
				}
			}
			this.PrimaryPropertyList.Clear();
			using (List<CraftingStatData>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CraftingStatData statData = enumerator2.Current;
					if (statData.IsValid)
					{
						float num = 0f;
						if (this.IsInOrderMode && flag)
						{
							WeaponAttributeVM weaponAttributeVM = this.ActiveCraftingOrder.WeaponAttributes.FirstOrDefault<WeaponAttributeVM>((WeaponAttributeVM x) => x.AttributeType == statData.Type && x.DamageType == statData.DamageType);
							num = ((weaponAttributeVM != null) ? weaponAttributeVM.AttributeValue : 0f);
						}
						float num2 = MathF.Max(statData.MaxValue, num);
						CraftingListPropertyItem craftingListPropertyItem = new CraftingListPropertyItem(statData.DescriptionText, num2, statData.CurValue, num, statData.Type, false);
						this.PrimaryPropertyList.Add(craftingListPropertyItem);
						craftingListPropertyItem.IsValidForUsage = true;
					}
				}
			}
			this.PrimaryPropertyList.Sort(new WeaponDesignVM.WeaponPropertyComparer());
			CraftingOrderItemVM activeCraftingOrder = this.ActiveCraftingOrder;
			this.MissingPropertyWarningText = CampaignUIHelper.GetCraftingOrderMissingPropertyWarningText((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder : null, currentCraftedItemObject);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0005DF0C File Offset: 0x0005C10C
		private IEnumerable<CraftingStatData> GetOrderStatDatas(CraftingOrder order)
		{
			if (order == null)
			{
				return null;
			}
			WeaponComponentData weaponComponentData;
			return order.GetStatDataForItem(order.PreCraftedWeaponDesignItem, out weaponComponentData);
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0005DF2C File Offset: 0x0005C12C
		private void RefreshWeaponFlags()
		{
			this.WeaponFlagIconsList.Clear();
			foreach (CraftingPieceListVM craftingPieceListVM in this.PieceLists)
			{
				if (craftingPieceListVM.SelectedPiece != null)
				{
					using (IEnumerator<CraftingItemFlagVM> enumerator2 = craftingPieceListVM.SelectedPiece.ItemAttributeIcons.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CraftingItemFlagVM iconData = enumerator2.Current;
							if (!this.WeaponFlagIconsList.Any<ItemFlagVM>((ItemFlagVM x) => x.Icon == iconData.Icon))
							{
								this.WeaponFlagIconsList.Add(iconData);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0005DFF4 File Offset: 0x0005C1F4
		private void OnSetItemPieceManually(CraftingPieceVM piece)
		{
			this.OnSetItemPiece(piece, 0, true, false);
			this.RefreshItem();
			this.AddHistoryKey();
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0005E00C File Offset: 0x0005C20C
		private void OnSetItemPiece(CraftingPieceVM piece, int scalePercentage = 0, bool shouldUpdateWholeWeapon = true, bool forceUpdatePiece = false)
		{
			CraftingPiece.PieceTypes pieceType = (CraftingPiece.PieceTypes)piece.PieceType;
			this._pieceListsDictionary[pieceType].SelectedPiece.IsSelected = false;
			bool updatePiece = this._updatePiece;
			if (!this._isAutoSelectingPieces)
			{
				this.UnsetPieceNewlyUnlocked(piece);
			}
			if (updatePiece)
			{
				this._crafting.SwitchToPiece(piece.CraftingPiece);
				if (!forceUpdatePiece)
				{
					this._updatePiece = false;
				}
			}
			piece.IsSelected = true;
			this._pieceListsDictionary[pieceType].SelectedPiece = piece;
			int num = ((scalePercentage != 0) ? scalePercentage : this._crafting.SelectedPieces[(int)pieceType].ScalePercentage) - 100;
			switch (pieceType)
			{
			case CraftingPiece.PieceTypes.Blade:
				this.BladeSize = num;
				this.SelectedBladePiece = piece;
				break;
			case CraftingPiece.PieceTypes.Guard:
				this.GuardSize = num;
				this.SelectedGuardPiece = piece;
				break;
			case CraftingPiece.PieceTypes.Handle:
				this.HandleSize = num;
				this.SelectedHandlePiece = piece;
				break;
			case CraftingPiece.PieceTypes.Pommel:
				this.PommelSize = num;
				this.SelectedPommelPiece = piece;
				break;
			}
			if (this.IsInFreeMode)
			{
				WeaponClassVM currentWeaponClass = this.GetCurrentWeaponClass();
				if (currentWeaponClass != null)
				{
					currentWeaponClass.RegisterSelectedPiece(pieceType, piece.CraftingPiece.CraftingPiece.StringId);
				}
			}
			this._updatePiece = updatePiece;
			this.RefreshAlternativeUsageList();
			if (shouldUpdateWholeWeapon)
			{
				Action onRefresh = this._onRefresh;
				if (onRefresh != null)
				{
					onRefresh();
				}
			}
			this.PieceLists.ApplyActionOnAllItems(delegate(CraftingPieceListVM x)
			{
				x.Refresh();
			});
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0005E16B File Offset: 0x0005C36B
		public void RefreshItem()
		{
			this.RefreshStats();
			this.RefreshWeaponFlags();
			this.RefreshDifficulty();
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0005E190 File Offset: 0x0005C390
		private void RefreshDifficulty()
		{
			this.CurrentDifficulty = Campaign.Current.Models.SmithingModel.CalculateWeaponDesignDifficulty(this._crafting.CurrentWeaponDesign);
			if (this.IsInOrderMode)
			{
				this.CurrentOrderDifficulty = MathF.Round(this.ActiveCraftingOrder.CraftingOrder.OrderDifficulty);
			}
			this._currentCraftingSkillText.SetTextVariable("SKILL_VALUE", this.CurrentHeroCraftingSkill);
			this._currentCraftingSkillText.SetTextVariable("SKILL_NAME", DefaultSkills.Crafting.Name);
			this.CurrentCraftingSkillValueText = this._currentCraftingSkillText.ToString();
			this.CurrentDifficultyText = this.GetCurrentDifficultyText(this.CurrentHeroCraftingSkill, this.CurrentDifficulty);
			this.CurrentOrderDifficultyText = this.GetCurrentOrderDifficultyText(this.CurrentOrderDifficulty);
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0005E252 File Offset: 0x0005C452
		private string GetCurrentDifficultyText(int skillValue, int difficultyValue)
		{
			this._difficultyTextobj.SetTextVariable("DIFFICULTY", difficultyValue);
			return this._difficultyTextobj.ToString();
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0005E271 File Offset: 0x0005C471
		private string GetCurrentOrderDifficultyText(int orderDifficulty)
		{
			this._orderDifficultyTextObj.SetTextVariable("DIFFICULTY", orderDifficulty.ToString());
			return this._orderDifficultyTextObj.ToString();
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0005E298 File Offset: 0x0005C498
		private void RefreshCurrentHeroSkillLevel()
		{
			Func<CraftingAvailableHeroItemVM> getCurrentCraftingHero = this._getCurrentCraftingHero;
			int? num;
			if (getCurrentCraftingHero == null)
			{
				num = null;
			}
			else
			{
				CraftingAvailableHeroItemVM craftingAvailableHeroItemVM = getCurrentCraftingHero();
				num = ((craftingAvailableHeroItemVM != null) ? new int?(craftingAvailableHeroItemVM.Hero.CharacterObject.GetSkillValue(DefaultSkills.Crafting)) : null);
			}
			this.CurrentHeroCraftingSkill = num ?? 0;
			this.IsCurrentHeroAtMaxCraftingSkill = this.CurrentHeroCraftingSkill >= 300;
			this._currentCraftingSkillText.SetTextVariable("SKILL_VALUE", this.CurrentHeroCraftingSkill);
			this.CurrentCraftingSkillValueText = this._currentCraftingSkillText.ToString();
			this.CurrentDifficultyText = this.GetCurrentDifficultyText(this.CurrentHeroCraftingSkill, this.CurrentDifficulty);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0005E358 File Offset: 0x0005C558
		public bool HaveUnlockedAllSelectedPieces()
		{
			foreach (CraftingPieceListVM craftingPieceListVM in this.PieceLists)
			{
				if (craftingPieceListVM.IsEnabled)
				{
					CraftingPieceVM selectedPiece = craftingPieceListVM.SelectedPiece;
					if (((selectedPiece != null) ? selectedPiece.CraftingPiece : null) != null && !craftingPieceListVM.SelectedPiece.PlayerHasPiece)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0005E3D0 File Offset: 0x0005C5D0
		private void AddHistoryKey()
		{
			if (this._shouldRecordHistory)
			{
				this._crafting.UpdateHistory();
			}
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0005E3E8 File Offset: 0x0005C5E8
		public void SwitchToPiece(WeaponDesignElement usedPiece)
		{
			CraftingPieceVM craftingPieceVM = this._pieceListsDictionary[usedPiece.CraftingPiece.PieceType].Pieces.FirstOrDefault<CraftingPieceVM>((CraftingPieceVM p) => p.CraftingPiece.CraftingPiece == usedPiece.CraftingPiece);
			this.OnSetItemPiece(craftingPieceVM, usedPiece.ScalePercentage, true, false);
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0005E448 File Offset: 0x0005C648
		internal void SetDesignManually(CraftingTemplate craftingTemplate, ValueTuple<CraftingPiece, int>[] pieces, bool forceChangeTemplate = false)
		{
			int num = this._primaryUsages.IndexOf(craftingTemplate);
			if ((this.IsInFreeMode && forceChangeTemplate) || num == this._selectedWeaponClassIndex)
			{
				this.RefreshWeaponDesignMode(this.ActiveCraftingOrder, this._primaryUsages.IndexOf(craftingTemplate), true);
				for (int i = 0; i < pieces.Length; i++)
				{
					ValueTuple<CraftingPiece, int> currentPiece = pieces[i];
					if (currentPiece.Item1 != null)
					{
						CraftingPieceVM craftingPieceVM = this._pieceListsDictionary[currentPiece.Item1.PieceType].Pieces.FirstOrDefault<CraftingPieceVM>((CraftingPieceVM piece) => piece.CraftingPiece.CraftingPiece == currentPiece.Item1);
						if (craftingPieceVM != null)
						{
							this.OnSetItemPiece(craftingPieceVM, currentPiece.Item2, true, false);
							this._crafting.ScaleThePiece(currentPiece.Item1.PieceType, currentPiece.Item2);
						}
					}
				}
				this.RefreshDifficulty();
				Action onRefresh = this._onRefresh;
				if (onRefresh == null)
				{
					return;
				}
				onRefresh();
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06001855 RID: 6229 RVA: 0x0005E54A File Offset: 0x0005C74A
		// (set) Token: 0x06001856 RID: 6230 RVA: 0x0005E552 File Offset: 0x0005C752
		[DataSourceProperty]
		public MBBindingList<TierFilterTypeVM> TierFilters
		{
			get
			{
				return this._tierFilters;
			}
			set
			{
				if (value != this._tierFilters)
				{
					this._tierFilters = value;
					base.OnPropertyChangedWithValue<MBBindingList<TierFilterTypeVM>>(value, "TierFilters");
				}
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06001857 RID: 6231 RVA: 0x0005E570 File Offset: 0x0005C770
		// (set) Token: 0x06001858 RID: 6232 RVA: 0x0005E578 File Offset: 0x0005C778
		[DataSourceProperty]
		public string CurrentCraftedWeaponTemplateId
		{
			get
			{
				return this._currentCraftedWeaponTemplateId;
			}
			set
			{
				if (value != this._currentCraftedWeaponTemplateId)
				{
					this._currentCraftedWeaponTemplateId = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCraftedWeaponTemplateId");
				}
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06001859 RID: 6233 RVA: 0x0005E59B File Offset: 0x0005C79B
		// (set) Token: 0x0600185A RID: 6234 RVA: 0x0005E5A3 File Offset: 0x0005C7A3
		[DataSourceProperty]
		public string ChooseOrderText
		{
			get
			{
				return this._chooseOrderText;
			}
			set
			{
				if (value != this._chooseOrderText)
				{
					this._chooseOrderText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChooseOrderText");
				}
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x0600185B RID: 6235 RVA: 0x0005E5C6 File Offset: 0x0005C7C6
		// (set) Token: 0x0600185C RID: 6236 RVA: 0x0005E5CE File Offset: 0x0005C7CE
		[DataSourceProperty]
		public string ChooseWeaponTypeText
		{
			get
			{
				return this._chooseWeaponTypeText;
			}
			set
			{
				if (value != this._chooseWeaponTypeText)
				{
					this._chooseWeaponTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChooseWeaponTypeText");
				}
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x0005E5F1 File Offset: 0x0005C7F1
		// (set) Token: 0x0600185E RID: 6238 RVA: 0x0005E5F9 File Offset: 0x0005C7F9
		[DataSourceProperty]
		public string CurrentCraftedWeaponTypeText
		{
			get
			{
				return this._currentCraftedWeaponTypeText;
			}
			set
			{
				if (value != this._currentCraftedWeaponTypeText)
				{
					this._currentCraftedWeaponTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCraftedWeaponTypeText");
				}
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x0005E61C File Offset: 0x0005C81C
		// (set) Token: 0x06001860 RID: 6240 RVA: 0x0005E624 File Offset: 0x0005C824
		[DataSourceProperty]
		public MBBindingList<CraftingPieceListVM> PieceLists
		{
			get
			{
				return this._pieceLists;
			}
			set
			{
				if (value != this._pieceLists)
				{
					this._pieceLists = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingPieceListVM>>(value, "PieceLists");
				}
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x0005E642 File Offset: 0x0005C842
		// (set) Token: 0x06001862 RID: 6242 RVA: 0x0005E64A File Offset: 0x0005C84A
		[DataSourceProperty]
		public int SelectedPieceTypeIndex
		{
			get
			{
				return this._selectedPieceTypeIndex;
			}
			set
			{
				if (value != this._selectedPieceTypeIndex)
				{
					this._selectedPieceTypeIndex = value;
					base.OnPropertyChangedWithValue(value, "SelectedPieceTypeIndex");
				}
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x0005E668 File Offset: 0x0005C868
		// (set) Token: 0x06001864 RID: 6244 RVA: 0x0005E670 File Offset: 0x0005C870
		[DataSourceProperty]
		public bool ShowOnlyUnlockedPieces
		{
			get
			{
				return this._showOnlyUnlockedPieces;
			}
			set
			{
				if (value != this._showOnlyUnlockedPieces)
				{
					this._showOnlyUnlockedPieces = value;
					base.OnPropertyChangedWithValue(value, "ShowOnlyUnlockedPieces");
				}
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x0005E68E File Offset: 0x0005C88E
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x0005E696 File Offset: 0x0005C896
		[DataSourceProperty]
		public string MissingPropertyWarningText
		{
			get
			{
				return this._missingPropertyWarningText;
			}
			set
			{
				if (value != this._missingPropertyWarningText)
				{
					this._missingPropertyWarningText = value;
					base.OnPropertyChangedWithValue<string>(value, "MissingPropertyWarningText");
				}
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06001867 RID: 6247 RVA: 0x0005E6B9 File Offset: 0x0005C8B9
		// (set) Token: 0x06001868 RID: 6248 RVA: 0x0005E6C1 File Offset: 0x0005C8C1
		[DataSourceProperty]
		public WeaponDesignResultPopupVM CraftingResultPopup
		{
			get
			{
				return this._craftingResultPopup;
			}
			set
			{
				if (value != this._craftingResultPopup)
				{
					this._craftingResultPopup = value;
					base.OnPropertyChangedWithValue<WeaponDesignResultPopupVM>(value, "CraftingResultPopup");
				}
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06001869 RID: 6249 RVA: 0x0005E6DF File Offset: 0x0005C8DF
		// (set) Token: 0x0600186A RID: 6250 RVA: 0x0005E6E7 File Offset: 0x0005C8E7
		[DataSourceProperty]
		public bool IsOrderButtonActive
		{
			get
			{
				return this._isOrderButtonActive;
			}
			set
			{
				if (value != this._isOrderButtonActive)
				{
					this._isOrderButtonActive = value;
					base.OnPropertyChangedWithValue(value, "IsOrderButtonActive");
				}
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x0600186B RID: 6251 RVA: 0x0005E705 File Offset: 0x0005C905
		// (set) Token: 0x0600186C RID: 6252 RVA: 0x0005E70D File Offset: 0x0005C90D
		[DataSourceProperty]
		public bool IsInOrderMode
		{
			get
			{
				return this._isInOrderMode;
			}
			set
			{
				if (value != this._isInOrderMode)
				{
					this._isInOrderMode = value;
					base.OnPropertyChangedWithValue(value, "IsInOrderMode");
					base.OnPropertyChanged("IsInFreeMode");
				}
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x0600186D RID: 6253 RVA: 0x0005E736 File Offset: 0x0005C936
		// (set) Token: 0x0600186E RID: 6254 RVA: 0x0005E741 File Offset: 0x0005C941
		[DataSourceProperty]
		public bool IsInFreeMode
		{
			get
			{
				return !this._isInOrderMode;
			}
			set
			{
				if (value != this.IsInFreeMode)
				{
					this._isInOrderMode = !value;
					base.OnPropertyChangedWithValue(value, "IsInFreeMode");
					base.OnPropertyChanged("IsInOrderMode");
				}
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x0600186F RID: 6255 RVA: 0x0005E76D File Offset: 0x0005C96D
		// (set) Token: 0x06001870 RID: 6256 RVA: 0x0005E775 File Offset: 0x0005C975
		[DataSourceProperty]
		public string FreeModeButtonText
		{
			get
			{
				return this._freeModeButtonText;
			}
			set
			{
				if (value != this._freeModeButtonText)
				{
					this._freeModeButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "FreeModeButtonText");
				}
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x0005E798 File Offset: 0x0005C998
		// (set) Token: 0x06001872 RID: 6258 RVA: 0x0005E7A0 File Offset: 0x0005C9A0
		[DataSourceProperty]
		public CraftingOrderItemVM ActiveCraftingOrder
		{
			get
			{
				return this._activeCraftingOrder;
			}
			set
			{
				if (value != this._activeCraftingOrder)
				{
					this._activeCraftingOrder = value;
					base.OnPropertyChangedWithValue<CraftingOrderItemVM>(value, "ActiveCraftingOrder");
				}
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x0005E7BE File Offset: 0x0005C9BE
		// (set) Token: 0x06001874 RID: 6260 RVA: 0x0005E7C6 File Offset: 0x0005C9C6
		[DataSourceProperty]
		public CraftingOrderPopupVM CraftingOrderPopup
		{
			get
			{
				return this._craftingOrderPopup;
			}
			set
			{
				if (value != this._craftingOrderPopup)
				{
					this._craftingOrderPopup = value;
					base.OnPropertyChangedWithValue<CraftingOrderPopupVM>(value, "CraftingOrderPopup");
				}
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06001875 RID: 6261 RVA: 0x0005E7E4 File Offset: 0x0005C9E4
		// (set) Token: 0x06001876 RID: 6262 RVA: 0x0005E7EC File Offset: 0x0005C9EC
		[DataSourceProperty]
		public WeaponClassSelectionPopupVM WeaponClassSelectionPopup
		{
			get
			{
				return this._weaponClassSelectionPopup;
			}
			set
			{
				if (value != this._weaponClassSelectionPopup)
				{
					this._weaponClassSelectionPopup = value;
					base.OnPropertyChangedWithValue<WeaponClassSelectionPopupVM>(value, "WeaponClassSelectionPopup");
				}
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x0005E80A File Offset: 0x0005CA0A
		// (set) Token: 0x06001878 RID: 6264 RVA: 0x0005E812 File Offset: 0x0005CA12
		[DataSourceProperty]
		public MBBindingList<CraftingListPropertyItem> PrimaryPropertyList
		{
			get
			{
				return this._primaryPropertyList;
			}
			set
			{
				if (value != this._primaryPropertyList)
				{
					this._primaryPropertyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingListPropertyItem>>(value, "PrimaryPropertyList");
				}
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06001879 RID: 6265 RVA: 0x0005E830 File Offset: 0x0005CA30
		// (set) Token: 0x0600187A RID: 6266 RVA: 0x0005E838 File Offset: 0x0005CA38
		[DataSourceProperty]
		public MBBindingList<WeaponDesignResultPropertyItemVM> DesignResultPropertyList
		{
			get
			{
				return this._designResultPropertyList;
			}
			set
			{
				if (value != this._designResultPropertyList)
				{
					this._designResultPropertyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponDesignResultPropertyItemVM>>(value, "DesignResultPropertyList");
				}
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x0600187B RID: 6267 RVA: 0x0005E856 File Offset: 0x0005CA56
		// (set) Token: 0x0600187C RID: 6268 RVA: 0x0005E85E File Offset: 0x0005CA5E
		[DataSourceProperty]
		public SelectorVM<CraftingSecondaryUsageItemVM> SecondaryUsageSelector
		{
			get
			{
				return this._secondaryUsageSelector;
			}
			set
			{
				if (value != this._secondaryUsageSelector)
				{
					this._secondaryUsageSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<CraftingSecondaryUsageItemVM>>(value, "SecondaryUsageSelector");
				}
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x0005E87C File Offset: 0x0005CA7C
		// (set) Token: 0x0600187E RID: 6270 RVA: 0x0005E884 File Offset: 0x0005CA84
		[DataSourceProperty]
		public ItemCollectionElementViewModel CraftedItemVisual
		{
			get
			{
				return this._craftedItemVisual;
			}
			set
			{
				if (value != this._craftedItemVisual)
				{
					this._craftedItemVisual = value;
					base.OnPropertyChangedWithValue<ItemCollectionElementViewModel>(value, "CraftedItemVisual");
				}
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x0005E8A2 File Offset: 0x0005CAA2
		// (set) Token: 0x06001880 RID: 6272 RVA: 0x0005E8AA File Offset: 0x0005CAAA
		[DataSourceProperty]
		public bool IsInFinalCraftingStage
		{
			get
			{
				return this._isInFinalCraftingStage;
			}
			set
			{
				if (value != this._isInFinalCraftingStage)
				{
					this._isInFinalCraftingStage = value;
					base.OnPropertyChangedWithValue(value, "IsInFinalCraftingStage");
				}
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x0005E8C8 File Offset: 0x0005CAC8
		// (set) Token: 0x06001882 RID: 6274 RVA: 0x0005E8D0 File Offset: 0x0005CAD0
		[DataSourceProperty]
		public string ItemName
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (value != this._itemName)
				{
					this._itemName = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemName");
				}
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x0005E8F3 File Offset: 0x0005CAF3
		// (set) Token: 0x06001884 RID: 6276 RVA: 0x0005E8FB File Offset: 0x0005CAFB
		[DataSourceProperty]
		public bool IsScabbardVisible
		{
			get
			{
				return this._isScabbardVisible;
			}
			set
			{
				if (value != this._isScabbardVisible)
				{
					this._isScabbardVisible = value;
					base.OnPropertyChangedWithValue(value, "IsScabbardVisible");
					this._crafting.ReIndex(false);
					Action onRefresh = this._onRefresh;
					if (onRefresh == null)
					{
						return;
					}
					onRefresh();
				}
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x0005E935 File Offset: 0x0005CB35
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x0005E93D File Offset: 0x0005CB3D
		[DataSourceProperty]
		public bool CurrentWeaponHasScabbard
		{
			get
			{
				return this._currentWeaponHasScabbard;
			}
			set
			{
				if (value != this._currentWeaponHasScabbard)
				{
					this._currentWeaponHasScabbard = value;
					base.OnPropertyChangedWithValue(value, "CurrentWeaponHasScabbard");
				}
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x0005E95B File Offset: 0x0005CB5B
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x0005E963 File Offset: 0x0005CB63
		[DataSourceProperty]
		public int CurrentDifficulty
		{
			get
			{
				return this._currentDifficulty;
			}
			set
			{
				if (value != this._currentDifficulty)
				{
					this._currentDifficulty = value;
					base.OnPropertyChangedWithValue(value, "CurrentDifficulty");
				}
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x0005E981 File Offset: 0x0005CB81
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x0005E989 File Offset: 0x0005CB89
		[DataSourceProperty]
		public int CurrentOrderDifficulty
		{
			get
			{
				return this._currentOrderDifficulty;
			}
			set
			{
				if (value != this._currentOrderDifficulty)
				{
					this._currentOrderDifficulty = value;
					base.OnPropertyChangedWithValue(value, "CurrentOrderDifficulty");
				}
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x0005E9A7 File Offset: 0x0005CBA7
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x0005E9AF File Offset: 0x0005CBAF
		[DataSourceProperty]
		public int MaxDifficulty
		{
			get
			{
				return this._maxDifficulty;
			}
			set
			{
				if (value != this._maxDifficulty)
				{
					this._maxDifficulty = value;
					base.OnPropertyChangedWithValue(value, "MaxDifficulty");
				}
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x0005E9CD File Offset: 0x0005CBCD
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x0005E9D5 File Offset: 0x0005CBD5
		[DataSourceProperty]
		public bool IsCurrentHeroAtMaxCraftingSkill
		{
			get
			{
				return this._isCurrentHeroAtMaxCraftingSkill;
			}
			set
			{
				if (value != this._isCurrentHeroAtMaxCraftingSkill)
				{
					this._isCurrentHeroAtMaxCraftingSkill = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentHeroAtMaxCraftingSkill");
				}
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x0005E9F3 File Offset: 0x0005CBF3
		// (set) Token: 0x06001890 RID: 6288 RVA: 0x0005E9FB File Offset: 0x0005CBFB
		[DataSourceProperty]
		public int CurrentHeroCraftingSkill
		{
			get
			{
				return this._currentHeroCraftingSkill;
			}
			set
			{
				if (value != this._currentHeroCraftingSkill)
				{
					this._currentHeroCraftingSkill = value;
					base.OnPropertyChangedWithValue(value, "CurrentHeroCraftingSkill");
				}
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x0005EA19 File Offset: 0x0005CC19
		// (set) Token: 0x06001892 RID: 6290 RVA: 0x0005EA21 File Offset: 0x0005CC21
		[DataSourceProperty]
		public string CurrentDifficultyText
		{
			get
			{
				return this._currentDifficultyText;
			}
			set
			{
				if (value != this._currentDifficultyText)
				{
					this._currentDifficultyText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentDifficultyText");
				}
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06001893 RID: 6291 RVA: 0x0005EA44 File Offset: 0x0005CC44
		// (set) Token: 0x06001894 RID: 6292 RVA: 0x0005EA4C File Offset: 0x0005CC4C
		[DataSourceProperty]
		public string CurrentOrderDifficultyText
		{
			get
			{
				return this._currentOrderDifficultyText;
			}
			set
			{
				if (value != this._currentOrderDifficultyText)
				{
					this._currentOrderDifficultyText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentOrderDifficultyText");
				}
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06001895 RID: 6293 RVA: 0x0005EA6F File Offset: 0x0005CC6F
		// (set) Token: 0x06001896 RID: 6294 RVA: 0x0005EA77 File Offset: 0x0005CC77
		[DataSourceProperty]
		public string CurrentCraftingSkillValueText
		{
			get
			{
				return this._currentCraftingSkillValueText;
			}
			set
			{
				if (value != this._currentCraftingSkillValueText)
				{
					this._currentCraftingSkillValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCraftingSkillValueText");
				}
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x0005EA9A File Offset: 0x0005CC9A
		// (set) Token: 0x06001898 RID: 6296 RVA: 0x0005EAA2 File Offset: 0x0005CCA2
		[DataSourceProperty]
		public string DifficultyText
		{
			get
			{
				return this._difficultyText;
			}
			set
			{
				if (value != this._difficultyText)
				{
					this._difficultyText = value;
					base.OnPropertyChangedWithValue<string>(value, "DifficultyText");
				}
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x0005EAC5 File Offset: 0x0005CCC5
		// (set) Token: 0x0600189A RID: 6298 RVA: 0x0005EACD File Offset: 0x0005CCCD
		[DataSourceProperty]
		public string DefaultUsageText
		{
			get
			{
				return this._defaultUsageText;
			}
			set
			{
				if (value != this._defaultUsageText)
				{
					this._defaultUsageText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefaultUsageText");
				}
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x0600189B RID: 6299 RVA: 0x0005EAF0 File Offset: 0x0005CCF0
		// (set) Token: 0x0600189C RID: 6300 RVA: 0x0005EAF8 File Offset: 0x0005CCF8
		[DataSourceProperty]
		public string AlternativeUsageText
		{
			get
			{
				return this._alternativeUsageText;
			}
			set
			{
				if (value != this._alternativeUsageText)
				{
					this._alternativeUsageText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlternativeUsageText");
				}
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x0005EB1B File Offset: 0x0005CD1B
		// (set) Token: 0x0600189E RID: 6302 RVA: 0x0005EB23 File Offset: 0x0005CD23
		[DataSourceProperty]
		public BasicTooltipViewModel OrderDisabledReasonHint
		{
			get
			{
				return this._orderDisabledReasonHint;
			}
			set
			{
				if (value != this._orderDisabledReasonHint)
				{
					this._orderDisabledReasonHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "OrderDisabledReasonHint");
				}
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x0005EB41 File Offset: 0x0005CD41
		// (set) Token: 0x060018A0 RID: 6304 RVA: 0x0005EB49 File Offset: 0x0005CD49
		[DataSourceProperty]
		public HintViewModel ShowOnlyUnlockedPiecesHint
		{
			get
			{
				return this._showOnlyUnlockedPiecesHint;
			}
			set
			{
				if (value != this._showOnlyUnlockedPiecesHint)
				{
					this._showOnlyUnlockedPiecesHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShowOnlyUnlockedPiecesHint");
				}
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x0005EB67 File Offset: 0x0005CD67
		// (set) Token: 0x060018A2 RID: 6306 RVA: 0x0005EB6F File Offset: 0x0005CD6F
		[DataSourceProperty]
		public BasicTooltipViewModel DifficultyExplanationHint
		{
			get
			{
				return this._difficultyExplanationHint;
			}
			set
			{
				if (value != this._difficultyExplanationHint)
				{
					this._difficultyExplanationHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DifficultyExplanationHint");
				}
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x0005EB8D File Offset: 0x0005CD8D
		// (set) Token: 0x060018A4 RID: 6308 RVA: 0x0005EB95 File Offset: 0x0005CD95
		[DataSourceProperty]
		public CraftingPieceListVM ActivePieceList
		{
			get
			{
				return this._activePieceList;
			}
			set
			{
				if (value != this._activePieceList)
				{
					this._activePieceList = value;
					base.OnPropertyChangedWithValue<CraftingPieceListVM>(value, "ActivePieceList");
				}
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0005EBB3 File Offset: 0x0005CDB3
		// (set) Token: 0x060018A6 RID: 6310 RVA: 0x0005EBBB File Offset: 0x0005CDBB
		[DataSourceProperty]
		public CraftingPieceListVM BladePieceList
		{
			get
			{
				return this._bladePieceList;
			}
			set
			{
				if (value != this._bladePieceList)
				{
					this._bladePieceList = value;
					base.OnPropertyChangedWithValue<CraftingPieceListVM>(value, "BladePieceList");
				}
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x0005EBD9 File Offset: 0x0005CDD9
		// (set) Token: 0x060018A8 RID: 6312 RVA: 0x0005EBE1 File Offset: 0x0005CDE1
		[DataSourceProperty]
		public CraftingPieceListVM GuardPieceList
		{
			get
			{
				return this._guardPieceList;
			}
			set
			{
				if (value != this._guardPieceList)
				{
					this._guardPieceList = value;
					base.OnPropertyChangedWithValue<CraftingPieceListVM>(value, "GuardPieceList");
				}
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x0005EBFF File Offset: 0x0005CDFF
		// (set) Token: 0x060018AA RID: 6314 RVA: 0x0005EC07 File Offset: 0x0005CE07
		[DataSourceProperty]
		public CraftingPieceListVM HandlePieceList
		{
			get
			{
				return this._handlePieceList;
			}
			set
			{
				if (value != this._handlePieceList)
				{
					this._handlePieceList = value;
					base.OnPropertyChangedWithValue<CraftingPieceListVM>(value, "HandlePieceList");
				}
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x0005EC25 File Offset: 0x0005CE25
		// (set) Token: 0x060018AC RID: 6316 RVA: 0x0005EC2D File Offset: 0x0005CE2D
		[DataSourceProperty]
		public CraftingPieceListVM PommelPieceList
		{
			get
			{
				return this._pommelPieceList;
			}
			set
			{
				if (value != this._pommelPieceList)
				{
					this._pommelPieceList = value;
					base.OnPropertyChangedWithValue<CraftingPieceListVM>(value, "PommelPieceList");
				}
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0005EC4B File Offset: 0x0005CE4B
		// (set) Token: 0x060018AE RID: 6318 RVA: 0x0005EC53 File Offset: 0x0005CE53
		[DataSourceProperty]
		public CraftingPieceVM SelectedBladePiece
		{
			get
			{
				return this._selectedBladePiece;
			}
			set
			{
				if (value != this._selectedBladePiece)
				{
					this._selectedBladePiece = value;
					base.OnPropertyChangedWithValue<CraftingPieceVM>(value, "SelectedBladePiece");
				}
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x0005EC71 File Offset: 0x0005CE71
		// (set) Token: 0x060018B0 RID: 6320 RVA: 0x0005EC79 File Offset: 0x0005CE79
		[DataSourceProperty]
		public CraftingPieceVM SelectedGuardPiece
		{
			get
			{
				return this._selectedGuardPiece;
			}
			set
			{
				if (value != this._selectedGuardPiece)
				{
					this._selectedGuardPiece = value;
					base.OnPropertyChangedWithValue<CraftingPieceVM>(value, "SelectedGuardPiece");
				}
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0005EC97 File Offset: 0x0005CE97
		// (set) Token: 0x060018B2 RID: 6322 RVA: 0x0005EC9F File Offset: 0x0005CE9F
		[DataSourceProperty]
		public CraftingPieceVM SelectedHandlePiece
		{
			get
			{
				return this._selectedHandlePiece;
			}
			set
			{
				if (value != this._selectedHandlePiece)
				{
					this._selectedHandlePiece = value;
					base.OnPropertyChangedWithValue<CraftingPieceVM>(value, "SelectedHandlePiece");
				}
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x0005ECBD File Offset: 0x0005CEBD
		// (set) Token: 0x060018B4 RID: 6324 RVA: 0x0005ECC5 File Offset: 0x0005CEC5
		[DataSourceProperty]
		public CraftingPieceVM SelectedPommelPiece
		{
			get
			{
				return this._selectedPommelPiece;
			}
			set
			{
				if (value != this._selectedPommelPiece)
				{
					this._selectedPommelPiece = value;
					base.OnPropertyChangedWithValue<CraftingPieceVM>(value, "SelectedPommelPiece");
				}
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0005ECE4 File Offset: 0x0005CEE4
		// (set) Token: 0x060018B6 RID: 6326 RVA: 0x0005ED48 File Offset: 0x0005CF48
		[DataSourceProperty]
		public int ActivePieceSize
		{
			get
			{
				if (this.ActivePieceList == null)
				{
					return 0;
				}
				switch (this.ActivePieceList.PieceType)
				{
				case CraftingPiece.PieceTypes.Blade:
					return this.BladeSize;
				case CraftingPiece.PieceTypes.Guard:
					return this.GuardSize;
				case CraftingPiece.PieceTypes.Handle:
					return this.HandleSize;
				case CraftingPiece.PieceTypes.Pommel:
					return this.PommelSize;
				}
				return 0;
			}
			set
			{
				if (value == this.ActivePieceSize || this.ActivePieceList == null)
				{
					return;
				}
				switch (this.ActivePieceList.PieceType)
				{
				case CraftingPiece.PieceTypes.Invalid:
				case CraftingPiece.PieceTypes.NumberOfPieceTypes:
					break;
				case CraftingPiece.PieceTypes.Blade:
					this.BladeSize = value;
					return;
				case CraftingPiece.PieceTypes.Guard:
					this.GuardSize = value;
					return;
				case CraftingPiece.PieceTypes.Handle:
					this.HandleSize = value;
					return;
				case CraftingPiece.PieceTypes.Pommel:
					this.PommelSize = value;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x0005EDB3 File Offset: 0x0005CFB3
		// (set) Token: 0x060018B8 RID: 6328 RVA: 0x0005EDBC File Offset: 0x0005CFBC
		[DataSourceProperty]
		public int BladeSize
		{
			get
			{
				return this._bladeSize;
			}
			set
			{
				if (value != this._bladeSize)
				{
					this._bladeSize = value;
					base.OnPropertyChangedWithValue(value, "BladeSize");
					if (this._crafting != null && this._updatePiece && this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(CraftingPiece.PieceTypes.Blade))
					{
						int num = 100 + value;
						this._crafting.ScaleThePiece(CraftingPiece.PieceTypes.Blade, num);
						this.RefreshItem();
					}
					base.OnPropertyChanged("ActivePieceSize");
				}
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0005EE2B File Offset: 0x0005D02B
		// (set) Token: 0x060018BA RID: 6330 RVA: 0x0005EE34 File Offset: 0x0005D034
		[DataSourceProperty]
		public int GuardSize
		{
			get
			{
				return this._guardSize;
			}
			set
			{
				if (value != this._guardSize)
				{
					this._guardSize = value;
					base.OnPropertyChangedWithValue(value, "GuardSize");
					if (this._crafting != null && this._updatePiece && this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(CraftingPiece.PieceTypes.Guard))
					{
						int num = 100 + value;
						this._crafting.ScaleThePiece(CraftingPiece.PieceTypes.Guard, num);
						this.RefreshItem();
					}
					base.OnPropertyChanged("ActivePieceSize");
				}
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x0005EEA3 File Offset: 0x0005D0A3
		// (set) Token: 0x060018BC RID: 6332 RVA: 0x0005EEAC File Offset: 0x0005D0AC
		[DataSourceProperty]
		public int HandleSize
		{
			get
			{
				return this._handleSize;
			}
			set
			{
				if (value != this._handleSize)
				{
					this._handleSize = value;
					base.OnPropertyChangedWithValue(value, "HandleSize");
					if (this._crafting != null && this._updatePiece && this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(CraftingPiece.PieceTypes.Handle))
					{
						int num = 100 + value;
						this._crafting.ScaleThePiece(CraftingPiece.PieceTypes.Handle, num);
						this.RefreshItem();
					}
					base.OnPropertyChanged("ActivePieceSize");
				}
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x0005EF1B File Offset: 0x0005D11B
		// (set) Token: 0x060018BE RID: 6334 RVA: 0x0005EF24 File Offset: 0x0005D124
		[DataSourceProperty]
		public int PommelSize
		{
			get
			{
				return this._pommelSize;
			}
			set
			{
				if (value != this._pommelSize)
				{
					this._pommelSize = value;
					base.OnPropertyChangedWithValue(value, "PommelSize");
					if (this._crafting != null && this._updatePiece && this._crafting.CurrentCraftingTemplate.IsPieceTypeUsable(CraftingPiece.PieceTypes.Pommel))
					{
						int num = 100 + value;
						this._crafting.ScaleThePiece(CraftingPiece.PieceTypes.Pommel, num);
						this.RefreshItem();
					}
					base.OnPropertyChanged("ActivePieceSize");
				}
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x0005EF93 File Offset: 0x0005D193
		// (set) Token: 0x060018C0 RID: 6336 RVA: 0x0005EF9B File Offset: 0x0005D19B
		[DataSourceProperty]
		public string ComponentSizeLbl
		{
			get
			{
				return this._componentSizeLbl;
			}
			set
			{
				if (value != this._componentSizeLbl)
				{
					this._componentSizeLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ComponentSizeLbl");
				}
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x0005EFBE File Offset: 0x0005D1BE
		// (set) Token: 0x060018C2 RID: 6338 RVA: 0x0005EFC6 File Offset: 0x0005D1C6
		[DataSourceProperty]
		public bool IsWeaponCivilian
		{
			get
			{
				return this._isWeaponCivilian;
			}
			set
			{
				if (value != this._isWeaponCivilian)
				{
					this._isWeaponCivilian = value;
					base.OnPropertyChangedWithValue(value, "IsWeaponCivilian");
				}
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x0005EFE4 File Offset: 0x0005D1E4
		// (set) Token: 0x060018C4 RID: 6340 RVA: 0x0005EFEC File Offset: 0x0005D1EC
		[DataSourceProperty]
		public HintViewModel ScabbardHint
		{
			get
			{
				return this._scabbardHint;
			}
			set
			{
				if (value != this._scabbardHint)
				{
					this._scabbardHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ScabbardHint");
				}
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x0005F00A File Offset: 0x0005D20A
		// (set) Token: 0x060018C6 RID: 6342 RVA: 0x0005F012 File Offset: 0x0005D212
		[DataSourceProperty]
		public HintViewModel RandomizeHint
		{
			get
			{
				return this._randomizeHint;
			}
			set
			{
				if (value != this._randomizeHint)
				{
					this._randomizeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RandomizeHint");
				}
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x0005F030 File Offset: 0x0005D230
		// (set) Token: 0x060018C8 RID: 6344 RVA: 0x0005F038 File Offset: 0x0005D238
		[DataSourceProperty]
		public HintViewModel UndoHint
		{
			get
			{
				return this._undoHint;
			}
			set
			{
				if (value != this._undoHint)
				{
					this._undoHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "UndoHint");
				}
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0005F056 File Offset: 0x0005D256
		// (set) Token: 0x060018CA RID: 6346 RVA: 0x0005F05E File Offset: 0x0005D25E
		[DataSourceProperty]
		public HintViewModel RedoHint
		{
			get
			{
				return this._redoHint;
			}
			set
			{
				if (value != this._redoHint)
				{
					this._redoHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RedoHint");
				}
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x0005F07C File Offset: 0x0005D27C
		// (set) Token: 0x060018CC RID: 6348 RVA: 0x0005F084 File Offset: 0x0005D284
		[DataSourceProperty]
		public MBBindingList<ItemFlagVM> WeaponFlagIconsList
		{
			get
			{
				return this._weaponFlagIconsList;
			}
			set
			{
				if (value != this._weaponFlagIconsList)
				{
					this._weaponFlagIconsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ItemFlagVM>>(value, "WeaponFlagIconsList");
				}
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x0005F0A2 File Offset: 0x0005D2A2
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x0005F0AA File Offset: 0x0005D2AA
		[DataSourceProperty]
		public CraftingHistoryVM CraftingHistory
		{
			get
			{
				return this._craftingHistory;
			}
			set
			{
				if (value != this._craftingHistory)
				{
					this._craftingHistory = value;
					base.OnPropertyChangedWithValue<CraftingHistoryVM>(value, "CraftingHistory");
				}
			}
		}

		// Token: 0x04000B10 RID: 2832
		private WeaponDesignVM.CraftingPieceTierFilter _currentTierFilter = WeaponDesignVM.CraftingPieceTierFilter.All;

		// Token: 0x04000B11 RID: 2833
		public const int MAX_SKILL_LEVEL = 300;

		// Token: 0x04000B12 RID: 2834
		public ItemObject CraftedItemObject;

		// Token: 0x04000B13 RID: 2835
		private int _selectedWeaponClassIndex;

		// Token: 0x04000B14 RID: 2836
		private readonly List<CraftingPiece> _newlyUnlockedPieces;

		// Token: 0x04000B15 RID: 2837
		private readonly List<CraftingTemplate> _primaryUsages;

		// Token: 0x04000B16 RID: 2838
		private readonly WeaponDesignVM.PieceTierComparer _pieceTierComparer;

		// Token: 0x04000B17 RID: 2839
		private readonly WeaponDesignVM.TemplateComparer _templateComparer;

		// Token: 0x04000B18 RID: 2840
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000B19 RID: 2841
		private readonly Action _onRefresh;

		// Token: 0x04000B1A RID: 2842
		private readonly Action _onWeaponCrafted;

		// Token: 0x04000B1B RID: 2843
		private readonly Func<CraftingAvailableHeroItemVM> _getCurrentCraftingHero;

		// Token: 0x04000B1C RID: 2844
		private readonly Action<CraftingOrder> _refreshHeroAvailabilities;

		// Token: 0x04000B1D RID: 2845
		private Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> _getItemUsageSetFlags;

		// Token: 0x04000B1E RID: 2846
		private Crafting _crafting;

		// Token: 0x04000B1F RID: 2847
		private bool _updatePiece = true;

		// Token: 0x04000B20 RID: 2848
		private Dictionary<CraftingPiece.PieceTypes, CraftingPieceListVM> _pieceListsDictionary;

		// Token: 0x04000B21 RID: 2849
		private Dictionary<CraftingPiece, CraftingPieceVM> _pieceVMs;

		// Token: 0x04000B22 RID: 2850
		private TextObject _difficultyTextobj = new TextObject("{=cbbUzYX3}Difficulty: {DIFFICULTY}", null);

		// Token: 0x04000B23 RID: 2851
		private TextObject _orderDifficultyTextObj = new TextObject("{=8szijlHj}Order Difficulty: {DIFFICULTY}", null);

		// Token: 0x04000B24 RID: 2852
		private bool _isAutoSelectingPieces;

		// Token: 0x04000B25 RID: 2853
		private bool _shouldRecordHistory;

		// Token: 0x04000B26 RID: 2854
		private MBBindingList<TierFilterTypeVM> _tierFilters;

		// Token: 0x04000B27 RID: 2855
		private string _currentCraftedWeaponTemplateId;

		// Token: 0x04000B28 RID: 2856
		private string _chooseOrderText;

		// Token: 0x04000B29 RID: 2857
		private string _chooseWeaponTypeText;

		// Token: 0x04000B2A RID: 2858
		private string _currentCraftedWeaponTypeText;

		// Token: 0x04000B2B RID: 2859
		private MBBindingList<CraftingPieceListVM> _pieceLists;

		// Token: 0x04000B2C RID: 2860
		private int _selectedPieceTypeIndex;

		// Token: 0x04000B2D RID: 2861
		private bool _showOnlyUnlockedPieces;

		// Token: 0x04000B2E RID: 2862
		private string _missingPropertyWarningText;

		// Token: 0x04000B2F RID: 2863
		private bool _isInFinalCraftingStage;

		// Token: 0x04000B30 RID: 2864
		private string _componentSizeLbl;

		// Token: 0x04000B31 RID: 2865
		private string _itemName;

		// Token: 0x04000B32 RID: 2866
		private string _difficultyText;

		// Token: 0x04000B33 RID: 2867
		private int _bladeSize;

		// Token: 0x04000B34 RID: 2868
		private int _pommelSize;

		// Token: 0x04000B35 RID: 2869
		private int _handleSize;

		// Token: 0x04000B36 RID: 2870
		private int _guardSize;

		// Token: 0x04000B37 RID: 2871
		private CraftingPieceVM _selectedBladePiece;

		// Token: 0x04000B38 RID: 2872
		private CraftingPieceVM _selectedGuardPiece;

		// Token: 0x04000B39 RID: 2873
		private CraftingPieceVM _selectedHandlePiece;

		// Token: 0x04000B3A RID: 2874
		private CraftingPieceVM _selectedPommelPiece;

		// Token: 0x04000B3B RID: 2875
		private CraftingPieceListVM _activePieceList;

		// Token: 0x04000B3C RID: 2876
		private CraftingPieceListVM _bladePieceList;

		// Token: 0x04000B3D RID: 2877
		private CraftingPieceListVM _guardPieceList;

		// Token: 0x04000B3E RID: 2878
		private CraftingPieceListVM _handlePieceList;

		// Token: 0x04000B3F RID: 2879
		private CraftingPieceListVM _pommelPieceList;

		// Token: 0x04000B40 RID: 2880
		private string _alternativeUsageText;

		// Token: 0x04000B41 RID: 2881
		private string _defaultUsageText;

		// Token: 0x04000B42 RID: 2882
		private bool _isScabbardVisible;

		// Token: 0x04000B43 RID: 2883
		private bool _currentWeaponHasScabbard;

		// Token: 0x04000B44 RID: 2884
		public SelectorVM<CraftingSecondaryUsageItemVM> _secondaryUsageSelector;

		// Token: 0x04000B45 RID: 2885
		private ItemCollectionElementViewModel _craftedItemVisual;

		// Token: 0x04000B46 RID: 2886
		private MBBindingList<CraftingListPropertyItem> _primaryPropertyList;

		// Token: 0x04000B47 RID: 2887
		private MBBindingList<WeaponDesignResultPropertyItemVM> _designResultPropertyList;

		// Token: 0x04000B48 RID: 2888
		private int _currentDifficulty;

		// Token: 0x04000B49 RID: 2889
		private int _currentOrderDifficulty;

		// Token: 0x04000B4A RID: 2890
		private int _maxDifficulty;

		// Token: 0x04000B4B RID: 2891
		private string _currentDifficultyText;

		// Token: 0x04000B4C RID: 2892
		private string _currentOrderDifficultyText;

		// Token: 0x04000B4D RID: 2893
		private string _currentCraftingSkillValueText;

		// Token: 0x04000B4E RID: 2894
		private bool _isCurrentHeroAtMaxCraftingSkill;

		// Token: 0x04000B4F RID: 2895
		private int _currentHeroCraftingSkill;

		// Token: 0x04000B50 RID: 2896
		private bool _isWeaponCivilian;

		// Token: 0x04000B51 RID: 2897
		private HintViewModel _scabbardHint;

		// Token: 0x04000B52 RID: 2898
		private HintViewModel _randomizeHint;

		// Token: 0x04000B53 RID: 2899
		private HintViewModel _undoHint;

		// Token: 0x04000B54 RID: 2900
		private HintViewModel _redoHint;

		// Token: 0x04000B55 RID: 2901
		private HintViewModel _showOnlyUnlockedPiecesHint;

		// Token: 0x04000B56 RID: 2902
		private BasicTooltipViewModel _difficultyExplanationHint;

		// Token: 0x04000B57 RID: 2903
		private BasicTooltipViewModel _orderDisabledReasonHint;

		// Token: 0x04000B58 RID: 2904
		private CraftingOrderItemVM _activeCraftingOrder;

		// Token: 0x04000B59 RID: 2905
		private CraftingOrderPopupVM _craftingOrderPopup;

		// Token: 0x04000B5A RID: 2906
		private WeaponClassSelectionPopupVM _weaponClassSelectionPopup;

		// Token: 0x04000B5B RID: 2907
		private string _freeModeButtonText;

		// Token: 0x04000B5C RID: 2908
		private bool _isOrderButtonActive;

		// Token: 0x04000B5D RID: 2909
		private bool _isInOrderMode;

		// Token: 0x04000B5E RID: 2910
		private WeaponDesignResultPopupVM _craftingResultPopup;

		// Token: 0x04000B5F RID: 2911
		private MBBindingList<ItemFlagVM> _weaponFlagIconsList;

		// Token: 0x04000B60 RID: 2912
		private CraftingHistoryVM _craftingHistory;

		// Token: 0x04000B61 RID: 2913
		private TextObject _currentCraftingSkillText;

		// Token: 0x02000260 RID: 608
		[Flags]
		public enum CraftingPieceTierFilter
		{
			// Token: 0x0400129E RID: 4766
			None = 0,
			// Token: 0x0400129F RID: 4767
			Tier1 = 1,
			// Token: 0x040012A0 RID: 4768
			Tier2 = 2,
			// Token: 0x040012A1 RID: 4769
			Tier3 = 4,
			// Token: 0x040012A2 RID: 4770
			Tier4 = 8,
			// Token: 0x040012A3 RID: 4771
			Tier5 = 16,
			// Token: 0x040012A4 RID: 4772
			All = 31
		}

		// Token: 0x02000261 RID: 609
		public class PieceTierComparer : IComparer<CraftingPieceVM>
		{
			// Token: 0x0600256D RID: 9581 RVA: 0x00081A90 File Offset: 0x0007FC90
			public int Compare(CraftingPieceVM x, CraftingPieceVM y)
			{
				if (x.Tier != y.Tier)
				{
					return x.Tier.CompareTo(y.Tier);
				}
				return x.CraftingPiece.CraftingPiece.StringId.CompareTo(y.CraftingPiece.CraftingPiece.StringId);
			}
		}

		// Token: 0x02000262 RID: 610
		public class TemplateComparer : IComparer<CraftingTemplate>
		{
			// Token: 0x0600256F RID: 9583 RVA: 0x00081AED File Offset: 0x0007FCED
			public int Compare(CraftingTemplate x, CraftingTemplate y)
			{
				return string.Compare(x.StringId, y.StringId, StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x02000263 RID: 611
		public class WeaponPropertyComparer : IComparer<CraftingListPropertyItem>
		{
			// Token: 0x06002571 RID: 9585 RVA: 0x00081B0C File Offset: 0x0007FD0C
			public int Compare(CraftingListPropertyItem x, CraftingListPropertyItem y)
			{
				return ((int)x.Type).CompareTo((int)y.Type);
			}
		}
	}
}
