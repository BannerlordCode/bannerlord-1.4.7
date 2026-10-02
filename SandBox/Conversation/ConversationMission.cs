using System;
using System.Collections.Generic;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Conversation
{
	// Token: 0x020000CA RID: 202
	public static class ConversationMission
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0003AEFE File Offset: 0x000390FE
		public static Agent OneToOneConversationAgent
		{
			get
			{
				return Campaign.Current.ConversationManager.OneToOneConversationAgent as Agent;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0003AF14 File Offset: 0x00039114
		public static CharacterObject OneToOneConversationCharacter
		{
			get
			{
				return Campaign.Current.ConversationManager.OneToOneConversationCharacter;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0003AF25 File Offset: 0x00039125
		public static Agent CurrentSpeakerAgent
		{
			get
			{
				return Campaign.Current.ConversationManager.SpeakerAgent as Agent;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0003AF3B File Offset: 0x0003913B
		public static IEnumerable<Agent> ConversationAgents
		{
			get
			{
				foreach (IAgent agent in Campaign.Current.ConversationManager.ConversationAgents)
				{
					yield return agent as Agent;
				}
				IEnumerator<IAgent> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0003AF44 File Offset: 0x00039144
		public static void StartConversationWithAgent(Agent agent)
		{
			MissionConversationLogic missionBehavior = Mission.Current.GetMissionBehavior<MissionConversationLogic>();
			if (missionBehavior == null)
			{
				return;
			}
			missionBehavior.StartConversation(agent, true, false);
		}
	}
}
