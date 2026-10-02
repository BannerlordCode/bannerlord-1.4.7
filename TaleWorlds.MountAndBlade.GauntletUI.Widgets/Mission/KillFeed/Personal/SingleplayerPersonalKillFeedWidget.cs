using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.Personal
{
	// Token: 0x020000FC RID: 252
	public class SingleplayerPersonalKillFeedWidget : Widget
	{
		// Token: 0x06000D6C RID: 3436 RVA: 0x00024C4F File Offset: 0x00022E4F
		public SingleplayerPersonalKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00024C64 File Offset: 0x00022E64
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
				child.PositionYOffset = Mathf.Lerp(child.PositionYOffset, this.GetVerticalPositionOfChildByIndex(i), 0.2f);
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00024CDB File Offset: 0x00022EDB
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.PositionYOffset = this.GetVerticalPositionOfChildByIndex(child.GetSiblingIndex());
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00024CFC File Offset: 0x00022EFC
		private float GetVerticalPositionOfChildByIndex(int indexOfChild)
		{
			return -1f * this._normalWidgetHeight * (float)(base.ChildCount - indexOfChild - 1);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00024D18 File Offset: 0x00022F18
		private void UpdateSpeedModifiers()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				SingleplayerPersonalKillFeedItemWidget singleplayerPersonalKillFeedItemWidget = base.GetChild(i) as SingleplayerPersonalKillFeedItemWidget;
				float num = MathF.Pow((float)(base.ChildCount - i), 0.33f);
				singleplayerPersonalKillFeedItemWidget.SetSpeedModifier(num);
			}
		}

		// Token: 0x04000612 RID: 1554
		private float _normalWidgetHeight = -1f;
	}
}
