using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Conversation.Tags;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000237 RID: 567
	public class ConversationManager
	{
		// Token: 0x06002252 RID: 8786 RVA: 0x0009859E File Offset: 0x0009679E
		public int CreateConversationSentenceIndex()
		{
			int numConversationSentencesCreated = this._numConversationSentencesCreated;
			this._numConversationSentencesCreated++;
			return numConversationSentencesCreated;
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x000985B4 File Offset: 0x000967B4
		public string CurrentSentenceText
		{
			get
			{
				TextObject textObject = this._currentSentenceText;
				if (this.OneToOneConversationCharacter != null)
				{
					textObject = this.FindMatchingTextOrNull(textObject.GetID(), this.OneToOneConversationCharacter);
					if (textObject == null)
					{
						textObject = this._currentSentenceText;
					}
				}
				return MBTextManager.DiscardAnimationTagsAndCheckAnimationTagPositions(textObject.CopyTextObject().ToString());
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002254 RID: 8788 RVA: 0x00098603 File Offset: 0x00096803
		private int DialogRepeatCount
		{
			get
			{
				if (this._dialogRepeatObjects.Count > 0)
				{
					return this._dialogRepeatObjects[this._currentRepeatedDialogSetIndex].Count;
				}
				return 1;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x0009862B File Offset: 0x0009682B
		public bool IsConversationFlowActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00098634 File Offset: 0x00096834
		public ConversationManager()
		{
			this._sentences = new List<ConversationSentence>();
			this.stateMap = new Dictionary<string, int>();
			this.stateMap.Add("start", 0);
			this.stateMap.Add("event_triggered", 1);
			this.stateMap.Add("member_chat", 2);
			this.stateMap.Add("prisoner_chat", 3);
			this.stateMap.Add("close_window", 4);
			this._numberOfStateIndices = 5;
			this._isActive = false;
			this._executeDoOptionContinue = false;
			this.InitializeTags();
			this.ConversationAnimationManager = new ConversationAnimationManager();
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x0009870F File Offset: 0x0009690F
		// (set) Token: 0x06002258 RID: 8792 RVA: 0x00098717 File Offset: 0x00096917
		public List<ConversationSentenceOption> CurOptions { get; protected set; }

		// Token: 0x06002259 RID: 8793 RVA: 0x00098720 File Offset: 0x00096920
		public void StartNew(int startingToken, bool setActionsInstantly)
		{
			this._usedIndices.Clear();
			this.ActiveToken = startingToken;
			this._currentSentence = -1;
			this.ResetRepeatedDialogSystem();
			this._lastSelectedDialogObject = null;
			Debug.Print("--------------- Conversation Start --------------- ", 0, Debug.DebugColor.White, 4503599627370496UL);
			Debug.Print(string.Concat(new object[]
			{
				"Conversation character name: ",
				this.OneToOneConversationCharacter.Name,
				"\nid: ",
				this.OneToOneConversationCharacter.StringId,
				"\nculture:",
				this.OneToOneConversationCharacter.Culture,
				"\npersona:",
				this.OneToOneConversationCharacter.GetPersona().Name
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			this._mainAgent.OnConversationStarted();
			if (CampaignMission.Current != null)
			{
				foreach (IAgent agent in this.ConversationAgents)
				{
					CampaignMission.Current.OnConversationStart(agent, setActionsInstantly);
				}
			}
			this.ProcessPartnerSentence();
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00098840 File Offset: 0x00096A40
		private bool ProcessPartnerSentence()
		{
			List<ConversationSentenceOption> sentenceOptions = this.GetSentenceOptions(false, false);
			bool flag = false;
			if (sentenceOptions.Count > 0)
			{
				this.ProcessSentence(sentenceOptions[0]);
				flag = true;
			}
			IConversationStateHandler handler = this.Handler;
			if (handler != null)
			{
				handler.OnConversationContinue();
			}
			return flag;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00098884 File Offset: 0x00096A84
		public void ProcessSentence(ConversationSentenceOption conversationSentenceOption)
		{
			ConversationSentence conversationSentence = this._sentences[conversationSentenceOption.SentenceNo];
			Debug.Print(conversationSentenceOption.DebugInfo, 0, Debug.DebugColor.White, 4503599627370496UL);
			this.ActiveToken = conversationSentence.OutputToken;
			this.UpdateSpeakerAndListenerAgents(conversationSentence);
			if (CampaignMission.Current != null)
			{
				CampaignMission.Current.OnProcessSentence();
			}
			this._lastSelectedDialogObject = conversationSentenceOption.RepeatObject;
			this._currentSentence = conversationSentenceOption.SentenceNo;
			if (Game.Current == null)
			{
				throw new MBNullParameterException("Game");
			}
			this.UpdateCurrentSentenceText();
			int count = this._sentences.Count;
			conversationSentence.RunConsequence(Game.Current);
			if (conversationSentence.IsUsedOnce)
			{
				this._usedIndices.Add(conversationSentence.Index);
			}
			if (CampaignMission.Current != null)
			{
				string[] conversationAnimations = MBTextManager.GetConversationAnimations(this._currentSentenceText);
				string text = "";
				VoiceObject voiceObject;
				string text2;
				if (MBTextManager.TryGetVoiceObject(this._currentSentenceText, out voiceObject, out text2))
				{
					text = Campaign.Current.Models.VoiceOverModel.GetSoundPathForCharacter((CharacterObject)this.SpeakerAgent.Character, voiceObject);
				}
				CampaignMission.Current.OnConversationPlay(conversationAnimations[0], conversationAnimations[1], conversationAnimations[2], conversationAnimations[3], text);
			}
			if (0 > this._currentSentence || this._currentSentence >= count)
			{
				Debug.FailedAssert("CurrentSentence is not valid.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Conversation\\ConversationManager.cs", "ProcessSentence", 417);
			}
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000989D4 File Offset: 0x00096BD4
		private void UpdateSpeakerAndListenerAgents(ConversationSentence sentence)
		{
			if (sentence.IsSpeaker != null)
			{
				if (sentence.IsSpeaker(this._mainAgent))
				{
					this.SetSpeakerAgent(this._mainAgent);
					goto IL_008B;
				}
				using (IEnumerator<IAgent> enumerator = this.ConversationAgents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IAgent agent = enumerator.Current;
						if (sentence.IsSpeaker(agent))
						{
							this.SetSpeakerAgent(agent);
							break;
						}
					}
					goto IL_008B;
				}
			}
			this.SetSpeakerAgent((!sentence.IsPlayer) ? this.ConversationAgents[0] : this._mainAgent);
			IL_008B:
			if (sentence.IsListener != null)
			{
				if (sentence.IsListener(this._mainAgent))
				{
					this.SetListenerAgent(this._mainAgent);
					return;
				}
				using (IEnumerator<IAgent> enumerator = this.ConversationAgents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IAgent agent2 = enumerator.Current;
						if (sentence.IsListener(agent2))
						{
							this.SetListenerAgent(agent2);
							break;
						}
					}
					return;
				}
			}
			this.SetListenerAgent((!sentence.IsPlayer) ? this._mainAgent : this.ConversationAgents[0]);
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00098B14 File Offset: 0x00096D14
		private void SetSpeakerAgent(IAgent agent)
		{
			if (this._speakerAgent != agent)
			{
				this._speakerAgent = agent;
				if (this._speakerAgent != null && this._speakerAgent.Character is CharacterObject)
				{
					StringHelpers.SetCharacterProperties("SPEAKER", agent.Character as CharacterObject, null, false);
				}
			}
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x00098B64 File Offset: 0x00096D64
		private void SetListenerAgent(IAgent agent)
		{
			if (this._listenerAgent != agent)
			{
				this._listenerAgent = agent;
				if (this._listenerAgent != null && this._listenerAgent.Character is CharacterObject)
				{
					StringHelpers.SetCharacterProperties("LISTENER", agent.Character as CharacterObject, null, false);
				}
			}
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x00098BB4 File Offset: 0x00096DB4
		public void UpdateCurrentSentenceText()
		{
			TextObject textObject;
			if (this._currentSentence >= 0)
			{
				textObject = this._sentences[this._currentSentence].Text;
			}
			else
			{
				if (Campaign.Current == null)
				{
					throw new MBNullParameterException("Campaign");
				}
				textObject = GameTexts.FindText("str_error_string", null);
			}
			this._currentSentenceText = textObject;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x00098C08 File Offset: 0x00096E08
		public bool IsConversationEnded()
		{
			return this.ActiveToken == 4;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x00098C13 File Offset: 0x00096E13
		public void ClearCurrentOptions()
		{
			if (this.CurOptions == null)
			{
				this.CurOptions = new List<ConversationSentenceOption>();
			}
			this.CurOptions.Clear();
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x00098C34 File Offset: 0x00096E34
		public void AddToCurrentOptions(TextObject text, string id, bool isClickable, TextObject hintText)
		{
			ConversationSentenceOption conversationSentenceOption = new ConversationSentenceOption
			{
				SentenceNo = 0,
				Text = text,
				Id = id,
				RepeatObject = null,
				DebugInfo = null,
				IsClickable = isClickable,
				HintText = hintText
			};
			this.CurOptions.Add(conversationSentenceOption);
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x00098C90 File Offset: 0x00096E90
		public void GetPlayerSentenceOptions()
		{
			this.CurOptions = this.GetSentenceOptions(true, true);
			if (this.CurOptions.Count > 0)
			{
				ConversationSentenceOption conversationSentenceOption = this.CurOptions[0];
				foreach (ConversationSentenceOption conversationSentenceOption2 in this.CurOptions)
				{
					if (this._sentences[conversationSentenceOption2.SentenceNo].IsListener != null)
					{
						conversationSentenceOption = conversationSentenceOption2;
						break;
					}
				}
				this.UpdateSpeakerAndListenerAgents(this._sentences[conversationSentenceOption.SentenceNo]);
			}
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x00098D38 File Offset: 0x00096F38
		public int GetStateIndex(string str)
		{
			int num;
			if (this.stateMap.ContainsKey(str))
			{
				num = this.stateMap[str];
			}
			else
			{
				num = this._numberOfStateIndices;
				Dictionary<string, int> dictionary = this.stateMap;
				int numberOfStateIndices = this._numberOfStateIndices;
				this._numberOfStateIndices = numberOfStateIndices + 1;
				dictionary.Add(str, numberOfStateIndices);
			}
			return num;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00098D87 File Offset: 0x00096F87
		internal void Build()
		{
			this.SortSentences();
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x00098D8F File Offset: 0x00096F8F
		public void DisableSentenceSort()
		{
			this._sortSentenceIsDisabled = true;
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x00098D98 File Offset: 0x00096F98
		public void EnableSentenceSort()
		{
			this._sortSentenceIsDisabled = false;
			this.SortSentences();
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00098DA7 File Offset: 0x00096FA7
		private void SortSentences()
		{
			this._sentences = this._sentences.OrderByDescending<ConversationSentence, int>((ConversationSentence pair) => pair.Priority).ToList<ConversationSentence>();
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00098DE0 File Offset: 0x00096FE0
		private void SortLastSentence()
		{
			int num = this._sentences.Count - 1;
			ConversationSentence conversationSentence = this._sentences[num];
			int priority = conversationSentence.Priority;
			int num2 = num - 1;
			while (num2 >= 0 && this._sentences[num2].Priority < priority)
			{
				this._sentences[num2 + 1] = this._sentences[num2];
				num = num2;
				num2--;
			}
			this._sentences[num] = conversationSentence;
			if (this.CurOptions != null)
			{
				for (int i = 0; i < this.CurOptions.Count; i++)
				{
					if (this.CurOptions[i].SentenceNo >= num)
					{
						ConversationSentenceOption conversationSentenceOption = this.CurOptions[i];
						conversationSentenceOption.SentenceNo = this.CurOptions[i].SentenceNo + 1;
						this.CurOptions[i] = conversationSentenceOption;
					}
				}
			}
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00098ECC File Offset: 0x000970CC
		private List<ConversationSentenceOption> GetSentenceOptions(bool onlyPlayer, bool processAfterOneOption)
		{
			List<ConversationSentenceOption> list = new List<ConversationSentenceOption>();
			ConversationManager.SetupTextVariables();
			for (int i = 0; i < this._sentences.Count; i++)
			{
				if (this.GetSentenceMatch(i, onlyPlayer))
				{
					ConversationSentence conversationSentence = this._sentences[i];
					int num = 1;
					this._dialogRepeatLines.Clear();
					this._currentRepeatIndex = 0;
					if (conversationSentence.IsRepeatable)
					{
						num = this.DialogRepeatCount;
					}
					for (int j = 0; j < num; j++)
					{
						this._dialogRepeatLines.Add(conversationSentence.Text.CopyTextObject());
						if (conversationSentence.RunCondition())
						{
							conversationSentence.IsClickable = conversationSentence.RunClickableCondition();
							if (conversationSentence.IsWithVariation)
							{
								TextObject textObject = this.FindMatchingTextOrNull(conversationSentence.Id, this.OneToOneConversationCharacter);
								GameTexts.SetVariable("VARIATION_TEXT_TAGGED_LINE", textObject);
							}
							string text = (conversationSentence.IsPlayer ? "P  -> (" : "AI -> (") + conversationSentence.Id + ") - ";
							ConversationSentenceOption conversationSentenceOption = new ConversationSentenceOption
							{
								SentenceNo = i,
								Text = this.GetCurrentDialogLine(),
								Id = conversationSentence.Id,
								RepeatObject = this.GetCurrentProcessedRepeatObject(),
								DebugInfo = text,
								IsClickable = conversationSentence.IsClickable,
								HasPersuasion = conversationSentence.HasPersuasion,
								SkillName = conversationSentence.SkillName,
								TraitName = conversationSentence.TraitName,
								IsSpecial = conversationSentence.IsSpecial,
								IsUsedOnce = conversationSentence.IsUsedOnce,
								HintText = conversationSentence.HintText,
								PersuationOptionArgs = conversationSentence.PersuationOptionArgs
							};
							list.Add(conversationSentenceOption);
							if (conversationSentence.IsRepeatable)
							{
								this._currentRepeatIndex++;
							}
							if (!processAfterOneOption)
							{
								return list;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000990A8 File Offset: 0x000972A8
		private bool GetSentenceMatch(int sentenceIndex, bool onlyPlayer)
		{
			if (0 > sentenceIndex || sentenceIndex >= this._sentences.Count)
			{
				throw new MBOutOfRangeException("Sentence index is not valid.");
			}
			bool flag = this._sentences[sentenceIndex].InputToken != this.ActiveToken;
			if (!flag && onlyPlayer)
			{
				flag = !this._sentences[sentenceIndex].IsPlayer;
			}
			if (!flag)
			{
				flag = this._sentences[sentenceIndex].IsUsedOnce && this._usedIndices.Contains(this._sentences[sentenceIndex].Index);
			}
			return !flag;
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00099146 File Offset: 0x00097346
		internal object GetCurrentProcessedRepeatObject()
		{
			if (this._dialogRepeatObjects.Count <= 0)
			{
				return null;
			}
			return this._dialogRepeatObjects[this._currentRepeatedDialogSetIndex][this._currentRepeatIndex];
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00099174 File Offset: 0x00097374
		internal TextObject GetCurrentDialogLine()
		{
			if (this._dialogRepeatLines.Count <= this._currentRepeatIndex)
			{
				return null;
			}
			return this._dialogRepeatLines[this._currentRepeatIndex];
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x0009919C File Offset: 0x0009739C
		internal object GetSelectedRepeatObject()
		{
			return this._lastSelectedDialogObject;
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000991A4 File Offset: 0x000973A4
		internal void SetDialogRepeatCount(IReadOnlyList<object> dialogRepeatObjects, int maxRepeatedDialogsInConversation)
		{
			this._dialogRepeatObjects.Clear();
			bool flag = dialogRepeatObjects.Count > maxRepeatedDialogsInConversation + 1;
			List<object> list = new List<object>(maxRepeatedDialogsInConversation);
			for (int i = 0; i < dialogRepeatObjects.Count; i++)
			{
				object obj = dialogRepeatObjects[i];
				if (flag && i % maxRepeatedDialogsInConversation == 0)
				{
					list = new List<object>(maxRepeatedDialogsInConversation);
					this._dialogRepeatObjects.Add(list);
				}
				list.Add(obj);
			}
			if (!flag && !list.IsEmpty<object>())
			{
				this._dialogRepeatObjects.Add(list);
			}
			this._currentRepeatedDialogSetIndex = 0;
			this._currentRepeatIndex = 0;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00099230 File Offset: 0x00097430
		internal static void DialogRepeatContinueListing()
		{
			Campaign campaign = Campaign.Current;
			ConversationManager conversationManager = ((campaign != null) ? campaign.ConversationManager : null);
			if (conversationManager != null)
			{
				conversationManager._currentRepeatedDialogSetIndex++;
				if (conversationManager._currentRepeatedDialogSetIndex >= conversationManager._dialogRepeatObjects.Count)
				{
					conversationManager._currentRepeatedDialogSetIndex = 0;
				}
				conversationManager._currentRepeatIndex = 0;
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x00099284 File Offset: 0x00097484
		internal static bool IsThereMultipleRepeatablePages()
		{
			Campaign campaign = Campaign.Current;
			if (campaign == null)
			{
				return false;
			}
			ConversationManager conversationManager = campaign.ConversationManager;
			int? num = ((conversationManager != null) ? new int?(conversationManager._dialogRepeatObjects.Count) : null);
			int num2 = 1;
			return (num.GetValueOrDefault() > num2) & (num != null);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000992D4 File Offset: 0x000974D4
		private void ResetRepeatedDialogSystem()
		{
			this._currentRepeatedDialogSetIndex = 0;
			this._currentRepeatIndex = 0;
			this._dialogRepeatObjects.Clear();
			this._dialogRepeatLines.Clear();
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000992FA File Offset: 0x000974FA
		internal ConversationSentence AddDialogLine(ConversationSentence dialogLine)
		{
			this._sentences.Add(dialogLine);
			if (!this._sortSentenceIsDisabled)
			{
				this.SortLastSentence();
			}
			return dialogLine;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00099318 File Offset: 0x00097518
		public void AddDialogFlow(DialogFlow dialogFlow, object relatedObject = null)
		{
			foreach (DialogFlowLine dialogFlowLine in dialogFlow.Lines)
			{
				string text = this.CreateId();
				uint num = (dialogFlowLine.ByPlayer ? 1U : 0U) | (dialogFlowLine.IsRepeatable ? 2U : 0U) | (dialogFlowLine.IsSpecialOption ? 4U : 0U) | (dialogFlowLine.IsUsedOnce ? 8U : 0U);
				this.AddDialogLine(new ConversationSentence(text, dialogFlowLine.HasVariation ? new TextObject("{=!}{VARIATION_TEXT_TAGGED_LINE}", null) : dialogFlowLine.Text, dialogFlowLine.InputToken, dialogFlowLine.OutputToken, dialogFlowLine.ConditionDelegate, dialogFlowLine.ClickableConditionDelegate, dialogFlowLine.ConsequenceDelegate, num, dialogFlow.Priority, 0, 0, relatedObject, dialogFlowLine.HasVariation, dialogFlowLine.SpeakerDelegate, dialogFlowLine.ListenerDelegate, null));
				GameText gameText = Game.Current.GameTextManager.AddGameText(text);
				foreach (KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>> keyValuePair in dialogFlowLine.Variations)
				{
					gameText.AddVariationWithId("", keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00099490 File Offset: 0x00097690
		public ConversationSentence AddDialogLineMultiAgent(string id, string inputToken, string outputToken, TextObject text, ConversationSentence.OnConditionDelegate conditionDelegate, ConversationSentence.OnConsequenceDelegate consequenceDelegate, int agentIndex, int nextAgentIndex, int priority = 100, ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = null)
		{
			return this.AddDialogLine(new ConversationSentence(id, text, inputToken, outputToken, conditionDelegate, clickableConditionDelegate, consequenceDelegate, 0U, priority, agentIndex, nextAgentIndex, null, false, null, null, null));
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000994BF File Offset: 0x000976BF
		internal string CreateToken()
		{
			string text = string.Format("atk:{0}", this._autoToken);
			this._autoToken++;
			return text;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000994E4 File Offset: 0x000976E4
		private string CreateId()
		{
			string text = string.Format("adg:{0}", this._autoId);
			this._autoId++;
			return text;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00099509 File Offset: 0x00097709
		internal void SetupGameStringsForConversation()
		{
			StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, null, false);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00099522 File Offset: 0x00097722
		internal void OnConsequence(ConversationSentence sentence)
		{
			Action<ConversationSentence> consequenceRunned = this.ConsequenceRunned;
			if (consequenceRunned == null)
			{
				return;
			}
			consequenceRunned(sentence);
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00099535 File Offset: 0x00097735
		internal void OnCondition(ConversationSentence sentence)
		{
			Action<ConversationSentence> conditionRunned = this.ConditionRunned;
			if (conditionRunned == null)
			{
				return;
			}
			conditionRunned(sentence);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00099548 File Offset: 0x00097748
		internal void OnClickableCondition(ConversationSentence sentence)
		{
			Action<ConversationSentence> clickableConditionRunned = this.ClickableConditionRunned;
			if (clickableConditionRunned == null)
			{
				return;
			}
			clickableConditionRunned(sentence);
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600227C RID: 8828 RVA: 0x0009955C File Offset: 0x0009775C
		// (remove) Token: 0x0600227D RID: 8829 RVA: 0x00099594 File Offset: 0x00097794
		public event Action<ConversationSentence> ConsequenceRunned;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600227E RID: 8830 RVA: 0x000995CC File Offset: 0x000977CC
		// (remove) Token: 0x0600227F RID: 8831 RVA: 0x00099604 File Offset: 0x00097804
		public event Action<ConversationSentence> ConditionRunned;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06002280 RID: 8832 RVA: 0x0009963C File Offset: 0x0009783C
		// (remove) Token: 0x06002281 RID: 8833 RVA: 0x00099674 File Offset: 0x00097874
		public event Action<ConversationSentence> ClickableConditionRunned;

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x000996A9 File Offset: 0x000978A9
		public IReadOnlyList<IAgent> ConversationAgents
		{
			get
			{
				return this._conversationAgents;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002283 RID: 8835 RVA: 0x000996B1 File Offset: 0x000978B1
		public IAgent OneToOneConversationAgent
		{
			get
			{
				if (this.ConversationAgents.IsEmpty<IAgent>() || this.ConversationAgents.Count > 1)
				{
					return null;
				}
				return this.ConversationAgents[0];
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x000996DC File Offset: 0x000978DC
		public IAgent SpeakerAgent
		{
			get
			{
				if (this.ConversationAgents != null)
				{
					return this._speakerAgent;
				}
				return null;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x000996EE File Offset: 0x000978EE
		public IAgent ListenerAgent
		{
			get
			{
				if (this.ConversationAgents != null)
				{
					return this._listenerAgent;
				}
				return null;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x00099700 File Offset: 0x00097900
		// (set) Token: 0x06002287 RID: 8839 RVA: 0x00099708 File Offset: 0x00097908
		public bool IsConversationInProgress { get; private set; }

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002288 RID: 8840 RVA: 0x00099711 File Offset: 0x00097911
		public Hero OneToOneConversationHero
		{
			get
			{
				if (this.OneToOneConversationCharacter != null)
				{
					return this.OneToOneConversationCharacter.HeroObject;
				}
				return null;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x00099728 File Offset: 0x00097928
		public CharacterObject OneToOneConversationCharacter
		{
			get
			{
				if (this.OneToOneConversationAgent != null)
				{
					return (CharacterObject)this.OneToOneConversationAgent.Character;
				}
				return null;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600228A RID: 8842 RVA: 0x00099744 File Offset: 0x00097944
		public IEnumerable<CharacterObject> ConversationCharacters
		{
			get
			{
				new List<CharacterObject>();
				foreach (IAgent agent in this.ConversationAgents)
				{
					yield return (CharacterObject)agent.Character;
				}
				IEnumerator<IAgent> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00099754 File Offset: 0x00097954
		public bool IsAgentInConversation(IAgent agent)
		{
			return this.ConversationAgents.Contains(agent);
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x00099762 File Offset: 0x00097962
		public MobileParty ConversationParty
		{
			get
			{
				return this._conversationParty;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x0009976A File Offset: 0x0009796A
		// (set) Token: 0x0600228E RID: 8846 RVA: 0x00099772 File Offset: 0x00097972
		public bool NeedsToActivateForMapConversation { get; private set; }

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600228F RID: 8847 RVA: 0x0009977C File Offset: 0x0009797C
		// (remove) Token: 0x06002290 RID: 8848 RVA: 0x000997B4 File Offset: 0x000979B4
		public event Action ConversationSetup;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06002291 RID: 8849 RVA: 0x000997EC File Offset: 0x000979EC
		// (remove) Token: 0x06002292 RID: 8850 RVA: 0x00099824 File Offset: 0x00097A24
		public event Action ConversationBegin;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06002293 RID: 8851 RVA: 0x0009985C File Offset: 0x00097A5C
		// (remove) Token: 0x06002294 RID: 8852 RVA: 0x00099894 File Offset: 0x00097A94
		public event Action ConversationEnd;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06002295 RID: 8853 RVA: 0x000998CC File Offset: 0x00097ACC
		// (remove) Token: 0x06002296 RID: 8854 RVA: 0x00099904 File Offset: 0x00097B04
		public event Action ConversationEndOneShot;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06002297 RID: 8855 RVA: 0x0009993C File Offset: 0x00097B3C
		// (remove) Token: 0x06002298 RID: 8856 RVA: 0x00099974 File Offset: 0x00097B74
		public event Action ConversationContinued;

		// Token: 0x06002299 RID: 8857 RVA: 0x000999A9 File Offset: 0x00097BA9
		private void SetupConversation()
		{
			this.IsConversationInProgress = true;
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationInstall();
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000999C2 File Offset: 0x00097BC2
		public void BeginConversation()
		{
			this.IsConversationInProgress = true;
			if (this.ConversationSetup != null)
			{
				this.ConversationSetup();
			}
			if (this.ConversationBegin != null)
			{
				this.ConversationBegin();
			}
			this.NeedsToActivateForMapConversation = false;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x000999F8 File Offset: 0x00097BF8
		public void EndConversation()
		{
			Debug.Print("--------------- Conversation End --------------- ", 0, Debug.DebugColor.White, 4503599627370496UL);
			if (CampaignMission.Current != null)
			{
				foreach (IAgent agent in this.ConversationAgents)
				{
					CampaignMission.Current.OnConversationEnd(agent);
				}
			}
			this._conversationParty = null;
			if (this.ConversationEndOneShot != null)
			{
				this.ConversationEndOneShot();
				this.ConversationEndOneShot = null;
			}
			if (this.ConversationEnd != null)
			{
				this.ConversationEnd();
			}
			this.IsConversationInProgress = false;
			foreach (IAgent agent2 in this.ConversationAgents)
			{
				agent2.SetAsConversationAgent(false);
			}
			Campaign.Current.CurrentConversationContext = ConversationContext.Default;
			CampaignEventDispatcher.Instance.OnConversationEnded(this.ConversationCharacters);
			if (ConversationManager.GetPersuasionIsActive())
			{
				ConversationManager.EndPersuasion();
			}
			this._conversationAgents.Clear();
			this._speakerAgent = null;
			this._listenerAgent = null;
			this._mainAgent = null;
			if (this.IsConversationFlowActive)
			{
				this.OnConversationDeactivate();
			}
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationUninstall();
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00099B40 File Offset: 0x00097D40
		public void DoOption(int optionIndex)
		{
			this.LastSelectedButtonIndex = optionIndex;
			this.ProcessSentence(this.CurOptions[optionIndex]);
			if (this._isActive)
			{
				this.DoOptionContinue();
				return;
			}
			this._executeDoOptionContinue = true;
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x00099B74 File Offset: 0x00097D74
		public void DoOption(string optionID)
		{
			int count = Campaign.Current.ConversationManager.CurOptions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.CurOptions[i].Id == optionID)
				{
					this.DoOption(i);
					return;
				}
			}
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00099BC3 File Offset: 0x00097DC3
		public void DoConversationContinuedCallback()
		{
			if (this.ConversationContinued != null)
			{
				this.ConversationContinued();
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00099BD8 File Offset: 0x00097DD8
		public void DoOptionContinue()
		{
			if (this.IsConversationEnded() && this._sentences[this._currentSentence].IsPlayer)
			{
				this.EndConversation();
				return;
			}
			this.ProcessPartnerSentence();
			this.DoConversationContinuedCallback();
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x00099C10 File Offset: 0x00097E10
		public void ContinueConversation()
		{
			if (this.CurOptions.Count <= 1)
			{
				if (this.IsConversationEnded())
				{
					this.EndConversation();
					return;
				}
				if (!this.ProcessPartnerSentence() && this.ListenerAgent.Character == Hero.MainHero.CharacterObject)
				{
					this.EndConversation();
					return;
				}
				this.DoConversationContinuedCallback();
				if (CampaignMission.Current != null)
				{
					CampaignMission.Current.OnConversationContinue();
				}
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x00099C78 File Offset: 0x00097E78
		public void SetupAndStartMissionConversation(IAgent agent, IAgent mainAgent, bool setActionsInstantly)
		{
			this.SetupConversation();
			this._mainAgent = mainAgent;
			this._conversationAgents.Clear();
			this.AddConversationAgent(agent);
			this._conversationParty = null;
			this.StartNew(0, setActionsInstantly);
			if (!this.IsConversationFlowActive)
			{
				this.OnConversationActivate();
			}
			this.BeginConversation();
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x00099CC8 File Offset: 0x00097EC8
		public void SetupAndStartMissionConversationWithMultipleAgents(IEnumerable<IAgent> agents, IAgent mainAgent)
		{
			this.SetupConversation();
			this._mainAgent = mainAgent;
			this._conversationAgents.Clear();
			this.AddConversationAgents(agents, true);
			this._conversationParty = null;
			this.StartNew(0, true);
			if (!this.IsConversationFlowActive)
			{
				this.OnConversationActivate();
			}
			this.BeginConversation();
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x00099D18 File Offset: 0x00097F18
		public void SetupAndStartMapConversation(MobileParty party, IAgent agent, IAgent mainAgent)
		{
			this._conversationParty = party;
			this._mainAgent = mainAgent;
			this._conversationAgents.Clear();
			this.AddConversationAgent(agent);
			this.SetupConversation();
			this.StartNew(0, true);
			this.NeedsToActivateForMapConversation = true;
			if (!this.IsConversationFlowActive)
			{
				this.OnConversationActivate();
			}
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x00099D68 File Offset: 0x00097F68
		public void AddConversationAgents(IEnumerable<IAgent> agents, bool setActionsInstantly)
		{
			foreach (IAgent agent in agents)
			{
				if (agent.IsActive() && !this.ConversationAgents.Contains(agent))
				{
					this.AddConversationAgent(agent);
					CampaignMission.Current.OnConversationStart(agent, setActionsInstantly);
				}
			}
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x00099DD4 File Offset: 0x00097FD4
		public void RemoveConversationAgent(IAgent agent)
		{
			if (agent.IsActive() && this.ConversationAgents.Contains(agent) && this.ConversationAgents.Count > 1)
			{
				CampaignMission.Current.OnConversationEnd(agent);
				agent.SetAsConversationAgent(false);
				this._conversationAgents.Remove(agent);
				return;
			}
			Debug.FailedAssert("Failed to remove conversation agent.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Conversation\\ConversationManager.cs", "RemoveConversationAgent", 1249);
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x00099E3E File Offset: 0x0009803E
		private void AddConversationAgent(IAgent agent)
		{
			this._conversationAgents.Add(agent);
			agent.SetAsConversationAgent(true);
			CampaignEventDispatcher.Instance.OnAgentJoinedConversation(agent);
			agent.OnConversationStarted();
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x00099E64 File Offset: 0x00098064
		public bool IsConversationAgent(IAgent agent)
		{
			return this.ConversationAgents != null && this.ConversationAgents.Contains(agent);
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x00099E7C File Offset: 0x0009807C
		public void RemoveRelatedLines(object o)
		{
			this._sentences.RemoveAll((ConversationSentence s) => s.RelatedObject == o);
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x00099EAE File Offset: 0x000980AE
		// (set) Token: 0x060022AA RID: 8874 RVA: 0x00099EB6 File Offset: 0x000980B6
		public IConversationStateHandler Handler { get; set; }

		// Token: 0x060022AB RID: 8875 RVA: 0x00099EBF File Offset: 0x000980BF
		public void OnConversationDeactivate()
		{
			this._isActive = false;
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationDeactivate();
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x00099ED8 File Offset: 0x000980D8
		public void OnConversationActivate()
		{
			this._isActive = true;
			if (this._executeDoOptionContinue)
			{
				this._executeDoOptionContinue = false;
				this.DoOptionContinue();
			}
			IConversationStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConversationActivate();
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x00099F08 File Offset: 0x00098108
		public TextObject FindMatchingTextOrNull(string id, CharacterObject character)
		{
			float num = -2.1474836E+09f;
			TextObject textObject = null;
			GameText gameText = Game.Current.GameTextManager.GetGameText(id);
			if (gameText != null)
			{
				foreach (GameText.GameTextVariation gameTextVariation in gameText.Variations)
				{
					float num2 = this.FindMatchingScore(character, gameTextVariation.Tags);
					if (num2 > num)
					{
						textObject = gameTextVariation.Text;
						num = num2;
					}
				}
			}
			return textObject;
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x00099F8C File Offset: 0x0009818C
		private float FindMatchingScore(CharacterObject character, GameTextManager.ChoiceTag[] choiceTags)
		{
			float num = 0f;
			foreach (GameTextManager.ChoiceTag choiceTag in choiceTags)
			{
				if (choiceTag.TagName != "DefaultTag")
				{
					if (this.IsTagApplicable(choiceTag.TagName, character) == choiceTag.IsTagReversed)
					{
						return -2.1474836E+09f;
					}
					uint weight = choiceTag.Weight;
					num += weight;
				}
			}
			return num;
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x00099FF8 File Offset: 0x000981F8
		private void InitializeTags()
		{
			this._tags = new Dictionary<string, ConversationTag>();
			string name = typeof(ConversationTag).Assembly.GetName().Name;
			foreach (Assembly assembly in ModuleHelper.GetActiveGameAssemblies())
			{
				bool flag = false;
				if (name == assembly.GetName().Name)
				{
					flag = true;
				}
				else
				{
					AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
					for (int i = 0; i < referencedAssemblies.Length; i++)
					{
						if (referencedAssemblies[i].Name == name)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					foreach (Type type in assembly.GetTypesSafe(null))
					{
						if (type.IsSubclassOf(typeof(ConversationTag)))
						{
							ConversationTag conversationTag = Activator.CreateInstance(type) as ConversationTag;
							this._tags.Add(conversationTag.StringId, conversationTag);
						}
					}
				}
			}
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0009A130 File Offset: 0x00098330
		private static void SetupTextVariables()
		{
			StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, null, false);
			int num = 1;
			foreach (CharacterObject characterObject in CharacterObject.ConversationCharacters)
			{
				string text = ((num == 1) ? "" : ("_" + num));
				StringHelpers.SetCharacterProperties("CONVERSATION_CHARACTER" + text, characterObject, null, false);
			}
			MBTextManager.SetTextVariable("CURRENT_SETTLEMENT_NAME", (Settlement.CurrentSettlement == null) ? TextObject.GetEmpty() : Settlement.CurrentSettlement.Name, false);
			ConversationHelper.ConversationTroopCommentShown = false;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x0009A1E8 File Offset: 0x000983E8
		public IEnumerable<string> GetApplicableTagNames(CharacterObject character)
		{
			foreach (ConversationTag conversationTag in this._tags.Values)
			{
				if (conversationTag.IsApplicableTo(character))
				{
					yield return conversationTag.StringId;
				}
			}
			Dictionary<string, ConversationTag>.ValueCollection.Enumerator enumerator = default(Dictionary<string, ConversationTag>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x0009A200 File Offset: 0x00098400
		public bool IsTagApplicable(string tagId, CharacterObject character)
		{
			ConversationTag conversationTag;
			if (this._tags.TryGetValue(tagId, out conversationTag))
			{
				return conversationTag.IsApplicableTo(character);
			}
			Debug.FailedAssert("Asking for a nonexistent tag: " + tagId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Conversation\\ConversationManager.cs", "IsTagApplicable", 1486);
			return false;
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x0009A248 File Offset: 0x00098448
		public void OpenMapConversation(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			GameStateManager gameStateManager = GameStateManager.Current;
			(((gameStateManager != null) ? gameStateManager.ActiveState : null) as MapState).OnMapConversationStarts(playerCharacterData, conversationPartnerData);
			PartyBase party = conversationPartnerData.Party;
			this.SetupAndStartMapConversation((party != null) ? party.MobileParty : null, new MapConversationAgent(conversationPartnerData.Character), new MapConversationAgent(CharacterObject.PlayerCharacter));
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0009A29F File Offset: 0x0009849F
		public static void StartPersuasion(float goalValue, float successValue, float failValue, float criticalSuccessValue, float criticalFailValue, float initialProgress = -1f, PersuasionDifficulty difficulty = PersuasionDifficulty.Medium)
		{
			ConversationManager._persuasion = new Persuasion(goalValue, successValue, failValue, criticalSuccessValue, criticalFailValue, initialProgress, difficulty);
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0009A2B5 File Offset: 0x000984B5
		public static void EndPersuasion()
		{
			ConversationManager._persuasion = null;
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0009A2BD File Offset: 0x000984BD
		public static void PersuasionCommitProgress(PersuasionOptionArgs persuasionOptionArgs)
		{
			ConversationManager._persuasion.CommitProgress(persuasionOptionArgs);
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x0009A2CA File Offset: 0x000984CA
		public static void Clear()
		{
			ConversationManager._persuasion = null;
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x0009A2D2 File Offset: 0x000984D2
		public void GetPersuasionChanceValues(out float successValue, out float critSuccessValue, out float critFailValue)
		{
			successValue = ConversationManager._persuasion.SuccessValue;
			critSuccessValue = ConversationManager._persuasion.CriticalSuccessValue;
			critFailValue = ConversationManager._persuasion.CriticalFailValue;
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0009A2F8 File Offset: 0x000984F8
		public static bool GetPersuasionIsActive()
		{
			return ConversationManager._persuasion != null;
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x0009A302 File Offset: 0x00098502
		public static bool GetPersuasionProgressSatisfied()
		{
			return ConversationManager._persuasion.Progress >= ConversationManager._persuasion.GoalValue;
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0009A31D File Offset: 0x0009851D
		public static bool GetPersuasionIsFailure()
		{
			return ConversationManager._persuasion.Progress < 0f;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x0009A330 File Offset: 0x00098530
		public static float GetPersuasionProgress()
		{
			return ConversationManager._persuasion.Progress;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x0009A33C File Offset: 0x0009853C
		public static float GetPersuasionGoalValue()
		{
			return ConversationManager._persuasion.GoalValue;
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0009A348 File Offset: 0x00098548
		public static IEnumerable<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> GetPersuasionChosenOptions()
		{
			return ConversationManager._persuasion.GetChosenOptions();
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x0009A354 File Offset: 0x00098554
		public void GetPersuasionChances(ConversationSentenceOption conversationSentenceOption, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance)
		{
			ConversationSentence conversationSentence = this._sentences[conversationSentenceOption.SentenceNo];
			if (conversationSentenceOption.HasPersuasion)
			{
				Campaign.Current.Models.PersuasionModel.GetChances(conversationSentence.PersuationOptionArgs, out successChance, out critSuccessChance, out critFailChance, out failChance, ConversationManager._persuasion.DifficultyMultiplier);
				return;
			}
			successChance = 0f;
			critSuccessChance = 0f;
			critFailChance = 0f;
			failChance = 0f;
		}

		// Token: 0x04000A22 RID: 2594
		private int _currentRepeatedDialogSetIndex;

		// Token: 0x04000A23 RID: 2595
		private int _currentRepeatIndex;

		// Token: 0x04000A24 RID: 2596
		private int _autoId;

		// Token: 0x04000A25 RID: 2597
		private int _autoToken;

		// Token: 0x04000A26 RID: 2598
		private HashSet<int> _usedIndices = new HashSet<int>();

		// Token: 0x04000A27 RID: 2599
		private int _numConversationSentencesCreated;

		// Token: 0x04000A28 RID: 2600
		private List<ConversationSentence> _sentences;

		// Token: 0x04000A29 RID: 2601
		private int _numberOfStateIndices;

		// Token: 0x04000A2A RID: 2602
		public int ActiveToken;

		// Token: 0x04000A2B RID: 2603
		private int _currentSentence;

		// Token: 0x04000A2C RID: 2604
		private TextObject _currentSentenceText;

		// Token: 0x04000A2D RID: 2605
		public List<Tuple<string, CharacterObject>> DetailedDebugLog = new List<Tuple<string, CharacterObject>>();

		// Token: 0x04000A2E RID: 2606
		public string CurrentFaceAnimationRecord;

		// Token: 0x04000A2F RID: 2607
		private object _lastSelectedDialogObject;

		// Token: 0x04000A30 RID: 2608
		private readonly List<List<object>> _dialogRepeatObjects = new List<List<object>>();

		// Token: 0x04000A31 RID: 2609
		private readonly List<TextObject> _dialogRepeatLines = new List<TextObject>();

		// Token: 0x04000A32 RID: 2610
		private bool _isActive;

		// Token: 0x04000A33 RID: 2611
		private bool _executeDoOptionContinue;

		// Token: 0x04000A34 RID: 2612
		public int LastSelectedButtonIndex;

		// Token: 0x04000A35 RID: 2613
		public ConversationAnimationManager ConversationAnimationManager;

		// Token: 0x04000A36 RID: 2614
		private IAgent _mainAgent;

		// Token: 0x04000A37 RID: 2615
		private IAgent _speakerAgent;

		// Token: 0x04000A38 RID: 2616
		private IAgent _listenerAgent;

		// Token: 0x04000A39 RID: 2617
		private Dictionary<string, ConversationTag> _tags;

		// Token: 0x04000A3A RID: 2618
		private bool _sortSentenceIsDisabled;

		// Token: 0x04000A3B RID: 2619
		private Dictionary<string, int> stateMap;

		// Token: 0x04000A40 RID: 2624
		private List<IAgent> _conversationAgents = new List<IAgent>();

		// Token: 0x04000A42 RID: 2626
		public bool CurrentConversationIsFirst;

		// Token: 0x04000A43 RID: 2627
		private MobileParty _conversationParty;

		// Token: 0x04000A4B RID: 2635
		private static Persuasion _persuasion;

		// Token: 0x02000645 RID: 1605
		public class TaggedString
		{
			// Token: 0x040019B4 RID: 6580
			public TextObject Text;

			// Token: 0x040019B5 RID: 6581
			public List<GameTextManager.ChoiceTag> ChoiceTags = new List<GameTextManager.ChoiceTag>();

			// Token: 0x040019B6 RID: 6582
			public int FacialAnimation;
		}
	}
}
