using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000086 RID: 134
	public class MaterialValueOffsetTextWidget : TextWidget
	{
		// Token: 0x06000785 RID: 1925 RVA: 0x00015F78 File Offset: 0x00014178
		public MaterialValueOffsetTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00015F84 File Offset: 0x00014184
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._visualDirty)
			{
				base.Brush.TextValueFactor += this.ValueOffset;
				base.Brush.TextSaturationFactor += this.SaturationOffset;
				base.Brush.TextHueFactor += this.HueOffset;
				foreach (Style style in base.Brush.Styles)
				{
					style.TextValueFactor += this.ValueOffset;
					style.TextSaturationFactor += this.SaturationOffset;
					style.TextHueFactor += this.HueOffset;
				}
				this._visualDirty = false;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x00016070 File Offset: 0x00014270
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x00016078 File Offset: 0x00014278
		public float ValueOffset
		{
			get
			{
				return this._valueOffset;
			}
			set
			{
				if (this._valueOffset != value)
				{
					this._valueOffset = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00016091 File Offset: 0x00014291
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x00016099 File Offset: 0x00014299
		public float SaturationOffset
		{
			get
			{
				return this._saturationOffset;
			}
			set
			{
				if (this._saturationOffset != value)
				{
					this._saturationOffset = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x000160B2 File Offset: 0x000142B2
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x000160BA File Offset: 0x000142BA
		public float HueOffset
		{
			get
			{
				return this._hueOffset;
			}
			set
			{
				if (this._hueOffset != value)
				{
					this._hueOffset = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x0400034A RID: 842
		private bool _visualDirty;

		// Token: 0x0400034B RID: 843
		private float _valueOffset;

		// Token: 0x0400034C RID: 844
		private float _saturationOffset;

		// Token: 0x0400034D RID: 845
		private float _hueOffset;
	}
}
