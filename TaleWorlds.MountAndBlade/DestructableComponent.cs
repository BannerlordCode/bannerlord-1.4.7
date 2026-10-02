using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000343 RID: 835
	public class DestructableComponent : SynchedMissionObject, IFocusable
	{
		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06002F16 RID: 12054 RVA: 0x000B7910 File Offset: 0x000B5B10
		// (remove) Token: 0x06002F17 RID: 12055 RVA: 0x000B7948 File Offset: 0x000B5B48
		public event Action OnNextDestructionState;

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06002F18 RID: 12056 RVA: 0x000B7980 File Offset: 0x000B5B80
		// (remove) Token: 0x06002F19 RID: 12057 RVA: 0x000B79B8 File Offset: 0x000B5BB8
		public event DestructableComponent.OnHitTakenAndDestroyedDelegate OnDestroyed;

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06002F1A RID: 12058 RVA: 0x000B79F0 File Offset: 0x000B5BF0
		// (remove) Token: 0x06002F1B RID: 12059 RVA: 0x000B7A28 File Offset: 0x000B5C28
		public event DestructableComponent.OnHitTakenAndDestroyedDelegate OnHitTaken;

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002F1C RID: 12060 RVA: 0x000B7A5D File Offset: 0x000B5C5D
		// (set) Token: 0x06002F1D RID: 12061 RVA: 0x000B7A68 File Offset: 0x000B5C68
		public float HitPoint
		{
			get
			{
				return this._hitPoint;
			}
			set
			{
				if (!this._hitPoint.Equals(value))
				{
					this._hitPoint = MathF.Max(value, 0f);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SyncObjectHitpoints(base.Id, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
				}
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002F1E RID: 12062 RVA: 0x000B7AB9 File Offset: 0x000B5CB9
		public FocusableObjectType FocusableObjectType
		{
			get
			{
				return FocusableObjectType.None;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002F1F RID: 12063 RVA: 0x000B7ABC File Offset: 0x000B5CBC
		public virtual bool IsFocusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000B7ABF File Offset: 0x000B5CBF
		public bool IsDestroyed
		{
			get
			{
				return this.HitPoint <= 0f;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002F21 RID: 12065 RVA: 0x000B7AD1 File Offset: 0x000B5CD1
		// (set) Token: 0x06002F22 RID: 12066 RVA: 0x000B7AD9 File Offset: 0x000B5CD9
		public GameEntity CurrentState { get; private set; }

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002F23 RID: 12067 RVA: 0x000B7AE2 File Offset: 0x000B5CE2
		private bool HasDestructionState
		{
			get
			{
				return this._destructionStates != null && !this._destructionStates.IsEmpty<string>();
			}
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000B7AFC File Offset: 0x000B5CFC
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this._referenceEntity = null;
			this._previousState = null;
			this._originalState = null;
			this.CurrentState = null;
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000B7B24 File Offset: 0x000B5D24
		protected DestructableComponent()
		{
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000B7B7C File Offset: 0x000B5D7C
		protected internal override void OnInit()
		{
			base.OnInit();
			this._hitPoint = this.MaxHitPoint;
			this._referenceEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag));
			if (!string.IsNullOrEmpty(this.DestructionStates))
			{
				this._destructionStates = this.DestructionStates.Replace(" ", string.Empty).Split(new char[] { ',' });
				bool flag = false;
				string[] destructionStates = this._destructionStates;
				for (int i = 0; i < destructionStates.Length; i++)
				{
					string item = destructionStates[i];
					if (!string.IsNullOrEmpty(item))
					{
						WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == item);
						if (weakGameEntity.IsValid)
						{
							weakGameEntity.AddBodyFlags(BodyFlags.Moveable, true);
							PhysicsShape bodyShape = weakGameEntity.GetBodyShape();
							if (bodyShape != null)
							{
								PhysicsShape.AddPreloadQueueWithName(bodyShape.GetName(), weakGameEntity.GetGlobalScale());
								flag = true;
							}
						}
						else
						{
							GameEntity gameEntity = TaleWorlds.Engine.GameEntity.Instantiate(null, item, false, true, "");
							List<GameEntity> list = new List<GameEntity>();
							gameEntity.GetChildrenRecursive(ref list);
							list.Add(gameEntity);
							foreach (GameEntity gameEntity2 in list)
							{
								PhysicsShape bodyShape2 = gameEntity2.GetBodyShape();
								if (bodyShape2 != null)
								{
									Vec3 globalScale = gameEntity2.GetGlobalScale();
									Vec3 globalScale2 = this._referenceEntity.GetGlobalScale();
									Vec3 vec = new Vec3(globalScale.x * globalScale2.x, globalScale.y * globalScale2.y, globalScale.z * globalScale2.z, -1f);
									PhysicsShape.AddPreloadQueueWithName(bodyShape2.GetName(), vec);
									flag = true;
								}
							}
						}
					}
				}
				if (flag)
				{
					PhysicsShape.ProcessPreloadQueue();
				}
			}
			WeakGameEntity originalState = this.GetOriginalState(base.GameEntity);
			this._originalState = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(originalState.IsValid ? originalState : base.GameEntity);
			this.CurrentState = this._originalState;
			this._originalState.AddBodyFlags(BodyFlags.Moveable, true);
			List<WeakGameEntity> list2 = new List<WeakGameEntity>();
			base.GameEntity.GetChildrenRecursive(ref list2);
			foreach (WeakGameEntity weakGameEntity2 in list2.Where<WeakGameEntity>((WeakGameEntity child) => child.BodyFlag.HasAnyFlag(BodyFlags.Dynamic)))
			{
				weakGameEntity2.SetPhysicsState(false, true);
				weakGameEntity2.SetFrameChanged();
			}
			this._heavyHitParticles = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity).CollectChildrenEntitiesWithTag(this.HeavyHitParticlesTag);
			base.GameEntity.SetAnimationSoundActivation(true);
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000B7E94 File Offset: 0x000B6094
		public WeakGameEntity GetOriginalState(WeakGameEntity parent)
		{
			int childCount = parent.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				WeakGameEntity child = parent.GetChild(i);
				if (!child.HasScriptOfType<DestructableComponent>())
				{
					if (child.HasTag(this.OriginalStateTag))
					{
						return child;
					}
					WeakGameEntity originalState = this.GetOriginalState(child);
					if (originalState.IsValid)
					{
						return originalState;
					}
				}
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000B7EF0 File Offset: 0x000B60F0
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._referenceEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag));
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000B7F38 File Offset: 0x000B6138
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName.Equals(this.ReferenceEntityTag))
			{
				this._referenceEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag));
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000B7F8E File Offset: 0x000B618E
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.Reset();
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000B7F9C File Offset: 0x000B619C
		public void Reset()
		{
			this.RestoreEntity();
			this._hitPoint = this.MaxHitPoint;
			this._currentStateIndex = 0;
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000B7FB8 File Offset: 0x000B61B8
		private void RestoreEntity()
		{
			if (this._destructionStates != null)
			{
				int j;
				int i;
				for (i = 0; i < this._destructionStates.Length; i = j + 1)
				{
					WeakGameEntity weakGameEntity = base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == this._destructionStates[i].ToString());
					if (weakGameEntity.IsValid)
					{
						Skeleton skeleton = weakGameEntity.Skeleton;
						if (skeleton != null)
						{
							skeleton.SetAnimationAtChannel(-1, 0, 1f, -1f, 0f);
						}
					}
					j = i;
				}
			}
			if (this.CurrentState != this._originalState)
			{
				this.CurrentState.SetVisibilityExcludeParents(false);
				this.CurrentState.SetPhysicsState(false, true);
				this.CurrentState = this._originalState;
			}
			this.CurrentState.SetVisibilityExcludeParents(true);
			this.CurrentState.SetPhysicsState(true, true);
			this.CurrentState.SetFrameChanged();
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000B80B0 File Offset: 0x000B62B0
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (this._referenceEntity != null && this._referenceEntity != base.GameEntity && MBEditor.IsEntitySelected(this._referenceEntity))
			{
				new Vec3(-2f, -0.5f, -1f, -1f);
				new Vec3(2f, 0.5f, 1f, -1f);
				MatrixFrame identity = MatrixFrame.Identity;
				this._referenceEntity.Root.GetMeshBendedFrame(this._referenceEntity.GetGlobalFrame(), ref identity);
			}
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000B814C File Offset: 0x000B634C
		public void TriggerOnHit(Agent attackerAgent, int inflictedDamage, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ScriptComponentBehavior attackerScriptComponentBehavior)
		{
			bool flag;
			float num;
			float num2;
			float num3;
			this.OnHit(attackerAgent, inflictedDamage, impactPosition, impactDirection, in weapon, affectorWeaponSlotOrMissileIndex, attackerScriptComponentBehavior, out flag, out num, out num2, out num3);
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000B8174 File Offset: 0x000B6374
		protected internal override bool OnHit(Agent attackerAgent, int inflictedDamage, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ScriptComponentBehavior attackerScriptComponentBehavior, out bool reportDamage, out float modifiedDamage, out float fireDamage, out float modifiedFireDamage)
		{
			reportDamage = false;
			modifiedDamage = (float)inflictedDamage;
			fireDamage = -1f;
			modifiedFireDamage = -1f;
			if (base.IsDisabled)
			{
				return true;
			}
			MissionWeapon missionWeapon = weapon;
			if (missionWeapon.IsEmpty && !(attackerScriptComponentBehavior is BatteringRam))
			{
				inflictedDamage = 0;
			}
			else if (this.DestroyedByStoneOnly)
			{
				missionWeapon = weapon;
				WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
				if ((currentUsageItem.WeaponClass != WeaponClass.Sling && currentUsageItem.WeaponClass != WeaponClass.Stone && currentUsageItem.WeaponClass != WeaponClass.Boulder && currentUsageItem.WeaponClass != WeaponClass.BallistaBoulder && currentUsageItem.WeaponClass != WeaponClass.BallistaStone) || !currentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.NotUsableWithOneHand))
				{
					inflictedDamage = 0;
				}
			}
			bool isDestroyed = this.IsDestroyed;
			if (this.DestroyOnAnyHit)
			{
				inflictedDamage = (int)(this.MaxHitPoint + 1f);
			}
			if (inflictedDamage > 0 && !isDestroyed)
			{
				this.HitPoint -= (float)inflictedDamage;
				if ((float)inflictedDamage > this.HeavyHitParticlesThreshold)
				{
					this.BurstHeavyHitParticles();
				}
				int num = this.CalculateNextDestructionLevel(inflictedDamage);
				if (!this.IsDestroyed)
				{
					DestructableComponent.OnHitTakenAndDestroyedDelegate onHitTaken = this.OnHitTaken;
					if (onHitTaken != null)
					{
						onHitTaken(this, attackerAgent, in weapon, attackerScriptComponentBehavior, inflictedDamage);
					}
				}
				else if (this.IsDestroyed && !isDestroyed)
				{
					Mission.Current.OnObjectDisabled(this);
					DestructableComponent.OnHitTakenAndDestroyedDelegate onHitTaken2 = this.OnHitTaken;
					if (onHitTaken2 != null)
					{
						onHitTaken2(this, attackerAgent, in weapon, attackerScriptComponentBehavior, inflictedDamage);
					}
					DestructableComponent.OnHitTakenAndDestroyedDelegate onDestroyed = this.OnDestroyed;
					if (onDestroyed != null)
					{
						onDestroyed(this, attackerAgent, in weapon, attackerScriptComponentBehavior, inflictedDamage);
					}
					MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
					globalFrame.origin += globalFrame.rotation.u * this.SoundAndParticleEffectHeightOffset + globalFrame.rotation.f * this.SoundAndParticleEffectForwardOffset;
					globalFrame.rotation.Orthonormalize();
					if (this.ParticleEffectOnDestroy != "")
					{
						Mission.Current.Scene.CreateBurstParticle(ParticleSystemManager.GetRuntimeIdByName(this.ParticleEffectOnDestroy), globalFrame);
					}
					if (this.SoundEffectOnDestroy != "")
					{
						Mission.Current.MakeSound(SoundEvent.GetEventIdFromString(this.SoundEffectOnDestroy), globalFrame.origin, false, true, (attackerAgent != null) ? attackerAgent.Index : (-1), -1);
					}
				}
				this.SetDestructionLevel(num, -1, (float)inflictedDamage, impactPosition, impactDirection, false);
				reportDamage = true;
			}
			return !this.PassHitOnToParent;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000B83D4 File Offset: 0x000B65D4
		public void BurstHeavyHitParticles()
		{
			foreach (GameEntity gameEntity in this._heavyHitParticles)
			{
				gameEntity.BurstEntityParticle(false);
			}
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new BurstAllHeavyHitParticles(base.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000B844C File Offset: 0x000B664C
		private int CalculateNextDestructionLevel(int inflictedDamage)
		{
			if (this.HasDestructionState)
			{
				int num = this._destructionStates.Length;
				float num2 = this.MaxHitPoint / (float)num;
				float num3 = this.MaxHitPoint;
				int num4 = 0;
				while (num3 - num2 >= this.HitPoint)
				{
					num3 -= num2;
					num4++;
				}
				Func<int, int, int, int> onCalculateDestructionStateIndex = this.OnCalculateDestructionStateIndex;
				return (onCalculateDestructionStateIndex != null) ? onCalculateDestructionStateIndex(num4, inflictedDamage, this.DestructionStates.Length) : num4;
			}
			if (this.IsDestroyed)
			{
				return this._currentStateIndex + 1;
			}
			return this._currentStateIndex;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000B84CC File Offset: 0x000B66CC
		public void SetDestructionLevel(int state, int forcedId, float blowMagnitude, Vec3 blowPosition, Vec3 blowDirection, bool noEffects = false)
		{
			if (this._currentStateIndex != state)
			{
				float num = MBMath.ClampFloat(blowMagnitude, 1f, DestructableComponent.MaxBlowMagnitude);
				this._currentStateIndex = state;
				this.ReplaceEntityWithBrokenEntity(forcedId);
				if (this.CurrentState != null)
				{
					List<GameEntity> list = new List<GameEntity>();
					if (this.CurrentState.Parent != null)
					{
						list.Add(this.CurrentState);
					}
					this.CurrentState.GetChildrenRecursive(ref list);
					foreach (GameEntity gameEntity in list)
					{
						if (gameEntity.BodyFlag.HasAnyFlag(BodyFlags.Dynamic))
						{
							gameEntity.Parent.RemoveChild(gameEntity, true, true, false, 178);
							gameEntity.SetPhysicsState(true, true);
							gameEntity.SetFrameChanged();
						}
					}
					if (!GameNetwork.IsDedicatedServer && !noEffects)
					{
						this.CurrentState.BurstEntityParticle(true);
						this.ApplyPhysics(num, blowPosition, blowDirection);
					}
					Action onNextDestructionState = this.OnNextDestructionState;
					if (onNextDestructionState != null)
					{
						onNextDestructionState();
					}
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					if (this.CurrentState != null)
					{
						MissionObject firstScriptOfType = this.CurrentState.GetFirstScriptOfType<MissionObject>();
						if (firstScriptOfType != null)
						{
							forcedId = firstScriptOfType.Id.Id;
						}
					}
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new SyncObjectDestructionLevel(base.Id, state, forcedId, num, blowPosition, blowDirection));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000B863C File Offset: 0x000B683C
		private void ApplyPhysics(float blowMagnitude, Vec3 blowPosition, Vec3 blowDirection)
		{
			if (this.CurrentState != null)
			{
				IEnumerable<GameEntity> enumerable = from child in this.CurrentState.GetChildren()
					where child.HasBody() && child.BodyFlag.HasAnyFlag(BodyFlags.Dynamic) && !child.HasScriptOfType<SpawnedItemEntity>()
					select child;
				int num = enumerable.Count<GameEntity>();
				float num2 = ((num > 1) ? (blowMagnitude / (float)num) : blowMagnitude);
				foreach (GameEntity gameEntity in enumerable)
				{
					gameEntity.ApplyLocalImpulseToDynamicBody(Vec3.Zero, blowDirection * num2);
					Mission.Current.AddTimerToDynamicEntity(gameEntity, 10f + MBRandom.RandomFloat * 2f);
				}
			}
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000B8700 File Offset: 0x000B6900
		private void ReplaceEntityWithBrokenEntity(int forcedId)
		{
			this._previousState = this.CurrentState;
			this._previousState.SetVisibilityExcludeParents(false);
			this._previousState.SetPhysicsState(false, true);
			if (this.HasDestructionState)
			{
				bool flag;
				this.CurrentState = this.AddBrokenEntity(this._destructionStates[this._currentStateIndex - 1], out flag);
				if (flag)
				{
					if (this._originalState != base.GameEntity)
					{
						base.GameEntity.AddChild(this.CurrentState.WeakEntity, true);
					}
					if (forcedId != -1)
					{
						MissionObject firstScriptOfType = this.CurrentState.GetFirstScriptOfType<MissionObject>();
						if (firstScriptOfType != null)
						{
							firstScriptOfType.Id = new MissionObjectId(forcedId, true);
							using (IEnumerator<GameEntity> enumerator = this.CurrentState.GetChildren().GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									GameEntity gameEntity = enumerator.Current;
									MissionObject firstScriptOfType2 = gameEntity.GetFirstScriptOfType<MissionObject>();
									if (firstScriptOfType2 != null && firstScriptOfType2.Id.CreatedAtRuntime)
									{
										firstScriptOfType2.Id = new MissionObjectId(++forcedId, true);
									}
								}
								return;
							}
						}
						MBDebug.ShowWarning("Current destruction state doesn't have mission object script component.");
					}
				}
			}
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000B8820 File Offset: 0x000B6A20
		protected internal override bool MovesEntity()
		{
			return true;
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000B8823 File Offset: 0x000B6A23
		public void PreDestroy()
		{
			DestructableComponent.OnHitTakenAndDestroyedDelegate onDestroyed = this.OnDestroyed;
			if (onDestroyed != null)
			{
				onDestroyed(this, null, in MissionWeapon.Invalid, null, 0);
			}
			this.SetVisibleSynched(false, true);
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000B8848 File Offset: 0x000B6A48
		private GameEntity AddBrokenEntity(string prefab, out bool newCreated)
		{
			if (!string.IsNullOrEmpty(prefab))
			{
				int childCount = base.GameEntity.ChildCount;
				int num = 0;
				WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
				for (int i = 0; i < childCount; i++)
				{
					WeakGameEntity child = base.GameEntity.GetChild(i);
					if (child.Name == prefab)
					{
						num++;
						if (MBRandom.RandomInt(num) == 0)
						{
							weakGameEntity = child;
						}
					}
				}
				GameEntity gameEntity;
				if (weakGameEntity.IsValid)
				{
					gameEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
					weakGameEntity.SetVisibilityExcludeParents(true);
					weakGameEntity.SetPhysicsState(true, true);
					if (!GameNetwork.IsClientOrReplay)
					{
						MissionObject firstScriptOfType = weakGameEntity.GetFirstScriptOfType<MissionObject>();
						if (firstScriptOfType != null)
						{
							firstScriptOfType.SetAbilityOfFaces(true);
						}
					}
					newCreated = false;
				}
				else
				{
					gameEntity = TaleWorlds.Engine.GameEntity.Instantiate(Mission.Current.Scene, prefab, this._referenceEntity.GetGlobalFrame(), true);
					if (gameEntity != null)
					{
						gameEntity.SetMobility(TaleWorlds.Engine.GameEntity.Mobility.Stationary);
					}
					if (base.GameEntity.Parent.IsValid)
					{
						base.GameEntity.Parent.AddChild(gameEntity.WeakEntity, true);
					}
					newCreated = true;
				}
				if (this._referenceEntity.Skeleton != null && gameEntity.Skeleton != null)
				{
					Skeleton skeleton = ((this.CurrentState != this._originalState) ? this.CurrentState : this._referenceEntity).Skeleton;
					int animationIndexAtChannel = skeleton.GetAnimationIndexAtChannel(0);
					float animationParameterAtChannel = skeleton.GetAnimationParameterAtChannel(0);
					if (animationIndexAtChannel != -1)
					{
						gameEntity.Skeleton.SetAnimationAtChannel(animationIndexAtChannel, 0, 1f, -1f, animationParameterAtChannel);
						gameEntity.ResumeSkeletonAnimation();
					}
				}
				WeakGameEntity weakGameEntity2 = base.GameEntity;
				while (weakGameEntity2 != null)
				{
					ColorAssigner firstScriptOfType2 = weakGameEntity2.GetFirstScriptOfType<ColorAssigner>();
					if (firstScriptOfType2 != null)
					{
						firstScriptOfType2.SetColor(gameEntity.WeakEntity);
						break;
					}
					weakGameEntity2 = weakGameEntity2.Parent;
				}
				return gameEntity;
			}
			newCreated = false;
			return null;
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x000B8A20 File Offset: 0x000B6C20
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteFloatToPacket(MathF.Max(this.HitPoint, 0f), CompressionMission.UsableGameObjectHealthCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this._currentStateIndex, CompressionMission.UsableGameObjectDestructionStateCompressionInfo);
			if (this._currentStateIndex != 0)
			{
				MissionObject firstScriptOfType = this.CurrentState.GetFirstScriptOfType<MissionObject>();
				GameNetworkMessage.WriteBoolToPacket(firstScriptOfType != null);
				if (firstScriptOfType != null)
				{
					GameNetworkMessage.WriteMissionObjectIdToPacket(firstScriptOfType.Id);
				}
			}
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000B8A88 File Offset: 0x000B6C88
		public override void AddStuckMissile(GameEntity missileEntity)
		{
			if (this.CurrentState != null)
			{
				this.CurrentState.AddChild(missileEntity, false);
				return;
			}
			base.GameEntity.AddChild(missileEntity.WeakEntity, false);
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000B8AC8 File Offset: 0x000B6CC8
		protected internal override bool OnCheckForProblems()
		{
			bool flag = base.OnCheckForProblems();
			if (!(string.IsNullOrEmpty(this.ReferenceEntityTag) ? base.GameEntity : base.GameEntity.GetFirstChildEntityWithTag(this.ReferenceEntityTag)).IsValid)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "Reference entity must be assigned. Root entity is " + base.GameEntity.Root.Name + ", child is " + base.GameEntity.Name);
				flag = true;
			}
			string[] array = this.DestructionStates.Replace(" ", string.Empty).Split(new char[] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string destructionState = array[i];
				if (!string.IsNullOrEmpty(destructionState) && !base.GameEntity.GetChildren().FirstOrDefault<WeakGameEntity>((WeakGameEntity x) => x.Name == destructionState).IsValid && TaleWorlds.Engine.GameEntity.Instantiate(null, destructionState, false, true, "") == null)
				{
					MBEditor.AddEntityWarning(base.GameEntity, "Destruction state '" + destructionState + "' is not valid.");
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000B8C17 File Offset: 0x000B6E17
		public void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000B8C19 File Offset: 0x000B6E19
		public void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000B8C1B File Offset: 0x000B6E1B
		public TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			return null;
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000B8C20 File Offset: 0x000B6E20
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			DestructableComponent.DestructableComponentRecord destructableComponentRecord = (DestructableComponent.DestructableComponentRecord)synchedMissionObjectReadableRecord.Item2;
			this.HitPoint = destructableComponentRecord.HitPoint;
			if (destructableComponentRecord.DestructionState != 0)
			{
				if (this.IsDestroyed)
				{
					DestructableComponent.OnHitTakenAndDestroyedDelegate onDestroyed = this.OnDestroyed;
					if (onDestroyed != null)
					{
						onDestroyed(this, null, in MissionWeapon.Invalid, null, 0);
					}
				}
				this.SetDestructionLevel(destructableComponentRecord.DestructionState, destructableComponentRecord.ForceIndex, 0f, Vec3.Zero, Vec3.Zero, true);
			}
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000B8CA0 File Offset: 0x000B6EA0
		public TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			int num;
			TextObject textObject;
			if (int.TryParse(gameEntity.Name.Split(new char[] { '_' }).Last<string>(), out num))
			{
				string text = gameEntity.Name;
				text = text.Remove(text.Length - num.ToString().Length);
				text += "x";
				if (GameTexts.TryGetText("str_destructible_component", out textObject, text))
				{
					return textObject;
				}
			}
			if (GameTexts.TryGetText("str_destructible_component", out textObject, gameEntity.Name))
			{
				return textObject;
			}
			return null;
		}

		// Token: 0x0400130A RID: 4874
		public const string CleanStateTag = "operational";

		// Token: 0x0400130B RID: 4875
		public static float MaxBlowMagnitude = 20f;

		// Token: 0x0400130C RID: 4876
		public string DestructionStates;

		// Token: 0x0400130D RID: 4877
		public bool DestroyedByStoneOnly;

		// Token: 0x0400130E RID: 4878
		public bool CanBeDestroyedInitially = true;

		// Token: 0x0400130F RID: 4879
		public float MaxHitPoint = 100f;

		// Token: 0x04001310 RID: 4880
		public bool DestroyOnAnyHit;

		// Token: 0x04001311 RID: 4881
		public bool PassHitOnToParent;

		// Token: 0x04001312 RID: 4882
		public string ReferenceEntityTag;

		// Token: 0x04001313 RID: 4883
		public string HeavyHitParticlesTag;

		// Token: 0x04001314 RID: 4884
		public float HeavyHitParticlesThreshold = 5f;

		// Token: 0x04001315 RID: 4885
		public string ParticleEffectOnDestroy = "";

		// Token: 0x04001316 RID: 4886
		public string SoundEffectOnDestroy = "";

		// Token: 0x04001317 RID: 4887
		public float SoundAndParticleEffectHeightOffset;

		// Token: 0x04001318 RID: 4888
		public float SoundAndParticleEffectForwardOffset;

		// Token: 0x0400131C RID: 4892
		public BattleSideEnum BattleSide = BattleSideEnum.None;

		// Token: 0x0400131D RID: 4893
		[EditableScriptComponentVariable(false, "")]
		public Func<int, int, int, int> OnCalculateDestructionStateIndex;

		// Token: 0x0400131E RID: 4894
		private float _hitPoint;

		// Token: 0x0400131F RID: 4895
		private string OriginalStateTag = "operational";

		// Token: 0x04001320 RID: 4896
		private GameEntity _referenceEntity;

		// Token: 0x04001321 RID: 4897
		private GameEntity _previousState;

		// Token: 0x04001322 RID: 4898
		private GameEntity _originalState;

		// Token: 0x04001324 RID: 4900
		private string[] _destructionStates;

		// Token: 0x04001325 RID: 4901
		private int _currentStateIndex;

		// Token: 0x04001326 RID: 4902
		private List<GameEntity> _heavyHitParticles;

		// Token: 0x0200061A RID: 1562
		[DefineSynchedMissionObjectType(typeof(DestructableComponent))]
		public struct DestructableComponentRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AAA RID: 2730
			// (get) Token: 0x06003FB4 RID: 16308 RVA: 0x000F7B42 File Offset: 0x000F5D42
			// (set) Token: 0x06003FB5 RID: 16309 RVA: 0x000F7B4A File Offset: 0x000F5D4A
			public float HitPoint { get; private set; }

			// Token: 0x17000AAB RID: 2731
			// (get) Token: 0x06003FB6 RID: 16310 RVA: 0x000F7B53 File Offset: 0x000F5D53
			// (set) Token: 0x06003FB7 RID: 16311 RVA: 0x000F7B5B File Offset: 0x000F5D5B
			public int DestructionState { get; private set; }

			// Token: 0x17000AAC RID: 2732
			// (get) Token: 0x06003FB8 RID: 16312 RVA: 0x000F7B64 File Offset: 0x000F5D64
			// (set) Token: 0x06003FB9 RID: 16313 RVA: 0x000F7B6C File Offset: 0x000F5D6C
			public int ForceIndex { get; private set; }

			// Token: 0x17000AAD RID: 2733
			// (get) Token: 0x06003FBA RID: 16314 RVA: 0x000F7B75 File Offset: 0x000F5D75
			// (set) Token: 0x06003FBB RID: 16315 RVA: 0x000F7B7D File Offset: 0x000F5D7D
			public bool IsMissionObject { get; private set; }

			// Token: 0x06003FBC RID: 16316 RVA: 0x000F7B86 File Offset: 0x000F5D86
			public DestructableComponentRecord(float hitPoint, int destructionState, int forceIndex, bool isMissionObject)
			{
				this.HitPoint = hitPoint;
				this.DestructionState = destructionState;
				this.ForceIndex = forceIndex;
				this.IsMissionObject = isMissionObject;
			}

			// Token: 0x06003FBD RID: 16317 RVA: 0x000F7BA8 File Offset: 0x000F5DA8
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.HitPoint = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.UsableGameObjectHealthCompressionInfo, ref bufferReadValid);
				this.DestructionState = GameNetworkMessage.ReadIntFromPacket(CompressionMission.UsableGameObjectDestructionStateCompressionInfo, ref bufferReadValid);
				this.ForceIndex = -1;
				if (this.DestructionState != 0)
				{
					this.IsMissionObject = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
					if (this.IsMissionObject)
					{
						this.ForceIndex = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref bufferReadValid).Id;
					}
				}
				return bufferReadValid;
			}
		}

		// Token: 0x0200061B RID: 1563
		// (Invoke) Token: 0x06003FBF RID: 16319
		public delegate void OnHitTakenAndDestroyedDelegate(DestructableComponent target, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage);
	}
}
