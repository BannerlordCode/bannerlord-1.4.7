using System;
using System.Collections.Generic;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.ServiceDiscovery.Client;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000060 RID: 96
	public static class MultiplayerMain
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000D29A File Offset: 0x0000B49A
		public static LobbyClient GameClient
		{
			get
			{
				return NetworkMain.GameClient;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000D2A4 File Offset: 0x0000B4A4
		static MultiplayerMain()
		{
			ServiceAddressManager.Initalize();
			MultiplayerMain._lobbyClientApplicationConfiguration = new ClientApplicationConfiguration();
			MultiplayerMain._lobbyClientApplicationConfiguration.FillFrom("LobbyClient");
			ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo("Multiplayer");
			if (!GameNetwork.IsDedicatedServer && moduleInfo != null)
			{
				MultiplayerMain._diamondClientApplication = new DiamondClientApplication(moduleInfo.Version);
				MultiplayerMain._diamondClientApplication.Initialize(MultiplayerMain._lobbyClientApplicationConfiguration);
				NetworkMain.SetPeers(MultiplayerMain._diamondClientApplication.GetClient<LobbyClient>("LobbyClient"), new CommunityClient(), null);
				MachineId.Initialize();
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D328 File Offset: 0x0000B528
		public static void Initialize(IGameNetworkHandler gameNetworkHandler)
		{
			Debug.Print("Initializing NetworkMain", 0, Debug.DebugColor.White, 17592186044416UL);
			MBCommon.CurrentGameType = MBCommon.GameType.Single;
			GameNetwork.InitializeCompressionInfos();
			if (!MultiplayerMain.IsInitialized)
			{
				MultiplayerMain.IsInitialized = true;
				GameNetwork.Initialize(gameNetworkHandler);
			}
			PermaMuteList.SetPermanentMuteAvailableCallback(() => PlatformServices.Instance.IsPermanentMuteAvailable);
			Debug.Print("NetworkMain Initialized", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D3A3 File Offset: 0x0000B5A3
		public static void InitializeAsDedicatedServer(IGameNetworkHandler gameNetworkHandler)
		{
			MBCommon.CurrentGameType = MBCommon.GameType.MultiServer;
			GameNetwork.InitializeCompressionInfos();
			if (!MultiplayerMain.IsInitialized)
			{
				MultiplayerMain.IsInitialized = true;
				GameNetwork.Initialize(gameNetworkHandler);
				GameStartupInfo startupInfo = Module.CurrentModule.StartupInfo;
				GameNetwork.SetServerBandwidthLimitInMbps(startupInfo.ServerBandwidthLimitInMbps);
				GameNetwork.SetServerTickRate((double)startupInfo.ServerTickRate);
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D3E3 File Offset: 0x0000B5E3
		internal static void Tick(float dt)
		{
			if (MultiplayerMain.IsInitialized)
			{
				if (MultiplayerMain.GameClient != null)
				{
					MultiplayerMain.GameClient.Update();
				}
				if (MultiplayerMain._diamondClientApplication != null)
				{
					MultiplayerMain._diamondClientApplication.Update();
				}
				GameNetwork.Tick(dt);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000D414 File Offset: 0x0000B614
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000D41B File Offset: 0x0000B61B
		public static bool IsInitialized { get; private set; } = false;

		// Token: 0x060002DB RID: 731 RVA: 0x0000D423 File Offset: 0x0000B623
		public static MultiplayerGameType[] GetAvailableRankedGameModes()
		{
			return new MultiplayerGameType[]
			{
				MultiplayerGameType.Captain,
				MultiplayerGameType.Skirmish
			};
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D433 File Offset: 0x0000B633
		public static MultiplayerGameType[] GetAvailableCustomGameModes()
		{
			return new MultiplayerGameType[]
			{
				MultiplayerGameType.TeamDeathmatch,
				MultiplayerGameType.Siege
			};
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000D43F File Offset: 0x0000B63F
		public static MultiplayerGameType[] GetAvailableQuickPlayGameModes()
		{
			return new MultiplayerGameType[]
			{
				MultiplayerGameType.Captain,
				MultiplayerGameType.Skirmish
			};
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000D44F File Offset: 0x0000B64F
		public static string[] GetAvailableMatchmakerRegions()
		{
			return new string[] { "USE", "USW", "EU", "EA" };
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000D477 File Offset: 0x0000B677
		public static string GetUserDefaultRegion()
		{
			return "None";
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000D47E File Offset: 0x0000B67E
		public static string GetUserCurrentRegion()
		{
			LobbyClient gameClient = MultiplayerMain.GameClient;
			if (gameClient != null && gameClient.LoggedIn && MultiplayerMain.GameClient.PlayerData != null)
			{
				return MultiplayerMain.GameClient.PlayerData.LastRegion;
			}
			return MultiplayerMain.GetUserDefaultRegion();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		public static string[] GetUserSelectedGameTypes()
		{
			LobbyClient gameClient = MultiplayerMain.GameClient;
			if (gameClient != null && gameClient.LoggedIn)
			{
				return MultiplayerMain.GameClient.PlayerData.LastGameTypes;
			}
			return new string[0];
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000D4DF File Offset: 0x0000B6DF
		[CommandLineFunctionality.CommandLineArgumentFunction("gettoken", "customserver")]
		public static string GetDedicatedCustomServerAuthToken(List<string> strings)
		{
			if (!(Common.PlatformFileHelper is PlatformFileHelperPC))
			{
				return "Platform not supported.";
			}
			if (MultiplayerMain.GameClient == null)
			{
				return "Not logged into lobby.";
			}
			MultiplayerMain.GetDedicatedCustomServerAuthToken();
			return string.Empty;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000D50C File Offset: 0x0000B70C
		private static async void GetDedicatedCustomServerAuthToken()
		{
			string text = await MultiplayerMain.GameClient.GetDedicatedCustomServerAuthToken();
			if (text == null)
			{
				MBDebug.EchoCommandWindow("Could not get token.");
			}
			else
			{
				PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Tokens");
				PlatformFilePath platformFilePath = new PlatformFilePath(platformDirectoryPath, "DedicatedCustomServerAuthToken.txt");
				FileHelper.SaveFileString(platformFilePath, text);
				MBDebug.EchoCommandWindow(text + " (Saved to " + platformFilePath.FileFullPath + ")");
			}
		}

		// Token: 0x040000E8 RID: 232
		private static ClientApplicationConfiguration _lobbyClientApplicationConfiguration;

		// Token: 0x040000E9 RID: 233
		private static DiamondClientApplication _diamondClientApplication;
	}
}
