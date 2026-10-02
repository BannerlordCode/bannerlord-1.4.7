using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000EA RID: 234
	public class OrderOfBattleFormationClassContainerWidget : Widget
	{
		// Token: 0x06000C0F RID: 3087 RVA: 0x0002122D File Offset: 0x0001F42D
		public OrderOfBattleFormationClassContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00021236 File Offset: 0x0001F436
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x0002123E File Offset: 0x0001F43E
		[Editor(false)]
		public SliderWidget WeightSlider
		{
			get
			{
				return this._weightSlider;
			}
			set
			{
				if (value != this._weightSlider)
				{
					this._weightSlider = value;
					base.OnPropertyChanged<SliderWidget>(value, "WeightSlider");
				}
			}
		}

		// Token: 0x04000571 RID: 1393
		private SliderWidget _weightSlider;
	}
}
