using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Disguise
{
	// Token: 0x02000101 RID: 257
	public class MissionSuspicionFillerBrushWidget : Widget
	{
		// Token: 0x06000DCC RID: 3532 RVA: 0x00025E09 File Offset: 0x00024009
		public MissionSuspicionFillerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00025E14 File Offset: 0x00024014
		private void UpdateBrushState(float suspicionRatio)
		{
			if (suspicionRatio >= 1f)
			{
				BrushWidget circleIcon = this.CircleIcon;
				if (circleIcon != null)
				{
					circleIcon.SetState("Full");
				}
				Widget detectionFillContainer = this.DetectionFillContainer;
				if (detectionFillContainer != null)
				{
					detectionFillContainer.SetState("Full");
				}
				BrushWidget exclamationMark = this.ExclamationMark;
				if (exclamationMark == null)
				{
					return;
				}
				exclamationMark.SetState("Full");
				return;
			}
			else if (suspicionRatio > this._currentSuspicionRatio)
			{
				BrushWidget circleIcon2 = this.CircleIcon;
				if (circleIcon2 != null)
				{
					circleIcon2.SetState("Increasing");
				}
				Widget detectionFillContainer2 = this.DetectionFillContainer;
				if (detectionFillContainer2 != null)
				{
					detectionFillContainer2.SetState("Increasing");
				}
				BrushWidget exclamationMark2 = this.ExclamationMark;
				if (exclamationMark2 == null)
				{
					return;
				}
				exclamationMark2.SetState("Increasing");
				return;
			}
			else
			{
				BrushWidget circleIcon3 = this.CircleIcon;
				if (circleIcon3 != null)
				{
					circleIcon3.SetState("Decreasing");
				}
				Widget detectionFillContainer3 = this.DetectionFillContainer;
				if (detectionFillContainer3 != null)
				{
					detectionFillContainer3.SetState("Decreasing");
				}
				BrushWidget exclamationMark3 = this.ExclamationMark;
				if (exclamationMark3 == null)
				{
					return;
				}
				exclamationMark3.SetState("Decreasing");
				return;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00025EF7 File Offset: 0x000240F7
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x00025EFF File Offset: 0x000240FF
		public float CurrentSuspicionRatio
		{
			get
			{
				return this._currentSuspicionRatio;
			}
			set
			{
				if (value != this._currentSuspicionRatio)
				{
					this.UpdateBrushState(value);
					this._currentSuspicionRatio = value;
					base.OnPropertyChanged(value, "CurrentSuspicionRatio");
				}
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00025F24 File Offset: 0x00024124
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00025F2C File Offset: 0x0002412C
		public BrushWidget ExclamationMark
		{
			get
			{
				return this._exclamationMark;
			}
			set
			{
				if (value != this._exclamationMark)
				{
					this._exclamationMark = value;
					base.OnPropertyChanged<BrushWidget>(value, "ExclamationMark");
				}
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00025F4A File Offset: 0x0002414A
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00025F52 File Offset: 0x00024152
		public Widget DetectionFillContainer
		{
			get
			{
				return this._detectionFillContainer;
			}
			set
			{
				if (value != this._detectionFillContainer)
				{
					this._detectionFillContainer = value;
					base.OnPropertyChanged<Widget>(value, "DetectionFillContainer");
				}
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00025F70 File Offset: 0x00024170
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x00025F78 File Offset: 0x00024178
		public BrushWidget CircleIcon
		{
			get
			{
				return this._circleIcon;
			}
			set
			{
				if (value != this._circleIcon)
				{
					this._circleIcon = value;
					base.OnPropertyChanged<BrushWidget>(value, "CircleIcon");
				}
			}
		}

		// Token: 0x04000641 RID: 1601
		private float _currentSuspicionRatio;

		// Token: 0x04000642 RID: 1602
		private BrushWidget _exclamationMark;

		// Token: 0x04000643 RID: 1603
		private Widget _detectionFillContainer;

		// Token: 0x04000644 RID: 1604
		private BrushWidget _circleIcon;
	}
}
