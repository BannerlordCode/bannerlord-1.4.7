using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C2 RID: 450
	public class MBCommon
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x0005F431 File Offset: 0x0005D631
		// (set) Token: 0x06001B16 RID: 6934 RVA: 0x0005F438 File Offset: 0x0005D638
		public static MBCommon.GameType CurrentGameType
		{
			get
			{
				return MBCommon._currentGameType;
			}
			set
			{
				MBCommon._currentGameType = value;
				MBAPI.IMBWorld.SetGameType((int)value);
			}
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0005F44B File Offset: 0x0005D64B
		public static void PauseGameEngine()
		{
			MBCommon.IsPaused = true;
			MBAPI.IMBWorld.PauseGame();
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0005F45D File Offset: 0x0005D65D
		public static void UnPauseGameEngine()
		{
			MBCommon.IsPaused = false;
			MBAPI.IMBWorld.UnpauseGame();
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0005F46F File Offset: 0x0005D66F
		public static float GetApplicationTime()
		{
			return MBAPI.IMBWorld.GetGlobalTime(MBCommon.TimeType.Application);
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0005F47C File Offset: 0x0005D67C
		public static float GetTotalMissionTime()
		{
			return MBAPI.IMBWorld.GetGlobalTime(MBCommon.TimeType.Mission);
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x0005F489 File Offset: 0x0005D689
		public static bool IsDebugMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0005F48C File Offset: 0x0005D68C
		public static void FixSkeletons()
		{
			MBAPI.IMBWorld.FixSkeletons();
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x0005F498 File Offset: 0x0005D698
		// (set) Token: 0x06001B1E RID: 6942 RVA: 0x0005F49F File Offset: 0x0005D69F
		public static bool IsPaused { get; private set; }

		// Token: 0x06001B1F RID: 6943 RVA: 0x0005F4A7 File Offset: 0x0005D6A7
		public static void CheckResourceModifications()
		{
			MBAPI.IMBWorld.CheckResourceModifications();
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0005F4B4 File Offset: 0x0005D6B4
		public static int Hash(int i, object o)
		{
			return ((i * 397) ^ o.GetHashCode()).ToString().GetHashCode();
		}

		// Token: 0x040008FD RID: 2301
		private static MBCommon.GameType _currentGameType;

		// Token: 0x02000507 RID: 1287
		public enum GameType
		{
			// Token: 0x04001CBF RID: 7359
			Single,
			// Token: 0x04001CC0 RID: 7360
			MultiClient,
			// Token: 0x04001CC1 RID: 7361
			MultiServer,
			// Token: 0x04001CC2 RID: 7362
			MultiClientServer,
			// Token: 0x04001CC3 RID: 7363
			SingleReplay,
			// Token: 0x04001CC4 RID: 7364
			SingleRecord
		}

		// Token: 0x02000508 RID: 1288
		[EngineStruct("rglTimer_type", false, null)]
		public enum TimeType
		{
			// Token: 0x04001CC6 RID: 7366
			[CustomEngineStructMemberData("Real_timer")]
			Application,
			// Token: 0x04001CC7 RID: 7367
			[CustomEngineStructMemberData("Tactical_timer")]
			Mission
		}
	}
}
