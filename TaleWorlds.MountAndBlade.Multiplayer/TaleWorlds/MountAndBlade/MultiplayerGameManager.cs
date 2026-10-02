using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000020 RID: 32
	public class MultiplayerGameManager : MBGameManager
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x00007A4F File Offset: 0x00005C4F
		public MultiplayerGameManager()
		{
			MBMusicManager mbmusicManager = MBMusicManager.Current;
			if (mbmusicManager == null)
			{
				return;
			}
			mbmusicManager.PauseMusicManagerSystem();
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007A68 File Offset: 0x00005C68
		protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			nextStep = GameManagerLoadingSteps.None;
			switch (gameManagerLoadingStep)
			{
			case GameManagerLoadingSteps.PreInitializeZerothStep:
				nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
				return;
			case GameManagerLoadingSteps.FirstInitializeFirstStep:
				MBGameManager.LoadModuleData(false);
				MBDebug.Print("Game creating...", 0, Debug.DebugColor.White, 17592186044416UL);
				MBGlobals.InitializeReferences();
				Game.CreateGame(new MultiplayerGame(), this).DoLoading();
				nextStep = GameManagerLoadingSteps.WaitSecondStep;
				return;
			case GameManagerLoadingSteps.WaitSecondStep:
				MBGameManager.StartNewGame();
				nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
				return;
			case GameManagerLoadingSteps.SecondInitializeThirdState:
				nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.PostInitializeFourthState : GameManagerLoadingSteps.SecondInitializeThirdState);
				return;
			case GameManagerLoadingSteps.PostInitializeFourthState:
			{
				bool flag = true;
				foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
				{
					flag = flag && mbsubModuleBase.DoLoading(Game.Current);
				}
				nextStep = (flag ? GameManagerLoadingSteps.FinishLoadingFifthStep : GameManagerLoadingSteps.PostInitializeFourthState);
				return;
			}
			case GameManagerLoadingSteps.FinishLoadingFifthStep:
				nextStep = GameManagerLoadingSteps.None;
				return;
			default:
				return;
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007B54 File Offset: 0x00005D54
		public override void OnLoadFinished()
		{
			base.OnLoadFinished();
			MBGlobals.InitializeReferences();
			GameState gameState;
			if (GameNetwork.IsDedicatedServer)
			{
				DedicatedServerType dedicatedServerType = Module.CurrentModule.StartupInfo.DedicatedServerType;
				gameState = Game.Current.GameStateManager.CreateState<UnspecifiedDedicatedServerState>();
				Utilities.SetFrameLimiterWithSleep(true);
			}
			else
			{
				gameState = Game.Current.GameStateManager.CreateState<LobbyState>();
			}
			Game.Current.GameStateManager.CleanAndPushState(gameState, 0);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007BBE File Offset: 0x00005DBE
		public override void OnAfterCampaignStart(Game game)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				MultiplayerMain.InitializeAsDedicatedServer(new GameNetworkHandler());
				return;
			}
			MultiplayerMain.Initialize(new GameNetworkHandler());
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007BDC File Offset: 0x00005DDC
		public override void OnNewCampaignStart(Game game, object starterObject)
		{
			foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.CollectSubModules())
			{
				mbsubModuleBase.OnMultiplayerGameStart(game, starterObject);
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00007C34 File Offset: 0x00005E34
		public override void OnSessionInvitationAccepted(SessionInvitationType sessionInvitationType)
		{
			if (sessionInvitationType == SessionInvitationType.Multiplayer)
			{
				return;
			}
			base.OnSessionInvitationAccepted(sessionInvitationType);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007C42 File Offset: 0x00005E42
		public override void OnPlatformRequestedMultiplayer()
		{
		}
	}
}
