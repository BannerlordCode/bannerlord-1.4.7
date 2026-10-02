using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000045 RID: 69
	public abstract class PopUpBaseVM : ViewModel
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x00016350 File Offset: 0x00014550
		public PopUpBaseVM(Action closeQuery)
		{
			this._closeQuery = closeQuery;
		}

		// Token: 0x060005DE RID: 1502
		public abstract void ExecuteAffirmativeAction();

		// Token: 0x060005DF RID: 1503
		public abstract void ExecuteNegativeAction();

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001635F File Offset: 0x0001455F
		public virtual void OnTick(float dt)
		{
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00016361 File Offset: 0x00014561
		public virtual void OnClearData()
		{
			this.TitleText = null;
			this.PopUpLabel = null;
			this.ButtonOkLabel = null;
			this.ButtonCancelLabel = null;
			this.IsButtonOkShown = false;
			this.IsButtonCancelShown = false;
			this.IsButtonOkEnabled = false;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00016394 File Offset: 0x00014594
		public void ForceRefreshKeyVisuals()
		{
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.RefreshValues();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.RefreshValues();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000163B7 File Offset: 0x000145B7
		public void CloseQuery()
		{
			Action closeQuery = this._closeQuery;
			if (closeQuery == null)
			{
				return;
			}
			closeQuery();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000163C9 File Offset: 0x000145C9
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x000163F2 File Offset: 0x000145F2
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x000163FA File Offset: 0x000145FA
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

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0001641D File Offset: 0x0001461D
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x00016425 File Offset: 0x00014625
		[DataSourceProperty]
		public string PopUpLabel
		{
			get
			{
				return this._popUpLabel;
			}
			set
			{
				if (value != this._popUpLabel)
				{
					this._popUpLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "PopUpLabel");
				}
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00016448 File Offset: 0x00014648
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x00016450 File Offset: 0x00014650
		[DataSourceProperty]
		public string ButtonOkLabel
		{
			get
			{
				return this._buttonOkLabel;
			}
			set
			{
				if (value != this._buttonOkLabel)
				{
					this._buttonOkLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonOkLabel");
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00016473 File Offset: 0x00014673
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x0001647B File Offset: 0x0001467B
		[DataSourceProperty]
		public string ButtonCancelLabel
		{
			get
			{
				return this._buttonCancelLabel;
			}
			set
			{
				if (value != this._buttonCancelLabel)
				{
					this._buttonCancelLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonCancelLabel");
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0001649E File Offset: 0x0001469E
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x000164A6 File Offset: 0x000146A6
		[DataSourceProperty]
		public bool IsButtonOkShown
		{
			get
			{
				return this._isButtonOkShown;
			}
			set
			{
				if (value != this._isButtonOkShown)
				{
					this._isButtonOkShown = value;
					base.OnPropertyChangedWithValue(value, "IsButtonOkShown");
				}
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x000164C4 File Offset: 0x000146C4
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x000164CC File Offset: 0x000146CC
		[DataSourceProperty]
		public bool IsButtonCancelShown
		{
			get
			{
				return this._isButtonCancelShown;
			}
			set
			{
				if (value != this._isButtonCancelShown)
				{
					this._isButtonCancelShown = value;
					base.OnPropertyChangedWithValue(value, "IsButtonCancelShown");
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000164EA File Offset: 0x000146EA
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x000164F2 File Offset: 0x000146F2
		[DataSourceProperty]
		public bool IsButtonOkEnabled
		{
			get
			{
				return this._isButtonOkEnabled;
			}
			set
			{
				if (value != this._isButtonOkEnabled)
				{
					this._isButtonOkEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsButtonOkEnabled");
				}
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00016510 File Offset: 0x00014710
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00016518 File Offset: 0x00014718
		[DataSourceProperty]
		public bool IsButtonCancelEnabled
		{
			get
			{
				return this._isButtonCancelEnabled;
			}
			set
			{
				if (value != this._isButtonCancelEnabled)
				{
					this._isButtonCancelEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsButtonCancelEnabled");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00016536 File Offset: 0x00014736
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0001653E File Offset: 0x0001473E
		[DataSourceProperty]
		public HintViewModel ButtonOkHint
		{
			get
			{
				return this._buttonOkHint;
			}
			set
			{
				if (value != this._buttonOkHint)
				{
					this._buttonOkHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ButtonOkHint");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001655C File Offset: 0x0001475C
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00016564 File Offset: 0x00014764
		[DataSourceProperty]
		public HintViewModel ButtonCancelHint
		{
			get
			{
				return this._buttonCancelHint;
			}
			set
			{
				if (value != this._buttonCancelHint)
				{
					this._buttonCancelHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ButtonCancelHint");
				}
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00016582 File Offset: 0x00014782
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00016591 File Offset: 0x00014791
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x000165A0 File Offset: 0x000147A0
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x000165A8 File Offset: 0x000147A8
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x000165C6 File Offset: 0x000147C6
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x000165CE File Offset: 0x000147CE
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x040002A0 RID: 672
		protected Action _affirmativeAction;

		// Token: 0x040002A1 RID: 673
		protected Action _negativeAction;

		// Token: 0x040002A2 RID: 674
		private Action _closeQuery;

		// Token: 0x040002A3 RID: 675
		private string _titleText;

		// Token: 0x040002A4 RID: 676
		private string _popUpLabel;

		// Token: 0x040002A5 RID: 677
		private string _buttonOkLabel;

		// Token: 0x040002A6 RID: 678
		private string _buttonCancelLabel;

		// Token: 0x040002A7 RID: 679
		private bool _isButtonOkShown;

		// Token: 0x040002A8 RID: 680
		private bool _isButtonCancelShown;

		// Token: 0x040002A9 RID: 681
		private bool _isButtonOkEnabled;

		// Token: 0x040002AA RID: 682
		private bool _isButtonCancelEnabled;

		// Token: 0x040002AB RID: 683
		private HintViewModel _buttonOkHint;

		// Token: 0x040002AC RID: 684
		private HintViewModel _buttonCancelHint;

		// Token: 0x040002AD RID: 685
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040002AE RID: 686
		private InputKeyItemVM _doneInputKey;
	}
}
