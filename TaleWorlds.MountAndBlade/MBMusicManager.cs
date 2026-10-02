using System;
using System.Collections.Generic;
using System.Threading;
using psai.net;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D5 RID: 469
	public class MBMusicManager
	{
		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001BDE RID: 7134 RVA: 0x000608D0 File Offset: 0x0005EAD0
		// (set) Token: 0x06001BDF RID: 7135 RVA: 0x000608D7 File Offset: 0x0005EAD7
		public static MBMusicManager Current { get; private set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x000608DF File Offset: 0x0005EADF
		// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x000608E7 File Offset: 0x0005EAE7
		public MusicMode CurrentMode { get; private set; }

		// Token: 0x06001BE2 RID: 7138 RVA: 0x000608F0 File Offset: 0x0005EAF0
		private MBMusicManager()
		{
			if (!NativeConfig.DisableSound)
			{
				List<string> list = new List<string>();
				foreach (MbObjectXmlInformation mbObjectXmlInformation in XmlResource.MbprojXmls)
				{
					if (mbObjectXmlInformation.Id == "soln_soundtrack")
					{
						string moduleName = mbObjectXmlInformation.ModuleName;
						list.Add(moduleName);
					}
				}
				PsaiCore.Instance.LoadSoundtrackFromProjectFile(list);
			}
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x00060984 File Offset: 0x0005EB84
		public static bool IsCreationCompleted()
		{
			return MBMusicManager._creationCompleted;
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x0006098B File Offset: 0x0005EB8B
		private static void ProcessCreation(object callback)
		{
			MBMusicManager.Current = new MBMusicManager();
			MusicParameters.LoadFromXml();
			MBMusicManager._creationCompleted = true;
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000609A2 File Offset: 0x0005EBA2
		public static void Create()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(MBMusicManager.ProcessCreation));
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000609B8 File Offset: 0x0005EBB8
		public static void Initialize()
		{
			if (!MBMusicManager._initialized)
			{
				MBMusicManager.Current._battleMode = new MBMusicManager.BattleMusicMode();
				MBMusicManager.Current._campaignMode = new MBMusicManager.CampaignMusicMode();
				MBMusicManager.Current.CurrentMode = MusicMode.Paused;
				MBMusicManager.Current._menuModeActivationTimer = 0.5f;
				MBMusicManager._initialized = true;
				Debug.Print("MusicManager Initialize completed.", 0, Debug.DebugColor.Green, 281474976710656UL);
			}
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x00060A1F File Offset: 0x0005EC1F
		public void OnCampaignMusicHandlerInit(IMusicHandler campaignMusicHandler)
		{
			this._campaignMusicHandler = campaignMusicHandler;
			this._activeMusicHandler = this._campaignMusicHandler;
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x00060A34 File Offset: 0x0005EC34
		public void OnCampaignMusicHandlerFinalize()
		{
			this._campaignMusicHandler = null;
			this.CheckActiveHandler();
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x00060A43 File Offset: 0x0005EC43
		public void OnBattleMusicHandlerInit(IMusicHandler battleMusicHandler)
		{
			this._battleMusicHandler = battleMusicHandler;
			this._activeMusicHandler = this._battleMusicHandler;
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x00060A58 File Offset: 0x0005EC58
		public void OnBattleMusicHandlerFinalize()
		{
			this._battleMusicHandler = null;
			this.CheckActiveHandler();
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x00060A67 File Offset: 0x0005EC67
		public void OnSilencedMusicHandlerInit(IMusicHandler silencedMusicHandler)
		{
			this._silencedMusicHandler = silencedMusicHandler;
			this._activeMusicHandler = this._silencedMusicHandler;
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x00060A7C File Offset: 0x0005EC7C
		public void OnSilencedMusicHandlerFinalize()
		{
			this._silencedMusicHandler = null;
			this.CheckActiveHandler();
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x00060A8B File Offset: 0x0005EC8B
		private void CheckActiveHandler()
		{
			IMusicHandler musicHandler;
			if ((musicHandler = this._battleMusicHandler) == null)
			{
				musicHandler = this._silencedMusicHandler ?? this._campaignMusicHandler;
			}
			this._activeMusicHandler = musicHandler;
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x00060AB0 File Offset: 0x0005ECB0
		private void ActivateMenuMode()
		{
			if (!this._systemPaused)
			{
				this.CurrentMode = MusicMode.Menu;
				MusicTheme musicTheme = (ModuleHelper.IsModuleActive("NavalDLC") ? MusicTheme.NavalMainTheme : MusicTheme.MainTheme);
				PsaiCore.Instance.MenuModeEnter((int)musicTheme, 0.5f);
			}
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x00060AF2 File Offset: 0x0005ECF2
		private void DeactivateMenuMode()
		{
			PsaiCore.Instance.MenuModeLeave();
			this.CurrentMode = MusicMode.Paused;
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x00060B06 File Offset: 0x0005ED06
		public void ActivateBattleMode()
		{
			if (!this._systemPaused)
			{
				this.CurrentMode = MusicMode.Battle;
			}
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x00060B17 File Offset: 0x0005ED17
		public void DeactivateBattleMode()
		{
			PsaiCore.Instance.StopMusic(true, 3f);
			this.CurrentMode = MusicMode.Paused;
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x00060B31 File Offset: 0x0005ED31
		public void ActivateCampaignMode()
		{
			if (!this._systemPaused)
			{
				this.CurrentMode = MusicMode.Campaign;
			}
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x00060B42 File Offset: 0x0005ED42
		public void DeactivateCampaignMode()
		{
			PsaiCore.Instance.StopMusic(true, 3f);
			this.CurrentMode = MusicMode.Paused;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x00060B5C File Offset: 0x0005ED5C
		public void DeactivateCurrentMode()
		{
			switch (this.CurrentMode)
			{
			case MusicMode.Menu:
				break;
			case MusicMode.Campaign:
				this.DeactivateCampaignMode();
				return;
			case MusicMode.Battle:
				this.DeactivateBattleMode();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x00060B92 File Offset: 0x0005ED92
		private bool CheckMenuModeActivationTimer()
		{
			return this._menuModeActivationTimer <= 0f;
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x00060BA4 File Offset: 0x0005EDA4
		public void UnpauseMusicManagerSystem()
		{
			if (this._systemPaused)
			{
				this._systemPaused = false;
				this._menuModeActivationTimer = 1f;
			}
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x00060BC0 File Offset: 0x0005EDC0
		public void PauseMusicManagerSystem()
		{
			if (!this._systemPaused)
			{
				if (this.CurrentMode == MusicMode.Menu)
				{
					this.DeactivateMenuMode();
				}
				this._systemPaused = true;
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x00060BE0 File Offset: 0x0005EDE0
		public void StartTheme(MusicTheme theme, float startIntensity, bool queueEndSegment = false)
		{
			PsaiCore.Instance.TriggerMusicTheme((int)theme, startIntensity);
			if (queueEndSegment)
			{
				PsaiCore.Instance.StopMusic(false, 3f);
			}
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x00060C03 File Offset: 0x0005EE03
		public void StartThemeWithConstantIntensity(MusicTheme theme, bool queueEndSegment = false)
		{
			PsaiCore.Instance.HoldCurrentIntensity(true);
			this.StartTheme(theme, 0f, queueEndSegment);
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x00060C1E File Offset: 0x0005EE1E
		public void ForceStopThemeWithFadeOut()
		{
			PsaiCore.Instance.StopMusic(true, 3f);
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x00060C31 File Offset: 0x0005EE31
		public void ChangeCurrentThemeIntensity(float deltaIntensity)
		{
			PsaiCore.Instance.AddToCurrentIntensity(deltaIntensity);
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x00060C40 File Offset: 0x0005EE40
		public void Update(float dt)
		{
			if (Utilities.EngineFrameNo == this._latestFrameUpdatedNo)
			{
				return;
			}
			this._latestFrameUpdatedNo = Utilities.EngineFrameNo;
			if (this._menuModeActivationTimer > 0f)
			{
				this._menuModeActivationTimer -= dt;
			}
			if (!this._systemPaused)
			{
				if (GameStateManager.Current != null && GameStateManager.Current.ActiveState != null)
				{
					GameState activeState = GameStateManager.Current.ActiveState;
					MusicMode currentMode = this.CurrentMode;
					if (currentMode != MusicMode.Paused)
					{
						if (currentMode == MusicMode.Menu)
						{
							if (!activeState.IsMusicMenuState)
							{
								this.DeactivateMenuMode();
							}
						}
					}
					else if (activeState.IsMusicMenuState && this.CheckMenuModeActivationTimer())
					{
						this.ActivateMenuMode();
					}
				}
				if (this._activeMusicHandler != null)
				{
					this._activeMusicHandler.OnUpdated(dt);
				}
			}
			PsaiCore.Instance.Update();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x00060CFC File Offset: 0x0005EEFC
		public MusicTheme GetSiegeTheme(BasicCultureObject culture)
		{
			return this._battleMode.GetSiegeTheme(culture);
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x00060D0A File Offset: 0x0005EF0A
		public MusicTheme GetBattleTheme(BasicCultureObject culture, int battleSize, out bool isPaganBattle)
		{
			return this._battleMode.GetBattleTheme(culture, battleSize, out isPaganBattle);
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x00060D1A File Offset: 0x0005EF1A
		public MusicTheme GetBattleEndTheme(BasicCultureObject culture, bool isVictory)
		{
			return this._battleMode.GetBattleEndTheme(culture, isVictory);
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00060D29 File Offset: 0x0005EF29
		public MusicTheme GetBattleTurnsOneSideTheme(BasicCultureObject culture, bool isPositive, bool isPaganBattle)
		{
			if (isPaganBattle)
			{
				if (!isPositive)
				{
					return MusicTheme.PaganTurnsNegative;
				}
				return MusicTheme.PaganTurnsPositive;
			}
			else
			{
				if (!isPositive)
				{
					return MusicTheme.BattleTurnsNegative;
				}
				return MusicTheme.BattleTurnsPositive;
			}
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00060D40 File Offset: 0x0005EF40
		public MusicTheme GetCampaignMusicTheme(BasicCultureObject culture, bool isDark, bool isWarMode, bool isAtSea)
		{
			MusicTheme musicTheme = MusicTheme.None;
			if (!isDark && isWarMode)
			{
				musicTheme = this._campaignMode.GetCampaignDramaticThemeWithCulture(culture);
			}
			if (isAtSea)
			{
				musicTheme = this._campaignMode.GetSeaCampignMusic(culture);
			}
			if (musicTheme != MusicTheme.None)
			{
				return musicTheme;
			}
			return this._campaignMode.GetCampaignTheme(culture, isDark);
		}

		// Token: 0x04000958 RID: 2392
		private const string CultureEmpire = "empire";

		// Token: 0x04000959 RID: 2393
		private const string CultureSturgia = "sturgia";

		// Token: 0x0400095A RID: 2394
		private const string CultureAserai = "aserai";

		// Token: 0x0400095B RID: 2395
		private const string CultureVlandia = "vlandia";

		// Token: 0x0400095C RID: 2396
		private const string CultureBattania = "battania";

		// Token: 0x0400095D RID: 2397
		private const string CultureKhuzait = "khuzait";

		// Token: 0x0400095E RID: 2398
		private const string CultureNord = "nord";

		// Token: 0x0400095F RID: 2399
		private const float DefaultFadeOutDurationInSeconds = 3f;

		// Token: 0x04000960 RID: 2400
		private const float MenuModeActivationTimerInSeconds = 0.5f;

		// Token: 0x04000963 RID: 2403
		private MBMusicManager.BattleMusicMode _battleMode;

		// Token: 0x04000964 RID: 2404
		private MBMusicManager.CampaignMusicMode _campaignMode;

		// Token: 0x04000965 RID: 2405
		private IMusicHandler _campaignMusicHandler;

		// Token: 0x04000966 RID: 2406
		private IMusicHandler _battleMusicHandler;

		// Token: 0x04000967 RID: 2407
		private IMusicHandler _silencedMusicHandler;

		// Token: 0x04000968 RID: 2408
		private IMusicHandler _activeMusicHandler;

		// Token: 0x04000969 RID: 2409
		private static bool _initialized;

		// Token: 0x0400096A RID: 2410
		private static bool _creationCompleted;

		// Token: 0x0400096B RID: 2411
		private float _menuModeActivationTimer;

		// Token: 0x0400096C RID: 2412
		private bool _systemPaused;

		// Token: 0x0400096D RID: 2413
		private int _latestFrameUpdatedNo = -1;

		// Token: 0x0200050C RID: 1292
		private class CampaignMusicMode
		{
			// Token: 0x06003BB5 RID: 15285 RVA: 0x000EEF12 File Offset: 0x000ED112
			public CampaignMusicMode()
			{
				this._factionSpecificCampaignThemeSelectionFactor = 0.35f;
				this._factionSpecificCampaignDramaticThemeSelectionFactor = 0.35f;
			}

			// Token: 0x06003BB6 RID: 15286 RVA: 0x000EEF30 File Offset: 0x000ED130
			public MusicTheme GetCampaignTheme(BasicCultureObject culture, bool isDark)
			{
				if (isDark)
				{
					return MusicTheme.CampaignDark;
				}
				MusicTheme campaignThemeWithCulture = this.GetCampaignThemeWithCulture(culture);
				MusicTheme musicTheme;
				if (campaignThemeWithCulture == MusicTheme.None)
				{
					musicTheme = MusicTheme.CampaignStandard;
					this._factionSpecificCampaignThemeSelectionFactor += 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignThemeSelectionFactor);
				}
				else
				{
					musicTheme = campaignThemeWithCulture;
					this._factionSpecificCampaignThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignThemeSelectionFactor);
				}
				return musicTheme;
			}

			// Token: 0x06003BB7 RID: 15287 RVA: 0x000EEF90 File Offset: 0x000ED190
			private MusicTheme GetCampaignThemeWithCulture(BasicCultureObject culture)
			{
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificCampaignThemeSelectionFactor)
				{
					this._factionSpecificCampaignThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignThemeSelectionFactor);
					if (culture.StringId == "empire")
					{
						if (MBRandom.NondeterministicRandomFloat >= 0.5f)
						{
							return MusicTheme.EmpireCampaignB;
						}
						return MusicTheme.EmpireCampaignA;
					}
					else
					{
						if (culture.StringId == "sturgia")
						{
							return MusicTheme.SturgiaCampaignA;
						}
						if (culture.StringId == "aserai")
						{
							return MusicTheme.AseraiCampaignA;
						}
						if (culture.StringId == "vlandia")
						{
							return MusicTheme.VlandiaCampaignA;
						}
						if (culture.StringId == "khuzait")
						{
							return MusicTheme.KhuzaitCampaignA;
						}
						if (culture.StringId == "battania")
						{
							return MusicTheme.BattaniaCampaignA;
						}
						if (culture.StringId == "nord")
						{
							return MusicTheme.NordCampaign;
						}
					}
				}
				return MusicTheme.None;
			}

			// Token: 0x06003BB8 RID: 15288 RVA: 0x000EF070 File Offset: 0x000ED270
			public MusicTheme GetCampaignDramaticThemeWithCulture(BasicCultureObject culture)
			{
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificCampaignDramaticThemeSelectionFactor)
				{
					this._factionSpecificCampaignDramaticThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificCampaignDramaticThemeSelectionFactor);
					if (culture.StringId == "empire")
					{
						return MusicTheme.EmpireCampaignDramatic;
					}
					if (culture.StringId == "sturgia")
					{
						return MusicTheme.SturgiaCampaignDramatic;
					}
					if (culture.StringId == "aserai")
					{
						return MusicTheme.AseraiCampaignDramatic;
					}
					if (culture.StringId == "vlandia")
					{
						return MusicTheme.VlandiaCampaignDramatic;
					}
					if (culture.StringId == "khuzait")
					{
						return MusicTheme.KhuzaitCampaignDramatic;
					}
					if (culture.StringId == "battania")
					{
						return MusicTheme.BattaniaCampaignDramatic;
					}
					if (culture.StringId == "nord")
					{
						return MusicTheme.NordCampaign;
					}
				}
				this._factionSpecificCampaignDramaticThemeSelectionFactor += 0.1f;
				MBMath.ClampUnit(ref this._factionSpecificCampaignDramaticThemeSelectionFactor);
				return MusicTheme.None;
			}

			// Token: 0x06003BB9 RID: 15289 RVA: 0x000EF160 File Offset: 0x000ED360
			public MusicTheme GetSeaCampignMusic(BasicCultureObject culture)
			{
				if (culture.StringId == "sturgia" || culture.StringId == "battania" || culture.StringId == "nord")
				{
					return MusicTheme.SeaCampaignNorthern;
				}
				if (culture.StringId == "aserai" || culture.StringId == "vlandia" || culture.StringId == "khuzait" || culture.StringId == "empire")
				{
					return MusicTheme.SeaCampaignSouthern;
				}
				return MusicTheme.None;
			}

			// Token: 0x04001CDF RID: 7391
			private const float DefaultSelectionFactorForFactionSpecificCampaignTheme = 0.35f;

			// Token: 0x04001CE0 RID: 7392
			private const float SelectionFactorDecayAmountForFactionSpecificCampaignTheme = 0.1f;

			// Token: 0x04001CE1 RID: 7393
			private const float SelectionFactorGrowthAmountForFactionSpecificCampaignTheme = 0.1f;

			// Token: 0x04001CE2 RID: 7394
			private float _factionSpecificCampaignThemeSelectionFactor;

			// Token: 0x04001CE3 RID: 7395
			private float _factionSpecificCampaignDramaticThemeSelectionFactor;
		}

		// Token: 0x0200050D RID: 1293
		private class BattleMusicMode
		{
			// Token: 0x06003BBA RID: 15290 RVA: 0x000EF1F8 File Offset: 0x000ED3F8
			public BattleMusicMode()
			{
				this._factionSpecificBattleThemeSelectionFactor = 0.35f;
				this._factionSpecificSiegeThemeSelectionFactor = 0.35f;
			}

			// Token: 0x06003BBB RID: 15291 RVA: 0x000EF218 File Offset: 0x000ED418
			private MusicTheme GetBattleThemeWithCulture(BasicCultureObject culture, out bool isPaganBattle)
			{
				isPaganBattle = false;
				MusicTheme musicTheme = MusicTheme.None;
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificBattleThemeSelectionFactor)
				{
					this._factionSpecificBattleThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificBattleThemeSelectionFactor);
					if (culture.StringId == "sturgia" || culture.StringId == "aserai" || culture.StringId == "khuzait" || culture.StringId == "battania")
					{
						isPaganBattle = true;
						musicTheme = ((MBRandom.NondeterministicRandomFloat < 0.5f) ? MusicTheme.BattlePaganA : MusicTheme.BattlePaganB);
					}
					else if (culture.StringId == "nord")
					{
						musicTheme = MusicTheme.BattleNord;
					}
					else
					{
						musicTheme = ((MBRandom.NondeterministicRandomFloat < 0.5f) ? MusicTheme.CombatA : MusicTheme.CombatB);
					}
				}
				return musicTheme;
			}

			// Token: 0x06003BBC RID: 15292 RVA: 0x000EF2E4 File Offset: 0x000ED4E4
			private MusicTheme GetSiegeThemeWithCulture(BasicCultureObject culture)
			{
				MusicTheme musicTheme = MusicTheme.None;
				if (MBRandom.NondeterministicRandomFloat <= this._factionSpecificSiegeThemeSelectionFactor)
				{
					this._factionSpecificSiegeThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificSiegeThemeSelectionFactor);
					if (culture.StringId == "sturgia" || culture.StringId == "aserai" || culture.StringId == "khuzait" || culture.StringId == "battania")
					{
						musicTheme = MusicTheme.PaganSiege;
					}
				}
				return musicTheme;
			}

			// Token: 0x06003BBD RID: 15293 RVA: 0x000EF36C File Offset: 0x000ED56C
			private MusicTheme GetVictoryThemeForCulture(BasicCultureObject culture)
			{
				if (MBRandom.NondeterministicRandomFloat <= 0.65f)
				{
					if (culture.StringId == "empire")
					{
						return MusicTheme.EmpireVictory;
					}
					if (culture.StringId == "sturgia" || culture.StringId == "nord")
					{
						return MusicTheme.SturgiaVictory;
					}
					if (culture.StringId == "aserai")
					{
						return MusicTheme.AseraiVictory;
					}
					if (culture.StringId == "vlandia")
					{
						return MusicTheme.VlandiaVictory;
					}
					if (culture.StringId == "khuzait")
					{
						return MusicTheme.KhuzaitVictory;
					}
					if (culture.StringId == "battania")
					{
						return MusicTheme.BattaniaVictory;
					}
				}
				return MusicTheme.None;
			}

			// Token: 0x06003BBE RID: 15294 RVA: 0x000EF418 File Offset: 0x000ED618
			public MusicTheme GetBattleTheme(BasicCultureObject culture, int battleSize, out bool isPaganBattle)
			{
				MusicTheme battleThemeWithCulture = this.GetBattleThemeWithCulture(culture, out isPaganBattle);
				MusicTheme musicTheme;
				if (battleThemeWithCulture == MusicTheme.None)
				{
					musicTheme = (((float)battleSize < (float)MusicParameters.SmallBattleTreshold - (float)MusicParameters.SmallBattleTreshold * 0.2f * MBRandom.NondeterministicRandomFloat) ? MusicTheme.BattleSmall : MusicTheme.BattleMedium);
					this._factionSpecificBattleThemeSelectionFactor += 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificBattleThemeSelectionFactor);
				}
				else
				{
					musicTheme = battleThemeWithCulture;
					this._factionSpecificBattleThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificBattleThemeSelectionFactor);
				}
				return musicTheme;
			}

			// Token: 0x06003BBF RID: 15295 RVA: 0x000EF498 File Offset: 0x000ED698
			public MusicTheme GetSiegeTheme(BasicCultureObject culture)
			{
				MusicTheme siegeThemeWithCulture = this.GetSiegeThemeWithCulture(culture);
				MusicTheme musicTheme;
				if (siegeThemeWithCulture == MusicTheme.None)
				{
					musicTheme = MusicTheme.BattleSiege;
					this._factionSpecificSiegeThemeSelectionFactor += 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificSiegeThemeSelectionFactor);
				}
				else
				{
					musicTheme = siegeThemeWithCulture;
					this._factionSpecificSiegeThemeSelectionFactor -= 0.1f;
					MBMath.ClampUnit(ref this._factionSpecificSiegeThemeSelectionFactor);
				}
				return musicTheme;
			}

			// Token: 0x06003BC0 RID: 15296 RVA: 0x000EF4F4 File Offset: 0x000ED6F4
			public MusicTheme GetBattleEndTheme(BasicCultureObject culture, bool isVictorious)
			{
				MusicTheme musicTheme;
				if (isVictorious)
				{
					MusicTheme victoryThemeForCulture = this.GetVictoryThemeForCulture(culture);
					if (victoryThemeForCulture == MusicTheme.None)
					{
						musicTheme = MusicTheme.BattleVictory;
					}
					else
					{
						musicTheme = victoryThemeForCulture;
					}
				}
				else
				{
					musicTheme = MusicTheme.BattleDefeat;
				}
				return musicTheme;
			}

			// Token: 0x04001CE4 RID: 7396
			private const float DefaultSelectionFactorForFactionSpecificBattleTheme = 0.35f;

			// Token: 0x04001CE5 RID: 7397
			private const float SelectionFactorDecayAmountForFactionSpecificBattleTheme = 0.1f;

			// Token: 0x04001CE6 RID: 7398
			private const float SelectionFactorGrowthAmountForFactionSpecificBattleTheme = 0.1f;

			// Token: 0x04001CE7 RID: 7399
			private const float DefaultSelectionFactorForFactionSpecificVictoryTheme = 0.65f;

			// Token: 0x04001CE8 RID: 7400
			private float _factionSpecificBattleThemeSelectionFactor;

			// Token: 0x04001CE9 RID: 7401
			private float _factionSpecificSiegeThemeSelectionFactor;
		}
	}
}
