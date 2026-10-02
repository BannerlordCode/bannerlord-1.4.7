using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x0200001B RID: 27
	public class GameTypeSelectionGroupVM : ViewModel
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000091DE File Offset: 0x000073DE
		// (set) Token: 0x06000135 RID: 309 RVA: 0x000091E6 File Offset: 0x000073E6
		public string SelectedGameTypeString { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000136 RID: 310 RVA: 0x000091EF File Offset: 0x000073EF
		// (set) Token: 0x06000137 RID: 311 RVA: 0x000091F7 File Offset: 0x000073F7
		public CustomBattlePlayerType SelectedPlayerType { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00009200 File Offset: 0x00007400
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00009208 File Offset: 0x00007408
		public CustomBattlePlayerSide SelectedPlayerSide { get; private set; }

		// Token: 0x0600013A RID: 314 RVA: 0x00009214 File Offset: 0x00007414
		public GameTypeSelectionGroupVM(Action<CustomBattlePlayerType> onPlayerTypeChange, Action<string> onGameTypeChange)
		{
			this._onPlayerTypeChange = onPlayerTypeChange;
			this._onGameTypeChange = onGameTypeChange;
			this.GameTypeSelection = new SelectorVM<GameTypeItemVM>(0, new Action<SelectorVM<GameTypeItemVM>>(this.OnGameTypeSelection));
			this.PlayerTypeSelection = new SelectorVM<PlayerTypeItemVM>(0, new Action<SelectorVM<PlayerTypeItemVM>>(this.OnPlayerTypeSelection));
			this.PlayerSideSelection = new SelectorVM<PlayerSideItemVM>(0, new Action<SelectorVM<PlayerSideItemVM>>(this.OnPlayerSideSelection));
			this.RefreshValues();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009284 File Offset: 0x00007484
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.GameTypeText = new TextObject("{=JPimShCw}Game Type", null).ToString();
			this.PlayerTypeText = new TextObject("{=bKg8Mmwb}Player Type", null).ToString();
			this.PlayerSideText = new TextObject("{=P3rMg4uZ}Player Side", null).ToString();
			this.GameTypeSelection.ItemList.Clear();
			this.PlayerTypeSelection.ItemList.Clear();
			this.PlayerSideSelection.ItemList.Clear();
			foreach (Tuple<string, string> tuple in CustomBattleData.GameTypes)
			{
				this.GameTypeSelection.AddItem(new GameTypeItemVM(tuple.Item1, tuple.Item2));
			}
			foreach (Tuple<string, CustomBattlePlayerType> tuple2 in CustomBattleData.PlayerTypes)
			{
				this.PlayerTypeSelection.AddItem(new PlayerTypeItemVM(tuple2.Item1, tuple2.Item2));
			}
			foreach (Tuple<string, CustomBattlePlayerSide> tuple3 in CustomBattleData.PlayerSides)
			{
				this.PlayerSideSelection.AddItem(new PlayerSideItemVM(tuple3.Item1, tuple3.Item2));
			}
			this.GameTypeSelection.SelectedIndex = 0;
			this.PlayerTypeSelection.SelectedIndex = 0;
			this.PlayerSideSelection.SelectedIndex = 0;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000942C File Offset: 0x0000762C
		public void RandomizeAll()
		{
			this.GameTypeSelection.ExecuteRandomize();
			this.PlayerTypeSelection.ExecuteRandomize();
			this.PlayerSideSelection.ExecuteRandomize();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000944F File Offset: 0x0000764F
		private void OnGameTypeSelection(SelectorVM<GameTypeItemVM> selector)
		{
			this.SelectedGameTypeString = selector.SelectedItem.GameTypeStringId;
			this._onGameTypeChange(this.SelectedGameTypeString);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009473 File Offset: 0x00007673
		private void OnPlayerTypeSelection(SelectorVM<PlayerTypeItemVM> selector)
		{
			this.SelectedPlayerType = selector.SelectedItem.PlayerType;
			this._onPlayerTypeChange(this.SelectedPlayerType);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00009497 File Offset: 0x00007697
		private void OnPlayerSideSelection(SelectorVM<PlayerSideItemVM> selector)
		{
			this.SelectedPlayerSide = selector.SelectedItem.PlayerSide;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000094AA File Offset: 0x000076AA
		// (set) Token: 0x06000141 RID: 321 RVA: 0x000094B2 File Offset: 0x000076B2
		[DataSourceProperty]
		public SelectorVM<GameTypeItemVM> GameTypeSelection
		{
			get
			{
				return this._gameTypeSelection;
			}
			set
			{
				if (value != this._gameTypeSelection)
				{
					this._gameTypeSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<GameTypeItemVM>>(value, "GameTypeSelection");
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000094D0 File Offset: 0x000076D0
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000094D8 File Offset: 0x000076D8
		[DataSourceProperty]
		public SelectorVM<PlayerTypeItemVM> PlayerTypeSelection
		{
			get
			{
				return this._playerTypeSelection;
			}
			set
			{
				if (value != this._playerTypeSelection)
				{
					this._playerTypeSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<PlayerTypeItemVM>>(value, "PlayerTypeSelection");
				}
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000094F6 File Offset: 0x000076F6
		// (set) Token: 0x06000145 RID: 325 RVA: 0x000094FE File Offset: 0x000076FE
		[DataSourceProperty]
		public SelectorVM<PlayerSideItemVM> PlayerSideSelection
		{
			get
			{
				return this._playerSideSelection;
			}
			set
			{
				if (value != this._playerSideSelection)
				{
					this._playerSideSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<PlayerSideItemVM>>(value, "PlayerSideSelection");
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000951C File Offset: 0x0000771C
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00009524 File Offset: 0x00007724
		[DataSourceProperty]
		public string GameTypeText
		{
			get
			{
				return this._gameTypeText;
			}
			set
			{
				if (value != this._gameTypeText)
				{
					this._gameTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypeText");
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00009547 File Offset: 0x00007747
		// (set) Token: 0x06000149 RID: 329 RVA: 0x0000954F File Offset: 0x0000774F
		[DataSourceProperty]
		public string PlayerTypeText
		{
			get
			{
				return this._playerTypeText;
			}
			set
			{
				if (value != this._playerTypeText)
				{
					this._playerTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerTypeText");
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00009572 File Offset: 0x00007772
		// (set) Token: 0x0600014B RID: 331 RVA: 0x0000957A File Offset: 0x0000777A
		[DataSourceProperty]
		public string PlayerSideText
		{
			get
			{
				return this._playerSideText;
			}
			set
			{
				if (value != this._playerSideText)
				{
					this._playerSideText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerSideText");
				}
			}
		}

		// Token: 0x040000D6 RID: 214
		private readonly Action<CustomBattlePlayerType> _onPlayerTypeChange;

		// Token: 0x040000D7 RID: 215
		private readonly Action<string> _onGameTypeChange;

		// Token: 0x040000D8 RID: 216
		private SelectorVM<GameTypeItemVM> _gameTypeSelection;

		// Token: 0x040000D9 RID: 217
		private SelectorVM<PlayerTypeItemVM> _playerTypeSelection;

		// Token: 0x040000DA RID: 218
		private SelectorVM<PlayerSideItemVM> _playerSideSelection;

		// Token: 0x040000DB RID: 219
		private string _gameTypeText;

		// Token: 0x040000DC RID: 220
		private string _playerTypeText;

		// Token: 0x040000DD RID: 221
		private string _playerSideText;
	}
}
