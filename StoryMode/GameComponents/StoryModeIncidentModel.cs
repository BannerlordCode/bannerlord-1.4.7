using System;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace StoryMode.GameComponents
{
	// Token: 0x02000044 RID: 68
	public class StoryModeIncidentModel : IncidentModel
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x000190B5 File Offset: 0x000172B5
		public override CampaignTime GetMinGlobalCooldownTime()
		{
			return base.BaseModel.GetMinGlobalCooldownTime();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000190C2 File Offset: 0x000172C2
		public override CampaignTime GetMaxGlobalCooldownTime()
		{
			return base.BaseModel.GetMaxGlobalCooldownTime();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000190CF File Offset: 0x000172CF
		public override float GetIncidentTriggerGlobalProbability()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetIncidentTriggerGlobalProbability();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000190EE File Offset: 0x000172EE
		public override float GetIncidentTriggerProbabilityDuringSiege()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetIncidentTriggerProbabilityDuringSiege();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001910D File Offset: 0x0001730D
		public override float GetIncidentTriggerProbabilityDuringWait()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetIncidentTriggerProbabilityDuringWait();
		}
	}
}
