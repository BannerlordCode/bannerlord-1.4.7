using System;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens.Scripts
{
	// Token: 0x0200005E RID: 94
	public class MultiThreadedStressTestsScreen : ScreenBase
	{
		// Token: 0x06000386 RID: 902 RVA: 0x0001A948 File Offset: 0x00018B48
		protected override void OnActivate()
		{
			base.OnActivate();
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.All, "mono_renderscene");
			this._scene.Read("mp_ruins_2");
			this._sceneView = SceneView.CreateSceneView();
			this._sceneView.SetScene(this._scene);
			this._sceneView.SetSceneUsesShadows(true);
			Camera camera = Camera.CreateCamera();
			camera.Frame = this._scene.ReadAndCalculateInitialCamera();
			this._sceneView.SetCamera(camera);
			this._workerThreads = new List<Thread>();
			Thread thread = new Thread(delegate
			{
				MultiThreadedStressTestsScreen.MultiThreadedTestFunctions.MeshMerger(InputLayout.Input_layout_regular);
			});
			thread.Name = "StressTester|Mesh Merger Thread";
			this._workerThreads.Add(thread);
			Thread thread2 = new Thread(delegate
			{
				MultiThreadedStressTestsScreen.MultiThreadedTestFunctions.MeshMerger(InputLayout.Input_layout_normal_map);
			});
			thread2.Name = "StressTester|Mesh Merger Thread";
			this._workerThreads.Add(thread2);
			Thread thread3 = new Thread(delegate
			{
				MultiThreadedStressTestsScreen.MultiThreadedTestFunctions.MeshMerger(InputLayout.Input_layout_skinning);
			});
			thread3.Name = "StressTester|Mesh Merger Thread";
			this._workerThreads.Add(thread3);
			for (int i = 0; i < this._workerThreads.Count; i++)
			{
				this._workerThreads[i].Start();
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001AAB4 File Offset: 0x00018CB4
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this._sceneView = null;
			this._scene = null;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001AACC File Offset: 0x00018CCC
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			bool flag = true;
			for (int i = 0; i < this._workerThreads.Count; i++)
			{
				if (this._workerThreads[i].IsAlive)
				{
					flag = false;
				}
			}
			if (flag)
			{
				ScreenManager.PopScreen();
			}
		}

		// Token: 0x040001E3 RID: 483
		private List<Thread> _workerThreads;

		// Token: 0x040001E4 RID: 484
		private Scene _scene;

		// Token: 0x040001E5 RID: 485
		private SceneView _sceneView;

		// Token: 0x020000D2 RID: 210
		public static class MultiThreadedTestFunctions
		{
			// Token: 0x06000634 RID: 1588 RVA: 0x0002ABAC File Offset: 0x00028DAC
			public static void MeshMerger(InputLayout layout)
			{
				Mesh mesh = Mesh.GetRandomMeshWithVdecl((int)layout);
				mesh = mesh.CreateCopy();
				UIntPtr uintPtr = mesh.LockEditDataWrite();
				Mesh mesh2 = Mesh.GetRandomMeshWithVdecl((int)layout);
				mesh2 = mesh2.CreateCopy();
				Mesh randomMeshWithVdecl = Mesh.GetRandomMeshWithVdecl((int)layout);
				Mesh randomMeshWithVdecl2 = Mesh.GetRandomMeshWithVdecl((int)layout);
				mesh.AddMesh(randomMeshWithVdecl, MatrixFrame.Identity);
				mesh2.AddMesh(randomMeshWithVdecl2, MatrixFrame.Identity);
				mesh.AddMesh(mesh2, MatrixFrame.Identity);
				int num = mesh.AddFaceCorner(new Vec3(0f, 0f, 1f, -1f), new Vec3(0f, 0f, 1f, -1f), new Vec2(0f, 1f), 268435455U, uintPtr);
				int num2 = mesh.AddFaceCorner(new Vec3(0f, 1f, 0f, -1f), new Vec3(0f, 0f, 1f, -1f), new Vec2(1f, 0f), 268435455U, uintPtr);
				int num3 = mesh.AddFaceCorner(new Vec3(0f, 1f, 1f, -1f), new Vec3(0f, 0f, 1f, -1f), new Vec2(1f, 1f), 268435455U, uintPtr);
				mesh.AddFace(num, num2, num3, uintPtr);
				mesh.UnlockEditDataWrite(uintPtr);
			}

			// Token: 0x06000635 RID: 1589 RVA: 0x0002AD14 File Offset: 0x00028F14
			public static void SceneHandler(SceneView view)
			{
				int i = 0;
				while (i < 500)
				{
					view.SetSceneUsesShadows(false);
					view.SetRenderWithPostfx(false);
					Thread.Sleep(5000);
					view.SetSceneUsesShadows(true);
					view.SetRenderWithPostfx(true);
					Thread.Sleep(5000);
					view.SetSceneUsesContour(true);
					Thread.Sleep(5000);
				}
			}
		}
	}
}
