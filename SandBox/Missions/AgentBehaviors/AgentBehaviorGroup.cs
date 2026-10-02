using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A2 RID: 162
	public abstract class AgentBehaviorGroup
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0002CC8B File Offset: 0x0002AE8B
		public Agent OwnerAgent
		{
			get
			{
				return this.Navigator.OwnerAgent;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0002CC98 File Offset: 0x0002AE98
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0002CCA0 File Offset: 0x0002AEA0
		public AgentBehavior ScriptedBehavior { get; private set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0002CCA9 File Offset: 0x0002AEA9
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0002CCB1 File Offset: 0x0002AEB1
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0002CCD8 File Offset: 0x0002AED8
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
		public Mission Mission { get; private set; }

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002CCE9 File Offset: 0x0002AEE9
		protected AgentBehaviorGroup(AgentNavigator navigator, Mission mission)
		{
			this.Mission = mission;
			this.Behaviors = new List<AgentBehavior>();
			this.Navigator = navigator;
			this._isActive = false;
			this.ScriptedBehavior = null;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002CD24 File Offset: 0x0002AF24
		public T AddBehavior<T>() where T : AgentBehavior
		{
			T t = Activator.CreateInstance(typeof(T), new object[] { this }) as T;
			if (t != null)
			{
				foreach (AgentBehavior agentBehavior in this.Behaviors)
				{
					if (agentBehavior.GetType() == t.GetType())
					{
						return agentBehavior as T;
					}
				}
				this.Behaviors.Add(t);
				return t;
			}
			return t;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002CDD8 File Offset: 0x0002AFD8
		public T GetBehavior<T>() where T : AgentBehavior
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior is T)
				{
					return (T)((object)agentBehavior);
				}
			}
			return default(T);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002CE40 File Offset: 0x0002B040
		public bool HasBehavior<T>() where T : AgentBehavior
		{
			using (List<AgentBehavior>.Enumerator enumerator = this.Behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is T)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002CE9C File Offset: 0x0002B09C
		public void RemoveBehavior<T>() where T : AgentBehavior
		{
			for (int i = 0; i < this.Behaviors.Count; i++)
			{
				if (this.Behaviors[i] is T)
				{
					bool isActive = this.Behaviors[i].IsActive;
					this.Behaviors[i].IsActive = false;
					if (this.ScriptedBehavior == this.Behaviors[i])
					{
						this.ScriptedBehavior = null;
					}
					this.Behaviors.RemoveAt(i);
					if (isActive)
					{
						this.ForceThink(0f);
					}
				}
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002CF2C File Offset: 0x0002B12C
		public void SetScriptedBehavior<T>() where T : AgentBehavior
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior is T)
				{
					this.ScriptedBehavior = agentBehavior;
					this.ForceThink(0f);
					break;
				}
			}
			foreach (AgentBehavior agentBehavior2 in this.Behaviors)
			{
				if (agentBehavior2 != this.ScriptedBehavior)
				{
					agentBehavior2.IsActive = false;
				}
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002CFE0 File Offset: 0x0002B1E0
		public void DisableScriptedBehavior()
		{
			if (this.ScriptedBehavior != null)
			{
				this.ScriptedBehavior.IsActive = false;
				this.ScriptedBehavior = null;
				this.ForceThink(0f);
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002D008 File Offset: 0x0002B208
		public void DisableAllBehaviors()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				agentBehavior.IsActive = false;
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002D05C File Offset: 0x0002B25C
		public AgentBehavior GetActiveBehavior()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					return agentBehavior;
				}
			}
			return null;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002D0B8 File Offset: 0x0002B2B8
		public virtual void Tick(float dt, bool isSimulation)
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002D0BA File Offset: 0x0002B2BA
		public virtual void ConversationTick()
		{
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002D0BC File Offset: 0x0002B2BC
		public virtual void OnAgentRemoved(Agent agent)
		{
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002D0BE File Offset: 0x0002B2BE
		protected virtual void OnActivate()
		{
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002D0C0 File Offset: 0x0002B2C0
		protected virtual void OnDeactivate()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				agentBehavior.IsActive = false;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002D114 File Offset: 0x0002B314
		public virtual float GetScore(bool isSimulation)
		{
			return 0f;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002D11B File Offset: 0x0002B31B
		public virtual void ForceThink(float inSeconds)
		{
		}

		// Token: 0x04000397 RID: 919
		public AgentNavigator Navigator;

		// Token: 0x04000398 RID: 920
		public List<AgentBehavior> Behaviors;

		// Token: 0x04000399 RID: 921
		protected float CheckBehaviorTime = 5f;

		// Token: 0x0400039A RID: 922
		protected Timer CheckBehaviorTimer;

		// Token: 0x0400039C RID: 924
		private bool _isActive;
	}
}
