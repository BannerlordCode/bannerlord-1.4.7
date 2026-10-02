using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby
{
	// Token: 0x0200016E RID: 366
	public class MultiplayerLocalDataManager
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000102A1 File Offset: 0x0000E4A1
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x000102A8 File Offset: 0x0000E4A8
		public static MultiplayerLocalDataManager Instance { get; private set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000102B0 File Offset: 0x0000E4B0
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x000102B8 File Offset: 0x0000E4B8
		public TauntSlotDataContainer TauntSlotData { get; private set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x000102C1 File Offset: 0x0000E4C1
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x000102C9 File Offset: 0x0000E4C9
		public MatchHistoryDataContainer MatchHistory { get; private set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x000102D2 File Offset: 0x0000E4D2
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x000102DA File Offset: 0x0000E4DA
		public FavoriteServerDataContainer FavoriteServers { get; private set; }

		// Token: 0x06000A25 RID: 2597 RVA: 0x000102E3 File Offset: 0x0000E4E3
		private MultiplayerLocalDataManager()
		{
			this.TauntSlotData = new TauntSlotDataContainer();
			this.MatchHistory = new MatchHistoryDataContainer();
			this.FavoriteServers = new FavoriteServerDataContainer();
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001030C File Offset: 0x0000E50C
		public static void InitializeManager()
		{
			if (MultiplayerLocalDataManager.Instance != null)
			{
				Debug.FailedAssert("Multiplayer local data manager is already initialized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "InitializeManager", 34);
				return;
			}
			MultiplayerLocalDataManager.Instance = new MultiplayerLocalDataManager();
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00010336 File Offset: 0x0000E536
		public static void FinalizeManager()
		{
			if (MultiplayerLocalDataManager.Instance == null)
			{
				Debug.FailedAssert("Multiplayer local data manager is not initialized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\Lobby\\MultiplayerLocalDataManager.cs", "FinalizeManager", 45);
				return;
			}
			MultiplayerLocalDataManager.Instance.WaitForAsyncOperations();
			MultiplayerLocalDataManager.Instance = null;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00010368 File Offset: 0x0000E568
		public async void Tick(float dt)
		{
			if (!this._isBusy)
			{
				this._isBusy = true;
				await this.TauntSlotData.Tick(dt);
				await this.MatchHistory.Tick(dt);
				await this.FavoriteServers.Tick(dt);
				this._isBusy = false;
			}
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x000103A9 File Offset: 0x0000E5A9
		private void WaitForAsyncOperations()
		{
			while (this._isBusy)
			{
			}
		}

		// Token: 0x040004FA RID: 1274
		internal const float FileUpdateIntervalInSeconds = 2f;

		// Token: 0x040004FE RID: 1278
		private bool _isBusy;
	}
}
