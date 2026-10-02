using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.DamageFeed
{
	// Token: 0x02000102 RID: 258
	public class MissionAgentDamageFeedItemWidget : Widget
	{
		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00025F96 File Offset: 0x00024196
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x00025F9E File Offset: 0x0002419E
		public float FadeInTime { get; set; } = 0.1f;

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00025FA7 File Offset: 0x000241A7
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x00025FAF File Offset: 0x000241AF
		public float StayTime { get; set; } = 1.5f;

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00025FB8 File Offset: 0x000241B8
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x00025FC0 File Offset: 0x000241C0
		public float FadeOutTime { get; set; } = 0.3f;

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00025FC9 File Offset: 0x000241C9
		// (set) Token: 0x06000DDD RID: 3549 RVA: 0x00025FD1 File Offset: 0x000241D1
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000DDE RID: 3550 RVA: 0x00025FDA File Offset: 0x000241DA
		public MissionAgentDamageFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002600F File Offset: 0x0002420F
		public void ShowFeed()
		{
			this._isShown = true;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00026018 File Offset: 0x00024218
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._isInitialized)
			{
				this.SetGlobalAlphaRecursively(0f);
				this._isInitialized = true;
			}
			if (this._isShown)
			{
				this.TimeSinceCreation += dt * this._speedModifier;
				if (this.TimeSinceCreation <= this.FadeInTime)
				{
					this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 1f, this.TimeSinceCreation / this.FadeInTime));
					return;
				}
				if (this.TimeSinceCreation - this.FadeInTime <= this.StayTime)
				{
					this.SetGlobalAlphaRecursively(1f);
					return;
				}
				if (this.TimeSinceCreation - (this.FadeInTime + this.StayTime) <= this.FadeOutTime)
				{
					this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0f, (this.TimeSinceCreation - (this.FadeInTime + this.StayTime)) / this.FadeOutTime));
					if (base.AlphaFactor <= 0.1f)
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
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00026134 File Offset: 0x00024334
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x04000648 RID: 1608
		private float _speedModifier = 1f;

		// Token: 0x0400064A RID: 1610
		private bool _isInitialized;

		// Token: 0x0400064B RID: 1611
		private bool _isShown;
	}
}
