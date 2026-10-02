using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000082 RID: 130
	internal class DialogFlowLine
	{
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x00050E18 File Offset: 0x0004F018
		// (set) Token: 0x060010DD RID: 4317 RVA: 0x00050E0F File Offset: 0x0004F00F
		public List<KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>> Variations { get; private set; }

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00050E20 File Offset: 0x0004F020
		public bool HasVariation
		{
			get
			{
				return this.Variations.Count > 0;
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00050E30 File Offset: 0x0004F030
		internal DialogFlowLine()
		{
			this.Variations = new List<KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>>();
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00050E43 File Offset: 0x0004F043
		public void AddVariation(TextObject text, List<GameTextManager.ChoiceTag> list)
		{
			this.Variations.Add(new KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>(text, list));
		}

		// Token: 0x0400053D RID: 1341
		internal TextObject Text;

		// Token: 0x0400053E RID: 1342
		internal string InputToken;

		// Token: 0x0400053F RID: 1343
		internal string OutputToken;

		// Token: 0x04000540 RID: 1344
		internal bool ByPlayer;

		// Token: 0x04000541 RID: 1345
		internal ConversationSentence.OnConditionDelegate ConditionDelegate;

		// Token: 0x04000542 RID: 1346
		internal ConversationSentence.OnClickableConditionDelegate ClickableConditionDelegate;

		// Token: 0x04000543 RID: 1347
		internal ConversationSentence.OnConsequenceDelegate ConsequenceDelegate;

		// Token: 0x04000544 RID: 1348
		internal ConversationSentence.OnMultipleConversationConsequenceDelegate SpeakerDelegate;

		// Token: 0x04000545 RID: 1349
		internal ConversationSentence.OnMultipleConversationConsequenceDelegate ListenerDelegate;

		// Token: 0x04000546 RID: 1350
		internal bool IsRepeatable;

		// Token: 0x04000547 RID: 1351
		internal bool IsSpecialOption;

		// Token: 0x04000548 RID: 1352
		internal bool IsUsedOnce;
	}
}
