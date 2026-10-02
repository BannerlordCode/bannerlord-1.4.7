using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007B RID: 123
	public class OptionsGamepadKeyLocationWidget : Widget
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x000136B7 File Offset: 0x000118B7
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x000136BF File Offset: 0x000118BF
		public bool ForceVisible { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000136C8 File Offset: 0x000118C8
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x000136D0 File Offset: 0x000118D0
		public int KeyID { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x000136D9 File Offset: 0x000118D9
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x000136E1 File Offset: 0x000118E1
		public int NormalPositionXOffset { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x000136EA File Offset: 0x000118EA
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x000136F2 File Offset: 0x000118F2
		public int NormalPositionYOffset { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x000136FB File Offset: 0x000118FB
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00013703 File Offset: 0x00011903
		public int NormalSizeXOfImage { get; private set; } = -1;

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x0001370C File Offset: 0x0001190C
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x00013714 File Offset: 0x00011914
		public int NormalSizeYOfImage { get; private set; } = -1;

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001371D File Offset: 0x0001191D
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x00013725 File Offset: 0x00011925
		public int CurrentSizeXOfImage { get; private set; } = -1;

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0001372E File Offset: 0x0001192E
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x00013736 File Offset: 0x00011936
		public int CurrentSizeYOfImage { get; private set; } = -1;

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0001373F File Offset: 0x0001193F
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x00013747 File Offset: 0x00011947
		public bool IsKeyToTheLeftOfTheGamepad { get; private set; }

		// Token: 0x060006C4 RID: 1732 RVA: 0x00013750 File Offset: 0x00011950
		public OptionsGamepadKeyLocationWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00013780 File Offset: 0x00011980
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._valuesInitialized)
			{
				this.NormalSizeXOfImage = base.ParentWidget.Sprite.Width;
				this.NormalSizeYOfImage = base.ParentWidget.Sprite.Height;
				this.CurrentSizeXOfImage = (int)(base.ParentWidget.SuggestedWidth * base._scaleToUse);
				this.CurrentSizeYOfImage = (int)(base.ParentWidget.SuggestedHeight * base._scaleToUse);
				this._keyVisualWidget = null;
				this._keyNameTextWidgets.Clear();
				List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
				for (int i = 0; i < allChildrenRecursive.Count; i++)
				{
					TextWidget textWidget;
					if ((textWidget = allChildrenRecursive[i] as TextWidget) != null)
					{
						this._keyNameTextWidgets.Add(textWidget);
					}
					InputKeyVisualWidget inputKeyVisualWidget;
					if (this._keyVisualWidget == null && (inputKeyVisualWidget = allChildrenRecursive[i] as InputKeyVisualWidget) != null)
					{
						this._keyVisualWidget = inputKeyVisualWidget;
					}
				}
				this._valuesInitialized = true;
				this.IsKeyToTheLeftOfTheGamepad = (float)this.NormalPositionXOffset < (float)this.NormalSizeXOfImage / 2f;
			}
			float num = base.ParentWidget.SuggestedWidth / (float)this.NormalSizeXOfImage;
			float num2 = base.ParentWidget.SuggestedHeight / (float)this.NormalSizeYOfImage;
			base.PositionXOffset = (float)this.NormalPositionXOffset * num;
			base.PositionYOffset = (float)this.NormalPositionYOffset * num2;
			List<TextWidget> keyNameTextWidgets = this._keyNameTextWidgets;
			if (keyNameTextWidgets != null && keyNameTextWidgets.Count == 1)
			{
				this._keyNameTextWidgets[0].Text = this._actionText;
			}
			base.IsVisible = !string.IsNullOrEmpty(this._actionText) || this.ForceVisible;
			if (this._valuesInitialized)
			{
				if (this.IsKeyToTheLeftOfTheGamepad)
				{
					this._keyNameTextWidgets.ForEach(delegate(TextWidget t)
					{
						t.ScaledSuggestedWidth = MathF.Abs(this._parentAreaWidget.GlobalPosition.X - this._keyVisualWidget.GlobalPosition.X);
						t.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Right;
					});
					return;
				}
				this._keyNameTextWidgets.ForEach(delegate(TextWidget t)
				{
					t.ScaledSuggestedWidth = this._parentAreaWidget.GlobalPosition.X + this._parentAreaWidget.Size.X - (this._keyVisualWidget.GlobalPosition.X + this._keyVisualWidget.Size.X);
					t.Brush.TextHorizontalAlignment = TextHorizontalAlignment.Left;
				});
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001395C File Offset: 0x00011B5C
		internal void SetKeyProperties(string actionText, Widget parentAreaWidget)
		{
			this._actionText = actionText;
			List<TextWidget> keyNameTextWidgets = this._keyNameTextWidgets;
			if (keyNameTextWidgets != null && keyNameTextWidgets.Count == 1)
			{
				this._keyNameTextWidgets[0].Text = this._actionText;
			}
			this._parentAreaWidget = parentAreaWidget;
			this._valuesInitialized = false;
		}

		// Token: 0x040002E8 RID: 744
		private bool _valuesInitialized;

		// Token: 0x040002E9 RID: 745
		private string _actionText;

		// Token: 0x040002EA RID: 746
		private Widget _parentAreaWidget;

		// Token: 0x040002EB RID: 747
		private List<TextWidget> _keyNameTextWidgets = new List<TextWidget>();

		// Token: 0x040002EC RID: 748
		private InputKeyVisualWidget _keyVisualWidget;
	}
}
