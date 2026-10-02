using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000C0 RID: 192
	public class MultiplayerGeneralKillFeedWidget : Widget
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0001BDE1 File Offset: 0x00019FE1
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x0001BDE9 File Offset: 0x00019FE9
		public float VerticalPaddingAmount { get; set; } = 3f;

		// Token: 0x060009FC RID: 2556 RVA: 0x0001BDF2 File Offset: 0x00019FF2
		public MultiplayerGeneralKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0001BE10 File Offset: 0x0001A010
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._normalWidgetHeight <= 0f && base.ChildCount > 1)
			{
				this._normalWidgetHeight = base.GetChild(0).ScaledSuggestedHeight * base._inverseScaleToUse;
			}
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				child.PositionYOffset = Mathf.Lerp(child.PositionYOffset, this.GetVerticalPositionOfChildByIndex(i, base.ChildCount), 0.35f);
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0001BE8D File Offset: 0x0001A08D
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.PositionYOffset = this.GetVerticalPositionOfChildByIndex(child.GetSiblingIndex(), base.ChildCount);
			this.UpdateSpeedModifiers();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
		private float GetVerticalPositionOfChildByIndex(int indexOfChild, int numOfTotalChild)
		{
			int num = numOfTotalChild - 1 - indexOfChild;
			return (this._normalWidgetHeight + this.VerticalPaddingAmount) * (float)num;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0001BED8 File Offset: 0x0001A0D8
		private void UpdateSpeedModifiers()
		{
			if (base.ChildCount > this._speedUpWidgetLimit)
			{
				float num = (float)(base.ChildCount - this._speedUpWidgetLimit) / 20f + 1f;
				for (int i = 0; i < base.ChildCount - this._speedUpWidgetLimit; i++)
				{
					(base.GetChild(i) as MultiplayerGeneralKillFeedItemWidget).SetSpeedModifier(num);
				}
			}
		}

		// Token: 0x04000486 RID: 1158
		private float _normalWidgetHeight;

		// Token: 0x04000487 RID: 1159
		private int _speedUpWidgetLimit = 10;
	}
}
