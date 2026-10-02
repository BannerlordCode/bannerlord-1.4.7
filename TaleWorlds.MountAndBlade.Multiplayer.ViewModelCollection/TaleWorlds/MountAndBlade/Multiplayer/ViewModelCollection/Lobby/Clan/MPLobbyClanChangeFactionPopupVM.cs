using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000066 RID: 102
	public class MPLobbyClanChangeFactionPopupVM : ViewModel
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x0001E7B7 File Offset: 0x0001C9B7
		public MPLobbyClanChangeFactionPopupVM()
		{
			this.PrepareFactionsList();
			this.CanChangeFaction = false;
			this.RefreshValues();
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0001E7D2 File Offset: 0x0001C9D2
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=ghjSIyIL}Choose Culture", null).ToString();
			this.ApplyText = new TextObject("{=BAaS5Dkc}Apply", null).ToString();
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0001E808 File Offset: 0x0001CA08
		private void PrepareFactionsList()
		{
			this._selectedFaction = null;
			this.FactionsList = new MBBindingList<MPCultureItemVM>
			{
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection))
			};
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0001E948 File Offset: 0x0001CB48
		private void OnFactionSelection(MPCultureItemVM faction)
		{
			if (faction != this._selectedFaction)
			{
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = false;
				}
				this._selectedFaction = faction;
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = true;
					this.CanChangeFaction = true;
				}
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0001E994 File Offset: 0x0001CB94
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0001E99D File Offset: 0x0001CB9D
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0001E9A8 File Offset: 0x0001CBA8
		public void ExecuteChangeFaction()
		{
			BasicCultureObject @object = Game.Current.ObjectManager.GetObject<BasicCultureObject>(this._selectedFaction.CultureCode);
			Banner banner = new Banner(NetworkMain.GameClient.ClanInfo.Sigil);
			banner.ChangeIconColors(@object.ForegroundColor1);
			banner.ChangePrimaryColor(@object.BackgroundColor1);
			NetworkMain.GameClient.ChangeClanSigil(banner.Serialize());
			NetworkMain.GameClient.ChangeClanFaction(this._selectedFaction.CultureCode);
			this.ExecuteClosePopup();
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0001EA28 File Offset: 0x0001CC28
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0001EA51 File Offset: 0x0001CC51
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0001EA60 File Offset: 0x0001CC60
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0001EA6F File Offset: 0x0001CC6F
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x0001EA77 File Offset: 0x0001CC77
		[DataSourceProperty]
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
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0001EA94 File Offset: 0x0001CC94
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x0001EA9C File Offset: 0x0001CC9C
		[DataSourceProperty]
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
					base.OnPropertyChanged("DoneInputKey");
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0001EAB9 File Offset: 0x0001CCB9
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x0001EAC1 File Offset: 0x0001CCC1
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0001EADE File Offset: 0x0001CCDE
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x0001EAE6 File Offset: 0x0001CCE6
		[DataSourceProperty]
		public bool CanChangeFaction
		{
			get
			{
				return this._canChangeFaction;
			}
			set
			{
				if (value != this._canChangeFaction)
				{
					this._canChangeFaction = value;
					base.OnPropertyChanged("CanChangeFaction");
				}
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0001EB03 File Offset: 0x0001CD03
		// (set) Token: 0x060009D5 RID: 2517 RVA: 0x0001EB0B File Offset: 0x0001CD0B
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0001EB2D File Offset: 0x0001CD2D
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x0001EB35 File Offset: 0x0001CD35
		[DataSourceProperty]
		public string ApplyText
		{
			get
			{
				return this._applyText;
			}
			set
			{
				if (value != this._applyText)
				{
					this._applyText = value;
					base.OnPropertyChanged("ApplyText");
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0001EB57 File Offset: 0x0001CD57
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0001EB5F File Offset: 0x0001CD5F
		[DataSourceProperty]
		public MBBindingList<MPCultureItemVM> FactionsList
		{
			get
			{
				return this._factionsList;
			}
			set
			{
				if (value != this._factionsList)
				{
					this._factionsList = value;
					base.OnPropertyChanged("FactionsList");
				}
			}
		}

		// Token: 0x04000481 RID: 1153
		private MPCultureItemVM _selectedFaction;

		// Token: 0x04000482 RID: 1154
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000483 RID: 1155
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000484 RID: 1156
		private bool _isSelected;

		// Token: 0x04000485 RID: 1157
		private bool _canChangeFaction;

		// Token: 0x04000486 RID: 1158
		private string _titleText;

		// Token: 0x04000487 RID: 1159
		private string _applyText;

		// Token: 0x04000488 RID: 1160
		private MBBindingList<MPCultureItemVM> _factionsList;
	}
}
