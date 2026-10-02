using System;
using System.Collections.Generic;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A6 RID: 166
	public class ChangeLocationBehavior : AgentBehavior
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x0002F0C8 File Offset: 0x0002D2C8
		public ChangeLocationBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			this._initializeTime = base.Mission.CurrentTime;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002F0F4 File Offset: 0x0002D2F4
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._selectedDoor == null)
			{
				Passage passage = this.SelectADoor();
				if (passage != null)
				{
					this._selectedDoor = passage;
					base.Navigator.SetTarget(this._selectedDoor, false, Agent.AIScriptedFrameFlags.None);
					return;
				}
			}
			else if (this._selectedDoor.ToLocation.CharacterCount >= this._selectedDoor.ToLocation.ProsperityMax)
			{
				base.Navigator.SetTarget(null, false, Agent.AIScriptedFrameFlags.None);
				base.Navigator.ForceThink(0f);
				this._selectedDoor = null;
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002F178 File Offset: 0x0002D378
		private Passage SelectADoor()
		{
			Passage passage = null;
			List<Passage> list = new List<Passage>();
			foreach (UsableMachine usableMachine in this._missionAgentHandler.TownPassageProps)
			{
				Passage passage2 = (Passage)usableMachine;
				if (passage2.GetVacantStandingPointForAI(base.OwnerAgent) != null && passage2.ToLocation.CharacterCount < passage2.ToLocation.ProsperityMax)
				{
					list.Add(passage2);
				}
			}
			if (list.Count > 0)
			{
				passage = list[MBRandom.RandomInt(list.Count)];
			}
			return passage;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002F220 File Offset: 0x0002D420
		protected override void OnActivate()
		{
			base.OnActivate();
			this._selectedDoor = null;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002F22F File Offset: 0x0002D42F
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this._selectedDoor = null;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002F23E File Offset: 0x0002D43E
		public override string GetDebugInfo()
		{
			if (this._selectedDoor != null)
			{
				return "Go to " + this._selectedDoor.ToLocation.StringId;
			}
			return "Change Location no target";
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002F268 File Offset: 0x0002D468
		public override float GetAvailability(bool isSimulation)
		{
			float num = 0f;
			bool flag = false;
			bool flag2 = false;
			LocationCharacter locationCharacter = CampaignMission.Current.Location.GetLocationCharacter(base.OwnerAgent.Origin);
			if (base.Mission.CurrentTime < 5f || locationCharacter.FixedLocation || !this._missionAgentHandler.HasPassages())
			{
				return 0f;
			}
			foreach (UsableMachine usableMachine in this._missionAgentHandler.TownPassageProps)
			{
				Passage passage = usableMachine as Passage;
				if (passage.ToLocation.CanAIEnter(locationCharacter) && passage.ToLocation.CharacterCount < passage.ToLocation.ProsperityMax)
				{
					flag = true;
					if (passage.PilotStandingPoint.GameEntity.GetGlobalFrame().origin.Distance(base.OwnerAgent.Position) < 1f)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag)
			{
				if (!flag2)
				{
					num = (CampaignMission.Current.Location.IsIndoor ? 0.1f : 0.05f);
				}
				else if (base.Mission.CurrentTime - this._initializeTime > 10f)
				{
					num = 0.01f;
				}
			}
			return num;
		}

		// Token: 0x040003B1 RID: 945
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x040003B2 RID: 946
		private readonly float _initializeTime;

		// Token: 0x040003B3 RID: 947
		private Passage _selectedDoor;
	}
}
