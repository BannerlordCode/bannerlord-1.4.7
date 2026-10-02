using System;
using SandBox.View.Missions;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x0200001F RID: 31
	[OverrideView(typeof(MissionHideoutAmbushCinematicView))]
	public class MissionGauntletHideoutAmbushCinematicView : MissionHideoutAmbushCinematicView
	{
		// Token: 0x060001BD RID: 445 RVA: 0x0000B922 File Offset: 0x00009B22
		public MissionGauntletHideoutAmbushCinematicView()
		{
			this._gauntletLayer = new MissionGauntletHideoutAmbushCinematicView.HideoutAmbushCutsceneGauntletLayer(10, false);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B938 File Offset: 0x00009B38
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B951 File Offset: 0x00009B51
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B96C File Offset: 0x00009B6C
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

		// Token: 0x0400008B RID: 139
		private MissionGauntletHideoutAmbushCinematicView.HideoutAmbushCutsceneGauntletLayer _gauntletLayer;

		// Token: 0x0200007D RID: 125
		private class HideoutAmbushCutsceneGauntletLayer : GauntletLayer
		{
			// Token: 0x0600044C RID: 1100 RVA: 0x00018430 File Offset: 0x00016630
			public HideoutAmbushCutsceneGauntletLayer(int localOrder, bool shouldClear = false)
				: base("MissionHideoutAmbushCutscene", localOrder, shouldClear)
			{
			}

			// Token: 0x0600044D RID: 1101 RVA: 0x0001843F File Offset: 0x0001663F
			public override bool HitTest()
			{
				return true;
			}
		}
	}
}
