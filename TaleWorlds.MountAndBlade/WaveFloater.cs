using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000381 RID: 897
	public class WaveFloater : ScriptComponentBehavior
	{
		// Token: 0x060033A7 RID: 13223 RVA: 0x000D4165 File Offset: 0x000D2365
		private float ConvertToRadians(float angle)
		{
			return 0.017453292f * angle;
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000D4170 File Offset: 0x000D2370
		private void SetMatrix()
		{
			this.resetMF = base.GameEntity.GetGlobalFrame();
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000D4194 File Offset: 0x000D2394
		private void ResetMatrix()
		{
			base.GameEntity.SetGlobalFrame(in this.resetMF, true);
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000D41B8 File Offset: 0x000D23B8
		private void CalculateAxis()
		{
			this.axis = new Vec3(Convert.ToSingle(this.oscillateAtX), Convert.ToSingle(this.oscillateAtY), Convert.ToSingle(this.oscillateAtZ), -1f);
			base.GameEntity.GetGlobalFrame().TransformToParent(in this.axis);
			this.axis.Normalize();
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000D421F File Offset: 0x000D241F
		private float CalculateSpeed(float fq, float maxVal, bool angular)
		{
			if (!angular)
			{
				return maxVal * 2f * fq * 1f;
			}
			return maxVal * 3.1415927f / 90f * fq * 1f;
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000D424C File Offset: 0x000D244C
		private void CalculateOscilations()
		{
			this.ResetMatrix();
			this.oscillationStart = base.GameEntity.GetGlobalFrame();
			this.oscillationEnd = base.GameEntity.GetGlobalFrame();
			this.oscillationStart.rotation.RotateAboutAnArbitraryVector(in this.axis, -this.ConvertToRadians(this.maxOscillationAngle));
			this.oscillationEnd.rotation.RotateAboutAnArbitraryVector(in this.axis, this.ConvertToRadians(this.maxOscillationAngle));
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000D42CC File Offset: 0x000D24CC
		private void Oscillate()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			globalFrame.rotation = Mat3.Lerp(in this.oscillationStart.rotation, in this.oscillationEnd.rotation, MathF.Clamp(this.oscillationPercentage, 0f, 1f));
			base.GameEntity.SetGlobalFrame(in globalFrame, true);
			this.oscillationPercentage = (1f + MathF.Cos(this.oscillationSpeed * 1f * this.et)) / 2f;
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000D435C File Offset: 0x000D255C
		private void CalculateBounces()
		{
			this.ResetMatrix();
			this.bounceXStart = base.GameEntity.GetGlobalFrame();
			this.bounceXEnd = base.GameEntity.GetGlobalFrame();
			this.bounceYStart = base.GameEntity.GetGlobalFrame();
			this.bounceYEnd = base.GameEntity.GetGlobalFrame();
			this.bounceZStart = base.GameEntity.GetGlobalFrame();
			this.bounceZEnd = base.GameEntity.GetGlobalFrame();
			this.bounceXStart.origin.x = this.bounceXStart.origin.x + this.maxBounceXDistance;
			this.bounceXEnd.origin.x = this.bounceXEnd.origin.x - this.maxBounceXDistance;
			this.bounceYStart.origin.y = this.bounceYStart.origin.y + this.maxBounceYDistance;
			this.bounceYEnd.origin.y = this.bounceYEnd.origin.y - this.maxBounceYDistance;
			this.bounceZStart.origin.z = this.bounceZStart.origin.z + this.maxBounceZDistance;
			this.bounceZEnd.origin.z = this.bounceZEnd.origin.z - this.maxBounceZDistance;
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000D4484 File Offset: 0x000D2684
		private void Bounce()
		{
			if (this.bounceX)
			{
				MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
				matrixFrame.origin.x = Vec3.Lerp(this.bounceXStart.origin, this.bounceXEnd.origin, MathF.Clamp(this.bounceXPercentage, 0f, 1f)).x;
				base.GameEntity.SetGlobalFrame(in matrixFrame, true);
				this.bounceXPercentage = (1f + MathF.Sin(this.bounceXSpeed * 1f * this.et)) / 2f;
			}
			if (this.bounceY)
			{
				MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
				matrixFrame.origin.y = Vec3.Lerp(this.bounceYStart.origin, this.bounceYEnd.origin, MathF.Clamp(this.bounceYPercentage, 0f, 1f)).y;
				base.GameEntity.SetGlobalFrame(in matrixFrame, true);
				this.bounceYPercentage = (1f + MathF.Cos(this.bounceYSpeed * 1f * this.et)) / 2f;
			}
			if (this.bounceZ)
			{
				MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
				matrixFrame.origin.z = Vec3.Lerp(this.bounceZStart.origin, this.bounceZEnd.origin, MathF.Clamp(this.bounceZPercentage, 0f, 1f)).z;
				base.GameEntity.SetGlobalFrame(in matrixFrame, true);
				this.bounceZPercentage = (1f + MathF.Cos(this.bounceZSpeed * 1f * this.et)) / 2f;
			}
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000D4654 File Offset: 0x000D2854
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetMatrix();
			this.oscillate = this.oscillateAtX || this.oscillateAtY || this.oscillateAtZ;
			this.bounce = this.bounceX || this.bounceY || this.bounceZ;
			this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
			this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
			this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
			this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
			this.CalculateBounces();
			this.CalculateAxis();
			this.CalculateOscilations();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000D4730 File Offset: 0x000D2930
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.SetMatrix();
			this.oscillate = this.oscillateAtX || this.oscillateAtY || this.oscillateAtZ;
			this.bounce = this.bounceX || this.bounceY || this.bounceZ;
			this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
			this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
			this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
			this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
			this.CalculateBounces();
			this.CalculateAxis();
			this.CalculateOscilations();
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000D47FD File Offset: 0x000D29FD
		protected internal override void OnSceneSave(string saveFolder)
		{
			base.OnSceneSave(saveFolder);
			this.ResetMatrix();
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x000D480C File Offset: 0x000D2A0C
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.et += dt;
			if (this.oscillate)
			{
				this.Oscillate();
			}
			if (this.bounce)
			{
				this.Bounce();
			}
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x000D483F File Offset: 0x000D2A3F
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000D4849 File Offset: 0x000D2A49
		protected internal override void OnTick(float dt)
		{
			this.et += dt;
			if (this.oscillate)
			{
				this.Oscillate();
			}
			if (this.bounce)
			{
				this.Bounce();
			}
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000D4878 File Offset: 0x000D2A78
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "largeObject")
			{
				this.ResetMatrix();
				this.oscillateAtX = true;
				this.oscillateAtY = true;
				this.oscillationFrequency = 1.5f;
				this.maxOscillationAngle = 7.4f;
				this.bounceX = true;
				this.bounceXFrequency = 2f;
				this.maxBounceXDistance = 0.1f;
				this.bounceY = true;
				this.bounceYFrequency = 0.2f;
				this.maxBounceYDistance = 0.5f;
				this.bounceZ = true;
				this.bounceZFrequency = 0.6f;
				this.maxBounceZDistance = 0.22f;
				this.CalculateAxis();
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				this.CalculateOscilations();
				this.CalculateOscilations();
				this.oscillate = true;
				this.bounce = true;
				return;
			}
			if (variableName == "smallObject")
			{
				this.ResetMatrix();
				this.oscillateAtX = true;
				this.oscillateAtY = true;
				this.oscillateAtZ = true;
				this.oscillationFrequency = 1f;
				this.maxOscillationAngle = 11f;
				this.bounceX = true;
				this.bounceXFrequency = 1.5f;
				this.maxBounceXDistance = 0.3f;
				this.bounceY = true;
				this.bounceYFrequency = 1.5f;
				this.maxBounceYDistance = 0.2f;
				this.bounceZ = true;
				this.bounceZFrequency = 1f;
				this.maxBounceZDistance = 0.1f;
				this.CalculateAxis();
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				this.CalculateOscilations();
				this.CalculateOscilations();
				this.oscillate = true;
				this.bounce = true;
				return;
			}
			if (variableName == "oscillateAtX" || variableName == "oscillateAtY" || variableName == "oscillateAtZ")
			{
				if (this.oscillateAtX || this.oscillateAtY || this.oscillateAtZ)
				{
					if (!this.oscillate)
					{
						if (!this.bounce)
						{
							this.SetMatrix();
						}
						this.oscillate = true;
					}
				}
				else
				{
					this.oscillate = false;
					if (!this.bounce)
					{
						this.ResetMatrix();
					}
				}
				this.CalculateAxis();
				this.CalculateOscilations();
				return;
			}
			if (variableName == "oscillationFrequency")
			{
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				return;
			}
			if (variableName == "maxOscillationAngle")
			{
				this.maxOscillationAngle = MathF.Clamp(this.maxOscillationAngle, 0f, 90f);
				this.oscillationSpeed = this.CalculateSpeed(this.oscillationFrequency, this.maxOscillationAngle, true);
				this.CalculateOscilations();
				return;
			}
			if (variableName == "bounceX" || variableName == "bounceY" || variableName == "bounceZ")
			{
				if (this.bounceX || this.bounceY || this.bounceZ)
				{
					if (!this.bounce)
					{
						if (!this.oscillate)
						{
							this.SetMatrix();
						}
						this.bounce = true;
					}
				}
				else
				{
					this.bounce = false;
					if (!this.oscillate)
					{
						this.ResetMatrix();
					}
				}
				this.CalculateBounces();
				return;
			}
			if (variableName == "bounceXFrequency")
			{
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				return;
			}
			if (variableName == "bounceYFrequency")
			{
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				return;
			}
			if (variableName == "bounceZFrequency")
			{
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				return;
			}
			if (variableName == "maxBounceXDistance")
			{
				this.bounceXSpeed = this.CalculateSpeed(this.bounceXFrequency, this.maxBounceXDistance, false);
				this.CalculateBounces();
				return;
			}
			if (variableName == "maxBounceYDistance")
			{
				this.bounceYSpeed = this.CalculateSpeed(this.bounceYFrequency, this.maxBounceYDistance, false);
				this.CalculateBounces();
				return;
			}
			if (variableName == "maxBounceZDistance")
			{
				this.bounceZSpeed = this.CalculateSpeed(this.bounceZFrequency, this.maxBounceZDistance, false);
				this.CalculateBounces();
			}
		}

		// Token: 0x040015B9 RID: 5561
		public SimpleButton largeObject;

		// Token: 0x040015BA RID: 5562
		public SimpleButton smallObject;

		// Token: 0x040015BB RID: 5563
		public bool oscillateAtX;

		// Token: 0x040015BC RID: 5564
		public bool oscillateAtY;

		// Token: 0x040015BD RID: 5565
		public bool oscillateAtZ;

		// Token: 0x040015BE RID: 5566
		public float oscillationFrequency = 1f;

		// Token: 0x040015BF RID: 5567
		public float maxOscillationAngle = 10f;

		// Token: 0x040015C0 RID: 5568
		public bool bounceX;

		// Token: 0x040015C1 RID: 5569
		public float bounceXFrequency = 14f;

		// Token: 0x040015C2 RID: 5570
		public float maxBounceXDistance = 0.3f;

		// Token: 0x040015C3 RID: 5571
		public bool bounceY;

		// Token: 0x040015C4 RID: 5572
		public float bounceYFrequency = 14f;

		// Token: 0x040015C5 RID: 5573
		public float maxBounceYDistance = 0.3f;

		// Token: 0x040015C6 RID: 5574
		public bool bounceZ;

		// Token: 0x040015C7 RID: 5575
		public float bounceZFrequency = 14f;

		// Token: 0x040015C8 RID: 5576
		public float maxBounceZDistance = 0.3f;

		// Token: 0x040015C9 RID: 5577
		private Vec3 axis;

		// Token: 0x040015CA RID: 5578
		private float oscillationSpeed = 1f;

		// Token: 0x040015CB RID: 5579
		private float oscillationPercentage = 0.5f;

		// Token: 0x040015CC RID: 5580
		private MatrixFrame resetMF;

		// Token: 0x040015CD RID: 5581
		private MatrixFrame oscillationStart;

		// Token: 0x040015CE RID: 5582
		private MatrixFrame oscillationEnd;

		// Token: 0x040015CF RID: 5583
		private bool oscillate;

		// Token: 0x040015D0 RID: 5584
		private float bounceXSpeed = 1f;

		// Token: 0x040015D1 RID: 5585
		private float bounceXPercentage = 0.5f;

		// Token: 0x040015D2 RID: 5586
		private MatrixFrame bounceXStart;

		// Token: 0x040015D3 RID: 5587
		private MatrixFrame bounceXEnd;

		// Token: 0x040015D4 RID: 5588
		private float bounceYSpeed = 1f;

		// Token: 0x040015D5 RID: 5589
		private float bounceYPercentage = 0.5f;

		// Token: 0x040015D6 RID: 5590
		private MatrixFrame bounceYStart;

		// Token: 0x040015D7 RID: 5591
		private MatrixFrame bounceYEnd;

		// Token: 0x040015D8 RID: 5592
		private float bounceZSpeed = 1f;

		// Token: 0x040015D9 RID: 5593
		private float bounceZPercentage = 0.5f;

		// Token: 0x040015DA RID: 5594
		private MatrixFrame bounceZStart;

		// Token: 0x040015DB RID: 5595
		private MatrixFrame bounceZEnd;

		// Token: 0x040015DC RID: 5596
		private bool bounce;

		// Token: 0x040015DD RID: 5597
		private float et;

		// Token: 0x040015DE RID: 5598
		private const float SPEED_MODIFIER = 1f;
	}
}
