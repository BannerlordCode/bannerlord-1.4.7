using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000068 RID: 104
	public class PartyQuestProgressWidget : Widget
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x000108EE File Offset: 0x0000EAEE
		public PartyQuestProgressWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000108F8 File Offset: 0x0000EAF8
		private void UpdateDividers()
		{
			if (this.DividerContainer == null || this.DividerBrush == null)
			{
				return;
			}
			int itemCount = this.ItemCount;
			if (this.DividerContainer.ChildCount > itemCount)
			{
				int num = this.DividerContainer.ChildCount - itemCount;
				for (int i = 0; i < num; i++)
				{
					this.DividerContainer.RemoveChild(this.DividerContainer.GetChild(i));
				}
			}
			else if (itemCount > this.DividerContainer.ChildCount)
			{
				int num2 = itemCount - this.DividerContainer.ChildCount;
				for (int j = 0; j < num2; j++)
				{
					this.DividerContainer.AddChild(this.CreateDivider());
				}
			}
			this.UpdateDividerPositions();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000109A4 File Offset: 0x0000EBA4
		private Widget CreateDivider()
		{
			Widget widget = new Widget(base.Context);
			widget.WidthSizePolicy = SizePolicy.StretchToParent;
			widget.HeightSizePolicy = SizePolicy.StretchToParent;
			BrushWidget brushWidget = new BrushWidget(base.Context);
			brushWidget.WidthSizePolicy = SizePolicy.Fixed;
			brushWidget.HeightSizePolicy = SizePolicy.Fixed;
			brushWidget.Brush = this.DividerBrush;
			brushWidget.SuggestedWidth = (float)brushWidget.ReadOnlyBrush.Sprite.Width;
			brushWidget.SuggestedHeight = (float)brushWidget.ReadOnlyBrush.Sprite.Height;
			brushWidget.HorizontalAlignment = HorizontalAlignment.Right;
			brushWidget.VerticalAlignment = VerticalAlignment.Center;
			brushWidget.PositionXOffset = (float)brushWidget.ReadOnlyBrush.Sprite.Width * 0.5f;
			widget.AddChild(brushWidget);
			return widget;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00010A50 File Offset: 0x0000EC50
		private void UpdateDividerPositions()
		{
			int childCount = this.DividerContainer.ChildCount;
			float num = this.DividerContainer.Size.X / (float)(childCount + 1);
			for (int i = 0; i < childCount; i++)
			{
				Widget child = this.DividerContainer.GetChild(i);
				child.PositionXOffset = (float)i * num - child.Size.X / 2f;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00010ABC File Offset: 0x0000ECBC
		[Editor(false)]
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
			set
			{
				if (this._itemCount != value)
				{
					this._itemCount = value;
					base.OnPropertyChanged(value, "ItemCount");
					this.UpdateDividers();
				}
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00010AE0 File Offset: 0x0000ECE0
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		[Editor(false)]
		public ListPanel DividerContainer
		{
			get
			{
				return this._dividerContainer;
			}
			set
			{
				if (this._dividerContainer != value)
				{
					this._dividerContainer = value;
					base.OnPropertyChanged<ListPanel>(value, "DividerContainer");
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00010B06 File Offset: 0x0000ED06
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00010B0E File Offset: 0x0000ED0E
		[Editor(false)]
		public Brush DividerBrush
		{
			get
			{
				return this._dividerBrush;
			}
			set
			{
				if (this._dividerBrush != value)
				{
					this._dividerBrush = value;
					base.OnPropertyChanged<Brush>(value, "DividerBrush");
				}
			}
		}

		// Token: 0x0400025E RID: 606
		private int _itemCount;

		// Token: 0x0400025F RID: 607
		private ListPanel _dividerContainer;

		// Token: 0x04000260 RID: 608
		private Brush _dividerBrush;
	}
}
