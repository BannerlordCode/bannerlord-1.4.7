using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200008B RID: 139
	public class VisualTrackerMissionBehavior : MissionLogic
	{
		// Token: 0x06000564 RID: 1380 RVA: 0x00023CF7 File Offset: 0x00021EF7
		public override void OnAgentCreated(Agent agent)
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00023CF9 File Offset: 0x00021EF9
		public override void AfterStart()
		{
			this.Refresh();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00023D01 File Offset: 0x00021F01
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._visualTrackerManager.TrackedObjectsVersion != this._trackedObjectsVersion)
			{
				this.Refresh();
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00023D23 File Offset: 0x00021F23
		private void Refresh()
		{
			if (PlayerEncounter.LocationEncounter != null)
			{
				this.RefreshCommonAreas();
			}
			this._trackedObjectsVersion = this._visualTrackerManager.TrackedObjectsVersion;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00023D44 File Offset: 0x00021F44
		public void RegisterLocalOnlyObject(ITrackableBase obj)
		{
			using (List<TrackedObject>.Enumerator enumerator = this._currentTrackedObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Object == obj)
					{
						return;
					}
				}
			}
			this._currentTrackedObjects.Add(new TrackedObject(obj));
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00023DAC File Offset: 0x00021FAC
		private void RefreshCommonAreas()
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			foreach (CommonAreaMarker commonAreaMarker in base.Mission.ActiveMissionObjects.FindAllWithType<CommonAreaMarker>().ToList<CommonAreaMarker>())
			{
				if (settlement.Alleys.Count >= commonAreaMarker.AreaIndex)
				{
					this.RegisterLocalOnlyObject(commonAreaMarker);
				}
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00023E2C File Offset: 0x0002202C
		public override List<CompassItemUpdateParams> GetCompassTargets()
		{
			List<CompassItemUpdateParams> list = new List<CompassItemUpdateParams>();
			foreach (TrackedObject trackedObject in this._currentTrackedObjects)
			{
				list.Add(new CompassItemUpdateParams(trackedObject.Object, TargetIconType.Flag_A, trackedObject.Position, 4288256409U, uint.MaxValue));
			}
			return list;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00023EA0 File Offset: 0x000220A0
		private void RemoveLocalObject(ITrackableBase obj)
		{
			this._currentTrackedObjects.RemoveAll((TrackedObject x) => x.Object == obj);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00023ED2 File Offset: 0x000220D2
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			this.RemoveLocalObject(affectedAgent);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00023EDB File Offset: 0x000220DB
		public override void OnAgentDeleted(Agent affectedAgent)
		{
			this.RemoveLocalObject(affectedAgent);
		}

		// Token: 0x040002D0 RID: 720
		private List<TrackedObject> _currentTrackedObjects = new List<TrackedObject>();

		// Token: 0x040002D1 RID: 721
		private int _trackedObjectsVersion = -1;

		// Token: 0x040002D2 RID: 722
		private readonly VisualTrackerManager _visualTrackerManager = Campaign.Current.VisualTrackerManager;

		// Token: 0x0200018A RID: 394
		public enum AgentTrackTypes
		{
			// Token: 0x0400076C RID: 1900
			AvailableIssue,
			// Token: 0x0400076D RID: 1901
			ActiveIssue,
			// Token: 0x0400076E RID: 1902
			ActiveStoryQuest,
			// Token: 0x0400076F RID: 1903
			TrackedIssue,
			// Token: 0x04000770 RID: 1904
			TrackedStoryQuest
		}
	}
}
