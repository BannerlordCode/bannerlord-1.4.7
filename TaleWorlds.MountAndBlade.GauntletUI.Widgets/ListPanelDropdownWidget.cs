using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002C RID: 44
	public class ListPanelDropdownWidget : DropdownWidget
	{
		// Token: 0x0600023F RID: 575 RVA: 0x000082B0 File Offset: 0x000064B0
		public ListPanelDropdownWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000240 RID: 576 RVA: 0x000082B9 File Offset: 0x000064B9
		protected override void OpenPanel()
		{
			base.OpenPanel();
			if (this.ListPanelContainer != null)
			{
				this.ListPanelContainer.IsVisible = true;
			}
			base.Button.IsSelected = true;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000082E1 File Offset: 0x000064E1
		protected override void ClosePanel()
		{
			if (this.ListPanelContainer != null)
			{
				this.ListPanelContainer.IsVisible = false;
			}
			base.Button.IsSelected = false;
			base.ClosePanel();
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00008309 File Offset: 0x00006509
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00008311 File Offset: 0x00006511
		[Editor(false)]
		public Widget ListPanelContainer
		{
			get
			{
				return this._listPanelContainer;
			}
			set
			{
				if (this._listPanelContainer != value)
				{
					this._listPanelContainer = value;
					base.OnPropertyChanged<Widget>(value, "ListPanelContainer");
				}
			}
		}

		// Token: 0x0400010F RID: 271
		private Widget _listPanelContainer;
	}
}
