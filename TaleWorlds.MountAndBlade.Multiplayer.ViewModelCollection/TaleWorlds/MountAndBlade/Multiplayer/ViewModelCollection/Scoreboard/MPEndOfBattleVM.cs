using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x02000023 RID: 35
	public class MPEndOfBattleVM : ViewModel
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00009D24 File Offset: 0x00007F24
		private MissionRepresentativeBase missionRep
		{
			get
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				if (myPeer == null)
				{
					return null;
				}
				VirtualPlayer virtualPlayer = myPeer.VirtualPlayer;
				if (virtualPlayer == null)
				{
					return null;
				}
				return virtualPlayer.GetComponent<MissionRepresentativeBase>();
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00009D44 File Offset: 0x00007F44
		public MPEndOfBattleVM(Mission mission, MissionScoreboardComponent missionScoreboardComponent, bool isSingleTeam)
		{
			this._missionScoreboardComponent = missionScoreboardComponent;
			this._gameMode = mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._lobbyComponent = mission.GetMissionBehavior<MissionLobbyComponent>();
			this._lobbyComponent.OnPostMatchEnded += this.OnPostMatchEnded;
			this._isSingleTeam = isSingleTeam;
			this.RefreshValues();
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009D9C File Offset: 0x00007F9C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CountdownTitle = new TextObject("{=wGjQgQlY}Next Game begins in:", null).ToString();
			this.Header = new TextObject("{=HXxNfncd}End of Battle", null).ToString();
			MPEndOfBattleSideVM allySide = this.AllySide;
			if (allySide != null)
			{
				allySide.RefreshValues();
			}
			MPEndOfBattleSideVM enemySide = this.EnemySide;
			if (enemySide == null)
			{
				return;
			}
			enemySide.RefreshValues();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009DFC File Offset: 0x00007FFC
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._lobbyComponent.OnPostMatchEnded -= this.OnPostMatchEnded;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00009E1B File Offset: 0x0000801B
		public void Tick(float dt)
		{
			this.Countdown = MathF.Ceiling(this._gameMode.RemainingTime);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009E33 File Offset: 0x00008033
		private void OnPostMatchEnded()
		{
			this.OnFinalRoundEnded();
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00009E3C File Offset: 0x0000803C
		private void OnFinalRoundEnded()
		{
			if (this._isSingleTeam)
			{
				return;
			}
			this.IsAvailable = true;
			this.InitSides();
			MissionScoreboardComponent missionScoreboardComponent = this._missionScoreboardComponent;
			BattleSideEnum battleSideEnum = ((missionScoreboardComponent != null) ? missionScoreboardComponent.GetMatchWinnerSide() : BattleSideEnum.None);
			if (battleSideEnum == this._enemyBattleSide)
			{
				this.BattleResult = 0;
				this.ResultText = GameTexts.FindText("str_defeat", null).ToString();
				return;
			}
			if (battleSideEnum == this._allyBattleSide)
			{
				this.BattleResult = 1;
				this.ResultText = GameTexts.FindText("str_victory", null).ToString();
				return;
			}
			this.BattleResult = 2;
			this.ResultText = GameTexts.FindText("str_draw", null).ToString();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00009EE0 File Offset: 0x000080E0
		private void InitSides()
		{
			this._allyBattleSide = BattleSideEnum.Attacker;
			this._enemyBattleSide = BattleSideEnum.Defender;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				Team team = missionPeer.Team;
				if (team != null && team.Side == BattleSideEnum.Defender)
				{
					this._allyBattleSide = BattleSideEnum.Defender;
					this._enemyBattleSide = BattleSideEnum.Attacker;
				}
			}
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide = this._missionScoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == this._allyBattleSide);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide2 = this._missionScoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == this._enemyBattleSide);
			string text = ((missionScoreboardSide != null && missionScoreboardSide.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			string text2 = ((missionScoreboardSide2 != null && missionScoreboardSide2.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject basicCultureObject = (string.IsNullOrEmpty(text) ? null : MBObjectManager.Instance.GetObject<BasicCultureObject>(text));
			BasicCultureObject basicCultureObject2 = (string.IsNullOrEmpty(text2) ? null : MBObjectManager.Instance.GetObject<BasicCultureObject>(text2));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(basicCultureObject, basicCultureObject2);
			if (missionScoreboardSide != null)
			{
				this.AllySide = new MPEndOfBattleSideVM(this._missionScoreboardComponent, missionScoreboardSide, multiplayerBattleColors.AttackerColors);
			}
			if (missionScoreboardSide2 != null)
			{
				this.EnemySide = new MPEndOfBattleSideVM(this._missionScoreboardComponent, missionScoreboardSide2, multiplayerBattleColors.DefenderColors);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000A01D File Offset: 0x0000821D
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000A025 File Offset: 0x00008225
		[DataSourceProperty]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAvailable");
				}
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000A043 File Offset: 0x00008243
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000A04B File Offset: 0x0000824B
		[DataSourceProperty]
		public string CountdownTitle
		{
			get
			{
				return this._countdownTitle;
			}
			set
			{
				if (value != this._countdownTitle)
				{
					this._countdownTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "CountdownTitle");
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000A06E File Offset: 0x0000826E
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000A076 File Offset: 0x00008276
		[DataSourceProperty]
		public int Countdown
		{
			get
			{
				return this._countdown;
			}
			set
			{
				if (value != this._countdown)
				{
					this._countdown = value;
					base.OnPropertyChangedWithValue(value, "Countdown");
				}
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000A094 File Offset: 0x00008294
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000A09C File Offset: 0x0000829C
		[DataSourceProperty]
		public string Header
		{
			get
			{
				return this._header;
			}
			set
			{
				if (value != this._header)
				{
					this._header = value;
					base.OnPropertyChangedWithValue<string>(value, "Header");
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000A0BF File Offset: 0x000082BF
		// (set) Token: 0x06000280 RID: 640 RVA: 0x0000A0C7 File Offset: 0x000082C7
		[DataSourceProperty]
		public int BattleResult
		{
			get
			{
				return this._battleResult;
			}
			set
			{
				if (value != this._battleResult)
				{
					this._battleResult = value;
					base.OnPropertyChangedWithValue(value, "BattleResult");
				}
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000A0E5 File Offset: 0x000082E5
		// (set) Token: 0x06000282 RID: 642 RVA: 0x0000A0ED File Offset: 0x000082ED
		[DataSourceProperty]
		public string ResultText
		{
			get
			{
				return this._resultText;
			}
			set
			{
				if (value != this._resultText)
				{
					this._resultText = value;
					base.OnPropertyChangedWithValue<string>(value, "ResultText");
				}
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000A110 File Offset: 0x00008310
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000A118 File Offset: 0x00008318
		[DataSourceProperty]
		public MPEndOfBattleSideVM AllySide
		{
			get
			{
				return this._allySide;
			}
			set
			{
				if (value != this._allySide)
				{
					this._allySide = value;
					base.OnPropertyChangedWithValue<MPEndOfBattleSideVM>(value, "AllySide");
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000A136 File Offset: 0x00008336
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000A13E File Offset: 0x0000833E
		[DataSourceProperty]
		public MPEndOfBattleSideVM EnemySide
		{
			get
			{
				return this._enemySide;
			}
			set
			{
				if (value != this._enemySide)
				{
					this._enemySide = value;
					base.OnPropertyChangedWithValue<MPEndOfBattleSideVM>(value, "EnemySide");
				}
			}
		}

		// Token: 0x04000149 RID: 329
		private MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x0400014A RID: 330
		private MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x0400014B RID: 331
		private MissionLobbyComponent _lobbyComponent;

		// Token: 0x0400014C RID: 332
		private bool _isSingleTeam;

		// Token: 0x0400014D RID: 333
		private BattleSideEnum _allyBattleSide;

		// Token: 0x0400014E RID: 334
		private BattleSideEnum _enemyBattleSide;

		// Token: 0x0400014F RID: 335
		private bool _isAvailable;

		// Token: 0x04000150 RID: 336
		private string _countdownTitle;

		// Token: 0x04000151 RID: 337
		private int _countdown;

		// Token: 0x04000152 RID: 338
		private string _header;

		// Token: 0x04000153 RID: 339
		private int _battleResult;

		// Token: 0x04000154 RID: 340
		private string _resultText;

		// Token: 0x04000155 RID: 341
		private MPEndOfBattleSideVM _allySide;

		// Token: 0x04000156 RID: 342
		private MPEndOfBattleSideVM _enemySide;
	}
}
