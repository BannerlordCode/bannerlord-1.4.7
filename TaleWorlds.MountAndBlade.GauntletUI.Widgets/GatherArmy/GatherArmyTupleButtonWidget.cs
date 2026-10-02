using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GatherArmy
{
	// Token: 0x0200014E RID: 334
	public class GatherArmyTupleButtonWidget : ButtonWidget
	{
		// Token: 0x060011BE RID: 4542 RVA: 0x000315C2 File Offset: 0x0002F7C2
		public GatherArmyTupleButtonWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x000315D2 File Offset: 0x0002F7D2
		protected override void HandleClick()
		{
			if (!this.IsTransferDisabled && (this.IsInCart || this.IsEligible))
			{
				base.HandleClick();
			}
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000315F4 File Offset: 0x0002F7F4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsTransferDisabled || (!this.IsInCart && !this.IsEligible))
			{
				this.SetState("Disabled");
				return;
			}
			if (this.IsInCart)
			{
				this.SetState("Selected");
				return;
			}
			if (base.IsPressed)
			{
				this.SetState("Pressed");
				return;
			}
			if (base.IsHovered)
			{
				this.SetState("Hovered");
				return;
			}
			this.SetState("Default");
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00031673 File Offset: 0x0002F873
		// (set) Token: 0x060011C2 RID: 4546 RVA: 0x0003167B File Offset: 0x0002F87B
		[Editor(false)]
		public bool IsInCart
		{
			get
			{
				return this._isInCart;
			}
			set
			{
				if (this._isInCart != value)
				{
					this._isInCart = value;
					base.OnPropertyChanged(value, "IsInCart");
				}
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00031699 File Offset: 0x0002F899
		// (set) Token: 0x060011C4 RID: 4548 RVA: 0x000316A1 File Offset: 0x0002F8A1
		[Editor(false)]
		public bool IsEligible
		{
			get
			{
				return this._isEligible;
			}
			set
			{
				if (this._isEligible != value)
				{
					this._isEligible = value;
					base.OnPropertyChanged(value, "IsEligible");
				}
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x000316BF File Offset: 0x0002F8BF
		// (set) Token: 0x060011C6 RID: 4550 RVA: 0x000316C7 File Offset: 0x0002F8C7
		[Editor(false)]
		public bool IsTransferDisabled
		{
			get
			{
				return this._isTransferDisabled;
			}
			set
			{
				if (this._isTransferDisabled != value)
				{
					this._isTransferDisabled = value;
					base.OnPropertyChanged(value, "IsTransferDisabled");
				}
			}
		}

		// Token: 0x04000819 RID: 2073
		private bool _isInCart;

		// Token: 0x0400081A RID: 2074
		private bool _isEligible;

		// Token: 0x0400081B RID: 2075
		private bool _isTransferDisabled;
	}
}
