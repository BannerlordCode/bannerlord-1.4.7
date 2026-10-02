using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000379 RID: 889
	public class SpawnedItemEntity : UsableMissionObject
	{
		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06003268 RID: 12904 RVA: 0x000CD4DE File Offset: 0x000CB6DE
		public MissionWeapon WeaponCopy
		{
			get
			{
				return this._weapon;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x000CD4E6 File Offset: 0x000CB6E6
		// (set) Token: 0x0600326A RID: 12906 RVA: 0x000CD4EE File Offset: 0x000CB6EE
		public bool HasLifeTime
		{
			get
			{
				return this._hasLifeTime;
			}
			set
			{
				if (this._hasLifeTime != value)
				{
					this._hasLifeTime = value;
					base.SetScriptComponentToTickMT(this.GetTickRequirement());
				}
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600326B RID: 12907 RVA: 0x000CD50C File Offset: 0x000CB70C
		// (set) Token: 0x0600326C RID: 12908 RVA: 0x000CD514 File Offset: 0x000CB714
		private bool PhysicsStopped
		{
			get
			{
				return this._physicsStopped;
			}
			set
			{
				if (this._physicsStopped != value)
				{
					this._physicsStopped = value;
					base.SetScriptComponentToTickMT(this.GetTickRequirement());
				}
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600326D RID: 12909 RVA: 0x000CD532 File Offset: 0x000CB732
		public bool IsRemoved
		{
			get
			{
				return this._ownerGameEntity == null;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600326F RID: 12911 RVA: 0x000CD549 File Offset: 0x000CB749
		// (set) Token: 0x0600326E RID: 12910 RVA: 0x000CD540 File Offset: 0x000CB740
		public bool SpawnedOnACorpse { get; private set; }

		// Token: 0x06003270 RID: 12912 RVA: 0x000CD551 File Offset: 0x000CB751
		public TextObject GetActionMessage(ItemObject weaponToReplaceWith, bool fillUp)
		{
			if (weaponToReplaceWith != null)
			{
				MBTextManager.SetTextVariable("ITEM_NAME", weaponToReplaceWith.Name, false);
				return GameTexts.FindText("str_ui_swap", null);
			}
			if (!fillUp)
			{
				return GameTexts.FindText("str_ui_equip", null);
			}
			return GameTexts.FindText("str_ui_fill", null);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000CD590 File Offset: 0x000CB790
		public TextObject GetDescriptionMessage(bool fillUp)
		{
			if (!fillUp)
			{
				return this._weapon.GetModifiedItemName();
			}
			return GameTexts.FindText("str_inventory_weapon", this._weapon.CurrentUsageItem.WeaponClass.ToString());
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x000CD5D4 File Offset: 0x000CB7D4
		public override bool LockUserFrames
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06003273 RID: 12915 RVA: 0x000CD5D7 File Offset: 0x000CB7D7
		// (set) Token: 0x06003274 RID: 12916 RVA: 0x000CD5DF File Offset: 0x000CB7DF
		public Mission.WeaponSpawnFlags SpawnFlags { get; private set; }

		// Token: 0x06003275 RID: 12917 RVA: 0x000CD5E8 File Offset: 0x000CB7E8
		public void Initialize(MissionWeapon weapon, bool hasLifeTime, Mission.WeaponSpawnFlags spawnFlags, in Vec3 fakeSimulationVelocity, bool spawnedOnACorpse = false)
		{
			this._weapon = weapon;
			this.HasLifeTime = hasLifeTime;
			this.SpawnFlags = spawnFlags;
			this._fakeSimulationVelocity = fakeSimulationVelocity;
			this.SpawnedOnACorpse = spawnedOnACorpse;
			if (this.HasLifeTime)
			{
				float num = 0f;
				if (!this._weapon.IsEmpty)
				{
					num = (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.QuickFadeOut) ? 5f : 180f);
					base.IsDeactivated = this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.CannotBePickedUp);
					if (this._weapon.CurrentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.RangedWeapon | WeaponFlags.NotUsableWithOneHand | WeaponFlags.Consumable))
					{
						this._lastSoundPlayTime = 0.333f;
					}
					else
					{
						this._lastSoundPlayTime = -0.333f;
					}
				}
				else
				{
					base.IsDeactivated = true;
				}
				this._deletionTimer = new Timer(Mission.Current.CurrentTime, num, true);
			}
			else
			{
				this._deletionTimer = new Timer(Mission.Current.CurrentTime, float.MaxValue, true);
			}
			if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics))
			{
				this._disablePhysicsTimer = new Timer(Mission.Current.CurrentTime, 10f, true);
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000CD720 File Offset: 0x000CB920
		protected internal override void OnInit()
		{
			base.OnInit();
			this._ownerGameEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity);
			if (!string.IsNullOrEmpty(this.WeaponName))
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(this.WeaponName);
				this._weapon = new MissionWeapon(@object, null, null);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x000CD784 File Offset: 0x000CB984
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (GameNetwork.IsClientOrReplay || base.HasUser || !this.PhysicsStopped)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2;
			}
			if (this.HasLifeTime)
			{
				ScriptComponentBehavior.TickRequirement tickRequirement = base.GetTickRequirement();
				if (tickRequirement.HasAnyFlag(ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2))
				{
					tickRequirement |= ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel2;
				}
				else
				{
					tickRequirement |= ScriptComponentBehavior.TickRequirement.TickOccasionally;
				}
				return tickRequirement;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000CD7E0 File Offset: 0x000CB9E0
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._disableDynamicPhysicsNextFrame)
			{
				this.DisableDynamicBody();
				this._disableDynamicPhysicsNextFrame = false;
				return;
			}
			if (GameNetwork.IsClientOrReplay && this._clientSyncData != null)
			{
				if (this._clientSyncData.Timer.Check(Mission.Current.CurrentTime))
				{
					this._ownerGameEntity.SetAlpha(1f);
					this._clientSyncData = null;
					return;
				}
				float duration = this._clientSyncData.Timer.Duration;
				float num = MBMath.ClampFloat(this._clientSyncData.Timer.ElapsedTime() / duration, 0f, 1f);
				if (num < (1f - 0.1f / duration) * 0.5f)
				{
					this._ownerGameEntity.SetAlpha(1f - num * 2f);
					return;
				}
				if (num < (1f + 0.1f / duration) * 0.5f)
				{
					this._ownerGameEntity.SetAlpha(0f);
					this._ownerGameEntity.SetGlobalFrame(in this._clientSyncData.Frame, true);
					GameEntity parent = this._clientSyncData.Parent;
					if (parent != null)
					{
						parent.AddChild(this._ownerGameEntity, true);
					}
					this._clientSyncData.Timer.Reset(Mission.Current.CurrentTime - duration * (1f + 0.1f / duration) * 0.5f);
					return;
				}
				this._ownerGameEntity.SetAlpha(num * 2f - 1f);
			}
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000CD958 File Offset: 0x000CBB58
		protected internal override void OnTickParallel2(float dt)
		{
			base.OnTickParallel2(dt);
			if (!GameNetwork.IsClientOrReplay)
			{
				if (base.HasUser)
				{
					ActionIndexCache currentAction = base.UserAgent.GetCurrentAction(this._usedChannelIndex);
					if (currentAction == this._successActionIndex)
					{
						base.UserAgent.StopUsingGameObjectMT(base.UserAgent.CanUseObject(this) && !base.UserAgent.IsInWater(), Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					else if (currentAction != this._progressActionIndex)
					{
						base.UserAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
				}
				else if (this.HasLifeTime && this._deletionTimer.Check(Mission.Current.CurrentTime))
				{
					this._readyToBeDeleted = true;
				}
				if (!this.PhysicsStopped)
				{
					if (this._ownerGameEntity != null)
					{
						if (this._weapon.IsBanner())
						{
							MatrixFrame globalFrame = this._ownerGameEntity.GetGlobalFrame();
							this._fakeSimulationVelocity.z = this._fakeSimulationVelocity.z - dt * 9.8f;
							globalFrame.origin += this._fakeSimulationVelocity * dt;
							this._ownerGameEntity.SetGlobalFrame(in globalFrame, true);
							if (this._ownerGameEntity.Scene.GetGroundHeightAtPosition(globalFrame.origin, BodyFlags.CommonCollisionExcludeFlags) > globalFrame.origin.z + 0.3f)
							{
								this.PhysicsStopped = true;
								return;
							}
						}
						else
						{
							Vec3 globalPosition = this._ownerGameEntity.GlobalPosition;
							if (globalPosition.z <= CompressionBasic.PositionCompressionInfo.GetMinimumValue() + 5f)
							{
								this._readyToBeDeleted = true;
							}
							if (!this._ownerGameEntity.BodyFlag.HasAnyFlag(BodyFlags.Dynamic))
							{
								this.PhysicsStopped = true;
								return;
							}
							MatrixFrame globalFrame2 = this._ownerGameEntity.GetGlobalFrame();
							if (!globalFrame2.rotation.IsUnit())
							{
								globalFrame2.rotation.Orthonormalize();
								this._ownerGameEntity.SetGlobalFrame(in globalFrame2, true);
							}
							bool flag = this._disablePhysicsTimer.Check(Mission.Current.CurrentTime);
							if ((flag || this._disablePhysicsTimer.ElapsedTime() > 1f) && (flag || this._ownerGameEntity.IsDynamicBodyStationaryMT()))
							{
								this._groundEntityWhenDisabled = this.TryFindProperGroundEntityForSpawnedEntity();
								this._disableDynamicPhysicsNextFrame = true;
							}
							if (!this.PhysicsStopped && this._disablePhysicsTimer.ElapsedTime() > 0.2f)
							{
								Vec3 vec;
								Vec3 vec2;
								this._ownerGameEntity.GetPhysicsMinMax(true, out vec, out vec2, true);
								MatrixFrame globalFrame3 = this._ownerGameEntity.GetGlobalFrame();
								MatrixFrame previousGlobalFrame = this._ownerGameEntity.GetPreviousGlobalFrame();
								Vec3 vec3 = globalFrame3.TransformToParent(in vec);
								Vec3 vec4 = previousGlobalFrame.TransformToParent(in vec);
								Vec3 vec5 = globalFrame3.TransformToParent(in vec2);
								Vec3 vec6 = previousGlobalFrame.TransformToParent(in vec2);
								Vec3 vec7 = Vec3.Vec3Min(vec3, vec5);
								Vec3 vec8 = Vec3.Vec3Min(vec4, vec6);
								Vec3 vec9 = Vec3.Vec3Max(vec3, vec5);
								float waterLevelAtPositionMT = Mission.Current.GetWaterLevelAtPositionMT(vec7.AsVec2, !GameNetwork.IsMultiplayer);
								bool flag2 = vec7.z < waterLevelAtPositionMT;
								bool flag3 = vec8.z < waterLevelAtPositionMT;
								if (flag2)
								{
									this._disablePhysicsTimer.AdjustStartTime(dt * 0.8f);
									float num = waterLevelAtPositionMT - 3.5f;
									if (vec9.z < num)
									{
										this._readyToBeDeleted = true;
									}
									if (!flag3)
									{
										BodyFlags bodyFlags;
										base.GameEntity.Scene.GetGroundHeightAndBodyFlagsAtPosition(globalFrame3.origin, out bodyFlags, BodyFlags.CommonCollisionExcludeFlagsForCombat);
										if (!bodyFlags.HasAnyFlag(BodyFlags.Moveable))
										{
											Vec3 linearVelocityMT = this._ownerGameEntity.GetLinearVelocityMT();
											float num2 = this._ownerGameEntity.Mass * linearVelocityMT.Length;
											if (!this._alreadyMadeWaterDropSound && num2 > 0f)
											{
												num2 *= 0.0625f;
												num2 = MathF.Min(num2, 1f);
												Vec3 vec10 = globalPosition;
												vec10.z = waterLevelAtPositionMT;
												SoundEventParameter soundEventParameter = new SoundEventParameter("Size", num2);
												Mission.Current.MakeSound(ItemPhysicsSoundContainer.SoundCodePhysicsWater, vec10, false, true, -1, -1, ref soundEventParameter);
												this._alreadyMadeWaterDropSound = true;
											}
										}
									}
								}
								if (flag2 != flag3)
								{
									float num3 = (flag2 ? 100f : 1f);
									PhysicsMaterial physicsMaterial = base.GameEntity.GetPhysicsMaterial();
									float num4 = physicsMaterial.GetLinearDamping() * num3;
									float num5 = physicsMaterial.GetAngularDamping() * num3;
									if (num4 > 15f)
									{
										num4 = 15f;
									}
									if (num5 > 15f)
									{
										num5 = 15f;
									}
									base.GameEntity.SetDampingMT(num4, num5);
									return;
								}
							}
						}
					}
					else
					{
						this.PhysicsStopped = true;
					}
				}
			}
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x000CDDC8 File Offset: 0x000CBFC8
		private void DisableDynamicBody()
		{
			using (new TWSharedMutexWriteLock(Scene.PhysicsAndRayCastLock))
			{
				if (this._groundEntityWhenDisabled != null)
				{
					this._groundEntityWhenDisabled.AddChild(TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity), true);
				}
				if (!this._weapon.IsEmpty && !this._ownerGameEntity.BodyFlag.HasAnyFlag(BodyFlags.Disabled))
				{
					this._ownerGameEntity.SetPhysicsMoveToBatched(true);
					this._ownerGameEntity.ConvertDynamicBodyToRayCast();
				}
				else
				{
					this._ownerGameEntity.RemovePhysics(false);
				}
				this.ClampEntityPositionForStoppingIfNeeded();
				this.PhysicsStopped = true;
				if ((!base.IsDeactivated || this._groundEntityWhenDisabled != null) && !this._weapon.IsEmpty && GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					MissionObjectId id = base.Id;
					GameEntity groundEntityWhenDisabled = this._groundEntityWhenDisabled;
					GameNetwork.WriteMessage(new StopPhysicsAndSetFrameOfMissionObject(id, (groundEntityWhenDisabled != null) ? groundEntityWhenDisabled.GetFirstScriptOfType<MissionObject>().Id : MissionObjectId.Invalid, this._ownerGameEntity.GetLocalFrame()));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x000CDEE8 File Offset: 0x000CC0E8
		private GameEntity TryFindProperGroundEntityForSpawnedEntity()
		{
			Vec3 vec;
			Vec3 vec2;
			this._ownerGameEntity.GetPhysicsMinMax(true, out vec, out vec2, false);
			float num = vec2.z - vec.z;
			vec.z = vec2.z - 0.001f;
			Vec3 vec3 = (vec2 + vec) * 0.5f;
			float num2;
			Vec3 vec4;
			WeakGameEntity weakGameEntity;
			this._ownerGameEntity.Scene.RayCastForClosestEntityOrTerrain(vec3, vec3 - new Vec3(0f, 0f, num + 0.5f, -1f), out num2, out vec4, out weakGameEntity, 0.01f, BodyFlags.CommonCollisionExcludeFlagsForCombat);
			GameEntity gameEntity;
			if (!weakGameEntity.IsValid)
			{
				gameEntity = null;
			}
			else
			{
				MissionObject firstScriptOfTypeInFamily = weakGameEntity.GetFirstScriptOfTypeInFamily<MissionObject>();
				gameEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity((firstScriptOfTypeInFamily != null) ? firstScriptOfTypeInFamily.GameEntity : WeakGameEntity.Invalid);
			}
			this._groundEntityWhenDisabled = gameEntity;
			if (MathF.Abs(vec4.z - vec3.z) <= num + 0.5f)
			{
				return this._groundEntityWhenDisabled;
			}
			vec2.z = vec3.z;
			vec.z = vec3.z - 0.001f;
			this._ownerGameEntity.Scene.BoxCast(vec, vec2, false, Vec3.Zero, -Vec3.Up, num + 0.5f, out num2, out vec4, out weakGameEntity, BodyFlags.CommonCollisionExcludeFlagsForCombat);
			if (!MathF.IsValidValue(num2))
			{
				this._readyToBeDeleted = true;
				return null;
			}
			GameEntity gameEntity2;
			if (!weakGameEntity.IsValid)
			{
				gameEntity2 = null;
			}
			else
			{
				MissionObject firstScriptOfTypeInFamily2 = weakGameEntity.GetFirstScriptOfTypeInFamily<MissionObject>();
				gameEntity2 = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity((firstScriptOfTypeInFamily2 != null) ? firstScriptOfTypeInFamily2.GameEntity : WeakGameEntity.Invalid);
			}
			this._groundEntityWhenDisabled = gameEntity2;
			if (this._groundEntityWhenDisabled != null && MathF.Abs(vec4.z - vec3.z) <= num + 0.5f)
			{
				return this._groundEntityWhenDisabled;
			}
			return null;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000CE094 File Offset: 0x000CC294
		protected internal override void OnTickOccasionally(float currentFrameDeltaTime)
		{
			this.OnTickParallel2(currentFrameDeltaTime);
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000CE0A0 File Offset: 0x000CC2A0
		private void ClampEntityPositionForStoppingIfNeeded()
		{
			float minimumValue = CompressionBasic.PositionCompressionInfo.GetMinimumValue();
			float maximumValue = CompressionBasic.PositionCompressionInfo.GetMaximumValue();
			Vec3 vec = base.GameEntity.GetFrame().origin;
			bool flag;
			vec = vec.ClampedCopy(minimumValue, maximumValue, out flag);
			if (flag)
			{
				base.GameEntity.SetLocalPosition(vec);
			}
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x000CE0F7 File Offset: 0x000CC2F7
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			if (base.CreatedAtRuntime)
			{
				Mission.Current.AddSpawnedItemEntityCreatedAtRuntime(this);
			}
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x000CE114 File Offset: 0x000CC314
		protected override void OnRemoved(int removeReason)
		{
			if (base.HasUser && !GameNetwork.IsClientOrReplay)
			{
				base.UserAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			base.OnRemoved(removeReason);
			base.InvalidateWeakPointersIfValid();
			this._ownerGameEntity = null;
			Agent userAgent = base.UserAgent;
			if (userAgent != null)
			{
				userAgent.OnItemRemovedFromScene();
			}
			Agent movingAgent = this.MovingAgent;
			if (movingAgent == null)
			{
				return;
			}
			movingAgent.OnItemRemovedFromScene();
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000CE172 File Offset: 0x000CC372
		public void AttachWeaponToWeapon(MissionWeapon attachedWeapon, ref MatrixFrame attachLocalFrame)
		{
			this._weapon.AttachWeapon(attachedWeapon, ref attachLocalFrame);
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000CE184 File Offset: 0x000CC384
		public bool IsReadyToBeDeleted()
		{
			return (!base.HasUser && this._readyToBeDeleted) || (this._groundEntityWhenDisabled != null && !this._groundEntityWhenDisabled.HasScene()) || (this._groundEntityWhenDisabled != null && !this._groundEntityWhenDisabled.IsVisibleIncludeParents() && (!this._groundEntityWhenDisabled.HasBody() || this._groundEntityWhenDisabled.BodyFlag.HasAnyFlag(BodyFlags.Disabled)));
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x000CE1FC File Offset: 0x000CC3FC
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.GameEntity.SetPhysicsMoveToBatched(false);
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (isSuccessful)
			{
				if (this._clientSyncData != null)
				{
					this._clientSyncData = null;
					base.GameEntity.SetAlpha(1f);
				}
				bool flag;
				userAgent.OnItemPickup(this, (EquipmentIndex)preferenceIndex, out flag);
				if (flag)
				{
					this._readyToBeDeleted = true;
					this.PhysicsStopped = true;
					base.IsDeactivated = true;
				}
			}
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x000CE268 File Offset: 0x000CC468
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			base.GameEntity.SetPhysicsMoveToBatched(false);
			base.OnUse(userAgent, agentBoneIndex);
			if (!GameNetwork.IsClientOrReplay)
			{
				MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
				float num = globalFrame.origin.z;
				num = Math.Max(num, globalFrame.origin.z + globalFrame.rotation.u.z * (float)this._weapon.CurrentUsageItem.WeaponLength * 0.0075f);
				float eyeGlobalHeight = userAgent.GetEyeGlobalHeight();
				bool isLeftStance = userAgent.GetIsLeftStance();
				ItemObject.ItemTypeEnum itemType = this._weapon.Item.ItemType;
				if (userAgent.HasMount)
				{
					this._usedChannelIndex = 1;
					MatrixFrame frame = userAgent.Frame;
					bool flag = Vec2.DotProduct(frame.rotation.f.AsVec2.LeftVec(), (base.GameEntity.GetGlobalFrame().origin - frame.origin).AsVec2) > 0f;
					if (num < eyeGlobalHeight * 0.7f + userAgent.Position.z)
					{
						if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_left_begin : ActionIndexCache.act_pickup_from_right_down_horseback_left_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_left_end : ActionIndexCache.act_pickup_from_right_down_horseback_left_end);
						}
						else
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_begin : ActionIndexCache.act_pickup_from_right_down_horseback_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_down_horseback_end : ActionIndexCache.act_pickup_from_right_down_horseback_end);
						}
					}
					else if (num < eyeGlobalHeight * 1.1f + userAgent.Position.z)
					{
						if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_left_begin : ActionIndexCache.act_pickup_from_right_middle_horseback_left_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_left_end : ActionIndexCache.act_pickup_from_right_middle_horseback_left_end);
						}
						else
						{
							this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_begin : ActionIndexCache.act_pickup_from_right_middle_horseback_begin);
							this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_middle_horseback_end : ActionIndexCache.act_pickup_from_right_middle_horseback_end);
						}
					}
					else if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_left_begin : ActionIndexCache.act_pickup_from_right_up_horseback_left_begin);
						this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_left_end : ActionIndexCache.act_pickup_from_right_up_horseback_left_end);
					}
					else
					{
						this._progressActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_begin : ActionIndexCache.act_pickup_from_right_up_horseback_begin);
						this._successActionIndex = (flag ? ActionIndexCache.act_pickup_from_left_up_horseback_end : ActionIndexCache.act_pickup_from_right_up_horseback_end);
					}
				}
				else if (this._weapon.CurrentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.RangedWeapon | WeaponFlags.NotUsableWithOneHand | WeaponFlags.Consumable))
				{
					this._usedChannelIndex = 0;
					this._progressActionIndex = ActionIndexCache.act_pickup_boulder_begin;
					this._successActionIndex = ActionIndexCache.act_pickup_boulder_end;
				}
				else if (num < eyeGlobalHeight * 0.4f + userAgent.Position.z)
				{
					this._usedChannelIndex = 0;
					if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_left_begin_left_stance : ActionIndexCache.act_pickup_down_left_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_left_end_left_stance : ActionIndexCache.act_pickup_down_left_end);
					}
					else
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_begin_left_stance : ActionIndexCache.act_pickup_down_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_down_end_left_stance : ActionIndexCache.act_pickup_down_end);
					}
				}
				else if (num < eyeGlobalHeight * 1.1f + userAgent.Position.z)
				{
					this._usedChannelIndex = 1;
					if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_left_begin_left_stance : ActionIndexCache.act_pickup_middle_left_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_left_end_left_stance : ActionIndexCache.act_pickup_middle_left_end);
					}
					else
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_begin_left_stance : ActionIndexCache.act_pickup_middle_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_middle_end_left_stance : ActionIndexCache.act_pickup_middle_end);
					}
				}
				else
				{
					this._usedChannelIndex = 1;
					if (this._weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.AttachmentMask) || itemType == ItemObject.ItemTypeEnum.Bow || itemType == ItemObject.ItemTypeEnum.Shield)
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_left_begin_left_stance : ActionIndexCache.act_pickup_up_left_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_left_end_left_stance : ActionIndexCache.act_pickup_up_left_end);
					}
					else
					{
						this._progressActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_begin_left_stance : ActionIndexCache.act_pickup_up_begin);
						this._successActionIndex = (isLeftStance ? ActionIndexCache.act_pickup_up_end_left_stance : ActionIndexCache.act_pickup_up_end);
					}
				}
				this.SetVisibleSynched(true, false);
				userAgent.SetActionChannel(this._usedChannelIndex, in this._progressActionIndex, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x000CE790 File Offset: 0x000CC990
		public override bool IsDisabledForAgent(Agent agent)
		{
			return (this._weapon.IsAnyConsumable() && this._weapon.Amount == 0) || (this._weapon.IsBanner() && !MissionGameModels.Current.BattleBannerBearersModel.IsInteractableFormationBanner(this, agent));
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000CE7DC File Offset: 0x000CC9DC
		protected internal override void OnPhysicsCollision(ref PhysicsContact contact, WeakGameEntity entity0, WeakGameEntity entity1)
		{
			if (!GameNetwork.IsDedicatedServer && contact.NumberOfContactPairs > 0)
			{
				PhysicsContactInfo physicsContactInfo = default(PhysicsContactInfo);
				bool flag = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < contact.NumberOfContactPairs; i++)
				{
					for (int j = 0; j < contact[i].NumberOfContacts; j++)
					{
						if (!flag || contact[i][j].Impulse.LengthSquared > physicsContactInfo.Impulse.LengthSquared)
						{
							physicsContactInfo = contact[i][j];
							flag = true;
						}
					}
					switch (contact[i].ContactEventType)
					{
					case PhysicsEventType.CollisionStart:
						num++;
						break;
					case PhysicsEventType.CollisionStay:
						num2++;
						break;
					case PhysicsEventType.CollisionEnd:
						num3++;
						break;
					default:
						Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Usables\\SpawnedItemEntity.cs", "OnPhysicsCollision", 804);
						break;
					}
				}
				if (num2 > 0)
				{
					this.PlayPhysicsRollSound(physicsContactInfo.Impulse, physicsContactInfo.Position, physicsContactInfo.PhysicsMaterial1);
					return;
				}
				if (num > 0)
				{
					this.PlayPhysicsCollisionSound(physicsContactInfo.Impulse, physicsContactInfo.PhysicsMaterial1, physicsContactInfo.Position);
				}
			}
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000CE91C File Offset: 0x000CCB1C
		private void PlayPhysicsCollisionSound(Vec3 impulse, PhysicsMaterial collidedMat, Vec3 collisionPoint)
		{
			float num = this._deletionTimer.ElapsedTime();
			if (impulse.LengthSquared > 0.0025000002f && this._lastSoundPlayTime + 0.333f < num)
			{
				this._lastSoundPlayTime = num;
				WeaponClass weaponClass = this._weapon.CurrentUsageItem.WeaponClass;
				float num2 = impulse.Length;
				bool flag = false;
				int num3;
				int num4;
				int num5;
				switch (weaponClass)
				{
				case WeaponClass.Dagger:
				case WeaponClass.ThrowingAxe:
				case WeaponClass.ThrowingKnife:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsDaggerlikeStone;
					goto IL_01AA;
				case WeaponClass.OneHandedSword:
				case WeaponClass.OneHandedAxe:
				case WeaponClass.Mace:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSwordlikeStone;
					goto IL_01AA;
				case WeaponClass.TwoHandedSword:
				case WeaponClass.TwoHandedAxe:
				case WeaponClass.TwoHandedMace:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsGreatswordlikeStone;
					goto IL_01AA;
				case WeaponClass.OneHandedPolearm:
				case WeaponClass.TwoHandedPolearm:
				case WeaponClass.LowGripPolearm:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone;
					goto IL_01AA;
				case WeaponClass.Arrow:
				case WeaponClass.Bolt:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeStone;
					goto IL_01AA;
				case WeaponClass.SlingStone:
				case WeaponClass.Sling:
				case WeaponClass.Stone:
				case WeaponClass.BallistaStone:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone;
					goto IL_01AA;
				case WeaponClass.Bow:
				case WeaponClass.Crossbow:
				case WeaponClass.Javelin:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsBowlikeStone;
					goto IL_01AA;
				case WeaponClass.Boulder:
				case WeaponClass.BallistaBoulder:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderStone;
					flag = true;
					goto IL_01AA;
				case WeaponClass.SmallShield:
				case WeaponClass.LargeShield:
					num3 = ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeDefault;
					num4 = ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeWood;
					num5 = ItemPhysicsSoundContainer.SoundCodePhysicsShieldlikeStone;
					goto IL_01AA;
				}
				num3 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeDefault;
				num4 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeWood;
				num5 = ItemPhysicsSoundContainer.SoundCodePhysicsSpearlikeStone;
				IL_01AA:
				if (!flag)
				{
					num2 *= 0.16666667f;
					num2 = MBMath.ClampFloat(num2, 0f, 1f);
				}
				else
				{
					num2 = (num2 - 7f) * 0.030303031f * 0.1f + 0.9f;
					num2 = MBMath.ClampFloat(num2, 0.9f, 1f);
				}
				int num6 = num3;
				if (collidedMat.IsValid)
				{
					string name = collidedMat.Name;
					if (name.Contains("wood"))
					{
						num6 = num4;
					}
					else if (name.Contains("stone"))
					{
						num6 = num5;
					}
				}
				SoundEventParameter soundEventParameter = new SoundEventParameter("Force", num2);
				Mission.Current.MakeSound(num6, collisionPoint, true, false, -1, -1, ref soundEventParameter);
			}
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x000CEB80 File Offset: 0x000CCD80
		private void PlayPhysicsRollSound(Vec3 impulse, Vec3 collisionPoint, PhysicsMaterial collidedMat)
		{
			WeaponComponentData currentUsageItem = this._weapon.CurrentUsageItem;
			if (currentUsageItem.WeaponClass == WeaponClass.Boulder && currentUsageItem.WeaponFlags.HasAllFlags(WeaponFlags.RangedWeapon | WeaponFlags.NotUsableWithOneHand | WeaponFlags.Consumable))
			{
				float num = this._deletionTimer.ElapsedTime();
				if (impulse.LengthSquared > 0.0001f && this._lastSoundPlayTime + 0.333f < num)
				{
					if (this._rollingSoundEvent == null || !this._rollingSoundEvent.IsValid)
					{
						this._lastSoundPlayTime = num;
						int num2 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderDefault;
						string name = collidedMat.Name;
						if (name.Contains("stone"))
						{
							num2 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderStone;
						}
						else if (name.Contains("wood"))
						{
							num2 = ItemPhysicsSoundContainer.SoundCodePhysicsBoulderWood;
						}
						this._rollingSoundEvent = SoundEvent.CreateEvent(num2, Mission.Current.Scene);
						this._rollingSoundEvent.PlayInPosition(collisionPoint);
					}
					float num3 = impulse.Length * 0.033333335f;
					num3 = MBMath.ClampFloat(num3, 0f, 1f);
					this._rollingSoundEvent.SetParameter("Force", num3);
					this._rollingSoundEvent.SetPosition(collisionPoint);
				}
			}
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x000CECA1 File Offset: 0x000CCEA1
		public bool IsStuckMissile()
		{
			return this.SpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.AsMissile);
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x000CECAF File Offset: 0x000CCEAF
		public bool IsQuiverAndNotEmpty()
		{
			return this._weapon.Item.PrimaryWeapon.IsConsumable && this._weapon.Amount > 0;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000CECD8 File Offset: 0x000CCED8
		public bool IsBanner()
		{
			return this._weapon.IsBanner();
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x000CECE5 File Offset: 0x000CCEE5
		public override TextObject GetInfoTextForBeingNotInteractable(Agent userAgent)
		{
			if (!base.IsDeactivated && this._weapon.IsAnyConsumable() && this._weapon.Amount == 0)
			{
				return GameTexts.FindText("str_ui_empty_quiver", null);
			}
			return base.GetInfoTextForBeingNotInteractable(userAgent);
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x000CED1C File Offset: 0x000CCF1C
		public void StopPhysicsAndSetFrameForClient(MatrixFrame frame, GameEntity parent)
		{
			if (parent != null)
			{
				frame = parent.GetGlobalFrame().TransformToParent(in frame);
			}
			frame.rotation.Orthonormalize();
			this._clientSyncData = new SpawnedItemEntity.ClientSyncData();
			this._clientSyncData.Frame = frame;
			this._clientSyncData.Timer = new Timer(Mission.Current.CurrentTime, 0.5f, false);
			this._clientSyncData.Parent = parent;
			if (!this.PhysicsStopped)
			{
				this.PhysicsStopped = true;
				if (!this._weapon.IsEmpty && !base.GameEntity.BodyFlag.HasAnyFlag(BodyFlags.Disabled))
				{
					base.GameEntity.DisableDynamicBodySimulation();
					return;
				}
				base.GameEntity.RemovePhysics(false);
			}
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x000CEDDD File Offset: 0x000CCFDD
		public void ConsumeWeaponAmount(short consumedAmount)
		{
			this._weapon.Consume(consumedAmount);
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x000CEDEC File Offset: 0x000CCFEC
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x000CEDEF File Offset: 0x000CCFEF
		public void RequestDeletionOnNextTick()
		{
			this._deletionTimer = new Timer(Mission.Current.CurrentTime, -1f, true);
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x000CEE0C File Offset: 0x000CD00C
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, false);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000CEE16 File Offset: 0x000CD016
		public SpawnedItemEntity()
			: base(false)
		{
		}

		// Token: 0x0400155D RID: 5469
		private MissionWeapon _weapon;

		// Token: 0x0400155E RID: 5470
		private bool _hasLifeTime;

		// Token: 0x0400155F RID: 5471
		public string WeaponName = "";

		// Token: 0x04001560 RID: 5472
		private const float LongLifeTime = 180f;

		// Token: 0x04001561 RID: 5473
		private const float DisablePhysicsTime = 10f;

		// Token: 0x04001562 RID: 5474
		private const float QuickFadeoutLifeTime = 5f;

		// Token: 0x04001563 RID: 5475
		private const float TotalFadeOutInDuration = 0.5f;

		// Token: 0x04001564 RID: 5476
		private const float PreventStationaryCheckTime = 1f;

		// Token: 0x04001565 RID: 5477
		private Timer _disablePhysicsTimer;

		// Token: 0x04001566 RID: 5478
		private bool _physicsStopped;

		// Token: 0x04001567 RID: 5479
		private bool _readyToBeDeleted;

		// Token: 0x04001568 RID: 5480
		private Timer _deletionTimer;

		// Token: 0x04001569 RID: 5481
		private int _usedChannelIndex;

		// Token: 0x0400156A RID: 5482
		private ActionIndexCache _progressActionIndex;

		// Token: 0x0400156B RID: 5483
		private ActionIndexCache _successActionIndex;

		// Token: 0x0400156C RID: 5484
		private float _lastSoundPlayTime;

		// Token: 0x0400156D RID: 5485
		private const float MinSoundDelay = 0.333f;

		// Token: 0x0400156E RID: 5486
		private SoundEvent _rollingSoundEvent;

		// Token: 0x0400156F RID: 5487
		private SpawnedItemEntity.ClientSyncData _clientSyncData;

		// Token: 0x04001570 RID: 5488
		private GameEntity _ownerGameEntity;

		// Token: 0x04001571 RID: 5489
		private Vec3 _fakeSimulationVelocity;

		// Token: 0x04001572 RID: 5490
		private bool _alreadyMadeWaterDropSound;

		// Token: 0x04001574 RID: 5492
		private bool _disableDynamicPhysicsNextFrame;

		// Token: 0x04001575 RID: 5493
		private GameEntity _groundEntityWhenDisabled;

		// Token: 0x0200064A RID: 1610
		private class ClientSyncData
		{
			// Token: 0x04002140 RID: 8512
			public MatrixFrame Frame;

			// Token: 0x04002141 RID: 8513
			public GameEntity Parent;

			// Token: 0x04002142 RID: 8514
			public Timer Timer;
		}
	}
}
