using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace SandBox.GameComponents
{
	// Token: 0x020000C7 RID: 199
	public class SandboxBattleSpawnModel : BattleSpawnModel
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x0003A287 File Offset: 0x00038487
		public override void OnMissionStart()
		{
			MissionReinforcementsHelper.OnMissionStart();
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0003A28E File Offset: 0x0003848E
		public override void OnMissionEnd()
		{
			MissionReinforcementsHelper.OnMissionEnd();
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0003A298 File Offset: 0x00038498
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public override List<ValueTuple<IAgentOriginBase, int>> GetInitialSpawnAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins)
		{
			List<ValueTuple<IAgentOriginBase, int>> list = new List<ValueTuple<IAgentOriginBase, int>>();
			SandboxBattleSpawnModel.FormationOrderOfBattleConfiguration[] array;
			if (SandboxBattleSpawnModel.GetOrderOfBattleConfigurationsForFormations(battleSide, troopOrigins, out array))
			{
				using (List<IAgentOriginBase>.Enumerator enumerator = troopOrigins.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IAgentOriginBase agentOriginBase = enumerator.Current;
						SandboxBattleSpawnModel.OrderOfBattleInnerClassType orderOfBattleInnerClassType;
						FormationClass formationClass = SandboxBattleSpawnModel.FindBestOrderOfBattleFormationClassAssignmentForTroop(battleSide, agentOriginBase, array, out orderOfBattleInnerClassType);
						ValueTuple<IAgentOriginBase, int> valueTuple = new ValueTuple<IAgentOriginBase, int>(agentOriginBase, (int)formationClass);
						list.Add(valueTuple);
						if (orderOfBattleInnerClassType == SandboxBattleSpawnModel.OrderOfBattleInnerClassType.PrimaryClass)
						{
							SandboxBattleSpawnModel.FormationOrderOfBattleConfiguration[] array2 = array;
							FormationClass formationClass2 = formationClass;
							array2[(int)formationClass2].PrimaryClassTroopCount = array2[(int)formationClass2].PrimaryClassTroopCount + 1;
						}
						else if (orderOfBattleInnerClassType == SandboxBattleSpawnModel.OrderOfBattleInnerClassType.SecondaryClass)
						{
							SandboxBattleSpawnModel.FormationOrderOfBattleConfiguration[] array3 = array;
							FormationClass formationClass3 = formationClass;
							array3[(int)formationClass3].SecondaryClassTroopCount = array3[(int)formationClass3].SecondaryClassTroopCount + 1;
						}
					}
					return list;
				}
			}
			foreach (IAgentOriginBase agentOriginBase2 in troopOrigins)
			{
				ValueTuple<IAgentOriginBase, int> valueTuple2 = new ValueTuple<IAgentOriginBase, int>(agentOriginBase2, (int)Mission.Current.GetAgentTroopClass(battleSide, agentOriginBase2.Troop));
				list.Add(valueTuple2);
			}
			return list;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0003A39C File Offset: 0x0003859C
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public override List<ValueTuple<IAgentOriginBase, int>> GetReinforcementAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins)
		{
			return MissionReinforcementsHelper.GetReinforcementAssignments(battleSide, troopOrigins);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0003A3A8 File Offset: 0x000385A8
		private static bool GetOrderOfBattleConfigurationsForFormations(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins, out SandboxBattleSpawnModel.FormationOrderOfBattleConfiguration[] formationOrderOfBattleConfigurations)
		{
			formationOrderOfBattleConfigurations = new SandboxBattleSpawnModel.FormationOrderOfBattleConfiguration[8];
			Campaign campaign = Campaign.Current;
			OrderOfBattleCampaignBehavior orderOfBattleCampaignBehavior = ((campaign != null) ? campaign.GetCampaignBehavior<OrderOfBattleCampaignBehavior>() : null);
			if (orderOfBattleCampaignBehavior == null)
			{
				return false;
			}
			for (int i = 0; i < 8; i++)
			{
				OrderOfBattleCampaignBehavior orderOfBattleCampaignBehavior2 = orderOfBattleCampaignBehavior;
				int num = i;
				bool isSiegeBattle = Mission.Current.IsSiegeBattle;
				MobileParty mainParty = MobileParty.MainParty;
				if (orderOfBattleCampaignBehavior2.GetFormationDataAtIndex(num, isSiegeBattle, ((mainParty != null) ? mainParty.Army : null) != null) == null)
				{
					return false;
				}
			}
			int[] array = SandboxBattleSpawnModel.CalculateTroopCountsPerDefaultFormation(battleSide, troopOrigins);
			for (int j = 0; j < 8; j++)
			{
				OrderOfBattleCampaignBehavior orderOfBattleCampaignBehavior3 = orderOfBattleCampaignBehavior;
				int num2 = j;
				bool isSiegeBattle2 = Mission.Current.IsSiegeBattle;
				MobileParty mainParty2 = MobileParty.MainParty;
				OrderOfBattleCampaignBehavior.OrderOfBattleFormationData formationDataAtIndex = orderOfBattleCampaignBehavior3.GetFormationDataAtIndex(num2, isSiegeBattle2, ((mainParty2 != null) ? mainParty2.Army : null) != null);
				formationOrderOfBattleConfigurations[j].OOBFormationClass = formationDataAtIndex.FormationClass;
				formationOrderOfBattleConfigurations[j].Captain = formationDataAtIndex.Captain;
				FormationClass formationClass = FormationClass.NumberOfAllFormations;
				FormationClass formationClass2 = FormationClass.NumberOfAllFormations;
				switch (formationDataAtIndex.FormationClass)
				{
				case DeploymentFormationClass.Infantry:
					formationClass = FormationClass.Infantry;
					break;
				case DeploymentFormationClass.Ranged:
					formationClass = FormationClass.Ranged;
					break;
				case DeploymentFormationClass.Cavalry:
					formationClass = FormationClass.Cavalry;
					break;
				case DeploymentFormationClass.HorseArcher:
					formationClass = FormationClass.HorseArcher;
					break;
				case DeploymentFormationClass.InfantryAndRanged:
					formationClass = FormationClass.Infantry;
					formationClass2 = FormationClass.Ranged;
					break;
				case DeploymentFormationClass.CavalryAndHorseArcher:
					formationClass = FormationClass.Cavalry;
					formationClass2 = FormationClass.HorseArcher;
					break;
				}
				formationOrderOfBattleConfigurations[j].PrimaryFormationClass = formationClass;
				if (formationClass != FormationClass.NumberOfAllFormations)
				{
					formationOrderOfBattleConfigurations[j].PrimaryClassDesiredTroopCount = (int)Math.Ceiling((double)((float)array[(int)formationClass] * ((float)formationDataAtIndex.PrimaryClassWeight / 100f)));
				}
				formationOrderOfBattleConfigurations[j].SecondaryFormationClass = formationClass2;
				if (formationClass2 != FormationClass.NumberOfAllFormations)
				{
					formationOrderOfBattleConfigurations[j].SecondaryClassDesiredTroopCount = (int)Math.Ceiling((double)((float)array[(int)formationClass2] * ((float)formationDataAtIndex.SecondaryClassWeight / 100f)));
				}
			}
			return true;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0003A544 File Offset: 0x00038744
		private static int[] CalculateTroopCountsPerDefaultFormation(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins)
		{
			int[] array = new int[4];
			foreach (IAgentOriginBase agentOriginBase in troopOrigins)
			{
				FormationClass formationClass = Mission.Current.GetAgentTroopClass(battleSide, agentOriginBase.Troop).DefaultClass();
				array[(int)formationClass]++;
			}
			return array;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0003A5B8 File Offset: 0x000387B8
		private static FormationClass FindBestOrderOfBattleFormationClassAssignmentForTroop(BattleSideEnum battleSide, IAgentOriginBase origin, SandboxBattleSpawnModel.FormationOrderOfBattleConfiguration[] formationOrderOfBattleConfigurations, out SandboxBattleSpawnModel.OrderOfBattleInnerClassType bestClassInnerClassType)
		{
			FormationClass formationClass = Mission.Current.GetAgentTroopClass(battleSide, origin.Troop).DefaultClass();
			FormationClass formationClass2 = formationClass;
			float num = float.MinValue;
			bestClassInnerClassType = SandboxBattleSpawnModel.OrderOfBattleInnerClassType.None;
			for (int i = 0; i < 8; i++)
			{
				CharacterObject characterObject;
				if (origin.Troop.IsHero && (characterObject = origin.Troop as CharacterObject) != null && characterObject.HeroObject == formationOrderOfBattleConfigurations[i].Captain)
				{
					formationClass2 = (FormationClass)i;
					bestClassInnerClassType = SandboxBattleSpawnModel.OrderOfBattleInnerClassType.None;
					break;
				}
				if (formationClass == formationOrderOfBattleConfigurations[i].PrimaryFormationClass)
				{
					float num2 = (float)formationOrderOfBattleConfigurations[i].PrimaryClassDesiredTroopCount;
					float num3 = (float)formationOrderOfBattleConfigurations[i].PrimaryClassTroopCount;
					float num4 = 1f - num3 / (num2 + 1f);
					if (num4 > num)
					{
						formationClass2 = (FormationClass)i;
						bestClassInnerClassType = SandboxBattleSpawnModel.OrderOfBattleInnerClassType.PrimaryClass;
						num = num4;
					}
				}
				else if (formationClass == formationOrderOfBattleConfigurations[i].SecondaryFormationClass)
				{
					float num5 = (float)formationOrderOfBattleConfigurations[i].SecondaryClassDesiredTroopCount;
					float num6 = (float)formationOrderOfBattleConfigurations[i].SecondaryClassTroopCount;
					float num7 = 1f - num6 / (num5 + 1f);
					if (num7 > num)
					{
						formationClass2 = (FormationClass)i;
						bestClassInnerClassType = SandboxBattleSpawnModel.OrderOfBattleInnerClassType.SecondaryClass;
						num = num7;
					}
				}
			}
			return formationClass2;
		}

		// Token: 0x020001DC RID: 476
		private enum OrderOfBattleInnerClassType
		{
			// Token: 0x040008E4 RID: 2276
			None,
			// Token: 0x040008E5 RID: 2277
			PrimaryClass,
			// Token: 0x040008E6 RID: 2278
			SecondaryClass
		}

		// Token: 0x020001DD RID: 477
		private struct FormationOrderOfBattleConfiguration
		{
			// Token: 0x040008E7 RID: 2279
			public DeploymentFormationClass OOBFormationClass;

			// Token: 0x040008E8 RID: 2280
			public FormationClass PrimaryFormationClass;

			// Token: 0x040008E9 RID: 2281
			public int PrimaryClassTroopCount;

			// Token: 0x040008EA RID: 2282
			public int PrimaryClassDesiredTroopCount;

			// Token: 0x040008EB RID: 2283
			public FormationClass SecondaryFormationClass;

			// Token: 0x040008EC RID: 2284
			public int SecondaryClassTroopCount;

			// Token: 0x040008ED RID: 2285
			public int SecondaryClassDesiredTroopCount;

			// Token: 0x040008EE RID: 2286
			public Hero Captain;
		}
	}
}
