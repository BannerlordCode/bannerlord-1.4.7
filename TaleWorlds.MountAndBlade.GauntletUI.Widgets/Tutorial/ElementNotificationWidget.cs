using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x02000048 RID: 72
	public class ElementNotificationWidget : Widget
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x0000C855 File Offset: 0x0000AA55
		public ElementNotificationWidget(UIContext context)
			: base(context)
		{
			base.IsVisible = false;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000C870 File Offset: 0x0000AA70
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			string elementID = this.ElementID;
			if (elementID != null && elementID.Any<char>() && this.ElementToHighlight == null && !this._doesNotHaveElement)
			{
				this.ElementToHighlight = this.FindElementWithID(base.EventManager.Root, this.ElementID);
				this._doesNotHaveElement = true;
				if (this.ElementToHighlight != null)
				{
					this.TutorialFrameWidget.IsVisible = true;
					this.TutorialFrameWidget.IsHighlightEnabled = true;
					this.TutorialFrameWidget.ParentWidget = this.ElementToHighlight;
					if (this.ElementToHighlight.HeightSizePolicy == SizePolicy.CoverChildren || this.ElementToHighlight.WidthSizePolicy == SizePolicy.CoverChildren)
					{
						this.TutorialFrameWidget.WidthSizePolicy = SizePolicy.Fixed;
						this.TutorialFrameWidget.HeightSizePolicy = SizePolicy.Fixed;
						this._shouldSyncSize = true;
					}
					else
					{
						this.TutorialFrameWidget.WidthSizePolicy = SizePolicy.StretchToParent;
						this.TutorialFrameWidget.HeightSizePolicy = SizePolicy.StretchToParent;
						this._shouldSyncSize = false;
					}
				}
			}
			if (this._shouldSyncSize && this.ElementToHighlight != null && this.ElementToHighlight.Size.X > 1f && this.ElementToHighlight.Size.Y > 1f)
			{
				base.ScaledSuggestedWidth = this.ElementToHighlight.Size.X - 1f;
				base.ScaledSuggestedHeight = this.ElementToHighlight.Size.Y - 1f;
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000C9DC File Offset: 0x0000ABDC
		private Widget FindElementWithID(Widget current, string ID)
		{
			if (current != null)
			{
				for (int i = 0; i < current.ChildCount; i++)
				{
					if (current.GetChild(i).Id == ID)
					{
						return current.GetChild(i);
					}
					Widget widget = this.FindElementWithID(current.GetChild(i), ID);
					if (widget != null)
					{
						return widget;
					}
				}
			}
			return null;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000CA2E File Offset: 0x0000AC2E
		private void ResetHighlight()
		{
			if (this.TutorialFrameWidget != null)
			{
				this.TutorialFrameWidget.ParentWidget = this;
				this._doesNotHaveElement = false;
				this.TutorialFrameWidget.IsVisible = false;
				this.TutorialFrameWidget.IsHighlightEnabled = false;
				this.ElementToHighlight = null;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000CA6A File Offset: 0x0000AC6A
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0000CA74 File Offset: 0x0000AC74
		[Editor(false)]
		public string ElementID
		{
			get
			{
				return this._elementID;
			}
			set
			{
				if (this._elementID != value)
				{
					if (this._elementID != string.Empty && value == string.Empty)
					{
						this.ResetHighlight();
					}
					this._elementID = value;
					base.OnPropertyChanged<string>(value, "ElementID");
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000CAC7 File Offset: 0x0000ACC7
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0000CACF File Offset: 0x0000ACCF
		[Editor(false)]
		public Widget ElementToHighlight
		{
			get
			{
				return this._elementToHighlight;
			}
			set
			{
				if (this._elementToHighlight != value)
				{
					this._elementToHighlight = value;
					base.OnPropertyChanged<Widget>(value, "ElementToHighlight");
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000CAED File Offset: 0x0000ACED
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0000CAF5 File Offset: 0x0000ACF5
		[Editor(false)]
		public TutorialHighlightItemBrushWidget TutorialFrameWidget
		{
			get
			{
				return this._tutorialFrameWidget;
			}
			set
			{
				if (this._tutorialFrameWidget != value)
				{
					this._tutorialFrameWidget = value;
					base.OnPropertyChanged<TutorialHighlightItemBrushWidget>(value, "TutorialFrameWidget");
					if (this._tutorialFrameWidget != null)
					{
						this._tutorialFrameWidget.IsVisible = false;
					}
				}
			}
		}

		// Token: 0x040001A8 RID: 424
		private bool _doesNotHaveElement;

		// Token: 0x040001A9 RID: 425
		private bool _shouldSyncSize;

		// Token: 0x040001AA RID: 426
		private string _elementID = string.Empty;

		// Token: 0x040001AB RID: 427
		private Widget _elementToHighlight;

		// Token: 0x040001AC RID: 428
		private TutorialHighlightItemBrushWidget _tutorialFrameWidget;
	}
}
