using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer
{
	// Token: 0x0200003B RID: 59
	public class MPChatVM : ViewModel, IChatHandler
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00013AAA File Offset: 0x00011CAA
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x00013AB4 File Offset: 0x00011CB4
		public ChatChannelType ActiveChannelType
		{
			get
			{
				return this._activeChannelType;
			}
			set
			{
				if ((value == ChatChannelType.All || value == ChatChannelType.Team) && !GameNetwork.IsClient && NetworkMain.GameClient == null && NetworkMain.CommunityClient == null)
				{
					this._activeChannelType = ChatChannelType.None;
				}
				else if (value != this._activeChannelType)
				{
					this._activeChannelType = value;
					this.RefreshActiveChannelNameData();
				}
				this.IsChatDisabled = value == ChatChannelType.None;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00013B08 File Offset: 0x00011D08
		private string _playerName
		{
			get
			{
				string text = ((NetworkMain.GameClient.PlayerData != null) ? NetworkMain.GameClient.Name : new TextObject("{=!}ERROR: MISSING PLAYERDATA", null).ToString());
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
				if (missionPeer != null && !missionPeer.IsAgentAliveForChatting)
				{
					GameTexts.SetVariable("PLAYER_NAME", "{=!}" + text);
					text = GameTexts.FindText("str_chat_message_dead_player", null).ToString();
				}
				return text;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00013B88 File Offset: 0x00011D88
		public MPChatVM()
		{
			this._allMessages = new List<MPChatLineVM>();
			this._requestedMessages = new Queue<MPChatLineVM>();
			this.MessageHistory = new MBBindingList<MPChatLineVM>();
			this.CombatLogHint = new HintViewModel();
			this.IncludeCombatLog = BannerlordConfig.ReportDamage;
			this.IncludeBark = BannerlordConfig.ReportBark;
			InformationManager.DisplayMessageInternal += this.OnDisplayMessageReceived;
			InformationManager.ClearAllMessagesInternal += this.ClearAllMessages;
			InformationManager.HideAllMessagesInternal += this.HideAllMessages;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
			this.MaxMessageLength = 100;
			this._recentlySentMessagesTimes = new List<float>();
			this.RefreshValues();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00013CA8 File Offset: 0x00011EA8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CombatLogHint.HintText = new TextObject("{=FRSGOfUJ}Toggle include Combat Log", null);
			this.ToggleCombatLogText = new TextObject("{=rx18kyZb}Combat Log", null).ToString();
			this.ToggleBarkText = new TextObject("{=NuMQvQxg}Shouts", null).ToString();
			this.UpdateHideShowText(this._isInspectingMessages);
			this.UpdateShortcutTexts();
			this.RefreshActiveChannelNameData();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00013D18 File Offset: 0x00011F18
		private void RefreshActiveChannelNameData()
		{
			if (this.ActiveChannelType == ChatChannelType.None)
			{
				this.ActiveChannelNameText = string.Empty;
				this.ActiveChannelColor = Color.White;
				return;
			}
			string text = GameTexts.FindText("str_multiplayer_chat_channel", this.ActiveChannelType.ToString()).ToString();
			GameTexts.SetVariable("STR", text);
			this.ActiveChannelNameText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			this.ActiveChannelColor = this.GetChannelColor(this.ActiveChannelType);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00013D9C File Offset: 0x00011F9C
		private void OnOptionChange(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportDamage)
			{
				this.IncludeCombatLog = BannerlordConfig.ReportDamage;
				return;
			}
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportBark)
			{
				this.IncludeBark = BannerlordConfig.ReportBark;
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00013DBF File Offset: 0x00011FBF
		public void ToggleIncludeCombatLog()
		{
			this.IncludeCombatLog = !this.IncludeCombatLog;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00013DD0 File Offset: 0x00011FD0
		public void ExecuteToggleIncludeShouts()
		{
			this.IncludeBark = !this.IncludeBark;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00013DE4 File Offset: 0x00011FE4
		private void UpdateHideShowText(bool isInspecting)
		{
			TextObject textObject;
			if (this._game != null && isInspecting)
			{
				textObject = this._hideText;
				textObject.SetTextVariable("KEY", this._getToggleChatKeyText() ?? TextObject.GetEmpty());
			}
			else
			{
				textObject = TextObject.GetEmpty();
			}
			this.HideShowText = textObject.ToString();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00013E3C File Offset: 0x0001203C
		private void UpdateShortcutTexts()
		{
			TextObject cycleChannelsText = this._cycleChannelsText;
			string text = "KEY";
			Func<TextObject> getCycleChannelsKeyText = this._getCycleChannelsKeyText;
			cycleChannelsText.SetTextVariable(text, ((getCycleChannelsKeyText != null) ? getCycleChannelsKeyText() : null) ?? TextObject.GetEmpty());
			this.CycleThroughChannelsText = this._cycleChannelsText.ToString();
			if (Input.IsGamepadActive)
			{
				TextObject sendMessageTextObject = this._sendMessageTextObject;
				string text2 = "KEY";
				Func<TextObject> getSendMessageKeyText = this._getSendMessageKeyText;
				sendMessageTextObject.SetTextVariable(text2, ((getSendMessageKeyText != null) ? getSendMessageKeyText() : null) ?? TextObject.GetEmpty());
				this.SendMessageText = this._sendMessageTextObject.ToString();
				TextObject cancelSendingTextObject = this._cancelSendingTextObject;
				string text3 = "KEY";
				Func<TextObject> getCancelSendingKeyText = this._getCancelSendingKeyText;
				cancelSendingTextObject.SetTextVariable(text3, ((getCancelSendingKeyText != null) ? getCancelSendingKeyText() : null) ?? TextObject.GetEmpty());
				this.CancelSendingText = this._cancelSendingTextObject.ToString();
				return;
			}
			this.SendMessageText = string.Empty;
			this.CancelSendingText = string.Empty;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00013F20 File Offset: 0x00012120
		public void Tick(float dt)
		{
			while (this._requestedMessages.Count > 0)
			{
				this.AddChatLine(this._requestedMessages.Dequeue());
			}
			float applicationTime = Time.ApplicationTime;
			for (int i = 0; i < this._recentlySentMessagesTimes.Count; i++)
			{
				if (applicationTime - this._recentlySentMessagesTimes[i] >= 15f)
				{
					this._recentlySentMessagesTimes.RemoveAt(i);
				}
			}
			this.CheckChatFading(dt);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00013F94 File Offset: 0x00012194
		public void Hide()
		{
			this._allMessages.ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
			this.MessageHistory.ToList<MPChatLineVM>().ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00013FFC File Offset: 0x000121FC
		public void Clear()
		{
			this._allMessages.ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
			this.MessageHistory.ToList<MPChatLineVM>().ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
			this._allMessages.Clear();
			this.MessageHistory.Clear();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00014078 File Offset: 0x00012278
		private void OnDisplayMessageReceived(InformationMessage informationMessage)
		{
			if (this.IsChatAllowedByOptions())
			{
				this.HandleAddChatLineRequest(informationMessage);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00014089 File Offset: 0x00012289
		private void ClearAllMessages()
		{
			this.Clear();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00014091 File Offset: 0x00012291
		private void HideAllMessages()
		{
			this.Hide();
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001409C File Offset: 0x0001229C
		public void UpdateObjects(Game game, Mission mission)
		{
			if (this._game != game)
			{
				if (this._game != null)
				{
					this.ClearGame();
				}
				this._game = game;
				if (this._game != null)
				{
					this.SetGame();
				}
			}
			if (this._mission != mission)
			{
				if (this._mission != null)
				{
					this.ClearMission();
				}
				this._mission = mission;
				if (this._mission != null)
				{
					this.SetMission();
				}
			}
			if (this._game != null)
			{
				ChatBox gameHandler = this._game.GetGameHandler<ChatBox>();
				if (this._chatBox != gameHandler)
				{
					if (this._chatBox != null)
					{
						this.ClearChatBox();
					}
					this._chatBox = gameHandler;
					if (this._chatBox != null)
					{
						this.SetChatBox();
					}
				}
			}
			this.IsOptionsAvailable = this.IsInspectingMessages && this.IsTypingText;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00014158 File Offset: 0x00012358
		private void ClearGame()
		{
			this._game = null;
			this.ActiveChannelType = ChatChannelType.None;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00014168 File Offset: 0x00012368
		private void ClearChatBox()
		{
			if (this._chatBox != null)
			{
				this._chatBox.PlayerMessageReceived -= this.OnPlayerMessageReceived;
				this._chatBox.WhisperMessageSent -= this.OnWhisperMessageSent;
				this._chatBox.WhisperMessageReceived -= this.OnWhisperMessageReceived;
				this._chatBox.ErrorWhisperMessageReceived -= this.OnErrorWhisperMessageReceived;
				this._chatBox.ServerMessage -= this.OnServerMessage;
				this._chatBox.ServerAdminMessage -= this.OnServerAdminMessage;
				this._chatBox = null;
				this.ActiveChannelType = ChatChannelType.None;
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00014218 File Offset: 0x00012418
		private void SetGame()
		{
			this.UpdateHideShowText(this.IsInspectingMessages);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00014228 File Offset: 0x00012428
		private void SetChatBox()
		{
			this._chatBox.PlayerMessageReceived += this.OnPlayerMessageReceived;
			this._chatBox.WhisperMessageSent += this.OnWhisperMessageSent;
			this._chatBox.WhisperMessageReceived += this.OnWhisperMessageReceived;
			this._chatBox.ErrorWhisperMessageReceived += this.OnErrorWhisperMessageReceived;
			this._chatBox.ServerMessage += this.OnServerMessage;
			this._chatBox.ServerAdminMessage += this.OnServerAdminMessage;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000142C0 File Offset: 0x000124C0
		private void SetMission()
		{
			this.ActiveChannelType = ChatChannelType.All;
			Game game = Game.Current;
			bool flag;
			if (game == null)
			{
				flag = false;
			}
			else
			{
				ChatBox gameHandler = game.GetGameHandler<ChatBox>();
				bool? flag2 = ((gameHandler != null) ? new bool?(gameHandler.IsContentRestricted) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			this.IsChatDisabled = flag;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00014319 File Offset: 0x00012519
		private void ClearMission()
		{
			this._mission = null;
			this.ActiveChannelType = ChatChannelType.None;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00014329 File Offset: 0x00012529
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._game != null)
			{
				this.ClearGame();
			}
			if (this._mission != null)
			{
				this.ClearMission();
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00014350 File Offset: 0x00012550
		private void ExecuteSendMessage()
		{
			string text = this.WrittenText;
			if (string.IsNullOrEmpty(text))
			{
				this.WrittenText = string.Empty;
				return;
			}
			if (text.Length > this.MaxMessageLength)
			{
				text = this.WrittenText.Substring(0, this.MaxMessageLength);
			}
			text = Regex.Replace(text.Trim(), "\\s+", " ");
			if (text.StartsWith("/"))
			{
				string[] array = text.Split(new char[] { ' ' });
				ChatChannelType chatChannelType = ChatChannelType.None;
				LobbyClient gameClient = NetworkMain.GameClient;
				if (gameClient != null && gameClient.Connected)
				{
					string text2 = array[0].ToLower();
					if (!(text2 == "/all") && !(text2 == "/a"))
					{
						if (!(text2 == "/team") && !(text2 == "/t"))
						{
							if (!(text2 == "/ab"))
							{
								if (text2 == "/ac")
								{
									if (Mission.Current != null)
									{
										MissionLobbyComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
										if (missionBehavior != null)
										{
											missionBehavior.RequestAdminMessage(string.Join(" ", array.Skip<string>(1)), false);
										}
									}
								}
							}
							else if (Mission.Current != null)
							{
								MissionLobbyComponent missionBehavior2 = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
								if (missionBehavior2 != null)
								{
									missionBehavior2.RequestAdminMessage(string.Join(" ", array.Skip<string>(1)), true);
								}
							}
						}
						else
						{
							chatChannelType = ChatChannelType.Team;
						}
					}
					else
					{
						chatChannelType = ChatChannelType.All;
					}
				}
				this.ActiveChannelType = chatChannelType;
			}
			else
			{
				ChatChannelType activeChannelType = this.ActiveChannelType;
				if (activeChannelType != ChatChannelType.Private)
				{
					if (activeChannelType - ChatChannelType.All <= 2)
					{
						this.CheckSpamAndSendMessage(this.ActiveChannelType, text);
					}
					else
					{
						Debug.FailedAssert("Player in invalid channel", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Multiplayer\\MPChatVM.cs", "ExecuteSendMessage", 496);
					}
				}
			}
			this.WrittenText = "";
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000144F8 File Offset: 0x000126F8
		private void CheckSpamAndSendMessage(ChatChannelType channelType, string textToSend)
		{
			if (this._recentlySentMessagesTimes.Count >= 5)
			{
				GameTexts.SetVariable("SECONDS", (15f - (Time.ApplicationTime - this._recentlySentMessagesTimes[0])).ToString("0.0"));
				this.AddChatLine(new MPChatLineVM(new TextObject("{=76VR5o8h}You must wait {SECONDS} seconds before sending another message.", null).ToString(), this.GetChannelColor(ChatChannelType.System), "Default"));
				return;
			}
			this._recentlySentMessagesTimes.Add(Time.ApplicationTime);
			this.SendMessageToChannel(this.ActiveChannelType, textToSend);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00014588 File Offset: 0x00012788
		private void HandleAddChatLineRequest(InformationMessage informationMessage)
		{
			string information = informationMessage.Information;
			string text = (string.IsNullOrEmpty(informationMessage.Category) ? "Default" : informationMessage.Category);
			Color color = informationMessage.Color;
			MPChatLineVM mpchatLineVM = new MPChatLineVM(information, color, text);
			this._requestedMessages.Enqueue(mpchatLineVM);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000145D4 File Offset: 0x000127D4
		public void SendMessageToChannel(ChatChannelType channel, string message)
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			if (gameClient != null && gameClient.Connected)
			{
				switch (channel)
				{
				case ChatChannelType.All:
					this._chatBox.SendMessageToAll(message);
					return;
				case ChatChannelType.Team:
					this._chatBox.SendMessageToTeam(message);
					return;
				}
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00014628 File Offset: 0x00012828
		void IChatHandler.ReceiveChatMessage(ChatChannelType channel, string sender, string message)
		{
			TextObject textObject;
			if (channel == ChatChannelType.Private)
			{
				textObject = new TextObject("{=6syoutpV}From {WHISPER_TARGET}", null);
				textObject.SetTextVariable("WHISPER_TARGET", sender);
			}
			else
			{
				textObject = TextObject.GetEmpty();
			}
			this.AddMessage(message, sender, channel, textObject);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00014664 File Offset: 0x00012864
		private void AddMessage(string msg, string author, ChatChannelType type, TextObject customChannelName = null)
		{
			Color channelColor = this.GetChannelColor(type);
			string text = ((!TextObject.IsNullOrEmpty(customChannelName)) ? customChannelName.ToString() : type.ToString());
			MPChatLineVM mpchatLineVM = new MPChatLineVM(string.Concat(new string[] { "(", text, ") ", author, ": ", msg }), channelColor, "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000146DC File Offset: 0x000128DC
		private void AddChatLine(MPChatLineVM chatLine)
		{
			if (NativeConfig.DisableGuiMessages || chatLine == null)
			{
				return;
			}
			this._allMessages.Add(chatLine);
			int num = this._maxHistoryCount * 5;
			if (this._allMessages.Count > num)
			{
				this._allMessages.RemoveAt(0);
			}
			if (this.IsMessageIncluded(chatLine))
			{
				this.MessageHistory.Add(chatLine);
				if (this.MessageHistory.Count > this._maxHistoryCount)
				{
					this.MessageHistory.RemoveAt(0);
				}
			}
			this.RefreshVisibility();
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00014760 File Offset: 0x00012960
		public void CheckChatFading(float dt)
		{
			foreach (MPChatLineVM mpchatLineVM in this._allMessages)
			{
				mpchatLineVM.HandleFading(dt);
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000147B4 File Offset: 0x000129B4
		private void ChatHistoryFilterToggled()
		{
			this.MessageHistory.Clear();
			int num = 0;
			while (num < this._allMessages.Count && this.MessageHistory.Count < this._maxHistoryCount)
			{
				MPChatLineVM mpchatLineVM = this._allMessages[num];
				if (this.IsMessageIncluded(mpchatLineVM))
				{
					this.MessageHistory.Add(mpchatLineVM);
				}
				num++;
			}
			this.RefreshVisibility();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001481D File Offset: 0x00012A1D
		private bool IsMessageIncluded(MPChatLineVM chatLine)
		{
			if (chatLine.Category == "Combat")
			{
				return this.IncludeCombatLog;
			}
			return !(chatLine.Category == "Bark") || this.IncludeBark;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00014852 File Offset: 0x00012A52
		public void SetChatDisabledStateChangedCallback(Action<bool> onChatDisabledStateChanged)
		{
			this._onChatDisabledStateChanged = onChatDisabledStateChanged;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001485B File Offset: 0x00012A5B
		public void SetGetKeyTextFromKeyIDFunc(Func<TextObject> getToggleChatKeyText)
		{
			this._getToggleChatKeyText = getToggleChatKeyText;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00014864 File Offset: 0x00012A64
		public void SetGetCycleChannelKeyTextFunc(Func<TextObject> getCycleChannelsKeyText)
		{
			this._getCycleChannelsKeyText = getCycleChannelsKeyText;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001486D File Offset: 0x00012A6D
		public void SetGetSendMessageKeyTextFunc(Func<TextObject> getSendMessageKeyText)
		{
			this._getSendMessageKeyText = getSendMessageKeyText;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00014876 File Offset: 0x00012A76
		public void SetGetCancelSendingKeyTextFunc(Func<TextObject> getCancelSendingKeyText)
		{
			this._getCancelSendingKeyText = getCancelSendingKeyText;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00014880 File Offset: 0x00012A80
		private void OnPlayerMessageReceived(NetworkCommunicator player, string message, bool toTeamOnly)
		{
			MissionPeer component = player.GetComponent<MissionPeer>();
			string text = ((component != null) ? component.DisplayedName : null) ?? player.UserName;
			if (component != null && !component.IsAgentAliveForChatting)
			{
				GameTexts.SetVariable("PLAYER_NAME", text);
				text = GameTexts.FindText("str_chat_message_dead_player", null).ToString();
			}
			this.AddMessage(message, text, toTeamOnly ? ChatChannelType.Team : ChatChannelType.All, null);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000148E8 File Offset: 0x00012AE8
		private void OnWhisperMessageReceived(string fromUserName, string message)
		{
			this.AddMessage(message, fromUserName, ChatChannelType.Private, null);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000148F4 File Offset: 0x00012AF4
		private void OnErrorWhisperMessageReceived(string toUserName)
		{
			TextObject textObject = new TextObject("{=61isYVW0}Player {USER_NAME} is not found", null);
			textObject.SetTextVariable("USER_NAME", toUserName);
			MPChatLineVM mpchatLineVM = new MPChatLineVM(textObject.ToString(), Color.White, "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00014935 File Offset: 0x00012B35
		private void OnWhisperMessageSent(string message, string whisperTarget)
		{
			this.AddMessage(message, whisperTarget, ChatChannelType.Private, null);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00014944 File Offset: 0x00012B44
		private void OnServerMessage(string message)
		{
			MPChatLineVM mpchatLineVM = new MPChatLineVM(message, Color.White, "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001496C File Offset: 0x00012B6C
		private void OnServerAdminMessage(string message)
		{
			MPChatLineVM mpchatLineVM = new MPChatLineVM("[Admin]: " + message, Color.ConvertStringToColor("#CC0099FF"), "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000149A0 File Offset: 0x00012BA0
		private Color GetChannelColor(ChatChannelType type)
		{
			string text;
			switch (type)
			{
			case ChatChannelType.Private:
				text = "#8C1ABDFF";
				break;
			case ChatChannelType.All:
				text = "#EC943EFF";
				break;
			case ChatChannelType.Team:
				text = "#05C5F7FF";
				break;
			case ChatChannelType.Party:
				text = "#05C587FF";
				break;
			case ChatChannelType.System:
				text = "#FF0000FF";
				break;
			case ChatChannelType.Custom:
				text = "#FF0000FF";
				break;
			default:
				text = "#FFFFFFFF";
				break;
			}
			return Color.ConvertStringToColor(text);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00014A09 File Offset: 0x00012C09
		public bool IsChatAllowedByOptions()
		{
			if (GameNetwork.IsMultiplayer)
			{
				return BannerlordConfig.EnableMultiplayerChatBox;
			}
			return BannerlordConfig.EnableSingleplayerChatBox && (Mission.Current == null || !BannerlordConfig.HideBattleUI);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00014A32 File Offset: 0x00012C32
		public void TypeToChannelAll(bool startTyping = false)
		{
			this.ActiveChannelType = ChatChannelType.All;
			if (startTyping)
			{
				this.StartTyping();
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00014A44 File Offset: 0x00012C44
		public void TypeToChannelTeam(bool startTyping = false)
		{
			this.ActiveChannelType = ChatChannelType.Team;
			if (startTyping)
			{
				this.StartTyping();
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00014A56 File Offset: 0x00012C56
		public void StartInspectingMessages()
		{
			this.IsInspectingMessages = true;
			this.IsTypingText = false;
			this.WrittenText = "";
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00014A71 File Offset: 0x00012C71
		public void StopInspectingMessages()
		{
			this.IsInspectingMessages = false;
			this.IsTypingText = false;
			this.WrittenText = "";
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00014A8C File Offset: 0x00012C8C
		public void StartTyping()
		{
			this.IsTypingText = true;
			this.IsInspectingMessages = true;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00014A9C File Offset: 0x00012C9C
		public void StopTyping(bool resetWrittenText = false)
		{
			this.IsTypingText = false;
			this.IsInspectingMessages = false;
			if (resetWrittenText)
			{
				this.WrittenText = "";
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00014ABA File Offset: 0x00012CBA
		public void SendCurrentlyTypedMessage()
		{
			this.ExecuteSendMessage();
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00014AC4 File Offset: 0x00012CC4
		private void RefreshVisibility()
		{
			foreach (MPChatLineVM mpchatLineVM in this._allMessages)
			{
				mpchatLineVM.ToggleForceVisible(this.IsTypingText || this.IsInspectingMessages);
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00014B28 File Offset: 0x00012D28
		public void ExecuteSaveSizes()
		{
			BannerlordConfig.ChatBoxSizeX = this.ChatBoxSizeX;
			BannerlordConfig.ChatBoxSizeY = this.ChatBoxSizeY;
			BannerlordConfig.Save();
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00014B46 File Offset: 0x00012D46
		public void SetMessageHistoryCapacity(int capacity)
		{
			this._maxHistoryCount = capacity;
			MBBindingList<MPChatLineVM> messageHistory = this.MessageHistory;
			if (messageHistory == null)
			{
				return;
			}
			messageHistory.Clear();
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00014B5F File Offset: 0x00012D5F
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x00014B67 File Offset: 0x00012D67
		[DataSourceProperty]
		public float ChatBoxSizeX
		{
			get
			{
				return this._chatBoxSizeX;
			}
			set
			{
				if (value != this._chatBoxSizeX)
				{
					this._chatBoxSizeX = value;
					base.OnPropertyChangedWithValue(value, "ChatBoxSizeX");
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00014B85 File Offset: 0x00012D85
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x00014B8D File Offset: 0x00012D8D
		[DataSourceProperty]
		public float ChatBoxSizeY
		{
			get
			{
				return this._chatBoxSizeY;
			}
			set
			{
				if (value != this._chatBoxSizeY)
				{
					this._chatBoxSizeY = value;
					base.OnPropertyChangedWithValue(value, "ChatBoxSizeY");
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00014BAB File Offset: 0x00012DAB
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x00014BB3 File Offset: 0x00012DB3
		[DataSourceProperty]
		public int MaxMessageLength
		{
			get
			{
				return this._maxMessageLength;
			}
			set
			{
				if (value != this._maxMessageLength)
				{
					this._maxMessageLength = value;
					base.OnPropertyChangedWithValue(value, "MaxMessageLength");
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00014BD1 File Offset: 0x00012DD1
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00014BD9 File Offset: 0x00012DD9
		[DataSourceProperty]
		public bool IsTypingText
		{
			get
			{
				return this._isTypingText;
			}
			set
			{
				if (value != this._isTypingText)
				{
					this._isTypingText = value;
					base.OnPropertyChangedWithValue(value, "IsTypingText");
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00014BFD File Offset: 0x00012DFD
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00014C05 File Offset: 0x00012E05
		[DataSourceProperty]
		public bool IsInspectingMessages
		{
			get
			{
				return this._isInspectingMessages;
			}
			set
			{
				if (value != this._isInspectingMessages)
				{
					this._isInspectingMessages = value;
					this.UpdateHideShowText(this._isInspectingMessages);
					this.UpdateShortcutTexts();
					base.OnPropertyChangedWithValue(value, "IsInspectingMessages");
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00014C3B File Offset: 0x00012E3B
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x00014C43 File Offset: 0x00012E43
		[DataSourceProperty]
		public bool IsChatDisabled
		{
			get
			{
				return this._isChatDisabled;
			}
			set
			{
				if (value != this._isChatDisabled)
				{
					this._isChatDisabled = value;
					if (value)
					{
						this.StopTyping(true);
					}
					Action<bool> onChatDisabledStateChanged = this._onChatDisabledStateChanged;
					if (onChatDisabledStateChanged != null)
					{
						onChatDisabledStateChanged(value);
					}
					base.OnPropertyChangedWithValue(value, "IsChatDisabled");
				}
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00014C7D File Offset: 0x00012E7D
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00014C85 File Offset: 0x00012E85
		[DataSourceProperty]
		public bool ShowHideShowHint
		{
			get
			{
				return this._showHideShowHint;
			}
			set
			{
				if (value != this._showHideShowHint)
				{
					this._showHideShowHint = value;
					base.OnPropertyChangedWithValue(value, "ShowHideShowHint");
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00014CA3 File Offset: 0x00012EA3
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x00014CAB File Offset: 0x00012EAB
		[DataSourceProperty]
		public bool IsOptionsAvailable
		{
			get
			{
				return this._isOptionsAvailable;
			}
			set
			{
				if (value != this._isOptionsAvailable)
				{
					this._isOptionsAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsOptionsAvailable");
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00014CC9 File Offset: 0x00012EC9
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x00014CD1 File Offset: 0x00012ED1
		[DataSourceProperty]
		public bool ShouldHaveOffset
		{
			get
			{
				return this._shouldHaveOffset;
			}
			set
			{
				if (value != this._shouldHaveOffset)
				{
					this._shouldHaveOffset = value;
					base.OnPropertyChangedWithValue(value, "ShouldHaveOffset");
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00014CEF File Offset: 0x00012EEF
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x00014CF7 File Offset: 0x00012EF7
		[DataSourceProperty]
		public string WrittenText
		{
			get
			{
				return this._writtenText;
			}
			set
			{
				if (value != this._writtenText)
				{
					this._writtenText = value;
					base.OnPropertyChangedWithValue<string>(value, "WrittenText");
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00014D1A File Offset: 0x00012F1A
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x00014D22 File Offset: 0x00012F22
		[DataSourceProperty]
		public Color ActiveChannelColor
		{
			get
			{
				return this._activeChannelColor;
			}
			set
			{
				if (value != this._activeChannelColor)
				{
					this._activeChannelColor = value;
					base.OnPropertyChangedWithValue(value, "ActiveChannelColor");
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00014D45 File Offset: 0x00012F45
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x00014D4D File Offset: 0x00012F4D
		[DataSourceProperty]
		public string ActiveChannelNameText
		{
			get
			{
				return this._activeChannelNameText;
			}
			set
			{
				if (value != this._activeChannelNameText)
				{
					this._activeChannelNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActiveChannelNameText");
				}
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00014D70 File Offset: 0x00012F70
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00014D78 File Offset: 0x00012F78
		[DataSourceProperty]
		public string HideShowText
		{
			get
			{
				return this._hideShowText;
			}
			set
			{
				if (value != this._hideShowText)
				{
					this._hideShowText = value;
					base.OnPropertyChangedWithValue<string>(value, "HideShowText");
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00014D9B File Offset: 0x00012F9B
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00014DA3 File Offset: 0x00012FA3
		[DataSourceProperty]
		public string ToggleCombatLogText
		{
			get
			{
				return this._toggleCombatLogText;
			}
			set
			{
				if (value != this._toggleCombatLogText)
				{
					this._toggleCombatLogText = value;
					base.OnPropertyChangedWithValue<string>(value, "ToggleCombatLogText");
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00014DC6 File Offset: 0x00012FC6
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00014DCE File Offset: 0x00012FCE
		[DataSourceProperty]
		public string ToggleBarkText
		{
			get
			{
				return this._toggleBarkText;
			}
			set
			{
				if (value != this._toggleBarkText)
				{
					this._toggleBarkText = value;
					base.OnPropertyChangedWithValue<string>(value, "ToggleBarkText");
				}
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00014DF1 File Offset: 0x00012FF1
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00014DF9 File Offset: 0x00012FF9
		[DataSourceProperty]
		public string CycleThroughChannelsText
		{
			get
			{
				return this._cycleThroughChannelsText;
			}
			set
			{
				if (value != this._cycleThroughChannelsText)
				{
					this._cycleThroughChannelsText = value;
					base.OnPropertyChangedWithValue<string>(value, "CycleThroughChannelsText");
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00014E1C File Offset: 0x0001301C
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x00014E24 File Offset: 0x00013024
		[DataSourceProperty]
		public string SendMessageText
		{
			get
			{
				return this._sendMessageText;
			}
			set
			{
				if (value != this._sendMessageText)
				{
					this._sendMessageText = value;
					base.OnPropertyChangedWithValue<string>(value, "SendMessageText");
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00014E47 File Offset: 0x00013047
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00014E4F File Offset: 0x0001304F
		[DataSourceProperty]
		public string CancelSendingText
		{
			get
			{
				return this._cancelSendingText;
			}
			set
			{
				if (value != this._cancelSendingText)
				{
					this._cancelSendingText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelSendingText");
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00014E72 File Offset: 0x00013072
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x00014E7A File Offset: 0x0001307A
		[DataSourceProperty]
		public MBBindingList<MPChatLineVM> MessageHistory
		{
			get
			{
				return this._messageHistory;
			}
			set
			{
				if (value != this._messageHistory)
				{
					this._messageHistory = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPChatLineVM>>(value, "MessageHistory");
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00014E98 File Offset: 0x00013098
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00014EA0 File Offset: 0x000130A0
		[DataSourceProperty]
		public HintViewModel CombatLogHint
		{
			get
			{
				return this._combatLogHint;
			}
			set
			{
				if (value != this._combatLogHint)
				{
					this._combatLogHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CombatLogHint");
				}
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00014EBE File Offset: 0x000130BE
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x00014EC6 File Offset: 0x000130C6
		[DataSourceProperty]
		public bool IncludeCombatLog
		{
			get
			{
				return this._includeCombatLog;
			}
			set
			{
				if (value != this._includeCombatLog)
				{
					this._includeCombatLog = value;
					base.OnPropertyChangedWithValue(value, "IncludeCombatLog");
					this.ChatHistoryFilterToggled();
					BannerlordConfig.ReportDamage = value;
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00014EF0 File Offset: 0x000130F0
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x00014EF8 File Offset: 0x000130F8
		[DataSourceProperty]
		public bool IncludeBark
		{
			get
			{
				return this._includeBark;
			}
			set
			{
				if (value != this._includeBark)
				{
					this._includeBark = value;
					base.OnPropertyChangedWithValue(value, "IncludeBark");
					this.ChatHistoryFilterToggled();
					BannerlordConfig.ReportBark = value;
				}
			}
		}

		// Token: 0x04000242 RID: 578
		private readonly TextObject _hideText = new TextObject("{=ou5KJERr}Press '{KEY}' to hide", null);

		// Token: 0x04000243 RID: 579
		private readonly TextObject _cycleChannelsText = new TextObject("{=Dhb2N5JD}Press '{KEY}' to cycle through channels", null);

		// Token: 0x04000244 RID: 580
		private readonly TextObject _sendMessageTextObject = new TextObject("{=f64QfbTO}'{KEY}' to send", null);

		// Token: 0x04000245 RID: 581
		private readonly TextObject _cancelSendingTextObject = new TextObject("{=U1rHNqOk}'{KEY}' to cancel", null);

		// Token: 0x04000246 RID: 582
		public const string DefaultCategory = "Default";

		// Token: 0x04000247 RID: 583
		public const string CombatCategory = "Combat";

		// Token: 0x04000248 RID: 584
		public const string SocialCategory = "Social";

		// Token: 0x04000249 RID: 585
		public const string BarkCategory = "Bark";

		// Token: 0x0400024A RID: 586
		private int _maxHistoryCount = 100;

		// Token: 0x0400024B RID: 587
		private const int _spamDetectionInterval = 15;

		// Token: 0x0400024C RID: 588
		private const int _maxMessagesAllowedPerInterval = 5;

		// Token: 0x0400024D RID: 589
		private List<float> _recentlySentMessagesTimes;

		// Token: 0x0400024E RID: 590
		private readonly List<MPChatLineVM> _allMessages;

		// Token: 0x0400024F RID: 591
		private readonly Queue<MPChatLineVM> _requestedMessages;

		// Token: 0x04000250 RID: 592
		private Action<bool> _onChatDisabledStateChanged;

		// Token: 0x04000251 RID: 593
		private Func<TextObject> _getToggleChatKeyText;

		// Token: 0x04000252 RID: 594
		private Func<TextObject> _getCycleChannelsKeyText;

		// Token: 0x04000253 RID: 595
		private Func<TextObject> _getSendMessageKeyText;

		// Token: 0x04000254 RID: 596
		private Func<TextObject> _getCancelSendingKeyText;

		// Token: 0x04000255 RID: 597
		private ChatBox _chatBox;

		// Token: 0x04000256 RID: 598
		private Game _game;

		// Token: 0x04000257 RID: 599
		private Mission _mission;

		// Token: 0x04000258 RID: 600
		private ChatChannelType _activeChannelType = ChatChannelType.None;

		// Token: 0x04000259 RID: 601
		private float _chatBoxSizeX;

		// Token: 0x0400025A RID: 602
		private float _chatBoxSizeY;

		// Token: 0x0400025B RID: 603
		private int _maxMessageLength;

		// Token: 0x0400025C RID: 604
		private string _writtenText = "";

		// Token: 0x0400025D RID: 605
		private string _activeChannelNameText;

		// Token: 0x0400025E RID: 606
		private string _hideShowText;

		// Token: 0x0400025F RID: 607
		private string _toggleCombatLogText;

		// Token: 0x04000260 RID: 608
		private string _toggleBarkText;

		// Token: 0x04000261 RID: 609
		private string _cycleThroughChannelsText;

		// Token: 0x04000262 RID: 610
		private string _sendMessageText;

		// Token: 0x04000263 RID: 611
		private string _cancelSendingText;

		// Token: 0x04000264 RID: 612
		private MBBindingList<MPChatLineVM> _messageHistory;

		// Token: 0x04000265 RID: 613
		private bool _includeCombatLog;

		// Token: 0x04000266 RID: 614
		private bool _includeBark;

		// Token: 0x04000267 RID: 615
		private bool _isTypingText;

		// Token: 0x04000268 RID: 616
		private bool _isInspectingMessages;

		// Token: 0x04000269 RID: 617
		private bool _isChatDisabled;

		// Token: 0x0400026A RID: 618
		private bool _showHideShowHint;

		// Token: 0x0400026B RID: 619
		private bool _isOptionsAvailable;

		// Token: 0x0400026C RID: 620
		private bool _shouldHaveOffset;

		// Token: 0x0400026D RID: 621
		private HintViewModel _combatLogHint;

		// Token: 0x0400026E RID: 622
		private Color _activeChannelColor;
	}
}
