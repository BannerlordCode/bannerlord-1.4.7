using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D4 RID: 212
	public class AgentAmmoTextWidget : TextWidget
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x0001E7EC File Offset: 0x0001C9EC
		public AgentAmmoTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0001E7F5 File Offset: 0x0001C9F5
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsAlertEnabled)
			{
				this.SetState("Alert");
				return;
			}
			this.SetState("Default");
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0001E81D File Offset: 0x0001CA1D
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x0001E825 File Offset: 0x0001CA25
		public bool IsAlertEnabled
		{
			get
			{
				return this._isAlertEnabled;
			}
			set
			{
				if (this._isAlertEnabled != value)
				{
					this._isAlertEnabled = value;
					base.OnPropertyChanged(value, "IsAlertEnabled");
				}
			}
		}

		// Token: 0x040004ED RID: 1261
		private bool _isAlertEnabled;
	}
}
