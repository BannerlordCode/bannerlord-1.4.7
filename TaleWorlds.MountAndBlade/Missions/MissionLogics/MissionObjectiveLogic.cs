using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.Missions.MissionLogics
{
	// Token: 0x020003EB RID: 1003
	public class MissionObjectiveLogic : MissionLogic
	{
		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x0600370C RID: 14092 RVA: 0x000E390F File Offset: 0x000E1B0F
		public MissionObjective CurrentObjective
		{
			get
			{
				return this._currentObjective;
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x000E3920 File Offset: 0x000E1B20
		public void StartObjective(MissionObjective objective)
		{
			if (objective == null || objective.IsStarted || objective.IsCompleted)
			{
				Debug.FailedAssert("Trying to start an invalid mission objective.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\MissionObjectiveLogic.cs", "StartObjective", 20);
				return;
			}
			this.CompleteCurrentObjective();
			this._currentObjective = objective;
			if (this._currentObjective != null && this._currentObjective.GetIsActivationRequirementsMet())
			{
				this.StartObjectiveAux();
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x000E397F File Offset: 0x000E1B7F
		private void StartObjectiveAux()
		{
			Debug.Print("Mission: Start objective: " + this._currentObjective.UniqueId, 0, Debug.DebugColor.White, 17592186044416UL);
			this._currentObjective.Start();
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x000E39B4 File Offset: 0x000E1BB4
		public void CompleteCurrentObjective()
		{
			if (this._currentObjective == null)
			{
				return;
			}
			Debug.Print("Mission: Complete objective: " + this._currentObjective.UniqueId, 0, Debug.DebugColor.White, 17592186044416UL);
			this._currentObjective.Complete();
			this._currentObjective = null;
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x000E3A04 File Offset: 0x000E1C04
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._currentObjective != null && !this._currentObjective.IsStarted && this._currentObjective.GetIsActivationRequirementsMet())
			{
				this.StartObjectiveAux();
			}
			MissionObjective currentObjective = this._currentObjective;
			if (currentObjective != null)
			{
				currentObjective.Tick(dt);
			}
			if (this._currentObjective != null && this._currentObjective.IsStarted && this._currentObjective.GetIsCompletionRequirementsMet())
			{
				this.CompleteCurrentObjective();
			}
		}

		// Token: 0x040017B3 RID: 6067
		private MissionObjective _currentObjective;
	}
}
