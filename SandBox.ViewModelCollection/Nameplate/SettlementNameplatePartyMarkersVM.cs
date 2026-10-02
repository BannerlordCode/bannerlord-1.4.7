using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001F RID: 31
	public class SettlementNameplatePartyMarkersVM : ViewModel
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000C546 File Offset: 0x0000A746
		public SettlementNameplatePartyMarkersVM(Settlement settlement)
		{
			this._settlement = settlement;
			this.PartiesInSettlement = new MBBindingList<SettlementNameplatePartyMarkerItemVM>();
			this._itemComparer = new SettlementNameplatePartyMarkersVM.PartyMarkerItemComparer();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000C56C File Offset: 0x0000A76C
		private void PopulatePartyList()
		{
			this.PartiesInSettlement.Clear();
			foreach (MobileParty mobileParty in this._settlement.Parties.Where<MobileParty>((MobileParty p) => this.IsMobilePartyValid(p)))
			{
				this.PartiesInSettlement.Add(new SettlementNameplatePartyMarkerItemVM(mobileParty));
			}
			this.PartiesInSettlement.Sort(this._itemComparer);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000C5F8 File Offset: 0x0000A7F8
		private bool IsMobilePartyValid(MobileParty party)
		{
			if (party.IsGarrison || party.IsMilitia)
			{
				return false;
			}
			if (party.IsMainParty && (!party.IsMainParty || Campaign.Current.IsMainHeroDisguised))
			{
				return false;
			}
			if (party.Army != null)
			{
				Army army = party.Army;
				return army != null && army.LeaderParty.IsMainParty && !Campaign.Current.IsMainHeroDisguised;
			}
			return true;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000C668 File Offset: 0x0000A868
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement == this._settlement)
			{
				SettlementNameplatePartyMarkerItemVM settlementNameplatePartyMarkerItemVM = this.PartiesInSettlement.SingleOrDefault<SettlementNameplatePartyMarkerItemVM>((SettlementNameplatePartyMarkerItemVM p) => p.Party == party);
				if (settlementNameplatePartyMarkerItemVM != null)
				{
					this.PartiesInSettlement.Remove(settlementNameplatePartyMarkerItemVM);
				}
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		private void OnSettlementEntered(MobileParty partyEnteredSettlement, Settlement settlement, Hero leader)
		{
			if (settlement == this._settlement && partyEnteredSettlement != null && this.PartiesInSettlement.SingleOrDefault<SettlementNameplatePartyMarkerItemVM>((SettlementNameplatePartyMarkerItemVM p) => p.Party == partyEnteredSettlement) == null && this.IsMobilePartyValid(partyEnteredSettlement))
			{
				this.PartiesInSettlement.Add(new SettlementNameplatePartyMarkerItemVM(partyEnteredSettlement));
				this.PartiesInSettlement.Sort(this._itemComparer);
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000C72D File Offset: 0x0000A92D
		private void OnMapEventEnded(MapEvent obj)
		{
			if (obj.MapEventSettlement != null && obj.MapEventSettlement == this._settlement)
			{
				this.PopulatePartyList();
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000C74C File Offset: 0x0000A94C
		public void RegisterEvents()
		{
			if (!this._eventsRegistered)
			{
				this.PopulatePartyList();
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				this._eventsRegistered = true;
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000C7B3 File Offset: 0x0000A9B3
		public void UnloadEvents()
		{
			if (this._eventsRegistered)
			{
				CampaignEvents.SettlementEntered.ClearListeners(this);
				CampaignEvents.OnSettlementLeftEvent.ClearListeners(this);
				CampaignEvents.MapEventEnded.ClearListeners(this);
				this.PartiesInSettlement.Clear();
				this._eventsRegistered = false;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000C7F0 File Offset: 0x0000A9F0
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000C7F8 File Offset: 0x0000A9F8
		public MBBindingList<SettlementNameplatePartyMarkerItemVM> PartiesInSettlement
		{
			get
			{
				return this._partiesInSettlement;
			}
			set
			{
				if (value != this._partiesInSettlement)
				{
					this._partiesInSettlement = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementNameplatePartyMarkerItemVM>>(value, "PartiesInSettlement");
				}
			}
		}

		// Token: 0x04000167 RID: 359
		private Settlement _settlement;

		// Token: 0x04000168 RID: 360
		private bool _eventsRegistered;

		// Token: 0x04000169 RID: 361
		private SettlementNameplatePartyMarkersVM.PartyMarkerItemComparer _itemComparer;

		// Token: 0x0400016A RID: 362
		private MBBindingList<SettlementNameplatePartyMarkerItemVM> _partiesInSettlement;

		// Token: 0x02000087 RID: 135
		public class PartyMarkerItemComparer : IComparer<SettlementNameplatePartyMarkerItemVM>
		{
			// Token: 0x06000676 RID: 1654 RVA: 0x00016B90 File Offset: 0x00014D90
			public int Compare(SettlementNameplatePartyMarkerItemVM x, SettlementNameplatePartyMarkerItemVM y)
			{
				return x.SortIndex.CompareTo(y.SortIndex);
			}
		}
	}
}
