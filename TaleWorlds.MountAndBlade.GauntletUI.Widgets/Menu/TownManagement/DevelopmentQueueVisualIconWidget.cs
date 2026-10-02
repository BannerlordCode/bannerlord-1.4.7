using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010B RID: 267
	public class DevelopmentQueueVisualIconWidget : Widget
	{
		// Token: 0x06000E3E RID: 3646 RVA: 0x00027507 File Offset: 0x00025707
		public DevelopmentQueueVisualIconWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00027518 File Offset: 0x00025718
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animState == DevelopmentQueueVisualIconWidget.AnimState.Start)
			{
				this._tickCount += 1f;
				if (this._tickCount > 20f)
				{
					this._animState = DevelopmentQueueVisualIconWidget.AnimState.Starting;
					return;
				}
			}
			else if (this._animState == DevelopmentQueueVisualIconWidget.AnimState.Starting)
			{
				BrushWidget inProgressIconWidget = this.InProgressIconWidget;
				if (inProgressIconWidget != null)
				{
					inProgressIconWidget.BrushRenderer.RestartAnimation();
				}
				this._animState = DevelopmentQueueVisualIconWidget.AnimState.Playing;
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00027584 File Offset: 0x00025784
		private void UpdateVisual(int index)
		{
			if (this.InProgressIconWidget != null && this.QueueIconWidget != null)
			{
				base.IsVisible = index >= 0;
				this.InProgressIconWidget.IsVisible = index == 0;
				this._animState = (this.InProgressIconWidget.IsVisible ? DevelopmentQueueVisualIconWidget.AnimState.Start : DevelopmentQueueVisualIconWidget.AnimState.Idle);
				this._tickCount = 0f;
				this.QueueIconWidget.IsVisible = index > 0;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x000275EE File Offset: 0x000257EE
		// (set) Token: 0x06000E42 RID: 3650 RVA: 0x000275F6 File Offset: 0x000257F6
		[Editor(false)]
		public int QueueIndex
		{
			get
			{
				return this._queueIndex;
			}
			set
			{
				if (this._queueIndex != value)
				{
					this._queueIndex = value;
					base.OnPropertyChanged(value, "QueueIndex");
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0002761B File Offset: 0x0002581B
		// (set) Token: 0x06000E44 RID: 3652 RVA: 0x00027623 File Offset: 0x00025823
		[Editor(false)]
		public Widget QueueIconWidget
		{
			get
			{
				return this._queueIconWidget;
			}
			set
			{
				if (this._queueIconWidget != value)
				{
					this._queueIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "QueueIconWidget");
					this.UpdateVisual(this.QueueIndex);
				}
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0002764D File Offset: 0x0002584D
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x00027655 File Offset: 0x00025855
		[Editor(false)]
		public BrushWidget InProgressIconWidget
		{
			get
			{
				return this._inProgressIconWidget;
			}
			set
			{
				if (this._inProgressIconWidget != value)
				{
					this._inProgressIconWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "InProgressIconWidget");
					this.UpdateVisual(this.QueueIndex);
				}
			}
		}

		// Token: 0x04000676 RID: 1654
		private DevelopmentQueueVisualIconWidget.AnimState _animState;

		// Token: 0x04000677 RID: 1655
		private float _tickCount;

		// Token: 0x04000678 RID: 1656
		private int _queueIndex = -1;

		// Token: 0x04000679 RID: 1657
		private Widget _queueIconWidget;

		// Token: 0x0400067A RID: 1658
		private BrushWidget _inProgressIconWidget;

		// Token: 0x020001C7 RID: 455
		public enum AnimState
		{
			// Token: 0x04000A22 RID: 2594
			Idle,
			// Token: 0x04000A23 RID: 2595
			Start,
			// Token: 0x04000A24 RID: 2596
			Starting,
			// Token: 0x04000A25 RID: 2597
			Playing
		}
	}
}
