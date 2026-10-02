using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information.RundownTooltip
{
	// Token: 0x02000149 RID: 329
	public class RundownColumnDividerCollectionWidget : ListPanel
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x00030D8C File Offset: 0x0002EF8C
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x00030D94 File Offset: 0x0002EF94
		public float DividerWidth { get; set; }

		// Token: 0x06001192 RID: 4498 RVA: 0x00030D9D File Offset: 0x0002EF9D
		public RundownColumnDividerCollectionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00030DC8 File Offset: 0x0002EFC8
		public void Refresh(IReadOnlyList<float> columnWidths)
		{
			base.RemoveAllChildren();
			for (int i = 0; i < columnWidths.Count - 1; i++)
			{
				Widget widget = this.CreateFixedSpaceWidget(columnWidths[i] * base._inverseScaleToUse - this.DividerWidth);
				base.AddChild(widget);
				base.AddChild(this.CreateDividerWidget());
			}
			base.AddChild(this.CreateStretchedSpaceWidget());
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00030E29 File Offset: 0x0002F029
		private Widget CreateFixedSpaceWidget(float width)
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.StretchToParent,
				SuggestedWidth = width
			};
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00030E4B File Offset: 0x0002F04B
		private Widget CreateStretchedSpaceWidget()
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.StretchToParent,
				HeightSizePolicy = SizePolicy.StretchToParent
			};
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00030E66 File Offset: 0x0002F066
		private Widget CreateDividerWidget()
		{
			return new Widget(base.Context)
			{
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.StretchToParent,
				SuggestedWidth = this.DividerWidth,
				Sprite = this.DividerSprite,
				Color = this.DividerColor
			};
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x00030EA5 File Offset: 0x0002F0A5
		// (set) Token: 0x06001198 RID: 4504 RVA: 0x00030EAD File Offset: 0x0002F0AD
		[Editor(false)]
		public Sprite DividerSprite
		{
			get
			{
				return this._dividerSprite;
			}
			set
			{
				if (value != this._dividerSprite)
				{
					this._dividerSprite = value;
					base.OnPropertyChanged<Sprite>(value, "DividerSprite");
				}
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x00030ECB File Offset: 0x0002F0CB
		// (set) Token: 0x0600119A RID: 4506 RVA: 0x00030ED3 File Offset: 0x0002F0D3
		[Editor(false)]
		public Color DividerColor
		{
			get
			{
				return this._dividerColor;
			}
			set
			{
				if (value != this._dividerColor)
				{
					this._dividerColor = value;
					base.OnPropertyChanged(value, "DividerColor");
				}
			}
		}

		// Token: 0x04000808 RID: 2056
		private Sprite _dividerSprite;

		// Token: 0x04000809 RID: 2057
		private Color _dividerColor = new Color(1f, 1f, 1f, 1f);
	}
}
