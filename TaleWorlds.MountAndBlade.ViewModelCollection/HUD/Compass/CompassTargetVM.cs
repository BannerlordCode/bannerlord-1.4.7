using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass
{
	// Token: 0x02000068 RID: 104
	public class CompassTargetVM : ViewModel
	{
		// Token: 0x06000814 RID: 2068 RVA: 0x0001C338 File Offset: 0x0001A538
		public CompassTargetVM(TargetIconType iconType, uint color, uint color2, Banner banner, bool isAttacker, bool isAlly)
		{
			this.IconType = iconType.ToString();
			this.LetterCode = this.GetLetterCode(iconType);
			this.RefreshColor(color, color2);
			this.IsFlag = iconType >= TargetIconType.Flag_A && iconType <= TargetIconType.Flag_I;
			this.IsAttacker = isAttacker;
			this.IsEnemy = !isAlly;
			if (banner == null)
			{
				this.Banner = new BannerImageIdentifierVM(null, false);
				return;
			}
			this.Banner = new BannerImageIdentifierVM(banner, false);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001C3BC File Offset: 0x0001A5BC
		private string GetLetterCode(TargetIconType iconType)
		{
			switch (iconType)
			{
			case TargetIconType.Flag_A:
				return "A";
			case TargetIconType.Flag_B:
				return "B";
			case TargetIconType.Flag_C:
				return "C";
			case TargetIconType.Flag_D:
				return "D";
			case TargetIconType.Flag_E:
				return "E";
			case TargetIconType.Flag_F:
				return "F";
			case TargetIconType.Flag_G:
				return "G";
			case TargetIconType.Flag_H:
				return "H";
			case TargetIconType.Flag_I:
				return "I";
			default:
				return "";
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001C434 File Offset: 0x0001A634
		public void RefreshColor(uint color, uint color2)
		{
			if (color != 0U)
			{
				string text = color.ToString("X");
				char c = text[0];
				char c2 = text[1];
				text = text.Remove(0, 2);
				text = text.Add(c.ToString() + c2.ToString(), false);
				this.Color = "#" + text;
			}
			else
			{
				this.Color = "#FFFFFFFF";
			}
			if (color2 != 0U)
			{
				string text2 = color2.ToString("X");
				char c3 = text2[0];
				char c4 = text2[1];
				text2 = text2.Remove(0, 2);
				text2 = text2.Add(c3.ToString() + c4.ToString(), false);
				this.Color2 = "#" + text2;
				return;
			}
			this.Color2 = "#FFFFFFFF";
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001C506 File Offset: 0x0001A706
		public virtual void Refresh(float circleX, float x, float distance)
		{
			this.FullPosition = circleX;
			this.Position = x;
			this.Distance = MathF.Round(distance);
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0001C522 File Offset: 0x0001A722
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x0001C52C File Offset: 0x0001A72C
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner && (value == null || this._banner == null || this._banner.Id != value.Id))
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0001C578 File Offset: 0x0001A778
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0001C580 File Offset: 0x0001A780
		[DataSourceProperty]
		public bool IsFlag
		{
			get
			{
				return this._isFlag;
			}
			set
			{
				if (value != this._isFlag)
				{
					this._isFlag = value;
					base.OnPropertyChangedWithValue(value, "IsFlag");
				}
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0001C59E File Offset: 0x0001A79E
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x0001C5A6 File Offset: 0x0001A7A6
		[DataSourceProperty]
		public int Distance
		{
			get
			{
				return this._distance;
			}
			set
			{
				if (value != this._distance)
				{
					this._distance = value;
					base.OnPropertyChangedWithValue(value, "Distance");
				}
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x0001C5CC File Offset: 0x0001A7CC
		[DataSourceProperty]
		public string Color2
		{
			get
			{
				return this._color2;
			}
			set
			{
				if (value != this._color2)
				{
					this._color2 = value;
					base.OnPropertyChangedWithValue<string>(value, "Color2");
				}
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0001C5EF File Offset: 0x0001A7EF
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0001C5F7 File Offset: 0x0001A7F7
		[DataSourceProperty]
		public string Color
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
					base.OnPropertyChangedWithValue<string>(value, "Color");
				}
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0001C61A File Offset: 0x0001A81A
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0001C622 File Offset: 0x0001A822
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
					this.IconSpriteType = value;
				}
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001C64C File Offset: 0x0001A84C
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0001C654 File Offset: 0x0001A854
		[DataSourceProperty]
		public string IconSpriteType
		{
			get
			{
				return this._iconSpriteType;
			}
			set
			{
				if (value != this._iconSpriteType)
				{
					this._iconSpriteType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconSpriteType");
				}
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001C677 File Offset: 0x0001A877
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001C67F File Offset: 0x0001A87F
		[DataSourceProperty]
		public string LetterCode
		{
			get
			{
				return this._letterCode;
			}
			set
			{
				if (value != this._letterCode)
				{
					this._letterCode = value;
					base.OnPropertyChangedWithValue<string>(value, "LetterCode");
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001C6A2 File Offset: 0x0001A8A2
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x0001C6AA File Offset: 0x0001A8AA
		[DataSourceProperty]
		public float FullPosition
		{
			get
			{
				return this._fullPosition;
			}
			set
			{
				if (MathF.Abs(value - this._fullPosition) > 1E-45f)
				{
					this._fullPosition = value;
					base.OnPropertyChangedWithValue(value, "FullPosition");
				}
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0001C6D3 File Offset: 0x0001A8D3
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x0001C6DB File Offset: 0x0001A8DB
		[DataSourceProperty]
		public float Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (MathF.Abs(value - this._position) > 1E-45f)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x0001C704 File Offset: 0x0001A904
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x0001C70C File Offset: 0x0001A90C
		[DataSourceProperty]
		public bool IsAttacker
		{
			get
			{
				return this._isAttacker;
			}
			set
			{
				if (value != this._isAttacker)
				{
					this._isAttacker = value;
					base.OnPropertyChangedWithValue(value, "IsAttacker");
				}
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0001C72A File Offset: 0x0001A92A
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x0001C732 File Offset: 0x0001A932
		[DataSourceProperty]
		public bool IsEnemy
		{
			get
			{
				return this._isEnemy;
			}
			set
			{
				if (value != this._isEnemy)
				{
					this._isEnemy = value;
					base.OnPropertyChangedWithValue(value, "IsEnemy");
				}
			}
		}

		// Token: 0x0400039C RID: 924
		private int _distance;

		// Token: 0x0400039D RID: 925
		private string _color;

		// Token: 0x0400039E RID: 926
		private string _color2;

		// Token: 0x0400039F RID: 927
		private BannerImageIdentifierVM _banner;

		// Token: 0x040003A0 RID: 928
		private string _iconType;

		// Token: 0x040003A1 RID: 929
		private string _iconSpriteType;

		// Token: 0x040003A2 RID: 930
		private string _letterCode;

		// Token: 0x040003A3 RID: 931
		private float _position;

		// Token: 0x040003A4 RID: 932
		private float _fullPosition;

		// Token: 0x040003A5 RID: 933
		private bool _isAttacker;

		// Token: 0x040003A6 RID: 934
		private bool _isEnemy;

		// Token: 0x040003A7 RID: 935
		private bool _isFlag;
	}
}
