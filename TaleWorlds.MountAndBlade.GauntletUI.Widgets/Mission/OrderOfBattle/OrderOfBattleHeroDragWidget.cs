using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000F0 RID: 240
	public class OrderOfBattleHeroDragWidget : Widget
	{
		// Token: 0x06000C52 RID: 3154 RVA: 0x000219AA File Offset: 0x0001FBAA
		public OrderOfBattleHeroDragWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000219B4 File Offset: 0x0001FBB4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!base.IsVisible)
			{
				return;
			}
			if (this._isDirty && this.StackDragWidget != null)
			{
				base.RemoveAllChildren();
				for (int i = 0; i < this.StackCount; i++)
				{
					BrushWidget brushWidget = new BrushWidget(base.Context)
					{
						Brush = this.StackDragWidget.ReadOnlyBrush,
						DoNotAcceptEvents = false,
						SuggestedHeight = this.StackDragWidget.SuggestedHeight,
						SuggestedWidth = this.StackDragWidget.SuggestedWidth,
						ScaledPositionXOffset = (float)(i * 5),
						ScaledPositionYOffset = (float)(i * 5)
					};
					if (i == this.StackCount - 1)
					{
						BrushWidget brushWidget2 = new BrushWidget(brushWidget.Context)
						{
							Brush = base.Context.GetBrush(this.InnerBrushName),
							WidthSizePolicy = SizePolicy.StretchToParent,
							HeightSizePolicy = SizePolicy.StretchToParent,
							MarginBottom = 5f,
							MarginTop = 5f,
							MarginLeft = 5f,
							MarginRight = 5f,
							HorizontalAlignment = HorizontalAlignment.Center,
							VerticalAlignment = VerticalAlignment.Center
						};
						ImageIdentifierWidget imageIdentifierWidget = new ImageIdentifierWidget(brushWidget.Context)
						{
							WidthSizePolicy = SizePolicy.Fixed,
							HeightSizePolicy = SizePolicy.Fixed,
							SuggestedWidth = this.StackThumbnailWidget.SuggestedWidth,
							SuggestedHeight = this.StackThumbnailWidget.SuggestedHeight,
							MarginTop = this.StackThumbnailWidget.MarginTop,
							MarginLeft = this.StackThumbnailWidget.MarginLeft,
							AdditionalArgs = this.StackThumbnailWidget.AdditionalArgs,
							ImageId = this.StackThumbnailWidget.ImageId,
							TextureProviderName = this.StackThumbnailWidget.TextureProviderName
						};
						brushWidget.AddChild(brushWidget2);
						brushWidget.AddChild(imageIdentifierWidget);
					}
					base.AddChild(brushWidget);
				}
				this._isDirty = false;
			}
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00021B84 File Offset: 0x0001FD84
		private void OnStackCountChanged()
		{
			this._isDirty = true;
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x00021B8D File Offset: 0x0001FD8D
		// (set) Token: 0x06000C56 RID: 3158 RVA: 0x00021B95 File Offset: 0x0001FD95
		[Editor(false)]
		public int StackCount
		{
			get
			{
				return this._stackCount;
			}
			set
			{
				if (value != this._stackCount)
				{
					this._stackCount = value;
					base.OnPropertyChanged(value, "StackCount");
					this.OnStackCountChanged();
				}
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00021BB9 File Offset: 0x0001FDB9
		// (set) Token: 0x06000C58 RID: 3160 RVA: 0x00021BC1 File Offset: 0x0001FDC1
		[Editor(false)]
		public BrushWidget StackDragWidget
		{
			get
			{
				return this._stackDragWidget;
			}
			set
			{
				if (value != this._stackDragWidget)
				{
					this._stackDragWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "StackDragWidget");
				}
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00021BDF File Offset: 0x0001FDDF
		// (set) Token: 0x06000C5A RID: 3162 RVA: 0x00021BE7 File Offset: 0x0001FDE7
		[Editor(false)]
		public ImageIdentifierWidget StackThumbnailWidget
		{
			get
			{
				return this._stackThumbnailWidget;
			}
			set
			{
				if (value != this._stackThumbnailWidget)
				{
					this._stackThumbnailWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "StackThumbnailWidget");
				}
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x00021C05 File Offset: 0x0001FE05
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x00021C0D File Offset: 0x0001FE0D
		[Editor(false)]
		public string InnerBrushName
		{
			get
			{
				return this._innerBrushName;
			}
			set
			{
				if (value != this._innerBrushName)
				{
					this._innerBrushName = value;
					base.OnPropertyChanged<string>(value, "InnerBrushName");
				}
			}
		}

		// Token: 0x0400058E RID: 1422
		private bool _isDirty;

		// Token: 0x0400058F RID: 1423
		private int _stackCount;

		// Token: 0x04000590 RID: 1424
		private BrushWidget _stackDragWidget;

		// Token: 0x04000591 RID: 1425
		private ImageIdentifierWidget _stackThumbnailWidget;

		// Token: 0x04000592 RID: 1426
		private string _innerBrushName;
	}
}
