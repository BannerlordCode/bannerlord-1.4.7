using System;
using SandBox.View.Missions;
using SandBox.ViewModelCollection.Missions;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000018 RID: 24
	[OverrideView(typeof(MissionAgentAlarmStateView))]
	public class MissionGauntletAgentAlarmStateView : MissionAgentAlarmStateView
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0000A3EF File Offset: 0x000085EF
		public MissionGauntletAgentAlarmStateView()
		{
			this._dataSource = new MissionAgentAlarmStateVM();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A404 File Offset: 0x00008604
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource.Initialize(base.Mission, base.MissionScreen.CombatCamera);
			this._layer = new GauntletLayer("MissionAlarmState", 10, false);
			this._layer.LoadMovie("AgentAlarmStateMissionView", this._dataSource);
			base.MissionScreen.AddLayer(this._layer);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A46E File Offset: 0x0000866E
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._layer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layer = null;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A4A0 File Offset: 0x000086A0
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			base.OnAgentBuild(agent, banner);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentBuild(agent, banner);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A4BC File Offset: 0x000086BC
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			base.OnAgentTeamChanged(prevTeam, newTeam, agent);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentTeamChanged(prevTeam, newTeam, agent);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A4DA File Offset: 0x000086DA
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentRemoved(affectedAgent);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A4F8 File Offset: 0x000086F8
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Update();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A511 File Offset: 0x00008711
		protected override void OnResumeView()
		{
			base.OnResumeView();
			ScreenManager.SetSuspendLayer(this._layer, false);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A525 File Offset: 0x00008725
		protected override void OnSuspendView()
		{
			base.OnSuspendView();
			ScreenManager.SetSuspendLayer(this._layer, true);
		}

		// Token: 0x0400006F RID: 111
		private GauntletLayer _layer;

		// Token: 0x04000070 RID: 112
		private MissionAgentAlarmStateVM _dataSource;
	}
}
