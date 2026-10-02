using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000060 RID: 96
	public class PopupSceneSequence : ScriptComponentBehavior
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x0001B611 File Offset: 0x00019811
		public void InitializeWithAgentVisuals(AgentVisuals visuals)
		{
			this._agentVisuals = visuals;
			this._time = 0f;
			this._triggered = false;
			this._state = 0;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001B63B File Offset: 0x0001983B
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001B64F File Offset: 0x0001984F
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001B65C File Offset: 0x0001985C
		protected override void OnTick(float dt)
		{
			this._time += dt;
			if (!this._triggered)
			{
				if (this._state == 0 && this._time >= this.InitialActivationTime)
				{
					this._triggered = true;
					this.OnInitialState();
				}
				if (this._state == 1 && this._time >= this.PositiveActivationTime)
				{
					this._triggered = true;
					this.OnPositiveState();
				}
				if (this._state == 2 && this._time >= this.NegativeActivationTime)
				{
					this._triggered = true;
					this.OnNegativeState();
				}
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001B6EA File Offset: 0x000198EA
		public virtual void OnInitialState()
		{
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001B6EC File Offset: 0x000198EC
		public virtual void OnPositiveState()
		{
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001B6EE File Offset: 0x000198EE
		public virtual void OnNegativeState()
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001B6F0 File Offset: 0x000198F0
		public void SetInitialState()
		{
			this._triggered = false;
			this._state = 0;
			this._time = 0f;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001B70B File Offset: 0x0001990B
		public void SetPositiveState()
		{
			this._triggered = false;
			this._state = 1;
			this._time = 0f;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001B726 File Offset: 0x00019926
		public void SetNegativeState()
		{
			this._triggered = false;
			this._state = 2;
			this._time = 0f;
		}

		// Token: 0x0400020A RID: 522
		public float InitialActivationTime;

		// Token: 0x0400020B RID: 523
		public float PositiveActivationTime;

		// Token: 0x0400020C RID: 524
		public float NegativeActivationTime;

		// Token: 0x0400020D RID: 525
		protected AgentVisuals _agentVisuals;

		// Token: 0x0400020E RID: 526
		protected float _time;

		// Token: 0x0400020F RID: 527
		protected bool _triggered;

		// Token: 0x04000210 RID: 528
		protected int _state;
	}
}
