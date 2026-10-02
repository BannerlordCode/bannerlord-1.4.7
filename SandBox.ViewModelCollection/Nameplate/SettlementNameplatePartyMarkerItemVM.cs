using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001E RID: 30
	public class SettlementNameplatePartyMarkerItemVM : ViewModel
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000C3DD File Offset: 0x0000A5DD
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000C3E5 File Offset: 0x0000A5E5
		public MobileParty Party { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000C3EE File Offset: 0x0000A5EE
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000C3F6 File Offset: 0x0000A5F6
		public int SortIndex { get; private set; }

		// Token: 0x060002D4 RID: 724 RVA: 0x0000C400 File Offset: 0x0000A600
		public SettlementNameplatePartyMarkerItemVM(MobileParty mobileParty)
		{
			this.Party = mobileParty;
			this.IsBandit = mobileParty.IsBandit;
			if (mobileParty.IsCaravan)
			{
				this.IsCaravan = true;
				this.SortIndex = 1;
				return;
			}
			if (mobileParty.IsLordParty && mobileParty.LeaderHero != null)
			{
				this.IsLord = true;
				Clan actualClan = mobileParty.ActualClan;
				this.Visual = new BannerImageIdentifierVM((actualClan != null) ? actualClan.Banner : null, true);
				this.SortIndex = 0;
				return;
			}
			this.IsDefault = true;
			this.SortIndex = 2;
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000C488 File Offset: 0x0000A688
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000C490 File Offset: 0x0000A690
		public BannerImageIdentifierVM Visual
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
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000C4AE File Offset: 0x0000A6AE
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000C4B6 File Offset: 0x0000A6B6
		public bool IsCaravan
		{
			get
			{
				return this._isCaravan;
			}
			set
			{
				if (value != this._isCaravan)
				{
					this._isCaravan = value;
					base.OnPropertyChangedWithValue(value, "IsCaravan");
				}
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000C4D4 File Offset: 0x0000A6D4
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000C4DC File Offset: 0x0000A6DC
		public bool IsLord
		{
			get
			{
				return this._isLord;
			}
			set
			{
				if (value != this._isLord)
				{
					this._isLord = value;
					base.OnPropertyChangedWithValue(value, "IsLord");
				}
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000C4FA File Offset: 0x0000A6FA
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000C502 File Offset: 0x0000A702
		public bool IsDefault
		{
			get
			{
				return this._isDefault;
			}
			set
			{
				if (value != this._isDefault)
				{
					this._isDefault = value;
					base.OnPropertyChangedWithValue(value, "IsDefault");
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000C520 File Offset: 0x0000A720
		// (set) Token: 0x060002DE RID: 734 RVA: 0x0000C528 File Offset: 0x0000A728
		public bool IsBandit
		{
			get
			{
				return this._isBandit;
			}
			set
			{
				if (value != this._isBandit)
				{
					this._isBandit = value;
					base.OnPropertyChangedWithValue(value, "IsBandit");
				}
			}
		}

		// Token: 0x04000162 RID: 354
		private BannerImageIdentifierVM _visual;

		// Token: 0x04000163 RID: 355
		private bool _isCaravan;

		// Token: 0x04000164 RID: 356
		private bool _isLord;

		// Token: 0x04000165 RID: 357
		private bool _isDefault;

		// Token: 0x04000166 RID: 358
		private bool _isBandit;
	}
}
