using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000C2 RID: 194
	public class MultiplayerPersonalKillFeedWidget : Widget
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0001C6A6 File Offset: 0x0001A8A6
		private int _speedUpWidgetLimit
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001C6A9 File Offset: 0x0001A8A9
		public MultiplayerPersonalKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				child.PositionYOffset = Mathf.Lerp(child.PositionYOffset, this.GetVerticalPositionOfChildByIndex(i), 0.35f);
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this.UpdateSpeedModifiers();
			this.UpdateMaxTargetAlphas();
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0001C714 File Offset: 0x0001A914
		private void UpdateMaxTargetAlphas()
		{
			for (int i = base.ChildCount - 1; i >= 0; i--)
			{
				MultiplayerPersonalKillFeedItemWidget multiplayerPersonalKillFeedItemWidget = base.GetChild(i) as MultiplayerPersonalKillFeedItemWidget;
				if (i <= base.ChildCount - 1 && i >= base.ChildCount - 4)
				{
					multiplayerPersonalKillFeedItemWidget.SetMaxAlphaValue(1f);
				}
				else if (i == base.ChildCount - 5)
				{
					multiplayerPersonalKillFeedItemWidget.SetMaxAlphaValue(0.7f);
				}
				else if (i == base.ChildCount - 6)
				{
					multiplayerPersonalKillFeedItemWidget.SetMaxAlphaValue(0.4f);
				}
				else if (i == base.ChildCount - 7)
				{
					multiplayerPersonalKillFeedItemWidget.SetMaxAlphaValue(0.15f);
				}
				else
				{
					multiplayerPersonalKillFeedItemWidget.SetMaxAlphaValue(0f);
				}
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001C7C0 File Offset: 0x0001A9C0
		private float GetVerticalPositionOfChildByIndex(int indexOfChild)
		{
			float num = 0f;
			for (int i = base.ChildCount - 1; i > indexOfChild; i--)
			{
				num += base.GetChild(i).Size.Y * base._inverseScaleToUse;
			}
			return num;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0001C804 File Offset: 0x0001AA04
		private void UpdateSpeedModifiers()
		{
			if (base.ChildCount > this._speedUpWidgetLimit)
			{
				float num = (float)(base.ChildCount - this._speedUpWidgetLimit) / 2f + 1f;
				for (int i = 0; i < base.ChildCount - this._speedUpWidgetLimit; i++)
				{
					MultiplayerPersonalKillFeedItemWidget multiplayerPersonalKillFeedItemWidget = base.GetChild(i) as MultiplayerPersonalKillFeedItemWidget;
					if (multiplayerPersonalKillFeedItemWidget != null)
					{
						multiplayerPersonalKillFeedItemWidget.SetSpeedModifier(num);
					}
				}
			}
		}
	}
}
