using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026F RID: 623
	public class AgentVictoryLogic : MissionLogic
	{
		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0007B3E2 File Offset: 0x000795E2
		public AgentVictoryLogic.CheerActionGroupEnum CheerActionGroup
		{
			get
			{
				return this._cheerActionGroup;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0007B3EA File Offset: 0x000795EA
		public AgentVictoryLogic.CheerReactionTimeSettings CheerReactionTimerData
		{
			get
			{
				return this._cheerReactionTimerData;
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x0007B3F4 File Offset: 0x000795F4
		public override void AfterStart()
		{
			base.Mission.MissionCloseTimeAfterFinish = 60f;
			this._cheeringAgents = new List<AgentVictoryLogic.CheeringAgent>();
			this.SetCheerReactionTimerSettings(1f, 8f);
			if (base.Mission.PlayerTeam != null)
			{
				base.Mission.PlayerTeam.PlayerOrderController.OnOrderIssued += new OnOrderIssuedDelegate(this.MasterOrderControllerOnOrderIssued);
			}
			Mission.Current.IsBattleInRetreatEvent += this.CheckIfIsInRetreat;
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x0007B470 File Offset: 0x00079670
		private void MasterOrderControllerOnOrderIssued(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, object[] delegateparams)
		{
			MBList<Formation> mblist = appliedFormations.ToMBList<Formation>();
			for (int i = this._cheeringAgents.Count - 1; i >= 0; i--)
			{
				Agent agent = this._cheeringAgents[i].Agent;
				if (mblist.Contains(agent.Formation))
				{
					this._cheeringAgents[i].OrderReceived();
				}
			}
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x0007B4D0 File Offset: 0x000796D0
		public void SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum cheerActionGroup = AgentVictoryLogic.CheerActionGroupEnum.None)
		{
			this._cheerActionGroup = cheerActionGroup;
			switch (this._cheerActionGroup)
			{
			case AgentVictoryLogic.CheerActionGroupEnum.LowCheerActions:
				this._selectedCheerActions = this._lowCheerActions;
				return;
			case AgentVictoryLogic.CheerActionGroupEnum.MidCheerActions:
				this._selectedCheerActions = this._midCheerActions;
				return;
			case AgentVictoryLogic.CheerActionGroupEnum.HighCheerActions:
				this._selectedCheerActions = this._highCheerActions;
				return;
			default:
				this._selectedCheerActions = null;
				return;
			}
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x0007B52F File Offset: 0x0007972F
		public void SetCheerReactionTimerSettings(float minDuration = 1f, float maxDuration = 8f)
		{
			this._cheerReactionTimerData = new AgentVictoryLogic.CheerReactionTimeSettings(minDuration, maxDuration);
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0007B53E File Offset: 0x0007973E
		public override void OnClearScene()
		{
			this._cheeringAgents.Clear();
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x0007B54C File Offset: 0x0007974C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			VictoryComponent component = affectedAgent.GetComponent<VictoryComponent>();
			if (component != null)
			{
				affectedAgent.RemoveComponent(component);
			}
			for (int i = 0; i < this._cheeringAgents.Count; i++)
			{
				if (this._cheeringAgents[i].Agent == affectedAgent)
				{
					this._cheeringAgents.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x0007B5A2 File Offset: 0x000797A2
		protected override void OnEndMission()
		{
			Mission.Current.IsBattleInRetreatEvent -= this.CheckIfIsInRetreat;
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x0007B5BA File Offset: 0x000797BA
		public override void OnMissionTick(float dt)
		{
			if (this._cheeringAgents.Count > 0)
			{
				this.CheckAnimationAndVoice();
			}
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x0007B5D0 File Offset: 0x000797D0
		private void CheckAnimationAndVoice()
		{
			for (int i = this._cheeringAgents.Count - 1; i >= 0; i--)
			{
				Agent agent = this._cheeringAgents[i].Agent;
				bool gotOrderRecently = this._cheeringAgents[i].GotOrderRecently;
				bool isCheeringOnRetreat = this._cheeringAgents[i].IsCheeringOnRetreat;
				bool flag = this._cheeringAgents[i].IsCheeringPaused;
				VictoryComponent component = agent.GetComponent<VictoryComponent>();
				if (component != null)
				{
					HumanAIComponent component2 = agent.GetComponent<HumanAIComponent>();
					bool flag2 = ((component2 != null) ? component2.GetCurrentlyMovingGameObject() : null) != null;
					bool flag3 = agent.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement) || agent.GetCurrentAnimationFlag(1).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement);
					bool flag4 = agent.IsInWater() || agent.IsUsingGameObject || flag2 || flag3;
					if (this.CheckIfIsInRetreat() && gotOrderRecently)
					{
						agent.RemoveComponent(component);
						agent.SetActionChannel(1, in ActionIndexCache.act_none, false, (AnimFlags)((long)Math.Min(agent.GetCurrentActionPriority(1), 73)), 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						if (MBRandom.RandomFloat > 0.25f)
						{
							agent.MakeVoice(SkinVoiceManager.VoiceType.Yell, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
						}
						if (isCheeringOnRetreat)
						{
							agent.ClearTargetFrame();
						}
						this._cheeringAgents.RemoveAt(i);
					}
					else if (flag != flag4)
					{
						this._cheeringAgents[i].UpdatePauseState(flag4);
						flag = this._cheeringAgents[i].IsCheeringPaused;
					}
					if ((!this.CheckIfIsInRetreat() || !gotOrderRecently) && !flag && component.CheckTimer())
					{
						if (!agent.IsActive())
						{
							Debug.FailedAssert("Agent trying to cheer without being active", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\AgentVictoryLogic.cs", "CheckAnimationAndVoice", 250);
							Debug.Print("Agent trying to cheer without being active", 0, Debug.DebugColor.White, 17592186044416UL);
						}
						bool flag5;
						this.ChooseWeaponToCheerWithCheerAndUpdateTimer(agent, out flag5);
						if (flag5)
						{
							component.ChangeTimerDuration(6f, 12f);
						}
					}
				}
			}
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x0007B7C8 File Offset: 0x000799C8
		private void SelectVictoryCondition(BattleSideEnum side)
		{
			if (this._cheerActionGroup == AgentVictoryLogic.CheerActionGroupEnum.None)
			{
				BattleObserverMissionLogic missionBehavior = Mission.Current.GetMissionBehavior<BattleObserverMissionLogic>();
				if (missionBehavior != null)
				{
					float deathToBuiltAgentRatioForSide = missionBehavior.GetDeathToBuiltAgentRatioForSide(side);
					if (deathToBuiltAgentRatioForSide < 0.25f)
					{
						this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.HighCheerActions);
						return;
					}
					if (deathToBuiltAgentRatioForSide < 0.75f)
					{
						this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.MidCheerActions);
						return;
					}
					this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.LowCheerActions);
					return;
				}
				else
				{
					this.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.MidCheerActions);
				}
			}
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x0007B824 File Offset: 0x00079A24
		public void SetTimersOfVictoryReactionsOnBattleEnd(BattleSideEnum side)
		{
			this._isInRetreat = false;
			this.SelectVictoryCondition(side);
			foreach (Team team in base.Mission.Teams)
			{
				if (team.Side == side)
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							formation.SetMovementOrder(MovementOrder.MovementOrderStop);
						}
					}
				}
			}
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsHuman && agent.IsAIControlled && agent.Team != null && side == agent.Team.Side && agent.CurrentWatchState == Agent.WatchState.Alarmed && agent.GetComponent<VictoryComponent>() == null)
				{
					this.RegisterAgentForCheerCheck(agent, false, this._cheerReactionTimerData.MinDuration, this._cheerReactionTimerData.MaxDuration);
				}
			}
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0007B97C File Offset: 0x00079B7C
		private void RegisterAgentForCheerCheck(Agent agent, bool isCheeringOnRetreat, float minReactionTime, float maxReactionTime)
		{
			agent.AddComponent(new VictoryComponent(agent, new RandomTimer(base.Mission.CurrentTime, minReactionTime, maxReactionTime)));
			this._cheeringAgents.Add(new AgentVictoryLogic.CheeringAgent(agent, isCheeringOnRetreat));
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0007B9B0 File Offset: 0x00079BB0
		public void SetTimersOfVictoryReactionsOnRetreat(BattleSideEnum side)
		{
			this._isInRetreat = true;
			this.SelectVictoryCondition(side);
			List<Agent> list = base.Mission.Agents.Where<Agent>((Agent agent) => agent.IsHuman && agent.IsAIControlled && agent.Team.Side == side).ToList<Agent>();
			int num = (int)((float)list.Count * 0.5f);
			List<Agent> list2 = new List<Agent>();
			int num2 = 0;
			while (num2 < list.Count && list2.Count != num)
			{
				Agent agent3 = list[num2];
				EquipmentIndex primaryWieldedItemIndex = agent3.GetPrimaryWieldedItemIndex();
				bool flag = primaryWieldedItemIndex != EquipmentIndex.None && agent3.Equipment[primaryWieldedItemIndex].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction);
				EquipmentIndex offhandWieldedItemIndex = agent3.GetOffhandWieldedItemIndex();
				bool flag2 = offhandWieldedItemIndex != EquipmentIndex.None && agent3.Equipment[offhandWieldedItemIndex].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction);
				HumanAIComponent component = agent3.GetComponent<HumanAIComponent>();
				bool flag3 = ((component != null) ? component.GetCurrentlyMovingGameObject() : null) != null;
				bool flag4 = agent3.GetCurrentAnimationFlag(0).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement) || agent3.GetCurrentAnimationFlag(1).HasAnyFlag(AnimFlags.anf_synch_with_ladder_movement);
				if (!agent3.IsInWater() && !flag && !flag2 && !agent3.IsUsingGameObject && !flag3 && !flag4)
				{
					int num3 = list.Count - num2;
					int num4 = num - list2.Count;
					int num5 = num3 - num4;
					float num6 = MBMath.ClampFloat((float)(num - num5) / (float)num, 0f, 1f);
					float num7;
					Vec3 vec;
					if (num6 < 1f && agent3.TryGetImmediateEnemyAgentMovementData(out num7, out vec))
					{
						float maximumForwardUnlimitedSpeed = agent3.GetMaximumForwardUnlimitedSpeed();
						float num8 = num7;
						if (maximumForwardUnlimitedSpeed > num8)
						{
							float num9 = (agent3.Position - vec).LengthSquared / (maximumForwardUnlimitedSpeed - num8);
							if (num9 < 900f)
							{
								float num10 = num6 - -1f;
								float num11 = num9 / 900f;
								num6 = -1f + num10 * num11;
							}
						}
					}
					if (MBRandom.RandomFloat <= 0.5f + 0.5f * num6)
					{
						list2.Add(agent3);
					}
				}
				num2++;
			}
			foreach (Agent agent2 in list2)
			{
				MatrixFrame frame = agent2.Frame;
				Vec2 asVec = frame.origin.AsVec2;
				Vec3 f = frame.rotation.f;
				agent2.SetTargetPositionAndDirectionSynched(ref asVec, ref f);
				this.SetTimersOfVictoryReactionsForSingleAgent(agent2, this._cheerReactionTimerData.MinDuration, this._cheerReactionTimerData.MaxDuration, true);
			}
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x0007BC7C File Offset: 0x00079E7C
		public void SetTimersOfVictoryReactionsOnTournamentVictoryForAgent(Agent agent, float minStartTime, float maxStartTime)
		{
			this._selectedCheerActions = this._midCheerActions;
			this.SetTimersOfVictoryReactionsForSingleAgent(agent, minStartTime, maxStartTime, false);
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0007BC94 File Offset: 0x00079E94
		private void SetTimersOfVictoryReactionsForSingleAgent(Agent agent, float minStartTime, float maxStartTime, bool isCheeringOnRetreat)
		{
			if (agent.IsActive() && agent.IsHuman && agent.IsAIControlled)
			{
				this.RegisterAgentForCheerCheck(agent, isCheeringOnRetreat, minStartTime, maxStartTime);
			}
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0007BCBC File Offset: 0x00079EBC
		private void ChooseWeaponToCheerWithCheerAndUpdateTimer(Agent cheerAgent, out bool resetTimer)
		{
			resetTimer = false;
			if (cheerAgent.GetCurrentActionType(1) != Agent.ActionCodeType.EquipUnequip)
			{
				EquipmentIndex primaryWieldedItemIndex = cheerAgent.GetPrimaryWieldedItemIndex();
				bool flag = primaryWieldedItemIndex != EquipmentIndex.None && !cheerAgent.Equipment[primaryWieldedItemIndex].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction);
				if (!flag)
				{
					EquipmentIndex equipmentIndex = EquipmentIndex.None;
					for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex2++)
					{
						if (!cheerAgent.Equipment[equipmentIndex2].IsEmpty && !cheerAgent.Equipment[equipmentIndex2].Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnAnyAction))
						{
							equipmentIndex = equipmentIndex2;
							break;
						}
					}
					if (equipmentIndex == EquipmentIndex.None)
					{
						if (primaryWieldedItemIndex != EquipmentIndex.None)
						{
							cheerAgent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimation);
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						cheerAgent.TryToWieldWeaponInSlot(equipmentIndex, Agent.WeaponWieldActionType.WithAnimation, false);
					}
				}
				if (flag)
				{
					ActionIndexCache[] array = this._selectedCheerActions;
					if (cheerAgent.HasMount)
					{
						array = this._midCheerActions;
					}
					cheerAgent.SetActionChannel(1, in array[MBRandom.RandomInt(array.Length)], false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					cheerAgent.MakeVoice(SkinVoiceManager.VoiceType.Victory, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					resetTimer = true;
				}
			}
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x0007BDF1 File Offset: 0x00079FF1
		private bool CheckIfIsInRetreat()
		{
			return this._isInRetreat;
		}

		// Token: 0x04000D5E RID: 3422
		private const float HighCheerThreshold = 0.25f;

		// Token: 0x04000D5F RID: 3423
		private const float MidCheerThreshold = 0.75f;

		// Token: 0x04000D60 RID: 3424
		private const float YellIfOrderedInRetreatProbability = 0.25f;

		// Token: 0x04000D61 RID: 3425
		private AgentVictoryLogic.CheerActionGroupEnum _cheerActionGroup;

		// Token: 0x04000D62 RID: 3426
		private AgentVictoryLogic.CheerReactionTimeSettings _cheerReactionTimerData;

		// Token: 0x04000D63 RID: 3427
		private readonly ActionIndexCache[] _lowCheerActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_cheering_low_01,
			ActionIndexCache.act_cheering_low_02,
			ActionIndexCache.act_cheering_low_03,
			ActionIndexCache.act_cheering_low_04,
			ActionIndexCache.act_cheering_low_05,
			ActionIndexCache.act_cheering_low_06,
			ActionIndexCache.act_cheering_low_07,
			ActionIndexCache.act_cheering_low_08,
			ActionIndexCache.act_cheering_low_09,
			ActionIndexCache.act_cheering_low_10
		};

		// Token: 0x04000D64 RID: 3428
		private readonly ActionIndexCache[] _midCheerActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_cheer_1,
			ActionIndexCache.act_cheer_2,
			ActionIndexCache.act_cheer_3,
			ActionIndexCache.act_cheer_4
		};

		// Token: 0x04000D65 RID: 3429
		private readonly ActionIndexCache[] _highCheerActions = new ActionIndexCache[]
		{
			ActionIndexCache.act_cheering_high_01,
			ActionIndexCache.act_cheering_high_02,
			ActionIndexCache.act_cheering_high_03,
			ActionIndexCache.act_cheering_high_04,
			ActionIndexCache.act_cheering_high_05,
			ActionIndexCache.act_cheering_high_06,
			ActionIndexCache.act_cheering_high_07,
			ActionIndexCache.act_cheering_high_08
		};

		// Token: 0x04000D66 RID: 3430
		private ActionIndexCache[] _selectedCheerActions;

		// Token: 0x04000D67 RID: 3431
		private List<AgentVictoryLogic.CheeringAgent> _cheeringAgents;

		// Token: 0x04000D68 RID: 3432
		private bool _isInRetreat;

		// Token: 0x02000545 RID: 1349
		public enum CheerActionGroupEnum
		{
			// Token: 0x04001DB8 RID: 7608
			None,
			// Token: 0x04001DB9 RID: 7609
			LowCheerActions,
			// Token: 0x04001DBA RID: 7610
			MidCheerActions,
			// Token: 0x04001DBB RID: 7611
			HighCheerActions
		}

		// Token: 0x02000546 RID: 1350
		public struct CheerReactionTimeSettings
		{
			// Token: 0x06003C81 RID: 15489 RVA: 0x000F1418 File Offset: 0x000EF618
			public CheerReactionTimeSettings(float minDuration, float maxDuration)
			{
				this.MinDuration = minDuration;
				this.MaxDuration = maxDuration;
			}

			// Token: 0x04001DBC RID: 7612
			public readonly float MinDuration;

			// Token: 0x04001DBD RID: 7613
			public readonly float MaxDuration;
		}

		// Token: 0x02000547 RID: 1351
		private class CheeringAgent
		{
			// Token: 0x17000A54 RID: 2644
			// (get) Token: 0x06003C82 RID: 15490 RVA: 0x000F1428 File Offset: 0x000EF628
			// (set) Token: 0x06003C83 RID: 15491 RVA: 0x000F1430 File Offset: 0x000EF630
			public bool GotOrderRecently { get; private set; }

			// Token: 0x17000A55 RID: 2645
			// (get) Token: 0x06003C84 RID: 15492 RVA: 0x000F1439 File Offset: 0x000EF639
			// (set) Token: 0x06003C85 RID: 15493 RVA: 0x000F1441 File Offset: 0x000EF641
			public bool IsCheeringPaused { get; private set; }

			// Token: 0x06003C86 RID: 15494 RVA: 0x000F144A File Offset: 0x000EF64A
			public CheeringAgent(Agent agent, bool isCheeringOnRetreat)
			{
				this.Agent = agent;
				this.IsCheeringOnRetreat = isCheeringOnRetreat;
			}

			// Token: 0x06003C87 RID: 15495 RVA: 0x000F1460 File Offset: 0x000EF660
			public void OrderReceived()
			{
				this.GotOrderRecently = true;
			}

			// Token: 0x06003C88 RID: 15496 RVA: 0x000F1469 File Offset: 0x000EF669
			internal void UpdatePauseState(bool shouldCheeringBePaused)
			{
				this.IsCheeringPaused = shouldCheeringBePaused;
			}

			// Token: 0x04001DBE RID: 7614
			public readonly Agent Agent;

			// Token: 0x04001DBF RID: 7615
			public readonly bool IsCheeringOnRetreat;
		}
	}
}
