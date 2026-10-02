using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Options;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys;
using TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GamepadOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000073 RID: 115
	public class OptionsVM : ViewModel
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0001E02E File Offset: 0x0001C22E
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x0001E036 File Offset: 0x0001C236
		public OptionsVM.OptionsMode CurrentOptionsMode { get; private set; }

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001E040 File Offset: 0x0001C240
		public OptionsVM(bool autoHandleClose, OptionsVM.OptionsMode optionsMode, Action<KeyOptionVM> onKeybindRequest, Action onBrightnessExecute = null, Action onExposureExecute = null)
		{
			this._onKeybindRequest = onKeybindRequest;
			this._autoHandleClose = autoHandleClose;
			this.CurrentOptionsMode = optionsMode;
			this._onBrightnessExecute = onBrightnessExecute;
			this._onExposureExecute = onExposureExecute;
			this._groupedCategories = new List<GroupedOptionCategoryVM>();
			NativeOptions.RefreshOptionsData();
			bool flag = this.CurrentOptionsMode == OptionsVM.OptionsMode.Multiplayer;
			bool flag2 = this.CurrentOptionsMode == OptionsVM.OptionsMode.MainMenu;
			this._gameplayOptionCategory = new GroupedOptionCategoryVM(this, new TextObject("{=2zcrC0h1}Gameplay", null), OptionsProvider.GetGameplayOptionCategory(flag2, flag), true, true);
			this._audioOptionCategory = new GroupedOptionCategoryVM(this, new TextObject("{=xebFLnH2}Audio", null), OptionsProvider.GetAudioOptionCategory(flag), true, false);
			this._videoOptionCategory = new GroupedOptionCategoryVM(this, new TextObject("{=gamevideo}Video", null), OptionsProvider.GetVideoOptionCategory(flag2, new Action(this.OnBrightnessClick), new Action(this.OnExposureClick), new Action(this.ExecuteBenchmark)), true, false);
			bool flag3 = true;
			this._performanceOptionCategory = new GroupedOptionCategoryVM(this, new TextObject("{=fM9E7frB}Performance", null), OptionsProvider.GetPerformanceOptionCategory(flag), flag3, false);
			this._groupedCategories.Add(this._videoOptionCategory);
			this._groupedCategories.Add(this._audioOptionCategory);
			this._groupedCategories.Add(this._gameplayOptionCategory);
			this._performanceManagedOptions = this._performanceOptionCategory.GetManagedOptions();
			this._gameKeyCategory = new GameKeyOptionCategoryVM(this._onKeybindRequest, OptionsProvider.GetGameKeyCategoriesList(this.CurrentOptionsMode == OptionsVM.OptionsMode.Multiplayer), OptionsProvider.GetHiddenGameKeys(ModuleHelper.IsModuleActive("NavalDLC")));
			TextObject textObject = new TextObject("{=SQpGQzTI}Controller", null);
			this._gamepadCategory = new GamepadOptionCategoryVM(this, textObject, OptionsProvider.GetControllerOptionCategory(), true, true);
			this._categories = new List<ViewModel>();
			this._categories.Add(this._videoOptionCategory);
			this._categories.Add(this._performanceOptionCategory);
			this._categories.Add(this._audioOptionCategory);
			this._categories.Add(this._gameplayOptionCategory);
			this._categories.Add(this._gameKeyCategory);
			this._categories.Add(this._gamepadCategory);
			this.SetSelectedCategory(0);
			if (onBrightnessExecute == null)
			{
				this.BrightnessPopUp = new BrightnessOptionVM(null);
			}
			if (onExposureExecute == null)
			{
				this.ExposurePopUp = new ExposureOptionVM(null);
			}
			if (Game.Current != null && this._autoHandleClose)
			{
				Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
			}
			this._refreshRateOption = this.VideoOptions.GetOption(NativeOptions.NativeOptionsType.RefreshRate);
			this._resolutionOption = this.VideoOptions.GetOption(NativeOptions.NativeOptionsType.ScreenResolution);
			this._monitorOption = this.VideoOptions.GetOption(NativeOptions.NativeOptionsType.SelectedMonitor);
			this._displayModeOption = this.VideoOptions.GetOption(NativeOptions.NativeOptionsType.DisplayMode);
			this._overallOption = this.PerformanceOptions.GetOption(NativeOptions.NativeOptionsType.OverAll) as StringOptionDataVM;
			this._dlssOption = this.PerformanceOptions.GetOption(NativeOptions.NativeOptionsType.DLSS);
			this._dynamicResolutionOptions = new List<GenericOptionDataVM>
			{
				this.PerformanceOptions.GetOption(NativeOptions.NativeOptionsType.DynamicResolution),
				this.PerformanceOptions.GetOption(NativeOptions.NativeOptionsType.DynamicResolutionTarget)
			};
			this.IsConsole = true;
			GroupedOptionCategoryVM performanceOptionCategory = this._performanceOptionCategory;
			if (performanceOptionCategory != null)
			{
				performanceOptionCategory.InitializeDependentConfigs(new Action<IOptionData, float>(this.UpdateDependentConfigs));
			}
			this.IsConsole = false;
			this.RefreshValues();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			this._isInitialized = true;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001E3A4 File Offset: 0x0001C5A4
		public OptionsVM(OptionsVM.OptionsMode optionsMode, Action onClose, Action<KeyOptionVM> onKeybindRequest, Action onBrightnessExecute = null, Action onExposureExecute = null)
			: this(false, optionsMode, onKeybindRequest, null, null)
		{
			this._onClose = onClose;
			this._onBrightnessExecute = onBrightnessExecute;
			this._onExposureExecute = onExposureExecute;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001E3C8 File Offset: 0x0001C5C8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OptionsLbl = new TextObject("{=NqarFr4P}Options", null).ToString();
			this.CancelLbl = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.DoneLbl = new TextObject("{=WiNRdfsm}Done", null).ToString();
			this.ResetLbl = new TextObject("{=mAxXKaXp}Reset", null).ToString();
			this.VideoMemoryUsageName = Module.CurrentModule.GlobalTextManager.FindText("str_gpu_memory_usage", null).ToString();
			this.GameKeyOptionGroups.RefreshValues();
			this.GamepadOptions.RefreshValues();
			BrightnessOptionVM brightnessPopUp = this.BrightnessPopUp;
			if (brightnessPopUp != null)
			{
				brightnessPopUp.RefreshValues();
			}
			ExposureOptionVM exposurePopUp = this.ExposurePopUp;
			if (exposurePopUp != null)
			{
				exposurePopUp.RefreshValues();
			}
			this.UpdateVideoMemoryUsage();
			this._categories.ForEach(delegate(ViewModel g)
			{
				g.RefreshValues();
			});
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001E4BB File Offset: 0x0001C6BB
		public void ExecuteCloseOptions()
		{
			if (this._onClose != null)
			{
				this._onClose();
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001E4D0 File Offset: 0x0001C6D0
		protected void OnBrightnessClick()
		{
			if (this._onBrightnessExecute == null)
			{
				this.BrightnessPopUp.Visible = true;
				return;
			}
			this._onBrightnessExecute();
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001E4F2 File Offset: 0x0001C6F2
		protected void OnExposureClick()
		{
			if (this._onExposureExecute == null)
			{
				this.ExposurePopUp.Visible = true;
				return;
			}
			this._onExposureExecute();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001E514 File Offset: 0x0001C714
		public ViewModel GetActiveCategory()
		{
			if (this.CategoryIndex >= 0 && this.CategoryIndex < this._categories.Count)
			{
				return this._categories[this.CategoryIndex];
			}
			return null;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001E545 File Offset: 0x0001C745
		public int GetIndexOfCategory(ViewModel categoryVM)
		{
			return this._categories.IndexOf(categoryVM);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001E554 File Offset: 0x0001C754
		public float GetConfig(IOptionData data)
		{
			if (!data.IsNative())
			{
				return ManagedOptions.GetConfig((ManagedOptions.ManagedOptionsType)data.GetOptionType());
			}
			NativeOptions.NativeOptionsType nativeOptionsType = (NativeOptions.NativeOptionsType)data.GetOptionType();
			if (nativeOptionsType == NativeOptions.NativeOptionsType.OverAll)
			{
				return (float)NativeConfig.AutoGFXQuality;
			}
			return NativeOptions.GetConfig(nativeOptionsType);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001E598 File Offset: 0x0001C798
		public void SetConfig(IOptionData data, float val)
		{
			if (!this._isInitialized)
			{
				return;
			}
			this.UpdateDependentConfigs(data, val);
			this.UpdateEnabledStates();
			NativeOptions.ConfigQuality autoGFXQuality = NativeConfig.AutoGFXQuality;
			NativeOptions.ConfigQuality configQuality = (this.IsManagedOptionsConflictWithOverallSettings((int)autoGFXQuality) ? NativeOptions.ConfigQuality.GFXCustom : autoGFXQuality);
			if (data.IsNative())
			{
				NativeOptions.NativeOptionsType nativeOptionsType = (NativeOptions.NativeOptionsType)data.GetOptionType();
				if (nativeOptionsType == NativeOptions.NativeOptionsType.OverAll)
				{
					if (MathF.Abs(val - (float)this._overallConfigCount) <= 0.01f)
					{
						goto IL_01AE;
					}
					Utilities.SetGraphicsPreset((int)val);
					foreach (GenericOptionDataVM genericOptionDataVM in this.VideoOptions.AllOptions)
					{
						if (!genericOptionDataVM.IsAction)
						{
							float num = (genericOptionDataVM.IsNative ? this.GetDefaultOptionForOverallNativeSettings((NativeOptions.NativeOptionsType)genericOptionDataVM.GetOptionType(), (int)val) : this.GetDefaultOptionForOverallManagedSettings((ManagedOptions.ManagedOptionsType)genericOptionDataVM.GetOptionType(), (int)val));
							if (num >= 0f)
							{
								genericOptionDataVM.SetValue(num);
								genericOptionDataVM.UpdateValue();
							}
						}
					}
					using (IEnumerator<GenericOptionDataVM> enumerator = this.PerformanceOptions.AllOptions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GenericOptionDataVM genericOptionDataVM2 = enumerator.Current;
							float num2 = (genericOptionDataVM2.IsNative ? this.GetDefaultOptionForOverallNativeSettings((NativeOptions.NativeOptionsType)genericOptionDataVM2.GetOptionType(), (int)val) : this.GetDefaultOptionForOverallManagedSettings((ManagedOptions.ManagedOptionsType)genericOptionDataVM2.GetOptionType(), (int)val));
							if (num2 >= 0f)
							{
								genericOptionDataVM2.SetValue(num2);
								genericOptionDataVM2.UpdateValue();
							}
						}
						goto IL_01AE;
					}
				}
				if (this._overallOption != null && (this._overallOption.Selector.SelectedIndex != this._overallConfigCount || configQuality != NativeOptions.ConfigQuality.GFXCustom) && OptionsProvider.GetDefaultNativeOptions().ContainsKey(nativeOptionsType))
				{
					this._overallOption.Selector.SelectedIndex = (int)configQuality;
				}
				IL_01AE:
				if (!this._isCancelling && (nativeOptionsType == NativeOptions.NativeOptionsType.SelectedAdapter || nativeOptionsType == NativeOptions.NativeOptionsType.SoundDevice))
				{
					InformationManager.ShowInquiry(new InquiryData(Module.CurrentModule.GlobalTextManager.FindText("str_option_restart_required", null).ToString(), Module.CurrentModule.GlobalTextManager.FindText("str_option_restart_required_desc", null).ToString(), true, false, Module.CurrentModule.GlobalTextManager.FindText("str_ok", null).ToString(), string.Empty, null, null, "", 0f, null, null, null), false, false);
				}
				this.UpdateVideoMemoryUsage();
				return;
			}
			ManagedOptions.ManagedOptionsType managedOptionsType = (ManagedOptions.ManagedOptionsType)data.GetOptionType();
			if (this._overallOption != null && (this._overallOption.Selector.SelectedIndex != this._overallConfigCount || configQuality != NativeOptions.ConfigQuality.GFXCustom) && OptionsProvider.GetDefaultManagedOptions().ContainsKey(managedOptionsType))
			{
				this._overallOption.Selector.SelectedIndex = (int)configQuality;
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001E848 File Offset: 0x0001CA48
		private void UpdateEnabledStates()
		{
			foreach (GenericOptionDataVM genericOptionDataVM in this._groupedCategories.SelectMany<GroupedOptionCategoryVM, GenericOptionDataVM>((GroupedOptionCategoryVM c) => c.AllOptions))
			{
				genericOptionDataVM.UpdateEnableState();
			}
			foreach (GenericOptionDataVM genericOptionDataVM2 in this._performanceOptionCategory.AllOptions)
			{
				genericOptionDataVM2.UpdateEnableState();
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001E8F4 File Offset: 0x0001CAF4
		private void UpdateDependentConfigs(IOptionData data, float val)
		{
			if (data.IsNative())
			{
				NativeOptions.NativeOptionsType nativeOptionsType = (NativeOptions.NativeOptionsType)data.GetOptionType();
				if (nativeOptionsType == NativeOptions.NativeOptionsType.SelectedMonitor || nativeOptionsType == NativeOptions.NativeOptionsType.DLSS)
				{
					NativeOptions.RefreshOptionsData();
					this._resolutionOption.UpdateData(false);
					this._refreshRateOption.UpdateData(false);
				}
				if (nativeOptionsType == NativeOptions.NativeOptionsType.ScreenResolution || nativeOptionsType == NativeOptions.NativeOptionsType.SelectedMonitor)
				{
					NativeOptions.RefreshOptionsData();
					this._refreshRateOption.UpdateData(false);
					if (NativeOptions.GetIsDLSSAvailable())
					{
						GenericOptionDataVM dlssOption = this._dlssOption;
						if (dlssOption == null)
						{
							return;
						}
						dlssOption.UpdateData(false);
					}
				}
			}
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001E970 File Offset: 0x0001CB70
		private bool IsManagedOptionsConflictWithOverallSettings(int overallSettingsOption)
		{
			return this._performanceManagedOptions.Any<IOptionData>((IOptionData o) => (float)((int)o.GetValue(false)) != this.GetDefaultOptionForOverallManagedSettings((ManagedOptions.ManagedOptionsType)o.GetOptionType(), overallSettingsOption));
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001E9A8 File Offset: 0x0001CBA8
		private float GetDefaultOptionForOverallNativeSettings(NativeOptions.NativeOptionsType option, int overallSettingsOption)
		{
			if (overallSettingsOption >= this._overallConfigCount || overallSettingsOption < 0)
			{
				return -1f;
			}
			float[] array;
			if (OptionsProvider.GetDefaultNativeOptions().TryGetValue(option, out array))
			{
				return array[overallSettingsOption];
			}
			return -1f;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001E9E0 File Offset: 0x0001CBE0
		private float GetDefaultOptionForOverallManagedSettings(ManagedOptions.ManagedOptionsType option, int overallSettingsOption)
		{
			if (overallSettingsOption >= this._overallConfigCount || overallSettingsOption < 0)
			{
				return -1f;
			}
			float[] array;
			if (OptionsProvider.GetDefaultManagedOptions().TryGetValue(option, out array))
			{
				return array[overallSettingsOption];
			}
			return -1f;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001EA18 File Offset: 0x0001CC18
		private bool IsCategoryAvailable(ViewModel category)
		{
			GameKeyOptionCategoryVM gameKeyOptionCategoryVM;
			if ((gameKeyOptionCategoryVM = category as GameKeyOptionCategoryVM) != null)
			{
				return gameKeyOptionCategoryVM.IsEnabled;
			}
			GamepadOptionCategoryVM gamepadOptionCategoryVM;
			if ((gamepadOptionCategoryVM = category as GamepadOptionCategoryVM) != null)
			{
				return gamepadOptionCategoryVM.IsEnabled;
			}
			GroupedOptionCategoryVM groupedOptionCategoryVM;
			return (groupedOptionCategoryVM = category as GroupedOptionCategoryVM) == null || groupedOptionCategoryVM.IsEnabled;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001EA59 File Offset: 0x0001CC59
		private int GetPreviousAvailableCategoryIndex(int currentCategoryIndex)
		{
			if (--currentCategoryIndex < 0)
			{
				currentCategoryIndex = this._categories.Count - 1;
			}
			if (!this.IsCategoryAvailable(this._categories[currentCategoryIndex]))
			{
				return this.GetPreviousAvailableCategoryIndex(currentCategoryIndex);
			}
			return currentCategoryIndex;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001EA90 File Offset: 0x0001CC90
		public void SelectPreviousCategory()
		{
			int previousAvailableCategoryIndex = this.GetPreviousAvailableCategoryIndex(this.CategoryIndex);
			this.SetSelectedCategory(previousAvailableCategoryIndex);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001EAB1 File Offset: 0x0001CCB1
		private int GetNextAvailableCategoryIndex(int currentCategoryIndex)
		{
			if (++currentCategoryIndex >= this._categories.Count)
			{
				currentCategoryIndex = 0;
			}
			if (!this.IsCategoryAvailable(this._categories[currentCategoryIndex]))
			{
				return this.GetNextAvailableCategoryIndex(currentCategoryIndex);
			}
			return currentCategoryIndex;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001EAE8 File Offset: 0x0001CCE8
		public void SelectNextCategory()
		{
			int nextAvailableCategoryIndex = this.GetNextAvailableCategoryIndex(this.CategoryIndex);
			this.SetSelectedCategory(nextAvailableCategoryIndex);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001EB09 File Offset: 0x0001CD09
		private void SetSelectedCategory(int index)
		{
			this.CategoryIndex = index;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001EB12 File Offset: 0x0001CD12
		private void OnGamepadActiveStateChanged()
		{
			if (!this.IsCategoryAvailable(this._categories[this.CategoryIndex]))
			{
				if (this.GetNextAvailableCategoryIndex(this.CategoryIndex) > this.CategoryIndex)
				{
					this.SelectNextCategory();
					return;
				}
				this.SelectPreviousCategory();
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001EB50 File Offset: 0x0001CD50
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM previousTabInputKey = this.PreviousTabInputKey;
			if (previousTabInputKey != null)
			{
				previousTabInputKey.OnFinalize();
			}
			InputKeyItemVM nextTabInputKey = this.NextTabInputKey;
			if (nextTabInputKey != null)
			{
				nextTabInputKey.OnFinalize();
			}
			InputKeyItemVM resetInputKey = this.ResetInputKey;
			if (resetInputKey != null)
			{
				resetInputKey.OnFinalize();
			}
			GamepadOptionCategoryVM gamepadOptions = this.GamepadOptions;
			if (gamepadOptions != null)
			{
				gamepadOptions.OnFinalize();
			}
			GameKeyOptionCategoryVM gameKeyOptionGroups = this.GameKeyOptionGroups;
			if (gameKeyOptionGroups != null)
			{
				gameKeyOptionGroups.OnFinalize();
			}
			ExposureOptionVM exposurePopUp = this.ExposurePopUp;
			if (exposurePopUp != null)
			{
				exposurePopUp.OnFinalize();
			}
			BrightnessOptionVM brightnessPopUp = this.BrightnessPopUp;
			if (brightnessPopUp != null)
			{
				brightnessPopUp.OnFinalize();
			}
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001EC1C File Offset: 0x0001CE1C
		protected void HandleCancel(bool autoHandleClose)
		{
			this._isCancelling = true;
			this._groupedCategories.ForEach(delegate(GroupedOptionCategoryVM c)
			{
				c.Cancel();
			});
			this._gameKeyCategory.Cancel();
			this._performanceOptionCategory.Cancel();
			this.CloseScreen(autoHandleClose);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001EC77 File Offset: 0x0001CE77
		private void CloseScreen(bool autoHandleClose)
		{
			this.ExecuteCloseOptions();
			if (autoHandleClose)
			{
				if (Game.Current != null)
				{
					Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
				}
				ScreenManager.PopScreen();
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001ECA0 File Offset: 0x0001CEA0
		public void ExecuteCancel()
		{
			if (this.IsOptionsChanged())
			{
				string text = new TextObject("{=peUP9ZZj}Are you sure? You made some changes and they will be lost.", null).ToString();
				InformationManager.ShowInquiry(new InquiryData("", text, true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), delegate
				{
					this.HandleCancel(this._autoHandleClose);
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			this.HandleCancel(this._autoHandleClose);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001ED24 File Offset: 0x0001CF24
		protected void OnDone()
		{
			this.ApplyChangedOptions();
			GenericOptionDataVM monitorOption = this._monitorOption;
			if (monitorOption == null || !monitorOption.IsChanged())
			{
				GenericOptionDataVM resolutionOption = this._resolutionOption;
				if (resolutionOption == null || !resolutionOption.IsChanged())
				{
					GenericOptionDataVM refreshRateOption = this._refreshRateOption;
					if (refreshRateOption == null || !refreshRateOption.IsChanged())
					{
						GenericOptionDataVM displayModeOption = this._displayModeOption;
						if (displayModeOption == null || !displayModeOption.IsChanged())
						{
							this.CloseScreen(this._autoHandleClose);
							return;
						}
					}
				}
			}
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=lCZMJt2k}Video Options Have Been Changed", null).ToString(), new TextObject("{=pK4EyTZC}Do you want to keep these settings?", null).ToString(), true, true, Module.CurrentModule.GlobalTextManager.FindText("str_ok", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), delegate
			{
				GenericOptionDataVM monitorOption2 = this._monitorOption;
				if (monitorOption2 != null)
				{
					monitorOption2.ApplyValue();
				}
				GenericOptionDataVM resolutionOption2 = this._resolutionOption;
				if (resolutionOption2 != null)
				{
					resolutionOption2.ApplyValue();
				}
				GenericOptionDataVM refreshRateOption2 = this._refreshRateOption;
				if (refreshRateOption2 != null)
				{
					refreshRateOption2.ApplyValue();
				}
				GenericOptionDataVM displayModeOption2 = this._displayModeOption;
				if (displayModeOption2 != null)
				{
					displayModeOption2.ApplyValue();
				}
				this.CloseScreen(this._autoHandleClose);
			}, delegate
			{
				GenericOptionDataVM monitorOption3 = this._monitorOption;
				if (monitorOption3 != null)
				{
					monitorOption3.Cancel();
				}
				GenericOptionDataVM resolutionOption3 = this._resolutionOption;
				if (resolutionOption3 != null)
				{
					resolutionOption3.Cancel();
				}
				GenericOptionDataVM refreshRateOption3 = this._refreshRateOption;
				if (refreshRateOption3 != null)
				{
					refreshRateOption3.Cancel();
				}
				GenericOptionDataVM displayModeOption3 = this._displayModeOption;
				if (displayModeOption3 != null)
				{
					displayModeOption3.Cancel();
				}
				NativeOptions.ApplyConfigChanges(true);
			}, "", 10f, delegate
			{
				GenericOptionDataVM monitorOption4 = this._monitorOption;
				if (monitorOption4 != null)
				{
					monitorOption4.Cancel();
				}
				GenericOptionDataVM resolutionOption4 = this._resolutionOption;
				if (resolutionOption4 != null)
				{
					resolutionOption4.Cancel();
				}
				GenericOptionDataVM refreshRateOption4 = this._refreshRateOption;
				if (refreshRateOption4 != null)
				{
					refreshRateOption4.Cancel();
				}
				GenericOptionDataVM displayModeOption4 = this._displayModeOption;
				if (displayModeOption4 != null)
				{
					displayModeOption4.Cancel();
				}
				NativeOptions.ApplyConfigChanges(true);
			}, null, null), false, false);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001EE20 File Offset: 0x0001D020
		private void ApplyChangedOptions()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			IEnumerable<GenericOptionDataVM> enumerable = this._groupedCategories.SelectMany<GroupedOptionCategoryVM, GenericOptionDataVM>((GroupedOptionCategoryVM c) => c.AllOptions);
			foreach (GenericOptionDataVM genericOptionDataVM in enumerable)
			{
				genericOptionDataVM.UpdateValue();
				if (genericOptionDataVM.IsNative && !genericOptionDataVM.GetOptionData().IsAction())
				{
					NativeOptions.NativeOptionsType nativeOptionsType = (NativeOptions.NativeOptionsType)genericOptionDataVM.GetOptionType();
					if (nativeOptionsType <= NativeOptions.NativeOptionsType.TextureQuality)
					{
						if (nativeOptionsType <= NativeOptions.NativeOptionsType.SelectedMonitor)
						{
							if (nativeOptionsType == NativeOptions.NativeOptionsType.TrailAmount)
							{
								num = (genericOptionDataVM.IsChanged() ? 1 : 0);
								continue;
							}
							if (nativeOptionsType - NativeOptions.NativeOptionsType.DisplayMode > 1)
							{
								continue;
							}
						}
						else if (nativeOptionsType - NativeOptions.NativeOptionsType.ScreenResolution > 1)
						{
							if (nativeOptionsType == NativeOptions.NativeOptionsType.TextureBudget)
							{
								num2 = (genericOptionDataVM.IsChanged() ? 1 : 0);
								continue;
							}
							if (nativeOptionsType != NativeOptions.NativeOptionsType.TextureQuality)
							{
								continue;
							}
							num2 = (genericOptionDataVM.IsChanged() ? 1 : 0);
							continue;
						}
					}
					else if (nativeOptionsType <= NativeOptions.NativeOptionsType.SSR)
					{
						if (nativeOptionsType == NativeOptions.NativeOptionsType.TextureFiltering)
						{
							num8 = (genericOptionDataVM.IsChanged() ? 1 : 0);
							continue;
						}
						if (nativeOptionsType == NativeOptions.NativeOptionsType.DepthOfField)
						{
							num4 = (genericOptionDataVM.IsChanged() ? 1 : 0);
							continue;
						}
						if (nativeOptionsType != NativeOptions.NativeOptionsType.SSR)
						{
							continue;
						}
						num6 = (genericOptionDataVM.IsChanged() ? 1 : 0);
						continue;
					}
					else
					{
						switch (nativeOptionsType)
						{
						case NativeOptions.NativeOptionsType.Bloom:
							num3 = (genericOptionDataVM.IsChanged() ? 1 : 0);
							continue;
						case NativeOptions.NativeOptionsType.FilmGrain:
							continue;
						case NativeOptions.NativeOptionsType.MotionBlur:
							num5 = (genericOptionDataVM.IsChanged() ? 1 : 0);
							continue;
						case NativeOptions.NativeOptionsType.SharpenAmount:
							num9 = (genericOptionDataVM.IsChanged() ? 1 : 0);
							continue;
						default:
							if (nativeOptionsType != NativeOptions.NativeOptionsType.DynamicResolution)
							{
								if (nativeOptionsType != NativeOptions.NativeOptionsType.DynamicResolutionTarget)
								{
									continue;
								}
								num10 = (genericOptionDataVM.IsChanged() ? 1 : 0);
								continue;
							}
							break;
						}
					}
					num7 = ((num7 == 1 || genericOptionDataVM.IsChanged()) ? 1 : 0);
				}
			}
			NativeOptions.Apply(num2, num9, num3, num4, num5, num6, num7, num8, num, num10);
			SaveResult saveResult = NativeOptions.SaveConfig();
			SaveResult saveResult2 = ManagedOptions.SaveConfig();
			if (saveResult != SaveResult.Success || saveResult2 != SaveResult.Success)
			{
				SaveResult saveResult3 = ((saveResult != SaveResult.Success) ? saveResult : saveResult2);
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=oZrVNUOk}Error", null).ToString(), Module.CurrentModule.GlobalTextManager.FindText("str_config_save_result", saveResult3.ToString()).ToString(), true, false, Module.CurrentModule.GlobalTextManager.FindText("str_ok", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
			}
			this.GameKeyOptionGroups.ApplyValues();
			HotKeyManager.MarkForSave();
			enumerable = enumerable.Concat<GenericOptionDataVM>(this._performanceOptionCategory.AllOptions);
			enumerable = enumerable.Where<GenericOptionDataVM>((GenericOptionDataVM x) => x != this._monitorOption && x != this._resolutionOption && x != this._refreshRateOption && x != this._displayModeOption);
			foreach (GenericOptionDataVM genericOptionDataVM2 in enumerable)
			{
				genericOptionDataVM2.ApplyValue();
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001F170 File Offset: 0x0001D370
		protected void ExecuteBenchmark()
		{
			GameStateManager.StateActivateCommand = "state_string.benchmark_start";
			bool flag;
			CommandLineFunctionality.CallFunction("benchmark.cpu_benchmark", "", out flag);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001F19C File Offset: 0x0001D39C
		public void ExecuteDone()
		{
			bool flag = false;
			int currentEstimatedGPUMemoryCostMB = Utilities.GetCurrentEstimatedGPUMemoryCostMB();
			int gpumemoryMB = Utilities.GetGPUMemoryMB();
			if (!flag && gpumemoryMB <= currentEstimatedGPUMemoryCostMB)
			{
				InformationManager.ShowInquiry(new InquiryData(Module.CurrentModule.GlobalTextManager.FindText("str_gpu_memory_caution_title", null).ToString(), Module.CurrentModule.GlobalTextManager.FindText("str_gpu_memory_caution_text", null).ToString(), true, false, Module.CurrentModule.GlobalTextManager.FindText("str_ok", null).ToString(), null, delegate
				{
					this.OnDone();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			this.OnDone();
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001F23C File Offset: 0x0001D43C
		protected void ExecuteReset()
		{
			InformationManager.ShowInquiry(new InquiryData("", new TextObject("{=cDzWYQrz}Reset to default settings?", null).ToString(), true, true, new TextObject("{=oHaWR73d}Ok", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), new Action(this.OnResetToDefaults), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001F2A8 File Offset: 0x0001D4A8
		public bool IsOptionsChanged()
		{
			return (this._groupedCategories.Any<GroupedOptionCategoryVM>((GroupedOptionCategoryVM c) => c.IsChanged()) || this.GameKeyOptionGroups.IsChanged()) | this._performanceOptionCategory.IsChanged();
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001F2FB File Offset: 0x0001D4FB
		private void OnResetToDefaults()
		{
			this._groupedCategories.ForEach(delegate(GroupedOptionCategoryVM g)
			{
				g.ResetData();
			});
			this._performanceOptionCategory.ResetData();
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001F334 File Offset: 0x0001D534
		private void UpdateVideoMemoryUsage()
		{
			int currentEstimatedGPUMemoryCostMB = Utilities.GetCurrentEstimatedGPUMemoryCostMB();
			int gpumemoryMB = Utilities.GetGPUMemoryMB();
			this.VideoMemoryUsageNormalized = (float)currentEstimatedGPUMemoryCostMB / (float)gpumemoryMB;
			TextObject textObject = Module.CurrentModule.GlobalTextManager.FindText("str_gpu_memory_usage_value_text", null);
			textObject.SetTextVariable("ESTIMATED", currentEstimatedGPUMemoryCostMB);
			textObject.SetTextVariable("TOTAL", gpumemoryMB);
			this.VideoMemoryUsageText = textObject.ToString();
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001F394 File Offset: 0x0001D594
		internal GenericOptionDataVM GetOptionItem(IOptionData option)
		{
			bool flag = Input.ControllerType.IsPlaystation();
			MBTextManager.SetTextVariable("IS_PLAYSTATION", flag ? 1 : 0);
			if (!option.IsAction())
			{
				string text = (option.IsNative() ? ((NativeOptions.NativeOptionsType)option.GetOptionType()).ToString() : ((ManagedOptions.ManagedOptionsType)option.GetOptionType()).ToString());
				TextObject textObject = Module.CurrentModule.GlobalTextManager.FindText("str_options_type", text);
				TextObject textObject2 = Module.CurrentModule.GlobalTextManager.FindText("str_options_description", text);
				textObject2.SetTextVariable("newline", "\n");
				if (option is IBooleanOptionData)
				{
					return new BooleanOptionDataVM(this, option as IBooleanOptionData, textObject, textObject2)
					{
						ImageIDs = new string[]
						{
							text + "_0",
							text + "_1"
						}
					};
				}
				if (option is INumericOptionData)
				{
					return new NumericOptionDataVM(this, option as INumericOptionData, textObject, textObject2);
				}
				if (option is ISelectionOptionData)
				{
					ISelectionOptionData selectionOptionData = option as ISelectionOptionData;
					StringOptionDataVM stringOptionDataVM = new StringOptionDataVM(this, selectionOptionData, textObject, textObject2);
					string[] array = new string[selectionOptionData.GetSelectableOptionsLimit()];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = text + "_" + i;
					}
					stringOptionDataVM.ImageIDs = array;
					return stringOptionDataVM;
				}
				ActionOptionData actionOptionData;
				if ((actionOptionData = option as ActionOptionData) != null)
				{
					TextObject textObject3 = Module.CurrentModule.GlobalTextManager.FindText("str_options_type_action", text);
					return new ActionOptionDataVM(actionOptionData.OnAction, this, actionOptionData, textObject, textObject3, textObject2);
				}
				Debug.FailedAssert("Given option data does not match with any option type!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\GameOptions\\OptionsVM.cs", "GetOptionItem", 897);
				return null;
			}
			else
			{
				ActionOptionData actionOptionData2;
				if ((actionOptionData2 = option as ActionOptionData) != null)
				{
					string text2 = option.GetOptionType() as string;
					TextObject textObject4 = Module.CurrentModule.GlobalTextManager.FindText("str_options_type_action", text2);
					TextObject textObject5 = Module.CurrentModule.GlobalTextManager.FindText("str_options_type", text2);
					TextObject textObject6 = Module.CurrentModule.GlobalTextManager.FindText("str_options_description", text2);
					textObject6.SetTextVariable("newline", "\n");
					return new ActionOptionDataVM(actionOptionData2.OnAction, this, actionOptionData2, textObject5, textObject4, textObject6);
				}
				Debug.FailedAssert("Given option data does not match with any option type!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\GameOptions\\OptionsVM.cs", "GetOptionItem", 920);
				return null;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0001F5F3 File Offset: 0x0001D7F3
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x0001F5FB File Offset: 0x0001D7FB
		[DataSourceProperty]
		public int CategoryIndex
		{
			get
			{
				return this._categoryIndex;
			}
			set
			{
				if (value != this._categoryIndex)
				{
					this._categoryIndex = value;
					base.OnPropertyChangedWithValue(value, "CategoryIndex");
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0001F619 File Offset: 0x0001D819
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0001F621 File Offset: 0x0001D821
		[DataSourceProperty]
		public string OptionsLbl
		{
			get
			{
				return this._optionsLbl;
			}
			set
			{
				if (value != this._optionsLbl)
				{
					this._optionsLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "OptionsLbl");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0001F644 File Offset: 0x0001D844
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0001F64C File Offset: 0x0001D84C
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0001F66F File Offset: 0x0001D86F
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x0001F677 File Offset: 0x0001D877
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0001F69A File Offset: 0x0001D89A
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0001F6A2 File Offset: 0x0001D8A2
		[DataSourceProperty]
		public string ResetLbl
		{
			get
			{
				return this._resetLbl;
			}
			set
			{
				if (value != this._resetLbl)
				{
					this._resetLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetLbl");
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0001F6C5 File Offset: 0x0001D8C5
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x0001F6CD File Offset: 0x0001D8CD
		[DataSourceProperty]
		public bool IsConsole
		{
			get
			{
				return this._isConsole;
			}
			set
			{
				if (value != this._isConsole)
				{
					this._isConsole = value;
					base.OnPropertyChangedWithValue(value, "IsConsole");
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0001F6EB File Offset: 0x0001D8EB
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0001F6F3 File Offset: 0x0001D8F3
		public bool IsDevelopmentMode
		{
			get
			{
				return this._isDevelopmentMode;
			}
			set
			{
				if (value != this._isDevelopmentMode)
				{
					this._isDevelopmentMode = value;
					base.OnPropertyChangedWithValue(value, "IsDevelopmentMode");
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0001F711 File Offset: 0x0001D911
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0001F719 File Offset: 0x0001D919
		[DataSourceProperty]
		public string VideoMemoryUsageName
		{
			get
			{
				return this._videoMemoryUsageName;
			}
			set
			{
				if (this._videoMemoryUsageName != value)
				{
					this._videoMemoryUsageName = value;
					base.OnPropertyChangedWithValue<string>(value, "VideoMemoryUsageName");
				}
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0001F73C File Offset: 0x0001D93C
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x0001F744 File Offset: 0x0001D944
		[DataSourceProperty]
		public string VideoMemoryUsageText
		{
			get
			{
				return this._videoMemoryUsageText;
			}
			set
			{
				if (this._videoMemoryUsageText != value)
				{
					this._videoMemoryUsageText = value;
					base.OnPropertyChangedWithValue<string>(value, "VideoMemoryUsageText");
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0001F767 File Offset: 0x0001D967
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x0001F76F File Offset: 0x0001D96F
		[DataSourceProperty]
		public float VideoMemoryUsageNormalized
		{
			get
			{
				return this._videoMemoryUsageNormalized;
			}
			set
			{
				if (this._videoMemoryUsageNormalized != value)
				{
					this._videoMemoryUsageNormalized = value;
					base.OnPropertyChangedWithValue(value, "VideoMemoryUsageNormalized");
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x0001F78D File Offset: 0x0001D98D
		[DataSourceProperty]
		public GameKeyOptionCategoryVM GameKeyOptionGroups
		{
			get
			{
				return this._gameKeyCategory;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0001F795 File Offset: 0x0001D995
		[DataSourceProperty]
		public GamepadOptionCategoryVM GamepadOptions
		{
			get
			{
				return this._gamepadCategory;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0001F79D File Offset: 0x0001D99D
		[DataSourceProperty]
		public GroupedOptionCategoryVM PerformanceOptions
		{
			get
			{
				return this._performanceOptionCategory;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0001F7A5 File Offset: 0x0001D9A5
		[DataSourceProperty]
		public GroupedOptionCategoryVM AudioOptions
		{
			get
			{
				return this._audioOptionCategory;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x0001F7AD File Offset: 0x0001D9AD
		[DataSourceProperty]
		public GroupedOptionCategoryVM GameplayOptions
		{
			get
			{
				return this._gameplayOptionCategory;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0001F7B5 File Offset: 0x0001D9B5
		[DataSourceProperty]
		public GroupedOptionCategoryVM VideoOptions
		{
			get
			{
				return this._videoOptionCategory;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0001F7BD File Offset: 0x0001D9BD
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x0001F7C5 File Offset: 0x0001D9C5
		[DataSourceProperty]
		public BrightnessOptionVM BrightnessPopUp
		{
			get
			{
				return this._brightnessPopUp;
			}
			set
			{
				if (value != this._brightnessPopUp)
				{
					this._brightnessPopUp = value;
					base.OnPropertyChangedWithValue<BrightnessOptionVM>(value, "BrightnessPopUp");
				}
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0001F7E3 File Offset: 0x0001D9E3
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x0001F7EB File Offset: 0x0001D9EB
		[DataSourceProperty]
		public ExposureOptionVM ExposurePopUp
		{
			get
			{
				return this._exposurePopUp;
			}
			set
			{
				if (value != this._exposurePopUp)
				{
					this._exposurePopUp = value;
					base.OnPropertyChangedWithValue<ExposureOptionVM>(value, "ExposurePopUp");
				}
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001F809 File Offset: 0x0001DA09
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001F818 File Offset: 0x0001DA18
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001F827 File Offset: 0x0001DA27
		public void SetPreviousTabInputKey(HotKey hotkey)
		{
			this.PreviousTabInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001F836 File Offset: 0x0001DA36
		public void SetNextTabInputKey(HotKey hotkey)
		{
			this.NextTabInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001F845 File Offset: 0x0001DA45
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0001F854 File Offset: 0x0001DA54
		// (set) Token: 0x0600093D RID: 2365 RVA: 0x0001F85C File Offset: 0x0001DA5C
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0001F87A File Offset: 0x0001DA7A
		// (set) Token: 0x0600093F RID: 2367 RVA: 0x0001F882 File Offset: 0x0001DA82
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0001F8A0 File Offset: 0x0001DAA0
		// (set) Token: 0x06000941 RID: 2369 RVA: 0x0001F8A8 File Offset: 0x0001DAA8
		[DataSourceProperty]
		public InputKeyItemVM PreviousTabInputKey
		{
			get
			{
				return this._previousTabInputKey;
			}
			set
			{
				if (value != this._previousTabInputKey)
				{
					this._previousTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousTabInputKey");
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0001F8C6 File Offset: 0x0001DAC6
		// (set) Token: 0x06000943 RID: 2371 RVA: 0x0001F8CE File Offset: 0x0001DACE
		[DataSourceProperty]
		public InputKeyItemVM NextTabInputKey
		{
			get
			{
				return this._nextTabInputKey;
			}
			set
			{
				if (value != this._nextTabInputKey)
				{
					this._nextTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextTabInputKey");
				}
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0001F8EC File Offset: 0x0001DAEC
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x0001F8F4 File Offset: 0x0001DAF4
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x040003F7 RID: 1015
		private readonly Action _onClose;

		// Token: 0x040003F8 RID: 1016
		private readonly Action _onBrightnessExecute;

		// Token: 0x040003F9 RID: 1017
		private readonly Action _onExposureExecute;

		// Token: 0x040003FA RID: 1018
		private readonly StringOptionDataVM _overallOption;

		// Token: 0x040003FB RID: 1019
		private readonly GenericOptionDataVM _dlssOption;

		// Token: 0x040003FC RID: 1020
		private readonly List<GenericOptionDataVM> _dynamicResolutionOptions = new List<GenericOptionDataVM>();

		// Token: 0x040003FD RID: 1021
		private readonly GenericOptionDataVM _refreshRateOption;

		// Token: 0x040003FE RID: 1022
		private readonly GenericOptionDataVM _resolutionOption;

		// Token: 0x040003FF RID: 1023
		private readonly GenericOptionDataVM _monitorOption;

		// Token: 0x04000400 RID: 1024
		private readonly GenericOptionDataVM _displayModeOption;

		// Token: 0x04000401 RID: 1025
		private readonly bool _autoHandleClose;

		// Token: 0x04000402 RID: 1026
		protected readonly GroupedOptionCategoryVM _gameplayOptionCategory;

		// Token: 0x04000403 RID: 1027
		private readonly GroupedOptionCategoryVM _audioOptionCategory;

		// Token: 0x04000404 RID: 1028
		private readonly GroupedOptionCategoryVM _videoOptionCategory;

		// Token: 0x04000405 RID: 1029
		protected readonly GameKeyOptionCategoryVM _gameKeyCategory;

		// Token: 0x04000406 RID: 1030
		private readonly GamepadOptionCategoryVM _gamepadCategory;

		// Token: 0x04000407 RID: 1031
		private bool _isInitialized;

		// Token: 0x04000408 RID: 1032
		private Action<KeyOptionVM> _onKeybindRequest;

		// Token: 0x04000409 RID: 1033
		protected readonly GroupedOptionCategoryVM _performanceOptionCategory;

		// Token: 0x0400040A RID: 1034
		private readonly int _overallConfigCount = NativeSelectionOptionData.GetOptionsLimit(NativeOptions.NativeOptionsType.OverAll) - 1;

		// Token: 0x0400040B RID: 1035
		private bool _isCancelling;

		// Token: 0x0400040C RID: 1036
		private readonly IEnumerable<IOptionData> _performanceManagedOptions;

		// Token: 0x0400040D RID: 1037
		protected readonly List<GroupedOptionCategoryVM> _groupedCategories;

		// Token: 0x0400040E RID: 1038
		private readonly List<ViewModel> _categories;

		// Token: 0x0400040F RID: 1039
		private int _categoryIndex;

		// Token: 0x04000410 RID: 1040
		private string _optionsLbl;

		// Token: 0x04000411 RID: 1041
		private string _cancelLbl;

		// Token: 0x04000412 RID: 1042
		private string _doneLbl;

		// Token: 0x04000413 RID: 1043
		private string _resetLbl;

		// Token: 0x04000414 RID: 1044
		private bool _isDevelopmentMode;

		// Token: 0x04000415 RID: 1045
		private bool _isConsole;

		// Token: 0x04000416 RID: 1046
		private float _videoMemoryUsageNormalized;

		// Token: 0x04000417 RID: 1047
		private string _videoMemoryUsageName;

		// Token: 0x04000418 RID: 1048
		private string _videoMemoryUsageText;

		// Token: 0x04000419 RID: 1049
		private BrightnessOptionVM _brightnessPopUp;

		// Token: 0x0400041A RID: 1050
		private ExposureOptionVM _exposurePopUp;

		// Token: 0x0400041B RID: 1051
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400041C RID: 1052
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400041D RID: 1053
		private InputKeyItemVM _previousTabInputKey;

		// Token: 0x0400041E RID: 1054
		private InputKeyItemVM _nextTabInputKey;

		// Token: 0x0400041F RID: 1055
		private InputKeyItemVM _resetInputKey;

		// Token: 0x02000106 RID: 262
		public enum OptionsDataType
		{
			// Token: 0x040006A6 RID: 1702
			None = -1,
			// Token: 0x040006A7 RID: 1703
			BooleanOption,
			// Token: 0x040006A8 RID: 1704
			NumericOption,
			// Token: 0x040006A9 RID: 1705
			MultipleSelectionOption = 3,
			// Token: 0x040006AA RID: 1706
			InputOption,
			// Token: 0x040006AB RID: 1707
			ActionOption
		}

		// Token: 0x02000107 RID: 263
		public enum OptionsMode
		{
			// Token: 0x040006AD RID: 1709
			MainMenu,
			// Token: 0x040006AE RID: 1710
			Singleplayer,
			// Token: 0x040006AF RID: 1711
			Multiplayer
		}
	}
}
