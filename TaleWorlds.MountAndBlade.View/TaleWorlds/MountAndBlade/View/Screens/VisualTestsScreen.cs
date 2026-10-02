using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x0200005D RID: 93
	public class VisualTestsScreen : ScreenBase
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00019F3F File Offset: 0x0001813F
		private int CamPointCount
		{
			get
			{
				return this.CamPoints.Count;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00019F4C File Offset: 0x0001814C
		public bool StartedRendering()
		{
			return this._sceneLayer.SceneView.ReadyToRender();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00019F60 File Offset: 0x00018160
		public string GetSubTestName(VisualTestsScreen.CameraPointTestType type)
		{
			if (type == VisualTestsScreen.CameraPointTestType.Albedo)
			{
				return "_albedo";
			}
			if (type == VisualTestsScreen.CameraPointTestType.Normal)
			{
				return "_normal";
			}
			if (type == VisualTestsScreen.CameraPointTestType.Specular)
			{
				return "_specular";
			}
			if (type == VisualTestsScreen.CameraPointTestType.AO)
			{
				return "_ao";
			}
			if (type == VisualTestsScreen.CameraPointTestType.OnlyAmbient)
			{
				return "_onlyambient";
			}
			if (type == VisualTestsScreen.CameraPointTestType.OnlyDirect)
			{
				return "_onlydirect";
			}
			if (type == VisualTestsScreen.CameraPointTestType.Final)
			{
				return "_final";
			}
			return "";
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00019FB7 File Offset: 0x000181B7
		public Utilities.EngineRenderDisplayMode GetRenderMode(VisualTestsScreen.CameraPointTestType type)
		{
			if (type == VisualTestsScreen.CameraPointTestType.Albedo)
			{
				return Utilities.EngineRenderDisplayMode.ShowAlbedo;
			}
			if (type == VisualTestsScreen.CameraPointTestType.Normal)
			{
				return Utilities.EngineRenderDisplayMode.ShowNormals;
			}
			if (type == VisualTestsScreen.CameraPointTestType.Specular)
			{
				return Utilities.EngineRenderDisplayMode.ShowSpecular;
			}
			if (type == VisualTestsScreen.CameraPointTestType.AO)
			{
				return Utilities.EngineRenderDisplayMode.ShowOcclusion;
			}
			if (type == VisualTestsScreen.CameraPointTestType.OnlyAmbient)
			{
				return Utilities.EngineRenderDisplayMode.ShowDisableSunLighting;
			}
			if (type == VisualTestsScreen.CameraPointTestType.OnlyDirect)
			{
				return Utilities.EngineRenderDisplayMode.ShowDisableAmbientLighting;
			}
			return Utilities.EngineRenderDisplayMode.ShowNone;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00019FE0 File Offset: 0x000181E0
		public VisualTestsScreen(bool isValidTest, NativeOptions.ConfigQuality preset, string sceneName, DateTime testTime, List<string> testTypesToCheck)
		{
			this.isValidTest_ = isValidTest;
			this.preset_ = preset;
			this.scene_name = sceneName;
			this.testTime = testTime;
			VisualTestsScreen.isSceneSuccess = true;
			this._failDirectory = string.Concat(new object[] { this._failDirectory, "/", sceneName, "_", preset });
			this.testTypesToCheck_ = testTypesToCheck;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001A0B3 File Offset: 0x000182B3
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._sceneLayer = new SceneLayer(true, true);
			base.AddLayer(this._sceneLayer);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001A0D4 File Offset: 0x000182D4
		protected override void OnActivate()
		{
			base.OnActivate();
			if (!this.isValidTest_)
			{
				this.date = this.testTime.ToString("dd-MM-yyyy hh-mmtt");
				this._pathDirectory = this._pathDirectory + this.date + "/";
				Directory.CreateDirectory(this._pathDirectory);
				this._reportFile = this._pathDirectory + "report.txt";
			}
			this.CreateScene();
			this._scene.Tick(0f);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001A159 File Offset: 0x00018359
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001A164 File Offset: 0x00018364
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			LoadingWindow.DisableGlobalLoadingWindow();
			MessageManager.EraseMessageLines();
			if (!this._sceneLayer.ReadyToRender())
			{
				return;
			}
			this.SetTestCamera();
			if (Utilities.GetNumberOfShaderCompilationsInProgress() > 0)
			{
				return;
			}
			float num = ((this._scene.GetName() == "visualtestmorph") ? 0.01f : 0f);
			this._scene.Tick(num);
			int num2 = 5;
			this.frameCounter++;
			if (this.frameCounter < num2)
			{
				return;
			}
			this.TakeScreenshotAndAnalyze();
			if (this.CurCameraIndex >= this.CamPointCount)
			{
				ScreenManager.PopScreen();
				return;
			}
			this.frameCounter = 0;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001A20C File Offset: 0x0001840C
		private void CreateScene()
		{
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.All, "mono_renderscene");
			this._scene.SetName("VisualTestScreen");
			this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._scene);
			this._scene.Read(this.scene_name);
			this._scene.SetUseConstantTime(true);
			this._scene.SetOcclusionMode(true);
			this._scene.OptimizeScene(true, true);
			this._sceneLayer.SetScene(this._scene);
			this._sceneLayer.SceneView.SetSceneUsesShadows(true);
			this._sceneLayer.SceneView.SetForceShaderCompilation(true);
			this._sceneLayer.SceneView.SetClearGbuffer(true);
			this._camera = Camera.CreateCamera();
			this.GetCameraPoints();
			MessageManager.EraseMessageLines();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001A2DD File Offset: 0x000184DD
		private bool ShouldCheckTestModeWithTag(string mode, GameEntity entity)
		{
			if (this.testTypesToCheck_.Count > 0)
			{
				return this.testTypesToCheck_.Contains(mode) && entity.HasTag(mode);
			}
			return entity.HasTag(mode);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001A30C File Offset: 0x0001850C
		private bool ShouldCheckTestMode(string mode)
		{
			return this.testTypesToCheck_.Contains(mode);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001A31C File Offset: 0x0001851C
		private void GetCameraPoints()
		{
			this.CamPoints = new List<VisualTestsScreen.CameraPoint>();
			foreach (GameEntity gameEntity in (from o in this._scene.FindEntitiesWithTag("test_camera")
				orderby o.Name
				select o).ToList<GameEntity>())
			{
				if (!gameEntity.HasTag("exclude_" + (int)this.preset_))
				{
					VisualTestsScreen.CameraPoint cameraPoint = new VisualTestsScreen.CameraPoint();
					cameraPoint.CamFrame = gameEntity.GetFrame();
					cameraPoint.CameraName = gameEntity.Name;
					HashSet<VisualTestsScreen.CameraPointTestType> hashSet = new HashSet<VisualTestsScreen.CameraPointTestType>();
					if (this.ShouldCheckTestModeWithTag("gbuffer", gameEntity))
					{
						hashSet.Add(VisualTestsScreen.CameraPointTestType.Albedo);
						hashSet.Add(VisualTestsScreen.CameraPointTestType.Normal);
						hashSet.Add(VisualTestsScreen.CameraPointTestType.Specular);
						hashSet.Add(VisualTestsScreen.CameraPointTestType.AO);
					}
					if (this.ShouldCheckTestMode("albedo"))
					{
						hashSet.Add(VisualTestsScreen.CameraPointTestType.Albedo);
					}
					if (this.ShouldCheckTestMode("normal"))
					{
						hashSet.Add(VisualTestsScreen.CameraPointTestType.Normal);
					}
					if (this.ShouldCheckTestMode("specular"))
					{
						hashSet.Add(VisualTestsScreen.CameraPointTestType.Specular);
					}
					if (this.ShouldCheckTestMode("ao"))
					{
						hashSet.Add(VisualTestsScreen.CameraPointTestType.AO);
					}
					if (this.ShouldCheckTestModeWithTag("only_ambient", gameEntity))
					{
						hashSet.Add(VisualTestsScreen.CameraPointTestType.OnlyAmbient);
					}
					foreach (VisualTestsScreen.CameraPointTestType cameraPointTestType in hashSet)
					{
						cameraPoint.TestTypes.Add(cameraPointTestType);
					}
					cameraPoint.TestTypes.Add(VisualTestsScreen.CameraPointTestType.Final);
					this.CamPoints.Add(cameraPoint);
				}
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001A500 File Offset: 0x00018700
		private void SetTestCamera()
		{
			VisualTestsScreen.CameraPoint cameraPoint = this.CamPoints[this.CurCameraIndex];
			MatrixFrame camFrame = cameraPoint.CamFrame;
			this._camera.Frame = camFrame;
			float aspectRatio = Screen.AspectRatio;
			this._camera.SetFovVertical(1.0471976f, aspectRatio, 0.1f, 500f);
			this._sceneLayer.SetCamera(this._camera);
			VisualTestsScreen.CameraPointTestType cameraPointTestType = cameraPoint.TestTypes[this.TestSubIndex];
			Utilities.SetRenderMode(this.GetRenderMode(cameraPointTestType));
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001A580 File Offset: 0x00018780
		protected override void OnFinalize()
		{
			MBDebug.Print("On finalized called for scene: " + this.scene_name, 0, Debug.DebugColor.White, 17592186044416UL);
			base.OnFinalize();
			this._sceneLayer.ClearAll();
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._scene, this._agentRendererSceneController, false);
			this._agentRendererSceneController = null;
			this._scene = null;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001A5DF File Offset: 0x000187DF
		public void Reset()
		{
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001A5E4 File Offset: 0x000187E4
		private void TakeScreenshotAndAnalyze()
		{
			VisualTestsScreen.CameraPoint cameraPoint = this.CamPoints[this.CurCameraIndex];
			VisualTestsScreen.CameraPointTestType cameraPointTestType = cameraPoint.TestTypes[this.TestSubIndex];
			this.GetRenderMode(cameraPointTestType);
			bool flag = true;
			string text;
			if (this.isValidTest_)
			{
				text = string.Concat(new string[]
				{
					this._validReadDirectory,
					this.scene_name,
					"_",
					cameraPoint.CameraName,
					"_",
					this.GetSubTestName(cameraPointTestType),
					"_preset_",
					NativeOptions.GetGFXPresetName(this.preset_),
					".bmp"
				});
			}
			else
			{
				text = string.Concat(new string[]
				{
					this._validReadDirectory,
					this.scene_name,
					"_",
					cameraPoint.CameraName,
					"_",
					this.GetSubTestName(cameraPointTestType),
					"_preset_",
					NativeOptions.GetGFXPresetName(this.preset_),
					".bmp"
				});
			}
			string text2 = string.Concat(new string[]
			{
				this.scene_name,
				"_",
				cameraPoint.CameraName,
				"_",
				this.GetSubTestName(cameraPointTestType),
				"_preset_",
				NativeOptions.GetGFXPresetName(this.preset_),
				".bmp"
			});
			string text3 = this._pathDirectory + text2;
			MBDebug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print(text3, 0, Debug.DebugColor.White, 17592186044416UL);
			if (this.isValidTest_)
			{
				Utilities.TakeScreenshot(text);
			}
			else
			{
				Utilities.TakeScreenshot(text3);
			}
			NativeOptions.GetGFXPresetName(this.preset_);
			if (!this.isValidTest_)
			{
				if (File.Exists(text))
				{
					if (!this.AnalyzeImageDifferences(text, text3))
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			if (!flag)
			{
				if (!Directory.Exists(this._failDirectory))
				{
					Directory.CreateDirectory(TestCommonBase.GetAttachmentsFolderPath());
				}
				if (!Directory.Exists(this._failDirectory))
				{
					Directory.CreateDirectory(this._failDirectory);
				}
				string text4 = this._failDirectory + "/" + cameraPoint.CameraName + this.GetSubTestName(cameraPointTestType);
				if (!Directory.Exists(text4))
				{
					Directory.CreateDirectory(text4);
				}
				string text5 = text4 + "/branch_result.bmp";
				string text6 = text4 + "/valid.bmp";
				if (File.Exists(text5))
				{
					File.Delete(text5);
				}
				if (File.Exists(text6))
				{
					File.Delete(text6);
				}
				File.Copy(text3, text5);
				if (File.Exists(text))
				{
					if (File.Exists(text6))
					{
						File.Delete(text6);
					}
					File.Copy(text, text6);
				}
				VisualTestsScreen.isSceneSuccess = false;
			}
			this.TestSubIndex++;
			if (cameraPoint.TestTypes.Count == this.TestSubIndex)
			{
				this.CurCameraIndex++;
				this.TestSubIndex = 0;
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001A8C8 File Offset: 0x00018AC8
		private bool AnalyzeImageDifferences(string path1, string path2)
		{
			byte[] array = File.ReadAllBytes(path1);
			byte[] array2 = File.ReadAllBytes(path2);
			if (array.Length != array2.Length)
			{
				return false;
			}
			float num = 0f;
			for (int i = 0; i < array.Length; i++)
			{
				float num2 = (float)array[i];
				float num3 = (float)array2[i];
				float num4 = MathF.Max(MathF.Abs(num2 - num3), 0f);
				num += num4;
			}
			num /= (float)array.Length;
			return num < 0.5f;
		}

		// Token: 0x040001CF RID: 463
		private Scene _scene;

		// Token: 0x040001D0 RID: 464
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x040001D1 RID: 465
		private Camera _camera;

		// Token: 0x040001D2 RID: 466
		private SceneLayer _sceneLayer;

		// Token: 0x040001D3 RID: 467
		private List<VisualTestsScreen.CameraPoint> CamPoints;

		// Token: 0x040001D4 RID: 468
		private DateTime testTime;

		// Token: 0x040001D5 RID: 469
		private string _validWriteDirectory = Utilities.GetVisualTestsValidatePath();

		// Token: 0x040001D6 RID: 470
		private string _validReadDirectory = Utilities.GetBasePath() + "ValidVisuals/";

		// Token: 0x040001D7 RID: 471
		private string _pathDirectory = Utilities.GetVisualTestsTestFilesPath();

		// Token: 0x040001D8 RID: 472
		private string _failDirectory = TestCommonBase.GetAttachmentsFolderPath();

		// Token: 0x040001D9 RID: 473
		private string _reportFile = "report.txt";

		// Token: 0x040001DA RID: 474
		private int CurCameraIndex;

		// Token: 0x040001DB RID: 475
		private int TestSubIndex;

		// Token: 0x040001DC RID: 476
		private bool isValidTest_ = true;

		// Token: 0x040001DD RID: 477
		private NativeOptions.ConfigQuality preset_;

		// Token: 0x040001DE RID: 478
		public static bool isSceneSuccess = true;

		// Token: 0x040001DF RID: 479
		private string date;

		// Token: 0x040001E0 RID: 480
		private string scene_name;

		// Token: 0x040001E1 RID: 481
		private int frameCounter = -200;

		// Token: 0x040001E2 RID: 482
		private List<string> testTypesToCheck_ = new List<string>();

		// Token: 0x020000CF RID: 207
		public enum CameraPointTestType
		{
			// Token: 0x0400039E RID: 926
			Final,
			// Token: 0x0400039F RID: 927
			Albedo,
			// Token: 0x040003A0 RID: 928
			Normal,
			// Token: 0x040003A1 RID: 929
			Specular,
			// Token: 0x040003A2 RID: 930
			AO,
			// Token: 0x040003A3 RID: 931
			OnlyAmbient,
			// Token: 0x040003A4 RID: 932
			OnlyDirect
		}

		// Token: 0x020000D0 RID: 208
		public class CameraPoint
		{
			// Token: 0x06000630 RID: 1584 RVA: 0x0002AB64 File Offset: 0x00028D64
			public CameraPoint()
			{
				this.TestTypes = new List<VisualTestsScreen.CameraPointTestType>();
				this.CamFrame = MatrixFrame.Identity;
				this.CameraName = "";
			}

			// Token: 0x040003A5 RID: 933
			public MatrixFrame CamFrame;

			// Token: 0x040003A6 RID: 934
			public string CameraName;

			// Token: 0x040003A7 RID: 935
			public List<VisualTestsScreen.CameraPointTestType> TestTypes;
		}
	}
}
