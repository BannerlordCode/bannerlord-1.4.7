using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu
{
	// Token: 0x0200007F RID: 127
	public class EscapeMenuItemVM : ViewModel
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x00026687 File Offset: 0x00024887
		public EscapeMenuItemVM(TextObject item, Action<object> onExecute, object identifier, Func<Tuple<bool, TextObject>> getIsDisabledAndReason, bool isPositiveBehaviored = false)
		{
			this._onExecute = onExecute;
			this._identifier = identifier;
			this._itemObj = item;
			this.ActionText = this._itemObj.ToString();
			this.IsPositiveBehaviored = isPositiveBehaviored;
			this._getIsDisabledAndReason = getIsDisabledAndReason;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000266C8 File Offset: 0x000248C8
		public override void RefreshValues()
		{
			base.RefreshValues();
			Func<Tuple<bool, TextObject>> getIsDisabledAndReason = this._getIsDisabledAndReason;
			Tuple<bool, TextObject> tuple = ((getIsDisabledAndReason != null) ? getIsDisabledAndReason() : null);
			this.IsDisabled = tuple.Item1;
			this.DisabledHint = new HintViewModel(tuple.Item2, null);
			this.ActionText = this._itemObj.ToString();
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002671D File Offset: 0x0002491D
		public void ExecuteAction()
		{
			this._onExecute(this._identifier);
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00026730 File Offset: 0x00024930
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x00026738 File Offset: 0x00024938
		[DataSourceProperty]
		public HintViewModel DisabledHint
		{
			get
			{
				return this._disabledHint;
			}
			set
			{
				if (value != this._disabledHint)
				{
					this._disabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledHint");
				}
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00026756 File Offset: 0x00024956
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0002675E File Offset: 0x0002495E
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00026781 File Offset: 0x00024981
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x00026789 File Offset: 0x00024989
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x000267A7 File Offset: 0x000249A7
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x000267AF File Offset: 0x000249AF
		[DataSourceProperty]
		public bool IsPositiveBehaviored
		{
			get
			{
				return this._isPositiveBehaviored;
			}
			set
			{
				if (value != this._isPositiveBehaviored)
				{
					this._isPositiveBehaviored = value;
					base.OnPropertyChangedWithValue(value, "IsPositiveBehaviored");
				}
			}
		}

		// Token: 0x040004D6 RID: 1238
		private readonly object _identifier;

		// Token: 0x040004D7 RID: 1239
		private readonly Action<object> _onExecute;

		// Token: 0x040004D8 RID: 1240
		private readonly TextObject _itemObj;

		// Token: 0x040004D9 RID: 1241
		private readonly Func<Tuple<bool, TextObject>> _getIsDisabledAndReason;

		// Token: 0x040004DA RID: 1242
		private HintViewModel _disabledHint;

		// Token: 0x040004DB RID: 1243
		private string _actionText;

		// Token: 0x040004DC RID: 1244
		private bool _isDisabled;

		// Token: 0x040004DD RID: 1245
		private bool _isPositiveBehaviored;
	}
}
