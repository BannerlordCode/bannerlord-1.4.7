using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x0200018E RID: 398
	public class BarterItemCountControlButtonWidget : ButtonWidget
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x00037EA8 File Offset: 0x000360A8
		// (set) Token: 0x0600148D RID: 5261 RVA: 0x00037EB0 File Offset: 0x000360B0
		public float IncreaseToHoldDelay { get; set; } = 1f;

		// Token: 0x0600148E RID: 5262 RVA: 0x00037EB9 File Offset: 0x000360B9
		public BarterItemCountControlButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00037ED0 File Offset: 0x000360D0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this._totalTime += dt;
			if (base.IsPressed && this._clickStartTime + this.IncreaseToHoldDelay < this._totalTime)
			{
				base.EventFired("MoveOne", Array.Empty<object>());
			}
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00037F1F File Offset: 0x0003611F
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			this._clickStartTime = this._totalTime;
			base.EventFired("MoveOne", Array.Empty<object>());
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00037F43 File Offset: 0x00036143
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.OnMouseReleased(isFromInput);
			this._clickStartTime = 0f;
		}

		// Token: 0x04000953 RID: 2387
		private float _clickStartTime;

		// Token: 0x04000954 RID: 2388
		private float _totalTime;
	}
}
