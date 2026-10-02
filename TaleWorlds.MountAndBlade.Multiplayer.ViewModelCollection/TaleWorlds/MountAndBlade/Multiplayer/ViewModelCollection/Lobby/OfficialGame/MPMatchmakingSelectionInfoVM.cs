using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.OfficialGame
{
	// Token: 0x02000044 RID: 68
	public class MPMatchmakingSelectionInfoVM : ViewModel
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x00014AC6 File Offset: 0x00012CC6
		public MPMatchmakingSelectionInfoVM()
		{
			this.Name = "";
			this.Description = "";
			this.ExtraInfos = new MBBindingList<StringPairItemVM>();
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00014AF0 File Offset: 0x00012CF0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._playersDescription = new TextObject("{=RfXJdNye}Players", null).ToString();
			this._averagePlaytimeDescription = new TextObject("{=YAaAlbkX}Avg. Playtime", null).ToString();
			this._roundsDescription = new TextObject("{=iKtIhlbo}Rounds", null).ToString();
			this._roundTimeDescription = new TextObject("{=r5WzivPb}Round Time", null).ToString();
			this._objectivesDescription = new TextObject("{=gqNxq11A}Objectives", null).ToString();
			this._troopsDescription = new TextObject("{=5k4dxUEJ}Troops", null).ToString();
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00014B88 File Offset: 0x00012D88
		public void UpdateForGameType(string gameTypeStr)
		{
			this.Name = GameTexts.FindText("str_multiplayer_official_game_type_name", gameTypeStr).ToString();
			MBTextManager.SetTextVariable("newline", "\n", false);
			this.Description = GameTexts.FindText("str_multiplayer_official_game_type_description", gameTypeStr).ToString();
			this.ExtraInfos.Clear();
			int num = MultiplayerOptions.Instance.GetNumberOfPlayersForGameMode(gameTypeStr) / 2;
			int roundCountForGameMode = MultiplayerOptions.Instance.GetRoundCountForGameMode(gameTypeStr);
			int roundTimeLimitInMinutesForGameMode = MultiplayerOptions.Instance.GetRoundTimeLimitInMinutesForGameMode(gameTypeStr);
			int num2 = ((roundCountForGameMode == 1) ? 1 : (roundCountForGameMode / 2 + 1));
			int num3 = num2 * roundTimeLimitInMinutesForGameMode;
			MBTextManager.SetTextVariable("PLAYER_COUNT", num.ToString(), false);
			string text = GameTexts.FindText("str_multiplayer_official_game_type_player_info_for_versus", null).ToString();
			MBTextManager.SetTextVariable("PLAY_TIME", num3.ToString(), false);
			string text2 = GameTexts.FindText("str_multiplayer_official_game_type_playtime_info_in_minutes", null).ToString();
			MBTextManager.SetTextVariable("ROUND_COUNT", num2.ToString(), false);
			string text3 = GameTexts.FindText("str_multiplayer_official_game_type_rounds_info_for_best_of", null).ToString();
			MBTextManager.SetTextVariable("PLAY_TIME", roundTimeLimitInMinutesForGameMode.ToString(), false);
			string text4 = GameTexts.FindText("str_multiplayer_official_game_type_playtime_info_in_minutes", null).ToString();
			string text5 = GameTexts.FindText("str_multiplayer_official_game_type_objective_info", gameTypeStr).ToString();
			string text6 = GameTexts.FindText("str_multiplayer_official_game_type_troops_info", gameTypeStr).ToString();
			this.ExtraInfos.Add(new StringPairItemVM(this._playersDescription, text, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._averagePlaytimeDescription, text2, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._roundsDescription, text3, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._roundTimeDescription, text4, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._objectivesDescription, text5, null));
			this.ExtraInfos.Add(new StringPairItemVM(this._troopsDescription, text6, null));
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00014D5E File Offset: 0x00012F5E
		public void SetEnabled(bool isEnabled)
		{
			this.IsEnabled = isEnabled;
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00014D67 File Offset: 0x00012F67
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x00014D6F File Offset: 0x00012F6F
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00014D92 File Offset: 0x00012F92
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x00014D9A File Offset: 0x00012F9A
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00014DB8 File Offset: 0x00012FB8
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x00014DC0 File Offset: 0x00012FC0
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00014DE3 File Offset: 0x00012FE3
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00014DEB File Offset: 0x00012FEB
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> ExtraInfos
		{
			get
			{
				return this._extraInfos;
			}
			set
			{
				if (value != this._extraInfos)
				{
					this._extraInfos = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "ExtraInfos");
				}
			}
		}

		// Token: 0x040002FE RID: 766
		private string _playersDescription;

		// Token: 0x040002FF RID: 767
		private string _averagePlaytimeDescription;

		// Token: 0x04000300 RID: 768
		private string _roundsDescription;

		// Token: 0x04000301 RID: 769
		private string _roundTimeDescription;

		// Token: 0x04000302 RID: 770
		private string _objectivesDescription;

		// Token: 0x04000303 RID: 771
		private string _troopsDescription;

		// Token: 0x04000304 RID: 772
		private string _name;

		// Token: 0x04000305 RID: 773
		private string _description;

		// Token: 0x04000306 RID: 774
		private bool _isEnabled;

		// Token: 0x04000307 RID: 775
		private MBBindingList<StringPairItemVM> _extraInfos;
	}
}
