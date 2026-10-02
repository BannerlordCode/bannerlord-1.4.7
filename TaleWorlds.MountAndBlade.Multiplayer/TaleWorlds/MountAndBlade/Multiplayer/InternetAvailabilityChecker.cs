using System;
using System.Threading.Tasks;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000052 RID: 82
	public static class InternetAvailabilityChecker
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000B9C5 File Offset: 0x00009BC5
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000B9CC File Offset: 0x00009BCC
		public static bool InternetConnectionAvailable
		{
			get
			{
				return InternetAvailabilityChecker._internetConnectionAvailable;
			}
			private set
			{
				if (value != InternetAvailabilityChecker._internetConnectionAvailable)
				{
					InternetAvailabilityChecker._internetConnectionAvailable = value;
					Action<bool> onInternetConnectionAvailabilityChanged = InternetAvailabilityChecker.OnInternetConnectionAvailabilityChanged;
					if (onInternetConnectionAvailabilityChanged == null)
					{
						return;
					}
					onInternetConnectionAvailabilityChanged(value);
				}
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000B9EC File Offset: 0x00009BEC
		private static async void CheckInternetConnection()
		{
			if (NetworkMain.GameClient != null)
			{
				InternetAvailabilityChecker.InternetConnectionAvailable = await NetworkMain.GameClient.CheckConnection();
			}
			InternetAvailabilityChecker._lastInternetConnectionCheck = DateTime.Now.Ticks;
			InternetAvailabilityChecker._checkingConnection = false;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000BA20 File Offset: 0x00009C20
		internal static void Tick(float dt)
		{
			long num = (InternetAvailabilityChecker.InternetConnectionAvailable ? 300000000L : 100000000L);
			if (Module.CurrentModule != null && Module.CurrentModule.StartupInfo.StartupType != GameStartupType.Singleplayer && !InternetAvailabilityChecker._checkingConnection && DateTime.Now.Ticks - InternetAvailabilityChecker._lastInternetConnectionCheck > num)
			{
				InternetAvailabilityChecker._checkingConnection = true;
				Task.Run(delegate
				{
					InternetAvailabilityChecker.CheckInternetConnection();
				});
			}
		}

		// Token: 0x040000DB RID: 219
		public static Action<bool> OnInternetConnectionAvailabilityChanged;

		// Token: 0x040000DC RID: 220
		private static bool _internetConnectionAvailable;

		// Token: 0x040000DD RID: 221
		private static long _lastInternetConnectionCheck;

		// Token: 0x040000DE RID: 222
		private static bool _checkingConnection;

		// Token: 0x040000DF RID: 223
		private const long InternetConnectionCheckIntervalShort = 100000000L;

		// Token: 0x040000E0 RID: 224
		private const long InternetConnectionCheckIntervalLong = 300000000L;
	}
}
