using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.SceneNotification;
using TaleWorlds.MountAndBlade.View.Scripts;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.SceneNotification
{
	// Token: 0x0200002A RID: 42
	public class GauntletSceneNotification : GlobalLayer
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000980A File Offset: 0x00007A0A
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00009811 File Offset: 0x00007A11
		public static GauntletSceneNotification Current { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00009819 File Offset: 0x00007A19
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00009824 File Offset: 0x00007A24
		private GauntletSceneNotification()
		{
			this._dataSource = new SceneNotificationVM(new Action(this.OnPositiveAction), new Action(this.CloseNotification), new Func<string>(this.GetContinueKeyText));
			this._notificationQueue = new Queue<GauntletSceneNotification.SceneNotificationQueueItem>();
			this._contextProviders = new List<ISceneNotificationContextProvider>();
			this._gauntletLayer = new GauntletLayer("SceneNotification", 19600, false);
			this._gauntletLayer.LoadMovie("SceneNotification", this._dataSource);
			base.Layer = this._gauntletLayer;
			MBInformationManager.OnShowSceneNotification += this.OnShowSceneNotification;
			MBInformationManager.OnHideSceneNotification += this.OnHideSceneNotification;
			MBInformationManager.IsAnySceneNotificationActive += this.IsAnySceneNotifiationActive;
			this._gauntletLayer.GamepadNavigationContext.GainNavigationAfterFrames(2, null);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000098FA File Offset: 0x00007AFA
		private bool IsAnySceneNotifiationActive()
		{
			return this._isActive;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009902 File Offset: 0x00007B02
		public static void Initialize()
		{
			if (GauntletSceneNotification.Current == null)
			{
				GauntletSceneNotification.Current = new GauntletSceneNotification();
				ScreenManager.AddGlobalLayer(GauntletSceneNotification.Current, false);
				ScreenManager.SetSuspendLayer(GauntletSceneNotification.Current.Layer, true);
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009930 File Offset: 0x00007B30
		private void OnHideSceneNotification()
		{
			this.CloseNotification();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009938 File Offset: 0x00007B38
		private void OnShowSceneNotification(SceneNotificationData campaignNotification)
		{
			this._notificationQueue.Enqueue(new GauntletSceneNotification.SceneNotificationQueueItem
			{
				Data = campaignNotification,
				FramesUntilDisplay = 2
			});
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009958 File Offset: 0x00007B58
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._isActive && (MBGameManager.Current == null || Game.Current == null || MBGameManager.Current == null || MBGameManager.Current.IsEnding))
			{
				this._notificationQueue.Clear();
				this.CloseNotification();
				return;
			}
			if (this._dataSource != null)
			{
				SceneNotificationVM dataSource = this._dataSource;
				PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
				dataSource.EndProgress = ((cameraPathScript != null) ? cameraPathScript.GetCameraFade() : 0f);
				PopupSceneCameraPath cameraPathScript2 = this._cameraPathScript;
				if (cameraPathScript2 != null)
				{
					cameraPathScript2.SetIsReady(this._dataSource.IsReady);
				}
				if (this._dataSource.IsReady && this._scene != null)
				{
					this._scene.WaitWaterRendererCPUSimulation();
					this._scene.Tick(dt);
				}
			}
			if (this._isPendingSceneLoad)
			{
				if (this._isActive)
				{
					this.OpenScene();
					base.Layer.IsFocusLayer = true;
					ScreenManager.TrySetFocus(base.Layer);
					base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				}
				else
				{
					Debug.FailedAssert("Scene load was pending but scene notification is not active", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "OnTick", 121);
				}
				this._isPendingSceneLoad = false;
			}
			this.QueueTick();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009A84 File Offset: 0x00007C84
		private void QueueTick()
		{
			if (!this._isActive && this._notificationQueue.Count > 0)
			{
				GauntletSceneNotification.SceneNotificationQueueItem sceneNotificationQueueItem = this._notificationQueue.Peek();
				if (sceneNotificationQueueItem.FramesUntilDisplay > 0)
				{
					sceneNotificationQueueItem.FramesUntilDisplay--;
					return;
				}
				SceneNotificationData.RelevantContextType relevantContext = sceneNotificationQueueItem.Data.RelevantContext;
				if (this.IsGivenContextApplicableToCurrentContext(relevantContext))
				{
					GauntletSceneNotification.SceneNotificationQueueItem sceneNotificationQueueItem2 = this._notificationQueue.Dequeue();
					this.CreateSceneNotification(sceneNotificationQueueItem2.Data);
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009AF8 File Offset: 0x00007CF8
		private void OnPositiveAction()
		{
			PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
			if (cameraPathScript != null)
			{
				cameraPathScript.SetPositiveState();
			}
			foreach (PopupSceneSpawnPoint popupSceneSpawnPoint in this._sceneCharacterScripts)
			{
				popupSceneSpawnPoint.SetPositiveState();
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009B5C File Offset: 0x00007D5C
		private void OpenScene()
		{
			SceneNotificationData activeData = this._dataSource.ActiveData;
			SceneNotificationData.SceneNotificationCharacter[] sceneNotificationCharacters = activeData.GetSceneNotificationCharacters();
			Banner[] banners = activeData.GetBanners();
			SceneNotificationData.SceneNotificationShip[] ships = activeData.GetShips();
			this._scene = Scene.CreateNewScene(true, true, DecalAtlasGroup.Battle, "mono_renderscene");
			this._scene.SetUsesDeleteLaterSystem(true);
			SceneInitializationData sceneInitializationData = new SceneInitializationData(true)
			{
				InitPhysicsWorld = activeData.SceneProperties.InitializePhysics
			};
			if (sceneInitializationData.InitPhysicsWorld)
			{
				this._scene.EnableInclusiveAsyncPhysx();
			}
			this._scene.Read(activeData.SceneID, ref sceneInitializationData, "");
			if (sceneInitializationData.InitPhysicsWorld)
			{
				this._scene.EnableFixedTick();
				this._scene.SetFixedTickCallbackActive(true);
			}
			this._scene.DisableStaticShadows(activeData.SceneProperties.DisableStaticShadows);
			SceneNotificationData.NotificationSceneProperties notificationSceneProperties = activeData.SceneProperties;
			if (notificationSceneProperties.OverriddenWaterStrength != null)
			{
				Scene scene = this._scene;
				notificationSceneProperties = activeData.SceneProperties;
				scene.SetWaterStrength(notificationSceneProperties.OverriddenWaterStrength.Value);
			}
			this._scene.SetClothSimulationState(true);
			this._scene.SetShadow(true);
			this._scene.SetDynamicShadowmapCascadesRadiusMultiplier(0.1f);
			this._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(this._scene);
			this._agentRendererSceneController.SetEnforcedVisibilityForAllAgents(this._scene);
			this._sceneCharacterScripts = new List<PopupSceneSpawnPoint>();
			this._customPrefabBannerEntities = new Dictionary<string, GameEntity>();
			GameEntity firstEntityWithScriptComponent = this._scene.GetFirstEntityWithScriptComponent<PopupSceneCameraPath>();
			this._cameraPathScript = ((firstEntityWithScriptComponent != null) ? firstEntityWithScriptComponent.GetFirstScriptOfType<PopupSceneCameraPath>() : null);
			PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
			if (cameraPathScript != null)
			{
				cameraPathScript.Initialize();
			}
			PopupSceneCameraPath cameraPathScript2 = this._cameraPathScript;
			if (cameraPathScript2 != null)
			{
				cameraPathScript2.SetInitialState();
			}
			if (sceneNotificationCharacters != null)
			{
				int num = 1;
				foreach (SceneNotificationData.SceneNotificationCharacter sceneNotificationCharacter in sceneNotificationCharacters)
				{
					BasicCharacterObject character = sceneNotificationCharacter.Character;
					if (character == null)
					{
						num++;
					}
					else
					{
						string text = "spawnpoint_player_" + num.ToString();
						GameEntity gameEntity = this._scene.FindEntitiesWithTag(text).ToList<GameEntity>().FirstOrDefault<GameEntity>();
						if (gameEntity == null)
						{
							num++;
						}
						else
						{
							PopupSceneSpawnPoint firstScriptOfType = gameEntity.GetFirstScriptOfType<PopupSceneSpawnPoint>();
							MatrixFrame frame = gameEntity.GetFrame();
							Equipment equipment = character.FirstBattleEquipment;
							if (sceneNotificationCharacter.OverriddenEquipment != null)
							{
								equipment = sceneNotificationCharacter.OverriddenEquipment;
							}
							else if (sceneNotificationCharacter.UseCivilianEquipment)
							{
								equipment = character.FirstCivilianEquipment;
							}
							BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
							if (sceneNotificationCharacter.OverriddenBodyProperties != default(BodyProperties))
							{
								bodyProperties = sceneNotificationCharacter.OverriddenBodyProperties;
							}
							uint num2 = character.Culture.Color;
							uint num3 = character.Culture.Color2;
							if (sceneNotificationCharacter.CustomColor1 != 4294967295U)
							{
								num2 = sceneNotificationCharacter.CustomColor1;
							}
							if (sceneNotificationCharacter.CustomColor2 != 4294967295U)
							{
								num3 = sceneNotificationCharacter.CustomColor2;
							}
							Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(character.Race);
							AgentVisuals agentVisuals = AgentVisuals.Create(new AgentVisualsData().UseMorphAnims(true).Equipment(equipment).Race(character.Race)
								.BodyProperties(bodyProperties)
								.SkeletonType(character.IsFemale ? SkeletonType.Female : SkeletonType.Male)
								.Frame(frame)
								.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, character.IsFemale, "_facegen"))
								.Scene(this._scene)
								.Monster(baseMonsterFromRace)
								.PrepareImmediately(true)
								.UseTranslucency(true)
								.UseTesselation(true)
								.ClothColor1(num2)
								.ClothColor2(num3), "notification_agent_visuals_" + num, false, false, false);
							AgentVisuals agentVisuals2 = null;
							if (sceneNotificationCharacter.UseHorse)
							{
								ItemObject item = equipment[EquipmentIndex.ArmorItemEndSlot].Item;
								string randomMountKeyString = MountCreationKey.GetRandomMountKeyString(item, character.GetMountKeySeed());
								MBActionSet actionSet = MBGlobals.GetActionSet(item.HorseComponent.Monster.ActionSetCode);
								agentVisuals2 = AgentVisuals.Create(new AgentVisualsData().Equipment(equipment).Frame(frame).ActionSet(actionSet)
									.Scene(this._scene)
									.Monster(item.HorseComponent.Monster)
									.Scale(item.ScaleFactor)
									.PrepareImmediately(true)
									.UseTranslucency(true)
									.UseTesselation(true)
									.MountCreationKey(randomMountKeyString), "notification_mount_visuals_" + num, false, false, false);
							}
							firstScriptOfType.InitializeWithAgentVisuals(agentVisuals, agentVisuals2);
							agentVisuals.SetAgentLodZeroOrMaxExternal(true);
							if (agentVisuals2 != null)
							{
								agentVisuals2.SetAgentLodZeroOrMaxExternal(true);
							}
							firstScriptOfType.SetInitialState();
							this._sceneCharacterScripts.Add(firstScriptOfType);
							if (!string.IsNullOrEmpty(firstScriptOfType.BannerTagToUseForAddedPrefab) && firstScriptOfType.AddedPrefabComponent != null)
							{
								this._customPrefabBannerEntities.Add(firstScriptOfType.BannerTagToUseForAddedPrefab, GameEntity.CreateFromWeakEntity(firstScriptOfType.AddedPrefabComponent.GetEntity()));
							}
							num++;
						}
					}
				}
			}
			if (banners != null)
			{
				for (int j = 0; j < banners.Length; j++)
				{
					Banner banner = banners[j];
					string text2 = "banner_" + (j + 1).ToString();
					GameEntity bannerEntity = this._scene.FindEntityWithTag(text2);
					if (bannerEntity != null)
					{
						BannerVisual bannerVisual = (BannerVisual)banner.BannerVisual;
						BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
						bannerVisual.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture t)
						{
							this.OnBannerTableauRenderDone(bannerEntity, t);
						}, true);
					}
					else
					{
						GameEntity entity;
						if (this._customPrefabBannerEntities.TryGetValue(text2, out entity))
						{
							BannerVisual bannerVisual2 = (BannerVisual)banner.BannerVisual;
							BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
							bannerVisual2.GetTableauTextureLarge(in bannerDebugInfo, delegate(Texture t)
							{
								this.OnBannerTableauRenderDone(entity, t);
							}, true);
						}
					}
				}
			}
			if (ships != null)
			{
				int num4 = 1;
				foreach (SceneNotificationData.SceneNotificationShip sceneNotificationShip in ships)
				{
					if (string.IsNullOrEmpty(sceneNotificationShip.ShipPrefabId))
					{
						num4++;
						Debug.FailedAssert("Scene notification ship does not have a valid prefab", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "OpenScene", 366);
					}
					else
					{
						string text3 = "spawnpoint_ship_" + num4.ToString();
						GameEntity gameEntity2 = this._scene.FindEntityWithTag(text3);
						if (gameEntity2 == null)
						{
							Debug.FailedAssert("Ship spawn point entity with tag: " + text3 + " was not found", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "OpenScene", 374);
							num4++;
						}
						else
						{
							List<GameEntity> list = gameEntity2.GetChildren().ToList<GameEntity>();
							for (int l = 0; l < list.Count; l++)
							{
								GameEntity gameEntity3 = list[l];
								this._scene.RemoveEntity(gameEntity3, 62);
							}
							gameEntity2.GetFirstScriptOfType<PopupSceneShipSpawnPoint>();
							GameEntity gameEntity4 = VisualShipFactory.CreateVisualShip(sceneNotificationShip.ShipPrefabId, this._scene, sceneNotificationShip.ShipUpgrades, sceneNotificationShip.ShipSeed, sceneNotificationShip.ShipHitPointRatio, sceneNotificationShip.SailColor1, sceneNotificationShip.SailColor2, true, true);
							gameEntity2.AddChild(gameEntity4, false);
							num4++;
						}
					}
				}
			}
			this._dataSource.Scene = this._scene;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000A27C File Offset: 0x0000847C
		private void OnBannerTableauRenderDone(GameEntity bannerEntity, Texture bannerTexture)
		{
			if (bannerEntity != null)
			{
				foreach (Mesh mesh in bannerEntity.GetAllMeshesWithTag("banner_replacement_mesh"))
				{
					this.ApplyBannerTextureToMesh(mesh, bannerTexture);
				}
				Skeleton skeleton = bannerEntity.Skeleton;
				if (((skeleton != null) ? skeleton.GetAllMeshes() : null) != null)
				{
					Skeleton skeleton2 = bannerEntity.Skeleton;
					foreach (Mesh mesh2 in ((skeleton2 != null) ? skeleton2.GetAllMeshes() : null))
					{
						if (mesh2.HasTag("banner_replacement_mesh"))
						{
							this.ApplyBannerTextureToMesh(mesh2, bannerTexture);
						}
					}
				}
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000A348 File Offset: 0x00008548
		private void ApplyBannerTextureToMesh(Mesh bannerMesh, Texture bannerTexture)
		{
			if (bannerMesh != null)
			{
				Material material = bannerMesh.GetMaterial().CreateCopy();
				material.SetTexture(Material.MBTextureType.DiffuseMap2, bannerTexture);
				uint num = (uint)material.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
				ulong shaderFlags = material.GetShaderFlags();
				material.SetShaderFlags(shaderFlags | (ulong)num);
				bannerMesh.SetMaterial(material);
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000A3A0 File Offset: 0x000085A0
		private void CreateSceneNotification(SceneNotificationData data)
		{
			if (this._isActive)
			{
				Debug.FailedAssert("Trying to create scene notification while another notification is playing", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\SceneNotification\\GauntletSceneNotification.cs", "CreateSceneNotification", 443);
				return;
			}
			this._isActive = true;
			this._dataSource.CreateNotification(data);
			ScreenManager.SetSuspendLayer(base.Layer, false);
			this._isLastActiveGameStatePaused = data.PauseActiveState;
			if (this._isLastActiveGameStatePaused)
			{
				GameStateManager.Current.RegisterActiveStateDisableRequest(this);
				MBCommon.PauseGameEngine();
			}
			this._dataSource.EndProgress = 0f;
			this._isPendingSceneLoad = true;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000A42C File Offset: 0x0000862C
		private void CloseNotification()
		{
			if (!this._isActive)
			{
				return;
			}
			this._dataSource.ClearData();
			this._isActive = false;
			base.Layer.InputRestrictions.ResetInputRestrictions();
			ScreenManager.SetSuspendLayer(base.Layer, true);
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			if (this._isLastActiveGameStatePaused)
			{
				GameStateManager gameStateManager = GameStateManager.Current;
				if (gameStateManager != null)
				{
					gameStateManager.UnregisterActiveStateDisableRequest(this);
				}
				MBCommon.UnPauseGameEngine();
			}
			PopupSceneCameraPath cameraPathScript = this._cameraPathScript;
			if (cameraPathScript != null)
			{
				cameraPathScript.Destroy();
			}
			if (this._sceneCharacterScripts != null)
			{
				foreach (PopupSceneSpawnPoint popupSceneSpawnPoint in this._sceneCharacterScripts)
				{
					popupSceneSpawnPoint.Destroy();
				}
				this._sceneCharacterScripts = null;
			}
			MBAgentRendererSceneController.DestructAgentRendererSceneController(this._scene, this._agentRendererSceneController, false);
			this._scene.ClearAll();
			this._scene.ManualInvalidate();
			this._scene = null;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000A538 File Offset: 0x00008738
		private string GetContinueKeyText()
		{
			Module currentModule = Module.CurrentModule;
			GameTextManager gameTextManager = ((currentModule != null) ? currentModule.GlobalTextManager : null);
			if (gameTextManager == null)
			{
				return string.Empty;
			}
			if (Input.IsGamepadActive)
			{
				return gameTextManager.FindText("str_click_to_continue_console", null).SetTextVariable("CONSOLE_KEY_NAME", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("ConversationHotKeyCategory", "ContinueClick"), 1f)).ToString();
			}
			return gameTextManager.FindText("str_click_to_continue", null).ToString();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000A5AD File Offset: 0x000087AD
		public void OnFinalize()
		{
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000A5C1 File Offset: 0x000087C1
		public void RegisterContextProvider(ISceneNotificationContextProvider provider)
		{
			this._contextProviders.Add(provider);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000A5CF File Offset: 0x000087CF
		public bool RemoveContextProvider(ISceneNotificationContextProvider provider)
		{
			return this._contextProviders.Remove(provider);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000A5E0 File Offset: 0x000087E0
		private bool IsGivenContextApplicableToCurrentContext(SceneNotificationData.RelevantContextType givenContextType)
		{
			if (LoadingWindow.IsLoadingWindowActive)
			{
				return false;
			}
			if (givenContextType == SceneNotificationData.RelevantContextType.Any)
			{
				return true;
			}
			for (int i = 0; i < this._contextProviders.Count; i++)
			{
				ISceneNotificationContextProvider sceneNotificationContextProvider = this._contextProviders[i];
				if (sceneNotificationContextProvider != null && !sceneNotificationContextProvider.IsContextAllowed(givenContextType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040000DD RID: 221
		private readonly GauntletLayer _gauntletLayer;

		// Token: 0x040000DE RID: 222
		private readonly Queue<GauntletSceneNotification.SceneNotificationQueueItem> _notificationQueue;

		// Token: 0x040000DF RID: 223
		private readonly List<ISceneNotificationContextProvider> _contextProviders;

		// Token: 0x040000E0 RID: 224
		private SceneNotificationVM _dataSource;

		// Token: 0x040000E1 RID: 225
		private bool _isActive;

		// Token: 0x040000E2 RID: 226
		private bool _isLastActiveGameStatePaused;

		// Token: 0x040000E3 RID: 227
		private bool _isPendingSceneLoad;

		// Token: 0x040000E4 RID: 228
		private Scene _scene;

		// Token: 0x040000E5 RID: 229
		private MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x040000E6 RID: 230
		private List<PopupSceneSpawnPoint> _sceneCharacterScripts;

		// Token: 0x040000E7 RID: 231
		private PopupSceneCameraPath _cameraPathScript;

		// Token: 0x040000E8 RID: 232
		private Dictionary<string, GameEntity> _customPrefabBannerEntities;

		// Token: 0x02000053 RID: 83
		private class SceneNotificationQueueItem
		{
			// Token: 0x040001C7 RID: 455
			public SceneNotificationData Data;

			// Token: 0x040001C8 RID: 456
			public int FramesUntilDisplay;
		}
	}
}
