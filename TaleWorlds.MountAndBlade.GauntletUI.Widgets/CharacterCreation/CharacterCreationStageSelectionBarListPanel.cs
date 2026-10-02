using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation
{
	// Token: 0x02000188 RID: 392
	public class CharacterCreationStageSelectionBarListPanel : ListPanel
	{
		// Token: 0x06001445 RID: 5189 RVA: 0x0003727B File Offset: 0x0003547B
		public CharacterCreationStageSelectionBarListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x000372A4 File Offset: 0x000354A4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.RefreshButtonList();
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x000372B4 File Offset: 0x000354B4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.BarFillWidget != null && this.TotalStagesCount != 0 && this._buttonsInitialized && this.CurrentStageIndex != -1)
			{
				this.BarFillWidget.ScaledSuggestedWidth = this.BarCanvasWidget.Size.X - this._stageButtonsList[this._stageButtonsList.Count - 1 - this.CurrentStageIndex].LocalPosition.X;
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00037330 File Offset: 0x00035530
		private void RefreshButtonList()
		{
			if (!this._buttonsInitialized)
			{
				this._stageButtonsList = new List<ButtonWidget>();
				if (base.HasChild(this.StageButtonTemplate))
				{
					base.RemoveChild(this.StageButtonTemplate);
				}
				base.RemoveAllChildren();
				if (this.StageButtonTemplate != null && this.EmptyButtonBrush != null && this.FullButtonBrush != null && this.FullBrightButtonBrush != null)
				{
					if (this.TotalStagesCount == 0)
					{
						this.BarCanvasWidget.IsVisible = false;
						base.IsVisible = false;
					}
					else
					{
						for (int i = 0; i < this.TotalStagesCount; i++)
						{
							ButtonWidget buttonWidget = new ButtonWidget(base.Context);
							base.AddChild(buttonWidget);
							buttonWidget.Brush = this.StageButtonTemplate.ReadOnlyBrush;
							bool flag = false;
							if (i == this.CurrentStageIndex)
							{
								buttonWidget.Brush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == this.FullBrightButtonBrush);
							}
							else if (i <= this.OpenedStageIndex || (this.OpenedStageIndex == -1 && i < this.CurrentStageIndex))
							{
								buttonWidget.Brush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == this.FullButtonBrush);
								flag = true;
							}
							else
							{
								buttonWidget.Brush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == this.EmptyButtonBrush);
							}
							buttonWidget.DoNotAcceptEvents = !flag;
							buttonWidget.SuggestedHeight = this.StageButtonTemplate.SuggestedHeight;
							buttonWidget.SuggestedWidth = this.StageButtonTemplate.SuggestedWidth;
							buttonWidget.DoNotPassEventsToChildren = this.StageButtonTemplate.DoNotPassEventsToChildren;
							buttonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnStageSelection));
							this._stageButtonsList.Add(buttonWidget);
						}
					}
					this._buttonsInitialized = true;
				}
			}
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00037504 File Offset: 0x00035704
		private void OnStageSelection(Widget stageButton)
		{
			int num = this._stageButtonsList.IndexOf(stageButton as ButtonWidget);
			base.EventFired("OnStageSelection", new object[] { num });
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x0003753D File Offset: 0x0003573D
		// (set) Token: 0x0600144B RID: 5195 RVA: 0x00037545 File Offset: 0x00035745
		[Editor(false)]
		public ButtonWidget StageButtonTemplate
		{
			get
			{
				return this._stageButtonTemplate;
			}
			set
			{
				if (this._stageButtonTemplate != value)
				{
					this._stageButtonTemplate = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StageButtonTemplate");
					if (value != null)
					{
						base.RemoveChild(value);
					}
				}
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0003756D File Offset: 0x0003576D
		// (set) Token: 0x0600144D RID: 5197 RVA: 0x00037575 File Offset: 0x00035775
		[Editor(false)]
		public Widget BarFillWidget
		{
			get
			{
				return this._barFillWidget;
			}
			set
			{
				if (this._barFillWidget != value)
				{
					this._barFillWidget = value;
					base.OnPropertyChanged<Widget>(value, "BarFillWidget");
				}
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00037593 File Offset: 0x00035793
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x0003759B File Offset: 0x0003579B
		[Editor(false)]
		public Widget BarCanvasWidget
		{
			get
			{
				return this._barCanvasWidget;
			}
			set
			{
				if (this._barCanvasWidget != value)
				{
					this._barCanvasWidget = value;
					base.OnPropertyChanged<Widget>(value, "BarCanvasWidget");
				}
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x000375B9 File Offset: 0x000357B9
		// (set) Token: 0x06001451 RID: 5201 RVA: 0x000375C1 File Offset: 0x000357C1
		[Editor(false)]
		public int CurrentStageIndex
		{
			get
			{
				return this._currentStageIndex;
			}
			set
			{
				if (this._currentStageIndex != value)
				{
					this._currentStageIndex = value;
					base.OnPropertyChanged(value, "CurrentStageIndex");
					this._buttonsInitialized = false;
				}
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x000375E6 File Offset: 0x000357E6
		// (set) Token: 0x06001453 RID: 5203 RVA: 0x000375EE File Offset: 0x000357EE
		[Editor(false)]
		public int TotalStagesCount
		{
			get
			{
				return this._totalStagesCount;
			}
			set
			{
				if (this._totalStagesCount != value)
				{
					this._totalStagesCount = value;
					base.OnPropertyChanged(value, "TotalStagesCount");
				}
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x0003760C File Offset: 0x0003580C
		// (set) Token: 0x06001455 RID: 5205 RVA: 0x00037614 File Offset: 0x00035814
		[Editor(false)]
		public int OpenedStageIndex
		{
			get
			{
				return this._openedStageIndex;
			}
			set
			{
				if (this._openedStageIndex != value)
				{
					this._openedStageIndex = value;
					base.OnPropertyChanged(value, "OpenedStageIndex");
				}
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00037632 File Offset: 0x00035832
		// (set) Token: 0x06001457 RID: 5207 RVA: 0x0003763A File Offset: 0x0003583A
		[Editor(false)]
		public string FullButtonBrush
		{
			get
			{
				return this._fullButtonBrush;
			}
			set
			{
				if (this._fullButtonBrush != value)
				{
					this._fullButtonBrush = value;
					base.OnPropertyChanged<string>(value, "FullButtonBrush");
				}
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0003765D File Offset: 0x0003585D
		// (set) Token: 0x06001459 RID: 5209 RVA: 0x00037665 File Offset: 0x00035865
		[Editor(false)]
		public string EmptyButtonBrush
		{
			get
			{
				return this._emptyButtonBrush;
			}
			set
			{
				if (this._emptyButtonBrush != value)
				{
					this._emptyButtonBrush = value;
					base.OnPropertyChanged<string>(value, "EmptyButtonBrush");
				}
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x00037688 File Offset: 0x00035888
		// (set) Token: 0x0600145B RID: 5211 RVA: 0x00037690 File Offset: 0x00035890
		[Editor(false)]
		public string FullBrightButtonBrush
		{
			get
			{
				return this._fullBrightButtonBrush;
			}
			set
			{
				if (this._fullBrightButtonBrush != value)
				{
					this._fullBrightButtonBrush = value;
					base.OnPropertyChanged<string>(value, "FullBrightButtonBrush");
				}
			}
		}

		// Token: 0x04000932 RID: 2354
		private List<ButtonWidget> _stageButtonsList = new List<ButtonWidget>();

		// Token: 0x04000933 RID: 2355
		private bool _buttonsInitialized;

		// Token: 0x04000934 RID: 2356
		private ButtonWidget _stageButtonTemplate;

		// Token: 0x04000935 RID: 2357
		private int _currentStageIndex = -1;

		// Token: 0x04000936 RID: 2358
		private int _totalStagesCount = -1;

		// Token: 0x04000937 RID: 2359
		private int _openedStageIndex = -1;

		// Token: 0x04000938 RID: 2360
		private string _fullButtonBrush;

		// Token: 0x04000939 RID: 2361
		private string _emptyButtonBrush;

		// Token: 0x0400093A RID: 2362
		private string _fullBrightButtonBrush;

		// Token: 0x0400093B RID: 2363
		private Widget _barFillWidget;

		// Token: 0x0400093C RID: 2364
		private Widget _barCanvasWidget;
	}
}
