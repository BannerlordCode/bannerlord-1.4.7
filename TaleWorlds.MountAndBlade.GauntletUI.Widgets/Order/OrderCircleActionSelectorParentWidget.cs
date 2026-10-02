using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000070 RID: 112
	public class OrderCircleActionSelectorParentWidget : Widget
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00011DE8 File Offset: 0x0000FFE8
		public OrderCircleActionSelectorParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00011DF1 File Offset: 0x0000FFF1
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00011DFA File Offset: 0x0000FFFA
		private void UpdateInputRestrictions()
		{
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00011DFC File Offset: 0x0000FFFC
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x00011E04 File Offset: 0x00010004
		[Editor(false)]
		public bool IsInFreeCameraMode
		{
			get
			{
				return this._isInFreeCameraMode;
			}
			set
			{
				if (value != this._isInFreeCameraMode)
				{
					this._isInFreeCameraMode = value;
					base.OnPropertyChanged(value, "IsInFreeCameraMode");
					this.UpdateInputRestrictions();
				}
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00011E28 File Offset: 0x00010028
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x00011E30 File Offset: 0x00010030
		public CircleActionSelectorWidget CircleActionSelectorWidget
		{
			get
			{
				return this._circleActionSelectorWidget;
			}
			set
			{
				if (value != this._circleActionSelectorWidget)
				{
					this._circleActionSelectorWidget = value;
					base.OnPropertyChanged<CircleActionSelectorWidget>(value, "CircleActionSelectorWidget");
					this.UpdateInputRestrictions();
				}
			}
		}

		// Token: 0x04000299 RID: 665
		private bool _isInFreeCameraMode;

		// Token: 0x0400029A RID: 666
		private CircleActionSelectorWidget _circleActionSelectorWidget;
	}
}
