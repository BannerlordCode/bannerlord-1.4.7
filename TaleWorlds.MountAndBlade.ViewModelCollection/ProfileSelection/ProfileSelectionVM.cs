using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.ProfileSelection
{
	// Token: 0x02000017 RID: 23
	public class ProfileSelectionVM : ViewModel
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x000069A4 File Offset: 0x00004BA4
		public ProfileSelectionVM(bool isDirectPlayPossible)
		{
			this.SelectProfileText = new TextObject("{=wubDWOlh}Select Profile", null).ToString();
			this.SelectProfileKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SelectProfile"), false);
			this.PlayKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Play"), false);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00006A10 File Offset: 0x00004C10
		public void OnActivate(bool isDirectPlayPossible)
		{
			this.IsPlayEnabled = isDirectPlayPossible;
			if (!string.IsNullOrEmpty(PlatformServices.Instance.UserDisplayName))
			{
				this.PlayText = new TextObject("{=FTXx0aRp}Play as", null).ToString() + PlatformServices.Instance.UserDisplayName;
				return;
			}
			this.PlayText = new TextObject("{=playgame}Play", null).ToString();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00006A71 File Offset: 0x00004C71
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.SelectProfileKey.OnFinalize();
			this.PlayKey.OnFinalize();
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00006A8F File Offset: 0x00004C8F
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00006A97 File Offset: 0x00004C97
		[DataSourceProperty]
		public string SelectProfileText
		{
			get
			{
				return this._selectProfileText;
			}
			set
			{
				if (value != this._selectProfileText)
				{
					this._selectProfileText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectProfileText");
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00006ABA File Offset: 0x00004CBA
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00006AC2 File Offset: 0x00004CC2
		[DataSourceProperty]
		public bool IsPlayEnabled
		{
			get
			{
				return this._isPlayEnabled;
			}
			set
			{
				if (value != this._isPlayEnabled)
				{
					this._isPlayEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPlayEnabled");
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00006AE0 File Offset: 0x00004CE0
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00006AE8 File Offset: 0x00004CE8
		[DataSourceProperty]
		public string PlayText
		{
			get
			{
				return this._playText;
			}
			set
			{
				if (value != this._playText)
				{
					this._playText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayText");
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00006B0B File Offset: 0x00004D0B
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00006B13 File Offset: 0x00004D13
		[DataSourceProperty]
		public InputKeyItemVM SelectProfileKey
		{
			get
			{
				return this._selectProfileKey;
			}
			set
			{
				if (value != this._selectProfileKey)
				{
					this._selectProfileKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "SelectProfileKey");
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00006B31 File Offset: 0x00004D31
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00006B39 File Offset: 0x00004D39
		[DataSourceProperty]
		public InputKeyItemVM PlayKey
		{
			get
			{
				return this._playKey;
			}
			set
			{
				if (value != this._playKey)
				{
					this._playKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PlayKey");
				}
			}
		}

		// Token: 0x040000D2 RID: 210
		private bool _isPlayEnabled;

		// Token: 0x040000D3 RID: 211
		private string _selectProfileText;

		// Token: 0x040000D4 RID: 212
		private string _playText;

		// Token: 0x040000D5 RID: 213
		private InputKeyItemVM _playKey;

		// Token: 0x040000D6 RID: 214
		private InputKeyItemVM _selectProfileKey;
	}
}
