using System;
using SandBox.Conversation;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Issues.IssueQuestTasks
{
	// Token: 0x020000BD RID: 189
	public class BeginConversationInitiatedByAIQuestTask : QuestTaskBase
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x000349D2 File Offset: 0x00032BD2
		public BeginConversationInitiatedByAIQuestTask(Agent agent, Action onSucceededAction, Action onFailedAction, Action onCanceledAction, DialogFlow dialogFlow = null)
			: base(dialogFlow, onSucceededAction, onFailedAction, onCanceledAction)
		{
			this._conversationAgent = agent;
			base.IsLogged = false;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000349EE File Offset: 0x00032BEE
		public void MissionTick(float dt)
		{
			if (Mission.Current.MainAgent == null || this._conversationAgent == null)
			{
				return;
			}
			if (!this._conversationOpened && Mission.Current.Mode != MissionMode.Conversation)
			{
				this.OpenConversation(this._conversationAgent);
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00034A26 File Offset: 0x00032C26
		private void OpenConversation(Agent agent)
		{
			ConversationMission.StartConversationWithAgent(agent);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00034A2E File Offset: 0x00032C2E
		protected override void OnFinished()
		{
			this._conversationAgent = null;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00034A37 File Offset: 0x00032C37
		public override void SetReferences()
		{
			CampaignEvents.MissionTickEvent.AddNonSerializedListener(this, new Action<float>(this.MissionTick));
		}

		// Token: 0x04000427 RID: 1063
		private bool _conversationOpened;

		// Token: 0x04000428 RID: 1064
		private Agent _conversationAgent;
	}
}
