using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A5 RID: 165
	public class SettlementGovernorSelectionVM : ViewModel
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x00041B38 File Offset: 0x0003FD38
		public SettlementGovernorSelectionVM(Settlement settlement, Action<Hero> onDone)
		{
			this._settlement = settlement;
			this._onDone = onDone;
			this.AvailableGovernors = new MBBindingList<SettlementGovernorSelectionItemVM>
			{
				new SettlementGovernorSelectionItemVM(null, new Action<SettlementGovernorSelectionItemVM>(this.OnSelection))
			};
			if (((settlement != null) ? settlement.OwnerClan : null) != null)
			{
				using (List<Hero>.Enumerator enumerator = settlement.OwnerClan.Heroes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Hero hero = enumerator.Current;
						if (Campaign.Current.Models.ClanPoliticsModel.CanHeroBeGovernor(hero) && !this.AvailableGovernors.Any<SettlementGovernorSelectionItemVM>((SettlementGovernorSelectionItemVM G) => G.Governor == hero) && (hero.GovernorOf == this._settlement.Town || hero.GovernorOf == null))
						{
							this.AvailableGovernors.Add(new SettlementGovernorSelectionItemVM(hero, new Action<SettlementGovernorSelectionItemVM>(this.OnSelection)));
						}
					}
				}
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00041C68 File Offset: 0x0003FE68
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AvailableGovernors.ApplyActionOnAllItems(delegate(SettlementGovernorSelectionItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00041C9A File Offset: 0x0003FE9A
		private void OnSelection(SettlementGovernorSelectionItemVM item)
		{
			Action<Hero> onDone = this._onDone;
			if (onDone == null)
			{
				return;
			}
			onDone(item.Governor);
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00041CB2 File Offset: 0x0003FEB2
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x00041CBA File Offset: 0x0003FEBA
		[DataSourceProperty]
		public MBBindingList<SettlementGovernorSelectionItemVM> AvailableGovernors
		{
			get
			{
				return this._availableGovernors;
			}
			set
			{
				if (value != this._availableGovernors)
				{
					this._availableGovernors = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementGovernorSelectionItemVM>>(value, "AvailableGovernors");
				}
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00041CD8 File Offset: 0x0003FED8
		// (set) Token: 0x06000FFD RID: 4093 RVA: 0x00041CE0 File Offset: 0x0003FEE0
		[DataSourceProperty]
		public int CurrentGovernorIndex
		{
			get
			{
				return this._currentGovernorIndex;
			}
			set
			{
				if (value != this._currentGovernorIndex)
				{
					this._currentGovernorIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentGovernorIndex");
				}
			}
		}

		// Token: 0x04000748 RID: 1864
		private readonly Settlement _settlement;

		// Token: 0x04000749 RID: 1865
		private readonly Action<Hero> _onDone;

		// Token: 0x0400074A RID: 1866
		private MBBindingList<SettlementGovernorSelectionItemVM> _availableGovernors;

		// Token: 0x0400074B RID: 1867
		private int _currentGovernorIndex;
	}
}
