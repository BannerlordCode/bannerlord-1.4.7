using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000158 RID: 344
	public class DefaultSiegeLordsHallFightModel : SiegeLordsHallFightModel
	{
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x0008753F File Offset: 0x0008573F
		public override float AreaLostRatio
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001A98 RID: 6808 RVA: 0x00087546 File Offset: 0x00085746
		public override float AttackerDefenderTroopCountRatio
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x0008754D File Offset: 0x0008574D
		public override float DefenderMaxArcherRatio
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x00087554 File Offset: 0x00085754
		public override int MaxDefenderSideTroopCount
		{
			get
			{
				return 27;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x00087558 File Offset: 0x00085758
		public override int MaxDefenderArcherCount
		{
			get
			{
				return MathF.Round((float)this.MaxDefenderSideTroopCount * this.DefenderMaxArcherRatio);
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x0008756D File Offset: 0x0008576D
		public override int MaxAttackerSideTroopCount
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001A9D RID: 6813 RVA: 0x00087571 File Offset: 0x00085771
		public override int DefenderTroopNumberForSuccessfulPullBack
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00087578 File Offset: 0x00085778
		public override FlattenedTroopRoster GetPriorityListForLordsHallFightMission(MapEvent playerMapEvent, BattleSideEnum side, int troopCount)
		{
			List<MapEventParty> list = (from x in playerMapEvent.PartiesOnSide(side)
				where x.Party.IsMobile
				select x).ToList<MapEventParty>();
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(list.Sum<MapEventParty>((MapEventParty x) => x.Party.MemberRoster.TotalHealthyCount));
			foreach (MapEventParty mapEventParty in list)
			{
				flattenedTroopRoster.Add(mapEventParty.Party.MemberRoster.GetTroopRoster());
			}
			List<FlattenedTroopRosterElement> list2 = flattenedTroopRoster.Where<FlattenedTroopRosterElement>((FlattenedTroopRosterElement x) => !x.Troop.IsHero && x.Troop.IsRanged && !x.IsWounded).ToList<FlattenedTroopRosterElement>();
			list2.Shuffle<FlattenedTroopRosterElement>();
			List<FlattenedTroopRosterElement> list3 = flattenedTroopRoster.Where<FlattenedTroopRosterElement>((FlattenedTroopRosterElement x) => !x.Troop.IsHero && !x.Troop.IsRanged && !x.IsWounded).ToList<FlattenedTroopRosterElement>();
			list3.Shuffle<FlattenedTroopRosterElement>();
			flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => !x.Troop.IsHero || x.IsWounded);
			int num = troopCount - flattenedTroopRoster.Count<FlattenedTroopRosterElement>();
			if (num > 0)
			{
				int count = list2.Count;
				int count2 = list3.Count;
				int num2 = MathF.Min(count, Campaign.Current.Models.SiegeLordsHallFightModel.MaxDefenderArcherCount);
				int num3 = 0;
				int num4 = 0;
				while (num > 0 && (num3 < num2 || num4 < count2))
				{
					if (num3 < num2)
					{
						FlattenedTroopRosterElement flattenedTroopRosterElement = list2[num3];
						flattenedTroopRoster.Add(flattenedTroopRosterElement.Troop, false, flattenedTroopRosterElement.Xp);
						num--;
					}
					if (num4 < count2 && num > 0)
					{
						FlattenedTroopRosterElement flattenedTroopRosterElement2 = list3[num4];
						flattenedTroopRoster.Add(flattenedTroopRosterElement2.Troop, false, flattenedTroopRosterElement2.Xp);
						num--;
					}
					num3++;
					num4++;
				}
			}
			return flattenedTroopRoster;
		}
	}
}
