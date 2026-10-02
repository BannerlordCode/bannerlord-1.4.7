using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.Launcher.Library.CustomWidgets
{
	// Token: 0x02000024 RID: 36
	public class LauncherNewsWidget : Widget
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000066F1 File Offset: 0x000048F1
		// (set) Token: 0x0600016A RID: 362 RVA: 0x000066F9 File Offset: 0x000048F9
		public float TimeToShowNewsItemInSeconds { get; set; } = 6.5f;

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006702 File Offset: 0x00004902
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000670A File Offset: 0x0000490A
		public ListPanel RadioButtonContainer { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00006713 File Offset: 0x00004913
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000671B File Offset: 0x0000491B
		public Widget TimeLeftFillWidget { get; set; }

		// Token: 0x0600016F RID: 367 RVA: 0x00006724 File Offset: 0x00004924
		public LauncherNewsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006740 File Offset: 0x00004940
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._firstFrame)
			{
				this._templateRadioButton = this.RadioButtonContainer.GetChild(0) as ButtonWidget;
				this._templateRadioButton.ClickEventHandlers.Add(new Action<Widget>(this.OnNewsRadioButtonClick));
				this._templateRadioButton.IsVisible = false;
				this._firstFrame = false;
			}
			if (base.ChildCount > 1)
			{
				this._currentNewsVisibleTime += dt;
				if (this._currentNewsVisibleTime >= this.TimeToShowNewsItemInSeconds)
				{
					int num = (this._currentShownNewsIndex + 1) % base.ChildCount;
					this.SetCurrentNewsItemIndex(num);
				}
				this.TimeLeftFillWidget.SuggestedWidth = this._currentNewsVisibleTime / this.TimeToShowNewsItemInSeconds * this.TimeLeftFillWidget.ParentWidget.Size.X * base._inverseScaleToUse;
			}
			else
			{
				this._currentNewsVisibleTime = 0f;
				this.TimeLeftFillWidget.SuggestedWidth = 0f;
			}
			if (this._isRadioButtonVisibilityDirty)
			{
				bool flag = base.ChildCount > 1;
				for (int i = 0; i < this.RadioButtonContainer.ChildCount; i++)
				{
					this.RadioButtonContainer.GetChild(i).IsVisible = flag;
				}
				this._isRadioButtonVisibilityDirty = false;
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006874 File Offset: 0x00004A74
		private void OnNewsRadioButtonClick(Widget obj)
		{
			int siblingIndex = obj.GetSiblingIndex();
			this.SetCurrentNewsItemIndex(siblingIndex);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006890 File Offset: 0x00004A90
		private void SetCurrentNewsItemIndex(int indexOfNewsItem)
		{
			if (indexOfNewsItem != this._currentShownNewsIndex)
			{
				(this.RadioButtonContainer.GetChild(this._currentShownNewsIndex) as ButtonWidget).IsSelected = false;
				base.GetChild(this._currentShownNewsIndex).IsVisible = false;
				this._currentShownNewsIndex = indexOfNewsItem;
				(this.RadioButtonContainer.GetChild(this._currentShownNewsIndex) as ButtonWidget).IsSelected = true;
				base.GetChild(this._currentShownNewsIndex).IsVisible = true;
				base.GetChild(this._currentShownNewsIndex).GetChild(0).GetChild(0)
					.SetGlobalAlphaRecursively(0f);
				this._currentNewsVisibleTime = 0f;
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000693C File Offset: 0x00004B3C
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.IsVisible = child.GetSiblingIndex() == this._currentShownNewsIndex;
			if (this.RadioButtonContainer.ChildCount != base.ChildCount)
			{
				this.RadioButtonContainer.AddChild(this.GetDefaultNewsItemRadioButton());
			}
			(this.RadioButtonContainer.GetChild(this._currentShownNewsIndex) as ButtonWidget).IsSelected = true;
			this._isRadioButtonVisibilityDirty = true;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000069AC File Offset: 0x00004BAC
		protected override void OnAfterChildRemoved(Widget child, int previousIndexOfChild)
		{
			base.OnAfterChildRemoved(child, previousIndexOfChild);
			if (this.RadioButtonContainer.ChildCount != base.ChildCount)
			{
				this.RadioButtonContainer.RemoveChild(this.RadioButtonContainer.GetChild(this.RadioButtonContainer.ChildCount - 1));
			}
			if (this._currentShownNewsIndex >= this.RadioButtonContainer.ChildCount && this._currentShownNewsIndex > 0)
			{
				this._currentShownNewsIndex--;
				(this.RadioButtonContainer.GetChild(this._currentShownNewsIndex) as ButtonWidget).IsSelected = true;
				base.GetChild(this._currentShownNewsIndex).IsVisible = child.GetSiblingIndex() == this._currentShownNewsIndex;
			}
			this._isRadioButtonVisibilityDirty = true;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006A64 File Offset: 0x00004C64
		private ButtonWidget GetDefaultNewsItemRadioButton()
		{
			return new ButtonWidget(base.Context)
			{
				WidthSizePolicy = this._templateRadioButton.WidthSizePolicy,
				HeightSizePolicy = this._templateRadioButton.HeightSizePolicy,
				Brush = this._templateRadioButton.ReadOnlyBrush,
				SuggestedHeight = this._templateRadioButton.SuggestedHeight,
				SuggestedWidth = this._templateRadioButton.SuggestedWidth,
				ScaledSuggestedWidth = this._templateRadioButton.ScaledSuggestedWidth,
				ScaledSuggestedHeight = this._templateRadioButton.ScaledSuggestedHeight,
				MarginLeft = this._templateRadioButton.MarginLeft,
				MarginRight = this._templateRadioButton.MarginRight,
				MarginTop = this._templateRadioButton.MarginTop,
				MarginBottom = this._templateRadioButton.MarginBottom,
				ClickEventHandlers = 
				{
					new Action<Widget>(this.OnNewsRadioButtonClick)
				}
			};
		}

		// Token: 0x040000AE RID: 174
		private int _currentShownNewsIndex;

		// Token: 0x040000AF RID: 175
		private float _currentNewsVisibleTime;

		// Token: 0x040000B2 RID: 178
		private ButtonWidget _templateRadioButton;

		// Token: 0x040000B3 RID: 179
		private bool _firstFrame = true;

		// Token: 0x040000B4 RID: 180
		private bool _isRadioButtonVisibilityDirty;
	}
}
