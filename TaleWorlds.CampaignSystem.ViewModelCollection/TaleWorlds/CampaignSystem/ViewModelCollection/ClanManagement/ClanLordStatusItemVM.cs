using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000127 RID: 295
	public class ClanLordStatusItemVM : ViewModel
	{
		// Token: 0x06001AF7 RID: 6903 RVA: 0x00064F58 File Offset: 0x00063158
		public ClanLordStatusItemVM(ClanLordStatusItemVM.LordStatus status, TextObject hintText)
		{
			this.Type = (int)status;
			this.Hint = new HintViewModel(hintText, null);
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x00064F7B File Offset: 0x0006317B
		// (set) Token: 0x06001AF9 RID: 6905 RVA: 0x00064F83 File Offset: 0x00063183
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x00064FA1 File Offset: 0x000631A1
		// (set) Token: 0x06001AFB RID: 6907 RVA: 0x00064FA9 File Offset: 0x000631A9
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

		// Token: 0x04000C92 RID: 3218
		private int _type = -1;

		// Token: 0x04000C93 RID: 3219
		private HintViewModel _hint;

		// Token: 0x02000285 RID: 645
		public enum LordStatus
		{
			// Token: 0x040012E5 RID: 4837
			Dead,
			// Token: 0x040012E6 RID: 4838
			Married,
			// Token: 0x040012E7 RID: 4839
			Pregnant,
			// Token: 0x040012E8 RID: 4840
			InBattle,
			// Token: 0x040012E9 RID: 4841
			InSiege,
			// Token: 0x040012EA RID: 4842
			Child,
			// Token: 0x040012EB RID: 4843
			Prisoner,
			// Token: 0x040012EC RID: 4844
			Sick
		}
	}
}
