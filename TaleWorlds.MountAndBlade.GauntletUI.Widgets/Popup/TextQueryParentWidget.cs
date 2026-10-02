using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Popup
{
	// Token: 0x02000062 RID: 98
	public class TextQueryParentWidget : Widget
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x00010001 File Offset: 0x0000E201
		public TextQueryParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001000A File Offset: 0x0000E20A
		private void FocusOnTextQuery()
		{
			base.EventManager.FocusedWidget = this.TextInputWidget;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001001D File Offset: 0x0000E21D
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			if (this.TextInputWidget != null)
			{
				this.FocusOnTextQuery();
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00010033 File Offset: 0x0000E233
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0001003B File Offset: 0x0000E23B
		[Editor(false)]
		public EditableTextWidget TextInputWidget
		{
			get
			{
				return this._editableTextWidget;
			}
			set
			{
				if (value != this._editableTextWidget)
				{
					this._editableTextWidget = value;
					this.FocusOnTextQuery();
				}
			}
		}

		// Token: 0x04000246 RID: 582
		private EditableTextWidget _editableTextWidget;
	}
}
