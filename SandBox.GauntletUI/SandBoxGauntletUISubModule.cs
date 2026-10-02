using System;
using SandBox.GauntletUI.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.SceneNotification;

namespace SandBox.GauntletUI
{
	// Token: 0x02000012 RID: 18
	public class SandBoxGauntletUISubModule : MBSubModuleBase
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00007C5B File Offset: 0x00005E5B
		public SandBoxGauntletUISubModule()
		{
			this._conversationListener = new SandBoxGauntletUISubModule.SandBoxGameStateManagerListener();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007C6E File Offset: 0x00005E6E
		public override void OnCampaignStart(Game game, object starterObject)
		{
			base.OnCampaignStart(game, starterObject);
			if (!this._gameStarted && game.GameType is Campaign)
			{
				this._gameStarted = true;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007C94 File Offset: 0x00005E94
		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);
			if (!this._gameStarted && game.GameType is Campaign)
			{
				this._gameStarted = true;
				SandBoxGauntletGameNotification.Initialize();
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007CBF File Offset: 0x00005EBF
		public override void OnGameEnd(Game game)
		{
			base.OnGameEnd(game);
			if (this._gameStarted && game.GameType is Campaign)
			{
				this._gameStarted = false;
				GauntletGameNotification.Initialize();
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007CE9 File Offset: 0x00005EE9
		public override void BeginGameStart(Game game)
		{
			base.BeginGameStart(game);
			if (Campaign.Current != null)
			{
				Campaign.Current.VisualCreator.MapEventVisualCreator = new GauntletMapEventVisualCreator();
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007D10 File Offset: 0x00005F10
		protected override void OnApplicationTick(float dt)
		{
			base.OnApplicationTick(dt);
			if (!this._initializedConversationHandler)
			{
				Game game = Game.Current;
				if (((game != null) ? game.GameStateManager : null) != null)
				{
					Game.Current.GameStateManager.RegisterListener(this._conversationListener);
					this._registeredGameStateManager = Game.Current.GameStateManager;
					this._initializedConversationHandler = true;
					goto IL_008C;
				}
			}
			if (this._initializedConversationHandler)
			{
				Game game2 = Game.Current;
				if (((game2 != null) ? game2.GameStateManager : null) == null)
				{
					this._registeredGameStateManager.UnregisterListener(this._conversationListener);
					this._initializedConversationHandler = false;
					this._registeredGameStateManager = null;
				}
			}
			IL_008C:
			if (!this._initialized && GauntletSceneNotification.Current != null)
			{
				if (!Utilities.CommandLineArgumentExists("VisualTests"))
				{
					GauntletSceneNotification.Current.RegisterContextProvider(new SandboxSceneNotificationContextProvider());
				}
				this._initialized = true;
			}
		}

		// Token: 0x04000058 RID: 88
		private bool _gameStarted;

		// Token: 0x04000059 RID: 89
		private bool _initialized;

		// Token: 0x0400005A RID: 90
		private GameStateManager _registeredGameStateManager;

		// Token: 0x0400005B RID: 91
		private bool _initializedConversationHandler;

		// Token: 0x0400005C RID: 92
		private SandBoxGauntletUISubModule.SandBoxGameStateManagerListener _conversationListener;

		// Token: 0x02000055 RID: 85
		private class SandBoxGameStateManagerListener : IGameStateManagerListener
		{
			// Token: 0x060003F4 RID: 1012 RVA: 0x00017FFC File Offset: 0x000161FC
			void IGameStateManagerListener.OnCleanStates()
			{
				this.UpdateCampaignMission();
			}

			// Token: 0x060003F5 RID: 1013 RVA: 0x00018004 File Offset: 0x00016204
			void IGameStateManagerListener.OnCreateState(GameState gameState)
			{
			}

			// Token: 0x060003F6 RID: 1014 RVA: 0x00018006 File Offset: 0x00016206
			void IGameStateManagerListener.OnPopState(GameState gameState)
			{
				this.UpdateCampaignMission();
				if (gameState is MissionState || gameState is MapState)
				{
					CampaignInformationManager.ClearAllDialogNotifications(false);
				}
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x00018024 File Offset: 0x00016224
			void IGameStateManagerListener.OnPushState(GameState gameState, bool isTopGameState)
			{
				this.UpdateCampaignMission();
				if (gameState is MissionState || gameState is MapState)
				{
					CampaignInformationManager.ClearAllDialogNotifications(false);
				}
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x00018042 File Offset: 0x00016242
			void IGameStateManagerListener.OnSavedGameLoadFinished()
			{
			}

			// Token: 0x060003F9 RID: 1017 RVA: 0x00018044 File Offset: 0x00016244
			private void UpdateCampaignMission()
			{
				ICampaignMission campaignMission = CampaignMission.Current;
				if (campaignMission == null)
				{
					return;
				}
				campaignMission.OnGameStateChanged();
			}
		}
	}
}
