using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A1 RID: 161
	public abstract class AgentBehavior
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0002CBC9 File Offset: 0x0002ADC9
		public AgentNavigator Navigator
		{
			get
			{
				return this.BehaviorGroup.Navigator;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0002CBD6 File Offset: 0x0002ADD6
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x0002CBDE File Offset: 0x0002ADDE
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
					if (this._isActive)
					{
						this.OnActivate();
						return;
					}
					this.OnDeactivate();
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0002CC05 File Offset: 0x0002AE05
		public Agent OwnerAgent
		{
			get
			{
				return this.Navigator.OwnerAgent;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0002CC12 File Offset: 0x0002AE12
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0002CC1A File Offset: 0x0002AE1A
		public Mission Mission { get; private set; }

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002CC24 File Offset: 0x0002AE24
		protected AgentBehavior(AgentBehaviorGroup behaviorGroup)
		{
			this.Mission = behaviorGroup.Mission;
			this.CheckTime = 40f + MBRandom.RandomFloat * 20f;
			this.BehaviorGroup = behaviorGroup;
			this._isActive = false;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002CC73 File Offset: 0x0002AE73
		public virtual float GetAvailability(bool isSimulation)
		{
			return 0f;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002CC7A File Offset: 0x0002AE7A
		public virtual void Tick(float dt, bool isSimulation)
		{
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002CC7C File Offset: 0x0002AE7C
		public virtual void ConversationTick()
		{
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002CC7E File Offset: 0x0002AE7E
		protected virtual void OnActivate()
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002CC80 File Offset: 0x0002AE80
		protected virtual void OnDeactivate()
		{
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002CC82 File Offset: 0x0002AE82
		public virtual bool CheckStartWithBehavior()
		{
			return false;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002CC85 File Offset: 0x0002AE85
		public virtual void OnSpecialTargetChanged()
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002CC87 File Offset: 0x0002AE87
		public virtual void SetCustomWanderTarget(UsableMachine customUsableMachine)
		{
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0002CC89 File Offset: 0x0002AE89
		public virtual void OnAgentRemoved(Agent agent)
		{
		}

		// Token: 0x060006AF RID: 1711
		public abstract string GetDebugInfo();

		// Token: 0x04000393 RID: 915
		public float CheckTime = 15f;

		// Token: 0x04000394 RID: 916
		protected readonly AgentBehaviorGroup BehaviorGroup;

		// Token: 0x04000395 RID: 917
		private bool _isActive;
	}
}
