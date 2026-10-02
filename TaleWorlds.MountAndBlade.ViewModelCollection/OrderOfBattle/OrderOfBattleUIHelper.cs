using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000035 RID: 53
	internal static class OrderOfBattleUIHelper
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x0000FAE4 File Offset: 0x0000DCE4
		internal static List<Agent> GetExcludedAgentsForTransfer(OrderOfBattleFormationItemVM formationVM, FormationClass formationClass)
		{
			List<Agent> list = new List<Agent>();
			if (formationVM.HasCaptain)
			{
				list.Add(formationVM.Captain.Agent);
			}
			if (formationVM.HeroTroops.Count > 0)
			{
				list.AddRange(formationVM.HeroTroops.Select<OrderOfBattleHeroItemVM, Agent>((OrderOfBattleHeroItemVM t) => t.Agent));
			}
			foreach (IFormationUnit formationUnit in formationVM.Formation.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				if (agent.Banner != null || !OrderOfBattleUIHelper.IsAgentInFormationClass(agent, formationClass))
				{
					list.Add(agent);
				}
			}
			return list.Distinct<Agent>().ToList<Agent>();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		[return: TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })]
		internal static ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> CreateMassTransferData(OrderOfBattleFormationClassVM affectedClass, FormationClass formationClass, TroopTraitsMask filter, int unitCount)
		{
			List<Agent> excludedAgentsForTransfer = OrderOfBattleUIHelper.GetExcludedAgentsForTransfer(affectedClass.BelongedFormationItem, formationClass);
			return new ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>(affectedClass.BelongedFormationItem.Formation, unitCount, filter, excludedAgentsForTransfer);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000FBF0 File Offset: 0x0000DDF0
		[return: TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })]
		internal static ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> CreateMassTransferData(OrderOfBattleFormationItemVM affectedFormation, FormationClass formationClass, TroopTraitsMask filter, int unitCount)
		{
			List<Agent> excludedAgentsForTransfer = OrderOfBattleUIHelper.GetExcludedAgentsForTransfer(affectedFormation, formationClass);
			return new ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>(affectedFormation.Formation, unitCount, filter, excludedAgentsForTransfer);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000FC14 File Offset: 0x0000DE14
		internal static ValueTuple<int, bool, bool> GetRelevantTroopTransferParameters(OrderOfBattleFormationClassVM classVM)
		{
			if (classVM == null)
			{
				return new ValueTuple<int, bool, bool>(0, false, false);
			}
			DeploymentFormationClass orderOfBattleFormationClass = classVM.Class.GetOrderOfBattleFormationClass();
			bool flag = orderOfBattleFormationClass == DeploymentFormationClass.Ranged || orderOfBattleFormationClass == DeploymentFormationClass.HorseArcher;
			bool flag2 = orderOfBattleFormationClass == DeploymentFormationClass.Cavalry || orderOfBattleFormationClass == DeploymentFormationClass.HorseArcher;
			return new ValueTuple<int, bool, bool>(OrderOfBattleUIHelper.GetTotalCountOfUnitsInClass(classVM.BelongedFormationItem.Formation, classVM.Class), flag, flag2);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000FC70 File Offset: 0x0000DE70
		internal static OrderOfBattleFormationClassVM GetFormationClassWithExtremumWeight(List<OrderOfBattleFormationClassVM> classes, bool isMinimum)
		{
			if (classes.Count == 0)
			{
				return null;
			}
			if (classes.Count == 1)
			{
				return classes[0];
			}
			OrderOfBattleFormationClassVM orderOfBattleFormationClassVM = classes[0];
			for (int i = 1; i < classes.Count; i++)
			{
				if ((isMinimum && classes[i].Weight < orderOfBattleFormationClassVM.Weight) || (!isMinimum && classes[i].Weight > orderOfBattleFormationClassVM.Weight))
				{
					orderOfBattleFormationClassVM = classes[i];
				}
			}
			return orderOfBattleFormationClassVM;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000FCE8 File Offset: 0x0000DEE8
		internal static List<OrderOfBattleFormationClassVM> GetMatchingClasses(List<OrderOfBattleFormationItemVM> formationList, OrderOfBattleFormationClassVM formationClass, Func<OrderOfBattleFormationClassVM, bool> predicate = null)
		{
			List<OrderOfBattleFormationClassVM> list = new List<OrderOfBattleFormationClassVM>();
			for (int i = 0; i < formationList.Count; i++)
			{
				OrderOfBattleFormationItemVM orderOfBattleFormationItemVM = formationList[i];
				for (int j = 0; j < orderOfBattleFormationItemVM.Classes.Count; j++)
				{
					OrderOfBattleFormationClassVM orderOfBattleFormationClassVM = orderOfBattleFormationItemVM.Classes[j];
					if (orderOfBattleFormationClassVM.Class == formationClass.Class && (predicate == null || predicate(orderOfBattleFormationClassVM)))
					{
						list.Add(orderOfBattleFormationClassVM);
					}
				}
			}
			return list;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000FD5D File Offset: 0x0000DF5D
		internal static bool IsAgentInFormationClass(Agent agent, FormationClass fc)
		{
			switch (fc)
			{
			case FormationClass.Infantry:
				return QueryLibrary.IsInfantry(agent);
			case FormationClass.Ranged:
				return QueryLibrary.IsRanged(agent);
			case FormationClass.Cavalry:
				return QueryLibrary.IsCavalry(agent);
			case FormationClass.HorseArcher:
				return QueryLibrary.IsRangedCavalry(agent);
			default:
				return false;
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000FD94 File Offset: 0x0000DF94
		private static List<Agent> GetBannerBearersOfFormation(Formation formation)
		{
			Mission mission = Mission.Current;
			List<Agent> list;
			if (mission == null)
			{
				list = null;
			}
			else
			{
				BannerBearerLogic missionBehavior = mission.GetMissionBehavior<BannerBearerLogic>();
				list = ((missionBehavior != null) ? missionBehavior.GetFormationBannerBearers(formation) : null);
			}
			List<Agent> list2 = list;
			if (list2 != null)
			{
				return list2;
			}
			return new List<Agent>();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000FDCC File Offset: 0x0000DFCC
		private static int GetCountOfUnitsInClass(OrderOfBattleFormationClassVM classVM, bool includeHeroes, bool includeBannerBearers)
		{
			Formation formation = classVM.BelongedFormationItem.Formation;
			FormationClass fc = classVM.Class;
			return formation.GetCountOfUnitsWithCondition((Agent agent) => (includeHeroes || !agent.IsHero) && (includeBannerBearers || agent.Banner == null) && OrderOfBattleUIHelper.IsAgentInFormationClass(agent, fc));
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000FE18 File Offset: 0x0000E018
		internal static int GetTotalCountOfUnitsInClass(Formation formation, FormationClass fc)
		{
			switch (fc)
			{
			case FormationClass.Infantry:
				return MathF.Round(formation.QuerySystem.InfantryUnitRatio * (float)formation.CountOfUnits);
			case FormationClass.Ranged:
				return MathF.Round(formation.QuerySystem.RangedUnitRatio * (float)formation.CountOfUnits);
			case FormationClass.Cavalry:
				return MathF.Round(formation.QuerySystem.CavalryUnitRatio * (float)formation.CountOfUnits);
			case FormationClass.HorseArcher:
				return MathF.Round(formation.QuerySystem.RangedCavalryUnitRatio * (float)formation.CountOfUnits);
			default:
				return 0;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000FEA2 File Offset: 0x0000E0A2
		internal static int GetCountOfRealUnitsInClass(OrderOfBattleFormationClassVM classVM)
		{
			return OrderOfBattleUIHelper.GetTotalCountOfUnitsInClass(classVM.BelongedFormationItem.Formation, classVM.Class);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000FEBC File Offset: 0x0000E0BC
		internal static int GetVisibleCountOfUnitsInClass(OrderOfBattleFormationClassVM classVM)
		{
			OrderOfBattleFormationItemVM belongedFormationItem = classVM.BelongedFormationItem;
			Formation formation = classVM.BelongedFormationItem.Formation;
			if (belongedFormationItem.Classes.Where<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => !c.IsUnset).ToList<OrderOfBattleFormationClassVM>().Count == 1)
			{
				return classVM.BelongedFormationItem.Formation.CountOfUnits;
			}
			return OrderOfBattleUIHelper.GetCountOfUnitsInClass(classVM, true, true);
		}
	}
}
