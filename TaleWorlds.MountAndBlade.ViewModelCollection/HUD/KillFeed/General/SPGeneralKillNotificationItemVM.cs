using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.General
{
	// Token: 0x0200005F RID: 95
	public class SPGeneralKillNotificationItemVM : ViewModel
	{
		// Token: 0x06000797 RID: 1943 RVA: 0x0001B070 File Offset: 0x00019270
		public SPGeneralKillNotificationItemVM(Agent affectedAgent, Agent affectorAgent, bool isHeadshot, bool isSuicide, bool isDrowning, Action<SPGeneralKillNotificationItemVM> onRemove)
		{
			this._affectedAgent = affectedAgent;
			this._affectorAgent = affectorAgent;
			this._onRemove = onRemove;
			this._showNames = BannerlordConfig.KillFeedVisualType == 0;
			this.InitProperties(this._affectedAgent, this._affectorAgent, isHeadshot, isSuicide, isDrowning);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001B0FC File Offset: 0x000192FC
		private void InitProperties(Agent affectedAgent, Agent affectorAgent, bool isHeadshot, bool isSuicide, bool isDrowning)
		{
			if (!this._showNames)
			{
				if (affectorAgent == null)
				{
					goto IL_004D;
				}
				BasicCharacterObject character = affectorAgent.Character;
				bool? flag = ((character != null) ? new bool?(character.IsHero) : null);
				bool flag2 = true;
				if (!((flag.GetValueOrDefault() == flag2) & (flag != null)))
				{
					goto IL_004D;
				}
			}
			this.MurdererName = affectorAgent.Name;
			IL_004D:
			this.MurdererType = SPGeneralKillNotificationItemVM.GetAgentType(affectorAgent);
			if (!this._showNames)
			{
				BasicCharacterObject character2 = affectedAgent.Character;
				if (character2 == null || !character2.IsHero)
				{
					goto IL_0081;
				}
			}
			this.VictimName = affectedAgent.Name;
			IL_0081:
			this.VictimType = SPGeneralKillNotificationItemVM.GetAgentType(affectedAgent);
			this.IsUnconscious = affectedAgent.State == AgentState.Unconscious;
			this.IsHeadshot = isHeadshot;
			this.IsSuicide = isSuicide;
			this.IsDrowning = isDrowning;
			Team team = affectedAgent.Team;
			Color color;
			if (team != null && team.IsValid)
			{
				if (affectedAgent.Team.IsPlayerAlly)
				{
					color = this._enemyColor;
				}
				else
				{
					color = this._friendlyColor;
				}
			}
			else
			{
				color = Color.FromUint(4284111450U);
			}
			this.BackgroundColor = color;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001B204 File Offset: 0x00019404
		private static string GetAgentType(Agent agent)
		{
			if (((agent != null) ? agent.Character : null) == null)
			{
				return "None";
			}
			switch (agent.Character.DefaultFormationGroup)
			{
			case 0:
				return "Infantry_Light";
			case 1:
				return "Archer_Light";
			case 2:
				return "Cavalry_Light";
			case 3:
				return "HorseArcher_Light";
			case 4:
			case 5:
				return "Infantry_Heavy";
			case 6:
				return "Cavalry_Light";
			case 7:
				return "Cavalry_Heavy";
			case 8:
			case 9:
			case 10:
				return "Infantry_Heavy";
			default:
				return "None";
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001B29A File Offset: 0x0001949A
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0001B2A8 File Offset: 0x000194A8
		// (set) Token: 0x0600079C RID: 1948 RVA: 0x0001B2B0 File Offset: 0x000194B0
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

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0001B2D3 File Offset: 0x000194D3
		// (set) Token: 0x0600079E RID: 1950 RVA: 0x0001B2DB File Offset: 0x000194DB
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

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0001B2FE File Offset: 0x000194FE
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x0001B306 File Offset: 0x00019506
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

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0001B329 File Offset: 0x00019529
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x0001B331 File Offset: 0x00019531
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

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x0001B354 File Offset: 0x00019554
		// (set) Token: 0x060007A4 RID: 1956 RVA: 0x0001B35C File Offset: 0x0001955C
		[DataSourceProperty]
		public bool IsUnconscious
		{
			get
			{
				return this._isUnconscious;
			}
			set
			{
				if (value != this._isUnconscious)
				{
					this._isUnconscious = value;
					base.OnPropertyChangedWithValue(value, "IsUnconscious");
				}
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0001B37A File Offset: 0x0001957A
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x0001B382 File Offset: 0x00019582
		[DataSourceProperty]
		public bool IsHeadshot
		{
			get
			{
				return this._isHeadshot;
			}
			set
			{
				if (value != this._isHeadshot)
				{
					this._isHeadshot = value;
					base.OnPropertyChangedWithValue(value, "IsHeadshot");
				}
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001B3A0 File Offset: 0x000195A0
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0001B3A8 File Offset: 0x000195A8
		[DataSourceProperty]
		public bool IsSuicide
		{
			get
			{
				return this._isSuicide;
			}
			set
			{
				if (value != this._isSuicide)
				{
					this._isSuicide = value;
					base.OnPropertyChangedWithValue(value, "IsSuicide");
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0001B3C6 File Offset: 0x000195C6
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x0001B3CE File Offset: 0x000195CE
		[DataSourceProperty]
		public bool IsDrowning
		{
			get
			{
				return this._isDrowning;
			}
			set
			{
				if (value != this._isDrowning)
				{
					this._isDrowning = value;
					base.OnPropertyChangedWithValue(value, "IsDrowning");
				}
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0001B3EC File Offset: 0x000195EC
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0001B3F4 File Offset: 0x000195F4
		[DataSourceProperty]
		public Color BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				if (value != this._backgroundColor)
				{
					this._backgroundColor = value;
					base.OnPropertyChangedWithValue(value, "BackgroundColor");
				}
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0001B417 File Offset: 0x00019617
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0001B41F File Offset: 0x0001961F
		[DataSourceProperty]
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (value != this._isPaused)
				{
					this._isPaused = value;
					base.OnPropertyChangedWithValue(value, "IsPaused");
				}
			}
		}

		// Token: 0x0400035B RID: 859
		private readonly Color _friendlyColor = new Color(0.54296875f, 0.77734375f, 0.421875f, 1f);

		// Token: 0x0400035C RID: 860
		private readonly Color _enemyColor = new Color(0.953125f, 0.48828125f, 0.42578125f, 1f);

		// Token: 0x0400035D RID: 861
		private readonly Agent _affectedAgent;

		// Token: 0x0400035E RID: 862
		private readonly Agent _affectorAgent;

		// Token: 0x0400035F RID: 863
		private readonly Action<SPGeneralKillNotificationItemVM> _onRemove;

		// Token: 0x04000360 RID: 864
		private readonly bool _showNames;

		// Token: 0x04000361 RID: 865
		private string _murdererName;

		// Token: 0x04000362 RID: 866
		private string _murdererType;

		// Token: 0x04000363 RID: 867
		private string _victimName;

		// Token: 0x04000364 RID: 868
		private string _victimType;

		// Token: 0x04000365 RID: 869
		private bool _isUnconscious;

		// Token: 0x04000366 RID: 870
		private bool _isHeadshot;

		// Token: 0x04000367 RID: 871
		private bool _isSuicide;

		// Token: 0x04000368 RID: 872
		private bool _isDrowning;

		// Token: 0x04000369 RID: 873
		private Color _backgroundColor;

		// Token: 0x0400036A RID: 874
		private bool _isPaused;
	}
}
