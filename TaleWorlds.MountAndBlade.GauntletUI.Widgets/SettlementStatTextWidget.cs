using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003F RID: 63
	public class SettlementStatTextWidget : TextWidget
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x0000BA79 File Offset: 0x00009C79
		public SettlementStatTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000BA84 File Offset: 0x00009C84
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			switch (this._state)
			{
			case SettlementStatTextWidget.State.Idle:
				if (this.IsWarning)
				{
					this.SetState("Warning");
				}
				else
				{
					this.SetState("Default");
				}
				this._state = SettlementStatTextWidget.State.Start;
				return;
			case SettlementStatTextWidget.State.Start:
				this._state = ((base.BrushRenderer.Brush != null) ? SettlementStatTextWidget.State.Playing : SettlementStatTextWidget.State.Start);
				return;
			case SettlementStatTextWidget.State.Playing:
				base.BrushRenderer.RestartAnimation();
				this._state = SettlementStatTextWidget.State.End;
				break;
			case SettlementStatTextWidget.State.End:
				break;
			default:
				return;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000BB08 File Offset: 0x00009D08
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000BB10 File Offset: 0x00009D10
		[Editor(false)]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChanged(value, "IsWarning");
					this.SetState(this._isWarning ? "Warning" : "Default");
				}
			}
		}

		// Token: 0x0400018A RID: 394
		private SettlementStatTextWidget.State _state;

		// Token: 0x0400018B RID: 395
		private bool _isWarning;

		// Token: 0x020001A2 RID: 418
		public enum State
		{
			// Token: 0x040009B2 RID: 2482
			Idle,
			// Token: 0x040009B3 RID: 2483
			Start,
			// Token: 0x040009B4 RID: 2484
			Playing,
			// Token: 0x040009B5 RID: 2485
			End
		}
	}
}
