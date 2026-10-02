using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x02000118 RID: 280
	public class MapEventVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000ED5 RID: 3797 RVA: 0x00028D25 File Offset: 0x00026F25
		public MapEventVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00028D3C File Offset: 0x00026F3C
		private void UpdateVisual(int type)
		{
			if (this._initialUpdate)
			{
				this.RegisterBrushStatesOfWidget();
				this._initialUpdate = false;
			}
			switch (type)
			{
			case 1:
				this.SetState("Raid");
				return;
			case 2:
				this.SetState("Siege");
				return;
			case 3:
				this.SetState("Battle");
				return;
			case 4:
				this.SetState("Rebellion");
				return;
			case 5:
				this.SetState("SallyOut");
				return;
			default:
				this.SetState("None");
				return;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x00028DC3 File Offset: 0x00026FC3
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x00028DCB File Offset: 0x00026FCB
		[Editor(false)]
		public int MapEventType
		{
			get
			{
				return this._mapEventType;
			}
			set
			{
				if (this._mapEventType != value)
				{
					this._mapEventType = value;
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x040006C0 RID: 1728
		private bool _initialUpdate = true;

		// Token: 0x040006C1 RID: 1729
		private int _mapEventType = -1;
	}
}
