using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012A RID: 298
	public class ClanPartyBehaviorSelectorVM : SelectorVM<SelectorItemVM>
	{
		// Token: 0x06001C08 RID: 7176 RVA: 0x00067C98 File Offset: 0x00065E98
		public ClanPartyBehaviorSelectorVM(int selectedIndex, Action<SelectorVM<SelectorItemVM>> onChange)
			: base(selectedIndex, onChange)
		{
			this.ActionsDisabledHint = new HintViewModel();
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06001C09 RID: 7177 RVA: 0x00067CAD File Offset: 0x00065EAD
		// (set) Token: 0x06001C0A RID: 7178 RVA: 0x00067CB5 File Offset: 0x00065EB5
		[DataSourceProperty]
		public bool CanUseActions
		{
			get
			{
				return this._canUseActions;
			}
			set
			{
				if (value != this._canUseActions)
				{
					this._canUseActions = value;
					base.OnPropertyChangedWithValue(value, "CanUseActions");
				}
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00067CD3 File Offset: 0x00065ED3
		// (set) Token: 0x06001C0C RID: 7180 RVA: 0x00067CDB File Offset: 0x00065EDB
		[DataSourceProperty]
		public HintViewModel ActionsDisabledHint
		{
			get
			{
				return this._actionsDisabledHint;
			}
			set
			{
				if (value != this._actionsDisabledHint)
				{
					this._actionsDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ActionsDisabledHint");
				}
			}
		}

		// Token: 0x04000D14 RID: 3348
		private bool _canUseActions;

		// Token: 0x04000D15 RID: 3349
		private HintViewModel _actionsDisabledHint;
	}
}
