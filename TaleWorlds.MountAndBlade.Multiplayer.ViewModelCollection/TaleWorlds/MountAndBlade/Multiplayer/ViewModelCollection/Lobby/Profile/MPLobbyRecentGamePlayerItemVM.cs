using System;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003E RID: 62
	public class MPLobbyRecentGamePlayerItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x00013E58 File Offset: 0x00012058
		public MPLobbyRecentGamePlayerItemVM(PlayerId playerId, MatchHistoryData matchOfThePlayer, Action<MPLobbyRecentGamePlayerItemVM> onActivatePlayerActions)
			: base(playerId, "", null, null)
		{
			this.MatchOfThePlayer = matchOfThePlayer;
			this._onActivatePlayerActions = onActivatePlayerActions;
			PlayerInfo playerInfo = this.MatchOfThePlayer.Players.FirstOrDefault<PlayerInfo>((PlayerInfo p) => p.PlayerId == playerId.ToString());
			if (playerInfo != null)
			{
				this.KillCount = playerInfo.Kill;
				this.DeathCount = playerInfo.Death;
				this.AssistCount = playerInfo.Assist;
			}
			this.RefreshValues();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00013EDD File Offset: 0x000120DD
		private void ExecuteActivatePlayerActions()
		{
			Action<MPLobbyRecentGamePlayerItemVM> onActivatePlayerActions = this._onActivatePlayerActions;
			if (onActivatePlayerActions == null)
			{
				return;
			}
			onActivatePlayerActions(this);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00013EF0 File Offset: 0x000120F0
		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00013EF8 File Offset: 0x000120F8
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00013F00 File Offset: 0x00012100
		[DataSourceProperty]
		public int KillCount
		{
			get
			{
				return this._killCount;
			}
			set
			{
				if (value != this._killCount)
				{
					this._killCount = value;
					base.OnPropertyChangedWithValue(value, "KillCount");
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00013F1E File Offset: 0x0001211E
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x00013F26 File Offset: 0x00012126
		[DataSourceProperty]
		public int DeathCount
		{
			get
			{
				return this._deathCount;
			}
			set
			{
				if (value != this._deathCount)
				{
					this._deathCount = value;
					base.OnPropertyChangedWithValue(value, "DeathCount");
				}
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00013F44 File Offset: 0x00012144
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00013F4C File Offset: 0x0001214C
		[DataSourceProperty]
		public int AssistCount
		{
			get
			{
				return this._assistCount;
			}
			set
			{
				if (value != this._assistCount)
				{
					this._assistCount = value;
					base.OnPropertyChangedWithValue(value, "AssistCount");
				}
			}
		}

		// Token: 0x040002D6 RID: 726
		public readonly MatchHistoryData MatchOfThePlayer;

		// Token: 0x040002D7 RID: 727
		private readonly Action<MPLobbyRecentGamePlayerItemVM> _onActivatePlayerActions;

		// Token: 0x040002D8 RID: 728
		private int _killCount;

		// Token: 0x040002D9 RID: 729
		private int _deathCount;

		// Token: 0x040002DA RID: 730
		private int _assistCount;
	}
}
