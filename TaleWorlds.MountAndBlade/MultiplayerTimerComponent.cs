using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C2 RID: 706
	public class MultiplayerTimerComponent : MissionNetwork
	{
		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x00099E28 File Offset: 0x00098028
		// (set) Token: 0x0600288E RID: 10382 RVA: 0x00099E30 File Offset: 0x00098030
		public bool IsTimerRunning { get; private set; }

		// Token: 0x0600288F RID: 10383 RVA: 0x00099E39 File Offset: 0x00098039
		public void StartTimerAsServer(float duration)
		{
			this._missionTimer = new MissionTimer(duration);
			this.IsTimerRunning = true;
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x00099E4E File Offset: 0x0009804E
		public void StartTimerAsClient(float startTime, float duration)
		{
			this._missionTimer = MissionTimer.CreateSynchedTimerClient(startTime, duration);
			this.IsTimerRunning = true;
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x00099E64 File Offset: 0x00098064
		public float GetRemainingTime(bool isSynched)
		{
			if (!this.IsTimerRunning)
			{
				return 0f;
			}
			float remainingTimeInSeconds = this._missionTimer.GetRemainingTimeInSeconds(isSynched);
			if (isSynched)
			{
				return MathF.Min(remainingTimeInSeconds, this._missionTimer.GetTimerDuration());
			}
			return remainingTimeInSeconds;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x00099EA2 File Offset: 0x000980A2
		public bool CheckIfTimerPassed()
		{
			return this.IsTimerRunning && this._missionTimer.Check(false);
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x00099EBA File Offset: 0x000980BA
		public MissionTime GetCurrentTimerStartTime()
		{
			return this._missionTimer.GetStartTime();
		}

		// Token: 0x04000F91 RID: 3985
		private MissionTimer _missionTimer;
	}
}
