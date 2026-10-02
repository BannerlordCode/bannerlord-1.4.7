using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu
{
	// Token: 0x02000080 RID: 128
	public class EscapeMenuVM : ViewModel
	{
		// Token: 0x06000AB0 RID: 2736 RVA: 0x000267D0 File Offset: 0x000249D0
		public EscapeMenuVM(IEnumerable<EscapeMenuItemVM> items, TextObject title = null)
		{
			this._titleObj = title;
			this.MenuItems = new MBBindingList<EscapeMenuItemVM>();
			if (items != null)
			{
				foreach (EscapeMenuItemVM escapeMenuItemVM in items)
				{
					this.MenuItems.Add(escapeMenuItemVM);
				}
			}
			this.Tips = new GameTipsVM(true, true);
			this.RefreshValues();
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002684C File Offset: 0x00024A4C
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject titleObj = this._titleObj;
			this.Title = ((titleObj != null) ? titleObj.ToString() : null) ?? "";
			this.MenuItems.ApplyActionOnAllItems(delegate(EscapeMenuItemVM x)
			{
				x.RefreshValues();
			});
			this.Tips.RefreshValues();
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000268B5 File Offset: 0x00024AB5
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000268B8 File Offset: 0x00024AB8
		public void RefreshItems(IEnumerable<EscapeMenuItemVM> items)
		{
			this.MenuItems.Clear();
			foreach (EscapeMenuItemVM escapeMenuItemVM in items)
			{
				this.MenuItems.Add(escapeMenuItemVM);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00026910 File Offset: 0x00024B10
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x00026918 File Offset: 0x00024B18
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0002693B File Offset: 0x00024B3B
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00026943 File Offset: 0x00024B43
		[DataSourceProperty]
		public MBBindingList<EscapeMenuItemVM> MenuItems
		{
			get
			{
				return this._menuItems;
			}
			set
			{
				if (value != this._menuItems)
				{
					this._menuItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<EscapeMenuItemVM>>(value, "MenuItems");
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00026961 File Offset: 0x00024B61
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x00026969 File Offset: 0x00024B69
		[DataSourceProperty]
		public GameTipsVM Tips
		{
			get
			{
				return this._tips;
			}
			set
			{
				if (value != this._tips)
				{
					this._tips = value;
					base.OnPropertyChangedWithValue<GameTipsVM>(value, "Tips");
				}
			}
		}

		// Token: 0x040004DE RID: 1246
		private readonly TextObject _titleObj;

		// Token: 0x040004DF RID: 1247
		private string _title;

		// Token: 0x040004E0 RID: 1248
		private MBBindingList<EscapeMenuItemVM> _menuItems;

		// Token: 0x040004E1 RID: 1249
		private GameTipsVM _tips;
	}
}
