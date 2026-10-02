using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000335 RID: 821
	public class MapAtmosphereProbe : ScriptComponentBehavior
	{
		// Token: 0x06002E1E RID: 11806 RVA: 0x000B20F4 File Offset: 0x000B02F4
		public float GetInfluenceAmount(Vec3 worldPosition)
		{
			return MBMath.SmoothStep(this.minRadius, this.maxRadius, worldPosition.Distance(base.GameEntity.GetGlobalFrame().origin));
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000B212C File Offset: 0x000B032C
		public MapAtmosphereProbe()
		{
			this.hideAllProbes = MapAtmosphereProbe.hideAllProbesStatic;
			if (MBEditor.IsEditModeOn)
			{
				this.innerSphereMesh = MetaMesh.GetCopy("physics_sphere_detailed", true, false);
				this.outerSphereMesh = MetaMesh.GetCopy("physics_sphere_detailed", true, false);
				this.innerSphereMesh.SetMaterial(Material.GetFromResource("light_radius_visualizer"));
				this.outerSphereMesh.SetMaterial(Material.GetFromResource("light_radius_visualizer"));
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000B21C4 File Offset: 0x000B03C4
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (this.visualizeRadius && !MapAtmosphereProbe.hideAllProbesStatic)
			{
				uint num = 16711680U;
				uint num2 = 720640U;
				if (MBEditor.IsEntitySelected(base.GameEntity))
				{
					num |= 2147483648U;
					num2 |= 2147483648U;
				}
				else
				{
					num |= 1073741824U;
					num2 |= 1073741824U;
				}
				this.innerSphereMesh.SetFactor1(num);
				this.outerSphereMesh.SetFactor1(num2);
				MatrixFrame matrixFrame;
				matrixFrame.origin = base.GameEntity.GetGlobalFrame().origin;
				matrixFrame.rotation = Mat3.Identity;
				matrixFrame.rotation.ApplyScaleLocal(this.minRadius);
				MatrixFrame matrixFrame2;
				matrixFrame2.origin = base.GameEntity.GetGlobalFrame().origin;
				matrixFrame2.rotation = Mat3.Identity;
				matrixFrame2.rotation.ApplyScaleLocal(this.maxRadius);
				this.innerSphereMesh.SetVectorArgument(this.minRadius, this.maxRadius, 0f, 0f);
				this.outerSphereMesh.SetVectorArgument(this.minRadius, this.maxRadius, 0f, 0f);
				MBEditor.RenderEditorMesh(this.innerSphereMesh, matrixFrame);
				MBEditor.RenderEditorMesh(this.outerSphereMesh, matrixFrame2);
			}
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000B230C File Offset: 0x000B050C
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "minRadius")
			{
				this.minRadius = MBMath.ClampFloat(this.minRadius, 0.1f, this.maxRadius);
			}
			if (variableName == "maxRadius")
			{
				this.maxRadius = MBMath.ClampFloat(this.maxRadius, this.minRadius, float.MaxValue);
			}
			if (variableName == "hideAllProbes")
			{
				MapAtmosphereProbe.hideAllProbesStatic = this.hideAllProbes;
			}
		}

		// Token: 0x04001246 RID: 4678
		public bool visualizeRadius = true;

		// Token: 0x04001247 RID: 4679
		public bool hideAllProbes = true;

		// Token: 0x04001248 RID: 4680
		public static bool hideAllProbesStatic = true;

		// Token: 0x04001249 RID: 4681
		public float minRadius = 1f;

		// Token: 0x0400124A RID: 4682
		public float maxRadius = 2f;

		// Token: 0x0400124B RID: 4683
		public float rainDensity;

		// Token: 0x0400124C RID: 4684
		public float temperature;

		// Token: 0x0400124D RID: 4685
		public string atmosphereType;

		// Token: 0x0400124E RID: 4686
		public string colorGrade;

		// Token: 0x0400124F RID: 4687
		private MetaMesh innerSphereMesh;

		// Token: 0x04001250 RID: 4688
		private MetaMesh outerSphereMesh;
	}
}
