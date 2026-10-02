using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x02000107 RID: 263
	public class DescriptionItemVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000DFB RID: 3579 RVA: 0x000264C6 File Offset: 0x000246C6
		public DescriptionItemVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000264D6 File Offset: 0x000246D6
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

		// Token: 0x06000DFD RID: 3581 RVA: 0x00026500 File Offset: 0x00024700
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("Gold");
				return;
			case 1:
				this.SetState("Production");
				return;
			case 2:
				this.SetState("Militia");
				return;
			case 3:
				this.SetState("Prosperity");
				return;
			case 4:
				this.SetState("Food");
				return;
			case 5:
				this.SetState("Loyalty");
				return;
			case 6:
				this.SetState("Security");
				return;
			case 7:
				this.SetState("Garrison");
				return;
			default:
				return;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x00026593 File Offset: 0x00024793
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x0002659B File Offset: 0x0002479B
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

		// Token: 0x04000657 RID: 1623
		private bool _determinedVisual;

		// Token: 0x04000658 RID: 1624
		private int _type = -1;
	}
}
