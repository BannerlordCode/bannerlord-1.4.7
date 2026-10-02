using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000382 RID: 898
	public class WindMill : ScriptComponentBehavior
	{
		// Token: 0x060033B8 RID: 13240 RVA: 0x000D4DE7 File Offset: 0x000D2FE7
		protected internal override void OnInit()
		{
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x000D4DF8 File Offset: 0x000D2FF8
		private void Rotate(float dt)
		{
			float num = this.rotationSpeed * 0.001f * dt;
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.rotation.RotateAboutForward(num);
			base.GameEntity.SetLocalFrame(ref frame, false);
			base.GameEntity.UpdateTriadFrameForEditorForAllChildren();
			this.currentRotation += num;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x000D4E5D File Offset: 0x000D305D
		private static bool IsRotationPhaseInsidePhaseBoundaries(float currentPhase, float startPhase, float endPhase)
		{
			if (endPhase <= startPhase)
			{
				return currentPhase > startPhase;
			}
			return currentPhase > startPhase && currentPhase < endPhase;
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x000D4E74 File Offset: 0x000D3074
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

		// Token: 0x060033BC RID: 13244 RVA: 0x000D4EC0 File Offset: 0x000D30C0
		private void DoWaterMillCalculation()
		{
			float num = (float)base.GameEntity.ChildCount;
			if (num > 0f)
			{
				IEnumerable<WeakGameEntity> children = base.GameEntity.GetChildren();
				float num2 = 6.28f / num;
				foreach (WeakGameEntity weakGameEntity in children)
				{
					int integerFromStringEnd = WindMill.GetIntegerFromStringEnd(weakGameEntity.Name);
					float num3 = this.currentRotation % 6.28f;
					float num4 = (num2 * (float)integerFromStringEnd + this.waterSplashPhaseOffset) % 6.28f;
					float num5 = (num4 + num2 * this.waterSplashIntervalMultiplier) % 6.28f;
					if (WindMill.IsRotationPhaseInsidePhaseBoundaries(num3, num4, num5))
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

		// Token: 0x060033BD RID: 13245 RVA: 0x000D4F90 File Offset: 0x000D3190
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.TickParallel;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x000D4F9A File Offset: 0x000D319A
		protected internal override void OnTickParallel(float dt)
		{
			this.Rotate(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x000D4FB1 File Offset: 0x000D31B1
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.Rotate(dt);
			if (this.isWaterMill)
			{
				this.DoWaterMillCalculation();
			}
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000D4FD0 File Offset: 0x000D31D0
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "testMesh")
			{
				if (this.testMesh != null)
				{
					base.GameEntity.AddMultiMesh(this.testMesh, true);
					return;
				}
			}
			else if (variableName == "testTexture")
			{
				if (this.testTexture != null)
				{
					Material material = base.GameEntity.GetFirstMesh().GetMaterial().CreateCopy();
					material.SetTexture(Material.MBTextureType.DiffuseMap, this.testTexture);
					base.GameEntity.SetMaterialForAllMeshes(material);
					return;
				}
			}
			else
			{
				if (variableName == "testEntity")
				{
					this.testEntity != null;
					return;
				}
				if (variableName == "testButton")
				{
					this.rotationSpeed *= 2f;
				}
			}
		}

		// Token: 0x040015DF RID: 5599
		public float rotationSpeed = 100f;

		// Token: 0x040015E0 RID: 5600
		public float waterSplashPhaseOffset;

		// Token: 0x040015E1 RID: 5601
		public float waterSplashIntervalMultiplier = 1f;

		// Token: 0x040015E2 RID: 5602
		public MetaMesh testMesh;

		// Token: 0x040015E3 RID: 5603
		public Texture testTexture;

		// Token: 0x040015E4 RID: 5604
		public GameEntity testEntity;

		// Token: 0x040015E5 RID: 5605
		public bool isWaterMill;

		// Token: 0x040015E6 RID: 5606
		private float currentRotation;
	}
}
