using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Order
{
	// Token: 0x020000E5 RID: 229
	public class OrderFormationClassVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000BD8 RID: 3032 RVA: 0x00020BAF File Offset: 0x0001EDAF
		public OrderFormationClassVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00020BC0 File Offset: 0x0001EDC0
		private void UpdateVisual()
		{
			switch (this.FormationClassValue)
			{
			case 0:
				this.SetState("Infantry");
				return;
			case 1:
				this.SetState("Ranged");
				return;
			case 2:
				this.SetState("Cavalry");
				return;
			case 3:
				this.SetState("HorseArcher");
				return;
			default:
				this.SetState("Infantry");
				return;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00020C27 File Offset: 0x0001EE27
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x00020C2F File Offset: 0x0001EE2F
		[Editor(false)]
		public int FormationClassValue
		{
			get
			{
				return this._formationClassValue;
			}
			set
			{
				if (this._formationClassValue != value)
				{
					this._formationClassValue = value;
					base.OnPropertyChanged(value, "FormationClassValue");
					this.UpdateVisual();
				}
			}
		}

		// Token: 0x0400055A RID: 1370
		private int _formationClassValue = -1;
	}
}
