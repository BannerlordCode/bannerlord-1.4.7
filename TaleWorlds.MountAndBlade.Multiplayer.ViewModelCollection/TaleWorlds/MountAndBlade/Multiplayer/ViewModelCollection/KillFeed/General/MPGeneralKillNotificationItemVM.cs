using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.General
{
	// Token: 0x0200008C RID: 140
	public class MPGeneralKillNotificationItemVM : ViewModel
	{
		// Token: 0x06000D7E RID: 3454 RVA: 0x00029876 File Offset: 0x00027A76
		public MPGeneralKillNotificationItemVM(Agent affectedAgent, Agent affectorAgent, Agent assistedAgent, Action<MPGeneralKillNotificationItemVM> onRemove)
		{
			this._onRemove = onRemove;
			this.IsDamageNotification = false;
			this.InitProperties(affectedAgent, affectorAgent);
			this.InitDeathProperties(affectedAgent, affectorAgent, assistedAgent);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000298AC File Offset: 0x00027AAC
		public virtual void InitProperties(Agent affectedAgent, Agent affectorAgent)
		{
			this.IsItemInitializationOver = false;
			uint num;
			uint num2;
			this.GetAgentColors(affectorAgent, out num, out num2);
			TargetIconType multiplayerAgentType = this.GetMultiplayerAgentType(affectorAgent);
			Banner agentBanner = this.GetAgentBanner(affectorAgent);
			bool? flag;
			if (affectorAgent == null)
			{
				flag = null;
			}
			else
			{
				Team team = affectorAgent.Team;
				flag = ((team != null) ? new bool?(team.IsPlayerAlly) : null);
			}
			bool flag2 = flag ?? false;
			uint num3;
			uint num4;
			this.GetAgentColors(affectedAgent, out num3, out num4);
			TargetIconType multiplayerAgentType2 = this.GetMultiplayerAgentType(affectedAgent);
			Banner agentBanner2 = this.GetAgentBanner(affectedAgent);
			Team team2 = affectedAgent.Team;
			bool flag3 = team2 != null && team2.IsPlayerAlly;
			this.MurdererName = ((affectorAgent != null) ? ((affectorAgent.MissionPeer != null) ? affectorAgent.MissionPeer.DisplayedName : affectorAgent.Name) : "");
			this.MurdererType = multiplayerAgentType.ToString();
			this.IsMurdererBot = affectorAgent != null && !affectorAgent.IsPlayerControlled;
			this.MurdererCompassElement = new MPTeammateCompassTargetVM(multiplayerAgentType, num, num2, agentBanner, flag2);
			this.VictimName = ((affectedAgent.MissionPeer != null) ? affectedAgent.MissionPeer.DisplayedName : affectedAgent.Name);
			this.VictimType = multiplayerAgentType2.ToString();
			this.IsVictimBot = !affectedAgent.IsPlayerControlled;
			this.VictimCompassElement = new MPTeammateCompassTargetVM(multiplayerAgentType2, num3, num4, agentBanner2, flag3);
			this.IsPlayerDeath = affectedAgent.IsMainAgent;
			if (flag2 && flag3)
			{
				this.Color1 = Color.FromUint(4278190080U);
				this.Color2 = Color.FromUint(uint.MaxValue);
			}
			else if (!flag2 && !flag3)
			{
				this.Color1 = Color.FromUint(4281545266U);
				this.Color2 = Color.FromUint(uint.MaxValue);
			}
			else
			{
				this.Color1 = Color.FromUint(num);
				this.Color2 = Color.FromUint(num2);
			}
			if (this.IsVictimBot)
			{
				Formation formation = affectedAgent.Formation;
				Agent main = Agent.Main;
				if (formation == ((main != null) ? main.Formation : null))
				{
					this.IsRelatedToFriendlyTroop = true;
					this.IsFriendlyTroopDeath = true;
					goto IL_0220;
				}
			}
			if (this.IsMurdererBot && affectorAgent != null)
			{
				Formation formation2 = affectorAgent.Formation;
				Agent main2 = Agent.Main;
				if (formation2 == ((main2 != null) ? main2.Formation : null))
				{
					this.IsRelatedToFriendlyTroop = true;
				}
			}
			IL_0220:
			this.IsItemInitializationOver = true;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00029AE0 File Offset: 0x00027CE0
		public void InitDeathProperties(Agent affectedAgent, Agent affectorAgent, Agent assistedAgent)
		{
			this.IsItemInitializationOver = false;
			if (affectorAgent != null && affectorAgent.IsMainAgent)
			{
				MBTextManager.SetTextVariable("TROOP_NAME", affectedAgent.NameTextObject.ToString(), false);
				this.Message = GameTexts.FindText("str_kill_feed_message", null).ToString();
			}
			else if (affectedAgent.IsMainAgent)
			{
				MBTextManager.SetTextVariable("TROOP_NAME", (affectorAgent != null) ? affectorAgent.ToString() : null, false);
				this.Message = GameTexts.FindText("str_death_feed_message", null).ToString();
			}
			else if (assistedAgent != null && assistedAgent.IsMainAgent)
			{
				MBTextManager.SetTextVariable("TROOP_NAME", affectedAgent.NameTextObject.ToString(), false);
				this.Message = GameTexts.FindText("str_assist_feed_message", null).ToString();
			}
			this.IsItemInitializationOver = true;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00029BA4 File Offset: 0x00027DA4
		protected TargetIconType GetMultiplayerAgentType(Agent agent)
		{
			if (agent == null)
			{
				return TargetIconType.None;
			}
			if (!agent.IsHuman)
			{
				return TargetIconType.Monster;
			}
			MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(agent.Character);
			if (mpheroClassForCharacter == null)
			{
				Debug.FailedAssert("Hero class is not set for agent: " + agent.Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\KillFeed\\General\\MPGeneralKillNotificationItemVM.cs", "GetMultiplayerAgentType", 116);
				return TargetIconType.None;
			}
			return mpheroClassForCharacter.IconType;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00029BF8 File Offset: 0x00027DF8
		private Banner GetAgentBanner(Agent agent)
		{
			Banner banner = this.DefaultBanner;
			if (agent != null)
			{
				MissionPeer missionPeer = agent.MissionPeer;
				MissionPeer missionPeer2 = ((missionPeer != null) ? missionPeer.GetComponent<MissionPeer>() : null);
				if (agent.Team != null && missionPeer2 != null)
				{
					banner = new Banner(missionPeer2.Peer.BannerCode, agent.Team.Color, agent.Team.Color2);
				}
				else if (agent.Team != null && agent.Formation != null && !string.IsNullOrEmpty(agent.Formation.BannerCode))
				{
					banner = new Banner(agent.Formation.BannerCode, agent.Team.Color, agent.Team.Color2);
				}
				else if (agent.Team != null)
				{
					banner = agent.Team.Banner;
				}
			}
			return banner;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00029CB9 File Offset: 0x00027EB9
		private void GetAgentColors(Agent agent, out uint color1, out uint color2)
		{
			if (((agent != null) ? agent.Team : null) != null)
			{
				color1 = agent.Team.Color;
				color2 = agent.Team.Color2;
				return;
			}
			color1 = 4284111450U;
			color2 = uint.MaxValue;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00029CEE File Offset: 0x00027EEE
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00029CFC File Offset: 0x00027EFC
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x00029D04 File Offset: 0x00027F04
		[DataSourceProperty]
		public string MurdererName
		{
			get
			{
				return this._murdererName;
			}
			set
			{
				if (value != this._murdererName)
				{
					this._murdererName = value;
					base.OnPropertyChangedWithValue<string>(value, "MurdererName");
				}
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00029D27 File Offset: 0x00027F27
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x00029D2F File Offset: 0x00027F2F
		[DataSourceProperty]
		public string MurdererType
		{
			get
			{
				return this._murdererType;
			}
			set
			{
				if (value != this._murdererType)
				{
					this._murdererType = value;
					base.OnPropertyChangedWithValue<string>(value, "MurdererType");
				}
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00029D52 File Offset: 0x00027F52
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x00029D5A File Offset: 0x00027F5A
		[DataSourceProperty]
		public string VictimName
		{
			get
			{
				return this._victimName;
			}
			set
			{
				if (value != this._victimName)
				{
					this._victimName = value;
					base.OnPropertyChangedWithValue<string>(value, "VictimName");
				}
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00029D7D File Offset: 0x00027F7D
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00029D85 File Offset: 0x00027F85
		[DataSourceProperty]
		public string VictimType
		{
			get
			{
				return this._victimType;
			}
			set
			{
				if (value != this._victimType)
				{
					this._victimType = value;
					base.OnPropertyChangedWithValue<string>(value, "VictimType");
				}
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00029DA8 File Offset: 0x00027FA8
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x00029DB0 File Offset: 0x00027FB0
		[DataSourceProperty]
		public bool IsDamageNotification
		{
			get
			{
				return this._isDamageNotification;
			}
			set
			{
				if (value != this._isDamageNotification)
				{
					this._isDamageNotification = value;
					base.OnPropertyChangedWithValue(value, "IsDamageNotification");
				}
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00029DCE File Offset: 0x00027FCE
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x00029DD6 File Offset: 0x00027FD6
		[DataSourceProperty]
		public bool IsDamagedMount
		{
			get
			{
				return this._isDamagedMount;
			}
			set
			{
				if (value != this._isDamagedMount)
				{
					this._isDamagedMount = value;
					base.OnPropertyChangedWithValue(value, "IsDamagedMount");
				}
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00029DF4 File Offset: 0x00027FF4
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x00029DFC File Offset: 0x00027FFC
		[DataSourceProperty]
		public Color Color1
		{
			get
			{
				return this._color1;
			}
			set
			{
				if (value != this._color1)
				{
					this._color1 = value;
					base.OnPropertyChangedWithValue(value, "Color1");
				}
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00029E1F File Offset: 0x0002801F
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x00029E27 File Offset: 0x00028027
		[DataSourceProperty]
		public Color Color2
		{
			get
			{
				return this._color2;
			}
			set
			{
				if (value != this._color2)
				{
					this._color2 = value;
					base.OnPropertyChangedWithValue(value, "Color2");
				}
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00029E4A File Offset: 0x0002804A
		// (set) Token: 0x06000D96 RID: 3478 RVA: 0x00029E52 File Offset: 0x00028052
		[DataSourceProperty]
		public MPTeammateCompassTargetVM MurdererCompassElement
		{
			get
			{
				return this._murdererCompassElement;
			}
			set
			{
				if (value != this._murdererCompassElement)
				{
					this._murdererCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "MurdererCompassElement");
				}
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00029E70 File Offset: 0x00028070
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00029E78 File Offset: 0x00028078
		[DataSourceProperty]
		public MPTeammateCompassTargetVM VictimCompassElement
		{
			get
			{
				return this._victimCompassElement;
			}
			set
			{
				if (value != this._victimCompassElement)
				{
					this._victimCompassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "VictimCompassElement");
				}
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00029E96 File Offset: 0x00028096
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00029E9E File Offset: 0x0002809E
		[DataSourceProperty]
		public bool IsPlayerDeath
		{
			get
			{
				return this._isPlayerDeath;
			}
			set
			{
				if (value != this._isPlayerDeath)
				{
					this._isPlayerDeath = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerDeath");
				}
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00029EBC File Offset: 0x000280BC
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x00029EC4 File Offset: 0x000280C4
		[DataSourceProperty]
		public bool IsItemInitializationOver
		{
			get
			{
				return this._isItemInitializationOver;
			}
			set
			{
				if (value != this._isItemInitializationOver)
				{
					this._isItemInitializationOver = value;
					base.OnPropertyChangedWithValue(value, "IsItemInitializationOver");
				}
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00029EE2 File Offset: 0x000280E2
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00029EEA File Offset: 0x000280EA
		[DataSourceProperty]
		public bool IsVictimBot
		{
			get
			{
				return this._isVictimBot;
			}
			set
			{
				if (value != this._isVictimBot)
				{
					this._isVictimBot = value;
					base.OnPropertyChangedWithValue(value, "IsVictimBot");
				}
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00029F08 File Offset: 0x00028108
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00029F10 File Offset: 0x00028110
		[DataSourceProperty]
		public bool IsMurdererBot
		{
			get
			{
				return this._isMurdererBot;
			}
			set
			{
				if (value != this._isMurdererBot)
				{
					this._isMurdererBot = value;
					base.OnPropertyChangedWithValue(value, "IsMurdererBot");
				}
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00029F2E File Offset: 0x0002812E
		// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x00029F36 File Offset: 0x00028136
		[DataSourceProperty]
		public bool IsRelatedToFriendlyTroop
		{
			get
			{
				return this._isRelatedToFriendlyTroop;
			}
			set
			{
				if (value != this._isRelatedToFriendlyTroop)
				{
					this._isRelatedToFriendlyTroop = value;
					base.OnPropertyChangedWithValue(value, "IsRelatedToFriendlyTroop");
				}
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00029F54 File Offset: 0x00028154
		// (set) Token: 0x06000DA4 RID: 3492 RVA: 0x00029F5C File Offset: 0x0002815C
		[DataSourceProperty]
		public bool IsFriendlyTroopDeath
		{
			get
			{
				return this._isFriendlyTroopDeath;
			}
			set
			{
				if (value != this._isFriendlyTroopDeath)
				{
					this._isFriendlyTroopDeath = value;
					base.OnPropertyChangedWithValue(value, "IsFriendlyTroopDeath");
				}
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00029F7A File Offset: 0x0002817A
		// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x00029F82 File Offset: 0x00028182
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x04000627 RID: 1575
		private readonly Action<MPGeneralKillNotificationItemVM> _onRemove;

		// Token: 0x04000628 RID: 1576
		private readonly Banner DefaultBanner = Banner.CreateOneColoredEmptyBanner(92);

		// Token: 0x04000629 RID: 1577
		private string _murdererName;

		// Token: 0x0400062A RID: 1578
		private string _murdererType;

		// Token: 0x0400062B RID: 1579
		private string _victimName;

		// Token: 0x0400062C RID: 1580
		private string _victimType;

		// Token: 0x0400062D RID: 1581
		private MPTeammateCompassTargetVM _murdererCompassElement;

		// Token: 0x0400062E RID: 1582
		private MPTeammateCompassTargetVM _victimCompassElement;

		// Token: 0x0400062F RID: 1583
		private Color _color1;

		// Token: 0x04000630 RID: 1584
		private Color _color2;

		// Token: 0x04000631 RID: 1585
		private bool _isPlayerDeath;

		// Token: 0x04000632 RID: 1586
		private bool _isItemInitializationOver;

		// Token: 0x04000633 RID: 1587
		private bool _isVictimBot;

		// Token: 0x04000634 RID: 1588
		private bool _isMurdererBot;

		// Token: 0x04000635 RID: 1589
		private bool _isDamageNotification;

		// Token: 0x04000636 RID: 1590
		private bool _isDamagedMount;

		// Token: 0x04000637 RID: 1591
		private bool _isRelatedToFriendlyTroop;

		// Token: 0x04000638 RID: 1592
		private bool _isFriendlyTroopDeath;

		// Token: 0x04000639 RID: 1593
		private string _message;
	}
}
