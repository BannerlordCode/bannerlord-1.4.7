using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A8 RID: 168
	public class MultiplayerClassLoadoutVM : ViewModel
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x00030F06 File Offset: 0x0002F106
		private MissionRepresentativeBase missionRep
		{
			get
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				if (myPeer == null)
				{
					return null;
				}
				VirtualPlayer virtualPlayer = myPeer.VirtualPlayer;
				if (virtualPlayer == null)
				{
					return null;
				}
				return virtualPlayer.GetComponent<MissionRepresentativeBase>();
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00030F24 File Offset: 0x0002F124
		private Team _playerTeam
		{
			get
			{
				if (!GameNetwork.IsMyPeerReady)
				{
					return null;
				}
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component.Team == null || component.Team.Side == BattleSideEnum.None)
				{
					return null;
				}
				return component.Team;
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00030F64 File Offset: 0x0002F164
		public MultiplayerClassLoadoutVM(MissionMultiplayerGameModeBaseClient gameMode, Action<MultiplayerClassDivisions.MPHeroClass> onRefreshSelection, MultiplayerClassDivisions.MPHeroClass initialHeroSelection)
		{
			MBTextManager.SetTextVariable("newline", "\n", false);
			this._isInitializing = true;
			this._onRefreshSelection = onRefreshSelection;
			this._missionMultiplayerGameMode = gameMode;
			this._mission = gameMode.Mission;
			Team team = GameNetwork.MyPeer.GetComponent<MissionPeer>().Team;
			this.Classes = new MBBindingList<HeroClassGroupVM>();
			this.HeroInformation = new HeroInformationVM();
			this._enemyDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this._missionLobbyEquipmentNetworkComponent = Mission.Current.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this.IsGoldEnabled = this._missionMultiplayerGameMode.IsGameModeUsingGold;
			if (this.IsGoldEnabled)
			{
				this.Gold = this._missionMultiplayerGameMode.GetGoldAmount();
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = MultiplayerBattleColors.CreateWith(@object, object2).GetPeerColors(component);
			HeroClassVM heroClassVM = null;
			foreach (MultiplayerClassDivisions.MPHeroClassGroup mpheroClassGroup in MultiplayerClassDivisions.MultiplayerHeroClassGroups)
			{
				HeroClassGroupVM heroClassGroupVM = new HeroClassGroupVM(new Action<HeroClassVM>(this.RefreshCharacter), new Action<HeroPerkVM, MPPerkVM>(this.OnSelectPerk), mpheroClassGroup, peerColors);
				if (heroClassGroupVM.IsValid)
				{
					this.Classes.Add(heroClassGroupVM);
				}
			}
			int num = ((initialHeroSelection != null) ? ((!gameMode.IsGameModeUsingCasualGold) ? ((gameMode.GameType == MultiplayerGameType.Battle) ? initialHeroSelection.TroopBattleCost : initialHeroSelection.TroopCost) : initialHeroSelection.TroopCasualCost) : 0);
			if (initialHeroSelection == null || (this.IsGoldEnabled && num > this.Gold))
			{
				HeroClassGroupVM heroClassGroupVM2 = this.Classes.FirstOrDefault<HeroClassGroupVM>();
				heroClassVM = ((heroClassGroupVM2 != null) ? heroClassGroupVM2.SubClasses.FirstOrDefault<HeroClassVM>() : null);
			}
			else
			{
				foreach (HeroClassGroupVM heroClassGroupVM3 in this.Classes)
				{
					foreach (HeroClassVM heroClassVM2 in heroClassGroupVM3.SubClasses)
					{
						if (heroClassVM2.HeroClass == initialHeroSelection)
						{
							heroClassVM = heroClassVM2;
							break;
						}
					}
					if (heroClassVM != null)
					{
						break;
					}
				}
				if (heroClassVM == null)
				{
					HeroClassGroupVM heroClassGroupVM4 = this.Classes.FirstOrDefault<HeroClassGroupVM>();
					heroClassVM = ((heroClassGroupVM4 != null) ? heroClassGroupVM4.SubClasses.FirstOrDefault<HeroClassVM>() : null);
				}
			}
			this._isInitializing = false;
			this.RefreshCharacter(heroClassVM);
			this._teammateDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this.Teammates = new MBBindingList<MPPlayerVM>();
			this.Enemies = new MBBindingList<MPPlayerVM>();
			MissionPeer.OnEquipmentIndexRefreshed += this.RefreshPeerDivision;
			MissionPeer.OnPerkSelectionUpdated += this.RefreshPeerPerkSelection;
			NetworkCommunicator.OnPeerComponentAdded += this.OnPeerComponentAdded;
			BasicCultureObject culture = component.Culture;
			this.CultureId = culture.StringId;
			this.CultureColor1 = peerColors.Color1;
			this.CultureColor2 = peerColors.Color2;
			if (Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>())
			{
				this.ShowAttackerOrDefenderIcons = true;
				this.IsAttacker = team.Side == BattleSideEnum.Attacker;
			}
			this.RefreshValues();
			this._isTeammateAndEnemiesRelevant = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().IsGameModeTactical && !Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>() && Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().GameType != MultiplayerGameType.Battle;
			if (this._isTeammateAndEnemiesRelevant)
			{
				this.OnRefreshTeamMembers();
				this.OnRefreshEnemyMembers();
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000312EC File Offset: 0x0002F4EC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdateSpawnAndTimerLabels();
			string strValue = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			TextObject textObject = new TextObject("{=XJTX8w8M}Warmup Phase - {GAME_MODE}{newline}Waiting for players to join", null);
			textObject.SetTextVariable("GAME_MODE", GameTexts.FindText("str_multiplayer_official_game_type_name", strValue));
			this.WarmupInfoText = textObject.ToString();
			BasicCultureObject culture = GameNetwork.MyPeer.GetComponent<MissionPeer>().Culture;
			this.Culture = culture.Name.ToString();
			this.Classes.ApplyActionOnAllItems(delegate(HeroClassGroupVM x)
			{
				x.RefreshValues();
			});
			this.CurrentSelectedClass.RefreshValues();
			this.HeroInformation.RefreshValues();
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x000313A0 File Offset: 0x0002F5A0
		private void UpdateSpawnAndTimerLabels()
		{
			string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
			GameTexts.SetVariable("USE_KEY", keyHyperlinkText);
			this.SpawnLabelText = GameTexts.FindText("str_skirmish_battle_press_action_to_spawn", null).ToString();
			if (this._missionMultiplayerGameMode.RoundComponent != null)
			{
				if (!this._missionMultiplayerGameMode.IsInWarmup && !this._missionMultiplayerGameMode.IsRoundInProgress)
				{
					this.IsSpawnTimerVisible = true;
					return;
				}
				this.IsSpawnTimerVisible = false;
				this.IsSpawnLabelVisible = true;
				if (this._missionMultiplayerGameMode.IsRoundInProgress && (this._missionMultiplayerGameMode is MissionMultiplayerGameModeFlagDominationClient && this._missionMultiplayerGameMode.GameType == MultiplayerGameType.Skirmish) && GameNetwork.MyPeer.GetComponent<MissionPeer>() != null)
				{
					this.IsSpawnForfeitLabelVisible = true;
					string keyHyperlinkText2 = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", "ForfeitSpawn"), 1f);
					GameTexts.SetVariable("ALT_WEAP_KEY", keyHyperlinkText2);
					this.SpawnForfeitLabelText = GameTexts.FindText("str_skirmish_battle_press_alternative_to_forfeit_spawning", null).ToString();
					return;
				}
			}
			else
			{
				this.IsSpawnTimerVisible = false;
				this.IsSpawnLabelVisible = true;
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x000314B5 File Offset: 0x0002F6B5
		public override void OnFinalize()
		{
			base.OnFinalize();
			MissionPeer.OnEquipmentIndexRefreshed -= this.RefreshPeerDivision;
			MissionPeer.OnPerkSelectionUpdated -= this.RefreshPeerPerkSelection;
			NetworkCommunicator.OnPeerComponentAdded -= this.OnPeerComponentAdded;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x000314F0 File Offset: 0x0002F6F0
		private void RefreshCharacter(HeroClassVM heroClass)
		{
			if (this._isInitializing)
			{
				return;
			}
			foreach (HeroClassGroupVM heroClassGroupVM in this.Classes)
			{
				foreach (HeroClassVM heroClassVM in heroClassGroupVM.SubClasses)
				{
					heroClassVM.IsSelected = false;
				}
			}
			heroClass.IsSelected = true;
			this.CurrentSelectedClass = heroClass;
			if (GameNetwork.IsMyPeerReady)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				int num = MultiplayerClassDivisions.GetMPHeroClasses(heroClass.HeroClass.Culture).ToList<MultiplayerClassDivisions.MPHeroClass>().IndexOf(heroClass.HeroClass);
				component.NextSelectedTroopIndex = num;
			}
			this.HeroInformation.RefreshWith(heroClass.HeroClass, heroClass.SelectedPerks);
			this._missionLobbyEquipmentNetworkComponent.EquipmentUpdated();
			if (this._missionMultiplayerGameMode.IsGameModeUsingGold)
			{
				this.Gold = this._missionMultiplayerGameMode.GetGoldAmount();
			}
			List<IReadOnlyPerkObject> list = heroClass.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>();
			this.HeroInformation.RefreshWith(this.HeroInformation.HeroClass, list);
			List<Tuple<HeroPerkVM, MPPerkVM>> list2 = new List<Tuple<HeroPerkVM, MPPerkVM>>();
			foreach (HeroPerkVM heroPerkVM in heroClass.Perks)
			{
				list2.Add(new Tuple<HeroPerkVM, MPPerkVM>(heroPerkVM, heroPerkVM.SelectedPerkItem));
			}
			list2.ForEach(delegate(Tuple<HeroPerkVM, MPPerkVM> p)
			{
				this.OnSelectPerk(p.Item1, p.Item2);
			});
			Action<MultiplayerClassDivisions.MPHeroClass> onRefreshSelection = this._onRefreshSelection;
			if (onRefreshSelection == null)
			{
				return;
			}
			onRefreshSelection(heroClass.HeroClass);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x000316C4 File Offset: 0x0002F8C4
		private void OnSelectPerk(HeroPerkVM heroPerk, MPPerkVM candidate)
		{
			if (GameNetwork.IsMyPeerReady && this.HeroInformation.HeroClass != null && this.CurrentSelectedClass != null)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (!GameNetwork.IsServer || component.SelectPerk(heroPerk.PerkIndex, candidate.PerkIndex, -1))
				{
					this._missionLobbyEquipmentNetworkComponent.PerkUpdated(heroPerk.PerkIndex, candidate.PerkIndex);
				}
				List<IReadOnlyPerkObject> list = this.CurrentSelectedClass.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>();
				if (list.Count > 0)
				{
					this.HeroInformation.RefreshWith(this.HeroInformation.HeroClass, list);
				}
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003178C File Offset: 0x0002F98C
		public void RefreshPeerDivision(MissionPeer peer, int divisionType)
		{
			MPPlayerVM mpplayerVM = this.Teammates.FirstOrDefault<MPPlayerVM>((MPPlayerVM t) => t.Peer == peer);
			if (mpplayerVM != null)
			{
				mpplayerVM.RefreshDivision(false);
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x000317C8 File Offset: 0x0002F9C8
		private void RefreshPeerPerkSelection(MissionPeer peer)
		{
			MPPlayerVM mpplayerVM = this.Teammates.FirstOrDefault<MPPlayerVM>((MPPlayerVM t) => t.Peer == peer);
			if (mpplayerVM != null)
			{
				mpplayerVM.RefreshActivePerks();
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00031804 File Offset: 0x0002FA04
		public void Tick(float dt)
		{
			if (this._missionMultiplayerGameMode != null)
			{
				this.IsInWarmup = this._missionMultiplayerGameMode.IsInWarmup;
				this.IsGoldEnabled = !this.IsInWarmup && this._missionMultiplayerGameMode.IsGameModeUsingGold;
				if (this.IsGoldEnabled)
				{
					this.Gold = this._missionMultiplayerGameMode.GetGoldAmount();
				}
				foreach (HeroClassGroupVM heroClassGroupVM in this.Classes)
				{
					foreach (HeroClassVM heroClassVM in heroClassGroupVM.SubClasses)
					{
						heroClassVM.IsGoldEnabled = this.IsGoldEnabled;
					}
				}
			}
			this.RefreshRemainingTime();
			this._updateTimeElapsed += dt;
			if (this._updateTimeElapsed < 1f)
			{
				return;
			}
			this._updateTimeElapsed = 0f;
			if (this._isTeammateAndEnemiesRelevant)
			{
				this.OnRefreshTeamMembers();
				this.OnRefreshEnemyMembers();
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0003191C File Offset: 0x0002FB1C
		private void OnPeerComponentAdded(PeerComponent component)
		{
			if (component.IsMine && component is MissionRepresentativeBase)
			{
				this._isTeammateAndEnemiesRelevant = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().IsGameModeTactical && !Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>();
				if (this._isTeammateAndEnemiesRelevant)
				{
					this.OnRefreshTeamMembers();
					this.OnRefreshEnemyMembers();
				}
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00031974 File Offset: 0x0002FB74
		private void OnRefreshTeamMembers()
		{
			List<MPPlayerVM> list = this.Teammates.ToList<MPPlayerVM>();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this._playerTeam != null && missionPeer.Team == this._playerTeam)
				{
					if (!this._teammateDictionary.ContainsKey(missionPeer))
					{
						MPPlayerVM mpplayerVM = new MPPlayerVM(missionPeer);
						this.Teammates.Add(mpplayerVM);
						this._teammateDictionary.Add(missionPeer, mpplayerVM);
					}
					else
					{
						list.Remove(this._teammateDictionary[missionPeer]);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM2 in list)
			{
				this.Teammates.Remove(mpplayerVM2);
				this._teammateDictionary.Remove(mpplayerVM2.Peer);
			}
			foreach (MPPlayerVM mpplayerVM3 in this.Teammates)
			{
				if (mpplayerVM3.CompassElement == null)
				{
					mpplayerVM3.RefreshDivision(false);
				}
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00031AD8 File Offset: 0x0002FCD8
		private void OnRefreshEnemyMembers()
		{
			List<MPPlayerVM> list = this.Enemies.ToList<MPPlayerVM>();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this._playerTeam != null && missionPeer.Team != null && missionPeer.Team != this._playerTeam && missionPeer.Team != Mission.Current.SpectatorTeam)
				{
					if (!this._enemyDictionary.ContainsKey(missionPeer))
					{
						MPPlayerVM mpplayerVM = new MPPlayerVM(missionPeer);
						this.Enemies.Add(mpplayerVM);
						this._enemyDictionary.Add(missionPeer, mpplayerVM);
					}
					else
					{
						list.Remove(this._enemyDictionary[missionPeer]);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM2 in list)
			{
				this.Enemies.Remove(mpplayerVM2);
				this._enemyDictionary.Remove(mpplayerVM2.Peer);
			}
			foreach (MPPlayerVM mpplayerVM3 in this.Enemies)
			{
				mpplayerVM3.RefreshDivision(false);
				mpplayerVM3.UpdateDisabled();
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00031C54 File Offset: 0x0002FE54
		public void OnPeerEquipmentRefreshed(MissionPeer peer)
		{
			if (this._teammateDictionary.ContainsKey(peer))
			{
				this._teammateDictionary[peer].RefreshActivePerks();
				return;
			}
			if (this._enemyDictionary.ContainsKey(peer))
			{
				this._enemyDictionary[peer].RefreshActivePerks();
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00031CA0 File Offset: 0x0002FEA0
		public void OnGoldUpdated()
		{
			foreach (HeroClassGroupVM heroClassGroupVM in this.Classes)
			{
				heroClassGroupVM.SubClasses.ApplyActionOnAllItems(delegate(HeroClassVM sc)
				{
					sc.UpdateEnabled();
				});
			}
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00031D10 File Offset: 0x0002FF10
		public void RefreshRemainingTime()
		{
			int num = MathF.Ceiling(this._missionMultiplayerGameMode.RemainingTime);
			this.RemainingTimeText = TimeSpan.FromSeconds((double)num).ToString("mm':'ss");
			this.WarnRemainingTime = (float)num < 5f;
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00031D57 File Offset: 0x0002FF57
		// (set) Token: 0x06000FFF RID: 4095 RVA: 0x00031D5F File Offset: 0x0002FF5F
		[DataSourceProperty]
		public string Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value != this._culture)
				{
					this._culture = value;
					base.OnPropertyChangedWithValue<string>(value, "Culture");
				}
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x00031D82 File Offset: 0x0002FF82
		// (set) Token: 0x06001001 RID: 4097 RVA: 0x00031D8A File Offset: 0x0002FF8A
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x00031DAD File Offset: 0x0002FFAD
		// (set) Token: 0x06001003 RID: 4099 RVA: 0x00031DB5 File Offset: 0x0002FFB5
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x00031DD8 File Offset: 0x0002FFD8
		// (set) Token: 0x06001005 RID: 4101 RVA: 0x00031DE0 File Offset: 0x0002FFE0
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x00031E03 File Offset: 0x00030003
		// (set) Token: 0x06001007 RID: 4103 RVA: 0x00031E0B File Offset: 0x0003000B
		[DataSourceProperty]
		public bool IsSpawnTimerVisible
		{
			get
			{
				return this._isSpawnTimerVisible;
			}
			set
			{
				if (value != this._isSpawnTimerVisible)
				{
					this._isSpawnTimerVisible = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnTimerVisible");
				}
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x00031E29 File Offset: 0x00030029
		// (set) Token: 0x06001009 RID: 4105 RVA: 0x00031E31 File Offset: 0x00030031
		[DataSourceProperty]
		public string SpawnLabelText
		{
			get
			{
				return this._spawnLabelText;
			}
			set
			{
				if (value != this._spawnLabelText)
				{
					this._spawnLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpawnLabelText");
				}
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x00031E54 File Offset: 0x00030054
		// (set) Token: 0x0600100B RID: 4107 RVA: 0x00031E5C File Offset: 0x0003005C
		[DataSourceProperty]
		public bool IsSpawnLabelVisible
		{
			get
			{
				return this._isSpawnLabelVisible;
			}
			set
			{
				if (value != this._isSpawnLabelVisible)
				{
					this._isSpawnLabelVisible = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnLabelVisible");
				}
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x00031E7A File Offset: 0x0003007A
		// (set) Token: 0x0600100D RID: 4109 RVA: 0x00031E82 File Offset: 0x00030082
		[DataSourceProperty]
		public bool ShowAttackerOrDefenderIcons
		{
			get
			{
				return this._showAttackerOrDefenderIcons;
			}
			set
			{
				if (value != this._showAttackerOrDefenderIcons)
				{
					this._showAttackerOrDefenderIcons = value;
					base.OnPropertyChangedWithValue(value, "ShowAttackerOrDefenderIcons");
				}
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x00031EA0 File Offset: 0x000300A0
		// (set) Token: 0x0600100F RID: 4111 RVA: 0x00031EA8 File Offset: 0x000300A8
		[DataSourceProperty]
		public bool IsAttacker
		{
			get
			{
				return this._isAttacker;
			}
			set
			{
				if (value != this._isAttacker)
				{
					this._isAttacker = value;
					base.OnPropertyChangedWithValue(value, "IsAttacker");
				}
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x00031EC6 File Offset: 0x000300C6
		// (set) Token: 0x06001011 RID: 4113 RVA: 0x00031ECE File Offset: 0x000300CE
		[DataSourceProperty]
		public string SpawnForfeitLabelText
		{
			get
			{
				return this._spawnForfeitLabelText;
			}
			set
			{
				if (value != this._spawnForfeitLabelText)
				{
					this._spawnForfeitLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpawnForfeitLabelText");
				}
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x00031EF1 File Offset: 0x000300F1
		// (set) Token: 0x06001013 RID: 4115 RVA: 0x00031EF9 File Offset: 0x000300F9
		[DataSourceProperty]
		public bool IsSpawnForfeitLabelVisible
		{
			get
			{
				return this._isSpawnForfeitLabelVisible;
			}
			set
			{
				if (value != this._isSpawnForfeitLabelVisible)
				{
					this._isSpawnForfeitLabelVisible = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnForfeitLabelVisible");
				}
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x00031F17 File Offset: 0x00030117
		// (set) Token: 0x06001015 RID: 4117 RVA: 0x00031F1F File Offset: 0x0003011F
		[DataSourceProperty]
		public int Gold
		{
			get
			{
				return this._gold;
			}
			set
			{
				if (value != this._gold)
				{
					this._gold = value;
					base.OnPropertyChangedWithValue(value, "Gold");
				}
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x00031F3D File Offset: 0x0003013D
		// (set) Token: 0x06001017 RID: 4119 RVA: 0x00031F45 File Offset: 0x00030145
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Teammates
		{
			get
			{
				return this._teammates;
			}
			set
			{
				if (value != this._teammates)
				{
					this._teammates = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Teammates");
				}
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x00031F63 File Offset: 0x00030163
		// (set) Token: 0x06001019 RID: 4121 RVA: 0x00031F6B File Offset: 0x0003016B
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Enemies
		{
			get
			{
				return this._enemies;
			}
			set
			{
				if (value != this._enemies)
				{
					this._enemies = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x00031F89 File Offset: 0x00030189
		// (set) Token: 0x0600101B RID: 4123 RVA: 0x00031F91 File Offset: 0x00030191
		[DataSourceProperty]
		public HeroInformationVM HeroInformation
		{
			get
			{
				return this._heroInformation;
			}
			set
			{
				if (value != this._heroInformation)
				{
					this._heroInformation = value;
					base.OnPropertyChangedWithValue<HeroInformationVM>(value, "HeroInformation");
				}
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x00031FAF File Offset: 0x000301AF
		// (set) Token: 0x0600101D RID: 4125 RVA: 0x00031FB7 File Offset: 0x000301B7
		[DataSourceProperty]
		public HeroClassVM CurrentSelectedClass
		{
			get
			{
				return this._currentSelectedClass;
			}
			set
			{
				if (value != this._currentSelectedClass)
				{
					this._currentSelectedClass = value;
					base.OnPropertyChangedWithValue<HeroClassVM>(value, "CurrentSelectedClass");
				}
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x00031FD5 File Offset: 0x000301D5
		// (set) Token: 0x0600101F RID: 4127 RVA: 0x00031FDD File Offset: 0x000301DD
		[DataSourceProperty]
		public string RemainingTimeText
		{
			get
			{
				return this._remainingTimeText;
			}
			set
			{
				if (value != this._remainingTimeText)
				{
					this._remainingTimeText = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingTimeText");
				}
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x00032000 File Offset: 0x00030200
		// (set) Token: 0x06001021 RID: 4129 RVA: 0x00032008 File Offset: 0x00030208
		[DataSourceProperty]
		public bool WarnRemainingTime
		{
			get
			{
				return this._warnRemainingTime;
			}
			set
			{
				if (value != this._warnRemainingTime)
				{
					this._warnRemainingTime = value;
					base.OnPropertyChangedWithValue(value, "WarnRemainingTime");
				}
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x00032026 File Offset: 0x00030226
		// (set) Token: 0x06001023 RID: 4131 RVA: 0x0003202E File Offset: 0x0003022E
		[DataSourceProperty]
		public MBBindingList<HeroClassGroupVM> Classes
		{
			get
			{
				return this._classes;
			}
			set
			{
				if (value != this._classes)
				{
					this._classes = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroClassGroupVM>>(value, "Classes");
				}
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x0003204C File Offset: 0x0003024C
		// (set) Token: 0x06001025 RID: 4133 RVA: 0x00032054 File Offset: 0x00030254
		[DataSourceProperty]
		public bool IsGoldEnabled
		{
			get
			{
				return this._isGoldEnabled;
			}
			set
			{
				if (value != this._isGoldEnabled)
				{
					this._isGoldEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsGoldEnabled");
				}
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x00032072 File Offset: 0x00030272
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x0003207A File Offset: 0x0003027A
		[DataSourceProperty]
		public bool IsInWarmup
		{
			get
			{
				return this._isInWarmup;
			}
			set
			{
				if (value != this._isInWarmup)
				{
					this._isInWarmup = value;
					base.OnPropertyChangedWithValue(value, "IsInWarmup");
				}
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x00032098 File Offset: 0x00030298
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x000320A0 File Offset: 0x000302A0
		[DataSourceProperty]
		public string WarmupInfoText
		{
			get
			{
				return this._warmupInfoText;
			}
			set
			{
				if (value != this._warmupInfoText)
				{
					this._warmupInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarmupInfoText");
				}
			}
		}

		// Token: 0x04000768 RID: 1896
		public const float UPDATE_INTERVAL = 1f;

		// Token: 0x04000769 RID: 1897
		private float _updateTimeElapsed;

		// Token: 0x0400076A RID: 1898
		private readonly Action<MultiplayerClassDivisions.MPHeroClass> _onRefreshSelection;

		// Token: 0x0400076B RID: 1899
		private readonly MissionMultiplayerGameModeBaseClient _missionMultiplayerGameMode;

		// Token: 0x0400076C RID: 1900
		private Dictionary<MissionPeer, MPPlayerVM> _enemyDictionary;

		// Token: 0x0400076D RID: 1901
		private readonly Mission _mission;

		// Token: 0x0400076E RID: 1902
		private bool _isTeammateAndEnemiesRelevant;

		// Token: 0x0400076F RID: 1903
		private const float REMAINING_TIME_WARNING_THRESHOLD = 5f;

		// Token: 0x04000770 RID: 1904
		private MissionLobbyEquipmentNetworkComponent _missionLobbyEquipmentNetworkComponent;

		// Token: 0x04000771 RID: 1905
		private bool _isInitializing;

		// Token: 0x04000772 RID: 1906
		private Dictionary<MissionPeer, MPPlayerVM> _teammateDictionary;

		// Token: 0x04000773 RID: 1907
		private int _gold;

		// Token: 0x04000774 RID: 1908
		private string _culture;

		// Token: 0x04000775 RID: 1909
		private string _cultureId;

		// Token: 0x04000776 RID: 1910
		private string _spawnLabelText;

		// Token: 0x04000777 RID: 1911
		private string _spawnForfeitLabelText;

		// Token: 0x04000778 RID: 1912
		private string _remainingTimeText;

		// Token: 0x04000779 RID: 1913
		private bool _warnRemainingTime;

		// Token: 0x0400077A RID: 1914
		private bool _isSpawnTimerVisible;

		// Token: 0x0400077B RID: 1915
		private bool _isSpawnLabelVisible;

		// Token: 0x0400077C RID: 1916
		private bool _isSpawnForfeitLabelVisible;

		// Token: 0x0400077D RID: 1917
		private bool _isGoldEnabled;

		// Token: 0x0400077E RID: 1918
		private bool _isInWarmup;

		// Token: 0x0400077F RID: 1919
		private bool _showAttackerOrDefenderIcons;

		// Token: 0x04000780 RID: 1920
		private bool _isAttacker;

		// Token: 0x04000781 RID: 1921
		private string _warmupInfoText;

		// Token: 0x04000782 RID: 1922
		private Color _cultureColor1;

		// Token: 0x04000783 RID: 1923
		private Color _cultureColor2;

		// Token: 0x04000784 RID: 1924
		private MBBindingList<HeroClassGroupVM> _classes;

		// Token: 0x04000785 RID: 1925
		private HeroInformationVM _heroInformation;

		// Token: 0x04000786 RID: 1926
		private HeroClassVM _currentSelectedClass;

		// Token: 0x04000787 RID: 1927
		private MBBindingList<MPPlayerVM> _teammates;

		// Token: 0x04000788 RID: 1928
		private MBBindingList<MPPlayerVM> _enemies;
	}
}
