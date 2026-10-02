using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000362 RID: 866
	public class SynchedMissionObject : MissionObject
	{
		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x000CA4D4 File Offset: 0x000C86D4
		// (set) Token: 0x06003197 RID: 12695 RVA: 0x000CA4DC File Offset: 0x000C86DC
		public uint Color { get; private set; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003198 RID: 12696 RVA: 0x000CA4E5 File Offset: 0x000C86E5
		// (set) Token: 0x06003199 RID: 12697 RVA: 0x000CA4ED File Offset: 0x000C86ED
		public uint Color2 { get; private set; }

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x000CA4F6 File Offset: 0x000C86F6
		public bool SynchronizeCompleted
		{
			get
			{
				return this._synchState == SynchedMissionObject.SynchState.SynchronizeCompleted;
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000CA501 File Offset: 0x000C8701
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x000CA515 File Offset: 0x000C8715
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (!this.SynchronizeCompleted)
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000CA530 File Offset: 0x000C8730
		protected internal override void OnTick(float dt)
		{
			if (!this.SynchronizeCompleted)
			{
				MatrixFrame frame = base.GameEntity.GetFrame();
				if ((this._synchState == SynchedMissionObject.SynchState.SynchronizePosition && this._lastSynchedFrame.origin.NearlyEquals(in frame.origin, 1E-05f)) || this._lastSynchedFrame.NearlyEquals(frame, 1E-05f))
				{
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeCompleted);
					return;
				}
				MatrixFrame matrixFrame;
				matrixFrame.origin = ((this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime) ? MBMath.Lerp(this._firstFrame.origin, this._lastSynchedFrame.origin, this._timer / this._duration, 0.2f * dt) : MBMath.Lerp(frame.origin, this._lastSynchedFrame.origin, 8f * dt, 0.2f * dt));
				if (this._synchState == SynchedMissionObject.SynchState.SynchronizeFrame || this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime)
				{
					matrixFrame.rotation.s = MBMath.Lerp(frame.rotation.s, this._lastSynchedFrame.rotation.s, 8f * dt, 0.2f * dt);
					matrixFrame.rotation.f = MBMath.Lerp(frame.rotation.f, this._lastSynchedFrame.rotation.f, 8f * dt, 0.2f * dt);
					matrixFrame.rotation.u = MBMath.Lerp(frame.rotation.u, this._lastSynchedFrame.rotation.u, 8f * dt, 0.2f * dt);
					if (matrixFrame.origin != this._lastSynchedFrame.origin || matrixFrame.rotation.s != this._lastSynchedFrame.rotation.s || matrixFrame.rotation.f != this._lastSynchedFrame.rotation.f || matrixFrame.rotation.u != this._lastSynchedFrame.rotation.u)
					{
						matrixFrame.rotation.Orthonormalize();
						if (this._lastSynchedFrame.rotation.HasScale())
						{
							Vec3 scaleVector = this._lastSynchedFrame.rotation.GetScaleVector();
							matrixFrame.rotation.ApplyScaleLocal(in scaleVector);
						}
					}
					base.GameEntity.SetFrame(ref matrixFrame, true);
				}
				else
				{
					base.GameEntity.SetLocalPosition(matrixFrame.origin);
				}
				this._timer = MathF.Min(this._timer + dt, this._duration);
			}
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000CA7BC File Offset: 0x000C89BC
		private void SetSynchState(SynchedMissionObject.SynchState newState)
		{
			if (newState != this._synchState)
			{
				this._synchState = newState;
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000CA7DA File Offset: 0x000C89DA
		public void SetLocalPositionSmoothStep(ref Vec3 targetPosition)
		{
			this._lastSynchedFrame.origin = targetPosition;
			this.SetSynchState(SynchedMissionObject.SynchState.SynchronizePosition);
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000CA7F4 File Offset: 0x000C89F4
		public virtual void SetVisibleSynched(bool value, bool forceChildrenVisible = false)
		{
			bool flag = base.GameEntity.IsVisibleIncludeParents() != value;
			List<WeakGameEntity> list = null;
			if (!flag && forceChildrenVisible)
			{
				list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				using (List<WeakGameEntity>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetPhysicsState() != value)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (base.GameEntity.IsValid && flag)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectVisibility(base.Id, value));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.SetVisibilityExcludeParents(value);
				if (forceChildrenVisible)
				{
					if (list == null)
					{
						list = new List<WeakGameEntity>();
						base.GameEntity.GetChildrenRecursive(ref list);
					}
					foreach (WeakGameEntity weakGameEntity in list)
					{
						weakGameEntity.SetVisibilityExcludeParents(value);
					}
				}
			}
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000CA920 File Offset: 0x000C8B20
		public virtual void SetPhysicsStateSynched(bool value, bool setChildren = true)
		{
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000CA922 File Offset: 0x000C8B22
		public virtual void SetDisabledSynched()
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetMissionObjectDisabled(base.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			base.SetDisabledAndMakeInvisible(false, false);
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000CA950 File Offset: 0x000C8B50
		public void SetFrameSynched(ref MatrixFrame frame, bool isClient = false)
		{
			MatrixFrame frame2 = base.GameEntity.GetFrame();
			if ((in frame2) != (in frame) || this._synchState != SynchedMissionObject.SynchState.SynchronizeCompleted)
			{
				this._duration = 0f;
				this._timer = 0f;
				if (GameNetwork.IsClientOrReplay)
				{
					this._lastSynchedFrame = frame;
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrame);
					return;
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectFrame(base.Id, ref frame));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeCompleted);
				base.GameEntity.SetFrame(ref frame, true);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000CA9FC File Offset: 0x000C8BFC
		public void SetGlobalFrameSynched(ref MatrixFrame frame, bool isClient = false)
		{
			this._duration = 0f;
			this._timer = 0f;
			MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
			if ((in matrixFrame) != (in frame))
			{
				if (GameNetwork.IsClientOrReplay)
				{
					MatrixFrame matrixFrame2;
					if (!base.GameEntity.Parent.IsValid)
					{
						matrixFrame2 = frame;
					}
					else
					{
						matrixFrame = base.GameEntity.Parent.GetGlobalFrame();
						matrixFrame2 = matrixFrame.TransformToLocalNonOrthogonal(in frame);
					}
					this._lastSynchedFrame = matrixFrame2;
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrame);
					return;
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectGlobalFrame(base.Id, ref frame));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeCompleted);
				base.GameEntity.SetGlobalFrame(in frame, true);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000CAADC File Offset: 0x000C8CDC
		public void SetFrameSynchedOverTime(ref MatrixFrame frame, float duration, bool isClient = false)
		{
			MatrixFrame frame2 = base.GameEntity.GetFrame();
			if ((in frame2) != (in frame) || duration.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._firstFrame = base.GameEntity.GetFrame();
				this._lastSynchedFrame = frame;
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
				this._duration = (duration.ApproximatelyEqualsTo(0f, 1E-05f) ? 0.1f : duration);
				this._timer = 0f;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectFrameOverTime(base.Id, ref frame, duration));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x000CABA0 File Offset: 0x000C8DA0
		public void SetGlobalFrameSynchedOverTime(ref MatrixFrame frame, float duration, bool isClient = false)
		{
			MatrixFrame matrixFrame = base.GameEntity.GetGlobalFrame();
			if ((in matrixFrame) != (in frame) || duration.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._firstFrame = base.GameEntity.GetFrame();
				MatrixFrame matrixFrame2;
				if (!base.GameEntity.Parent.IsValid)
				{
					matrixFrame2 = frame;
				}
				else
				{
					matrixFrame = base.GameEntity.Parent.GetGlobalFrame();
					matrixFrame2 = matrixFrame.TransformToLocalNonOrthogonal(in frame);
				}
				this._lastSynchedFrame = matrixFrame2;
				this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
				this._duration = (duration.ApproximatelyEqualsTo(0f, 1E-05f) ? 0.1f : duration);
				this._timer = 0f;
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectGlobalFrameOverTime(base.Id, ref frame, duration));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
			}
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000CAC9A File Offset: 0x000C8E9A
		public void SetAnimationAtChannelSynched(string animationName, int channelNo, float animationSpeed = 1f)
		{
			this.SetAnimationAtChannelSynched(MBAnimation.GetAnimationIndexWithName(animationName), channelNo, animationSpeed);
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000CACAC File Offset: 0x000C8EAC
		public void SetAnimationAtChannelSynched(int animationIndex, int channelNo, float animationSpeed = 1f)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				int animationIndexAtChannel = base.GameEntity.Skeleton.GetAnimationIndexAtChannel(channelNo);
				bool flag = true;
				if (animationIndexAtChannel == animationIndex && base.GameEntity.Skeleton.GetAnimationSpeedAtChannel(channelNo).ApproximatelyEqualsTo(animationSpeed, 1E-05f) && base.GameEntity.Skeleton.GetAnimationParameterAtChannel(channelNo) < 0.02f)
				{
					flag = false;
				}
				if (flag)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationAtChannel(base.Id, channelNo, animationIndex, animationSpeed));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
				}
			}
			base.GameEntity.Skeleton.SetAnimationAtChannel(animationIndex, channelNo, animationSpeed, -1f, 0f);
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000CAD6C File Offset: 0x000C8F6C
		public void SetAnimationChannelParameterSynched(int channelNo, float parameter)
		{
			if (!base.GameEntity.Skeleton.GetAnimationParameterAtChannel(channelNo).ApproximatelyEqualsTo(parameter, 1E-05f))
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationChannelParameter(base.Id, channelNo, parameter));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(channelNo, parameter);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000CADE4 File Offset: 0x000C8FE4
		public void PauseSkeletonAnimationSynched()
		{
			if (!base.GameEntity.IsSkeletonAnimationPaused())
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationPaused(base.Id, true));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.PauseSkeletonAnimation();
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
			}
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000CAE44 File Offset: 0x000C9044
		public void ResumeSkeletonAnimationSynched()
		{
			if (base.GameEntity.IsSkeletonAnimationPaused())
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectAnimationPaused(base.Id, false));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.ResumeSkeletonAnimation();
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchAnimation;
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000CAEA4 File Offset: 0x000C90A4
		public void BurstParticlesSynched(bool doChildren = true)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new BurstMissionObjectParticles(base.Id, false));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			base.GameEntity.BurstEntityParticle(doChildren);
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000CAEE8 File Offset: 0x000C90E8
		public void ApplyImpulseSynched(Vec3 localPosition, Vec3 impulse)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetMissionObjectImpulse(base.Id, localPosition, impulse));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			base.GameEntity.ApplyLocalImpulseToDynamicBody(localPosition, impulse);
			this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchTransform;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000CAF38 File Offset: 0x000C9138
		public void AddBodyFlagsSynched(BodyFlags flags, bool applyToChildren = true)
		{
			if ((base.GameEntity.BodyFlag & flags) != flags)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new AddMissionObjectBodyFlags(base.Id, flags, applyToChildren));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.AddBodyFlags(flags, applyToChildren);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchBodyFlags;
			}
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000CAF9C File Offset: 0x000C919C
		public void RemoveBodyFlagsSynched(BodyFlags flags, bool applyToChildren = true)
		{
			if ((base.GameEntity.BodyFlag & flags) != BodyFlags.None)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new RemoveMissionObjectBodyFlags(base.Id, flags, applyToChildren));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				base.GameEntity.RemoveBodyFlags(flags, applyToChildren);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SynchBodyFlags;
			}
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000CB000 File Offset: 0x000C9200
		public void SetTeamColors(uint color, uint color2)
		{
			this.Color = color;
			this.Color2 = color2;
			base.GameEntity.SetColor(color, color2, "use_team_color");
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000CB030 File Offset: 0x000C9230
		public virtual void SetTeamColorsSynched(uint color, uint color2)
		{
			if (base.GameEntity.IsValid)
			{
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SetMissionObjectColors(base.Id, color, color2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				this.SetTeamColors(color, color2);
				this._initialSynchFlags |= SynchedMissionObject.SynchFlags.SyncColors;
			}
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000CB08C File Offset: 0x000C928C
		public virtual void WriteToNetwork()
		{
			GameNetworkMessage.WriteBoolToPacket(base.GameEntity.GetVisibilityExcludeParents());
			GameNetworkMessage.WriteBoolToPacket(this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchTransform));
			if (this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchTransform))
			{
				GameNetworkMessage.WriteMatrixFrameToPacket(base.GameEntity.GetFrame());
				GameNetworkMessage.WriteBoolToPacket(this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
				if (this._synchState == SynchedMissionObject.SynchState.SynchronizeFrameOverTime)
				{
					GameNetworkMessage.WriteMatrixFrameToPacket(this._lastSynchedFrame);
					GameNetworkMessage.WriteFloatToPacket(this._duration - this._timer, CompressionMission.FlagCapturePointDurationCompressionInfo);
				}
			}
			Skeleton skeleton = base.GameEntity.Skeleton;
			GameNetworkMessage.WriteBoolToPacket(skeleton != null);
			if (skeleton != null)
			{
				int animationIndexAtChannel = skeleton.GetAnimationIndexAtChannel(0);
				bool flag = animationIndexAtChannel >= 0;
				GameNetworkMessage.WriteBoolToPacket(flag && this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchAnimation));
				if (flag && this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SynchAnimation))
				{
					float animationSpeedAtChannel = skeleton.GetAnimationSpeedAtChannel(0);
					float animationParameterAtChannel = skeleton.GetAnimationParameterAtChannel(0);
					GameNetworkMessage.WriteIntToPacket(animationIndexAtChannel, CompressionBasic.AnimationIndexCompressionInfo);
					GameNetworkMessage.WriteFloatToPacket(animationSpeedAtChannel, CompressionBasic.AnimationSpeedCompressionInfo);
					GameNetworkMessage.WriteFloatToPacket(animationParameterAtChannel, CompressionBasic.AnimationProgressCompressionInfo);
					GameNetworkMessage.WriteBoolToPacket(base.GameEntity.IsSkeletonAnimationPaused());
				}
			}
			GameNetworkMessage.WriteBoolToPacket(this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SyncColors));
			if (this._initialSynchFlags.HasAnyFlag(SynchedMissionObject.SynchFlags.SyncColors))
			{
				GameNetworkMessage.WriteUintToPacket(this.Color, CompressionBasic.ColorCompressionInfo);
				GameNetworkMessage.WriteUintToPacket(this.Color2, CompressionBasic.ColorCompressionInfo);
			}
			GameNetworkMessage.WriteBoolToPacket(base.IsDisabled);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000CB200 File Offset: 0x000C9400
		public virtual void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			BaseSynchedMissionObjectReadableRecord item = synchedMissionObjectReadableRecord.Item1;
			if (allowVisibilityUpdate)
			{
				base.GameEntity.SetVisibilityExcludeParents(item.SetVisibilityExcludeParents);
			}
			if (item.SynchTransform)
			{
				MatrixFrame gameObjectFrame = item.GameObjectFrame;
				base.GameEntity.SetFrame(ref gameObjectFrame, true);
				if (item.SynchronizeFrameOverTime)
				{
					this._firstFrame = item.GameObjectFrame;
					this._lastSynchedFrame = item.LastSynchedFrame;
					this.SetSynchState(SynchedMissionObject.SynchState.SynchronizeFrameOverTime);
					this._duration = item.Duration;
					this._timer = 0f;
					if (this._duration.ApproximatelyEqualsTo(0f, 1E-05f))
					{
						this._duration = 0.1f;
					}
				}
			}
			if (item.HasSkeleton && item.SynchAnimation)
			{
				base.GameEntity.Skeleton.SetAnimationAtChannel(item.AnimationIndex, 0, item.AnimationSpeed, 0f, 0f);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, item.AnimationParameter);
				if (item.IsSkeletonAnimationPaused)
				{
					base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, base.GameEntity.GetGlobalFrame(), true);
					base.GameEntity.PauseSkeletonAnimation();
				}
				else
				{
					base.GameEntity.ResumeSkeletonAnimation();
				}
			}
			if (item.SynchColors)
			{
				this.SetTeamColors(item.Color, item.Color2);
			}
			if (item.IsDisabled)
			{
				base.SetDisabledAndMakeInvisible(false, false);
			}
		}

		// Token: 0x040014F7 RID: 5367
		private SynchedMissionObject.SynchFlags _initialSynchFlags;

		// Token: 0x040014F8 RID: 5368
		private SynchedMissionObject.SynchState _synchState;

		// Token: 0x040014F9 RID: 5369
		private MatrixFrame _lastSynchedFrame;

		// Token: 0x040014FA RID: 5370
		private MatrixFrame _firstFrame;

		// Token: 0x040014FB RID: 5371
		private float _timer;

		// Token: 0x040014FC RID: 5372
		private float _duration;

		// Token: 0x0200063F RID: 1599
		private enum SynchState
		{
			// Token: 0x0400210F RID: 8463
			SynchronizeCompleted,
			// Token: 0x04002110 RID: 8464
			SynchronizePosition,
			// Token: 0x04002111 RID: 8465
			SynchronizeFrame,
			// Token: 0x04002112 RID: 8466
			SynchronizeFrameOverTime
		}

		// Token: 0x02000640 RID: 1600
		[Flags]
		public enum SynchFlags : uint
		{
			// Token: 0x04002114 RID: 8468
			SynchNone = 0U,
			// Token: 0x04002115 RID: 8469
			SynchTransform = 1U,
			// Token: 0x04002116 RID: 8470
			SynchAnimation = 2U,
			// Token: 0x04002117 RID: 8471
			SynchBodyFlags = 4U,
			// Token: 0x04002118 RID: 8472
			SyncColors = 8U,
			// Token: 0x04002119 RID: 8473
			SynchAll = 4294967295U
		}
	}
}
