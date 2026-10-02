using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000169 RID: 361
	public class CraftingPieceTypeSelectorButtonWidget : ButtonWidget
	{
		// Token: 0x0600130E RID: 4878 RVA: 0x00033FD4 File Offset: 0x000321D4
		public CraftingPieceTypeSelectorButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00033FDD File Offset: 0x000321DD
		public override void SetState(string stateName)
		{
			base.SetState(stateName);
			Widget visualsWidget = this.VisualsWidget;
			if (visualsWidget == null)
			{
				return;
			}
			visualsWidget.SetState(stateName);
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x00033FF7 File Offset: 0x000321F7
		// (set) Token: 0x06001311 RID: 4881 RVA: 0x00033FFF File Offset: 0x000321FF
		public Widget VisualsWidget
		{
			get
			{
				return this._visualsWidget;
			}
			set
			{
				if (value != this._visualsWidget)
				{
					this._visualsWidget = value;
				}
			}
		}

		// Token: 0x040008A5 RID: 2213
		private Widget _visualsWidget;
	}
}
