using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x0200012F RID: 303
	public class DecisionSupporterGridWidget : GridWidget
	{
		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0002B905 File Offset: 0x00029B05
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x0002B90D File Offset: 0x00029B0D
		public int VisibleCount { get; set; } = 4;

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x0002B916 File Offset: 0x00029B16
		// (set) Token: 0x06000FCF RID: 4047 RVA: 0x0002B91E File Offset: 0x00029B1E
		public TextWidget MoreTextWidget { get; set; }

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0002B927 File Offset: 0x00029B27
		public DecisionSupporterGridWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0002B937 File Offset: 0x00029B37
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.IsVisible = child.GetSiblingIndex() < this.VisibleCount;
			this.UpdateMoreText();
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0002B95C File Offset: 0x00029B5C
		private void UpdateMoreText()
		{
			if (this.MoreTextWidget != null)
			{
				this.MoreTextWidget.IsVisible = base.ChildCount > this.VisibleCount;
				if (this.MoreTextWidget.IsVisible)
				{
					this.MoreTextWidget.Text = "+" + (base.ChildCount - this.VisibleCount);
				}
			}
		}
	}
}
