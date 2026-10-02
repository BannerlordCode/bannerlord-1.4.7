using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F6 RID: 502
	public class CustomBattleInitializationModel : BattleInitializationModel
	{
		// Token: 0x06001D8C RID: 7564 RVA: 0x000652E8 File Offset: 0x000634E8
		public override List<FormationClass> GetAllAvailableTroopTypes()
		{
			List<FormationClass> list = new List<FormationClass>();
			foreach (Agent agent in Mission.Current.PlayerTeam.ActiveAgents)
			{
				BasicCharacterObject character = agent.Character;
				if (character.IsInfantry && !character.IsMounted && !list.Contains(FormationClass.Infantry))
				{
					list.Add(FormationClass.Infantry);
				}
				if (character.IsRanged && !character.IsMounted && !list.Contains(FormationClass.Ranged))
				{
					list.Add(FormationClass.Ranged);
				}
				if (character.IsMounted && !character.IsRanged && !list.Contains(FormationClass.Cavalry))
				{
					list.Add(FormationClass.Cavalry);
				}
				if (character.IsMounted && character.IsRanged && !list.Contains(FormationClass.HorseArcher))
				{
					list.Add(FormationClass.HorseArcher);
				}
			}
			return list;
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000653D0 File Offset: 0x000635D0
		protected override bool CanPlayerSideDeployWithOrderOfBattleAux()
		{
			if (Mission.Current.IsSallyOutBattle)
			{
				return false;
			}
			Team playerTeam = Mission.Current.PlayerTeam;
			return Mission.Current.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>().GetNumberOfPlayerControllableTroops() >= 20;
		}
	}
}
