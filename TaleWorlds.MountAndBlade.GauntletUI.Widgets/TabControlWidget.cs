using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000043 RID: 67
	public class TabControlWidget : Widget
	{
		// Token: 0x060003CB RID: 971 RVA: 0x0000C0FD File Offset: 0x0000A2FD
		public TabControlWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000C108 File Offset: 0x0000A308
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this.FirstButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnFirstButtonClick)))
			{
				this.FirstButton.ClickEventHandlers.Add(new Action<Widget>(this.OnFirstButtonClick));
			}
			if (!this.SecondButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnSecondButtonClick)))
			{
				this.SecondButton.ClickEventHandlers.Add(new Action<Widget>(this.OnSecondButtonClick));
			}
			this.FirstButton.IsSelected = this.FirstItem.IsVisible;
			this.SecondButton.IsSelected = this.SecondItem.IsVisible;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000C1BC File Offset: 0x0000A3BC
		public void OnFirstButtonClick(Widget widget)
		{
			if (!this._firstItem.IsVisible && this._secondItem.IsVisible)
			{
				this._secondItem.IsVisible = false;
				this._firstItem.IsVisible = true;
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000C1F0 File Offset: 0x0000A3F0
		public void OnSecondButtonClick(Widget widget)
		{
			if (this._firstItem.IsVisible && !this._secondItem.IsVisible)
			{
				this._secondItem.IsVisible = true;
				this._firstItem.IsVisible = false;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000C224 File Offset: 0x0000A424
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0000C22C File Offset: 0x0000A42C
		[Editor(false)]
		public ButtonWidget FirstButton
		{
			get
			{
				return this._firstButton;
			}
			set
			{
				if (this._firstButton != value)
				{
					this._firstButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FirstButton");
				}
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000C24A File Offset: 0x0000A44A
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0000C252 File Offset: 0x0000A452
		[Editor(false)]
		public ButtonWidget SecondButton
		{
			get
			{
				return this._secondButton;
			}
			set
			{
				if (this._secondButton != value)
				{
					this._secondButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "SecondButton");
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000C270 File Offset: 0x0000A470
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0000C278 File Offset: 0x0000A478
		[Editor(false)]
		public Widget SecondItem
		{
			get
			{
				return this._secondItem;
			}
			set
			{
				if (this._secondItem != value)
				{
					this._secondItem = value;
					base.OnPropertyChanged<Widget>(value, "SecondItem");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000C296 File Offset: 0x0000A496
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000C29E File Offset: 0x0000A49E
		[Editor(false)]
		public Widget FirstItem
		{
			get
			{
				return this._firstItem;
			}
			set
			{
				if (this._firstItem != value)
				{
					this._firstItem = value;
					base.OnPropertyChanged<Widget>(value, "FirstItem");
				}
			}
		}

		// Token: 0x04000198 RID: 408
		private ButtonWidget _firstButton;

		// Token: 0x04000199 RID: 409
		private ButtonWidget _secondButton;

		// Token: 0x0400019A RID: 410
		private Widget _firstItem;

		// Token: 0x0400019B RID: 411
		private Widget _secondItem;
	}
}
