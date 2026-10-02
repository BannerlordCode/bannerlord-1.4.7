using System;
using SandBox.View.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x0200001E RID: 30
	[OverrideView(typeof(EavesdroppingMissionCameraView))]
	public class MissionGauntletEavesdroppingCameraView : EavesdroppingMissionCameraView
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x0000B829 File Offset: 0x00009A29
		public MissionGauntletEavesdroppingCameraView()
		{
			this._gauntletLayer = new MissionGauntletEavesdroppingCameraView.EavesdroppingGauntletLayer(10, false);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B83F File Offset: 0x00009A3F
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B858 File Offset: 0x00009A58
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000B874 File Offset: 0x00009A74
		protected override void SetPlayerMovementEnabled(bool isPlayerMovementEnabled)
		{
			base.SetPlayerMovementEnabled(isPlayerMovementEnabled);
			for (int i = 0; i < base.Mission.MissionBehaviors.Count; i++)
			{
				MissionBattleUIBaseView missionBattleUIBaseView;
				if ((missionBattleUIBaseView = base.Mission.MissionBehaviors[i] as MissionBattleUIBaseView) != null)
				{
					if (!isPlayerMovementEnabled)
					{
						missionBattleUIBaseView.SuspendView();
					}
					else
					{
						missionBattleUIBaseView.ResumeView();
					}
				}
			}
			if (isPlayerMovementEnabled)
			{
				this._gauntletLayer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				return;
			}
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
		}

		// Token: 0x0400008A RID: 138
		private MissionGauntletEavesdroppingCameraView.EavesdroppingGauntletLayer _gauntletLayer;

		// Token: 0x0200007C RID: 124
		private class EavesdroppingGauntletLayer : GauntletLayer
		{
			// Token: 0x0600044A RID: 1098 RVA: 0x0001841E File Offset: 0x0001661E
			public EavesdroppingGauntletLayer(int localOrder, bool shouldClear = false)
				: base("MissionEavesdropping", localOrder, shouldClear)
			{
			}

			// Token: 0x0600044B RID: 1099 RVA: 0x0001842D File Offset: 0x0001662D
			public override bool HitTest()
			{
				return true;
			}
		}
	}
}
