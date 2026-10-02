using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000232 RID: 562
	public class ConversationAnimData
	{
		// Token: 0x06002244 RID: 8772 RVA: 0x0009755F File Offset: 0x0009575F
		public ConversationAnimData()
		{
			this.Reactions = new Dictionary<string, string>();
		}

		// Token: 0x04000A0B RID: 2571
		[SaveableField(0)]
		public string IdleAnimStart;

		// Token: 0x04000A0C RID: 2572
		[SaveableField(1)]
		public string IdleAnimLoop;

		// Token: 0x04000A0D RID: 2573
		[SaveableField(2)]
		public int FamilyType;

		// Token: 0x04000A0E RID: 2574
		[SaveableField(3)]
		public int MountFamilyType;

		// Token: 0x04000A0F RID: 2575
		[SaveableField(4)]
		public Dictionary<string, string> Reactions;
	}
}
