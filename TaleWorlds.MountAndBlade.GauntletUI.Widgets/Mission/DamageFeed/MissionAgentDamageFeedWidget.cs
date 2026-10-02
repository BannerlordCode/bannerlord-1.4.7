using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.DamageFeed
{
	// Token: 0x02000103 RID: 259
	public class MissionAgentDamageFeedWidget : Widget
	{
		// Token: 0x06000DE2 RID: 3554 RVA: 0x00026146 File Offset: 0x00024346
		public MissionAgentDamageFeedWidget(UIContext context)
			: base(context)
		{
			this._feedItemQueue = new Queue<MissionAgentDamageFeedItemWidget>();
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00026164 File Offset: 0x00024364
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			MissionAgentDamageFeedItemWidget missionAgentDamageFeedItemWidget = (MissionAgentDamageFeedItemWidget)child;
			this._feedItemQueue.Enqueue(missionAgentDamageFeedItemWidget);
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00026191 File Offset: 0x00024391
		protected override void OnBeforeChildRemoved(Widget child)
		{
			this._activeFeedItem = null;
			base.OnBeforeChildRemoved(child);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x000261A4 File Offset: 0x000243A4
		protected override void OnUpdate(float dt)
		{
			if (this._activeFeedItem == null && this._feedItemQueue.Count > 0)
			{
				MissionAgentDamageFeedItemWidget missionAgentDamageFeedItemWidget = this._feedItemQueue.Dequeue();
				this._activeFeedItem = missionAgentDamageFeedItemWidget;
				this._activeFeedItem.ShowFeed();
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x000261E8 File Offset: 0x000243E8
		private void UpdateSpeedModifiers()
		{
			if (base.ChildCount > this._speedUpWidgetLimit)
			{
				float num = (float)(base.ChildCount - this._speedUpWidgetLimit) / 3f + 1f;
				for (int i = 0; i < base.ChildCount - this._speedUpWidgetLimit; i++)
				{
					((MissionAgentDamageFeedItemWidget)base.GetChild(i)).SetSpeedModifier(num);
				}
			}
		}

		// Token: 0x0400064C RID: 1612
		private int _speedUpWidgetLimit = 1;

		// Token: 0x0400064D RID: 1613
		private readonly Queue<MissionAgentDamageFeedItemWidget> _feedItemQueue;

		// Token: 0x0400064E RID: 1614
		private MissionAgentDamageFeedItemWidget _activeFeedItem;
	}
}
