using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.Usables;
using SandBox.ViewModelCollection.Missions.NameMarker;
using SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Missions.NameMarkers
{
	// Token: 0x02000032 RID: 50
	public class StealthNameMarkerProvider : MissionNameMarkerProvider
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x00012F0F File Offset: 0x0001110F
		protected override void OnInitialize(Mission mission)
		{
			base.OnInitialize(mission);
			this._stealthAreaMissionLogic = mission.GetMissionBehavior<StealthAreaMissionLogic>();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00012F24 File Offset: 0x00011124
		protected override void OnDestroy(Mission mission)
		{
			base.OnDestroy(mission);
			this._stealthAreaMissionLogic = null;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00012F34 File Offset: 0x00011134
		public override void CreateMarkers(List<MissionNameMarkerTargetBaseVM> markers)
		{
			this.CreateStealthAreaMarkers(markers);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00012F40 File Offset: 0x00011140
		private void CreateStealthAreaMarkers(List<MissionNameMarkerTargetBaseVM> markers)
		{
			if (this._stealthAreaMissionLogic == null)
			{
				return;
			}
			if (Mission.Current == null)
			{
				return;
			}
			if (Agent.Main != null)
			{
				foreach (StealthAreaUsePoint stealthAreaUsePoint in Mission.Current.ActiveMissionObjects.FindAllWithType<StealthAreaUsePoint>())
				{
					if (stealthAreaUsePoint.IsUsableByAgent(Agent.Main))
					{
						MissionStealthAreaUsePointNameMarkerTargetVM missionStealthAreaUsePointNameMarkerTargetVM = new MissionStealthAreaUsePointNameMarkerTargetVM(stealthAreaUsePoint);
						markers.Add(missionStealthAreaUsePointNameMarkerTargetVM);
					}
				}
			}
		}

		// Token: 0x040000F7 RID: 247
		private StealthAreaMissionLogic _stealthAreaMissionLogic;
	}
}
