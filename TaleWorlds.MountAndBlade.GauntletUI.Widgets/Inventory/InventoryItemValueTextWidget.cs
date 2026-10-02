using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000141 RID: 321
	public class InventoryItemValueTextWidget : TextWidget
	{
		// Token: 0x060010D4 RID: 4308 RVA: 0x0002E299 File Offset: 0x0002C499
		public InventoryItemValueTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
		private void HandleVisuals()
		{
			if (!this._firstHandled)
			{
				this.RegisterBrushStatesOfWidget();
				this._firstHandled = true;
			}
			switch (this.ProfitType)
			{
			case -2:
				this.SetState("VeryBad");
				return;
			case -1:
				this.SetState("Bad");
				return;
			case 0:
				this.SetState("Default");
				return;
			case 1:
				this.SetState("Good");
				return;
			case 2:
				this.SetState("VeryGood");
				return;
			default:
				return;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0002E326 File Offset: 0x0002C526
		// (set) Token: 0x060010D7 RID: 4311 RVA: 0x0002E32E File Offset: 0x0002C52E
		[Editor(false)]
		public int ProfitType
		{
			get
			{
				return this._profitType;
			}
			set
			{
				if (this._profitType != value)
				{
					this._profitType = value;
					base.OnPropertyChanged(value, "ProfitType");
					this.HandleVisuals();
				}
			}
		}

		// Token: 0x04000796 RID: 1942
		private bool _firstHandled;

		// Token: 0x04000797 RID: 1943
		private int _profitType;
	}
}
