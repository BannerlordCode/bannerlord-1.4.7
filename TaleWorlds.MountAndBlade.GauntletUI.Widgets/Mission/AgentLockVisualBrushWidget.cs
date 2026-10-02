using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D6 RID: 214
	public class AgentLockVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x0001EBDE File Offset: 0x0001CDDE
		public AgentLockVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.ScaledPositionXOffset = this.Position.X - base.Size.X / 2f;
			base.ScaledPositionYOffset = this.Position.Y - base.Size.Y / 2f;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0001EC50 File Offset: 0x0001CE50
		private void UpdateVisualState(int lockState)
		{
			if (lockState == 0)
			{
				this.SetState("Possible");
				return;
			}
			if (lockState != 1)
			{
				return;
			}
			this.SetState("Active");
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0001EC71 File Offset: 0x0001CE71
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x0001EC79 File Offset: 0x0001CE79
		[Editor(false)]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0001EC9C File Offset: 0x0001CE9C
		// (set) Token: 0x06000AF9 RID: 2809 RVA: 0x0001ECA4 File Offset: 0x0001CEA4
		[Editor(false)]
		public int LockState
		{
			get
			{
				return this._lockState;
			}
			set
			{
				if (this._lockState != value)
				{
					this._lockState = value;
					base.OnPropertyChanged(value, "LockState");
					this.UpdateVisualState(value);
				}
			}
		}

		// Token: 0x040004F8 RID: 1272
		private Vec2 _position;

		// Token: 0x040004F9 RID: 1273
		private int _lockState = -1;
	}
}
