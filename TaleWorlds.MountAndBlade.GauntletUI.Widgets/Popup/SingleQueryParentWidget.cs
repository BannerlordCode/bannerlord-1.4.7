using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Popup
{
	// Token: 0x02000061 RID: 97
	public class SingleQueryParentWidget : Widget
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x0000FF7D File Offset: 0x0000E17D
		public SingleQueryParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000FF86 File Offset: 0x0000E186
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.DescriptionScrollbar.IsVisible)
			{
				this.DescriptionScrollablePanel.GamepadNavigationIndex = 0;
				return;
			}
			this.DescriptionScrollablePanel.GamepadNavigationIndex = -1;
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0000FFB5 File Offset: 0x0000E1B5
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0000FFBD File Offset: 0x0000E1BD
		[Editor(false)]
		public ScrollablePanel DescriptionScrollablePanel
		{
			get
			{
				return this._descriptionScrollablePanel;
			}
			set
			{
				if (value != this._descriptionScrollablePanel)
				{
					this._descriptionScrollablePanel = value;
					base.OnPropertyChanged<ScrollablePanel>(value, "DescriptionScrollablePanel");
				}
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0000FFDB File Offset: 0x0000E1DB
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x0000FFE3 File Offset: 0x0000E1E3
		[Editor(false)]
		public ScrollbarWidget DescriptionScrollbar
		{
			get
			{
				return this._descriptionScrollbar;
			}
			set
			{
				if (value != this._descriptionScrollbar)
				{
					this._descriptionScrollbar = value;
					base.OnPropertyChanged<ScrollbarWidget>(value, "DescriptionScrollbar");
				}
			}
		}

		// Token: 0x04000244 RID: 580
		private ScrollablePanel _descriptionScrollablePanel;

		// Token: 0x04000245 RID: 581
		private ScrollbarWidget _descriptionScrollbar;
	}
}
