using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000024 RID: 36
	public class MPLobbyClassFilterVM : ViewModel
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000A186 File Offset: 0x00008386
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000A18E File Offset: 0x0000838E
		public MPLobbyClassFilterClassItemVM SelectedClassItem { get; private set; }

		// Token: 0x0600028B RID: 651 RVA: 0x0000A198 File Offset: 0x00008398
		public MPLobbyClassFilterVM(Action<MPLobbyClassFilterClassItemVM, bool> onSelectionChange)
		{
			this._onSelectionChange = onSelectionChange;
			this.Factions = new MBBindingList<MPLobbyClassFilterFactionItemVM>();
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("empire", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("vlandia", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("battania", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("sturgia", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("khuzait", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.Factions.Add(new MPLobbyClassFilterFactionItemVM("aserai", true, new Action<MPLobbyClassFilterFactionItemVM>(this.OnFactionFilterChanged), new Action<MPLobbyClassFilterClassItemVM>(this.OnSelectionChange)));
			this.ActiveClassGroups = new MBBindingList<MPLobbyClassFilterClassGroupItemVM>();
			this.Factions[0].IsActive = true;
			this.RefreshValues();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000A2F4 File Offset: 0x000084F4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=Q50X65NB}Classes", null).ToString();
			this.Factions.ApplyActionOnAllItems(delegate(MPLobbyClassFilterFactionItemVM x)
			{
				x.RefreshValues();
			});
			this.ActiveClassGroups.ApplyActionOnAllItems(delegate(MPLobbyClassFilterClassGroupItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A371 File Offset: 0x00008571
		private void OnFactionFilterChanged(MPLobbyClassFilterFactionItemVM factionItemVm)
		{
			this.ActiveClassGroups = factionItemVm.ClassGroups;
			this.OnSelectionChange(factionItemVm.SelectedClassItem);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000A38B File Offset: 0x0000858B
		private void OnSelectionChange(MPLobbyClassFilterClassItemVM selectedItemVm)
		{
			this.SelectedClassItem = selectedItemVm;
			Action<MPLobbyClassFilterClassItemVM, bool> onSelectionChange = this._onSelectionChange;
			if (onSelectionChange == null)
			{
				return;
			}
			onSelectionChange(selectedItemVm, false);
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000A3A6 File Offset: 0x000085A6
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000A3AE File Offset: 0x000085AE
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
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000A3D1 File Offset: 0x000085D1
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000A3D9 File Offset: 0x000085D9
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterFactionItemVM> Factions
		{
			get
			{
				return this._factions;
			}
			set
			{
				if (value != this._factions)
				{
					this._factions = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterFactionItemVM>>(value, "Factions");
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000A3F7 File Offset: 0x000085F7
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000A3FF File Offset: 0x000085FF
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterClassGroupItemVM> ActiveClassGroups
		{
			get
			{
				return this._activeClassGroups;
			}
			set
			{
				if (value != this._activeClassGroups)
				{
					this._activeClassGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterClassGroupItemVM>>(value, "ActiveClassGroups");
				}
			}
		}

		// Token: 0x04000157 RID: 343
		private Action<MPLobbyClassFilterClassItemVM, bool> _onSelectionChange;

		// Token: 0x04000159 RID: 345
		private string _titleText;

		// Token: 0x0400015A RID: 346
		private MBBindingList<MPLobbyClassFilterFactionItemVM> _factions;

		// Token: 0x0400015B RID: 347
		private MBBindingList<MPLobbyClassFilterClassGroupItemVM> _activeClassGroups;
	}
}
