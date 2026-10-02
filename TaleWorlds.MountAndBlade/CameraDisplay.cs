using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000329 RID: 809
	public class CameraDisplay : ScriptComponentBehavior
	{
		// Token: 0x06002DCD RID: 11725 RVA: 0x000B0F0C File Offset: 0x000AF10C
		private void BuildView()
		{
			this._sceneView = SceneView.CreateSceneView();
			this._myCamera = Camera.CreateCamera();
			this._sceneView.SetScene(base.GameEntity.Scene);
			this._sceneView.SetPostfxFromConfig();
			this._sceneView.SetRenderOption(View.ViewRenderOptions.ClearColor, false);
			this._sceneView.SetRenderOption(View.ViewRenderOptions.ClearDepth, true);
			this._sceneView.SetScale(new Vec2(0.2f, 0.2f));
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000B0F88 File Offset: 0x000AF188
		private void SetCamera()
		{
			Vec2 realScreenResolution = Screen.RealScreenResolution;
			float num = realScreenResolution.x / realScreenResolution.y;
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			this._myCamera.SetFovVertical(0.7853982f, num, 0.2f, 200f);
			this._myCamera.Frame = globalFrame;
			this._sceneView.SetCamera(this._myCamera);
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000B0FF0 File Offset: 0x000AF1F0
		private void RenderCameraFrustrum()
		{
			this._myCamera.RenderFrustrum();
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000B0FFD File Offset: 0x000AF1FD
		protected internal override void OnEditorInit()
		{
			this.BuildView();
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000B1005 File Offset: 0x000AF205
		protected internal override void OnInit()
		{
			this.BuildView();
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000B100D File Offset: 0x000AF20D
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (MBEditor.IsEntitySelected(base.GameEntity))
			{
				this.RenderCameraFrustrum();
				this._sceneView.SetEnable(true);
				return;
			}
			this._sceneView.SetEnable(false);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000B1042 File Offset: 0x000AF242
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this._sceneView = null;
			this._myCamera = null;
		}

		// Token: 0x0400120F RID: 4623
		private Camera _myCamera;

		// Token: 0x04001210 RID: 4624
		private SceneView _sceneView;

		// Token: 0x04001211 RID: 4625
		public int renderOrder;
	}
}
