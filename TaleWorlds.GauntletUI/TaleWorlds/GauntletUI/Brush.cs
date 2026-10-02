using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200000B RID: 11
	public class Brush
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000030E4 File Offset: 0x000012E4
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000030EC File Offset: 0x000012EC
		public Brush ClonedFrom { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000030F5 File Offset: 0x000012F5
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000030FD File Offset: 0x000012FD
		public Brush OverriddenBrush { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003106 File Offset: 0x00001306
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000310E File Offset: 0x0000130E
		[Editor(false)]
		public string Name { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003117 File Offset: 0x00001317
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000311F File Offset: 0x0000131F
		[Editor(false)]
		public float TransitionDuration { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003128 File Offset: 0x00001328
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003130 File Offset: 0x00001330
		public Style DefaultStyle { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003139 File Offset: 0x00001339
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003146 File Offset: 0x00001346
		public Font Font
		{
			get
			{
				return this.DefaultStyle.Font;
			}
			set
			{
				this.DefaultStyle.Font = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003154 File Offset: 0x00001354
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003161 File Offset: 0x00001361
		public FontStyle FontStyle
		{
			get
			{
				return this.DefaultStyle.FontStyle;
			}
			set
			{
				this.DefaultStyle.FontStyle = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000316F File Offset: 0x0000136F
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000317C File Offset: 0x0000137C
		public int FontSize
		{
			get
			{
				return this.DefaultStyle.FontSize;
			}
			set
			{
				this.DefaultStyle.FontSize = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000318A File Offset: 0x0000138A
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003192 File Offset: 0x00001392
		[Editor(false)]
		public TextHorizontalAlignment TextHorizontalAlignment { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000319B File Offset: 0x0000139B
		// (set) Token: 0x06000082 RID: 130 RVA: 0x000031A3 File Offset: 0x000013A3
		[Editor(false)]
		public TextVerticalAlignment TextVerticalAlignment { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000031AC File Offset: 0x000013AC
		// (set) Token: 0x06000084 RID: 132 RVA: 0x000031B4 File Offset: 0x000013B4
		[Editor(false)]
		public float GlobalColorFactor { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000031BD File Offset: 0x000013BD
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000031C5 File Offset: 0x000013C5
		[Editor(false)]
		public float GlobalAlphaFactor { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000031CE File Offset: 0x000013CE
		// (set) Token: 0x06000088 RID: 136 RVA: 0x000031D6 File Offset: 0x000013D6
		[Editor(false)]
		public Color GlobalColor { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000031DF File Offset: 0x000013DF
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000031E7 File Offset: 0x000013E7
		public SoundProperties SoundProperties { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000031F0 File Offset: 0x000013F0
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000031FD File Offset: 0x000013FD
		public Sprite Sprite
		{
			get
			{
				return this.DefaultStyleLayer.Sprite;
			}
			set
			{
				this.DefaultStyleLayer.Sprite = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000320B File Offset: 0x0000140B
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003218 File Offset: 0x00001418
		[Editor(false)]
		public bool VerticalFlip
		{
			get
			{
				return this.DefaultStyleLayer.VerticalFlip;
			}
			set
			{
				this.DefaultStyleLayer.VerticalFlip = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003226 File Offset: 0x00001426
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003233 File Offset: 0x00001433
		[Editor(false)]
		public bool HorizontalFlip
		{
			get
			{
				return this.DefaultStyleLayer.HorizontalFlip;
			}
			set
			{
				this.DefaultStyleLayer.HorizontalFlip = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003241 File Offset: 0x00001441
		// (set) Token: 0x06000092 RID: 146 RVA: 0x0000324E File Offset: 0x0000144E
		public Color Color
		{
			get
			{
				return this.DefaultStyleLayer.Color;
			}
			set
			{
				this.DefaultStyleLayer.Color = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000325C File Offset: 0x0000145C
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003269 File Offset: 0x00001469
		public float ColorFactor
		{
			get
			{
				return this.DefaultStyleLayer.ColorFactor;
			}
			set
			{
				this.DefaultStyleLayer.ColorFactor = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003277 File Offset: 0x00001477
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003284 File Offset: 0x00001484
		public float AlphaFactor
		{
			get
			{
				return this.DefaultStyleLayer.AlphaFactor;
			}
			set
			{
				this.DefaultStyleLayer.AlphaFactor = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003292 File Offset: 0x00001492
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000329F File Offset: 0x0000149F
		public float HueFactor
		{
			get
			{
				return this.DefaultStyleLayer.HueFactor;
			}
			set
			{
				this.DefaultStyleLayer.HueFactor = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000032AD File Offset: 0x000014AD
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000032BA File Offset: 0x000014BA
		public float SaturationFactor
		{
			get
			{
				return this.DefaultStyleLayer.SaturationFactor;
			}
			set
			{
				this.DefaultStyleLayer.SaturationFactor = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000032C8 File Offset: 0x000014C8
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000032D5 File Offset: 0x000014D5
		public float ValueFactor
		{
			get
			{
				return this.DefaultStyleLayer.ValueFactor;
			}
			set
			{
				this.DefaultStyleLayer.ValueFactor = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000032E3 File Offset: 0x000014E3
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000032F0 File Offset: 0x000014F0
		public Color FontColor
		{
			get
			{
				return this.DefaultStyle.FontColor;
			}
			set
			{
				this.DefaultStyle.FontColor = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000032FE File Offset: 0x000014FE
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000330B File Offset: 0x0000150B
		public float TextColorFactor
		{
			get
			{
				return this.DefaultStyle.TextColorFactor;
			}
			set
			{
				this.DefaultStyle.TextColorFactor = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003319 File Offset: 0x00001519
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003326 File Offset: 0x00001526
		public float TextAlphaFactor
		{
			get
			{
				return this.DefaultStyle.TextAlphaFactor;
			}
			set
			{
				this.DefaultStyle.TextAlphaFactor = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003334 File Offset: 0x00001534
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003341 File Offset: 0x00001541
		public float TextHueFactor
		{
			get
			{
				return this.DefaultStyle.TextHueFactor;
			}
			set
			{
				this.DefaultStyle.TextHueFactor = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000334F File Offset: 0x0000154F
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000335C File Offset: 0x0000155C
		public float TextSaturationFactor
		{
			get
			{
				return this.DefaultStyle.TextSaturationFactor;
			}
			set
			{
				this.DefaultStyle.TextSaturationFactor = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000336A File Offset: 0x0000156A
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003377 File Offset: 0x00001577
		public float TextValueFactor
		{
			get
			{
				return this.DefaultStyle.TextValueFactor;
			}
			set
			{
				this.DefaultStyle.TextValueFactor = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003385 File Offset: 0x00001585
		[Editor(false)]
		public Dictionary<string, BrushLayer>.ValueCollection Layers
		{
			get
			{
				return this._layers.Values;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003392 File Offset: 0x00001592
		public StyleLayer DefaultStyleLayer
		{
			get
			{
				return this.DefaultStyle.DefaultLayer;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000339F File Offset: 0x0000159F
		public BrushLayer DefaultLayer
		{
			get
			{
				return this._layers["Default"];
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000033B4 File Offset: 0x000015B4
		public Brush()
		{
			this._styles = new Dictionary<string, Style>();
			this._layers = new Dictionary<string, BrushLayer>();
			this._brushAnimations = new Dictionary<string, BrushAnimation>();
			this.SoundProperties = new SoundProperties();
			this.TextHorizontalAlignment = TextHorizontalAlignment.Center;
			this.TextVerticalAlignment = TextVerticalAlignment.Center;
			BrushLayer brushLayer = new BrushLayer();
			brushLayer.Name = "Default";
			this._layers.Add(brushLayer.Name, brushLayer);
			this.DefaultStyle = new Style(new List<BrushLayer> { brushLayer });
			this.DefaultStyle.Name = "Default";
			this.DefaultStyle.SetAsDefaultStyle();
			this.AddStyle(this.DefaultStyle);
			this.ClonedFrom = null;
			this.TransitionDuration = 0.05f;
			this.GlobalColorFactor = 1f;
			this.GlobalAlphaFactor = 1f;
			this.GlobalColor = Color.White;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003498 File Offset: 0x00001698
		public Style GetStyle(string name)
		{
			Style style;
			this._styles.TryGetValue(name, out style);
			return style;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000034B5 File Offset: 0x000016B5
		[Editor(false)]
		public Dictionary<string, Style>.ValueCollection Styles
		{
			get
			{
				return this._styles.Values;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000034C4 File Offset: 0x000016C4
		public Style GetStyleOrDefault(string name)
		{
			Style style;
			this._styles.TryGetValue(name, out style);
			return style ?? this.DefaultStyle;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000034EC File Offset: 0x000016EC
		public void AddStyle(Style style)
		{
			string name = style.Name;
			this._styles.Add(name, style);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000350D File Offset: 0x0000170D
		public void RemoveStyle(string styleName)
		{
			this._styles.Remove(styleName);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000351C File Offset: 0x0000171C
		public void AddLayer(BrushLayer layer)
		{
			this._layers.Add(layer.Name, layer);
			foreach (Style style in this.Styles)
			{
				style.AddLayer(new StyleLayer(layer));
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003584 File Offset: 0x00001784
		public void RemoveLayer(string layerName)
		{
			this._layers.Remove(layerName);
			foreach (Style style in this.Styles)
			{
				style.RemoveLayer(layerName);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000035E4 File Offset: 0x000017E4
		public BrushLayer GetLayer(string name)
		{
			BrushLayer brushLayer;
			if (this._layers.TryGetValue(name, out brushLayer))
			{
				return brushLayer;
			}
			return null;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003604 File Offset: 0x00001804
		internal void FillForOverride(Brush originalBrush)
		{
			this.OverriddenBrush = originalBrush;
			this.FillFrom(this.OverriddenBrush);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000361C File Offset: 0x0000181C
		public void FillFrom(Brush brush)
		{
			this.Name = brush.Name;
			this.TransitionDuration = brush.TransitionDuration;
			this.TextVerticalAlignment = brush.TextVerticalAlignment;
			this.TextHorizontalAlignment = brush.TextHorizontalAlignment;
			this.GlobalColorFactor = brush.GlobalColorFactor;
			this.GlobalAlphaFactor = brush.GlobalAlphaFactor;
			this.GlobalColor = brush.GlobalColor;
			this._layers = new Dictionary<string, BrushLayer>();
			foreach (BrushLayer brushLayer in brush._layers.Values)
			{
				BrushLayer brushLayer2 = new BrushLayer();
				brushLayer2.FillFrom(brushLayer);
				this._layers.Add(brushLayer2.Name, brushLayer2);
			}
			this._styles = new Dictionary<string, Style>();
			Style style = brush._styles["Default"];
			Style style2 = new Style(this._layers.Values);
			style2.SetAsDefaultStyle();
			style2.FillFrom(style);
			this._styles.Add(style2.Name, style2);
			this.DefaultStyle = style2;
			foreach (Style style3 in brush._styles.Values)
			{
				if (style3.Name != "Default")
				{
					Style style4 = new Style(this._layers.Values);
					style4.DefaultStyle = this.DefaultStyle;
					style4.FillFrom(style3);
					this._styles.Add(style4.Name, style4);
				}
			}
			this._brushAnimations = new Dictionary<string, BrushAnimation>();
			foreach (BrushAnimation brushAnimation in brush._brushAnimations.Values)
			{
				BrushAnimation brushAnimation2 = new BrushAnimation();
				brushAnimation2.FillFrom(brushAnimation);
				this._brushAnimations.Add(brushAnimation2.Name, brushAnimation2);
			}
			this.SoundProperties = new SoundProperties();
			this.SoundProperties.FillFrom(brush.SoundProperties);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003860 File Offset: 0x00001A60
		public Brush Clone()
		{
			Brush brush = new Brush();
			brush.FillFrom(this);
			brush.Name = this.Name + "(Clone)";
			brush.ClonedFrom = this;
			return brush;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000388B File Offset: 0x00001A8B
		public void AddAnimation(BrushAnimation animation)
		{
			this._brushAnimations.Add(animation.Name, animation);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000038A0 File Offset: 0x00001AA0
		public BrushAnimation GetAnimation(string name)
		{
			BrushAnimation brushAnimation;
			if (name != null && this._brushAnimations.TryGetValue(name, out brushAnimation))
			{
				return brushAnimation;
			}
			return null;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000038C3 File Offset: 0x00001AC3
		public IEnumerable<BrushAnimation> GetAnimations()
		{
			return this._brushAnimations.Values;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000038D0 File Offset: 0x00001AD0
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return base.ToString();
			}
			return this.Name;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000038EC File Offset: 0x00001AEC
		public bool IsCloneRelated(Brush brush)
		{
			return this.ClonedFrom == brush || brush.ClonedFrom == this || brush.ClonedFrom == this.ClonedFrom;
		}

		// Token: 0x04000025 RID: 37
		private const float DefaultTransitionDuration = 0.05f;

		// Token: 0x0400002D RID: 45
		private Dictionary<string, Style> _styles;

		// Token: 0x0400002E RID: 46
		private Dictionary<string, BrushLayer> _layers;

		// Token: 0x0400002F RID: 47
		private Dictionary<string, BrushAnimation> _brushAnimations;
	}
}
