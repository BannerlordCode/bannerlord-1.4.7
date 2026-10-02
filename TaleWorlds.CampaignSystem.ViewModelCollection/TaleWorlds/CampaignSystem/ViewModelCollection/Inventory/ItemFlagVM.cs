using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x0200008F RID: 143
	public class ItemFlagVM : ViewModel
	{
		// Token: 0x06000C6E RID: 3182 RVA: 0x00032CCF File Offset: 0x00030ECF
		public ItemFlagVM(string iconName, TextObject hint)
		{
			this.Icon = this.GetIconPath(iconName);
			this.Hint = new HintViewModel(hint, null);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00032CF4 File Offset: 0x00030EF4
		private string GetIconPath(string iconName)
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "GetIconPath");
			mbstringBuilder.Append<string>("<img src=\"SPGeneral\\");
			mbstringBuilder.Append<string>(iconName);
			mbstringBuilder.Append<string>("\"/>");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00032D41 File Offset: 0x00030F41
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x00032D49 File Offset: 0x00030F49
		[DataSourceProperty]
		public string Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				if (value != this._icon)
				{
					this._icon = value;
					base.OnPropertyChangedWithValue<string>(value, "Icon");
				}
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00032D6C File Offset: 0x00030F6C
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x00032D74 File Offset: 0x00030F74
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x04000592 RID: 1426
		private string _icon;

		// Token: 0x04000593 RID: 1427
		private HintViewModel _hint;
	}
}
