using System;
using System.Collections.Generic;
using SandBox.View.Missions.NameMarkers;
using SandBox.ViewModelCollection.Missions.NameMarker;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000020 RID: 32
	[OverrideView(typeof(MissionNameMarkerUIHandler))]
	public class MissionGauntletNameMarkerView : MissionNameMarkerUIHandler
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x0000BA24 File Offset: 0x00009C24
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._nameMarkerProviders = MissionNameMarkerFactory.CollectProviders();
			for (int i = 0; i < this._nameMarkerProviders.Count; i++)
			{
				this._nameMarkerProviders[i].Initialize(base.Mission, new Action(this.SetMarkersDirty));
			}
			this._dataSource = new MissionNameMarkerVM(this._nameMarkerProviders, base.MissionScreen.CombatCamera);
			this._gauntletLayer = new GauntletLayer("MissionNameMarker", 1, false);
			this._gauntletLayer.LoadMovie("NameMarker", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			if (Campaign.Current != null)
			{
				this._lastVisualTrackerVersion = Campaign.Current.VisualTrackerManager.TrackedObjectsVersion;
				CampaignEvents.ConversationEnded.AddNonSerializedListener(this, new Action<IEnumerable<CharacterObject>>(this.OnConversationEnd));
			}
			MissionNameMarkerFactory.OnProvidersChanged += this.OnMarkersChanged;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000BB18 File Offset: 0x00009D18
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			for (int i = 0; i < this._nameMarkerProviders.Count; i++)
			{
				this._nameMarkerProviders[i].Destroy(base.Mission);
			}
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			if (Campaign.Current != null)
			{
				CampaignEvents.ConversationEnded.ClearListeners(this);
			}
			InformationManager.HideAllMessages();
			MissionNameMarkerFactory.OnProvidersChanged -= this.OnMarkersChanged;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000BBAC File Offset: 0x00009DAC
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (!base.IsViewCreated)
			{
				return;
			}
			if (base.IsViewSuspended != this._gauntletLayer.IsActive)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, base.IsViewSuspended);
			}
			for (int i = 0; i < this._nameMarkerProviders.Count; i++)
			{
				this._nameMarkerProviders[i].Tick(dt);
			}
			if (base.Input.IsGameKeyDown(5))
			{
				this._dataSource.IsEnabled = true;
			}
			else
			{
				this._dataSource.IsEnabled = false;
			}
			if (Campaign.Current != null && this._lastVisualTrackerVersion != Campaign.Current.VisualTrackerManager.TrackedObjectsVersion)
			{
				this.SetMarkersDirty();
				this._lastVisualTrackerVersion = Campaign.Current.VisualTrackerManager.TrackedObjectsVersion;
			}
			this._dataSource.Tick(dt);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000BC84 File Offset: 0x00009E84
		private void OnMarkersChanged()
		{
			List<MissionNameMarkerProvider> list;
			List<MissionNameMarkerProvider> list2;
			MissionNameMarkerFactory.UpdateProviders(this._nameMarkerProviders.ToArray(), out list, out list2);
			for (int i = 0; i < list2.Count; i++)
			{
				this._nameMarkerProviders.Remove(list2[i]);
			}
			for (int j = 0; j < list.Count; j++)
			{
				this._nameMarkerProviders.Add(list[j]);
			}
			this.SetMarkersDirty();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000BCF2 File Offset: 0x00009EF2
		public override void SetMarkersDirty()
		{
			MissionNameMarkerVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.SetTargetsDirty();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000BD04 File Offset: 0x00009F04
		public override void OnAgentBuild(Agent affectedAgent, Banner banner)
		{
			base.OnAgentBuild(affectedAgent, banner);
			if (base.Mission.Mode != MissionMode.Battle)
			{
				this.SetMarkersDirty();
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000BD22 File Offset: 0x00009F22
		public override void OnAgentDeleted(Agent affectedAgent)
		{
			if (base.Mission.Mode != MissionMode.Battle)
			{
				this.SetMarkersDirty();
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000BD38 File Offset: 0x00009F38
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (base.Mission.Mode != MissionMode.Battle)
			{
				this.SetMarkersDirty();
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000BD4E File Offset: 0x00009F4E
		private void OnConversationEnd(IEnumerable<CharacterObject> conversationCharacters)
		{
			if (base.Mission.Mode != MissionMode.Battle)
			{
				this.SetMarkersDirty();
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000BD64 File Offset: 0x00009F64
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000BD89 File Offset: 0x00009F89
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000BDAE File Offset: 0x00009FAE
		protected override void OnResumeView()
		{
			base.OnResumeView();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000BDB6 File Offset: 0x00009FB6
		protected override void OnSuspendView()
		{
			base.OnSuspendView();
		}

		// Token: 0x0400008C RID: 140
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400008D RID: 141
		private MissionNameMarkerVM _dataSource;

		// Token: 0x0400008E RID: 142
		private List<MissionNameMarkerProvider> _nameMarkerProviders;

		// Token: 0x0400008F RID: 143
		private int _lastVisualTrackerVersion;
	}
}
