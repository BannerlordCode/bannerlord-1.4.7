using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000BF RID: 191
	public class MultiplayerGeneralKillFeedItemWidget : Widget
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0001BCA5 File Offset: 0x00019EA5
		private float CurrentAlpha
		{
			get
			{
				return base.AlphaFactor;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0001BCAD File Offset: 0x00019EAD
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x0001BCB5 File Offset: 0x00019EB5
		public float TimeSinceCreation { get; private set; }

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001BCBE File Offset: 0x00019EBE
		public MultiplayerGeneralKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0001BCD4 File Offset: 0x00019ED4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this._initialized = true;
			}
			this.TimeSinceCreation += dt * this._speedModifier;
			if (this.TimeSinceCreation <= 0.15f)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(this.CurrentAlpha, 1f, this.TimeSinceCreation / 0.15f));
				return;
			}
			if (this.TimeSinceCreation - 0.15f <= 3.5f)
			{
				this.SetGlobalAlphaRecursively(1f);
				return;
			}
			if (this.TimeSinceCreation - 3.65f <= 1f)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(this.CurrentAlpha, 0f, (this.TimeSinceCreation - 3.65f) / 1f));
				if (this.CurrentAlpha <= 0.1f)
				{
					base.EventFired("OnRemove", Array.Empty<object>());
					return;
				}
			}
			else
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0001BDCF File Offset: 0x00019FCF
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x04000480 RID: 1152
		private const float FadeInTime = 0.15f;

		// Token: 0x04000481 RID: 1153
		private const float StayTime = 3.5f;

		// Token: 0x04000482 RID: 1154
		private const float FadeOutTime = 1f;

		// Token: 0x04000483 RID: 1155
		private float _speedModifier = 1f;

		// Token: 0x04000485 RID: 1157
		private bool _initialized;
	}
}
