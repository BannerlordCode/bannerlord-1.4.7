using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Screens;

namespace SandBox.View.Missions
{
	// Token: 0x02000014 RID: 20
	public class MissionCampaignBattleSpectatorView : MissionView
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00005000 File Offset: 0x00003200
		public override void AfterStart()
		{
			base.MissionScreen.SetCustomAgentListToSpectateGatherer(new MissionScreen.GatherCustomAgentListToSpectateDelegate(this.SpectateListGatherer));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000501C File Offset: 0x0000321C
		private int CalculateAgentScore(Agent agent)
		{
			Mission mission = agent.Mission;
			CharacterObject characterObject = (CharacterObject)agent.Character;
			int num = (agent.IsPlayerControlled ? 2000000 : 0);
			if (agent.Team != null && agent.Team.IsValid)
			{
				num += ((mission.PlayerTeam != null && mission.PlayerTeam.IsValid && agent.Team.IsEnemyOf(mission.PlayerTeam)) ? 0 : 1000000);
				if (agent.Team.GeneralAgent == agent)
				{
					num += 500000;
				}
				else if (characterObject.IsHero)
				{
					if (characterObject.HeroObject.IsLord)
					{
						num += 125000;
					}
					else
					{
						num += 250000;
					}
					using (List<Formation>.Enumerator enumerator = agent.Team.FormationsIncludingEmpty.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.Captain == agent)
							{
								num += 100000;
							}
						}
					}
				}
				if (characterObject.IsMounted)
				{
					num += 50000;
				}
				if (!characterObject.IsRanged)
				{
					num += 25000;
				}
				num += (int)agent.Health;
			}
			return num;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005158 File Offset: 0x00003358
		private List<Agent> SpectateListGatherer(Agent forcedAgentToInclude)
		{
			return base.Mission.AllAgents.WhereQ<Agent>((Agent x) => x.IsCameraAttachable() || x == forcedAgentToInclude).OrderByDescending<Agent, int>(new Func<Agent, int>(this.CalculateAgentScore)).ToList<Agent>();
		}
	}
}
