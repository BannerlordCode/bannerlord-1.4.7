using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x0200008D RID: 141
	public class KingdomSettlementVillageItemVM : ViewModel
	{
		// Token: 0x06000C2D RID: 3117 RVA: 0x000323C0 File Offset: 0x000305C0
		public KingdomSettlementVillageItemVM(Village village)
		{
			this._village = village;
			this.VisualPath = village.BackgroundMeshName + "_t";
			this.RefreshValues();
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x000323EB File Offset: 0x000305EB
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._village.Name.ToString();
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00032409 File Offset: 0x00030609
		private void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[]
			{
				this._village.Settlement,
				true
			});
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00032437 File Offset: 0x00030637
		private void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0003243E File Offset: 0x0003063E
		public void ExecuteLink()
		{
			if (this._village != null && this._village.Settlement != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._village.Settlement.EncyclopediaLink);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00032474 File Offset: 0x00030674
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x0003247C File Offset: 0x0003067C
		[DataSourceProperty]
		public ImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0003249A File Offset: 0x0003069A
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x000324A2 File Offset: 0x000306A2
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x000324C5 File Offset: 0x000306C5
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x000324CD File Offset: 0x000306CD
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
					base.OnPropertyChangedWithValue<string>(value, "VisualPath");
				}
			}
		}

		// Token: 0x04000573 RID: 1395
		private Village _village;

		// Token: 0x04000574 RID: 1396
		private ImageIdentifierVM _visual;

		// Token: 0x04000575 RID: 1397
		private string _name;

		// Token: 0x04000576 RID: 1398
		private string _visualPath;
	}
}
