using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.EndOfRound
{
	// Token: 0x020000A0 RID: 160
	public class MultiplayerEndOfRoundVM : ViewModel
	{
		// Token: 0x06000F4D RID: 3917 RVA: 0x0002F1C0 File Offset: 0x0002D3C0
		public MultiplayerEndOfRoundVM(MissionScoreboardComponent scoreboardComponent, MissionLobbyComponent missionLobbyComponent, IRoundComponent multiplayerRoundComponent)
		{
			this._scoreboardComponent = scoreboardComponent;
			this._multiplayerRoundComponent = multiplayerRoundComponent;
			this._missionLobbyComponent = missionLobbyComponent;
			this._victoryText = new TextObject("{=RCuCoVgd}ROUND WON", null).ToString();
			this._defeatText = new TextObject("{=Dbkx4v90}ROUND LOST", null).ToString();
			this._roundEndReasonAllyTeamSideDepletedTextObject = new TextObject("{=9M4G8DDd}Your team was wiped out", null);
			this._roundEndReasonEnemyTeamSideDepletedTextObject = new TextObject("{=jPXglGWT}Enemy team was wiped out", null);
			this._roundEndReasonAllyTeamRoundTimeEndedTextObject = new TextObject("{=x1HZy70i}Your team had the upper hand at timeout", null);
			this._roundEndReasonEnemyTeamRoundTimeEndedTextObject = new TextObject("{=Dc3fFblo}Enemy team had the upper hand at timeout", null);
			this._roundEndReasonRoundTimeEndedWithDrawTextObject = new TextObject("{=i3dJSlD0}No team had the upper hand at timeout", null);
			if (this._missionLobbyComponent.MissionType == MultiplayerGameType.Battle || this._missionLobbyComponent.MissionType == MultiplayerGameType.Captain || this._missionLobbyComponent.MissionType == MultiplayerGameType.Skirmish)
			{
				this._roundEndReasonAllyTeamGameModeSpecificEndedTextObject = new TextObject("{=xxuzZJ3G}Your team ran out of morale", null);
				this._roundEndReasonEnemyTeamGameModeSpecificEndedTextObject = new TextObject("{=c6c9eYrD}Enemy team ran out of morale", null);
			}
			else
			{
				this._roundEndReasonAllyTeamGameModeSpecificEndedTextObject = TextObject.GetEmpty();
				this._roundEndReasonEnemyTeamGameModeSpecificEndedTextObject = TextObject.GetEmpty();
			}
			this.AttackerSide = new MultiplayerEndOfRoundSideVM();
			this.DefenderSide = new MultiplayerEndOfRoundSideVM();
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0002F2E3 File Offset: 0x0002D4E3
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._multiplayerRoundComponent != null)
			{
				this.Refresh();
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0002F2FC File Offset: 0x0002D4FC
		public void Refresh()
		{
			BattleSideEnum allyBattleSideEnum = BattleSideEnum.None;
			BattleSideEnum battleSideEnum = BattleSideEnum.None;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null && missionPeer.Team != null)
			{
				allyBattleSideEnum = missionPeer.Team.Side;
				battleSideEnum = ((allyBattleSideEnum == BattleSideEnum.Attacker) ? BattleSideEnum.Defender : BattleSideEnum.Attacker);
			}
			bool flag = allyBattleSideEnum == BattleSideEnum.Attacker;
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide = this._scoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == BattleSideEnum.Attacker);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide2 = this._scoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == BattleSideEnum.Defender);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide3 = (flag ? missionScoreboardSide : missionScoreboardSide2);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide4 = (flag ? missionScoreboardSide2 : missionScoreboardSide);
			BasicCultureObject culture = missionScoreboardSide3.GetCulture();
			BasicCultureObject culture2 = missionScoreboardSide4.GetCulture();
			bool flag2 = this._multiplayerRoundComponent.RoundWinner == allyBattleSideEnum;
			bool flag3 = this._multiplayerRoundComponent.RoundWinner == battleSideEnum;
			this.AttackerMVPTitleText = this.GetMVPTitleText(culture);
			this.DefenderMVPTitleText = this.GetMVPTitleText(culture2);
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(culture, culture2);
			this.AttackerSide.SetData(culture, missionScoreboardSide3.SideScore, flag2, multiplayerBattleColors.AttackerColors);
			this.DefenderSide.SetData(culture2, missionScoreboardSide4.SideScore, flag3, multiplayerBattleColors.DefenderColors);
			if (this._scoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == allyBattleSideEnum) != null && this._multiplayerRoundComponent != null)
			{
				bool flag4 = false;
				if (this._multiplayerRoundComponent.RoundWinner == allyBattleSideEnum)
				{
					this.IsRoundWinner = true;
					this.Title = this._victoryText;
				}
				else if (this._multiplayerRoundComponent.RoundWinner == battleSideEnum)
				{
					this.IsRoundWinner = false;
					this.Title = this._defeatText;
				}
				else
				{
					flag4 = true;
				}
				RoundEndReason roundEndReason = this._multiplayerRoundComponent.RoundEndReason;
				if (roundEndReason == RoundEndReason.SideDepleted)
				{
					this.Description = (this.IsRoundWinner ? this._roundEndReasonEnemyTeamSideDepletedTextObject.ToString() : this._roundEndReasonAllyTeamSideDepletedTextObject.ToString());
					return;
				}
				if (roundEndReason == RoundEndReason.GameModeSpecificEnded)
				{
					this.Description = (this.IsRoundWinner ? this._roundEndReasonEnemyTeamGameModeSpecificEndedTextObject.ToString() : this._roundEndReasonAllyTeamGameModeSpecificEndedTextObject.ToString());
					return;
				}
				if (roundEndReason == RoundEndReason.RoundTimeEnded)
				{
					this.Description = (this.IsRoundWinner ? this._roundEndReasonAllyTeamRoundTimeEndedTextObject.ToString() : (flag4 ? this._roundEndReasonRoundTimeEndedWithDrawTextObject.ToString() : this._roundEndReasonEnemyTeamRoundTimeEndedTextObject.ToString()));
				}
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0002F588 File Offset: 0x0002D788
		public void OnMVPSelected(MissionPeer mvpPeer)
		{
			BasicCharacterObject @object = MBObjectManager.Instance.GetObject<BasicCharacterObject>("mp_character");
			@object.UpdatePlayerCharacterBodyProperties(mvpPeer.Peer.BodyProperties, mvpPeer.Peer.Race, mvpPeer.Peer.IsFemale);
			@object.Age = mvpPeer.Peer.BodyProperties.Age;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			Team team = mvpPeer.Team;
			BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
			Team team2 = missionPeer.Team;
			BattleSideEnum? battleSideEnum2 = ((team2 != null) ? new BattleSideEnum?(team2.Side) : null);
			if ((battleSideEnum.GetValueOrDefault() == battleSideEnum2.GetValueOrDefault()) & (battleSideEnum != null == (battleSideEnum2 != null)))
			{
				this.AttackerMVP = new MPPlayerVM(mvpPeer);
				this.AttackerMVP.RefreshDivision(false);
				this.AttackerMVP.RefreshPreview(@object, mvpPeer.Peer.BodyProperties.DynamicProperties, mvpPeer.Peer.IsFemale);
				this.HasAttackerMVP = true;
				return;
			}
			this.DefenderMVP = new MPPlayerVM(mvpPeer);
			this.DefenderMVP.RefreshDivision(false);
			this.DefenderMVP.RefreshPreview(@object, mvpPeer.Peer.BodyProperties.DynamicProperties, mvpPeer.Peer.IsFemale);
			this.HasDefenderMVP = true;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0002F6F0 File Offset: 0x0002D8F0
		private string GetMVPTitleText(BasicCultureObject culture)
		{
			if (culture.StringId == "vlandia")
			{
				return new TextObject("{=3VosbFR0}Vlandian Champion", null).ToString();
			}
			if (culture.StringId == "sturgia")
			{
				return new TextObject("{=AGUXiN8u}Voivode", null).ToString();
			}
			if (culture.StringId == "khuzait")
			{
				return new TextObject("{=F2h2cT4q}Khan's Chosen", null).ToString();
			}
			if (culture.StringId == "battania")
			{
				return new TextObject("{=eWPN3HmE}Hero of Battania", null).ToString();
			}
			if (culture.StringId == "aserai")
			{
				return new TextObject("{=5zNfxZ7B}War Prince", null).ToString();
			}
			if (culture.StringId == "empire")
			{
				return new TextObject("{=wwbIcqsq}Conqueror", null).ToString();
			}
			Debug.FailedAssert("Invalid Culture ID for MVP Title", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\EndOfRound\\MultiplayerEndOfRoundVM.cs", "GetMVPTitleText", 205);
			return string.Empty;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0002F7ED File Offset: 0x0002D9ED
		private void OnIsShownChanged()
		{
			if (!this.IsShown)
			{
				this.HasAttackerMVP = false;
				this.HasDefenderMVP = false;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0002F805 File Offset: 0x0002DA05
		// (set) Token: 0x06000F54 RID: 3924 RVA: 0x0002F80D File Offset: 0x0002DA0D
		[DataSourceProperty]
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChangedWithValue(value, "IsShown");
					this.OnIsShownChanged();
				}
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0002F831 File Offset: 0x0002DA31
		// (set) Token: 0x06000F56 RID: 3926 RVA: 0x0002F839 File Offset: 0x0002DA39
		[DataSourceProperty]
		public bool HasAttackerMVP
		{
			get
			{
				return this._hasAttackerMVP;
			}
			set
			{
				if (value != this._hasAttackerMVP)
				{
					this._hasAttackerMVP = value;
					base.OnPropertyChangedWithValue(value, "HasAttackerMVP");
				}
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0002F857 File Offset: 0x0002DA57
		// (set) Token: 0x06000F58 RID: 3928 RVA: 0x0002F85F File Offset: 0x0002DA5F
		[DataSourceProperty]
		public bool HasDefenderMVP
		{
			get
			{
				return this._hasDefenderMVP;
			}
			set
			{
				if (value != this._hasDefenderMVP)
				{
					this._hasDefenderMVP = value;
					base.OnPropertyChangedWithValue(value, "HasDefenderMVP");
				}
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0002F87D File Offset: 0x0002DA7D
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0002F885 File Offset: 0x0002DA85
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0002F8A8 File Offset: 0x0002DAA8
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x0002F8B0 File Offset: 0x0002DAB0
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

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0002F8D3 File Offset: 0x0002DAD3
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0002F8DB File Offset: 0x0002DADB
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0002F8FE File Offset: 0x0002DAFE
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0002F906 File Offset: 0x0002DB06
		[DataSourceProperty]
		public bool IsRoundWinner
		{
			get
			{
				return this._isRoundWinner;
			}
			set
			{
				if (value != this._isRoundWinner)
				{
					this._isRoundWinner = value;
					base.OnPropertyChangedWithValue(value, "IsRoundWinner");
				}
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0002F924 File Offset: 0x0002DB24
		// (set) Token: 0x06000F62 RID: 3938 RVA: 0x0002F92C File Offset: 0x0002DB2C
		[DataSourceProperty]
		public MultiplayerEndOfRoundSideVM AttackerSide
		{
			get
			{
				return this._attackerSide;
			}
			set
			{
				if (value != this._attackerSide)
				{
					this._attackerSide = value;
					base.OnPropertyChangedWithValue<MultiplayerEndOfRoundSideVM>(value, "AttackerSide");
				}
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0002F94A File Offset: 0x0002DB4A
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0002F952 File Offset: 0x0002DB52
		[DataSourceProperty]
		public MultiplayerEndOfRoundSideVM DefenderSide
		{
			get
			{
				return this._defenderSide;
			}
			set
			{
				if (value != this._defenderSide)
				{
					this._defenderSide = value;
					base.OnPropertyChangedWithValue<MultiplayerEndOfRoundSideVM>(value, "DefenderSide");
				}
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0002F970 File Offset: 0x0002DB70
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x0002F978 File Offset: 0x0002DB78
		[DataSourceProperty]
		public MPPlayerVM AttackerMVP
		{
			get
			{
				return this._attackerMVP;
			}
			set
			{
				if (value != this._attackerMVP)
				{
					this._attackerMVP = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "AttackerMVP");
				}
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0002F996 File Offset: 0x0002DB96
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x0002F99E File Offset: 0x0002DB9E
		[DataSourceProperty]
		public MPPlayerVM DefenderMVP
		{
			get
			{
				return this._defenderMVP;
			}
			set
			{
				if (value != this._defenderMVP)
				{
					this._defenderMVP = value;
					base.OnPropertyChangedWithValue<MPPlayerVM>(value, "DefenderMVP");
				}
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0002F9BC File Offset: 0x0002DBBC
		// (set) Token: 0x06000F6A RID: 3946 RVA: 0x0002F9C4 File Offset: 0x0002DBC4
		[DataSourceProperty]
		public string AttackerMVPTitleText
		{
			get
			{
				return this._attackerMVPTitleText;
			}
			set
			{
				if (value != this._attackerMVPTitleText)
				{
					this._attackerMVPTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttackerMVPTitleText");
				}
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0002F9E7 File Offset: 0x0002DBE7
		// (set) Token: 0x06000F6C RID: 3948 RVA: 0x0002F9EF File Offset: 0x0002DBEF
		[DataSourceProperty]
		public string DefenderMVPTitleText
		{
			get
			{
				return this._defenderMVPTitleText;
			}
			set
			{
				if (value != this._defenderMVPTitleText)
				{
					this._defenderMVPTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefenderMVPTitleText");
				}
			}
		}

		// Token: 0x0400070F RID: 1807
		private readonly MissionScoreboardComponent _scoreboardComponent;

		// Token: 0x04000710 RID: 1808
		private readonly MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000711 RID: 1809
		private readonly IRoundComponent _multiplayerRoundComponent;

		// Token: 0x04000712 RID: 1810
		private readonly string _victoryText;

		// Token: 0x04000713 RID: 1811
		private readonly string _defeatText;

		// Token: 0x04000714 RID: 1812
		private readonly TextObject _roundEndReasonAllyTeamSideDepletedTextObject;

		// Token: 0x04000715 RID: 1813
		private readonly TextObject _roundEndReasonEnemyTeamSideDepletedTextObject;

		// Token: 0x04000716 RID: 1814
		private readonly TextObject _roundEndReasonAllyTeamRoundTimeEndedTextObject;

		// Token: 0x04000717 RID: 1815
		private readonly TextObject _roundEndReasonEnemyTeamRoundTimeEndedTextObject;

		// Token: 0x04000718 RID: 1816
		private readonly TextObject _roundEndReasonAllyTeamGameModeSpecificEndedTextObject;

		// Token: 0x04000719 RID: 1817
		private readonly TextObject _roundEndReasonEnemyTeamGameModeSpecificEndedTextObject;

		// Token: 0x0400071A RID: 1818
		private readonly TextObject _roundEndReasonRoundTimeEndedWithDrawTextObject;

		// Token: 0x0400071B RID: 1819
		private bool _isShown;

		// Token: 0x0400071C RID: 1820
		private bool _hasAttackerMVP;

		// Token: 0x0400071D RID: 1821
		private bool _hasDefenderMVP;

		// Token: 0x0400071E RID: 1822
		private string _title;

		// Token: 0x0400071F RID: 1823
		private string _description;

		// Token: 0x04000720 RID: 1824
		private string _cultureId;

		// Token: 0x04000721 RID: 1825
		private bool _isRoundWinner;

		// Token: 0x04000722 RID: 1826
		private MultiplayerEndOfRoundSideVM _attackerSide;

		// Token: 0x04000723 RID: 1827
		private MultiplayerEndOfRoundSideVM _defenderSide;

		// Token: 0x04000724 RID: 1828
		private MPPlayerVM _attackerMVP;

		// Token: 0x04000725 RID: 1829
		private MPPlayerVM _defenderMVP;

		// Token: 0x04000726 RID: 1830
		private string _attackerMVPTitleText;

		// Token: 0x04000727 RID: 1831
		private string _defenderMVPTitleText;
	}
}
