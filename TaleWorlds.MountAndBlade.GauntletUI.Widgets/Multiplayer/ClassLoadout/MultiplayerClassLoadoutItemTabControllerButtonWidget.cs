using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CB RID: 203
	public class MultiplayerClassLoadoutItemTabControllerButtonWidget : ButtonWidget
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x0001DA75 File Offset: 0x0001BC75
		public MultiplayerClassLoadoutItemTabControllerButtonWidget(UIContext context)
			: base(context)
		{
			this._itemTabs = new List<MultiplayerItemTabButtonWidget>();
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0001DA8C File Offset: 0x0001BC8C
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this._itemTabs.Clear();
			for (int i = 0; i < this.ItemTabList.ChildCount; i++)
			{
				MultiplayerItemTabButtonWidget multiplayerItemTabButtonWidget = (MultiplayerItemTabButtonWidget)this.ItemTabList.GetChild(i);
				multiplayerItemTabButtonWidget.boolPropertyChanged += this.TabWidgetPropertyChanged;
				this._itemTabs.Add(multiplayerItemTabButtonWidget);
			}
			this.ItemTabList.OnInitialized += this.ItemTabListInitialized;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001DB08 File Offset: 0x0001BD08
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			for (int i = 0; i < this._itemTabs.Count; i++)
			{
				this._itemTabs[i].boolPropertyChanged -= this.TabWidgetPropertyChanged;
			}
			this._itemTabs.Clear();
			this.ItemTabList.OnInitialized -= this.ItemTabListInitialized;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0001DB70 File Offset: 0x0001BD70
		protected override void OnUpdate(float dt)
		{
			if (this.CursorWidget == null || float.IsNaN(this._targetPositionXOffset) || this.AnimationSpeed <= 0f || MathF.Abs(this.CursorWidget.PositionXOffset - this._targetPositionXOffset) <= 1E-05f)
			{
				return;
			}
			int num = MathF.Sign(this._targetPositionXOffset - this.CursorWidget.PositionXOffset);
			float num2 = MathF.Min(this.AnimationSpeed * dt, 1f);
			this.CursorWidget.PositionXOffset = MathF.Lerp(this.CursorWidget.PositionXOffset, this._targetPositionXOffset, num2, 1E-05f);
			if ((num < 0 && this.CursorWidget.PositionXOffset < this._targetPositionXOffset) || (num > 0 && this.CursorWidget.PositionXOffset > this._targetPositionXOffset))
			{
				this.CursorWidget.PositionXOffset = this._targetPositionXOffset;
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001DC4F File Offset: 0x0001BE4F
		private void TabWidgetPropertyChanged(PropertyOwnerObject sender, string propertyName, bool value)
		{
			if (propertyName == "IsSelected" && value)
			{
				this.SelectedTabChanged(null);
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0001DC67 File Offset: 0x0001BE67
		private void ItemTabListInitialized()
		{
			this.SelectedTabChanged(null);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0001DC70 File Offset: 0x0001BE70
		private void SelectedTabChanged(Widget widget)
		{
			if (this.CursorWidget == null || this.ItemTabList.IntValue < 0)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < this.ItemTabList.ChildCount; i++)
			{
				ButtonWidget buttonWidget = (ButtonWidget)this.ItemTabList.GetChild(i);
				if (buttonWidget.IsVisible)
				{
					num2++;
					if (buttonWidget.IsSelected)
					{
						num = num2 - 1;
					}
				}
			}
			this.CalculateTargetPosition(num, num2);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0001DCE0 File Offset: 0x0001BEE0
		private void CalculateTargetPosition(int selectedIndex, int activeTabCount)
		{
			float num = this.ItemTabList.Size.X / base._scaleToUse;
			float num2 = num / (float)activeTabCount;
			float num3 = (float)selectedIndex * num2 + num2 / 2f;
			this._targetPositionXOffset = num3 - num / 2f;
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0001DD26 File Offset: 0x0001BF26
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x0001DD30 File Offset: 0x0001BF30
		[DataSourceProperty]
		public MultiplayerClassLoadoutItemTabListPanel ItemTabList
		{
			get
			{
				return this._itemTabList;
			}
			set
			{
				if (value != this._itemTabList)
				{
					MultiplayerClassLoadoutItemTabListPanel itemTabList = this._itemTabList;
					if (itemTabList != null)
					{
						itemTabList.SelectEventHandlers.Remove(new Action<Widget>(this.SelectedTabChanged));
					}
					this._itemTabList = value;
					MultiplayerClassLoadoutItemTabListPanel itemTabList2 = this._itemTabList;
					if (itemTabList2 != null)
					{
						itemTabList2.SelectEventHandlers.Add(new Action<Widget>(this.SelectedTabChanged));
					}
					base.OnPropertyChanged<MultiplayerClassLoadoutItemTabListPanel>(value, "ItemTabList");
				}
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0001DD9E File Offset: 0x0001BF9E
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x0001DDA6 File Offset: 0x0001BFA6
		[DataSourceProperty]
		public Widget CursorWidget
		{
			get
			{
				return this._cursorWidget;
			}
			set
			{
				if (value != this._cursorWidget)
				{
					this._cursorWidget = value;
					base.OnPropertyChanged<Widget>(value, "CursorWidget");
				}
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0001DDC4 File Offset: 0x0001BFC4
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x0001DDCC File Offset: 0x0001BFCC
		[DataSourceProperty]
		public float AnimationSpeed
		{
			get
			{
				return this._animationSpeed;
			}
			set
			{
				if (value != this._animationSpeed)
				{
					this._animationSpeed = value;
					base.OnPropertyChanged(value, "AnimationSpeed");
				}
			}
		}

		// Token: 0x040004D5 RID: 1237
		private readonly List<MultiplayerItemTabButtonWidget> _itemTabs;

		// Token: 0x040004D6 RID: 1238
		private float _targetPositionXOffset;

		// Token: 0x040004D7 RID: 1239
		private MultiplayerClassLoadoutItemTabListPanel _itemTabList;

		// Token: 0x040004D8 RID: 1240
		private Widget _cursorWidget;

		// Token: 0x040004D9 RID: 1241
		private float _animationSpeed;
	}
}
