using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.General
{
	// Token: 0x020000FE RID: 254
	public class SingleplayerGeneralKillFeedWidget : Widget
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0002526E File Offset: 0x0002346E
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x00025276 File Offset: 0x00023476
		public float VerticalPaddingAmount { get; set; } = 3f;

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002527F File Offset: 0x0002347F
		public SingleplayerGeneralKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000252A0 File Offset: 0x000234A0
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
				child.PositionYOffset = Mathf.Lerp(child.PositionYOffset, this.GetVerticalPositionOfChildByIndex(i), 0.35f);
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00025317 File Offset: 0x00023517
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.PositionYOffset = this.GetVerticalPositionOfChildByIndex(child.GetSiblingIndex());
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00025338 File Offset: 0x00023538
		private float GetVerticalPositionOfChildByIndex(int indexOfChild)
		{
			return (this._normalWidgetHeight + this.VerticalPaddingAmount) * (float)(base.ChildCount - indexOfChild - 1);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00025354 File Offset: 0x00023554
		private void UpdateSpeedModifiers()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				SingleplayerGeneralKillFeedItemWidget singleplayerGeneralKillFeedItemWidget = base.GetChild(i) as SingleplayerGeneralKillFeedItemWidget;
				float num = MathF.Pow((float)(base.ChildCount - i), 0.33f);
				singleplayerGeneralKillFeedItemWidget.SetSpeedModifier(num);
			}
		}

		// Token: 0x04000628 RID: 1576
		private float _normalWidgetHeight = -1f;
	}
}
