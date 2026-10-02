using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x0200005D RID: 93
	public class MPCustomGameFilterItemVM : ViewModel
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x0001BBCD File Offset: 0x00019DCD
		public MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType filterType, TextObject description, Func<GameServerEntry, bool> getFilterApplicaple, Action onSelectionChange)
		{
			this._filterType = filterType;
			this._descriptionObj = description;
			this.GetIsApplicaple = getFilterApplicaple;
			this._onSelectionChange = onSelectionChange;
			this.SetInitialState();
			this.RefreshValues();
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001BBFE File Offset: 0x00019DFE
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Description = this._descriptionObj.ToString();
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001BC18 File Offset: 0x00019E18
		private void SetInitialState()
		{
			switch (this._filterType)
			{
			case MPCustomGameFiltersVM.CustomGameFilterType.NotFull:
				this.IsSelected = BannerlordConfig.HideFullServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPlayers:
				this.IsSelected = BannerlordConfig.HideEmptyServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPasswordProtection:
				this.IsSelected = BannerlordConfig.HidePasswordProtectedServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.IsOfficial:
				this.IsSelected = BannerlordConfig.HideUnofficialServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.ModuleCompatible:
				this.IsSelected = BannerlordConfig.HideModuleIncompatibleServers;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.Favorite:
				this.IsSelected = BannerlordConfig.ShowOnlyFavoriteServers;
				return;
			default:
				return;
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001BC94 File Offset: 0x00019E94
		private void OnToggled()
		{
			this._onSelectionChange();
			switch (this._filterType)
			{
			case MPCustomGameFiltersVM.CustomGameFilterType.NotFull:
				BannerlordConfig.HideFullServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPlayers:
				BannerlordConfig.HideEmptyServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.HasPasswordProtection:
				BannerlordConfig.HidePasswordProtectedServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.IsOfficial:
				BannerlordConfig.HideUnofficialServers = this.IsSelected;
				return;
			case MPCustomGameFiltersVM.CustomGameFilterType.ModuleCompatible:
				BannerlordConfig.HideModuleIncompatibleServers = this.IsSelected;
				return;
			default:
				return;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0001BD0B File Offset: 0x00019F0B
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x0001BD13 File Offset: 0x00019F13
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
					base.OnPropertyChangedWithValue(value, "IsSelected");
					this.OnToggled();
				}
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0001BD37 File Offset: 0x00019F37
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x0001BD3F File Offset: 0x00019F3F
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

		// Token: 0x04000404 RID: 1028
		public readonly Func<GameServerEntry, bool> GetIsApplicaple;

		// Token: 0x04000405 RID: 1029
		public readonly Action _onSelectionChange;

		// Token: 0x04000406 RID: 1030
		private readonly TextObject _descriptionObj;

		// Token: 0x04000407 RID: 1031
		private MPCustomGameFiltersVM.CustomGameFilterType _filterType;

		// Token: 0x04000408 RID: 1032
		private bool _isSelected;

		// Token: 0x04000409 RID: 1033
		private string _description;
	}
}
