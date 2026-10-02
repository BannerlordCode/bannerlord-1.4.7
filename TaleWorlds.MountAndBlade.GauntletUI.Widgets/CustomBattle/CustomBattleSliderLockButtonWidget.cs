using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CustomBattle
{
	// Token: 0x0200015E RID: 350
	public class CustomBattleSliderLockButtonWidget : ButtonWidget
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001288 RID: 4744 RVA: 0x0003304F File Offset: 0x0003124F
		// (set) Token: 0x06001289 RID: 4745 RVA: 0x00033057 File Offset: 0x00031257
		public Brush LockOpenedBrush { get; set; }

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x00033060 File Offset: 0x00031260
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x00033068 File Offset: 0x00031268
		public Brush LockClosedBrush { get; set; }

		// Token: 0x0600128C RID: 4748 RVA: 0x00033071 File Offset: 0x00031271
		public CustomBattleSliderLockButtonWidget(UIContext context)
			: base(context)
		{
			base.boolPropertyChanged += this.CustomBattleSliderLockButtonWidget_PropertyChanged;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0003308C File Offset: 0x0003128C
		private void CustomBattleSliderLockButtonWidget_PropertyChanged(PropertyOwnerObject widget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsSelected")
			{
				base.Brush = (propertyValue ? this.LockClosedBrush : this.LockOpenedBrush);
			}
		}
	}
}
