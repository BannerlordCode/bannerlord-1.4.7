using System;
using System.Linq;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GamepadOptions
{
	// Token: 0x02000076 RID: 118
	public class GamepadOptionKeyItemVM : ViewModel
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x00020369 File Offset: 0x0001E569
		public GameKey GamepadKey { get; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x00020371 File Offset: 0x0001E571
		public HotKey GamepadHotKey { get; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x00020379 File Offset: 0x0001E579
		public InputKey? Key { get; }

		// Token: 0x06000975 RID: 2421 RVA: 0x00020384 File Offset: 0x0001E584
		public GamepadOptionKeyItemVM(GameKey gamepadGameKey)
		{
			this.GamepadKey = gamepadGameKey;
			this.Action = Module.CurrentModule.GlobalTextManager.FindText("str_key_name", this.GamepadKey.GroupId + "_" + this.GamepadKey.StringId).ToString();
			this.Key = new InputKey?(gamepadGameKey.ControllerKey.InputKey);
			this.KeyId = (int)this.Key.Value;
			string text;
			if (gamepadGameKey == null)
			{
				text = null;
			}
			else
			{
				Key controllerKey = gamepadGameKey.ControllerKey;
				text = ((controllerKey != null) ? controllerKey.InputKey.ToString() : null);
			}
			this.KeyIdAsString = text ?? string.Empty;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0002043C File Offset: 0x0001E63C
		public GamepadOptionKeyItemVM(HotKey gamepadHotKey)
		{
			this.GamepadHotKey = gamepadHotKey;
			this.Action = Module.CurrentModule.GlobalTextManager.FindText("str_key_name", this.GamepadHotKey.GroupId + "_" + this.GamepadHotKey.Id).ToString();
			Key key = gamepadHotKey.Keys.FirstOrDefault<Key>((Key k) => k.IsControllerInput);
			InputKey? inputKey;
			InputKey? inputKey2;
			if (key == null)
			{
				inputKey = null;
				inputKey2 = inputKey;
			}
			else
			{
				inputKey2 = new InputKey?(key.InputKey);
			}
			this.Key = inputKey2;
			inputKey = this.Key;
			this.KeyId = (int)inputKey.Value;
			inputKey = this.Key;
			this.KeyIdAsString = ((inputKey != null) ? inputKey.GetValueOrDefault().ToString() : null) ?? string.Empty;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0002052C File Offset: 0x0001E72C
		public GamepadOptionKeyItemVM(InputKey key, TextObject name)
		{
			this.Key = new InputKey?(key);
			InputKey? inputKey = this.Key;
			this.KeyId = (int)inputKey.Value;
			inputKey = this.Key;
			this.KeyIdAsString = ((inputKey != null) ? inputKey.GetValueOrDefault().ToString() : null) ?? string.Empty;
			this._nameObject = name;
			this.Action = this._nameObject.ToString();
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x000205B0 File Offset: 0x0001E7B0
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.GamepadKey != null)
			{
				this.Action = Module.CurrentModule.GlobalTextManager.FindText("str_key_name", this.GamepadKey.GroupId + "_" + this.GamepadKey.StringId).ToString();
				return;
			}
			if (this.GamepadHotKey != null)
			{
				this.Action = Module.CurrentModule.GlobalTextManager.FindText("str_key_name", this.GamepadHotKey.GroupId + "_" + this.GamepadHotKey.Id).ToString();
				return;
			}
			if (this._nameObject != null)
			{
				this.Action = this._nameObject.ToString();
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00020672 File Offset: 0x0001E872
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0002067A File Offset: 0x0001E87A
		[DataSourceProperty]
		public string Action
		{
			get
			{
				return this._action;
			}
			set
			{
				if (value != this._action)
				{
					this._action = value;
					base.OnPropertyChangedWithValue<string>(value, "Action");
				}
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0002069D File Offset: 0x0001E89D
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x000206A5 File Offset: 0x0001E8A5
		[DataSourceProperty]
		public int KeyId
		{
			get
			{
				return this._keyId;
			}
			set
			{
				if (value != this._keyId)
				{
					this._keyId = value;
					base.OnPropertyChangedWithValue(value, "KeyId");
				}
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x000206C3 File Offset: 0x0001E8C3
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x000206CB File Offset: 0x0001E8CB
		[DataSourceProperty]
		public string KeyIdAsString
		{
			get
			{
				return this._keyIdAsString;
			}
			set
			{
				if (value != this._keyIdAsString)
				{
					this._keyIdAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "KeyIdAsString");
				}
			}
		}

		// Token: 0x0400042F RID: 1071
		private TextObject _nameObject;

		// Token: 0x04000431 RID: 1073
		private string _action;

		// Token: 0x04000432 RID: 1074
		private string _keyIdAsString;

		// Token: 0x04000433 RID: 1075
		private int _keyId;
	}
}
