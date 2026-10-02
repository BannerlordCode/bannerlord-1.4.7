using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000273 RID: 627
	public class BannerBearerLogic : MissionLogic
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06002313 RID: 8979 RVA: 0x0007C5C0 File Offset: 0x0007A7C0
		// (remove) Token: 0x06002314 RID: 8980 RVA: 0x0007C5F8 File Offset: 0x0007A7F8
		public event Action<Formation> OnBannerBearersUpdated;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06002315 RID: 8981 RVA: 0x0007C630 File Offset: 0x0007A830
		// (remove) Token: 0x06002316 RID: 8982 RVA: 0x0007C668 File Offset: 0x0007A868
		public event Action<Agent, bool> OnBannerBearerAgentUpdated;

		// Token: 0x06002317 RID: 8983 RVA: 0x0007C69D File Offset: 0x0007A89D
		public BannerBearerLogic()
		{
			this._bannerSearcherUpdateTimer = new BasicMissionTimer();
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0007C6DC File Offset: 0x0007A8DC
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x0007C6E4 File Offset: 0x0007A8E4
		public IMissionAgentSpawnLogic AgentSpawnLogic { get; private set; }

		// Token: 0x0600231A RID: 8986 RVA: 0x0007C6F0 File Offset: 0x0007A8F0
		public bool IsFormationBanner(Formation formation, SpawnedItemEntity spawnedItem)
		{
			if (!BannerBearerLogic.IsBannerItem(spawnedItem.WeaponCopy.Item))
			{
				return false;
			}
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(spawnedItem.GameEntity);
			return formationControllerFromBannerEntity != null && formationControllerFromBannerEntity.Formation == formation;
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0007C730 File Offset: 0x0007A930
		public bool HasBannerOnGround(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			return formationControllerFromFormation != null && formationControllerFromFormation.HasBannerOnGround();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0007C750 File Offset: 0x0007A950
		public BannerComponent GetActiveBanner(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation == null)
			{
				return null;
			}
			if (!formationControllerFromFormation.HasActiveBannerBearers())
			{
				return null;
			}
			return formationControllerFromFormation.BannerItem.BannerComponent;
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0007C780 File Offset: 0x0007A980
		public List<Agent> GetFormationBannerBearers(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation != null)
			{
				return formationControllerFromFormation.BannerBearers;
			}
			return new List<Agent>();
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0007C7A4 File Offset: 0x0007A9A4
		public ItemObject GetFormationBanner(Formation formation)
		{
			ItemObject itemObject = null;
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation != null)
			{
				itemObject = formationControllerFromFormation.BannerItem;
			}
			return itemObject;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0007C7C8 File Offset: 0x0007A9C8
		public bool IsBannerSearchingAgent(Agent agent)
		{
			if (agent.Formation != null)
			{
				BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(agent.Formation);
				if (formationControllerFromFormation != null)
				{
					return formationControllerFromFormation.IsBannerSearchingAgent(agent);
				}
			}
			return false;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0007C7F8 File Offset: 0x0007A9F8
		public int GetMissingBannerCount(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation == null || formationControllerFromFormation.BannerItem == null)
			{
				return 0;
			}
			int num = MissionGameModels.Current.BattleBannerBearersModel.GetDesiredNumberOfBannerBearersForFormation(formation) - formationControllerFromFormation.NumberOfBanners;
			if (num <= 0)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0007C83C File Offset: 0x0007AA3C
		public Formation GetFormationFromBanner(SpawnedItemEntity spawnedItem)
		{
			WeakGameEntity weakGameEntity = spawnedItem.GameEntity;
			weakGameEntity = ((!weakGameEntity.IsValid) ? spawnedItem.GameEntityWithWorldPosition.GameEntity : weakGameEntity);
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(weakGameEntity);
			if (formationControllerFromBannerEntity == null)
			{
				return null;
			}
			return formationControllerFromBannerEntity.Formation;
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0007C87C File Offset: 0x0007AA7C
		public void SetFormationBanner(Formation formation, ItemObject newBanner)
		{
			if (newBanner != null)
			{
				BannerBearerLogic.IsBannerItem(newBanner);
			}
			BannerBearerLogic.FormationBannerController formationBannerController = this.GetFormationControllerFromFormation(formation);
			if (formationBannerController != null)
			{
				if (formationBannerController.BannerItem != newBanner)
				{
					formationBannerController.SetBannerItem(newBanner);
				}
			}
			else
			{
				formationBannerController = new BannerBearerLogic.FormationBannerController(formation, newBanner, this, base.Mission);
				this._formationBannerData.Add(formation, formationBannerController);
			}
			formationBannerController.UpdateBannerBearersForDeployment();
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0007C8D8 File Offset: 0x0007AAD8
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MissionGameModels.Current.BattleBannerBearersModel.InitializeModel(this);
			this.AgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			base.Mission.OnItemPickUp += this.OnItemPickup;
			base.Mission.OnItemDrop += this.OnItemDrop;
			this._initialSpawnEquipments.Clear();
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0007C948 File Offset: 0x0007AB48
		protected override void OnEndMission()
		{
			base.OnEndMission();
			MissionGameModels.Current.BattleBannerBearersModel.FinalizeModel();
			base.Mission.OnItemPickUp -= this.OnItemPickup;
			base.Mission.OnItemDrop -= this.OnItemDrop;
			this.AgentSpawnLogic = null;
			this._isMissionEnded = true;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0007C9A6 File Offset: 0x0007ABA6
		public override void OnDeploymentFinished()
		{
			this._initialSpawnEquipments.Clear();
			this._isMissionEnded = false;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0007C9BC File Offset: 0x0007ABBC
		public override void OnMissionTick(float dt)
		{
			if (this._bannerSearcherUpdateTimer.ElapsedTime >= 3f)
			{
				foreach (BannerBearerLogic.FormationBannerController formationBannerController in this._formationBannerData.Values)
				{
					formationBannerController.UpdateBannerSearchers();
				}
				this._bannerSearcherUpdateTimer.Reset();
			}
			if (base.Mission.Mode == MissionMode.Deployment && !this._playerFormationsRequiringUpdate.IsEmpty<BannerBearerLogic.FormationBannerController>())
			{
				foreach (BannerBearerLogic.FormationBannerController formationBannerController2 in this._playerFormationsRequiringUpdate)
				{
					formationBannerController2.UpdateBannerBearersForDeployment();
				}
				this._playerFormationsRequiringUpdate.Clear();
			}
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0007CA94 File Offset: 0x0007AC94
		public void OnItemPickup(Agent agent, SpawnedItemEntity spawnedItem)
		{
			if (!BannerBearerLogic.IsBannerItem(spawnedItem.WeaponCopy.Item))
			{
				return;
			}
			WeakGameEntity gameEntity = spawnedItem.GameEntity;
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(gameEntity);
			if (formationControllerFromBannerEntity != null)
			{
				formationControllerFromBannerEntity.OnBannerEntityPickedUp(GameEntity.CreateFromWeakEntity(gameEntity), agent);
				formationControllerFromBannerEntity.UpdateAgentStats(false);
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0007CAE0 File Offset: 0x0007ACE0
		public void OnItemDrop(Agent agent, SpawnedItemEntity spawnedItem)
		{
			if (!BannerBearerLogic.IsBannerItem(spawnedItem.WeaponCopy.Item))
			{
				return;
			}
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(spawnedItem.GameEntity);
			if (formationControllerFromBannerEntity != null)
			{
				formationControllerFromBannerEntity.OnBannerEntityDropped(GameEntity.CreateFromWeakEntity(spawnedItem.GameEntity));
				formationControllerFromBannerEntity.UpdateAgentStats(false);
			}
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0007CB2B File Offset: 0x0007AD2B
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.Banner != null && agentState == AgentState.Routed)
			{
				this.RemoveBannerOfAgent(affectedAgent);
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0007CB40 File Offset: 0x0007AD40
		public override void OnAgentPanicked(Agent affectedAgent)
		{
			if (affectedAgent.Banner != null)
			{
				affectedAgent.Mission.AddTickAction(Mission.MissionTickAction.DropItem, affectedAgent, 4, 0);
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0007CB5C File Offset: 0x0007AD5C
		public void UpdateAgent(Agent agent, bool willBecomeBannerBearer)
		{
			if (willBecomeBannerBearer)
			{
				Formation formation = agent.Formation;
				BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
				ItemObject bannerItem = formationControllerFromFormation.BannerItem;
				if (agent.Banner != null)
				{
					this.RemoveBannerOfAgent(agent);
				}
				Equipment equipment = this.CreateBannerEquipmentForAgent(agent, bannerItem);
				agent.UpdateSpawnEquipmentAndRefreshVisuals(equipment);
				GameEntity gameEntity = GameEntity.CreateFromWeakEntity(agent.GetWeaponEntityFromEquipmentSlot(EquipmentIndex.ExtraWeaponSlot));
				this.AddBannerEntity(formationControllerFromFormation, gameEntity);
				formationControllerFromFormation.OnBannerEntityPickedUp(gameEntity, agent);
			}
			else if (agent.Banner != null)
			{
				this.RemoveBannerOfAgent(agent);
				agent.UpdateSpawnEquipmentAndRefreshVisuals(this._initialSpawnEquipments[agent]);
			}
			agent.ForceUpdateCachedAndFormationValues(false, false);
			agent.SetIsAIPaused(true);
			Action<Agent, bool> onBannerBearerAgentUpdated = this.OnBannerBearerAgentUpdated;
			if (onBannerBearerAgentUpdated == null)
			{
				return;
			}
			onBannerBearerAgentUpdated(agent, willBecomeBannerBearer);
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0007CC08 File Offset: 0x0007AE08
		public Agent SpawnBannerBearer(IAgentOriginBase troopOrigin, bool isPlayerSide, Formation formation, bool spawnWithHorse, bool isReinforcement, int formationTroopCount, int formationTroopIndex, bool isAlarmed, bool wieldInitialWeapons, Vec3? initialPosition, Vec2? initialDirection, string specialActionSetSuffix = null, bool useTroopClassForSpawn = false)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			ItemObject bannerItem = formationControllerFromFormation.BannerItem;
			Agent agent = base.Mission.SpawnTroop(troopOrigin, isPlayerSide, true, spawnWithHorse, isReinforcement, formationTroopCount, formationTroopIndex, isAlarmed, wieldInitialWeapons, initialPosition, initialDirection, specialActionSetSuffix, bannerItem, formationControllerFromFormation.Formation.FormationIndex, useTroopClassForSpawn);
			agent.ForceUpdateCachedAndFormationValues(false, false);
			GameEntity gameEntity = GameEntity.CreateFromWeakEntity(agent.GetWeaponEntityFromEquipmentSlot(EquipmentIndex.ExtraWeaponSlot));
			this.AddBannerEntity(formationControllerFromFormation, gameEntity);
			formationControllerFromFormation.OnBannerEntityPickedUp(gameEntity, agent);
			return agent;
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0007CC79 File Offset: 0x0007AE79
		public static bool IsBannerItem(ItemObject item)
		{
			return item != null && item.IsBannerItem && item.BannerComponent != null;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0007CC91 File Offset: 0x0007AE91
		private void AddBannerEntity(BannerBearerLogic.FormationBannerController formationBannerController, GameEntity bannerEntity)
		{
			this._bannerToFormationMap.Add(bannerEntity.Pointer, formationBannerController);
			formationBannerController.AddBannerEntity(bannerEntity);
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0007CCAC File Offset: 0x0007AEAC
		private void RemoveBannerEntity(BannerBearerLogic.FormationBannerController formationBannerController, WeakGameEntity bannerEntity)
		{
			this._bannerToFormationMap.Remove(bannerEntity.Pointer);
			formationBannerController.RemoveBannerEntity(bannerEntity);
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0007CCC8 File Offset: 0x0007AEC8
		private BannerBearerLogic.FormationBannerController GetFormationControllerFromFormation(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationBannerController;
			if (!this._formationBannerData.TryGetValue(formation, out formationBannerController))
			{
				return null;
			}
			return formationBannerController;
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0007CCE8 File Offset: 0x0007AEE8
		private BannerBearerLogic.FormationBannerController GetFormationControllerFromBannerEntity(WeakGameEntity bannerEntity)
		{
			BannerBearerLogic.FormationBannerController formationBannerController;
			if (this._bannerToFormationMap.TryGetValue(bannerEntity.Pointer, out formationBannerController))
			{
				return formationBannerController;
			}
			return null;
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0007CD10 File Offset: 0x0007AF10
		private Equipment CreateBannerEquipmentForAgent(Agent agent, ItemObject bannerItem)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			if (!this._initialSpawnEquipments.ContainsKey(agent))
			{
				this._initialSpawnEquipments[agent] = spawnEquipment;
			}
			Equipment equipment = new Equipment(spawnEquipment);
			ItemObject bannerBearerReplacementWeapon = MissionGameModels.Current.BattleBannerBearersModel.GetBannerBearerReplacementWeapon(agent.Character);
			equipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(bannerBearerReplacementWeapon, null, null, false);
			for (int i = 1; i < 4; i++)
			{
				equipment[i] = default(EquipmentElement);
			}
			equipment[EquipmentIndex.ExtraWeaponSlot] = new EquipmentElement(bannerItem, null, null, false);
			return equipment;
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0007CD9C File Offset: 0x0007AF9C
		private void RemoveBannerOfAgent(Agent agent)
		{
			WeakGameEntity weaponEntityFromEquipmentSlot = agent.GetWeaponEntityFromEquipmentSlot(EquipmentIndex.ExtraWeaponSlot);
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(weaponEntityFromEquipmentSlot);
			if (formationControllerFromBannerEntity != null)
			{
				this.RemoveBannerEntity(formationControllerFromBannerEntity, weaponEntityFromEquipmentSlot);
				formationControllerFromBannerEntity.UpdateAgentStats(false);
			}
		}

		// Token: 0x04000D74 RID: 3444
		public const float DefaultBannerBearerAgentDefensiveness = 1f;

		// Token: 0x04000D75 RID: 3445
		public const float BannerSearcherUpdatePeriod = 3f;

		// Token: 0x04000D79 RID: 3449
		private readonly Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController> _bannerToFormationMap = new Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController>();

		// Token: 0x04000D7A RID: 3450
		private readonly Dictionary<Formation, BannerBearerLogic.FormationBannerController> _formationBannerData = new Dictionary<Formation, BannerBearerLogic.FormationBannerController>();

		// Token: 0x04000D7B RID: 3451
		private readonly Dictionary<Agent, Equipment> _initialSpawnEquipments = new Dictionary<Agent, Equipment>();

		// Token: 0x04000D7C RID: 3452
		private readonly BasicMissionTimer _bannerSearcherUpdateTimer;

		// Token: 0x04000D7D RID: 3453
		private readonly List<BannerBearerLogic.FormationBannerController> _playerFormationsRequiringUpdate = new List<BannerBearerLogic.FormationBannerController>();

		// Token: 0x04000D7E RID: 3454
		private bool _isMissionEnded;

		// Token: 0x0200054D RID: 1357
		private class FormationBannerController
		{
			// Token: 0x17000A56 RID: 2646
			// (get) Token: 0x06003C97 RID: 15511 RVA: 0x000F1553 File Offset: 0x000EF753
			// (set) Token: 0x06003C98 RID: 15512 RVA: 0x000F155B File Offset: 0x000EF75B
			public Formation Formation { get; private set; }

			// Token: 0x17000A57 RID: 2647
			// (get) Token: 0x06003C99 RID: 15513 RVA: 0x000F1564 File Offset: 0x000EF764
			// (set) Token: 0x06003C9A RID: 15514 RVA: 0x000F156C File Offset: 0x000EF76C
			public ItemObject BannerItem { get; private set; }

			// Token: 0x17000A58 RID: 2648
			// (get) Token: 0x06003C9B RID: 15515 RVA: 0x000F1575 File Offset: 0x000EF775
			public bool HasBanner
			{
				get
				{
					return this.BannerItem != null;
				}
			}

			// Token: 0x17000A59 RID: 2649
			// (get) Token: 0x06003C9C RID: 15516 RVA: 0x000F1580 File Offset: 0x000EF780
			public List<Agent> BannerBearers
			{
				get
				{
					return (from instance in this._bannerInstances.Values
						where instance.IsOnAgent
						select instance.BannerBearer).ToList<Agent>();
				}
			}

			// Token: 0x17000A5A RID: 2650
			// (get) Token: 0x06003C9D RID: 15517 RVA: 0x000F15E8 File Offset: 0x000EF7E8
			public List<GameEntity> BannersOnGround
			{
				get
				{
					return (from instance in this._bannerInstances.Values
						where instance.IsOnGround
						select instance.Entity).ToList<GameEntity>();
				}
			}

			// Token: 0x17000A5B RID: 2651
			// (get) Token: 0x06003C9E RID: 15518 RVA: 0x000F164D File Offset: 0x000EF84D
			public int NumberOfBannerBearers
			{
				get
				{
					return this._bannerInstances.Values.Count<BannerBearerLogic.FormationBannerController.BannerInstance>((BannerBearerLogic.FormationBannerController.BannerInstance instance) => instance.IsOnAgent);
				}
			}

			// Token: 0x17000A5C RID: 2652
			// (get) Token: 0x06003C9F RID: 15519 RVA: 0x000F167E File Offset: 0x000EF87E
			public int NumberOfBanners
			{
				get
				{
					return this._bannerInstances.Count;
				}
			}

			// Token: 0x17000A5D RID: 2653
			// (get) Token: 0x06003CA0 RID: 15520 RVA: 0x000F168B File Offset: 0x000EF88B
			public static float BannerSearchDistance
			{
				get
				{
					return 9f;
				}
			}

			// Token: 0x06003CA1 RID: 15521 RVA: 0x000F1694 File Offset: 0x000EF894
			public FormationBannerController(Formation formation, ItemObject bannerItem, BannerBearerLogic bannerLogic, Mission mission)
			{
				this.Formation = formation;
				this.Formation.OnUnitAdded += this.OnAgentAdded;
				this.Formation.OnUnitRemoved += this.OnAgentRemoved;
				this.Formation.OnBeforeMovementOrderApplied += this.OnBeforeFormationMovementOrderApplied;
				this.Formation.OnAfterArrangementOrderApplied += this.OnAfterArrangementOrderApplied;
				this._bannerInstances = new Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance>();
				this._bannerSearchers = new Dictionary<Agent, ValueTuple<GameEntity, float>>();
				this._requiresAgentStatUpdate = false;
				this._lastActiveBannerBearerCount = 0;
				this._bannerLogic = bannerLogic;
				this._mission = mission;
				this.SetBannerItem(bannerItem);
			}

			// Token: 0x06003CA2 RID: 15522 RVA: 0x000F174F File Offset: 0x000EF94F
			public void SetBannerItem(ItemObject bannerItem)
			{
				if (bannerItem != null)
				{
					BannerBearerLogic.IsBannerItem(bannerItem);
				}
				this.BannerItem = bannerItem;
			}

			// Token: 0x06003CA3 RID: 15523 RVA: 0x000F1765 File Offset: 0x000EF965
			public bool HasBannerEntity(GameEntity bannerEntity)
			{
				return bannerEntity != null && this._bannerInstances.Keys.Contains(bannerEntity.Pointer);
			}

			// Token: 0x06003CA4 RID: 15524 RVA: 0x000F1788 File Offset: 0x000EF988
			public bool HasBannerOnGround()
			{
				if (this.HasBanner)
				{
					return this._bannerInstances.Any<KeyValuePair<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance>>((KeyValuePair<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance> instance) => instance.Value.IsOnGround);
				}
				return false;
			}

			// Token: 0x06003CA5 RID: 15525 RVA: 0x000F17BE File Offset: 0x000EF9BE
			public bool HasActiveBannerBearers()
			{
				return this.GetNumberOfActiveBannerBearers() > 0;
			}

			// Token: 0x06003CA6 RID: 15526 RVA: 0x000F17C9 File Offset: 0x000EF9C9
			public bool IsBannerSearchingAgent(Agent agent)
			{
				return this._bannerSearchers.Keys.Contains(agent);
			}

			// Token: 0x06003CA7 RID: 15527 RVA: 0x000F17DC File Offset: 0x000EF9DC
			public int GetNumberOfActiveBannerBearers()
			{
				int num = 0;
				if (this.HasBanner)
				{
					BattleBannerBearersModel bannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
					num = this._bannerInstances.Values.Count<BannerBearerLogic.FormationBannerController.BannerInstance>((BannerBearerLogic.FormationBannerController.BannerInstance instance) => instance.IsOnAgent && bannerBearersModel.CanBannerBearerProvideEffectToFormation(instance.BannerBearer, this.Formation));
				}
				return num;
			}

			// Token: 0x06003CA8 RID: 15528 RVA: 0x000F182E File Offset: 0x000EFA2E
			public void UpdateAgentStats(bool forceUpdate = false)
			{
				if (forceUpdate || this._requiresAgentStatUpdate)
				{
					this.Formation.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						agent.UpdateAgentProperties();
						Agent mountAgent = agent.MountAgent;
						if (mountAgent != null)
						{
							mountAgent.UpdateAgentProperties();
						}
					}, null);
					this._requiresAgentStatUpdate = false;
				}
			}

			// Token: 0x06003CA9 RID: 15529 RVA: 0x000F1870 File Offset: 0x000EFA70
			private unsafe void RepositionFormation()
			{
				this.Formation.SetMovementOrder(*this.Formation.GetReadonlyMovementOrderReference());
				this.Formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.ForceUpdateCachedAndFormationValues(true, false);
				}, null);
				this.Formation.SetHasPendingUnitPositions(false);
			}

			// Token: 0x06003CAA RID: 15530 RVA: 0x000F18D0 File Offset: 0x000EFAD0
			public void UpdateBannerSearchers()
			{
				List<GameEntity> bannersOnGround = this.BannersOnGround;
				if (!this._bannerSearchers.IsEmpty<KeyValuePair<Agent, ValueTuple<GameEntity, float>>>())
				{
					List<Agent> list = new List<Agent>();
					using (Dictionary<Agent, ValueTuple<GameEntity, float>>.Enumerator enumerator = this._bannerSearchers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Agent, ValueTuple<GameEntity, float>> searcherTuple = enumerator.Current;
							Agent key = searcherTuple.Key;
							if (key.IsActive())
							{
								if (!bannersOnGround.Any<GameEntity>((GameEntity bannerEntity) => bannerEntity.Pointer == searcherTuple.Value.Item1.Pointer))
								{
									list.Add(key);
								}
							}
							else
							{
								list.Add(key);
							}
						}
					}
					foreach (Agent agent in list)
					{
						this.RemoveBannerSearcher(agent);
					}
				}
				using (List<GameEntity>.Enumerator enumerator3 = bannersOnGround.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						GameEntity banner = enumerator3.Current;
						bool flag = false;
						if (this._bannerSearchers.IsEmpty<KeyValuePair<Agent, ValueTuple<GameEntity, float>>>())
						{
							flag = true;
						}
						else
						{
							KeyValuePair<Agent, ValueTuple<GameEntity, float>> keyValuePair = this._bannerSearchers.FirstOrDefault<KeyValuePair<Agent, ValueTuple<GameEntity, float>>>(([TupleElementNames(new string[] { "bannerEntity", "lastDistance" })] KeyValuePair<Agent, ValueTuple<GameEntity, float>> tuple) => tuple.Value.Item1.Pointer == banner.Pointer);
							if (keyValuePair.Key == null)
							{
								flag = true;
							}
							else
							{
								Agent key2 = keyValuePair.Key;
								if (key2.IsActive())
								{
									GameEntity item = keyValuePair.Value.Item1;
									float item2 = keyValuePair.Value.Item2;
									float num = key2.Position.AsVec2.Distance(item.GlobalPosition.AsVec2);
									if (num <= item2 && num < BannerBearerLogic.FormationBannerController.BannerSearchDistance)
									{
										this._bannerSearchers[key2] = new ValueTuple<GameEntity, float>(item, num);
									}
									else
									{
										this.RemoveBannerSearcher(key2);
										flag = true;
									}
								}
								else
								{
									this.RemoveBannerSearcher(key2);
									flag = true;
								}
							}
						}
						if (flag)
						{
							float num2;
							Agent agent2 = this.FindBestSearcherForBanner(banner, out num2);
							if (agent2 != null)
							{
								this.AddBannerSearcher(agent2, banner, num2);
							}
						}
					}
				}
			}

			// Token: 0x06003CAB RID: 15531 RVA: 0x000F1B34 File Offset: 0x000EFD34
			public void UpdateBannerBearersForDeployment()
			{
				List<Agent> bannerBearers = this.BannerBearers;
				List<ValueTuple<Agent, bool>> list = new List<ValueTuple<Agent, bool>>();
				int num = 0;
				BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
				if (battleBannerBearersModel.CanFormationDeployBannerBearers(this.Formation))
				{
					num = battleBannerBearersModel.GetDesiredNumberOfBannerBearersForFormation(this.Formation);
					using (List<Agent>.Enumerator enumerator = bannerBearers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Agent agent = enumerator.Current;
							if (num > 0 && agent.Formation == this.Formation)
							{
								num--;
							}
							else
							{
								list.Add(new ValueTuple<Agent, bool>(agent, false));
							}
						}
						goto IL_00C2;
					}
				}
				foreach (Agent agent2 in bannerBearers)
				{
					list.Add(new ValueTuple<Agent, bool>(agent2, false));
				}
				IL_00C2:
				if (num > 0)
				{
					List<Agent> list2 = this.FindBannerBearableAgents(num);
					int num2 = 0;
					while (num2 < list2.Count && num > 0)
					{
						Agent agent3 = list2[num2];
						list.Add(new ValueTuple<Agent, bool>(agent3, true));
						num--;
						num2++;
					}
				}
				if (!list.IsEmpty<ValueTuple<Agent, bool>>())
				{
					BattleSideEnum side = this.Formation.Team.Side;
					this._bannerLogic.AgentSpawnLogic.GetSpawnHorses(side);
					BattleSideEnum side2 = this._mission.PlayerTeam.Side;
					foreach (ValueTuple<Agent, bool> valueTuple in list)
					{
						this._bannerLogic.UpdateAgent(valueTuple.Item1, valueTuple.Item2);
					}
				}
				this.UpdateAgentStats(false);
				this.RepositionFormation();
				Action<Formation> onBannerBearersUpdated = this._bannerLogic.OnBannerBearersUpdated;
				if (onBannerBearersUpdated == null)
				{
					return;
				}
				onBannerBearersUpdated(this.Formation);
			}

			// Token: 0x06003CAC RID: 15532 RVA: 0x000F1D1C File Offset: 0x000EFF1C
			public void AddBannerEntity(GameEntity entity)
			{
				if (!this._bannerInstances.ContainsKey(entity.Pointer))
				{
					this._bannerInstances.Add(entity.Pointer, new BannerBearerLogic.FormationBannerController.BannerInstance(null, entity, BannerBearerLogic.FormationBannerController.BannerState.Initialized));
				}
			}

			// Token: 0x06003CAD RID: 15533 RVA: 0x000F1D4A File Offset: 0x000EFF4A
			public void RemoveBannerEntity(WeakGameEntity entity)
			{
				this._bannerInstances.Remove(entity.Pointer);
				this.UpdateBannerSearchers();
				this.CheckRequiresAgentStatUpdate();
			}

			// Token: 0x06003CAE RID: 15534 RVA: 0x000F1D6B File Offset: 0x000EFF6B
			public void OnBannerEntityPickedUp(GameEntity entity, Agent agent)
			{
				this._bannerInstances[entity.Pointer] = new BannerBearerLogic.FormationBannerController.BannerInstance(agent, entity, BannerBearerLogic.FormationBannerController.BannerState.OnAgent);
				if (agent.IsAIControlled)
				{
					agent.ResetEnemyCaches();
					agent.Defensiveness = 1f;
				}
				this.UpdateBannerSearchers();
				this.CheckRequiresAgentStatUpdate();
			}

			// Token: 0x06003CAF RID: 15535 RVA: 0x000F1DAB File Offset: 0x000EFFAB
			public void OnBannerEntityDropped(GameEntity entity)
			{
				this._bannerInstances[entity.Pointer] = new BannerBearerLogic.FormationBannerController.BannerInstance(null, entity, BannerBearerLogic.FormationBannerController.BannerState.OnGround);
				this.UpdateBannerSearchers();
				this.CheckRequiresAgentStatUpdate();
			}

			// Token: 0x06003CB0 RID: 15536 RVA: 0x000F1DD2 File Offset: 0x000EFFD2
			public void OnBeforeFormationMovementOrderApplied(Formation formation, MovementOrder.MovementOrderEnum orderType)
			{
				if (formation == this.Formation)
				{
					this.UpdateBannerBearerArrangementPositions();
				}
			}

			// Token: 0x06003CB1 RID: 15537 RVA: 0x000F1DE3 File Offset: 0x000EFFE3
			public void OnAfterArrangementOrderApplied(Formation formation, ArrangementOrder.ArrangementOrderEnum orderEnum)
			{
				if (formation == this.Formation)
				{
					this.UpdateBannerBearerArrangementPositions();
				}
			}

			// Token: 0x06003CB2 RID: 15538 RVA: 0x000F1DF4 File Offset: 0x000EFFF4
			private Agent FindBestSearcherForBanner(GameEntity banner, out float distance)
			{
				distance = float.MaxValue;
				Agent agent = null;
				Vec2 asVec = banner.GlobalPosition.AsVec2;
				this._mission.GetNearbyAllyAgents(asVec, BannerBearerLogic.FormationBannerController.BannerSearchDistance, this.Formation.Team, this._nearbyAllyAgentsListCache);
				BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
				foreach (Agent agent2 in this._nearbyAllyAgentsListCache)
				{
					if (agent2.Formation == this.Formation && battleBannerBearersModel.CanAgentPickUpAnyBanner(agent2))
					{
						float num = agent2.Position.AsVec2.Distance(asVec);
						if (num < distance && !this._bannerSearchers.ContainsKey(agent2))
						{
							agent = agent2;
							distance = num;
						}
					}
				}
				return agent;
			}

			// Token: 0x06003CB3 RID: 15539 RVA: 0x000F1EDC File Offset: 0x000F00DC
			private List<Agent> FindBannerBearableAgents(int count)
			{
				List<Agent> list = new List<Agent>();
				if (count > 0)
				{
					BattleBannerBearersModel bannerBearerModel = MissionGameModels.Current.BattleBannerBearersModel;
					using (List<IFormationUnit>.Enumerator enumerator = this.Formation.UnitsWithoutLooseDetachedOnes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Agent agent2;
							if ((agent2 = enumerator.Current as Agent) != null && (agent2.Banner == null || agent2.Banner != this.BannerItem) && bannerBearerModel.CanAgentBecomeBannerBearer(agent2))
							{
								list.Add(agent2);
							}
						}
					}
					list = list.OrderByDescending<Agent, int>((Agent agent) => bannerBearerModel.GetAgentBannerBearingPriority(agent)).ToList<Agent>();
				}
				return list;
			}

			// Token: 0x06003CB4 RID: 15540 RVA: 0x000F1F9C File Offset: 0x000F019C
			private void UpdateBannerBearerArrangementPositions()
			{
				List<Agent> list = (from instance in this._bannerInstances.Values
					where instance.IsOnAgent && instance.BannerBearer.Formation == this.Formation
					select instance.BannerBearer).ToList<Agent>();
				List<FormationArrangementModel.ArrangementPosition> bannerBearerPositions = MissionGameModels.Current.FormationArrangementsModel.GetBannerBearerPositions(this.Formation, list.Count);
				if (bannerBearerPositions == null || bannerBearerPositions.IsEmpty<FormationArrangementModel.ArrangementPosition>())
				{
					return;
				}
				int i = 0;
				foreach (Agent agent in list)
				{
					if (agent != null && agent.IsAIControlled && agent.Formation == this.Formation)
					{
						int num;
						int num2;
						agent.GetFormationFileAndRankInfo(out num, out num2);
						while (i < bannerBearerPositions.Count)
						{
							FormationArrangementModel.ArrangementPosition arrangementPosition = bannerBearerPositions[i];
							int fileIndex = arrangementPosition.FileIndex;
							int rankIndex = arrangementPosition.RankIndex;
							bool flag = num == fileIndex && num2 == rankIndex;
							if (!flag)
							{
								IFormationUnit unit = this.Formation.Arrangement.GetUnit(fileIndex, rankIndex);
								Agent agent2;
								if (unit != null && (agent2 = unit as Agent) != null)
								{
									if (agent2 == agent)
									{
										flag = true;
									}
									else if (agent2 != this.Formation.Captain)
									{
										this.Formation.SwitchUnitLocations(agent, agent2);
										flag = true;
									}
								}
							}
							if (flag)
							{
								i++;
								break;
							}
							i++;
						}
					}
				}
			}

			// Token: 0x06003CB5 RID: 15541 RVA: 0x000F212C File Offset: 0x000F032C
			private void OnAgentAdded(Formation formation, Agent agent)
			{
				if (this.Formation == formation)
				{
					if (!this._bannerLogic._isMissionEnded && this._mission.Mode == MissionMode.Deployment && formation.Team.IsPlayerTeam && MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
					{
						int minimumFormationTroopCountToBearBanners = MissionGameModels.Current.BattleBannerBearersModel.GetMinimumFormationTroopCountToBearBanners();
						if (formation.CountOfUnits == minimumFormationTroopCountToBearBanners && !this._bannerLogic._playerFormationsRequiringUpdate.Contains(this))
						{
							this._bannerLogic._playerFormationsRequiringUpdate.Add(this);
							return;
						}
					}
					else
					{
						this.UpdateBannerSearchers();
					}
				}
			}

			// Token: 0x06003CB6 RID: 15542 RVA: 0x000F21C0 File Offset: 0x000F03C0
			private void OnAgentRemoved(Formation formation, Agent agent)
			{
				if (this.Formation == formation)
				{
					if (!this._bannerLogic._isMissionEnded && this._mission.Mode == MissionMode.Deployment && formation.Team.IsPlayerTeam && MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
					{
						int minimumFormationTroopCountToBearBanners = MissionGameModels.Current.BattleBannerBearersModel.GetMinimumFormationTroopCountToBearBanners();
						if (formation.CountOfUnits == minimumFormationTroopCountToBearBanners - 1 && !this._bannerLogic._playerFormationsRequiringUpdate.Contains(this))
						{
							this._bannerLogic._playerFormationsRequiringUpdate.Add(this);
							return;
						}
					}
					else
					{
						this.UpdateBannerSearchers();
					}
				}
			}

			// Token: 0x06003CB7 RID: 15543 RVA: 0x000F2258 File Offset: 0x000F0458
			private void CheckRequiresAgentStatUpdate()
			{
				if (!this._requiresAgentStatUpdate)
				{
					int numberOfActiveBannerBearers = this.GetNumberOfActiveBannerBearers();
					if ((numberOfActiveBannerBearers > 0 && this._lastActiveBannerBearerCount == 0) || (numberOfActiveBannerBearers == 0 && this._lastActiveBannerBearerCount > 0))
					{
						this._requiresAgentStatUpdate = true;
						this._lastActiveBannerBearerCount = numberOfActiveBannerBearers;
					}
				}
			}

			// Token: 0x06003CB8 RID: 15544 RVA: 0x000F229A File Offset: 0x000F049A
			private void AddBannerSearcher(Agent searcher, GameEntity banner, float distance)
			{
				this._bannerSearchers.Add(searcher, new ValueTuple<GameEntity, float>(banner, distance));
				HumanAIComponent humanAIComponent = searcher.HumanAIComponent;
				if (humanAIComponent == null)
				{
					return;
				}
				humanAIComponent.DisablePickUpForAgentIfNeeded();
			}

			// Token: 0x06003CB9 RID: 15545 RVA: 0x000F22BF File Offset: 0x000F04BF
			private void RemoveBannerSearcher(Agent searcher)
			{
				this._bannerSearchers.Remove(searcher);
				if (searcher.IsActive())
				{
					HumanAIComponent humanAIComponent = searcher.HumanAIComponent;
					if (humanAIComponent == null)
					{
						return;
					}
					humanAIComponent.DisablePickUpForAgentIfNeeded();
				}
			}

			// Token: 0x04001DCD RID: 7629
			private int _lastActiveBannerBearerCount;

			// Token: 0x04001DCE RID: 7630
			private bool _requiresAgentStatUpdate;

			// Token: 0x04001DCF RID: 7631
			private BannerBearerLogic _bannerLogic;

			// Token: 0x04001DD0 RID: 7632
			private Mission _mission;

			// Token: 0x04001DD1 RID: 7633
			[TupleElementNames(new string[] { "bannerEntity", "lastDistance" })]
			private Dictionary<Agent, ValueTuple<GameEntity, float>> _bannerSearchers;

			// Token: 0x04001DD2 RID: 7634
			private readonly Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance> _bannerInstances;

			// Token: 0x04001DD3 RID: 7635
			private MBList<Agent> _nearbyAllyAgentsListCache = new MBList<Agent>();

			// Token: 0x020006B5 RID: 1717
			public enum BannerState
			{
				// Token: 0x0400231C RID: 8988
				Initialized,
				// Token: 0x0400231D RID: 8989
				OnAgent,
				// Token: 0x0400231E RID: 8990
				OnGround
			}

			// Token: 0x020006B6 RID: 1718
			public struct BannerInstance
			{
				// Token: 0x17000B03 RID: 2819
				// (get) Token: 0x060041FC RID: 16892 RVA: 0x000FCA5D File Offset: 0x000FAC5D
				public bool IsOnGround
				{
					get
					{
						return this.State == BannerBearerLogic.FormationBannerController.BannerState.OnGround;
					}
				}

				// Token: 0x17000B04 RID: 2820
				// (get) Token: 0x060041FD RID: 16893 RVA: 0x000FCA68 File Offset: 0x000FAC68
				public bool IsOnAgent
				{
					get
					{
						return this.State == BannerBearerLogic.FormationBannerController.BannerState.OnAgent;
					}
				}

				// Token: 0x060041FE RID: 16894 RVA: 0x000FCA73 File Offset: 0x000FAC73
				public BannerInstance(Agent bannerBearer, GameEntity entity, BannerBearerLogic.FormationBannerController.BannerState state)
				{
					this.BannerBearer = bannerBearer;
					this.Entity = entity;
					this.State = state;
				}

				// Token: 0x0400231F RID: 8991
				public readonly Agent BannerBearer;

				// Token: 0x04002320 RID: 8992
				public readonly GameEntity Entity;

				// Token: 0x04002321 RID: 8993
				private readonly BannerBearerLogic.FormationBannerController.BannerState State;
			}
		}
	}
}
