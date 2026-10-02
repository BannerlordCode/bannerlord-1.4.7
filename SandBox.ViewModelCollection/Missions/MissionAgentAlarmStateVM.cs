using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002B RID: 43
	public class MissionAgentAlarmStateVM : ViewModel
	{
		// Token: 0x0600037F RID: 895 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public MissionAgentAlarmStateVM()
		{
			this.Targets = new MBBindingList<MissionAgentAlarmTargetVM>();
			this._stealthBoxes = new List<StealthBox>();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000F2C8 File Offset: 0x0000D4C8
		public void Initialize(Mission mission, Camera camera)
		{
			this._mission = mission;
			this._camera = camera;
			this._isInitialized = true;
			this._areStealthBoxesDirty = true;
			this.RefreshTargets();
			StealthBox.OnBoxInitialized += this.OnStealthBoxInitialized;
			StealthBox.OnBoxRemoved += this.OnStealthBoxRemoved;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000F319 File Offset: 0x0000D519
		public override void OnFinalize()
		{
			base.OnFinalize();
			StealthBox.OnBoxInitialized -= this.OnStealthBoxInitialized;
			StealthBox.OnBoxRemoved -= this.OnStealthBoxRemoved;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000F343 File Offset: 0x0000D543
		private void OnStealthBoxInitialized(StealthBox stealthBox)
		{
			this._areStealthBoxesDirty = true;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000F34C File Offset: 0x0000D54C
		private void OnStealthBoxRemoved(StealthBox stealthBox)
		{
			this._areStealthBoxesDirty = true;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000F358 File Offset: 0x0000D558
		private void RefreshStealthBoxEntities()
		{
			this._stealthBoxes.Clear();
			Mission mission = Mission.Current;
			if (((mission != null) ? mission.Scene : null) == null)
			{
				return;
			}
			List<GameEntity> list = new List<GameEntity>();
			Mission.Current.Scene.GetAllEntitiesWithScriptComponent<StealthBox>(ref list);
			for (int i = 0; i < list.Count; i++)
			{
				StealthBox firstScriptOfTypeRecursive = list[i].GetFirstScriptOfTypeRecursive<StealthBox>();
				if (firstScriptOfTypeRecursive != null)
				{
					this._stealthBoxes.Add(firstScriptOfTypeRecursive);
				}
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000F3D0 File Offset: 0x0000D5D0
		public void Update()
		{
			if (!this._isInitialized)
			{
				return;
			}
			if (this._disguiseMissionLogic == null)
			{
				Mission mission = this._mission;
				this._disguiseMissionLogic = ((mission != null) ? mission.GetMissionBehavior<DisguiseMissionLogic>() : null);
			}
			DisguiseMissionLogic disguiseMissionLogic = this._disguiseMissionLogic;
			bool flag = disguiseMissionLogic != null && disguiseMissionLogic.IsInStealthMode;
			this.IsMainAgentInSafeArea = this.IsMainAgentInStealthArea();
			for (int i = 0; i < this.Targets.Count; i++)
			{
				MissionAgentAlarmTargetVM missionAgentAlarmTargetVM = this.Targets[i];
				if (this._disguiseMissionLogic == null)
				{
					missionAgentAlarmTargetVM.IsStealthModeEnabled = true;
					missionAgentAlarmTargetVM.IsMainAgentInVisibilityRange = SandBoxUIHelper.IsAgentInVisibilityRangeApproximate(missionAgentAlarmTargetVM.TargetAgent, Agent.Main);
					missionAgentAlarmTargetVM.IsInVision = true;
					missionAgentAlarmTargetVM.IsSuspected = missionAgentAlarmTargetVM.AlarmProgress > 0;
					missionAgentAlarmTargetVM.UpdateScreenPosition(this._camera);
					missionAgentAlarmTargetVM.UpdateValues();
				}
				else
				{
					missionAgentAlarmTargetVM.IsStealthModeEnabled = flag;
					DisguiseMissionLogic.ShadowingAgentOffenseInfo agentOffenseInfo = this._disguiseMissionLogic.GetAgentOffenseInfo(missionAgentAlarmTargetVM.TargetAgent);
					if (agentOffenseInfo != null)
					{
						missionAgentAlarmTargetVM.IsMainAgentInVisibilityRange = SandBoxUIHelper.IsAgentInVisibilityRangeApproximate(missionAgentAlarmTargetVM.TargetAgent, Agent.Main);
						missionAgentAlarmTargetVM.IsInVision = agentOffenseInfo.CanPlayerCameraSeeTheAgent;
						missionAgentAlarmTargetVM.IsSuspected = missionAgentAlarmTargetVM.AlarmProgress > 0;
					}
					missionAgentAlarmTargetVM.UpdateScreenPosition(this._camera);
					missionAgentAlarmTargetVM.UpdateValues();
				}
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		private bool IsMainAgentInStealthArea()
		{
			Agent main = Agent.Main;
			if (main == null)
			{
				return false;
			}
			Mission mission = Mission.Current;
			if (((mission != null) ? mission.Scene : null) == null)
			{
				return false;
			}
			if (this._areStealthBoxesDirty)
			{
				this.RefreshStealthBoxEntities();
				this._areStealthBoxesDirty = false;
			}
			for (int i = 0; i < this._stealthBoxes.Count; i++)
			{
				if (this._stealthBoxes[i].IsAgentInside(main))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F574 File Offset: 0x0000D774
		public void OnAgentRemoved(Agent agent)
		{
			MissionAgentAlarmTargetVM agentTargetFromAgent = this.GetAgentTargetFromAgent(agent);
			if (agentTargetFromAgent != null)
			{
				this.Targets.Remove(agentTargetFromAgent);
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F59C File Offset: 0x0000D79C
		private void RefreshTargets()
		{
			this.Targets.Clear();
			foreach (Agent agent in Mission.Current.Agents)
			{
				if (agent != null && SandBoxUIHelper.CanAgentBeAlarmed(agent))
				{
					this.Targets.Add(new MissionAgentAlarmTargetVM(agent, new Action<MissionAgentAlarmTargetVM>(this.OnRemoveTarget)));
				}
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000F620 File Offset: 0x0000D820
		public void OnAgentBuild(Agent agent, Banner banner)
		{
			this.RefreshTargets();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000F628 File Offset: 0x0000D828
		public void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			if (agent != null && agent == Agent.Main)
			{
				this.RefreshTargets();
				return;
			}
			MissionAgentAlarmTargetVM agentTargetFromAgent = this.GetAgentTargetFromAgent(agent);
			if (agentTargetFromAgent == null && SandBoxUIHelper.CanAgentBeAlarmed(agent))
			{
				this.Targets.Add(new MissionAgentAlarmTargetVM(agent, new Action<MissionAgentAlarmTargetVM>(this.OnRemoveTarget)));
				return;
			}
			if (agentTargetFromAgent != null && (newTeam == Team.Invalid || (newTeam == null || newTeam.IsPlayerAlly)))
			{
				this.Targets.Remove(agentTargetFromAgent);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000F69E File Offset: 0x0000D89E
		private void OnRemoveTarget(MissionAgentAlarmTargetVM targetToRemove)
		{
			this.Targets.Remove(targetToRemove);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000F6B0 File Offset: 0x0000D8B0
		private MissionAgentAlarmTargetVM GetAgentTargetFromAgent(Agent agent)
		{
			for (int i = 0; i < this.Targets.Count; i++)
			{
				MissionAgentAlarmTargetVM missionAgentAlarmTargetVM = this.Targets[i];
				if (missionAgentAlarmTargetVM.TargetAgent == agent)
				{
					return missionAgentAlarmTargetVM;
				}
			}
			return null;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000F6EC File Offset: 0x0000D8EC
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
		[DataSourceProperty]
		public MBBindingList<MissionAgentAlarmTargetVM> Targets
		{
			get
			{
				return this._targets;
			}
			set
			{
				if (value != this._targets)
				{
					this._targets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentAlarmTargetVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000F712 File Offset: 0x0000D912
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0000F71A File Offset: 0x0000D91A
		[DataSourceProperty]
		public bool IsMainAgentInSafeArea
		{
			get
			{
				return this._isMainAgentInSafeArea;
			}
			set
			{
				if (value != this._isMainAgentInSafeArea)
				{
					this._isMainAgentInSafeArea = value;
					base.OnPropertyChangedWithValue(value, "IsMainAgentInSafeArea");
				}
			}
		}

		// Token: 0x040001C6 RID: 454
		private bool _isInitialized;

		// Token: 0x040001C7 RID: 455
		private Mission _mission;

		// Token: 0x040001C8 RID: 456
		private Camera _camera;

		// Token: 0x040001C9 RID: 457
		private DisguiseMissionLogic _disguiseMissionLogic;

		// Token: 0x040001CA RID: 458
		private bool _areStealthBoxesDirty;

		// Token: 0x040001CB RID: 459
		private List<StealthBox> _stealthBoxes;

		// Token: 0x040001CC RID: 460
		private bool _isMainAgentInSafeArea;

		// Token: 0x040001CD RID: 461
		private MBBindingList<MissionAgentAlarmTargetVM> _targets;
	}
}
