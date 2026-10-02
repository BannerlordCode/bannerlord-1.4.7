using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information
{
	// Token: 0x02000146 RID: 326
	public class MultiSelectionElementsWidget : Widget
	{
		// Token: 0x06001137 RID: 4407 RVA: 0x0002F5B9 File Offset: 0x0002D7B9
		public MultiSelectionElementsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0002F5CD File Offset: 0x0002D7CD
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._updateRequired)
			{
				this.UpdateElementsList();
				this._updateRequired = false;
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0002F5EB File Offset: 0x0002D7EB
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			if (child is ListPanel)
			{
				this._elementContainer = child as ListPanel;
				this._elementContainer.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnElementAdded));
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0002F624 File Offset: 0x0002D824
		private void OnElementAdded(Widget parentWidget, Widget addedWidget)
		{
			this._updateRequired = true;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0002F630 File Offset: 0x0002D830
		private void UpdateElementsList()
		{
			this._elementsList.Clear();
			for (int i = 0; i < this._elementContainer.ChildCount; i++)
			{
				ButtonWidget buttonWidget = this._elementContainer.GetChild(i).GetChild(0) as ButtonWidget;
				this._elementsList.Add(buttonWidget);
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x0002F682 File Offset: 0x0002D882
		// (set) Token: 0x0600113D RID: 4413 RVA: 0x0002F68A File Offset: 0x0002D88A
		[Editor(false)]
		public ButtonWidget DoneButtonWidget
		{
			get
			{
				return this._doneButtonWidget;
			}
			set
			{
				if (this._doneButtonWidget != value)
				{
					this._doneButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "DoneButtonWidget");
				}
			}
		}

		// Token: 0x040007CD RID: 1997
		private bool _updateRequired;

		// Token: 0x040007CE RID: 1998
		private List<ButtonWidget> _elementsList = new List<ButtonWidget>();

		// Token: 0x040007CF RID: 1999
		private ButtonWidget _doneButtonWidget;

		// Token: 0x040007D0 RID: 2000
		private ListPanel _elementContainer;
	}
}
