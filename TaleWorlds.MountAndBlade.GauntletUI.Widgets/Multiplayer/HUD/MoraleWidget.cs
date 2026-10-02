using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C6 RID: 198
	public class MoraleWidget : Widget
	{
		// Token: 0x06000A42 RID: 2626 RVA: 0x0001CC8E File Offset: 0x0001AE8E
		public MoraleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001CC97 File Offset: 0x0001AE97
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this._moraleItemWidgets = this.CreateItemWidgets(this.ItemContainer);
			this.SetItemWidgetColors(this._teamColor);
			this.SetItemGlowWidgetColors(this._teamColorSecondary);
			this.RestartAnimations();
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001CCD0 File Offset: 0x0001AED0
		protected override void OnUpdate(float dt)
		{
			if (this._triggerAnimations)
			{
				if (this._animWaitFrame >= 1)
				{
					this.HandleAnimation();
					this._triggerAnimations = false;
					this._animWaitFrame = 0;
				}
				else
				{
					this._animWaitFrame++;
				}
			}
			if (!this._initialized)
			{
				this.FlowArrowWidget.LeftSideArrow = this.ExtendToLeft;
				this._initialized = true;
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001CD32 File Offset: 0x0001AF32
		private void RestartAnimations()
		{
			this._triggerAnimations = true;
			this._animWaitFrame = 0;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0001CD42 File Offset: 0x0001AF42
		private void UpdateArrows(int flowLevel)
		{
			if (this.Container == null || this.FlowArrowWidget == null)
			{
				return;
			}
			this.FlowArrowWidget.SetFlowLevel(flowLevel);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001CD64 File Offset: 0x0001AF64
		private void UpdateMoraleMask()
		{
			int num = MathF.Floor((float)this.MoralePercentage / 100f * 10f);
			for (int i = 0; i < this._moraleItemWidgets.Length; i++)
			{
				MoraleWidget.MoraleItemWidget moraleItemWidget = this._moraleItemWidgets[i];
				float num2 = 0f;
				if (i < num)
				{
					num2 = 1f;
					if (!moraleItemWidget.ItemGlowWidget.IsVisible)
					{
						this.RestartAnimations();
					}
				}
				else if (i == num)
				{
					float num3 = 10f;
					num2 = ((float)this.MoralePercentage - (float)num * num3) / num3;
					if (!moraleItemWidget.ItemWidget.IsVisible && !MBMath.ApproximatelyEquals(num2, 0f, 1E-05f))
					{
						this.RestartAnimations();
					}
				}
				moraleItemWidget.SetFillAmount(num2, 12);
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001CE18 File Offset: 0x0001B018
		private string GetCurrentStateName()
		{
			string text;
			if (this.MoralePercentage < 20)
			{
				text = "IsCriticalAnim";
			}
			else if (this.IncreaseLevel > 0)
			{
				text = "IncreaseAnim";
			}
			else
			{
				text = "Default";
			}
			return text;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001CE50 File Offset: 0x0001B050
		private void HandleAnimation()
		{
			for (int i = 0; i < this._moraleItemWidgets.Length; i++)
			{
				MoraleWidget.MoraleItemWidget moraleItemWidget = this._moraleItemWidgets[i];
				moraleItemWidget.ItemWidget.SetState(this._currentStateName);
				moraleItemWidget.ItemWidget.BrushRenderer.RestartAnimation();
				moraleItemWidget.ItemGlowWidget.SetState(this._currentStateName);
				moraleItemWidget.ItemGlowWidget.BrushRenderer.RestartAnimation();
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0001CEBC File Offset: 0x0001B0BC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdateMoraleMask();
			string currentStateName = this.GetCurrentStateName();
			if (this._currentStateName != currentStateName)
			{
				this._currentStateName = currentStateName;
				this.RestartAnimations();
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0001CEF8 File Offset: 0x0001B0F8
		private MoraleWidget.MoraleItemWidget[] CreateItemWidgets(Widget containerWidget)
		{
			MoraleWidget.MoraleItemWidget[] array = new MoraleWidget.MoraleItemWidget[10];
			for (int i = 0; i < 10; i++)
			{
				Widget widget = new Widget(base.Context);
				widget.UpdateChildrenStates = true;
				widget.WidthSizePolicy = SizePolicy.Fixed;
				widget.HeightSizePolicy = SizePolicy.Fixed;
				widget.SuggestedWidth = 39f;
				widget.SuggestedHeight = 38f;
				if (this.ExtendToLeft)
				{
					widget.HorizontalAlignment = HorizontalAlignment.Right;
					widget.MarginRight = (float)i * 28f;
				}
				else
				{
					widget.HorizontalAlignment = HorizontalAlignment.Left;
					widget.MarginLeft = (float)i * 28f;
				}
				widget.AddState("IncreaseAnim");
				widget.AddState("IsCriticalAnim");
				containerWidget.AddChild(widget);
				Widget widget2 = new Widget(base.Context);
				widget2.ClipContents = true;
				widget2.UpdateChildrenStates = true;
				widget2.WidthSizePolicy = SizePolicy.StretchToParent;
				widget2.HeightSizePolicy = SizePolicy.Fixed;
				widget2.VerticalAlignment = VerticalAlignment.Bottom;
				widget.AddChild(widget2);
				BrushWidget brushWidget = new BrushWidget(base.Context);
				brushWidget.WidthSizePolicy = SizePolicy.Fixed;
				brushWidget.HeightSizePolicy = SizePolicy.Fixed;
				brushWidget.VerticalAlignment = VerticalAlignment.Bottom;
				brushWidget.Brush = this.ItemGlowBrush;
				brushWidget.SuggestedWidth = 39f;
				brushWidget.SuggestedHeight = 38f;
				brushWidget.AddState("IncreaseAnim");
				brushWidget.AddState("IsCriticalAnim");
				widget2.AddChild(brushWidget);
				BrushWidget brushWidget2 = new BrushWidget(base.Context);
				brushWidget2.WidthSizePolicy = SizePolicy.StretchToParent;
				brushWidget2.HeightSizePolicy = SizePolicy.StretchToParent;
				brushWidget2.Brush = this.ItemBackgroundBrush;
				widget.AddChild(brushWidget2);
				BrushWidget brushWidget3 = new BrushWidget(base.Context);
				brushWidget3.WidthSizePolicy = SizePolicy.Fixed;
				brushWidget3.HeightSizePolicy = SizePolicy.Fixed;
				brushWidget3.VerticalAlignment = VerticalAlignment.Bottom;
				brushWidget3.Brush = this.ItemBrush;
				brushWidget3.SuggestedWidth = 39f;
				brushWidget3.SuggestedHeight = 38f;
				brushWidget3.AddState("IncreaseAnim");
				brushWidget3.AddState("IsCriticalAnim");
				widget2.AddChild(brushWidget3);
				array[i] = new MoraleWidget.MoraleItemWidget(widget, widget2, brushWidget3, brushWidget, brushWidget2);
			}
			return array;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001D0F8 File Offset: 0x0001B2F8
		private void SetItemWidgetColors(Color color)
		{
			if (this._moraleItemWidgets != null)
			{
				foreach (MoraleWidget.MoraleItemWidget moraleItemWidget in this._moraleItemWidgets)
				{
					this.SetSingleItemWidgetColor(moraleItemWidget, color);
				}
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0001D130 File Offset: 0x0001B330
		private void SetSingleItemWidgetColor(MoraleWidget.MoraleItemWidget widget, Color color)
		{
			widget.ItemWidget.Brush.Color = color;
			foreach (Style style in widget.ItemWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = color;
				}
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0001D1B4 File Offset: 0x0001B3B4
		private void SetItemGlowWidgetColors(Color color)
		{
			if (this._moraleItemWidgets != null)
			{
				foreach (MoraleWidget.MoraleItemWidget moraleItemWidget in this._moraleItemWidgets)
				{
					this.SetSingleItemGlowWidgetColor(moraleItemWidget, color);
				}
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0001D1EC File Offset: 0x0001B3EC
		private void SetSingleItemGlowWidgetColor(MoraleWidget.MoraleItemWidget widget, Color color)
		{
			widget.ItemGlowWidget.Brush.Color = color;
			foreach (Style style in widget.ItemGlowWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = color;
				}
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0001D270 File Offset: 0x0001B470
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x0001D278 File Offset: 0x0001B478
		[DataSourceProperty]
		public int IncreaseLevel
		{
			get
			{
				return this._increaseLevel;
			}
			set
			{
				if (this._increaseLevel != value)
				{
					this._increaseLevel = value;
					base.OnPropertyChanged(value, "IncreaseLevel");
					this.UpdateArrows(this._increaseLevel);
				}
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0001D2A2 File Offset: 0x0001B4A2
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x0001D2AA File Offset: 0x0001B4AA
		[DataSourceProperty]
		public int MoralePercentage
		{
			get
			{
				return this._moralePercentage;
			}
			set
			{
				if (this._moralePercentage != value)
				{
					this._moralePercentage = value;
					base.OnPropertyChanged(value, "MoralePercentage");
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0001D2C8 File Offset: 0x0001B4C8
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0001D2D0 File Offset: 0x0001B4D0
		[DataSourceProperty]
		public Widget Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (this._container != value)
				{
					this._container = value;
					base.OnPropertyChanged<Widget>(value, "Container");
				}
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0001D2EE File Offset: 0x0001B4EE
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x0001D2F6 File Offset: 0x0001B4F6
		[DataSourceProperty]
		public Widget ItemContainer
		{
			get
			{
				return this._itemContainer;
			}
			set
			{
				if (this._itemContainer != value)
				{
					this._itemContainer = value;
					base.OnPropertyChanged<Widget>(value, "ItemContainer");
				}
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0001D314 File Offset: 0x0001B514
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0001D31C File Offset: 0x0001B51C
		[DataSourceProperty]
		public Brush ItemBrush
		{
			get
			{
				return this._itemBrush;
			}
			set
			{
				if (this._itemBrush != value)
				{
					this._itemBrush = value;
					base.OnPropertyChanged<Brush>(value, "ItemBrush");
				}
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0001D33A File Offset: 0x0001B53A
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x0001D342 File Offset: 0x0001B542
		[DataSourceProperty]
		public Brush ItemGlowBrush
		{
			get
			{
				return this._itemGlowBrush;
			}
			set
			{
				if (this._itemGlowBrush != value)
				{
					this._itemGlowBrush = value;
					base.OnPropertyChanged<Brush>(value, "ItemGlowBrush");
				}
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0001D360 File Offset: 0x0001B560
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x0001D368 File Offset: 0x0001B568
		[DataSourceProperty]
		public Brush ItemBackgroundBrush
		{
			get
			{
				return this._itemBackgroundBrush;
			}
			set
			{
				if (this._itemBackgroundBrush != value)
				{
					this._itemBackgroundBrush = value;
					base.OnPropertyChanged<Brush>(value, "ItemBackgroundBrush");
				}
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0001D386 File Offset: 0x0001B586
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0001D38E File Offset: 0x0001B58E
		[DataSourceProperty]
		public string TeamColorAsStr
		{
			get
			{
				return this._teamColorAsStr;
			}
			set
			{
				if (this._teamColorAsStr != value && value != null)
				{
					this._teamColorAsStr = value;
					base.OnPropertyChanged<string>(value, "TeamColorAsStr");
					this._teamColor = Color.ConvertStringToColor(value);
					this.SetItemWidgetColors(this._teamColor);
				}
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0001D3CC File Offset: 0x0001B5CC
		// (set) Token: 0x06000A61 RID: 2657 RVA: 0x0001D3D4 File Offset: 0x0001B5D4
		[DataSourceProperty]
		public string TeamColorAsStrSecondary
		{
			get
			{
				return this._teamColorAsStrSecondary;
			}
			set
			{
				if (this._teamColorAsStrSecondary != value && value != null)
				{
					this._teamColorAsStrSecondary = value;
					base.OnPropertyChanged<string>(value, "TeamColorAsStrSecondary");
					this._teamColorSecondary = Color.ConvertStringToColor(value);
					this.SetItemGlowWidgetColors(this._teamColorSecondary);
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0001D412 File Offset: 0x0001B612
		// (set) Token: 0x06000A63 RID: 2659 RVA: 0x0001D41A File Offset: 0x0001B61A
		[DataSourceProperty]
		public MoraleArrowBrushWidget FlowArrowWidget
		{
			get
			{
				return this._flowArrowWidget;
			}
			set
			{
				if (this._flowArrowWidget != value)
				{
					this._flowArrowWidget = value;
					base.OnPropertyChanged<MoraleArrowBrushWidget>(value, "FlowArrowWidget");
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0001D438 File Offset: 0x0001B638
		// (set) Token: 0x06000A65 RID: 2661 RVA: 0x0001D440 File Offset: 0x0001B640
		[DataSourceProperty]
		public bool ExtendToLeft
		{
			get
			{
				return this._extendToLeft;
			}
			set
			{
				if (this._extendToLeft != value)
				{
					this._extendToLeft = value;
					base.OnPropertyChanged(value, "ExtendToLeft");
				}
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0001D45E File Offset: 0x0001B65E
		// (set) Token: 0x06000A67 RID: 2663 RVA: 0x0001D466 File Offset: 0x0001B666
		[DataSourceProperty]
		public bool AreMoralesIndependent
		{
			get
			{
				return this._areMoralesIndependent;
			}
			set
			{
				if (this._areMoralesIndependent != value)
				{
					this._areMoralesIndependent = value;
					base.OnPropertyChanged(value, "AreMoralesIndependent");
				}
			}
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0001D484 File Offset: 0x0001B684
		private float PingPong(float min, float max, float time)
		{
			float num = max - min;
			bool flag = (int)(time / num) % 2 == 0;
			float num2 = time % num;
			if (!flag)
			{
				return max - num2;
			}
			return num2 + min;
		}

		// Token: 0x040004A7 RID: 1191
		private const int ItemCount = 10;

		// Token: 0x040004A8 RID: 1192
		private const float ItemDistance = 28f;

		// Token: 0x040004A9 RID: 1193
		private const int ItemWidth = 39;

		// Token: 0x040004AA RID: 1194
		private const int ItemHeight = 38;

		// Token: 0x040004AB RID: 1195
		private const int FillMargin = 12;

		// Token: 0x040004AC RID: 1196
		private MoraleWidget.MoraleItemWidget[] _moraleItemWidgets;

		// Token: 0x040004AD RID: 1197
		private bool _initialized;

		// Token: 0x040004AE RID: 1198
		private bool _triggerAnimations;

		// Token: 0x040004AF RID: 1199
		private int _animWaitFrame;

		// Token: 0x040004B0 RID: 1200
		private string _currentStateName;

		// Token: 0x040004B1 RID: 1201
		private Color _teamColor;

		// Token: 0x040004B2 RID: 1202
		private Color _teamColorSecondary;

		// Token: 0x040004B3 RID: 1203
		private int _increaseLevel;

		// Token: 0x040004B4 RID: 1204
		private int _moralePercentage;

		// Token: 0x040004B5 RID: 1205
		private Widget _container;

		// Token: 0x040004B6 RID: 1206
		private Widget _itemContainer;

		// Token: 0x040004B7 RID: 1207
		private Brush _itemBrush;

		// Token: 0x040004B8 RID: 1208
		private Brush _itemGlowBrush;

		// Token: 0x040004B9 RID: 1209
		private Brush _itemBackgroundBrush;

		// Token: 0x040004BA RID: 1210
		private MoraleArrowBrushWidget _flowArrowWidget;

		// Token: 0x040004BB RID: 1211
		private bool _extendToLeft;

		// Token: 0x040004BC RID: 1212
		private bool _areMoralesIndependent;

		// Token: 0x040004BD RID: 1213
		private string _teamColorAsStr;

		// Token: 0x040004BE RID: 1214
		private string _teamColorAsStrSecondary;

		// Token: 0x020001BD RID: 445
		private class MoraleItemWidget
		{
			// Token: 0x17000766 RID: 1894
			// (get) Token: 0x0600153C RID: 5436 RVA: 0x00039B81 File Offset: 0x00037D81
			// (set) Token: 0x0600153D RID: 5437 RVA: 0x00039B89 File Offset: 0x00037D89
			public Widget ParentWidget { get; private set; }

			// Token: 0x17000767 RID: 1895
			// (get) Token: 0x0600153E RID: 5438 RVA: 0x00039B92 File Offset: 0x00037D92
			// (set) Token: 0x0600153F RID: 5439 RVA: 0x00039B9A File Offset: 0x00037D9A
			public Widget MaskWidget { get; private set; }

			// Token: 0x17000768 RID: 1896
			// (get) Token: 0x06001540 RID: 5440 RVA: 0x00039BA3 File Offset: 0x00037DA3
			// (set) Token: 0x06001541 RID: 5441 RVA: 0x00039BAB File Offset: 0x00037DAB
			public BrushWidget ItemWidget { get; private set; }

			// Token: 0x17000769 RID: 1897
			// (get) Token: 0x06001542 RID: 5442 RVA: 0x00039BB4 File Offset: 0x00037DB4
			// (set) Token: 0x06001543 RID: 5443 RVA: 0x00039BBC File Offset: 0x00037DBC
			public BrushWidget ItemGlowWidget { get; private set; }

			// Token: 0x1700076A RID: 1898
			// (get) Token: 0x06001544 RID: 5444 RVA: 0x00039BC5 File Offset: 0x00037DC5
			// (set) Token: 0x06001545 RID: 5445 RVA: 0x00039BCD File Offset: 0x00037DCD
			public Widget ItemBackgroundWidget { get; private set; }

			// Token: 0x06001546 RID: 5446 RVA: 0x00039BD6 File Offset: 0x00037DD6
			public MoraleItemWidget(Widget parentWidget, Widget maskWidget, BrushWidget itemWidget, BrushWidget itemGlowWidget, Widget itemBackgroundWidget)
			{
				this.ParentWidget = parentWidget;
				this.MaskWidget = maskWidget;
				this.ItemWidget = itemWidget;
				this.ItemGlowWidget = itemGlowWidget;
				this.ItemBackgroundWidget = itemBackgroundWidget;
			}

			// Token: 0x06001547 RID: 5447 RVA: 0x00039C04 File Offset: 0x00037E04
			public void SetFillAmount(float fill, int fillMargin)
			{
				bool flag = MBMath.ApproximatelyEquals(fill, 0f, 1E-05f);
				bool flag2 = MBMath.ApproximatelyEquals(fill, 1f, 1E-05f);
				if (flag)
				{
					this.MaskWidget.SuggestedHeight = 0f;
				}
				else if (flag2)
				{
					this.MaskWidget.SuggestedHeight = this.ItemWidget.SuggestedHeight;
				}
				else
				{
					int num = MathF.Floor(this.ItemWidget.SuggestedHeight - (float)(fillMargin * 2));
					this.MaskWidget.SuggestedHeight = (float)fillMargin + (float)num * fill;
				}
				this.ItemWidget.IsVisible = !flag;
				this.ItemGlowWidget.IsVisible = flag2;
			}
		}
	}
}
