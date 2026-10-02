using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000023 RID: 35
	public class CaravanTransactionNotificationItemVM : SettlementNotificationItemBaseVM
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000E202 File Offset: 0x0000C402
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0000E20A File Offset: 0x0000C40A
		public MobileParty CaravanParty { get; private set; }

		// Token: 0x06000352 RID: 850 RVA: 0x0000E214 File Offset: 0x0000C414
		public CaravanTransactionNotificationItemVM(Action<SettlementNotificationItemBaseVM> onRemove, MobileParty caravanParty, List<ValueTuple<EquipmentElement, int>> items, int createdTick)
			: base(onRemove, createdTick)
		{
			this._items = items;
			List<ValueTuple<EquipmentElement, int>> list = this._items.DistinctBy<ValueTuple<EquipmentElement, int>, EquipmentElement>((ValueTuple<EquipmentElement, int> i) => i.Item1).ToList<ValueTuple<EquipmentElement, int>>();
			if (list.Count < this._items.Count)
			{
				this._items = list;
			}
			this.CaravanParty = caravanParty;
			this._isSelling = this._items.Count > 0 && this._items[0].Item2 > 0;
			base.Text = SandBoxUIHelper.GetItemsTradedNotificationText(this._items, this._isSelling);
			Hero leaderHero = caravanParty.LeaderHero;
			base.CharacterName = ((leaderHero != null) ? leaderHero.Name.ToString() : null) ?? caravanParty.Name.ToString();
			if (caravanParty.Party.Owner != null)
			{
				base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(this.CaravanParty.Party.Owner.CharacterObject, false));
			}
			else
			{
				CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(this.CaravanParty.Party);
				if (visualPartyLeader != null)
				{
					base.CharacterVisual = new CharacterImageIdentifierVM(SandBoxUIHelper.GetCharacterCode(visualPartyLeader, false));
				}
			}
			base.RelationType = 0;
			if (caravanParty != null)
			{
				IFaction mapFaction = caravanParty.MapFaction;
				base.RelationType = ((mapFaction != null && mapFaction.IsAtWarWith(Hero.MainHero.Clan)) ? (-1) : 1);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000E380 File Offset: 0x0000C580
		public void AddNewItems(List<ValueTuple<EquipmentElement, int>> newItems)
		{
			CaravanTransactionNotificationItemVM.<>c__DisplayClass7_0 CS$<>8__locals1 = new CaravanTransactionNotificationItemVM.<>c__DisplayClass7_0();
			CS$<>8__locals1.newItems = newItems;
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.newItems.Count; i = j + 1)
			{
				ValueTuple<EquipmentElement, int> valueTuple = this._items.FirstOrDefault<ValueTuple<EquipmentElement, int>>((ValueTuple<EquipmentElement, int> t) => t.Item1.Equals(CS$<>8__locals1.newItems[i].Item1));
				if (!valueTuple.Item1.IsEmpty)
				{
					int num = this._items.IndexOf(valueTuple);
					valueTuple.Item2 += CS$<>8__locals1.newItems[i].Item2;
					this._items[num] = valueTuple;
					if (this._items[num].Item2 == 0)
					{
						this._items.RemoveAt(num);
					}
				}
				else
				{
					this._items.Add(new ValueTuple<EquipmentElement, int>(CS$<>8__locals1.newItems[i].Item1, CS$<>8__locals1.newItems[i].Item2));
				}
				j = i;
			}
			this._isSelling = this._items.Count > 0 && this._items[0].Item2 > 0;
			base.Text = SandBoxUIHelper.GetItemsTradedNotificationText(this._items, this._isSelling);
		}

		// Token: 0x040001B0 RID: 432
		private List<ValueTuple<EquipmentElement, int>> _items;

		// Token: 0x040001B1 RID: 433
		private bool _isSelling;
	}
}
