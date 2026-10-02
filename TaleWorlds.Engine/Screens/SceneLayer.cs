using System;
using System.Numerics;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine.Screens
{
	// Token: 0x020000A2 RID: 162
	public class SceneLayer : ScreenLayer
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00011BC8 File Offset: 0x0000FDC8
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x00011BD0 File Offset: 0x0000FDD0
		public bool ClearSceneOnFinalize { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x00011BD9 File Offset: 0x0000FDD9
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x00011BE1 File Offset: 0x0000FDE1
		public bool AutoToggleSceneView { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00011BEA File Offset: 0x0000FDEA
		public SceneView SceneView
		{
			get
			{
				return this._sceneView;
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00011BF2 File Offset: 0x0000FDF2
		public SceneLayer(bool clearSceneOnFinalize = true, bool autoToggleSceneView = true)
			: base("SceneLayer", -100)
		{
			this.ClearSceneOnFinalize = clearSceneOnFinalize;
			base.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
			this._sceneView = SceneView.CreateSceneView();
			this.AutoToggleSceneView = autoToggleSceneView;
			base.IsFocusLayer = true;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00011C2E File Offset: 0x0000FE2E
		protected override void OnActivate()
		{
			base.OnActivate();
			if (this.AutoToggleSceneView)
			{
				this._sceneView.SetEnable(true);
			}
			ScreenManager.TrySetFocus(this);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00011C50 File Offset: 0x0000FE50
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			if (this.AutoToggleSceneView)
			{
				this._sceneView.SetEnable(false);
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00011C6C File Offset: 0x0000FE6C
		protected override void OnFinalize()
		{
			if (this.ClearSceneOnFinalize)
			{
				this._sceneView.ClearAll(true, true);
			}
			base.OnFinalize();
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00011C89 File Offset: 0x0000FE89
		public void SetScene(Scene scene)
		{
			this._sceneView.SetScene(scene);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00011C97 File Offset: 0x0000FE97
		public void SetRenderWithPostfx(bool value)
		{
			this._sceneView.SetRenderWithPostfx(value);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00011CA5 File Offset: 0x0000FEA5
		public void SetPostfxConfigParams(int value)
		{
			this._sceneView.SetPostfxConfigParams(value);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00011CB3 File Offset: 0x0000FEB3
		public void SetCamera(Camera camera)
		{
			this._sceneView.SetCamera(camera);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00011CC1 File Offset: 0x0000FEC1
		public void SetPostfxFromConfig()
		{
			this._sceneView.SetPostfxFromConfig();
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00011CCE File Offset: 0x0000FECE
		public Vec2 WorldPointToScreenPoint(Vec3 position)
		{
			return this._sceneView.WorldPointToScreenPoint(position);
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00011CDC File Offset: 0x0000FEDC
		public Vec2 ScreenPointToViewportPoint(Vec2 position)
		{
			return this._sceneView.ScreenPointToViewportPoint(position);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00011CEA File Offset: 0x0000FEEA
		public bool ProjectedMousePositionOnGround(out Vec3 groundPosition, out Vec3 groundNormal, bool mouseVisible, BodyFlags excludeBodyOwnerFlags, bool checkOccludedSurface)
		{
			return this._sceneView.ProjectedMousePositionOnGround(out groundPosition, out groundNormal, mouseVisible, excludeBodyOwnerFlags, checkOccludedSurface);
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00011CFE File Offset: 0x0000FEFE
		public void TranslateMouse(ref Vec3 worldMouseNear, ref Vec3 worldMouseFar, float maxDistance = -1f)
		{
			this._sceneView.TranslateMouse(ref worldMouseNear, ref worldMouseFar, maxDistance);
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00011D0E File Offset: 0x0000FF0E
		public void SetSceneUsesSkybox(bool value)
		{
			this._sceneView.SetSceneUsesSkybox(value);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00011D1C File Offset: 0x0000FF1C
		public void SetSceneUsesShadows(bool value)
		{
			this._sceneView.SetSceneUsesShadows(value);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00011D2A File Offset: 0x0000FF2A
		public void SetSceneUsesContour(bool value)
		{
			this._sceneView.SetSceneUsesContour(value);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00011D38 File Offset: 0x0000FF38
		public void SetShadowmapResolutionMultiplier(float value)
		{
			this._sceneView.SetShadowmapResolutionMultiplier(value);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00011D46 File Offset: 0x0000FF46
		public void SetFocusedShadowmap(bool enable, ref Vec3 center, float radius)
		{
			this._sceneView.SetFocusedShadowmap(enable, ref center, radius);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00011D56 File Offset: 0x0000FF56
		public void DoNotClear(bool value)
		{
			this._sceneView.DoNotClear(value);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00011D64 File Offset: 0x0000FF64
		public bool ReadyToRender()
		{
			return this._sceneView.ReadyToRender();
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00011D71 File Offset: 0x0000FF71
		public void SetCleanScreenUntilLoadingDone(bool value)
		{
			this._sceneView.SetCleanScreenUntilLoadingDone(value);
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00011D7F File Offset: 0x0000FF7F
		public void ClearAll()
		{
			this._sceneView.ClearAll(true, true);
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00011D8E File Offset: 0x0000FF8E
		public void ClearRuntimeGPUMemory(bool remove_terrain)
		{
			this._sceneView.ClearAll(false, remove_terrain);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00011D9D File Offset: 0x0000FF9D
		protected override void RefreshGlobalOrder(ref int currentOrder)
		{
			this._sceneView.SetRenderOrder(currentOrder);
			currentOrder++;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00011DB4 File Offset: 0x0000FFB4
		public override bool HitTest(Vector2 position)
		{
			bool flag = position.X >= 0f && position.X < Screen.RealScreenResolutionWidth;
			bool flag2 = position.Y >= 0f && position.Y < Screen.RealScreenResolutionHeight;
			return flag && flag2;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00011E00 File Offset: 0x00010000
		public override bool HitTest()
		{
			Vector2 vector = (Vector2)base.Input.GetMousePositionPixel();
			bool flag = vector.X >= 0f && vector.X < Screen.RealScreenResolutionWidth;
			bool flag2 = vector.Y >= 0f && vector.Y < Screen.RealScreenResolutionHeight;
			return flag && flag2;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00011E5B File Offset: 0x0001005B
		public override bool FocusTest()
		{
			return true;
		}

		// Token: 0x04000219 RID: 537
		private SceneView _sceneView;
	}
}
