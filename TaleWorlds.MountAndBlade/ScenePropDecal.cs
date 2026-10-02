using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033C RID: 828
	public class ScenePropDecal : ScriptComponentBehavior
	{
		// Token: 0x06002E50 RID: 11856 RVA: 0x000B2CC8 File Offset: 0x000B0EC8
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetUpMaterial();
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000B2CD6 File Offset: 0x000B0ED6
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.SetUpMaterial();
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000B2CE4 File Offset: 0x000B0EE4
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			this.SetUpMaterial();
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000B2CF4 File Offset: 0x000B0EF4
		private void EnsureUniqueMaterial()
		{
			Material fromResource = Material.GetFromResource(this.MaterialName);
			this.UniqueMaterial = fromResource.CreateCopy();
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000B2D1C File Offset: 0x000B0F1C
		private void SetUpMaterial()
		{
			this.EnsureUniqueMaterial();
			Texture texture = Texture.CheckAndGetFromResource(this.DiffuseTexture);
			Texture texture2 = Texture.CheckAndGetFromResource(this.NormalTexture);
			Texture texture3 = Texture.CheckAndGetFromResource(this.SpecularTexture);
			Texture texture4 = Texture.CheckAndGetFromResource(this.MaskTexture);
			if (texture != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.DiffuseMap, texture);
			}
			if (texture2 != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.BumpMap, texture2);
			}
			if (texture3 != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.SpecularMap, texture3);
			}
			if (texture4 != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, texture4);
				this.UniqueMaterial.AddMaterialShaderFlag("use_areamap", false);
			}
			this.UniqueMaterial.SetAlphaTestValue(this.AlphaTestValue);
			base.GameEntity.SetMaterialForAllMeshes(this.UniqueMaterial);
			Mesh firstMesh = base.GameEntity.GetFirstMesh();
			if (firstMesh != null)
			{
				if (this.UniqueMaterial != null)
				{
					firstMesh.SetVectorArgument(this.TilingSize, this.TilingSize, this.TilingOffset, this.TilingOffset);
				}
				firstMesh.SetVectorArgument2(this.TextureSweepX, this.TextureSweepY, 0f, this.UseBaseNormals ? 1f : 0f);
			}
		}

		// Token: 0x04001262 RID: 4706
		public string DiffuseTexture;

		// Token: 0x04001263 RID: 4707
		public string NormalTexture;

		// Token: 0x04001264 RID: 4708
		public string SpecularTexture;

		// Token: 0x04001265 RID: 4709
		public string MaskTexture;

		// Token: 0x04001266 RID: 4710
		public bool UseBaseNormals;

		// Token: 0x04001267 RID: 4711
		public float TilingSize = 1f;

		// Token: 0x04001268 RID: 4712
		public float TilingOffset;

		// Token: 0x04001269 RID: 4713
		public float AlphaTestValue;

		// Token: 0x0400126A RID: 4714
		public float TextureSweepX;

		// Token: 0x0400126B RID: 4715
		public float TextureSweepY;

		// Token: 0x0400126C RID: 4716
		public string MaterialName = "deferred_decal_material";

		// Token: 0x0400126D RID: 4717
		protected Material UniqueMaterial;
	}
}
