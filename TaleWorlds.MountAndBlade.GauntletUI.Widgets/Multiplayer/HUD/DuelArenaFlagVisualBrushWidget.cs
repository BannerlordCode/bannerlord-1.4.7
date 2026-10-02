using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C3 RID: 195
	public class DuelArenaFlagVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000A2C RID: 2604 RVA: 0x0001C86A File Offset: 0x0001AA6A
		public DuelArenaFlagVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0001C87C File Offset: 0x0001AA7C
		private void UpdateVisual()
		{
			switch (this.ArenaType)
			{
			case 0:
				this.SetState("Infantry");
				return;
			case 1:
				this.SetState("Archery");
				return;
			case 2:
				this.SetState("Cavalry");
				return;
			default:
				this.SetState("Infantry");
				return;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0001C8D3 File Offset: 0x0001AAD3
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x0001C8DB File Offset: 0x0001AADB
		[Editor(false)]
		public int ArenaType
		{
			get
			{
				return this._arenaType;
			}
			set
			{
				if (this._arenaType != value)
				{
					this._arenaType = value;
					base.OnPropertyChanged(value, "ArenaType");
					this.UpdateVisual();
				}
			}
		}

		// Token: 0x04000499 RID: 1177
		private int _arenaType = -1;
	}
}
