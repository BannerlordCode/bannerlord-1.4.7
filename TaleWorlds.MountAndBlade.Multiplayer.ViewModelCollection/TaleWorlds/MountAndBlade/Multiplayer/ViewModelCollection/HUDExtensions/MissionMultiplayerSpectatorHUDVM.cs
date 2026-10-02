using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000095 RID: 149
	public class MissionMultiplayerSpectatorHUDVM : ViewModel
	{
		// Token: 0x06000EA9 RID: 3753 RVA: 0x0002D238 File Offset: 0x0002B438
		public MissionMultiplayerSpectatorHUDVM(Mission mission)
		{
			this._mission = mission;
			MissionLobbyComponent missionBehavior = mission.GetMissionBehavior<MissionLobbyComponent>();
			this._isTeamsEnabled = missionBehavior.MissionType != MultiplayerGameType.Duel;
			this._isFlagDominationMode = Mission.Current.HasMissionBehavior<MissionMultiplayerGameModeFlagDominationClient>();
			this.RefreshValues();
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0002D288 File Offset: 0x0002B488
		public override void RefreshValues()
		{
			base.RefreshValues();
			string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
			GameTexts.SetVariable("USE_KEY", keyHyperlinkText);
			this.TakeControlText = GameTexts.FindText("str_sergeant_battle_press_action_to_control_bot_2", null).ToString();
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002D2D3 File Offset: 0x0002B4D3
		public void Tick(float dt)
		{
			if (this._mission.MainAgent != null)
			{
				this.SpectatedPlayerNeutrality = -1;
			}
			this.UpdateDynamicProperties();
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
		private void UpdateDynamicProperties()
		{
			this.AgentHasShield = false;
			this.AgentHasMount = false;
			this.ShowAgentHealth = false;
			this.AgentHasRangedWeapon = false;
			if (this.SpectatedPlayerNeutrality > 0 && this._spectatedAgent != null)
			{
				this.ShowAgentHealth = true;
				this.SpectatedPlayerHealthLimit = this._spectatedAgent.HealthLimit;
				this.SpectatedPlayerCurrentHealth = this._spectatedAgent.Health;
				this.AgentHasMount = this._spectatedAgent.MountAgent != null;
				if (this.AgentHasMount)
				{
					this.SpectatedPlayerMountCurrentHealth = this._spectatedAgent.MountAgent.Health;
					this.SpectatedPlayerMountHealthLimit = this._spectatedAgent.MountAgent.HealthLimit;
				}
				EquipmentIndex primaryWieldedItemIndex = this._spectatedAgent.GetPrimaryWieldedItemIndex();
				EquipmentIndex offhandWieldedItemIndex = this._spectatedAgent.GetOffhandWieldedItemIndex();
				int num = -1;
				if (primaryWieldedItemIndex != EquipmentIndex.None && this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem != null)
				{
					if (this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon && this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsConsumable)
					{
						int ammoAmount = this._spectatedAgent.Equipment.GetAmmoAmount(primaryWieldedItemIndex);
						if (this._spectatedAgent.Equipment[primaryWieldedItemIndex].ModifiedMaxAmount == 1 || ammoAmount > 0)
						{
							num = ((this._spectatedAgent.Equipment[primaryWieldedItemIndex].ModifiedMaxAmount == 1) ? (-1) : ammoAmount);
						}
					}
					else if (this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon)
					{
						bool flag = this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == WeaponClass.Crossbow;
						num = this._spectatedAgent.Equipment.GetAmmoAmount(primaryWieldedItemIndex) + (int)(flag ? this._spectatedAgent.Equipment[primaryWieldedItemIndex].Ammo : 0);
					}
				}
				if (offhandWieldedItemIndex != EquipmentIndex.None && this._spectatedAgent.Equipment[offhandWieldedItemIndex].CurrentUsageItem != null)
				{
					MissionWeapon missionWeapon = this._spectatedAgent.Equipment[offhandWieldedItemIndex];
					this.AgentHasShield = missionWeapon.CurrentUsageItem.IsShield;
					if (this.AgentHasShield)
					{
						this.SpectatedPlayerShieldHealthLimit = (float)missionWeapon.ModifiedMaxHitPoints;
						this.SpectatedPlayerShieldCurrentHealth = (float)missionWeapon.HitPoints;
					}
				}
				this.AgentHasRangedWeapon = num >= 0;
				this.SpectatedPlayerAmmoAmount = num;
			}
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0002D570 File Offset: 0x0002B770
		internal void OnSpectatedAgentFocusIn(Agent followedAgent)
		{
			this._spectatedAgent = followedAgent;
			int num = 0;
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (component != null && component.Team != this._mission.SpectatorTeam && component.Team == followedAgent.Team && this._isTeamsEnabled)
			{
				num = 1;
			}
			this.SpectatedPlayerNeutrality = num;
			MissionPeer missionPeer = followedAgent.MissionPeer;
			this.SpectatedPlayerName = ((missionPeer != null) ? missionPeer.DisplayedName : null) ?? followedAgent.Name.ToString();
			this.CanTakeControlOfSpectatedAgent = this._isFlagDominationMode && ((component != null) ? component.ControlledFormation : null) != null && component.ControlledFormation == followedAgent.Formation;
			this.CompassElement = null;
			this.AgentHasCompassElement = false;
			MissionPeer missionPeer2;
			if ((missionPeer2 = followedAgent.MissionPeer) == null)
			{
				Formation formation = followedAgent.Formation;
				if (formation == null)
				{
					missionPeer2 = null;
				}
				else
				{
					Agent playerOwner = formation.PlayerOwner;
					missionPeer2 = ((playerOwner != null) ? playerOwner.MissionPeer : null);
				}
			}
			MissionPeer missionPeer3 = missionPeer2;
			if (missionPeer3 != null)
			{
				MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(missionPeer3, false);
				TargetIconType targetIconType = ((mpheroClassForPeer != null) ? mpheroClassForPeer.IconType : TargetIconType.None);
				Banner banner = new Banner(missionPeer3.Peer.BannerCode, missionPeer3.Team.Color, missionPeer3.Team.Color2);
				this.CompassElement = new MPTeammateCompassTargetVM(targetIconType, missionPeer3.Team.Color, missionPeer3.Team.Color2, banner, missionPeer3.Team.IsPlayerAlly);
				this.AgentHasCompassElement = true;
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0002D6C9 File Offset: 0x0002B8C9
		internal void OnSpectatedAgentFocusOut(Agent followedPeer)
		{
			this._spectatedAgent = null;
			this.SpectatedPlayerNeutrality = -1;
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0002D6D9 File Offset: 0x0002B8D9
		// (set) Token: 0x06000EB0 RID: 3760 RVA: 0x0002D6E1 File Offset: 0x0002B8E1
		[DataSourceProperty]
		public int SpectatedPlayerNeutrality
		{
			get
			{
				return this._spectatedPlayerNeutrality;
			}
			set
			{
				if (value != this._spectatedPlayerNeutrality)
				{
					this._spectatedPlayerNeutrality = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerNeutrality");
					this.IsSpectatingAgent = value >= 0;
				}
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x0002D70C File Offset: 0x0002B90C
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x0002D714 File Offset: 0x0002B914
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

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0002D732 File Offset: 0x0002B932
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x0002D73A File Offset: 0x0002B93A
		[DataSourceProperty]
		public bool IsSpectatingAgent
		{
			get
			{
				return this._isSpectatingPlayer;
			}
			set
			{
				if (value != this._isSpectatingPlayer)
				{
					this._isSpectatingPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsSpectatingAgent");
				}
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0002D758 File Offset: 0x0002B958
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x0002D760 File Offset: 0x0002B960
		[DataSourceProperty]
		public bool AgentHasCompassElement
		{
			get
			{
				return this._agentHasCompassElement;
			}
			set
			{
				if (value != this._agentHasCompassElement)
				{
					this._agentHasCompassElement = value;
					base.OnPropertyChangedWithValue(value, "AgentHasCompassElement");
				}
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0002D77E File Offset: 0x0002B97E
		// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x0002D786 File Offset: 0x0002B986
		[DataSourceProperty]
		public bool AgentHasMount
		{
			get
			{
				return this._agentHasMount;
			}
			set
			{
				if (value != this._agentHasMount)
				{
					this._agentHasMount = value;
					base.OnPropertyChangedWithValue(value, "AgentHasMount");
				}
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0002D7A4 File Offset: 0x0002B9A4
		// (set) Token: 0x06000EBA RID: 3770 RVA: 0x0002D7AC File Offset: 0x0002B9AC
		[DataSourceProperty]
		public bool ShowAgentHealth
		{
			get
			{
				return this._showAgentHealth;
			}
			set
			{
				if (value != this._showAgentHealth)
				{
					this._showAgentHealth = value;
					base.OnPropertyChangedWithValue(value, "ShowAgentHealth");
				}
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0002D7CA File Offset: 0x0002B9CA
		// (set) Token: 0x06000EBC RID: 3772 RVA: 0x0002D7D2 File Offset: 0x0002B9D2
		[DataSourceProperty]
		public bool AgentHasRangedWeapon
		{
			get
			{
				return this._agentHasRangedWeapon;
			}
			set
			{
				if (value != this._agentHasRangedWeapon)
				{
					this._agentHasRangedWeapon = value;
					base.OnPropertyChangedWithValue(value, "AgentHasRangedWeapon");
				}
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0002D7F0 File Offset: 0x0002B9F0
		// (set) Token: 0x06000EBE RID: 3774 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
		[DataSourceProperty]
		public bool AgentHasShield
		{
			get
			{
				return this._agentHasShield;
			}
			set
			{
				if (value != this._agentHasShield)
				{
					this._agentHasShield = value;
					base.OnPropertyChangedWithValue(value, "AgentHasShield");
				}
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0002D816 File Offset: 0x0002BA16
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x0002D81E File Offset: 0x0002BA1E
		[DataSourceProperty]
		public bool CanTakeControlOfSpectatedAgent
		{
			get
			{
				return this._canTakeControlOfSpectatedAgent;
			}
			set
			{
				if (value != this._canTakeControlOfSpectatedAgent)
				{
					this._canTakeControlOfSpectatedAgent = value;
					base.OnPropertyChangedWithValue(value, "CanTakeControlOfSpectatedAgent");
				}
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0002D83C File Offset: 0x0002BA3C
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x0002D844 File Offset: 0x0002BA44
		[DataSourceProperty]
		public string SpectatedPlayerName
		{
			get
			{
				return this._spectatedPlayerName;
			}
			set
			{
				if (value != this._spectatedPlayerName)
				{
					this._spectatedPlayerName = value;
					base.OnPropertyChangedWithValue<string>(value, "SpectatedPlayerName");
				}
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0002D867 File Offset: 0x0002BA67
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x0002D86F File Offset: 0x0002BA6F
		[DataSourceProperty]
		public string TakeControlText
		{
			get
			{
				return this._takeControlText;
			}
			set
			{
				if (value != this._takeControlText)
				{
					this._takeControlText = value;
					base.OnPropertyChangedWithValue<string>(value, "TakeControlText");
				}
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0002D892 File Offset: 0x0002BA92
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0002D89A File Offset: 0x0002BA9A
		[DataSourceProperty]
		public float SpectatedPlayerHealthLimit
		{
			get
			{
				return this._spectatedPlayerHealthLimit;
			}
			set
			{
				if (value != this._spectatedPlayerHealthLimit)
				{
					this._spectatedPlayerHealthLimit = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerHealthLimit");
				}
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0002D8B8 File Offset: 0x0002BAB8
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x0002D8C0 File Offset: 0x0002BAC0
		[DataSourceProperty]
		public float SpectatedPlayerCurrentHealth
		{
			get
			{
				return this._spectatedPlayerCurrentHealth;
			}
			set
			{
				if (value != this._spectatedPlayerCurrentHealth)
				{
					this._spectatedPlayerCurrentHealth = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerCurrentHealth");
				}
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0002D8DE File Offset: 0x0002BADE
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x0002D8E6 File Offset: 0x0002BAE6
		[DataSourceProperty]
		public float SpectatedPlayerMountCurrentHealth
		{
			get
			{
				return this._spectatedPlayerMountCurrentHealth;
			}
			set
			{
				if (value != this._spectatedPlayerMountCurrentHealth)
				{
					this._spectatedPlayerMountCurrentHealth = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerMountCurrentHealth");
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0002D904 File Offset: 0x0002BB04
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x0002D90C File Offset: 0x0002BB0C
		[DataSourceProperty]
		public float SpectatedPlayerMountHealthLimit
		{
			get
			{
				return this._spectatedPlayerMountHealthLimit;
			}
			set
			{
				if (value != this._spectatedPlayerMountHealthLimit)
				{
					this._spectatedPlayerMountHealthLimit = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerMountHealthLimit");
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0002D92A File Offset: 0x0002BB2A
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x0002D932 File Offset: 0x0002BB32
		[DataSourceProperty]
		public float SpectatedPlayerShieldCurrentHealth
		{
			get
			{
				return this._spectatedPlayerShieldCurrentHealth;
			}
			set
			{
				if (value != this._spectatedPlayerShieldCurrentHealth)
				{
					this._spectatedPlayerShieldCurrentHealth = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerShieldCurrentHealth");
				}
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0002D950 File Offset: 0x0002BB50
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x0002D958 File Offset: 0x0002BB58
		[DataSourceProperty]
		public float SpectatedPlayerShieldHealthLimit
		{
			get
			{
				return this._spectatedPlayerShieldHealthLimit;
			}
			set
			{
				if (value != this._spectatedPlayerShieldHealthLimit)
				{
					this._spectatedPlayerShieldHealthLimit = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerShieldHealthLimit");
				}
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0002D976 File Offset: 0x0002BB76
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x0002D97E File Offset: 0x0002BB7E
		[DataSourceProperty]
		public int SpectatedPlayerAmmoAmount
		{
			get
			{
				return this._spectatedPlayerAmmoAmount;
			}
			set
			{
				if (value != this._spectatedPlayerAmmoAmount)
				{
					this._spectatedPlayerAmmoAmount = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerAmmoAmount");
				}
			}
		}

		// Token: 0x040006BE RID: 1726
		private readonly Mission _mission;

		// Token: 0x040006BF RID: 1727
		private readonly bool _isTeamsEnabled;

		// Token: 0x040006C0 RID: 1728
		private readonly bool _isFlagDominationMode;

		// Token: 0x040006C1 RID: 1729
		private Agent _spectatedAgent;

		// Token: 0x040006C2 RID: 1730
		private string _spectatedPlayerName;

		// Token: 0x040006C3 RID: 1731
		private string _takeControlText;

		// Token: 0x040006C4 RID: 1732
		private int _spectatedPlayerNeutrality = -1;

		// Token: 0x040006C5 RID: 1733
		private bool _isSpectatingPlayer;

		// Token: 0x040006C6 RID: 1734
		private bool _canTakeControlOfSpectatedAgent;

		// Token: 0x040006C7 RID: 1735
		private bool _agentHasMount;

		// Token: 0x040006C8 RID: 1736
		private bool _agentHasShield;

		// Token: 0x040006C9 RID: 1737
		private bool _showAgentHealth;

		// Token: 0x040006CA RID: 1738
		private bool _agentHasRangedWeapon;

		// Token: 0x040006CB RID: 1739
		private bool _agentHasCompassElement;

		// Token: 0x040006CC RID: 1740
		private float _spectatedPlayerHealthLimit;

		// Token: 0x040006CD RID: 1741
		private float _spectatedPlayerCurrentHealth;

		// Token: 0x040006CE RID: 1742
		private float _spectatedPlayerMountCurrentHealth;

		// Token: 0x040006CF RID: 1743
		private float _spectatedPlayerMountHealthLimit;

		// Token: 0x040006D0 RID: 1744
		private float _spectatedPlayerShieldCurrentHealth;

		// Token: 0x040006D1 RID: 1745
		private float _spectatedPlayerShieldHealthLimit;

		// Token: 0x040006D2 RID: 1746
		private int _spectatedPlayerAmmoAmount;

		// Token: 0x040006D3 RID: 1747
		private MPTeammateCompassTargetVM _compassElement;
	}
}
