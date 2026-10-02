using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.BoardGame
{
	// Token: 0x0200005F RID: 95
	public class BoardGameInstructionVM : ViewModel
	{
		// Token: 0x060005C8 RID: 1480 RVA: 0x000155F6 File Offset: 0x000137F6
		public BoardGameInstructionVM(CultureObject.BoardGameType game, int instructionIndex)
		{
			this._game = game;
			this._instructionIndex = instructionIndex;
			this.GameType = this._game.ToString();
			this.RefreshValues();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001562C File Offset: 0x0001382C
		public override void RefreshValues()
		{
			base.RefreshValues();
			GameTexts.SetVariable("newline", "\n");
			this.TitleText = GameTexts.FindText("str_board_game_title", this._game.ToString() + "_" + this._instructionIndex).ToString();
			this.DescriptionText = GameTexts.FindText("str_board_game_instruction", this._game.ToString() + "_" + this._instructionIndex).ToString();
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x000156C4 File Offset: 0x000138C4
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x000156CC File Offset: 0x000138CC
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x000156EA File Offset: 0x000138EA
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x000156F2 File Offset: 0x000138F2
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

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00015715 File Offset: 0x00013915
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x0001571D File Offset: 0x0001391D
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00015740 File Offset: 0x00013940
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x00015748 File Offset: 0x00013948
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

		// Token: 0x040002DC RID: 732
		private readonly CultureObject.BoardGameType _game;

		// Token: 0x040002DD RID: 733
		private readonly int _instructionIndex;

		// Token: 0x040002DE RID: 734
		private bool _isEnabled;

		// Token: 0x040002DF RID: 735
		private string _titleText;

		// Token: 0x040002E0 RID: 736
		private string _descriptionText;

		// Token: 0x040002E1 RID: 737
		private string _gameType;
	}
}
