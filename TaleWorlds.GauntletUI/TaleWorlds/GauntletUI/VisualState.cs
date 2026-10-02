using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000037 RID: 55
	public class VisualState
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0000FAEE File Offset: 0x0000DCEE
		// (set) Token: 0x060003BD RID: 957 RVA: 0x0000FAF6 File Offset: 0x0000DCF6
		public string State { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000FAFF File Offset: 0x0000DCFF
		// (set) Token: 0x060003BF RID: 959 RVA: 0x0000FB07 File Offset: 0x0000DD07
		public float TransitionDuration
		{
			get
			{
				return this._transitionDuration;
			}
			set
			{
				this._transitionDuration = value;
				this.GotTransitionDuration = true;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000FB17 File Offset: 0x0000DD17
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000FB1F File Offset: 0x0000DD1F
		public float PositionXOffset
		{
			get
			{
				return this._positionXOffset;
			}
			set
			{
				this._positionXOffset = value;
				this.GotPositionXOffset = true;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000FB2F File Offset: 0x0000DD2F
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x0000FB37 File Offset: 0x0000DD37
		public float PositionYOffset
		{
			get
			{
				return this._positionYOffset;
			}
			set
			{
				this._positionYOffset = value;
				this.GotPositionYOffset = true;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000FB47 File Offset: 0x0000DD47
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x0000FB4F File Offset: 0x0000DD4F
		public float SuggestedWidth
		{
			get
			{
				return this._suggestedWidth;
			}
			set
			{
				this._suggestedWidth = value;
				this.GotSuggestedWidth = true;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000FB5F File Offset: 0x0000DD5F
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000FB67 File Offset: 0x0000DD67
		public float SuggestedHeight
		{
			get
			{
				return this._suggestedHeight;
			}
			set
			{
				this._suggestedHeight = value;
				this.GotSuggestedHeight = true;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000FB77 File Offset: 0x0000DD77
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0000FB7F File Offset: 0x0000DD7F
		public float MarginTop
		{
			get
			{
				return this._marginTop;
			}
			set
			{
				this._marginTop = value;
				this.GotMarginTop = true;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000FB8F File Offset: 0x0000DD8F
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000FB97 File Offset: 0x0000DD97
		public float MarginBottom
		{
			get
			{
				return this._marginBottom;
			}
			set
			{
				this._marginBottom = value;
				this.GotMarginBottom = true;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000FBA7 File Offset: 0x0000DDA7
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000FBAF File Offset: 0x0000DDAF
		public float MarginLeft
		{
			get
			{
				return this._marginLeft;
			}
			set
			{
				this._marginLeft = value;
				this.GotMarginLeft = true;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000FBBF File Offset: 0x0000DDBF
		// (set) Token: 0x060003CF RID: 975 RVA: 0x0000FBC7 File Offset: 0x0000DDC7
		public float MarginRight
		{
			get
			{
				return this._marginRight;
			}
			set
			{
				this._marginRight = value;
				this.GotMarginRight = true;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000FBD7 File Offset: 0x0000DDD7
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0000FBDF File Offset: 0x0000DDDF
		public bool GotTransitionDuration { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000FBE8 File Offset: 0x0000DDE8
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0000FBF0 File Offset: 0x0000DDF0
		public bool GotPositionXOffset { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000FBF9 File Offset: 0x0000DDF9
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0000FC01 File Offset: 0x0000DE01
		public bool GotPositionYOffset { get; private set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x0000FC0A File Offset: 0x0000DE0A
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x0000FC12 File Offset: 0x0000DE12
		public bool GotSuggestedWidth { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000FC1B File Offset: 0x0000DE1B
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0000FC23 File Offset: 0x0000DE23
		public bool GotSuggestedHeight { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000FC2C File Offset: 0x0000DE2C
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000FC34 File Offset: 0x0000DE34
		public bool GotMarginTop { get; private set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000FC3D File Offset: 0x0000DE3D
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0000FC45 File Offset: 0x0000DE45
		public bool GotMarginBottom { get; private set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000FC4E File Offset: 0x0000DE4E
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0000FC56 File Offset: 0x0000DE56
		public bool GotMarginLeft { get; private set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0000FC5F File Offset: 0x0000DE5F
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x0000FC67 File Offset: 0x0000DE67
		public bool GotMarginRight { get; private set; }

		// Token: 0x060003E2 RID: 994 RVA: 0x0000FC70 File Offset: 0x0000DE70
		public VisualState(string state)
		{
			this.State = state;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000FC80 File Offset: 0x0000DE80
		public void FillFromWidget(Widget widget)
		{
			this.PositionXOffset = widget.PositionXOffset;
			this.PositionYOffset = widget.PositionYOffset;
			this.SuggestedWidth = widget.SuggestedWidth;
			this.SuggestedHeight = widget.SuggestedHeight;
			this.MarginTop = widget.MarginTop;
			this.MarginBottom = widget.MarginBottom;
			this.MarginLeft = widget.MarginLeft;
			this.MarginRight = widget.MarginRight;
		}

		// Token: 0x040001D7 RID: 471
		private float _transitionDuration;

		// Token: 0x040001D8 RID: 472
		private float _positionXOffset;

		// Token: 0x040001D9 RID: 473
		private float _positionYOffset;

		// Token: 0x040001DA RID: 474
		private float _suggestedWidth;

		// Token: 0x040001DB RID: 475
		private float _suggestedHeight;

		// Token: 0x040001DC RID: 476
		private float _marginTop;

		// Token: 0x040001DD RID: 477
		private float _marginBottom;

		// Token: 0x040001DE RID: 478
		private float _marginLeft;

		// Token: 0x040001DF RID: 479
		private float _marginRight;
	}
}
