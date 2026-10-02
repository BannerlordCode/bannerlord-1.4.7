using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x0200004E RID: 78
	public class MissionAgentLockVisualizerVM : ViewModel
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x000181BD File Offset: 0x000163BD
		public MissionAgentLockVisualizerVM()
		{
			this._allTrackedAgentsSet = new Dictionary<Agent, MissionAgentLockItemVM>();
			this.AllTrackedAgents = new MBBindingList<MissionAgentLockItemVM>();
			this.IsEnabled = true;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000181E4 File Offset: 0x000163E4
		public void OnActiveLockAgentChange(Agent oldAgent, Agent newAgent)
		{
			if (oldAgent != null && this._allTrackedAgentsSet.ContainsKey(oldAgent))
			{
				this.AllTrackedAgents.Remove(this._allTrackedAgentsSet[oldAgent]);
				this._allTrackedAgentsSet.Remove(oldAgent);
			}
			if (newAgent != null)
			{
				if (this._allTrackedAgentsSet.ContainsKey(newAgent))
				{
					this._allTrackedAgentsSet[newAgent].SetLockState(MissionAgentLockItemVM.LockStates.Active);
					return;
				}
				MissionAgentLockItemVM missionAgentLockItemVM = new MissionAgentLockItemVM(newAgent, MissionAgentLockItemVM.LockStates.Active);
				this._allTrackedAgentsSet.Add(newAgent, missionAgentLockItemVM);
				this.AllTrackedAgents.Add(missionAgentLockItemVM);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001826C File Offset: 0x0001646C
		public void OnPossibleLockAgentChange(Agent oldPossibleAgent, Agent newPossibleAgent)
		{
			if (oldPossibleAgent != null && this._allTrackedAgentsSet.ContainsKey(oldPossibleAgent))
			{
				this.AllTrackedAgents.Remove(this._allTrackedAgentsSet[oldPossibleAgent]);
				this._allTrackedAgentsSet.Remove(oldPossibleAgent);
			}
			if (newPossibleAgent != null)
			{
				if (this._allTrackedAgentsSet.ContainsKey(newPossibleAgent))
				{
					this._allTrackedAgentsSet[newPossibleAgent].SetLockState(MissionAgentLockItemVM.LockStates.Possible);
					return;
				}
				MissionAgentLockItemVM missionAgentLockItemVM = new MissionAgentLockItemVM(newPossibleAgent, MissionAgentLockItemVM.LockStates.Possible);
				this._allTrackedAgentsSet.Add(newPossibleAgent, missionAgentLockItemVM);
				this.AllTrackedAgents.Add(missionAgentLockItemVM);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x000182F4 File Offset: 0x000164F4
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x000182FC File Offset: 0x000164FC
		[DataSourceProperty]
		public MBBindingList<MissionAgentLockItemVM> AllTrackedAgents
		{
			get
			{
				return this._allTrackedAgents;
			}
			set
			{
				if (value != this._allTrackedAgents)
				{
					this._allTrackedAgents = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentLockItemVM>>(value, "AllTrackedAgents");
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001831A File Offset: 0x0001651A
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x00018322 File Offset: 0x00016522
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					if (!value)
					{
						this.AllTrackedAgents.Clear();
						this._allTrackedAgentsSet.Clear();
					}
				}
			}
		}

		// Token: 0x040002EC RID: 748
		private readonly Dictionary<Agent, MissionAgentLockItemVM> _allTrackedAgentsSet;

		// Token: 0x040002ED RID: 749
		private MBBindingList<MissionAgentLockItemVM> _allTrackedAgents;

		// Token: 0x040002EE RID: 750
		private bool _isEnabled;
	}
}
