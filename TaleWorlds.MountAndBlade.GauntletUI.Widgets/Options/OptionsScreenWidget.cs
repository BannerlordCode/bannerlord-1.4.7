using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000079 RID: 121
	public class OptionsScreenWidget : Widget
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00013391 File Offset: 0x00011591
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x00013399 File Offset: 0x00011599
		public Widget VideoMemoryUsageWidget { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x000133A2 File Offset: 0x000115A2
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x000133AA File Offset: 0x000115AA
		public RichTextWidget CurrentOptionDescriptionWidget { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x000133B3 File Offset: 0x000115B3
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x000133BB File Offset: 0x000115BB
		public RichTextWidget CurrentOptionNameWidget { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x000133C4 File Offset: 0x000115C4
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x000133CC File Offset: 0x000115CC
		public RichTextWidget CurrentOptionExtraInformationWidget { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x000133D5 File Offset: 0x000115D5
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x000133DD File Offset: 0x000115DD
		public Widget CurrentOptionImageWidget { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000133E6 File Offset: 0x000115E6
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x000133EE File Offset: 0x000115EE
		public TabToggleWidget PerformanceTabToggle { get; set; }

		// Token: 0x060006A2 RID: 1698 RVA: 0x000133F7 File Offset: 0x000115F7
		public OptionsScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00013400 File Offset: 0x00011600
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.PerformanceTabToggle.TabControlWidget.OnActiveTabChange += this.OnActiveTabChange;
				this.VideoMemoryUsageWidget.IsVisible = false;
				this._initialized = true;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00013440 File Offset: 0x00011640
		private void OnActiveTabChange()
		{
			this.VideoMemoryUsageWidget.IsVisible = this.PerformanceTabToggle.TabControlWidget.ActiveTab.Id == "PerformanceOptionsPage";
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001346C File Offset: 0x0001166C
		protected override void OnDisconnectedFromRoot()
		{
			base.OnDisconnectedFromRoot();
			TabToggleWidget performanceTabToggle = this.PerformanceTabToggle;
			if (((performanceTabToggle != null) ? performanceTabToggle.TabControlWidget : null) != null)
			{
				this.PerformanceTabToggle.TabControlWidget.OnActiveTabChange += this.OnActiveTabChange;
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000134A4 File Offset: 0x000116A4
		public void SetCurrentOption(Widget currentOptionWidget, Sprite newgraphicsSprite)
		{
			if (this._currentOptionWidget != currentOptionWidget)
			{
				this._currentOptionWidget = currentOptionWidget;
				string text = "";
				string text2 = "";
				string text3 = "";
				if (this._currentOptionWidget != null)
				{
					OptionsItemWidget optionsItemWidget;
					OptionsKeyItemListPanel optionsKeyItemListPanel;
					if ((optionsItemWidget = this._currentOptionWidget as OptionsItemWidget) != null)
					{
						text = optionsItemWidget.OptionDescription;
						text2 = optionsItemWidget.OptionTitle;
					}
					else if ((optionsKeyItemListPanel = this._currentOptionWidget as OptionsKeyItemListPanel) != null)
					{
						text = optionsKeyItemListPanel.OptionDescription;
						text2 = optionsKeyItemListPanel.OptionTitle;
						text3 = optionsKeyItemListPanel.OptionExtraInformation;
					}
				}
				if (this.CurrentOptionDescriptionWidget != null)
				{
					this.CurrentOptionDescriptionWidget.Text = text;
				}
				if (this.CurrentOptionNameWidget != null)
				{
					this.CurrentOptionNameWidget.Text = text2;
				}
				if (this.CurrentOptionExtraInformationWidget != null)
				{
					this.CurrentOptionExtraInformationWidget.Text = text3;
				}
			}
			if (this.CurrentOptionImageWidget != null && this.CurrentOptionImageWidget.Sprite != newgraphicsSprite)
			{
				this.CurrentOptionImageWidget.Sprite = newgraphicsSprite;
				if (newgraphicsSprite != null)
				{
					float num = this.CurrentOptionImageWidget.SuggestedWidth / (float)newgraphicsSprite.Width;
					this.CurrentOptionImageWidget.SuggestedHeight = (float)newgraphicsSprite.Height * num;
				}
			}
		}

		// Token: 0x040002D2 RID: 722
		private Widget _currentOptionWidget;

		// Token: 0x040002D9 RID: 729
		private bool _initialized;
	}
}
