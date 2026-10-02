using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects.Usables;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017F RID: 383
	public class ClimbingMachineDetachment : IDetachment
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0004B8F4 File Offset: 0x00049AF4
		public MBReadOnlyList<Formation> UserFormations
		{
			get
			{
				return this._userFormations;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0004B8FC File Offset: 0x00049AFC
		public bool IsLoose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0004B8FF File Offset: 0x00049AFF
		// (set) Token: 0x06001460 RID: 5216 RVA: 0x0004B907 File Offset: 0x00049B07
		public bool IsActive { get; private set; }

		// Token: 0x06001461 RID: 5217 RVA: 0x0004B910 File Offset: 0x00049B10
		public ClimbingMachineDetachment(in MBList<ClimbingMachine> climbingMachines)
		{
			this._agents = new List<Agent>();
			this._userFormations = new MBList<Formation>();
			this._climbingMachines = climbingMachines;
			this._climberAgents = new MBList<Agent>();
			this.IsActive = true;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004B948 File Offset: 0x00049B48
		public void Deactivate()
		{
			this.IsActive = false;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0004B951 File Offset: 0x00049B51
		public void AddAgent(Agent agent, int slotIndex, Agent.AIScriptedFrameFlags customFlags = Agent.AIScriptedFrameFlags.None)
		{
			this._agents.Add(agent);
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0004B95F File Offset: 0x00049B5F
		public void AddAgentAtSlotIndex(Agent agent, int slotIndex)
		{
			this.AddAgent(agent, slotIndex, Agent.AIScriptedFrameFlags.None);
			Formation formation = agent.Formation;
			if (formation != null)
			{
				formation.DetachUnit(agent, true);
			}
			agent.Detachment = this;
			agent.SetDetachmentWeight(1f);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0004B98F File Offset: 0x00049B8F
		void IDetachment.FormationStartUsing(Formation formation)
		{
			this._userFormations.Add(formation);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0004B99D File Offset: 0x00049B9D
		void IDetachment.FormationStopUsing(Formation formation)
		{
			this._userFormations.Remove(formation);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0004B9AC File Offset: 0x00049BAC
		public bool IsUsedByFormation(Formation formation)
		{
			return this._userFormations.Contains(formation);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0004B9BA File Offset: 0x00049BBA
		Agent IDetachment.GetMovingAgentAtSlotIndex(int slotIndex)
		{
			if (slotIndex >= this._agents.Count)
			{
				return null;
			}
			return this._agents[slotIndex];
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0004B9D8 File Offset: 0x00049BD8
		void IDetachment.GetSlotIndexWeightTuples(List<ValueTuple<int, float>> slotIndexWeightTuples)
		{
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004B9DA File Offset: 0x00049BDA
		bool IDetachment.IsSlotAtIndexAvailableForAgent(int slotIndex, Agent agent)
		{
			return false;
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0004B9DD File Offset: 0x00049BDD
		bool IDetachment.IsAgentEligible(Agent agent)
		{
			return agent.Detachment == this;
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0004B9E8 File Offset: 0x00049BE8
		void IDetachment.UnmarkDetachment()
		{
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004B9EA File Offset: 0x00049BEA
		bool IDetachment.IsDetachmentRecentlyEvaluated()
		{
			return true;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0004B9ED File Offset: 0x00049BED
		void IDetachment.MarkSlotAtIndex(int slotIndex)
		{
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0004B9EF File Offset: 0x00049BEF
		bool IDetachment.IsAgentUsingOrInterested(Agent agent)
		{
			return this._agents.Contains(agent);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0004BA00 File Offset: 0x00049C00
		void IDetachment.OnFormationLeave(Formation formation)
		{
			for (int i = this._agents.Count - 1; i >= 0; i--)
			{
				Agent agent = this._agents[i];
				if (agent.Formation == formation && !agent.IsPlayerControlled)
				{
					((IDetachment)this).RemoveAgent(agent);
					formation.AttachUnit(agent);
				}
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0004BA51 File Offset: 0x00049C51
		public bool IsStandingPointAvailableForAgent(Agent agent)
		{
			return false;
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0004BA54 File Offset: 0x00049C54
		public List<float> GetTemplateCostsOfAgent(Agent candidate, List<float> oldValue)
		{
			return oldValue;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0004BA57 File Offset: 0x00049C57
		float IDetachment.GetExactCostOfAgentAtSlot(Agent candidate, int slotIndex)
		{
			return float.MaxValue;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0004BA5E File Offset: 0x00049C5E
		public float GetTemplateWeightOfAgent(Agent candidate)
		{
			return float.MaxValue;
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0004BA68 File Offset: 0x00049C68
		public float? GetWeightOfAgentAtNextSlot(List<Agent> newAgents, out Agent match)
		{
			match = null;
			return null;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0004BA84 File Offset: 0x00049C84
		public float? GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> agentTemplateScores, out Agent match)
		{
			match = null;
			return null;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0004BA9D File Offset: 0x00049C9D
		public float? GetWeightOfAgentAtOccupiedSlot(Agent detachedAgent, List<Agent> newAgents, out Agent match)
		{
			match = null;
			return new float?(float.MaxValue);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0004BAAC File Offset: 0x00049CAC
		public void RemoveAgent(Agent agent)
		{
			this._agents.Remove(agent);
			agent.DisableScriptedMovement();
			agent.DisableScriptedCombatMovement();
			this._climberAgents.Remove(agent);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0004BAD4 File Offset: 0x00049CD4
		public int GetNumberOfUsableSlots()
		{
			return int.MaxValue;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0004BADC File Offset: 0x00049CDC
		public WorldFrame? GetAgentFrame(Agent agent)
		{
			return null;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0004BAF4 File Offset: 0x00049CF4
		public float? GetWeightOfNextSlot(BattleSideEnum side)
		{
			return null;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0004BB0A File Offset: 0x00049D0A
		public float GetWeightOfOccupiedSlot(Agent agent)
		{
			return float.MinValue;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0004BB11 File Offset: 0x00049D11
		float IDetachment.GetDetachmentWeight(BattleSideEnum side)
		{
			return float.MinValue;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0004BB18 File Offset: 0x00049D18
		void IDetachment.ResetEvaluation()
		{
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0004BB1A File Offset: 0x00049D1A
		bool IDetachment.IsEvaluated()
		{
			return true;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0004BB1D File Offset: 0x00049D1D
		void IDetachment.SetAsEvaluated()
		{
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0004BB1F File Offset: 0x00049D1F
		float IDetachment.GetDetachmentWeightFromCache()
		{
			return float.MinValue;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0004BB26 File Offset: 0x00049D26
		float IDetachment.ComputeAndCacheDetachmentWeight(BattleSideEnum side)
		{
			return float.MinValue;
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0004BB30 File Offset: 0x00049D30
		public void TickClimbingMachines()
		{
			if (!this.IsActive)
			{
				return;
			}
			if (this._userFormations.Count > 0)
			{
				for (int i = this._userFormations[0].UnitsWithoutLooseDetachedOnes.Count - 1; i >= 0; i--)
				{
					Agent agent;
					if ((agent = this._userFormations[0].UnitsWithoutLooseDetachedOnes[i] as Agent) != null && !agent.IsPlayerControlled && agent.IsDetachableFromFormation && agent.IsInWater() && agent.Formation != null && !this._climberAgents.Contains(agent))
					{
						if (agent.AIMoveToGameObjectIsEnabled() || agent.CurrentlyUsedGameObject != null)
						{
							agent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.None);
						}
						this._climberAgents.Add(agent);
						this.AddAgentAtSlotIndex(agent, 0);
						if (agent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
						{
							agent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
						}
						if (agent.GetOffhandWieldedItemIndex() != EquipmentIndex.None)
						{
							agent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
						}
						if (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack | AgentFlag.CanDefend))
						{
							agent.SetAgentFlags(agent.GetAgentFlags() & ~(AgentFlag.CanAttack | AgentFlag.CanDefend));
						}
					}
				}
				for (int j = this._userFormations[0].DetachedUnits.Count - 1; j >= 0; j--)
				{
					Agent agent2 = this._userFormations[0].DetachedUnits[j];
					if (!agent2.IsPlayerControlled && agent2.IsDetachableFromFormation && agent2.Detachment != this && agent2.IsInWater())
					{
						agent2.Detachment.RemoveAgent(agent2);
						if (agent2.Formation != null && !this._climberAgents.Contains(agent2))
						{
							agent2.Formation.AttachUnit(agent2);
							this._climberAgents.Add(agent2);
							this.AddAgentAtSlotIndex(agent2, 0);
							if (agent2.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
							{
								agent2.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.Instant);
							}
							if (agent2.GetOffhandWieldedItemIndex() != EquipmentIndex.None)
							{
								agent2.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.Instant);
							}
							if (agent2.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack | AgentFlag.CanDefend))
							{
								agent2.SetAgentFlags(agent2.GetAgentFlags() & ~(AgentFlag.CanAttack | AgentFlag.CanDefend));
							}
						}
					}
				}
				for (int k = this._climberAgents.Count - 1; k >= 0; k--)
				{
					Agent agent3 = this._climberAgents[k];
					if (agent3.IsActive())
					{
						if (agent3.IsInWater())
						{
							float num = float.MaxValue;
							ClimbingMachine climbingMachine = null;
							foreach (ClimbingMachine climbingMachine2 in this._climbingMachines)
							{
								float num2 = climbingMachine2.GameEntity.GlobalPosition.DistanceSquared(agent3.Position) * (Vec2.DotProduct((agent3.Position.AsVec2 - climbingMachine2.GameEntity.GlobalPosition.AsVec2).Normalized(), climbingMachine2.PilotStandingPoint.GameEntity.GetGlobalFrame().rotation.f.AsVec2.Normalized()) + 2f);
								if (num2 < num)
								{
									num = num2;
									climbingMachine = climbingMachine2;
								}
							}
							if (climbingMachine != null)
							{
								StandingPoint standingPoint = null;
								foreach (StandingPoint standingPoint2 in climbingMachine.StandingPoints)
								{
									if (!standingPoint2.HasUser && !standingPoint2.IsDeactivated)
									{
										standingPoint = standingPoint2;
										break;
									}
								}
								if (standingPoint != null && agent3.CanReachAndUseObject(standingPoint, standingPoint.GetUserFrameForAgent(agent3).Origin.AsVec2.DistanceSquared(agent3.Position.AsVec2)) && MathF.Abs(standingPoint.GameEntity.GlobalPosition.z - agent3.Position.z) < 1.5f)
								{
									agent3.DisableScriptedMovement();
									agent3.UseGameObject(standingPoint, -1);
								}
								else
								{
									WorldPosition worldPosition = new WorldPosition(climbingMachine.GameEntity.Scene, climbingMachine.GameEntity.GlobalPosition);
									agent3.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.None);
								}
							}
						}
						else if (agent3.CurrentlyUsedGameObject == null)
						{
							this._climberAgents.RemoveAt(k);
							this.RemoveAgent(agent3);
							Formation formation = agent3.Formation;
							if (formation != null)
							{
								formation.AttachUnit(agent3);
							}
						}
					}
					else
					{
						this._climberAgents.RemoveAt(k);
						this.RemoveAgent(agent3);
						Formation formation2 = agent3.Formation;
						if (formation2 != null)
						{
							formation2.AttachUnit(agent3);
						}
					}
				}
			}
		}

		// Token: 0x04000572 RID: 1394
		private const int Capacity = 2147483647;

		// Token: 0x04000573 RID: 1395
		private readonly List<Agent> _agents;

		// Token: 0x04000574 RID: 1396
		private readonly MBList<Formation> _userFormations;

		// Token: 0x04000575 RID: 1397
		private readonly MBReadOnlyList<ClimbingMachine> _climbingMachines;

		// Token: 0x04000576 RID: 1398
		private readonly MBList<Agent> _climberAgents;
	}
}
