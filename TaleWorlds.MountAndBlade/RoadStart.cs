using System;
using System.Linq.Expressions;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033B RID: 827
	public class RoadStart : ScriptComponentBehavior
	{
		// Token: 0x06002E48 RID: 11848 RVA: 0x000B2B18 File Offset: 0x000B0D18
		protected internal override void OnInit()
		{
			this.pathEntity = TaleWorlds.Engine.GameEntity.CreateEmpty(base.Scene, false, true, true);
			this.pathEntity.Name = "Road_Entity";
			this.UpdatePathMesh();
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000B2B44 File Offset: 0x000B0D44
		protected internal override void OnEditorInit()
		{
			this.OnInit();
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000B2B4C File Offset: 0x000B0D4C
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			if (this.pathEntity != null)
			{
				this.pathEntity.Remove(removeReason);
			}
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000B2B70 File Offset: 0x000B0D70
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (base.Scene.IsEntityFrameChanged(base.GameEntity.Name))
			{
				this.UpdatePathMesh();
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000B2BA8 File Offset: 0x000B0DA8
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == MBGlobals.GetMemberName<string>(Expression.Lambda<Func<string>>(Expression.Field(Expression.Constant(this, typeof(RoadStart)), fieldof(RoadStart.materialName)), Array.Empty<ParameterExpression>())))
			{
				this.UpdatePathMesh();
			}
			if (this.pathMesh != null)
			{
				this.pathMesh.SetVectorArgument2(this.textureSweepX, this.textureSweepY, 0f, 0f);
			}
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000B2C28 File Offset: 0x000B0E28
		private void UpdatePathMesh()
		{
			this.pathEntity.ClearComponents();
			this.pathMesh = MetaMesh.CreateMetaMesh(null);
			Material fromResource = Material.GetFromResource(this.materialName);
			if (fromResource != null)
			{
				this.pathMesh.SetMaterial(fromResource);
			}
			else
			{
				this.pathMesh.SetMaterial(Material.GetDefaultMaterial());
			}
			this.pathEntity.AddMultiMesh(this.pathMesh, true);
			this.pathMesh.SetVectorArgument2(this.textureSweepX, this.textureSweepY, 0f, 0f);
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000B2CB2 File Offset: 0x000B0EB2
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x0400125D RID: 4701
		public float textureSweepX;

		// Token: 0x0400125E RID: 4702
		public float textureSweepY;

		// Token: 0x0400125F RID: 4703
		public string materialName = "blood_decal_terrain_material";

		// Token: 0x04001260 RID: 4704
		private GameEntity pathEntity;

		// Token: 0x04001261 RID: 4705
		private MetaMesh pathMesh;
	}
}
