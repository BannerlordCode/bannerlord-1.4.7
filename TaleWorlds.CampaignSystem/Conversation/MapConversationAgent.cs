using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000231 RID: 561
	public class MapConversationAgent : IAgent
	{
		// Token: 0x06002239 RID: 8761 RVA: 0x00097525 File Offset: 0x00095725
		public MapConversationAgent(CharacterObject characterObject)
		{
			this._characterObject = characterObject;
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x00097534 File Offset: 0x00095734
		public BasicCharacterObject Character
		{
			get
			{
				return this._characterObject;
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x0009753C File Offset: 0x0009573C
		public bool IsEnemyOf(IAgent agent)
		{
			return false;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x0009753F File Offset: 0x0009573F
		public bool IsFriendOf(IAgent agent)
		{
			return true;
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x00097542 File Offset: 0x00095742
		public AgentState State
		{
			get
			{
				return AgentState.Active;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x00097545 File Offset: 0x00095745
		public IMissionTeam Team
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x00097548 File Offset: 0x00095748
		public IAgentOriginBase Origin
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x0009754B File Offset: 0x0009574B
		public float Age
		{
			get
			{
				return this.Character.Age;
			}
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00097558 File Offset: 0x00095758
		public bool IsActive()
		{
			return true;
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0009755B File Offset: 0x0009575B
		public void SetAsConversationAgent(bool set)
		{
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x0009755D File Offset: 0x0009575D
		public void OnConversationStarted()
		{
		}

		// Token: 0x04000A09 RID: 2569
		private CharacterObject _characterObject;

		// Token: 0x04000A0A RID: 2570
		public bool DeliveredLine;
	}
}
