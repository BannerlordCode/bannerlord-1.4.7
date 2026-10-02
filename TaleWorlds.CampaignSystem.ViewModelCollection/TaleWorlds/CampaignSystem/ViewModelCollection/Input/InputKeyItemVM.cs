using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Input
{
	// Token: 0x0200009A RID: 154
	public class InputKeyItemVM : ViewModel
	{
		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0003E8A4 File Offset: 0x0003CAA4
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x0003E8AC File Offset: 0x0003CAAC
		public GameKey GameKey { get; private set; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0003E8B5 File Offset: 0x0003CAB5
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x0003E8BD File Offset: 0x0003CABD
		public HotKey HotKey { get; private set; }

		// Token: 0x06000EFD RID: 3837 RVA: 0x0003E8C6 File Offset: 0x0003CAC6
		private InputKeyItemVM()
		{
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged += this.OnKeybindsChanged;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0003E8FF File Offset: 0x0003CAFF
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged -= this.OnKeybindsChanged;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0003E938 File Offset: 0x0003CB38
		private void OnGamepadActiveStateChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0003E940 File Offset: 0x0003CB40
		private void OnKeybindsChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0003E948 File Offset: 0x0003CB48
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ForceRefresh();
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0003E956 File Offset: 0x0003CB56
		public void SetForcedVisibility(bool? isVisible)
		{
			this._forcedVisibility = isVisible;
			this.UpdateVisibility();
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0003E968 File Offset: 0x0003CB68
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

		// Token: 0x06000F04 RID: 3844 RVA: 0x0003E9E4 File Offset: 0x0003CBE4
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

		// Token: 0x06000F05 RID: 3845 RVA: 0x0003EAFC File Offset: 0x0003CCFC
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

		// Token: 0x06000F06 RID: 3846 RVA: 0x0003EBA4 File Offset: 0x0003CDA4
		private void UpdateVisibility()
		{
			this.IsVisible = this._forcedVisibility ?? (!this._isVisibleToConsoleOnly || Input.IsGamepadActive);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0003EBE0 File Offset: 0x0003CDE0
		public static InputKeyItemVM CreateFromGameKey(GameKey gameKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0003EBFB File Offset: 0x0003CDFB
		public static InputKeyItemVM CreateFromHotKey(HotKey hotKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003EC16 File Offset: 0x0003CE16
		public static InputKeyItemVM CreateFromHotKeyWithForcedName(HotKey hotKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0003EC38 File Offset: 0x0003CE38
		public static InputKeyItemVM CreateFromGameKeyWithForcedName(GameKey gameKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0003EC5A File Offset: 0x0003CE5A
		public static InputKeyItemVM CreateFromForcedID(string forcedID, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM._forcedID = forcedID;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0003EC7C File Offset: 0x0003CE7C
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x0003EC84 File Offset: 0x0003CE84
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

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0003ECA7 File Offset: 0x0003CEA7
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x0003ECAF File Offset: 0x0003CEAF
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

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0003ECD2 File Offset: 0x0003CED2
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x0003ECDA File Offset: 0x0003CEDA
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

		// Token: 0x040006C7 RID: 1735
		private bool _isVisibleToConsoleOnly;

		// Token: 0x040006C8 RID: 1736
		private TextObject _forcedName;

		// Token: 0x040006C9 RID: 1737
		private string _forcedID;

		// Token: 0x040006CA RID: 1738
		private bool? _forcedVisibility;

		// Token: 0x040006CB RID: 1739
		private string _keyID;

		// Token: 0x040006CC RID: 1740
		private string _keyName;

		// Token: 0x040006CD RID: 1741
		private bool _isVisible;
	}
}
