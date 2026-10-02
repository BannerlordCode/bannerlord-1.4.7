using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200034D RID: 845
	public class PathLastNodeFixer : UsableMissionObjectComponent
	{
		// Token: 0x06002FA4 RID: 12196 RVA: 0x000BBB90 File Offset: 0x000B9D90
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			Path pathWithName = this._scene.GetPathWithName(this.PathHolder.PathEntity);
			this.Update(pathWithName);
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000BBBC2 File Offset: 0x000B9DC2
		protected internal override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			this._scene = scene;
			this.Update();
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000BBBD8 File Offset: 0x000B9DD8
		public void Update()
		{
			Path pathWithName = this._scene.GetPathWithName(this.PathHolder.PathEntity);
			this.Update(pathWithName);
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000BBC03 File Offset: 0x000B9E03
		private void Update(Path path)
		{
			path != null;
		}

		// Token: 0x0400137E RID: 4990
		public IPathHolder PathHolder;

		// Token: 0x0400137F RID: 4991
		private Scene _scene;
	}
}
