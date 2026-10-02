using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000135 RID: 309
	public class KingdomDecisionPopupWidget : Widget
	{
		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x0002C285 File Offset: 0x0002A485
		// (set) Token: 0x06001018 RID: 4120 RVA: 0x0002C28D File Offset: 0x0002A48D
		public int DelayAfterKingsDecision { get; set; } = 5;

		// Token: 0x06001019 RID: 4121 RVA: 0x0002C296 File Offset: 0x0002A496
		public KingdomDecisionPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0002C2B1 File Offset: 0x0002A4B1
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._kingDecisionDoneTime != -1f && base.EventManager.Time - this._kingDecisionDoneTime > (float)this.DelayAfterKingsDecision)
			{
				this.ExecuteFinalDone();
			}
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0002C2E8 File Offset: 0x0002A4E8
		private void ExecuteFinalDone()
		{
			base.EventFired("FinalDone", Array.Empty<object>());
			this._kingDecisionDoneTime = -1f;
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				KingdomDecisionOptionWidget kingdomDecisionOptionWidget;
				if ((kingdomDecisionOptionWidget = allChildrenRecursive[i] as KingdomDecisionOptionWidget) != null)
				{
					kingdomDecisionOptionWidget.OnFinalDone();
				}
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0002C340 File Offset: 0x0002A540
		private void OnKingsDecisionDone()
		{
			this._kingDecisionDoneTime = base.EventManager.Time;
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				KingdomDecisionOptionWidget kingdomDecisionOptionWidget;
				if ((kingdomDecisionOptionWidget = allChildrenRecursive[i] as KingdomDecisionOptionWidget) != null)
				{
					kingdomDecisionOptionWidget.OnKingsDecisionDone();
				}
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x0002C38D File Offset: 0x0002A58D
		// (set) Token: 0x0600101E RID: 4126 RVA: 0x0002C395 File Offset: 0x0002A595
		[Editor(false)]
		public bool IsKingsDecisionDone
		{
			get
			{
				return this._isKingsDecisionDone;
			}
			set
			{
				if (this._isKingsDecisionDone != value)
				{
					this._isKingsDecisionDone = value;
					base.OnPropertyChanged(value, "IsKingsDecisionDone");
					if (value)
					{
						this.OnKingsDecisionDone();
					}
				}
			}
		}

		// Token: 0x0400074E RID: 1870
		private float _kingDecisionDoneTime = -1f;

		// Token: 0x0400074F RID: 1871
		private bool _isKingsDecisionDone;
	}
}
