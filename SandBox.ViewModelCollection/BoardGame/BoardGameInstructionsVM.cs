using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.BoardGame
{
	// Token: 0x0200005E RID: 94
	public class BoardGameInstructionsVM : ViewModel
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x00015234 File Offset: 0x00013434
		public BoardGameInstructionsVM(CultureObject.BoardGameType boardGameType)
		{
			this._boardGameType = boardGameType;
			this.InstructionList = new MBBindingList<BoardGameInstructionVM>();
			for (int i = 0; i < this.GetNumberOfInstructions(this._boardGameType); i++)
			{
				this.InstructionList.Add(new BoardGameInstructionVM(this._boardGameType, i));
			}
			this._currentInstructionIndex = 0;
			if (this.InstructionList.Count > 0)
			{
				this.InstructionList[0].IsEnabled = true;
			}
			this.RefreshValues();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000152B4 File Offset: 0x000134B4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.InstructionsText = GameTexts.FindText("str_how_to_play", null).ToString();
			this.PreviousText = GameTexts.FindText("str_previous", null).ToString();
			this.NextText = GameTexts.FindText("str_next", null).ToString();
			this.InstructionList.ApplyActionOnAllItems(delegate(BoardGameInstructionVM x)
			{
				x.RefreshValues();
			});
			if (this._currentInstructionIndex >= 0 && this._currentInstructionIndex < this.InstructionList.Count)
			{
				TextObject textObject = new TextObject("{=hUSmlhNh}{CURRENT_PAGE}/{TOTAL_PAGES}", null);
				textObject.SetTextVariable("CURRENT_PAGE", (this._currentInstructionIndex + 1).ToString());
				textObject.SetTextVariable("TOTAL_PAGES", this.InstructionList.Count.ToString());
				this.CurrentPageText = textObject.ToString();
				this.IsPreviousButtonEnabled = this._currentInstructionIndex != 0;
				this.IsNextButtonEnabled = this._currentInstructionIndex < this.InstructionList.Count - 1;
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x000153D0 File Offset: 0x000135D0
		public void ExecuteShowPrevious()
		{
			if (this._currentInstructionIndex > 0 && this._currentInstructionIndex < this.InstructionList.Count)
			{
				this.InstructionList[this._currentInstructionIndex].IsEnabled = false;
				this._currentInstructionIndex--;
				this.InstructionList[this._currentInstructionIndex].IsEnabled = true;
				this.RefreshValues();
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001543C File Offset: 0x0001363C
		public void ExecuteShowNext()
		{
			if (this._currentInstructionIndex >= 0 && this._currentInstructionIndex < this.InstructionList.Count - 1)
			{
				this.InstructionList[this._currentInstructionIndex].IsEnabled = false;
				this._currentInstructionIndex++;
				this.InstructionList[this._currentInstructionIndex].IsEnabled = true;
				this.RefreshValues();
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000154A9 File Offset: 0x000136A9
		private int GetNumberOfInstructions(CultureObject.BoardGameType game)
		{
			switch (game)
			{
			case CultureObject.BoardGameType.Seega:
				return 4;
			case CultureObject.BoardGameType.Puluc:
				return 5;
			case CultureObject.BoardGameType.Konane:
				return 3;
			case CultureObject.BoardGameType.MuTorere:
				return 2;
			case CultureObject.BoardGameType.Tablut:
				return 4;
			case CultureObject.BoardGameType.BaghChal:
				return 4;
			default:
				return 0;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000154D8 File Offset: 0x000136D8
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x000154E0 File Offset: 0x000136E0
		[DataSourceProperty]
		public bool IsPreviousButtonEnabled
		{
			get
			{
				return this._isPreviousButtonEnabled;
			}
			set
			{
				if (value != this._isPreviousButtonEnabled)
				{
					this._isPreviousButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPreviousButtonEnabled");
				}
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x000154FE File Offset: 0x000136FE
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x00015506 File Offset: 0x00013706
		[DataSourceProperty]
		public bool IsNextButtonEnabled
		{
			get
			{
				return this._isNextButtonEnabled;
			}
			set
			{
				if (value != this._isNextButtonEnabled)
				{
					this._isNextButtonEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNextButtonEnabled");
				}
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00015524 File Offset: 0x00013724
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0001552C File Offset: 0x0001372C
		[DataSourceProperty]
		public string InstructionsText
		{
			get
			{
				return this._instructionsText;
			}
			set
			{
				if (value != this._instructionsText)
				{
					this._instructionsText = value;
					base.OnPropertyChangedWithValue<string>(value, "InstructionsText");
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001554F File Offset: 0x0001374F
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x00015557 File Offset: 0x00013757
		[DataSourceProperty]
		public string PreviousText
		{
			get
			{
				return this._previousText;
			}
			set
			{
				if (value != this._previousText)
				{
					this._previousText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousText");
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001557A File Offset: 0x0001377A
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00015582 File Offset: 0x00013782
		[DataSourceProperty]
		public string NextText
		{
			get
			{
				return this._nextText;
			}
			set
			{
				if (value != this._nextText)
				{
					this._nextText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextText");
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x000155A5 File Offset: 0x000137A5
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x000155AD File Offset: 0x000137AD
		[DataSourceProperty]
		public string CurrentPageText
		{
			get
			{
				return this._currentPageText;
			}
			set
			{
				if (value != this._currentPageText)
				{
					this._currentPageText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentPageText");
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x000155D0 File Offset: 0x000137D0
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x000155D8 File Offset: 0x000137D8
		[DataSourceProperty]
		public MBBindingList<BoardGameInstructionVM> InstructionList
		{
			get
			{
				return this._instructionList;
			}
			set
			{
				if (value != this._instructionList)
				{
					this._instructionList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BoardGameInstructionVM>>(value, "InstructionList");
				}
			}
		}

		// Token: 0x040002D3 RID: 723
		private readonly CultureObject.BoardGameType _boardGameType;

		// Token: 0x040002D4 RID: 724
		private int _currentInstructionIndex;

		// Token: 0x040002D5 RID: 725
		private bool _isPreviousButtonEnabled;

		// Token: 0x040002D6 RID: 726
		private bool _isNextButtonEnabled;

		// Token: 0x040002D7 RID: 727
		private string _instructionsText;

		// Token: 0x040002D8 RID: 728
		private string _previousText;

		// Token: 0x040002D9 RID: 729
		private string _nextText;

		// Token: 0x040002DA RID: 730
		private string _currentPageText;

		// Token: 0x040002DB RID: 731
		private MBBindingList<BoardGameInstructionVM> _instructionList;
	}
}
