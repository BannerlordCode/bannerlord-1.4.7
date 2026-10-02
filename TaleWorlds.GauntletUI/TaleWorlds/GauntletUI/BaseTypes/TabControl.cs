using System;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000069 RID: 105
	public class TabControl : Widget
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600073A RID: 1850 RVA: 0x0001F204 File Offset: 0x0001D404
		// (remove) Token: 0x0600073B RID: 1851 RVA: 0x0001F23C File Offset: 0x0001D43C
		public event OnActiveTabChangeEvent OnActiveTabChange;

		// Token: 0x0600073C RID: 1852 RVA: 0x0001F271 File Offset: 0x0001D471
		public TabControl(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001F27A File Offset: 0x0001D47A
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			if (child == this.ActiveTab)
			{
				this.ActiveTab = null;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001F293 File Offset: 0x0001D493
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001F29B File Offset: 0x0001D49B
		[Editor(false)]
		public Widget ActiveTab
		{
			get
			{
				return this._activeTab;
			}
			private set
			{
				if (this._activeTab != value)
				{
					this._activeTab = value;
					OnActiveTabChangeEvent onActiveTabChange = this.OnActiveTabChange;
					if (onActiveTabChange == null)
					{
						return;
					}
					onActiveTabChange();
				}
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
		private void SetActiveTab(int index)
		{
			Widget child = base.GetChild(index);
			this.SetActiveTab(child);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001F2DC File Offset: 0x0001D4DC
		public void SetActiveTab(string tabName)
		{
			Widget widget = base.FindChild(tabName);
			this.SetActiveTab(widget);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001F2F8 File Offset: 0x0001D4F8
		private void SetActiveTab(Widget newTab)
		{
			if (this.ActiveTab != newTab && newTab != null)
			{
				if (this.ActiveTab != null)
				{
					this.ActiveTab.IsVisible = false;
				}
				this.ActiveTab = newTab;
				this.ActiveTab.IsVisible = true;
				this.SelectedIndex = base.GetChildIndex(this.ActiveTab);
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001F34C File Offset: 0x0001D54C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.ActiveTab != null && this.ActiveTab.ParentWidget == null)
			{
				this.ActiveTab = null;
			}
			if (this.ActiveTab == null || this.ActiveTab.IsDisabled)
			{
				for (int i = 0; i < base.ChildCount; i++)
				{
					Widget child = base.GetChild(i);
					if (child.IsEnabled && !string.IsNullOrEmpty(child.Id))
					{
						this.ActiveTab = child;
						break;
					}
				}
			}
			for (int j = 0; j < base.ChildCount; j++)
			{
				Widget child2 = base.GetChild(j);
				if (this.ActiveTab != child2 && (child2.IsEnabled || child2.IsVisible))
				{
					child2.IsVisible = false;
				}
				if (this.ActiveTab == child2)
				{
					child2.IsVisible = true;
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001F411 File Offset: 0x0001D611
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0001F419 File Offset: 0x0001D619
		[DataSourceProperty]
		public int SelectedIndex
		{
			get
			{
				return this._selectedIndex;
			}
			set
			{
				if (this._selectedIndex != value)
				{
					this._selectedIndex = value;
					this.SetActiveTab(this._selectedIndex);
					base.OnPropertyChanged(value, "SelectedIndex");
				}
			}
		}

		// Token: 0x04000361 RID: 865
		private Widget _activeTab;

		// Token: 0x04000362 RID: 866
		private int _selectedIndex;
	}
}
