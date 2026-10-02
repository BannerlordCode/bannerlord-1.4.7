using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000084 RID: 132
	public class ReplayMissionView : MissionView
	{
		// Token: 0x06000503 RID: 1283 RVA: 0x00025577 File Offset: 0x00023777
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._resetTime = 0f;
			this._replayMissionLogic = base.Mission.GetMissionBehavior<ReplayMissionLogic>();
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0002559C File Offset: 0x0002379C
		public override void OnPreMissionTick(float dt)
		{
			base.OnPreMissionTick(dt);
			base.Mission.Recorder.ProcessRecordUntilTime(base.Mission.CurrentTime - this._resetTime);
			bool isInputOverridden = this._isInputOverridden;
			if (base.Mission.CurrentState == Mission.State.Continuing && base.Mission.Recorder.IsEndOfRecord())
			{
				if (MBEditor._isEditorMissionOn)
				{
					MBEditor.LeaveEditMissionMode();
					return;
				}
				base.Mission.EndMission();
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00025611 File Offset: 0x00023811
		public void OverrideInput(bool isOverridden)
		{
			this._isInputOverridden = isOverridden;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0002561C File Offset: 0x0002381C
		public void ResetReplay()
		{
			this._resetTime = base.Mission.CurrentTime;
			base.Mission.ResetMission();
			base.Mission.Teams.Clear();
			base.Mission.Recorder.RestartRecord();
			MBCommon.UnPauseGameEngine();
			base.Mission.Scene.TimeSpeed = 1f;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00025680 File Offset: 0x00023880
		public void Rewind(float time)
		{
			this._resetTime = MathF.Min(this._resetTime + time, base.Mission.CurrentTime);
			base.Mission.ResetMission();
			base.Mission.Teams.Clear();
			base.Mission.Recorder.RestartRecord();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000256D6 File Offset: 0x000238D6
		public void FastForward(float time)
		{
			this._resetTime -= time;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000256E6 File Offset: 0x000238E6
		public void Pause()
		{
			if (!MBCommon.IsPaused && base.Mission.Scene.TimeSpeed.ApproximatelyEqualsTo(1f, 1E-05f))
			{
				MBCommon.PauseGameEngine();
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00025718 File Offset: 0x00023918
		public void Resume()
		{
			if (MBCommon.IsPaused || !base.Mission.Scene.TimeSpeed.ApproximatelyEqualsTo(1f, 1E-05f))
			{
				MBCommon.UnPauseGameEngine();
				base.Mission.Scene.TimeSpeed = 1f;
			}
		}

		// Token: 0x040002D5 RID: 725
		private float _resetTime;

		// Token: 0x040002D6 RID: 726
		private bool _isInputOverridden;

		// Token: 0x040002D7 RID: 727
		private ReplayMissionLogic _replayMissionLogic;
	}
}
