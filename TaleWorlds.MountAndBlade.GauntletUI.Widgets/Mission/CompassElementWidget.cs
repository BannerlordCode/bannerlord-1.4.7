using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D8 RID: 216
	public class CompassElementWidget : Widget
	{
		// Token: 0x06000AFE RID: 2814 RVA: 0x0001ED88 File Offset: 0x0001CF88
		public CompassElementWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0001ED9C File Offset: 0x0001CF9C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.HandleDistanceFading(dt);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0001EDAC File Offset: 0x0001CFAC
		private void HandleDistanceFading(float dt)
		{
			if (this.Distance < 10)
			{
				this._alpha -= 2f * dt;
			}
			else
			{
				this._alpha += 2f * dt;
			}
			this._alpha = MBMath.ClampFloat(this._alpha, 0f, 1f);
			if (this.BannerWidget != null)
			{
				int childCount = this.BannerWidget.ChildCount;
				for (int i = 0; i < childCount; i++)
				{
					Widget child = this.FlagWidget.GetChild(i);
					Color color = child.Color;
					color.Alpha = this._alpha;
					child.Color = color;
				}
			}
			if (this.FlagWidget != null)
			{
				int childCount2 = this.FlagWidget.ChildCount;
				for (int j = 0; j < childCount2; j++)
				{
					Widget child2 = this.FlagWidget.GetChild(j);
					Color color2 = child2.Color;
					color2.Alpha = this._alpha;
					child2.Color = color2;
				}
			}
			base.IsVisible = this._alpha > 1E-05f;
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0001EEAE File Offset: 0x0001D0AE
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x0001EEB6 File Offset: 0x0001D0B6
		[DataSourceProperty]
		public float Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (Math.Abs(this._position - value) > 1E-45f)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0001EEDF File Offset: 0x0001D0DF
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x0001EEE7 File Offset: 0x0001D0E7
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (this._distance != value)
				{
					this._distance = value;
					base.OnPropertyChanged(value, "Distance");
				}
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0001EF05 File Offset: 0x0001D105
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0001EF0D File Offset: 0x0001D10D
		[DataSourceProperty]
		public Widget BannerWidget
		{
			get
			{
				return this._bannerWidget;
			}
			set
			{
				if (this._bannerWidget != value)
				{
					this._bannerWidget = value;
					base.OnPropertyChanged<Widget>(value, "BannerWidget");
				}
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0001EF2B File Offset: 0x0001D12B
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x0001EF33 File Offset: 0x0001D133
		[DataSourceProperty]
		public Widget FlagWidget
		{
			get
			{
				return this._flagWidget;
			}
			set
			{
				if (this._flagWidget != value)
				{
					this._flagWidget = value;
					base.OnPropertyChanged<Widget>(value, "FlagWidget");
				}
			}
		}

		// Token: 0x040004FC RID: 1276
		private float _alpha = 1f;

		// Token: 0x040004FD RID: 1277
		private float _position;

		// Token: 0x040004FE RID: 1278
		private int _distance;

		// Token: 0x040004FF RID: 1279
		private Widget _bannerWidget;

		// Token: 0x04000500 RID: 1280
		private Widget _flagWidget;
	}
}
