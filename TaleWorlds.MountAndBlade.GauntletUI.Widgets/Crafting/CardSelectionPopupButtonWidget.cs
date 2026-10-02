using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000162 RID: 354
	public class CardSelectionPopupButtonWidget : ButtonWidget
	{
		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x000335CC File Offset: 0x000317CC
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x000335D4 File Offset: 0x000317D4
		public CircularAutoScrollablePanelWidget PropertiesContainer { get; set; }

		// Token: 0x060012B0 RID: 4784 RVA: 0x000335DD File Offset: 0x000317DD
		public CardSelectionPopupButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000335E6 File Offset: 0x000317E6
		public override void SetState(string stateName)
		{
			base.SetState(stateName);
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetState(stateName);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00033600 File Offset: 0x00031800
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetHoverBegin();
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00033618 File Offset: 0x00031818
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetHoverEnd();
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00033630 File Offset: 0x00031830
		protected override void OnMouseScroll()
		{
			base.OnMouseScroll();
			CircularAutoScrollablePanelWidget propertiesContainer = this.PropertiesContainer;
			if (propertiesContainer == null)
			{
				return;
			}
			propertiesContainer.SetScrollMouse();
		}
	}
}
