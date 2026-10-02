using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000084 RID: 132
	public class DialogFlow
	{
		// Token: 0x060010E3 RID: 4323 RVA: 0x00050E7C File Offset: 0x0004F07C
		private DialogFlow(string startingToken, int priority = 100)
		{
			this._currentToken = startingToken;
			this.Priority = priority;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00050EA0 File Offset: 0x0004F0A0
		private DialogFlow Line(TextObject text, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, bool isRepeatable = false, string inputToken = null, string outputToken = null)
		{
			string text2 = outputToken ?? Campaign.Current.ConversationManager.CreateToken();
			this.AddLine(text, inputToken ?? this._currentToken, text2, byPlayer, speakerDelegate, listenerDelegate, isRepeatable, false, false);
			this._currentToken = text2;
			return this;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00050EE8 File Offset: 0x0004F0E8
		public DialogFlow Variation(string text, params object[] propertiesAndWeights)
		{
			return this.Variation(new TextObject(text, null), propertiesAndWeights);
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00050EF8 File Offset: 0x0004F0F8
		public DialogFlow Variation(TextObject text, params object[] propertiesAndWeights)
		{
			for (int i = 0; i < propertiesAndWeights.Length; i += 2)
			{
				string text2 = (string)propertiesAndWeights[i];
				int num = Convert.ToInt32(propertiesAndWeights[i + 1]);
				List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
				list.Add(new GameTextManager.ChoiceTag(text2, num));
				this.Lines[this.Lines.Count - 1].AddVariation(text, list);
			}
			return this;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00050F5A File Offset: 0x0004F15A
		public DialogFlow NpcLine(string npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.NpcLine(new TextObject(npcText, null), speakerDelegate, listenerDelegate, inputToken, outputToken);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00050F6F File Offset: 0x0004F16F
		public DialogFlow NpcLine(TextObject npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.Line(npcText, false, speakerDelegate, listenerDelegate, false, inputToken, outputToken);
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00050F80 File Offset: 0x0004F180
		public DialogFlow NpcLineWithVariation(string npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			DialogFlow dialogFlow = this.Line(TextObject.GetEmpty(), false, speakerDelegate, listenerDelegate, false, inputToken, outputToken);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this.Lines[this.Lines.Count - 1].AddVariation(new TextObject(npcText, null), list);
			return dialogFlow;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00050FDC File Offset: 0x0004F1DC
		public DialogFlow NpcLineWithVariation(TextObject npcText, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			DialogFlow dialogFlow = this.Line(TextObject.GetEmpty(), false, speakerDelegate, listenerDelegate, false, inputToken, outputToken);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this.Lines[this.Lines.Count - 1].AddVariation(npcText, list);
			return dialogFlow;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00051032 File Offset: 0x0004F232
		public DialogFlow PlayerLine(string playerText, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.Line(new TextObject(playerText, null), true, null, listenerDelegate, false, inputToken, outputToken);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00051048 File Offset: 0x0004F248
		public DialogFlow PlayerLine(TextObject playerText, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.Line(playerText, true, null, listenerDelegate, false, inputToken, outputToken);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00051058 File Offset: 0x0004F258
		private DialogFlow BeginOptions(bool byPlayer, string inputToken = null, bool optionUsedOnce = false)
		{
			this._curDialogFlowContext = new DialogFlowContext(inputToken ?? this._currentToken, byPlayer, this._curDialogFlowContext, optionUsedOnce);
			return this;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00051079 File Offset: 0x0004F279
		public DialogFlow BeginPlayerOptions(string inputToken = null, bool optionUsedOnce = false)
		{
			return this.BeginOptions(true, inputToken, optionUsedOnce);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00051084 File Offset: 0x0004F284
		public DialogFlow BeginNpcOptions(string inputToken = null, bool optionUsedOnce = false)
		{
			return this.BeginOptions(false, inputToken, optionUsedOnce);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00051090 File Offset: 0x0004F290
		private DialogFlow Option(TextObject text, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, bool isRepeatable = false, bool isSpecialOption = false, string inputToken = null, string outputToken = null)
		{
			string text2 = outputToken ?? Campaign.Current.ConversationManager.CreateToken();
			this.AddLine(text, inputToken ?? this._curDialogFlowContext.Token, text2, byPlayer, speakerDelegate, listenerDelegate, isRepeatable, isSpecialOption, this._curDialogFlowContext.OptionsUsedOnlyOnce);
			this._currentToken = text2;
			return this;
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000510E8 File Offset: 0x0004F2E8
		public DialogFlow PlayerOption(string text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			return this.PlayerOption(new TextObject(text, null), listenerDelegate, inputToken, outputToken);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000510FC File Offset: 0x0004F2FC
		public DialogFlow PlayerOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, true, null, listenerDelegate, false, false, inputToken, outputToken);
			return this;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0005111C File Offset: 0x0004F31C
		public DialogFlow PlayerSpecialOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, true, null, listenerDelegate, false, true, inputToken, outputToken);
			return this;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0005113C File Offset: 0x0004F33C
		public DialogFlow PlayerRepeatableOption(TextObject text, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, true, null, listenerDelegate, true, false, inputToken, outputToken);
			return this;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0005115C File Offset: 0x0004F35C
		public DialogFlow NpcOption(string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(new TextObject(text, null), false, speakerDelegate, listenerDelegate, false, false, inputToken, outputToken);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00051190 File Offset: 0x0004F390
		public DialogFlow NpcOption(TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(text, false, speakerDelegate, listenerDelegate, false, false, inputToken, outputToken);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x000511BC File Offset: 0x0004F3BC
		public DialogFlow NpcOptionWithVariation(string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.NpcOptionWithVariation(new TextObject(text, null), conditionDelegate, speakerDelegate, listenerDelegate, inputToken, outputToken);
			return this;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x000511D8 File Offset: 0x0004F3D8
		public DialogFlow NpcOptionWithVariation(TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null, string inputToken = null, string outputToken = null)
		{
			this.Option(TextObject.GetEmpty(), false, speakerDelegate, listenerDelegate, false, false, inputToken, outputToken);
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			list.Add(new GameTextManager.ChoiceTag("DefaultTag", 1));
			this._lastLine.AddVariation(text, list);
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0005122C File Offset: 0x0004F42C
		private DialogFlow EndOptions(bool byPlayer)
		{
			this._curDialogFlowContext = this._curDialogFlowContext.Parent;
			return this;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00051240 File Offset: 0x0004F440
		public DialogFlow EndPlayerOptions()
		{
			return this.EndOptions(true);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00051249 File Offset: 0x0004F449
		public DialogFlow EndNpcOptions()
		{
			return this.EndOptions(false);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00051252 File Offset: 0x0004F452
		public DialogFlow Condition(ConversationSentence.OnConditionDelegate conditionDelegate)
		{
			this._lastLine.ConditionDelegate = conditionDelegate;
			return this;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00051261 File Offset: 0x0004F461
		public DialogFlow ClickableCondition(ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate)
		{
			this._lastLine.ClickableConditionDelegate = clickableConditionDelegate;
			return this;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00051270 File Offset: 0x0004F470
		public DialogFlow Consequence(ConversationSentence.OnConsequenceDelegate consequenceDelegate)
		{
			this._lastLine.ConsequenceDelegate = consequenceDelegate;
			return this;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0005127F File Offset: 0x0004F47F
		public static DialogFlow CreateDialogFlow(string inputToken = null, int priority = 100)
		{
			return new DialogFlow(inputToken ?? Campaign.Current.ConversationManager.CreateToken(), priority);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0005129C File Offset: 0x0004F49C
		private DialogFlowLine AddLine(TextObject text, string inputToken, string outputToken, bool byPlayer, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate, bool isRepeatable, bool isSpecialOption = false, bool usedOncePerConversation = false)
		{
			DialogFlowLine dialogFlowLine = new DialogFlowLine();
			dialogFlowLine.Text = text;
			dialogFlowLine.InputToken = inputToken;
			dialogFlowLine.OutputToken = outputToken;
			dialogFlowLine.ByPlayer = byPlayer;
			dialogFlowLine.SpeakerDelegate = speakerDelegate;
			dialogFlowLine.ListenerDelegate = listenerDelegate;
			dialogFlowLine.IsRepeatable = isRepeatable;
			dialogFlowLine.IsSpecialOption = isSpecialOption;
			dialogFlowLine.IsUsedOnce = usedOncePerConversation;
			this.Lines.Add(dialogFlowLine);
			this._lastLine = dialogFlowLine;
			return dialogFlowLine;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00051308 File Offset: 0x0004F508
		public DialogFlow NpcDefaultOption(string text)
		{
			return this.NpcOption(text, null, null, null, null, null);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00051316 File Offset: 0x0004F516
		public DialogFlow GenerateToken(out string token)
		{
			token = Campaign.Current.ConversationManager.CreateToken();
			return this;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0005132A File Offset: 0x0004F52A
		public DialogFlow GotoDialogState(string input)
		{
			this._lastLine.OutputToken = input;
			this._currentToken = input;
			return this;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00051340 File Offset: 0x0004F540
		public DialogFlow GotoDialogStateBranched(string input, ConversationSentence.OnConditionDelegate conditionDelegate, string alternative)
		{
			string text = ((conditionDelegate != null && conditionDelegate()) ? input : alternative);
			this._lastLine.OutputToken = text;
			this._currentToken = text;
			return this;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00051371 File Offset: 0x0004F571
		public DialogFlow GetOutputToken(out string oState)
		{
			oState = this._lastLine.OutputToken;
			return this;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00051381 File Offset: 0x0004F581
		public DialogFlow GoBackToDialogState(string iState)
		{
			this._currentToken = iState;
			return this;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0005138B File Offset: 0x0004F58B
		public DialogFlow CloseDialog()
		{
			this.GotoDialogState("close_window");
			return this;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0005139A File Offset: 0x0004F59A
		private ConversationSentence AddDialogLine(ConversationSentence dialogLine)
		{
			Campaign.Current.ConversationManager.AddDialogLine(dialogLine);
			return dialogLine;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x000513B0 File Offset: 0x0004F5B0
		public ConversationSentence AddPlayerLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, object relatedObject, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 1U, priority, 0, 0, relatedObject, false, speakerDelegate, listenerDelegate, persuasionOptionDelegate));
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000513EC File Offset: 0x0004F5EC
		public ConversationSentence AddDialogLine(string id, string inputToken, string outputToken, string text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, object relatedObject, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate speakerDelegate = null, ConversationSentence.OnMultipleConversationConsequenceDelegate listenerDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, new TextObject(text, null), inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, 0, 0, relatedObject, false, speakerDelegate, listenerDelegate, null));
		}

		// Token: 0x0400054E RID: 1358
		internal readonly List<DialogFlowLine> Lines = new List<DialogFlowLine>();

		// Token: 0x0400054F RID: 1359
		internal readonly int Priority;

		// Token: 0x04000550 RID: 1360
		private string _currentToken;

		// Token: 0x04000551 RID: 1361
		private DialogFlowLine _lastLine;

		// Token: 0x04000552 RID: 1362
		private DialogFlowContext _curDialogFlowContext;
	}
}
