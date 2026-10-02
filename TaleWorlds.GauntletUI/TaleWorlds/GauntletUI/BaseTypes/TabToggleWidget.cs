using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200006A RID: 106
	public class TabToggleWidget : ButtonWidget
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0001F443 File Offset: 0x0001D643
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0001F44B File Offset: 0x0001D64B
		public TabControl TabControlWidget { get; set; }

		// Token: 0x06000748 RID: 1864 RVA: 0x0001F454 File Offset: 0x0001D654
		public TabToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001F45D File Offset: 0x0001D65D
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this.TabControlWidget != null && !string.IsNullOrEmpty(this.TabName))
			{
				this.TabControlWidget.SetActiveTab(this.TabName);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001F48C File Offset: 0x0001D68C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			bool flag = false;
			if (this.TabControlWidget == null || string.IsNullOrEmpty(this.TabName))
			{
				flag = true;
			}
			else
			{
				Widget widget = this.TabControlWidget.FindChild(this.TabName);
				if (widget == null || widget.IsDisabled)
				{
					flag = true;
				}
			}
			base.IsDisabled = flag;
			base.IsSelected = this.DetermineIfIsSelected();
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001F4F0 File Offset: 0x0001D6F0
		private bool DetermineIfIsSelected()
		{
			TabControl tabControlWidget = this.TabControlWidget;
			return ((tabControlWidget != null) ? tabControlWidget.ActiveTab : null) != null && !string.IsNullOrEmpty(this.TabName) && this.TabControlWidget.ActiveTab.Id == this.TabName && base.IsVisible;
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001F543 File Offset: 0x0001D743
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x0001F54B File Offset: 0x0001D74B
		[Editor(false)]
		public string TabName
		{
			get
			{
				return this._tabName;
			}
			set
			{
				if (this._tabName != value)
				{
					this._tabName = value;
					base.OnPropertyChanged<string>(value, "TabName");
				}
			}
		}

		// Token: 0x04000364 RID: 868
		private string _tabName;
	}
}
