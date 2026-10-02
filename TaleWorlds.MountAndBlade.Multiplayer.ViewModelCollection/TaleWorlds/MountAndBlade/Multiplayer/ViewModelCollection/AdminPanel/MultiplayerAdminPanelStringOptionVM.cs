using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B0 RID: 176
	public class MultiplayerAdminPanelStringOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x0600109C RID: 4252 RVA: 0x00033B27 File Offset: 0x00031D27
		public MultiplayerAdminPanelStringOptionVM(IAdminPanelOption<string> option)
			: base(option)
		{
			this._option = option;
			this.Text = this._option.GetValue();
			this.IsStringOption = true;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00033B4F File Offset: 0x00031D4F
		public override void UpdateValues()
		{
			base.UpdateValues();
			this.Text = this._option.GetValue();
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x00033B68 File Offset: 0x00031D68
		// (set) Token: 0x0600109F RID: 4255 RVA: 0x00033B70 File Offset: 0x00031D70
		[DataSourceProperty]
		public bool IsStringOption
		{
			get
			{
				return this._isStringOption;
			}
			set
			{
				if (value != this._isStringOption)
				{
					this._isStringOption = value;
					base.OnPropertyChangedWithValue(value, "IsStringOption");
				}
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00033B8E File Offset: 0x00031D8E
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x00033B96 File Offset: 0x00031D96
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
					IAdminPanelOption<string> option = this._option;
					if (option == null)
					{
						return;
					}
					option.SetValue(value);
				}
			}
		}

		// Token: 0x040007BD RID: 1981
		private new readonly IAdminPanelOption<string> _option;

		// Token: 0x040007BE RID: 1982
		private bool _isStringOption;

		// Token: 0x040007BF RID: 1983
		private string _text;
	}
}
