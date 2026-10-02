using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x02000038 RID: 56
	public class MPLobbyLeaderboardPlayerItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x06000542 RID: 1346 RVA: 0x0001224C File Offset: 0x0001044C
		public MPLobbyLeaderboardPlayerItemVM(int rank, PlayerLeaderboardData playerLeaderboardData, Action<MPLobbyLeaderboardPlayerItemVM> onActivatePlayerActions)
			: base(playerLeaderboardData.PlayerId, playerLeaderboardData.Name, null, null)
		{
			this.Rank = rank;
			base.Rating = playerLeaderboardData.Rating;
			base.RatingID = playerLeaderboardData.RankId;
			this._onActivatePlayerActions = onActivatePlayerActions;
			this.RefreshValues();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00012299 File Offset: 0x00010499
		private void ExecuteActivatePlayerActions()
		{
			Action<MPLobbyLeaderboardPlayerItemVM> onActivatePlayerActions = this._onActivatePlayerActions;
			if (onActivatePlayerActions == null)
			{
				return;
			}
			onActivatePlayerActions(this);
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x000122AC File Offset: 0x000104AC
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x000122B4 File Offset: 0x000104B4
		[DataSourceProperty]
		public int Rank
		{
			get
			{
				return this._rank;
			}
			set
			{
				if (value != this._rank)
				{
					this._rank = value;
					base.OnPropertyChangedWithValue(value, "Rank");
				}
			}
		}

		// Token: 0x04000282 RID: 642
		public readonly MatchHistoryData MatchOfThePlayer;

		// Token: 0x04000283 RID: 643
		private readonly Action<MPLobbyLeaderboardPlayerItemVM> _onActivatePlayerActions;

		// Token: 0x04000284 RID: 644
		private int _rank;
	}
}
