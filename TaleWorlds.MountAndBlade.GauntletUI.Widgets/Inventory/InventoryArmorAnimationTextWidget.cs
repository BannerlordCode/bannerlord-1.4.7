using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000138 RID: 312
	public class InventoryArmorAnimationTextWidget : TextWidget
	{
		// Token: 0x06001045 RID: 4165 RVA: 0x0002C86A File Offset: 0x0002AA6A
		public InventoryArmorAnimationTextWidget(UIContext context)
			: base(context)
		{
			base.FloatText = 0f;
			this._isSettingInitialValue = true;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0002C885 File Offset: 0x0002AA85
		private void HandleAnimation(float oldValue, float newValue)
		{
			if (!this._isSettingInitialValue)
			{
				if (oldValue > newValue)
				{
					this.SetState("Decrease");
					return;
				}
				if (oldValue < newValue)
				{
					this.SetState("Increase");
					return;
				}
				this.SetState("Default");
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0002C8BA File Offset: 0x0002AABA
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x0002C8C2 File Offset: 0x0002AAC2
		[Editor(false)]
		public float FloatAmount
		{
			get
			{
				return this._floatAmount;
			}
			set
			{
				if (this._floatAmount != value)
				{
					this.HandleAnimation(this._floatAmount, value);
					this._floatAmount = value;
					base.FloatText = this._floatAmount;
					base.OnPropertyChanged(value, "FloatAmount");
				}
				this._isSettingInitialValue = false;
			}
		}

		// Token: 0x0400075F RID: 1887
		private bool _isSettingInitialValue;

		// Token: 0x04000760 RID: 1888
		private float _floatAmount;
	}
}
