using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A1 RID: 417
	public static class MBAPI
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x00052C84 File Offset: 0x00050E84
		private static T GetObject<T>() where T : class
		{
			object obj;
			if (MBAPI._objects.TryGetValue(typeof(T).FullName, out obj))
			{
				return obj as T;
			}
			return default(T);
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00052CC4 File Offset: 0x00050EC4
		internal static void SetObjects(Dictionary<string, object> objects)
		{
			MBAPI._objects = objects;
			MBAPI.IMBTestRun = MBAPI.GetObject<IMBTestRun>();
			MBAPI.IMBActionSet = MBAPI.GetObject<IMBActionSet>();
			MBAPI.IMBAgent = MBAPI.GetObject<IMBAgent>();
			MBAPI.IMBAnimation = MBAPI.GetObject<IMBAnimation>();
			MBAPI.IMBDelegate = MBAPI.GetObject<IMBDelegate>();
			MBAPI.IMBItem = MBAPI.GetObject<IMBItem>();
			MBAPI.IMBEditor = MBAPI.GetObject<IMBEditor>();
			MBAPI.IMBMission = MBAPI.GetObject<IMBMission>();
			MBAPI.IMBMultiplayerData = MBAPI.GetObject<IMBMultiplayerData>();
			MBAPI.IMouseManager = MBAPI.GetObject<IMouseManager>();
			MBAPI.IMBNetwork = MBAPI.GetObject<IMBNetwork>();
			MBAPI.IMBPeer = MBAPI.GetObject<IMBPeer>();
			MBAPI.IMBSkeletonExtensions = MBAPI.GetObject<IMBSkeletonExtensions>();
			MBAPI.IMBGameEntityExtensions = MBAPI.GetObject<IMBGameEntityExtensions>();
			MBAPI.IMBScreen = MBAPI.GetObject<IMBScreen>();
			MBAPI.IMBSoundEvent = MBAPI.GetObject<IMBSoundEvent>();
			MBAPI.IMBVoiceManager = MBAPI.GetObject<IMBVoiceManager>();
			MBAPI.IMBTeam = MBAPI.GetObject<IMBTeam>();
			MBAPI.IMBWorld = MBAPI.GetObject<IMBWorld>();
			MBAPI.IInput = MBAPI.GetObject<IInput>();
			MBAPI.IMBMessageManager = MBAPI.GetObject<IMBMessageManager>();
			MBAPI.IMBWindowManager = MBAPI.GetObject<IMBWindowManager>();
			MBAPI.IMBDebugExtensions = MBAPI.GetObject<IMBDebugExtensions>();
			MBAPI.IMBGame = MBAPI.GetObject<IMBGame>();
			MBAPI.IMBFaceGen = MBAPI.GetObject<IMBFaceGen>();
			MBAPI.IMBMapScene = MBAPI.GetObject<IMBMapScene>();
			MBAPI.IMBBannerlordChecker = MBAPI.GetObject<IMBBannerlordChecker>();
			MBAPI.IMBAgentVisuals = MBAPI.GetObject<IMBAgentVisuals>();
			MBAPI.IMBBannerlordTableauManager = MBAPI.GetObject<IMBBannerlordTableauManager>();
			MBAPI.IMBBannerlordConfig = MBAPI.GetObject<IMBBannerlordConfig>();
		}

		// Token: 0x04000855 RID: 2133
		internal static IMBTestRun IMBTestRun;

		// Token: 0x04000856 RID: 2134
		internal static IMBActionSet IMBActionSet;

		// Token: 0x04000857 RID: 2135
		internal static IMBAgent IMBAgent;

		// Token: 0x04000858 RID: 2136
		internal static IMBAgentVisuals IMBAgentVisuals;

		// Token: 0x04000859 RID: 2137
		internal static IMBAnimation IMBAnimation;

		// Token: 0x0400085A RID: 2138
		internal static IMBDelegate IMBDelegate;

		// Token: 0x0400085B RID: 2139
		internal static IMBItem IMBItem;

		// Token: 0x0400085C RID: 2140
		internal static IMBEditor IMBEditor;

		// Token: 0x0400085D RID: 2141
		internal static IMBMission IMBMission;

		// Token: 0x0400085E RID: 2142
		internal static IMBMultiplayerData IMBMultiplayerData;

		// Token: 0x0400085F RID: 2143
		internal static IMouseManager IMouseManager;

		// Token: 0x04000860 RID: 2144
		internal static IMBNetwork IMBNetwork;

		// Token: 0x04000861 RID: 2145
		internal static IMBPeer IMBPeer;

		// Token: 0x04000862 RID: 2146
		internal static IMBSkeletonExtensions IMBSkeletonExtensions;

		// Token: 0x04000863 RID: 2147
		internal static IMBGameEntityExtensions IMBGameEntityExtensions;

		// Token: 0x04000864 RID: 2148
		internal static IMBScreen IMBScreen;

		// Token: 0x04000865 RID: 2149
		internal static IMBSoundEvent IMBSoundEvent;

		// Token: 0x04000866 RID: 2150
		internal static IMBVoiceManager IMBVoiceManager;

		// Token: 0x04000867 RID: 2151
		internal static IMBTeam IMBTeam;

		// Token: 0x04000868 RID: 2152
		internal static IMBWorld IMBWorld;

		// Token: 0x04000869 RID: 2153
		internal static IInput IInput;

		// Token: 0x0400086A RID: 2154
		internal static IMBMessageManager IMBMessageManager;

		// Token: 0x0400086B RID: 2155
		internal static IMBWindowManager IMBWindowManager;

		// Token: 0x0400086C RID: 2156
		internal static IMBDebugExtensions IMBDebugExtensions;

		// Token: 0x0400086D RID: 2157
		internal static IMBGame IMBGame;

		// Token: 0x0400086E RID: 2158
		internal static IMBFaceGen IMBFaceGen;

		// Token: 0x0400086F RID: 2159
		internal static IMBMapScene IMBMapScene;

		// Token: 0x04000870 RID: 2160
		internal static IMBBannerlordChecker IMBBannerlordChecker;

		// Token: 0x04000871 RID: 2161
		internal static IMBBannerlordTableauManager IMBBannerlordTableauManager;

		// Token: 0x04000872 RID: 2162
		internal static IMBBannerlordConfig IMBBannerlordConfig;

		// Token: 0x04000873 RID: 2163
		private static Dictionary<string, object> _objects;
	}
}
