using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.BattleScore;

namespace SandBox.Missions.BattleScore
{
	// Token: 0x0200009F RID: 159
	public class SandboxMissionBattleScoreContext : BattleScoreContext
	{
		// Token: 0x06000697 RID: 1687 RVA: 0x0002CABB File Offset: 0x0002ACBB
		public SandboxMissionBattleScoreContext(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0002CACA File Offset: 0x0002ACCA
		public override bool IsPowerComparisonRelevant
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0002CAD0 File Offset: 0x0002ACD0
		public override Banner GetAttackerBanner()
		{
			if (Campaign.Current == null)
			{
				return null;
			}
			MapEvent battle = PlayerEncounter.Battle;
			Banner banner;
			if (battle == null)
			{
				banner = null;
			}
			else
			{
				MapEventSide attackerSide = battle.AttackerSide;
				banner = ((attackerSide != null) ? attackerSide.LeaderParty.Banner : null);
			}
			Banner banner2;
			if ((banner2 = banner) == null)
			{
				Mission mission = this._mission;
				if (mission == null)
				{
					return null;
				}
				banner2 = mission.Teams.Attacker.Banner;
			}
			return banner2;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0002CB28 File Offset: 0x0002AD28
		public override Banner GetDefenderBanner()
		{
			if (Campaign.Current == null)
			{
				return null;
			}
			MapEvent battle = PlayerEncounter.Battle;
			Banner banner;
			if (battle == null)
			{
				banner = null;
			}
			else
			{
				MapEventSide defenderSide = battle.DefenderSide;
				banner = ((defenderSide != null) ? defenderSide.LeaderParty.Banner : null);
			}
			Banner banner2;
			if ((banner2 = banner) == null)
			{
				Mission mission = this._mission;
				if (mission == null)
				{
					return null;
				}
				banner2 = mission.Teams.Defender.Banner;
			}
			return banner2;
		}

		// Token: 0x04000391 RID: 913
		private readonly Mission _mission;
	}
}
