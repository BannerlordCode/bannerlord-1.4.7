using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Library;

namespace SandBox.View.Conversation
{
	// Token: 0x0200007A RID: 122
	public class ConversationViewManager
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00027CAF File Offset: 0x00025EAF
		public static ConversationViewManager Instance
		{
			get
			{
				return SandBoxViewSubModule.ConversationViewManager;
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00027CB8 File Offset: 0x00025EB8
		public ConversationViewManager()
		{
			this.FillEventHandlers();
			Campaign.Current.ConversationManager.ConditionRunned += this.OnCondition;
			Campaign.Current.ConversationManager.ConsequenceRunned += this.OnConsequence;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00027D08 File Offset: 0x00025F08
		private void FillEventHandlers()
		{
			this._conditionEventHandlers = new Dictionary<string, ConversationViewEventHandlerDelegate>();
			this._consequenceEventHandlers = new Dictionary<string, ConversationViewEventHandlerDelegate>();
			Assembly assembly = typeof(ConversationViewEventHandlerDelegate).Assembly;
			this.FillEventHandlersWith(assembly);
			foreach (Assembly assembly2 in assembly.GetReferencingAssembliesSafe(null))
			{
				this.FillEventHandlersWith(assembly2);
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00027D64 File Offset: 0x00025F64
		private void FillEventHandlersWith(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypesSafe(null))
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					object[] customAttributesSafe = methodInfo.GetCustomAttributesSafe(typeof(ConversationViewEventHandler), false);
					if (customAttributesSafe != null && customAttributesSafe.Length != 0)
					{
						foreach (ConversationViewEventHandler conversationViewEventHandler in customAttributesSafe)
						{
							ConversationViewEventHandlerDelegate conversationViewEventHandlerDelegate = Delegate.CreateDelegate(typeof(ConversationViewEventHandlerDelegate), methodInfo) as ConversationViewEventHandlerDelegate;
							if (conversationViewEventHandler.Type == ConversationViewEventHandler.EventType.OnCondition)
							{
								if (!this._conditionEventHandlers.ContainsKey(conversationViewEventHandler.Id))
								{
									this._conditionEventHandlers.Add(conversationViewEventHandler.Id, conversationViewEventHandlerDelegate);
								}
								else
								{
									this._conditionEventHandlers[conversationViewEventHandler.Id] = conversationViewEventHandlerDelegate;
								}
							}
							else if (conversationViewEventHandler.Type == ConversationViewEventHandler.EventType.OnConsequence)
							{
								if (!this._consequenceEventHandlers.ContainsKey(conversationViewEventHandler.Id))
								{
									this._consequenceEventHandlers.Add(conversationViewEventHandler.Id, conversationViewEventHandlerDelegate);
								}
								else
								{
									this._consequenceEventHandlers[conversationViewEventHandler.Id] = conversationViewEventHandlerDelegate;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00027ED8 File Offset: 0x000260D8
		private void OnConsequence(ConversationSentence sentence)
		{
			ConversationViewEventHandlerDelegate conversationViewEventHandlerDelegate;
			if (this._consequenceEventHandlers.TryGetValue(sentence.Id, out conversationViewEventHandlerDelegate))
			{
				conversationViewEventHandlerDelegate();
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00027F00 File Offset: 0x00026100
		private void OnCondition(ConversationSentence sentence)
		{
			ConversationViewEventHandlerDelegate conversationViewEventHandlerDelegate;
			if (this._conditionEventHandlers.TryGetValue(sentence.Id, out conversationViewEventHandlerDelegate))
			{
				conversationViewEventHandlerDelegate();
			}
		}

		// Token: 0x0400026E RID: 622
		private Dictionary<string, ConversationViewEventHandlerDelegate> _conditionEventHandlers;

		// Token: 0x0400026F RID: 623
		private Dictionary<string, ConversationViewEventHandlerDelegate> _consequenceEventHandlers;
	}
}
