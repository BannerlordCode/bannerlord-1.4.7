using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004F RID: 79
	public class TutorialPanelImageWidget : ImageWidget
	{
		// Token: 0x0600044C RID: 1100 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public TutorialPanelImageWidget(UIContext context)
			: base(context)
		{
			base.UseGlobalTimeForAnimation = true;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000DD68 File Offset: 0x0000BF68
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animState == TutorialPanelImageWidget.AnimState.Start)
			{
				this._tickCount++;
				if (this._tickCount > 20)
				{
					this._animState = TutorialPanelImageWidget.AnimState.Starting;
					return;
				}
			}
			else if (this._animState == TutorialPanelImageWidget.AnimState.Starting)
			{
				BrushListPanel tutorialPanel = this.TutorialPanel;
				if (tutorialPanel != null)
				{
					tutorialPanel.BrushRenderer.RestartAnimation();
				}
				this._animState = TutorialPanelImageWidget.AnimState.Playing;
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000DDCB File Offset: 0x0000BFCB
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this.Initialize();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		private void Initialize()
		{
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				this._animState = TutorialPanelImageWidget.AnimState.Idle;
				this._tickCount = 0;
			}
			else if (this._animState != TutorialPanelImageWidget.AnimState.Start)
			{
				this.SetState("Default");
				this._animState = TutorialPanelImageWidget.AnimState.Start;
				base.Context.TwoDimensionContext.PlaySound("panels/tutorial");
			}
			base.IsVisible = base.IsEnabled;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000DE48 File Offset: 0x0000C048
		protected override void RefreshState()
		{
			base.RefreshState();
			this.Initialize();
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0000DE56 File Offset: 0x0000C056
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x0000DE5E File Offset: 0x0000C05E
		[Editor(false)]
		public BrushListPanel TutorialPanel
		{
			get
			{
				return this._tutorialPanel;
			}
			set
			{
				if (this._tutorialPanel != value)
				{
					this._tutorialPanel = value;
					base.OnPropertyChanged<BrushListPanel>(value, "TutorialPanel");
					if (this._tutorialPanel != null)
					{
						this._tutorialPanel.UseGlobalTimeForAnimation = true;
					}
				}
			}
		}

		// Token: 0x040001D5 RID: 469
		private TutorialPanelImageWidget.AnimState _animState;

		// Token: 0x040001D6 RID: 470
		private int _tickCount;

		// Token: 0x040001D7 RID: 471
		private BrushListPanel _tutorialPanel;

		// Token: 0x020001AE RID: 430
		public enum AnimState
		{
			// Token: 0x040009DC RID: 2524
			Idle,
			// Token: 0x040009DD RID: 2525
			Start,
			// Token: 0x040009DE RID: 2526
			Starting,
			// Token: 0x040009DF RID: 2527
			Playing
		}
	}
}
