using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000069 RID: 105
	public class ActionOptionDataVM : GenericOptionDataVM
	{
		// Token: 0x06000830 RID: 2096 RVA: 0x0001C750 File Offset: 0x0001A950
		public ActionOptionDataVM(Action onAction, OptionsVM optionsVM, IOptionData option, TextObject name, TextObject optionActionName, TextObject description)
			: base(optionsVM, option, name, description, OptionsVM.OptionsDataType.ActionOption)
		{
			this._onAction = onAction;
			this._optionActionName = optionActionName;
			this.RefreshValues();
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001C774 File Offset: 0x0001A974
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._optionActionName != null)
			{
				this.ActionName = this._optionActionName.ToString();
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001C79B File Offset: 0x0001A99B
		private void ExecuteAction()
		{
			Action onAction = this._onAction;
			if (onAction == null)
			{
				return;
			}
			onAction.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001C7B3 File Offset: 0x0001A9B3
		public override void Cancel()
		{
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001C7B5 File Offset: 0x0001A9B5
		public override bool IsChanged()
		{
			return false;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001C7B8 File Offset: 0x0001A9B8
		public override void ResetData()
		{
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001C7BA File Offset: 0x0001A9BA
		public override void SetValue(float value)
		{
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001C7BC File Offset: 0x0001A9BC
		public override void UpdateValue()
		{
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001C7BE File Offset: 0x0001A9BE
		public override void ApplyValue()
		{
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0001C7C0 File Offset: 0x0001A9C0
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x0001C7C8 File Offset: 0x0001A9C8
		[DataSourceProperty]
		public string ActionName
		{
			get
			{
				return this._actionName;
			}
			set
			{
				if (value != this._actionName)
				{
					this._actionName = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionName");
				}
			}
		}

		// Token: 0x040003A8 RID: 936
		private readonly Action _onAction;

		// Token: 0x040003A9 RID: 937
		private readonly TextObject _optionActionName;

		// Token: 0x040003AA RID: 938
		private string _actionName;
	}
}
