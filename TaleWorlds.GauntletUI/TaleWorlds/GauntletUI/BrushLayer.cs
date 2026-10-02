using System;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000012 RID: 18
	public class BrushLayer : IBrushLayerData
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000066CD File Offset: 0x000048CD
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000066D5 File Offset: 0x000048D5
		public uint Version { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000066DE File Offset: 0x000048DE
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000066E8 File Offset: 0x000048E8
		[Editor(false)]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000671A File Offset: 0x0000491A
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00006724 File Offset: 0x00004924
		[Editor(false)]
		public Sprite Sprite
		{
			get
			{
				return this._sprite;
			}
			set
			{
				if (value != this._sprite)
				{
					this._sprite = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00006751 File Offset: 0x00004951
		// (set) Token: 0x06000105 RID: 261 RVA: 0x0000675C File Offset: 0x0000495C
		[Editor(false)]
		public ImageFit.ImageFitTypes ImageFitType
		{
			get
			{
				return this._imageFitType;
			}
			set
			{
				if (value != this._imageFitType)
				{
					this._imageFitType = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00006789 File Offset: 0x00004989
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00006794 File Offset: 0x00004994
		[Editor(false)]
		public ImageFit.ImageHorizontalAlignments ImageFitHorizontalAlignment
		{
			get
			{
				return this._imageFitHorizontalAlignment;
			}
			set
			{
				if (value != this._imageFitHorizontalAlignment)
				{
					this._imageFitHorizontalAlignment = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000067C1 File Offset: 0x000049C1
		// (set) Token: 0x06000109 RID: 265 RVA: 0x000067CC File Offset: 0x000049CC
		[Editor(false)]
		public ImageFit.ImageVerticalAlignments ImageFitVerticalAlignment
		{
			get
			{
				return this._imageFitVerticalAlignment;
			}
			set
			{
				if (value != this._imageFitVerticalAlignment)
				{
					this._imageFitVerticalAlignment = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000067F9 File Offset: 0x000049F9
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00006804 File Offset: 0x00004A04
		[Editor(false)]
		public Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (value != this._color)
				{
					this._color = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00006836 File Offset: 0x00004A36
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00006840 File Offset: 0x00004A40
		[Editor(false)]
		public float ColorFactor
		{
			get
			{
				return this._colorFactor;
			}
			set
			{
				if (value != this._colorFactor)
				{
					this._colorFactor = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000686D File Offset: 0x00004A6D
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00006878 File Offset: 0x00004A78
		[Editor(false)]
		public float AlphaFactor
		{
			get
			{
				return this._alphaFactor;
			}
			set
			{
				if (value != this._alphaFactor)
				{
					this._alphaFactor = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000068A5 File Offset: 0x00004AA5
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000068B0 File Offset: 0x00004AB0
		[Editor(false)]
		public float HueFactor
		{
			get
			{
				return this._hueFactor;
			}
			set
			{
				if (value != this._hueFactor)
				{
					this._hueFactor = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000068DD File Offset: 0x00004ADD
		// (set) Token: 0x06000113 RID: 275 RVA: 0x000068E8 File Offset: 0x00004AE8
		[Editor(false)]
		public float SaturationFactor
		{
			get
			{
				return this._saturationFactor;
			}
			set
			{
				if (value != this._saturationFactor)
				{
					this._saturationFactor = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00006915 File Offset: 0x00004B15
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00006920 File Offset: 0x00004B20
		[Editor(false)]
		public float ValueFactor
		{
			get
			{
				return this._valueFactor;
			}
			set
			{
				if (value != this._valueFactor)
				{
					this._valueFactor = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000694D File Offset: 0x00004B4D
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00006958 File Offset: 0x00004B58
		[Editor(false)]
		public bool IsHidden
		{
			get
			{
				return this._isHidden;
			}
			set
			{
				if (value != this._isHidden)
				{
					this._isHidden = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00006985 File Offset: 0x00004B85
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00006990 File Offset: 0x00004B90
		[Editor(false)]
		public bool UseOverlayAlphaAsMask
		{
			get
			{
				return this._useOverlayAlphaAsMask;
			}
			set
			{
				if (value != this._useOverlayAlphaAsMask)
				{
					this._useOverlayAlphaAsMask = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000069BD File Offset: 0x00004BBD
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000069C8 File Offset: 0x00004BC8
		[Editor(false)]
		public float XOffset
		{
			get
			{
				return this._xOffset;
			}
			set
			{
				if (value != this._xOffset)
				{
					this._xOffset = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000069F5 File Offset: 0x00004BF5
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00006A00 File Offset: 0x00004C00
		[Editor(false)]
		public float YOffset
		{
			get
			{
				return this._yOffset;
			}
			set
			{
				if (value != this._yOffset)
				{
					this._yOffset = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006A2D File Offset: 0x00004C2D
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00006A38 File Offset: 0x00004C38
		[Editor(false)]
		public float Rotation
		{
			get
			{
				return this._rotation;
			}
			set
			{
				if (value != this._rotation)
				{
					this._rotation = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00006A65 File Offset: 0x00004C65
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00006A70 File Offset: 0x00004C70
		[Editor(false)]
		public float ExtendLeft
		{
			get
			{
				return this._extendLeft;
			}
			set
			{
				if (value != this._extendLeft)
				{
					this._extendLeft = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006A9D File Offset: 0x00004C9D
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00006AA8 File Offset: 0x00004CA8
		[Editor(false)]
		public float ExtendRight
		{
			get
			{
				return this._extendRight;
			}
			set
			{
				if (value != this._extendRight)
				{
					this._extendRight = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006AD5 File Offset: 0x00004CD5
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00006AE0 File Offset: 0x00004CE0
		[Editor(false)]
		public float ExtendTop
		{
			get
			{
				return this._extendTop;
			}
			set
			{
				if (value != this._extendTop)
				{
					this._extendTop = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00006B0D File Offset: 0x00004D0D
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00006B18 File Offset: 0x00004D18
		[Editor(false)]
		public float ExtendBottom
		{
			get
			{
				return this._extendBottom;
			}
			set
			{
				if (value != this._extendBottom)
				{
					this._extendBottom = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00006B45 File Offset: 0x00004D45
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00006B50 File Offset: 0x00004D50
		[Editor(false)]
		public float OverridenWidth
		{
			get
			{
				return this._overridenWidth;
			}
			set
			{
				if (value != this._overridenWidth)
				{
					this._overridenWidth = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00006B7D File Offset: 0x00004D7D
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00006B88 File Offset: 0x00004D88
		[Editor(false)]
		public float OverridenHeight
		{
			get
			{
				return this._overridenHeight;
			}
			set
			{
				if (value != this._overridenHeight)
				{
					this._overridenHeight = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006BB5 File Offset: 0x00004DB5
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00006BC0 File Offset: 0x00004DC0
		[Editor(false)]
		public BrushLayerSizePolicy WidthPolicy
		{
			get
			{
				return this._widthPolicy;
			}
			set
			{
				if (value != this._widthPolicy)
				{
					this._widthPolicy = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006BED File Offset: 0x00004DED
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00006BF8 File Offset: 0x00004DF8
		[Editor(false)]
		public BrushLayerSizePolicy HeightPolicy
		{
			get
			{
				return this._heightPolicy;
			}
			set
			{
				if (value != this._heightPolicy)
				{
					this._heightPolicy = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006C25 File Offset: 0x00004E25
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00006C30 File Offset: 0x00004E30
		[Editor(false)]
		public bool HorizontalFlip
		{
			get
			{
				return this._horizontalFlip;
			}
			set
			{
				if (value != this._horizontalFlip)
				{
					this._horizontalFlip = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00006C5D File Offset: 0x00004E5D
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00006C68 File Offset: 0x00004E68
		[Editor(false)]
		public bool VerticalFlip
		{
			get
			{
				return this._verticalFlip;
			}
			set
			{
				if (value != this._verticalFlip)
				{
					this._verticalFlip = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00006C95 File Offset: 0x00004E95
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00006CA0 File Offset: 0x00004EA0
		[Editor(false)]
		public BrushOverlayMethod OverlayMethod
		{
			get
			{
				return this._overlayMethod;
			}
			set
			{
				if (value != this._overlayMethod)
				{
					this._overlayMethod = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00006CCD File Offset: 0x00004ECD
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00006CD8 File Offset: 0x00004ED8
		[Editor(false)]
		public Sprite OverlaySprite
		{
			get
			{
				return this._overlaySprite;
			}
			set
			{
				this._overlaySprite = value;
				uint version = this.Version;
				this.Version = version + 1U;
				if (this._overlaySprite != null)
				{
					if (this.OverlayMethod == BrushOverlayMethod.None)
					{
						this.OverlayMethod = BrushOverlayMethod.CoverWithTexture;
						return;
					}
				}
				else
				{
					this.OverlayMethod = BrushOverlayMethod.None;
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00006D1B File Offset: 0x00004F1B
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00006D24 File Offset: 0x00004F24
		[Editor(false)]
		public float OverlayXOffset
		{
			get
			{
				return this._overlayXOffset;
			}
			set
			{
				if (value != this._overlayXOffset)
				{
					this._overlayXOffset = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006D51 File Offset: 0x00004F51
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00006D5C File Offset: 0x00004F5C
		[Editor(false)]
		public float OverlayYOffset
		{
			get
			{
				return this._overlayYOffset;
			}
			set
			{
				if (value != this._overlayYOffset)
				{
					this._overlayYOffset = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006D89 File Offset: 0x00004F89
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00006D94 File Offset: 0x00004F94
		[Editor(false)]
		public bool UseRandomBaseOverlayXOffset
		{
			get
			{
				return this._useRandomBaseOverlayXOffset;
			}
			set
			{
				if (value != this._useRandomBaseOverlayXOffset)
				{
					this._useRandomBaseOverlayXOffset = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00006DC1 File Offset: 0x00004FC1
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00006DCC File Offset: 0x00004FCC
		[Editor(false)]
		public bool UseRandomBaseOverlayYOffset
		{
			get
			{
				return this._useRandomBaseOverlayYOffset;
			}
			set
			{
				if (value != this._useRandomBaseOverlayYOffset)
				{
					this._useRandomBaseOverlayYOffset = value;
					uint version = this.Version;
					this.Version = version + 1U;
				}
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006DFC File Offset: 0x00004FFC
		public BrushLayer()
		{
			this.Color = new Color(1f, 1f, 1f, 1f);
			this.ColorFactor = 1f;
			this.AlphaFactor = 1f;
			this.HueFactor = 0f;
			this.SaturationFactor = 0f;
			this.ValueFactor = 0f;
			this.XOffset = 0f;
			this.YOffset = 0f;
			this.Rotation = 0f;
			this.IsHidden = false;
			this.WidthPolicy = BrushLayerSizePolicy.StretchToTarget;
			this.HeightPolicy = BrushLayerSizePolicy.StretchToTarget;
			this.HorizontalFlip = false;
			this.VerticalFlip = false;
			this.OverlayMethod = BrushOverlayMethod.None;
			this.ExtendLeft = 0f;
			this.ExtendRight = 0f;
			this.ExtendTop = 0f;
			this.ExtendBottom = 0f;
			this.OverlayXOffset = 0f;
			this.OverlayYOffset = 0f;
			this.UseRandomBaseOverlayXOffset = false;
			this.UseRandomBaseOverlayYOffset = false;
			this.UseOverlayAlphaAsMask = false;
			this.ImageFitType = ImageFit.ImageFitTypes.StretchToFit;
			this.ImageFitHorizontalAlignment = ImageFit.ImageHorizontalAlignments.Center;
			this.ImageFitVerticalAlignment = ImageFit.ImageVerticalAlignments.Center;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006F1C File Offset: 0x0000511C
		public void FillFrom(BrushLayer brushLayer)
		{
			this.Sprite = brushLayer.Sprite;
			this.Color = brushLayer.Color;
			this.ColorFactor = brushLayer.ColorFactor;
			this.AlphaFactor = brushLayer.AlphaFactor;
			this.HueFactor = brushLayer.HueFactor;
			this.SaturationFactor = brushLayer.SaturationFactor;
			this.ValueFactor = brushLayer.ValueFactor;
			this.XOffset = brushLayer.XOffset;
			this.YOffset = brushLayer.YOffset;
			this.Rotation = brushLayer.Rotation;
			this.Name = brushLayer.Name;
			this.IsHidden = brushLayer.IsHidden;
			this.WidthPolicy = brushLayer.WidthPolicy;
			this.HeightPolicy = brushLayer.HeightPolicy;
			this.OverridenWidth = brushLayer.OverridenWidth;
			this.OverridenHeight = brushLayer.OverridenHeight;
			this.HorizontalFlip = brushLayer.HorizontalFlip;
			this.VerticalFlip = brushLayer.VerticalFlip;
			this.OverlayMethod = brushLayer.OverlayMethod;
			this.OverlaySprite = brushLayer.OverlaySprite;
			this.ExtendLeft = brushLayer.ExtendLeft;
			this.ExtendRight = brushLayer.ExtendRight;
			this.ExtendTop = brushLayer.ExtendTop;
			this.ExtendBottom = brushLayer.ExtendBottom;
			this.OverlayXOffset = brushLayer.OverlayXOffset;
			this.OverlayYOffset = brushLayer.OverlayYOffset;
			this.UseRandomBaseOverlayXOffset = brushLayer.UseRandomBaseOverlayXOffset;
			this.UseRandomBaseOverlayYOffset = brushLayer.UseRandomBaseOverlayYOffset;
			this.UseOverlayAlphaAsMask = brushLayer.UseOverlayAlphaAsMask;
			this.ImageFitType = brushLayer.ImageFitType;
			this.ImageFitHorizontalAlignment = brushLayer.ImageFitHorizontalAlignment;
			this.ImageFitVerticalAlignment = brushLayer.ImageFitVerticalAlignment;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000070AC File Offset: 0x000052AC
		public float GetValueAsFloat(BrushAnimationProperty.BrushAnimationPropertyType propertyType)
		{
			switch (propertyType)
			{
			case BrushAnimationProperty.BrushAnimationPropertyType.ColorFactor:
				return this.ColorFactor;
			case BrushAnimationProperty.BrushAnimationPropertyType.Color:
			case BrushAnimationProperty.BrushAnimationPropertyType.FontColor:
				break;
			case BrushAnimationProperty.BrushAnimationPropertyType.AlphaFactor:
				return this.AlphaFactor;
			case BrushAnimationProperty.BrushAnimationPropertyType.HueFactor:
				return this.HueFactor;
			case BrushAnimationProperty.BrushAnimationPropertyType.SaturationFactor:
				return this.SaturationFactor;
			case BrushAnimationProperty.BrushAnimationPropertyType.ValueFactor:
				return this.ValueFactor;
			case BrushAnimationProperty.BrushAnimationPropertyType.OverlayXOffset:
				return this.OverlayXOffset;
			case BrushAnimationProperty.BrushAnimationPropertyType.OverlayYOffset:
				return this.OverlayYOffset;
			default:
				switch (propertyType)
				{
				case BrushAnimationProperty.BrushAnimationPropertyType.XOffset:
					return this.XOffset;
				case BrushAnimationProperty.BrushAnimationPropertyType.YOffset:
					return this.YOffset;
				case BrushAnimationProperty.BrushAnimationPropertyType.Rotation:
					return this.Rotation;
				case BrushAnimationProperty.BrushAnimationPropertyType.OverridenWidth:
					return this.OverridenWidth;
				case BrushAnimationProperty.BrushAnimationPropertyType.OverridenHeight:
					return this.OverridenHeight;
				case BrushAnimationProperty.BrushAnimationPropertyType.ExtendLeft:
					return this.ExtendLeft;
				case BrushAnimationProperty.BrushAnimationPropertyType.ExtendRight:
					return this.ExtendRight;
				case BrushAnimationProperty.BrushAnimationPropertyType.ExtendTop:
					return this.ExtendTop;
				case BrushAnimationProperty.BrushAnimationPropertyType.ExtendBottom:
					return this.ExtendBottom;
				}
				break;
			}
			Debug.FailedAssert("Invalid value type or property name for data source.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Brush\\BrushLayer.cs", "GetValueAsFloat", 747);
			return 0f;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000071BA File Offset: 0x000053BA
		public Color GetValueAsColor(BrushAnimationProperty.BrushAnimationPropertyType propertyType)
		{
			if (propertyType == BrushAnimationProperty.BrushAnimationPropertyType.Color)
			{
				return this.Color;
			}
			Debug.FailedAssert("Invalid value type or property name for data source.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Brush\\BrushLayer.cs", "GetValueAsColor", 761);
			return Color.Black;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000071E5 File Offset: 0x000053E5
		public Sprite GetValueAsSprite(BrushAnimationProperty.BrushAnimationPropertyType propertyType)
		{
			if (propertyType == BrushAnimationProperty.BrushAnimationPropertyType.Sprite)
			{
				return this.Sprite;
			}
			if (propertyType != BrushAnimationProperty.BrushAnimationPropertyType.OverlaySprite)
			{
				Debug.FailedAssert("Invalid value type or property name for data source.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\Brush\\BrushLayer.cs", "GetValueAsSprite", 778);
				return null;
			}
			return this.OverlaySprite;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000721B File Offset: 0x0000541B
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.Name))
			{
				return this.Name;
			}
			return base.ToString();
		}

		// Token: 0x04000051 RID: 81
		private string _name;

		// Token: 0x04000052 RID: 82
		private Sprite _sprite;

		// Token: 0x04000053 RID: 83
		private ImageFit.ImageFitTypes _imageFitType;

		// Token: 0x04000054 RID: 84
		private ImageFit.ImageHorizontalAlignments _imageFitHorizontalAlignment;

		// Token: 0x04000055 RID: 85
		private ImageFit.ImageVerticalAlignments _imageFitVerticalAlignment;

		// Token: 0x04000056 RID: 86
		private Color _color;

		// Token: 0x04000057 RID: 87
		private float _colorFactor;

		// Token: 0x04000058 RID: 88
		private float _alphaFactor;

		// Token: 0x04000059 RID: 89
		private float _hueFactor;

		// Token: 0x0400005A RID: 90
		private float _saturationFactor;

		// Token: 0x0400005B RID: 91
		private float _valueFactor;

		// Token: 0x0400005C RID: 92
		private bool _isHidden;

		// Token: 0x0400005D RID: 93
		private bool _useOverlayAlphaAsMask;

		// Token: 0x0400005E RID: 94
		private float _xOffset;

		// Token: 0x0400005F RID: 95
		private float _yOffset;

		// Token: 0x04000060 RID: 96
		private float _rotation;

		// Token: 0x04000061 RID: 97
		private float _extendLeft;

		// Token: 0x04000062 RID: 98
		private float _extendRight;

		// Token: 0x04000063 RID: 99
		private float _extendTop;

		// Token: 0x04000064 RID: 100
		private float _extendBottom;

		// Token: 0x04000065 RID: 101
		private float _overridenWidth;

		// Token: 0x04000066 RID: 102
		private float _overridenHeight;

		// Token: 0x04000067 RID: 103
		private BrushLayerSizePolicy _widthPolicy;

		// Token: 0x04000068 RID: 104
		private BrushLayerSizePolicy _heightPolicy;

		// Token: 0x04000069 RID: 105
		private bool _horizontalFlip;

		// Token: 0x0400006A RID: 106
		private bool _verticalFlip;

		// Token: 0x0400006B RID: 107
		private BrushOverlayMethod _overlayMethod;

		// Token: 0x0400006C RID: 108
		private Sprite _overlaySprite;

		// Token: 0x0400006D RID: 109
		private float _overlayXOffset;

		// Token: 0x0400006E RID: 110
		private float _overlayYOffset;

		// Token: 0x0400006F RID: 111
		private bool _useRandomBaseOverlayXOffset;

		// Token: 0x04000070 RID: 112
		private bool _useRandomBaseOverlayYOffset;
	}
}
