using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000059 RID: 89
	public class MPLobbyPlayerRecentGameDataVM : ViewModel
	{
		// Token: 0x0600087C RID: 2172 RVA: 0x0001B6AB File Offset: 0x000198AB
		public MPLobbyPlayerRecentGameDataVM(int result, string gameType, string map, string date)
		{
			this.Result = result;
			this.GameType = gameType;
			this.Map = map;
			this.Date = date;
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x0001B6D0 File Offset: 0x000198D0
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x0001B6D8 File Offset: 0x000198D8
		[DataSourceProperty]
		public int Result
		{
			get
			{
				return this._result;
			}
			set
			{
				if (value != this._result)
				{
					this._result = value;
					base.OnPropertyChangedWithValue(value, "Result");
				}
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x0001B6F6 File Offset: 0x000198F6
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x0001B6FE File Offset: 0x000198FE
		[DataSourceProperty]
		public string GameType
		{
			get
			{
				return this._gameType;
			}
			set
			{
				if (value != this._gameType)
				{
					this._gameType = value;
					base.OnPropertyChangedWithValue<string>(value, "GameType");
				}
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x0001B721 File Offset: 0x00019921
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x0001B729 File Offset: 0x00019929
		[DataSourceProperty]
		public string Map
		{
			get
			{
				return this._map;
			}
			set
			{
				if (value != this._map)
				{
					this._map = value;
					base.OnPropertyChangedWithValue<string>(value, "Map");
				}
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x0001B74C File Offset: 0x0001994C
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x0001B754 File Offset: 0x00019954
		[DataSourceProperty]
		public string Date
		{
			get
			{
				return this._date;
			}
			set
			{
				if (value != this._date)
				{
					this._date = value;
					base.OnPropertyChangedWithValue<string>(value, "Date");
				}
			}
		}

		// Token: 0x040003F2 RID: 1010
		private int _result;

		// Token: 0x040003F3 RID: 1011
		private string _gameType;

		// Token: 0x040003F4 RID: 1012
		private string _map;

		// Token: 0x040003F5 RID: 1013
		private string _date;
	}
}
