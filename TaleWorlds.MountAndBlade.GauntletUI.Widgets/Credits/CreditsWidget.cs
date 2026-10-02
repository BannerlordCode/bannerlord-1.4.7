using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Credits
{
	// Token: 0x02000161 RID: 353
	public class CreditsWidget : Widget
	{
		// Token: 0x060012A1 RID: 4769 RVA: 0x000333CF File Offset: 0x000315CF
		public CreditsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00033404 File Offset: 0x00031604
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.RootItemWidget != null)
			{
				this.RootItemWidget.PositionYOffset = this._currentOffset;
				if (this._doNotScrollTimer > 0f)
				{
					this._doNotScrollTimer -= dt;
				}
				else
				{
					this._targetOffset -= dt * this.ScrollPixelsPerSecond;
				}
				this._currentOffset = MathF.Lerp(this._currentOffset, this._targetOffset, MathF.Min(1f, dt * 10f), 1E-05f);
				if (this._currentOffset < -this.RootItemWidget.Size.Y * base._inverseScaleToUse)
				{
					this._currentOffset = 1080f;
					this._targetOffset = 1080f;
				}
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x000334C9 File Offset: 0x000316C9
		protected override bool OnPreviewMouseScroll()
		{
			return true;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x000334CC File Offset: 0x000316CC
		protected override bool OnPreviewRightStickMovement()
		{
			return true;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x000334CF File Offset: 0x000316CF
		protected override void OnMouseScroll()
		{
			base.OnMouseScroll();
			this.OnScroll(base.EventManager.DeltaMouseScroll * 0.5f);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000334EE File Offset: 0x000316EE
		protected override void OnRightStickMovement()
		{
			base.OnRightStickMovement();
			this.OnScroll(base.EventManager.RightStickVerticalScrollAmount);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00033508 File Offset: 0x00031708
		private void OnScroll(float scrollAmount)
		{
			if (this._targetOffset <= 0f || scrollAmount <= 0f)
			{
				this._targetOffset += scrollAmount;
				this._targetOffset = MathF.Min(this._targetOffset, 0f);
			}
			this._doNotScrollTimer = this.ManualScrollWaitTimer;
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0003355A File Offset: 0x0003175A
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x00033562 File Offset: 0x00031762
		[Editor(false)]
		public Widget RootItemWidget
		{
			get
			{
				return this._rootItemWidget;
			}
			set
			{
				if (this._rootItemWidget != value)
				{
					this._rootItemWidget = value;
					base.OnPropertyChanged<Widget>(value, "RootItemWidget");
				}
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x00033580 File Offset: 0x00031780
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x00033588 File Offset: 0x00031788
		[Editor(false)]
		public float ScrollPixelsPerSecond
		{
			get
			{
				return this._scrollPixelsPerSecond;
			}
			set
			{
				if (this._scrollPixelsPerSecond != value)
				{
					this._scrollPixelsPerSecond = value;
					base.OnPropertyChanged(value, "ScrollPixelsPerSecond");
				}
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x000335A6 File Offset: 0x000317A6
		// (set) Token: 0x060012AD RID: 4781 RVA: 0x000335AE File Offset: 0x000317AE
		[Editor(false)]
		public float ManualScrollWaitTimer
		{
			get
			{
				return this._manualScrollWaitTimer;
			}
			set
			{
				if (this._manualScrollWaitTimer != value)
				{
					this._manualScrollWaitTimer = value;
					base.OnPropertyChanged(value, "ManualScrollWaitTimer");
				}
			}
		}

		// Token: 0x04000875 RID: 2165
		private float _currentOffset = 1080f;

		// Token: 0x04000876 RID: 2166
		private float _targetOffset = 1080f;

		// Token: 0x04000877 RID: 2167
		private float _doNotScrollTimer;

		// Token: 0x04000878 RID: 2168
		private Widget _rootItemWidget;

		// Token: 0x04000879 RID: 2169
		private float _scrollPixelsPerSecond = 75f;

		// Token: 0x0400087A RID: 2170
		private float _manualScrollWaitTimer = 1f;
	}
}
