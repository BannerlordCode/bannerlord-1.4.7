using System;
using TaleWorlds.Engine;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x0200005B RID: 91
	public class SceneEditorLayer : ScreenLayer
	{
		// Token: 0x06000366 RID: 870 RVA: 0x00019D91 File Offset: 0x00017F91
		public SceneEditorLayer()
			: base("SceneEditorLayer", -100)
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00019DA0 File Offset: 0x00017FA0
		protected override void OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00019DA8 File Offset: 0x00017FA8
		protected override void Tick(float dt)
		{
			base.Tick(dt);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00019DB1 File Offset: 0x00017FB1
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00019DBC File Offset: 0x00017FBC
		protected override void RefreshGlobalOrder(ref int currentOrder)
		{
			SceneView editorSceneView = MBEditor.GetEditorSceneView();
			if (editorSceneView != null)
			{
				editorSceneView.SetRenderOrder(currentOrder);
				currentOrder++;
			}
		}
	}
}
