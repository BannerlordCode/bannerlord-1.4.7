using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002B RID: 43
	public class MPLobbyGameTypeVM : ViewModel
	{
		// Token: 0x06000334 RID: 820 RVA: 0x0000C36E File Offset: 0x0000A56E
		public MPLobbyGameTypeVM(string gameType, bool isCasual, Action<string> onSelection)
		{
			this.GameTypeID = gameType;
			this.IsCasual = isCasual;
			this._onSelection = onSelection;
			this.RefreshValues();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000C391 File Offset: 0x0000A591
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Hint = new HintViewModel(GameTexts.FindText("str_multiplayer_game_stats_description", this.GameTypeID), null);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000C3B5 File Offset: 0x0000A5B5
		private void OnSelected()
		{
			Action<string> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this.GameTypeID);
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000C3CD File Offset: 0x0000A5CD
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000C3D5 File Offset: 0x0000A5D5
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
					if (value)
					{
						this.OnSelected();
					}
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000C3FC File Offset: 0x0000A5FC
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0000C404 File Offset: 0x0000A604
		[DataSourceProperty]
		public string GameTypeID
		{
			get
			{
				return this._gameTypeID;
			}
			set
			{
				if (value != this._gameTypeID)
				{
					this._gameTypeID = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypeID");
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000C427 File Offset: 0x0000A627
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000C42F File Offset: 0x0000A62F
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040001A7 RID: 423
		private readonly Action<string> _onSelection;

		// Token: 0x040001A8 RID: 424
		public readonly bool IsCasual;

		// Token: 0x040001A9 RID: 425
		private bool _isSelected;

		// Token: 0x040001AA RID: 426
		private string _gameTypeID;

		// Token: 0x040001AB RID: 427
		private HintViewModel _hint;
	}
}
