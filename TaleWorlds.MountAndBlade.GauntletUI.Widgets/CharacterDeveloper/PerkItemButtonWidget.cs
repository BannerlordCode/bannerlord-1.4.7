using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000182 RID: 386
	public class PerkItemButtonWidget : ButtonWidget
	{
		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x000367C8 File Offset: 0x000349C8
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x000367D0 File Offset: 0x000349D0
		public Brush NotEarnedPerkBrush { get; set; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x000367D9 File Offset: 0x000349D9
		// (set) Token: 0x060013FB RID: 5115 RVA: 0x000367E1 File Offset: 0x000349E1
		public Brush EarnedNotSelectedPerkBrush { get; set; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x000367EA File Offset: 0x000349EA
		// (set) Token: 0x060013FD RID: 5117 RVA: 0x000367F2 File Offset: 0x000349F2
		public Brush EarnedActivePerkBrush { get; set; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x000367FB File Offset: 0x000349FB
		// (set) Token: 0x060013FF RID: 5119 RVA: 0x00036803 File Offset: 0x00034A03
		public Brush EarnedNotActivePerkBrush { get; set; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x0003680C File Offset: 0x00034A0C
		// (set) Token: 0x06001401 RID: 5121 RVA: 0x00036814 File Offset: 0x00034A14
		public Brush EarnedPreviousPerkNotSelectedPerkBrush { get; set; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x0003681D File Offset: 0x00034A1D
		// (set) Token: 0x06001403 RID: 5123 RVA: 0x00036825 File Offset: 0x00034A25
		public BrushWidget PerkVisualWidgetParent { get; set; }

		// Token: 0x06001404 RID: 5124 RVA: 0x0003682E File Offset: 0x00034A2E
		public PerkItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00036840 File Offset: 0x00034A40
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.PerkVisualWidget != null && ((this.PerkVisualWidget.Sprite != null && base.Context.SpriteData.GetSprite(this.PerkVisualWidget.Sprite.Name) == null) || this.PerkVisualWidget.Sprite == null))
			{
				this.PerkVisualWidget.Sprite = base.Context.SpriteData.GetSprite("SPPerks\\locked_fallback");
			}
			if (this._animState == PerkItemButtonWidget.AnimState.Start)
			{
				this._tickCount++;
				if (this._tickCount > 20)
				{
					this._animState = PerkItemButtonWidget.AnimState.Starting;
					return;
				}
			}
			else if (this._animState == PerkItemButtonWidget.AnimState.Starting)
			{
				BrushWidget perkVisualWidgetParent = this.PerkVisualWidgetParent;
				if (perkVisualWidgetParent != null)
				{
					perkVisualWidgetParent.BrushRenderer.RestartAnimation();
				}
				this._animState = PerkItemButtonWidget.AnimState.Playing;
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00036908 File Offset: 0x00034B08
		private void SetColorState(bool isActive)
		{
			if (this.PerkVisualWidget != null)
			{
				float num = (isActive ? 1f : 1f);
				float num2 = (isActive ? 1.3f : 0.75f);
				List<BrushWidget> list = base.FindChildrenWithType<BrushWidget>(false);
				for (int i = 0; i < list.Count; i++)
				{
					foreach (Style style in list[i].Brush.Styles)
					{
						for (int j = 0; j < style.LayerCount; j++)
						{
							StyleLayer layer = style.GetLayer(j);
							layer.AlphaFactor = num;
							layer.ColorFactor = num2;
						}
					}
				}
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000369D4 File Offset: 0x00034BD4
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this._isSelectable)
			{
				base.Context.TwoDimensionContext.PlaySound("popup");
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000369FC File Offset: 0x00034BFC
		private void UpdatePerkStateVisual(int perkState)
		{
			if (this.PerkVisualWidgetParent == null)
			{
				return;
			}
			switch (perkState)
			{
			case 0:
				this.PerkVisualWidgetParent.Brush = this.NotEarnedPerkBrush;
				this._isSelectable = false;
				return;
			case 1:
			{
				this.PerkVisualWidgetParent.Brush = this.EarnedNotSelectedPerkBrush;
				this._animState = PerkItemButtonWidget.AnimState.Start;
				this._isSelectable = true;
				float transitionDuration = this.PerkVisualWidgetParent.Brush.TransitionDuration;
				return;
			}
			case 2:
				this.PerkVisualWidgetParent.Brush = this.EarnedActivePerkBrush;
				this._isSelectable = false;
				return;
			case 3:
				this.PerkVisualWidgetParent.Brush = this.EarnedNotActivePerkBrush;
				this._isSelectable = false;
				return;
			case 4:
				this.PerkVisualWidgetParent.Brush = this.EarnedPreviousPerkNotSelectedPerkBrush;
				this._isSelectable = false;
				return;
			default:
				Debug.FailedAssert("Perk visual state is not defined", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\CharacterDeveloper\\PerkItemButtonWidget.cs", "UpdatePerkStateVisual", 132);
				return;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00036ADF File Offset: 0x00034CDF
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x00036AE7 File Offset: 0x00034CE7
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (this._level != value)
				{
					this._level = value;
					base.OnPropertyChanged(value, "Level");
				}
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00036B05 File Offset: 0x00034D05
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x00036B0D File Offset: 0x00034D0D
		public Widget PerkVisualWidget
		{
			get
			{
				return this._perkVisualWidget;
			}
			set
			{
				if (this._perkVisualWidget != value)
				{
					this._perkVisualWidget = value;
					base.OnPropertyChanged<Widget>(value, "PerkVisualWidget");
				}
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x00036B2B File Offset: 0x00034D2B
		// (set) Token: 0x0600140E RID: 5134 RVA: 0x00036B33 File Offset: 0x00034D33
		public int PerkState
		{
			get
			{
				return this._perkState;
			}
			set
			{
				if (this._perkState != value)
				{
					this._perkState = value;
					base.OnPropertyChanged(value, "PerkState");
					this.UpdatePerkStateVisual(this.PerkState);
				}
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x00036B5D File Offset: 0x00034D5D
		// (set) Token: 0x06001410 RID: 5136 RVA: 0x00036B65 File Offset: 0x00034D65
		public int AlternativeType
		{
			get
			{
				return this._alternativeType;
			}
			set
			{
				if (this._alternativeType != value)
				{
					this._alternativeType = value;
					base.OnPropertyChanged(value, "AlternativeType");
				}
			}
		}

		// Token: 0x04000913 RID: 2323
		private PerkItemButtonWidget.AnimState _animState;

		// Token: 0x04000914 RID: 2324
		private int _tickCount;

		// Token: 0x04000915 RID: 2325
		private bool _isSelectable;

		// Token: 0x04000916 RID: 2326
		private int _level;

		// Token: 0x04000917 RID: 2327
		private int _alternativeType;

		// Token: 0x04000918 RID: 2328
		private int _perkState = -1;

		// Token: 0x04000919 RID: 2329
		private Widget _perkVisualWidget;

		// Token: 0x020001D6 RID: 470
		public enum AnimState
		{
			// Token: 0x04000A59 RID: 2649
			Idle,
			// Token: 0x04000A5A RID: 2650
			Start,
			// Token: 0x04000A5B RID: 2651
			Starting,
			// Token: 0x04000A5C RID: 2652
			Playing
		}
	}
}
