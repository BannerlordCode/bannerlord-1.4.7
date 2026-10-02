using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000219 RID: 537
	public class EditorGame : GameType
	{
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x0006C2FA File Offset: 0x0006A4FA
		public static EditorGame Current
		{
			get
			{
				return Game.Current.GameType as EditorGame;
			}
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0006C314 File Offset: 0x0006A514
		protected override void OnInitialize()
		{
			Game currentGame = base.CurrentGame;
			IGameStarter gameStarter = new BasicGameStarter();
			this.InitializeGameModels(gameStarter);
			base.GameManager.InitializeGameStarter(currentGame, gameStarter);
			base.GameManager.OnGameStart(base.CurrentGame, gameStarter);
			MBObjectManager objectManager = currentGame.ObjectManager;
			currentGame.SetBasicModels(gameStarter.Models);
			currentGame.CreateGameManager();
			base.GameManager.BeginGameStart(base.CurrentGame);
			currentGame.InitializeDefaultGameObjects();
			currentGame.LoadBasicFiles();
			this.LoadCustomGameXmls();
			objectManager.UnregisterNonReadyObjects();
			currentGame.SetDefaultEquipments(new Dictionary<string, Equipment>());
			objectManager.UnregisterNonReadyObjects();
			base.GameManager.OnNewCampaignStart(base.CurrentGame, null);
			base.GameManager.OnAfterCampaignStart(base.CurrentGame);
			base.GameManager.OnGameInitializationFinished(base.CurrentGame);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0006C3DC File Offset: 0x0006A5DC
		private void InitializeGameModels(IGameStarter basicGameStarter)
		{
			basicGameStarter.AddModel<AgentStatCalculateModel>(new CustomBattleAgentStatCalculateModel());
			basicGameStarter.AddModel<AgentApplyDamageModel>(new CustomAgentApplyDamageModel());
			basicGameStarter.AddModel<ApplyWeatherEffectsModel>(new CustomBattleApplyWeatherEffectsModel());
			basicGameStarter.AddModel<BattleMoraleModel>(new CustomBattleMoraleModel());
			basicGameStarter.AddModel<BattleInitializationModel>(new CustomBattleInitializationModel());
			basicGameStarter.AddModel<BattleSpawnModel>(new CustomBattleSpawnModel());
			basicGameStarter.AddModel<AgentDecideKilledOrUnconsciousModel>(new DefaultAgentDecideKilledOrUnconsciousModel());
			basicGameStarter.AddModel<RidingModel>(new DefaultRidingModel());
			basicGameStarter.AddModel<StrikeMagnitudeCalculationModel>(new DefaultStrikeMagnitudeModel());
			basicGameStarter.AddModel<BattleBannerBearersModel>(new CustomBattleBannerBearersModel());
			basicGameStarter.AddModel<FormationArrangementModel>(new DefaultFormationArrangementModel());
			basicGameStarter.AddModel<DamageParticleModel>(new DefaultDamageParticleModel());
			basicGameStarter.AddModel<ItemPickupModel>(new DefaultItemPickupModel());
			basicGameStarter.AddModel<MissionSiegeEngineCalculationModel>(new DefaultSiegeEngineCalculationModel());
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0006C484 File Offset: 0x0006A684
		private void LoadCustomGameXmls()
		{
			base.ObjectManager.LoadXML("Items", false);
			base.ObjectManager.LoadXML("EquipmentRosters", false);
			base.ObjectManager.LoadXML("NPCCharacters", false);
			base.ObjectManager.LoadXML("SPCultures", false);
			if (ModuleHelper.IsModuleActive("NavalDLC"))
			{
				base.ObjectManager.LoadXML("ShipPhysicsReferences", false);
				base.ObjectManager.LoadXML("MissionShips", false);
			}
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0006C503 File Offset: 0x0006A703
		protected override void BeforeRegisterTypes(MBObjectManager objectManager)
		{
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0006C508 File Offset: 0x0006A708
		protected override void OnRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<BasicCharacterObject>("NPCCharacter", "NPCCharacters", 43U, true, false);
			objectManager.RegisterType<BasicCultureObject>("Culture", "SPCultures", 17U, true, false);
			if (ModuleHelper.IsModuleActive("NavalDLC"))
			{
				objectManager.RegisterType<MissionShipObject>("MissionShip", "MissionShips", 57U, true, false);
				objectManager.RegisterType<ShipPhysicsReference>("ShipPhysicsReference", "ShipPhysicsReferences", 64U, true, false);
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0006C571 File Offset: 0x0006A771
		protected override void DoLoadingForGameType(GameTypeLoadingStates gameTypeLoadingState, out GameTypeLoadingStates nextState)
		{
			nextState = GameTypeLoadingStates.None;
			switch (gameTypeLoadingState)
			{
			case GameTypeLoadingStates.InitializeFirstStep:
				base.CurrentGame.Initialize();
				nextState = GameTypeLoadingStates.WaitSecondStep;
				return;
			case GameTypeLoadingStates.WaitSecondStep:
				nextState = GameTypeLoadingStates.LoadVisualsThirdState;
				return;
			case GameTypeLoadingStates.LoadVisualsThirdState:
				nextState = GameTypeLoadingStates.PostInitializeFourthState;
				break;
			case GameTypeLoadingStates.PostInitializeFourthState:
				break;
			default:
				return;
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0006C5A3 File Offset: 0x0006A7A3
		public override void OnDestroy()
		{
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0006C5A5 File Offset: 0x0006A7A5
		public override void OnStateChanged(GameState oldState)
		{
		}
	}
}
