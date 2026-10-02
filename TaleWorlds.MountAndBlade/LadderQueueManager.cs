using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000349 RID: 841
	public class LadderQueueManager : MissionObject
	{
		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000B8ED8 File Offset: 0x000B70D8
		// (set) Token: 0x06002F5B RID: 12123 RVA: 0x000B8EE0 File Offset: 0x000B70E0
		public bool IsDeactivated { get; private set; }

		// Token: 0x06002F5C RID: 12124 RVA: 0x000B8EE9 File Offset: 0x000B70E9
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000B8EFD File Offset: 0x000B70FD
		public void DeactivateImmediate()
		{
			this.IsDeactivated = true;
			this._deactivationDelayTimerElapsed = true;
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000B8F10 File Offset: 0x000B7110
		public void Deactivate()
		{
			this.IsDeactivated = true;
			this._deactivateTimer.Reset(Mission.Current.CurrentTime, 10f);
			int num = Math.Min(2, this._queuedAgentCount);
			int num2 = 0;
			int num3 = 0;
			while (num3 < this._queuedAgents.Count && num > 0)
			{
				if (this._queuedAgents[num3] != null)
				{
					num2++;
					if (num2 == num)
					{
						this.RemoveAgentFromQueueAtIndex(num3);
						num--;
						num3 = -1;
						num2 = 0;
					}
				}
				num3++;
			}
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000B8F8C File Offset: 0x000B718C
		public void Activate()
		{
			this.IsDeactivated = false;
			this._deactivationDelayTimerElapsed = false;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000B8F9C File Offset: 0x000B719C
		public void Initialize(int managedNavigationFaceId, MatrixFrame managedFrame, Vec3 managedDirection, BattleSideEnum managedSide, int maxUserCount, float arcAngle, float queueBeginDistance, float queueRowSize, float costPerRow, float baseCost, bool blockUsage, float agentSpacing, float zDifferenceToStopUsing, float distanceToStopUsing2d, bool doesManageMultipleIDs, int managedNavigationFaceAlternateID1, int managedNavigationFaceAlternateID2, int maxClimberCount, int maxRunnerCount)
		{
			this.ManagedNavigationFaceId = managedNavigationFaceId;
			this._managedEntitialFrame = managedFrame;
			this._managedEntitialDirection = managedDirection.AsVec2.Normalized();
			MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
			this._managedGlobalFrame = matrixFrame.TransformToParent(in managedFrame);
			this._managedGlobalWorldPosition = new WorldPosition(base.GameEntity.GetScenePointer(), UIntPtr.Zero, this._managedGlobalFrame.origin, false);
			this._managedGlobalWorldPosition.GetGroundVec3();
			matrixFrame = base.GameEntity.GetGlobalFrame();
			this._managedGlobalDirection = matrixFrame.rotation.TransformToParent(in managedDirection).AsVec2.Normalized();
			this._lastCachedGameEntityGlobalPosition = base.GameEntity.GetGlobalFrame().origin;
			this._managedSide = managedSide;
			this._maxUserCount = maxUserCount;
			this._arcAngle = arcAngle;
			this._queueBeginDistance = queueBeginDistance;
			this._queueRowSize = queueRowSize;
			this._costPerRow = costPerRow;
			this._baseCost = baseCost;
			this._blockUsage = blockUsage;
			this._agentSpacing = agentSpacing;
			this._zDifferenceToStopUsing = zDifferenceToStopUsing;
			this._distanceToStopUsing2d = distanceToStopUsing2d;
			this._doesManageMultipleIDs = doesManageMultipleIDs;
			this.ManagedNavigationFaceAlternateID1 = managedNavigationFaceAlternateID1;
			this.ManagedNavigationFaceAlternateID2 = managedNavigationFaceAlternateID2;
			this._maxClimberCount = maxClimberCount;
			this._maxRunnerCount = maxRunnerCount;
			this._lastUserCostPenaltyPerLadder = new ValueTuple<float, bool>[3];
			this._deactivateTimer = new Timer(0f, 0f, true);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000B910E File Offset: 0x000B730E
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000B9128 File Offset: 0x000B7328
		private void UpdateGlobalFrameCache()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			if (this._lastCachedGameEntityGlobalPosition != globalFrame.origin)
			{
				this._lastCachedGameEntityGlobalPosition = globalFrame.origin;
				this._managedGlobalFrame = globalFrame.TransformToParent(in this._managedEntitialFrame);
				this._managedGlobalWorldPosition = new WorldPosition(base.GameEntity.GetScenePointer(), UIntPtr.Zero, this._managedGlobalFrame.origin, false);
				this._managedGlobalWorldPosition.GetGroundVec3MT();
				Vec3 vec = new Vec3(this._managedEntitialDirection, 0f, -1f);
				this._managedGlobalDirection = globalFrame.rotation.TransformToParent(in vec).AsVec2.Normalized();
			}
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000B91EC File Offset: 0x000B73EC
		private void OnTickParallelAux(float dt)
		{
			if (this.IsDeactivated && !this._deactivationDelayTimerElapsed && this._deactivateTimer.Check(Mission.Current.CurrentTime))
			{
				this._deactivationDelayTimerElapsed = true;
			}
			if (this._deactivationDelayTimerElapsed)
			{
				this._userAgents.Clear();
				for (int i = 0; i < this._queuedAgents.Count; i++)
				{
					if (this._queuedAgents[i] != null && this._queuedAgents[i].IsActive())
					{
						this.RemoveAgentFromQueueAtIndex(i);
					}
				}
				this._queuedAgents.Clear();
				return;
			}
			this.UpdateGlobalFrameCache();
			Vec3 groundVec = this._managedGlobalWorldPosition.GetGroundVec3();
			this._timeSinceLastUpdate += dt;
			if (this._timeSinceLastUpdate < this._updatePeriod)
			{
				return;
			}
			if (this._neighborLadderQueueManager != null && (float)this._neighborLadderQueueManager._queuedAgentCount < (float)this._queuedAgentCount * 0.4f && this._neighborLadderQueueManager.CostAddition < this.CostAddition * 0.6667f)
			{
				this.FlushQueueManager();
			}
			this._usingAgentResetTime -= this._timeSinceLastUpdate;
			this._timeSinceLastUpdate = 0f;
			this._updatePeriod = 0.2f + MBRandom.RandomFloat * 0.1f;
			StackArray.StackArray3Float stackArray3Float = default(StackArray.StackArray3Float);
			int num = 0;
			for (int j = this._userAgents.Count - 1; j >= 0; j--)
			{
				Agent agent = this._userAgents[j];
				bool flag = false;
				int currentNavigationFaceId = agent.GetCurrentNavigationFaceId();
				float num2 = ((this._zDifferenceToStopUsing > 0.01f) ? ((agent.Position.z - groundVec.z) / this._zDifferenceToStopUsing) : 1.01f);
				if (!agent.IsActive())
				{
					flag = true;
				}
				else if (this._usingAgentResetTime < 0f && (num2 > 1f || (agent.Position.AsVec2 - groundVec.AsVec2).LengthSquared > this._distanceToStopUsing2d * this._distanceToStopUsing2d))
				{
					if (currentNavigationFaceId == this.ManagedNavigationFaceId || (this._doesManageMultipleIDs && (currentNavigationFaceId == this.ManagedNavigationFaceAlternateID1 || currentNavigationFaceId == this.ManagedNavigationFaceAlternateID2)))
					{
						flag = true;
					}
					else if (!this.ShouldAgentUseTheLadder(agent))
					{
						flag = true;
					}
				}
				if (flag)
				{
					HumanAIComponent humanAIComponent = this._userAgents[j].HumanAIComponent;
					if (humanAIComponent != null)
					{
						humanAIComponent.AdjustSpeedLimit(this._userAgents[j], -1f, false);
					}
					this._userAgents[j].SetIsLadderQueueUsing(false);
					this._userAgents.RemoveAt(j);
				}
				else
				{
					bool flag2 = false;
					if (currentNavigationFaceId == this.ManagedNavigationFaceId)
					{
						ref StackArray.StackArray3Float ptr = ref stackArray3Float;
						ptr[0] = ptr[0] + ((num2 < 1f) ? (1f - num2 * num2 * num2) : 0f);
						num++;
						flag2 = true;
					}
					else if (this._doesManageMultipleIDs)
					{
						if (currentNavigationFaceId == this.ManagedNavigationFaceAlternateID1)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[1] = ptr[1] + ((num2 < 1f) ? (1f - num2 * num2 * num2) : 0f);
							num++;
							flag2 = true;
						}
						else if (currentNavigationFaceId == this.ManagedNavigationFaceAlternateID2)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[2] = ptr[2] + ((num2 < 1f) ? (1f - num2 * num2 * num2) : 0f);
							num++;
							flag2 = true;
						}
					}
					if (!flag2)
					{
						if (this._userAgents[j].HasPathThroughNavigationFaceIdFromDirectionMT(this.ManagedNavigationFaceId, this._managedGlobalDirection))
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[0] = ptr[0] + 0.3f;
						}
						else if (this._userAgents[j].HasPathThroughNavigationFaceIdFromDirectionMT(this.ManagedNavigationFaceAlternateID1, this._managedGlobalDirection))
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[1] = ptr[1] + 0.3f;
						}
						else if (this._userAgents[j].HasPathThroughNavigationFaceIdFromDirectionMT(this.ManagedNavigationFaceAlternateID2, this._managedGlobalDirection))
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[2] = ptr[2] + 0.3f;
						}
					}
				}
			}
			if (this._neighborLadderQueueManager != null)
			{
				for (int k = this._neighborLadderQueueManager._userAgents.Count - 1; k >= 0; k--)
				{
					int currentNavigationFaceId2 = this._neighborLadderQueueManager._userAgents[k].GetCurrentNavigationFaceId();
					if (currentNavigationFaceId2 == this.ManagedNavigationFaceId)
					{
						ref StackArray.StackArray3Float ptr = ref stackArray3Float;
						ptr[0] = ptr[0] + 0.3f;
						num++;
					}
					else if (this._doesManageMultipleIDs)
					{
						if (currentNavigationFaceId2 == this.ManagedNavigationFaceAlternateID1)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[1] = ptr[1] + 0.3f;
							num++;
						}
						else if (currentNavigationFaceId2 == this.ManagedNavigationFaceAlternateID2)
						{
							ref StackArray.StackArray3Float ptr = ref stackArray3Float;
							ptr[2] = ptr[2] + 0.3f;
							num++;
						}
					}
				}
			}
			for (int l = 0; l < 3; l++)
			{
				if (!this._lastUserCostPenaltyPerLadder[l].Item1.ApproximatelyEqualsTo(stackArray3Float[l], 1E-05f))
				{
					this._lastUserCostPenaltyPerLadder[l].Item1 = stackArray3Float[l];
					this._lastUserCostPenaltyPerLadder[l].Item2 = true;
				}
			}
			for (int m = this._queuedAgents.Count - 1; m >= 0; m--)
			{
				if (this._queuedAgents[m] != null)
				{
					if (!this.ConditionsAreMet(this._queuedAgents[m], Agent.AIScriptedFrameFlags.GoToPosition))
					{
						this.RemoveAgentFromQueueAtIndex(m);
					}
					else
					{
						float num3 = MBRandom.RandomFloat * (float)this._maxUserCount;
						if (num3 > 0.7f)
						{
							int num4;
							int num5;
							this.GetParentIndicesForQueueIndex(m, out num4, out num5);
							if (num4 >= 0 && this._queuedAgents[num4] == null && num3 > ((num5 >= 0) ? 0.85f : 0.7f))
							{
								this.MoveAgentFromQueueIndexToQueueIndex(m, num4);
							}
							else if (num5 >= 0 && this._queuedAgents[num5] == null)
							{
								this.MoveAgentFromQueueIndexToQueueIndex(m, num5);
							}
						}
					}
				}
			}
			int num6 = this._queuedAgents.Count - 1;
			while (num6 >= 0 && this._queuedAgents[num6] == null)
			{
				this._queuedAgents.RemoveAt(num6--);
			}
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, groundVec.AsVec2, 30f, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (this.ConditionsAreMet(lastFoundAgent, Agent.AIScriptedFrameFlags.None) && lastFoundAgent.Position.DistanceSquared(groundVec) < 900f && !this._queuedAgents.Contains(lastFoundAgent) && lastFoundAgent.HasPathThroughNavigationFacesIDFromDirectionMT(this.ManagedNavigationFaceId, this.ManagedNavigationFaceAlternateID1, this.ManagedNavigationFaceAlternateID2, Vec2.Zero))
				{
					if (this._neighborLadderQueueManager == null)
					{
						this.AddAgentToQueue(lastFoundAgent);
					}
					else if (!this._neighborLadderQueueManager._userAgents.Contains(lastFoundAgent))
					{
						this.AddAgentToQueue(lastFoundAgent);
					}
					else
					{
						lastFoundAgent.SetIsLadderQueueUsing(true);
						this._userAgents.Add(lastFoundAgent);
					}
				}
				AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
			}
			int num7 = this._userAgents.Count - num;
			int num8 = Math.Min(this._maxClimberCount - num, this._maxRunnerCount - num7);
			if (!this._blockUsage && num8 > 0)
			{
				float num9 = float.MaxValue;
				int num10 = -1;
				for (int n = 0; n < this._queuedAgents.Count; n++)
				{
					if (this._queuedAgents[n] != null)
					{
						float lengthSquared = (this._queuedAgents[n].Position - groundVec).LengthSquared;
						if (lengthSquared < num9)
						{
							num9 = lengthSquared;
							num10 = n;
						}
					}
				}
				if (num10 >= 0)
				{
					this._queuedAgents[num10].SetIsLadderQueueUsing(true);
					this._userAgents.Add(this._queuedAgents[num10]);
					this._queuedAgents[num10].HumanAIComponent.AdjustSpeedLimit(this._queuedAgents[num10], 0.2f, true);
					this._usingAgentResetTime = 2f;
					this.RemoveAgentFromQueueAtIndex(num10);
				}
			}
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000B9A44 File Offset: 0x000B7C44
		protected internal override void OnTickParallel(float dt)
		{
			if (this._neighborLadderQueueManager == null)
			{
				this.OnTickParallelAux(dt);
				return;
			}
			LadderQueueManager ladderQueueManager = ((base.Id.Id < this._neighborLadderQueueManager.Id.Id) ? this : this._neighborLadderQueueManager);
			lock (ladderQueueManager)
			{
				this.OnTickParallelAux(dt);
			}
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000B9AB8 File Offset: 0x000B7CB8
		protected internal override void OnTick(float dt)
		{
			if (!GameNetwork.IsClientOrReplay && !this.IsDeactivated && !this._blockUsage)
			{
				if (this.ManagedNavigationFaceId > 1 && this._lastUserCostPenaltyPerLadder[0].Item2)
				{
					this._lastUserCostPenaltyPerLadder[0].Item2 = false;
					this.CostAddition = this.GetNavigationFaceCostPerClimber(this._lastUserCostPenaltyPerLadder[0].Item1);
					Mission.Current.SetNavigationFaceCostWithIdAroundPosition(this.ManagedNavigationFaceId, this._managedGlobalWorldPosition.GetGroundVec3(), this.CostAddition);
				}
				if (this.ManagedNavigationFaceAlternateID1 > 1 && this._lastUserCostPenaltyPerLadder[1].Item2)
				{
					this._lastUserCostPenaltyPerLadder[1].Item2 = false;
					Mission.Current.SetNavigationFaceCostWithIdAroundPosition(this.ManagedNavigationFaceAlternateID1, this._managedGlobalWorldPosition.GetGroundVec3(), this.GetNavigationFaceCostPerClimber(this._lastUserCostPenaltyPerLadder[1].Item1));
				}
				if (this.ManagedNavigationFaceAlternateID2 > 1 && this._lastUserCostPenaltyPerLadder[2].Item2)
				{
					this._lastUserCostPenaltyPerLadder[2].Item2 = false;
					Mission.Current.SetNavigationFaceCostWithIdAroundPosition(this.ManagedNavigationFaceAlternateID2, this._managedGlobalWorldPosition.GetGroundVec3(), this.GetNavigationFaceCostPerClimber(this._lastUserCostPenaltyPerLadder[2].Item1));
				}
			}
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000B9C14 File Offset: 0x000B7E14
		private bool ConditionsAreMet(Agent agent, Agent.AIScriptedFrameFlags flags)
		{
			return agent.IsAIControlled && agent.IsActive() && agent.Team != null && agent.Team.Side == this._managedSide && agent.MovementLockedState == AgentMovementLockedState.None && !agent.IsUsingGameObject && !agent.InteractingWithAnyGameObject() && !agent.IsDetachedFromFormation && agent.Position.z - this._managedGlobalWorldPosition.GetGroundZ() < this._zDifferenceToStopUsing && !this._userAgents.Contains(agent) && agent.GetScriptedFlags() == flags && agent.GetCurrentNavigationFaceId() != this.ManagedNavigationFaceId && (!this._doesManageMultipleIDs || (agent.GetCurrentNavigationFaceId() != this.ManagedNavigationFaceAlternateID1 && agent.GetCurrentNavigationFaceId() != this.ManagedNavigationFaceAlternateID2));
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000B9CEF File Offset: 0x000B7EEF
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this._userAgents.Clear();
			this.IsDeactivated = true;
			this._deactivationDelayTimerElapsed = true;
			this._queuedAgents.Clear();
			this._queuedAgentCount = 0;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000B9D24 File Offset: 0x000B7F24
		private void GetParentIndicesForQueueIndex(int queueIndex, out int parentIndex1, out int parentIndex2)
		{
			parentIndex1 = -1;
			parentIndex2 = -1;
			Vec2i coordinatesForQueueIndex = this.GetCoordinatesForQueueIndex(queueIndex);
			int num = coordinatesForQueueIndex.Y - 1;
			if (num >= 0)
			{
				int num2 = MathF.Max(this.GetRowSize(num) - 1, 1);
				int num3 = MathF.Max(this.GetRowSize(coordinatesForQueueIndex.Y) - 1, 1);
				float num4 = (float)coordinatesForQueueIndex.X * (float)num2 / (float)num3;
				parentIndex1 = (int)num4;
				float num5 = MathF.Abs(num4 - (float)parentIndex1);
				if (num5 > 0.2f)
				{
					if (num5 > 0.8f)
					{
						parentIndex1++;
					}
					else
					{
						parentIndex2 = parentIndex1 + 1;
					}
				}
				parentIndex1 = this.GetQueueIndexForCoordinates(new Vec2i(parentIndex1, num));
				if (parentIndex2 >= 0)
				{
					parentIndex2 = this.GetQueueIndexForCoordinates(new Vec2i(parentIndex2, num));
				}
			}
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000B9DDC File Offset: 0x000B7FDC
		private float GetScoreForAddingAgentToQueueIndex(Vec3 agentPosition, int queueIndex, out int scoreOfQueueIndex)
		{
			scoreOfQueueIndex = queueIndex;
			float num = float.MinValue;
			if (this._queuedAgents.Count <= queueIndex || this._queuedAgents[queueIndex] == null)
			{
				int num2;
				int num3;
				this.GetParentIndicesForQueueIndex(queueIndex, out num2, out num3);
				if (num2 < 0 || (this._queuedAgents.Count > num2 && this._queuedAgents[num2] != null) || (num3 >= 0 && this._queuedAgents.Count > num3 && this._queuedAgents[num3] != null))
				{
					Vec2i coordinatesForQueueIndex = this.GetCoordinatesForQueueIndex(queueIndex);
					num = (float)coordinatesForQueueIndex.Y * this._queueRowSize * -3f;
					num -= (agentPosition.AsVec2 - this.GetQueuePositionForCoordinates(coordinatesForQueueIndex, -1).AsVec2).Length;
				}
				if (num2 >= 0 && (this._queuedAgents.Count <= num2 || this._queuedAgents[num2] == null))
				{
					int num4;
					float scoreForAddingAgentToQueueIndex = this.GetScoreForAddingAgentToQueueIndex(agentPosition, num2, out num4);
					if (num < scoreForAddingAgentToQueueIndex)
					{
						scoreOfQueueIndex = num4;
						num = scoreForAddingAgentToQueueIndex;
					}
				}
				if (num3 >= 0 && (this._queuedAgents.Count <= num3 || this._queuedAgents[num3] == null))
				{
					int num5;
					float scoreForAddingAgentToQueueIndex2 = this.GetScoreForAddingAgentToQueueIndex(agentPosition, num3, out num5);
					if (num < scoreForAddingAgentToQueueIndex2)
					{
						scoreOfQueueIndex = num5;
						num = scoreForAddingAgentToQueueIndex2;
					}
				}
			}
			return num;
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000B9F14 File Offset: 0x000B8114
		private void AddAgentToQueue(Agent agent)
		{
			int y = this.GetCoordinatesForQueueIndex(this._queuedAgents.Count).Y;
			int rowSize = this.GetRowSize(y);
			Vec3 position = agent.Position;
			int num = -1;
			float num2 = float.MinValue;
			for (int i = 0; i < rowSize; i++)
			{
				int num3;
				float scoreForAddingAgentToQueueIndex = this.GetScoreForAddingAgentToQueueIndex(position, this.GetQueueIndexForCoordinates(new Vec2i(i, y)), out num3);
				if (scoreForAddingAgentToQueueIndex > num2)
				{
					num2 = scoreForAddingAgentToQueueIndex;
					num = num3;
				}
			}
			while (this._queuedAgents.Count <= num)
			{
				this._queuedAgents.Add(null);
			}
			this._queuedAgents[num] = agent;
			WorldPosition queuePositionForIndex = this.GetQueuePositionForIndex(num, agent.Index);
			agent.SetScriptedPosition(ref queuePositionForIndex, false, Agent.AIScriptedFrameFlags.None);
			agent.SetIsInLadderQueue(true);
			this._queuedAgentCount++;
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000B9FE0 File Offset: 0x000B81E0
		private void RemoveAgentFromQueueAtIndex(int queueIndex)
		{
			this._queuedAgentCount--;
			if (!this._queuedAgents[queueIndex].IsUsingGameObject && (!this._queuedAgents[queueIndex].IsAIControlled || !this._queuedAgents[queueIndex].AIMoveToGameObjectIsEnabled()))
			{
				this._queuedAgents[queueIndex].DisableScriptedMovement();
				this._queuedAgents[queueIndex].SetIsInLadderQueue(false);
			}
			this._queuedAgents[queueIndex] = null;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000BA064 File Offset: 0x000B8264
		private float GetNavigationFaceCost(int rowIndex)
		{
			return this._baseCost + (float)MathF.Max(rowIndex - 1, 0) * this._costPerRow;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000BA07E File Offset: 0x000B827E
		private float GetNavigationFaceCostPerClimber(float costPenalty)
		{
			return this._baseCost + costPenalty * this._costPerRow;
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000BA090 File Offset: 0x000B8290
		private void MoveAgentFromQueueIndexToQueueIndex(int fromQueueIndex, int toQueueIndex)
		{
			this._queuedAgents[toQueueIndex] = this._queuedAgents[fromQueueIndex];
			this._queuedAgents[fromQueueIndex] = null;
			WorldPosition queuePositionForIndex = this.GetQueuePositionForIndex(toQueueIndex, this._queuedAgents[toQueueIndex].Index);
			this._queuedAgents[toQueueIndex].SetScriptedPosition(ref queuePositionForIndex, false, Agent.AIScriptedFrameFlags.None);
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000BA0F0 File Offset: 0x000B82F0
		private int GetRowSize(int rowIndex)
		{
			float num = this._arcAngle * (this._queueBeginDistance + this._queueRowSize * (float)rowIndex);
			return 1 + (int)(num / this._agentSpacing);
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000BA120 File Offset: 0x000B8320
		private int GetQueueIndexForCoordinates(Vec2i coordinates)
		{
			int num = coordinates.X;
			for (int i = 0; i < coordinates.Y; i++)
			{
				num += this.GetRowSize(i);
			}
			return num;
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000BA150 File Offset: 0x000B8350
		private Vec2i GetCoordinatesForQueueIndex(int queueIndex)
		{
			Vec2i vec2i = default(Vec2i);
			for (;;)
			{
				int rowSize = this.GetRowSize(vec2i.Y);
				if (rowSize > queueIndex)
				{
					break;
				}
				queueIndex -= rowSize;
				vec2i.Y++;
			}
			vec2i.X = queueIndex;
			return vec2i;
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000BA194 File Offset: 0x000B8394
		private WorldPosition GetQueuePositionForCoordinates(Vec2i coordinates, int randomSeed)
		{
			MatrixFrame managedGlobalFrame = this._managedGlobalFrame;
			WorldPosition managedGlobalWorldPosition = this._managedGlobalWorldPosition;
			float num = 0f;
			int rowSize = this.GetRowSize(coordinates.Y);
			if (rowSize > 1)
			{
				num = this._arcAngle * ((float)coordinates.X / (float)(rowSize - 1) - 0.5f);
			}
			managedGlobalFrame.rotation.RotateAboutForward(num);
			managedGlobalFrame.origin += managedGlobalFrame.rotation.u * (this._queueBeginDistance + this._queueRowSize * (float)coordinates.Y);
			if (randomSeed >= 0)
			{
				Random random = new Random(coordinates.X * 100000 + coordinates.Y * 10000000 + randomSeed);
				managedGlobalFrame.rotation.RotateAboutForward(random.NextFloat() * 3.1415927f * 2f);
				managedGlobalFrame.origin += managedGlobalFrame.rotation.u * random.NextFloat() * 0.3f;
			}
			managedGlobalWorldPosition.SetVec2(managedGlobalFrame.origin.AsVec2);
			return managedGlobalWorldPosition;
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000BA2BA File Offset: 0x000B84BA
		private WorldPosition GetQueuePositionForIndex(int queueIndex, int randomSeed)
		{
			return this.GetQueuePositionForCoordinates(this.GetCoordinatesForQueueIndex(queueIndex), randomSeed);
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000BA2CC File Offset: 0x000B84CC
		public void FlushQueueManager()
		{
			int num = this._queuedAgentCount / 2;
			for (int i = this._queuedAgents.Count - 1; i >= num; i--)
			{
				if (this._queuedAgents[i] != null)
				{
					this.RemoveAgentFromQueueAtIndex(i);
				}
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000BA30F File Offset: 0x000B850F
		public void AssignNeighborQueueManager(LadderQueueManager neighborLadderQueueManager)
		{
			this._neighborLadderQueueManager = neighborLadderQueueManager;
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000BA318 File Offset: 0x000B8518
		private bool IsFormationPositionOtherSideOfTheCastle(Agent agent)
		{
			UIntPtr navMesh = agent.Formation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMesh();
			UIntPtr navMesh2 = agent.GetWorldPosition().GetNavMesh();
			ref PathFaceRecord pathFaceRecordFromNavMeshFacePointer = base.Scene.GetPathFaceRecordFromNavMeshFacePointer(navMesh);
			PathFaceRecord pathFaceRecordFromNavMeshFacePointer2 = base.Scene.GetPathFaceRecordFromNavMeshFacePointer(navMesh2);
			return pathFaceRecordFromNavMeshFacePointer.FaceGroupIndex % 10 != pathFaceRecordFromNavMeshFacePointer2.FaceGroupIndex % 10;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000BA378 File Offset: 0x000B8578
		private bool ShouldAgentUseTheLadder(Agent agent)
		{
			if (!agent.IsFormationFrameEnabled)
			{
				return agent.HasPathThroughNavigationFacesIDFromDirectionMT(this.ManagedNavigationFaceId, this.ManagedNavigationFaceAlternateID1, this.ManagedNavigationFaceAlternateID2, Vec2.Zero);
			}
			return this.IsFormationPositionOtherSideOfTheCastle(agent);
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000BA3A7 File Offset: 0x000B85A7
		public void OnFormationFrameChanged(Agent agent, bool hasFrame, WorldPosition frame)
		{
			if (agent.IsInLadderQueue && this._queuedAgents.Contains(agent) && (!this.ConditionsAreMet(agent, Agent.AIScriptedFrameFlags.GoToPosition) || !this.ShouldAgentUseTheLadder(agent)))
			{
				this.RemoveAgentFromQueueAtIndex(this._queuedAgents.IndexOf(agent));
			}
		}

		// Token: 0x04001328 RID: 4904
		public int ManagedNavigationFaceId;

		// Token: 0x04001329 RID: 4905
		public int ManagedNavigationFaceAlternateID1;

		// Token: 0x0400132A RID: 4906
		public int ManagedNavigationFaceAlternateID2;

		// Token: 0x0400132B RID: 4907
		public float CostAddition;

		// Token: 0x0400132C RID: 4908
		private readonly List<Agent> _userAgents = new List<Agent>();

		// Token: 0x0400132D RID: 4909
		private readonly List<Agent> _queuedAgents = new List<Agent>();

		// Token: 0x0400132E RID: 4910
		private MatrixFrame _managedEntitialFrame;

		// Token: 0x0400132F RID: 4911
		private Vec2 _managedEntitialDirection;

		// Token: 0x04001330 RID: 4912
		private Vec3 _lastCachedGameEntityGlobalPosition;

		// Token: 0x04001331 RID: 4913
		private MatrixFrame _managedGlobalFrame;

		// Token: 0x04001332 RID: 4914
		private WorldPosition _managedGlobalWorldPosition;

		// Token: 0x04001333 RID: 4915
		private Vec2 _managedGlobalDirection;

		// Token: 0x04001334 RID: 4916
		private BattleSideEnum _managedSide;

		// Token: 0x04001335 RID: 4917
		private bool _blockUsage;

		// Token: 0x04001336 RID: 4918
		private int _maxUserCount;

		// Token: 0x04001337 RID: 4919
		private int _queuedAgentCount;

		// Token: 0x04001338 RID: 4920
		private float _arcAngle = 2.3561945f;

		// Token: 0x04001339 RID: 4921
		private float _queueBeginDistance = 1f;

		// Token: 0x0400133A RID: 4922
		private float _queueRowSize = 0.8f;

		// Token: 0x0400133B RID: 4923
		private float _agentSpacing = 1f;

		// Token: 0x0400133C RID: 4924
		private float _timeSinceLastUpdate;

		// Token: 0x0400133D RID: 4925
		private float _updatePeriod;

		// Token: 0x0400133E RID: 4926
		private float _usingAgentResetTime;

		// Token: 0x0400133F RID: 4927
		private float _costPerRow;

		// Token: 0x04001340 RID: 4928
		private float _baseCost;

		// Token: 0x04001341 RID: 4929
		private float _zDifferenceToStopUsing = 2f;

		// Token: 0x04001342 RID: 4930
		private float _distanceToStopUsing2d = 5f;

		// Token: 0x04001343 RID: 4931
		private bool _doesManageMultipleIDs;

		// Token: 0x04001344 RID: 4932
		private int _maxClimberCount = 18;

		// Token: 0x04001345 RID: 4933
		private int _maxRunnerCount = 6;

		// Token: 0x04001346 RID: 4934
		private Timer _deactivateTimer;

		// Token: 0x04001347 RID: 4935
		private bool _deactivationDelayTimerElapsed = true;

		// Token: 0x04001348 RID: 4936
		private LadderQueueManager _neighborLadderQueueManager;

		// Token: 0x04001349 RID: 4937
		private ValueTuple<float, bool>[] _lastUserCostPenaltyPerLadder;
	}
}
