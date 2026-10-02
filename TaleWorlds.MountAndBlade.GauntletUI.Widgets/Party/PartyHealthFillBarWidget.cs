using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000065 RID: 101
	public class PartyHealthFillBarWidget : FillBar
	{
		// Token: 0x0600056A RID: 1386 RVA: 0x000104D8 File Offset: 0x0000E6D8
		public PartyHealthFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00010524 File Offset: 0x0000E724
		private void HealthUpdated()
		{
			if (this.brushLayer == null)
			{
				this.brushLayer = base.Brush.GetLayer("DefaultFill");
			}
			base.CurrentAmount = (base.InitialAmount = this.Health);
			if (this.IsWounded)
			{
				this.brushLayer.Color = this.WoundedColor;
			}
			else if (this.Health >= this.FullHealthyLimit)
			{
				this.brushLayer.Color = this.FullHealthyColor;
			}
			else
			{
				this.brushLayer.Color = this.HealthyColor;
			}
			if (this.HealthText != null)
			{
				this.HealthText.Text = this.Health + "%";
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x000105D9 File Offset: 0x0000E7D9
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x000105E1 File Offset: 0x0000E7E1
		[Editor(false)]
		public int Health
		{
			get
			{
				return this._health;
			}
			set
			{
				if (this._health != value)
				{
					this._health = value;
					base.OnPropertyChanged(value, "Health");
					this.HealthUpdated();
				}
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00010605 File Offset: 0x0000E805
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0001060D File Offset: 0x0000E80D
		[Editor(false)]
		public bool IsWounded
		{
			get
			{
				return this._isWounded;
			}
			set
			{
				if (this._isWounded != value)
				{
					this._isWounded = value;
					base.OnPropertyChanged(value, "IsWounded");
					this.HealthUpdated();
				}
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00010631 File Offset: 0x0000E831
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00010639 File Offset: 0x0000E839
		[Editor(false)]
		public TextWidget HealthText
		{
			get
			{
				return this._healthText;
			}
			set
			{
				if (this._healthText != value)
				{
					this._healthText = value;
					base.OnPropertyChanged<TextWidget>(value, "HealthText");
					this.HealthUpdated();
				}
			}
		}

		// Token: 0x04000250 RID: 592
		private readonly int FullHealthyLimit = 90;

		// Token: 0x04000251 RID: 593
		private readonly Color WoundedColor = Color.FromUint(4290199102U);

		// Token: 0x04000252 RID: 594
		private readonly Color HealthyColor = Color.FromUint(4291732560U);

		// Token: 0x04000253 RID: 595
		private readonly Color FullHealthyColor = Color.FromUint(4284921662U);

		// Token: 0x04000254 RID: 596
		private BrushLayer brushLayer;

		// Token: 0x04000255 RID: 597
		private int _health;

		// Token: 0x04000256 RID: 598
		private bool _isWounded;

		// Token: 0x04000257 RID: 599
		private TextWidget _healthText;
	}
}
