using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C8 RID: 200
	public class ServerStatusItemBrushWidget : BrushWidget
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x0001D79C File Offset: 0x0001B99C
		public ServerStatusItemBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0001D7AC File Offset: 0x0001B9AC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.RegisterBrushStatesOfWidget();
				this._initialized = true;
				this.OnStatusChange(this.Status);
			}
			if (Math.Abs(base.ReadOnlyBrush.GlobalAlphaFactor - this._currentAlphaTarget) > 0.001f)
			{
				this.SetGlobalAlphaRecursively(MathF.Lerp(base.ReadOnlyBrush.GlobalAlphaFactor, this._currentAlphaTarget, dt * 5f, 1E-05f));
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0001D827 File Offset: 0x0001BA27
		private void OnStatusChange(int value)
		{
			this.SetState(value.ToString());
			if (value == 0)
			{
				this._currentAlphaTarget = 0f;
				return;
			}
			if (value - 1 > 1)
			{
				return;
			}
			this._currentAlphaTarget = 1f;
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0001D857 File Offset: 0x0001BA57
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x0001D85F File Offset: 0x0001BA5F
		public int Status
		{
			get
			{
				return this._status;
			}
			set
			{
				if (value != this._status)
				{
					this._status = value;
					base.OnPropertyChanged(value, "Status");
					this.OnStatusChange(value);
				}
			}
		}

		// Token: 0x040004CF RID: 1231
		private float _currentAlphaTarget;

		// Token: 0x040004D0 RID: 1232
		private bool _initialized;

		// Token: 0x040004D1 RID: 1233
		private int _status = -1;
	}
}
