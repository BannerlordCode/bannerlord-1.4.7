using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Input
{
	// Token: 0x02000049 RID: 73
	public class InputKeyItemVM : ViewModel
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00017010 File Offset: 0x00015210
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x00017018 File Offset: 0x00015218
		public GameKey GameKey { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00017021 File Offset: 0x00015221
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x00017029 File Offset: 0x00015229
		public HotKey HotKey { get; private set; }

		// Token: 0x0600062F RID: 1583 RVA: 0x00017032 File Offset: 0x00015232
		private InputKeyItemVM()
		{
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged += this.OnKeybindsChanged;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001706B File Offset: 0x0001526B
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged -= this.OnKeybindsChanged;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000170A4 File Offset: 0x000152A4
		private void OnGamepadActiveStateChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000170AC File Offset: 0x000152AC
		private void OnKeybindsChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000170B4 File Offset: 0x000152B4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ForceRefresh();
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000170C2 File Offset: 0x000152C2
		public void SetForcedVisibility(bool? isVisible)
		{
			this._forcedVisibility = isVisible;
			this.UpdateVisibility();
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000170D4 File Offset: 0x000152D4
		private void ForceRefresh()
		{
			this.UpdateVisibility();
			this.KeyID = string.Empty;
			this.KeyName = string.Empty;
			if (this._forcedID != null)
			{
				this.KeyID = this._forcedID;
				TextObject forcedName = this._forcedName;
				this.KeyName = ((forcedName != null) ? forcedName.ToString() : null) ?? string.Empty;
				return;
			}
			this.KeyID = this.GetKeyId();
			this.KeyName = this.GetKeyName().ToString();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00017150 File Offset: 0x00015350
		private string GetKeyId()
		{
			if (Input.IsGamepadActive)
			{
				if (this.GameKey != null)
				{
					Key controllerKey = this.GameKey.ControllerKey;
					if (controllerKey == null)
					{
						return null;
					}
					return controllerKey.InputKey.ToString();
				}
				else if (this.HotKey != null)
				{
					Key key = this.HotKey.Keys.Find((Key k) => k.IsControllerInput);
					if (key == null)
					{
						return null;
					}
					return key.InputKey.ToString();
				}
			}
			if (this.GameKey != null)
			{
				Key keyboardKey = this.GameKey.KeyboardKey;
				if (keyboardKey == null)
				{
					return null;
				}
				return keyboardKey.InputKey.ToString();
			}
			else
			{
				if (this.HotKey == null)
				{
					return string.Empty;
				}
				Key key2 = this.HotKey.Keys.Find((Key k) => !k.IsControllerInput);
				if (key2 == null)
				{
					return null;
				}
				return key2.InputKey.ToString();
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00017268 File Offset: 0x00015468
		private TextObject GetKeyName()
		{
			if (this._forcedName != null)
			{
				return this._forcedName;
			}
			if (Game.Current != null)
			{
				if (this.HotKey != null)
				{
					return Game.Current.GameTextManager.FindText("str_key_name", this.HotKey.GroupId + "_" + this.HotKey.Id);
				}
				if (this.GameKey != null)
				{
					return Game.Current.GameTextManager.FindText("str_key_name", this.GameKey.GroupId + "_" + this.GameKey.StringId);
				}
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00017310 File Offset: 0x00015510
		private void UpdateVisibility()
		{
			this.IsVisible = this._forcedVisibility ?? (!this._isVisibleToConsoleOnly || Input.IsGamepadActive);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001734C File Offset: 0x0001554C
		public static InputKeyItemVM CreateFromGameKey(GameKey gameKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00017367 File Offset: 0x00015567
		public static InputKeyItemVM CreateFromHotKey(HotKey hotKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00017382 File Offset: 0x00015582
		public static InputKeyItemVM CreateFromHotKeyWithForcedName(HotKey hotKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000173A4 File Offset: 0x000155A4
		public static InputKeyItemVM CreateFromGameKeyWithForcedName(GameKey gameKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000173C6 File Offset: 0x000155C6
		public static InputKeyItemVM CreateFromForcedID(string forcedID, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM._forcedID = forcedID;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x000173E8 File Offset: 0x000155E8
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x000173F0 File Offset: 0x000155F0
		[DataSourceProperty]
		public string KeyID
		{
			get
			{
				return this._keyID;
			}
			set
			{
				if (value != this._keyID)
				{
					this._keyID = value;
					base.OnPropertyChangedWithValue<string>(value, "KeyID");
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x00017413 File Offset: 0x00015613
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x0001741B File Offset: 0x0001561B
		[DataSourceProperty]
		public string KeyName
		{
			get
			{
				return this._keyName;
			}
			set
			{
				if (value != this._keyName)
				{
					this._keyName = value;
					base.OnPropertyChangedWithValue<string>(value, "KeyName");
				}
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0001743E File Offset: 0x0001563E
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x00017446 File Offset: 0x00015646
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x040002C4 RID: 708
		private bool _isVisibleToConsoleOnly;

		// Token: 0x040002C5 RID: 709
		private TextObject _forcedName;

		// Token: 0x040002C6 RID: 710
		private string _forcedID;

		// Token: 0x040002C7 RID: 711
		private bool? _forcedVisibility;

		// Token: 0x040002C8 RID: 712
		private string _keyID;

		// Token: 0x040002C9 RID: 713
		private string _keyName;

		// Token: 0x040002CA RID: 714
		private bool _isVisible;
	}
}
