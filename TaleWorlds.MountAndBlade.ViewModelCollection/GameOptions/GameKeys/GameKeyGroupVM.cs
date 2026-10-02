using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions.GameKeys
{
	// Token: 0x02000077 RID: 119
	public class GameKeyGroupVM : ViewModel
	{
		// Token: 0x0600097F RID: 2431 RVA: 0x000206F0 File Offset: 0x0001E8F0
		public GameKeyGroupVM(string categoryId, IEnumerable<GameKey> keys, Action<KeyOptionVM> onKeybindRequest, Action<int, InputKey> setAllKeysOfId, Func<KeyOptionVM, string> getExtraInformation)
		{
			this._onKeybindRequest = onKeybindRequest;
			this._setAllKeysOfId = setAllKeysOfId;
			this._getExtraInformation = getExtraInformation;
			this._categoryId = categoryId;
			this._gameKeys = new MBBindingList<GameKeyOptionVM>();
			this._keys = keys;
			this.PopulateGameKeys();
			this.RefreshValues();
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00020740 File Offset: 0x0001E940
		private void PopulateGameKeys()
		{
			this.GameKeys.Clear();
			foreach (GameKey gameKey in this._keys)
			{
				if (Input.IsGamepadActive ? (((gameKey != null) ? gameKey.DefaultControllerKey : null) != null && (gameKey == null || gameKey.DefaultControllerKey.InputKey != InputKey.Invalid)) : (((gameKey != null) ? gameKey.DefaultKeyboardKey : null) != null && (gameKey == null || gameKey.DefaultKeyboardKey.InputKey != InputKey.Invalid)))
				{
					this.GameKeys.Add(new GameKeyOptionVM(gameKey, this._onKeybindRequest, new Action<GameKeyOptionVM, InputKey>(this.SetGameKey), this._getExtraInformation));
				}
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00020828 File Offset: 0x0001EA28
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Description = Module.CurrentModule.GlobalTextManager.FindText("str_key_category_name", this._categoryId).ToString();
			this.GameKeys.ApplyActionOnAllItems(delegate(GameKeyOptionVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0002088C File Offset: 0x0001EA8C
		private void SetGameKey(GameKeyOptionVM option, InputKey newKey)
		{
			InputKey inputKey = option.CurrentKey.InputKey;
			if (newKey != inputKey)
			{
				option.CurrentKey.ChangeKey(newKey);
				option.OptionValueText = Module.CurrentModule.GlobalTextManager.GetHotKeyGameTextFromKeyID(option.CurrentKey.ToString().ToLower()).ToString();
				option.UpdateIsChanged();
				this._setAllKeysOfId(option.CurrentGameKey.Id, newKey);
				GameKeyOptionVM gameKeyOptionVM = this.GameKeys.FirstOrDefault<GameKeyOptionVM>((GameKeyOptionVM k) => k != option && k.CurrentKey.InputKey == option.CurrentKey.InputKey);
				if (gameKeyOptionVM != null)
				{
					gameKeyOptionVM.Set(inputKey);
				}
				if (gameKeyOptionVM != null)
				{
					MBInformationManager.AddQuickInformation(new TextObject("{=gb2S2aRq}Swapped {FIRST_KEY} and {SECOND_KEY}", null).SetTextVariable("FIRST_KEY", option.Name).SetTextVariable("SECOND_KEY", gameKeyOptionVM.Name), -1000, null, null, "");
				}
			}
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00020990 File Offset: 0x0001EB90
		internal void Update()
		{
			foreach (GameKeyOptionVM gameKeyOptionVM in this.GameKeys)
			{
				gameKeyOptionVM.Update();
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x000209DC File Offset: 0x0001EBDC
		public void OnDone()
		{
			foreach (GameKeyOptionVM gameKeyOptionVM in this.GameKeys)
			{
				gameKeyOptionVM.OnDone();
			}
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00020A28 File Offset: 0x0001EC28
		internal bool IsChanged()
		{
			for (int i = 0; i < this.GameKeys.Count; i++)
			{
				if (this.GameKeys[i].IsChanged)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00020A61 File Offset: 0x0001EC61
		public void OnGamepadActiveStateChanged()
		{
			this.PopulateGameKeys();
			this.Update();
			this.OnDone();
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00020A75 File Offset: 0x0001EC75
		public void Cancel()
		{
			this.GameKeys.ApplyActionOnAllItems(delegate(GameKeyOptionVM g)
			{
				g.ExecuteRevert();
			});
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00020AA1 File Offset: 0x0001ECA1
		public void ApplyValues()
		{
			this.GameKeys.ApplyActionOnAllItems(delegate(GameKeyOptionVM g)
			{
				g.Apply();
			});
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00020ACD File Offset: 0x0001ECCD
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x00020AD5 File Offset: 0x0001ECD5
		[DataSourceProperty]
		public MBBindingList<GameKeyOptionVM> GameKeys
		{
			get
			{
				return this._gameKeys;
			}
			set
			{
				if (value != this._gameKeys)
				{
					this._gameKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameKeyOptionVM>>(value, "GameKeys");
				}
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00020AF3 File Offset: 0x0001ECF3
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x00020AFB File Offset: 0x0001ECFB
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x04000434 RID: 1076
		private readonly Action<KeyOptionVM> _onKeybindRequest;

		// Token: 0x04000435 RID: 1077
		private readonly Action<int, InputKey> _setAllKeysOfId;

		// Token: 0x04000436 RID: 1078
		private readonly Func<KeyOptionVM, string> _getExtraInformation;

		// Token: 0x04000437 RID: 1079
		private readonly string _categoryId;

		// Token: 0x04000438 RID: 1080
		private IEnumerable<GameKey> _keys;

		// Token: 0x04000439 RID: 1081
		private string _description;

		// Token: 0x0400043A RID: 1082
		private MBBindingList<GameKeyOptionVM> _gameKeys;
	}
}
