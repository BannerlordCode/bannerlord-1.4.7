using System;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.Engine
{
	// Token: 0x02000071 RID: 113
	public static class NativeConfig
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0000A908 File Offset: 0x00008B08
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0000A90F File Offset: 0x00008B0F
		public static bool CheatMode { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0000A917 File Offset: 0x00008B17
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x0000A91E File Offset: 0x00008B1E
		public static bool IsDevelopmentMode { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0000A926 File Offset: 0x00008B26
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x0000A92D File Offset: 0x00008B2D
		public static bool LocalizationDebugMode { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0000A935 File Offset: 0x00008B35
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x0000A93C File Offset: 0x00008B3C
		public static bool GetUIDebugMode { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0000A944 File Offset: 0x00008B44
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x0000A94B File Offset: 0x00008B4B
		public static bool DisableSound { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0000A953 File Offset: 0x00008B53
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0000A95A File Offset: 0x00008B5A
		public static bool EnableEditMode { get; private set; }

		// Token: 0x06000A60 RID: 2656 RVA: 0x0000A964 File Offset: 0x00008B64
		public static void OnConfigChanged()
		{
			NativeConfig.CheatMode = EngineApplicationInterface.IConfig.GetCheatMode();
			NativeConfig.IsDevelopmentMode = EngineApplicationInterface.IConfig.GetDevelopmentMode();
			NativeConfig.GetUIDebugMode = EngineApplicationInterface.IConfig.GetUIDebugMode();
			NativeConfig.LocalizationDebugMode = EngineApplicationInterface.IConfig.GetLocalizationDebugMode();
			NativeConfig.EnableEditMode = EngineApplicationInterface.IConfig.GetEnableEditMode();
			NativeConfig.DisableSound = EngineApplicationInterface.IConfig.GetDisableSound();
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0000A9CB File Offset: 0x00008BCB
		public static bool TableauCacheEnabled
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetTableauCacheMode();
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0000A9D7 File Offset: 0x00008BD7
		public static bool DoLocalizationCheckAtStartup
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDoLocalizationCheckAtStartup();
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0000A9E3 File Offset: 0x00008BE3
		public static bool EnableClothSimulation
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetEnableClothSimulation();
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0000A9EF File Offset: 0x00008BEF
		public static int CharacterDetail
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetCharacterDetail();
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0000A9FB File Offset: 0x00008BFB
		public static bool InvertMouse
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetInvertMouse();
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0000AA07 File Offset: 0x00008C07
		public static string LastOpenedScene
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetLastOpenedScene();
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0000AA13 File Offset: 0x00008C13
		public static int AutoSaveInMinutes
		{
			get
			{
				return EngineApplicationInterface.IConfig.AutoSaveInMinutes();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0000AA1F File Offset: 0x00008C1F
		public static bool GetUIDoNotUseGeneratedPrefabs
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetUIDoNotUseGeneratedPrefabs();
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0000AA2B File Offset: 0x00008C2B
		public static string DebugLoginUsername
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDebugLoginUserName();
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0000AA37 File Offset: 0x00008C37
		public static string DebugLogicPassword
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDebugLoginPassword();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0000AA43 File Offset: 0x00008C43
		public static bool DisableGuiMessages
		{
			get
			{
				return EngineApplicationInterface.IConfig.GetDisableGuiMessages();
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x0000AA4F File Offset: 0x00008C4F
		public static NativeOptions.ConfigQuality AutoGFXQuality
		{
			get
			{
				return (NativeOptions.ConfigQuality)EngineApplicationInterface.IConfig.GetAutoGFXQuality();
			}
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0000AA5B File Offset: 0x00008C5B
		public static void SetAutoConfigWrtHardware()
		{
			EngineApplicationInterface.IConfig.SetAutoConfigWrtHardware();
		}
	}
}
