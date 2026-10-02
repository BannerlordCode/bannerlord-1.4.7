using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x0200005E RID: 94
	public class MPCustomGameFiltersVM : ViewModel
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x0001BD64 File Offset: 0x00019F64
		public MPCustomGameFiltersVM()
		{
			this.SearchText = string.Empty;
			MBBindingList<MPCustomGameFilterItemVM> mbbindingList = new MBBindingList<MPCustomGameFilterItemVM>();
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.IsOfficial, new TextObject("{=Tlc2buKG}Is Official", null), (GameServerEntry x) => x.IsOfficial, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.HasPlayers, new TextObject("{=aB4Md0if}Has players", null), (GameServerEntry x) => x.PlayerCount > 0, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.HasPasswordProtection, new TextObject("{=v6J8ILV3}No password", null), (GameServerEntry x) => !x.PasswordProtected, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.NotFull, new TextObject("{=W4DLzPSb}Server not full", null), (GameServerEntry x) => x.MaxPlayerCount - x.PlayerCount > 0, new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.ModuleCompatible, new TextObject("{=CNR4cZwZ}Modules compatible", null), new Func<GameServerEntry, bool>(this.FilterByCompatibleModules), new Action(this.OnAnyFilterChange)));
			mbbindingList.Add(new MPCustomGameFilterItemVM(MPCustomGameFiltersVM.CustomGameFilterType.Favorite, new TextObject("{=BDdVhfuJ}Favorite", null), new Func<GameServerEntry, bool>(this.FilterByFavorites), new Action(this.OnAnyFilterChange)));
			this.Items = mbbindingList;
			this.RefreshValues();
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001BEFC File Offset: 0x0001A0FC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=OwqFpPwa}Filters", null).ToString();
			this.SearchInitialText = new TextObject("{=NLKmdNbt}Search", null).ToString();
			this.Items.ApplyActionOnAllItems(delegate(MPCustomGameFilterItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001BF68 File Offset: 0x0001A168
		public List<GameServerEntry> GetFilteredServerList(IEnumerable<GameServerEntry> unfilteredList)
		{
			List<GameServerEntry> list = unfilteredList.ToList<GameServerEntry>();
			IEnumerable<MPCustomGameFilterItemVM> enabledFilterItems = this.Items.Where<MPCustomGameFilterItemVM>((MPCustomGameFilterItemVM filterItem) => filterItem.IsSelected);
			if (enabledFilterItems.Any<MPCustomGameFilterItemVM>())
			{
				list.RemoveAll((GameServerEntry s) => enabledFilterItems.Any<MPCustomGameFilterItemVM>((MPCustomGameFilterItemVM fi) => !fi.GetIsApplicaple(s)));
			}
			if (!string.IsNullOrEmpty(this.SearchText))
			{
				list = list.Where<GameServerEntry>((GameServerEntry i) => i.ServerName.IndexOf(this.SearchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList<GameServerEntry>();
			}
			return list;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001BFFF File Offset: 0x0001A1FF
		private bool FilterByCompatibleModules(GameServerEntry serverEntry)
		{
			return NetworkMain.GameClient.LoadedUnofficialModules.IsCompatibleWith(serverEntry.LoadedModules, serverEntry.AllowsOptionalModules);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001C01C File Offset: 0x0001A21C
		private bool FilterByFavorites(GameServerEntry serverEntry)
		{
			FavoriteServerData favoriteServerData;
			return MultiplayerLocalDataManager.Instance.FavoriteServers.TryGetServerData(serverEntry, out favoriteServerData);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001C03B File Offset: 0x0001A23B
		private void OnAnyFilterChange()
		{
			Action onFiltersApplied = this.OnFiltersApplied;
			if (onFiltersApplied == null)
			{
				return;
			}
			onFiltersApplied();
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0001C04D File Offset: 0x0001A24D
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x0001C055 File Offset: 0x0001A255
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

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0001C078 File Offset: 0x0001A278
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x0001C080 File Offset: 0x0001A280
		[DataSourceProperty]
		public string SearchInitialText
		{
			get
			{
				return this._searchInitialText;
			}
			set
			{
				if (value != this._searchInitialText)
				{
					this._searchInitialText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchInitialText");
				}
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0001C0A3 File Offset: 0x0001A2A3
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x0001C0AB File Offset: 0x0001A2AB
		[DataSourceProperty]
		public string SearchText
		{
			get
			{
				return this._searchText;
			}
			set
			{
				if (value != this._searchText)
				{
					this._searchText = value;
					base.OnPropertyChangedWithValue<string>(value, "SearchText");
					this.OnAnyFilterChange();
				}
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x0001C0DC File Offset: 0x0001A2DC
		[DataSourceProperty]
		public MBBindingList<MPCustomGameFilterItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPCustomGameFilterItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x0400040A RID: 1034
		public Action OnFiltersApplied;

		// Token: 0x0400040B RID: 1035
		private string _titleText;

		// Token: 0x0400040C RID: 1036
		private string _searchInitialText;

		// Token: 0x0400040D RID: 1037
		private string _searchText;

		// Token: 0x0400040E RID: 1038
		private MBBindingList<MPCustomGameFilterItemVM> _items;

		// Token: 0x02000126 RID: 294
		public enum CustomGameFilterType
		{
			// Token: 0x0400093C RID: 2364
			Name,
			// Token: 0x0400093D RID: 2365
			NotFull,
			// Token: 0x0400093E RID: 2366
			HasPlayers,
			// Token: 0x0400093F RID: 2367
			HasPasswordProtection,
			// Token: 0x04000940 RID: 2368
			IsOfficial,
			// Token: 0x04000941 RID: 2369
			ModuleCompatible,
			// Token: 0x04000942 RID: 2370
			Favorite
		}
	}
}
