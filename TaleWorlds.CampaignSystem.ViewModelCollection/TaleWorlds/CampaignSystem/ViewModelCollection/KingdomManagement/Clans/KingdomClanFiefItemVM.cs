using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x02000085 RID: 133
	public class KingdomClanFiefItemVM : ViewModel
	{
		// Token: 0x06000B32 RID: 2866 RVA: 0x0002F95C File Offset: 0x0002DB5C
		public KingdomClanFiefItemVM(Settlement settlement)
		{
			this.Settlement = settlement;
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.VisualPath = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.RefreshValues();
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002F9A3 File Offset: 0x0002DBA3
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.FiefName = this.Settlement.Name.ToString();
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002F9C1 File Offset: 0x0002DBC1
		private void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this.Settlement, true });
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002F9EA File Offset: 0x0002DBEA
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002F9F1 File Offset: 0x0002DBF1
		public void ExecuteLink()
		{
			if (this.Settlement != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Settlement.EncyclopediaLink);
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0002FA15 File Offset: 0x0002DC15
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x0002FA1D File Offset: 0x0002DC1D
		[DataSourceProperty]
		public string VisualPath
		{
			get
			{
				return this._visualPath;
			}
			set
			{
				if (value != this._visualPath)
				{
					this._visualPath = value;
					base.OnPropertyChanged("FileName");
				}
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x0002FA3F File Offset: 0x0002DC3F
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x0002FA47 File Offset: 0x0002DC47
		[DataSourceProperty]
		public string FiefName
		{
			get
			{
				return this._fiefName;
			}
			set
			{
				if (value != this._fiefName)
				{
					this._fiefName = value;
					base.OnPropertyChangedWithValue<string>(value, "FiefName");
				}
			}
		}

		// Token: 0x040004FC RID: 1276
		private readonly Settlement Settlement;

		// Token: 0x040004FD RID: 1277
		private string _visualPath;

		// Token: 0x040004FE RID: 1278
		private string _fiefName;
	}
}
