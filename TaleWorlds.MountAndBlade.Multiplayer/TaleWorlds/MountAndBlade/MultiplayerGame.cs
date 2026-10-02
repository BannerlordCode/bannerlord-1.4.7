using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200001F RID: 31
	public class MultiplayerGame : GameType
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00007713 File Offset: 0x00005913
		public override bool IsCoreOnlyGameMode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00007716 File Offset: 0x00005916
		public static MultiplayerGame Current
		{
			get
			{
				return Game.Current.GameType as MultiplayerGame;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00007727 File Offset: 0x00005927
		public override bool RequiresTutorial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007734 File Offset: 0x00005934
		protected override void OnInitialize()
		{
			Game currentGame = base.CurrentGame;
			IGameStarter gameStarter = new BasicGameStarter();
			this.AddGameModels(gameStarter);
			base.GameManager.InitializeGameStarter(currentGame, gameStarter);
			base.GameManager.OnGameStart(base.CurrentGame, gameStarter);
			currentGame.SetBasicModels(gameStarter.Models);
			currentGame.CreateGameManager();
			base.GameManager.BeginGameStart(base.CurrentGame);
			currentGame.InitializeDefaultGameObjects();
			if (!GameNetwork.IsDedicatedServer)
			{
				currentGame.GameTextManager.LoadGameTexts();
			}
			currentGame.LoadBasicFiles();
			base.ObjectManager.LoadXML("Items", false);
			base.ObjectManager.LoadXML("MPCharacters", false);
			base.ObjectManager.LoadXML("BasicCultures", false);
			base.ObjectManager.LoadXML("MPClassDivisions", false);
			base.ObjectManager.UnregisterNonReadyObjects();
			MultiplayerClassDivisions.Initialize();
			BadgeManager.InitializeWithXML(ModuleHelper.GetModuleFullPath("Native") + "ModuleData/mpbadges.xml");
			base.GameManager.OnNewCampaignStart(base.CurrentGame, null);
			base.GameManager.OnAfterCampaignStart(base.CurrentGame);
			base.GameManager.OnGameInitializationFinished(base.CurrentGame);
			base.CurrentGame.AddGameHandler<ChatBox>();
			if (GameNetwork.IsDedicatedServer)
			{
				base.CurrentGame.AddGameHandler<MultiplayerGameLogger>();
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007878 File Offset: 0x00005A78
		private void AddGameModels(IGameStarter basicGameStarter)
		{
			basicGameStarter.AddModel<RidingModel>(new MultiplayerRidingModel());
			basicGameStarter.AddModel<StrikeMagnitudeCalculationModel>(new MultiplayerStrikeMagnitudeModel());
			basicGameStarter.AddModel<AgentStatCalculateModel>(new MultiplayerAgentStatCalculateModel());
			basicGameStarter.AddModel<AgentApplyDamageModel>(new MultiplayerAgentApplyDamageModel());
			basicGameStarter.AddModel<BattleMoraleModel>(new MultiplayerBattleMoraleModel());
			basicGameStarter.AddModel<BattleInitializationModel>(new MultiplayerBattleInitializationModel());
			basicGameStarter.AddModel<BattleSpawnModel>(new MultiplayerBattleSpawnModel());
			basicGameStarter.AddModel<BattleBannerBearersModel>(new MultiplayerBattleBannerBearersModel());
			basicGameStarter.AddModel<FormationArrangementModel>(new DefaultFormationArrangementModel());
			basicGameStarter.AddModel<AgentDecideKilledOrUnconsciousModel>(new DefaultAgentDecideKilledOrUnconsciousModel());
			basicGameStarter.AddModel<DamageParticleModel>(new DefaultDamageParticleModel());
			basicGameStarter.AddModel<ItemPickupModel>(new DefaultItemPickupModel());
			basicGameStarter.AddModel<MissionSiegeEngineCalculationModel>(new DefaultSiegeEngineCalculationModel());
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007914 File Offset: 0x00005B14
		public static Dictionary<string, Equipment> ReadDefaultEquipments(string defaultEquipmentsPath)
		{
			Dictionary<string, Equipment> dictionary = new Dictionary<string, Equipment>();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(defaultEquipmentsPath);
			foreach (object obj in xmlDocument.ChildNodes[0].ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					string value = xmlNode.Attributes["name"].Value;
					Equipment equipment = new Equipment(Equipment.EquipmentType.Battle);
					equipment.Deserialize(null, xmlNode);
					dictionary.Add(value, equipment);
				}
			}
			return dictionary;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000079C0 File Offset: 0x00005BC0
		protected override void BeforeRegisterTypes(MBObjectManager objectManager)
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000079C2 File Offset: 0x00005BC2
		protected override void OnRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<BasicCharacterObject>("NPCCharacter", "MPCharacters", 43U, true, false);
			objectManager.RegisterType<BasicCultureObject>("Culture", "BasicCultures", 17U, true, false);
			objectManager.RegisterType<MultiplayerClassDivisions.MPHeroClass>("MPClassDivision", "MPClassDivisions", 45U, true, false);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007A00 File Offset: 0x00005C00
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

		// Token: 0x060001A5 RID: 421 RVA: 0x00007A32 File Offset: 0x00005C32
		public override void OnDestroy()
		{
			BadgeManager.OnFinalize();
			MultiplayerOptions.Release();
			InformationManager.ClearAllMessages();
			MultiplayerClassDivisions.Release();
			AvatarServices.ClearAvatarCaches();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007A4D File Offset: 0x00005C4D
		public override void OnStateChanged(GameState oldState)
		{
		}
	}
}
