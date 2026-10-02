using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000114 RID: 276
	public class SettlementOverlayWallIconBrushWidget : BrushWidget
	{
		// Token: 0x06000EC4 RID: 3780 RVA: 0x00028A4F File Offset: 0x00026C4F
		public SettlementOverlayWallIconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00028A58 File Offset: 0x00026C58
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.SetState(this.WallsLevel.ToString());
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x00028A80 File Offset: 0x00026C80
		// (set) Token: 0x06000EC7 RID: 3783 RVA: 0x00028A88 File Offset: 0x00026C88
		[Editor(false)]
		public int WallsLevel
		{
			get
			{
				return this._wallsLevel;
			}
			set
			{
				if (this._wallsLevel != value)
				{
					this._wallsLevel = value;
					base.OnPropertyChanged(value, "WallsLevel");
				}
			}
		}

		// Token: 0x040006B9 RID: 1721
		private int _wallsLevel;
	}
}
