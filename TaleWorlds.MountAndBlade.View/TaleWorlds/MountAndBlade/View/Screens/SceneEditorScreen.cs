using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x0200005A RID: 90
	[GameStateScreen(typeof(EditorState))]
	public class SceneEditorScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x0600035D RID: 861 RVA: 0x00019CCB File Offset: 0x00017ECB
		public SceneEditorScreen(EditorState editorState)
		{
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00019CD4 File Offset: 0x00017ED4
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._editorLayer = new SceneEditorLayer();
			this._editorLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.Invalid);
			base.AddLayer(this._editorLayer);
			ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_core_parameters"));
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00019D29 File Offset: 0x00017F29
		protected override void OnActivate()
		{
			base.OnActivate();
			MouseManager.ActivateMouseCursor(CursorType.System);
			MBEditor.ActivateSceneEditorPresentation();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00019D3C File Offset: 0x00017F3C
		protected override void OnDeactivate()
		{
			MBEditor.DeactivateSceneEditorPresentation();
			MouseManager.ActivateMouseCursor(CursorType.Default);
			base.OnDeactivate();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00019D50 File Offset: 0x00017F50
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (this._editorLayer != null)
			{
				bool mouseVisible = Screen.GetMouseVisible();
				this._editorLayer.InputRestrictions.SetMouseVisibility(mouseVisible);
			}
			MBEditor.TickSceneEditorPresentation(dt);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00019D89 File Offset: 0x00017F89
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00019D8B File Offset: 0x00017F8B
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00019D8D File Offset: 0x00017F8D
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00019D8F File Offset: 0x00017F8F
		void IGameStateListener.OnFinalize()
		{
		}

		// Token: 0x040001CB RID: 459
		private SceneEditorLayer _editorLayer;
	}
}
