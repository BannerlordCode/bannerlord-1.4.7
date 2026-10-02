using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000085 RID: 133
	public class MaterialValueOffsetImageWidget : ImageWidget
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x00015E6C File Offset: 0x0001406C
		public MaterialValueOffsetImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00015E78 File Offset: 0x00014078
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._visualDirty)
			{
				foreach (Style style in base.Brush.Styles)
				{
					foreach (StyleLayer styleLayer in style.GetLayers())
					{
						styleLayer.ValueFactor += this.ValueOffset;
						styleLayer.SaturationFactor += this.SaturationOffset;
						styleLayer.HueFactor += this.HueOffset;
					}
				}
				this._visualDirty = false;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00015F30 File Offset: 0x00014130
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x00015F38 File Offset: 0x00014138
		public float ValueOffset
		{
			get
			{
				return this._valueOffset;
			}
			set
			{
				this._valueOffset = value;
				this._visualDirty = true;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00015F48 File Offset: 0x00014148
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x00015F50 File Offset: 0x00014150
		public float SaturationOffset
		{
			get
			{
				return this._saturationOffset;
			}
			set
			{
				this._saturationOffset = value;
				this._visualDirty = true;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00015F60 File Offset: 0x00014160
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x00015F68 File Offset: 0x00014168
		public float HueOffset
		{
			get
			{
				return this._hueOffset;
			}
			set
			{
				this._hueOffset = value;
				this._visualDirty = true;
			}
		}

		// Token: 0x04000346 RID: 838
		private bool _visualDirty;

		// Token: 0x04000347 RID: 839
		private float _valueOffset;

		// Token: 0x04000348 RID: 840
		private float _saturationOffset;

		// Token: 0x04000349 RID: 841
		private float _hueOffset;
	}
}
