using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003E RID: 62
	public class SelectorWidget : Widget
	{
		// Token: 0x060003A5 RID: 933 RVA: 0x0000B8DD File Offset: 0x00009ADD
		public SelectorWidget(UIContext context)
			: base(context)
		{
			this._listSelectionHandler = new Action<Widget>(this.OnSelectionChanged);
			this._listItemRemovedHandler = new Action<Widget, Widget>(this.OnListChanged);
			this._listItemAddedHandler = new Action<Widget, Widget>(this.OnListChanged);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000B91C File Offset: 0x00009B1C
		public void OnListChanged(Widget widget)
		{
			this.RefreshSelectedItem();
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000B924 File Offset: 0x00009B24
		public void OnListChanged(Widget parentWidget, Widget addedWidget)
		{
			this.RefreshSelectedItem();
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000B92C File Offset: 0x00009B2C
		public void OnSelectionChanged(Widget widget)
		{
			this.CurrentSelectedIndex = this.ListPanelValue;
			this.RefreshSelectedItem();
			base.OnPropertyChanged(this.CurrentSelectedIndex, "CurrentSelectedIndex");
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000B951 File Offset: 0x00009B51
		private void RefreshSelectedItem()
		{
			this.ListPanelValue = this.CurrentSelectedIndex;
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000B95F File Offset: 0x00009B5F
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0000B976 File Offset: 0x00009B76
		[Editor(false)]
		public int ListPanelValue
		{
			get
			{
				if (this.Container != null)
				{
					return this.Container.IntValue;
				}
				return -1;
			}
			set
			{
				if (this.Container != null && this.Container.IntValue != value)
				{
					this.Container.IntValue = value;
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000B99A File Offset: 0x00009B9A
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0000B9A2 File Offset: 0x00009BA2
		[Editor(false)]
		public int CurrentSelectedIndex
		{
			get
			{
				return this._currentSelectedIndex;
			}
			set
			{
				if (this._currentSelectedIndex != value && value >= 0)
				{
					this._currentSelectedIndex = value;
					this.RefreshSelectedItem();
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000B9BE File Offset: 0x00009BBE
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		[Editor(false)]
		public Container Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (this._container != null)
				{
					this._container.SelectEventHandlers.Remove(this._listSelectionHandler);
					this._container.ItemAddEventHandlers.Remove(this._listItemAddedHandler);
					this._container.ItemRemoveEventHandlers.Remove(this._listItemRemovedHandler);
				}
				this._container = value;
				if (this._container != null)
				{
					this._container.SelectEventHandlers.Add(this._listSelectionHandler);
					this._container.ItemAddEventHandlers.Add(this._listItemAddedHandler);
					this._container.ItemRemoveEventHandlers.Add(this._listItemRemovedHandler);
				}
				this.RefreshSelectedItem();
			}
		}

		// Token: 0x04000185 RID: 389
		private int _currentSelectedIndex;

		// Token: 0x04000186 RID: 390
		private Action<Widget> _listSelectionHandler;

		// Token: 0x04000187 RID: 391
		private Action<Widget, Widget> _listItemRemovedHandler;

		// Token: 0x04000188 RID: 392
		private Action<Widget, Widget> _listItemAddedHandler;

		// Token: 0x04000189 RID: 393
		private Container _container;
	}
}
