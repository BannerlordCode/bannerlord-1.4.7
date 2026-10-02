using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Inquiries
{
	// Token: 0x02000047 RID: 71
	public class SingleQueryPopUpVM : PopUpBaseVM
	{
		// Token: 0x06000613 RID: 1555 RVA: 0x00016A72 File Offset: 0x00014C72
		public SingleQueryPopUpVM(Action closeQuery)
			: base(closeQuery)
		{
			base.ButtonOkHint = new HintViewModel();
			base.ButtonCancelHint = new HintViewModel();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00016A94 File Offset: 0x00014C94
		public override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._data != null)
			{
				this.UpdateButtonEnabledStates();
				if (this._data.ExpireTime > 0f)
				{
					if (this._queryTimer > this._data.ExpireTime)
					{
						Action timeoutAction = this._data.TimeoutAction;
						if (timeoutAction != null)
						{
							timeoutAction();
						}
						base.CloseQuery();
						return;
					}
					this._queryTimer += dt;
					this.RemainingQueryTime = this._data.ExpireTime - this._queryTimer;
				}
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00016B1E File Offset: 0x00014D1E
		public override void ExecuteAffirmativeAction()
		{
			Action affirmativeAction = this._data.AffirmativeAction;
			if (affirmativeAction != null)
			{
				affirmativeAction();
			}
			base.CloseQuery();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00016B3C File Offset: 0x00014D3C
		public override void ExecuteNegativeAction()
		{
			Action negativeAction = this._data.NegativeAction;
			if (negativeAction != null)
			{
				negativeAction();
			}
			base.CloseQuery();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00016B5A File Offset: 0x00014D5A
		public override void OnClearData()
		{
			base.OnClearData();
			this._data = null;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00016B6C File Offset: 0x00014D6C
		private void UpdateButtonEnabledStates()
		{
			if (this._data.GetIsAffirmativeOptionEnabled != null)
			{
				ValueTuple<bool, string> valueTuple = this._data.GetIsAffirmativeOptionEnabled();
				base.IsButtonOkEnabled = valueTuple.Item1;
				if (!string.Equals(this._lastButtonOkHint, valueTuple.Item2, StringComparison.OrdinalIgnoreCase))
				{
					base.ButtonOkHint.HintText = (string.IsNullOrEmpty(valueTuple.Item2) ? TextObject.GetEmpty() : new TextObject("{=!}" + valueTuple.Item2, null));
					this._lastButtonOkHint = valueTuple.Item2;
				}
			}
			else
			{
				base.IsButtonOkEnabled = true;
				base.ButtonOkHint.HintText = TextObject.GetEmpty();
				this._lastButtonOkHint = string.Empty;
			}
			if (this._data.GetIsNegativeOptionEnabled != null)
			{
				ValueTuple<bool, string> valueTuple2 = this._data.GetIsNegativeOptionEnabled();
				base.IsButtonCancelEnabled = valueTuple2.Item1;
				if (!string.Equals(this._lastButtonCancelHint, valueTuple2.Item2, StringComparison.OrdinalIgnoreCase))
				{
					base.ButtonCancelHint.HintText = (string.IsNullOrEmpty(valueTuple2.Item2) ? TextObject.GetEmpty() : new TextObject("{=!}" + valueTuple2.Item2, null));
					this._lastButtonCancelHint = valueTuple2.Item2;
					return;
				}
			}
			else
			{
				base.IsButtonCancelEnabled = true;
				base.ButtonCancelHint.HintText = TextObject.GetEmpty();
				this._lastButtonCancelHint = string.Empty;
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00016CC0 File Offset: 0x00014EC0
		public void SetData(InquiryData data)
		{
			this._data = data;
			base.TitleText = this._data.TitleText;
			base.PopUpLabel = this._data.Text;
			base.ButtonOkLabel = this._data.AffirmativeText;
			base.ButtonCancelLabel = this._data.NegativeText;
			base.IsButtonOkShown = this._data.IsAffirmativeOptionShown;
			base.IsButtonCancelShown = this._data.IsNegativeOptionShown;
			this.IsTimerShown = this._data.ExpireTime > 0f;
			base.IsButtonOkEnabled = true;
			base.IsButtonCancelEnabled = true;
			this.UpdateButtonEnabledStates();
			this._queryTimer = 0f;
			this.TotalQueryTime = (float)MathF.Round(this._data.ExpireTime);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00016D88 File Offset: 0x00014F88
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00016D90 File Offset: 0x00014F90
		[DataSourceProperty]
		public float RemainingQueryTime
		{
			get
			{
				return this._remainingQueryTime;
			}
			set
			{
				if (value != this._remainingQueryTime)
				{
					this._remainingQueryTime = value;
					base.OnPropertyChangedWithValue(value, "RemainingQueryTime");
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00016DAE File Offset: 0x00014FAE
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00016DB6 File Offset: 0x00014FB6
		[DataSourceProperty]
		public float TotalQueryTime
		{
			get
			{
				return this._totalQueryTime;
			}
			set
			{
				if (value != this._totalQueryTime)
				{
					this._totalQueryTime = value;
					base.OnPropertyChangedWithValue(value, "TotalQueryTime");
				}
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00016DD4 File Offset: 0x00014FD4
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00016DDC File Offset: 0x00014FDC
		[DataSourceProperty]
		public bool IsTimerShown
		{
			get
			{
				return this._isTimerShown;
			}
			set
			{
				if (value != this._isTimerShown)
				{
					this._isTimerShown = value;
					base.OnPropertyChangedWithValue(value, "IsTimerShown");
				}
			}
		}

		// Token: 0x040002B7 RID: 695
		private InquiryData _data;

		// Token: 0x040002B8 RID: 696
		private float _queryTimer;

		// Token: 0x040002B9 RID: 697
		private string _lastButtonOkHint;

		// Token: 0x040002BA RID: 698
		private string _lastButtonCancelHint;

		// Token: 0x040002BB RID: 699
		private float _remainingQueryTime;

		// Token: 0x040002BC RID: 700
		private float _totalQueryTime;

		// Token: 0x040002BD RID: 701
		private bool _isTimerShown;
	}
}
