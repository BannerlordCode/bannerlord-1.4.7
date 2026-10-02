using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006A RID: 106
	public class PartyTroopManagementItemButtonWidget : ButtonWidget
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x00011419 File Offset: 0x0000F619
		public PartyTroopManagementItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00011424 File Offset: 0x0000F624
		public Widget GetActionButtonAtIndex(int index)
		{
			if (this.ActionButtonsContainer != null)
			{
				int num = 0;
				List<Widget> allChildrenRecursive = this.ActionButtonsContainer.GetAllChildrenRecursive(null);
				for (int i = 0; i < allChildrenRecursive.Count; i++)
				{
					if (allChildrenRecursive[i].Id == "ActionButton")
					{
						if (num == index)
						{
							return allChildrenRecursive[i];
						}
						num++;
					}
				}
			}
			return null;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00011482 File Offset: 0x0000F682
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001148A File Offset: 0x0000F68A
		public Widget ActionButtonsContainer
		{
			get
			{
				return this._actionButtonsContainer;
			}
			set
			{
				if (value != this._actionButtonsContainer)
				{
					this._actionButtonsContainer = value;
					base.OnPropertyChanged<Widget>(value, "ActionButtonsContainer");
				}
			}
		}

		// Token: 0x0400027A RID: 634
		private Widget _actionButtonsContainer;
	}
}
