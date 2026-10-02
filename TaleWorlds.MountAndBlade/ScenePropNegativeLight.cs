using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033D RID: 829
	public class ScenePropNegativeLight : ScriptComponentBehavior
	{
		// Token: 0x06002E57 RID: 11863 RVA: 0x000B2E9A File Offset: 0x000B109A
		protected internal override void OnEditorTick(float dt)
		{
			this.SetMeshParameters();
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000B2EA4 File Offset: 0x000B10A4
		private void SetMeshParameters()
		{
			MetaMesh metaMesh = base.GameEntity.GetMetaMesh(0);
			if (metaMesh != null)
			{
				metaMesh.SetVectorArgument(this.Flatness_X, this.Flatness_Y, this.Flatness_Z, this.Alpha);
				if (this.Is_Dark_Light)
				{
					metaMesh.SetVectorArgument2(1f, 0f, 0f, 0f);
					return;
				}
				metaMesh.SetVectorArgument2(0f, 0f, 0f, 0f);
			}
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000B2F25 File Offset: 0x000B1125
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetMeshParameters();
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000B2F33 File Offset: 0x000B1133
		protected internal override bool IsOnlyVisual()
		{
			return true;
		}

		// Token: 0x0400126E RID: 4718
		public float Flatness_X;

		// Token: 0x0400126F RID: 4719
		public float Flatness_Y;

		// Token: 0x04001270 RID: 4720
		public float Flatness_Z;

		// Token: 0x04001271 RID: 4721
		public float Alpha = 1f;

		// Token: 0x04001272 RID: 4722
		public bool Is_Dark_Light = true;
	}
}
