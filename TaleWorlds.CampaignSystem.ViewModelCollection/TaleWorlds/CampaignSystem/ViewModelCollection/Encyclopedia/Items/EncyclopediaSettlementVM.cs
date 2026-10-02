using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000E8 RID: 232
	public class EncyclopediaSettlementVM : ViewModel
	{
		// Token: 0x060015A7 RID: 5543 RVA: 0x000554D4 File Offset: 0x000536D4
		public EncyclopediaSettlementVM(Settlement settlement)
		{
			if (!settlement.IsHideout)
			{
				this._settlement = settlement;
			}
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.FileName = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.RefreshValues();
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x00055523 File Offset: 0x00053723
		public override void RefreshValues()
		{
			base.RefreshValues();
			Settlement settlement = this._settlement;
			this.NameText = ((settlement != null) ? settlement.Name.ToString() : null) ?? "";
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x00055551 File Offset: 0x00053751
		public void ExecuteLink()
		{
			if (this._settlement != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._settlement.EncyclopediaLink);
			}
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x00055575 File Offset: 0x00053775
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0005557C File Offset: 0x0005377C
		public void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this._settlement });
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x0005559C File Offset: 0x0005379C
		// (set) Token: 0x060015AD RID: 5549 RVA: 0x000555A4 File Offset: 0x000537A4
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

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x000555C7 File Offset: 0x000537C7
		// (set) Token: 0x060015AF RID: 5551 RVA: 0x000555CF File Offset: 0x000537CF
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

		// Token: 0x040009D9 RID: 2521
		private Settlement _settlement;

		// Token: 0x040009DA RID: 2522
		private string _fileName;

		// Token: 0x040009DB RID: 2523
		private string _nameText;
	}
}
