using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FB RID: 251
	[Serializable]
	public class MultipleBattleResult
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00005CB1 File Offset: 0x00003EB1
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x00005CB9 File Offset: 0x00003EB9
		public List<BattleResult> BattleResults { get; set; }

		// Token: 0x0600050B RID: 1291 RVA: 0x00005CC2 File Offset: 0x00003EC2
		public MultipleBattleResult()
		{
			this.BattleResults = new List<BattleResult>();
			this._currentBattleIndex = -1;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00005CDC File Offset: 0x00003EDC
		public void CreateNewBattleResult(string gameType)
		{
			BattleResult battleResult = new BattleResult();
			this.BattleResults.Add(battleResult);
			this._currentBattleIndex++;
			if (this._currentBattleIndex > 0)
			{
				foreach (KeyValuePair<string, BattlePlayerEntry> keyValuePair in this.BattleResults[this._currentBattleIndex - 1].PlayerEntries)
				{
					battleResult.AddOrUpdatePlayerEntry(PlayerId.FromString(keyValuePair.Key), keyValuePair.Value.TeamNo, gameType, Guid.Empty, -1);
				}
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00005D88 File Offset: 0x00003F88
		public BattleResult GetCurrentBattleResult()
		{
			return this.BattleResults[this._currentBattleIndex];
		}

		// Token: 0x040001B0 RID: 432
		private int _currentBattleIndex;
	}
}
