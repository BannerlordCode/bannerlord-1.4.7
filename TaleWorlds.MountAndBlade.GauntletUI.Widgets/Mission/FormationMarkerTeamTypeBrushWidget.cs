using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E0 RID: 224
	public class FormationMarkerTeamTypeBrushWidget : BrushWidget
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x00020558 File Offset: 0x0001E758
		public FormationMarkerTeamTypeBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00020561 File Offset: 0x0001E761
		private void UpdateState()
		{
			this.RegisterBrushStatesOfWidget();
			if (this.TeamType == 0)
			{
				this.SetState("Player");
				return;
			}
			if (this.TeamType == 1)
			{
				this.SetState("Ally");
				return;
			}
			this.SetState("Enemy");
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x0002059D File Offset: 0x0001E79D
		// (set) Token: 0x06000B9C RID: 2972 RVA: 0x000205A5 File Offset: 0x0001E7A5
		public int TeamType
		{
			get
			{
				return this._teamType;
			}
			set
			{
				if (this._teamType != value)
				{
					this._teamType = value;
					base.OnPropertyChanged(value, "TeamType");
					this.UpdateState();
				}
			}
		}

		// Token: 0x0400053C RID: 1340
		private int _teamType;
	}
}
