using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Options;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GamepadOptions
{
	// Token: 0x02000075 RID: 117
	public class GamepadOptionCategoryVM : GroupedOptionCategoryVM
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x0001FD44 File Offset: 0x0001DF44
		public GamepadOptionCategoryVM(OptionsVM options, TextObject name, OptionCategory category, bool isEnabled, bool isResetSupported = false)
			: base(options, name, category, isEnabled, isResetSupported)
		{
			this.OtherKeys = new MBBindingList<GamepadOptionKeyItemVM>();
			this.DpadKeys = new MBBindingList<GamepadOptionKeyItemVM>();
			this.LeftAnalogKeys = new MBBindingList<GamepadOptionKeyItemVM>();
			this.RightAnalogKeys = new MBBindingList<GamepadOptionKeyItemVM>();
			this.FaceKeys = new MBBindingList<GamepadOptionKeyItemVM>();
			this.LeftTriggerAndBumperKeys = new MBBindingList<GamepadOptionKeyItemVM>();
			this.RightTriggerAndBumperKeys = new MBBindingList<GamepadOptionKeyItemVM>();
			if (Input.ControllerType == Input.ControllerTypes.PlayStationDualSense)
			{
				this.SetCurrentGamepadType(GamepadOptionCategoryVM.GamepadType.Playstation5);
			}
			else if (Input.ControllerType == Input.ControllerTypes.PlayStationDualShock)
			{
				this.SetCurrentGamepadType(GamepadOptionCategoryVM.GamepadType.Playstation4);
			}
			else
			{
				this.SetCurrentGamepadType(GamepadOptionCategoryVM.GamepadType.Xbox);
			}
			this.Actions = new MBBindingList<SelectorVM<SelectorItemVM>>();
			this._categories = new SelectorVM<SelectorItemVM>(0, null);
			this._categories.AddItem(new SelectorItemVM(new TextObject("{=gamepadActionKeybind}Action", null)));
			this._categories.AddItem(new SelectorItemVM(new TextObject("{=gamepadMapKeybind}Map", null)));
			this._categories.SetOnChangeAction(new Action<SelectorVM<SelectorItemVM>>(this.OnCategoryChange));
			this._categories.SelectedIndex = 0;
			this.Actions.Add(this._categories);
			this.RefreshValues();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			base.IsEnabled = Input.IsGamepadActive;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0001FE90 File Offset: 0x0001E090
		private void OnCategoryChange(SelectorVM<SelectorItemVM> obj)
		{
			if (obj.SelectedIndex >= 0)
			{
				this.OtherKeys.Clear();
				this.DpadKeys.Clear();
				this.LeftAnalogKeys.Clear();
				this.RightAnalogKeys.Clear();
				this.FaceKeys.Clear();
				this.LeftTriggerAndBumperKeys.Clear();
				this.RightTriggerAndBumperKeys.Clear();
				IEnumerable<GamepadOptionKeyItemVM> enumerable = null;
				if (obj.SelectedIndex == 0)
				{
					enumerable = GamepadOptionCategoryVM.GetActionKeys();
				}
				else if (obj.SelectedIndex == 1)
				{
					enumerable = GamepadOptionCategoryVM.GetMapKeys();
				}
				foreach (GamepadOptionKeyItemVM gamepadOptionKeyItemVM in enumerable)
				{
					InputKey inputKey = gamepadOptionKeyItemVM.Key ?? InputKey.Invalid;
					if (Key.IsLeftAnalogInput(inputKey))
					{
						this.LeftAnalogKeys.Add(gamepadOptionKeyItemVM);
					}
					else if (Key.IsRightAnalogInput(inputKey))
					{
						this.RightAnalogKeys.Add(gamepadOptionKeyItemVM);
					}
					else if (Key.IsDpadInput(inputKey))
					{
						this.DpadKeys.Add(gamepadOptionKeyItemVM);
					}
					else if (Key.IsFaceKeyInput(inputKey))
					{
						this.FaceKeys.Add(gamepadOptionKeyItemVM);
					}
					else
					{
						this.OtherKeys.Add(gamepadOptionKeyItemVM);
					}
					if (Key.IsLeftBumperOrTriggerInput(inputKey))
					{
						this.LeftTriggerAndBumperKeys.Add(gamepadOptionKeyItemVM);
					}
					else if (Key.IsRightBumperOrTriggerInput(inputKey))
					{
						this.RightTriggerAndBumperKeys.Add(gamepadOptionKeyItemVM);
					}
				}
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		public override void RefreshValues()
		{
			base.RefreshValues();
			MBBindingList<GamepadOptionKeyItemVM> otherKeys = this.OtherKeys;
			if (otherKeys != null)
			{
				otherKeys.ApplyActionOnAllItems(delegate(GamepadOptionKeyItemVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<GamepadOptionKeyItemVM> leftAnalogKeys = this.LeftAnalogKeys;
			if (leftAnalogKeys != null)
			{
				leftAnalogKeys.ApplyActionOnAllItems(delegate(GamepadOptionKeyItemVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<GamepadOptionKeyItemVM> rightAnalogKeys = this.RightAnalogKeys;
			if (rightAnalogKeys != null)
			{
				rightAnalogKeys.ApplyActionOnAllItems(delegate(GamepadOptionKeyItemVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<GamepadOptionKeyItemVM> faceKeys = this.FaceKeys;
			if (faceKeys != null)
			{
				faceKeys.ApplyActionOnAllItems(delegate(GamepadOptionKeyItemVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<GamepadOptionKeyItemVM> dpadKeys = this.DpadKeys;
			if (dpadKeys != null)
			{
				dpadKeys.ApplyActionOnAllItems(delegate(GamepadOptionKeyItemVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<GamepadOptionKeyItemVM> leftTriggerAndBumperKeys = this.LeftTriggerAndBumperKeys;
			if (leftTriggerAndBumperKeys != null)
			{
				leftTriggerAndBumperKeys.ApplyActionOnAllItems(delegate(GamepadOptionKeyItemVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<GamepadOptionKeyItemVM> rightTriggerAndBumperKeys = this.RightTriggerAndBumperKeys;
			if (rightTriggerAndBumperKeys != null)
			{
				rightTriggerAndBumperKeys.ApplyActionOnAllItems(delegate(GamepadOptionKeyItemVM x)
				{
					x.RefreshValues();
				});
			}
			MBBindingList<SelectorVM<SelectorItemVM>> actions = this.Actions;
			if (actions == null)
			{
				return;
			}
			actions.ApplyActionOnAllItems(delegate(SelectorVM<SelectorItemVM> x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002018E File Offset: 0x0001E38E
		private void SetCurrentGamepadType(GamepadOptionCategoryVM.GamepadType type)
		{
			this.CurrentGamepadType = (int)type;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00020198 File Offset: 0x0001E398
		private void OnGamepadActiveStateChanged()
		{
			base.IsEnabled = Input.IsGamepadActive;
			Debug.Print("GAMEPAD TAB ENABLED: " + base.IsEnabled.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x000201D9 File Offset: 0x0001E3D9
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00020201 File Offset: 0x0001E401
		private static IEnumerable<GamepadOptionKeyItemVM> GetActionKeys()
		{
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLStick, new TextObject("{=i28Kjuay}Move Character", null));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerRStick, new TextObject("{=1hlaGzGI}Look", null));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLOption, new TextObject("{=9pgOGq7X}Log", null));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(31));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(33));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(25));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(15));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(13));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("ScoreboardHotKeyCategory").GetHotKey("HoldShow"));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(16));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(14));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(9));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(10));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(26));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(27));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("Generic").GetGameKey(5));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(34));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("ToggleEscapeMenu"));
			yield break;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002020A File Offset: 0x0001E40A
		private static IEnumerable<GamepadOptionKeyItemVM> GetMapKeys()
		{
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLStick, new TextObject("{=hdGay8xc}Map Cursor Move", null));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerRStick, new TextObject("{=atUHbDeM}Map Camera Rotate", null));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLUp, new TextObject("{=u78WUP9W}Fast Cursor Move Up", null));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLRight, new TextObject("{=bLPSaLNv}Fast Cursor Move Right", null));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLLeft, new TextObject("{=82LuSDnd}Fast Cursor Move Left", null));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLDown, new TextObject("{=nEpZvaEl}Fast Cursor Move Down", null));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetHotKey("MapChangeCursorMode"));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory").GetGameKey(39));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetGameKey(63));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetGameKey(56));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetGameKey(65));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetGameKey(57));
			yield return new GamepadOptionKeyItemVM(InputKey.ControllerLBumper, new TextObject("{=mueocuFG}Show Indicators", null));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetGameKey(64));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetGameKey(66));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("ToggleEscapeMenu"));
			yield return new GamepadOptionKeyItemVM(HotKeyManager.GetCategory("MapHotKeyCategory").GetHotKey("MapClick"));
			yield break;
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00020213 File Offset: 0x0001E413
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0002021B File Offset: 0x0001E41B
		[DataSourceProperty]
		public int CurrentGamepadType
		{
			get
			{
				return this._currentGamepadType;
			}
			set
			{
				if (value != this._currentGamepadType)
				{
					this._currentGamepadType = value;
					base.OnPropertyChangedWithValue(value, "CurrentGamepadType");
				}
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00020239 File Offset: 0x0001E439
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x00020241 File Offset: 0x0001E441
		[DataSourceProperty]
		public MBBindingList<GamepadOptionKeyItemVM> OtherKeys
		{
			get
			{
				return this._otherKeys;
			}
			set
			{
				if (value != this._otherKeys)
				{
					this._otherKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GamepadOptionKeyItemVM>>(value, "OtherKeys");
				}
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0002025F File Offset: 0x0001E45F
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x00020267 File Offset: 0x0001E467
		[DataSourceProperty]
		public MBBindingList<GamepadOptionKeyItemVM> DpadKeys
		{
			get
			{
				return this._dpadKeys;
			}
			set
			{
				if (value != this._dpadKeys)
				{
					this._dpadKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GamepadOptionKeyItemVM>>(value, "DpadKeys");
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x00020285 File Offset: 0x0001E485
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x0002028D File Offset: 0x0001E48D
		[DataSourceProperty]
		public MBBindingList<GamepadOptionKeyItemVM> LeftTriggerAndBumperKeys
		{
			get
			{
				return this._leftTriggerAndBumperKeys;
			}
			set
			{
				if (value != this._leftTriggerAndBumperKeys)
				{
					this._leftTriggerAndBumperKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GamepadOptionKeyItemVM>>(value, "LeftTriggerAndBumperKeys");
				}
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x000202AB File Offset: 0x0001E4AB
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x000202B3 File Offset: 0x0001E4B3
		[DataSourceProperty]
		public MBBindingList<GamepadOptionKeyItemVM> RightTriggerAndBumperKeys
		{
			get
			{
				return this._rightTriggerAndBumperKeys;
			}
			set
			{
				if (value != this._rightTriggerAndBumperKeys)
				{
					this._rightTriggerAndBumperKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GamepadOptionKeyItemVM>>(value, "RightTriggerAndBumperKeys");
				}
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x000202D1 File Offset: 0x0001E4D1
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x000202D9 File Offset: 0x0001E4D9
		[DataSourceProperty]
		public MBBindingList<GamepadOptionKeyItemVM> RightAnalogKeys
		{
			get
			{
				return this._rightAnalogKeys;
			}
			set
			{
				if (value != this._rightAnalogKeys)
				{
					this._rightAnalogKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GamepadOptionKeyItemVM>>(value, "RightAnalogKeys");
				}
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x000202F7 File Offset: 0x0001E4F7
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x000202FF File Offset: 0x0001E4FF
		[DataSourceProperty]
		public MBBindingList<GamepadOptionKeyItemVM> LeftAnalogKeys
		{
			get
			{
				return this._leftAnalogKeys;
			}
			set
			{
				if (value != this._leftAnalogKeys)
				{
					this._leftAnalogKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GamepadOptionKeyItemVM>>(value, "LeftAnalogKeys");
				}
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0002031D File Offset: 0x0001E51D
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x00020325 File Offset: 0x0001E525
		[DataSourceProperty]
		public MBBindingList<GamepadOptionKeyItemVM> FaceKeys
		{
			get
			{
				return this._faceKeys;
			}
			set
			{
				if (value != this._faceKeys)
				{
					this._faceKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GamepadOptionKeyItemVM>>(value, "FaceKeys");
				}
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00020343 File Offset: 0x0001E543
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x0002034B File Offset: 0x0001E54B
		[DataSourceProperty]
		public MBBindingList<SelectorVM<SelectorItemVM>> Actions
		{
			get
			{
				return this._actions;
			}
			set
			{
				if (value != this._actions)
				{
					this._actions = value;
					base.OnPropertyChangedWithValue<MBBindingList<SelectorVM<SelectorItemVM>>>(value, "Actions");
				}
			}
		}

		// Token: 0x04000423 RID: 1059
		private SelectorVM<SelectorItemVM> _categories;

		// Token: 0x04000424 RID: 1060
		private int _currentGamepadType = -1;

		// Token: 0x04000425 RID: 1061
		private MBBindingList<GamepadOptionKeyItemVM> _leftAnalogKeys;

		// Token: 0x04000426 RID: 1062
		private MBBindingList<GamepadOptionKeyItemVM> _rightAnalogKeys;

		// Token: 0x04000427 RID: 1063
		private MBBindingList<GamepadOptionKeyItemVM> _dpadKeys;

		// Token: 0x04000428 RID: 1064
		private MBBindingList<GamepadOptionKeyItemVM> _rightTriggerAndBumperKeys;

		// Token: 0x04000429 RID: 1065
		private MBBindingList<GamepadOptionKeyItemVM> _leftTriggerAndBumperKeys;

		// Token: 0x0400042A RID: 1066
		private MBBindingList<GamepadOptionKeyItemVM> _otherKeys;

		// Token: 0x0400042B RID: 1067
		private MBBindingList<GamepadOptionKeyItemVM> _faceKeys;

		// Token: 0x0400042C RID: 1068
		private MBBindingList<SelectorVM<SelectorItemVM>> _actions;

		// Token: 0x0200010A RID: 266
		private enum GamepadType
		{
			// Token: 0x040006BA RID: 1722
			Xbox,
			// Token: 0x040006BB RID: 1723
			Playstation4,
			// Token: 0x040006BC RID: 1724
			Playstation5
		}
	}
}
