using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x02000157 RID: 343
	public class EncyclopediaDividerButtonWidget : ButtonWidget
	{
		// Token: 0x06001243 RID: 4675 RVA: 0x000325B0 File Offset: 0x000307B0
		public EncyclopediaDividerButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000325B9 File Offset: 0x000307B9
		protected override void HandleClick()
		{
			base.HandleClick();
			this.UpdateItemListVisibility();
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000325CD File Offset: 0x000307CD
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.IsVisible = this.ItemListWidget.ChildCount > 0;
			this.UpdateCollapseIndicator();
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000325F0 File Offset: 0x000307F0
		private void UpdateItemListVisibility()
		{
			if (this.ItemListWidget != null && this.ItemListWidget != null)
			{
				this.ItemListWidget.IsVisible = !this.ItemListWidget.IsVisible;
			}
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0003261C File Offset: 0x0003081C
		private void UpdateCollapseIndicator()
		{
			if (this.ItemListWidget != null && this.ItemListWidget != null && this.CollapseIndicator != null)
			{
				if (this.ItemListWidget.IsVisible)
				{
					this.CollapseIndicator.SetState("Expanded");
					return;
				}
				this.CollapseIndicator.SetState("Collapsed");
			}
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0003266F File Offset: 0x0003086F
		private void CollapseIndicatorUpdated()
		{
			this.CollapseIndicator.AddState("Collapsed");
			this.CollapseIndicator.AddState("Expanded");
			this.UpdateCollapseIndicator();
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x00032697 File Offset: 0x00030897
		// (set) Token: 0x0600124A RID: 4682 RVA: 0x0003269F File Offset: 0x0003089F
		public Widget ItemListWidget
		{
			get
			{
				return this._itemListWidget;
			}
			set
			{
				if (value != this._itemListWidget)
				{
					this._itemListWidget = value;
					base.OnPropertyChanged<Widget>(value, "ItemListWidget");
				}
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x0600124B RID: 4683 RVA: 0x000326BD File Offset: 0x000308BD
		// (set) Token: 0x0600124C RID: 4684 RVA: 0x000326C5 File Offset: 0x000308C5
		public Widget CollapseIndicator
		{
			get
			{
				return this._collapseIndicator;
			}
			set
			{
				if (value != this._collapseIndicator)
				{
					this._collapseIndicator = value;
					base.OnPropertyChanged<Widget>(value, "CollapseIndicator");
					this.CollapseIndicatorUpdated();
				}
			}
		}

		// Token: 0x04000850 RID: 2128
		private Widget _itemListWidget;

		// Token: 0x04000851 RID: 2129
		private Widget _collapseIndicator;
	}
}
