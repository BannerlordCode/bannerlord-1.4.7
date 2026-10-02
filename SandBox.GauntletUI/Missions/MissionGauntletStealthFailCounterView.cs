using System;
using SandBox.Missions;
using SandBox.View.Missions;
using SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000022 RID: 34
	[OverrideView(typeof(MissionStealthFailCounterView))]
	public class MissionGauntletStealthFailCounterView : MissionStealthFailCounterView
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x0000BEE0 File Offset: 0x0000A0E0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._countdownCounterVM = new MissionStealthFailCounterVM();
			this._countdownLayer = new GauntletLayer("MissionStealthFailCounter", 10, false);
			this._countdownLayer.LoadMovie("MissionStealthFailCounter", this._countdownCounterVM);
			base.MissionScreen.AddLayer(this._countdownLayer);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000BF39 File Offset: 0x0000A139
		public override void AfterStart()
		{
			this._stealthFailCounterMissionLogic = base.Mission.GetMissionBehavior<StealthFailCounterMissionLogic>();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000BF4C File Offset: 0x0000A14C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._countdownCounterVM.OnFinalize();
			base.MissionScreen.RemoveLayer(this._countdownLayer);
			this._countdownLayer = null;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000BF77 File Offset: 0x0000A177
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._stealthFailCounterMissionLogic != null)
			{
				this._countdownCounterVM.UpdateFailCounter(this._stealthFailCounterMissionLogic.FailCounterElapsedTime, this._stealthFailCounterMissionLogic.FailCounterSeconds, this._stealthFailCounterMissionLogic.IsActive);
			}
		}

		// Token: 0x04000095 RID: 149
		private GauntletLayer _countdownLayer;

		// Token: 0x04000096 RID: 150
		private MissionStealthFailCounterVM _countdownCounterVM;

		// Token: 0x04000097 RID: 151
		private StealthFailCounterMissionLogic _stealthFailCounterMissionLogic;
	}
}
