using System;
using System.Collections.Generic;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000016 RID: 22
	public class MultiplayerAdminComponent : MissionNetwork
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000153 RID: 339 RVA: 0x00005950 File Offset: 0x00003B50
		// (remove) Token: 0x06000154 RID: 340 RVA: 0x00005988 File Offset: 0x00003B88
		public event MultiplayerAdminComponent.OnSetAdminMenuActiveStateDelegate OnSetAdminMenuActiveState;

		// Token: 0x06000155 RID: 341 RVA: 0x000059BD File Offset: 0x00003BBD
		public MultiplayerAdminComponent()
		{
			if (string.IsNullOrEmpty(MultiplayerIntermissionVotingManager.Instance.InitialGameType))
			{
				MultiplayerIntermissionVotingManager.Instance.InitialGameType = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000059E8 File Offset: 0x00003BE8
		public override void OnMissionStateActivated()
		{
			base.OnMissionStateActivated();
			MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = !MultiplayerIntermissionVotingManager.Instance.IsDisableMapVoteOverride;
			MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = !MultiplayerIntermissionVotingManager.Instance.IsDisableCultureVoteOverride;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005A1E File Offset: 0x00003C1E
		public void ChangeAdminMenuActiveState(bool isActive)
		{
			MultiplayerAdminComponent.OnSetAdminMenuActiveStateDelegate onSetAdminMenuActiveState = this.OnSetAdminMenuActiveState;
			if (onSetAdminMenuActiveState == null)
			{
				return;
			}
			onSetAdminMenuActiveState(isActive);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005A34 File Offset: 0x00003C34
		public void KickPlayer(NetworkCommunicator peerToKick, bool banPlayer)
		{
			if (GameNetwork.IsServer)
			{
				MissionPeer component = peerToKick.GetComponent<MissionPeer>();
				if (!peerToKick.IsMine && component != null && !peerToKick.IsAdmin)
				{
					DisconnectInfo disconnectInfo = peerToKick.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo") ?? new DisconnectInfo();
					disconnectInfo.Type = DisconnectType.KickedByHost;
					peerToKick.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo);
					GameNetwork.AddNetworkPeerToDisconnectAsServer(peerToKick);
					if (banPlayer)
					{
						CustomGameBannedPlayerManager.AddBannedPlayer(peerToKick.VirtualPlayer.Id, int.MaxValue);
						return;
					}
				}
			}
			else if (GameNetwork.IsClient && !peerToKick.IsMine)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new KickPlayer(peerToKick, banPlayer));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005ADC File Offset: 0x00003CDC
		public void GlobalMuteUnmutePlayer(NetworkCommunicator peerToMute, bool unmute)
		{
			if (GameNetwork.IsServer)
			{
				MissionPeer component = peerToMute.GetComponent<MissionPeer>();
				if (!peerToMute.IsMine && component != null && !peerToMute.IsAdmin)
				{
					PlayerId id = peerToMute.VirtualPlayer.Id;
					if (MultiplayerGlobalMutedPlayersManager.IsUserMuted(id) == unmute)
					{
						if (unmute)
						{
							MultiplayerGlobalMutedPlayersManager.UnmutePlayer(peerToMute.VirtualPlayer.Id);
						}
						else
						{
							MultiplayerGlobalMutedPlayersManager.MutePlayer(peerToMute.VirtualPlayer.Id);
						}
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SyncPlayerMuteState(id, !unmute));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						return;
					}
				}
			}
			else if (GameNetwork.IsClient && !peerToMute.IsMine)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new AdminMuteUnmutePlayer(peerToMute, unmute));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005B8C File Offset: 0x00003D8C
		public void EndWarmup()
		{
			if (GameNetwork.IsServer)
			{
				if (Mission.Current != null)
				{
					MultiplayerWarmupComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerWarmupComponent>();
					if (missionBehavior != null)
					{
						missionBehavior.EndWarmupProgress();
						return;
					}
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new AdminRequestEndWarmup());
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005BD0 File Offset: 0x00003DD0
		public void ChangeWelcomeMessage(string newWelcomeMessage)
		{
			if (GameNetwork.IsServer)
			{
				MultiplayerOptions.OptionType.WelcomeMessage.SetValue(newWelcomeMessage, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				this.SyncImmediateOptions();
				return;
			}
			if (MultiplayerOptions.OptionType.WelcomeMessage.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) != newWelcomeMessage)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ChangeWelcomeMessage(newWelcomeMessage));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005C0C File Offset: 0x00003E0C
		public void AdminAnnouncement(string message, bool isBroadcast)
		{
			if (GameNetwork.IsServer)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new ServerAdminMessage(message, isBroadcast));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				return;
			}
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(new AdminRequestAnnouncement(message, isBroadcast));
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005C44 File Offset: 0x00003E44
		public void ChangeClassRestriction(FormationClass classToChangeRestriction, bool newValue)
		{
			if (GameNetwork.IsServer)
			{
				this._missionLobbyComponent.ChangeClassRestriction(classToChangeRestriction, newValue);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new ChangeClassRestrictions(classToChangeRestriction, newValue));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				return;
			}
			if (!this._missionLobbyComponent.IsClassAvailable(classToChangeRestriction) != newValue)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new AdminRequestClassRestrictionChange(classToChangeRestriction, newValue));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005CA6 File Offset: 0x00003EA6
		public void AdminEndMission()
		{
			if (GameNetwork.IsServer)
			{
				this._missionLobbyComponent.SetStateEndingAsServer();
				return;
			}
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(new AdminRequestEndMission());
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005CCF File Offset: 0x00003ECF
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
			this._missionLobbyComponent.OnAdminMessageRequested += this.AdminAnnouncement;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005D00 File Offset: 0x00003F00
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<KickPlayer>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventKickPlayer));
				registerer.RegisterBaseHandler<ChangeWelcomeMessage>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventChangeWelcomeMessage));
				registerer.RegisterBaseHandler<AdminRequestAnnouncement>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestAnnouncement));
				registerer.RegisterBaseHandler<AdminRequestClassRestrictionChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestClassRestrictionChange));
				registerer.RegisterBaseHandler<AdminRequestEndMission>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestEndMission));
				registerer.RegisterBaseHandler<AdminUpdateMultiplayerOptions>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleAdminUpdateMultiplayerOptions));
				registerer.RegisterBaseHandler<AdminMuteUnmutePlayer>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventMuteUnmutePlayer));
				registerer.RegisterBaseHandler<AdminRequestEndWarmup>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestEndWarmup));
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005DA8 File Offset: 0x00003FA8
		private bool HandleAdminUpdateMultiplayerOptions(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminUpdateMultiplayerOptions adminUpdateMultiplayerOptions = (AdminUpdateMultiplayerOptions)baseMessage;
			if (peer.IsAdmin && adminUpdateMultiplayerOptions.Options != null)
			{
				bool flag = false;
				bool flag2 = false;
				string text = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				string text2 = null;
				string text3 = null;
				bool flag3 = false;
				bool flag4 = false;
				for (int i = 0; i < adminUpdateMultiplayerOptions.Options.Count; i++)
				{
					AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = adminUpdateMultiplayerOptions.Options[i];
					bool flag5 = true;
					if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.GameType)
					{
						flag5 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						if (adminMultiplayerOptionInfo.StringValue == MultiplayerIntermissionVotingManager.Instance.InitialGameType || (!flag5 && MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) == MultiplayerIntermissionVotingManager.Instance.InitialGameType))
						{
							flag = true;
						}
						if (flag5)
						{
							text = adminMultiplayerOptionInfo.StringValue;
						}
					}
					if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.Map)
					{
						flag5 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						flag2 = !flag5;
					}
					if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.CultureTeam1)
					{
						flag3 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						text2 = (flag3 ? adminMultiplayerOptionInfo.StringValue : null);
					}
					else if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.CultureTeam2)
					{
						flag4 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						text3 = (flag4 ? adminMultiplayerOptionInfo.StringValue : null);
					}
					else if (flag5)
					{
						switch (adminMultiplayerOptionInfo.OptionType.GetOptionProperty().OptionValueType)
						{
						case MultiplayerOptions.OptionValueType.Bool:
							adminMultiplayerOptionInfo.OptionType.SetValue(adminMultiplayerOptionInfo.BoolValue, adminMultiplayerOptionInfo.AccessMode);
							break;
						case MultiplayerOptions.OptionValueType.Integer:
						case MultiplayerOptions.OptionValueType.Enum:
							adminMultiplayerOptionInfo.OptionType.SetValue(adminMultiplayerOptionInfo.IntValue, adminMultiplayerOptionInfo.AccessMode);
							break;
						case MultiplayerOptions.OptionValueType.String:
							adminMultiplayerOptionInfo.OptionType.SetValue(adminMultiplayerOptionInfo.StringValue, adminMultiplayerOptionInfo.AccessMode);
							break;
						}
					}
				}
				if (flag2)
				{
					MultiplayerIntermissionVotingManager.Instance.IsMapSelectedByAdmin = false;
					if (flag)
					{
						if (MultiplayerIntermissionVotingManager.Instance.IsDisableMapVoteOverride)
						{
							MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = false;
							string id = MultiplayerIntermissionVotingManager.Instance.MapVoteItems.GetRandomElement<IntermissionVoteItem>().Id;
							MultiplayerOptions.OptionType.Map.SetValue(id, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
							Debug.Print("[Admin] game type was default and map was undecided. Voting was disabled. Selected map randomly from automated map pool: " + id, 0, Debug.DebugColor.White, 17592186044416UL);
						}
						else
						{
							MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = true;
							Debug.Print("[Admin] game type was default and map was undecided. Maps will be voted from automated map pool", 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
					else
					{
						MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = false;
						string randomElement = MultiplayerIntermissionVotingManager.Instance.GetUsableMaps(text).GetRandomElement<string>();
						MultiplayerOptions.OptionType.Map.SetValue(randomElement, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
						Debug.Print("[Admin] game type wasn't default and map was undecided. Selected map randomly from usable maps: " + randomElement + ".", 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
				else
				{
					MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = false;
					MultiplayerIntermissionVotingManager.Instance.IsMapSelectedByAdmin = true;
					Debug.Print("[Admin] next game type: " + text + " next map: " + MultiplayerOptions.OptionType.Map.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				if (flag3 && flag4)
				{
					MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = false;
					MultiplayerOptions.OptionType.CultureTeam1.SetValue(text2, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					MultiplayerOptions.OptionType.CultureTeam2.SetValue(text3, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					Debug.Print(string.Concat(new string[] { "[Admin] Both cultures were valid. Setting ", text2, " vs ", text3, " for next game." }), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else if (MultiplayerIntermissionVotingManager.Instance.IsDisableCultureVoteOverride)
				{
					MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = false;
					MultiplayerIntermissionVotingManager.Instance.SelectRandomCultures(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					string strValue = MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					string strValue2 = MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					Debug.Print(string.Concat(new string[] { "[Admin] Cultures weren't valid. Randomly setting ", strValue, " vs ", strValue2, " for next game." }), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else
				{
					MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = true;
					Debug.Print("[Admin] Cultures weren't valid. Culture voting is enabled", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new MultiplayerOptionsImmediate());
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				GameNetwork.BeginModuleEventAsServer(peer);
				GameNetwork.WriteMessage(new UpdateIntermissionVotingManagerValues());
				GameNetwork.EndModuleEventAsServer();
			}
			return true;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000061CC File Offset: 0x000043CC
		private bool HandleClientEventKickPlayer(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			KickPlayer kickPlayer = (KickPlayer)baseMessage;
			if (peer.IsAdmin)
			{
				this.KickPlayer(kickPlayer.PlayerPeer, kickPlayer.BanPlayer);
			}
			return true;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000061FC File Offset: 0x000043FC
		private bool HandleClientEventMuteUnmutePlayer(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminMuteUnmutePlayer adminMuteUnmutePlayer = (AdminMuteUnmutePlayer)baseMessage;
			if (peer.IsAdmin)
			{
				this.GlobalMuteUnmutePlayer(adminMuteUnmutePlayer.PlayerPeer, adminMuteUnmutePlayer.Unmute);
			}
			return true;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000622C File Offset: 0x0000442C
		private bool HandleClientEventChangeWelcomeMessage(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			ChangeWelcomeMessage changeWelcomeMessage = (ChangeWelcomeMessage)baseMessage;
			if (peer.IsAdmin)
			{
				this.ChangeWelcomeMessage(changeWelcomeMessage.NewWelcomeMessage);
			}
			return true;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006258 File Offset: 0x00004458
		private bool HandleClientEventAdminRequestClassRestrictionChange(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestClassRestrictionChange adminRequestClassRestrictionChange = (AdminRequestClassRestrictionChange)baseMessage;
			if (peer.IsAdmin)
			{
				this.ChangeClassRestriction(adminRequestClassRestrictionChange.ClassToChangeRestriction, adminRequestClassRestrictionChange.NewValue);
			}
			return true;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006288 File Offset: 0x00004488
		private bool HandleClientEventAdminRequestAnnouncement(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestAnnouncement adminRequestAnnouncement = (AdminRequestAnnouncement)baseMessage;
			if (peer.IsAdmin)
			{
				this.AdminAnnouncement(adminRequestAnnouncement.Message, adminRequestAnnouncement.IsAdminBroadcast);
			}
			return true;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000062B7 File Offset: 0x000044B7
		private bool HandleClientEventAdminRequestEndMission(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestEndMission adminRequestEndMission = (AdminRequestEndMission)baseMessage;
			if (peer.IsAdmin)
			{
				this.AdminEndMission();
			}
			return true;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000062D0 File Offset: 0x000044D0
		[CommandLineFunctionality.CommandLineArgumentFunction("announcement", "mp_admin")]
		public static string MPAdminAnnouncement(List<string> strings)
		{
			if (strings.Count == 0)
			{
				return "Wrong format! Usage: mp_admin.announcement {TEXT}";
			}
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			MultiplayerAdminComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>();
			if (missionBehavior == null)
			{
				return "Admin component could not be found!";
			}
			missionBehavior.AdminAnnouncement(string.Join(" ", strings), true);
			return "Success";
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006323 File Offset: 0x00004523
		private bool HandleClientEventAdminRequestEndWarmup(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestEndWarmup adminRequestEndWarmup = (AdminRequestEndWarmup)baseMessage;
			if (peer.IsAdmin)
			{
				this.EndWarmup();
			}
			return true;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000633B File Offset: 0x0000453B
		public override void OnRemoveBehavior()
		{
			MultiplayerAdminComponent._multiplayerAdminComponent = null;
			base.OnRemoveBehavior();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006349 File Offset: 0x00004549
		private void SyncImmediateOptions()
		{
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new MultiplayerOptionsImmediate());
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006364 File Offset: 0x00004564
		[CommandLineFunctionality.CommandLineArgumentFunction("kick_player", "mp_admin")]
		public static string MPAdminKickPlayer(List<string> strings)
		{
			if (MultiplayerAdminComponent._multiplayerAdminComponent == null)
			{
				return "Failed: MultiplayerAdminComponent has not been created.";
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer == null || !myPeer.IsAdmin)
			{
				return "Failed: Only admins can use mp_admin commands.";
			}
			if (strings.Count != 1)
			{
				return "Failed: Input is incorrect.";
			}
			string text = strings[0];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.UserName == text)
				{
					MultiplayerAdminComponent._multiplayerAdminComponent.KickPlayer(networkCommunicator, false);
					return "Player " + text + " has been kicked from the server.";
				}
			}
			return text + " could not be found.";
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000642C File Offset: 0x0000462C
		[CommandLineFunctionality.CommandLineArgumentFunction("ban_player", "mp_admin")]
		public static string MPAdminBanPlayer(List<string> strings)
		{
			if (MultiplayerAdminComponent._multiplayerAdminComponent == null)
			{
				return "Failed: MultiplayerAdminComponent has not been created.";
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer == null || !myPeer.IsAdmin)
			{
				return "Failed: Only admins can use mp_admin commands.";
			}
			if (strings.Count != 1)
			{
				return "Failed: Input is incorrect.";
			}
			string text = strings[0];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.UserName == text)
				{
					MultiplayerAdminComponent._multiplayerAdminComponent.KickPlayer(networkCommunicator, true);
					return "Player " + text + " has been banned from the server.";
				}
			}
			return text + " could not be found.";
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000064F4 File Offset: 0x000046F4
		[CommandLineFunctionality.CommandLineArgumentFunction("change_welcome_message", "mp_admin")]
		public static string MPAdminChangeWelcomeMessage(List<string> strings)
		{
			if (MultiplayerAdminComponent._multiplayerAdminComponent == null)
			{
				return "Failed: MultiplayerAdminComponent has not been created.";
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer == null || !myPeer.IsAdmin)
			{
				return "Failed: Only admins can use mp_host commands.";
			}
			string text = "";
			foreach (string text2 in strings)
			{
				text = text + text2 + " ";
			}
			MultiplayerAdminComponent._multiplayerAdminComponent.ChangeWelcomeMessage(text);
			return "Changed welcome message to: " + text;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006590 File Offset: 0x00004790
		[CommandLineFunctionality.CommandLineArgumentFunction("change_class_restriction", "mp_admin")]
		public static string MPAdminChangeClassRestriction(List<string> strings)
		{
			FormationClass formationClass;
			bool flag;
			if (strings.Count != 2 || !Enum.TryParse<FormationClass>(strings[0], out formationClass) || !bool.TryParse(strings[1], out flag))
			{
				return "Wrong format! Usage: mp_admin.change_class_restriction {FormationClass} {true/false}";
			}
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			MultiplayerAdminComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>();
			if (missionBehavior == null)
			{
				return "Admin component could not be found!";
			}
			missionBehavior.ChangeClassRestriction(formationClass, flag);
			return "Success";
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000065FC File Offset: 0x000047FC
		[CommandLineFunctionality.CommandLineArgumentFunction("restart_game", "mp_admin")]
		public static string MPHostRestartGame(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			MultiplayerAdminComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>();
			if (missionBehavior == null)
			{
				return "Admin component could not be found!";
			}
			missionBehavior.AdminEndMission();
			return "Success";
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006638 File Offset: 0x00004838
		[CommandLineFunctionality.CommandLineArgumentFunction("change_server_slots", "mp_admin")]
		public static string MPAdminChangeServerSlots(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			if (Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>() == null)
			{
				return "Admin component could not be found!";
			}
			if (strings.Count != 1)
			{
				return "Wrong format! Usage: mp_admin.change_server_slots {NUMBER}";
			}
			int num;
			if (int.TryParse(strings[0], out num))
			{
				MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(num, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
				return "Success";
			}
			return "Wrong format! Usage: mp_admin.change_server_slots {NUMBER}";
		}

		// Token: 0x0400003C RID: 60
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400003D RID: 61
		private static MultiplayerAdminComponent _multiplayerAdminComponent;

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x06000410 RID: 1040
		public delegate void OnSelectPlayerToKickDelegate(bool banPlayer);

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x06000414 RID: 1044
		public delegate void OnSetAdminMenuActiveStateDelegate(bool showMenu);
	}
}
