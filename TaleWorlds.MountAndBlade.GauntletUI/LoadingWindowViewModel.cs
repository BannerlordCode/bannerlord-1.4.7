using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x0200000D RID: 13
	public class LoadingWindowViewModel : ViewModel
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004643 File Offset: 0x00002843
		// (set) Token: 0x0600005A RID: 90 RVA: 0x0000464B File Offset: 0x0000284B
		public bool CurrentlyShowingMultiplayer { get; private set; }

		// Token: 0x0600005B RID: 91 RVA: 0x00004654 File Offset: 0x00002854
		public LoadingWindowViewModel(LoadingWindowViewModel.LoadImageDelegate loadImageDelegate, LoadingWindowViewModel.UnloadImageDelegate unloadImageDelegate)
		{
			this._unloadImageDelegate = unloadImageDelegate;
			this._loadImageDelegate = loadImageDelegate;
			if (this._loadImageDelegate != null)
			{
				string text;
				this._loadImageDelegate(this._currentImage + 1, out text);
				this.LoadingImageName = text;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000469C File Offset: 0x0000289C
		internal void Update()
		{
			if (this.Enabled)
			{
				bool flag = this.IsEligableForMultiplayerLoading();
				if (flag && !this.CurrentlyShowingMultiplayer)
				{
					this.SetForMultiplayer();
					return;
				}
				if (!flag && this.CurrentlyShowingMultiplayer)
				{
					this.SetForEmpty();
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000046DB File Offset: 0x000028DB
		private void HandleEnable()
		{
			if (this.IsEligableForMultiplayerLoading())
			{
				this.SetForMultiplayer();
				return;
			}
			this.SetForEmpty();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000046F4 File Offset: 0x000028F4
		private bool IsEligableForMultiplayerLoading()
		{
			return this._isMultiplayer && Mission.Current != null && Game.Current.GameStateManager.ActiveState is MissionState && ((MissionState)Game.Current.GameStateManager.ActiveState).MissionName != "MultiplayerPractice";
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004750 File Offset: 0x00002950
		private void SetForMultiplayer()
		{
			MissionState missionState = (MissionState)Game.Current.GameStateManager.ActiveState;
			string missionName = missionState.MissionName;
			string text;
			if (!(missionName == "MultiplayerTeamDeathmatch"))
			{
				if (!(missionName == "MultiplayerSiege"))
				{
					if (!(missionName == "MultiplayerBattle"))
					{
						if (!(missionName == "MultiplayerCaptain"))
						{
							if (!(missionName == "MultiplayerSkirmish"))
							{
								if (!(missionName == "MultiplayerDuel"))
								{
									text = missionState.MissionName;
								}
								else
								{
									text = "Duel";
								}
							}
							else
							{
								text = "Skirmish";
							}
						}
						else
						{
							text = "Captain";
						}
					}
					else
					{
						text = "Battle";
					}
				}
				else
				{
					text = "Siege";
				}
			}
			else
			{
				text = "TeamDeathmatch";
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.DescriptionText = GameTexts.FindText("str_multiplayer_official_game_type_explainer", text).ToString();
			}
			else
			{
				this.DescriptionText = "";
			}
			this.GameModeText = GameTexts.FindText("str_multiplayer_official_game_type_name", text).ToString();
			TextObject textObject;
			if (GameTexts.TryGetText("str_multiplayer_scene_name", out textObject, missionState.CurrentMission.SceneName))
			{
				this.TitleText = textObject.ToString();
			}
			else
			{
				this.TitleText = missionState.CurrentMission.SceneName;
			}
			this.LoadingImageName = missionState.CurrentMission.SceneName;
			this.CurrentlyShowingMultiplayer = true;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004891 File Offset: 0x00002A91
		private void SetForEmpty()
		{
			this.DescriptionText = "";
			this.TitleText = "";
			this.GameModeText = "";
			this.SetNextGenericImage();
			this.CurrentlyShowingMultiplayer = false;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000048C4 File Offset: 0x00002AC4
		private void SetNextGenericImage()
		{
			int num = ((this._currentImage >= 1) ? this._currentImage : this._totalGenericImageCount);
			this._currentImage = ((this._currentImage < this._totalGenericImageCount) ? (this._currentImage + 1) : 1);
			int num2 = ((this._currentImage < this._totalGenericImageCount) ? (this._currentImage + 1) : 1);
			if (this._unloadImageDelegate != null)
			{
				this._unloadImageDelegate(num);
			}
			if (this._loadImageDelegate != null)
			{
				string text;
				this._loadImageDelegate(num2, out text);
				this.LoadingImageName = text;
			}
			else
			{
				this.LoadingImageName = string.Empty;
			}
			this.IsDevelopmentMode = NativeConfig.IsDevelopmentMode;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000496B File Offset: 0x00002B6B
		public void SetTotalGenericImageCount(int totalGenericImageCount)
		{
			this._totalGenericImageCount = totalGenericImageCount;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00004974 File Offset: 0x00002B74
		// (set) Token: 0x06000064 RID: 100 RVA: 0x0000497C File Offset: 0x00002B7C
		[DataSourceProperty]
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (this._enabled != value)
				{
					this._enabled = value;
					base.OnPropertyChangedWithValue(value, "Enabled");
					if (value)
					{
						this.HandleEnable();
					}
				}
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000049A3 File Offset: 0x00002BA3
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000049AB File Offset: 0x00002BAB
		[DataSourceProperty]
		public bool IsDevelopmentMode
		{
			get
			{
				return this._isDevelopmentMode;
			}
			set
			{
				if (this._isDevelopmentMode != value)
				{
					this._isDevelopmentMode = value;
					base.OnPropertyChangedWithValue(value, "IsDevelopmentMode");
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000049C9 File Offset: 0x00002BC9
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000049D1 File Offset: 0x00002BD1
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (this._titleText != value)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000049F4 File Offset: 0x00002BF4
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000049FC File Offset: 0x00002BFC
		[DataSourceProperty]
		public string GameModeText
		{
			get
			{
				return this._gameModeText;
			}
			set
			{
				if (this._gameModeText != value)
				{
					this._gameModeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameModeText");
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004A1F File Offset: 0x00002C1F
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00004A27 File Offset: 0x00002C27
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (this._descriptionText != value)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00004A4A File Offset: 0x00002C4A
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00004A52 File Offset: 0x00002C52
		[DataSourceProperty]
		public bool IsMultiplayer
		{
			get
			{
				return this._isMultiplayer;
			}
			set
			{
				if (this._isMultiplayer != value)
				{
					this._isMultiplayer = value;
					base.OnPropertyChangedWithValue(value, "IsMultiplayer");
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00004A70 File Offset: 0x00002C70
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00004A78 File Offset: 0x00002C78
		[DataSourceProperty]
		public bool IsNavalDLCEnabled
		{
			get
			{
				return this._isNavalDLCEnabled;
			}
			set
			{
				if (this._isNavalDLCEnabled != value)
				{
					this._isNavalDLCEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNavalDLCEnabled");
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00004A96 File Offset: 0x00002C96
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00004A9E File Offset: 0x00002C9E
		[DataSourceProperty]
		public string LoadingImageName
		{
			get
			{
				return this._loadingImageName;
			}
			set
			{
				if (this._loadingImageName != value)
				{
					this._loadingImageName = value;
					base.OnPropertyChangedWithValue<string>(value, "LoadingImageName");
				}
			}
		}

		// Token: 0x04000046 RID: 70
		private int _currentImage;

		// Token: 0x04000047 RID: 71
		private int _totalGenericImageCount;

		// Token: 0x04000048 RID: 72
		private LoadingWindowViewModel.LoadImageDelegate _loadImageDelegate;

		// Token: 0x04000049 RID: 73
		private LoadingWindowViewModel.UnloadImageDelegate _unloadImageDelegate;

		// Token: 0x0400004B RID: 75
		private bool _enabled;

		// Token: 0x0400004C RID: 76
		private bool _isDevelopmentMode;

		// Token: 0x0400004D RID: 77
		private bool _isMultiplayer;

		// Token: 0x0400004E RID: 78
		private bool _isNavalDLCEnabled;

		// Token: 0x0400004F RID: 79
		private string _loadingImageName;

		// Token: 0x04000050 RID: 80
		private string _titleText;

		// Token: 0x04000051 RID: 81
		private string _descriptionText;

		// Token: 0x04000052 RID: 82
		private string _gameModeText;

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x06000331 RID: 817
		public delegate void UnloadImageDelegate(int index);

		// Token: 0x02000047 RID: 71
		// (Invoke) Token: 0x06000335 RID: 821
		public delegate void LoadImageDelegate(int index, out string imageName);
	}
}
