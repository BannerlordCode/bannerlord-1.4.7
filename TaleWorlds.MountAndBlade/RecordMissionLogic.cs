using System;
using System.Diagnostics;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000294 RID: 660
	public class RecordMissionLogic : MissionLogic
	{
		// Token: 0x0600249E RID: 9374 RVA: 0x00084B6E File Offset: 0x00082D6E
		public override void OnBehaviorInitialize()
		{
			base.Mission.Recorder.StartRecording();
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00084B80 File Offset: 0x00082D80
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._lastRecordedTime + 0.02f < base.Mission.CurrentTime)
			{
				this._lastRecordedTime = base.Mission.CurrentTime;
				base.Mission.Recorder.RecordCurrentState();
			}
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x00084BD0 File Offset: 0x00082DD0
		public override void OnEndMissionInternal()
		{
			base.OnEndMissionInternal();
			base.Mission.Recorder.BackupRecordToFile("Mission_record_" + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}_", DateTime.Now) + Process.GetCurrentProcess().Id, Game.Current.GameType.GetType().Name, base.Mission.SceneLevels);
			GameNetwork.ResetMissionData();
		}

		// Token: 0x04000E21 RID: 3617
		private float _lastRecordedTime = -1f;
	}
}
