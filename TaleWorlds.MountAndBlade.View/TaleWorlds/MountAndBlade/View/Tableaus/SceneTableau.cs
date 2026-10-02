using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000037 RID: 55
	public class SceneTableau
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000E097 File Offset: 0x0000C297
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000E09F File Offset: 0x0000C29F
		public Texture _texture { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
		public bool? IsReady
		{
			get
			{
				SceneView view = this.View;
				if (view == null)
				{
					return null;
				}
				return new bool?(view.ReadyToRender());
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000E0D3 File Offset: 0x0000C2D3
		public SceneTableau()
		{
			this.SetEnabled(true);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
		private void SetEnabled(bool enabled)
		{
			this._isEnabled = enabled;
			SceneView view = this.View;
			if (view == null)
			{
				return;
			}
			view.SetEnable(this._isEnabled);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000E118 File Offset: 0x0000C318
		private void CreateTexture()
		{
			this._texture = Texture.CreateRenderTarget("SceneTableau", this._tableauSizeX, this._tableauSizeY, true, false, false, false);
			this.View = SceneView.CreateSceneView();
			this.View.SetScene(this._tableauScene);
			this.View.SetRenderTarget(this._texture);
			this.View.SetAutoDepthTargetCreation(true);
			this.View.SetSceneUsesSkybox(true);
			this.View.SetClearColor(4294902015U);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000E19C File Offset: 0x0000C39C
		public void SetTargetSize(int width, int height)
		{
			this._isRotatingCharacter = false;
			if (width <= 0 || height <= 0)
			{
				this._tableauSizeX = 10;
				this._tableauSizeY = 10;
			}
			else
			{
				this.RenderScale = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.ResolutionScale) / 100f;
				this._tableauSizeX = (int)((float)width * this.RenderScale);
				this._tableauSizeY = (int)((float)height * this.RenderScale);
			}
			this._cameraRatio = (float)this._tableauSizeX / (float)this._tableauSizeY;
			SceneView view = this.View;
			SceneView view2 = this.View;
			if (view2 != null)
			{
				view2.SetEnable(false);
			}
			SceneView view3 = this.View;
			if (view3 != null)
			{
				view3.AddClearTask(true);
			}
			this.CreateTexture();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000E244 File Offset: 0x0000C444
		public void OnFinalize()
		{
			if (this._continuousRenderCamera != null)
			{
				this._continuousRenderCamera.ReleaseCameraEntity();
				this._continuousRenderCamera = null;
				this._cameraEntity = null;
			}
			SceneView view = this.View;
			if (view != null)
			{
				view.SetEnable(false);
			}
			SceneView view2 = this.View;
			if (view2 != null)
			{
				view2.AddClearTask(false);
			}
			Texture texture = this._texture;
			if (texture != null)
			{
				texture.Release();
			}
			this._texture = null;
			this._tableauScene = null;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000E2BC File Offset: 0x0000C4BC
		public void SetScene(object scene)
		{
			Scene scene2;
			if ((scene2 = scene as Scene) != null)
			{
				this._tableauScene = scene2;
				if (this._tableauSizeX != 0 && this._tableauSizeY != 0)
				{
					this.CreateTexture();
					return;
				}
			}
			else
			{
				Debug.FailedAssert("Given scene object is not Scene type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\SceneTableau.cs", "SetScene", 120);
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000E307 File Offset: 0x0000C507
		public void SetBannerCode(string value)
		{
			this.RefreshCharacterTableau(null);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000E310 File Offset: 0x0000C510
		private void RefreshCharacterTableau(Equipment oldEquipment = null)
		{
			if (!this._initialized)
			{
				this.FirstTimeInit();
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000E320 File Offset: 0x0000C520
		public void RotateCharacter(bool value)
		{
			this._isRotatingCharacter = value;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000E32C File Offset: 0x0000C52C
		public void OnTick(float dt)
		{
			if (this._animationFrequencyThreshold > this._animationGap)
			{
				this._animationGap += dt;
			}
			if (this.View != null)
			{
				if (this._continuousRenderCamera == null)
				{
					GameEntity gameEntity = this._tableauScene.FindEntityWithTag("customcamera");
					if (gameEntity != null)
					{
						this._continuousRenderCamera = Camera.CreateCamera();
						Vec3 vec = default(Vec3);
						gameEntity.GetCameraParamsFromCameraScript(this._continuousRenderCamera, ref vec);
						this._cameraEntity = gameEntity;
					}
				}
				this.PopupSceneContinuousRenderFunction();
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000E3B9 File Offset: 0x0000C5B9
		private void FirstTimeInit()
		{
			this._initialized = true;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000E3C4 File Offset: 0x0000C5C4
		private void PopupSceneContinuousRenderFunction()
		{
			GameEntity gameEntity = this._tableauScene.FindEntityWithTag("customcamera");
			this._tableauScene.SetShadow(true);
			this._tableauScene.EnsurePostfxSystem();
			this._tableauScene.SetMotionBlurMode(true);
			this._tableauScene.SetBloom(true);
			this._tableauScene.SetDynamicShadowmapCascadesRadiusMultiplier(1f);
			this.View.SetRenderWithPostfx(true);
			this.View.SetSceneUsesShadows(true);
			this.View.SetScene(this._tableauScene);
			this.View.SetSceneUsesSkybox(true);
			this.View.SetClearColor(4278190080U);
			this.View.SetFocusedShadowmap(false, ref this._frame.origin, 1.55f);
			this.View.SetEnable(true);
			if (gameEntity != null)
			{
				Vec3 vec = default(Vec3);
				gameEntity.GetCameraParamsFromCameraScript(this._continuousRenderCamera, ref vec);
				if (this._continuousRenderCamera != null)
				{
					Camera continuousRenderCamera = this._continuousRenderCamera;
					this.View.SetCamera(continuousRenderCamera);
				}
			}
		}

		// Token: 0x04000119 RID: 281
		private float _animationFrequencyThreshold = 2.5f;

		// Token: 0x0400011A RID: 282
		private MatrixFrame _frame;

		// Token: 0x0400011B RID: 283
		private Scene _tableauScene;

		// Token: 0x0400011C RID: 284
		private Camera _continuousRenderCamera;

		// Token: 0x0400011D RID: 285
		private GameEntity _cameraEntity;

		// Token: 0x0400011E RID: 286
		private float _cameraRatio;

		// Token: 0x0400011F RID: 287
		private bool _initialized;

		// Token: 0x04000120 RID: 288
		private int _tableauSizeX;

		// Token: 0x04000121 RID: 289
		private int _tableauSizeY;

		// Token: 0x04000122 RID: 290
		private SceneView View;

		// Token: 0x04000123 RID: 291
		private bool _isRotatingCharacter;

		// Token: 0x04000124 RID: 292
		private float _animationGap;

		// Token: 0x04000125 RID: 293
		private bool _isEnabled;

		// Token: 0x04000126 RID: 294
		private float RenderScale = 1f;
	}
}
