using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000AD RID: 173
	public class TroopUpgradeTracker
	{
		// Token: 0x06001379 RID: 4985 RVA: 0x0005AB4D File Offset: 0x00058D4D
		public void AddParty(MapEventParty mapEventParty)
		{
			this._mapEventParties.Add(mapEventParty);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0005AB5B File Offset: 0x00058D5B
		public void RemoveParty(MapEventParty mapEventParty)
		{
			this._mapEventParties.Add(mapEventParty);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0005AB6C File Offset: 0x00058D6C
		public void AddTrackedTroop(PartyBase party, CharacterObject character)
		{
			if (character.IsHero)
			{
				int count = Skills.All.Count;
				int[] array = new int[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = character.GetSkillValue(Skills.All[i]);
				}
				this._heroSkills[character.HeroObject] = array;
				return;
			}
			int num = party.MemberRoster.FindIndexOfTroop(character);
			if (num >= 0)
			{
				TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(num);
				int num2 = this.CalculateReadyToUpgradeSafe(ref elementCopyAtIndex, party);
				this._upgradedRegulars[new Tuple<PartyBase, CharacterObject>(party, character)] = num2;
			}
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0005AC04 File Offset: 0x00058E04
		public IEnumerable<SkillObject> CheckSkillUpgrades(Hero hero)
		{
			if (!this._heroSkills.IsEmpty<KeyValuePair<Hero, int[]>>() && this._heroSkills.ContainsKey(hero))
			{
				int[] oldSkillLevels = this._heroSkills[hero];
				int num;
				for (int i = 0; i < Skills.All.Count; i = num)
				{
					SkillObject skill = Skills.All[i];
					int newSkillLevel = hero.CharacterObject.GetSkillValue(skill);
					while (newSkillLevel > oldSkillLevels[i])
					{
						oldSkillLevels[i]++;
						yield return skill;
					}
					skill = null;
					num = i + 1;
				}
				oldSkillLevels = null;
			}
			yield break;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0005AC1C File Offset: 0x00058E1C
		public int CheckUpgradedCount(PartyBase party, CharacterObject character)
		{
			int num = 0;
			if (!character.IsHero && party != null)
			{
				int num2 = party.MemberRoster.FindIndexOfTroop(character);
				int num5;
				if (num2 >= 0)
				{
					TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(num2);
					int num3 = this.CalculateReadyToUpgradeSafe(ref elementCopyAtIndex, party);
					int num4;
					if (this._upgradedRegulars.TryGetValue(new Tuple<PartyBase, CharacterObject>(party, character), out num4) && num3 > num4)
					{
						num4 = MathF.Min(elementCopyAtIndex.Number, num4);
						num = num3 - num4;
						this._upgradedRegulars[new Tuple<PartyBase, CharacterObject>(party, character)] = num3;
					}
				}
				else if (this._upgradedRegulars.TryGetValue(new Tuple<PartyBase, CharacterObject>(party, character), out num5) && num5 > 0)
				{
					num = -num5;
				}
			}
			return num;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0005ACCC File Offset: 0x00058ECC
		private int CalculateReadyToUpgradeSafe(ref TroopRosterElement el, PartyBase owner)
		{
			int num = 0;
			CharacterObject character = el.Character;
			int num2;
			if (!character.IsHero && character.UpgradeTargets.Length != 0 && MobilePartyHelper.CanTroopGainXp(owner, el.Character, out num2))
			{
				int num3 = 0;
				for (int i = 0; i < character.UpgradeTargets.Length; i++)
				{
					int upgradeXpCost = character.GetUpgradeXpCost(owner, i);
					if (num3 < upgradeXpCost)
					{
						num3 = upgradeXpCost;
					}
				}
				if (num3 > 0)
				{
					MapEventParty mapEventParty = this._mapEventParties.Find((MapEventParty p) => p.Party == owner);
					int num4 = el.Xp;
					foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in mapEventParty.Troops)
					{
						if (flattenedTroopRosterElement.Troop == el.Character && !flattenedTroopRosterElement.IsKilled)
						{
							num4 += flattenedTroopRosterElement.XpGained;
						}
					}
					num = num4 / num3;
				}
			}
			return MathF.Max(MathF.Min(el.Number, num), 0);
		}

		// Token: 0x04000661 RID: 1633
		private Dictionary<Tuple<PartyBase, CharacterObject>, int> _upgradedRegulars = new Dictionary<Tuple<PartyBase, CharacterObject>, int>();

		// Token: 0x04000662 RID: 1634
		private List<MapEventParty> _mapEventParties = new List<MapEventParty>();

		// Token: 0x04000663 RID: 1635
		private Dictionary<Hero, int[]> _heroSkills = new Dictionary<Hero, int[]>();
	}
}
