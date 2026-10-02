using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin.Internal;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000067 RID: 103
	public class DefaultAdminPanelOptionProvider : IAdminPanelOptionProvider
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0000E7F7 File Offset: 0x0000C9F7
		public DefaultAdminPanelOptionProvider(MultiplayerAdminComponent adminComponent, MissionLobbyComponent missionLobbyComponent)
		{
			this._multiplayerAdminComponent = adminComponent;
			this._missionLobbyComponent = missionLobbyComponent;
			this._optionGroups = new MBList<IAdminPanelOptionGroup>();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E818 File Offset: 0x0000CA18
		public void OnTick(float dt)
		{
			for (int i = 0; i < this._optionGroups.Count; i++)
			{
				IAdminPanelTickable adminPanelTickable;
				if ((adminPanelTickable = this._optionGroups[i] as IAdminPanelTickable) != null)
				{
					adminPanelTickable.OnTick(dt);
				}
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E858 File Offset: 0x0000CA58
		public void OnFinalize()
		{
			if (this._optionGroups != null)
			{
				for (int i = 0; i < this._optionGroups.Count; i++)
				{
					this._optionGroups[i].OnFinalize();
				}
			}
			this._gameTypeOption = null;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E89C File Offset: 0x0000CA9C
		public IAdminPanelOption GetOptionWithId(string id)
		{
			foreach (IAdminPanelOptionGroup adminPanelOptionGroup in this._optionGroups)
			{
				foreach (IAdminPanelOption adminPanelOption in adminPanelOptionGroup.Options)
				{
					if (adminPanelOption.UniqueId == id)
					{
						return adminPanelOption;
					}
				}
			}
			return null;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E938 File Offset: 0x0000CB38
		public IAdminPanelAction GetActionWithId(string id)
		{
			foreach (IAdminPanelOptionGroup adminPanelOptionGroup in this._optionGroups)
			{
				foreach (IAdminPanelAction adminPanelAction in adminPanelOptionGroup.Actions)
				{
					if (adminPanelAction.UniqueId == id)
					{
						return adminPanelAction;
					}
				}
			}
			return null;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E9D4 File Offset: 0x0000CBD4
		public void ApplyOptions()
		{
			AdminUpdateMultiplayerOptions adminUpdateMultiplayerOptions = new AdminUpdateMultiplayerOptions();
			IEnumerable<IAdminPanelOption> enumerable = this._optionGroups.SelectMany<IAdminPanelOptionGroup, IAdminPanelOption>((IAdminPanelOptionGroup x) => x.Options);
			foreach (IAdminPanelOption adminPanelOption in enumerable)
			{
				IAdminPanelOptionInternal adminPanelOptionInternal;
				if ((adminPanelOptionInternal = adminPanelOption as IAdminPanelOptionInternal) != null)
				{
					MultiplayerOptions.OptionType optionType = adminPanelOptionInternal.GetOptionType();
					MultiplayerOptions.MultiplayerOptionsAccessMode optionAccessMode = adminPanelOptionInternal.GetOptionAccessMode();
					if (optionType != MultiplayerOptions.OptionType.NumOfSlots && optionAccessMode != MultiplayerOptions.MultiplayerOptionsAccessMode.NumAccessModes)
					{
						IAdminPanelOption<bool> adminPanelOption2;
						if ((adminPanelOption2 = adminPanelOption as IAdminPanelOption<bool>) != null)
						{
							adminUpdateMultiplayerOptions.AddMultiplayerOption(optionType, optionAccessMode, adminPanelOption2.GetValue());
						}
						IAdminPanelOption<int> adminPanelOption3;
						if ((adminPanelOption3 = adminPanelOption as IAdminPanelOption<int>) != null)
						{
							adminUpdateMultiplayerOptions.AddMultiplayerOption(optionType, optionAccessMode, adminPanelOption3.GetValue());
						}
						IAdminPanelOption<string> adminPanelOption4;
						if ((adminPanelOption4 = adminPanelOption as IAdminPanelOption<string>) != null)
						{
							adminUpdateMultiplayerOptions.AddMultiplayerOption(optionType, optionAccessMode, adminPanelOption4.GetValue());
						}
						IAdminPanelMultiSelectionOption adminPanelMultiSelectionOption;
						if ((adminPanelMultiSelectionOption = adminPanelOption as IAdminPanelMultiSelectionOption) != null)
						{
							adminUpdateMultiplayerOptions.AddMultiplayerOption(optionType, optionAccessMode, adminPanelMultiSelectionOption.GetValue().Value);
						}
					}
				}
			}
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(adminUpdateMultiplayerOptions);
			GameNetwork.EndModuleEventAsClient();
			using (IEnumerator<IAdminPanelOption> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IAdminPanelOptionInternal adminPanelOptionInternal2;
					if ((adminPanelOptionInternal2 = enumerator.Current as IAdminPanelOptionInternal) != null)
					{
						adminPanelOptionInternal2.OnApplyChanges();
					}
				}
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000EB3C File Offset: 0x0000CD3C
		public MBReadOnlyList<IAdminPanelOptionGroup> GetOptionGroups()
		{
			this._optionGroups.Clear();
			if (MultiplayerIntermissionVotingManager.Instance.IsAutomatedBattleSwitchingEnabled)
			{
				this._optionGroups.Add(this.GetMissionOptions());
			}
			this._optionGroups.Add(this.GetImmediateEffectOptions());
			this._optionGroups.Add(this.GetActions());
			return this._optionGroups;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000EB9C File Offset: 0x0000CD9C
		private T GetValueFromOption<T>(string optionId)
		{
			IAdminPanelOption<T> adminPanelOption;
			if ((adminPanelOption = ((IAdminPanelOptionProvider)this).GetOptionWithId(optionId) as IAdminPanelOption<T>) != null)
			{
				return adminPanelOption.GetValue();
			}
			Debug.FailedAssert(string.Format("Failed to find \"{0}\" type option with id: {1}", typeof(T), optionId), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Admin\\DefaultAdminPanelOptionProvider.cs", "GetValueFromOption", 185);
			return default(T);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000EBF4 File Offset: 0x0000CDF4
		private AdminPanelOptionGroup GetMissionOptions()
		{
			AdminPanelOptionGroup adminPanelOptionGroup = new AdminPanelOptionGroup("mission_options", new TextObject("{=xa8i1dM1}Mission Options", null), true);
			AdminPanelOption<IAdminPanelMultiSelectionItem> adminPanelOption = new DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption("next_game_type").BuildAvailableOptions(MultiplayerOptions.OptionType.GameType, true).BuildOptionType(MultiplayerOptions.OptionType.GameType, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, false, false).BuildName(new TextObject("{=JPimShCw}Game Type", null))
				.BuildDescription(new TextObject("{=ueFrMu6i}Next game type.", null))
				.BuildIsRequired(true);
			this._gameTypeOption = adminPanelOption as DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption;
			adminPanelOptionGroup.AddOption(adminPanelOption);
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelUsableMapsOption("next_map").BuildGameTypeOption(adminPanelOption as DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption).BuildOptionType(MultiplayerOptions.OptionType.Map, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, false, false).BuildName(new TextObject("{=w9m11T1y}Map", null))
				.BuildDescription(new TextObject("{=ok1CD7dH}Next map to play.", null))
				.BuildIsRequired(true));
			DefaultAdminPanelOptionProvider.AdminPanelCultureOption adminPanelCultureOption = new DefaultAdminPanelOptionProvider.AdminPanelCultureOption("next_culture_team_1").BuildAvailableOptions(MultiplayerOptions.OptionType.CultureTeam1, true).BuildOptionType(MultiplayerOptions.OptionType.CultureTeam1, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, false, false).BuildName(new TextObject("{=sGDo0mxT}Attacker Culture", null))
				.BuildDescription(new TextObject("{=wsOUaxf4}Culture of the attacker team in the next game.", null))
				.BuildIsRequired(true) as DefaultAdminPanelOptionProvider.AdminPanelCultureOption;
			DefaultAdminPanelOptionProvider.AdminPanelCultureOption adminPanelCultureOption2 = new DefaultAdminPanelOptionProvider.AdminPanelCultureOption("next_culture_team_2").BuildAvailableOptions(MultiplayerOptions.OptionType.CultureTeam2, true).BuildOptionType(MultiplayerOptions.OptionType.CultureTeam2, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, false, false).BuildName(new TextObject("{=CeERJpan}Defender Culture", null))
				.BuildDescription(new TextObject("{=0jMXI0qT}Culture of the defender team in the next game.", null))
				.BuildIsRequired(true) as DefaultAdminPanelOptionProvider.AdminPanelCultureOption;
			adminPanelCultureOption.BuildOtherCultureOption(adminPanelCultureOption2);
			adminPanelCultureOption2.BuildOtherCultureOption(adminPanelCultureOption);
			adminPanelOptionGroup.AddOption(adminPanelCultureOption);
			adminPanelOptionGroup.AddOption(adminPanelCultureOption2);
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("next_number_of_rounds").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[]
			{
				MultiplayerGameType.TeamDeathmatch.ToString(),
				MultiplayerGameType.Duel.ToString(),
				MultiplayerGameType.Siege.ToString()
			}).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.RoundTotal)
				.BuildOptionType(MultiplayerOptions.OptionType.RoundTotal, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, true, true)
				.BuildName(new TextObject("{=VwveHldM}Number of Rounds", null))
				.BuildDescription(new TextObject("{=ndCjGgEj}Total number of rounds in the next game.", null))
				.BuildIsRequired(true));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("next_min_score_to_win_duel").BuildGameTypeOption(this._gameTypeOption).BuildRequiredGameTypes(new string[] { MultiplayerGameType.Duel.ToString() }).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.MinScoreToWinDuel)
				.BuildOptionType(MultiplayerOptions.OptionType.MinScoreToWinDuel, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, true, true)
				.BuildName(new TextObject("{=JISyGr4E}Minimum Score to Win Duel", null))
				.BuildDescription(new TextObject("{=5V30jDb7}Minimum score required to win duels.", null))
				.BuildIsRequired(true));
			adminPanelOptionGroup.AddOption(new AdminPanelNumericOption("next_map_time_limit").SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.MapTimeLimit).BuildOptionType(MultiplayerOptions.OptionType.MapTimeLimit, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, true, true).BuildName(new TextObject("{=lf1eQ0tB}Map Time Limit", null))
				.BuildDescription(new TextObject("{=xgps8dXU}Time limit in the next game.", null))
				.BuildIsRequired(true));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("next_round_time_limit").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[]
			{
				MultiplayerGameType.TeamDeathmatch.ToString(),
				MultiplayerGameType.Duel.ToString(),
				MultiplayerGameType.Siege.ToString()
			}).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.RoundTimeLimit)
				.BuildOptionType(MultiplayerOptions.OptionType.RoundTimeLimit, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, true, true)
				.BuildName(new TextObject("{=9k0H0xu0}Round Time Limit", null))
				.BuildDescription(new TextObject("{=ApQhQe6u}Round time limit in the next game.", null))
				.BuildIsRequired(true));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("next_warmup_time_limit").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[]
			{
				MultiplayerGameType.TeamDeathmatch.ToString(),
				MultiplayerGameType.Duel.ToString()
			}).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds)
				.BuildOptionType(MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, true, true)
				.BuildName(new TextObject("{=XwZTiF8l}Warmup Time Limit", null))
				.BuildDescription(new TextObject("{=S5Ayobba}Warmup time limit in the next game.", null))
				.BuildIsRequired(true));
			adminPanelOptionGroup.AddOption(new AdminPanelNumericOption("next_max_num_players").SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.MaxNumberOfPlayers).BuildOptionType(MultiplayerOptions.OptionType.MaxNumberOfPlayers, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions, true, true).BuildName(new TextObject("{=tzcK3R0v}Maximum Number of Players", null))
				.BuildDescription(new TextObject("{=RENeJbg5}Maximum number of players in the next game.", null))
				.BuildIsRequired(true));
			adminPanelOptionGroup.AddAction(new DefaultAdminPanelOptionProvider.AdminPanelStartMissionAction("apply_and_start").BuildOptionGroups(this._optionGroups).BuildName(new TextObject("{=kwo09aDm}Apply and Start Mission", null)).BuildDescription(new TextObject("{=8D8KuKxk}Apply all changes and start a new mission.", null))
				.BuildOnActionExecutedCallback(delegate
				{
					this.ApplyOptions();
					this._multiplayerAdminComponent.ChangeAdminMenuActiveState(false);
					this._multiplayerAdminComponent.AdminEndMission();
				}));
			return adminPanelOptionGroup;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000F064 File Offset: 0x0000D264
		private AdminPanelOptionGroup GetImmediateEffectOptions()
		{
			AdminPanelOptionGroup adminPanelOptionGroup = new AdminPanelOptionGroup("immediate_effects", new TextObject("{=TcBcNdSE}Immediate Effects", null), false);
			adminPanelOptionGroup.AddOption(new AdminPanelOption<string>("welcome_message").BuildOptionType(MultiplayerOptions.OptionType.WelcomeMessage, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, true, true).BuildName(new TextObject("{=t2Oh6uty}Welcome Message", null)).BuildDescription(new TextObject("{=v1DiZaoK}Change the server welcome message.", null)));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("auto_balance_treshold").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[] { MultiplayerGameType.Duel.ToString() }).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.AutoTeamBalanceThreshold)
				.BuildOptionType(MultiplayerOptions.OptionType.AutoTeamBalanceThreshold, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, true, true)
				.BuildName(new TextObject("{=YdnTEREg}Team Balance Threshold", null))
				.BuildDescription(new TextObject("{=DenCZPAg}Change the team balance threshold value.", null)));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("friendly_fire_melee_percent").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[] { MultiplayerGameType.Duel.ToString() }).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent)
				.BuildOptionType(MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, true, true)
				.BuildName(new TextObject("{=VpQZquwB}Friendly Melee Damage", null))
				.BuildDescription(new TextObject("{=3HgzxHqT}Change the value of friendly melee damage.", null)));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("friendly_fire_melee_self_percent").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[] { MultiplayerGameType.Duel.ToString() }).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent)
				.BuildOptionType(MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, true, true)
				.BuildName(new TextObject("{=wLTiwbBt}Friendly Reflective Melee Damage", null))
				.BuildDescription(new TextObject("{=daq8AjgZ}Change the value of reflective friendly melee damage.", null)));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("friendly_fire_ranged_percent").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[] { MultiplayerGameType.Duel.ToString() }).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent)
				.BuildOptionType(MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, true, true)
				.BuildName(new TextObject("{=pzudHx88}Friendly Ranged Damage", null))
				.BuildDescription(new TextObject("{=0H1Pg2RF}Change the value of friendly ranged damage.", null)));
			adminPanelOptionGroup.AddOption(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption("friendly_fire_ranged_self_percent").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[] { MultiplayerGameType.Duel.ToString() }).SetMinimumAndMaximumFrom(MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent)
				.BuildOptionType(MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, true, true)
				.BuildName(new TextObject("{=ZYw87dlh}Friendly Reflective Ranged Damage", null))
				.BuildDescription(new TextObject("{=ih2t4B8E}Change the value of reflective friendly ranged damage.", null)));
			adminPanelOptionGroup.AddOption(new AdminPanelOption<bool>("allow_infantry").BuildName(new TextObject("{=H72xVNwz}Allow Infantry", null)).BuildDescription(new TextObject("{=FB9tHuWF}Allow usage of infantry troops in game.", null)).BuildDefaultValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.Infantry))
				.BuildInitialValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.Infantry))
				.BuildOnAppliedCallback(delegate(bool val)
				{
					this._multiplayerAdminComponent.ChangeClassRestriction(FormationClass.Infantry, !val);
				}));
			adminPanelOptionGroup.AddOption(new AdminPanelOption<bool>("allow_ranged").BuildName(new TextObject("{=wFlbhObU}Allow Archers", null)).BuildDescription(new TextObject("{=3MiLBVAH}Allow usage of archer troops in game.", null)).BuildDefaultValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.Ranged))
				.BuildInitialValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.Ranged))
				.BuildOnAppliedCallback(delegate(bool val)
				{
					this._multiplayerAdminComponent.ChangeClassRestriction(FormationClass.Ranged, !val);
				}));
			adminPanelOptionGroup.AddOption(new AdminPanelOption<bool>("allow_cavalry").BuildName(new TextObject("{=nboyCQpj}Allow Cavalry", null)).BuildDescription(new TextObject("{=iTZkSZXI}Allow usage of cavalry troops in game.", null)).BuildDefaultValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.Cavalry))
				.BuildInitialValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.Cavalry))
				.BuildOnAppliedCallback(delegate(bool val)
				{
					this._multiplayerAdminComponent.ChangeClassRestriction(FormationClass.Cavalry, !val);
				}));
			adminPanelOptionGroup.AddOption(new AdminPanelOption<bool>("allow_horse_archers").BuildName(new TextObject("{=6yTHziN5}Allow Horse Archers", null)).BuildDescription(new TextObject("{=P8dk4qSf}Allow usage of horse archer troops in game.", null)).BuildDefaultValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.HorseArcher))
				.BuildInitialValue(this._missionLobbyComponent.IsClassAvailable(FormationClass.HorseArcher))
				.BuildOnAppliedCallback(delegate(bool val)
				{
					this._multiplayerAdminComponent.ChangeClassRestriction(FormationClass.HorseArcher, !val);
				}));
			return adminPanelOptionGroup;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000F45C File Offset: 0x0000D65C
		private AdminPanelOptionGroup GetActions()
		{
			AdminPanelOptionGroup adminPanelOptionGroup = new AdminPanelOptionGroup("actions", new TextObject("{=Za3U3MY4}Actions", null), false);
			adminPanelOptionGroup.AddAction(new DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentAction("end_warmup").BuildGameTypeOption(this._gameTypeOption).BuildInvalidGameTypes(new string[]
			{
				MultiplayerGameType.TeamDeathmatch.ToString(),
				MultiplayerGameType.Duel.ToString()
			}).BuildName(new TextObject("{=AVDDCWhv}End Warmup", null))
				.BuildDescription(new TextObject("{=Q6HPNb6Q}Set warmup timer to maximum of 30 seconds.", null))
				.BuildOnActionExecutedCallback(delegate
				{
					this._multiplayerAdminComponent.EndWarmup();
				}));
			adminPanelOptionGroup.AddAction(new AdminPanelAction("mute_player").BuildName(new TextObject("{=QvxOnnZg}Mute Players", null)).BuildDescription(new TextObject("{=qMJsMUtO}Select players to mute.", null)).BuildOnActionExecutedCallback(delegate
			{
				List<InquiryElement> list = new List<InquiryElement>();
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (!MultiplayerGlobalMutedPlayersManager.IsUserMuted(networkCommunicator.VirtualPlayer.Id))
					{
						list.Add(new InquiryElement(networkCommunicator, networkCommunicator.UserName, null));
					}
				}
				MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=QvxOnnZg}Mute Players", null).ToString(), new TextObject("{=qMJsMUtO}Select players to mute.", null).ToString(), list, true, 0, 1, new TextObject("{=SfJgnzdq}Mute", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), delegate(List<InquiryElement> selectedPlayers)
				{
					if (selectedPlayers != null && selectedPlayers.Count == 1)
					{
						NetworkCommunicator networkCommunicator2 = (NetworkCommunicator)selectedPlayers[0].Identifier;
						if (networkCommunicator2 != null)
						{
							this._multiplayerAdminComponent.GlobalMuteUnmutePlayer(networkCommunicator2, false);
						}
					}
				}, null, string.Empty, true), false, false);
			}));
			adminPanelOptionGroup.AddAction(new AdminPanelAction("mute_player").BuildName(new TextObject("{=NkDBzEzd}Unmute Players", null)).BuildDescription(new TextObject("{=9zJaIpIZ}Select players to unmute.", null)).BuildOnActionExecutedCallback(delegate
			{
				List<InquiryElement> list2 = new List<InquiryElement>();
				foreach (NetworkCommunicator networkCommunicator3 in GameNetwork.NetworkPeers)
				{
					if (MultiplayerGlobalMutedPlayersManager.IsUserMuted(networkCommunicator3.VirtualPlayer.Id))
					{
						list2.Add(new InquiryElement(networkCommunicator3, networkCommunicator3.UserName, null));
					}
				}
				MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=NkDBzEzd}Unmute Players", null).ToString(), new TextObject("{=9zJaIpIZ}Select players to unmute.", null).ToString(), list2, true, 0, 1, new TextObject("{=HyG3eUFN}Unmute", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), delegate(List<InquiryElement> selectedPlayers)
				{
					if (selectedPlayers != null && selectedPlayers.Count == 1)
					{
						NetworkCommunicator networkCommunicator4 = (NetworkCommunicator)selectedPlayers[0].Identifier;
						if (networkCommunicator4 != null)
						{
							this._multiplayerAdminComponent.GlobalMuteUnmutePlayer(networkCommunicator4, true);
						}
					}
				}, null, string.Empty, true), false, false);
			}));
			adminPanelOptionGroup.AddAction(new AdminPanelAction("kick_player").BuildName(new TextObject("{=cPbHqGrI}Kick Player", null)).BuildDescription(new TextObject("{=lZxxVl17}Select a player to kick.", null)).BuildOnActionExecutedCallback(delegate
			{
				List<InquiryElement> list3 = new List<InquiryElement>();
				foreach (NetworkCommunicator networkCommunicator5 in GameNetwork.NetworkPeers)
				{
					list3.Add(new InquiryElement(networkCommunicator5, networkCommunicator5.UserName, null));
				}
				MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=cPbHqGrI}Kick Player", null).ToString(), new TextObject("{=RKNTl0Tn}Select player to kick", null).ToString(), list3, true, 0, 1, new TextObject("{=DdOgvhsV}Kick", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), delegate(List<InquiryElement> selectedPlayers)
				{
					if (selectedPlayers != null && selectedPlayers.Count == 1)
					{
						NetworkCommunicator networkCommunicator6 = (NetworkCommunicator)selectedPlayers[0].Identifier;
						if (networkCommunicator6 != null)
						{
							this._multiplayerAdminComponent.KickPlayer(networkCommunicator6, false);
						}
					}
				}, null, string.Empty, true), false, false);
			}));
			adminPanelOptionGroup.AddAction(new AdminPanelAction("ban_player").BuildName(new TextObject("{=pbp0GQdO}Ban Player", null)).BuildDescription(new TextObject("{=aJGlM29l}Select a player to ban.", null)).BuildOnActionExecutedCallback(delegate
			{
				List<InquiryElement> list4 = new List<InquiryElement>();
				foreach (NetworkCommunicator networkCommunicator7 in GameNetwork.NetworkPeers)
				{
					list4.Add(new InquiryElement(networkCommunicator7, networkCommunicator7.UserName, null));
				}
				MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=pbp0GQdO}Ban Player", null).ToString(), new TextObject("{=jw2VQYeK}Select player to ban", null).ToString(), list4, true, 0, 1, new TextObject("{=HjqcmY6X}Ban", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), delegate(List<InquiryElement> selectedPlayers)
				{
					if (selectedPlayers != null && selectedPlayers.Count == 1)
					{
						NetworkCommunicator networkCommunicator8 = (NetworkCommunicator)selectedPlayers[0].Identifier;
						if (networkCommunicator8 != null)
						{
							this._multiplayerAdminComponent.KickPlayer(networkCommunicator8, true);
						}
					}
				}, null, string.Empty, true), false, false);
			}));
			return adminPanelOptionGroup;
		}

		// Token: 0x040000FB RID: 251
		private readonly MultiplayerAdminComponent _multiplayerAdminComponent;

		// Token: 0x040000FC RID: 252
		private readonly MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x040000FD RID: 253
		private MBList<IAdminPanelOptionGroup> _optionGroups;

		// Token: 0x040000FE RID: 254
		private DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption _gameTypeOption;

		// Token: 0x020000C9 RID: 201
		public static class DefaultOptionIds
		{
			// Token: 0x04000295 RID: 661
			public const string NextGameType = "next_game_type";

			// Token: 0x04000296 RID: 662
			public const string NextMap = "next_map";

			// Token: 0x04000297 RID: 663
			public const string NextCultureTeam1 = "next_culture_team_1";

			// Token: 0x04000298 RID: 664
			public const string NextCultureTeam2 = "next_culture_team_2";

			// Token: 0x04000299 RID: 665
			public const string NextNumberOfRounds = "next_number_of_rounds";

			// Token: 0x0400029A RID: 666
			public const string NextMinScoreToWinDuel = "next_min_score_to_win_duel";

			// Token: 0x0400029B RID: 667
			public const string NextMapTimeLimit = "next_map_time_limit";

			// Token: 0x0400029C RID: 668
			public const string NextRoundTimeLimit = "next_round_time_limit";

			// Token: 0x0400029D RID: 669
			public const string NextWarmupTimeLimit = "next_warmup_time_limit";

			// Token: 0x0400029E RID: 670
			public const string NextMaxNumberOfPlayers = "next_max_num_players";

			// Token: 0x0400029F RID: 671
			public const string ApplyAndStartMission = "apply_and_start";

			// Token: 0x040002A0 RID: 672
			public const string WelcomeMessage = "welcome_message";

			// Token: 0x040002A1 RID: 673
			public const string AutoTeamBalanceTreshold = "auto_balance_treshold";

			// Token: 0x040002A2 RID: 674
			public const string FriendlyFireMeleePercent = "friendly_fire_melee_percent";

			// Token: 0x040002A3 RID: 675
			public const string FriendlyFireMeleeReflectionPercent = "friendly_fire_melee_self_percent";

			// Token: 0x040002A4 RID: 676
			public const string FriendlyFireRangedPercent = "friendly_fire_ranged_percent";

			// Token: 0x040002A5 RID: 677
			public const string FriendlyFireRangedReflectionPercent = "friendly_fire_ranged_self_percent";

			// Token: 0x040002A6 RID: 678
			public const string AllowInfantry = "allow_infantry";

			// Token: 0x040002A7 RID: 679
			public const string AllowRanged = "allow_ranged";

			// Token: 0x040002A8 RID: 680
			public const string AllowCavalry = "allow_cavalry";

			// Token: 0x040002A9 RID: 681
			public const string AllowHorseArchers = "allow_horse_archers";

			// Token: 0x040002AA RID: 682
			public const string EndWarmup = "end_warmup";

			// Token: 0x040002AB RID: 683
			public const string MutePlayer = "mute_player";

			// Token: 0x040002AC RID: 684
			public const string KickPlayer = "kick_player";

			// Token: 0x040002AD RID: 685
			public const string BanPlayer = "ban_player";
		}

		// Token: 0x020000CA RID: 202
		private class AdminPanelVotableMultiSelectionOption : AdminPanelMultiSelectionOption
		{
			// Token: 0x17000062 RID: 98
			// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00014ADB File Offset: 0x00012CDB
			// (set) Token: 0x060004DA RID: 1242 RVA: 0x00014AE3 File Offset: 0x00012CE3
			public bool IsUndecided { get; private set; }

			// Token: 0x060004DB RID: 1243 RVA: 0x00014AEC File Offset: 0x00012CEC
			public AdminPanelVotableMultiSelectionOption(string uniqueId)
				: base(uniqueId)
			{
				this._undecidedOption = new AdminPanelMultiSelectionItem(null, new TextObject("{=b5HkM0tT}Undecided", null), true, false, true);
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x00014B0F File Offset: 0x00012D0F
			protected override void OnValueChanged(IAdminPanelMultiSelectionItem previousValue, IAdminPanelMultiSelectionItem newValue)
			{
				base.OnValueChanged(previousValue, newValue);
				this.IsUndecided = this._selectedOption == this._undecidedOption;
			}

			// Token: 0x060004DD RID: 1245 RVA: 0x00014B30 File Offset: 0x00012D30
			public override AdminPanelMultiSelectionOption BuildAvailableOptions(MBReadOnlyList<IAdminPanelMultiSelectionItem> options)
			{
				base.BuildAvailableOptions(options);
				this.AddUndecidedOption();
				if (!this._availableOptions.Contains(base.CurrentValue) && this._availableOptions.Count > 0)
				{
					base.BuildInitialValue(this._availableOptions[0]);
					base.SetValue(this._availableOptions[0]);
				}
				return this;
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x00014B94 File Offset: 0x00012D94
			public override AdminPanelMultiSelectionOption BuildAvailableOptions(MultiplayerOptions.OptionType optionType, bool buildDefaultValue = true)
			{
				base.BuildAvailableOptions(optionType, false);
				this.AddUndecidedOption();
				if (!this._availableOptions.Contains(base.CurrentValue) && this._availableOptions.Count > 0)
				{
					base.BuildInitialValue(this._availableOptions[0]);
					base.SetValue(this._availableOptions[0]);
				}
				return this;
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x00014BF8 File Offset: 0x00012DF8
			protected void AddUndecidedOption()
			{
				for (int i = 0; i < this._availableOptions.Count; i++)
				{
					if (this._availableOptions[i] == this._undecidedOption || this._availableOptions[i].Value == this._undecidedOption.Value)
					{
						return;
					}
				}
				string text;
				if (!this.GetIsDisabled(out text))
				{
					this._availableOptions.Insert(0, this._undecidedOption);
					base.BuildDefaultValue(this._undecidedOption);
					base.BuildInitialValue(this._undecidedOption);
					base.SetValue(this._undecidedOption);
				}
			}

			// Token: 0x060004E0 RID: 1248 RVA: 0x00014C98 File Offset: 0x00012E98
			protected void RemoveUndecidedOption()
			{
				bool flag = false;
				for (int i = 0; i < this._availableOptions.Count; i++)
				{
					if (this._availableOptions[i] == this._undecidedOption || this._availableOptions[i].Value == this._undecidedOption.Value)
					{
						this._availableOptions.RemoveAt(i);
						flag = true;
						break;
					}
				}
				if (flag && this._availableOptions.Count > 0)
				{
					IAdminPanelMultiSelectionItem adminPanelMultiSelectionItem = this._availableOptions[0];
					base.BuildDefaultValue(adminPanelMultiSelectionItem);
					base.BuildInitialValue(adminPanelMultiSelectionItem);
					base.SetValue(adminPanelMultiSelectionItem);
				}
			}

			// Token: 0x040002AF RID: 687
			protected readonly IAdminPanelMultiSelectionItem _undecidedOption;
		}

		// Token: 0x020000CB RID: 203
		private class AdminPanelCultureOption : DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption
		{
			// Token: 0x060004E1 RID: 1249 RVA: 0x00014D39 File Offset: 0x00012F39
			public AdminPanelCultureOption(string uniqueId)
				: base(uniqueId)
			{
			}

			// Token: 0x060004E2 RID: 1250 RVA: 0x00014D44 File Offset: 0x00012F44
			public DefaultAdminPanelOptionProvider.AdminPanelCultureOption BuildOtherCultureOption(DefaultAdminPanelOptionProvider.AdminPanelCultureOption otherOption)
			{
				DefaultAdminPanelOptionProvider.AdminPanelCultureOption otherOption2 = this._otherOption;
				if (otherOption2 != null)
				{
					otherOption2.RemoveValueChangedCallback(new Action(this.OnOtherOptionValueChanged));
				}
				this._otherOption = otherOption;
				DefaultAdminPanelOptionProvider.AdminPanelCultureOption otherOption3 = this._otherOption;
				if (otherOption3 != null)
				{
					otherOption3.AddValueChangedCallback(new Action(this.OnOtherOptionValueChanged));
				}
				return this;
			}

			// Token: 0x060004E3 RID: 1251 RVA: 0x00014D93 File Offset: 0x00012F93
			public override void OnFinalize()
			{
				base.OnFinalize();
				DefaultAdminPanelOptionProvider.AdminPanelCultureOption otherOption = this._otherOption;
				if (otherOption == null)
				{
					return;
				}
				otherOption.RemoveValueChangedCallback(new Action(this.OnOtherOptionValueChanged));
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x00014DB8 File Offset: 0x00012FB8
			protected override void OnValueChanged(IAdminPanelMultiSelectionItem previousValue, IAdminPanelMultiSelectionItem newValue)
			{
				bool isUndecided = base.IsUndecided;
				base.OnValueChanged(previousValue, newValue);
				if (isUndecided && !base.IsUndecided)
				{
					this._shouldKeepUndecidedOption = true;
					return;
				}
				if (!isUndecided && base.IsUndecided)
				{
					this._shouldKeepUndecidedOption = false;
				}
			}

			// Token: 0x060004E5 RID: 1253 RVA: 0x00014DF9 File Offset: 0x00012FF9
			private void OnOtherOptionValueChanged()
			{
				if (this._otherOption.IsUndecided)
				{
					base.AddUndecidedOption();
					return;
				}
				if (!this._shouldKeepUndecidedOption)
				{
					base.RemoveUndecidedOption();
				}
			}

			// Token: 0x040002B0 RID: 688
			private bool _shouldKeepUndecidedOption;

			// Token: 0x040002B1 RID: 689
			private DefaultAdminPanelOptionProvider.AdminPanelCultureOption _otherOption;
		}

		// Token: 0x020000CC RID: 204
		private class AdminPanelUsableMapsOption : DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption
		{
			// Token: 0x060004E6 RID: 1254 RVA: 0x00014E20 File Offset: 0x00013020
			public AdminPanelUsableMapsOption(string uniqueId)
				: base(uniqueId)
			{
				this._optionsByGameType = new Dictionary<string, MBList<IAdminPanelMultiSelectionItem>>();
				this._disabledOption = new AdminPanelMultiSelectionItem(null, new TextObject("{=1JlzQIXE}Disabled", null), false, true, true);
				this._optionsByGameType["map_option_disabled"] = new MBList<IAdminPanelMultiSelectionItem> { this._disabledOption };
				this._optionsByGameType["map_option_undecided"] = new MBList<IAdminPanelMultiSelectionItem> { this._undecidedOption };
			}

			// Token: 0x060004E7 RID: 1255 RVA: 0x00014E9B File Offset: 0x0001309B
			public DefaultAdminPanelOptionProvider.AdminPanelUsableMapsOption BuildGameTypeOption(DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption gameTypeOption)
			{
				this._gameTypeOption = gameTypeOption;
				DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption gameTypeOption2 = this._gameTypeOption;
				if (gameTypeOption2 != null)
				{
					gameTypeOption2.AddValueChangedCallback(new Action(this.UpdateOptions));
				}
				this.UpdateOptions();
				return this;
			}

			// Token: 0x060004E8 RID: 1256 RVA: 0x00014EC8 File Offset: 0x000130C8
			public override void OnFinalize()
			{
				base.OnFinalize();
				DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption gameTypeOption = this._gameTypeOption;
				if (gameTypeOption != null)
				{
					gameTypeOption.RemoveValueChangedCallback(new Action(this.UpdateOptions));
				}
				this._gameTypeOption = null;
			}

			// Token: 0x060004E9 RID: 1257 RVA: 0x00014EF4 File Offset: 0x000130F4
			public override bool GetIsDisabled(out string reason)
			{
				if (this._availableOptions.Count == 1 && this._availableOptions[0] == this._disabledOption)
				{
					reason = new TextObject("{=2WOGNYG4}No available maps added for game type", null).ToString();
					return true;
				}
				reason = string.Empty;
				return false;
			}

			// Token: 0x060004EA RID: 1258 RVA: 0x00014F34 File Offset: 0x00013134
			private void UpdateOptions()
			{
				if (this._isUpdatingOptions)
				{
					return;
				}
				this._isUpdatingOptions = true;
				IAdminPanelMultiSelectionItem value = this._gameTypeOption.GetValue();
				List<string> usableMaps = MultiplayerIntermissionVotingManager.Instance.GetUsableMaps(value.Value);
				this.FilterAvailableOptions(usableMaps);
				string text;
				if (this._gameTypeOption.IsUndecided)
				{
					text = "map_option_undecided";
				}
				else if (usableMaps != null && usableMaps.Count > 0)
				{
					text = value.Value;
				}
				else
				{
					text = "map_option_disabled";
				}
				MBList<IAdminPanelMultiSelectionItem> mblist;
				if (this._optionsByGameType.TryGetValue(text, out mblist))
				{
					if (!this._availableOptions.SequenceEqual<IAdminPanelMultiSelectionItem>(mblist))
					{
						this.BuildAvailableOptions(mblist);
					}
					this._isUpdatingOptions = false;
					return;
				}
				MBList<IAdminPanelMultiSelectionItem> mblist2 = new MBList<IAdminPanelMultiSelectionItem>();
				for (int i = 0; i < usableMaps.Count; i++)
				{
					AdminPanelMultiSelectionItem adminPanelMultiSelectionItem = new AdminPanelMultiSelectionItem(usableMaps[i], null, false, false, true);
					mblist2.Add(adminPanelMultiSelectionItem);
				}
				this.BuildAvailableOptions(mblist2);
				this._optionsByGameType[text] = mblist2;
				this._isUpdatingOptions = false;
			}

			// Token: 0x060004EB RID: 1259 RVA: 0x0001502C File Offset: 0x0001322C
			private void FilterAvailableOptions(List<string> availableOptions)
			{
				if (availableOptions.Count == 0)
				{
					return;
				}
				MBReadOnlyList<MultiplayerGameTypeInfo> multiplayerGameTypes = Module.CurrentModule.GetMultiplayerGameTypes();
				List<string> list = new List<string>();
				MultiplayerGameTypeInfo multiplayerGameTypeInfo = multiplayerGameTypes.FirstOrDefault<MultiplayerGameTypeInfo>(delegate(MultiplayerGameTypeInfo x)
				{
					string gameType = x.GameType;
					IAdminPanelMultiSelectionItem value = this._gameTypeOption.GetValue();
					return gameType == ((value != null) ? value.Value : null);
				});
				if (multiplayerGameTypeInfo == null)
				{
					return;
				}
				IEnumerable<string> enumerable = multiplayerGameTypes.SelectMany<MultiplayerGameTypeInfo, string>((MultiplayerGameTypeInfo g) => g.Scenes);
				for (int i = 0; i < availableOptions.Count; i++)
				{
					string text = availableOptions[i];
					if (enumerable.Contains(text) && !multiplayerGameTypeInfo.Scenes.Contains(text))
					{
						list.Add(text);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					string text2 = list[j];
					availableOptions.Remove(text2);
				}
			}

			// Token: 0x040002B2 RID: 690
			private const string _disabledOptionTag = "map_option_disabled";

			// Token: 0x040002B3 RID: 691
			private const string _undecidedOptionTag = "map_option_undecided";

			// Token: 0x040002B4 RID: 692
			private readonly Dictionary<string, MBList<IAdminPanelMultiSelectionItem>> _optionsByGameType;

			// Token: 0x040002B5 RID: 693
			private readonly IAdminPanelMultiSelectionItem _disabledOption;

			// Token: 0x040002B6 RID: 694
			private bool _isUpdatingOptions;

			// Token: 0x040002B7 RID: 695
			private DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption _gameTypeOption;
		}

		// Token: 0x020000CD RID: 205
		private class AdminPanelStartMissionAction : AdminPanelAction
		{
			// Token: 0x060004ED RID: 1261 RVA: 0x0001511C File Offset: 0x0001331C
			public AdminPanelStartMissionAction(string uniqueId)
				: base(uniqueId)
			{
			}

			// Token: 0x060004EE RID: 1262 RVA: 0x00015125 File Offset: 0x00013325
			public DefaultAdminPanelOptionProvider.AdminPanelStartMissionAction BuildOptionGroups(MBReadOnlyList<IAdminPanelOptionGroup> optionGroups)
			{
				this._optionGroups = optionGroups;
				return this;
			}

			// Token: 0x060004EF RID: 1263 RVA: 0x00015130 File Offset: 0x00013330
			public override bool GetIsDisabled(out string reason)
			{
				reason = string.Empty;
				if (this._optionGroups != null)
				{
					for (int i = 0; i < this._optionGroups.Count; i++)
					{
						for (int j = 0; j < this._optionGroups[i].Options.Count; j++)
						{
							IAdminPanelOption adminPanelOption = this._optionGroups[i].Options[j];
							string text;
							if (adminPanelOption.IsRequired && adminPanelOption.GetIsAvailable() && adminPanelOption.GetIsDisabled(out text))
							{
								reason = new TextObject("{=TrY4VS1R}Please select valid values for options.", null).ToString();
								return true;
							}
						}
					}
				}
				if (!MultiplayerIntermissionVotingManager.Instance.IsAutomatedBattleSwitchingEnabled)
				{
					reason = new TextObject("{=0WDSCBNa}Server does not support automated battle switching.", null).ToString();
					return true;
				}
				return false;
			}

			// Token: 0x060004F0 RID: 1264 RVA: 0x000151EA File Offset: 0x000133EA
			public override void OnFinalize()
			{
				base.OnFinalize();
				this._optionGroups = null;
			}

			// Token: 0x040002B8 RID: 696
			private MBReadOnlyList<IAdminPanelOptionGroup> _optionGroups;
		}

		// Token: 0x020000CE RID: 206
		private class AdminPanelGameTypeDependentNumericOption : AdminPanelNumericOption
		{
			// Token: 0x060004F1 RID: 1265 RVA: 0x000151F9 File Offset: 0x000133F9
			public AdminPanelGameTypeDependentNumericOption(string uniqueId)
				: base(uniqueId)
			{
			}

			// Token: 0x060004F2 RID: 1266 RVA: 0x00015204 File Offset: 0x00013404
			public override bool GetIsAvailable()
			{
				if (this._gameTypeOption == null)
				{
					Debug.Print("Game type option is not set for game type dependent option: " + base.Name, 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert("Game type option is not set for game type dependent option: " + base.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Admin\\DefaultAdminPanelOptionProvider.cs", "GetIsAvailable", 994);
					return true;
				}
				if (this._gameTypeOption.IsUndecided)
				{
					return true;
				}
				string value = this._gameTypeOption.GetValue().Value;
				if (string.IsNullOrEmpty(value))
				{
					return true;
				}
				if (this._invalidGameTypes != null)
				{
					return !this._invalidGameTypes.Contains(value);
				}
				return this._requiredGameTypes == null || this._requiredGameTypes.Contains(value);
			}

			// Token: 0x060004F3 RID: 1267 RVA: 0x000152B8 File Offset: 0x000134B8
			public DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption BuildGameTypeOption(DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption gameTypeOption)
			{
				this._gameTypeOption = gameTypeOption;
				return this;
			}

			// Token: 0x060004F4 RID: 1268 RVA: 0x000152C4 File Offset: 0x000134C4
			public DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption BuildInvalidGameTypes(string[] gameTypes)
			{
				this._invalidGameTypes = new List<string>();
				if (gameTypes != null)
				{
					for (int i = 0; i < gameTypes.Length; i++)
					{
						this._invalidGameTypes.Add(gameTypes[i]);
					}
				}
				return this;
			}

			// Token: 0x060004F5 RID: 1269 RVA: 0x000152FC File Offset: 0x000134FC
			public DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentNumericOption BuildRequiredGameTypes(string[] gameTypes)
			{
				this._requiredGameTypes = new List<string>();
				if (gameTypes != null)
				{
					for (int i = 0; i < gameTypes.Length; i++)
					{
						this._requiredGameTypes.Add(gameTypes[i]);
					}
				}
				return this;
			}

			// Token: 0x040002B9 RID: 697
			private DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption _gameTypeOption;

			// Token: 0x040002BA RID: 698
			private List<string> _invalidGameTypes;

			// Token: 0x040002BB RID: 699
			private List<string> _requiredGameTypes;
		}

		// Token: 0x020000CF RID: 207
		private class AdminPanelGameTypeDependentAction : AdminPanelAction
		{
			// Token: 0x060004F6 RID: 1270 RVA: 0x00015334 File Offset: 0x00013534
			public AdminPanelGameTypeDependentAction(string uniqueId)
				: base(uniqueId)
			{
			}

			// Token: 0x060004F7 RID: 1271 RVA: 0x00015340 File Offset: 0x00013540
			public override bool GetIsAvailable()
			{
				if (this._gameTypeOption == null)
				{
					Debug.Print("Game type option is not set for game type dependent option: " + base.Name, 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert("Game type option is not set for game type dependent option: " + base.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Admin\\DefaultAdminPanelOptionProvider.cs", "GetIsAvailable", 1080);
					return true;
				}
				if (this._gameTypeOption.IsUndecided)
				{
					return true;
				}
				string value = this._gameTypeOption.GetValue().Value;
				if (string.IsNullOrEmpty(value))
				{
					return true;
				}
				if (this._invalidGameTypes != null)
				{
					return !this._invalidGameTypes.Contains(value);
				}
				return this._requiredGameTypes == null || this._requiredGameTypes.Contains(value);
			}

			// Token: 0x060004F8 RID: 1272 RVA: 0x000153F4 File Offset: 0x000135F4
			public DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentAction BuildGameTypeOption(DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption gameTypeOption)
			{
				this._gameTypeOption = gameTypeOption;
				return this;
			}

			// Token: 0x060004F9 RID: 1273 RVA: 0x00015400 File Offset: 0x00013600
			public DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentAction BuildInvalidGameTypes(string[] gameTypes)
			{
				this._invalidGameTypes = new List<string>();
				if (gameTypes != null)
				{
					for (int i = 0; i < gameTypes.Length; i++)
					{
						this._invalidGameTypes.Add(gameTypes[i]);
					}
				}
				return this;
			}

			// Token: 0x060004FA RID: 1274 RVA: 0x00015438 File Offset: 0x00013638
			public DefaultAdminPanelOptionProvider.AdminPanelGameTypeDependentAction BuildRequiredGameTypes(string[] gameTypes)
			{
				this._requiredGameTypes = new List<string>();
				if (gameTypes != null)
				{
					for (int i = 0; i < gameTypes.Length; i++)
					{
						this._requiredGameTypes.Add(gameTypes[i]);
					}
				}
				return this;
			}

			// Token: 0x040002BC RID: 700
			private DefaultAdminPanelOptionProvider.AdminPanelVotableMultiSelectionOption _gameTypeOption;

			// Token: 0x040002BD RID: 701
			private List<string> _invalidGameTypes;

			// Token: 0x040002BE RID: 702
			private List<string> _requiredGameTypes;
		}
	}
}
