using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200007B RID: 123
	public interface IAgent
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000853 RID: 2131
		BasicCharacterObject Character { get; }

		// Token: 0x06000854 RID: 2132
		bool IsEnemyOf(IAgent agent);

		// Token: 0x06000855 RID: 2133
		bool IsFriendOf(IAgent agent);

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000856 RID: 2134
		AgentState State { get; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000857 RID: 2135
		IMissionTeam Team { get; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000858 RID: 2136
		IAgentOriginBase Origin { get; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000859 RID: 2137
		float Age { get; }

		// Token: 0x0600085A RID: 2138
		bool IsActive();

		// Token: 0x0600085B RID: 2139
		void SetAsConversationAgent(bool set);

		// Token: 0x0600085C RID: 2140
		void OnConversationStarted();
	}
}
