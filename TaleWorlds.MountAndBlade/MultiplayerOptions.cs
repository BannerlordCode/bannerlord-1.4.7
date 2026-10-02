using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000309 RID: 777
	public class MultiplayerOptions
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002C6A RID: 11370 RVA: 0x000AA591 File Offset: 0x000A8791
		public static MultiplayerOptions Instance
		{
			get
			{
				MultiplayerOptions multiplayerOptions;
				if ((multiplayerOptions = MultiplayerOptions._instance) == null)
				{
					multiplayerOptions = (MultiplayerOptions._instance = new MultiplayerOptions());
				}
				return multiplayerOptions;
			}
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000AA5A8 File Offset: 0x000A87A8
		public MultiplayerOptions()
		{
			this._default = new MultiplayerOptions.MultiplayerOptionsContainer();
			this._current = new MultiplayerOptions.MultiplayerOptionsContainer();
			this._next = new MultiplayerOptions.MultiplayerOptionsContainer();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				this._current.CreateOption(optionType);
				this._default.CreateOption(optionType);
			}
			MBReadOnlyList<MultiplayerGameTypeInfo> multiplayerGameTypes = Module.CurrentModule.GetMultiplayerGameTypes();
			if (multiplayerGameTypes.Count > 0)
			{
				MultiplayerGameTypeInfo multiplayerGameTypeInfo = multiplayerGameTypes[0];
				this._current.UpdateOptionValue(MultiplayerOptions.OptionType.GameType, multiplayerGameTypeInfo.GameType);
				this._current.UpdateOptionValue(MultiplayerOptions.OptionType.PremadeMatchGameMode, multiplayerGameTypes.First<MultiplayerGameTypeInfo>((MultiplayerGameTypeInfo info) => info.GameType == "Skirmish").GameType);
				this._current.UpdateOptionValue(MultiplayerOptions.OptionType.Map, multiplayerGameTypeInfo.Scenes.FirstOrDefault<string>());
			}
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.CultureTeam1, MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()[0].StringId);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.CultureTeam2, MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()[2].StringId);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MaxNumberOfPlayers, 120);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart, 1);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds, 300);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MapTimeLimit, 30);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RoundTimeLimit, 120);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RoundPreparationTimeLimit, 10);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RoundTotal, 1);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RespawnPeriodTeam1, 3);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RespawnPeriodTeam2, 3);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MinScoreToWinMatch, 120000);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.AutoTeamBalanceThreshold, 0);
			this._current.CopyAllValuesTo(this._next);
			this._current.CopyAllValuesTo(this._default);
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x000AA78B File Offset: 0x000A898B
		public static void Release()
		{
			MultiplayerOptions._instance = null;
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x000AA793 File Offset: 0x000A8993
		public MultiplayerOptions.MultiplayerOption GetOptionFromOptionType(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			return this.GetContainer(mode).GetOptionFromOptionType(optionType);
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000AA7A4 File Offset: 0x000A89A4
		public void OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			string text = "";
			if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
			{
				text = MultiplayerOptions.OptionType.GameType.GetStrValue(mode);
			}
			else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
			{
				text = MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(mode);
			}
			MultiplayerOptions.OptionType.DisableInactivityKick.SetValue(false, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			if (!(text == "TeamDeathmatch"))
			{
				if (!(text == "Duel"))
				{
					if (!(text == "Siege"))
					{
						if (!(text == "Captain"))
						{
							if (!(text == "Skirmish"))
							{
								if (text == "Battle")
								{
									this.InitializeForBattle(mode);
								}
							}
							else
							{
								this.InitializeForSkirmish(mode);
							}
						}
						else
						{
							this.InitializeForCaptain(mode);
						}
					}
					else
					{
						this.InitializeForSiege(mode);
					}
				}
				else
				{
					this.InitializeForDuel(mode);
				}
			}
			else
			{
				this.InitializeForTeamDeathmatch(mode);
			}
			MBList<string> mapList = this.GetMapList();
			if (mapList.Count > 0)
			{
				MultiplayerOptions.OptionType.Map.SetValue(mapList[0], MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x000AA888 File Offset: 0x000A8A88
		public void InitializeNextAndDefaultOptionContainers()
		{
			this._current.CopyAllValuesTo(this._next);
			this._current.CopyAllValuesTo(this._default);
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x000AA8AC File Offset: 0x000A8AAC
		private void InitializeForTeamDeathmatch(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "TeamDeathmatch";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(0, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.MinScoreToWinMatch.SetValue(120000, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x000AA950 File Offset: 0x000A8B50
		private void InitializeForDuel(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Duel";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(0, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(MultiplayerOptions.OptionType.MapTimeLimit.GetMaximumValue(), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(0, mode);
			MultiplayerOptions.OptionType.MinScoreToWinDuel.SetValue(3, mode);
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x000AA9F0 File Offset: 0x000A8BF0
		private void InitializeForSiege(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Siege";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(0, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(180, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(12, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(30, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x000AAA98 File Offset: 0x000A8C98
		private void InitializeForCaptain(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Captain";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(25, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(6, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(300, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(5, mode);
			MultiplayerOptions.OptionType.RoundTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text) * 60, mode);
			MultiplayerOptions.OptionType.RoundPreparationTimeLimit.SetValue(20, mode);
			MultiplayerOptions.OptionType.RoundTotal.SetValue(this.GetRoundCountForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			MultiplayerOptions.OptionType.AllowPollsToKickPlayers.SetValue(true, mode);
			MultiplayerOptions.OptionType.SingleSpawn.SetValue(true, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x000AAB74 File Offset: 0x000A8D74
		private void InitializeForSkirmish(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Skirmish";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(6, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(300, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(5, mode);
			MultiplayerOptions.OptionType.RoundTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text) * 60, mode);
			MultiplayerOptions.OptionType.RoundPreparationTimeLimit.SetValue(20, mode);
			MultiplayerOptions.OptionType.RoundTotal.SetValue(this.GetRoundCountForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			MultiplayerOptions.OptionType.AllowPollsToKickPlayers.SetValue(true, mode);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x000AAC44 File Offset: 0x000A8E44
		private void InitializeForBattle(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Battle";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(25, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(25, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(6, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(300, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(90, mode);
			MultiplayerOptions.OptionType.RoundTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text) * 60, mode);
			MultiplayerOptions.OptionType.RoundPreparationTimeLimit.SetValue(20, mode);
			MultiplayerOptions.OptionType.RoundTotal.SetValue(this.GetRoundCountForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			MultiplayerOptions.OptionType.SingleSpawn.SetValue(true, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000AAD18 File Offset: 0x000A8F18
		public int GetNumberOfPlayersForGameMode(string gameModeID)
		{
			if (gameModeID == "TeamDeathmatch" || gameModeID == "Siege" || gameModeID == "Battle")
			{
				return 120;
			}
			if (gameModeID == "Captain" || gameModeID == "Skirmish")
			{
				return 12;
			}
			if (!(gameModeID == "Duel"))
			{
				return 0;
			}
			return 32;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x000AAD80 File Offset: 0x000A8F80
		public int GetRoundCountForGameMode(string gameModeID)
		{
			if (gameModeID == "TeamDeathmatch" || gameModeID == "Siege" || gameModeID == "Duel")
			{
				return 1;
			}
			if (gameModeID == "Battle")
			{
				return 9;
			}
			if (!(gameModeID == "Captain") && !(gameModeID == "Skirmish"))
			{
				return 0;
			}
			return 5;
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000AADE8 File Offset: 0x000A8FE8
		public int GetRoundTimeLimitInMinutesForGameMode(string gameModeID)
		{
			if (gameModeID == "TeamDeathmatch" || gameModeID == "Siege" || gameModeID == "Duel")
			{
				return 30;
			}
			if (gameModeID == "Battle")
			{
				return 20;
			}
			if (gameModeID == "Captain")
			{
				return 10;
			}
			if (!(gameModeID == "Skirmish"))
			{
				return 0;
			}
			return 7;
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000AAE54 File Offset: 0x000A9054
		public void InitializeFromCommandList(List<string> arguments)
		{
			foreach (string text in arguments)
			{
				GameNetwork.HandleConsoleCommand(text);
			}
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000AAEA0 File Offset: 0x000A90A0
		public void ResetDefaultsToCurrent()
		{
			this._current.CopyAllValuesTo(this._default);
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000AAEB4 File Offset: 0x000A90B4
		public List<string> GetMultiplayerOptionsTextList(MultiplayerOptions.OptionType optionType)
		{
			List<string> list = new List<string>();
			string text = new TextObject("{=vBkrw5VV}Random", null).ToString();
			string text2 = "-- " + text + " --";
			switch (optionType)
			{
			case MultiplayerOptions.OptionType.PremadeMatchGameMode:
				return (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					where q.GameType == "Skirmish" || q.GameType == "Captain"
					select GameTexts.FindText("str_multiplayer_official_game_type_name", q.GameType).ToString()).ToList<string>();
			case MultiplayerOptions.OptionType.GameType:
				return (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					select GameTexts.FindText("str_multiplayer_official_game_type_name", q.GameType).ToString()).ToList<string>();
			case MultiplayerOptions.OptionType.PremadeGameType:
				break;
			case MultiplayerOptions.OptionType.Map:
			{
				List<string> list2 = new List<string>();
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
				{
					list2 = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)).Scenes.ToList<string>();
				}
				else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					list2 = this.GetAvailableClanMatchScenes();
					list.Insert(0, text2);
				}
				using (List<string>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text3 = enumerator.Current;
						TextObject textObject;
						string text4;
						if (GameTexts.TryGetText("str_multiplayer_scene_name", out textObject, text3))
						{
							text4 = textObject.ToString();
						}
						else
						{
							text4 = text3;
						}
						list.Add(text4);
					}
					return list;
				}
				break;
			}
			case MultiplayerOptions.OptionType.CultureTeam1:
			case MultiplayerOptions.OptionType.CultureTeam2:
				list = (from c in MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()
					where c.IsMainCulture
					select c into x
					select MultiplayerOptions.GetLocalizedCultureNameFromStringID(x.StringId)).ToList<string>();
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					list.Insert(0, text2);
					return list;
				}
				return list;
			default:
				if (optionType != MultiplayerOptions.OptionType.SpectatorCamera)
				{
					return this.GetMultiplayerOptionsList(optionType);
				}
				return new List<string>
				{
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToAnyAgent.ToString()).ToString(),
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToAnyPlayer.ToString()).ToString(),
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToTeamMembers.ToString()).ToString(),
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToTeamMembersView.ToString()).ToString()
				};
			}
			list = new List<string>
			{
				new TextObject("{=H5tiRTya}Practice", null).ToString(),
				new TextObject("{=YNkPy4ta}Clan Match", null).ToString()
			};
			return list;
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x000AB19C File Offset: 0x000A939C
		public List<string> GetMultiplayerOptionsList(MultiplayerOptions.OptionType optionType)
		{
			List<string> list = new List<string>();
			switch (optionType)
			{
			case MultiplayerOptions.OptionType.PremadeMatchGameMode:
				list = (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					select q.GameType).ToList<string>();
				list.Remove("TeamDeathmatch");
				list.Remove("Duel");
				list.Remove("Siege");
				break;
			case MultiplayerOptions.OptionType.GameType:
				list = (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					select q.GameType).ToList<string>();
				break;
			case MultiplayerOptions.OptionType.PremadeGameType:
				list = new List<string>
				{
					PremadeGameType.Practice.ToString(),
					PremadeGameType.Clan.ToString()
				};
				break;
			case MultiplayerOptions.OptionType.Map:
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
				{
					list = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)).Scenes.ToList<string>();
				}
				else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					list = this.GetAvailableClanMatchScenes();
					list.Insert(0, "RandomSelection");
				}
				break;
			case MultiplayerOptions.OptionType.CultureTeam1:
			case MultiplayerOptions.OptionType.CultureTeam2:
				list = (from c in MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()
					where c.IsMainCulture
					select c into x
					select x.StringId).ToList<string>();
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					list.Insert(0, Parameters.RandomSelectionString);
				}
				break;
			default:
				if (optionType == MultiplayerOptions.OptionType.SpectatorCamera)
				{
					list = new List<string>
					{
						SpectatorCameraTypes.LockToAnyAgent.ToString(),
						SpectatorCameraTypes.LockToAnyPlayer.ToString(),
						SpectatorCameraTypes.LockToTeamMembers.ToString(),
						SpectatorCameraTypes.LockToTeamMembersView.ToString()
					};
				}
				break;
			}
			return list;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x000AB3C4 File Offset: 0x000A95C4
		private List<string> GetAvailableClanMatchScenes()
		{
			string[] array = new string[0];
			string[] array2;
			if (NetworkMain.GameClient.AvailableScenes.ScenesByGameTypes.TryGetValue(MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), out array2))
			{
				array = array2;
			}
			return array.ToList<string>();
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x000AB400 File Offset: 0x000A9600
		private MultiplayerOptions.MultiplayerOptionsContainer GetContainer(MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			switch (mode)
			{
			case MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions:
				return this._default;
			case MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions:
				return this._current;
			case MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions:
				return this._next;
			default:
				return null;
			}
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x000AB42C File Offset: 0x000A962C
		public void InitializeAllOptionsFromNext()
		{
			this._next.CopyAllValuesTo(this._current);
			this.UpdateMbMultiplayerData(this._current);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000AB44C File Offset: 0x000A964C
		private void UpdateMbMultiplayerData(MultiplayerOptions.MultiplayerOptionsContainer container)
		{
			container.GetOptionFromOptionType(MultiplayerOptions.OptionType.ServerName).GetValue(out MBMultiplayerData.ServerName);
			if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
			{
				container.GetOptionFromOptionType(MultiplayerOptions.OptionType.GameType).GetValue(out MBMultiplayerData.GameType);
			}
			else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
			{
				container.GetOptionFromOptionType(MultiplayerOptions.OptionType.PremadeMatchGameMode).GetValue(out MBMultiplayerData.GameType);
			}
			container.GetOptionFromOptionType(MultiplayerOptions.OptionType.Map).GetValue(out MBMultiplayerData.Map);
			container.GetOptionFromOptionType(MultiplayerOptions.OptionType.MaxNumberOfPlayers).GetValue(out MBMultiplayerData.PlayerCountLimit);
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x000AB4C8 File Offset: 0x000A96C8
		public MBList<string> GetMapList()
		{
			MultiplayerGameTypeInfo multiplayerGameTypeInfo = null;
			if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
			{
				multiplayerGameTypeInfo = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			}
			else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
			{
				multiplayerGameTypeInfo = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			}
			MBList<string> mblist = new MBList<string>();
			if (multiplayerGameTypeInfo.Scenes.Count > 0)
			{
				mblist.Add(multiplayerGameTypeInfo.Scenes[0]);
				MultiplayerOptions.OptionType.Map.SetValue(mblist[0], MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			return mblist;
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x000AB53C File Offset: 0x000A973C
		public string GetValueTextForOptionWithMultipleSelection(MultiplayerOptions.OptionType optionType)
		{
			MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
			MultiplayerOptions.OptionValueType optionValueType = optionProperty.OptionValueType;
			if (optionValueType == MultiplayerOptions.OptionValueType.Enum)
			{
				return Enum.ToObject(optionProperty.EnumType, optionType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)).ToString();
			}
			if (optionValueType != MultiplayerOptions.OptionValueType.String)
			{
				return null;
			}
			return optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x000AB584 File Offset: 0x000A9784
		public void SetValueForOptionWithMultipleSelectionFromText(MultiplayerOptions.OptionType optionType, string value)
		{
			MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
			MultiplayerOptions.OptionValueType optionValueType = optionProperty.OptionValueType;
			if (optionValueType != MultiplayerOptions.OptionValueType.Enum)
			{
				if (optionValueType == MultiplayerOptions.OptionValueType.String)
				{
					optionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
			else
			{
				optionType.SetValue((int)Enum.Parse(optionProperty.EnumType, value), MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			if (optionType == MultiplayerOptions.OptionType.GameType || optionType == MultiplayerOptions.OptionType.PremadeMatchGameMode)
			{
				this.OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x000AB5DC File Offset: 0x000A97DC
		private static string GetLocalizedCultureNameFromStringID(string cultureID)
		{
			if (cultureID == "sturgia")
			{
				return new TextObject("{=PjO7oY16}Sturgia", null).ToString();
			}
			if (cultureID == "vlandia")
			{
				return new TextObject("{=FjwRsf1C}Vlandia", null).ToString();
			}
			if (cultureID == "battania")
			{
				return new TextObject("{=0B27RrYJ}Battania", null).ToString();
			}
			if (cultureID == "empire")
			{
				return new TextObject("{=empirefaction}Empire", null).ToString();
			}
			if (cultureID == "khuzait")
			{
				return new TextObject("{=sZLd6VHi}Khuzait", null).ToString();
			}
			if (!(cultureID == "aserai"))
			{
				Debug.FailedAssert("Unidentified culture id: " + cultureID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\MultiplayerOptions.cs", "GetLocalizedCultureNameFromStringID", 974);
				return "";
			}
			return new TextObject("{=aseraifaction}Aserai", null).ToString();
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x000AB6C4 File Offset: 0x000A98C4
		public static bool TryGetOptionTypeFromString(string optionTypeString, out MultiplayerOptions.OptionType optionType, out MultiplayerOptionsProperty optionAttribute)
		{
			optionAttribute = null;
			for (optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				if (optionProperty != null && optionType.ToString().Equals(optionTypeString))
				{
					optionAttribute = optionProperty;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001193 RID: 4499
		private const int PlayerCountLimitMin = 1;

		// Token: 0x04001194 RID: 4500
		private const int PlayerCountLimitMax = 1023;

		// Token: 0x04001195 RID: 4501
		private const int PlayerCountLimitForMatchStartMin = 0;

		// Token: 0x04001196 RID: 4502
		private const int PlayerCountLimitForMatchStartMax = 20;

		// Token: 0x04001197 RID: 4503
		private const int MapTimeLimitMin = 1;

		// Token: 0x04001198 RID: 4504
		private const int MapTimeLimitMax = 60;

		// Token: 0x04001199 RID: 4505
		private const int WarmupTimeLimitMin = 60;

		// Token: 0x0400119A RID: 4506
		private const int WarmupTimeLimitMax = 3600;

		// Token: 0x0400119B RID: 4507
		private const int RoundLimitMin = 1;

		// Token: 0x0400119C RID: 4508
		private const int RoundLimitMax = 99;

		// Token: 0x0400119D RID: 4509
		private const int RoundTimeLimitMin = 60;

		// Token: 0x0400119E RID: 4510
		private const int RoundTimeLimitMax = 3600;

		// Token: 0x0400119F RID: 4511
		private const int RoundPreparationTimeLimitMin = 2;

		// Token: 0x040011A0 RID: 4512
		private const int RoundPreparationTimeLimitMax = 60;

		// Token: 0x040011A1 RID: 4513
		private const int RespawnPeriodMin = 1;

		// Token: 0x040011A2 RID: 4514
		private const int RespawnPeriodMax = 60;

		// Token: 0x040011A3 RID: 4515
		private const int GoldGainChangePercentageMin = -100;

		// Token: 0x040011A4 RID: 4516
		private const int GoldGainChangePercentageMax = 100;

		// Token: 0x040011A5 RID: 4517
		private const int PollAcceptThresholdMin = 0;

		// Token: 0x040011A6 RID: 4518
		private const int PollAcceptThresholdMax = 10;

		// Token: 0x040011A7 RID: 4519
		private const int BotsPerTeamLimitMin = 0;

		// Token: 0x040011A8 RID: 4520
		private const int BotsPerTeamLimitMax = 510;

		// Token: 0x040011A9 RID: 4521
		private const int BotsPerFormationLimitMin = 0;

		// Token: 0x040011AA RID: 4522
		private const int BotsPerFormationLimitMax = 100;

		// Token: 0x040011AB RID: 4523
		private const int FriendlyFireDamagePercentMin = 0;

		// Token: 0x040011AC RID: 4524
		private const int FriendlyFireDamagePercentMax = 2000;

		// Token: 0x040011AD RID: 4525
		private const int GameDefinitionIdMin = -2147483648;

		// Token: 0x040011AE RID: 4526
		private const int GameDefinitionIdMax = 2147483647;

		// Token: 0x040011AF RID: 4527
		private const int MaxScoreToEndDuel = 7;

		// Token: 0x040011B0 RID: 4528
		private static MultiplayerOptions _instance;

		// Token: 0x040011B1 RID: 4529
		private readonly MultiplayerOptions.MultiplayerOptionsContainer _default;

		// Token: 0x040011B2 RID: 4530
		private readonly MultiplayerOptions.MultiplayerOptionsContainer _current;

		// Token: 0x040011B3 RID: 4531
		private readonly MultiplayerOptions.MultiplayerOptionsContainer _next;

		// Token: 0x040011B4 RID: 4532
		public MultiplayerOptions.OptionsCategory CurrentOptionsCategory;

		// Token: 0x020005EE RID: 1518
		public enum MultiplayerOptionsAccessMode
		{
			// Token: 0x04001FC4 RID: 8132
			DefaultMapOptions,
			// Token: 0x04001FC5 RID: 8133
			CurrentMapOptions,
			// Token: 0x04001FC6 RID: 8134
			NextMapOptions,
			// Token: 0x04001FC7 RID: 8135
			NumAccessModes
		}

		// Token: 0x020005EF RID: 1519
		public enum OptionValueType
		{
			// Token: 0x04001FC9 RID: 8137
			Bool,
			// Token: 0x04001FCA RID: 8138
			Integer,
			// Token: 0x04001FCB RID: 8139
			Enum,
			// Token: 0x04001FCC RID: 8140
			String
		}

		// Token: 0x020005F0 RID: 1520
		public enum OptionType
		{
			// Token: 0x04001FCE RID: 8142
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Changes the name of the server in the server list", 0, 0, null, false, null)]
			ServerName,
			// Token: 0x04001FCF RID: 8143
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Welcome messages which is shown to all players when they enter the server.", 0, 0, null, false, null)]
			WelcomeMessage,
			// Token: 0x04001FD0 RID: 8144
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.Never, "Sets a password that clients have to enter before connecting to the server.", 0, 0, null, false, null)]
			GamePassword,
			// Token: 0x04001FD1 RID: 8145
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.Never, "Sets a password that allows players access to admin tools during the game.", 0, 0, null, false, null)]
			AdminPassword,
			// Token: 0x04001FD2 RID: 8146
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Never, "Sets ID of the private game definition.", -2147483648, 2147483647, null, false, null)]
			GameDefinitionId,
			// Token: 0x04001FD3 RID: 8147
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to start polls to kick other players.", 0, 0, null, false, null)]
			AllowPollsToKickPlayers,
			// Token: 0x04001FD4 RID: 8148
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to start polls to ban other players.", 0, 0, null, false, null)]
			AllowPollsToBanPlayers,
			// Token: 0x04001FD5 RID: 8149
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to start polls to change the current map.", 0, 0, null, false, null)]
			AllowPollsToChangeMaps,
			// Token: 0x04001FD6 RID: 8150
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to use their custom banner.", 0, 0, null, false, null)]
			AllowIndividualBanners,
			// Token: 0x04001FD7 RID: 8151
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Use animation progress dependent blocking.", 0, 0, null, false, null)]
			UseRealisticBlocking,
			// Token: 0x04001FD8 RID: 8152
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Changes the game type.", 0, 0, null, true, null)]
			PremadeMatchGameMode,
			// Token: 0x04001FD9 RID: 8153
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Changes the game type.", 0, 0, null, true, null)]
			GameType,
			// Token: 0x04001FDA RID: 8154
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Enum, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Type of the premade game.", 0, 1, null, true, typeof(PremadeGameType))]
			PremadeGameType,
			// Token: 0x04001FDB RID: 8155
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Map of the game.", 0, 0, null, true, null)]
			Map,
			// Token: 0x04001FDC RID: 8156
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Sets culture for team 1", 0, 0, null, true, null)]
			CultureTeam1,
			// Token: 0x04001FDD RID: 8157
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Sets culture for team 2", 0, 0, null, true, null)]
			CultureTeam2,
			// Token: 0x04001FDE RID: 8158
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Set the maximum amount of player allowed on the server.", 1, 1023, null, false, null)]
			MaxNumberOfPlayers,
			// Token: 0x04001FDF RID: 8159
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Set the amount of players that are needed to start the first round. If not met, players will just wait.", 0, 20, null, false, null)]
			MinNumberOfPlayersForMatchStart,
			// Token: 0x04001FE0 RID: 8160
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Amount of bots on team 1", 0, 510, null, false, null)]
			NumberOfBotsTeam1,
			// Token: 0x04001FE1 RID: 8161
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Amount of bots on team 2", 0, 510, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			NumberOfBotsTeam2,
			// Token: 0x04001FE2 RID: 8162
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Amount of bots per formation", 0, 100, new string[] { "Captain" }, false, null)]
			NumberOfBotsPerFormation,
			// Token: 0x04001FE3 RID: 8163
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much melee damage inflicted upon a friend is dealt back to the inflictor.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageMeleeSelfPercent,
			// Token: 0x04001FE4 RID: 8164
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much melee damage inflicted upon a friend is actually dealt.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageMeleeFriendPercent,
			// Token: 0x04001FE5 RID: 8165
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much ranged damage inflicted upon a friend is dealt back to the inflictor.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageRangedSelfPercent,
			// Token: 0x04001FE6 RID: 8166
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much ranged damage inflicted upon a friend is actually dealt.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageRangedFriendPercent,
			// Token: 0x04001FE7 RID: 8167
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Enum, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Who can spectators look at, and how.", 0, 7, null, true, typeof(SpectatorCameraTypes))]
			SpectatorCamera,
			// Token: 0x04001FE8 RID: 8168
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum duration for the warmup. In seconds.", 60, 3600, null, false, null)]
			WarmupTimeLimitInSeconds,
			// Token: 0x04001FE9 RID: 8169
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum duration for the map. In minutes.", 1, 60, null, false, null)]
			MapTimeLimit,
			// Token: 0x04001FEA RID: 8170
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum duration for each round. In seconds.", 60, 3600, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege" }, false, null)]
			RoundTimeLimit,
			// Token: 0x04001FEB RID: 8171
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Time available to select class/equipment. In seconds.", 2, 60, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege" }, false, null)]
			RoundPreparationTimeLimit,
			// Token: 0x04001FEC RID: 8172
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum amount of rounds before the game ends.", 1, 99, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege" }, false, null)]
			RoundTotal,
			// Token: 0x04001FED RID: 8173
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Wait time after death, before respawning again. In seconds.", 1, 60, new string[] { "Siege" }, false, null)]
			RespawnPeriodTeam1,
			// Token: 0x04001FEE RID: 8174
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Wait time after death, before respawning again. In seconds.", 1, 60, new string[] { "Siege" }, false, null)]
			RespawnPeriodTeam2,
			// Token: 0x04001FEF RID: 8175
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Unlimited gold option.", 0, 0, new string[] { "Battle", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			UnlimitedGold,
			// Token: 0x04001FF0 RID: 8176
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Gold gain multiplier from agent deaths.", -100, 100, new string[] { "Siege", "TeamDeathmatch" }, false, null)]
			GoldGainChangePercentageTeam1,
			// Token: 0x04001FF1 RID: 8177
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Gold gain multiplier from agent deaths.", -100, 100, new string[] { "Siege", "TeamDeathmatch" }, false, null)]
			GoldGainChangePercentageTeam2,
			// Token: 0x04001FF2 RID: 8178
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Min score to win match.", 0, 1023000, new string[] { "TeamDeathmatch" }, false, null)]
			MinScoreToWinMatch,
			// Token: 0x04001FF3 RID: 8179
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Min score to win duel.", 0, 7, new string[] { "Duel" }, false, null)]
			MinScoreToWinDuel,
			// Token: 0x04001FF4 RID: 8180
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Minimum needed difference in poll results before it is accepted.", 0, 10, null, false, null)]
			PollAcceptThreshold,
			// Token: 0x04001FF5 RID: 8181
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Maximum player imbalance between team 1 and team 2. Selecting 0 will disable auto team balancing.", 0, 30, null, false, null)]
			AutoTeamBalanceThreshold,
			// Token: 0x04001FF6 RID: 8182
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Enables mission recording.", 0, 0, null, false, null)]
			EnableMissionRecording,
			// Token: 0x04001FF7 RID: 8183
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Sets if the game mode uses single spawning.", 0, 0, null, false, null)]
			SingleSpawn,
			// Token: 0x04001FF8 RID: 8184
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Disables the inactivity kick timer.", 0, 0, null, false, null)]
			DisableInactivityKick,
			// Token: 0x04001FF9 RID: 8185
			NumOfSlots
		}

		// Token: 0x020005F1 RID: 1521
		public enum OptionsCategory
		{
			// Token: 0x04001FFB RID: 8187
			Default,
			// Token: 0x04001FFC RID: 8188
			PremadeMatch
		}

		// Token: 0x020005F2 RID: 1522
		public class MultiplayerOption
		{
			// Token: 0x06003F1D RID: 16157 RVA: 0x000F6462 File Offset: 0x000F4662
			public static MultiplayerOptions.MultiplayerOption CreateMultiplayerOption(MultiplayerOptions.OptionType optionType)
			{
				return new MultiplayerOptions.MultiplayerOption(optionType);
			}

			// Token: 0x06003F1E RID: 16158 RVA: 0x000F646A File Offset: 0x000F466A
			public static MultiplayerOptions.MultiplayerOption CopyMultiplayerOption(MultiplayerOptions.MultiplayerOption option)
			{
				return new MultiplayerOptions.MultiplayerOption(option.OptionType)
				{
					_intValue = option._intValue,
					_stringValue = option._stringValue
				};
			}

			// Token: 0x06003F1F RID: 16159 RVA: 0x000F6490 File Offset: 0x000F4690
			private MultiplayerOption(MultiplayerOptions.OptionType optionType)
			{
				this.OptionType = optionType;
				if (optionType.GetOptionProperty().OptionValueType == MultiplayerOptions.OptionValueType.String)
				{
					this._intValue = MultiplayerOptions.MultiplayerOption.IntegerValue.Invalid;
					this._stringValue = MultiplayerOptions.MultiplayerOption.StringValue.Create();
					return;
				}
				this._intValue = MultiplayerOptions.MultiplayerOption.IntegerValue.Create();
				this._stringValue = MultiplayerOptions.MultiplayerOption.StringValue.Invalid;
			}

			// Token: 0x06003F20 RID: 16160 RVA: 0x000F64E5 File Offset: 0x000F46E5
			public MultiplayerOptions.MultiplayerOption UpdateValue(bool value)
			{
				this.UpdateValue(value ? 1 : 0);
				return this;
			}

			// Token: 0x06003F21 RID: 16161 RVA: 0x000F64F6 File Offset: 0x000F46F6
			public MultiplayerOptions.MultiplayerOption UpdateValue(int value)
			{
				this._intValue.UpdateValue(value);
				return this;
			}

			// Token: 0x06003F22 RID: 16162 RVA: 0x000F6505 File Offset: 0x000F4705
			public MultiplayerOptions.MultiplayerOption UpdateValue(string value)
			{
				this._stringValue.UpdateValue(value);
				return this;
			}

			// Token: 0x06003F23 RID: 16163 RVA: 0x000F6514 File Offset: 0x000F4714
			public void GetValue(out bool value)
			{
				value = this._intValue.Value == 1;
			}

			// Token: 0x06003F24 RID: 16164 RVA: 0x000F6526 File Offset: 0x000F4726
			public void GetValue(out int value)
			{
				value = this._intValue.Value;
			}

			// Token: 0x06003F25 RID: 16165 RVA: 0x000F6535 File Offset: 0x000F4735
			public void GetValue(out string value)
			{
				value = this._stringValue.Value;
			}

			// Token: 0x04001FFD RID: 8189
			public readonly MultiplayerOptions.OptionType OptionType;

			// Token: 0x04001FFE RID: 8190
			private MultiplayerOptions.MultiplayerOption.IntegerValue _intValue;

			// Token: 0x04001FFF RID: 8191
			private MultiplayerOptions.MultiplayerOption.StringValue _stringValue;

			// Token: 0x020006C3 RID: 1731
			private struct IntegerValue
			{
				// Token: 0x17000B09 RID: 2825
				// (get) Token: 0x0600422B RID: 16939 RVA: 0x000FCD7C File Offset: 0x000FAF7C
				public static MultiplayerOptions.MultiplayerOption.IntegerValue Invalid
				{
					get
					{
						return default(MultiplayerOptions.MultiplayerOption.IntegerValue);
					}
				}

				// Token: 0x17000B0A RID: 2826
				// (get) Token: 0x0600422C RID: 16940 RVA: 0x000FCD92 File Offset: 0x000FAF92
				// (set) Token: 0x0600422D RID: 16941 RVA: 0x000FCD9A File Offset: 0x000FAF9A
				public bool IsValid { get; private set; }

				// Token: 0x17000B0B RID: 2827
				// (get) Token: 0x0600422E RID: 16942 RVA: 0x000FCDA3 File Offset: 0x000FAFA3
				// (set) Token: 0x0600422F RID: 16943 RVA: 0x000FCDAB File Offset: 0x000FAFAB
				public int Value { get; private set; }

				// Token: 0x06004230 RID: 16944 RVA: 0x000FCDB4 File Offset: 0x000FAFB4
				public static MultiplayerOptions.MultiplayerOption.IntegerValue Create()
				{
					return new MultiplayerOptions.MultiplayerOption.IntegerValue
					{
						IsValid = true
					};
				}

				// Token: 0x06004231 RID: 16945 RVA: 0x000FCDD2 File Offset: 0x000FAFD2
				public void UpdateValue(int value)
				{
					this.Value = value;
				}
			}

			// Token: 0x020006C4 RID: 1732
			private struct StringValue
			{
				// Token: 0x17000B0C RID: 2828
				// (get) Token: 0x06004232 RID: 16946 RVA: 0x000FCDDC File Offset: 0x000FAFDC
				public static MultiplayerOptions.MultiplayerOption.StringValue Invalid
				{
					get
					{
						return default(MultiplayerOptions.MultiplayerOption.StringValue);
					}
				}

				// Token: 0x17000B0D RID: 2829
				// (get) Token: 0x06004233 RID: 16947 RVA: 0x000FCDF2 File Offset: 0x000FAFF2
				// (set) Token: 0x06004234 RID: 16948 RVA: 0x000FCDFA File Offset: 0x000FAFFA
				public bool IsValid { get; private set; }

				// Token: 0x17000B0E RID: 2830
				// (get) Token: 0x06004235 RID: 16949 RVA: 0x000FCE03 File Offset: 0x000FB003
				// (set) Token: 0x06004236 RID: 16950 RVA: 0x000FCE0B File Offset: 0x000FB00B
				public string Value { get; private set; }

				// Token: 0x06004237 RID: 16951 RVA: 0x000FCE14 File Offset: 0x000FB014
				public static MultiplayerOptions.MultiplayerOption.StringValue Create()
				{
					return new MultiplayerOptions.MultiplayerOption.StringValue
					{
						IsValid = true
					};
				}

				// Token: 0x06004238 RID: 16952 RVA: 0x000FCE32 File Offset: 0x000FB032
				public void UpdateValue(string value)
				{
					this.Value = value;
				}
			}
		}

		// Token: 0x020005F3 RID: 1523
		private class MultiplayerOptionsContainer
		{
			// Token: 0x06003F26 RID: 16166 RVA: 0x000F6544 File Offset: 0x000F4744
			public MultiplayerOptionsContainer()
			{
				this._multiplayerOptions = new MultiplayerOptions.MultiplayerOption[43];
			}

			// Token: 0x06003F27 RID: 16167 RVA: 0x000F6559 File Offset: 0x000F4759
			public MultiplayerOptions.MultiplayerOption GetOptionFromOptionType(MultiplayerOptions.OptionType optionType)
			{
				return this._multiplayerOptions[(int)optionType];
			}

			// Token: 0x06003F28 RID: 16168 RVA: 0x000F6563 File Offset: 0x000F4763
			private void CopyOptionFromOther(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOption option)
			{
				this._multiplayerOptions[(int)optionType] = MultiplayerOptions.MultiplayerOption.CopyMultiplayerOption(option);
			}

			// Token: 0x06003F29 RID: 16169 RVA: 0x000F6573 File Offset: 0x000F4773
			public void CreateOption(MultiplayerOptions.OptionType optionType)
			{
				this._multiplayerOptions[(int)optionType] = MultiplayerOptions.MultiplayerOption.CreateMultiplayerOption(optionType);
			}

			// Token: 0x06003F2A RID: 16170 RVA: 0x000F6583 File Offset: 0x000F4783
			public void UpdateOptionValue(MultiplayerOptions.OptionType optionType, int value)
			{
				this._multiplayerOptions[(int)optionType].UpdateValue(value);
			}

			// Token: 0x06003F2B RID: 16171 RVA: 0x000F6594 File Offset: 0x000F4794
			public void UpdateOptionValue(MultiplayerOptions.OptionType optionType, string value)
			{
				this._multiplayerOptions[(int)optionType].UpdateValue(value);
			}

			// Token: 0x06003F2C RID: 16172 RVA: 0x000F65A5 File Offset: 0x000F47A5
			public void UpdateOptionValue(MultiplayerOptions.OptionType optionType, bool value)
			{
				this._multiplayerOptions[(int)optionType].UpdateValue(value ? 1 : 0);
			}

			// Token: 0x06003F2D RID: 16173 RVA: 0x000F65BC File Offset: 0x000F47BC
			public void CopyAllValuesTo(MultiplayerOptions.MultiplayerOptionsContainer other)
			{
				for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
				{
					other.CopyOptionFromOther(optionType, this._multiplayerOptions[(int)optionType]);
				}
			}

			// Token: 0x04002000 RID: 8192
			private readonly MultiplayerOptions.MultiplayerOption[] _multiplayerOptions;
		}
	}
}
