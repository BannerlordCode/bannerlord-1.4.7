using System;
using SandBox.Missions;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000010 RID: 16
	public class EavesdroppingMissionCameraView : MissionView
	{
		// Token: 0x06000074 RID: 116 RVA: 0x0000476C File Offset: 0x0000296C
		protected virtual void SetPlayerMovementEnabled(bool isPlayerMovementEnabled)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000476E File Offset: 0x0000296E
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.None;
			this._eavesdroppingMissionLogic = base.Mission.GetMissionBehavior<EavesdroppingMissionLogic>();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004790 File Offset: 0x00002990
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._eavesdroppingMissionLogic != null)
			{
				switch (this._cameraSwitchState)
				{
				case EavesdroppingMissionCameraView.CameraSwitchState.None:
					if ((this._eavesdroppingMissionLogic.EavesdropStarted && base.MissionScreen.CustomCamera == null) || (!this._eavesdroppingMissionLogic.EavesdropStarted && base.MissionScreen.CustomCamera != null))
					{
						if (this._eavesdroppingMissionLogic.EavesdropStarted && base.MissionScreen.CustomCamera == null)
						{
							this.SetPlayerMovementEnabled(false);
						}
						this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.ReadyForFadeOut;
						return;
					}
					break;
				case EavesdroppingMissionCameraView.CameraSwitchState.ReadyForFadeOut:
					ScreenFadeController.BeginFadeOutAndIn(0.5f, 0.5f, 0.5f);
					this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.FadeOutAndInStarted;
					return;
				case EavesdroppingMissionCameraView.CameraSwitchState.FadeOutAndInStarted:
					if (ScreenFadeController.IsFadedOut)
					{
						base.MissionScreen.CustomCamera = ((base.MissionScreen.CustomCamera == null) ? this._eavesdroppingMissionLogic.CurrentEavesdroppingCamera : null);
						if (base.MissionScreen.CustomCamera == null)
						{
							this.SetPlayerMovementEnabled(true);
						}
						this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.WaitingForFadeInToEnd;
						return;
					}
					break;
				case EavesdroppingMissionCameraView.CameraSwitchState.WaitingForFadeInToEnd:
					if (!ScreenFadeController.IsFadeActive)
					{
						this._cameraSwitchState = EavesdroppingMissionCameraView.CameraSwitchState.None;
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x04000017 RID: 23
		private EavesdroppingMissionCameraView.CameraSwitchState _cameraSwitchState;

		// Token: 0x04000018 RID: 24
		private EavesdroppingMissionLogic _eavesdroppingMissionLogic;

		// Token: 0x02000083 RID: 131
		private enum CameraSwitchState
		{
			// Token: 0x04000290 RID: 656
			None,
			// Token: 0x04000291 RID: 657
			ReadyForFadeOut,
			// Token: 0x04000292 RID: 658
			FadeOutAndInStarted,
			// Token: 0x04000293 RID: 659
			WaitingForFadeInToEnd
		}
	}
}
