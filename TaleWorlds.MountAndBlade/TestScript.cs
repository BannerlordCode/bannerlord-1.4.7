using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000367 RID: 871
	public class TestScript : ScriptComponentBehavior
	{
		// Token: 0x060031F7 RID: 12791 RVA: 0x000CBA38 File Offset: 0x000C9C38
		private void Move(float dt)
		{
			if (MathF.Abs(this.MoveDistance) < 1E-05f)
			{
				return;
			}
			Vec3 vec = new Vec3(this.MoveAxisX, this.MoveAxisY, this.MoveAxisZ, -1f);
			vec.Normalize();
			Vec3 vec2 = vec * this.MoveSpeed * this.MoveDirection;
			float num = vec2.Length * this.MoveDirection;
			if (this.CurrentDistance + num <= -this.MoveDistance)
			{
				this.MoveDirection = 1f;
				num *= -1f;
				vec2 *= -1f;
			}
			else if (this.CurrentDistance + num >= this.MoveDistance)
			{
				this.MoveDirection = -1f;
				num *= -1f;
				vec2 *= -1f;
			}
			this.CurrentDistance += num;
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.origin += vec2;
			base.GameEntity.SetFrame(ref frame, true);
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000CBB50 File Offset: 0x000C9D50
		private void Rotate(float dt)
		{
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.rotation.RotateAboutUp(this.rotationSpeed * 0.001f * dt);
			base.GameEntity.SetFrame(ref frame, true);
			this.currentRotation += this.rotationSpeed * 0.001f * dt;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000CBBB2 File Offset: 0x000C9DB2
		private bool isRotationPhaseInsidePhaseBoundries(float currentPhase, float startPhase, float endPhase)
		{
			if (endPhase <= startPhase)
			{
				return currentPhase > startPhase;
			}
			return currentPhase > startPhase && currentPhase < endPhase;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000CBBC8 File Offset: 0x000C9DC8
		public static int GetIntegerFromStringEnd(string str)
		{
			string text = "";
			for (int i = str.Length - 1; i > -1; i--)
			{
				char c = str[i];
				if (c < '0' || c > '9')
				{
					break;
				}
				text = c.ToString() + text;
			}
			return Convert.ToInt32(text);
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x000CBC14 File Offset: 0x000C9E14
		private void DoWaterMillCalculation()
		{
			float num = (float)base.GameEntity.ChildCount;
			if (num > 0f)
			{
				IEnumerable<WeakGameEntity> children = base.GameEntity.GetChildren();
				float num2 = 6.28f / num;
				foreach (WeakGameEntity weakGameEntity in children)
				{
					int integerFromStringEnd = TestScript.GetIntegerFromStringEnd(weakGameEntity.Name);
					float num3 = this.currentRotation % 6.28f;
					float num4 = (num2 * (float)integerFromStringEnd + this.waterSplashPhaseOffset) % 6.28f;
					float num5 = (num4 + num2 * this.waterSplashIntervalMultiplier) % 6.28f;
					if (this.isRotationPhaseInsidePhaseBoundries(num3, num4, num5))
					{
						weakGameEntity.ResumeParticleSystem(true);
					}
					else
					{
						weakGameEntity.PauseParticleSystem(true);
					}
				}
			}
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x000CBCEC File Offset: 0x000C9EEC
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x000CBD00 File Offset: 0x000C9F00
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x000CBD0A File Offset: 0x000C9F0A
		protected internal override void OnTick(float dt)
		{
			this.Rotate(dt);
			this.Move(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x000CBD28 File Offset: 0x000C9F28
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.Rotate(dt);
			this.Move(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
			if (this.sideRotatingEntity != null)
			{
				MatrixFrame frame = this.sideRotatingEntity.GetFrame();
				frame.rotation.RotateAboutSide(this.rotationSpeed * 0.01f * dt);
				this.sideRotatingEntity.SetFrame(ref frame, true);
			}
			if (this.forwardRotatingEntity != null)
			{
				MatrixFrame frame2 = this.forwardRotatingEntity.GetFrame();
				frame2.rotation.RotateAboutSide(this.rotationSpeed * 0.005f * dt);
				this.forwardRotatingEntity.SetFrame(ref frame2, true);
			}
		}

		// Token: 0x0400151F RID: 5407
		public string testString;

		// Token: 0x04001520 RID: 5408
		public float rotationSpeed;

		// Token: 0x04001521 RID: 5409
		public float waterSplashPhaseOffset;

		// Token: 0x04001522 RID: 5410
		public float waterSplashIntervalMultiplier = 1f;

		// Token: 0x04001523 RID: 5411
		public bool isWaterMill;

		// Token: 0x04001524 RID: 5412
		private float currentRotation;

		// Token: 0x04001525 RID: 5413
		public float MoveAxisX = 1f;

		// Token: 0x04001526 RID: 5414
		public float MoveAxisY;

		// Token: 0x04001527 RID: 5415
		public float MoveAxisZ;

		// Token: 0x04001528 RID: 5416
		public float MoveSpeed = 0.0001f;

		// Token: 0x04001529 RID: 5417
		public float MoveDistance = 10f;

		// Token: 0x0400152A RID: 5418
		protected float MoveDirection = 1f;

		// Token: 0x0400152B RID: 5419
		protected float CurrentDistance;

		// Token: 0x0400152C RID: 5420
		public GameEntity sideRotatingEntity;

		// Token: 0x0400152D RID: 5421
		public GameEntity forwardRotatingEntity;
	}
}
