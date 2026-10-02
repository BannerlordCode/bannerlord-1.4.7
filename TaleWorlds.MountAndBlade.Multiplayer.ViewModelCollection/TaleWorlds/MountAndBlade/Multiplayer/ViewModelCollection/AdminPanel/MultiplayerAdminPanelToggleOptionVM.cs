using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B1 RID: 177
	public class MultiplayerAdminPanelToggleOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x060010A2 RID: 4258 RVA: 0x00033BCA File Offset: 0x00031DCA
		public MultiplayerAdminPanelToggleOptionVM(IAdminPanelOption<bool> option)
			: base(option)
		{
			this._option = option;
			this.ToggleValue = this._option.GetValue();
			this.IsToggleOption = true;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00033BF2 File Offset: 0x00031DF2
		public override void UpdateValues()
		{
			base.UpdateValues();
			this.ToggleValue = this._option.GetValue();
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00033C0B File Offset: 0x00031E0B
		public void ExecuteToggle()
		{
			this.ToggleValue = !this.ToggleValue;
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00033C1C File Offset: 0x00031E1C
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x00033C24 File Offset: 0x00031E24
		[DataSourceProperty]
		public bool IsToggleOption
		{
			get
			{
				return this._isToggleOption;
			}
			set
			{
				if (value != this._isToggleOption)
				{
					this._isToggleOption = value;
					base.OnPropertyChangedWithValue(value, "IsToggleOption");
				}
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x00033C42 File Offset: 0x00031E42
		// (set) Token: 0x060010A8 RID: 4264 RVA: 0x00033C4A File Offset: 0x00031E4A
		[DataSourceProperty]
		public bool ToggleValue
		{
			get
			{
				return this._toggleValue;
			}
			set
			{
				if (value != this._toggleValue)
				{
					this._toggleValue = value;
					base.OnPropertyChangedWithValue(value, "ToggleValue");
					this._option.SetValue(value);
				}
			}
		}

		// Token: 0x040007C0 RID: 1984
		private new readonly IAdminPanelOption<bool> _option;

		// Token: 0x040007C1 RID: 1985
		private bool _isToggleOption;

		// Token: 0x040007C2 RID: 1986
		private bool _toggleValue;
	}
}
