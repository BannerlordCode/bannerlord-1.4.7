using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001D RID: 29
	public class FiefStatTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00005FF1 File Offset: 0x000041F1
		public FiefStatTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006001 File Offset: 0x00004201
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._determinedVisual)
			{
				this.RegisterBrushStatesOfWidget();
				this.UpdateVisual(this.Type);
				this._determinedVisual = true;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000602C File Offset: 0x0000422C
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("None");
				return;
			case 1:
				this.SetState("Wall");
				return;
			case 2:
				this.SetState("Garrison");
				return;
			case 3:
				this.SetState("Militia");
				return;
			case 4:
				this.SetState("Prosperity");
				return;
			case 5:
				this.SetState("Food");
				return;
			case 6:
				this.SetState("Loyalty");
				return;
			case 7:
				this.SetState("Security");
				return;
			case 8:
				this.SetState("Shipyard");
				return;
			case 9:
				this.SetState("Patrol");
				return;
			case 10:
				this.SetState("CoastalPatrol");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600016E RID: 366 RVA: 0x000060EF File Offset: 0x000042EF
		// (set) Token: 0x0600016F RID: 367 RVA: 0x000060F7 File Offset: 0x000042F7
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
				}
			}
		}

		// Token: 0x040000A9 RID: 169
		private bool _determinedVisual;

		// Token: 0x040000AA RID: 170
		private int _type = -1;
	}
}
