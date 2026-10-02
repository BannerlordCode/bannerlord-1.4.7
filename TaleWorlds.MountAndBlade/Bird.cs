using System;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000327 RID: 807
	public class Bird : MissionObject
	{
		// Token: 0x06002DBE RID: 11710 RVA: 0x000B040F File Offset: 0x000AE60F
		private Bird.State ComputeInitialState()
		{
			if (this.CanFly && !this._canLand)
			{
				return Bird.State.Airborne;
			}
			return Bird.State.Perched;
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000B0424 File Offset: 0x000AE624
		protected internal override void OnInit()
		{
			base.OnInit();
			base.GameEntity.SetAnimationSoundActivation(true);
			base.GameEntity.Skeleton.SetAnimationAtChannel(this._idleAnimation, 0, 1f, -1f, 0f);
			base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBRandom.RandomFloat * 0.5f);
			this._timer = new BasicMissionTimer();
			this.SetState(this.ComputeInitialState());
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000B04B4 File Offset: 0x000AE6B4
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			base.GameEntity.Skeleton.SetAnimationAtChannel(this._idleAnimation, 0, 1f, -1f, 0f);
			base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, 0f);
			this._timer = new BasicMissionTimer();
			this.SetState(this.ComputeInitialState());
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000B0520 File Offset: 0x000AE720
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000B052A File Offset: 0x000AE72A
		private void SetState(Bird.State newState)
		{
			this._state = newState;
			this.OnStateChanged();
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000B053C File Offset: 0x000AE73C
		private void OnStateChanged()
		{
			switch (this._state)
			{
			case Bird.State.TakingOff:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._takingOffAnimation, 0, 1f, -1f, 0f);
				this._timer.Reset();
				return;
			case Bird.State.Airborne:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._flyCycleAnimation, 0, 1f, -1f, 0f);
				this._timer.Set(-MBRandom.RandomFloatRanged(5f, 15f));
				this.ApplyAnimationDisplacement();
				return;
			case Bird.State.Landing:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._landingAnimation, 0, 1f, -1f, 0f);
				this._timer.Reset();
				this.ApplyAnimationDisplacement();
				return;
			case Bird.State.Perched:
				base.GameEntity.Skeleton.SetAnimationAtChannel(this._idleAnimation, 0, 1f, -1f, 0f);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBRandom.RandomFloat * 0.5f);
				this._timer.Set(-MBRandom.RandomFloatRanged(5f, 18f));
				return;
			default:
				Debug.FailedAssert("Unknown state please handle!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Bird.cs", "OnStateChanged", 138);
				return;
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000B06A4 File Offset: 0x000AE8A4
		protected internal override void OnTick(float dt)
		{
			switch (this._state)
			{
			case Bird.State.TakingOff:
				if (base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) > 0.99f)
				{
					this.SetState(Bird.State.Airborne);
					return;
				}
				break;
			case Bird.State.Airborne:
				if (this._timer.ElapsedTime <= 0f)
				{
					this.ApplyFlyingMovement(dt);
					return;
				}
				if (this._canLand)
				{
					this.SetState(Bird.State.Landing);
					return;
				}
				base.GameEntity.SetVisibilityExcludeParents(false);
				return;
			case Bird.State.Landing:
				if (base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) > 0.99f)
				{
					this.SetState(Bird.State.Perched);
					return;
				}
				break;
			case Bird.State.Perched:
				if (this.CanFly && this._timer.ElapsedTime > 0f && base.GameEntity.Skeleton.GetAnimationParameterAtChannel(0) > 0.99f)
				{
					this.SetState(Bird.State.TakingOff);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000B0790 File Offset: 0x000AE990
		private void ApplyFlyingMovement(float dt)
		{
			float num = this._flyingSpeedInKph * 0.2777778f * dt;
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 vec = globalFrame.rotation.f.NormalizedCopy();
			globalFrame.origin -= vec * num;
			base.GameEntity.SetGlobalFrame(in globalFrame, true);
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000B07FC File Offset: 0x000AE9FC
		private void ApplyAnimationDisplacement()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 vec = globalFrame.rotation.f.NormalizedCopy();
			Vec3 vec2 = globalFrame.rotation.u.NormalizedCopy();
			if (this._state == Bird.State.Airborne)
			{
				globalFrame.origin -= vec * 20f - vec2 * 6.5f;
			}
			else if (this._state == Bird.State.Landing)
			{
				globalFrame.origin -= vec * 20f + vec2 * 6.5f;
			}
			base.GameEntity.SetGlobalFrame(in globalFrame, true);
		}

		// Token: 0x04001202 RID: 4610
		[EditableScriptComponentVariable(true, "")]
		private float _flyingSpeedInKph = 14.4f;

		// Token: 0x04001203 RID: 4611
		[EditableScriptComponentVariable(true, "")]
		private string _idleAnimation = "anim_bird_idle";

		// Token: 0x04001204 RID: 4612
		[EditableScriptComponentVariable(true, "")]
		private string _landingAnimation = "anim_bird_landing";

		// Token: 0x04001205 RID: 4613
		[EditableScriptComponentVariable(true, "")]
		private string _takingOffAnimation = "anim_bird_flying";

		// Token: 0x04001206 RID: 4614
		[EditableScriptComponentVariable(true, "")]
		private string _flyCycleAnimation = "anim_bird_cycle";

		// Token: 0x04001207 RID: 4615
		[EditableScriptComponentVariable(true, "")]
		private bool _canLand = true;

		// Token: 0x04001208 RID: 4616
		public bool CanFly;

		// Token: 0x04001209 RID: 4617
		private Bird.State _state;

		// Token: 0x0400120A RID: 4618
		private BasicMissionTimer _timer;

		// Token: 0x02000605 RID: 1541
		private enum State
		{
			// Token: 0x04002033 RID: 8243
			TakingOff,
			// Token: 0x04002034 RID: 8244
			Airborne,
			// Token: 0x04002035 RID: 8245
			Landing,
			// Token: 0x04002036 RID: 8246
			Perched
		}
	}
}
