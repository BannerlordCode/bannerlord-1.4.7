using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000048 RID: 72
	public class TextQueryPopUpVM : PopUpBaseVM
	{
		// Token: 0x06000620 RID: 1568 RVA: 0x00016DFA File Offset: 0x00014FFA
		public TextQueryPopUpVM(Action closeQuery)
			: base(closeQuery)
		{
			this.DoneButtonDisabledReasonHint = new HintViewModel();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00016E10 File Offset: 0x00015010
		public void SetData(TextInquiryData data)
		{
			this._data = data;
			base.TitleText = this._data.TitleText;
			base.PopUpLabel = this._data.Text;
			base.ButtonOkLabel = this._data.AffirmativeText;
			base.ButtonCancelLabel = this._data.NegativeText;
			base.IsButtonOkShown = this._data.IsAffirmativeOptionShown;
			base.IsButtonCancelShown = this._data.IsNegativeOptionShown;
			this.IsInputObfuscated = this._data.IsInputObfuscated;
			this.InputText = this._data.DefaultInputText;
			Func<string, Tuple<bool, string>> textCondition = this._data.TextCondition;
			base.IsButtonOkEnabled = textCondition == null || textCondition(this.InputText).Item1;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00016ED4 File Offset: 0x000150D4
		public override void ExecuteAffirmativeAction()
		{
			Action<string> affirmativeAction = this._data.AffirmativeAction;
			if (affirmativeAction != null)
			{
				affirmativeAction(this.InputText);
			}
			base.CloseQuery();
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00016EF8 File Offset: 0x000150F8
		public override void ExecuteNegativeAction()
		{
			Action negativeAction = this._data.NegativeAction;
			if (negativeAction != null)
			{
				negativeAction();
			}
			base.CloseQuery();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00016F16 File Offset: 0x00015116
		public override void OnClearData()
		{
			base.OnClearData();
			this._data = null;
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00016FBC File Offset: 0x000151BC
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x00016F28 File Offset: 0x00015128
		[DataSourceProperty]
		public string InputText
		{
			get
			{
				return this._inputText;
			}
			set
			{
				if (value != this._inputText)
				{
					this._inputText = value;
					base.OnPropertyChangedWithValue<string>(value, "InputText");
					Func<string, Tuple<bool, string>> textCondition = this._data.TextCondition;
					Tuple<bool, string> tuple = ((textCondition != null) ? textCondition(value) : null);
					base.IsButtonOkEnabled = tuple == null || tuple.Item1;
					this.DoneButtonDisabledReasonHint.HintText = (string.IsNullOrEmpty((tuple != null) ? tuple.Item2 : null) ? TextObject.GetEmpty() : new TextObject("{=!}" + tuple.Item2, null));
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00016FC4 File Offset: 0x000151C4
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x00016FCC File Offset: 0x000151CC
		public bool IsInputObfuscated
		{
			get
			{
				return this._isInputObfuscated;
			}
			set
			{
				if (value != this._isInputObfuscated)
				{
					this._isInputObfuscated = value;
					base.OnPropertyChangedWithValue(value, "IsInputObfuscated");
				}
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00016FEA File Offset: 0x000151EA
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x00016FF2 File Offset: 0x000151F2
		[DataSourceProperty]
		public HintViewModel DoneButtonDisabledReasonHint
		{
			get
			{
				return this._doneButtonDisabledReasonHint;
			}
			set
			{
				if (value != this._doneButtonDisabledReasonHint)
				{
					this._doneButtonDisabledReasonHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneButtonDisabledReasonHint");
				}
			}
		}

		// Token: 0x040002BE RID: 702
		private TextInquiryData _data;

		// Token: 0x040002BF RID: 703
		[DataSourceProperty]
		private string _inputText;

		// Token: 0x040002C0 RID: 704
		private bool _isInputObfuscated;

		// Token: 0x040002C1 RID: 705
		private HintViewModel _doneButtonDisabledReasonHint;
	}
}
