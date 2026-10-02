using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000AB RID: 171
	public class MultiplayerAdminPanelActionOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x06001052 RID: 4178 RVA: 0x00032E31 File Offset: 0x00031031
		public MultiplayerAdminPanelActionOptionVM(IAdminPanelAction option)
			: base(null)
		{
			this._action = option;
			this.IsActionOption = true;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00032E48 File Offset: 0x00031048
		public override void RefreshValues()
		{
			base.RefreshValues();
			IAdminPanelAction action = this._action;
			base.OptionTitle = ((action != null) ? action.Name : null) ?? string.Empty;
			IAdminPanelAction action2 = this._action;
			base.OptionDescription = ((action2 != null) ? action2.Description : null) ?? string.Empty;
			IAdminPanelAction action3 = this._action;
			if (!string.IsNullOrEmpty((action3 != null) ? action3.Description : null))
			{
				base.DescriptionHint = new HintViewModel(new TextObject("{=!}" + this._action.Description, null), null);
				return;
			}
			base.DescriptionHint = null;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00032EE8 File Offset: 0x000310E8
		public override void UpdateValues()
		{
			base.UpdateValues();
			IAdminPanelAction action = this._action;
			base.IsFilteredOut = action != null && !action.GetIsAvailable();
			string empty = string.Empty;
			IAdminPanelAction action2 = this._action;
			base.IsDisabled = action2 != null && action2.GetIsDisabled(out empty);
			if (!string.IsNullOrEmpty(empty))
			{
				base.DisabledHint = new HintViewModel(new TextObject("{=!}" + empty, null), null);
				return;
			}
			base.DisabledHint = null;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00032F63 File Offset: 0x00031163
		public void ExecuteAction()
		{
			this._action.OnActionExecuted();
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x00032F70 File Offset: 0x00031170
		// (set) Token: 0x06001057 RID: 4183 RVA: 0x00032F78 File Offset: 0x00031178
		[DataSourceProperty]
		public bool IsActionOption
		{
			get
			{
				return this._isActionOption;
			}
			set
			{
				if (value != this._isActionOption)
				{
					this._isActionOption = value;
					base.OnPropertyChangedWithValue(value, "IsActionOption");
				}
			}
		}

		// Token: 0x0400079C RID: 1948
		private readonly IAdminPanelAction _action;

		// Token: 0x0400079D RID: 1949
		private bool _isActionOption;
	}
}
