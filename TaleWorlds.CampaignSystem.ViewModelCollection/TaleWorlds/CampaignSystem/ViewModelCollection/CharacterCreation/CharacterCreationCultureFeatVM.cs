using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x0200014C RID: 332
	public class CharacterCreationCultureFeatVM : ViewModel
	{
		// Token: 0x06001FB3 RID: 8115 RVA: 0x000741EB File Offset: 0x000723EB
		public CharacterCreationCultureFeatVM(bool isPositive, string description)
		{
			this.IsPositive = isPositive;
			this.Description = description;
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x00074201 File Offset: 0x00072401
		// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x00074209 File Offset: 0x00072409
		[DataSourceProperty]
		public bool IsPositive
		{
			get
			{
				return this._isPositive;
			}
			set
			{
				if (value != this._isPositive)
				{
					this._isPositive = value;
					base.OnPropertyChangedWithValue(value, "IsPositive");
				}
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x00074227 File Offset: 0x00072427
		// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x0007422F File Offset: 0x0007242F
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x04000EC7 RID: 3783
		private bool _isPositive;

		// Token: 0x04000EC8 RID: 3784
		private string _description;
	}
}
