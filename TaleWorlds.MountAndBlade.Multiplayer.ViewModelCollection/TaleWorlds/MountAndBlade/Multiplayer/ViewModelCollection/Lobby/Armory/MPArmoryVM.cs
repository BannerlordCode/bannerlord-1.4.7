using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x0200007C RID: 124
	public class MPArmoryVM : ViewModel
	{
		// Token: 0x06000C46 RID: 3142 RVA: 0x0002615C File Offset: 0x0002435C
		public MPArmoryVM(Action<BasicCharacterObject> onOpenFacegen, Action<MPArmoryCosmeticItemBaseVM> onItemObtainRequested, Func<string> getExitText)
		{
			this._getExitText = getExitText;
			this._onOpenFacegen = onOpenFacegen;
			this._character = MBObjectManager.Instance.GetObject<BasicCharacterObject>("mp_character");
			this.CanOpenFacegen = true;
			this.ClassFilter = new MPLobbyClassFilterVM(new Action<MPLobbyClassFilterClassItemVM, bool>(this.OnSelectedClassChanged));
			this.HeroPreview = new MPArmoryHeroPreviewVM();
			this.ClassStats = new MPArmoryClassStatsVM();
			this.HeroPerkSelection = new MPArmoryHeroPerkSelectionVM(new Action<HeroPerkVM, MPPerkVM>(this.OnSelectPerk), new Action(this.ForceRefreshCharacter));
			this.Cosmetics = new MPArmoryCosmeticsVM(new Func<List<IReadOnlyPerkObject>>(this.GetSelectedPerks));
			this.InitalizeCallbacks();
			this.RefreshValues();
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0002620C File Offset: 0x0002440C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.StatsText = new TextObject("{=ffjTMejn}Stats", null).ToString();
			this.CustomizationText = new TextObject("{=sPkRekRL}Customization", null).ToString();
			this.FacegenText = new TextObject("{=RSx1e5Wf}Edit Character", null).ToString();
			this.RefreshManageTauntButtonText();
			this.ClassFilter.RefreshValues();
			this.HeroPreview.RefreshValues();
			this.ClassStats.RefreshValues();
			this.Cosmetics.RefreshValues();
			this.HeroPerkSelection.RefreshValues();
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0002629E File Offset: 0x0002449E
		private void RefreshManageTauntButtonText()
		{
			this.ManageTauntsText = (this.IsManagingTaunts ? new TextObject("{=WiNRdfsm}Done", null).ToString() : new TextObject("{=58O7bWrD}Manage Taunts", null).ToString());
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000262D0 File Offset: 0x000244D0
		public override void OnFinalize()
		{
			this.HeroPreview = null;
			this._character = null;
			this.FinalizeCallbacks();
			this.Cosmetics.OnFinalize();
			base.OnFinalize();
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000262F8 File Offset: 0x000244F8
		private void InitalizeCallbacks()
		{
			CharacterViewModel.OnCustomAnimationFinished = (Action<CharacterViewModel>)Delegate.Combine(CharacterViewModel.OnCustomAnimationFinished, new Action<CharacterViewModel>(this.OnCharacterCustomAnimationFinished));
			MPArmoryCosmeticsVM.OnCosmeticPreview += this.OnHeroPreviewItemEquipped;
			MPArmoryCosmeticsVM.OnRemoveCosmeticFromPreview += this.RemoveHeroPreviewItem;
			MPArmoryCosmeticsVM.OnTauntAssignmentRefresh += this.OnTauntAssignmentRefresh;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00026358 File Offset: 0x00024558
		private void FinalizeCallbacks()
		{
			CharacterViewModel.OnCustomAnimationFinished = (Action<CharacterViewModel>)Delegate.Remove(CharacterViewModel.OnCustomAnimationFinished, new Action<CharacterViewModel>(this.OnCharacterCustomAnimationFinished));
			MPArmoryCosmeticsVM.OnCosmeticPreview -= this.OnHeroPreviewItemEquipped;
			MPArmoryCosmeticsVM.OnRemoveCosmeticFromPreview -= this.RemoveHeroPreviewItem;
			MPArmoryCosmeticsVM.OnTauntAssignmentRefresh -= this.OnTauntAssignmentRefresh;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x000263B8 File Offset: 0x000245B8
		public void OnTick(float dt)
		{
			if (this._tauntItemToRefreshNextAnimationWith != null)
			{
				MPArmoryCosmeticTauntItemVM tauntItemToRefreshNextAnimationWith = this._tauntItemToRefreshNextAnimationWith;
				Equipment equipment = LobbyTauntHelper.PrepareForTaunt(Equipment.CreateFromEquipmentCode(this.HeroPreview.HeroVisual.EquipmentCode), tauntItemToRefreshNextAnimationWith.TauntCosmeticElement, false);
				EquipmentIndex equipmentIndex;
				EquipmentIndex equipmentIndex2;
				bool flag;
				equipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
				this.HeroPreview.HeroVisual.EquipmentCode = equipment.CalculateEquipmentCode();
				this.HeroPreview.HeroVisual.RightHandWieldedEquipmentIndex = (int)equipmentIndex;
				if (!flag)
				{
					this.HeroPreview.HeroVisual.LeftHandWieldedEquipmentIndex = (int)equipmentIndex2;
				}
				this.HeroPreview.HeroVisual.ExecuteStartCustomAnimation(CosmeticsManagerHelper.GetSuitableTauntActionForEquipment(equipment, tauntItemToRefreshNextAnimationWith.TauntCosmeticElement), false, 0.25f);
				this._currentTauntPreviewAnimationSource = this._tauntItemToRefreshNextAnimationWith;
				this._tauntItemToRefreshNextAnimationWith = null;
			}
			if (this.HeroPreview.HeroVisual.IsPlayingCustomAnimations && this._currentTauntPreviewAnimationSource != null)
			{
				float num = MathF.Clamp(this.HeroPreview.HeroVisual.CustomAnimationProgressRatio, 0f, 1f);
				this._currentTauntPreviewAnimationSource.PreviewAnimationRatio = (float)((int)(num * 100f));
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000264C8 File Offset: 0x000246C8
		public void RefreshPlayerData(PlayerData playerData)
		{
			if (this._character != null)
			{
				this._character.UpdatePlayerCharacterBodyProperties(playerData.BodyProperties, playerData.Race, playerData.IsFemale);
				this._character.Age = playerData.BodyProperties.Age;
				this.HeroPreview.SetCharacter(this._character, playerData.BodyProperties.DynamicProperties, playerData.Race, playerData.IsFemale);
				this.ForceRefreshCharacter();
				this.Cosmetics.RefreshPlayerData(playerData);
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00026550 File Offset: 0x00024750
		public void ForceRefreshCharacter()
		{
			this.OnSelectedClassChanged(this.ClassFilter.SelectedClassItem, true);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00026564 File Offset: 0x00024764
		private void OnIsEnabledChanged()
		{
			PlayerData playerData = NetworkMain.GameClient.PlayerData;
			if (this.IsEnabled && playerData != null)
			{
				this.RefreshPlayerData(playerData);
			}
			if (this.IsEnabled)
			{
				this.Cosmetics.RefreshCosmeticInfoFromNetwork();
				if (this.IsManagingTaunts)
				{
					this.ExecuteToggleManageTauntsState();
					return;
				}
			}
			else
			{
				MPArmoryHeroPerkSelectionVM heroPerkSelection = this.HeroPerkSelection;
				foreach (HeroPerkVM heroPerkVM in ((heroPerkSelection != null) ? heroPerkSelection.Perks : null))
				{
					BasicTooltipViewModel hint = heroPerkVM.Hint;
					if (hint != null)
					{
						hint.ExecuteEndHint();
					}
				}
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00026604 File Offset: 0x00024804
		private void OnSelectedClassChanged(MPLobbyClassFilterClassItemVM selectedClassItem, bool forceUpdate = false)
		{
			if (this.HeroPreview == null || this.ClassStats == null || this.HeroPerkSelection == null)
			{
				return;
			}
			if (this._currentClassItem == selectedClassItem && !forceUpdate)
			{
				return;
			}
			this._currentClassItem = selectedClassItem;
			this.HeroPerkSelection.RefreshPerksListWithHero(selectedClassItem.HeroClass);
			this.HeroPreview.SetCharacterClass(selectedClassItem.HeroClass.HeroCharacter);
			this.HeroPreview.SetCharacterPerks(this.HeroPerkSelection.CurrentSelectedPerks);
			this.ClassStats.RefreshWith(selectedClassItem.HeroClass);
			this.ClassStats.HeroInformation.RefreshWith(this.HeroPerkSelection.CurrentHeroClass, this.HeroPerkSelection.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>());
			this.Cosmetics.RefreshSelectedClass(selectedClassItem.HeroClass, this.HeroPerkSelection.CurrentSelectedPerks);
			this.Cosmetics.RefreshCosmeticInfoFromNetwork();
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00026702 File Offset: 0x00024902
		public void SetCanOpenFacegen(bool enabled)
		{
			this.CanOpenFacegen = enabled;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0002670B File Offset: 0x0002490B
		private void ExecuteOpenFaceGen()
		{
			Action<BasicCharacterObject> onOpenFacegen = this._onOpenFacegen;
			if (onOpenFacegen == null)
			{
				return;
			}
			onOpenFacegen(this._character);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00026723 File Offset: 0x00024923
		public void ExecuteClearTauntSelection()
		{
			this.Cosmetics.ClearTauntSelections();
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00026730 File Offset: 0x00024930
		public void ExecuteToggleManageTauntsState()
		{
			this.IsManagingTaunts = !this.IsManagingTaunts;
			if (this.IsManagingTaunts)
			{
				this._canOpenFacegenBeforeTauntState = this.CanOpenFacegen;
				this.Cosmetics.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Taunt);
				this.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM s)
				{
					s.IsEnabled = true;
				});
				this.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM s)
				{
					s.IsFocused = false;
				});
			}
			else
			{
				this.Cosmetics.RefreshAvailableCategoriesBy(CosmeticsManager.CosmeticType.Clothing);
				this.Cosmetics.TauntSlots.ApplyActionOnAllItems(delegate(MPArmoryCosmeticTauntSlotVM s)
				{
					s.IsEnabled = false;
				});
				this.Cosmetics.ClearTauntSelections();
			}
			this.CanOpenFacegen = !this.IsManagingTaunts && this._canOpenFacegenBeforeTauntState;
			this.RefreshManageTauntButtonText();
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00026830 File Offset: 0x00024A30
		public void ExecuteSelectFocusedSlot()
		{
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.Cosmetics.TauntSlots)
			{
				if (mparmoryCosmeticTauntSlotVM.IsFocused)
				{
					mparmoryCosmeticTauntSlotVM.ExecuteSelect();
					break;
				}
			}
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0002688C File Offset: 0x00024A8C
		public void ExecuteEmptyFocusedSlot()
		{
			foreach (MPArmoryCosmeticTauntSlotVM mparmoryCosmeticTauntSlotVM in this.Cosmetics.TauntSlots)
			{
				if (mparmoryCosmeticTauntSlotVM.IsFocused)
				{
					MPArmoryCosmeticTauntItemVM assignedTauntItem = mparmoryCosmeticTauntSlotVM.AssignedTauntItem;
					if (assignedTauntItem != null && assignedTauntItem.IsUsed)
					{
						mparmoryCosmeticTauntSlotVM.AssignedTauntItem.ExecuteAction();
					}
					mparmoryCosmeticTauntSlotVM.EmptySlotKeyVisual.SetForcedVisibility(new bool?(false));
					mparmoryCosmeticTauntSlotVM.SelectKeyVisual.SetForcedVisibility(new bool?(false));
					break;
				}
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00026924 File Offset: 0x00024B24
		private void OnSelectPerk(HeroPerkVM heroPerk, MPPerkVM candidate)
		{
			if (this.ClassStats.HeroInformation.HeroClass != null && this.HeroPerkSelection.CurrentHeroClass != null)
			{
				List<IReadOnlyPerkObject> currentSelectedPerks = this.HeroPerkSelection.CurrentSelectedPerks;
				if (currentSelectedPerks.Count > 0)
				{
					this.ClassStats.HeroInformation.RefreshWith(this.HeroPerkSelection.CurrentHeroClass, currentSelectedPerks);
					this.HeroPreview.SetCharacterPerks(currentSelectedPerks);
					this.Cosmetics.RefreshSelectedClass(this._currentClassItem.HeroClass, currentSelectedPerks);
				}
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x000269A4 File Offset: 0x00024BA4
		private void RemoveHeroPreviewItem(MPArmoryCosmeticItemBaseVM itemVM)
		{
			MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
			if ((mparmoryCosmeticClothingItemVM = itemVM as MPArmoryCosmeticClothingItemVM) != null)
			{
				EquipmentIndex cosmeticEquipmentIndex = mparmoryCosmeticClothingItemVM.EquipmentElement.Item.GetCosmeticEquipmentIndex();
				MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
				if (heroPreview != null)
				{
					heroPreview.HeroVisual.SetEquipment(cosmeticEquipmentIndex, default(EquipmentElement));
				}
				MPArmoryHeroPreviewVM heroPreview2 = this.HeroPreview;
				string text = ((heroPreview2 != null) ? heroPreview2.HeroVisual.EquipmentCode : null);
				if (!string.IsNullOrEmpty(text))
				{
					this._lastValidEquipment = Equipment.CreateFromEquipmentCode(text);
				}
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00026A1C File Offset: 0x00024C1C
		private void OnHeroPreviewItemEquipped(MPArmoryCosmeticItemBaseVM itemVM)
		{
			MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
			MPArmoryCosmeticTauntItemVM mparmoryCosmeticTauntItemVM;
			if ((mparmoryCosmeticClothingItemVM = itemVM as MPArmoryCosmeticClothingItemVM) != null)
			{
				EquipmentElement equipmentElement = mparmoryCosmeticClothingItemVM.EquipmentElement;
				EquipmentIndex cosmeticEquipmentIndex = equipmentElement.Item.GetCosmeticEquipmentIndex();
				MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
				if (heroPreview != null)
				{
					heroPreview.HeroVisual.SetEquipment(cosmeticEquipmentIndex, equipmentElement);
				}
				MPArmoryHeroPreviewVM heroPreview2 = this.HeroPreview;
				string text = ((heroPreview2 != null) ? heroPreview2.HeroVisual.EquipmentCode : null);
				if (!string.IsNullOrEmpty(text))
				{
					this._lastValidEquipment = Equipment.CreateFromEquipmentCode(text);
					return;
				}
			}
			else if ((mparmoryCosmeticTauntItemVM = itemVM as MPArmoryCosmeticTauntItemVM) != null)
			{
				MPArmoryHeroPreviewVM heroPreview3 = this.HeroPreview;
				if (((heroPreview3 != null) ? heroPreview3.HeroVisual : null) != null)
				{
					this.HeroPreview.HeroVisual.ExecuteStopCustomAnimation();
					if (this._lastValidEquipment != null)
					{
						this.HeroPreview.HeroVisual.SetEquipment(this._lastValidEquipment);
					}
				}
				this._tauntItemToRefreshNextAnimationWith = mparmoryCosmeticTauntItemVM;
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00026AE4 File Offset: 0x00024CE4
		private void OnCharacterCustomAnimationFinished(CharacterViewModel character)
		{
			if (character == this.HeroPreview.HeroVisual && this._lastValidEquipment != null)
			{
				MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
				if (((heroPreview != null) ? heroPreview.HeroVisual : null) != null)
				{
					this.HeroPreview.HeroVisual.SetEquipment(this._lastValidEquipment);
					this.HeroPreview.HeroVisual.LeftHandWieldedEquipmentIndex = -1;
					this.HeroPreview.HeroVisual.RightHandWieldedEquipmentIndex = -1;
					this._currentTauntPreviewAnimationSource.PreviewAnimationRatio = 0f;
					this._currentTauntPreviewAnimationSource = null;
				}
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00026B6A File Offset: 0x00024D6A
		private void OnTauntAssignmentRefresh()
		{
			this.IsTauntAssignmentActive = this.Cosmetics.SelectedTauntItem != null || this.Cosmetics.SelectedTauntSlot != null;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00026B90 File Offset: 0x00024D90
		private void ResetHeroEquipment()
		{
			MPArmoryHeroPreviewVM heroPreview = this.HeroPreview;
			if (heroPreview == null)
			{
				return;
			}
			heroPreview.HeroVisual.SetEquipment(new Equipment());
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00026BAC File Offset: 0x00024DAC
		public static void ApplyPerkEffectsToEquipment(ref Equipment equipment, List<IReadOnlyPerkObject> selectedPerks)
		{
			MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(selectedPerks);
			IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null);
			if (enumerable != null)
			{
				foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable)
				{
					equipment[valueTuple.Item1] = valueTuple.Item2;
				}
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00026C18 File Offset: 0x00024E18
		private List<IReadOnlyPerkObject> GetSelectedPerks()
		{
			return this.HeroPerkSelection.CurrentSelectedPerks;
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00026C25 File Offset: 0x00024E25
		// (set) Token: 0x06000C60 RID: 3168 RVA: 0x00026C2D File Offset: 0x00024E2D
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					this.OnIsEnabledChanged();
				}
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00026C51 File Offset: 0x00024E51
		// (set) Token: 0x06000C62 RID: 3170 RVA: 0x00026C59 File Offset: 0x00024E59
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
					this.Cosmetics.IsManagingTaunts = value;
				}
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00026C83 File Offset: 0x00024E83
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x00026C8B File Offset: 0x00024E8B
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
					if (this._isTauntAssignmentActive)
					{
						Func<string> getExitText = this._getExitText;
						this.TauntAssignmentClickToCloseText = ((getExitText != null) ? getExitText() : null);
					}
				}
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00026CC9 File Offset: 0x00024EC9
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x00026CD1 File Offset: 0x00024ED1
		[DataSourceProperty]
		public bool CanOpenFacegen
		{
			get
			{
				return this._canOpenFacegen;
			}
			set
			{
				if (value != this._canOpenFacegen)
				{
					this._canOpenFacegen = value;
					base.OnPropertyChangedWithValue(value, "CanOpenFacegen");
				}
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00026CEF File Offset: 0x00024EEF
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x00026CF7 File Offset: 0x00024EF7
		[DataSourceProperty]
		public MPLobbyClassFilterVM ClassFilter
		{
			get
			{
				return this._classFilter;
			}
			set
			{
				if (value != this._classFilter)
				{
					this._classFilter = value;
					base.OnPropertyChangedWithValue<MPLobbyClassFilterVM>(value, "ClassFilter");
				}
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00026D15 File Offset: 0x00024F15
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x00026D1D File Offset: 0x00024F1D
		[DataSourceProperty]
		public MPArmoryHeroPreviewVM HeroPreview
		{
			get
			{
				return this._heroPreview;
			}
			set
			{
				if (value != this._heroPreview)
				{
					this._heroPreview = value;
					base.OnPropertyChangedWithValue<MPArmoryHeroPreviewVM>(value, "HeroPreview");
				}
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00026D3B File Offset: 0x00024F3B
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x00026D43 File Offset: 0x00024F43
		[DataSourceProperty]
		public MPArmoryClassStatsVM ClassStats
		{
			get
			{
				return this._classStats;
			}
			set
			{
				if (value != this._classStats)
				{
					this._classStats = value;
					base.OnPropertyChangedWithValue<MPArmoryClassStatsVM>(value, "ClassStats");
				}
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00026D61 File Offset: 0x00024F61
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x00026D69 File Offset: 0x00024F69
		[DataSourceProperty]
		public MPArmoryHeroPerkSelectionVM HeroPerkSelection
		{
			get
			{
				return this._heroPerkSelection;
			}
			set
			{
				if (value != this._heroPerkSelection)
				{
					this._heroPerkSelection = value;
					base.OnPropertyChangedWithValue<MPArmoryHeroPerkSelectionVM>(value, "HeroPerkSelection");
				}
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00026D87 File Offset: 0x00024F87
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x00026D8F File Offset: 0x00024F8F
		[DataSourceProperty]
		public MPArmoryCosmeticsVM Cosmetics
		{
			get
			{
				return this._cosmetics;
			}
			set
			{
				if (value != this._cosmetics)
				{
					this._cosmetics = value;
					base.OnPropertyChangedWithValue<MPArmoryCosmeticsVM>(value, "Cosmetics");
				}
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00026DAD File Offset: 0x00024FAD
		// (set) Token: 0x06000C72 RID: 3186 RVA: 0x00026DB5 File Offset: 0x00024FB5
		[DataSourceProperty]
		public string TauntAssignmentClickToCloseText
		{
			get
			{
				return this._tauntAssignmentClickToCloseText;
			}
			set
			{
				if (value != this._tauntAssignmentClickToCloseText)
				{
					this._tauntAssignmentClickToCloseText = value;
					base.OnPropertyChangedWithValue<string>(value, "TauntAssignmentClickToCloseText");
				}
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00026DD8 File Offset: 0x00024FD8
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x00026DE0 File Offset: 0x00024FE0
		[DataSourceProperty]
		public string StatsText
		{
			get
			{
				return this._statsText;
			}
			set
			{
				if (value != this._statsText)
				{
					this._statsText = value;
					base.OnPropertyChangedWithValue<string>(value, "StatsText");
				}
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00026E03 File Offset: 0x00025003
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x00026E0B File Offset: 0x0002500B
		[DataSourceProperty]
		public string CustomizationText
		{
			get
			{
				return this._customizationText;
			}
			set
			{
				if (value != this._customizationText)
				{
					this._customizationText = value;
					base.OnPropertyChangedWithValue<string>(value, "CustomizationText");
				}
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00026E2E File Offset: 0x0002502E
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x00026E36 File Offset: 0x00025036
		[DataSourceProperty]
		public string FacegenText
		{
			get
			{
				return this._facegenText;
			}
			set
			{
				if (value != this._facegenText)
				{
					this._facegenText = value;
					base.OnPropertyChangedWithValue<string>(value, "FacegenText");
				}
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00026E59 File Offset: 0x00025059
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x00026E61 File Offset: 0x00025061
		[DataSourceProperty]
		public string ManageTauntsText
		{
			get
			{
				return this._manageTauntsText;
			}
			set
			{
				if (value != this._manageTauntsText)
				{
					this._manageTauntsText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageTauntsText");
				}
			}
		}

		// Token: 0x04000595 RID: 1429
		private readonly Action<BasicCharacterObject> _onOpenFacegen;

		// Token: 0x04000596 RID: 1430
		private bool _canOpenFacegenBeforeTauntState;

		// Token: 0x04000597 RID: 1431
		private BasicCharacterObject _character;

		// Token: 0x04000598 RID: 1432
		private MPLobbyClassFilterClassItemVM _currentClassItem;

		// Token: 0x04000599 RID: 1433
		private Equipment _lastValidEquipment;

		// Token: 0x0400059A RID: 1434
		private Func<string> _getExitText;

		// Token: 0x0400059B RID: 1435
		private MPArmoryCosmeticTauntItemVM _tauntItemToRefreshNextAnimationWith;

		// Token: 0x0400059C RID: 1436
		private MPArmoryCosmeticTauntItemVM _currentTauntPreviewAnimationSource;

		// Token: 0x0400059D RID: 1437
		private bool _isEnabled;

		// Token: 0x0400059E RID: 1438
		private bool _isManagingTaunts;

		// Token: 0x0400059F RID: 1439
		private bool _isTauntAssignmentActive;

		// Token: 0x040005A0 RID: 1440
		private bool _canOpenFacegen;

		// Token: 0x040005A1 RID: 1441
		private MPLobbyClassFilterVM _classFilter;

		// Token: 0x040005A2 RID: 1442
		private MPArmoryHeroPreviewVM _heroPreview;

		// Token: 0x040005A3 RID: 1443
		private MPArmoryClassStatsVM _classStats;

		// Token: 0x040005A4 RID: 1444
		private MPArmoryHeroPerkSelectionVM _heroPerkSelection;

		// Token: 0x040005A5 RID: 1445
		private MPArmoryCosmeticsVM _cosmetics;

		// Token: 0x040005A6 RID: 1446
		private string _tauntAssignmentClickToCloseText;

		// Token: 0x040005A7 RID: 1447
		private string _statsText;

		// Token: 0x040005A8 RID: 1448
		private string _customizationText;

		// Token: 0x040005A9 RID: 1449
		private string _facegenText;

		// Token: 0x040005AA RID: 1450
		private string _manageTauntsText;
	}
}
