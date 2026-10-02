using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TaleWorlds.AchievementSystem;
using TaleWorlds.ActivitySystem;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Core;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;
using TaleWorlds.SaveSystem;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D7 RID: 727
	public sealed class Module : DotNetObject, IGameStateManagerOwner
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06002A14 RID: 10772 RVA: 0x000A0092 File Offset: 0x0009E292
		// (set) Token: 0x06002A15 RID: 10773 RVA: 0x000A0099 File Offset: 0x0009E299
		public static Module CurrentModule { get; private set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002A16 RID: 10774 RVA: 0x000A00A1 File Offset: 0x0009E2A1
		// (set) Token: 0x06002A17 RID: 10775 RVA: 0x000A00A9 File Offset: 0x0009E2A9
		public GameStateManager GlobalGameStateManager { get; private set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002A18 RID: 10776 RVA: 0x000A00B2 File Offset: 0x0009E2B2
		public bool MultiplayerRequested
		{
			get
			{
				return this.StartupInfo.StartupType == GameStartupType.Multiplayer || PlatformServices.SessionInvitationType == SessionInvitationType.Multiplayer || PlatformServices.IsPlatformRequestedMultiplayer;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002A19 RID: 10777 RVA: 0x000A00D1 File Offset: 0x0009E2D1
		// (set) Token: 0x06002A1A RID: 10778 RVA: 0x000A00D9 File Offset: 0x0009E2D9
		public bool ReturnToEditorState { get; private set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002A1B RID: 10779 RVA: 0x000A00E2 File Offset: 0x0009E2E2
		// (set) Token: 0x06002A1C RID: 10780 RVA: 0x000A00EA File Offset: 0x0009E2EA
		public bool LoadingFinished { get; private set; }

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000A00F3 File Offset: 0x0009E2F3
		// (set) Token: 0x06002A1E RID: 10782 RVA: 0x000A00FB File Offset: 0x0009E2FB
		public GameTextManager GlobalTextManager { get; private set; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000A0104 File Offset: 0x0009E304
		// (set) Token: 0x06002A20 RID: 10784 RVA: 0x000A010C File Offset: 0x0009E30C
		public bool IsOnlyCoreContentEnabled { get; private set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x000A0115 File Offset: 0x0009E315
		// (set) Token: 0x06002A22 RID: 10786 RVA: 0x000A011D File Offset: 0x0009E31D
		public JobManager JobManager { get; private set; }

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x000A0126 File Offset: 0x0009E326
		// (set) Token: 0x06002A24 RID: 10788 RVA: 0x000A012E File Offset: 0x0009E32E
		public GameStartupInfo StartupInfo { get; private set; }

		// Token: 0x06002A25 RID: 10789 RVA: 0x000A0138 File Offset: 0x0009E338
		private Module()
		{
			MBDebug.Print("Creating module...", 0, Debug.DebugColor.White, 17592186044416UL);
			this.StartupInfo = new GameStartupInfo();
			this._testContext = new TestContext();
			this._subModuleBases = new Dictionary<SubModuleInfo, MBSubModuleBase>();
			this.GlobalGameStateManager = new GameStateManager(this, GameStateManager.GameStateManagerType.Global);
			GameStateManager.Current = this.GlobalGameStateManager;
			this.GlobalTextManager = new GameTextManager();
			this.JobManager = new JobManager();
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000A01BC File Offset: 0x0009E3BC
		public MBReadOnlyList<MBSubModuleBase> CollectSubModules()
		{
			MBList<MBSubModuleBase> mblist = new MBList<MBSubModuleBase>();
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetAllModules())
			{
				if (moduleInfo.IsActive)
				{
					foreach (SubModuleInfo subModuleInfo in moduleInfo.SubModules)
					{
						MBSubModuleBase subModuleBase = this.GetSubModuleBase(subModuleInfo);
						if (subModuleBase != null)
						{
							mblist.Add(subModuleBase);
						}
					}
				}
			}
			return mblist;
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000A0268 File Offset: 0x0009E468
		internal static void CreateModule()
		{
			Module.CurrentModule = new Module();
			Utilities.SetLoadingScreenPercentage(0.4f);
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x000A0280 File Offset: 0x0009E480
		private AssemblyLoader.AssemblyLoadResult AddSubModule(SubModuleInfo subModuleInfo, Assembly subModuleAssembly)
		{
			ConstructorInfo constructor = subModuleAssembly.GetType(subModuleInfo.SubModuleClassTypeName).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new Type[0], null);
			Dictionary<string, Type> dictionary;
			AssemblyLoader.AssemblyLoadResult assemblyLoadResult = this.CollectModuleAssemblyTypes(subModuleInfo, subModuleAssembly, out dictionary);
			if (assemblyLoadResult == AssemblyLoader.AssemblyLoadResult.Success)
			{
				Managed.AddTypes(dictionary);
				MBSubModuleBase mbsubModuleBase = (MBSubModuleBase)constructor.Invoke(new object[0]);
				this._subModuleBases.Add(subModuleInfo, mbsubModuleBase);
			}
			return assemblyLoadResult;
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000A02E0 File Offset: 0x0009E4E0
		private AssemblyLoader.AssemblyLoadResult CollectModuleAssemblyTypes(SubModuleInfo subModule, Assembly moduleAssembly, out Dictionary<string, Type> types)
		{
			AssemblyLoader.AssemblyLoadResult assemblyLoadResult;
			try
			{
				types = new Dictionary<string, Type>();
				foreach (Type type in moduleAssembly.GetTypes())
				{
					if (typeof(ManagedObject).IsAssignableFrom(type) || typeof(DotNetObject).IsAssignableFrom(type))
					{
						types.Add(type.Name, type);
					}
				}
				assemblyLoadResult = AssemblyLoader.AssemblyLoadResult.Success;
			}
			catch (Exception ex)
			{
				MBDebug.Print("Error while getting types and loading" + ex.Message + "\nException: " + ex.GetType().Name, 0, Debug.DebugColor.White, 17592186044416UL);
				ReflectionTypeLoadException ex2;
				if ((ex2 = ex as ReflectionTypeLoadException) != null)
				{
					string text = "";
					foreach (Exception ex3 in ex2.LoaderExceptions)
					{
						MBDebug.Print("Loader Exceptions: " + ex3.Message, 0, Debug.DebugColor.White, 17592186044416UL);
						text = text + ex3.Message + Environment.NewLine;
					}
					Debug.SetCrashReportCustomString(text);
					foreach (Type type2 in ex2.Types)
					{
						if (type2 != null)
						{
							MBDebug.Print("Loaded Types: " + type2.FullName, 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
				}
				if (ex.InnerException != null)
				{
					MBDebug.Print("Inner excetion: " + ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				}
				types = null;
				assemblyLoadResult = AssemblyLoader.AssemblyLoadResult.CriticalError;
			}
			return assemblyLoadResult;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000A0484 File Offset: 0x0009E684
		private void InitializeSubModuleBases()
		{
			Managed.AddConstructorDelegateOfClass<SpawnedItemEntity>();
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnSubModuleLoad();
			}
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000A04E0 File Offset: 0x0009E6E0
		private void OnNewModuleLoaded()
		{
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnNewModuleLoad();
			}
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000A0538 File Offset: 0x0009E738
		private MBSubModuleBase GetSubModuleBase(SubModuleInfo subModuleInfo)
		{
			MBSubModuleBase mbsubModuleBase;
			if (this._subModuleBases.TryGetValue(subModuleInfo, out mbsubModuleBase))
			{
				return mbsubModuleBase;
			}
			return null;
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000A0558 File Offset: 0x0009E758
		private void FinalizeSubModulesBases()
		{
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnSubModuleUnloaded();
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000A05B0 File Offset: 0x0009E7B0
		[MBCallback(null, false)]
		internal void LoadSingleModule(string modulePath)
		{
			List<ModuleInfo> list = new List<ModuleInfo>();
			list.Add(ModuleHelper.InitializeSingleModule(modulePath));
			LocalizedTextManager.AddLocalizationXml(modulePath);
			this.LoadSubModules(list, true);
			BannerManager.ResetAndLoad();
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000A05E4 File Offset: 0x0009E7E4
		[MBCallback(null, false)]
		internal void Initialize()
		{
			MBDebug.Print("Module Initialize begin...", 0, Debug.DebugColor.White, 17592186044416UL);
			TWParallel.InitializeAndSetImplementation(new NativeParallelDriver());
			MBSaveLoad.SetSaveDriver(new AsyncFileSaveDriver());
			this.ProcessApplicationArguments();
			this.SetWindowTitle();
			this._initialStateOptions = new List<InitialStateOption>();
			this.FillMultiplayerGameTypes();
			if (!GameNetwork.IsDedicatedServer && !MBDebug.TestModeEnabled)
			{
				MBDebug.Print("Loading platform services...", 0, Debug.DebugColor.White, 17592186044416UL);
				this.LoadPlatformServices();
			}
			string[] array = null;
			ModuleHelper.InitializeModules(Utilities.GetModulesNames(), array);
			this.LoadLocalizationXmls();
			this.GlobalTextManager.LoadDefaultTexts();
			this.IsOnlyCoreContentEnabled = Utilities.IsOnlyCoreContentEnabled();
			NativeConfig.OnConfigChanged();
			List<ModuleInfo> modules = ModuleHelper.GetModules(null);
			this.LoadSubModules(modules, false);
			MBDebug.Print("Adding trace listener...", 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print("MBModuleBase Initialize begin...", 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print("MBModuleBase Initialize end...", 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.FindGameNetworkMessages();
			GameNetwork.FindSynchedMissionObjectTypes();
			HasTableauCache.CollectTableauCacheTypes();
			MBDebug.Print("Module Initialize end...", 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.TestModeEnabled = Utilities.CommandLineArgumentExists("/runTest");
			this.FindMissions();
			NativeOptions.ReadRGLConfigFiles();
			BannerlordConfig.Initialize();
			EngineController.ConfigChange += this.OnConfigChanged;
			EngineController.OnConstrainedStateChanged += this.OnConstrainedStateChange;
			ScreenManager.FocusGained += this.OnFocusGained;
			ScreenManager.PlatformTextRequested += this.OnPlatformTextRequested;
			PlatformServices.Instance.OnTextEnteredFromPlatform += this.OnTextEnteredFromPlatform;
			PlatformServices.Instance.OnTextCanceledFromPlatform += this.OnTextCanceledFromPlatform;
			SaveManager.InitializeGlobalDefinitionContext();
			this.EnsureAsyncJobsAreFinished();
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000A07A0 File Offset: 0x0009E9A0
		private bool OnPlatformTextRequested(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum)
		{
			IPlatformServices instance = PlatformServices.Instance;
			return instance != null && instance.ShowGamepadTextInput(descriptionText, initialText, (uint)maxLength, keyboardTypeEnum == 2);
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x000A07BC File Offset: 0x0009E9BC
		private void LoadLocalizationXmls()
		{
			List<string> list = new List<string>();
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules(null))
			{
				list.Add(moduleInfo.FolderPath);
			}
			LocalizedTextManager.LoadLocalizationXmls(list.ToArray());
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000A0828 File Offset: 0x0009EA28
		private void SetWindowTitle()
		{
			string applicationName = Utilities.GetApplicationName();
			string text;
			if (this.StartupInfo.StartupType == GameStartupType.Singleplayer)
			{
				text = applicationName + " - Singleplayer";
			}
			else if (this.StartupInfo.StartupType == GameStartupType.Multiplayer)
			{
				text = applicationName + " - Multiplayer";
			}
			else if (this.StartupInfo.StartupType == GameStartupType.GameServer)
			{
				text = string.Concat(new object[]
				{
					"[",
					Utilities.GetCurrentProcessID(),
					"] ",
					applicationName,
					" Dedicated Server Port:",
					this.StartupInfo.ServerPort
				});
			}
			else
			{
				text = applicationName;
			}
			text = Utilities.ProcessWindowTitle(text);
			Utilities.SetWindowTitle(text);
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000A08DA File Offset: 0x0009EADA
		private void EnsureAsyncJobsAreFinished()
		{
			if (!GameNetwork.IsDedicatedServer)
			{
				while (!MBMusicManager.IsCreationCompleted())
				{
					Thread.Sleep(1);
				}
			}
			if (!GameNetwork.IsDedicatedServer && !MBDebug.TestModeEnabled)
			{
				while (!AchievementManager.AchievementService.IsInitializationCompleted())
				{
					Thread.Sleep(1);
				}
			}
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000A0914 File Offset: 0x0009EB14
		private void ProcessApplicationArguments()
		{
			this.StartupInfo.StartupType = GameStartupType.None;
			string[] array = Utilities.GetFullCommandLineString().Split(new char[] { ' ' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].ToLowerInvariant();
				if (text == "/dedicatedmatchmakingserver".ToLower())
				{
					int num = Convert.ToInt32(array[i + 1]);
					string text2 = array[i + 2];
					sbyte b = Convert.ToSByte(array[i + 3]);
					string text3 = array[i + 4];
					i += 4;
					this.StartupInfo.StartupType = GameStartupType.GameServer;
					this.StartupInfo.DedicatedServerType = DedicatedServerType.Matchmaker;
					this.StartupInfo.ServerPort = num;
					this.StartupInfo.ServerRegion = text2;
					this.StartupInfo.ServerPriority = b;
					this.StartupInfo.ServerGameMode = text3.Trim();
				}
				else if (text == "/dedicatedcustomserver".ToLower())
				{
					int num2 = Convert.ToInt32(array[i + 1]);
					string text4 = array[i + 2];
					int num3 = Convert.ToInt32(array[i + 3]);
					i += 3;
					this.StartupInfo.StartupType = GameStartupType.GameServer;
					this.StartupInfo.DedicatedServerType = DedicatedServerType.Custom;
					this.StartupInfo.ServerPort = num2;
					this.StartupInfo.ServerRegion = text4;
					this.StartupInfo.Permission = num3;
				}
				else if (text == "/dedicatedcommunityserver".ToLower())
				{
					int num4 = Convert.ToInt32(array[i + 1]);
					i++;
					this.StartupInfo.StartupType = GameStartupType.GameServer;
					this.StartupInfo.DedicatedServerType = DedicatedServerType.Community;
					this.StartupInfo.ServerPort = num4;
				}
				else if (text == "/dedicatedcustomserverconfigfile".ToLower())
				{
					string text5 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerConfigFile = text5;
				}
				else if (text == "/dedicatedcustomservernameoverride".ToLower())
				{
					string text6 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerNameOverride = text6;
				}
				else if (text == "/dedicatedcustomserverpasswordoverride".ToLower())
				{
					string text7 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerPasswordOverride = text7;
				}
				else if (text == "/dedicatedcustomserverauthtoken".ToLower())
				{
					string text8 = array[i + 1];
					i++;
					this.StartupInfo.CustomGameServerAuthToken = text8;
				}
				else if (text == "/dedicatedcustomserverDontAllowOptionalModules".ToLower())
				{
					this.StartupInfo.CustomGameServerAllowsOptionalModules = false;
				}
				else if (text == "/playerHostedDedicatedServer".ToLower())
				{
					this.StartupInfo.PlayerHostedDedicatedServer = true;
				}
				else if (text == "/singleplatform")
				{
					this.StartupInfo.IsSinglePlatformServer = true;
				}
				else if (text == "/customserverhost")
				{
					string text9 = array[i + 1];
					i++;
					this.StartupInfo.CustomServerHostIP = text9;
				}
				else if (text == "/singleplayer".ToLower())
				{
					this.StartupInfo.StartupType = GameStartupType.Singleplayer;
				}
				else if (text == "/multiplayer".ToLower())
				{
					this.StartupInfo.StartupType = GameStartupType.Multiplayer;
				}
				else if (text == "/clientConfigurationCategory".ToLower())
				{
					ClientApplicationConfiguration.SetDefaultConfigurationCategory(array[i + 1]);
					i++;
				}
				else if (text == "/overridenusername".ToLower())
				{
					string text10 = array[i + 1];
					this.StartupInfo.OverridenUserName = text10;
					i++;
				}
				else if (text.StartsWith("-PlatformInterface".ToLowerInvariant()))
				{
					this.StartupInfo.PlatformInterface = text.Split(new char[] { '=' })[1];
				}
				else if (text.StartsWith("-epicuserid".ToLowerInvariant()))
				{
					this.StartupInfo.EpicUserId = text.Split(new char[] { '=' })[1];
				}
				else if (text.StartsWith("-epicusername".ToLowerInvariant()))
				{
					this.StartupInfo.EpicUserName = text.Split(new char[] { '=' })[1];
				}
				else if (text == "/continuegame".ToLower())
				{
					this.StartupInfo.IsContinueGame = true;
				}
				else if (text == "/serverbandwidthlimitmbps".ToLower())
				{
					double num5 = Convert.ToDouble(array[i + 1]);
					this.StartupInfo.ServerBandwidthLimitInMbps = num5;
					i++;
				}
				else if (text == "/tickrate".ToLower())
				{
					int num6 = Convert.ToInt32(array[i + 1]);
					this.StartupInfo.ServerTickRate = num6;
					i++;
				}
			}
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x000A0DB8 File Offset: 0x0009EFB8
		internal void OnApplicationTick(float dt)
		{
			bool isOnlyCoreContentEnabled = this.IsOnlyCoreContentEnabled;
			this.IsOnlyCoreContentEnabled = Utilities.IsOnlyCoreContentEnabled();
			if (isOnlyCoreContentEnabled != this.IsOnlyCoreContentEnabled && isOnlyCoreContentEnabled)
			{
				InitialState initialState;
				if ((initialState = GameStateManager.Current.ActiveState as InitialState) != null)
				{
					Utilities.DisableCoreGame();
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=CaSafuAH}Content Download Complete", null).ToString(), new TextObject("{=1nKa4pQX}Rest of the game content has been downloaded.", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, delegate
					{
						initialState.RefreshContentState();
					}, null, "", 0f, null, null, null), false, false);
				}
				else
				{
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=CaSafuAH}Content Download Complete", null).ToString(), new TextObject("{=BFhMw4bl}Rest of the game content has been downloaded. Do you want to return to the main menu?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), new Action(this.OnConfirmReturnToMainMenu), null, "", 0f, null, null, null), false, false);
					this._enableCoreContentOnReturnToRoot = true;
				}
			}
			if (this._synchronizationContext == null)
			{
				this._synchronizationContext = new SingleThreadedSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(this._synchronizationContext);
			}
			this._testContext.OnApplicationTick(dt);
			if (!GameNetwork.MultiplayerDisabled)
			{
				this.OnNetworkTick(dt);
			}
			if (GameStateManager.Current == null)
			{
				GameStateManager.Current = this.GlobalGameStateManager;
			}
			if (GameStateManager.Current == this.GlobalGameStateManager)
			{
				if (this.LoadingFinished && this.GlobalGameStateManager.ActiveState == null)
				{
					if (this.ReturnToEditorState)
					{
						this.ReturnToEditorState = false;
						this.SetEditorScreenAsRootScreen();
					}
					else
					{
						this.SetInitialModuleScreenAsRootScreen();
					}
				}
				this.GlobalGameStateManager.OnTick(dt);
			}
			Utilities.RunJobs();
			IPlatformServices instance = PlatformServices.Instance;
			if (instance != null)
			{
				instance.Tick(dt);
			}
			this._synchronizationContext.Tick();
			if (GameManagerBase.Current != null)
			{
				GameManagerBase.Current.OnTick(dt);
			}
			foreach (MBSubModuleBase mbsubModuleBase in this.CollectSubModules())
			{
				mbsubModuleBase.OnApplicationTick(dt);
			}
			this.JobManager.OnTick(dt);
			AvatarServices.UpdateAvatarServices(dt);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000A0FF8 File Offset: 0x0009F1F8
		private void OnConfirmReturnToMainMenu()
		{
			MBGameManager.EndGame();
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000A1000 File Offset: 0x0009F200
		private void OnNetworkTick(float dt)
		{
			foreach (MBSubModuleBase mbsubModuleBase in this.CollectSubModules())
			{
				mbsubModuleBase.OnNetworkTick(dt);
			}
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000A1054 File Offset: 0x0009F254
		[MBCallback(null, false)]
		internal void RunTest(string commandLine)
		{
			MBDebug.Print(" TEST MODE ENABLED. Command line string: " + commandLine, 0, Debug.DebugColor.White, 17592186044416UL);
			this._testContext.RunTestAux(commandLine);
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000A107E File Offset: 0x0009F27E
		[MBCallback(null, true)]
		internal void TickTest(float dt)
		{
			this._testContext.TickTest(dt);
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000A108C File Offset: 0x0009F28C
		[MBCallback(null, false)]
		internal void OnDumpCreated()
		{
			if (TestCommonBase.BaseInstance != null)
			{
				TestCommonBase.BaseInstance.ToggleTimeoutTimer();
				TestCommonBase.BaseInstance.StartTimeoutTimer();
			}
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000A10A9 File Offset: 0x0009F2A9
		[MBCallback(null, false)]
		internal void OnDumpCreationStarted()
		{
			if (TestCommonBase.BaseInstance != null)
			{
				TestCommonBase.BaseInstance.ToggleTimeoutTimer();
			}
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000A10BC File Offset: 0x0009F2BC
		public static void GetMetaMeshPackageMapping(Dictionary<string, string> metaMeshPackageMappings)
		{
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.HasArmorComponent)
				{
					string text = ((itemObject.Culture != null) ? itemObject.Culture.StringId : "shared") + "_armor";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text;
					metaMeshPackageMappings[itemObject.MultiMeshName + "_converted"] = text;
					metaMeshPackageMappings[itemObject.MultiMeshName + "_converted_slim"] = text;
					metaMeshPackageMappings[itemObject.MultiMeshName + "_slim"] = text;
				}
				if (itemObject.WeaponComponent != null)
				{
					string text2 = ((itemObject.Culture != null) ? itemObject.Culture.StringId : "shared") + "_weapon";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text2;
					if (itemObject.HolsterMeshName != null)
					{
						metaMeshPackageMappings[itemObject.HolsterMeshName] = text2;
					}
					if (itemObject.HolsterWithWeaponMeshName != null)
					{
						metaMeshPackageMappings[itemObject.HolsterWithWeaponMeshName] = text2;
					}
				}
				if (itemObject.HasHorseComponent)
				{
					string text3 = "horses";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text3;
				}
				if (itemObject.IsFood)
				{
					string text4 = "food";
					metaMeshPackageMappings[itemObject.MultiMeshName] = text4;
				}
			}
			foreach (CraftingPiece craftingPiece in Game.Current.ObjectManager.GetObjectTypeList<CraftingPiece>())
			{
				string text5 = ((craftingPiece.Culture != null) ? craftingPiece.Culture.StringId : "shared") + "_crafting";
				metaMeshPackageMappings[craftingPiece.MeshName] = text5;
			}
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000A12CC File Offset: 0x0009F4CC
		public static void GetItemMeshNames(HashSet<string> itemMeshNames)
		{
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (!itemObject.IsCraftedWeapon)
				{
					itemMeshNames.Add(itemObject.MultiMeshName);
				}
				if (itemObject.PrimaryWeapon != null)
				{
					if (itemObject.FlyingMeshName != null && !itemObject.FlyingMeshName.IsEmpty<char>())
					{
						itemMeshNames.Add(itemObject.FlyingMeshName);
					}
					if (itemObject.HolsterMeshName != null && !itemObject.HolsterMeshName.IsEmpty<char>())
					{
						itemMeshNames.Add(itemObject.HolsterMeshName);
					}
					if (itemObject.HolsterWithWeaponMeshName != null && !itemObject.HolsterWithWeaponMeshName.IsEmpty<char>())
					{
						itemMeshNames.Add(itemObject.HolsterWithWeaponMeshName);
					}
				}
				if (itemObject.HasHorseComponent)
				{
					foreach (KeyValuePair<string, bool> keyValuePair in itemObject.HorseComponent.AdditionalMeshesNameList)
					{
						if (keyValuePair.Key != null && !keyValuePair.Key.IsEmpty<char>())
						{
							itemMeshNames.Add(keyValuePair.Key);
						}
					}
				}
			}
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x000A1418 File Offset: 0x0009F618
		[MBCallback(null, false)]
		internal string GetMetaMeshPackageMapping()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Module.GetMetaMeshPackageMapping(dictionary);
			string text = "";
			foreach (string text2 in dictionary.Keys)
			{
				text = string.Concat(new string[]
				{
					text,
					text2,
					"|",
					dictionary[text2],
					","
				});
			}
			return text;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000A14A8 File Offset: 0x0009F6A8
		[MBCallback(null, false)]
		internal string GetItemMeshNames()
		{
			HashSet<string> hashSet = new HashSet<string>();
			Module.GetItemMeshNames(hashSet);
			foreach (CraftingPiece craftingPiece in MBObjectManager.Instance.GetObjectTypeList<CraftingPiece>())
			{
				hashSet.Add(craftingPiece.MeshName);
				if (craftingPiece.BladeData != null)
				{
					hashSet.Add(craftingPiece.BladeData.HolsterMeshName);
				}
			}
			foreach (BannerIconGroup bannerIconGroup in BannerManager.Instance.BannerIconGroups)
			{
				foreach (KeyValuePair<int, BannerIconData> keyValuePair in bannerIconGroup.AllIcons)
				{
					if (keyValuePair.Value.MaterialName != "")
					{
						hashSet.Add(keyValuePair.Value.MaterialName + keyValuePair.Value.TextureIndex);
					}
				}
			}
			string text = "";
			foreach (string text2 in hashSet)
			{
				if (text2 != null && !text2.IsEmpty<char>())
				{
					text = text + text2 + "#";
				}
			}
			return text;
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000A1654 File Offset: 0x0009F854
		[CommandLineFunctionality.CommandLineArgumentFunction("get_item_mesh_names", "module")]
		public static string GetCraftedItemMeshNames(List<string> arguments)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (CraftingPiece craftingPiece in MBObjectManager.Instance.GetObjectTypeList<CraftingPiece>())
			{
				hashSet.Add(craftingPiece.MeshName);
				if (craftingPiece.BladeData != null)
				{
					hashSet.Add(craftingPiece.BladeData.HolsterMeshName);
				}
			}
			string text = "";
			foreach (string text2 in hashSet)
			{
				if (text2 != null && !text2.IsEmpty<char>())
				{
					text = text + text2 + "#";
				}
			}
			return text;
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000A172C File Offset: 0x0009F92C
		[MBCallback(null, false)]
		internal string GetHorseMaterialNames()
		{
			HashSet<string> hashSet = new HashSet<string>();
			string text = "";
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.HasHorseComponent && itemObject.HorseComponent.HorseMaterialNames != null && itemObject.HorseComponent.HorseMaterialNames.Count > 0)
				{
					foreach (HorseComponent.MaterialProperty materialProperty in itemObject.HorseComponent.HorseMaterialNames)
					{
						hashSet.Add(materialProperty.Name);
					}
				}
			}
			foreach (string text2 in hashSet)
			{
				if (text2 != null && !text2.IsEmpty<char>())
				{
					text = text + text2 + "#";
				}
			}
			return text;
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000A185C File Offset: 0x0009FA5C
		public void SetInitialModuleScreenAsRootScreen()
		{
			if (GameStateManager.Current != this.GlobalGameStateManager)
			{
				GameStateManager.Current = this.GlobalGameStateManager;
			}
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnBeforeInitialModuleScreenSetAsRoot();
			}
			if (!GameNetwork.IsDedicatedServer)
			{
				string text = ModuleHelper.GetModuleFullPath("Native") + "Videos/TWLogo_and_Partners.ivf";
				string text2 = ModuleHelper.GetModuleFullPath("Native") + "Videos/TWLogo_and_Partners.ogg";
				if (!this._splashScreenPlayed && File.Exists(text) && (text2 == "" || File.Exists(text2)) && !Debugger.IsAttached)
				{
					VideoPlaybackState videoPlaybackState = this.GlobalGameStateManager.CreateState<VideoPlaybackState>();
					videoPlaybackState.SetStartingParameters(text, text2, string.Empty, 30f, true);
					videoPlaybackState.SetOnVideoFinisedDelegate(delegate
					{
						this.OnInitialModuleScreenActivated(true);
					});
					this.GlobalGameStateManager.CleanAndPushState(videoPlaybackState, 0);
					this._splashScreenPlayed = true;
					return;
				}
				this.OnInitialModuleScreenActivated(false);
			}
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000A1978 File Offset: 0x0009FB78
		private void OnInitialModuleScreenActivated(bool isFromSplashScreenVideo)
		{
			Utilities.EnableGlobalLoadingWindow();
			LoadingWindow.EnableGlobalLoadingWindow();
			if (!this.StartupInfo.IsContinueGame)
			{
				this.StartupInfo.IsContinueGame = PlatformServices.IsPlatformRequestedContinueGame && !this.IsOnlyCoreContentEnabled;
			}
			if (this._enableCoreContentOnReturnToRoot)
			{
				Utilities.DisableCoreGame();
				this._enableCoreContentOnReturnToRoot = false;
			}
			if (this.IsOnlyCoreContentEnabled && PlatformServices.SessionInvitationType == SessionInvitationType.Multiplayer)
			{
				PlatformServices.OnSessionInvitationHandled();
			}
			if (this.IsOnlyCoreContentEnabled && PlatformServices.IsPlatformRequestedMultiplayer)
			{
				PlatformServices.OnPlatformMultiplayerRequestHandled();
			}
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules((ModuleInfo x) => !x.IsActive))
			{
				this.ActivateModule(moduleInfo.Id);
			}
			if (this.IsOnlyCoreContentEnabled || !this.MultiplayerRequested)
			{
				this.GlobalGameStateManager.CleanAndPushState(this.GlobalGameStateManager.CreateState<InitialState>(), 0);
			}
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnInitialState();
			}
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000A1ACC File Offset: 0x0009FCCC
		private void OnSignInStateUpdated(bool isLoggedIn, TextObject message)
		{
			if (!isLoggedIn && !(this.GlobalGameStateManager.ActiveState is ProfileSelectionState))
			{
				this.GlobalGameStateManager.CleanAndPushState(this.GlobalGameStateManager.CreateState<ProfileSelectionState>(), 0);
			}
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000A1AFC File Offset: 0x0009FCFC
		[MBCallback(null, false)]
		internal bool SetEditorScreenAsRootScreen()
		{
			if (GameStateManager.Current != this.GlobalGameStateManager)
			{
				GameStateManager.Current = this.GlobalGameStateManager;
			}
			if (!(this.GlobalGameStateManager.ActiveState is EditorState))
			{
				this.GlobalGameStateManager.CleanAndPushState(GameStateManager.Current.CreateState<EditorState>(), 0);
				return true;
			}
			return false;
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000A1B4C File Offset: 0x0009FD4C
		private bool CheckAssemblyForMissionMethods(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(MissionMethod));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				if (referencedAssemblies[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000A1BA4 File Offset: 0x0009FDA4
		private void FindMissions()
		{
			MBDebug.Print("Searching Mission Methods", 0, Debug.DebugColor.White, 17592186044416UL);
			this._missionInfos = new List<MissionInfo>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (this.CheckAssemblyForMissionMethods(assembly))
				{
					foreach (Type type in assembly.GetTypesSafe(null))
					{
						object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(MissionManager), true);
						if (customAttributesSafe != null && customAttributesSafe.Length != 0)
						{
							list.Add(type);
						}
					}
				}
			}
			MBDebug.Print("Found " + list.Count + " mission managers", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (Type type2 in list)
			{
				foreach (MethodInfo methodInfo in type2.GetMethods(BindingFlags.Static | BindingFlags.Public))
				{
					object[] customAttributesSafe2 = methodInfo.GetCustomAttributesSafe(typeof(MissionMethod), true);
					if (customAttributesSafe2 != null && customAttributesSafe2.Length != 0)
					{
						MissionMethod missionMethod = customAttributesSafe2[0] as MissionMethod;
						MissionInfo missionInfo = new MissionInfo();
						missionInfo.Creator = methodInfo;
						missionInfo.Manager = type2;
						missionInfo.UsableByEditor = missionMethod.UsableByEditor;
						missionInfo.Name = methodInfo.Name;
						if (missionInfo.Name.StartsWith("Open"))
						{
							missionInfo.Name = missionInfo.Name.Substring(4);
						}
						if (missionInfo.Name.EndsWith("Mission"))
						{
							missionInfo.Name = missionInfo.Name.Substring(0, missionInfo.Name.Length - 7);
						}
						MissionInfo missionInfo2 = missionInfo;
						missionInfo2.Name = missionInfo2.Name + "[" + type2.Name + "]";
						this._missionInfos.Add(missionInfo);
					}
				}
			}
			MBDebug.Print("Found " + this._missionInfos.Count + " missions", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000A1E2C File Offset: 0x000A002C
		[MBCallback(null, false)]
		internal string GetMissionControllerClassNames()
		{
			string text = "";
			for (int i = 0; i < this._missionInfos.Count; i++)
			{
				MissionInfo missionInfo = this._missionInfos[i];
				if (missionInfo.UsableByEditor)
				{
					text += missionInfo.Name;
					if (i + 1 != this._missionInfos.Count)
					{
						text += " ";
					}
				}
			}
			return text;
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000A1E94 File Offset: 0x000A0094
		private void LoadPlatformServices()
		{
			IPlatformServices platformServices = null;
			Assembly assembly = null;
			string fullModulePath = EngineApplicationInterface.IUtil.GetFullModulePath("Native");
			PlatformInitParams platformInitParams = new PlatformInitParams();
			if (ApplicationPlatform.CurrentPlatform == Platform.WindowsSteam)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.Steam.dll", true);
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.WindowsEpic)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.Epic.dll", true);
				platformInitParams.Add("PlatformInterface", this.StartupInfo.PlatformInterface);
				platformInitParams.Add("EpicUserId", this.StartupInfo.EpicUserId);
				platformInitParams.Add("EpicUserName", this.StartupInfo.EpicUserName);
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.WindowsGOG)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.GOG.dll", true);
				platformInitParams.Add("AchievementDataXmlPath", Path.Combine(fullModulePath, "ModuleData", "AchievementData", "gog_achievement_data.xml"));
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.GDKDesktop || ApplicationPlatform.CurrentPlatform == Platform.Durango)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.GDK.dll", true);
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.Orbis)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.PlatformService.PS.dll", true);
				platformInitParams.Add("AchievementDataXmlPath", Path.Combine(fullModulePath, "ModuleData", "AchievementData", "ps_achievement_data.xml"));
			}
			else if (ApplicationPlatform.CurrentPlatform == Platform.WindowsNoPlatform)
			{
				string text = "TestUser" + DateTime.Now.Ticks % 10000L;
				if (!string.IsNullOrEmpty(this.StartupInfo.OverridenUserName))
				{
					text = this.StartupInfo.OverridenUserName;
				}
				platformServices = new TestPlatformServices(text);
			}
			if (assembly != null)
			{
				List<Type> typesSafe = assembly.GetTypesSafe(null);
				Type type = null;
				foreach (Type type2 in typesSafe)
				{
					if (type2.GetInterfaces().Contains(typeof(IPlatformServices)))
					{
						type = type2;
						break;
					}
				}
				platformServices = (IPlatformServices)type.GetConstructor(new Type[] { typeof(PlatformInitParams) }).Invoke(new object[] { platformInitParams });
			}
			if (platformServices != null)
			{
				PlatformServices.Setup(platformServices);
				PlatformServices.OnSessionInvitationAccepted = (Action<SessionInvitationType>)Delegate.Combine(PlatformServices.OnSessionInvitationAccepted, new Action<SessionInvitationType>(this.OnSessionInvitationAccepted));
				PlatformServices.OnPlatformRequestedMultiplayer = (Action)Delegate.Combine(PlatformServices.OnPlatformRequestedMultiplayer, new Action(this.OnPlatformRequestedMultiplayer));
				BannerlordFriendListService bannerlordFriendListService = new BannerlordFriendListService();
				ClanFriendListService clanFriendListService = new ClanFriendListService();
				RecentPlayersFriendListService recentPlayersFriendListService = new RecentPlayersFriendListService();
				PlatformServices.Initialize(new IFriendListService[] { bannerlordFriendListService, clanFriendListService, recentPlayersFriendListService });
				AchievementManager.AchievementService = platformServices.GetAchievementService();
				ActivityManager.ActivityService = platformServices.GetActivityService();
			}
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000A2178 File Offset: 0x000A0378
		private void OnSessionInvitationAccepted(SessionInvitationType targetGameType)
		{
			if (targetGameType == SessionInvitationType.Multiplayer)
			{
				if (this.IsOnlyCoreContentEnabled)
				{
					PlatformServices.OnSessionInvitationHandled();
					return;
				}
				this.JobManager.AddJob(new OnSessionInvitationAcceptedJob(targetGameType));
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000A219D File Offset: 0x000A039D
		private void OnPlatformRequestedMultiplayer()
		{
			if (this.IsOnlyCoreContentEnabled)
			{
				PlatformServices.OnPlatformMultiplayerRequestHandled();
				return;
			}
			this.JobManager.AddJob(new OnPlatformRequestedMultiplayerJob());
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000A21C0 File Offset: 0x000A03C0
		private void LoadSubModules(List<ModuleInfo> modules, bool loadNewModules)
		{
			MBDebug.Print("Loading submodules...", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (ModuleInfo moduleInfo in modules)
			{
				XmlResource.GetMbprojxmls(moduleInfo.Id);
				XmlResource.GetXmlListAndApply(moduleInfo.Id);
			}
			List<SubModuleInfo> list = new List<SubModuleInfo>();
			new List<ModuleInfo>();
			foreach (ModuleInfo moduleInfo2 in modules)
			{
				foreach (SubModuleInfo subModuleInfo in moduleInfo2.SubModules)
				{
					if (this.CheckIfSubmoduleCanBeLoadable(subModuleInfo) && !this._subModuleBases.ContainsKey(subModuleInfo))
					{
						string text = Path.Combine(moduleInfo2.FolderPath, "bin", Common.ConfigName);
						string text2 = Path.Combine(text, subModuleInfo.DLLName);
						string text3 = ManagedDllFolder.Name + subModuleInfo.DLLName;
						MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>> mblist = new MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>>();
						foreach (string text4 in subModuleInfo.Assemblies)
						{
							string text5 = Path.Combine(text, text4);
							string text6 = ManagedDllFolder.Name + text4;
							AssemblyLoader.AssemblyLoadResult assemblyLoadResult;
							AssemblyLoader.LoadFrom(File.Exists(text5) ? text5 : text6, out assemblyLoadResult, true);
							if (assemblyLoadResult != AssemblyLoader.AssemblyLoadResult.Success)
							{
								mblist.Add(new ValueTuple<string, AssemblyLoader.AssemblyLoadResult>(text4, assemblyLoadResult));
							}
						}
						string text7 = (File.Exists(text2) ? text2 : (File.Exists(text3) ? text3 : string.Empty));
						AssemblyLoader.AssemblyLoadResult assemblyLoadResult2 = AssemblyLoader.AssemblyLoadResult.Success;
						if (!string.IsNullOrEmpty(text7))
						{
							Assembly assembly = AssemblyLoader.LoadFrom(text7, out assemblyLoadResult2, true);
							if (assemblyLoadResult2 != AssemblyLoader.AssemblyLoadResult.CriticalError)
							{
								assemblyLoadResult2 = this.AddSubModule(subModuleInfo, assembly);
								if (assemblyLoadResult2 == AssemblyLoader.AssemblyLoadResult.Success && loadNewModules)
								{
									list.Add(subModuleInfo);
								}
							}
							if (assemblyLoadResult2 != AssemblyLoader.AssemblyLoadResult.Success)
							{
								this.HandleSubmoduleLoadError(moduleInfo2, subModuleInfo, assemblyLoadResult2, mblist);
							}
							else if (mblist.Count > 0)
							{
								this.HandleSubmoduleLoadError(moduleInfo2, null, AssemblyLoader.AssemblyLoadResult.LoadedWithErrors, mblist);
							}
						}
						else
						{
							string text8 = "Cannot find: " + text2;
							string text9 = "Error";
							Debug.ShowMessageBox(text8, text9, 4U);
						}
					}
				}
			}
			if (loadNewModules)
			{
				foreach (SubModuleInfo subModuleInfo2 in list)
				{
					MBSubModuleBase mbsubModuleBase = null;
					if (this._subModuleBases.TryGetValue(subModuleInfo2, out mbsubModuleBase))
					{
						mbsubModuleBase.OnSubModuleLoad();
					}
				}
				this.OnNewModuleLoaded();
				return;
			}
			this.InitializeSubModuleBases();
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000A24DC File Offset: 0x000A06DC
		private void HandleSubmoduleLoadError(ModuleInfo module, SubModuleInfo subModule, AssemblyLoader.AssemblyLoadResult result, MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>> assemblyLoadResults)
		{
			Debug.Print(module.Id + " could not be loaded correctly.", 0, Debug.DebugColor.White, 17592186044416UL);
			string text = "Error while loading " + module.Name;
			string assemblyLoadResultsMessage = this.GetAssemblyLoadResultsMessage(module, subModule, result, assemblyLoadResults);
			if (result == AssemblyLoader.AssemblyLoadResult.CriticalError)
			{
				Debug.Print(assemblyLoadResultsMessage, 0, Debug.DebugColor.White, 17592186044416UL);
				if (!module.IsOfficial)
				{
					Debug.ShowMessageBox(assemblyLoadResultsMessage, text, 256U);
				}
				throw new Exception();
			}
			if (result == AssemblyLoader.AssemblyLoadResult.LoadedWithErrors)
			{
				Debug.Print(assemblyLoadResultsMessage, 0, Debug.DebugColor.White, 17592186044416UL);
				if (!module.IsOfficial)
				{
					Debug.ShowMessageBox(assemblyLoadResultsMessage, text, 256U);
				}
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000A2584 File Offset: 0x000A0784
		private string GetAssemblyLoadResultsMessage(ModuleInfo module, SubModuleInfo subModule, AssemblyLoader.AssemblyLoadResult result, MBList<ValueTuple<string, AssemblyLoader.AssemblyLoadResult>> assemblyLoadResults)
		{
			string text = ((subModule != null) ? string.Concat(new string[] { "\"", module.Name, ".", subModule.Name, "\" submodule" }) : ("\"" + module.Name + "\" module"));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(text + " could not be loaded correctly due to a dependency conflict. This may cause the game to experience stability issues.");
			return stringBuilder.ToString();
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000A2600 File Offset: 0x000A0800
		public Type GetSubModuleType(string name)
		{
			foreach (KeyValuePair<SubModuleInfo, MBSubModuleBase> keyValuePair in this._subModuleBases)
			{
				if (keyValuePair.Key.SubModuleClassTypeName == name)
				{
					return keyValuePair.Value.GetType();
				}
			}
			return null;
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000A2674 File Offset: 0x000A0874
		public bool CheckIfSubmoduleCanBeLoadable(SubModuleInfo subModuleInfo)
		{
			if (subModuleInfo.Tags.Count > 0)
			{
				foreach (Tuple<SubModuleInfo.SubModuleTags, string> tuple in subModuleInfo.Tags)
				{
					if (!this.GetSubModuleValiditiy(tuple.Item1, tuple.Item2))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000A26EC File Offset: 0x000A08EC
		private bool GetSubModuleValiditiy(SubModuleInfo.SubModuleTags tag, string value)
		{
			switch (tag)
			{
			case SubModuleInfo.SubModuleTags.RejectedPlatform:
			{
				Platform platform;
				if (Enum.TryParse<Platform>(value, out platform))
				{
					return ApplicationPlatform.CurrentPlatform != platform;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.ExclusivePlatform:
			{
				Platform platform;
				if (Enum.TryParse<Platform>(value, out platform))
				{
					return ApplicationPlatform.CurrentPlatform == platform;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.DedicatedServerType:
			{
				string text = value.ToLower();
				if (text == "none")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.None;
				}
				if (text == "both" || text == "all")
				{
					return this.StartupInfo.DedicatedServerType != DedicatedServerType.None;
				}
				if (text == "custom")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.Custom;
				}
				if (text == "matchmaker")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.Matchmaker;
				}
				if (text == "community")
				{
					return this.StartupInfo.DedicatedServerType == DedicatedServerType.Community;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.IsNoRenderModeElement:
				return value.Equals("false");
			case SubModuleInfo.SubModuleTags.DependantRuntimeLibrary:
			{
				Runtime runtime;
				if (Enum.TryParse<Runtime>(value, out runtime))
				{
					return ApplicationPlatform.CurrentRuntimeLibrary == runtime;
				}
				break;
			}
			case SubModuleInfo.SubModuleTags.PlayerHostedDedicatedServer:
			{
				string text2 = value.ToLower();
				if (this.StartupInfo.PlayerHostedDedicatedServer)
				{
					return text2.Equals("true");
				}
				return text2.Equals("false");
			}
			}
			return true;
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000A2844 File Offset: 0x000A0A44
		[MBCallback(null, false)]
		internal static void MBThrowException()
		{
			Debug.FailedAssert("MBThrowException", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Module.cs", "MBThrowException", 1614);
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000A285F File Offset: 0x000A0A5F
		[MBCallback(null, false)]
		internal void OnEnterEditMode(bool isFirstTime)
		{
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000A2863 File Offset: 0x000A0A63
		[MBCallback(null, false)]
		internal static Module GetInstance()
		{
			return Module.CurrentModule;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000A286A File Offset: 0x000A0A6A
		[MBCallback(null, false)]
		internal static string GetGameStatus()
		{
			if (TestCommonBase.BaseInstance != null)
			{
				return TestCommonBase.BaseInstance.GetGameStatus();
			}
			return "";
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000A2884 File Offset: 0x000A0A84
		private void FinalizeModule()
		{
			if (Game.Current != null)
			{
				Game.Current.OnFinalize();
			}
			if (TestCommonBase.BaseInstance != null)
			{
				TestCommonBase.BaseInstance.OnFinalize();
			}
			this._testContext.FinalizeContext();
			MBInformationManager.Clear();
			InformationManager.Clear();
			ScreenManager.OnFinalize();
			BannerlordConfig.Save();
			this.FinalizeSubModulesBases();
			IPlatformServices instance = PlatformServices.Instance;
			if (instance != null)
			{
				instance.Terminate();
			}
			Common.MemoryCleanupGC(false);
			GC.WaitForPendingFinalizers();
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000A28F4 File Offset: 0x000A0AF4
		internal static void FinalizeCurrentModule()
		{
			Module.CurrentModule.FinalizeModule();
			Module.CurrentModule = null;
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000A2906 File Offset: 0x000A0B06
		[MBCallback(null, false)]
		internal void SetLoadingFinished()
		{
			this.LoadingFinished = true;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000A290F File Offset: 0x000A0B0F
		[MBCallback(null, false)]
		internal void OnCloseSceneEditorPresentation()
		{
			GameStateManager.Current.PopState(0);
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000A291C File Offset: 0x000A0B1C
		[MBCallback(null, false)]
		internal void OnSceneEditorModeOver()
		{
			GameStateManager.Current.PopState(0);
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000A292C File Offset: 0x000A0B2C
		private void OnConfigChanged()
		{
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnConfigChanged();
			}
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000A2984 File Offset: 0x000A0B84
		private void OnConstrainedStateChange(bool isConstrained)
		{
			if (!isConstrained)
			{
				PlatformServices.Instance.OnFocusGained();
			}
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000A2993 File Offset: 0x000A0B93
		private void OnFocusGained()
		{
			PlatformServices.Instance.OnFocusGained();
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000A299F File Offset: 0x000A0B9F
		private void OnTextEnteredFromPlatform(string text)
		{
			ScreenManager.OnOnscreenKeyboardDone(text);
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000A29A7 File Offset: 0x000A0BA7
		private void OnTextCanceledFromPlatform()
		{
			ScreenManager.OnOnscreenKeyboardCanceled();
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000A29AE File Offset: 0x000A0BAE
		[MBCallback(null, false)]
		internal void OnSkinsXMLHasChanged()
		{
			if (this.SkinsXMLHasChanged != null)
			{
				this.SkinsXMLHasChanged();
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06002A61 RID: 10849 RVA: 0x000A29C4 File Offset: 0x000A0BC4
		// (remove) Token: 0x06002A62 RID: 10850 RVA: 0x000A29FC File Offset: 0x000A0BFC
		public event Action SkinsXMLHasChanged;

		// Token: 0x06002A63 RID: 10851 RVA: 0x000A2A31 File Offset: 0x000A0C31
		[MBCallback(null, false)]
		internal void OnImguiProfilerTick()
		{
			if (this.ImguiProfilerTick != null)
			{
				this.ImguiProfilerTick();
			}
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000A2A48 File Offset: 0x000A0C48
		[MBCallback(null, false)]
		internal static string CreateProcessedSkinsXMLForNative(out string baseSkinsXmlPath)
		{
			List<string> list;
			XmlNode mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_skins", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			baseSkinsXmlPath = list[0];
			return stringWriter.ToString();
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000A2A84 File Offset: 0x000A0C84
		[MBCallback(null, false)]
		internal static string CreateProcessedItemHolstersXMLForNative(out string baseItemHolstersPath)
		{
			List<string> list;
			XmlNode mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_item_holsters", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			baseItemHolstersPath = list[0];
			return stringWriter.ToString();
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000A2AC0 File Offset: 0x000A0CC0
		[MBCallback(null, true)]
		internal static string CreateProcessedActionSetsXMLForNative()
		{
			List<string> list;
			XmlDocument xmlDocument = MBObjectManager.GetMergedXmlForNative("soln_action_sets", out list);
			Dictionary<string, XElement> dictionary = new Dictionary<string, XElement>();
			XDocument xdocument = MBObjectManager.ToXDocument(xmlDocument);
			IEnumerable<XElement> enumerable = xdocument.Descendants("action_set");
			for (int i = 0; i < enumerable.Count<XElement>(); i++)
			{
				XElement xelement = enumerable.ElementAt<XElement>(i);
				string text = xelement.FirstAttribute.ToString();
				if (dictionary.ContainsKey(text))
				{
					dictionary[text].Add(xelement.Descendants());
					xelement.Remove();
					i--;
				}
				else
				{
					dictionary.Add(text, xelement);
				}
			}
			xmlDocument = MBObjectManager.ToXmlDocument(xdocument);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlDocument.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000A2B80 File Offset: 0x000A0D80
		[MBCallback(null, true)]
		internal static string CreateProcessedActionTypesXMLForNative()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_action_types", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000A2BB4 File Offset: 0x000A0DB4
		[MBCallback(null, true)]
		internal static string CreateProcessedAnimationsXMLForNative(out string animationsXmlPaths)
		{
			List<string> list;
			XmlNode mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_animations", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			animationsXmlPaths = "";
			for (int i = 0; i < list.Count; i++)
			{
				animationsXmlPaths += list[i];
				if (i != list.Count - 1)
				{
					animationsXmlPaths += "\n";
				}
			}
			return stringWriter.ToString();
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000A2C28 File Offset: 0x000A0E28
		[MBCallback(null, true)]
		internal static string CreateProcessedVoiceDefinitionsXMLForNative()
		{
			List<string> list;
			XmlDocument xmlDocument = MBObjectManager.GetMergedXmlForNative("soln_voice_definitions", out list);
			XDocument xdocument = MBObjectManager.ToXDocument(xmlDocument);
			XElement xelement = xdocument.Descendants("voice_type_declarations").First<XElement>();
			for (int i = 1; i < xdocument.Descendants("voice_type_declarations").Count<XElement>(); i++)
			{
				xelement.Add(xdocument.Descendants("voice_type_declarations").ElementAt<XElement>(i).Descendants());
				xdocument.Descendants("voice_type_declarations").ElementAt<XElement>(i).Remove();
				i--;
			}
			for (int j = 0; j < xdocument.Descendants("voice_definition").Count<XElement>(); j++)
			{
				for (int k = j + 1; k < xdocument.Descendants("voice_definition").Count<XElement>(); k++)
				{
					if (xdocument.Descendants("voice_definition").ElementAt<XElement>(j).FirstAttribute.ToString() == xdocument.Descendants("voice_definition").ElementAt<XElement>(k).FirstAttribute.ToString())
					{
						xdocument.Descendants("voice_definition").ElementAt<XElement>(j).Add(xdocument.Descendants("voice_definition").ElementAt<XElement>(k).Descendants());
						xdocument.Descendants("voice_definition").ElementAt<XElement>(k).Remove();
						k--;
					}
				}
			}
			xmlDocument = MBObjectManager.ToXmlDocument(xdocument);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlDocument.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000A2DE4 File Offset: 0x000A0FE4
		[MBCallback(null, true)]
		internal static string CreateProcessedSoundEventDataXMLForNative()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_sound_event_data", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000A2E18 File Offset: 0x000A1018
		[MBCallback(null, true)]
		internal static string CreateProcessedSoundParamsXMLForNative()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_sound_parameter_data", out list);
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			mergedXmlForNative.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000A2E4C File Offset: 0x000A104C
		[MBCallback(null, false)]
		internal static string CreateProcessedModuleDataXMLForNative(string xmlType)
		{
			List<string> list;
			XmlDocument xmlDocument = MBObjectManager.GetMergedXmlForNative("soln_" + xmlType, out list);
			if (xmlType == "full_movement_sets")
			{
				XDocument xdocument = MBObjectManager.ToXDocument(xmlDocument);
				for (int i = 0; i < xdocument.Descendants("full_movement_set").Count<XElement>(); i++)
				{
					for (int j = i + 1; j < xdocument.Descendants("full_movement_set").Count<XElement>(); j++)
					{
						if (xdocument.Descendants("full_movement_set").ElementAt<XElement>(i).FirstAttribute.ToString() == xdocument.Descendants("full_movement_set").ElementAt<XElement>(j).FirstAttribute.ToString())
						{
							xdocument.Descendants("full_movement_set").ElementAt<XElement>(i).Add(xdocument.Descendants("full_movement_set").ElementAt<XElement>(j).Descendants());
							xdocument.Descendants("full_movement_set").ElementAt<XElement>(j).Remove();
							j--;
						}
					}
				}
				xmlDocument = MBObjectManager.ToXmlDocument(xdocument);
			}
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlDocument.WriteTo(xmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06002A6D RID: 10861 RVA: 0x000A2F9C File Offset: 0x000A119C
		// (remove) Token: 0x06002A6E RID: 10862 RVA: 0x000A2FD4 File Offset: 0x000A11D4
		public event Action ImguiProfilerTick;

		// Token: 0x06002A6F RID: 10863 RVA: 0x000A3009 File Offset: 0x000A1209
		public void ClearStateOptions()
		{
			this._initialStateOptions.Clear();
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000A3016 File Offset: 0x000A1216
		public void AddInitialStateOption(InitialStateOption initialStateOption)
		{
			this._initialStateOptions.Add(initialStateOption);
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x000A3024 File Offset: 0x000A1224
		public void OverrideInitialStateOption(string id, InitialStateOption newInitialStateOption)
		{
			for (int i = 0; i < this._initialStateOptions.Count; i++)
			{
				if (this._initialStateOptions[i].Id == id)
				{
					this._initialStateOptions[i] = newInitialStateOption;
					return;
				}
			}
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000A306E File Offset: 0x000A126E
		public IEnumerable<InitialStateOption> GetInitialStateOptions()
		{
			return this._initialStateOptions.OrderBy<InitialStateOption, int>((InitialStateOption s) => s.OrderIndex);
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000A309C File Offset: 0x000A129C
		public InitialStateOption GetInitialStateOptionWithId(string id)
		{
			foreach (InitialStateOption initialStateOption in this._initialStateOptions)
			{
				if (initialStateOption.Id == id)
				{
					return initialStateOption;
				}
			}
			return null;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000A3100 File Offset: 0x000A1300
		public void ExecuteInitialStateOptionWithId(string id)
		{
			InitialStateOption initialStateOptionWithId = this.GetInitialStateOptionWithId(id);
			if (initialStateOptionWithId != null)
			{
				initialStateOptionWithId.DoAction();
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000A311E File Offset: 0x000A131E
		public void SetCanLoadModules(bool canLoadModules)
		{
			EngineApplicationInterface.IUtil.SetCanLoadModules(canLoadModules);
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000A312B File Offset: 0x000A132B
		void IGameStateManagerOwner.OnStateStackEmpty()
		{
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000A312D File Offset: 0x000A132D
		void IGameStateManagerOwner.OnStateChanged(GameState oldState)
		{
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000A312F File Offset: 0x000A132F
		public void SetEditorMissionTester(IEditorMissionTester editorMissionTester)
		{
			this._editorMissionTester = editorMissionTester;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000A3138 File Offset: 0x000A1338
		[MBCallback(null, false)]
		internal void StartMissionForEditor(string missionName, string sceneName, string levels)
		{
			if (this._editorMissionTester != null)
			{
				this._editorMissionTester.StartMissionForEditor(missionName, sceneName, levels);
			}
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000A3150 File Offset: 0x000A1350
		[MBCallback(null, false)]
		internal void StartMissionForReplayEditor(string missionName, string sceneName, string levels, string fileName, bool record, float startTime, float endTime)
		{
			if (this._editorMissionTester != null)
			{
				this._editorMissionTester.StartMissionForReplayEditor(missionName, sceneName, levels, fileName, record, startTime, endTime);
			}
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000A3170 File Offset: 0x000A1370
		public void StartMissionForEditorAux(string missionName, string sceneName, string levels, bool forReplay, string replayFileName, bool isRecord)
		{
			GameStateManager.Current = Game.Current.GameStateManager;
			this.ReturnToEditorState = true;
			MissionInfo missionInfo = this._missionInfos.Find((MissionInfo mi) => mi.Name == missionName);
			if (missionInfo == null)
			{
				missionInfo = this._missionInfos.Find((MissionInfo mi) => mi.Name.Contains(missionName));
			}
			if (forReplay)
			{
				missionInfo.Creator.Invoke(null, new object[] { replayFileName, isRecord });
				return;
			}
			missionInfo.Creator.Invoke(null, new object[] { sceneName, levels });
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000A3215 File Offset: 0x000A1415
		private void FillMultiplayerGameTypes()
		{
			this._multiplayerGameModesWithNames = new Dictionary<string, MultiplayerGameMode>();
			this._multiplayerGameTypes = new MBList<MultiplayerGameTypeInfo>();
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000A3230 File Offset: 0x000A1430
		public MultiplayerGameMode GetMultiplayerGameMode(string gameType)
		{
			MultiplayerGameMode multiplayerGameMode;
			if (this._multiplayerGameModesWithNames.TryGetValue(gameType, out multiplayerGameMode))
			{
				return multiplayerGameMode;
			}
			return null;
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000A3250 File Offset: 0x000A1450
		public void AddMultiplayerGameMode(MultiplayerGameMode multiplayerGameMode)
		{
			this._multiplayerGameModesWithNames.Add(multiplayerGameMode.Name, multiplayerGameMode);
			this._multiplayerGameTypes.Add(new MultiplayerGameTypeInfo("Native", multiplayerGameMode.Name));
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000A327F File Offset: 0x000A147F
		public MBReadOnlyList<MultiplayerGameTypeInfo> GetMultiplayerGameTypes()
		{
			return this._multiplayerGameTypes;
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000A3288 File Offset: 0x000A1488
		public bool StartMultiplayerGame(string multiplayerGameType, string scene)
		{
			MultiplayerGameMode multiplayerGameMode;
			if (this._multiplayerGameModesWithNames.TryGetValue(multiplayerGameType, out multiplayerGameMode))
			{
				multiplayerGameMode.StartMultiplayerGame(scene);
				return true;
			}
			return false;
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000A32B0 File Offset: 0x000A14B0
		public async void ShutDownWithDelay(string reason, int seconds)
		{
			if (!this._isShuttingDown)
			{
				this._isShuttingDown = true;
				for (int i = 0; i < seconds; i++)
				{
					int num = seconds - i;
					string text = string.Concat(new object[] { "Shutting down in ", num, " seconds with reason '", reason, "'" });
					Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
					Console.WriteLine(text);
					await Task.Delay(1000);
				}
				if (Game.Current != null)
				{
					Debug.Print("Active game exist during ShutDownWithDelay", 0, Debug.DebugColor.White, 17592186044416UL);
					MBGameManager.EndGame();
				}
				Utilities.QuitGame();
			}
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000A32FC File Offset: 0x000A14FC
		public void DeactiveModule(string moduleId)
		{
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(moduleId);
			if (moduleInfo != null && moduleInfo.IsActive && !moduleInfo.IsNative)
			{
				Debug.Print("Deactivating Module: " + moduleId, 0, Debug.DebugColor.Green, 17592186044416UL);
				ModuleHelper.OnModuleDeactivated(moduleId);
				foreach (SubModuleInfo subModuleInfo in moduleInfo.SubModules)
				{
					MBSubModuleBase mbsubModuleBase;
					if (this._subModuleBases.TryGetValue(subModuleInfo, out mbsubModuleBase))
					{
						mbsubModuleBase.OnSubModuleDeactivated();
					}
				}
			}
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000A339C File Offset: 0x000A159C
		public void ActivateModule(string moduleId)
		{
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(moduleId);
			if (moduleInfo != null && !moduleInfo.IsActive)
			{
				Debug.Print("Activating Module: " + moduleId, 0, Debug.DebugColor.Green, 17592186044416UL);
				ModuleHelper.OnModuleActivated(moduleId);
				foreach (SubModuleInfo subModuleInfo in moduleInfo.SubModules)
				{
					MBSubModuleBase mbsubModuleBase;
					if (this._subModuleBases.TryGetValue(subModuleInfo, out mbsubModuleBase))
					{
						mbsubModuleBase.OnSubModuleActivated();
					}
				}
			}
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000A3434 File Offset: 0x000A1634
		internal void OnBeforeGameStart(MBGameManager mbGameManager)
		{
			List<string> list = new List<string>();
			foreach (MBSubModuleBase mbsubModuleBase in this._subModuleBases.Values)
			{
				mbsubModuleBase.OnBeforeGameStart(mbGameManager, list);
			}
			foreach (string text in list)
			{
				ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo(text);
				if (moduleInfo != null && moduleInfo.IsActive)
				{
					this.DeactiveModule(text);
				}
			}
			MBList<ModuleInfo> mblist = new MBList<ModuleInfo>();
			foreach (ModuleInfo moduleInfo2 in ModuleHelper.GetActiveModules())
			{
				foreach (DependedModule dependedModule in moduleInfo2.DependedModules)
				{
					foreach (string text2 in list)
					{
						ModuleInfo moduleInfo3 = ModuleHelper.GetModuleInfo(text2);
						if (moduleInfo3 != null && moduleInfo2 != moduleInfo3 && dependedModule.ModuleId == text2)
						{
							mblist.Add(moduleInfo3);
						}
					}
				}
			}
			if (mblist.Any<ModuleInfo>((ModuleInfo x) => !x.IsOfficial))
			{
				string.Join("\n", from x in mblist
					where !x.IsOfficial
					select x.Name);
			}
			InformationManager.ClearAllMessages();
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000A3650 File Offset: 0x000A1850
		internal void OnGameEnd()
		{
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules((ModuleInfo x) => !x.IsActive))
			{
				this.ActivateModule(moduleInfo.Id);
			}
		}

		// Token: 0x04001022 RID: 4130
		private bool _enableCoreContentOnReturnToRoot;

		// Token: 0x04001027 RID: 4135
		private List<MissionInfo> _missionInfos;

		// Token: 0x04001028 RID: 4136
		private TestContext _testContext;

		// Token: 0x0400102B RID: 4139
		private SingleThreadedSynchronizationContext _synchronizationContext;

		// Token: 0x0400102C RID: 4140
		private readonly Dictionary<SubModuleInfo, MBSubModuleBase> _subModuleBases;

		// Token: 0x0400102D RID: 4141
		private bool _splashScreenPlayed;

		// Token: 0x04001030 RID: 4144
		private List<InitialStateOption> _initialStateOptions;

		// Token: 0x04001031 RID: 4145
		private IEditorMissionTester _editorMissionTester;

		// Token: 0x04001032 RID: 4146
		private Dictionary<string, MultiplayerGameMode> _multiplayerGameModesWithNames;

		// Token: 0x04001033 RID: 4147
		private MBList<MultiplayerGameTypeInfo> _multiplayerGameTypes = new MBList<MultiplayerGameTypeInfo>();

		// Token: 0x04001034 RID: 4148
		private bool _isShuttingDown;

		// Token: 0x020005BC RID: 1468
		public enum XmlInformationType
		{
			// Token: 0x04001F0F RID: 7951
			Parameters,
			// Token: 0x04001F10 RID: 7952
			MbObjectType
		}

		// Token: 0x020005BD RID: 1469
		private enum StartupType
		{
			// Token: 0x04001F12 RID: 7954
			None,
			// Token: 0x04001F13 RID: 7955
			TestMode,
			// Token: 0x04001F14 RID: 7956
			GameServer,
			// Token: 0x04001F15 RID: 7957
			Singleplayer,
			// Token: 0x04001F16 RID: 7958
			Multiplayer,
			// Token: 0x04001F17 RID: 7959
			Count
		}
	}
}
