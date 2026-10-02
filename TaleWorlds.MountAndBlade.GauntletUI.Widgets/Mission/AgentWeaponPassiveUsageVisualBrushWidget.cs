using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D7 RID: 215
	public class AgentWeaponPassiveUsageVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000AFA RID: 2810 RVA: 0x0001ECC9 File Offset: 0x0001CEC9
		public AgentWeaponPassiveUsageVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0001ECDC File Offset: 0x0001CEDC
		private void UpdateVisualState()
		{
			if (this._firstUpdate)
			{
				this.RegisterBrushStatesOfWidget();
				this._firstUpdate = false;
			}
			switch (this.CouchLanceState)
			{
			case 0:
				base.IsVisible = false;
				return;
			case 1:
				base.IsVisible = true;
				this.SetState("ConditionsNotMet");
				return;
			case 2:
				base.IsVisible = true;
				this.SetState("Possible");
				return;
			case 3:
				this.SetState("Active");
				base.IsVisible = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0001ED5C File Offset: 0x0001CF5C
		// (set) Token: 0x06000AFD RID: 2813 RVA: 0x0001ED64 File Offset: 0x0001CF64
		[Editor(false)]
		public int CouchLanceState
		{
			get
			{
				return this._couchLanceState;
			}
			set
			{
				if (this._couchLanceState != value)
				{
					this._couchLanceState = value;
					base.OnPropertyChanged(value, "CouchLanceState");
					this.UpdateVisualState();
				}
			}
		}

		// Token: 0x040004FA RID: 1274
		private bool _firstUpdate;

		// Token: 0x040004FB RID: 1275
		private int _couchLanceState = -1;
	}
}
