using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000015 RID: 21
	public class DebugValueUpdateSlider : SliderWidget
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00005122 File Offset: 0x00003322
		public DebugValueUpdateSlider(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000512B File Offset: 0x0000332B
		protected override void OnValueIntChanged(int value)
		{
			base.OnValueIntChanged(value);
			this.OnValueChanged((float)value);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000513C File Offset: 0x0000333C
		protected override void OnValueFloatChanged(float value)
		{
			base.OnValueFloatChanged(value);
			this.OnValueChanged(value);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000514C File Offset: 0x0000334C
		private void OnValueChanged(float value)
		{
			if (this.WidgetToUpdate != null)
			{
				this.WidgetToUpdate.Text = this.WidgetToUpdate.GlobalPosition.Y.ToString("F0");
			}
			if (this.ValueToUpdate != null)
			{
				this.ValueToUpdate.InitialAmount = (int)value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000519E File Offset: 0x0000339E
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000051A6 File Offset: 0x000033A6
		public TextWidget WidgetToUpdate { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000051AF File Offset: 0x000033AF
		// (set) Token: 0x06000128 RID: 296 RVA: 0x000051B7 File Offset: 0x000033B7
		public FillBarVerticalWidget ValueToUpdate { get; set; }
	}
}
