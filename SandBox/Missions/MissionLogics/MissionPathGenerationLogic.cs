using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.AgentBehaviors;
using SandBox.Objects;
using SandBox.Objects.AnimationPoints;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Objects;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x0200007B RID: 123
	public class MissionPathGenerationLogic : MissionLogic
	{
		// Token: 0x06000501 RID: 1281 RVA: 0x00020410 File Offset: 0x0001E610
		public MissionPathGenerationLogic(CharacterObject defaultDisguiseCharacter)
		{
			this._defaultDisguiseCharacter = defaultDisguiseCharacter;
			this._selectedPath = null;
			this._nearbyLeftSideUsableMachinesCache = new List<MissionPathGenerationLogic.UsableMachineData>();
			this._nearbyRightSideUsableMachinesCache = new List<MissionPathGenerationLogic.UsableMachineData>();
			this._allTargetAgentPointOfInterest = new List<MissionPathGenerationLogic.PointOfInterestBaseData>();
			this._crossRoadAgentData = new Dictionary<Agent, bool>();
			this._visitBarrelEntities = new List<GameEntity>();
			this._startAndFinishPointPool = new List<GameEntity>();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000204A0 File Offset: 0x0001E6A0
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			if (userAgent.IsMainAgent)
			{
				GameEntity gameEntity = GameEntity.CreateFromWeakEntity(usedObject.GameEntity);
				if (this._visitBarrelEntities.Contains(gameEntity))
				{
					userAgent.SetActionChannel(0, in ActionIndexCache.act_smithing_machine_anvil_start, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					this._visitBarrelEntities.Remove(gameEntity);
				}
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00020510 File Offset: 0x0001E710
		private void SpawnDisguiseAgents()
		{
			foreach (MissionPathGenerationLogic.PointOfInterestBaseData pointOfInterestBaseData in this._selectedPath.Data)
			{
				MissionPathGenerationLogic.CrossRoadScoreData crossRoadScoreData;
				MissionPathGenerationLogic.StandingGuardSpawnData standingGuardSpawnData;
				MissionPathGenerationLogic.VisitPointNodeScoreData visitPointNodeScoreData;
				MissionPathGenerationLogic.LookBackPointData lookBackPointData;
				if ((crossRoadScoreData = pointOfInterestBaseData as MissionPathGenerationLogic.CrossRoadScoreData) != null)
				{
					this.SpawnCrossRoadAgents(crossRoadScoreData);
				}
				else if ((standingGuardSpawnData = pointOfInterestBaseData as MissionPathGenerationLogic.StandingGuardSpawnData) != null)
				{
					this.SpawnStandingGuards(standingGuardSpawnData);
				}
				else if ((visitPointNodeScoreData = pointOfInterestBaseData as MissionPathGenerationLogic.VisitPointNodeScoreData) != null)
				{
					this.SpawnVisitPointGuardsAndBlendPoints(visitPointNodeScoreData, true);
					this._allTargetAgentPointOfInterest.Add(visitPointNodeScoreData);
				}
				else if ((lookBackPointData = pointOfInterestBaseData as MissionPathGenerationLogic.LookBackPointData) != null)
				{
					this._allTargetAgentPointOfInterest.Add(lookBackPointData);
				}
			}
			this._allTargetAgentPointOfInterest = this._allTargetAgentPointOfInterest.OrderBy<MissionPathGenerationLogic.PointOfInterestBaseData, float>((MissionPathGenerationLogic.PointOfInterestBaseData x) => x.GetLocationRatio()).ToList<MissionPathGenerationLogic.PointOfInterestBaseData>();
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000205F8 File Offset: 0x0001E7F8
		private void SpawnVisitPointGuardsAndBlendPoints(MissionPathGenerationLogic.VisitPointNodeScoreData visitPointData, bool useAsBarrelPoint)
		{
			this.FadeOutUserAgentsInUsableMachine(visitPointData.VisitPointData.MissionObject as UsableMachine);
			MatrixFrame globalFrame = visitPointData.VisitPointData.MissionObject.GameEntity.GetGlobalFrame();
			WorldFrame worldFrame = new WorldFrame(globalFrame.rotation, new WorldPosition(visitPointData.VisitPointData.MissionObject.GameEntity.Scene, globalFrame.origin));
			if (useAsBarrelPoint)
			{
				Vec3 groundVec = worldFrame.Origin.GetGroundVec3();
				float num = float.MaxValue;
				Vec3 vec = Vec3.Zero;
				int num2 = 0;
				while ((float)num2 < 360f)
				{
					worldFrame.Rotation.RotateAboutUp(0.017453292f);
					Vec3 lastPointOnNavigationMeshFromWorldPositionToDestination = Mission.Current.Scene.GetLastPointOnNavigationMeshFromWorldPositionToDestination(ref worldFrame.Origin, worldFrame.Origin.AsVec2 + worldFrame.Rotation.f.AsVec2 * 30f);
					float num3 = worldFrame.Origin.AsVec2.Distance(lastPointOnNavigationMeshFromWorldPositionToDestination.AsVec2);
					if (num3 < num)
					{
						num = num3;
						vec = lastPointOnNavigationMeshFromWorldPositionToDestination;
					}
					num2++;
				}
				PathFaceRecord pathFaceRecord = new PathFaceRecord(-1, -1, -1);
				Mission.Current.Scene.GetNavMeshFaceIndex(ref pathFaceRecord, vec, true);
				Vec3 zero = Vec3.Zero;
				Mission.Current.Scene.GetNavMeshCenterPosition(pathFaceRecord.FaceIndex, ref zero);
				worldFrame.Origin.SetVec2(vec.AsVec2 + (zero.AsVec2 - vec.AsVec2) * 0.25f);
				float num4 = Vec3.AngleBetweenTwoVectors(groundVec - vec, worldFrame.Rotation.f);
				worldFrame.Rotation.RotateAboutUp(num4.ToRadians());
				GameEntity gameEntity = GameEntity.Instantiate(Mission.Current.Scene, "disguise_mission_interactable_barrel", worldFrame.ToGroundMatrixFrame(), true);
				this._visitBarrelEntities.Add(gameEntity);
				visitPointData.UsingAsInteractablePoint = true;
				visitPointData.VisitPointData.MissionObject = gameEntity.GetFirstScriptOfType<UsableMissionObject>();
			}
			else
			{
				Vec3 vec2 = worldFrame.Origin.GetGroundVec3() - worldFrame.Rotation.f;
				Agent agent = this._disguiseMissionLogic.SpawnDisguiseMissionAgentInternal(this._defaultDisguiseCharacter, vec2, worldFrame.Rotation.f.AsVec2.Normalized(), "_hideout_bandit", true);
				UsableMachine usableMachine = visitPointData.VisitPointData.MissionObject as UsableMachine;
				AnimationPoint animationPoint;
				if (usableMachine.StandingPoints.Any<StandingPoint>() && (animationPoint = usableMachine.StandingPoints[0] as AnimationPoint) != null)
				{
					Agent agent2 = agent;
					int num5 = 0;
					ActionIndexCache actionIndexCache = ActionIndexCache.Create(animationPoint.LoopStartAction);
					agent2.SetActionChannel(num5, in actionIndexCache, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, MBRandom.RandomFloat, false, -0.2f, 0, true);
				}
			}
			Vec2 vec3 = visitPointData.ClosestPointToBlendPoint.AsVec2 - visitPointData.PossibleBlendPointPosition.AsVec2;
			this._disguiseMissionLogic.SpawnDisguiseMissionAgentInternal(Settlement.CurrentSettlement.Culture.Beggar, visitPointData.PossibleBlendPointPosition.GetNavMeshVec3(), vec3.Normalized(), "_hideout_bandit", false).SetActionChannel(0, in ActionIndexCache.act_beggar_idle, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00020950 File Offset: 0x0001EB50
		private void SpawnStandingGuards(MissionPathGenerationLogic.StandingGuardSpawnData standingGuardSpawnPoint)
		{
			this.FadeOutUserAgentsInUsableMachine(standingGuardSpawnPoint.GuardPointData.MissionObject as UsableMachine);
			MatrixFrame globalFrame = standingGuardSpawnPoint.GuardPointData.MissionObject.GameEntity.GetGlobalFrame();
			this._disguiseMissionLogic.SpawnDisguiseMissionAgentInternal(this._defaultDisguiseCharacter, globalFrame.origin, standingGuardSpawnPoint.SpawnDirection.Normalized(), "_hideout_bandit", true);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000209B8 File Offset: 0x0001EBB8
		private void SpawnCrossRoadAgents(MissionPathGenerationLogic.CrossRoadScoreData selectedCrossRoad)
		{
			this.FadeOutUserAgentsInUsableMachine(selectedCrossRoad.LeftNode.MissionObject as UsableMachine);
			this.FadeOutUserAgentsInUsableMachine(selectedCrossRoad.RightNode.MissionObject as UsableMachine);
			MatrixFrame matrixFrame = ((MBRandom.RandomFloat < 0.5f) ? selectedCrossRoad.LeftNode.MissionObject.GameEntity.GetGlobalFrame() : selectedCrossRoad.RightNode.MissionObject.GameEntity.GetGlobalFrame());
			Agent agent = this._disguiseMissionLogic.SpawnDisguiseMissionAgentInternal(this._defaultDisguiseCharacter, matrixFrame.origin, matrixFrame.rotation.f.AsVec2.Normalized(), "_hideout_bandit", true);
			this._crossRoadAgentData.Add(agent, false);
			ScriptBehavior.AddTargetWithDelegate(agent, this.CrossRoadAgentSelectTargetDelegate(selectedCrossRoad), new ScriptBehavior.OnTargetReachedWaitDelegate(this.CrossRoadAgentWaitDelegate), new ScriptBehavior.OnTargetReachedDelegate(this.CrossRoadAgentOnTargetReachDelegate), 0f);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00020A9E File Offset: 0x0001EC9E
		private void CrossRoadAgentWaitDelegate(Agent agent, ref float waitTimeInSeconds)
		{
			waitTimeInSeconds = (float)MBRandom.RandomInt(6, 30);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00020AAB File Offset: 0x0001ECAB
		private bool CrossRoadAgentOnTargetReachDelegate(Agent agent1, ref Agent targetAgent, ref UsableMachine machine, ref WorldFrame frame)
		{
			this._crossRoadAgentData[agent1] = !this._crossRoadAgentData[agent1];
			return true;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00020AC9 File Offset: 0x0001ECC9
		private ScriptBehavior.SelectTargetDelegate CrossRoadAgentSelectTargetDelegate(MissionPathGenerationLogic.CrossRoadScoreData selectedCrossRoad)
		{
			return delegate(Agent agent1, ref Agent targetAgent, ref UsableMachine machine, ref WorldFrame frame, ref float customTargetReachedRangeThreshold, ref float customTargetReachedRotationThreshold)
			{
				customTargetReachedRangeThreshold = 2.5f;
				customTargetReachedRotationThreshold = 0.8f;
				if (this._crossRoadAgentData[agent1])
				{
					WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, selectedCrossRoad.LeftNode.MissionObject.GameEntity.GlobalPosition);
					frame = new WorldFrame(selectedCrossRoad.LeftNode.MissionObject.GameEntity.GetGlobalFrame().rotation, worldPosition);
				}
				else
				{
					WorldPosition worldPosition2 = new WorldPosition(Mission.Current.Scene, selectedCrossRoad.RightNode.MissionObject.GameEntity.GlobalPosition);
					frame = new WorldFrame(selectedCrossRoad.RightNode.MissionObject.GameEntity.GetGlobalFrame().rotation, worldPosition2);
				}
				return true;
			};
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00020AEC File Offset: 0x0001ECEC
		private float CalculateCrossRoadScoreForUsableMachines(MissionPathGenerationLogic.UsableMachineData leftSideUsableMachineData, MissionPathGenerationLogic.UsableMachineData rightSideUsableMachineData, NavigationPath originalPath, WorldPosition pathNodeStartPosition, WorldPosition pathNodeEndPosition)
		{
			if (leftSideUsableMachineData.PathDistanceRatio < 0.1f || rightSideUsableMachineData.PathDistanceRatio < 0.1f)
			{
				return 0f;
			}
			if (leftSideUsableMachineData.ClosestPointToPath.Distance(rightSideUsableMachineData.ClosestPointToPath) > pathNodeStartPosition.GetNavMeshVec3().Distance(pathNodeEndPosition.GetNavMeshVec3()))
			{
				return 0f;
			}
			this._tempWorldPosition.SetVec2(leftSideUsableMachineData.MissionObject.GameEntity.GlobalPosition.AsVec2);
			this._tempWorldPosition.GetNavMeshZ();
			WorldPosition tempWorldPosition = this._tempWorldPosition;
			this._tempWorldPosition.SetVec2(rightSideUsableMachineData.MissionObject.GameEntity.GlobalPosition.AsVec2);
			this._tempWorldPosition.GetNavMeshZ();
			WorldPosition tempWorldPosition2 = this._tempWorldPosition;
			float num;
			Mission.Current.Scene.GetPathDistanceBetweenPositions(ref tempWorldPosition, ref tempWorldPosition2, 0.37f, out num);
			if (num > (float)this.CrossRoadMaximumDistance || num < (float)this.CrossRoadMinimumDistance)
			{
				return 0f;
			}
			this._tempWorldPosition.SetVec2(leftSideUsableMachineData.ClosestPointToPath);
			this._tempWorldPosition.GetNavMeshZ();
			WorldPosition tempWorldPosition3 = this._tempWorldPosition;
			this._tempWorldPosition.SetVec2(rightSideUsableMachineData.ClosestPointToPath);
			this._tempWorldPosition.GetNavMeshZ();
			WorldPosition tempWorldPosition4 = this._tempWorldPosition;
			float num2;
			base.Mission.Scene.GetPathDistanceBetweenPositions(ref tempWorldPosition, ref tempWorldPosition2, 0.37f, out num2);
			pathNodeStartPosition.AsVec2.Distance(pathNodeEndPosition.AsVec2);
			float num3;
			Mission.Current.Scene.GetPathDistanceBetweenPositions(ref tempWorldPosition, ref tempWorldPosition3, 0.37f, out num3);
			float num4;
			Mission.Current.Scene.GetPathDistanceBetweenPositions(ref tempWorldPosition4, ref tempWorldPosition2, 0.37f, out num4);
			if (num3 > num4 && num4 / num3 < 0.2f)
			{
				return 0f;
			}
			if (num4 > num3 && num3 / num4 < 0.2f)
			{
				return 0f;
			}
			float num5 = (pathNodeEndPosition.AsVec2 - pathNodeStartPosition.AsVec2).AngleBetween(tempWorldPosition.AsVec2 - tempWorldPosition2.AsVec2).ToDegrees();
			if (Math.Abs(num5) > 150f || Math.Abs(num5) < 30f)
			{
				return 0f;
			}
			float num6;
			if (Math.Abs(num5) > 90f)
			{
				num6 = MBMath.Map(Math.Abs(num5), 90f, 150f, 1f, 0f);
			}
			else
			{
				num6 = MBMath.Map(Math.Abs(num5), 30f, 90f, 0f, 1f);
			}
			float num7 = MBMath.Map(num3 + num4, 0f, 20f, 0f, 1f);
			return num6 + num7;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00020DB0 File Offset: 0x0001EFB0
		private float CalculateVisitPointScore(MissionPathGenerationLogic.UsableMachineData usableMachineData, NavigationPath originalPath, WorldPosition pathNodeStart, WorldPosition pathNodeEnd, out Vec3 possibleBlendPointPosition, out float startingAngle, out Vec2 pathToVisitPointZero, out Vec2 closestPointToPath)
		{
			possibleBlendPointPosition = Vec3.Invalid;
			startingAngle = 0f;
			pathToVisitPointZero = Vec2.Zero;
			closestPointToPath = Vec2.Invalid;
			if (usableMachineData.PathDistanceRatio < 0.2f || usableMachineData.PathDistanceRatio > 0.9f)
			{
				return 0f;
			}
			WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, usableMachineData.MissionObject.GameEntity.GlobalPosition);
			this._tempWorldPosition.SetVec2(usableMachineData.ClosestPointToPath);
			this._tempWorldPosition.GetNavMeshZ();
			WorldPosition tempWorldPosition = this._tempWorldPosition;
			NavigationPath navigationPath = new NavigationPath();
			base.Mission.Scene.GetPathBetweenAIFaces(pathNodeStart.GetNearestNavMesh(), worldPosition.GetNearestNavMesh(), pathNodeStart.AsVec2, worldPosition.AsVec2, 0f, navigationPath, new int[] { this._disabledFaceId });
			Vec2 vec = pathNodeStart.AsVec2 + (pathNodeEnd.AsVec2 - pathNodeStart.AsVec2) * 0.5f;
			this._tempWorldPosition.SetVec2(vec);
			this._tempWorldPosition.GetNavMeshZ();
			WorldPosition tempWorldPosition2 = this._tempWorldPosition;
			Vec2 vec2 = navigationPath[0];
			float num = vec2.Distance(vec);
			for (int i = 0; i < navigationPath.Size - 1; i++)
			{
				Vec2 vec3 = navigationPath[i];
				Vec2 vec4 = navigationPath[i + 1];
				num += vec3.Distance(vec4);
			}
			if (num < (float)this.MinimumVisitPointDistance || num > (float)this.MaximumVisitPointDistance)
			{
				return 0f;
			}
			float num2 = 0f;
			vec2 = pathNodeEnd.GetNavMeshVec3().AsVec2 - pathNodeStart.GetNavMeshVec3().AsVec2;
			startingAngle = vec2.AngleBetween(navigationPath[0] - tempWorldPosition2.GetNavMeshVec3().AsVec2).ToDegrees();
			pathToVisitPointZero = navigationPath[0];
			if (Math.Abs(startingAngle) < 90f && Math.Abs(startingAngle) > 30f)
			{
				for (int j = 0; j < navigationPath.Size - 1; j++)
				{
					Vec2 vec5 = ((j == 0) ? tempWorldPosition.AsVec2 : navigationPath[j - 1]);
					Vec2 vec6 = navigationPath[j];
					Vec2 vec7 = navigationPath[j + 1];
					vec2 = vec6 - vec5;
					float num3 = vec2.AngleBetween(vec7 - vec6).ToDegrees();
					num2 += MBMath.Map(Math.Abs(num3), 0f, 90f, 1f, 0f);
					if ((float)j > (float)(navigationPath.Size - 1) * 0.25f && !possibleBlendPointPosition.IsValid)
					{
						this._tempWorldPosition.SetVec2(vec6);
						this._tempWorldPosition.GetNavMeshZ();
						WorldPosition tempWorldPosition3 = this._tempWorldPosition;
						Vec3 navMeshVec = tempWorldPosition3.GetNavMeshVec3();
						Vec3 vec8 = Vec3.Invalid;
						PathFaceRecord nullFaceRecord = PathFaceRecord.NullFaceRecord;
						int num4 = 0;
						float num5 = float.MaxValue;
						do
						{
							num4++;
							if (num4 > 150)
							{
								break;
							}
							vec8 = base.Mission.GetRandomPositionAroundPoint(navMeshVec, 2f, 6f, true);
							base.Mission.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, vec8, true);
							if (nullFaceRecord.FaceGroupIndex == this._disabledFaceId)
							{
								for (int k = 0; k < navigationPath.Size - 1; k++)
								{
									Vec2 vec9 = navigationPath[k];
									Vec2 vec10 = navigationPath[k + 1];
									vec2 = vec8.AsVec2;
									Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in vec9, in vec10, in vec2);
									vec2 = vec8.AsVec2;
									float num6 = vec2.Distance(closestPointOnLineSegmentToPoint);
									if (num6 < num5)
									{
										closestPointToPath = closestPointOnLineSegmentToPoint;
										num5 = num6;
									}
								}
							}
						}
						while (nullFaceRecord.FaceGroupIndex != this._disabledFaceId || num5 < 1.5f);
						if (num4 < 150)
						{
							possibleBlendPointPosition = vec8;
						}
					}
				}
				num2 /= (float)navigationPath.Size;
				if (possibleBlendPointPosition.IsValid)
				{
					vec2 = possibleBlendPointPosition.AsVec2;
					if (vec2.Distance(worldPosition.AsVec2) >= this.MinimumDistanceToBlendPointToVisitPoint)
					{
						for (int l = 0; l < originalPath.Size - 1; l++)
						{
							Vec2 vec11 = originalPath[l];
							for (int m = 0; m < navigationPath.Size - 1; m++)
							{
								vec2 = navigationPath[m];
								if (vec2.Distance(vec11) < 2f)
								{
									return 0f;
								}
							}
						}
						float num7 = (float)(this.MaximumVisitPointDistance + this.MinimumVisitPointDistance) * 0.5f;
						float num8;
						if (num > num7)
						{
							num8 = MBMath.Map(num, num7, (float)this.MaximumVisitPointDistance, 0.5f, 1f);
						}
						else
						{
							num8 = MBMath.Map(num, (float)this.MinimumVisitPointDistance, num7, 0f, 0.5f);
						}
						float num9 = 0f;
						UsableMachine usableMachine = usableMachineData.MissionObject as UsableMachine;
						if (usableMachine.StandingPoints.Count > 0)
						{
							if (usableMachine.StandingPoints.Count != usableMachine.StandingPoints.Count<StandingPoint>((StandingPoint x) => x.HasAlternative()))
							{
								if (usableMachine.StandingPoints.Count<StandingPoint>(delegate(StandingPoint x)
								{
									AnimationPoint animationPoint;
									return (animationPoint = x as AnimationPoint) != null && animationPoint.PairEntity != null;
								}) == 2)
								{
									num9 = 2f;
								}
							}
						}
						float num10 = ((usableMachineData.PathDistanceRatio > 0.75f) ? MBMath.Map(usableMachineData.PathDistanceRatio, 0.75f, 1f, 1f, 0f) : MBMath.Map(usableMachineData.PathDistanceRatio, 0f, 0.75f, 0f, 1f));
						return 5f + num2 + num8 + num10 + num9;
					}
				}
				return 0f;
			}
			return 0f;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000213A4 File Offset: 0x0001F5A4
		private float CalculateSpawnGuardScore(MissionPathGenerationLogic.UsableMachineData guardSpawnPointData, out Vec2 spawnRotation)
		{
			spawnRotation = Vec2.Zero;
			UsableMachine usableMachine = guardSpawnPointData.MissionObject as UsableMachine;
			if (usableMachine.PilotAgent != null)
			{
				return 0f;
			}
			using (List<StandingPoint>.Enumerator enumerator = usableMachine.StandingPoints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.UserAgent != null)
					{
						return 0f;
					}
				}
			}
			if (guardSpawnPointData.PathDistanceRatio < MissionPathGenerationLogic.MinimumGuardSpawnPathRatio)
			{
				return 0f;
			}
			float num = guardSpawnPointData.ClosestPointToPath.Distance(guardSpawnPointData.MissionObject.GameEntity.GlobalPosition.AsVec2);
			if (num < 3f)
			{
				return 0f;
			}
			float num2;
			if (num > 5f)
			{
				num2 = MBMath.Map(num, 5f, 30f, 1f, 0f);
			}
			else
			{
				num2 = MBMath.Map(num, 3f, 5f, 0f, 1f);
			}
			spawnRotation = guardSpawnPointData.ClosestPointToPath - guardSpawnPointData.MissionObject.GameEntity.GlobalPosition.AsVec2;
			return num2;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000214EC File Offset: 0x0001F6EC
		protected override void OnEndMission()
		{
			this._nearbyLeftSideUsableMachinesCache = null;
			this._nearbyRightSideUsableMachinesCache = null;
			this._allTargetAgentPointOfInterest = null;
			this._crossRoadAgentData = null;
			this._startAndFinishPointPool = null;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00021514 File Offset: 0x0001F714
		public void InitializeBehavior()
		{
			GameEntity gameEntity = base.Mission.Scene.FindEntityWithTag("navigation_mesh_deactivator");
			if (gameEntity != null)
			{
				NavigationMeshDeactivator firstScriptOfType = gameEntity.GetFirstScriptOfType<NavigationMeshDeactivator>();
				this._disabledFaceId = firstScriptOfType.DisableFaceWithId;
			}
			this._disguiseMissionLogic = Mission.Current.GetMissionBehavior<DisguiseMissionLogic>();
			Mission.Current.Scene.GetAllEntitiesWithScriptComponent<PassageUsePoint>(ref this._startAndFinishPointPool);
			for (int i = this._startAndFinishPointPool.Count - 1; i >= 0; i--)
			{
				Location toLocation = this._startAndFinishPointPool[i].GetFirstScriptOfType<PassageUsePoint>().ToLocation;
				string text = ((toLocation != null) ? toLocation.StringId : null);
				if (text == null || text == "lordshall" || text == "prison")
				{
					this._startAndFinishPointPool.RemoveAt(i);
				}
			}
			Mission.Current.Scene.GetAllEntitiesWithScriptComponent<CastleGate>(ref this._startAndFinishPointPool);
			foreach (GameEntity gameEntity2 in Mission.Current.Scene.FindEntitiesWithTag("sp_player_conversation"))
			{
				this._startAndFinishPointPool.Add(gameEntity2);
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0002164C File Offset: 0x0001F84C
		public override void OnMissionTick(float dt)
		{
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00021650 File Offset: 0x0001F850
		private void FadeOutUserAgentsInUsableMachine(UsableMachine usableMachine)
		{
			if (usableMachine.PilotAgent != null)
			{
				usableMachine.PilotAgent.FadeOut(true, true);
			}
			foreach (StandingPoint standingPoint in usableMachine.StandingPoints)
			{
				if (standingPoint.UserAgent != null)
				{
					standingPoint.UserAgent.FadeOut(true, true);
				}
			}
			usableMachine.SetDisabled(true);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000216D0 File Offset: 0x0001F8D0
		private MissionPathGenerationLogic.PointOfInterestScorePair CreatePathScorePair(MissionPathGenerationLogic.NavigationPathData pathData)
		{
			List<MissionPathGenerationLogic.VisitPointNodeScoreData> list = this.GetVisitPoints(pathData);
			List<MissionPathGenerationLogic.CrossRoadScoreData> list2 = this.GetCrossRoadPoints(pathData);
			if (list.Count == 0 && list2.Count == 0)
			{
				return null;
			}
			List<MissionPathGenerationLogic.PointOfInterestBaseData> list3 = new List<MissionPathGenerationLogic.PointOfInterestBaseData>();
			list.Shuffle<MissionPathGenerationLogic.VisitPointNodeScoreData>();
			list2.Shuffle<MissionPathGenerationLogic.CrossRoadScoreData>();
			if (list2.Count > 20)
			{
				list2 = list2.OrderByDescending<MissionPathGenerationLogic.CrossRoadScoreData, float>((MissionPathGenerationLogic.CrossRoadScoreData x) => x.Score).Take<MissionPathGenerationLogic.CrossRoadScoreData>(20).ToList<MissionPathGenerationLogic.CrossRoadScoreData>();
				list2.Shuffle<MissionPathGenerationLogic.CrossRoadScoreData>();
			}
			if (list.Count > 10)
			{
				list = list.OrderByDescending<MissionPathGenerationLogic.VisitPointNodeScoreData, float>((MissionPathGenerationLogic.VisitPointNodeScoreData x) => x.Score).Take<MissionPathGenerationLogic.VisitPointNodeScoreData>(10).ToList<MissionPathGenerationLogic.VisitPointNodeScoreData>();
				list.Shuffle<MissionPathGenerationLogic.VisitPointNodeScoreData>();
			}
			list3.AddRange(list);
			list3.AddRange(list2);
			list3.Shuffle<MissionPathGenerationLogic.PointOfInterestBaseData>();
			Stack<ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int>> stack = new Stack<ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int>>();
			stack.Push(new ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int>(new MissionPathGenerationLogic.PointOfInterestScorePair(pathData, new List<MissionPathGenerationLogic.PointOfInterestBaseData>(), 0f), 0));
			return this.CreatePathDataWith(stack, list3);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000217D4 File Offset: 0x0001F9D4
		private MissionPathGenerationLogic.PointOfInterestScorePair CreatePathDataWith(Stack<ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int>> stack, List<MissionPathGenerationLogic.PointOfInterestBaseData> pointOfInterestData)
		{
			MissionPathGenerationLogic.PointOfInterestScorePair pointOfInterestScorePair = null;
			while (stack.Count > 0)
			{
				ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int> valueTuple = stack.Pop();
				int i = valueTuple.Item2;
				while (i < pointOfInterestData.Count)
				{
					MissionPathGenerationLogic.PointOfInterestBaseData data = pointOfInterestData[i];
					if (valueTuple.Item1.Data.All<MissionPathGenerationLogic.PointOfInterestBaseData>((MissionPathGenerationLogic.PointOfInterestBaseData x) => !x.IsInRadius(data)))
					{
						MissionPathGenerationLogic.PointOfInterestScorePair pointOfInterestScorePair2 = valueTuple.Item1.Clone();
						pointOfInterestScorePair2.AddToData(data);
						if (i + 1 < pointOfInterestData.Count)
						{
							stack.Push(new ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int>(valueTuple.Item1, i + 1));
							stack.Push(new ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int>(pointOfInterestScorePair2, i + 1));
						}
						if (pointOfInterestScorePair == null || pointOfInterestScorePair2.IsBetterThan(pointOfInterestScorePair))
						{
							pointOfInterestScorePair = pointOfInterestScorePair2;
						}
						if (pointOfInterestScorePair2.IsSufficient())
						{
							return pointOfInterestScorePair2;
						}
						i++;
						valueTuple = new ValueTuple<MissionPathGenerationLogic.PointOfInterestScorePair, int>(pointOfInterestScorePair2, i);
						break;
					}
					else
					{
						i++;
					}
				}
				if (i == pointOfInterestData.Count && valueTuple.Item1.IsSufficient())
				{
					return valueTuple.Item1;
				}
			}
			return pointOfInterestScorePair;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000218DC File Offset: 0x0001FADC
		private MissionPathGenerationLogic.PointOfInterestScorePair GetRandomPath()
		{
			MissionPathGenerationLogic.PointOfInterestScorePair pathInternal = this.GetPathInternal();
			if (pathInternal != null)
			{
				if (MissionPathGenerationLogic.MaximumStandingGuardCountInPath > 0)
				{
					this.AddStandingGuardsToThePath(pathInternal);
				}
				if (MissionPathGenerationLogic.MaximumLookBackPointCountInPath > 0)
				{
					this.AddLookBackPointsToThePath(pathInternal);
				}
			}
			return pathInternal;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00021914 File Offset: 0x0001FB14
		private void AddLookBackPointsToThePath(MissionPathGenerationLogic.PointOfInterestScorePair path)
		{
			Dictionary<int, float> dictionary = new Dictionary<int, float>();
			int num = (int)((float)path.PathData.Path.Size * 0.25f);
			while ((float)num < (float)path.PathData.Path.Size * 0.9f)
			{
				Vec2 vec = path.PathData.Path[num];
				Vec2 vec2 = path.PathData.Path[num + 1];
				if (vec.IsNonZero() && vec2.IsNonZero())
				{
					float num2 = path.PathData.PathNodeAndDistances[vec] / path.PathData.TotalDistance;
					float num3 = path.PathData.PathNodeAndDistances[vec2] / path.PathData.TotalDistance;
					float num4 = (num2 + num3) * 0.5f;
					float num5 = 0f;
					int num6 = 0;
					foreach (MissionPathGenerationLogic.PointOfInterestBaseData pointOfInterestBaseData in path.Data)
					{
						float locationRatio = pointOfInterestBaseData.GetLocationRatio();
						if (locationRatio > num4 - 0.1f && locationRatio < num4 + 0.1f)
						{
							num5 += Math.Abs(num4 - locationRatio);
							num6++;
						}
					}
					if (num6 > 0)
					{
						num5 /= (float)num6;
					}
					dictionary.Add(num, num5);
				}
				num++;
			}
			if (dictionary.Any<KeyValuePair<int, float>>())
			{
				List<KeyValuePair<int, float>> list = dictionary.OrderByDescending<KeyValuePair<int, float>, float>((KeyValuePair<int, float> x) => x.Value).ToList<KeyValuePair<int, float>>();
				int num7 = 0;
				int num8 = ((MissionPathGenerationLogic.MaximumLookBackPointCountInPath > 0) ? MBRandom.RandomInt((int)((float)MissionPathGenerationLogic.MaximumLookBackPointCountInPath * 0.5f), MissionPathGenerationLogic.MaximumLookBackPointCountInPath) : 0);
				if (num8 > 0)
				{
					this._tempWorldPosition = new WorldPosition(Mission.Current.Scene, path.PathData.StartingGameEntity.GlobalPosition);
					this._tempWorldPosition.GetNavMeshZ();
					for (int i = 0; i < path.PathData.Path.Size - 1; i++)
					{
						Vec2 vec3 = path.PathData.Path[i];
						Vec2 vec4 = path.PathData.Path[i + 1];
						this._tempWorldPosition.SetVec2(vec3);
						this._tempWorldPosition.GetNavMeshZ();
						this._tempWorldPosition.SetVec2(vec4);
						this._tempWorldPosition.GetNavMeshZ();
						if (num7 != num8)
						{
							foreach (KeyValuePair<int, float> keyValuePair in list)
							{
								int key = keyValuePair.Key;
								if (i == key)
								{
									Vec2 vec5 = (vec3 + vec4) * 0.5f;
									float num9 = (path.PathData.PathNodeAndDistances[vec3] / path.PathData.TotalDistance + path.PathData.PathNodeAndDistances[vec4] / path.PathData.TotalDistance) * 0.5f;
									Vec2 vec6 = vec5 + (vec4 - vec3).Normalized();
									this._tempWorldPosition.SetVec2(vec5);
									this._tempWorldPosition.GetNavMeshZ();
									WorldPosition tempWorldPosition = this._tempWorldPosition;
									this._tempWorldPosition.SetVec2(vec6);
									this._tempWorldPosition.GetNavMeshZ();
									WorldPosition tempWorldPosition2 = this._tempWorldPosition;
									MissionPathGenerationLogic.LookBackPointData newData = new MissionPathGenerationLogic.LookBackPointData(tempWorldPosition, tempWorldPosition2, num9);
									if (path.Data.All<MissionPathGenerationLogic.PointOfInterestBaseData>((MissionPathGenerationLogic.PointOfInterestBaseData x) => !x.IsInRadius(newData)))
									{
										path.AddToData(newData);
										num7++;
									}
									if (num7 == num8)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00021D10 File Offset: 0x0001FF10
		private void AddStandingGuardsToThePath(MissionPathGenerationLogic.PointOfInterestScorePair path)
		{
			int num = (int)(path.PathData.TotalDistance / 10f);
			num = MBMath.ClampInt(num, MissionPathGenerationLogic.MinimumStandingGuardCountInPath, MissionPathGenerationLogic.MaximumStandingGuardCountInPath);
			List<MissionPathGenerationLogic.StandingGuardSpawnData> guardSpawnPoints = this.GetGuardSpawnPoints(path.PathData);
			int num2 = 0;
			Func<MissionPathGenerationLogic.StandingGuardSpawnData, bool> <>9__0;
			for (int i = 0; i < guardSpawnPoints.Count; i++)
			{
				IReadOnlyList<MissionPathGenerationLogic.StandingGuardSpawnData> readOnlyList = guardSpawnPoints;
				Func<MissionPathGenerationLogic.StandingGuardSpawnData, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (MissionPathGenerationLogic.StandingGuardSpawnData x) => path.Data.All<MissionPathGenerationLogic.PointOfInterestBaseData>((MissionPathGenerationLogic.PointOfInterestBaseData y) => !y.IsInRadius(x)));
				}
				MissionPathGenerationLogic.StandingGuardSpawnData randomElementWithPredicate = readOnlyList.GetRandomElementWithPredicate<MissionPathGenerationLogic.StandingGuardSpawnData>(func);
				if (randomElementWithPredicate != null)
				{
					path.AddToData(randomElementWithPredicate);
					num2++;
					if (num2 >= num)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00021DC0 File Offset: 0x0001FFC0
		public List<MissionPathGenerationLogic.PointOfInterestScorePair> GetAllPossiblePaths()
		{
			List<MissionPathGenerationLogic.PointOfInterestScorePair> list = new List<MissionPathGenerationLogic.PointOfInterestScorePair>();
			List<UsableMachine> usablePoints = base.Mission.GetMissionBehavior<MissionAgentHandler>().UsablePoints;
			for (int i = 0; i < this._startAndFinishPointPool.Count - 1; i++)
			{
				for (int j = i + 1; j < this._startAndFinishPointPool.Count; j++)
				{
					GameEntity gameEntity = this._startAndFinishPointPool[i];
					GameEntity gameEntity2 = this._startAndFinishPointPool[j];
					MissionPathGenerationLogic.NavigationPathData navigationPathData = new MissionPathGenerationLogic.NavigationPathData(usablePoints, gameEntity, gameEntity2, this._disabledFaceId);
					this._tempWorldPosition = new WorldPosition(Mission.Current.Scene, gameEntity.GlobalPosition);
					this._tempWorldPosition.GetNavMeshZ();
					if (navigationPathData.TotalDistance < (float)MissionPathGenerationLogic.MaximumPathDistance && navigationPathData.TotalDistance > (float)MissionPathGenerationLogic.MinimumPathDistance)
					{
						MissionPathGenerationLogic.PointOfInterestScorePair pointOfInterestScorePair = this.CreatePathScorePair(navigationPathData);
						if (pointOfInterestScorePair != null && pointOfInterestScorePair.Score > (float)MissionPathGenerationLogic.ScoreToAchieve)
						{
							if (MissionPathGenerationLogic.MaximumStandingGuardCountInPath > 0)
							{
								this.AddStandingGuardsToThePath(pointOfInterestScorePair);
							}
							if (MissionPathGenerationLogic.MaximumLookBackPointCountInPath > 0)
							{
								this.AddLookBackPointsToThePath(pointOfInterestScorePair);
							}
							list.Add(pointOfInterestScorePair);
						}
					}
					MissionPathGenerationLogic.NavigationPathData navigationPathData2 = navigationPathData.ReverseClone();
					this._tempWorldPosition = new WorldPosition(Mission.Current.Scene, gameEntity2.GlobalPosition);
					this._tempWorldPosition.GetNavMeshZ();
					if (navigationPathData2.TotalDistance < (float)MissionPathGenerationLogic.MaximumPathDistance && navigationPathData2.TotalDistance > (float)MissionPathGenerationLogic.MinimumPathDistance)
					{
						MissionPathGenerationLogic.PointOfInterestScorePair pointOfInterestScorePair2 = this.CreatePathScorePair(navigationPathData2);
						if (pointOfInterestScorePair2 != null && pointOfInterestScorePair2.Score > (float)MissionPathGenerationLogic.ScoreToAchieve)
						{
							if (MissionPathGenerationLogic.MaximumStandingGuardCountInPath > 0)
							{
								this.AddStandingGuardsToThePath(pointOfInterestScorePair2);
							}
							if (MissionPathGenerationLogic.MaximumLookBackPointCountInPath > 0)
							{
								this.AddLookBackPointsToThePath(pointOfInterestScorePair2);
							}
							list.Add(pointOfInterestScorePair2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00021F6E File Offset: 0x0002016E
		public bool IsOnLeftSide(Vec2 lineA, Vec2 lineB, Vec2 point)
		{
			return (lineB.x - lineA.x) * (point.y - lineA.y) - (lineB.y - lineA.y) * (point.x - lineA.x) > 0f;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00021FB0 File Offset: 0x000201B0
		private MissionPathGenerationLogic.PointOfInterestScorePair GetPathInternal()
		{
			List<UsableMachine> usablePoints = base.Mission.GetMissionBehavior<MissionAgentHandler>().UsablePoints;
			MissionPathGenerationLogic.PointOfInterestScorePair pointOfInterestScorePair = null;
			for (int i = 0; i < this._startAndFinishPointPool.Count - 1; i++)
			{
				for (int j = i + 1; j < this._startAndFinishPointPool.Count; j++)
				{
					GameEntity gameEntity = this._startAndFinishPointPool[i];
					GameEntity gameEntity2 = this._startAndFinishPointPool[j];
					MissionPathGenerationLogic.NavigationPathData navigationPathData = new MissionPathGenerationLogic.NavigationPathData(usablePoints, gameEntity, gameEntity2, this._disabledFaceId);
					this._tempWorldPosition = new WorldPosition(Mission.Current.Scene, gameEntity.GlobalPosition);
					this._tempWorldPosition.GetNavMeshZ();
					if (navigationPathData.TotalDistance < (float)MissionPathGenerationLogic.MaximumPathDistance && navigationPathData.TotalDistance > (float)MissionPathGenerationLogic.MinimumPathDistance)
					{
						MissionPathGenerationLogic.PointOfInterestScorePair pointOfInterestScorePair2 = this.CreatePathScorePair(navigationPathData);
						if (pointOfInterestScorePair2 != null)
						{
							if (pointOfInterestScorePair2.IsSufficient())
							{
								this._currentStarting = gameEntity;
								this._currentEnding = gameEntity2;
								return pointOfInterestScorePair2;
							}
							if (pointOfInterestScorePair == null || pointOfInterestScorePair2.IsBetterThan(pointOfInterestScorePair))
							{
								pointOfInterestScorePair = pointOfInterestScorePair2;
							}
						}
					}
					MissionPathGenerationLogic.NavigationPathData navigationPathData2 = navigationPathData.ReverseClone();
					this._tempWorldPosition = new WorldPosition(Mission.Current.Scene, gameEntity2.GlobalPosition);
					this._tempWorldPosition.GetNavMeshZ();
					if (navigationPathData2.TotalDistance < (float)MissionPathGenerationLogic.MaximumPathDistance && navigationPathData2.TotalDistance > (float)MissionPathGenerationLogic.MinimumPathDistance)
					{
						MissionPathGenerationLogic.PointOfInterestScorePair pointOfInterestScorePair3 = this.CreatePathScorePair(navigationPathData2);
						if (pointOfInterestScorePair3 != null)
						{
							if (pointOfInterestScorePair3.IsSufficient())
							{
								this._currentStarting = gameEntity2;
								this._currentEnding = gameEntity;
								return pointOfInterestScorePair3;
							}
							if (pointOfInterestScorePair == null || pointOfInterestScorePair3.IsBetterThan(pointOfInterestScorePair))
							{
								pointOfInterestScorePair = pointOfInterestScorePair3;
							}
						}
					}
				}
			}
			if (pointOfInterestScorePair != null)
			{
				this._currentStarting = pointOfInterestScorePair.PathData.StartingGameEntity;
				this._currentEnding = pointOfInterestScorePair.PathData.EndingGameEntity;
			}
			return pointOfInterestScorePair;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0002216C File Offset: 0x0002036C
		private List<MissionPathGenerationLogic.StandingGuardSpawnData> GetGuardSpawnPoints(MissionPathGenerationLogic.NavigationPathData pathData)
		{
			List<MissionPathGenerationLogic.StandingGuardSpawnData> list = new List<MissionPathGenerationLogic.StandingGuardSpawnData>();
			foreach (MissionPathGenerationLogic.UsableMachineData usableMachineData in pathData.ValidUsableMachinesData)
			{
				Vec2 vec;
				float num = this.CalculateSpawnGuardScore(usableMachineData, out vec);
				if (num > 0f)
				{
					list.Add(new MissionPathGenerationLogic.StandingGuardSpawnData(usableMachineData, vec, num));
				}
			}
			return list;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000221E0 File Offset: 0x000203E0
		private List<MissionPathGenerationLogic.VisitPointNodeScoreData> GetVisitPoints(MissionPathGenerationLogic.NavigationPathData pathData)
		{
			List<MissionPathGenerationLogic.VisitPointNodeScoreData> list = new List<MissionPathGenerationLogic.VisitPointNodeScoreData>();
			NavigationPath path = pathData.Path;
			for (int i = 0; i < path.Size - 1; i++)
			{
				Vec2 vec = path[i];
				Vec2 vec2 = path[i + 1];
				this._tempWorldPosition.SetVec2(vec);
				this._tempWorldPosition.GetNavMeshZ();
				WorldPosition tempWorldPosition = this._tempWorldPosition;
				this._tempWorldPosition.SetVec2(vec2);
				this._tempWorldPosition.GetNavMeshZ();
				WorldPosition tempWorldPosition2 = this._tempWorldPosition;
				foreach (MissionPathGenerationLogic.UsableMachineData usableMachineData in pathData.ValidUsableMachinesData)
				{
					if (!usableMachineData.IsAlreadyAddedToPath)
					{
						Vec3 vec3;
						float num2;
						Vec2 vec4;
						Vec2 vec5;
						float num = this.CalculateVisitPointScore(usableMachineData, path, tempWorldPosition, tempWorldPosition2, out vec3, out num2, out vec4, out vec5);
						if (num > 0f)
						{
							Vec2 vec6 = vec + (vec2 - vec) * 0.5f;
							this._tempWorldPosition.SetVec2(vec);
							this._tempWorldPosition.GetNavMeshZ();
							this._tempWorldPosition.SetVec2(vec6);
							this._tempWorldPosition.GetNavMeshZ();
							WorldPosition tempWorldPosition3 = this._tempWorldPosition;
							this._tempWorldPosition.SetVec2(vec2);
							this._tempWorldPosition.GetNavMeshVec3();
							this._tempWorldPosition.SetVec2(vec3.AsVec2);
							this._tempWorldPosition.GetNavMeshZ();
							WorldPosition tempWorldPosition4 = this._tempWorldPosition;
							this._tempWorldPosition.SetVec2(vec5);
							this._tempWorldPosition.GetNavMeshZ();
							WorldPosition tempWorldPosition5 = this._tempWorldPosition;
							float num3 = (pathData.PathNodeAndDistances[vec] / pathData.TotalDistance + pathData.PathNodeAndDistances[vec2] / pathData.TotalDistance) * 0.5f;
							list.Add(new MissionPathGenerationLogic.VisitPointNodeScoreData(usableMachineData, tempWorldPosition4, tempWorldPosition3, num3, num, num2, tempWorldPosition, tempWorldPosition2, vec4, tempWorldPosition5));
							usableMachineData.IsAlreadyAddedToPath = true;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000223F4 File Offset: 0x000205F4
		private List<MissionPathGenerationLogic.CrossRoadScoreData> GetCrossRoadPoints(MissionPathGenerationLogic.NavigationPathData pathData)
		{
			List<MissionPathGenerationLogic.CrossRoadScoreData> list = new List<MissionPathGenerationLogic.CrossRoadScoreData>();
			for (int i = 0; i < pathData.Path.Size - 1; i++)
			{
				this._nearbyLeftSideUsableMachinesCache.Clear();
				this._nearbyRightSideUsableMachinesCache.Clear();
				Vec2 vec = pathData.Path[i];
				Vec2 vec2 = pathData.Path[i + 1];
				this._tempWorldPosition.SetVec2(vec);
				this._tempWorldPosition.GetNavMeshZ();
				WorldPosition tempWorldPosition = this._tempWorldPosition;
				this._tempWorldPosition.SetVec2(vec2);
				this._tempWorldPosition.GetNavMeshZ();
				WorldPosition tempWorldPosition2 = this._tempWorldPosition;
				float num = vec2.DistanceSquared(vec);
				if (num > 25f && num < 100f)
				{
					foreach (MissionPathGenerationLogic.UsableMachineData usableMachineData in pathData.ValidUsableMachinesData)
					{
						if (!usableMachineData.IsAlreadyAddedToPath)
						{
							if (this.IsOnLeftSide(vec, vec2, usableMachineData.MissionObject.GameEntity.GlobalPosition.AsVec2))
							{
								this._nearbyLeftSideUsableMachinesCache.Add(usableMachineData);
							}
							else
							{
								this._nearbyRightSideUsableMachinesCache.Add(usableMachineData);
							}
						}
					}
					foreach (MissionPathGenerationLogic.UsableMachineData usableMachineData2 in this._nearbyLeftSideUsableMachinesCache)
					{
						foreach (MissionPathGenerationLogic.UsableMachineData usableMachineData3 in this._nearbyRightSideUsableMachinesCache)
						{
							usableMachineData2.MissionObject.GameEntity.GlobalPosition.Distance(usableMachineData3.MissionObject.GameEntity.GlobalPosition);
							if (!usableMachineData2.IsAlreadyAddedToPath && !usableMachineData3.IsAlreadyAddedToPath)
							{
								float num2 = this.CalculateCrossRoadScoreForUsableMachines(usableMachineData2, usableMachineData3, pathData.Path, tempWorldPosition, tempWorldPosition2);
								if (num2 > 0f)
								{
									list.Add(new MissionPathGenerationLogic.CrossRoadScoreData(usableMachineData2, usableMachineData3, num2));
									usableMachineData2.IsAlreadyAddedToPath = true;
									usableMachineData3.IsAlreadyAddedToPath = true;
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00022654 File Offset: 0x00020854
		private void ShowMissionFailedPopup()
		{
			object obj = new TextObject("{=CMu4B9fZ}Mission Failed", null);
			TextObject textObject = new TextObject("{=RcY8uZA1}You have lost the target.", null);
			TextObject textObject2 = new TextObject("{=DM6luo3c}Continue", null);
			InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, false, textObject2.ToString(), null, delegate
			{
				Mission.Current.EndMission();
			}, null, "", 0f, null, null, null), Campaign.Current.GameMode == CampaignGameMode.Campaign, false);
		}

		// Token: 0x04000297 RID: 663
		private const float MaximumPathNodeDistanceSquaredToCheckForCrossRoads = 100f;

		// Token: 0x04000298 RID: 664
		private const float MinimumPathNodeDistanceSquaredToCheckForCrossRoads = 25f;

		// Token: 0x04000299 RID: 665
		private const float StandingGuardCountPerXMeter = 10f;

		// Token: 0x0400029A RID: 666
		private const float HumanMonsterCapsuleRadius = 0.37f;

		// Token: 0x0400029B RID: 667
		private const float MinimumStandingGuardSpawnDistance = 3f;

		// Token: 0x0400029C RID: 668
		private const float OptimumStandingGuardSpawnDistance = 5f;

		// Token: 0x0400029D RID: 669
		private const float MaximumStandingGuardSpawnDistance = 30f;

		// Token: 0x0400029E RID: 670
		private const float DoNotSpawnVisitPointPathRatioMin = 0.2f;

		// Token: 0x0400029F RID: 671
		private const float DoNotSpawnVisitPointPathRatioMax = 0.9f;

		// Token: 0x040002A0 RID: 672
		private const float OptimumPathIndexRatioForVisitPoint = 0.75f;

		// Token: 0x040002A1 RID: 673
		private const float FilterPadding = 20f;

		// Token: 0x040002A2 RID: 674
		private const string VisitBarrelPrefabName = "disguise_mission_interactable_barrel";

		// Token: 0x040002A3 RID: 675
		private const bool PlayerCompromised = false;

		// Token: 0x040002A4 RID: 676
		private readonly CharacterObject _defaultDisguiseCharacter;

		// Token: 0x040002A5 RID: 677
		private int _disabledFaceId;

		// Token: 0x040002A6 RID: 678
		public static int MinimumPathDistance = 200;

		// Token: 0x040002A7 RID: 679
		public static int MaximumPathDistance = 600;

		// Token: 0x040002A8 RID: 680
		public float MinimumDistanceToBlendPointToVisitPoint = 5f;

		// Token: 0x040002A9 RID: 681
		private MissionPathGenerationLogic.PointOfInterestScorePair _selectedPath;

		// Token: 0x040002AA RID: 682
		public static int MinimumVisitPointCountInPath = 2;

		// Token: 0x040002AB RID: 683
		public static int MaximumVisitPointCountInPath = 10;

		// Token: 0x040002AC RID: 684
		public static int MinimumCrossRoadCountInPath = 2;

		// Token: 0x040002AD RID: 685
		public static int MaximumCrossRoadCountInPath = 10;

		// Token: 0x040002AE RID: 686
		public static int MinimumStandingGuardCountInPath = 5;

		// Token: 0x040002AF RID: 687
		public static int MaximumStandingGuardCountInPath = 50;

		// Token: 0x040002B0 RID: 688
		public static float MinimumGuardSpawnPathRatio = 0.15f;

		// Token: 0x040002B1 RID: 689
		public static int MaximumLookBackPointCountInPath;

		// Token: 0x040002B2 RID: 690
		public static int ScoreToAchieve;

		// Token: 0x040002B3 RID: 691
		private Dictionary<Agent, bool> _crossRoadAgentData;

		// Token: 0x040002B4 RID: 692
		private DisguiseMissionLogic _disguiseMissionLogic;

		// Token: 0x040002B5 RID: 693
		private readonly List<GameEntity> _visitBarrelEntities;

		// Token: 0x040002B6 RID: 694
		public List<GameEntity> _startAndFinishPointPool;

		// Token: 0x040002B7 RID: 695
		private GameEntity _currentStarting;

		// Token: 0x040002B8 RID: 696
		private GameEntity _currentEnding;

		// Token: 0x040002B9 RID: 697
		public int CrossRoadMaximumDistance = 30;

		// Token: 0x040002BA RID: 698
		public int CrossRoadMinimumDistance = 10;

		// Token: 0x040002BB RID: 699
		public int MinimumVisitPointDistance = 10;

		// Token: 0x040002BC RID: 700
		public int MaximumVisitPointDistance = 40;

		// Token: 0x040002BD RID: 701
		private List<MissionPathGenerationLogic.UsableMachineData> _nearbyLeftSideUsableMachinesCache;

		// Token: 0x040002BE RID: 702
		private List<MissionPathGenerationLogic.UsableMachineData> _nearbyRightSideUsableMachinesCache;

		// Token: 0x040002BF RID: 703
		private List<MissionPathGenerationLogic.PointOfInterestBaseData> _allTargetAgentPointOfInterest;

		// Token: 0x040002C0 RID: 704
		private WorldPosition _tempWorldPosition;

		// Token: 0x02000176 RID: 374
		public enum PointOfInterests
		{
			// Token: 0x0400072A RID: 1834
			VisitPoint,
			// Token: 0x0400072B RID: 1835
			CrossRoadPoint,
			// Token: 0x0400072C RID: 1836
			GuardSpawnPoint,
			// Token: 0x0400072D RID: 1837
			LookBackPoint
		}

		// Token: 0x02000177 RID: 375
		public class UsableMachineData
		{
			// Token: 0x06000E8A RID: 3722 RVA: 0x00065754 File Offset: 0x00063954
			public UsableMachineData(SynchedMissionObject missionObject, Vec2 closestPointToPath, float pathDistanceRatio)
			{
				this.MissionObject = missionObject;
				this.ClosestPointToPath = closestPointToPath;
				this.PathDistanceRatio = pathDistanceRatio;
				this.IsAlreadyAddedToPath = false;
			}

			// Token: 0x0400072E RID: 1838
			public SynchedMissionObject MissionObject;

			// Token: 0x0400072F RID: 1839
			public Vec2 ClosestPointToPath;

			// Token: 0x04000730 RID: 1840
			public float PathDistanceRatio;

			// Token: 0x04000731 RID: 1841
			public bool IsAlreadyAddedToPath;
		}

		// Token: 0x02000178 RID: 376
		public class NavigationPathData
		{
			// Token: 0x06000E8B RID: 3723 RVA: 0x00065778 File Offset: 0x00063978
			public NavigationPathData(List<UsableMachine> allUsablePoints, GameEntity startingEntity, GameEntity endingEntity, int disabledFaceId)
			{
				this.ValidUsableMachinesData = new List<MissionPathGenerationLogic.UsableMachineData>();
				this.StartingGameEntity = startingEntity;
				this.EndingGameEntity = endingEntity;
				this.Path = new NavigationPath();
				PathFaceRecord pathFaceRecord = new PathFaceRecord(-1, -1, -1);
				Mission.Current.Scene.GetNavMeshFaceIndex(ref pathFaceRecord, startingEntity.GlobalPosition, true);
				PathFaceRecord pathFaceRecord2 = new PathFaceRecord(-1, -1, -1);
				Mission.Current.Scene.GetNavMeshFaceIndex(ref pathFaceRecord2, endingEntity.GlobalPosition, true);
				Mission.Current.Scene.GetPathBetweenAIFaces(pathFaceRecord.FaceIndex, pathFaceRecord2.FaceIndex, startingEntity.GlobalPosition.AsVec2, endingEntity.GlobalPosition.AsVec2, 0f, this.Path, new int[] { disabledFaceId }, 1f);
				this.PathNodeAndDistances = new Dictionary<Vec2, float>();
				this.PathNodeAndDistances.Add(this.Path[0], 0f);
				float num = 0f;
				for (int i = 0; i < this.Path.Size - 1; i++)
				{
					Vec2 vec = this.Path[i];
					Vec2 vec2 = this.Path[i + 1];
					num += vec.Distance(vec2);
					this.PathNodeAndDistances.Add(vec2, num);
				}
				this.TotalDistance = num;
				this.InitializeUsablePoints(allUsablePoints);
			}

			// Token: 0x06000E8C RID: 3724 RVA: 0x000658D8 File Offset: 0x00063AD8
			private NavigationPathData(MissionPathGenerationLogic.NavigationPathData navigationPathData)
			{
				this.Path = new NavigationPath();
				this.Path.Size = navigationPathData.Path.Size;
				for (int i = 0; i < navigationPathData.Path.Size; i++)
				{
					this.Path.PathPoints[i] = navigationPathData.Path.PathPoints[this.Path.Size - 1 - i];
				}
				this.TotalDistance = navigationPathData.TotalDistance;
				this.PathNodeAndDistances = new Dictionary<Vec2, float>();
				foreach (KeyValuePair<Vec2, float> keyValuePair in navigationPathData.PathNodeAndDistances)
				{
					this.PathNodeAndDistances.Add(keyValuePair.Key, this.TotalDistance - keyValuePair.Value);
				}
				this.ValidUsableMachinesData = new List<MissionPathGenerationLogic.UsableMachineData>();
				foreach (MissionPathGenerationLogic.UsableMachineData usableMachineData in navigationPathData.ValidUsableMachinesData)
				{
					this.ValidUsableMachinesData.Add(new MissionPathGenerationLogic.UsableMachineData(usableMachineData.MissionObject, usableMachineData.ClosestPointToPath, 1f - usableMachineData.PathDistanceRatio));
				}
				this.StartingGameEntity = navigationPathData.EndingGameEntity;
				this.EndingGameEntity = navigationPathData.StartingGameEntity;
			}

			// Token: 0x06000E8D RID: 3725 RVA: 0x00065A54 File Offset: 0x00063C54
			public MissionPathGenerationLogic.NavigationPathData ReverseClone()
			{
				return new MissionPathGenerationLogic.NavigationPathData(this);
			}

			// Token: 0x06000E8E RID: 3726 RVA: 0x00065A5C File Offset: 0x00063C5C
			private bool GetPositionData(Vec2 position, out Vec2 closestPointToPath, out float pathDistanceRatio)
			{
				bool flag = false;
				closestPointToPath = Vec2.Invalid;
				pathDistanceRatio = 0f;
				float num = float.MaxValue;
				for (int i = 0; i < this.Path.Size - 1; i++)
				{
					Vec2 vec = this.Path[i];
					Vec2 vec2 = this.Path[i + 1];
					Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in vec, in vec2, in position);
					float num2 = position.DistanceSquared(closestPointOnLineSegmentToPoint);
					if (num2 < 2f)
					{
						flag = false;
						break;
					}
					if (num2 < 400f)
					{
						flag = true;
						if (num2 < num)
						{
							closestPointToPath = closestPointOnLineSegmentToPoint;
							num = num2;
							pathDistanceRatio = (this.PathNodeAndDistances[vec] + vec.Distance(closestPointOnLineSegmentToPoint)) / this.TotalDistance;
						}
					}
				}
				return flag;
			}

			// Token: 0x06000E8F RID: 3727 RVA: 0x00065B20 File Offset: 0x00063D20
			public void InitializeUsablePoints(List<UsableMachine> allUsableMachines)
			{
				float num = float.MaxValue;
				float num2 = float.MaxValue;
				float num3 = float.MinValue;
				float num4 = float.MinValue;
				for (int i = 0; i < this.Path.Size; i++)
				{
					Vec2 vec = this.Path[i];
					if (vec.X > num3)
					{
						num3 = vec.X;
					}
					if (vec.X < num)
					{
						num = vec.X;
					}
					if (vec.Y > num4)
					{
						num4 = vec.Y;
					}
					if (vec.Y < num2)
					{
						num2 = vec.Y;
					}
				}
				num3 += 20f;
				num4 += 20f;
				num -= 20f;
				num2 -= 20f;
				foreach (UsableMachine usableMachine in allUsableMachines)
				{
					Vec2 vec2;
					float num5;
					if (usableMachine.GameEntity.GlobalPosition.X <= num3 && usableMachine.GameEntity.GlobalPosition.X >= num && usableMachine.GameEntity.GlobalPosition.Y <= num4 && usableMachine.GameEntity.GlobalPosition.Y >= num2 && !(usableMachine is Chair) && this.GetPositionData(usableMachine.GameEntity.GlobalPosition.AsVec2, out vec2, out num5))
					{
						this.ValidUsableMachinesData.Add(new MissionPathGenerationLogic.UsableMachineData(usableMachine, vec2, num5));
					}
				}
			}

			// Token: 0x04000732 RID: 1842
			public GameEntity StartingGameEntity;

			// Token: 0x04000733 RID: 1843
			public GameEntity EndingGameEntity;

			// Token: 0x04000734 RID: 1844
			public NavigationPath Path;

			// Token: 0x04000735 RID: 1845
			public Dictionary<Vec2, float> PathNodeAndDistances;

			// Token: 0x04000736 RID: 1846
			public List<MissionPathGenerationLogic.UsableMachineData> ValidUsableMachinesData;

			// Token: 0x04000737 RID: 1847
			public float TotalDistance;
		}

		// Token: 0x02000179 RID: 377
		public abstract class PointOfInterestBaseData
		{
			// Token: 0x06000E90 RID: 3728
			public abstract MissionPathGenerationLogic.PointOfInterests GetPointOfInterestType();

			// Token: 0x06000E91 RID: 3729
			public abstract List<ValueTuple<Vec2, float>> GetPositionAndRadiusPairs();

			// Token: 0x06000E92 RID: 3730
			public abstract bool IsInRadius(MissionPathGenerationLogic.PointOfInterestBaseData otherPointOfInterest);

			// Token: 0x06000E93 RID: 3731
			public abstract float GetLocationRatio();

			// Token: 0x04000738 RID: 1848
			public float Score;
		}

		// Token: 0x0200017A RID: 378
		public class LookBackPointData : MissionPathGenerationLogic.PointOfInterestBaseData
		{
			// Token: 0x06000E95 RID: 3733 RVA: 0x00065CD8 File Offset: 0x00063ED8
			public LookBackPointData(WorldPosition position, WorldPosition direction, float pathDistanceRatio)
			{
				this.WorldPosition = position;
				this.PathDistanceRatio = pathDistanceRatio;
				this.DirectionWorldPosition = direction;
			}

			// Token: 0x06000E96 RID: 3734 RVA: 0x00065CF5 File Offset: 0x00063EF5
			public override MissionPathGenerationLogic.PointOfInterests GetPointOfInterestType()
			{
				return MissionPathGenerationLogic.PointOfInterests.LookBackPoint;
			}

			// Token: 0x06000E97 RID: 3735 RVA: 0x00065CF8 File Offset: 0x00063EF8
			public override List<ValueTuple<Vec2, float>> GetPositionAndRadiusPairs()
			{
				return new List<ValueTuple<Vec2, float>>
				{
					new ValueTuple<Vec2, float>(this.WorldPosition.GetNavMeshVec3().AsVec2, 10f)
				};
			}

			// Token: 0x06000E98 RID: 3736 RVA: 0x00065D30 File Offset: 0x00063F30
			public override bool IsInRadius(MissionPathGenerationLogic.PointOfInterestBaseData otherPointOfInterest)
			{
				if (otherPointOfInterest is MissionPathGenerationLogic.LookBackPointData)
				{
					foreach (ValueTuple<Vec2, float> valueTuple in this.GetPositionAndRadiusPairs())
					{
						foreach (ValueTuple<Vec2, float> valueTuple2 in otherPointOfInterest.GetPositionAndRadiusPairs())
						{
							Vec2 item = valueTuple.Item1;
							if (item.Distance(valueTuple2.Item1) < 25f)
							{
								return true;
							}
						}
					}
					return false;
				}
				return false;
			}

			// Token: 0x06000E99 RID: 3737 RVA: 0x00065DE4 File Offset: 0x00063FE4
			public override float GetLocationRatio()
			{
				return this.PathDistanceRatio;
			}

			// Token: 0x04000739 RID: 1849
			public WorldPosition WorldPosition;

			// Token: 0x0400073A RID: 1850
			public WorldPosition DirectionWorldPosition;

			// Token: 0x0400073B RID: 1851
			public float PathDistanceRatio;
		}

		// Token: 0x0200017B RID: 379
		public class VisitPointNodeScoreData : MissionPathGenerationLogic.PointOfInterestBaseData
		{
			// Token: 0x06000E9A RID: 3738 RVA: 0x00065DEC File Offset: 0x00063FEC
			public VisitPointNodeScoreData(MissionPathGenerationLogic.UsableMachineData visitPointData, WorldPosition possibleBlendPointPosition, WorldPosition visitPointPathStartPoint, float visitPointPathStartPointPathRatio, float score, float startingAngle, WorldPosition fWP, WorldPosition sWP, Vec2 pathToVisitPoint, WorldPosition closestPointToBlendPoint)
			{
				this.VisitPointData = visitPointData;
				this.PossibleBlendPointPosition = possibleBlendPointPosition;
				this.VisitPointPathStartPoint = visitPointPathStartPoint;
				this.Score = score;
				this.PathToVisitPoint = pathToVisitPoint;
				this.SWP = sWP;
				this.FWP = fWP;
				this.ClosestPointToBlendPoint = closestPointToBlendPoint;
				this.VisitPointPathStartPointPathRatio = visitPointPathStartPointPathRatio;
				this.StartingAngle = startingAngle;
				this.PositionAndRadiusPairs = new List<ValueTuple<Vec2, float>>();
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(visitPointData.MissionObject.GameEntity.GlobalPosition.AsVec2, 7f));
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(this.PossibleBlendPointPosition.AsVec2, 3f));
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(this.VisitPointPathStartPoint.AsVec2, 3f));
				this.UsingAsInteractablePoint = false;
			}

			// Token: 0x06000E9B RID: 3739 RVA: 0x00065ECE File Offset: 0x000640CE
			public override MissionPathGenerationLogic.PointOfInterests GetPointOfInterestType()
			{
				return MissionPathGenerationLogic.PointOfInterests.VisitPoint;
			}

			// Token: 0x06000E9C RID: 3740 RVA: 0x00065ED1 File Offset: 0x000640D1
			public override List<ValueTuple<Vec2, float>> GetPositionAndRadiusPairs()
			{
				return this.PositionAndRadiusPairs;
			}

			// Token: 0x06000E9D RID: 3741 RVA: 0x00065EDC File Offset: 0x000640DC
			public override bool IsInRadius(MissionPathGenerationLogic.PointOfInterestBaseData otherPointOfInterest)
			{
				float num = 1f;
				if (otherPointOfInterest is MissionPathGenerationLogic.VisitPointNodeScoreData)
				{
					num = 2f;
				}
				else if (otherPointOfInterest is MissionPathGenerationLogic.CrossRoadScoreData)
				{
					num = 0.5f;
				}
				foreach (ValueTuple<Vec2, float> valueTuple in this.PositionAndRadiusPairs)
				{
					Vec2 item = valueTuple.Item1;
					float item2 = valueTuple.Item2;
					foreach (ValueTuple<Vec2, float> valueTuple2 in otherPointOfInterest.GetPositionAndRadiusPairs())
					{
						Vec2 item3 = valueTuple2.Item1;
						float item4 = valueTuple2.Item2;
						if (item.Distance(item3) < (item2 + item4) * num)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06000E9E RID: 3742 RVA: 0x00065FBC File Offset: 0x000641BC
			public override float GetLocationRatio()
			{
				return this.VisitPointData.PathDistanceRatio;
			}

			// Token: 0x0400073C RID: 1852
			public MissionPathGenerationLogic.UsableMachineData VisitPointData;

			// Token: 0x0400073D RID: 1853
			public bool UsingAsInteractablePoint;

			// Token: 0x0400073E RID: 1854
			public WorldPosition PossibleBlendPointPosition;

			// Token: 0x0400073F RID: 1855
			public List<ValueTuple<Vec2, float>> PositionAndRadiusPairs;

			// Token: 0x04000740 RID: 1856
			public WorldPosition VisitPointPathStartPoint;

			// Token: 0x04000741 RID: 1857
			public float VisitPointPathStartPointPathRatio;

			// Token: 0x04000742 RID: 1858
			public WorldPosition ClosestPointToBlendPoint;

			// Token: 0x04000743 RID: 1859
			public WorldPosition FWP;

			// Token: 0x04000744 RID: 1860
			public WorldPosition SWP;

			// Token: 0x04000745 RID: 1861
			public float StartingAngle;

			// Token: 0x04000746 RID: 1862
			public Vec2 PathToVisitPoint;
		}

		// Token: 0x0200017C RID: 380
		public class CrossRoadScoreData : MissionPathGenerationLogic.PointOfInterestBaseData
		{
			// Token: 0x06000E9F RID: 3743 RVA: 0x00065FCC File Offset: 0x000641CC
			public CrossRoadScoreData(MissionPathGenerationLogic.UsableMachineData leftNode, MissionPathGenerationLogic.UsableMachineData rightNode, float score)
			{
				this.LeftNode = leftNode;
				this.RightNode = rightNode;
				this.Score = score;
				this.PositionAndRadiusPairs = new List<ValueTuple<Vec2, float>>();
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(this.LeftNode.MissionObject.GameEntity.GlobalPosition.AsVec2, 1f));
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(this.RightNode.MissionObject.GameEntity.GlobalPosition.AsVec2, 1f));
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(this.RightNode.ClosestPointToPath, 1f));
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(this.LeftNode.ClosestPointToPath, 1f));
			}

			// Token: 0x06000EA0 RID: 3744 RVA: 0x000660A9 File Offset: 0x000642A9
			public override MissionPathGenerationLogic.PointOfInterests GetPointOfInterestType()
			{
				return MissionPathGenerationLogic.PointOfInterests.CrossRoadPoint;
			}

			// Token: 0x06000EA1 RID: 3745 RVA: 0x000660AC File Offset: 0x000642AC
			public override List<ValueTuple<Vec2, float>> GetPositionAndRadiusPairs()
			{
				return this.PositionAndRadiusPairs;
			}

			// Token: 0x06000EA2 RID: 3746 RVA: 0x000660B4 File Offset: 0x000642B4
			public override bool IsInRadius(MissionPathGenerationLogic.PointOfInterestBaseData otherPointOfInterest)
			{
				foreach (ValueTuple<Vec2, float> valueTuple in this.PositionAndRadiusPairs)
				{
					Vec2 item = valueTuple.Item1;
					float item2 = valueTuple.Item2;
					foreach (ValueTuple<Vec2, float> valueTuple2 in otherPointOfInterest.GetPositionAndRadiusPairs())
					{
						Vec2 item3 = valueTuple2.Item1;
						float item4 = valueTuple2.Item2;
						if (item.Distance(item3) < item2 + item4)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06000EA3 RID: 3747 RVA: 0x00066170 File Offset: 0x00064370
			public override float GetLocationRatio()
			{
				return (this.LeftNode.PathDistanceRatio + this.RightNode.PathDistanceRatio) * 0.5f;
			}

			// Token: 0x04000747 RID: 1863
			public MissionPathGenerationLogic.UsableMachineData LeftNode;

			// Token: 0x04000748 RID: 1864
			public MissionPathGenerationLogic.UsableMachineData RightNode;

			// Token: 0x04000749 RID: 1865
			public List<ValueTuple<Vec2, float>> PositionAndRadiusPairs;
		}

		// Token: 0x0200017D RID: 381
		public class StandingGuardSpawnData : MissionPathGenerationLogic.PointOfInterestBaseData
		{
			// Token: 0x06000EA4 RID: 3748 RVA: 0x00066190 File Offset: 0x00064390
			public StandingGuardSpawnData(MissionPathGenerationLogic.UsableMachineData guardPointData, Vec2 spawnDirection, float score)
			{
				this.GuardPointData = guardPointData;
				this.SpawnDirection = spawnDirection;
				this.Score = score;
				this.PositionAndRadiusPairs = new List<ValueTuple<Vec2, float>>();
				this.PositionAndRadiusPairs.Add(new ValueTuple<Vec2, float>(this.GuardPointData.MissionObject.GameEntity.GlobalPosition.AsVec2, 2f));
			}

			// Token: 0x06000EA5 RID: 3749 RVA: 0x000661F8 File Offset: 0x000643F8
			public override MissionPathGenerationLogic.PointOfInterests GetPointOfInterestType()
			{
				return MissionPathGenerationLogic.PointOfInterests.GuardSpawnPoint;
			}

			// Token: 0x06000EA6 RID: 3750 RVA: 0x000661FB File Offset: 0x000643FB
			public override List<ValueTuple<Vec2, float>> GetPositionAndRadiusPairs()
			{
				return this.PositionAndRadiusPairs;
			}

			// Token: 0x06000EA7 RID: 3751 RVA: 0x00066204 File Offset: 0x00064404
			public override bool IsInRadius(MissionPathGenerationLogic.PointOfInterestBaseData otherPointOfInterest)
			{
				foreach (ValueTuple<Vec2, float> valueTuple in this.PositionAndRadiusPairs)
				{
					Vec2 item = valueTuple.Item1;
					float item2 = valueTuple.Item2;
					foreach (ValueTuple<Vec2, float> valueTuple2 in otherPointOfInterest.GetPositionAndRadiusPairs())
					{
						Vec2 item3 = valueTuple2.Item1;
						float item4 = valueTuple2.Item2;
						if (item.Distance(item3) < item2 + item4)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06000EA8 RID: 3752 RVA: 0x000662C0 File Offset: 0x000644C0
			public override float GetLocationRatio()
			{
				return this.GuardPointData.PathDistanceRatio;
			}

			// Token: 0x0400074A RID: 1866
			public MissionPathGenerationLogic.UsableMachineData GuardPointData;

			// Token: 0x0400074B RID: 1867
			public Vec2 SpawnDirection;

			// Token: 0x0400074C RID: 1868
			public List<ValueTuple<Vec2, float>> PositionAndRadiusPairs;
		}

		// Token: 0x0200017E RID: 382
		public class PointOfInterestScorePair
		{
			// Token: 0x17000137 RID: 311
			// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x000662CD File Offset: 0x000644CD
			public List<MissionPathGenerationLogic.PointOfInterestBaseData> Data
			{
				get
				{
					return this._data;
				}
			}

			// Token: 0x06000EAA RID: 3754 RVA: 0x000662D8 File Offset: 0x000644D8
			public PointOfInterestScorePair(MissionPathGenerationLogic.NavigationPathData pathData, List<MissionPathGenerationLogic.PointOfInterestBaseData> data, float score)
			{
				this.PathData = pathData;
				this._data = data;
				this.Score = score;
				this.PointOfInterestCount = new Dictionary<MissionPathGenerationLogic.PointOfInterests, int>();
				foreach (MissionPathGenerationLogic.PointOfInterests pointOfInterests in (MissionPathGenerationLogic.PointOfInterests[])Enum.GetValues(typeof(MissionPathGenerationLogic.PointOfInterests)))
				{
					this.PointOfInterestCount.Add(pointOfInterests, 0);
				}
				foreach (MissionPathGenerationLogic.PointOfInterestBaseData pointOfInterestBaseData in this._data)
				{
					Dictionary<MissionPathGenerationLogic.PointOfInterests, int> pointOfInterestCount = this.PointOfInterestCount;
					MissionPathGenerationLogic.PointOfInterests pointOfInterestType = pointOfInterestBaseData.GetPointOfInterestType();
					int i = pointOfInterestCount[pointOfInterestType];
					pointOfInterestCount[pointOfInterestType] = i + 1;
				}
			}

			// Token: 0x06000EAB RID: 3755 RVA: 0x000663A4 File Offset: 0x000645A4
			private PointOfInterestScorePair(MissionPathGenerationLogic.PointOfInterestScorePair otherPair)
			{
				this.PathData = otherPair.PathData;
				this._data = otherPair._data.ToList<MissionPathGenerationLogic.PointOfInterestBaseData>();
				this.Score = otherPair.Score;
				this.PointOfInterestCount = otherPair.PointOfInterestCount.ToDictionary<KeyValuePair<MissionPathGenerationLogic.PointOfInterests, int>, MissionPathGenerationLogic.PointOfInterests, int>((KeyValuePair<MissionPathGenerationLogic.PointOfInterests, int> x) => x.Key, (KeyValuePair<MissionPathGenerationLogic.PointOfInterests, int> x) => x.Value);
			}

			// Token: 0x06000EAC RID: 3756 RVA: 0x0006642F File Offset: 0x0006462F
			public MissionPathGenerationLogic.PointOfInterestScorePair Clone()
			{
				return new MissionPathGenerationLogic.PointOfInterestScorePair(this);
			}

			// Token: 0x06000EAD RID: 3757 RVA: 0x00066438 File Offset: 0x00064638
			public void AddToData(MissionPathGenerationLogic.PointOfInterestBaseData pointOfInterestToAdd)
			{
				Dictionary<MissionPathGenerationLogic.PointOfInterests, int> pointOfInterestCount = this.PointOfInterestCount;
				MissionPathGenerationLogic.PointOfInterests pointOfInterestType = pointOfInterestToAdd.GetPointOfInterestType();
				int num = pointOfInterestCount[pointOfInterestType];
				pointOfInterestCount[pointOfInterestType] = num + 1;
				this._data.Add(pointOfInterestToAdd);
				this.Score += pointOfInterestToAdd.Score;
			}

			// Token: 0x06000EAE RID: 3758 RVA: 0x00066484 File Offset: 0x00064684
			public bool IsDataEqualTo(MissionPathGenerationLogic.PointOfInterestScorePair other, MissionPathGenerationLogic.PointOfInterestBaseData newDataToAdd)
			{
				if (this.PathData != other.PathData || other.Data.Count + 1 != this.Data.Count || !this.Score.ApproximatelyEqualsTo(other.Score + newDataToAdd.Score, 1E-05f) || this.Data[this.Data.Count - 1] != newDataToAdd)
				{
					return false;
				}
				for (int i = other.Data.Count - 1; i >= 0; i--)
				{
					if (other.Data[i] != this.Data[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06000EAF RID: 3759 RVA: 0x0006652C File Offset: 0x0006472C
			public bool IsBetterThan(MissionPathGenerationLogic.PointOfInterestScorePair other)
			{
				float num = (float)(MissionPathGenerationLogic.MaximumVisitPointCountInPath + MissionPathGenerationLogic.MinimumVisitPointCountInPath) * 0.5f;
				float num2 = Math.Abs((float)this.PointOfInterestCount[MissionPathGenerationLogic.PointOfInterests.VisitPoint] - num);
				float num3 = Math.Abs((float)other.PointOfInterestCount[MissionPathGenerationLogic.PointOfInterests.VisitPoint] - num);
				float num4 = 0.5f;
				float num5 = ((this.Score >= other.Score) ? 0.2f : (-0.2f));
				float num6 = ((num3 >= num2) ? 0.2f : (-0.2f));
				return num4 + num5 + num6 > 0.5f;
			}

			// Token: 0x06000EB0 RID: 3760 RVA: 0x000665B4 File Offset: 0x000647B4
			public bool IsSufficient()
			{
				int num = this.PointOfInterestCount[MissionPathGenerationLogic.PointOfInterests.VisitPoint];
				int num2 = this.PointOfInterestCount[MissionPathGenerationLogic.PointOfInterests.CrossRoadPoint];
				return this.Score >= (float)MissionPathGenerationLogic.ScoreToAchieve && this.PathData.TotalDistance >= (float)MissionPathGenerationLogic.MinimumPathDistance && this.PathData.TotalDistance <= (float)MissionPathGenerationLogic.MaximumPathDistance && num >= MissionPathGenerationLogic.MinimumVisitPointCountInPath && num <= MissionPathGenerationLogic.MaximumVisitPointCountInPath && num2 >= MissionPathGenerationLogic.MinimumCrossRoadCountInPath && num2 <= MissionPathGenerationLogic.MaximumCrossRoadCountInPath;
			}

			// Token: 0x06000EB1 RID: 3761 RVA: 0x00066634 File Offset: 0x00064834
			public void ReOrderDataAccordingToPathRatios()
			{
				this._data = this._data.OrderBy<MissionPathGenerationLogic.PointOfInterestBaseData, float>((MissionPathGenerationLogic.PointOfInterestBaseData x) => x.GetLocationRatio()).ToList<MissionPathGenerationLogic.PointOfInterestBaseData>();
			}

			// Token: 0x0400074D RID: 1869
			public MissionPathGenerationLogic.NavigationPathData PathData;

			// Token: 0x0400074E RID: 1870
			private List<MissionPathGenerationLogic.PointOfInterestBaseData> _data;

			// Token: 0x0400074F RID: 1871
			public Dictionary<MissionPathGenerationLogic.PointOfInterests, int> PointOfInterestCount;

			// Token: 0x04000750 RID: 1872
			public float Score;
		}
	}
}
