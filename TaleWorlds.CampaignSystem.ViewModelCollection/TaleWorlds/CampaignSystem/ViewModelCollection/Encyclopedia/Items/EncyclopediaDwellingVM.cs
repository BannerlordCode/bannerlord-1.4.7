using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000E4 RID: 228
	public class EncyclopediaDwellingVM : ViewModel
	{
		// Token: 0x06001588 RID: 5512 RVA: 0x00055110 File Offset: 0x00053310
		public EncyclopediaDwellingVM(WorkshopType workshop)
		{
			this._workshop = workshop;
			this.FileName = workshop.StringId;
			this.RefreshValues();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00055131 File Offset: 0x00053331
		public EncyclopediaDwellingVM(VillageType villageType)
		{
			this._villageType = villageType;
			this.FileName = villageType.StringId;
			this.RefreshValues();
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00055154 File Offset: 0x00053354
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._workshop != null)
			{
				this.NameText = this._workshop.Name.ToString();
				return;
			}
			if (this._villageType != null)
			{
				this.NameText = this._villageType.ShortName.ToString();
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x000551A4 File Offset: 0x000533A4
		// (set) Token: 0x0600158C RID: 5516 RVA: 0x000551AC File Offset: 0x000533AC
		[DataSourceProperty]
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				if (value != this._fileName)
				{
					this._fileName = value;
					base.OnPropertyChangedWithValue<string>(value, "FileName");
				}
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x000551CF File Offset: 0x000533CF
		// (set) Token: 0x0600158E RID: 5518 RVA: 0x000551D7 File Offset: 0x000533D7
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x040009CC RID: 2508
		private readonly WorkshopType _workshop;

		// Token: 0x040009CD RID: 2509
		private readonly VillageType _villageType;

		// Token: 0x040009CE RID: 2510
		private string _fileName;

		// Token: 0x040009CF RID: 2511
		private string _nameText;
	}
}
