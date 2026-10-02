using System;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038E RID: 910
	public static class TroopFilteringUtilities
	{
		// Token: 0x0600346A RID: 13418 RVA: 0x000D7F70 File Offset: 0x000D6170
		public static TroopTraitsMask GetFilter(bool isMounted, bool isRanged, bool isMelee, bool hasHeavyArmor, bool hasThrown, bool hasSpear, bool hasShield)
		{
			TroopTraitsMask troopTraitsMask = TroopTraitsMask.None;
			if (hasHeavyArmor)
			{
				troopTraitsMask |= TroopTraitsMask.Armor;
			}
			if (hasThrown)
			{
				troopTraitsMask |= TroopTraitsMask.Thrown;
			}
			if (hasSpear)
			{
				troopTraitsMask |= TroopTraitsMask.Spear;
			}
			if (hasShield)
			{
				troopTraitsMask |= TroopTraitsMask.Shield;
			}
			if (isMelee)
			{
				troopTraitsMask |= TroopTraitsMask.Melee;
			}
			if (isRanged)
			{
				troopTraitsMask |= TroopTraitsMask.Ranged;
			}
			if (isMounted)
			{
				troopTraitsMask |= TroopTraitsMask.Mount;
			}
			return troopTraitsMask;
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000D7FB8 File Offset: 0x000D61B8
		public static TroopTraitsMask GetFilter(params FormationClass[] formationClasses)
		{
			TroopTraitsMask troopTraitsMask = TroopTraitsMask.None;
			if (formationClasses.Length == 1)
			{
				switch (formationClasses[0])
				{
				case FormationClass.Infantry:
					troopTraitsMask = TroopTraitsMask.Melee;
					break;
				case FormationClass.Ranged:
					troopTraitsMask = TroopTraitsMask.Ranged;
					break;
				case FormationClass.Cavalry:
					troopTraitsMask = TroopTraitsMask.Melee | TroopTraitsMask.Mount;
					break;
				case FormationClass.HorseArcher:
					troopTraitsMask = TroopTraitsMask.Ranged | TroopTraitsMask.Mount;
					break;
				}
			}
			else if (formationClasses.Length == 2)
			{
				if (formationClasses[0] == FormationClass.Infantry && formationClasses[1] == FormationClass.Ranged)
				{
					troopTraitsMask = TroopTraitsMask.Melee | TroopTraitsMask.Ranged;
				}
				if (formationClasses[0] == FormationClass.Cavalry && formationClasses[1] == FormationClass.HorseArcher)
				{
					troopTraitsMask = TroopTraitsMask.Melee | TroopTraitsMask.Ranged | TroopTraitsMask.Mount;
				}
			}
			return troopTraitsMask;
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000D801C File Offset: 0x000D621C
		public static TroopTraitsMask GetFilter(params FormationFilterType[] filterTypes)
		{
			TroopTraitsMask troopTraitsMask = TroopTraitsMask.None;
			if (filterTypes.Length != 0)
			{
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Heavy))
				{
					troopTraitsMask |= TroopTraitsMask.Armor;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Shield))
				{
					troopTraitsMask |= TroopTraitsMask.Shield;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Thrown))
				{
					troopTraitsMask |= TroopTraitsMask.Thrown;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Spear))
				{
					troopTraitsMask |= TroopTraitsMask.Spear;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.HighTier))
				{
					troopTraitsMask |= TroopTraitsMask.HighTier;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.LowTier))
				{
					troopTraitsMask |= TroopTraitsMask.LowTier;
				}
			}
			return troopTraitsMask;
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x000D8140 File Offset: 0x000D6340
		public static void GetPriorityFunction(TroopTraitsMask filter, out Func<Agent, int> priorityFunc)
		{
			priorityFunc = delegate(Agent agent)
			{
				if (agent == null || agent.Character == null)
				{
					return TroopFilteringUtilities.GetMaxPriority(filter);
				}
				return TroopFilteringUtilities.GetTroopPriority(agent.GetTraitsMask(), agent.Character.GetBattleTier(), filter);
			};
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x000D8168 File Offset: 0x000D6368
		public static void GetPriorityFunction(TroopTraitsMask filter, out Func<IAgentOriginBase, int> priorityFunc)
		{
			priorityFunc = delegate(IAgentOriginBase agentOrigin)
			{
				if (agentOrigin == null || agentOrigin.Troop == null)
				{
					return TroopFilteringUtilities.GetMaxPriority(filter);
				}
				return TroopFilteringUtilities.GetTroopPriority(agentOrigin.GetTraitsMask(), agentOrigin.Troop.GetBattleTier(), filter);
			};
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000D8190 File Offset: 0x000D6390
		public static int GetTroopPriority(TroopTraitsMask troopMask, int battleTier, TroopTraitsMask filter)
		{
			int num = 1;
			if ((filter & TroopTraitsMask.HighTier) != TroopTraitsMask.None)
			{
				num += battleTier;
			}
			if ((filter & TroopTraitsMask.LowTier) != TroopTraitsMask.None)
			{
				num += 7 - battleTier;
			}
			TroopTraitsMask troopTraitsMask = filter & troopMask;
			if ((troopTraitsMask & TroopTraitsMask.Shield) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Spear) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Thrown) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Armor) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Melee) != TroopTraitsMask.None || (troopTraitsMask & TroopTraitsMask.Ranged) != TroopTraitsMask.None)
			{
				num += 100;
			}
			if ((troopTraitsMask & TroopTraitsMask.Mount) != TroopTraitsMask.None)
			{
				num += 1000;
			}
			return num;
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000D8208 File Offset: 0x000D6408
		public static int GetMaxPriority(TroopTraitsMask filter)
		{
			return 1 + (((filter & TroopTraitsMask.HighTier) != TroopTraitsMask.None) ? 7 : 0) + (((filter & TroopTraitsMask.LowTier) != TroopTraitsMask.None) ? 7 : 0) + (((filter & TroopTraitsMask.Shield) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Spear) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Thrown) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Armor) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Melee) != TroopTraitsMask.None || (filter & TroopTraitsMask.Ranged) != TroopTraitsMask.None) ? 100 : 0) + (((filter & TroopTraitsMask.Mount) != TroopTraitsMask.None) ? 1000 : 0);
		}

		// Token: 0x04001615 RID: 5653
		public const int MinPriority = 1;

		// Token: 0x04001616 RID: 5654
		public const int EquipmentPriority = 10;

		// Token: 0x04001617 RID: 5655
		public const int EngagementTypePriority = 100;

		// Token: 0x04001618 RID: 5656
		public const int MountedPriority = 1000;
	}
}
