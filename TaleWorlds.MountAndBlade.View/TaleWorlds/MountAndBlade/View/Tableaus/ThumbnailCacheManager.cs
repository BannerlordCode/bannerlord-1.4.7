using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000039 RID: 57
	public class ThumbnailCacheManager
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000E51E File Offset: 0x0000C71E
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000E525 File Offset: 0x0000C725
		public static ThumbnailCacheManager Current { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000E52D File Offset: 0x0000C72D
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000E535 File Offset: 0x0000C735
		public MatrixFrame InventorySceneCameraFrame { get; private set; }

		// Token: 0x06000200 RID: 512 RVA: 0x0000E540 File Offset: 0x0000C740
		private void InitializeThumbnailCreator()
		{
			this._thumbnailCreatorView = ThumbnailCreatorView.CreateThumbnailCreatorView();
			ThumbnailCreatorView.renderCallback = (ThumbnailCreatorView.OnThumbnailRenderCompleteDelegate)Delegate.Combine(ThumbnailCreatorView.renderCallback, new ThumbnailCreatorView.OnThumbnailRenderCompleteDelegate(this.OnThumbnailRenderComplete));
			foreach (Scene scene in BannerlordTableauManager.TableauCharacterScenes)
			{
				this._thumbnailCreatorView.RegisterScene(scene, true);
			}
			SceneInitializationData sceneInitializationData = new SceneInitializationData(true);
			sceneInitializationData.InitPhysicsWorld = false;
			sceneInitializationData.DoNotUseLoadingScreen = true;
			this._inventoryScene = Scene.CreateNewScene(true, false, DecalAtlasGroup.Battle, "mono_renderscene");
			this._inventoryScene.Read("inventory_character_scene", ref sceneInitializationData, "");
			this._inventoryScene.SetShadow(true);
			this._inventoryScene.DisableStaticShadows(true);
			this.InventorySceneCameraFrame = this._inventoryScene.FindEntityWithTag("camera_instance").GetGlobalFrame();
			this._inventorySceneAgentRenderer = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._inventoryScene);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000E623 File Offset: 0x0000C823
		public bool IsCachedInventoryTableauSceneUsed()
		{
			return this._inventorySceneBeingUsed;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000E62B File Offset: 0x0000C82B
		public Scene GetCachedInventoryTableauScene()
		{
			this._inventorySceneBeingUsed = true;
			return this._inventoryScene;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000E63A File Offset: 0x0000C83A
		public void ReturnCachedInventoryTableauScene()
		{
			this._inventorySceneBeingUsed = false;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000E643 File Offset: 0x0000C843
		public bool IsCachedMapConversationTableauSceneUsed()
		{
			return this._mapConversationSceneBeingUsed;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000E64B File Offset: 0x0000C84B
		public Scene GetCachedMapConversationTableauScene()
		{
			this._mapConversationSceneBeingUsed = true;
			return this._mapConversationScene;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000E65A File Offset: 0x0000C85A
		public void ReturnCachedMapConversationTableauScene()
		{
			this._mapConversationSceneBeingUsed = false;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000E663 File Offset: 0x0000C863
		public static int GetNumberOfPendingRequests()
		{
			if (ThumbnailCacheManager.Current != null)
			{
				return ThumbnailCacheManager.Current._thumbnailCreatorView.GetNumberOfPendingRequests();
			}
			return 0;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000E67D File Offset: 0x0000C87D
		public static bool IsNativeMemoryCleared()
		{
			return ThumbnailCacheManager.Current != null && ThumbnailCacheManager.Current._thumbnailCreatorView.IsMemoryCleared();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000E697 File Offset: 0x0000C897
		public static void InitializeManager()
		{
			ThumbnailCacheManager.Current = new ThumbnailCacheManager();
			ThumbnailCacheManager.Current._thumbnailCaches = new List<IThumbnailCache>();
			ThumbnailCacheManager.Current.InitializeThumbnailCreator();
			ThumbnailCacheManager.Current._heroSilhouetteTexture = Texture.GetFromResource("hero_silhouette");
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000E6D0 File Offset: 0x0000C8D0
		public void RegisterThumbnailCache(IThumbnailCache thumbnailCache)
		{
			if (this._thumbnailCaches.Contains(thumbnailCache))
			{
				Debug.FailedAssert("Thumbnail cache already registered: " + thumbnailCache.GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\ThumbnailCacheManager.cs", "RegisterThumbnailCache", 139);
				return;
			}
			this._thumbnailCaches.Add(thumbnailCache);
			thumbnailCache.Initialize(this._thumbnailCreatorView);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000E730 File Offset: 0x0000C930
		public void UnregisterThumbnailCache(IThumbnailCache thumbnailCache)
		{
			if (!this._thumbnailCaches.Contains(thumbnailCache))
			{
				Debug.FailedAssert("Trying to remove a thumbnail cache that is not registered: " + thumbnailCache.GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\ThumbnailCacheManager.cs", "UnregisterThumbnailCache", 152);
				return;
			}
			this._thumbnailCaches.Remove(thumbnailCache);
			thumbnailCache.Destroy();
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000E788 File Offset: 0x0000C988
		public static void InitializeSandboxValues()
		{
			SceneInitializationData sceneInitializationData = new SceneInitializationData(true);
			sceneInitializationData.InitPhysicsWorld = false;
			sceneInitializationData.InitSkyboxFromStart = false;
			ThumbnailCacheManager.Current._mapConversationScene = Scene.CreateNewScene(true, false, DecalAtlasGroup.All, "mono_renderscene");
			ThumbnailCacheManager.Current._mapConversationScene.SetName("MapConversationTableau");
			ThumbnailCacheManager.Current._mapConversationScene.DisableStaticShadows(true);
			ThumbnailCacheManager.Current._mapConversationScene.Read("scn_conversation_tableau", ref sceneInitializationData, "");
			ThumbnailCacheManager.Current._mapConversationScene.SetShadow(true);
			ThumbnailCacheManager.Current._mapConversationSceneAgentRenderer = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(ThumbnailCacheManager.Current._mapConversationScene);
			Utilities.LoadVirtualTextureTileset("WorldMap");
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000E838 File Offset: 0x0000CA38
		public static void ReleaseSandboxValues()
		{
			MBAgentRendererSceneController.DestructAgentRendererSceneController(ThumbnailCacheManager.Current._mapConversationScene, ThumbnailCacheManager.Current._mapConversationSceneAgentRenderer, false);
			ThumbnailCacheManager.Current._mapConversationSceneAgentRenderer = null;
			ThumbnailCacheManager.Current._mapConversationScene.ClearAll();
			ThumbnailCacheManager.Current._mapConversationScene.ManualInvalidate();
			ThumbnailCacheManager.Current._mapConversationScene = null;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000E894 File Offset: 0x0000CA94
		public static void ClearManager()
		{
			Debug.Print("ThumbnailCacheManager::ClearManager", 0, Debug.DebugColor.White, 17592186044416UL);
			if (ThumbnailCacheManager.Current != null)
			{
				for (int i = 0; i < ThumbnailCacheManager.Current._thumbnailCaches.Count; i++)
				{
					ThumbnailCacheManager.Current._thumbnailCaches[i].Destroy();
				}
				ThumbnailCacheManager.Current._thumbnailCaches.Clear();
				ThumbnailCacheManager.Current._thumbnailCaches = null;
				MBAgentRendererSceneController.DestructAgentRendererSceneController(ThumbnailCacheManager.Current._inventoryScene, ThumbnailCacheManager.Current._inventorySceneAgentRenderer, true);
				Scene inventoryScene = ThumbnailCacheManager.Current._inventoryScene;
				if (inventoryScene != null)
				{
					inventoryScene.ManualInvalidate();
				}
				ThumbnailCacheManager.Current._inventoryScene = null;
				ThumbnailCreatorView.renderCallback = (ThumbnailCreatorView.OnThumbnailRenderCompleteDelegate)Delegate.Remove(ThumbnailCreatorView.renderCallback, new ThumbnailCreatorView.OnThumbnailRenderCompleteDelegate(ThumbnailCacheManager.Current.OnThumbnailRenderComplete));
				ThumbnailCacheManager.Current._thumbnailCreatorView.ClearRequests();
				ThumbnailCacheManager.Current._thumbnailCreatorView.ManualInvalidate();
				ThumbnailCacheManager.Current._thumbnailCreatorView = null;
				ThumbnailCacheManager.Current = null;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000E998 File Offset: 0x0000CB98
		private void OnThumbnailRenderComplete(string renderId, Texture renderTarget)
		{
			Texture texture = null;
			for (int i = 0; i < this._thumbnailCaches.Count; i++)
			{
				IThumbnailCache thumbnailCache = this._thumbnailCaches[i];
				if (thumbnailCache.GetValue(renderId, out texture) && texture == null)
				{
					thumbnailCache.Add(renderId, renderTarget);
				}
			}
			bool flag = false;
			for (int j = 0; j < this._thumbnailCaches.Count; j++)
			{
				flag = flag || this._thumbnailCaches[j].OnThumbnailRenderCompleted(renderId, renderTarget);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000EA20 File Offset: 0x0000CC20
		public TextureCreationInfo CreateTexture(ThumbnailCreationData thumbnailCreationData)
		{
			TextureCreationInfo textureCreationInfo = default(TextureCreationInfo);
			for (int i = 0; i < this._thumbnailCaches.Count; i++)
			{
				TextureCreationInfo textureCreationInfo2 = this._thumbnailCaches[i].CreateTexture(thumbnailCreationData);
				if (textureCreationInfo2.IsValid)
				{
					if (textureCreationInfo.IsValid && textureCreationInfo2.IsValid)
					{
						Debug.FailedAssert("Creating thumbnails in more than one caches: " + thumbnailCreationData.RenderId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\Tableaus\\ThumbnailCacheManager.cs", "CreateTexture", 253);
					}
					textureCreationInfo = textureCreationInfo2;
				}
			}
			return textureCreationInfo;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000EAA0 File Offset: 0x0000CCA0
		public bool DestroyTexture(ThumbnailCreationData thumbnailCreationData)
		{
			bool flag = false;
			for (int i = 0; i < this._thumbnailCaches.Count; i++)
			{
				if (this._thumbnailCaches[i].ReleaseTexture(thumbnailCreationData))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000EADC File Offset: 0x0000CCDC
		public void ForceClearAllCache(bool releaseImmediately)
		{
			for (int i = 0; i < this._thumbnailCaches.Count; i++)
			{
				this._thumbnailCaches[i].Clear(releaseImmediately);
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000EB11 File Offset: 0x0000CD11
		public Texture GetCachedHeroSilhouetteTexture()
		{
			return this._heroSilhouetteTexture;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000EB1C File Offset: 0x0000CD1C
		public void ClearUnusedCache()
		{
			for (int i = 0; i < this._thumbnailCaches.Count; i++)
			{
				this._thumbnailCaches[i].ClearUnusedCache();
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000EB50 File Offset: 0x0000CD50
		public void Tick(float dt)
		{
			for (int i = 0; i < this._thumbnailCaches.Count; i++)
			{
				this._thumbnailCaches[i].Tick(dt);
			}
		}

		// Token: 0x0400012A RID: 298
		private ThumbnailCreatorView _thumbnailCreatorView;

		// Token: 0x0400012B RID: 299
		private Scene _inventoryScene;

		// Token: 0x0400012C RID: 300
		private bool _inventorySceneBeingUsed;

		// Token: 0x0400012D RID: 301
		private MBAgentRendererSceneController _inventorySceneAgentRenderer;

		// Token: 0x0400012E RID: 302
		private Scene _mapConversationScene;

		// Token: 0x0400012F RID: 303
		private bool _mapConversationSceneBeingUsed;

		// Token: 0x04000130 RID: 304
		private MBAgentRendererSceneController _mapConversationSceneAgentRenderer;

		// Token: 0x04000132 RID: 306
		private List<IThumbnailCache> _thumbnailCaches;

		// Token: 0x04000133 RID: 307
		private Texture _heroSilhouetteTexture;
	}
}
