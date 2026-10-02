using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000353 RID: 851
	public class SiegeWeaponMovementComponent : UsableMissionObjectComponent
	{
		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060030BD RID: 12477 RVA: 0x000C50E3 File Offset: 0x000C32E3
		public bool HasApproachedTarget
		{
			get
			{
				return !this._pathTracker.PathExists() || this._pathTracker.PathTraveledPercentage > 0.7f;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x000C5106 File Offset: 0x000C3306
		// (set) Token: 0x060030BF RID: 12479 RVA: 0x000C510E File Offset: 0x000C330E
		public Vec3 Velocity { get; private set; }

		// Token: 0x060030C0 RID: 12480 RVA: 0x000C5118 File Offset: 0x000C3318
		protected internal override void OnAdded(Scene scene)
		{
			base.OnAdded(scene);
			this._path = scene.GetPathWithName(this.PathEntityName);
			MatrixFrame matrixFrame = this.MainObject.GameEntity.GetFrame();
			Vec3 scaleVector = matrixFrame.rotation.GetScaleVector();
			this._wheels = GameEntity.CreateFromWeakEntity(this.MainObject.GameEntity).CollectChildrenEntitiesWithTag("wheel");
			this._standingPoints = this.MainObject.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<StandingPoint>("move");
			this._pathTracker = new PathTracker(this._path, scaleVector);
			this._pathTracker.Reset();
			this.SetTargetFrame();
			MatrixFrame globalFrame = this.MainObject.GameEntity.GetGlobalFrame();
			this._standingPointLocalIKFrames = new MatrixFrame[this._standingPoints.Count];
			for (int i = 0; i < this._standingPoints.Count; i++)
			{
				MatrixFrame[] standingPointLocalIKFrames = this._standingPointLocalIKFrames;
				int num = i;
				matrixFrame = this._standingPoints[i].GameEntity.GetGlobalFrame();
				standingPointLocalIKFrames[num] = matrixFrame.TransformToLocal(in globalFrame);
				this._standingPoints[i].AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
			}
			this.Velocity = Vec3.Zero;
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000C5254 File Offset: 0x000C3454
		public void HighlightPath()
		{
			MatrixFrame[] array = new MatrixFrame[this._path.NumberOfPoints];
			this._path.GetPoints(array);
			for (int i = 1; i < this._path.NumberOfPoints; i++)
			{
				MatrixFrame matrixFrame = array[i];
			}
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000C52A4 File Offset: 0x000C34A4
		public void SetupGhostEntity()
		{
			Path pathWithName = this.MainObject.Scene.GetPathWithName(this.PathEntityName);
			Vec3 scaleVector = this.MainObject.GameEntity.GetFrame().rotation.GetScaleVector();
			this._pathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostEntityPathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostObjectPos = ((pathWithName != null) ? pathWithName.GetTotalLength() : 0f);
			this._wheels = GameEntity.CreateFromWeakEntity(this.MainObject.GameEntity).CollectChildrenEntitiesWithTag("wheel");
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x000C533F File Offset: 0x000C353F
		public bool HasArrivedAtTarget
		{
			get
			{
				return !this._pathTracker.PathExists() || this._pathTracker.HasReachedEnd;
			}
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x000C535C File Offset: 0x000C355C
		private void SetPath()
		{
			Path pathWithName = this.MainObject.Scene.GetPathWithName(this.PathEntityName);
			Vec3 scaleVector = this.MainObject.GameEntity.GetFrame().rotation.GetScaleVector();
			this._pathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostEntityPathTracker = new PathTracker(pathWithName, scaleVector);
			this._ghostObjectPos = ((pathWithName != null) ? pathWithName.GetTotalLength() : 0f);
			this.UpdateGhostObject(0f);
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x000C53E2 File Offset: 0x000C35E2
		// (set) Token: 0x060030C6 RID: 12486 RVA: 0x000C53EA File Offset: 0x000C35EA
		public float CurrentSpeed { get; private set; }

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000C53F3 File Offset: 0x000C35F3
		// (set) Token: 0x060030C8 RID: 12488 RVA: 0x000C53FB File Offset: 0x000C35FB
		public int MovementSoundCodeID { get; set; }

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000C5404 File Offset: 0x000C3604
		// (set) Token: 0x060030CA RID: 12490 RVA: 0x000C540C File Offset: 0x000C360C
		public float MinSpeed { get; set; }

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000C5415 File Offset: 0x000C3615
		// (set) Token: 0x060030CC RID: 12492 RVA: 0x000C541D File Offset: 0x000C361D
		public float MaxSpeed { get; set; }

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x000C5426 File Offset: 0x000C3626
		// (set) Token: 0x060030CE RID: 12494 RVA: 0x000C542E File Offset: 0x000C362E
		public string PathEntityName { get; set; }

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x000C5437 File Offset: 0x000C3637
		// (set) Token: 0x060030D0 RID: 12496 RVA: 0x000C543F File Offset: 0x000C363F
		public float GhostEntitySpeedMultiplier { get; set; }

		// Token: 0x17000925 RID: 2341
		// (set) Token: 0x060030D1 RID: 12497 RVA: 0x000C5448 File Offset: 0x000C3648
		public float WheelDiameter
		{
			set
			{
				this._wheelDiameter = value;
				this._wheelCircumference = this._wheelDiameter * 3.1415927f;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x000C5463 File Offset: 0x000C3663
		// (set) Token: 0x060030D3 RID: 12499 RVA: 0x000C546B File Offset: 0x000C366B
		public SynchedMissionObject MainObject { get; set; }

		// Token: 0x060030D4 RID: 12500 RVA: 0x000C5474 File Offset: 0x000C3674
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.UpdateGhostObject(dt);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000C5484 File Offset: 0x000C3684
		public void SetGhostVisibility(bool isVisible)
		{
			this.MainObject.GameEntity.CollectChildrenEntitiesWithTag("ghost_object").FirstOrDefault<WeakGameEntity>().SetVisibilityExcludeParents(isVisible);
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000C54B7 File Offset: 0x000C36B7
		public void OnEditorInit()
		{
			this.SetPath();
			this._wheels = GameEntity.CreateFromWeakEntity(this.MainObject.GameEntity).CollectChildrenEntitiesWithTag("wheel");
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000C54E0 File Offset: 0x000C36E0
		private void UpdateGhostObject(float dt)
		{
			if (this._pathTracker.HasChanged)
			{
				this.SetPath();
				this._pathTracker.Advance(this._pathTracker.GetPathLength());
				this._ghostEntityPathTracker.Advance(this._ghostEntityPathTracker.GetPathLength());
			}
			List<WeakGameEntity> list = this.MainObject.GameEntity.CollectChildrenEntitiesWithTag("ghost_object");
			if (this.MainObject.GameEntity.IsSelectedOnEditor())
			{
				if (this._pathTracker.IsValid)
				{
					float num = 10f;
					if (Input.DebugInput.IsShiftDown())
					{
						num = 1f;
					}
					if (Input.DebugInput.IsKeyDown(InputKey.MouseScrollUp))
					{
						this._ghostObjectPos += dt * num;
					}
					else if (Input.DebugInput.IsKeyDown(InputKey.MouseScrollDown))
					{
						this._ghostObjectPos -= dt * num;
					}
					this._ghostObjectPos = MBMath.ClampFloat(this._ghostObjectPos, 0f, this._pathTracker.GetPathLength());
				}
				else
				{
					this._ghostObjectPos = 0f;
				}
			}
			if (list.Count > 0)
			{
				WeakGameEntity weakGameEntity = list[0];
				IPathHolder pathHolder;
				if ((pathHolder = this.MainObject as IPathHolder) != null && pathHolder.EditorGhostEntityMove)
				{
					if (this._ghostEntityPathTracker.IsValid)
					{
						this._ghostEntityPathTracker.Advance(0.05f * this.GhostEntitySpeedMultiplier);
						MatrixFrame matrixFrame = this.LinearInterpolatedIK(ref this._ghostEntityPathTracker);
						weakGameEntity.SetGlobalFrame(in matrixFrame, true);
						if (this._ghostEntityPathTracker.HasReachedEnd)
						{
							this._ghostEntityPathTracker.Reset();
							return;
						}
					}
				}
				else if (this._pathTracker.IsValid)
				{
					this._pathTracker.Advance(this._ghostObjectPos);
					MatrixFrame matrixFrame2 = this.LinearInterpolatedIK(ref this._pathTracker);
					MatrixFrame matrixFrame3 = this.FindGroundFrameForWheels(ref matrixFrame2);
					weakGameEntity.SetGlobalFrame(in matrixFrame3, true);
					this._pathTracker.Reset();
				}
			}
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000C56C4 File Offset: 0x000C38C4
		private void RotateWheels(float angleInRadian)
		{
			foreach (GameEntity gameEntity in this._wheels)
			{
				MatrixFrame frame = gameEntity.GetFrame();
				frame.rotation.RotateAboutSide(angleInRadian);
				gameEntity.SetFrame(ref frame, true);
			}
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000C572C File Offset: 0x000C392C
		private MatrixFrame LinearInterpolatedIK(ref PathTracker pathTracker)
		{
			MatrixFrame matrixFrame;
			Vec3 vec;
			pathTracker.CurrentFrameAndColor(out matrixFrame, out vec);
			MatrixFrame matrixFrame2 = this.FindGroundFrameForWheels(ref matrixFrame);
			return MatrixFrame.Lerp(in matrixFrame, in matrixFrame2, vec.x);
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000C575C File Offset: 0x000C395C
		public void SetDistanceTraveledAsClient(float distance)
		{
			this._advancementError = distance - this._pathTracker.TotalDistanceTraveled;
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000C5771 File Offset: 0x000C3971
		public override bool IsOnTickRequired()
		{
			return true;
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000C5774 File Offset: 0x000C3974
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._ghostEntityPathTracker != null)
			{
				this.UpdateGhostObject(dt);
			}
			if (!this._pathTracker.PathExists() || this._pathTracker.HasReachedEnd)
			{
				this.CurrentSpeed = 0f;
				if (!GameNetwork.IsClientOrReplay)
				{
					foreach (StandingPoint standingPoint in this._standingPoints)
					{
						standingPoint.SetIsDeactivatedSynched(true);
					}
				}
			}
			this.TickSound();
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000C5810 File Offset: 0x000C3A10
		public void TickParallelManually(float dt)
		{
			if (this._pathTracker.PathExists() && !this._pathTracker.HasReachedEnd)
			{
				int num = 0;
				foreach (StandingPoint standingPoint in this._standingPoints)
				{
					if (standingPoint.HasUser && !standingPoint.UserAgent.IsInBeingStruckAction)
					{
						num++;
					}
				}
				if (num > 0)
				{
					int count = this._standingPoints.Count;
					this.CurrentSpeed = MBMath.Lerp(this.MinSpeed, this.MaxSpeed, (float)(num - 1) / (float)(count - 1), 1E-05f);
					MatrixFrame globalFrame = this.MainObject.GameEntity.GetGlobalFrame();
					for (int i = 0; i < this._standingPoints.Count; i++)
					{
						StandingPoint standingPoint2 = this._standingPoints[i];
						if (standingPoint2.HasUser)
						{
							Agent userAgent = standingPoint2.UserAgent;
							ActionIndexCache actionIndexCache = userAgent.GetCurrentAction(0);
							ActionIndexCache actionIndexCache2 = userAgent.GetCurrentAction(1);
							if (actionIndexCache != ActionIndexCache.act_usage_siege_machine_push)
							{
								if (userAgent.SetActionChannel(0, in ActionIndexCache.act_usage_siege_machine_push, false, (AnimFlags)0UL, 0f, this.CurrentSpeed, MBAnimation.GetAnimationBlendInPeriod(MBActionSet.GetAnimationIndexOfAction(userAgent.ActionSet, in ActionIndexCache.act_usage_siege_machine_push)) * this.CurrentSpeed, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache = ActionIndexCache.act_usage_siege_machine_push;
								}
								else if (MBMath.IsBetween((int)userAgent.GetCurrentActionType(0), 48, 52) && actionIndexCache != ActionIndexCache.act_strike_bent_over && userAgent.SetActionChannel(0, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache = ActionIndexCache.act_strike_bent_over;
								}
							}
							if (actionIndexCache2 != ActionIndexCache.act_usage_siege_machine_push)
							{
								if (userAgent.SetActionChannel(1, in ActionIndexCache.act_usage_siege_machine_push, false, (AnimFlags)0UL, 0f, this.CurrentSpeed, MBAnimation.GetAnimationBlendInPeriod(MBActionSet.GetAnimationIndexOfAction(userAgent.ActionSet, in ActionIndexCache.act_usage_siege_machine_push)) * this.CurrentSpeed, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache2 = ActionIndexCache.act_usage_siege_machine_push;
								}
								else if (MBMath.IsBetween((int)userAgent.GetCurrentActionType(1), 48, 52) && actionIndexCache2 != ActionIndexCache.act_strike_bent_over && userAgent.SetActionChannel(1, in ActionIndexCache.act_strike_bent_over, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									actionIndexCache2 = ActionIndexCache.act_strike_bent_over;
								}
							}
							if (actionIndexCache == ActionIndexCache.act_usage_siege_machine_push)
							{
								userAgent.SetCurrentActionSpeed(0, this.CurrentSpeed);
							}
							if (actionIndexCache2 == ActionIndexCache.act_usage_siege_machine_push)
							{
								userAgent.SetCurrentActionSpeed(1, this.CurrentSpeed);
							}
							if ((actionIndexCache == ActionIndexCache.act_usage_siege_machine_push || actionIndexCache == ActionIndexCache.act_strike_bent_over) && (actionIndexCache2 == ActionIndexCache.act_usage_siege_machine_push || actionIndexCache2 == ActionIndexCache.act_strike_bent_over))
							{
								standingPoint2.UserAgent.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._standingPointLocalIKFrames[i], in globalFrame, 0f);
							}
							else
							{
								standingPoint2.UserAgent.ClearHandInverseKinematics();
								if (!GameNetwork.IsClientOrReplay && userAgent.Controller != AgentControllerType.AI)
								{
									userAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
								}
							}
						}
					}
				}
				else
				{
					this.CurrentSpeed = this._advancementError;
				}
				if (!this.CurrentSpeed.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					float num2 = this.CurrentSpeed * dt;
					if (!this._advancementError.ApproximatelyEqualsTo(0f, 1E-05f))
					{
						float num3 = 3f * this.CurrentSpeed * dt * (float)MathF.Sign(this._advancementError);
						if (MathF.Abs(num3) >= MathF.Abs(this._advancementError))
						{
							num3 = this._advancementError;
							this._advancementError = 0f;
						}
						else
						{
							this._advancementError -= num3;
						}
						num2 += num3;
					}
					this._pathTracker.Advance(num2);
					this.SetTargetFrame();
					float num4 = num2 / this._wheelCircumference * 2f * 3.1415927f;
					this.RotateWheels(num4);
					if (GameNetwork.IsServerOrRecorder && this._pathTracker.TotalDistanceTraveled - this._lastSynchronizedDistance > 1f)
					{
						this._lastSynchronizedDistance = this._pathTracker.TotalDistanceTraveled;
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetSiegeMachineMovementDistance(this.MainObject.Id, this._lastSynchronizedDistance));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
				}
			}
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000C5CA8 File Offset: 0x000C3EA8
		public MatrixFrame GetInitialFrame()
		{
			PathTracker pathTracker = new PathTracker(this._path, Vec3.One);
			pathTracker.Reset();
			return this.LinearInterpolatedIK(ref pathTracker);
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000C5CD4 File Offset: 0x000C3ED4
		private void SetTargetFrame()
		{
			if (!this._pathTracker.PathExists())
			{
				return;
			}
			MatrixFrame matrixFrame = this.LinearInterpolatedIK(ref this._pathTracker);
			WeakGameEntity gameEntity = this.MainObject.GameEntity;
			this.Velocity = gameEntity.GlobalPosition;
			gameEntity.SetGlobalFrame(in matrixFrame, false);
			this.Velocity = (gameEntity.GlobalPosition - this.Velocity).NormalizedCopy() * this.CurrentSpeed;
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000C5D4C File Offset: 0x000C3F4C
		public MatrixFrame GetTargetFrame()
		{
			float totalDistanceTraveled = this._pathTracker.TotalDistanceTraveled;
			this._pathTracker.Advance(1000000f);
			MatrixFrame currentFrame = this._pathTracker.CurrentFrame;
			this._pathTracker.Reset();
			this._pathTracker.Advance(totalDistanceTraveled);
			return currentFrame;
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x000C5D97 File Offset: 0x000C3F97
		public void SetDestinationNavMeshIdState(bool enabled)
		{
			if (this.NavMeshIdToDisableOnDestination != -1)
			{
				Mission.Current.Scene.SetAbilityOfFacesWithId(this.NavMeshIdToDisableOnDestination, enabled);
			}
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x000C5DBC File Offset: 0x000C3FBC
		public void MoveToTargetAsClient()
		{
			if (this._pathTracker.IsValid)
			{
				float totalDistanceTraveled = this._pathTracker.TotalDistanceTraveled;
				this._pathTracker.Advance(1000000f);
				this.SetTargetFrame();
				float num = (this._pathTracker.TotalDistanceTraveled - totalDistanceTraveled) / this._wheelCircumference * 2f * 3.1415927f;
				this.RotateWheels(num);
			}
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000C5E20 File Offset: 0x000C4020
		private void TickSound()
		{
			if (this.CurrentSpeed > 0f)
			{
				this.PlayMovementSound();
				return;
			}
			this.StopMovementSound();
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000C5E3C File Offset: 0x000C403C
		private void PlayMovementSound()
		{
			if (!this._isMoveSoundPlaying)
			{
				this._movementSound = SoundEvent.CreateEvent(this.MovementSoundCodeID, this.MainObject.GameEntity.Scene);
				this._movementSound.Play();
				this._isMoveSoundPlaying = true;
			}
			this._movementSound.SetPosition(this.MainObject.GameEntity.GlobalPosition);
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000C5EA6 File Offset: 0x000C40A6
		private void StopMovementSound()
		{
			if (this._isMoveSoundPlaying)
			{
				this._movementSound.Stop();
				this._isMoveSoundPlaying = false;
			}
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000C5EC2 File Offset: 0x000C40C2
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.CurrentSpeed = 0f;
			this._lastSynchronizedDistance = 0f;
			this._advancementError = 0f;
			this._pathTracker.Reset();
			this.SetTargetFrame();
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000C5EFC File Offset: 0x000C40FC
		public float GetTotalDistanceTraveledForPathTracker()
		{
			return this._pathTracker.TotalDistanceTraveled;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000C5F09 File Offset: 0x000C4109
		private MatrixFrame FindGroundFrameForWheels(ref MatrixFrame frame)
		{
			return SiegeWeaponMovementComponent.FindGroundFrameForWheelsStatic(ref frame, this.AxleLength, this._wheelDiameter, this.MainObject.GameEntity, this._wheels, this.MainObject.Scene);
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000C5F39 File Offset: 0x000C4139
		public void SetTotalDistanceTraveledForPathTracker(float distanceTraveled)
		{
			this._pathTracker.TotalDistanceTraveled = distanceTraveled;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x000C5F47 File Offset: 0x000C4147
		public void SetTargetFrameForPathTracker()
		{
			this.SetTargetFrame();
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x000C5F50 File Offset: 0x000C4150
		public static MatrixFrame FindGroundFrameForWheelsStatic(ref MatrixFrame frame, float axleLength, float wheelDiameter, WeakGameEntity gameEntity, List<GameEntity> wheels, Scene scene)
		{
			Vec3.StackArray8Vec3 stackArray8Vec = default(Vec3.StackArray8Vec3);
			bool visibilityExcludeParents = gameEntity.GetVisibilityExcludeParents();
			if (visibilityExcludeParents)
			{
				gameEntity.SetVisibilityExcludeParents(false);
			}
			int num = 0;
			using (new TWSharedMutexReadLock(Scene.PhysicsAndRayCastLock))
			{
				foreach (GameEntity gameEntity2 in wheels)
				{
					Vec3 vec = frame.TransformToParent(in gameEntity2.GetFrame().origin);
					Vec3 vec2 = vec + frame.rotation.s * axleLength + (wheelDiameter * 0.5f + 0.5f) * frame.rotation.u;
					Vec3 vec3 = vec - frame.rotation.s * axleLength + (wheelDiameter * 0.5f + 0.5f) * frame.rotation.u;
					vec2.z = scene.GetGroundHeightAtPosition(vec2, BodyFlags.CommonCollisionExcludeFlags);
					vec3.z = scene.GetGroundHeightAtPosition(vec3, BodyFlags.CommonCollisionExcludeFlags);
					stackArray8Vec[num++] = vec2;
					stackArray8Vec[num++] = vec3;
				}
			}
			if (visibilityExcludeParents)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			Vec3 vec4 = default(Vec3);
			for (int i = 0; i < num; i++)
			{
				vec4 += stackArray8Vec[i];
			}
			vec4 /= (float)num;
			for (int j = 0; j < num; j++)
			{
				Vec3 vec5 = stackArray8Vec[j] - vec4;
				num2 += vec5.x * vec5.x;
				num3 += vec5.x * vec5.y;
				num4 += vec5.y * vec5.y;
				num5 += vec5.x * vec5.z;
				num6 += vec5.y * vec5.z;
			}
			float num7 = num2 * num4 - num3 * num3;
			float num8 = (num6 * num3 - num5 * num4) / num7;
			float num9 = (num3 * num5 - num2 * num6) / num7;
			MatrixFrame matrixFrame;
			matrixFrame.origin = vec4;
			matrixFrame.rotation.u = new Vec3(num8, num9, 1f, -1f);
			matrixFrame.rotation.u.Normalize();
			matrixFrame.rotation.f = frame.rotation.f;
			matrixFrame.rotation.f = matrixFrame.rotation.f - Vec3.DotProduct(matrixFrame.rotation.f, matrixFrame.rotation.u) * matrixFrame.rotation.u;
			matrixFrame.rotation.f.Normalize();
			matrixFrame.rotation.s = Vec3.CrossProduct(matrixFrame.rotation.f, matrixFrame.rotation.u);
			matrixFrame.rotation.s.Normalize();
			return matrixFrame;
		}

		// Token: 0x04001468 RID: 5224
		public const string GhostObjectTag = "ghost_object";

		// Token: 0x04001469 RID: 5225
		private const string WheelTag = "wheel";

		// Token: 0x0400146A RID: 5226
		public const string MoveStandingPointTag = "move";

		// Token: 0x0400146B RID: 5227
		public float AxleLength = 2.45f;

		// Token: 0x0400146C RID: 5228
		public int NavMeshIdToDisableOnDestination = -1;

		// Token: 0x0400146D RID: 5229
		private float _ghostObjectPos;

		// Token: 0x0400146E RID: 5230
		private List<GameEntity> _wheels;

		// Token: 0x0400146F RID: 5231
		private List<StandingPoint> _standingPoints;

		// Token: 0x04001470 RID: 5232
		private MatrixFrame[] _standingPointLocalIKFrames;

		// Token: 0x04001471 RID: 5233
		private SoundEvent _movementSound;

		// Token: 0x04001472 RID: 5234
		private float _wheelCircumference;

		// Token: 0x04001473 RID: 5235
		private bool _isMoveSoundPlaying;

		// Token: 0x04001474 RID: 5236
		private float _wheelDiameter;

		// Token: 0x04001475 RID: 5237
		private Path _path;

		// Token: 0x04001476 RID: 5238
		private PathTracker _pathTracker;

		// Token: 0x04001477 RID: 5239
		private PathTracker _ghostEntityPathTracker;

		// Token: 0x04001478 RID: 5240
		private float _advancementError;

		// Token: 0x04001479 RID: 5241
		private float _lastSynchronizedDistance;
	}
}
