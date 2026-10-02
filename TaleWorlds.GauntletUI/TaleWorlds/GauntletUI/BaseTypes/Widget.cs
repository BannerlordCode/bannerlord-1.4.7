using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.GauntletUI.Layout;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200006D RID: 109
	public class Widget : PropertyOwnerObject
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0001FE84 File Offset: 0x0001E084
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0001FE8C File Offset: 0x0001E08C
		public float ColorFactor { get; set; } = 1f;

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0001FE95 File Offset: 0x0001E095
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x0001FE9D File Offset: 0x0001E09D
		public float AlphaFactor { get; set; } = 1f;

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0001FEA6 File Offset: 0x0001E0A6
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x0001FEAE File Offset: 0x0001E0AE
		public float ValueFactor { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0001FEB7 File Offset: 0x0001E0B7
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x0001FEBF File Offset: 0x0001E0BF
		public float SaturationFactor { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0001FEC8 File Offset: 0x0001E0C8
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x0001FED0 File Offset: 0x0001E0D0
		public float ExtendLeft { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0001FED9 File Offset: 0x0001E0D9
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x0001FEE1 File Offset: 0x0001E0E1
		public float ExtendRight { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0001FEEA File Offset: 0x0001E0EA
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x0001FEF2 File Offset: 0x0001E0F2
		public float ExtendTop { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0001FEFB File Offset: 0x0001E0FB
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x0001FF03 File Offset: 0x0001E103
		public float ExtendBottom { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0001FF0C File Offset: 0x0001E10C
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x0001FF14 File Offset: 0x0001E114
		public bool VerticalFlip { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001FF1D File Offset: 0x0001E11D
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x0001FF25 File Offset: 0x0001E125
		public bool HorizontalFlip { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0001FF2E File Offset: 0x0001E12E
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x0001FF36 File Offset: 0x0001E136
		public int NinePatchTop { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0001FF3F File Offset: 0x0001E13F
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x0001FF47 File Offset: 0x0001E147
		public int NinePatchBottom { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0001FF50 File Offset: 0x0001E150
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0001FF58 File Offset: 0x0001E158
		public int NinePatchLeft { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0001FF61 File Offset: 0x0001E161
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0001FF69 File Offset: 0x0001E169
		public int NinePatchRight { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0001FF72 File Offset: 0x0001E172
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0001FF7A File Offset: 0x0001E17A
		public ImageFit ImageFit { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0001FF83 File Offset: 0x0001E183
		public float GlobalRotation
		{
			get
			{
				if (this.ParentWidget != null)
				{
					return this.Rotation + this.ParentWidget.GlobalRotation;
				}
				return this.Rotation;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0001FFA6 File Offset: 0x0001E1A6
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x0001FFAE File Offset: 0x0001E1AE
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
					this.SetMeasureAndLayoutDirty();
					this.EventManager.SetPositionsDirty();
				}
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0001FFD1 File Offset: 0x0001E1D1
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x0001FFD9 File Offset: 0x0001E1D9
		public float PivotX
		{
			get
			{
				return this._pivotX;
			}
			set
			{
				if (value != this._pivotX)
				{
					this._pivotX = value;
					this.SetMeasureAndLayoutDirty();
					this.EventManager.SetPositionsDirty();
				}
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x00020004 File Offset: 0x0001E204
		public float PivotY
		{
			get
			{
				return this._pivotY;
			}
			set
			{
				if (value != this._pivotY)
				{
					this._pivotY = value;
					this.SetMeasureAndLayoutDirty();
					this.EventManager.SetPositionsDirty();
				}
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00020027 File Offset: 0x0001E227
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x00020034 File Offset: 0x0001E234
		public float Left
		{
			get
			{
				return this._topLeft.X;
			}
			private set
			{
				if (value != this._topLeft.X)
				{
					this._topLeft.X = value;
					this.EventManager.SetPositionsDirty();
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0002005B File Offset: 0x0001E25B
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00020068 File Offset: 0x0001E268
		public float Top
		{
			get
			{
				return this._topLeft.Y;
			}
			private set
			{
				if (value != this._topLeft.Y)
				{
					this._topLeft.Y = value;
					this.EventManager.SetPositionsDirty();
				}
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0002008F File Offset: 0x0001E28F
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00020097 File Offset: 0x0001E297
		public Vector2 Size { get; private set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x000200A0 File Offset: 0x0001E2A0
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x000200A8 File Offset: 0x0001E2A8
		public bool FrictionEnabled { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000200B1 File Offset: 0x0001E2B1
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x000200B9 File Offset: 0x0001E2B9
		public Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (this._color != value)
				{
					this._color = value;
				}
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000200D0 File Offset: 0x0001E2D0
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x000200D8 File Offset: 0x0001E2D8
		[Editor(false)]
		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (this._id != value)
				{
					this._id = value;
					base.OnPropertyChanged<string>(value, "Id");
				}
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000200FB File Offset: 0x0001E2FB
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x00020103 File Offset: 0x0001E303
		public Vector2 LocalPosition { get; private set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0002010C File Offset: 0x0001E30C
		public Vector2 GlobalPosition
		{
			get
			{
				if (this.ParentWidget != null)
				{
					return this.LocalPosition + this.ParentWidget.GlobalPosition;
				}
				return this.LocalPosition;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00020134 File Offset: 0x0001E334
		public Rectangle2D GamepadCursorAreaRect
		{
			get
			{
				if (this._isGamepadCursorAreaDirty)
				{
					Vector2 localPosition = this.AreaRect.LocalPosition;
					Vector2 localScale = this.AreaRect.LocalScale;
					this._gamepadCursorAreaRect = this.AreaRect;
					float num = this.ExtendCursorAreaLeft * this._scaleToUse;
					float num2 = this.ExtendCursorAreaTop * this._scaleToUse;
					float num3 = this.ExtendCursorAreaRight * this._scaleToUse;
					float num4 = this.ExtendCursorAreaBottom * this._scaleToUse;
					this._gamepadCursorAreaRect.LocalPosition = new Vector2(localPosition.X - num, localPosition.Y - num2);
					this._gamepadCursorAreaRect.LocalScale = new Vector2(localScale.X + num + num3, localScale.Y + num2 + num4);
					if (this.ParentWidget != null)
					{
						this._gamepadCursorAreaRect.CalculateMatrixFrame(in this.ParentWidget.AreaRect);
					}
					else
					{
						Rectangle2D invalid = Rectangle2D.Invalid;
						this._gamepadCursorAreaRect.CalculateMatrixFrame(in invalid);
					}
				}
				return this._gamepadCursorAreaRect;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0002022A File Offset: 0x0001E42A
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x00020234 File Offset: 0x0001E434
		[Editor(false)]
		public bool DoNotUseCustomScaleAndChildren
		{
			get
			{
				return this._doNotUseCustomScaleAndChildren;
			}
			set
			{
				if (this._doNotUseCustomScaleAndChildren != value)
				{
					this._doNotUseCustomScaleAndChildren = value;
					base.OnPropertyChanged(value, "DoNotUseCustomScaleAndChildren");
					this.DoNotUseCustomScale = value;
					this.ApplyActionToAllChildren(delegate(Widget child)
					{
						child.DoNotUseCustomScaleAndChildren = value;
					});
				}
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00020297 File Offset: 0x0001E497
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0002029F File Offset: 0x0001E49F
		public bool DoNotUseCustomScale { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x000202A8 File Offset: 0x0001E4A8
		protected float _scaleToUse
		{
			get
			{
				if (!this.DoNotUseCustomScale)
				{
					return this.Context.CustomScale;
				}
				return this.Context.Scale;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x000202C9 File Offset: 0x0001E4C9
		protected float _inverseScaleToUse
		{
			get
			{
				if (!this.DoNotUseCustomScale)
				{
					return this.Context.CustomInverseScale;
				}
				return this.Context.InverseScale;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x000202EA File Offset: 0x0001E4EA
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x000202F2 File Offset: 0x0001E4F2
		[Editor(false)]
		public float SuggestedWidth
		{
			get
			{
				return this._suggestedWidth;
			}
			set
			{
				if (this._suggestedWidth != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._suggestedWidth = value;
					base.OnPropertyChanged(value, "SuggestedWidth");
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00020316 File Offset: 0x0001E516
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x0002031E File Offset: 0x0001E51E
		[Editor(false)]
		public float SuggestedHeight
		{
			get
			{
				return this._suggestedHeight;
			}
			set
			{
				if (this._suggestedHeight != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._suggestedHeight = value;
					base.OnPropertyChanged(value, "SuggestedHeight");
				}
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00020342 File Offset: 0x0001E542
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x00020351 File Offset: 0x0001E551
		public float ScaledSuggestedWidth
		{
			get
			{
				return this._scaleToUse * this.SuggestedWidth;
			}
			set
			{
				this.SuggestedWidth = value * this._inverseScaleToUse;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00020361 File Offset: 0x0001E561
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x00020370 File Offset: 0x0001E570
		public float ScaledSuggestedHeight
		{
			get
			{
				return this._scaleToUse * this.SuggestedHeight;
			}
			set
			{
				this.SuggestedHeight = value * this._inverseScaleToUse;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00020380 File Offset: 0x0001E580
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x00020388 File Offset: 0x0001E588
		[Editor(false)]
		public string HoveredCursorState
		{
			get
			{
				return this._hoveredCursorState;
			}
			set
			{
				if (this._hoveredCursorState != value)
				{
					string hoveredCursorState = this._hoveredCursorState;
					this._hoveredCursorState = value;
				}
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000203A6 File Offset: 0x0001E5A6
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x000203AE File Offset: 0x0001E5AE
		[Editor(false)]
		public bool AlternateClickEventHasSpecialEvent
		{
			get
			{
				return this._alternateClickEventHasSpecialEvent;
			}
			set
			{
				if (this._alternateClickEventHasSpecialEvent != value)
				{
					bool alternateClickEventHasSpecialEvent = this._alternateClickEventHasSpecialEvent;
					this._alternateClickEventHasSpecialEvent = value;
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000203C7 File Offset: 0x0001E5C7
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x000203D0 File Offset: 0x0001E5D0
		public Vector2 PosOffset
		{
			get
			{
				return this._positionOffset;
			}
			set
			{
				if (!this._positionOffset.X.ApproximatelyEqualsTo(value.X, 1E-05f) || !this._positionOffset.Y.ApproximatelyEqualsTo(value.Y, 1E-05f))
				{
					this.SetLayoutDirty();
					this._positionOffset = value;
					base.OnPropertyChanged(value, "PosOffset");
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00020430 File Offset: 0x0001E630
		public Vector2 ScaledPositionOffset
		{
			get
			{
				return this._positionOffset * this._scaleToUse;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00020443 File Offset: 0x0001E643
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x00020450 File Offset: 0x0001E650
		[Editor(false)]
		public float PositionXOffset
		{
			get
			{
				return this._positionOffset.X;
			}
			set
			{
				if (!this._positionOffset.X.ApproximatelyEqualsTo(value, 1E-05f))
				{
					this.SetLayoutDirty();
					this._positionOffset.X = value;
					base.OnPropertyChanged(value, "PositionXOffset");
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00020488 File Offset: 0x0001E688
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00020495 File Offset: 0x0001E695
		[Editor(false)]
		public float PositionYOffset
		{
			get
			{
				return this._positionOffset.Y;
			}
			set
			{
				if (!this._positionOffset.Y.ApproximatelyEqualsTo(value, 1E-05f))
				{
					this.SetLayoutDirty();
					this._positionOffset.Y = value;
					base.OnPropertyChanged(value, "PositionYOffset");
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x000204CD File Offset: 0x0001E6CD
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x000204E4 File Offset: 0x0001E6E4
		public float ScaledPositionXOffset
		{
			get
			{
				return this._positionOffset.X * this._scaleToUse;
			}
			set
			{
				float num = value * this._inverseScaleToUse;
				if (!num.ApproximatelyEqualsTo(this._positionOffset.X, 1E-05f))
				{
					this.SetLayoutDirty();
					this._positionOffset.X = num;
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00020524 File Offset: 0x0001E724
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x00020538 File Offset: 0x0001E738
		public float ScaledPositionYOffset
		{
			get
			{
				return this._positionOffset.Y * this._scaleToUse;
			}
			set
			{
				float num = value * this._inverseScaleToUse;
				if (!num.ApproximatelyEqualsTo(this._positionOffset.Y, 1E-05f))
				{
					this.SetLayoutDirty();
					this._positionOffset.Y = num;
				}
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00020578 File Offset: 0x0001E778
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x00020580 File Offset: 0x0001E780
		public Widget ParentWidget
		{
			get
			{
				return this._parent;
			}
			set
			{
				if (this.ParentWidget != value)
				{
					if (this._parent != null)
					{
						this._parent.OnBeforeChildRemoved(this);
						if (this.ConnectedToRoot)
						{
							this.EventManager.OnWidgetDisconnectedFromRoot(this);
						}
						int childIndex = this._parent.GetChildIndex(this);
						this._parent._children.Remove(this);
						this._parent.OnAfterChildRemoved(this, childIndex);
					}
					this._parent = value;
					if (this._parent != null)
					{
						this._parent._children.Add(this);
						if (this.ConnectedToRoot)
						{
							this.EventManager.OnWidgetConnectedToRoot(this);
						}
						this._parent.OnChildAdded(this);
					}
					this.SetMeasureAndLayoutDirty();
				}
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00020633 File Offset: 0x0001E833
		public EventManager EventManager
		{
			get
			{
				return this.Context.EventManager;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00020640 File Offset: 0x0001E840
		public IGamepadNavigationContext GamepadNavigationContext
		{
			get
			{
				return this.Context.GamepadNavigation;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0002064D File Offset: 0x0001E84D
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x00020655 File Offset: 0x0001E855
		public UIContext Context { get; private set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0002065E File Offset: 0x0001E85E
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x00020666 File Offset: 0x0001E866
		public Vector2 MeasuredSize { get; private set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0002066F File Offset: 0x0001E86F
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x00020677 File Offset: 0x0001E877
		[Editor(false)]
		public float MarginTop
		{
			get
			{
				return this._marginTop;
			}
			set
			{
				if (this._marginTop != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._marginTop = value;
					base.OnPropertyChanged(value, "MarginTop");
				}
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x0002069B File Offset: 0x0001E89B
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x000206A3 File Offset: 0x0001E8A3
		[Editor(false)]
		public float MarginLeft
		{
			get
			{
				return this._marginLeft;
			}
			set
			{
				if (this._marginLeft != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._marginLeft = value;
					base.OnPropertyChanged(value, "MarginLeft");
				}
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x000206C7 File Offset: 0x0001E8C7
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x000206CF File Offset: 0x0001E8CF
		[Editor(false)]
		public float MarginBottom
		{
			get
			{
				return this._marginBottom;
			}
			set
			{
				if (this._marginBottom != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._marginBottom = value;
					base.OnPropertyChanged(value, "MarginBottom");
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x000206F3 File Offset: 0x0001E8F3
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x000206FB File Offset: 0x0001E8FB
		[Editor(false)]
		public float MarginRight
		{
			get
			{
				return this._marginRight;
			}
			set
			{
				if (this._marginRight != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._marginRight = value;
					base.OnPropertyChanged(value, "MarginRight");
				}
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0002071F File Offset: 0x0001E91F
		public float ScaledMarginTop
		{
			get
			{
				return this._scaleToUse * this.MarginTop;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0002072E File Offset: 0x0001E92E
		public float ScaledMarginLeft
		{
			get
			{
				return this._scaleToUse * this.MarginLeft;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002073D File Offset: 0x0001E93D
		public float ScaledMarginBottom
		{
			get
			{
				return this._scaleToUse * this.MarginBottom;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0002074C File Offset: 0x0001E94C
		public float ScaledMarginRight
		{
			get
			{
				return this._scaleToUse * this.MarginRight;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0002075B File Offset: 0x0001E95B
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x00020764 File Offset: 0x0001E964
		[Editor(false)]
		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return this._verticalAlignment;
			}
			set
			{
				if (this._verticalAlignment != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._verticalAlignment = value;
					switch (value)
					{
					case VerticalAlignment.Top:
						base.OnPropertyChanged<string>("Top", "VerticalAlignment");
						return;
					case VerticalAlignment.Center:
						base.OnPropertyChanged<string>("Center", "VerticalAlignment");
						return;
					case VerticalAlignment.Bottom:
						base.OnPropertyChanged<string>("Bottom", "VerticalAlignment");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000207CC File Offset: 0x0001E9CC
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x000207D4 File Offset: 0x0001E9D4
		[Editor(false)]
		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return this._horizontalAlignment;
			}
			set
			{
				if (this._horizontalAlignment != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._horizontalAlignment = value;
					switch (value)
					{
					case HorizontalAlignment.Left:
						base.OnPropertyChanged<string>("Left", "HorizontalAlignment");
						return;
					case HorizontalAlignment.Center:
						base.OnPropertyChanged<string>("Center", "HorizontalAlignment");
						return;
					case HorizontalAlignment.Right:
						base.OnPropertyChanged<string>("Right", "HorizontalAlignment");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0002083C File Offset: 0x0001EA3C
		public float Right
		{
			get
			{
				return this._topLeft.X + this.Size.X;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00020855 File Offset: 0x0001EA55
		public float Bottom
		{
			get
			{
				return this._topLeft.Y + this.Size.Y;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002086E File Offset: 0x0001EA6E
		public int ChildCount
		{
			get
			{
				return this._children.Count;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0002087B File Offset: 0x0001EA7B
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x00020883 File Offset: 0x0001EA83
		[Editor(false)]
		public bool ForcePixelPerfectRenderPlacement
		{
			get
			{
				return this._forcePixelPerfectRenderPlacement;
			}
			set
			{
				if (this._forcePixelPerfectRenderPlacement != value)
				{
					this._forcePixelPerfectRenderPlacement = value;
					base.OnPropertyChanged(value, "ForcePixelPerfectRenderPlacement");
				}
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x000208A1 File Offset: 0x0001EAA1
		// (set) Token: 0x060007E1 RID: 2017 RVA: 0x000208A9 File Offset: 0x0001EAA9
		public bool UseGlobalTimeForAnimation { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x000208B2 File Offset: 0x0001EAB2
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x000208BA File Offset: 0x0001EABA
		public bool UseSpriteDimensions { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x000208C3 File Offset: 0x0001EAC3
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x000208CB File Offset: 0x0001EACB
		[Editor(false)]
		public SizePolicy WidthSizePolicy
		{
			get
			{
				return this._widthSizePolicy;
			}
			set
			{
				if (value != this._widthSizePolicy)
				{
					this.SetMeasureAndLayoutDirty();
					this._widthSizePolicy = value;
				}
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x000208E3 File Offset: 0x0001EAE3
		// (set) Token: 0x060007E7 RID: 2023 RVA: 0x000208EB File Offset: 0x0001EAEB
		[Editor(false)]
		public SizePolicy HeightSizePolicy
		{
			get
			{
				return this._heightSizePolicy;
			}
			set
			{
				if (value != this._heightSizePolicy)
				{
					this.SetMeasureAndLayoutDirty();
					this._heightSizePolicy = value;
				}
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00020903 File Offset: 0x0001EB03
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0002090B File Offset: 0x0001EB0B
		[Editor(false)]
		public bool AcceptDrag { get; set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x00020914 File Offset: 0x0001EB14
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x0002091C File Offset: 0x0001EB1C
		[Editor(false)]
		public bool AcceptDrop { get; set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00020925 File Offset: 0x0001EB25
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0002092D File Offset: 0x0001EB2D
		[Editor(false)]
		public bool HideOnDrag { get; set; } = true;

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00020936 File Offset: 0x0001EB36
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x0002093E File Offset: 0x0001EB3E
		[Editor(false)]
		public Widget DragWidget
		{
			get
			{
				return this._dragWidget;
			}
			set
			{
				if (this._dragWidget != value)
				{
					if (value != null)
					{
						this._dragWidget = value;
						this._dragWidget.IsVisible = false;
						return;
					}
					this._dragWidget = null;
				}
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00020967 File Offset: 0x0001EB67
		// (set) Token: 0x060007F1 RID: 2033 RVA: 0x00020979 File Offset: 0x0001EB79
		[Editor(false)]
		public bool ClipContents
		{
			get
			{
				return this.ClipVerticalContent && this.ClipHorizontalContent;
			}
			set
			{
				this.ClipHorizontalContent = value;
				this.ClipVerticalContent = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00020989 File Offset: 0x0001EB89
		// (set) Token: 0x060007F3 RID: 2035 RVA: 0x00020991 File Offset: 0x0001EB91
		[Editor(false)]
		public bool ClipHorizontalContent { get; set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0002099A File Offset: 0x0001EB9A
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x000209A2 File Offset: 0x0001EBA2
		[Editor(false)]
		public bool ClipVerticalContent { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x000209AB File Offset: 0x0001EBAB
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x000209B3 File Offset: 0x0001EBB3
		[Editor(false)]
		public bool CircularClipEnabled { get; set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x000209BC File Offset: 0x0001EBBC
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x000209C4 File Offset: 0x0001EBC4
		[Editor(false)]
		public float CircularClipRadius { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x000209CD File Offset: 0x0001EBCD
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x000209D5 File Offset: 0x0001EBD5
		[Editor(false)]
		public bool IsCircularClipRadiusHalfOfWidth { get; set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x000209DE File Offset: 0x0001EBDE
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x000209E6 File Offset: 0x0001EBE6
		[Editor(false)]
		public bool IsCircularClipRadiusHalfOfHeight { get; set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x000209EF File Offset: 0x0001EBEF
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x000209F7 File Offset: 0x0001EBF7
		[Editor(false)]
		public float CircularClipSmoothingRadius { get; set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00020A00 File Offset: 0x0001EC00
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x00020A08 File Offset: 0x0001EC08
		[Editor(false)]
		public float CircularClipXOffset { get; set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x00020A11 File Offset: 0x0001EC11
		// (set) Token: 0x06000803 RID: 2051 RVA: 0x00020A19 File Offset: 0x0001EC19
		[Editor(false)]
		public float CircularClipYOffset { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00020A22 File Offset: 0x0001EC22
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x00020A2A File Offset: 0x0001EC2A
		[Editor(false)]
		public bool RenderLate { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x00020A33 File Offset: 0x0001EC33
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x00020A3B File Offset: 0x0001EC3B
		[Editor(false)]
		public bool DoNotRenderIfNotFullyInsideScissor { get; set; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00020A44 File Offset: 0x0001EC44
		public bool FixedWidth
		{
			get
			{
				return this.WidthSizePolicy == SizePolicy.Fixed;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00020A4F File Offset: 0x0001EC4F
		public bool FixedHeight
		{
			get
			{
				return this.HeightSizePolicy == SizePolicy.Fixed;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x00020A5A File Offset: 0x0001EC5A
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00020A62 File Offset: 0x0001EC62
		public bool IsHovered
		{
			get
			{
				return this._isHovered;
			}
			private set
			{
				if (this._isHovered != value)
				{
					this._isHovered = value;
					this.RefreshState();
					base.OnPropertyChanged(value, "IsHovered");
				}
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x00020A86 File Offset: 0x0001EC86
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x00020A8E File Offset: 0x0001EC8E
		[Editor(false)]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (this._isDisabled != value)
				{
					this._isDisabled = value;
					base.OnPropertyChanged(value, "IsDisabled");
					this.RefreshState();
				}
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x00020AB2 File Offset: 0x0001ECB2
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x00020ABA File Offset: 0x0001ECBA
		[Editor(false)]
		public bool IsFocusable
		{
			get
			{
				return this._isFocusable;
			}
			set
			{
				if (this._isFocusable != value)
				{
					this._isFocusable = value;
					if (this.ConnectedToRoot)
					{
						base.OnPropertyChanged(value, "IsFocusable");
						this.RefreshState();
					}
				}
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x00020AE6 File Offset: 0x0001ECE6
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x00020AEE File Offset: 0x0001ECEE
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			private set
			{
				if (this._isFocused != value)
				{
					this._isFocused = value;
					this.RefreshState();
				}
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00020B06 File Offset: 0x0001ED06
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x00020B11 File Offset: 0x0001ED11
		[Editor(false)]
		public bool IsEnabled
		{
			get
			{
				return !this.IsDisabled;
			}
			set
			{
				if (value == this.IsDisabled)
				{
					this.IsDisabled = !value;
					base.OnPropertyChanged(value, "IsEnabled");
				}
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00020B32 File Offset: 0x0001ED32
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x00020B3A File Offset: 0x0001ED3A
		[Editor(false)]
		public bool RestartAnimationFirstFrame
		{
			get
			{
				return this._restartAnimationFirstFrame;
			}
			set
			{
				if (this._restartAnimationFirstFrame != value)
				{
					this._restartAnimationFirstFrame = value;
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x00020B4C File Offset: 0x0001ED4C
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x00020B54 File Offset: 0x0001ED54
		[Editor(false)]
		public bool DoNotPassEventsToChildren
		{
			get
			{
				return this._doNotPassEventsToChildren;
			}
			set
			{
				if (this._doNotPassEventsToChildren != value)
				{
					this._doNotPassEventsToChildren = value;
					base.OnPropertyChanged(value, "DoNotPassEventsToChildren");
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00020B72 File Offset: 0x0001ED72
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x00020B7A File Offset: 0x0001ED7A
		[Editor(false)]
		public bool DoNotAcceptEvents
		{
			get
			{
				return this._doNotAcceptEvents;
			}
			set
			{
				if (this._doNotAcceptEvents != value)
				{
					this._doNotAcceptEvents = value;
					base.OnPropertyChanged(value, "DoNotAcceptEvents");
				}
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00020B98 File Offset: 0x0001ED98
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00020BA3 File Offset: 0x0001EDA3
		[Editor(false)]
		public bool CanAcceptEvents
		{
			get
			{
				return !this.DoNotAcceptEvents;
			}
			set
			{
				this.DoNotAcceptEvents = !value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00020BAF File Offset: 0x0001EDAF
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x00020BB7 File Offset: 0x0001EDB7
		public bool IsPressed
		{
			get
			{
				return this._isPressed;
			}
			internal set
			{
				if (this._isPressed != value)
				{
					this._isPressed = value;
					this.RefreshState();
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00020BCF File Offset: 0x0001EDCF
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00020BD8 File Offset: 0x0001EDD8
		[Editor(false)]
		public bool IsHidden
		{
			get
			{
				return this._isHidden;
			}
			set
			{
				if (this._isHidden != value)
				{
					this.SetMeasureAndLayoutDirty();
					this._isHidden = value;
					this.RefreshState();
					base.OnPropertyChanged(value, "IsHidden");
					base.OnPropertyChanged(!value, "IsVisible");
					if (this.OnVisibilityChanged != null)
					{
						this.OnVisibilityChanged(this);
					}
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00020C30 File Offset: 0x0001EE30
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x00020C3B File Offset: 0x0001EE3B
		[Editor(false)]
		public bool IsVisible
		{
			get
			{
				return !this._isHidden;
			}
			set
			{
				if (value == this._isHidden)
				{
					this.IsHidden = !value;
				}
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00020C50 File Offset: 0x0001EE50
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x00020C58 File Offset: 0x0001EE58
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
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00020C6A File Offset: 0x0001EE6A
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x00020C74 File Offset: 0x0001EE74
		[Editor(false)]
		public VisualDefinition VisualDefinition
		{
			get
			{
				return this._visualDefinition;
			}
			set
			{
				if (this._visualDefinition != value)
				{
					VisualDefinition visualDefinition = this._visualDefinition;
					this._visualDefinition = value;
					this._stateTimer = 0f;
					if (this.ConnectedToRoot && (visualDefinition == null || this._visualDefinition == null))
					{
						this.EventManager.OnWidgetVisualDefinitionChanged(this);
					}
				}
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00020CC2 File Offset: 0x0001EEC2
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x00020CCA File Offset: 0x0001EECA
		public string CurrentState { get; protected set; } = "";

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00020CD3 File Offset: 0x0001EED3
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x00020CDB File Offset: 0x0001EEDB
		[Editor(false)]
		public bool UpdateChildrenStates
		{
			get
			{
				return this._updateChildrenStates;
			}
			set
			{
				if (this._updateChildrenStates != value)
				{
					this._updateChildrenStates = value;
					base.OnPropertyChanged(value, "UpdateChildrenStates");
					if (value && this.ChildCount > 0)
					{
						this.SetState(this.CurrentState);
					}
				}
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00020D11 File Offset: 0x0001EF11
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x00020D19 File Offset: 0x0001EF19
		public object Tag { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00020D22 File Offset: 0x0001EF22
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x00020D2A File Offset: 0x0001EF2A
		public ILayout LayoutImp { get; protected set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x00020D33 File Offset: 0x0001EF33
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x00020D3B File Offset: 0x0001EF3B
		[Editor(false)]
		public bool DropEventHandledManually { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x00020D44 File Offset: 0x0001EF44
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x00020D4C File Offset: 0x0001EF4C
		internal WidgetInfo WidgetInfo { get; private set; }

		// Token: 0x06000832 RID: 2098 RVA: 0x00020D58 File Offset: 0x0001EF58
		public List<Widget> GetAllChildrenAndThisRecursive()
		{
			List<Widget> list = new List<Widget>();
			list.Add(this);
			this.GetAllChildrenAux(list, null);
			return list;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00020D7C File Offset: 0x0001EF7C
		public void ApplyActionToAllChildren(Action<Widget> action)
		{
			for (int i = 0; i < this._children.Count; i++)
			{
				action(this._children[i]);
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00020DB4 File Offset: 0x0001EFB4
		public void ApplyActionToAllChildrenRecursive(Action<Widget> action)
		{
			for (int i = 0; i < this._children.Count; i++)
			{
				action(this._children[i]);
				this._children[i].ApplyActionToAllChildrenRecursive(action);
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00020DFC File Offset: 0x0001EFFC
		public List<TWidget> GetAllChildrenOfTypeRecursive<TWidget>(Func<TWidget, bool> predicate = null) where TWidget : Widget
		{
			List<TWidget> list = new List<TWidget>();
			this.GetChildrenOfTypeAux<TWidget>(list, predicate);
			return list;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00020E18 File Offset: 0x0001F018
		private void GetChildrenOfTypeAux<TWidget>(List<TWidget> list, Func<TWidget, bool> predicate = null)
		{
			for (int i = 0; i < this._children.Count; i++)
			{
				Widget widget;
				if ((widget = this._children[i]) is TWidget)
				{
					TWidget twidget = (TWidget)((object)widget);
					if (predicate == null || predicate(twidget))
					{
						list.Add(twidget);
					}
				}
				this._children[i].GetChildrenOfTypeAux<TWidget>(list, predicate);
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00020E80 File Offset: 0x0001F080
		public List<Widget> GetAllChildrenRecursive(Func<Widget, bool> predicate = null)
		{
			List<Widget> list = new List<Widget>();
			this.GetAllChildrenAux(list, predicate);
			return list;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00020E9C File Offset: 0x0001F09C
		private void GetAllChildrenAux(List<Widget> list, Func<Widget, bool> predicate = null)
		{
			for (int i = 0; i < this._children.Count; i++)
			{
				if (predicate == null || predicate(this._children[i]))
				{
					list.Add(this._children[i]);
				}
				this._children[i].GetAllChildrenAux(list, null);
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00020EFB File Offset: 0x0001F0FB
		public List<Widget> Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00020F04 File Offset: 0x0001F104
		public List<Widget> GetAllParents()
		{
			List<Widget> list = new List<Widget>();
			for (Widget widget = this.ParentWidget; widget != null; widget = widget.ParentWidget)
			{
				list.Add(widget);
			}
			return list;
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00020F32 File Offset: 0x0001F132
		public bool ConnectedToRoot
		{
			get
			{
				return this.Id == "Root" || (this.ParentWidget != null && this.ParentWidget.ConnectedToRoot);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x00020F5D File Offset: 0x0001F15D
		// (set) Token: 0x0600083D RID: 2109 RVA: 0x00020F65 File Offset: 0x0001F165
		[Editor(false)]
		public float MaxWidth
		{
			get
			{
				return this._maxWidth;
			}
			set
			{
				if (this._maxWidth != value)
				{
					this._maxWidth = value;
					this._gotMaxWidth = true;
					base.OnPropertyChanged(value, "MaxWidth");
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00020F8A File Offset: 0x0001F18A
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x00020F92 File Offset: 0x0001F192
		[Editor(false)]
		public float MaxHeight
		{
			get
			{
				return this._maxHeight;
			}
			set
			{
				if (this._maxHeight != value)
				{
					this._maxHeight = value;
					this._gotMaxHeight = true;
					base.OnPropertyChanged(value, "MaxHeight");
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x00020FB7 File Offset: 0x0001F1B7
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x00020FBF File Offset: 0x0001F1BF
		[Editor(false)]
		public float MinWidth
		{
			get
			{
				return this._minWidth;
			}
			set
			{
				if (this._minWidth != value)
				{
					this._minWidth = value;
					this._gotMinWidth = true;
					base.OnPropertyChanged(value, "MinWidth");
				}
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x00020FEC File Offset: 0x0001F1EC
		[Editor(false)]
		public float MinHeight
		{
			get
			{
				return this._minHeight;
			}
			set
			{
				if (this._minHeight != value)
				{
					this._minHeight = value;
					this._gotMinHeight = true;
					base.OnPropertyChanged(value, "MinHeight");
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x00021011 File Offset: 0x0001F211
		public float ScaledMaxWidth
		{
			get
			{
				return this._scaleToUse * this._maxWidth;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x00021020 File Offset: 0x0001F220
		public float ScaledMaxHeight
		{
			get
			{
				return this._scaleToUse * this._maxHeight;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0002102F File Offset: 0x0001F22F
		public float ScaledMinWidth
		{
			get
			{
				return this._scaleToUse * this._minWidth;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0002103E File Offset: 0x0001F23E
		public float ScaledMinHeight
		{
			get
			{
				return this._scaleToUse * this._minHeight;
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00021050 File Offset: 0x0001F250
		public Widget(UIContext context)
		{
			this.DropEventHandledManually = true;
			this.LayoutImp = new DefaultLayout();
			this._children = new List<Widget>();
			this._childRenderBuffer = new List<Widget>();
			this.Context = context;
			this._states = new List<string>();
			this.WidgetInfo = WidgetInfo.GetWidgetInfo(base.GetType());
			this.Sprite = null;
			this.ImageFit = new ImageFit();
			this.AreaRect = Rectangle2D.Create();
			this._isGamepadCursorAreaDirty = true;
			this._stateTimer = 0f;
			this._currentVisualStateAnimationState = VisualStateAnimationState.None;
			this._isFocusable = false;
			this._seed = 0;
			this._components = new List<WidgetComponent>();
			this.AddState("Default");
			this.SetState("Default");
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00021154 File Offset: 0x0001F354
		public T GetComponent<T>() where T : WidgetComponent
		{
			for (int i = 0; i < this._components.Count; i++)
			{
				WidgetComponent widgetComponent = this._components[i];
				if (widgetComponent is T)
				{
					return (T)((object)widgetComponent);
				}
			}
			return default(T);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0002119C File Offset: 0x0001F39C
		public void AddComponent(WidgetComponent component)
		{
			this._components.Add(component);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000211AA File Offset: 0x0001F3AA
		protected void SetMeasureAndLayoutDirty()
		{
			this.SetMeasureDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000211B8 File Offset: 0x0001F3B8
		protected void SetMeasureDirty()
		{
			this.EventManager.SetMeasureDirty();
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000211C5 File Offset: 0x0001F3C5
		protected void SetLayoutDirty()
		{
			this.EventManager.SetLayoutDirty();
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000211D4 File Offset: 0x0001F3D4
		internal void LayoutUpdated()
		{
			this.OnLayoutUpdated();
			for (int i = 0; i < this.ChildCount; i++)
			{
				Widget child = this.GetChild(i);
				if (child != null)
				{
					child.LayoutUpdated();
				}
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0002120A File Offset: 0x0001F40A
		protected virtual void OnLayoutUpdated()
		{
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002120C File Offset: 0x0001F40C
		public void AddState(string stateName)
		{
			if (!this._states.Contains(stateName))
			{
				this._states.Add(stateName);
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00021228 File Offset: 0x0001F428
		public bool ContainsState(string stateName)
		{
			return this._states.Contains(stateName);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00021238 File Offset: 0x0001F438
		public virtual void SetState(string stateName)
		{
			if (this.CurrentState != stateName)
			{
				this.CurrentState = stateName;
				this._stateTimer = 0f;
				if (this._currentVisualStateAnimationState != VisualStateAnimationState.None)
				{
					this._startVisualState = new VisualState("@StartState");
					this._startVisualState.FillFromWidget(this);
				}
				this._currentVisualStateAnimationState = VisualStateAnimationState.PlayingBasicTranisition;
			}
			if (this.UpdateChildrenStates)
			{
				for (int i = 0; i < this.ChildCount; i++)
				{
					Widget child = this.GetChild(i);
					if (!(child is ImageWidget) || !((ImageWidget)child).OverrideDefaultStateSwitchingEnabled)
					{
						child.SetState(this.CurrentState);
					}
				}
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x000212D4 File Offset: 0x0001F4D4
		public Widget FindChild(BindingPath path)
		{
			string firstNode = path.FirstNode;
			BindingPath subPath = path.SubPath;
			if (firstNode == "..")
			{
				return this.ParentWidget.FindChild(subPath);
			}
			if (firstNode == ".")
			{
				return this;
			}
			int i = 0;
			while (i < this._children.Count)
			{
				Widget widget = this._children[i];
				if (!string.IsNullOrEmpty(widget.Id) && widget.Id == firstNode)
				{
					if (subPath == null)
					{
						return widget;
					}
					return widget.FindChild(subPath);
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002136C File Offset: 0x0001F56C
		public Widget FindChild(string singlePathNode)
		{
			if (singlePathNode == "..")
			{
				return this.ParentWidget;
			}
			if (singlePathNode == ".")
			{
				return this;
			}
			for (int i = 0; i < this._children.Count; i++)
			{
				Widget widget = this._children[i];
				if (!string.IsNullOrEmpty(widget.Id) && widget.Id == singlePathNode)
				{
					return widget;
				}
			}
			return null;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000213E0 File Offset: 0x0001F5E0
		public Widget FindChild(WidgetSearchDelegate widgetSearchDelegate)
		{
			for (int i = 0; i < this._children.Count; i++)
			{
				Widget widget = this._children[i];
				if (widgetSearchDelegate(widget))
				{
					return widget;
				}
			}
			return null;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0002141C File Offset: 0x0001F61C
		public Widget FindChild(string id, bool includeAllChildren = false)
		{
			List<Widget> list = (includeAllChildren ? this.GetAllChildrenRecursive(null) : this._children);
			for (int i = 0; i < list.Count; i++)
			{
				Widget widget = list[i];
				if (!string.IsNullOrEmpty(widget.Id) && widget.Id == id)
				{
					return widget;
				}
			}
			return null;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00021474 File Offset: 0x0001F674
		public Widget GetFirstInChildrenAndThisRecursive(Func<Widget, bool> predicate)
		{
			if (predicate(this))
			{
				return this;
			}
			for (int i = 0; i < this._children.Count; i++)
			{
				Widget firstInChildrenAndThisRecursive = this._children[i].GetFirstInChildrenAndThisRecursive(predicate);
				if (firstInChildrenAndThisRecursive != null)
				{
					return firstInChildrenAndThisRecursive;
				}
			}
			return null;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x000214BC File Offset: 0x0001F6BC
		public Widget GetFirstInChildrenRecursive(Func<Widget, bool> predicate)
		{
			for (int i = 0; i < this._children.Count; i++)
			{
				if (predicate(this._children[i]))
				{
					return this._children[i];
				}
				Widget firstInChildrenRecursive = this._children[i].GetFirstInChildrenRecursive(predicate);
				if (firstInChildrenRecursive != null)
				{
					return firstInChildrenRecursive;
				}
			}
			return null;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002151C File Offset: 0x0001F71C
		public List<T> FindChildrenWithId<T>(string id, bool includeAllChildren = false) where T : Widget
		{
			List<Widget> list = (includeAllChildren ? this.GetAllChildrenRecursive(null) : this._children);
			List<T> list2 = new List<T>();
			for (int i = 0; i < list.Count; i++)
			{
				T t;
				if (!string.IsNullOrEmpty(list[i].Id) && (t = list[i] as T) != null && list[i].Id == id)
				{
					list2.Add(t);
				}
			}
			return list2;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002159C File Offset: 0x0001F79C
		public List<T> FindChildrenWithType<T>(bool includeAllChildren = false) where T : Widget
		{
			List<Widget> list = (includeAllChildren ? this.GetAllChildrenRecursive(null) : this._children);
			List<T> list2 = new List<T>();
			for (int i = 0; i < list.Count; i++)
			{
				T t;
				if (!string.IsNullOrEmpty(list[i].Id) && (t = list[i] as T) != null)
				{
					list2.Add(t);
				}
			}
			return list2;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00021608 File Offset: 0x0001F808
		public void RemoveAllChildren()
		{
			while (this._children.Count > 0)
			{
				this._children[0].ParentWidget = null;
			}
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002162C File Offset: 0x0001F82C
		private static float GetEaseOutBack(float t)
		{
			float num = 0.5f;
			float num2 = num + 1f;
			return 1f + num2 * MathF.Pow(t - 1f, 3f) + num * MathF.Pow(t - 1f, 2f);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00021674 File Offset: 0x0001F874
		internal void UpdateVisualDefinitions(float dt)
		{
			if (this.VisualDefinition != null && this._currentVisualStateAnimationState == VisualStateAnimationState.PlayingBasicTranisition)
			{
				if (this._startVisualState == null)
				{
					this._startVisualState = new VisualState("@StartState");
					this._startVisualState.FillFromWidget(this);
				}
				VisualState visualState = this.VisualDefinition.GetVisualState(this.CurrentState);
				if (visualState != null)
				{
					float num = (visualState.GotTransitionDuration ? visualState.TransitionDuration : this.VisualDefinition.TransitionDuration);
					float delayOnBegin = this.VisualDefinition.DelayOnBegin;
					if (this._stateTimer < num)
					{
						if (this._stateTimer >= delayOnBegin)
						{
							float num2 = (this._stateTimer - delayOnBegin) / (num - delayOnBegin);
							num2 = AnimationInterpolation.Ease(this.VisualDefinition.EaseType, this.VisualDefinition.EaseFunction, num2);
							this.PositionXOffset = (visualState.GotPositionXOffset ? Mathf.Lerp(this._startVisualState.PositionXOffset, visualState.PositionXOffset, num2) : this.PositionXOffset);
							this.PositionYOffset = (visualState.GotPositionYOffset ? Mathf.Lerp(this._startVisualState.PositionYOffset, visualState.PositionYOffset, num2) : this.PositionYOffset);
							this.SuggestedWidth = (visualState.GotSuggestedWidth ? Mathf.Lerp(this._startVisualState.SuggestedWidth, visualState.SuggestedWidth, num2) : this.SuggestedWidth);
							this.SuggestedHeight = (visualState.GotSuggestedHeight ? Mathf.Lerp(this._startVisualState.SuggestedHeight, visualState.SuggestedHeight, num2) : this.SuggestedHeight);
							this.MarginTop = (visualState.GotMarginTop ? Mathf.Lerp(this._startVisualState.MarginTop, visualState.MarginTop, num2) : this.MarginTop);
							this.MarginBottom = (visualState.GotMarginBottom ? Mathf.Lerp(this._startVisualState.MarginBottom, visualState.MarginBottom, num2) : this.MarginBottom);
							this.MarginLeft = (visualState.GotMarginLeft ? Mathf.Lerp(this._startVisualState.MarginLeft, visualState.MarginLeft, num2) : this.MarginLeft);
							this.MarginRight = (visualState.GotMarginRight ? Mathf.Lerp(this._startVisualState.MarginRight, visualState.MarginRight, num2) : this.MarginRight);
						}
					}
					else
					{
						this.PositionXOffset = (visualState.GotPositionXOffset ? visualState.PositionXOffset : this.PositionXOffset);
						this.PositionYOffset = (visualState.GotPositionYOffset ? visualState.PositionYOffset : this.PositionYOffset);
						this.SuggestedWidth = (visualState.GotSuggestedWidth ? visualState.SuggestedWidth : this.SuggestedWidth);
						this.SuggestedHeight = (visualState.GotSuggestedHeight ? visualState.SuggestedHeight : this.SuggestedHeight);
						this.MarginTop = (visualState.GotMarginTop ? visualState.MarginTop : this.MarginTop);
						this.MarginBottom = (visualState.GotMarginBottom ? visualState.MarginBottom : this.MarginBottom);
						this.MarginLeft = (visualState.GotMarginLeft ? visualState.MarginLeft : this.MarginLeft);
						this.MarginRight = (visualState.GotMarginRight ? visualState.MarginRight : this.MarginRight);
						this._startVisualState = visualState;
						this._currentVisualStateAnimationState = VisualStateAnimationState.None;
					}
				}
				else
				{
					this._currentVisualStateAnimationState = VisualStateAnimationState.None;
				}
			}
			this._stateTimer += dt;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000219B0 File Offset: 0x0001FBB0
		internal void Update(float dt)
		{
			this.OnUpdate(dt);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000219B9 File Offset: 0x0001FBB9
		internal void LateUpdate(float dt)
		{
			this.OnLateUpdate(dt);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000219C2 File Offset: 0x0001FBC2
		internal void ParallelUpdate(float dt)
		{
			if (!this._isInParallelOperation)
			{
				this._isInParallelOperation = true;
				this.OnParallelUpdate(dt);
				this._isInParallelOperation = false;
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000219E1 File Offset: 0x0001FBE1
		protected virtual void OnUpdate(float dt)
		{
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x000219E3 File Offset: 0x0001FBE3
		protected virtual void OnParallelUpdate(float dt)
		{
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000219E5 File Offset: 0x0001FBE5
		protected virtual void OnLateUpdate(float dt)
		{
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000219E7 File Offset: 0x0001FBE7
		protected virtual void RefreshState()
		{
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x000219EC File Offset: 0x0001FBEC
		public virtual void UpdateAnimationPropertiesSubTask(float alphaFactor)
		{
			this.AlphaFactor = alphaFactor;
			for (int i = 0; i < this._children.Count; i++)
			{
				this._children[i].UpdateAnimationPropertiesSubTask(alphaFactor);
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00021A28 File Offset: 0x0001FC28
		public void Measure(Vector2 measureSpec)
		{
			if (this.IsHidden)
			{
				this.MeasuredSize = Vector2.Zero;
				return;
			}
			this.OnMeasure(measureSpec);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00021A48 File Offset: 0x0001FC48
		private Vector2 ProcessSizeWithBoundaries(Vector2 input)
		{
			Vector2 vector = input;
			if (this._gotMinWidth && input.X < this.ScaledMinWidth)
			{
				vector.X = this.ScaledMinWidth;
			}
			if (this._gotMinHeight && input.Y < this.ScaledMinHeight)
			{
				vector.Y = this.ScaledMinHeight;
			}
			if (this._gotMaxWidth && input.X > this.ScaledMaxWidth)
			{
				vector.X = this.ScaledMaxWidth;
			}
			if (this._gotMaxHeight && input.Y > this.ScaledMaxHeight)
			{
				vector.Y = this.ScaledMaxHeight;
			}
			return vector;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00021AE4 File Offset: 0x0001FCE4
		private void OnMeasure(Vector2 measureSpec)
		{
			if (this.UseSpriteDimensions)
			{
				this.WidthSizePolicy = SizePolicy.Fixed;
				this.HeightSizePolicy = SizePolicy.Fixed;
				Sprite sprite = this.Sprite;
				int? num = ((sprite != null) ? new int?(sprite.Width) : null);
				this.SuggestedWidth = ((num != null) ? ((float)num.GetValueOrDefault()) : 0f);
				Sprite sprite2 = this.Sprite;
				num = ((sprite2 != null) ? new int?(sprite2.Height) : null);
				this.SuggestedHeight = ((num != null) ? ((float)num.GetValueOrDefault()) : 0f);
			}
			if (this.WidthSizePolicy == SizePolicy.Fixed)
			{
				measureSpec.X = this.ScaledSuggestedWidth;
			}
			else if (this.WidthSizePolicy == SizePolicy.StretchToParent)
			{
				measureSpec.X -= this.ScaledMarginLeft + this.ScaledMarginRight;
			}
			else
			{
				SizePolicy widthSizePolicy = this.WidthSizePolicy;
			}
			if (this.HeightSizePolicy == SizePolicy.Fixed)
			{
				measureSpec.Y = this.ScaledSuggestedHeight;
			}
			else if (this.HeightSizePolicy == SizePolicy.StretchToParent)
			{
				measureSpec.Y -= this.ScaledMarginTop + this.ScaledMarginBottom;
			}
			else
			{
				SizePolicy heightSizePolicy = this.HeightSizePolicy;
			}
			measureSpec = this.ProcessSizeWithBoundaries(measureSpec);
			Vector2 vector = this.MeasureChildren(measureSpec);
			Vector2 vector2 = new Vector2(0f, 0f);
			if (this.WidthSizePolicy == SizePolicy.Fixed)
			{
				vector2.X = this.ScaledSuggestedWidth;
			}
			else if (this.WidthSizePolicy == SizePolicy.CoverChildren)
			{
				vector2.X = vector.X;
			}
			else if (this.WidthSizePolicy == SizePolicy.StretchToParent)
			{
				vector2.X = measureSpec.X;
			}
			if (this.HeightSizePolicy == SizePolicy.Fixed)
			{
				vector2.Y = this.ScaledSuggestedHeight;
			}
			else if (this.HeightSizePolicy == SizePolicy.CoverChildren)
			{
				vector2.Y = vector.Y;
			}
			else if (this.HeightSizePolicy == SizePolicy.StretchToParent)
			{
				vector2.Y = measureSpec.Y;
			}
			vector2 = this.ProcessSizeWithBoundaries(vector2);
			this.MeasuredSize = vector2;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00021CC8 File Offset: 0x0001FEC8
		public bool CheckIsMyChildRecursive(Widget child)
		{
			for (Widget widget = ((child != null) ? child.ParentWidget : null); widget != null; widget = widget.ParentWidget)
			{
				if (widget == this)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00021CF5 File Offset: 0x0001FEF5
		private Vector2 MeasureChildren(Vector2 measureSpec)
		{
			return this.LayoutImp.MeasureChildren(this, measureSpec, this.Context.SpriteData, this._scaleToUse);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00021D15 File Offset: 0x0001FF15
		public void AddChild(Widget widget)
		{
			widget.ParentWidget = this;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00021D1E File Offset: 0x0001FF1E
		public void AddChildAtIndex(Widget widget, int index)
		{
			widget.ParentWidget = this;
			widget.SetSiblingIndex(index, false);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00021D30 File Offset: 0x0001FF30
		public void SwapChildren(Widget widget1, Widget widget2)
		{
			int num = this._children.IndexOf(widget1);
			int num2 = this._children.IndexOf(widget2);
			Widget widget3 = this._children[num];
			this._children[num] = this._children[num2];
			this._children[num2] = widget3;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00021D8C File Offset: 0x0001FF8C
		protected virtual void OnChildAdded(Widget child)
		{
			this.EventFired("ItemAdd", new object[] { child });
			if (this.DoNotUseCustomScaleAndChildren)
			{
				child.DoNotUseCustomScaleAndChildren = true;
			}
			if (this.UpdateChildrenStates && (!(child is ImageWidget) || !((ImageWidget)child).OverrideDefaultStateSwitchingEnabled))
			{
				child.SetState(this.CurrentState);
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00021DE6 File Offset: 0x0001FFE6
		public void RemoveChild(Widget widget)
		{
			widget.ParentWidget = null;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00021DF0 File Offset: 0x0001FFF0
		public virtual void OnBeforeRemovedChild(Widget widget)
		{
			if (this.IsHovered)
			{
				this.EventFired("HoverEnd", Array.Empty<object>());
			}
			for (int i = 0; i < this._children.Count; i++)
			{
				this._children[i].OnBeforeRemovedChild(widget);
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00021E3D File Offset: 0x0002003D
		public bool HasChild(Widget widget)
		{
			return this._children.Contains(widget);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00021E4B File Offset: 0x0002004B
		protected virtual void OnBeforeChildRemoved(Widget child)
		{
			this.EventFired("ItemRemove", new object[] { child });
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00021E62 File Offset: 0x00020062
		protected virtual void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			this.EventFired("AfterItemRemove", new object[] { child });
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00021E79 File Offset: 0x00020079
		public virtual void UpdateBrushes(float dt)
		{
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00021E7B File Offset: 0x0002007B
		public int GetChildIndex(Widget child)
		{
			return this._children.IndexOf(child);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00021E8C File Offset: 0x0002008C
		public int GetVisibleChildIndex(Widget child)
		{
			int num = -1;
			List<Widget> list = this._children.Where<Widget>((Widget c) => c.IsVisible).ToList<Widget>();
			if (list.Count > 0)
			{
				num = list.IndexOf(child);
			}
			return num;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00021EE0 File Offset: 0x000200E0
		public int GetFilterChildIndex(Widget child, Func<Widget, bool> childrenFilter)
		{
			int num = -1;
			List<Widget> list = this._children.Where<Widget>((Widget c) => childrenFilter(c)).ToList<Widget>();
			if (list.Count > 0)
			{
				num = list.IndexOf(child);
			}
			return num;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00021F2B File Offset: 0x0002012B
		public Widget GetChild(int i)
		{
			if (i < this._children.Count)
			{
				return this._children[i];
			}
			return null;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00021F4C File Offset: 0x0002014C
		public void Layout(float left, float bottom, float right, float top)
		{
			if (this.IsVisible)
			{
				this.SetLayout(left, bottom, right, top);
				Vector2 scaledPositionOffset = this.ScaledPositionOffset;
				this.Left += scaledPositionOffset.X;
				this.Top += scaledPositionOffset.Y;
				this.OnLayout(this.Left, this.Bottom, this.Right, this.Top);
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00021FB8 File Offset: 0x000201B8
		private void SetLayout(float left, float bottom, float right, float top)
		{
			left += this.ScaledMarginLeft;
			right -= this.ScaledMarginRight;
			top += this.ScaledMarginTop;
			bottom -= this.ScaledMarginBottom;
			float num = right - left;
			float num2 = bottom - top;
			float num3;
			if (this.HorizontalAlignment == HorizontalAlignment.Left)
			{
				num3 = left;
			}
			else if (this.HorizontalAlignment == HorizontalAlignment.Center)
			{
				num3 = left + num / 2f - this.MeasuredSize.X / 2f;
			}
			else
			{
				num3 = right - this.MeasuredSize.X;
			}
			float num4;
			if (this.VerticalAlignment == VerticalAlignment.Top)
			{
				num4 = top;
			}
			else if (this.VerticalAlignment == VerticalAlignment.Center)
			{
				num4 = top + num2 / 2f - this.MeasuredSize.Y / 2f;
			}
			else
			{
				num4 = bottom - this.MeasuredSize.Y;
			}
			this.Left = num3;
			this.Top = num4;
			this.Size = this.MeasuredSize;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00022095 File Offset: 0x00020295
		private void OnLayout(float left, float bottom, float right, float top)
		{
			this.LayoutImp.OnLayout(this, left, bottom, right, top);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000220A8 File Offset: 0x000202A8
		private void ParallelUpdateChildPositions()
		{
			TWParallel.ForWithoutRenderThread(0, this._children.Count, new TWParallel.ParallelForAuxPredicate(this.<ParallelUpdateChildPositions>g__UpdateChildPositionMT|483_0), 16);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000220C9 File Offset: 0x000202C9
		internal void UpdatePosition()
		{
			if (this.IsVisible)
			{
				this.LocalPosition = new Vector2(this.Left, this.Top);
				this.OnUpdatePosition();
				this._isGamepadCursorAreaDirty = true;
				this.OnUpdateChildPositions();
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00022100 File Offset: 0x00020300
		protected virtual void OnUpdatePosition()
		{
			Widget parentWidget = this.ParentWidget;
			Rectangle2D rectangle2D = ((parentWidget != null) ? parentWidget.AreaRect : this.EventManager.AreaRectangle);
			this.AreaRect.LocalPosition = new Vector2(this.LocalPosition.X, this.LocalPosition.Y);
			this.AreaRect.LocalPivot = new Vector2(this.PivotX, this.PivotY);
			this.AreaRect.LocalScale = new Vector2(this.Size.X, this.Size.Y);
			this.AreaRect.LocalRotation = this.Rotation;
			this.AreaRect.CalculateMatrixFrame(in rectangle2D);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000221B0 File Offset: 0x000203B0
		protected virtual void OnUpdateChildPositions()
		{
			if (this._children.Count >= 64)
			{
				this.ParallelUpdateChildPositions();
				return;
			}
			for (int i = 0; i < this._children.Count; i++)
			{
				this._children[i].UpdatePosition();
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x000221FA File Offset: 0x000203FA
		public virtual void HandleInput(IReadOnlyList<int> lastKeysPressed)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000221FC File Offset: 0x000203FC
		public bool IsPointInsideMeasuredArea(Vector2 p)
		{
			return this.AreaRect.IsPointInside(in p);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002220C File Offset: 0x0002040C
		public bool IsPointInsideGamepadCursorArea(Vector2 p)
		{
			return this.GamepadCursorAreaRect.IsPointInside(in p);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00022229 File Offset: 0x00020429
		public void Hide()
		{
			this.IsHidden = true;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00022232 File Offset: 0x00020432
		public void Show()
		{
			this.IsHidden = false;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0002223B File Offset: 0x0002043B
		public Vector2 GetLocalPoint(Vector2 globalPoint)
		{
			return globalPoint - this.GlobalPosition;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0002224C File Offset: 0x0002044C
		public void SetSiblingIndex(int index, bool force = false)
		{
			int siblingIndex = this.GetSiblingIndex();
			if (siblingIndex != index || force)
			{
				this.ParentWidget._children.RemoveAt(siblingIndex);
				this.ParentWidget._children.Insert(index, this);
				this.SetMeasureAndLayoutDirty();
				this.EventFired("SiblingIndexChanged", Array.Empty<object>());
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000222A4 File Offset: 0x000204A4
		public int GetSiblingIndex()
		{
			Widget parentWidget = this.ParentWidget;
			if (parentWidget == null)
			{
				return -1;
			}
			return parentWidget.GetChildIndex(this);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000222B8 File Offset: 0x000204B8
		public int GetVisibleSiblingIndex()
		{
			return this.ParentWidget.GetVisibleChildIndex(this);
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x000222C6 File Offset: 0x000204C6
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x000222CE File Offset: 0x000204CE
		public bool DisableRender { get; set; }

		// Token: 0x0600088B RID: 2187 RVA: 0x000222D8 File Offset: 0x000204D8
		public void Render(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (!this.IsHidden && !this.DisableRender)
			{
				bool flag = this.ClipHorizontalContent || this.ClipVerticalContent;
				if (flag)
				{
					drawContext.PushScissor(in this.AreaRect);
				}
				if (this.CircularClipEnabled)
				{
					if (this.IsCircularClipRadiusHalfOfHeight)
					{
						this.CircularClipRadius = this.Size.Y * 0.5f * this._inverseScaleToUse;
					}
					else if (this.IsCircularClipRadiusHalfOfWidth)
					{
						this.CircularClipRadius = this.Size.X * 0.5f * this._inverseScaleToUse;
					}
					Vector2 center = this.AreaRect.GetCenter();
					Vector2 vector = new Vector2(center.X + this.CircularClipXOffset * this._scaleToUse, center.Y + this.CircularClipYOffset * this._scaleToUse);
					drawContext.SetCircualMask(vector, this.CircularClipRadius * this._scaleToUse, this.CircularClipSmoothingRadius * this._scaleToUse);
				}
				bool flag2 = false;
				if (drawContext.ScissorTestEnabled)
				{
					if (this._calculateSizeFirstFrame || !drawContext.IsDiscardedByAnyScissor(in this.AreaRect))
					{
						flag2 = !this.DoNotRenderIfNotFullyInsideScissor || this.AreaRect.IsSubRectOf(in this.EventManager.AreaRectangle);
					}
				}
				else if (this._calculateSizeFirstFrame || this.AreaRect.IsCollide(in this.EventManager.AreaRectangle))
				{
					flag2 = true;
				}
				if (flag2)
				{
					this._isRendering = true;
					this.OnRender(twoDimensionContext, drawContext);
					for (int i = 0; i < this._children.Count; i++)
					{
						Widget widget = this._children[i];
						if (widget != null)
						{
							this._childRenderBuffer.Add(widget);
						}
					}
					for (int j = 0; j < this._childRenderBuffer.Count; j++)
					{
						Widget widget2 = this._childRenderBuffer[j];
						if (!widget2.RenderLate)
						{
							widget2.Render(twoDimensionContext, drawContext);
						}
					}
					for (int k = 0; k < this._childRenderBuffer.Count; k++)
					{
						Widget widget3 = this._childRenderBuffer[k];
						if (widget3.RenderLate)
						{
							widget3.Render(twoDimensionContext, drawContext);
						}
					}
					this._childRenderBuffer.Clear();
					this._isRendering = false;
				}
				if (this.CircularClipEnabled)
				{
					drawContext.ClearCircualMask();
				}
				if (flag)
				{
					drawContext.PopScissor();
				}
			}
			this._calculateSizeFirstFrame = false;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00022530 File Offset: 0x00020730
		protected virtual void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			Sprite sprite = this._sprite;
			if (((sprite != null) ? sprite.Texture : null) == null)
			{
				return;
			}
			ImageFit imageFit = this.ImageFit;
			Vector2 size = this.Size;
			Vector2 vector = new Vector2((float)this.Sprite.Width, (float)this.Sprite.Height);
			ImageFitResult fittedRectangle = imageFit.GetFittedRectangle(in size, in vector);
			float num = this.LocalPosition.X;
			float num2 = this.LocalPosition.Y;
			if (this.ForcePixelPerfectRenderPlacement)
			{
				num = (float)MathF.Round(num);
				num2 = (float)MathF.Round(num2);
			}
			SimpleMaterial simpleMaterial = drawContext.CreateSimpleMaterial();
			simpleMaterial.OverlayEnabled = false;
			simpleMaterial.CircularMaskingEnabled = false;
			simpleMaterial.Texture = this._sprite.Texture;
			simpleMaterial.NinePatchParameters = this._sprite.NinePatchParameters;
			if (this.NinePatchLeft != 0 || this.NinePatchRight != 0 || this.NinePatchTop != 0 || this.NinePatchBottom != 0)
			{
				simpleMaterial.NinePatchParameters = new SpriteNinePatchParameters(this.NinePatchLeft, this.NinePatchRight, this.NinePatchTop, this.NinePatchBottom);
			}
			simpleMaterial.Color = this.Color;
			simpleMaterial.ColorFactor = this.ColorFactor;
			simpleMaterial.AlphaFactor = this.AlphaFactor * this.Context.ContextAlpha;
			simpleMaterial.HueFactor = 0f;
			simpleMaterial.SaturationFactor = this.SaturationFactor;
			simpleMaterial.ValueFactor = this.ValueFactor;
			float num3 = this.ExtendLeft;
			if (this.HorizontalFlip)
			{
				num3 = this.ExtendRight;
			}
			float num4 = fittedRectangle.Width;
			num4 += (this.ExtendRight + this.ExtendLeft) * this._scaleToUse;
			num -= num3 * this._scaleToUse;
			float num5 = fittedRectangle.Height;
			float num6 = this.ExtendTop;
			if (this.VerticalFlip)
			{
				num6 = this.ExtendBottom;
			}
			num5 += (this.ExtendTop + this.ExtendBottom) * this._scaleToUse;
			num2 -= num6 * this._scaleToUse;
			num4 = (this.HorizontalFlip ? (-num4) : num4);
			num5 = (this.VerticalFlip ? (-num5) : num5);
			float num7 = ((num4 == 0f) ? 1f : (num4 / this.Size.X));
			float num8 = ((num5 == 0f) ? 1f : (num5 / this.Size.Y));
			this.AreaRect.SetVisualOffset(num - this.LocalPosition.X + fittedRectangle.OffsetX, num2 - this.LocalPosition.Y + fittedRectangle.OffsetY);
			this.AreaRect.SetVisualScale(num7, num8);
			this.AreaRect.ValidateVisuals();
			drawContext.DrawSprite(this._sprite, simpleMaterial, in this.AreaRect, this._scaleToUse);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x000227D8 File Offset: 0x000209D8
		protected void EventFired(string eventName, params object[] args)
		{
			if (this._eventTargets != null)
			{
				for (int i = 0; i < this._eventTargets.Count; i++)
				{
					this._eventTargets[i](this, eventName, args);
				}
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600088E RID: 2190 RVA: 0x00022817 File Offset: 0x00020A17
		// (remove) Token: 0x0600088F RID: 2191 RVA: 0x00022838 File Offset: 0x00020A38
		public event Action<Widget, string, object[]> EventFire
		{
			add
			{
				if (this._eventTargets == null)
				{
					this._eventTargets = new List<Action<Widget, string, object[]>>();
				}
				this._eventTargets.Add(value);
			}
			remove
			{
				if (this._eventTargets != null)
				{
					this._eventTargets.Remove(value);
				}
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000890 RID: 2192 RVA: 0x00022850 File Offset: 0x00020A50
		// (remove) Token: 0x06000891 RID: 2193 RVA: 0x00022888 File Offset: 0x00020A88
		public event Action<Widget> OnVisibilityChanged;

		// Token: 0x06000892 RID: 2194 RVA: 0x000228C0 File Offset: 0x00020AC0
		public bool IsRecursivelyVisible()
		{
			for (Widget widget = this; widget != null; widget = widget.ParentWidget)
			{
				if (!widget.IsVisible)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000228E6 File Offset: 0x00020AE6
		internal void HandleOnDisconnectedFromRoot()
		{
			this.OnDisconnectedFromRoot();
			if (this.IsHovered)
			{
				this.EventFired("HoverEnd", Array.Empty<object>());
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00022906 File Offset: 0x00020B06
		internal void HandleOnConnectedToRoot()
		{
			if (!this._seedSet)
			{
				this._seed = this.GetSiblingIndex();
				this._seedSet = true;
			}
			this.OnConnectedToRoot();
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00022929 File Offset: 0x00020B29
		protected virtual void OnDisconnectedFromRoot()
		{
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002292B File Offset: 0x00020B2B
		protected virtual void OnConnectedToRoot()
		{
			this.EventFired("ConnectedToRoot", Array.Empty<object>());
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002293D File Offset: 0x00020B3D
		public override string ToString()
		{
			return this.GetFullIDPath();
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00022945 File Offset: 0x00020B45
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x0002294D File Offset: 0x00020B4D
		public float ExtendCursorAreaTop { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00022956 File Offset: 0x00020B56
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x0002295E File Offset: 0x00020B5E
		public float ExtendCursorAreaRight { get; set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00022967 File Offset: 0x00020B67
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x0002296F File Offset: 0x00020B6F
		public float ExtendCursorAreaBottom { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00022978 File Offset: 0x00020B78
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00022980 File Offset: 0x00020B80
		public float ExtendCursorAreaLeft { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00022989 File Offset: 0x00020B89
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00022991 File Offset: 0x00020B91
		public float CursorAreaXOffset { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0002299A File Offset: 0x00020B9A
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x000229A2 File Offset: 0x00020BA2
		public float CursorAreaYOffset { get; set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x000229AB File Offset: 0x00020BAB
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x000229B6 File Offset: 0x00020BB6
		public bool AcceptNavigation
		{
			get
			{
				return !this.DoNotAcceptNavigation;
			}
			set
			{
				if (value == this.DoNotAcceptNavigation)
				{
					this.DoNotAcceptNavigation = !value;
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x000229CB File Offset: 0x00020BCB
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x000229D3 File Offset: 0x00020BD3
		public bool DoNotAcceptNavigation
		{
			get
			{
				return this._doNotAcceptNavigation;
			}
			set
			{
				if (value != this._doNotAcceptNavigation)
				{
					this._doNotAcceptNavigation = value;
					this.GamepadNavigationContext.OnWidgetNavigationStatusChanged(this);
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x000229F1 File Offset: 0x00020BF1
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x000229F9 File Offset: 0x00020BF9
		public bool IsUsingNavigation
		{
			get
			{
				return this._isUsingNavigation;
			}
			set
			{
				if (value != this._isUsingNavigation)
				{
					this._isUsingNavigation = value;
					base.OnPropertyChanged(value, "IsUsingNavigation");
				}
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00022A17 File Offset: 0x00020C17
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00022A1F File Offset: 0x00020C1F
		public bool UseSiblingIndexForNavigation
		{
			get
			{
				return this._useSiblingIndexForNavigation;
			}
			set
			{
				if (value != this._useSiblingIndexForNavigation)
				{
					this._useSiblingIndexForNavigation = value;
					if (value)
					{
						this.GamepadNavigationIndex = this.GetSiblingIndex();
					}
				}
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00022A40 File Offset: 0x00020C40
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00022A48 File Offset: 0x00020C48
		public int GamepadNavigationIndex
		{
			get
			{
				return this._gamepadNavigationIndex;
			}
			set
			{
				if (value != this._gamepadNavigationIndex)
				{
					this._gamepadNavigationIndex = value;
					this.GamepadNavigationContext.OnWidgetNavigationIndexUpdated(this);
					this.OnGamepadNavigationIndexUpdated(value);
					base.OnPropertyChanged(value, "GamepadNavigationIndex");
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00022A79 File Offset: 0x00020C79
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00022A81 File Offset: 0x00020C81
		public GamepadNavigationTypes UsedNavigationMovements
		{
			get
			{
				return this._usedNavigationMovements;
			}
			set
			{
				if (value != this._usedNavigationMovements)
				{
					this._usedNavigationMovements = value;
					this.Context.GamepadNavigation.OnWidgetUsedNavigationMovementsUpdated(this);
				}
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00022AA4 File Offset: 0x00020CA4
		protected virtual void OnGamepadNavigationIndexUpdated(int newIndex)
		{
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00022AA6 File Offset: 0x00020CA6
		public void OnGamepadNavigationFocusGain()
		{
			Action<Widget> onGamepadNavigationFocusGained = this.OnGamepadNavigationFocusGained;
			if (onGamepadNavigationFocusGained == null)
			{
				return;
			}
			onGamepadNavigationFocusGained(this);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00022ABC File Offset: 0x00020CBC
		internal bool PreviewEvent(GauntletEvent gauntletEvent)
		{
			bool flag = false;
			switch (gauntletEvent)
			{
			case GauntletEvent.MouseMove:
				flag = this.OnPreviewMouseMove();
				break;
			case GauntletEvent.MousePressed:
				flag = this.OnPreviewMousePressed();
				break;
			case GauntletEvent.MouseReleased:
				flag = this.OnPreviewMouseReleased();
				break;
			case GauntletEvent.MouseAlternatePressed:
				flag = this.OnPreviewMouseAlternatePressed();
				break;
			case GauntletEvent.MouseAlternateReleased:
				flag = this.OnPreviewMouseAlternateReleased();
				break;
			case GauntletEvent.DragHover:
				flag = this.OnPreviewDragHover();
				break;
			case GauntletEvent.DragBegin:
				flag = this.OnPreviewDragBegin();
				break;
			case GauntletEvent.DragEnd:
				flag = this.OnPreviewDragEnd();
				break;
			case GauntletEvent.Drop:
				flag = this.OnPreviewDrop();
				break;
			case GauntletEvent.MouseScroll:
				flag = this.OnPreviewMouseScroll();
				break;
			case GauntletEvent.RightStickMovement:
				flag = this.OnPreviewRightStickMovement();
				break;
			}
			return flag;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00022B61 File Offset: 0x00020D61
		protected virtual bool OnPreviewMousePressed()
		{
			return true;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00022B64 File Offset: 0x00020D64
		protected virtual bool OnPreviewMouseReleased()
		{
			return true;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00022B67 File Offset: 0x00020D67
		protected virtual bool OnPreviewMouseAlternatePressed()
		{
			return true;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00022B6A File Offset: 0x00020D6A
		protected virtual bool OnPreviewMouseAlternateReleased()
		{
			return true;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00022B6D File Offset: 0x00020D6D
		protected virtual bool OnPreviewDragBegin()
		{
			return this.AcceptDrag;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00022B75 File Offset: 0x00020D75
		protected virtual bool OnPreviewDragEnd()
		{
			return this.AcceptDrag;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00022B7D File Offset: 0x00020D7D
		protected virtual bool OnPreviewDrop()
		{
			return this.AcceptDrop;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00022B85 File Offset: 0x00020D85
		protected virtual bool OnPreviewMouseScroll()
		{
			return false;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00022B88 File Offset: 0x00020D88
		protected virtual bool OnPreviewRightStickMovement()
		{
			return false;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00022B8B File Offset: 0x00020D8B
		protected virtual bool OnPreviewMouseMove()
		{
			return true;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00022B8E File Offset: 0x00020D8E
		protected virtual bool OnPreviewDragHover()
		{
			return false;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00022B91 File Offset: 0x00020D91
		protected internal virtual void OnMousePressed()
		{
			this.IsPressed = true;
			this.EventFired("MouseDown", Array.Empty<object>());
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00022BAA File Offset: 0x00020DAA
		protected internal virtual void OnMouseReleased(bool isFromInput)
		{
			this.IsPressed = false;
			this.EventFired("MouseUp", Array.Empty<object>());
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00022BC3 File Offset: 0x00020DC3
		protected internal virtual void OnMouseAlternatePressed()
		{
			this.EventFired("MouseAlternateDown", Array.Empty<object>());
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00022BD5 File Offset: 0x00020DD5
		protected internal virtual void OnMouseAlternateReleased(bool isFromInput)
		{
			this.EventFired("MouseAlternateUp", Array.Empty<object>());
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00022BE7 File Offset: 0x00020DE7
		protected internal virtual void OnMouseMove()
		{
			this.EventFired("MouseMove", Array.Empty<object>());
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00022BF9 File Offset: 0x00020DF9
		protected internal virtual void OnHoverBegin()
		{
			this.IsHovered = true;
			this.EventFired("HoverBegin", Array.Empty<object>());
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00022C12 File Offset: 0x00020E12
		protected internal virtual void OnHoverEnd()
		{
			this.EventFired("HoverEnd", Array.Empty<object>());
			this.IsHovered = false;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00022C2B File Offset: 0x00020E2B
		protected internal virtual void OnDragBegin()
		{
			this.EventManager.BeginDragging(this);
			this.EventFired("DragBegin", Array.Empty<object>());
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00022C49 File Offset: 0x00020E49
		protected internal virtual void OnDragEnd()
		{
			this.EventFired("DragEnd", Array.Empty<object>());
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00022C5C File Offset: 0x00020E5C
		protected internal virtual bool OnDrop()
		{
			if (this.AcceptDrop)
			{
				bool flag = true;
				if (this.AcceptDropHandler != null)
				{
					flag = this.AcceptDropHandler(this, this.EventManager.DraggedWidget);
				}
				if (flag)
				{
					Widget widget = this.EventManager.ReleaseDraggedWidget();
					int num = -1;
					if (!this.DropEventHandledManually)
					{
						widget.ParentWidget = this;
					}
					this.EventFired("Drop", new object[] { widget, num });
					return true;
				}
			}
			return false;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00022CD4 File Offset: 0x00020ED4
		protected internal virtual void OnMouseScroll()
		{
			this.EventFired("MouseScroll", Array.Empty<object>());
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00022CE6 File Offset: 0x00020EE6
		protected internal virtual void OnRightStickMovement()
		{
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00022CE8 File Offset: 0x00020EE8
		protected internal virtual void OnDragHoverBegin()
		{
			this.EventFired("DragHoverBegin", Array.Empty<object>());
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00022CFA File Offset: 0x00020EFA
		protected internal virtual void OnDragHoverEnd()
		{
			this.EventFired("DragHoverEnd", Array.Empty<object>());
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00022D0C File Offset: 0x00020F0C
		protected internal virtual void OnGainFocus()
		{
			this.IsFocused = true;
			this.EventFired("FocusGained", Array.Empty<object>());
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00022D25 File Offset: 0x00020F25
		protected internal virtual void OnLoseFocus()
		{
			this.IsFocused = false;
			this.EventFired("FocusLost", Array.Empty<object>());
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00022D3E File Offset: 0x00020F3E
		protected internal virtual void OnMouseOverBegin()
		{
			this.EventFired("MouseOverBegin", Array.Empty<object>());
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00022D50 File Offset: 0x00020F50
		protected internal virtual void OnMouseOverEnd()
		{
			this.EventFired("MouseOverEnd", Array.Empty<object>());
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00022D62 File Offset: 0x00020F62
		protected internal virtual void OnContextActivated()
		{
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00022D64 File Offset: 0x00020F64
		protected internal virtual void OnContextDeactivated()
		{
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00022D68 File Offset: 0x00020F68
		[CompilerGenerated]
		private void <ParallelUpdateChildPositions>g__UpdateChildPositionMT|483_0(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._children[i].UpdatePosition();
			}
		}

		// Token: 0x04000373 RID: 883
		private Color _color = Color.White;

		// Token: 0x04000374 RID: 884
		private List<WidgetComponent> _components;

		// Token: 0x04000384 RID: 900
		private float _rotation;

		// Token: 0x04000385 RID: 901
		private float _pivotX;

		// Token: 0x04000386 RID: 902
		private float _pivotY;

		// Token: 0x04000387 RID: 903
		private Vector2 _topLeft;

		// Token: 0x0400038A RID: 906
		private string _id;

		// Token: 0x0400038C RID: 908
		private bool _isGamepadCursorAreaDirty;

		// Token: 0x0400038D RID: 909
		private Rectangle2D _gamepadCursorAreaRect;

		// Token: 0x0400038E RID: 910
		public Rectangle2D AreaRect;

		// Token: 0x0400038F RID: 911
		private Widget _parent;

		// Token: 0x04000390 RID: 912
		private List<Widget> _children;

		// Token: 0x04000391 RID: 913
		private List<Widget> _childRenderBuffer;

		// Token: 0x04000392 RID: 914
		private bool _isRendering;

		// Token: 0x04000393 RID: 915
		private bool _doNotUseCustomScaleAndChildren;

		// Token: 0x04000395 RID: 917
		protected bool _calculateSizeFirstFrame = true;

		// Token: 0x04000396 RID: 918
		private float _suggestedWidth;

		// Token: 0x04000397 RID: 919
		private float _suggestedHeight;

		// Token: 0x04000398 RID: 920
		private string _hoveredCursorState;

		// Token: 0x04000399 RID: 921
		private bool _alternateClickEventHasSpecialEvent;

		// Token: 0x0400039A RID: 922
		private Vector2 _positionOffset;

		// Token: 0x0400039D RID: 925
		private float _marginTop;

		// Token: 0x0400039E RID: 926
		private float _marginLeft;

		// Token: 0x0400039F RID: 927
		private float _marginBottom;

		// Token: 0x040003A0 RID: 928
		private float _marginRight;

		// Token: 0x040003A1 RID: 929
		private VerticalAlignment _verticalAlignment;

		// Token: 0x040003A2 RID: 930
		private HorizontalAlignment _horizontalAlignment;

		// Token: 0x040003A3 RID: 931
		private bool _forcePixelPerfectRenderPlacement;

		// Token: 0x040003A6 RID: 934
		private SizePolicy _widthSizePolicy;

		// Token: 0x040003A7 RID: 935
		private SizePolicy _heightSizePolicy;

		// Token: 0x040003AB RID: 939
		private Widget _dragWidget;

		// Token: 0x040003B7 RID: 951
		private bool _isHovered;

		// Token: 0x040003B8 RID: 952
		private bool _isDisabled;

		// Token: 0x040003B9 RID: 953
		private bool _isFocusable;

		// Token: 0x040003BA RID: 954
		private bool _isFocused;

		// Token: 0x040003BB RID: 955
		private bool _restartAnimationFirstFrame;

		// Token: 0x040003BC RID: 956
		private bool _doNotPassEventsToChildren;

		// Token: 0x040003BD RID: 957
		private bool _doNotAcceptEvents;

		// Token: 0x040003BE RID: 958
		public Func<Widget, Widget, bool> AcceptDropHandler;

		// Token: 0x040003BF RID: 959
		private bool _isPressed;

		// Token: 0x040003C0 RID: 960
		private bool _isHidden;

		// Token: 0x040003C1 RID: 961
		private Sprite _sprite;

		// Token: 0x040003C2 RID: 962
		private VisualDefinition _visualDefinition;

		// Token: 0x040003C3 RID: 963
		private List<string> _states;

		// Token: 0x040003C4 RID: 964
		protected float _stateTimer;

		// Token: 0x040003C6 RID: 966
		protected VisualState _startVisualState;

		// Token: 0x040003C7 RID: 967
		protected VisualStateAnimationState _currentVisualStateAnimationState;

		// Token: 0x040003C8 RID: 968
		private bool _updateChildrenStates;

		// Token: 0x040003CD RID: 973
		protected int _seed;

		// Token: 0x040003CE RID: 974
		private bool _seedSet;

		// Token: 0x040003CF RID: 975
		private float _maxWidth;

		// Token: 0x040003D0 RID: 976
		private float _maxHeight;

		// Token: 0x040003D1 RID: 977
		private float _minWidth;

		// Token: 0x040003D2 RID: 978
		private float _minHeight;

		// Token: 0x040003D3 RID: 979
		private bool _gotMaxWidth;

		// Token: 0x040003D4 RID: 980
		private bool _gotMaxHeight;

		// Token: 0x040003D5 RID: 981
		private bool _gotMinWidth;

		// Token: 0x040003D6 RID: 982
		private bool _gotMinHeight;

		// Token: 0x040003D7 RID: 983
		private bool _isInParallelOperation;

		// Token: 0x040003D9 RID: 985
		private List<Action<Widget, string, object[]>> _eventTargets;

		// Token: 0x040003E1 RID: 993
		private bool _doNotAcceptNavigation;

		// Token: 0x040003E2 RID: 994
		private bool _isUsingNavigation;

		// Token: 0x040003E3 RID: 995
		private bool _useSiblingIndexForNavigation;

		// Token: 0x040003E4 RID: 996
		protected internal int _gamepadNavigationIndex = -1;

		// Token: 0x040003E5 RID: 997
		private GamepadNavigationTypes _usedNavigationMovements;

		// Token: 0x040003E6 RID: 998
		public Action<Widget> OnGamepadNavigationFocusGained;
	}
}
