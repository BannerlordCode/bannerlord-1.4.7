using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038C RID: 908
	public class Team : IMissionTeam
	{
		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06003407 RID: 13319 RVA: 0x000D6498 File Offset: 0x000D4698
		// (remove) Token: 0x06003408 RID: 13320 RVA: 0x000D64D0 File Offset: 0x000D46D0
		public event Action<Team, Formation> OnFormationsChanged;

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06003409 RID: 13321 RVA: 0x000D6508 File Offset: 0x000D4708
		// (remove) Token: 0x0600340A RID: 13322 RVA: 0x000D6540 File Offset: 0x000D4740
		public event OnOrderIssuedDelegate OnOrderIssued;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x0600340B RID: 13323 RVA: 0x000D6578 File Offset: 0x000D4778
		// (remove) Token: 0x0600340C RID: 13324 RVA: 0x000D65B0 File Offset: 0x000D47B0
		public event Action<Formation> OnFormationAIActiveBehaviorChanged;

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x0600340D RID: 13325 RVA: 0x000D65E8 File Offset: 0x000D47E8
		// (remove) Token: 0x0600340E RID: 13326 RVA: 0x000D6620 File Offset: 0x000D4820
		public event Action<Team> OnFormationsChangedInDeployment;

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x000D6655 File Offset: 0x000D4855
		public BattleSideEnum Side { get; }

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06003410 RID: 13328 RVA: 0x000D665D File Offset: 0x000D485D
		public Mission Mission { get; }

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003411 RID: 13329 RVA: 0x000D6665 File Offset: 0x000D4865
		// (set) Token: 0x06003412 RID: 13330 RVA: 0x000D666D File Offset: 0x000D486D
		public MBList<Formation> FormationsIncludingEmpty { get; private set; }

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003413 RID: 13331 RVA: 0x000D6676 File Offset: 0x000D4876
		// (set) Token: 0x06003414 RID: 13332 RVA: 0x000D667E File Offset: 0x000D487E
		public MBList<Formation> FormationsIncludingSpecialAndEmpty { get; private set; }

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x000D6687 File Offset: 0x000D4887
		// (set) Token: 0x06003416 RID: 13334 RVA: 0x000D668F File Offset: 0x000D488F
		public TeamAIComponent TeamAI { get; private set; }

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06003417 RID: 13335 RVA: 0x000D6698 File Offset: 0x000D4898
		public bool IsPlayerTeam
		{
			get
			{
				return this.Mission != null && this.Mission.PlayerTeam == this;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x000D66B2 File Offset: 0x000D48B2
		public bool IsPlayerAlly
		{
			get
			{
				return this.Mission != null && this.Mission.PlayerTeam != null && this.Mission.PlayerTeam.Side == this.Side;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x000D66E3 File Offset: 0x000D48E3
		public TeamSideEnum TeamSide
		{
			get
			{
				if (this.IsPlayerTeam)
				{
					return TeamSideEnum.PlayerTeam;
				}
				if (!this.IsPlayerAlly)
				{
					return TeamSideEnum.EnemyTeam;
				}
				return TeamSideEnum.PlayerAllyTeam;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x0600341A RID: 13338 RVA: 0x000D66FA File Offset: 0x000D48FA
		public bool IsDefender
		{
			get
			{
				return this.Side == BattleSideEnum.Defender;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x000D6705 File Offset: 0x000D4905
		public bool IsAttacker
		{
			get
			{
				return this.Side == BattleSideEnum.Attacker;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x000D6710 File Offset: 0x000D4910
		// (set) Token: 0x0600341D RID: 13341 RVA: 0x000D6718 File Offset: 0x000D4918
		public uint Color { get; private set; }

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x000D6721 File Offset: 0x000D4921
		// (set) Token: 0x0600341F RID: 13343 RVA: 0x000D6729 File Offset: 0x000D4929
		public uint Color2 { get; private set; }

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000D6732 File Offset: 0x000D4932
		public Banner Banner { get; }

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x000D673A File Offset: 0x000D493A
		public OrderController MasterOrderController
		{
			get
			{
				return this._orderControllers[0];
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x000D6748 File Offset: 0x000D4948
		public OrderController PlayerOrderController
		{
			get
			{
				return this._orderControllers[1];
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x000D6756 File Offset: 0x000D4956
		// (set) Token: 0x06003424 RID: 13348 RVA: 0x000D675E File Offset: 0x000D495E
		public TeamQuerySystem QuerySystem { get; private set; }

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x000D6767 File Offset: 0x000D4967
		// (set) Token: 0x06003426 RID: 13350 RVA: 0x000D676F File Offset: 0x000D496F
		public DetachmentManager DetachmentManager { get; private set; }

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x000D6778 File Offset: 0x000D4978
		// (set) Token: 0x06003428 RID: 13352 RVA: 0x000D6780 File Offset: 0x000D4980
		public bool IsPlayerGeneral { get; private set; }

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003429 RID: 13353 RVA: 0x000D6789 File Offset: 0x000D4989
		// (set) Token: 0x0600342A RID: 13354 RVA: 0x000D6791 File Offset: 0x000D4991
		public bool IsPlayerSergeant { get; private set; }

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x000D679A File Offset: 0x000D499A
		public MBReadOnlyList<Agent> ActiveAgents
		{
			get
			{
				return this._activeAgents;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x0600342C RID: 13356 RVA: 0x000D67A2 File Offset: 0x000D49A2
		public MBReadOnlyList<Agent> TeamAgents
		{
			get
			{
				return this._teamAgents;
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x000D67AA File Offset: 0x000D49AA
		public MBReadOnlyList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>> CachedEnemyDataForFleeing
		{
			get
			{
				return this._cachedEnemyDataForFleeing;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x000D67B2 File Offset: 0x000D49B2
		public int TeamIndex
		{
			get
			{
				return this.MBTeam.Index;
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000D67C0 File Offset: 0x000D49C0
		public Team(MBTeam mbTeam, BattleSideEnum side, Mission mission, uint color = 4294967295U, uint color2 = 4294967295U, Banner banner = null)
		{
			this.MBTeam = mbTeam;
			this.Side = side;
			this.Mission = mission;
			this.Color = color;
			this.Color2 = color2;
			this.Banner = banner;
			this.IsPlayerGeneral = true;
			this.IsPlayerSergeant = false;
			if (this != Team._invalid)
			{
				this.Initialize();
			}
			this.MoraleChangeFactor = 1f;
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000D6830 File Offset: 0x000D4A30
		public void SetCustomOrderController(OrderController customMasterOrderController, OrderController customPlayerOrderController)
		{
			this._alreadyHasCustomOrderController = true;
			OrderController orderController = ((this._orderControllers.Count > 0) ? this._orderControllers[0] : null);
			object obj = ((this._orderControllers.Count > 1) ? this._orderControllers[1] : null);
			this._orderControllers.Clear();
			this._orderControllers.Add(customMasterOrderController);
			this._orderControllers.Add(customPlayerOrderController);
			if (orderController != null)
			{
				orderController.AssignDelegatesToController(customMasterOrderController);
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2.AssignDelegatesToController(customPlayerOrderController);
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000D68B8 File Offset: 0x000D4AB8
		public void UpdateCachedEnemyDataForFleeing()
		{
			if (this._cachedEnemyDataForFleeing.IsEmpty<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>>())
			{
				foreach (Team team in this.Mission.Teams)
				{
					if (team.IsEnemyOf(this))
					{
						foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
						{
							int countOfUnits = formation.CountOfUnits;
							if (countOfUnits > 0)
							{
								WorldPosition cachedMedianPosition = formation.CachedMedianPosition;
								float movementSpeedMaximum = formation.QuerySystem.MovementSpeedMaximum;
								bool flag = (formation.QuerySystem.IsCavalryFormation || formation.QuerySystem.IsRangedCavalryFormation) && formation.HasAnyMountedUnit;
								if (countOfUnits == 1)
								{
									Vec2 asVec = cachedMedianPosition.AsVec2;
									this._cachedEnemyDataForFleeing.Add(new ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>(movementSpeedMaximum, cachedMedianPosition, countOfUnits, asVec, asVec, flag));
								}
								else
								{
									Vec2 vec = formation.QuerySystem.EstimatedDirection.LeftVec();
									float num = formation.Width / 2f;
									Vec2 vec2 = cachedMedianPosition.AsVec2 - vec * num;
									Vec2 vec3 = cachedMedianPosition.AsVec2 + vec * num;
									this._cachedEnemyDataForFleeing.Add(new ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>(movementSpeedMaximum, cachedMedianPosition, countOfUnits, vec2, vec3, flag));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003432 RID: 13362 RVA: 0x000D6A68 File Offset: 0x000D4C68
		// (set) Token: 0x06003433 RID: 13363 RVA: 0x000D6A70 File Offset: 0x000D4C70
		public float MoraleChangeFactor { get; private set; }

		// Token: 0x06003434 RID: 13364 RVA: 0x000D6A7C File Offset: 0x000D4C7C
		private void Initialize()
		{
			this._activeAgents = new MBList<Agent>();
			this._teamAgents = new MBList<Agent>();
			this._cachedEnemyDataForFleeing = new MBList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>>();
			if (!GameNetwork.IsReplay)
			{
				this.FormationsIncludingSpecialAndEmpty = new MBList<Formation>(10);
				this.FormationsIncludingEmpty = new MBList<Formation>(8);
				for (int i = 0; i < 10; i++)
				{
					Formation formation = new Formation(this, i);
					this.FormationsIncludingSpecialAndEmpty.Add(formation);
					if (i < 8)
					{
						this.FormationsIncludingEmpty.Add(formation);
					}
					formation.AI.OnActiveBehaviorChanged += this.FormationAI_OnActiveBehaviorChanged;
				}
				if (this.Mission != null)
				{
					this._orderControllers = new List<OrderController>();
					OrderController orderController = new OrderController(this.Mission, this, null);
					this._orderControllers.Add(orderController);
					orderController.OnOrderIssued += this.OrderController_OnOrderIssued;
					OrderController orderController2 = new OrderController(this.Mission, this, null);
					this._orderControllers.Add(orderController2);
					orderController2.OnOrderIssued += this.OrderController_OnOrderIssued;
				}
				this.QuerySystem = new TeamQuerySystem(this);
				this.DetachmentManager = new DetachmentManager(this);
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000D6B98 File Offset: 0x000D4D98
		public void Reset()
		{
			if (!GameNetwork.IsReplay)
			{
				foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
				{
					formation.Reset();
				}
				List<OrderController> orderControllers = this._orderControllers;
				if (orderControllers != null && orderControllers.Count > 2)
				{
					for (int i = this._orderControllers.Count - 1; i >= 2; i--)
					{
						this._orderControllers[i].OnOrderIssued -= this.OrderController_OnOrderIssued;
						this._orderControllers.RemoveAt(i);
					}
				}
				this.QuerySystem = new TeamQuerySystem(this);
			}
			this._teamAgents.Clear();
			this._activeAgents.Clear();
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000D6C6C File Offset: 0x000D4E6C
		public void Clear()
		{
			if (!GameNetwork.IsReplay)
			{
				foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
				{
					formation.AI.OnActiveBehaviorChanged -= this.FormationAI_OnActiveBehaviorChanged;
				}
			}
			this.Reset();
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000D6CDC File Offset: 0x000D4EDC
		private void OrderController_OnOrderIssued(OrderType orderType, MBReadOnlyList<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			OnOrderIssuedDelegate onOrderIssued = this.OnOrderIssued;
			if (onOrderIssued == null)
			{
				return;
			}
			onOrderIssued(orderType, appliedFormations, orderController, delegateParams);
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000D6CF3 File Offset: 0x000D4EF3
		public static bool DoesFirstFormationClassContainSecond(FormationClass f1, FormationClass f2)
		{
			return (f1 & f2) == f2;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000D6CFB File Offset: 0x000D4EFB
		public static FormationClass GetFormationFormationClass(Formation f)
		{
			if (f.QuerySystem.IsRangedCavalryFormation)
			{
				return FormationClass.HorseArcher;
			}
			if (f.QuerySystem.IsCavalryFormation)
			{
				return FormationClass.Cavalry;
			}
			if (!f.QuerySystem.IsRangedFormation)
			{
				return FormationClass.Infantry;
			}
			return FormationClass.Ranged;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000D6D2B File Offset: 0x000D4F2B
		public static FormationClass GetPlayerTeamFormationClass(Agent mainAgent)
		{
			if (mainAgent.IsRangedCached && mainAgent.HasMount)
			{
				return FormationClass.HorseArcher;
			}
			if (mainAgent.IsRangedCached)
			{
				return FormationClass.Ranged;
			}
			if (!mainAgent.HasMount)
			{
				return FormationClass.Infantry;
			}
			return FormationClass.Cavalry;
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000D6D54 File Offset: 0x000D4F54
		public void AssignPlayerAsSergeantOfFormation(MissionPeer peer, FormationClass formationClass)
		{
			Formation formation = this.GetFormation(formationClass);
			formation.PlayerOwner = peer.ControlledAgent;
			formation.BannerCode = peer.Peer.BannerCode;
			if (peer.IsMine)
			{
				this.PlayerOrderController.Owner = peer.ControlledAgent;
			}
			else
			{
				this.GetOrderControllerOf(peer.ControlledAgent).Owner = peer.ControlledAgent;
			}
			formation.SetControlledByAI(false, false);
			foreach (MissionBehavior missionBehavior in this.Mission.MissionBehaviors)
			{
				missionBehavior.OnAssignPlayerAsSergeantOfFormation(peer.ControlledAgent);
			}
			if (peer.IsMine)
			{
				this.PlayerOrderController.SelectAllFormations(false);
			}
			peer.ControlledFormation = formation;
			if (GameNetwork.IsServer)
			{
				peer.ControlledAgent.ForceUpdateCachedAndFormationValues(false, false);
				if (!peer.IsMine)
				{
					GameNetwork.BeginModuleEventAsServer(peer.GetNetworkPeer());
					GameNetwork.WriteMessage(new AssignFormationToPlayer(peer.GetNetworkPeer(), formationClass));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000D6E68 File Offset: 0x000D5068
		private void FormationAI_OnActiveBehaviorChanged(Formation formation)
		{
			if (formation.CountOfUnits > 0)
			{
				Action<Formation> onFormationAIActiveBehaviorChanged = this.OnFormationAIActiveBehaviorChanged;
				if (onFormationAIActiveBehaviorChanged == null)
				{
					return;
				}
				onFormationAIActiveBehaviorChanged(formation);
			}
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000D6E84 File Offset: 0x000D5084
		public void AddTacticOption(TacticComponent tacticOption)
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.AddTacticOption(tacticOption);
			}
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000D6E9A File Offset: 0x000D509A
		public void RemoveTacticOption(Type tacticType)
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.RemoveTacticOption(tacticType);
			}
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000D6EB0 File Offset: 0x000D50B0
		public void ClearTacticOptions()
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.ClearTacticOptions();
			}
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000D6EC5 File Offset: 0x000D50C5
		public void ResetTactic()
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.ResetTactic(true);
			}
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000D6EDC File Offset: 0x000D50DC
		public void AddTeamAI(TeamAIComponent teamAI, bool forceNotAIControlled = false)
		{
			this.TeamAI = teamAI;
			foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetControlledByAI(!forceNotAIControlled && (this != this.Mission.PlayerTeam || !this.IsPlayerGeneral), false);
			}
			this.TeamAI.InitializeDetachments(this.Mission);
			this.TeamAI.CreateMissionSpecificBehaviors();
			this.TeamAI.ResetTactic(true);
			foreach (Formation formation2 in this.FormationsIncludingSpecialAndEmpty)
			{
				if (formation2.CountOfUnits > 0)
				{
					formation2.AI.Tick();
				}
			}
			this.TeamAI.TickOccasionally();
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000D6FD8 File Offset: 0x000D51D8
		public void DelegateCommandToAI()
		{
			foreach (Formation formation in this.FormationsIncludingEmpty)
			{
				formation.SetControlledByAI(true, false);
			}
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000D702C File Offset: 0x000D522C
		public void RearrangeFormationsAccordingToFilter([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] List<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>> MassTransferData)
		{
			List<Formation> list = new List<Formation>();
			foreach (ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> valueTuple in MassTransferData)
			{
				valueTuple.Item1.OnMassUnitTransferStart();
				if (valueTuple.Item1.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderStop && (valueTuple.Item1.CountOfUnits > 0 || valueTuple.Item2 > 0))
				{
					list.Add(valueTuple.Item1);
					valueTuple.Item1.SetMovementOrder(MovementOrder.MovementOrderMove(valueTuple.Item1.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None)));
				}
			}
			List<Agent>[] array = new List<Agent>[MassTransferData.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new List<Agent>();
			}
			List<FormationPocket> list2 = new List<FormationPocket>();
			for (int j = 0; j < MassTransferData.Count; j++)
			{
				TroopTraitsMask item = MassTransferData[j].Item3;
				Func<Agent, int> func;
				TroopFilteringUtilities.GetPriorityFunction(item, out func);
				int maxPriority = TroopFilteringUtilities.GetMaxPriority(item);
				list2.Add(new FormationPocket(func, maxPriority, MassTransferData[j].Item2, j));
			}
			list2.RemoveAll((FormationPocket pfamv) => pfamv.TroopCount <= 0);
			list2 = list2.OrderBy<FormationPocket, int>((FormationPocket pfamv) => pfamv.TroopCount).ToList<FormationPocket>();
			list2 = list2.OrderByDescending<FormationPocket, int>((FormationPocket pfamv) => pfamv.ScoreToSeek).ToList<FormationPocket>();
			List<IFormationUnit> list3 = new List<IFormationUnit>();
			list3 = MassTransferData.SelectMany<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>, IFormationUnit>(([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> mtd) => mtd.Item1.DetachedUnits.Concat<IFormationUnit>(mtd.Item1.Arrangement.GetAllUnits()).Except<IFormationUnit>(mtd.Item4)).ToList<IFormationUnit>();
			int num = MassTransferData.Sum<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>>(([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> mtd) => mtd.Item4.Count);
			int k = MassTransferData.Sum<ValueTuple<Formation, int, TroopTraitsMask, List<Agent>>>(([TupleElementNames(new string[] { "formation", "troopCount", "troopFilter", "excludedAgents" })] ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> mtd) => mtd.Item1.CountOfUnits) - num;
			int num2 = list2[0].ScoreToSeek;
			while (k > 0)
			{
				for (int l = 0; l < k; l++)
				{
					Agent agent = list3[l] as Agent;
					for (int m = 0; m < list2.Count; m++)
					{
						FormationPocket formationPocket = list2[m];
						int num3 = formationPocket.PriorityFunction(agent);
						if (num2 <= formationPocket.ScoreToSeek && num3 >= num2)
						{
							array[formationPocket.Index].Add(agent);
							formationPocket.AddTroop();
							if (formationPocket.IsFormationPocketFilled())
							{
								list2.RemoveAt(m);
							}
							k--;
							list3[l] = list3[k];
							l--;
							break;
						}
						if (num3 > formationPocket.BestScoreSoFar)
						{
							formationPocket.SetBestScoreSoFar(num3);
						}
					}
				}
				if (list2.Count == 0)
				{
					break;
				}
				for (int n = 0; n < list2.Count; n++)
				{
					list2[n].UpdateScoreToSeek();
				}
				list2.OrderByDescending<FormationPocket, int>((FormationPocket pfamv) => pfamv.ScoreToSeek);
				num2 = list2[0].ScoreToSeek;
			}
			for (int num4 = 0; num4 < array.Length; num4++)
			{
				foreach (Agent agent2 in array[num4])
				{
					agent2.Formation = MassTransferData[num4].Item1;
				}
			}
			foreach (ValueTuple<Formation, int, TroopTraitsMask, List<Agent>> valueTuple2 in MassTransferData)
			{
				this.TriggerOnFormationsChanged(valueTuple2.Item1);
				valueTuple2.Item1.OnMassUnitTransferEnd();
				if (valueTuple2.Item1.CountOfUnits > 0 && !valueTuple2.Item1.OrderPositionIsValid)
				{
					Vec2 averagePositionOfUnits = valueTuple2.Item1.GetAveragePositionOfUnits(false, false);
					float terrainHeight = this.Mission.Scene.GetTerrainHeight(averagePositionOfUnits, true);
					this.Mission.Scene.GetHeightAtPoint(averagePositionOfUnits, BodyFlags.None, ref terrainHeight);
					Vec3 vec = new Vec3(averagePositionOfUnits, terrainHeight, -1f);
					WorldPosition worldPosition = new WorldPosition(this.Mission.Scene, UIntPtr.Zero, vec, false);
					valueTuple2.Item1.SetPositioning(new WorldPosition?(worldPosition), null, null);
				}
			}
			foreach (Formation formation in list)
			{
				formation.SetMovementOrder(MovementOrder.MovementOrderStop);
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x000D7540 File Offset: 0x000D5740
		// (set) Token: 0x06003445 RID: 13381 RVA: 0x000D7548 File Offset: 0x000D5748
		public Formation GeneralsFormation { get; set; }

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003446 RID: 13382 RVA: 0x000D7551 File Offset: 0x000D5751
		// (set) Token: 0x06003447 RID: 13383 RVA: 0x000D7559 File Offset: 0x000D5759
		public Formation BodyGuardFormation { get; set; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x000D7562 File Offset: 0x000D5762
		// (set) Token: 0x06003449 RID: 13385 RVA: 0x000D756A File Offset: 0x000D576A
		public Agent GeneralAgent { get; set; }

		// Token: 0x0600344A RID: 13386 RVA: 0x000D7574 File Offset: 0x000D5774
		public void Tick(float dt)
		{
			if (!this._cachedEnemyDataForFleeing.IsEmpty<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>>())
			{
				this._cachedEnemyDataForFleeing.Clear();
			}
			if (this.Mission.AllowAiTicking)
			{
				if (this.Mission.RetreatSide != BattleSideEnum.None && this.Side == this.Mission.RetreatSide)
				{
					using (List<Formation>.Enumerator enumerator = this.FormationsIncludingSpecialAndEmpty.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Formation formation = enumerator.Current;
							if (formation.CountOfUnits > 0)
							{
								formation.SetMovementOrder(MovementOrder.MovementOrderRetreat);
							}
						}
						goto IL_00A8;
					}
				}
				if (this.TeamAI != null && this.HasBots)
				{
					this.TeamAI.Tick(dt);
				}
			}
			IL_00A8:
			if (!GameNetwork.IsReplay)
			{
				if (this._tickDetachments)
				{
					this.DetachmentManager.TickDetachments();
				}
				foreach (Formation formation2 in this.FormationsIncludingSpecialAndEmpty)
				{
					if (formation2.CountOfUnits > 0)
					{
						formation2.Tick(dt);
					}
				}
			}
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000D76A0 File Offset: 0x000D58A0
		public Formation GetFormation(FormationClass formationIndex)
		{
			return this.FormationsIncludingSpecialAndEmpty[(int)formationIndex];
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000D76B0 File Offset: 0x000D58B0
		public void SetIsEnemyOf(Team otherTeam, bool isEnemyOf)
		{
			this.MBTeam.SetIsEnemyOf(otherTeam.MBTeam, isEnemyOf);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new TeamSetIsEnemyOf(this.TeamIndex, otherTeam.TeamIndex, isEnemyOf));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000D7700 File Offset: 0x000D5900
		public bool IsEnemyOf(Team otherTeam)
		{
			return this.MBTeam.IsEnemyOf(otherTeam.MBTeam);
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000D7724 File Offset: 0x000D5924
		public bool IsFriendOf(Team otherTeam)
		{
			return this == otherTeam || !this.MBTeam.IsEnemyOf(otherTeam.MBTeam);
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x000D774E File Offset: 0x000D594E
		public IEnumerable<Agent> Heroes
		{
			get
			{
				Agent main = Agent.Main;
				if (main != null && main.Team == this)
				{
					yield return main;
				}
				yield break;
			}
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000D775E File Offset: 0x000D595E
		public void AddAgentToTeam(Agent unit)
		{
			this._teamAgents.Add(unit);
			this._activeAgents.Add(unit);
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000D7778 File Offset: 0x000D5978
		public void RemoveAgentFromTeam(Agent unit)
		{
			this._teamAgents.Remove(unit);
			this._activeAgents.Remove(unit);
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000D7794 File Offset: 0x000D5994
		public void DeactivateAgent(Agent agent)
		{
			this._activeAgents.Remove(agent);
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x000D77A4 File Offset: 0x000D59A4
		public void OnAgentRemoved(Agent agent)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
				{
					formation.AI.OnAgentRemoved(agent);
				}
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x000D7804 File Offset: 0x000D5A04
		public bool HasBots
		{
			get
			{
				foreach (Agent agent in this.ActiveAgents)
				{
					if (!agent.IsMount && !agent.IsPlayerControlled)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x000D7868 File Offset: 0x000D5A68
		public Agent Leader
		{
			get
			{
				if (Agent.Main != null && Agent.Main.Team == this)
				{
					return Agent.Main;
				}
				Agent agent = null;
				foreach (Agent agent2 in this.ActiveAgents)
				{
					if (agent == null || agent2.IsHero)
					{
						agent = agent2;
						if (agent.IsHero)
						{
							break;
						}
					}
				}
				return agent;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x000D78E8 File Offset: 0x000D5AE8
		// (set) Token: 0x06003457 RID: 13399 RVA: 0x000D7908 File Offset: 0x000D5B08
		public static Team Invalid
		{
			get
			{
				Team team;
				if ((team = Team._invalid) == null)
				{
					team = (Team._invalid = new Team(MBTeam.InvalidTeam, BattleSideEnum.None, null, uint.MaxValue, uint.MaxValue, null));
				}
				return team;
			}
			internal set
			{
				Team._invalid = value;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x000D7910 File Offset: 0x000D5B10
		public bool IsValid
		{
			get
			{
				return this.MBTeam.IsValid;
			}
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x000D792C File Offset: 0x000D5B2C
		public override string ToString()
		{
			return this.MBTeam.ToString();
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x000D794D File Offset: 0x000D5B4D
		public bool HasTeamAi
		{
			get
			{
				return this.TeamAI != null;
			}
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x000D7958 File Offset: 0x000D5B58
		public void OnMissionEnded()
		{
			if (this.HasTeamAi)
			{
				this.TeamAI.OnMissionEnded();
			}
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x000D796D File Offset: 0x000D5B6D
		public void TriggerOnFormationsChanged(Formation formation)
		{
			Action<Team, Formation> onFormationsChanged = this.OnFormationsChanged;
			if (onFormationsChanged == null)
			{
				return;
			}
			onFormationsChanged(this, formation);
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x000D7981 File Offset: 0x000D5B81
		public void TriggerOnFormationsChangedInDeployment()
		{
			Action<Team> onFormationsChangedInDeployment = this.OnFormationsChangedInDeployment;
			if (onFormationsChangedInDeployment == null)
			{
				return;
			}
			onFormationsChangedInDeployment(this);
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x000D7994 File Offset: 0x000D5B94
		public OrderController GetOrderControllerOf(Agent agent)
		{
			OrderController orderController = this._orderControllers.FirstOrDefault<OrderController>((OrderController oc) => oc.Owner == agent);
			if (orderController == null)
			{
				orderController = new OrderController(this.Mission, this, agent);
				this._orderControllers.Add(orderController);
				orderController.OnOrderIssued += this.OrderController_OnOrderIssued;
			}
			return orderController;
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x000D79FC File Offset: 0x000D5BFC
		public void SetPlayerRole(bool isPlayerGeneral, bool isPlayerSergeant)
		{
			this.IsPlayerGeneral = isPlayerGeneral;
			this.IsPlayerSergeant = isPlayerSergeant;
			foreach (Formation formation in this.FormationsIncludingSpecialAndEmpty)
			{
				formation.SetControlledByAI(this != this.Mission.PlayerTeam || !this.IsPlayerGeneral, false);
			}
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x000D7A78 File Offset: 0x000D5C78
		public bool HasAnyEnemyTeamsWithAgents(bool ignoreMountedAgents)
		{
			foreach (Team team in this.Mission.Teams)
			{
				if (team != this && team.IsEnemyOf(this) && team.ActiveAgents.Count > 0)
				{
					if (ignoreMountedAgents)
					{
						using (List<Agent>.Enumerator enumerator2 = team.ActiveAgents.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (!enumerator2.Current.HasMount)
								{
									return true;
								}
							}
							continue;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x000D7B34 File Offset: 0x000D5D34
		public bool HasAnyFormationsIncludingSpecialThatIsNotEmpty()
		{
			using (List<Formation>.Enumerator enumerator = this.FormationsIncludingSpecialAndEmpty.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CountOfUnits > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x000D7B90 File Offset: 0x000D5D90
		public int GetFormationCount()
		{
			int num = 0;
			using (List<Formation>.Enumerator enumerator = this.FormationsIncludingEmpty.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CountOfUnits > 0)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x000D7BEC File Offset: 0x000D5DEC
		public int GetAIControlledFormationCount()
		{
			int num = 0;
			foreach (Formation formation in this.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0 && formation.IsAIControlled)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000D7C50 File Offset: 0x000D5E50
		public Vec2 GetAveragePositionOfEnemies()
		{
			Vec2 vec = Vec2.Zero;
			int num = 0;
			foreach (Team team in this.Mission.Teams)
			{
				if (team.MBTeam.IsValid && this.IsEnemyOf(team))
				{
					foreach (Agent agent in team.ActiveAgents)
					{
						vec += agent.Position.AsVec2;
						num++;
					}
				}
			}
			if (num > 0)
			{
				vec *= 1f / (float)num;
				return vec;
			}
			return Vec2.Invalid;
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000D7D38 File Offset: 0x000D5F38
		public Vec2 GetAveragePosition()
		{
			Vec2 vec = Vec2.Zero;
			List<Agent> activeAgents = this.ActiveAgents;
			int num = 0;
			foreach (Agent agent in activeAgents)
			{
				vec += agent.Position.AsVec2;
				num++;
			}
			if (num > 0)
			{
				vec *= 1f / (float)num;
			}
			else
			{
				vec = Vec2.Invalid;
			}
			return vec;
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x000D7DC4 File Offset: 0x000D5FC4
		public WorldPosition GetMedianPosition(Vec2 averagePosition)
		{
			float num = float.MaxValue;
			Agent agent = null;
			foreach (Agent agent2 in this.ActiveAgents)
			{
				float num2 = agent2.Position.AsVec2.DistanceSquared(averagePosition);
				if (num2 <= num)
				{
					agent = agent2;
					num = num2;
				}
			}
			if (agent == null)
			{
				return WorldPosition.Invalid;
			}
			return agent.GetWorldPosition();
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000D7E4C File Offset: 0x000D604C
		public Vec2 GetWeightedAverageOfEnemies(Vec2 basePoint)
		{
			Vec2 vec = Vec2.Zero;
			float num = 0f;
			foreach (Team team in this.Mission.Teams)
			{
				if (team.MBTeam.IsValid && this.IsEnemyOf(team))
				{
					foreach (Agent agent in team.ActiveAgents)
					{
						Vec2 asVec = agent.Position.AsVec2;
						float lengthSquared = (basePoint - asVec).LengthSquared;
						float num2 = 1f / lengthSquared;
						vec += asVec * num2;
						num += num2;
					}
				}
			}
			if (num > 0f)
			{
				vec *= 1f / num;
				return vec;
			}
			return Vec2.Invalid;
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x000D7F64 File Offset: 0x000D6164
		public void DisableDetachmentTicking()
		{
			this._tickDetachments = false;
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x000D7F6D File Offset: 0x000D616D
		[Conditional("DEBUG")]
		private void TickStandingPointDebug()
		{
		}

		// Token: 0x040015F5 RID: 5621
		public readonly MBTeam MBTeam;

		// Token: 0x040015FE RID: 5630
		private List<OrderController> _orderControllers;

		// Token: 0x04001603 RID: 5635
		private MBList<Agent> _activeAgents;

		// Token: 0x04001604 RID: 5636
		private MBList<Agent> _teamAgents;

		// Token: 0x04001605 RID: 5637
		private bool _tickDetachments = true;

		// Token: 0x04001606 RID: 5638
		private MBList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>> _cachedEnemyDataForFleeing;

		// Token: 0x04001607 RID: 5639
		private bool _alreadyHasCustomOrderController;

		// Token: 0x0400160C RID: 5644
		private static Team _invalid;
	}
}
