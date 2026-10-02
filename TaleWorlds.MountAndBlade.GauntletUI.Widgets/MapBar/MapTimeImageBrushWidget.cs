using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.MapBar
{
	// Token: 0x02000116 RID: 278
	public class MapTimeImageBrushWidget : BrushWidget
	{
		// Token: 0x06000EC9 RID: 3785 RVA: 0x00028AAF File Offset: 0x00026CAF
		public MapTimeImageBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00028AB8 File Offset: 0x00026CB8
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			StyleLayer layer = base.Brush.DefaultStyle.GetLayer("Default");
			StyleLayer layer2 = base.Brush.DefaultStyle.GetLayer("Part2");
			if (!this._initialized)
			{
				this._offset = layer2.XOffset;
				this._initialized = true;
			}
			float overridenWidth = layer.OverridenWidth;
			float num = -overridenWidth * ((float)this.DayTime / 24f) + this._offset;
			float num2;
			if (this.DayTime > 12.0)
			{
				num2 = num + overridenWidth;
			}
			else
			{
				num2 = num - overridenWidth;
			}
			layer.XOffset = num;
			layer2.XOffset = num2;
			base.OnRender(twoDimensionContext, drawContext);
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00028B60 File Offset: 0x00026D60
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x00028B68 File Offset: 0x00026D68
		[Editor(false)]
		public double DayTime
		{
			get
			{
				return this._dayTime;
			}
			set
			{
				if (this._dayTime != value)
				{
					this._dayTime = value;
					base.OnPropertyChanged(value, "DayTime");
				}
			}
		}

		// Token: 0x040006BA RID: 1722
		private float _offset;

		// Token: 0x040006BB RID: 1723
		private bool _initialized;

		// Token: 0x040006BC RID: 1724
		private double _dayTime;
	}
}
