using System;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x0200000C RID: 12
	public class MPPlayerVM : ViewModel
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003B90 File Offset: 0x00001D90
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00003B98 File Offset: 0x00001D98
		public MissionPeer Peer { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003BA4 File Offset: 0x00001DA4
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

		// Token: 0x0600008C RID: 140 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public MPPlayerVM(Agent agent)
		{
			if (agent != null)
			{
				MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character);
				TargetIconType targetIconType = ((mpheroClassForCharacter != null) ? mpheroClassForCharacter.IconType : TargetIconType.None);
				Team team = agent.Team;
				uint num = ((team != null) ? team.Color : 0U);
				Team team2 = agent.Team;
				uint num2 = ((team2 != null) ? team2.Color2 : 0U);
				Banner banner = new Banner(agent.Team.Banner, num, num2);
				this.CompassElement = new MPTeammateCompassTargetVM(targetIconType, num, num2, banner, false);
				return;
			}
			this.CompassElement = new MPTeammateCompassTargetVM(TargetIconType.Monster, 0U, 0U, Banner.CreateOneColoredEmptyBanner(0), false);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003C88 File Offset: 0x00001E88
		public MPPlayerVM(MissionPeer peer)
		{
			this.Peer = peer;
			this._gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._missionRepresentative = peer.GetComponent<MissionRepresentativeBase>();
			this._isInParty = NetworkMain.GameClient.IsInParty;
			this._isKnownPlayer = NetworkMain.GameClient.IsKnownPlayer(this.Peer.Peer.Id);
			this.RefreshAvatar();
			this.Name = peer.DisplayedName;
			this.ActivePerks = new MBBindingList<MPPerkVM>();
			this.RefreshValues();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003D22 File Offset: 0x00001F22
		public void UpdateDisabled()
		{
			this.IsDead = !this.Peer.IsControlledAgentActive;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003D38 File Offset: 0x00001F38
		public void RefreshDivision(bool useCultureColors = false)
		{
			if (this.Peer == null || this.Peer.Culture == null)
			{
				return;
			}
			MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(this.Peer, false);
			TargetIconType targetIconType = ((mpheroClassForPeer != null) ? mpheroClassForPeer.IconType : TargetIconType.None);
			if (this._cachedClass == null || this._cachedClass != mpheroClassForPeer || this._cachedCulture == null || this._cachedCulture != this.Peer.Culture)
			{
				this._cachedClass = mpheroClassForPeer;
				this._cachedCulture = this.Peer.Culture;
				Team team = this.Peer.Team;
				uint num = ((team != null) ? team.Color : 0U);
				Team team2 = this.Peer.Team;
				uint num2 = ((team2 != null) ? team2.Color2 : 0U);
				if (useCultureColors)
				{
					BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = MultiplayerBattleColors.CreateWith(@object, object2).GetPeerColors(this.Peer);
					num = peerColors.Color1Uint;
					num2 = peerColors.Color2Uint;
				}
				Banner banner = new Banner(this.Peer.Peer.BannerCode, num, num2);
				TargetIconType targetIconType2 = targetIconType;
				uint num3 = num;
				uint num4 = num2;
				Banner banner2 = banner;
				Team team3 = this.Peer.Team;
				this.CompassElement = new MPTeammateCompassTargetVM(targetIconType2, num3, num4, banner2, team3 != null && team3.IsPlayerAlly);
				this.HasSetCompassElement = true;
				this.Name = this.Peer.DisplayedName;
				this.RefreshActivePerks();
				this.CultureID = this._cachedCulture.StringId;
			}
			this.CompassElement.RefreshTargetIconType(targetIconType);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003EB4 File Offset: 0x000020B4
		public void RefreshGold()
		{
			if (this.Peer != null && this._gameMode.IsGameModeUsingGold)
			{
				FlagDominationMissionRepresentative flagDominationMissionRepresentative;
				if ((flagDominationMissionRepresentative = this._missionRepresentative as FlagDominationMissionRepresentative) != null)
				{
					this.Gold = flagDominationMissionRepresentative.Gold;
					this.IsSpawnActive = this.Gold >= 100;
					return;
				}
			}
			else
			{
				this.IsSpawnActive = false;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003F0C File Offset: 0x0000210C
		public void RefreshTeam()
		{
			if (this.Peer == null)
			{
				return;
			}
			string bannerCode = this.Peer.Peer.BannerCode;
			Team team = this.Peer.Team;
			uint num = ((team != null) ? team.Color : 0U);
			Team team2 = this.Peer.Team;
			Banner banner = new Banner(bannerCode, num, (team2 != null) ? team2.Color2 : 0U);
			MPTeammateCompassTargetVM compassElement = this.CompassElement;
			Banner banner2 = banner;
			Team team3 = this.Peer.Team;
			compassElement.RefreshTeam(banner2, team3 != null && team3.IsPlayerAlly);
			CompassTargetVM compassElement2 = this.CompassElement;
			Team team4 = this.Peer.Team;
			uint num2 = ((team4 != null) ? team4.Color : 0U);
			Team team5 = this.Peer.Team;
			compassElement2.RefreshColor(num2, (team5 != null) ? team5.Color2 : 0U);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003FC4 File Offset: 0x000021C4
		public void RefreshProperties()
		{
			bool flag = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0;
			MissionPeer peer = this.Peer;
			this.IsValueEnabled = (((peer != null) ? peer.Team : null) != null && this.Peer.Team == this._playerTeam) || flag;
			if (this.IsValueEnabled)
			{
				if (flag)
				{
					this.ValuePercent = ((this.Peer.BotsUnderControlTotal != 0) ? ((int)((float)this.Peer.BotsUnderControlAlive / (float)this.Peer.BotsUnderControlTotal * 100f)) : 0);
					return;
				}
				this.ValuePercent = ((this.Peer.ControlledAgent != null) ? MathF.Ceiling(this.Peer.ControlledAgent.Health / this.Peer.ControlledAgent.HealthLimit * 100f) : 0);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004093 File Offset: 0x00002293
		public void RefreshPreview(BasicCharacterObject character, DynamicBodyProperties dynamicBodyProperties, bool isFemale)
		{
			this.Preview = new MPArmoryHeroPreviewVM();
			this.Preview.SetCharacter(character, dynamicBodyProperties, character.Race, isFemale);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000040B4 File Offset: 0x000022B4
		public void RefreshActivePerks()
		{
			this.ActivePerks.Clear();
			MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(this.Peer, false);
			if (this.Peer != null && this.Peer.Culture != null && mpheroClassForPeer != null)
			{
				foreach (MPPerkObject mpperkObject in this.Peer.SelectedPerks)
				{
					this.ActivePerks.Add(new MPPerkVM(null, mpperkObject, false, 0));
				}
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000414C File Offset: 0x0000234C
		public void RefreshAvatar()
		{
			if (NetworkMain.GameClient == null)
			{
				Debug.FailedAssert("Network is not enabled when trying to refresh avatars", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\MPPlayerVM.cs", "RefreshAvatar", 205);
				return;
			}
			if (this.Peer == null)
			{
				Debug.FailedAssert("Trying to refresh avatar of a player without peer!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\MPPlayerVM.cs", "RefreshAvatar", 211);
				return;
			}
			int num;
			if (!NetworkMain.GameClient.HasUserGeneratedContentPrivilege)
			{
				num = AvatarServices.GetForcedAvatarIndexOfPlayer(this.Peer.Peer.Id);
			}
			else
			{
				num = ((!BannerlordConfig.EnableGenericAvatars || this._isKnownPlayer) ? (-1) : AvatarServices.GetForcedAvatarIndexOfPlayer(this.Peer.Peer.Id));
			}
			this.Avatar = new PlayerAvatarImageIdentifierVM(this.Peer.Peer.Id, num);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004204 File Offset: 0x00002404
		public void ExecuteFocusBegin()
		{
			this.SetFocusState(true);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000420D File Offset: 0x0000240D
		public void ExecuteFocusEnd()
		{
			this.SetFocusState(false);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004218 File Offset: 0x00002418
		private void SetFocusState(bool isFocused)
		{
			uint num = (isFocused ? 4278255612U : 0U);
			if (this.Peer != null)
			{
				IAgentVisual agentVisualForPeer = this.Peer.GetAgentVisualForPeer(0);
				if (agentVisualForPeer != null)
				{
					agentVisualForPeer.GetCopyAgentVisualsData().AgentVisuals.SetContourColor(new uint?(num), true);
				}
			}
			this.IsFocused = isFocused;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004267 File Offset: 0x00002467
		// (set) Token: 0x0600009A RID: 154 RVA: 0x0000426F File Offset: 0x0000246F
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000428D File Offset: 0x0000248D
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00004295 File Offset: 0x00002495
		[DataSourceProperty]
		public int ValuePercent
		{
			get
			{
				return this._valuePercent;
			}
			set
			{
				if (value != this._valuePercent)
				{
					this._valuePercent = value;
					base.OnPropertyChangedWithValue(value, "ValuePercent");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000042B3 File Offset: 0x000024B3
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000042BB File Offset: 0x000024BB
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000042DE File Offset: 0x000024DE
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000042E6 File Offset: 0x000024E6
		[DataSourceProperty]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureID");
				}
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004309 File Offset: 0x00002509
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004311 File Offset: 0x00002511
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (value != this._isDead)
				{
					this._isDead = value;
					base.OnPropertyChangedWithValue(value, "IsDead");
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000432F File Offset: 0x0000252F
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004337 File Offset: 0x00002537
		[DataSourceProperty]
		public bool IsValueEnabled
		{
			get
			{
				return this._isValueEnabled;
			}
			set
			{
				if (value != this._isValueEnabled)
				{
					this._isValueEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsValueEnabled");
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004355 File Offset: 0x00002555
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000435D File Offset: 0x0000255D
		[DataSourceProperty]
		public bool HasSetCompassElement
		{
			get
			{
				return this._hasSetCompassElement;
			}
			set
			{
				if (value != this._hasSetCompassElement)
				{
					this._hasSetCompassElement = value;
					base.OnPropertyChangedWithValue(value, "HasSetCompassElement");
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000437B File Offset: 0x0000257B
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00004383 File Offset: 0x00002583
		[DataSourceProperty]
		public bool IsSpawnActive
		{
			get
			{
				return this._isSpawnActive;
			}
			set
			{
				if (value != this._isSpawnActive)
				{
					this._isSpawnActive = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnActive");
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000043A1 File Offset: 0x000025A1
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000043A9 File Offset: 0x000025A9
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000043C7 File Offset: 0x000025C7
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000043CF File Offset: 0x000025CF
		[DataSourceProperty]
		public MPTeammateCompassTargetVM CompassElement
		{
			get
			{
				return this._compassElement;
			}
			set
			{
				if (value != this._compassElement)
				{
					this._compassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "CompassElement");
				}
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000043ED File Offset: 0x000025ED
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000043F5 File Offset: 0x000025F5
		[DataSourceProperty]
		public PlayerAvatarImageIdentifierVM Avatar
		{
			get
			{
				return this._avatar;
			}
			set
			{
				if (value != this._avatar)
				{
					this._avatar = value;
					base.OnPropertyChangedWithValue<PlayerAvatarImageIdentifierVM>(value, "Avatar");
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004413 File Offset: 0x00002613
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000441B File Offset: 0x0000261B
		[DataSourceProperty]
		public MPArmoryHeroPreviewVM Preview
		{
			get
			{
				return this._preview;
			}
			set
			{
				if (value != this._preview)
				{
					this._preview = value;
					base.OnPropertyChangedWithValue<MPArmoryHeroPreviewVM>(value, "Preview");
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004439 File Offset: 0x00002639
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00004441 File Offset: 0x00002641
		[DataSourceProperty]
		public MBBindingList<MPPerkVM> ActivePerks
		{
			get
			{
				return this._activePerks;
			}
			set
			{
				if (value != this._activePerks)
				{
					this._activePerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPerkVM>>(value, "ActivePerks");
				}
			}
		}

		// Token: 0x04000053 RID: 83
		private MultiplayerClassDivisions.MPHeroClass _cachedClass;

		// Token: 0x04000054 RID: 84
		private BasicCultureObject _cachedCulture;

		// Token: 0x04000055 RID: 85
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x04000056 RID: 86
		private readonly MissionRepresentativeBase _missionRepresentative;

		// Token: 0x04000057 RID: 87
		private readonly bool _isInParty;

		// Token: 0x04000058 RID: 88
		private readonly bool _isKnownPlayer;

		// Token: 0x04000059 RID: 89
		private TextObject _genericPlayerName = new TextObject("{=RN6zHak0}Player", null);

		// Token: 0x0400005A RID: 90
		private const uint _focusedContourColor = 4278255612U;

		// Token: 0x0400005B RID: 91
		private const uint _defaultContourColor = 0U;

		// Token: 0x0400005C RID: 92
		private const uint _invalidColor = 0U;

		// Token: 0x0400005D RID: 93
		private int _gold;

		// Token: 0x0400005E RID: 94
		private int _valuePercent;

		// Token: 0x0400005F RID: 95
		private string _name;

		// Token: 0x04000060 RID: 96
		private string _cultureID;

		// Token: 0x04000061 RID: 97
		private bool _isDead;

		// Token: 0x04000062 RID: 98
		private bool _isValueEnabled;

		// Token: 0x04000063 RID: 99
		private bool _hasSetCompassElement;

		// Token: 0x04000064 RID: 100
		private bool _isSpawnActive;

		// Token: 0x04000065 RID: 101
		private bool _isFocused;

		// Token: 0x04000066 RID: 102
		private MPTeammateCompassTargetVM _compassElement;

		// Token: 0x04000067 RID: 103
		private PlayerAvatarImageIdentifierVM _avatar;

		// Token: 0x04000068 RID: 104
		private MPArmoryHeroPreviewVM _preview;

		// Token: 0x04000069 RID: 105
		private MBBindingList<MPPerkVM> _activePerks;
	}
}
