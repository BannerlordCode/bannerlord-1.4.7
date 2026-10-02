using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001C RID: 28
	public class FiefProfitTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000166 RID: 358 RVA: 0x00005F1D File Offset: 0x0000411D
		public FiefProfitTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005F2D File Offset: 0x0000412D
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

		// Token: 0x06000168 RID: 360 RVA: 0x00005F58 File Offset: 0x00004158
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("None");
				return;
			case 1:
				this.SetState("Tax");
				return;
			case 2:
				this.SetState("Tariff");
				return;
			case 3:
				this.SetState("Garrison");
				return;
			case 4:
				this.SetState("Village");
				return;
			case 5:
				this.SetState("Governor");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00005FCB File Offset: 0x000041CB
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00005FD3 File Offset: 0x000041D3
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

		// Token: 0x040000A7 RID: 167
		private bool _determinedVisual;

		// Token: 0x040000A8 RID: 168
		private int _type = -1;
	}
}
