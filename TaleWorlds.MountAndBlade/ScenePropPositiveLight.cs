using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033E RID: 830
	public class ScenePropPositiveLight : ScriptComponentBehavior
	{
		// Token: 0x06002E5B RID: 11867 RVA: 0x000B2F36 File Offset: 0x000B1136
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.SetMeshParams();
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000B2F45 File Offset: 0x000B1145
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetMeshParams();
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000B2F54 File Offset: 0x000B1154
		private void SetMeshParams()
		{
			MetaMesh metaMesh = base.GameEntity.GetMetaMesh(0);
			if (metaMesh != null)
			{
				uint num = this.CalculateFactor(new Vec3(this.DirectLightRed, this.DirectLightGreen, this.DirectLightBlue, -1f), this.DirectLightIntensity);
				metaMesh.SetFactor1Linear(num);
				uint num2 = this.CalculateFactor(new Vec3(this.AmbientLightRed, this.AmbientLightGreen, this.AmbientLightBlue, -1f), this.AmbientLightIntensity);
				metaMesh.SetFactor2Linear(num2);
				metaMesh.SetVectorArgument(this.Flatness_X, this.Flatness_Y, this.Flatness_Z, 1f);
			}
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000B2FF8 File Offset: 0x000B11F8
		private uint CalculateFactor(Vec3 color, float alpha)
		{
			float num = 10f;
			byte maxValue = byte.MaxValue;
			alpha = MathF.Min(MathF.Max(0f, alpha), num);
			return ((uint)(alpha / num * (float)maxValue) << 24) + ((uint)(color.x * (float)maxValue) << 16) + ((uint)(color.y * (float)maxValue) << 8) + (uint)(color.z * (float)maxValue);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000B3054 File Offset: 0x000B1254
		protected internal override bool IsOnlyVisual()
		{
			return true;
		}

		// Token: 0x04001273 RID: 4723
		public float Flatness_X;

		// Token: 0x04001274 RID: 4724
		public float Flatness_Y;

		// Token: 0x04001275 RID: 4725
		public float Flatness_Z;

		// Token: 0x04001276 RID: 4726
		public float DirectLightRed = 1f;

		// Token: 0x04001277 RID: 4727
		public float DirectLightGreen = 1f;

		// Token: 0x04001278 RID: 4728
		public float DirectLightBlue = 1f;

		// Token: 0x04001279 RID: 4729
		public float DirectLightIntensity = 1f;

		// Token: 0x0400127A RID: 4730
		public float AmbientLightRed;

		// Token: 0x0400127B RID: 4731
		public float AmbientLightGreen;

		// Token: 0x0400127C RID: 4732
		public float AmbientLightBlue = 1f;

		// Token: 0x0400127D RID: 4733
		public float AmbientLightIntensity = 1f;
	}
}
