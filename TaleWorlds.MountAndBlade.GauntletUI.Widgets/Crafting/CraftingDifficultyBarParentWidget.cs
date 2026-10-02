using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016B RID: 363
	public class CraftingDifficultyBarParentWidget : Widget
	{
		// Token: 0x06001323 RID: 4899 RVA: 0x00034225 File Offset: 0x00032425
		public CraftingDifficultyBarParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00034239 File Offset: 0x00032439
		private void OnWidgetPositionUpdated(PropertyOwnerObject ownerObject, string propertyName, object value)
		{
			if (propertyName == "Text")
			{
				this._areOffsetsDirty = true;
			}
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00034250 File Offset: 0x00032450
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.SmithingLevelTextWidget != null && this.OrderDifficultyTextWidget != null)
			{
				if (this._updatePositions)
				{
					TextWidget textWidget = ((this.OrderDifficulty < this.SmithingLevel) ? this.SmithingLevelTextWidget : this.OrderDifficultyTextWidget);
					TextWidget textWidget2 = ((textWidget == this.SmithingLevelTextWidget) ? this.OrderDifficultyTextWidget : this.SmithingLevelTextWidget);
					if (textWidget.GlobalPosition.Y + (textWidget.Size.Y + this._offsetIntolerance) >= textWidget2.GlobalPosition.Y)
					{
						textWidget.PositionYOffset = -textWidget.Size.Y;
						textWidget2.PositionYOffset = 0f;
					}
					else
					{
						textWidget.PositionYOffset = 0f;
						textWidget2.PositionYOffset = 0f;
					}
					this._updatePositions = false;
				}
				if (this._areOffsetsDirty)
				{
					this.SmithingLevelTextWidget.PositionYOffset = 0f;
					this.OrderDifficultyTextWidget.PositionYOffset = 0f;
					this._updatePositions = true;
					this._areOffsetsDirty = false;
				}
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x00034357 File Offset: 0x00032557
		// (set) Token: 0x06001327 RID: 4903 RVA: 0x0003435F File Offset: 0x0003255F
		public int OrderDifficulty { get; set; }

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x00034368 File Offset: 0x00032568
		// (set) Token: 0x06001329 RID: 4905 RVA: 0x00034370 File Offset: 0x00032570
		public int SmithingLevel { get; set; }

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x00034379 File Offset: 0x00032579
		// (set) Token: 0x0600132B RID: 4907 RVA: 0x00034381 File Offset: 0x00032581
		public TextWidget SmithingLevelTextWidget
		{
			get
			{
				return this._smithingLevelTextWidget;
			}
			set
			{
				if (value != this._smithingLevelTextWidget)
				{
					this._smithingLevelTextWidget = value;
					this._smithingLevelTextWidget.PropertyChanged += this.OnWidgetPositionUpdated;
				}
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x000343AA File Offset: 0x000325AA
		// (set) Token: 0x0600132D RID: 4909 RVA: 0x000343B2 File Offset: 0x000325B2
		public TextWidget OrderDifficultyTextWidget
		{
			get
			{
				return this._orderDifficultyTextWidget;
			}
			set
			{
				if (value != this._orderDifficultyTextWidget)
				{
					this._orderDifficultyTextWidget = value;
					this._orderDifficultyTextWidget.PropertyChanged += this.OnWidgetPositionUpdated;
				}
			}
		}

		// Token: 0x040008AD RID: 2221
		private float _offsetIntolerance = 3f;

		// Token: 0x040008AE RID: 2222
		private bool _areOffsetsDirty;

		// Token: 0x040008AF RID: 2223
		private bool _updatePositions;

		// Token: 0x040008B2 RID: 2226
		private TextWidget _smithingLevelTextWidget;

		// Token: 0x040008B3 RID: 2227
		private TextWidget _orderDifficultyTextWidget;
	}
}
