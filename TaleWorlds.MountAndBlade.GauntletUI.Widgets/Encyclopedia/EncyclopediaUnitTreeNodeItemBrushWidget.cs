using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015D RID: 349
	public class EncyclopediaUnitTreeNodeItemBrushWidget : BrushWidget
	{
		// Token: 0x0600127A RID: 4730 RVA: 0x00032D56 File Offset: 0x00030F56
		public EncyclopediaUnitTreeNodeItemBrushWidget(UIContext context)
			: base(context)
		{
			this._listItemAddedHandler = new Action<Widget, Widget>(this.OnListItemAdded);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00032D74 File Offset: 0x00030F74
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isLinesDirty)
			{
				if (this.ChildContainer.ChildCount == this.LineContainer.ChildCount)
				{
					float num = base.GlobalPosition.X + base.Size.X * 0.5f;
					for (int i = 0; i < this.ChildContainer.ChildCount; i++)
					{
						Widget child = this.ChildContainer.GetChild(i);
						Widget child2 = this.LineContainer.GetChild(i);
						float num2 = child.GlobalPosition.X + child.Size.X * 0.5f;
						bool flag = num > num2;
						child2.SetState(flag ? "Left" : "Right");
						float num3 = MathF.Abs(num - num2);
						child2.ScaledSuggestedWidth = num3;
						child2.ScaledPositionXOffset = (num3 * 0.5f + 5f * base._scaleToUse) * (float)(flag ? (-1) : 1);
					}
				}
				this._isLinesDirty = false;
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00032E80 File Offset: 0x00031080
		public void OnListItemAdded(Widget parentWidget, Widget addedWidget)
		{
			Widget widget = this.CreateLineWidget();
			if (this.ChildContainer.ChildCount == 1)
			{
				widget.SetState("Straight");
				return;
			}
			this._isLinesDirty = true;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00032EB8 File Offset: 0x000310B8
		private Widget CreateLineWidget()
		{
			BrushWidget brushWidget = new BrushWidget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.StretchToParent,
				Brush = this.LineBrush
			};
			brushWidget.SuggestedWidth = (float)brushWidget.ReadOnlyBrush.Sprite.Width;
			brushWidget.SuggestedHeight = (float)brushWidget.ReadOnlyBrush.Sprite.Height;
			brushWidget.HorizontalAlignment = HorizontalAlignment.Center;
			brushWidget.AddState("Left");
			brushWidget.AddState("Right");
			brushWidget.AddState("Straight");
			this.LineContainer.AddChild(brushWidget);
			return brushWidget;
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x00032F4E File Offset: 0x0003114E
		// (set) Token: 0x0600127F RID: 4735 RVA: 0x00032F56 File Offset: 0x00031156
		[Editor(false)]
		public bool IsAlternativeUpgrade
		{
			get
			{
				return this._isAlternativeUpgrade;
			}
			set
			{
				if (value != this._isAlternativeUpgrade)
				{
					this._isAlternativeUpgrade = value;
					base.OnPropertyChanged(value, "IsAlternativeUpgrade");
				}
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x00032F74 File Offset: 0x00031174
		// (set) Token: 0x06001281 RID: 4737 RVA: 0x00032F7C File Offset: 0x0003117C
		[Editor(false)]
		public ListPanel ChildContainer
		{
			get
			{
				return this._childContainer;
			}
			set
			{
				if (this._childContainer != value)
				{
					ListPanel childContainer = this._childContainer;
					if (childContainer != null)
					{
						childContainer.ItemAddEventHandlers.Remove(this._listItemAddedHandler);
					}
					this._childContainer = value;
					base.OnPropertyChanged<ListPanel>(value, "ChildContainer");
					ListPanel childContainer2 = this._childContainer;
					if (childContainer2 == null)
					{
						return;
					}
					childContainer2.ItemAddEventHandlers.Add(this._listItemAddedHandler);
				}
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x00032FDD File Offset: 0x000311DD
		// (set) Token: 0x06001283 RID: 4739 RVA: 0x00032FE5 File Offset: 0x000311E5
		[Editor(false)]
		public Widget LineContainer
		{
			get
			{
				return this._lineContainer;
			}
			set
			{
				if (this._lineContainer != value)
				{
					this._lineContainer = value;
					base.OnPropertyChanged<Widget>(value, "LineContainer");
				}
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x00033003 File Offset: 0x00031203
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0003300B File Offset: 0x0003120B
		[Editor(false)]
		public Brush LineBrush
		{
			get
			{
				return this._lineBrush;
			}
			set
			{
				if (this._lineBrush != value)
				{
					this._lineBrush = value;
					base.OnPropertyChanged<Brush>(value, "LineBrush");
				}
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x00033029 File Offset: 0x00031229
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x00033031 File Offset: 0x00031231
		[Editor(false)]
		public Brush AlternateLineBrush
		{
			get
			{
				return this._alternateLineBrush;
			}
			set
			{
				if (this._alternateLineBrush != value)
				{
					this._alternateLineBrush = value;
					base.OnPropertyChanged<Brush>(value, "AlternateLineBrush");
				}
			}
		}

		// Token: 0x04000863 RID: 2147
		private Action<Widget, Widget> _listItemAddedHandler;

		// Token: 0x04000864 RID: 2148
		private bool _isLinesDirty;

		// Token: 0x04000865 RID: 2149
		private bool _isAlternativeUpgrade;

		// Token: 0x04000866 RID: 2150
		private ListPanel _childContainer;

		// Token: 0x04000867 RID: 2151
		private Widget _lineContainer;

		// Token: 0x04000868 RID: 2152
		private Brush _lineBrush;

		// Token: 0x04000869 RID: 2153
		private Brush _alternateLineBrush;
	}
}
