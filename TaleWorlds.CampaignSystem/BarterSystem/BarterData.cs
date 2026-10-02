using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x02000475 RID: 1141
	public class BarterData
	{
		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x001729BE File Offset: 0x00170BBE
		public IFaction OffererMapFaction
		{
			get
			{
				Hero offererHero = this.OffererHero;
				return ((offererHero != null) ? offererHero.MapFaction : null) ?? this.OffererParty.MapFaction;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x060048D0 RID: 18640 RVA: 0x001729E1 File Offset: 0x00170BE1
		public IFaction OtherMapFaction
		{
			get
			{
				Hero otherHero = this.OtherHero;
				return ((otherHero != null) ? otherHero.MapFaction : null) ?? this.OtherParty.MapFaction;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x00172A04 File Offset: 0x00170C04
		public bool IsAiBarter { get; }

		// Token: 0x060048D2 RID: 18642 RVA: 0x00172A0C File Offset: 0x00170C0C
		public BarterData(Hero offerer, Hero other, PartyBase offererParty, PartyBase otherParty, BarterManager.BarterContextInitializer contextInitializer = null, int persuasionCostReduction = 0, bool isAiBarter = false)
		{
			this.OffererParty = offererParty;
			this.OtherParty = otherParty;
			this.OffererHero = offerer;
			this.OtherHero = other;
			this.ContextInitializer = contextInitializer;
			this.PersuasionCostReduction = persuasionCostReduction;
			this._barterables = new List<Barterable>(16);
			this._barterGroups = Campaign.Current.Models.DiplomacyModel.GetBarterGroups().ToList<BarterGroup>();
			this.IsAiBarter = isAiBarter;
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x00172A80 File Offset: 0x00170C80
		public void AddBarterable<T>(Barterable barterable, bool isContextDependent = false)
		{
			foreach (BarterGroup barterGroup in this._barterGroups)
			{
				if (barterGroup is T)
				{
					barterable.Initialize(barterGroup, isContextDependent);
					this._barterables.Add(barterable);
					break;
				}
			}
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x00172AEC File Offset: 0x00170CEC
		public void AddBarterGroup(BarterGroup barterGroup)
		{
			this._barterGroups.Add(barterGroup);
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x00172AFA File Offset: 0x00170CFA
		public List<BarterGroup> GetBarterGroups()
		{
			return this._barterGroups;
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x00172B02 File Offset: 0x00170D02
		public List<Barterable> GetBarterables()
		{
			return this._barterables;
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x00172B0C File Offset: 0x00170D0C
		public BarterGroup GetBarterGroup<T>()
		{
			IEnumerable<T> enumerable = this._barterGroups.OfType<T>();
			if (enumerable.IsEmpty<T>())
			{
				return null;
			}
			return enumerable.First<T>() as BarterGroup;
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x00172B3F File Offset: 0x00170D3F
		public List<Barterable> GetOfferedBarterables()
		{
			return (from barterable in this.GetBarterables()
				where barterable.IsOffered
				select barterable).ToList<Barterable>();
		}

		// Token: 0x0400141E RID: 5150
		public readonly Hero OffererHero;

		// Token: 0x0400141F RID: 5151
		public readonly Hero OtherHero;

		// Token: 0x04001420 RID: 5152
		public readonly PartyBase OffererParty;

		// Token: 0x04001421 RID: 5153
		public readonly PartyBase OtherParty;

		// Token: 0x04001422 RID: 5154
		private List<Barterable> _barterables;

		// Token: 0x04001423 RID: 5155
		private List<BarterGroup> _barterGroups;

		// Token: 0x04001424 RID: 5156
		public readonly BarterManager.BarterContextInitializer ContextInitializer;

		// Token: 0x04001425 RID: 5157
		public readonly int PersuasionCostReduction;
	}
}
