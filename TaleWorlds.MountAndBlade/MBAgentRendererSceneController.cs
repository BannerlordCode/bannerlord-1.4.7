using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019A RID: 410
	public class MBAgentRendererSceneController
	{
		// Token: 0x060015EE RID: 5614 RVA: 0x00051599 File Offset: 0x0004F799
		internal MBAgentRendererSceneController(UIntPtr pointer)
		{
			this._pointer = pointer;
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x000515A8 File Offset: 0x0004F7A8
		public void SetEnforcedVisibilityForAllAgents(Scene scene)
		{
			MBAPI.IMBAgentVisuals.SetEnforcedVisibilityForAllAgents(scene.Pointer, this._pointer);
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x000515C0 File Offset: 0x0004F7C0
		public static MBAgentRendererSceneController CreateNewAgentRendererSceneController(Scene scene)
		{
			return new MBAgentRendererSceneController(MBAPI.IMBAgentVisuals.CreateAgentRendererSceneController(scene.Pointer));
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000515D7 File Offset: 0x0004F7D7
		public void SetDoTimerBasedForcedSkeletonUpdates(bool value)
		{
			MBAPI.IMBAgentVisuals.SetDoTimerBasedForcedSkeletonUpdates(this._pointer, value);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x000515EA File Offset: 0x0004F7EA
		public static void DestructAgentRendererSceneController(Scene scene, MBAgentRendererSceneController rendererSceneController, bool deleteThisFrame)
		{
			MBAPI.IMBAgentVisuals.DestructAgentRendererSceneController(scene.Pointer, rendererSceneController._pointer, deleteThisFrame);
			rendererSceneController._pointer = UIntPtr.Zero;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0005160E File Offset: 0x0004F80E
		public static void ValidateAgentVisualsReseted(Scene scene, MBAgentRendererSceneController rendererSceneController)
		{
			MBAPI.IMBAgentVisuals.ValidateAgentVisualsReseted(scene.Pointer, rendererSceneController._pointer);
		}

		// Token: 0x04000717 RID: 1815
		private UIntPtr _pointer;
	}
}
