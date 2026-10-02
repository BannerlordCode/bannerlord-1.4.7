using System;
using System.Threading;
using Helpers;
using SandBox.View.Map.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000063 RID: 99
	public class MobilePartyVisual : MapEntityVisual<PartyBase>
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0001E3B7 File Offset: 0x0001C5B7
		public override float BearingRotation
		{
			get
			{
				return this._bearingRotation;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001E3C0 File Offset: 0x0001C5C0
		private Scene MapScene
		{
			get
			{
				if (this._mapScene == null && Campaign.Current != null && Campaign.Current.MapSceneWrapper != null)
				{
					this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
				}
				return this._mapScene;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001E40E File Offset: 0x0001C60E
		public override MapEntityVisual AttachedTo
		{
			get
			{
				MobileParty mobileParty = base.MapEntity.MobileParty;
				if (((mobileParty != null) ? mobileParty.AttachedTo : null) != null)
				{
					return MobilePartyVisualManager.Current.GetVisualOfEntity(base.MapEntity.MobileParty.AttachedTo.Party);
				}
				return null;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0001E44A File Offset: 0x0001C64A
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return ((IInteractablePoint)base.MapEntity).GetInteractionPosition(MobileParty.MainParty);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0001E45C File Offset: 0x0001C65C
		public override bool IsMobileEntity
		{
			get
			{
				return base.MapEntity.IsMobile;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0001E469 File Offset: 0x0001C669
		public override bool IsMainEntity
		{
			get
			{
				return base.MapEntity == PartyBase.MainParty;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0001E478 File Offset: 0x0001C678
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0001E480 File Offset: 0x0001C680
		public GameEntity StrategicEntity { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001E489 File Offset: 0x0001C689
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0001E491 File Offset: 0x0001C691
		public AgentVisuals HumanAgentVisuals { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001E49A File Offset: 0x0001C69A
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0001E4A2 File Offset: 0x0001C6A2
		public AgentVisuals MountAgentVisuals { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001E4AB File Offset: 0x0001C6AB
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x0001E4B3 File Offset: 0x0001C6B3
		public AgentVisuals CaravanMountAgentVisuals { get; private set; }

		// Token: 0x060003FA RID: 1018 RVA: 0x0001E4BC File Offset: 0x0001C6BC
		public MobilePartyVisual(PartyBase partyBase)
			: base(partyBase)
		{
			this.CircleLocalFrame = MatrixFrame.Identity;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001E4D0 File Offset: 0x0001C6D0
		public override bool IsEnemyOf(IFaction faction)
		{
			return FactionManager.IsAtWarAgainstFaction(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001E4E8 File Offset: 0x0001C6E8
		public override bool IsInSameFaction(IFaction faction)
		{
			return DiplomacyHelper.IsSameFactionAndNotEliminated(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001E500 File Offset: 0x0001C700
		public override bool IsAllyOf(IFaction faction)
		{
			return DiplomacyHelper.HasAllianceWithFaction(base.MapEntity.MapFaction, faction.MapFaction);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001E518 File Offset: 0x0001C718
		internal void OnPartyRemoved()
		{
			if (this.StrategicEntity != null)
			{
				this.RemoveVisualFromVisualsOfEntities();
				this.ReleaseResources();
				this.StrategicEntity.Remove(111);
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001E544 File Offset: 0x0001C744
		public override void OnTrackAction()
		{
			MobileParty mobileParty = base.MapEntity.MobileParty;
			if (mobileParty != null)
			{
				if (Campaign.Current.VisualTrackerManager.CheckTracked(mobileParty))
				{
					Campaign.Current.VisualTrackerManager.RemoveTrackedObject(mobileParty, false);
					return;
				}
				Campaign.Current.VisualTrackerManager.RegisterObject(mobileParty);
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001E594 File Offset: 0x0001C794
		public override bool OnMapClick(bool followModifierUsed)
		{
			MobileParty.NavigationType navigationType;
			if (this.IsMainEntity)
			{
				MobileParty.MainParty.SetMoveModeHold();
			}
			else if (base.MapEntity.MobileParty.IsCurrentlyAtSea == MobileParty.MainParty.IsCurrentlyAtSea && NavigationHelper.CanPlayerNavigateToPosition(base.MapEntity.MobileParty.Position, out navigationType))
			{
				if (followModifierUsed)
				{
					MobileParty.MainParty.SetMoveEscortParty(base.MapEntity.MobileParty, navigationType, false);
				}
				else
				{
					MobileParty.MainParty.SetMoveEngageParty(base.MapEntity.MobileParty, navigationType);
				}
			}
			return true;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001E620 File Offset: 0x0001C820
		public override void OnHover()
		{
			if (base.MapEntity.MapEvent != null)
			{
				InformationManager.ShowTooltip(typeof(MapEvent), new object[] { base.MapEntity.MapEvent });
				return;
			}
			if (base.MapEntity.IsMobile && base.MapEntity.IsVisible)
			{
				if (base.MapEntity.MobileParty.Army != null && base.MapEntity.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(base.MapEntity.MobileParty))
				{
					if (base.MapEntity.MobileParty.Army.LeaderParty.SiegeEvent != null)
					{
						InformationManager.ShowTooltip(typeof(SiegeEvent), new object[] { base.MapEntity.MobileParty.Army.LeaderParty.SiegeEvent });
						return;
					}
					InformationManager.ShowTooltip(typeof(Army), new object[]
					{
						base.MapEntity.MobileParty.Army,
						false,
						true
					});
					return;
				}
				else
				{
					if (base.MapEntity.MobileParty.SiegeEvent != null)
					{
						InformationManager.ShowTooltip(typeof(SiegeEvent), new object[] { base.MapEntity.MobileParty.SiegeEvent });
						return;
					}
					InformationManager.ShowTooltip(typeof(MobileParty), new object[]
					{
						base.MapEntity.MobileParty,
						false,
						true
					});
				}
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001E7B4 File Offset: 0x0001C9B4
		public override Vec3 GetVisualPosition()
		{
			return base.MapEntity.MobileParty.VisualPosition2DWithoutError.ToVec3(base.MapEntity.Position.AsVec3().Z);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
		public override void ReleaseResources()
		{
			this.ResetPartyIcon();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		public override bool IsVisibleOrFadingOut()
		{
			return this._entityAlpha > 0f;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001E80C File Offset: 0x0001CA0C
		public override void OnOpenEncyclopedia()
		{
			if (base.MapEntity.MobileParty.IsLordParty && base.MapEntity.MobileParty.LeaderHero != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(base.MapEntity.MobileParty.LeaderHero.EncyclopediaLink);
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001E864 File Offset: 0x0001CA64
		internal void Tick(float dt, float realDt, ref int dirtyPartiesCount, ref MobilePartyVisual[] dirtyPartiesList)
		{
			if (this.StrategicEntity == null)
			{
				return;
			}
			if (base.MapEntity.IsVisualDirty && (this._entityAlpha > 0f || base.MapEntity.IsVisible))
			{
				int num = Interlocked.Increment(ref dirtyPartiesCount);
				dirtyPartiesList[num] = this;
			}
			if (this.IsVisibleOrFadingOut() && this.StrategicEntity != null && (!base.MapEntity.MobileParty.IsCurrentlyAtSea || base.MapEntity.MobileParty.IsTransitionInProgress))
			{
				this.UpdateBearingRotation(realDt, dt);
				this._speed = (base.MapEntity.MobileParty.IsActive ? base.MapEntity.MobileParty.Speed : 0f);
				float num2 = ((this.MountAgentVisuals != null) ? 1.3f : 1f);
				float num3 = MathF.Min(0.25f * num2 * this._speed / 0.3f, 20f);
				bool flag = this.IsEntityMovingVisually();
				AgentVisuals humanAgentVisuals = this.HumanAgentVisuals;
				if (humanAgentVisuals != null)
				{
					humanAgentVisuals.Tick(this.MountAgentVisuals, dt, flag, num3);
				}
				AgentVisuals mountAgentVisuals = this.MountAgentVisuals;
				if (mountAgentVisuals != null)
				{
					mountAgentVisuals.Tick(null, dt, flag, num3);
				}
				AgentVisuals caravanMountAgentVisuals = this.CaravanMountAgentVisuals;
				if (caravanMountAgentVisuals != null)
				{
					caravanMountAgentVisuals.Tick(null, dt, flag, num3);
				}
				MobileParty mobileParty = base.MapEntity.MobileParty;
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin = this.GetVisualPosition();
				if (mobileParty.Army != null && mobileParty.AttachedTo == mobileParty.Army.LeaderParty && (base.MapEntity.MapEvent == null || !base.MapEntity.MapEvent.IsFieldBattle))
				{
					MatrixFrame frame = this.StrategicEntity.GetFrame();
					Vec2 vec = identity.origin.AsVec2 - frame.origin.AsVec2;
					if (vec.Length / dt > 20f)
					{
						identity.rotation.RotateAboutUp(this._bearingRotation);
					}
					else if (mobileParty.CurrentSettlement == null)
					{
						float num4 = MBMath.LerpRadians(frame.rotation.f.AsVec2.RotationInRadians, (vec + Vec2.FromRotation(this._bearingRotation) * 0.01f).RotationInRadians, Math.Min(6f * dt, 1f), 0.03f * dt, 10f * dt);
						identity.rotation.RotateAboutUp(num4);
					}
					else
					{
						float rotationInRadians = frame.rotation.f.AsVec2.RotationInRadians;
						identity.rotation.RotateAboutUp(rotationInRadians);
					}
				}
				else if (mobileParty.CurrentSettlement == null)
				{
					identity.rotation.RotateAboutUp(this.GetVisualRotation());
				}
				MatrixFrame matrixFrame = this.StrategicEntity.GetFrame();
				if (!matrixFrame.NearlyEquals(identity, 1E-05f))
				{
					this.StrategicEntity.SetFrame(ref identity, true);
					if (this.HumanAgentVisuals != null)
					{
						MatrixFrame matrixFrame2 = identity;
						matrixFrame2.rotation.ApplyScaleLocal(this.HumanAgentVisuals.GetScale());
						this.HumanAgentVisuals.GetWeakEntity().SetFrame(ref matrixFrame2, true);
					}
					if (this.MountAgentVisuals != null)
					{
						MatrixFrame matrixFrame3 = identity;
						matrixFrame3.rotation.ApplyScaleLocal(this.MountAgentVisuals.GetScale());
						this.MountAgentVisuals.GetWeakEntity().SetFrame(ref matrixFrame3, true);
					}
					if (this.CaravanMountAgentVisuals != null)
					{
						matrixFrame = this.CaravanMountAgentVisuals.GetFrame();
						MatrixFrame matrixFrame4 = identity.TransformToParent(in matrixFrame);
						matrixFrame4.rotation.ApplyScaleLocal(this.CaravanMountAgentVisuals.GetScale());
						this.CaravanMountAgentVisuals.GetWeakEntity().SetFrame(ref matrixFrame4, true);
					}
				}
				this.ApplyWindEffect();
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001EC24 File Offset: 0x0001CE24
		private void ApplyWindEffect()
		{
			if (this.HumanAgentVisuals != null && !this.HumanAgentVisuals.GetEquipment()[EquipmentIndex.ExtraWeaponSlot].IsEmpty)
			{
				this.HumanAgentVisuals.SetClothWindToWeaponAtIndex(-this.StrategicEntity.GetGlobalFrame().rotation.f, false, EquipmentIndex.ExtraWeaponSlot);
			}
			ClothSimulatorComponent clothSimulatorComponent;
			if (this._cachedBannerComponent.Item2 != null && (clothSimulatorComponent = this._cachedBannerComponent.Item2 as ClothSimulatorComponent) != null)
			{
				clothSimulatorComponent.SetForcedWind(-this.StrategicEntity.GetGlobalFrame().rotation.f, false);
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001ECC4 File Offset: 0x0001CEC4
		internal void OnStartup()
		{
			bool flag = false;
			if (base.MapEntity.IsMobile)
			{
				this.StrategicEntity = GameEntity.CreateEmpty(this.MapScene, true, true, true);
				if (!base.MapEntity.IsVisible)
				{
					this.StrategicEntity.EntityFlags |= EntityFlags.DoNotTick;
				}
			}
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(base.MapEntity);
			if (!flag)
			{
				this.CircleLocalFrame = MatrixFrame.Identity;
				if ((visualPartyLeader != null && visualPartyLeader.HasMount()) || base.MapEntity.MobileParty.IsCaravan)
				{
					MatrixFrame circleLocalFrame = this.CircleLocalFrame;
					Mat3 rotation = circleLocalFrame.rotation;
					rotation.ApplyScaleLocal(0.4625f);
					circleLocalFrame.rotation = rotation;
					this.CircleLocalFrame = circleLocalFrame;
				}
				else
				{
					MatrixFrame circleLocalFrame2 = this.CircleLocalFrame;
					Mat3 rotation2 = circleLocalFrame2.rotation;
					rotation2.ApplyScaleLocal(0.3725f);
					circleLocalFrame2.rotation = rotation2;
					this.CircleLocalFrame = circleLocalFrame2;
				}
			}
			this._bearingRotation = base.MapEntity.MobileParty.Bearing.RotationInRadians;
			this.StrategicEntity.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
			if (this.HumanAgentVisuals != null)
			{
				WeakGameEntity weakEntity = this.HumanAgentVisuals.GetWeakEntity();
				if (weakEntity != WeakGameEntity.Invalid)
				{
					weakEntity.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
				}
			}
			if (this.MountAgentVisuals != null)
			{
				WeakGameEntity weakEntity2 = this.MountAgentVisuals.GetWeakEntity();
				if (weakEntity2 != WeakGameEntity.Invalid)
				{
					weakEntity2.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
				}
			}
			if (this.CaravanMountAgentVisuals != null)
			{
				WeakGameEntity weakEntity3 = this.CaravanMountAgentVisuals.GetWeakEntity();
				if (weakEntity3 != WeakGameEntity.Invalid)
				{
					weakEntity3.SetVisibilityExcludeParents(base.MapEntity.IsVisible);
				}
			}
			this.StrategicEntity.SetReadyToRender(true);
			this.StrategicEntity.SetEntityEnvMapVisibility(false);
			this._entityAlpha = 0f;
			if (base.MapEntity.IsVisible)
			{
				if (base.MapEntity.MobileParty.IsTransitionInProgress)
				{
					this.TickFadingState(0.1f, 0.1f);
				}
				else
				{
					this._entityAlpha = 1f;
				}
			}
			this.AddVisualToVisualsOfEntities();
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001EEDC File Offset: 0x0001D0DC
		internal void TickFadingState(float realDt, float dt)
		{
			if ((!base.MapEntity.MobileParty.IsTransitionInProgress || !base.MapEntity.IsVisible) && ((this._entityAlpha < 1f && base.MapEntity.IsVisible) || (this._entityAlpha > 0f && !base.MapEntity.IsVisible)))
			{
				if (base.MapEntity.IsVisible)
				{
					if (this._entityAlpha <= 0f)
					{
						this.StrategicEntity.SetVisibilityExcludeParents(true);
						if (this.HumanAgentVisuals != null)
						{
							WeakGameEntity weakEntity = this.HumanAgentVisuals.GetWeakEntity();
							if (weakEntity != WeakGameEntity.Invalid)
							{
								weakEntity.SetVisibilityExcludeParents(true);
							}
						}
						if (this.MountAgentVisuals != null)
						{
							WeakGameEntity weakEntity2 = this.MountAgentVisuals.GetWeakEntity();
							if (weakEntity2 != WeakGameEntity.Invalid)
							{
								weakEntity2.SetVisibilityExcludeParents(true);
							}
						}
						if (this.CaravanMountAgentVisuals != null)
						{
							WeakGameEntity weakEntity3 = this.CaravanMountAgentVisuals.GetWeakEntity();
							if (weakEntity3 != WeakGameEntity.Invalid)
							{
								weakEntity3.SetVisibilityExcludeParents(true);
							}
						}
					}
					this._entityAlpha = MathF.Min(this._entityAlpha + MathF.Max(realDt, 1E-05f), 1f);
					this.StrategicEntity.SetAlpha(this._entityAlpha);
					if (this.HumanAgentVisuals != null)
					{
						WeakGameEntity weakEntity4 = this.HumanAgentVisuals.GetWeakEntity();
						if (weakEntity4 != WeakGameEntity.Invalid)
						{
							weakEntity4.SetAlpha(this._entityAlpha);
						}
					}
					if (this.MountAgentVisuals != null)
					{
						WeakGameEntity weakEntity5 = this.MountAgentVisuals.GetWeakEntity();
						if (weakEntity5 != WeakGameEntity.Invalid)
						{
							weakEntity5.SetAlpha(this._entityAlpha);
						}
					}
					if (this.CaravanMountAgentVisuals != null)
					{
						WeakGameEntity weakEntity6 = this.CaravanMountAgentVisuals.GetWeakEntity();
						if (weakEntity6 != WeakGameEntity.Invalid)
						{
							weakEntity6.SetAlpha(this._entityAlpha);
						}
					}
					this.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
					return;
				}
				this._entityAlpha = MathF.Max(this._entityAlpha - MathF.Max(realDt, 1E-05f), 0f);
				this.StrategicEntity.SetAlpha(this._entityAlpha);
				if (this.HumanAgentVisuals != null)
				{
					WeakGameEntity weakEntity7 = this.HumanAgentVisuals.GetWeakEntity();
					if (weakEntity7 != WeakGameEntity.Invalid)
					{
						weakEntity7.SetAlpha(this._entityAlpha);
					}
				}
				if (this.MountAgentVisuals != null)
				{
					WeakGameEntity weakEntity8 = this.MountAgentVisuals.GetWeakEntity();
					if (weakEntity8 != WeakGameEntity.Invalid)
					{
						weakEntity8.SetAlpha(this._entityAlpha);
					}
				}
				if (this.CaravanMountAgentVisuals != null)
				{
					WeakGameEntity weakEntity9 = this.CaravanMountAgentVisuals.GetWeakEntity();
					if (weakEntity9 != WeakGameEntity.Invalid)
					{
						weakEntity9.SetAlpha(this._entityAlpha);
					}
				}
				if (this._entityAlpha <= 0f)
				{
					this.StrategicEntity.SetVisibilityExcludeParents(false);
					if (this.HumanAgentVisuals != null)
					{
						WeakGameEntity weakEntity10 = this.HumanAgentVisuals.GetWeakEntity();
						if (weakEntity10 != WeakGameEntity.Invalid)
						{
							weakEntity10.SetVisibilityExcludeParents(false);
						}
					}
					if (this.MountAgentVisuals != null)
					{
						WeakGameEntity weakEntity11 = this.MountAgentVisuals.GetWeakEntity();
						if (weakEntity11 != WeakGameEntity.Invalid)
						{
							weakEntity11.SetVisibilityExcludeParents(false);
						}
					}
					if (this.CaravanMountAgentVisuals != null)
					{
						WeakGameEntity weakEntity12 = this.CaravanMountAgentVisuals.GetWeakEntity();
						if (weakEntity12 != WeakGameEntity.Invalid)
						{
							weakEntity12.SetVisibilityExcludeParents(false);
						}
					}
					this.StrategicEntity.EntityFlags |= EntityFlags.DoNotTick;
					return;
				}
			}
			else if (base.MapEntity.MobileParty.IsTransitionInProgress)
			{
				if ((base.MapEntity.MobileParty.Army == null || base.MapEntity.MobileParty.Army.LeaderParty == base.MapEntity.MobileParty || base.MapEntity.MobileParty.AttachedTo == null) && this.IsMobileEntity && this.GetTransitionProgress() < 1f)
				{
					this.TickTransitionFadeState(dt);
					return;
				}
			}
			else
			{
				MobilePartyVisualManager.Current.UnRegisterFadingVisual(this);
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001F2C4 File Offset: 0x0001D4C4
		private void UpdateBearingRotation(float realDt, float dt)
		{
			float num = MBMath.WrapAngle(base.MapEntity.MobileParty.Bearing.RotationInRadians - this._bearingRotation);
			float num2 = ((base.MapEntity.MapEvent != null) ? realDt : dt);
			this._bearingRotation += num * MathF.Min(num2 * 30f, 1f);
			this._bearingRotation = MBMath.WrapAngle(this._bearingRotation);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001F33C File Offset: 0x0001D53C
		private void TickTransitionFadeState(float dt)
		{
			float transitionProgress = this.GetTransitionProgress();
			if (base.MapEntity.MobileParty.IsCurrentlyAtSea)
			{
				this._entityAlpha = transitionProgress;
				AgentVisuals humanAgentVisuals = this.HumanAgentVisuals;
				if (humanAgentVisuals != null)
				{
					GameEntity entity = humanAgentVisuals.GetEntity();
					if (entity != null)
					{
						entity.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals mountAgentVisuals = this.MountAgentVisuals;
				if (mountAgentVisuals != null)
				{
					GameEntity entity2 = mountAgentVisuals.GetEntity();
					if (entity2 != null)
					{
						entity2.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals caravanMountAgentVisuals = this.CaravanMountAgentVisuals;
				if (caravanMountAgentVisuals != null)
				{
					GameEntity entity3 = caravanMountAgentVisuals.GetEntity();
					if (entity3 != null)
					{
						entity3.SetAlpha(this._entityAlpha);
					}
				}
				if (this.HumanAgentVisuals != null)
				{
					MatrixFrame frame = this.HumanAgentVisuals.GetEntity().GetFrame();
					CampaignVec2 campaignVec = base.MapEntity.MobileParty.EndPositionForNavigationTransition + base.MapEntity.MobileParty.ArmyPositionAdder;
					float num = MathF.Lerp(frame.origin.X, campaignVec.X, dt, 1E-05f);
					float num2 = MathF.Lerp(frame.origin.Y, campaignVec.Y, dt, 1E-05f);
					float num3 = MathF.Lerp(frame.origin.z, campaignVec.AsVec3().Z, dt, 1E-05f);
					frame.origin = new Vec3(num, num2, num3, -1f);
					GameEntity entity4 = this.HumanAgentVisuals.GetEntity();
					if (entity4 != null)
					{
						entity4.SetFrame(ref frame, false);
					}
					AgentVisuals mountAgentVisuals2 = this.MountAgentVisuals;
					if (mountAgentVisuals2 != null)
					{
						GameEntity entity5 = mountAgentVisuals2.GetEntity();
						if (entity5 != null)
						{
							entity5.SetFrame(ref frame, false);
						}
					}
					AgentVisuals caravanMountAgentVisuals2 = this.CaravanMountAgentVisuals;
					if (caravanMountAgentVisuals2 == null)
					{
						return;
					}
					GameEntity entity6 = caravanMountAgentVisuals2.GetEntity();
					if (entity6 == null)
					{
						return;
					}
					entity6.SetFrame(ref frame, false);
					return;
				}
			}
			else
			{
				this._entityAlpha = 1f - transitionProgress;
				AgentVisuals humanAgentVisuals2 = this.HumanAgentVisuals;
				if (humanAgentVisuals2 != null)
				{
					GameEntity entity7 = humanAgentVisuals2.GetEntity();
					if (entity7 != null)
					{
						entity7.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals mountAgentVisuals3 = this.MountAgentVisuals;
				if (mountAgentVisuals3 != null)
				{
					GameEntity entity8 = mountAgentVisuals3.GetEntity();
					if (entity8 != null)
					{
						entity8.SetAlpha(this._entityAlpha);
					}
				}
				AgentVisuals caravanMountAgentVisuals3 = this.CaravanMountAgentVisuals;
				if (caravanMountAgentVisuals3 == null)
				{
					return;
				}
				GameEntity entity9 = caravanMountAgentVisuals3.GetEntity();
				if (entity9 == null)
				{
					return;
				}
				entity9.SetAlpha(this._entityAlpha);
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001F558 File Offset: 0x0001D758
		internal void ValidateIsDirty()
		{
			if (base.MapEntity.MemberRoster.TotalManCount != 0)
			{
				this.RefreshPartyIcon();
				if ((this._entityAlpha < 1f && base.MapEntity.IsVisible) || (this._entityAlpha > 0f && !base.MapEntity.IsVisible))
				{
					MobilePartyVisualManager.Current.RegisterFadingVisual(this);
					return;
				}
			}
			else
			{
				this.ResetPartyIcon();
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001F5C4 File Offset: 0x0001D7C4
		private void RefreshPartyIcon()
		{
			if (base.MapEntity.IsVisualDirty)
			{
				base.MapEntity.OnVisualsUpdated();
				bool flag = true;
				bool flag2 = true;
				this.ResetPartyIcon();
				MatrixFrame circleLocalFrame = this.CircleLocalFrame;
				circleLocalFrame.origin = Vec3.Zero;
				this.CircleLocalFrame = circleLocalFrame;
				MobileParty mobileParty = base.MapEntity.MobileParty;
				if (((mobileParty != null) ? mobileParty.CurrentSettlement : null) != null)
				{
					this.AddVisualToVisualsOfEntities();
					if (!base.MapEntity.MobileParty.MapFaction.IsAtWarWith(base.MapEntity.MobileParty.CurrentSettlement.MapFaction))
					{
						Hero leaderHero = base.MapEntity.LeaderHero;
						if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
						{
							string bannerCode = base.MapEntity.LeaderHero.ClanBanner.BannerCode;
							if (string.IsNullOrEmpty(bannerCode))
							{
								goto IL_03FB;
							}
							MatrixFrame matrixFrame = MatrixFrame.Identity;
							Vec3 bannerPositionForParty = SettlementVisualManager.Current.GetSettlementVisual(base.MapEntity.MobileParty.CurrentSettlement).GetBannerPositionForParty(base.MapEntity.MobileParty);
							if (!bannerPositionForParty.IsValid)
							{
								goto IL_03FB;
							}
							matrixFrame.origin = bannerPositionForParty;
							MatrixFrame matrixFrame2 = this.StrategicEntity.GetGlobalFrame();
							matrixFrame.origin = matrixFrame2.TransformToLocal(in matrixFrame.origin);
							float num = MBMath.Map((float)base.MapEntity.NumberOfAllMembers / 400f * ((base.MapEntity.MobileParty.Army != null && base.MapEntity.MobileParty.Army.LeaderParty == base.MapEntity.MobileParty) ? 1.25f : 1f), 0f, 1f, 0.2f, 0.5f);
							matrixFrame = matrixFrame.Elevate(-num);
							matrixFrame.rotation.ApplyScaleLocal(num);
							matrixFrame2 = this.StrategicEntity.GetGlobalFrame();
							matrixFrame.rotation = matrixFrame2.rotation.TransformToLocal(in matrixFrame.rotation);
							this.StrategicEntity.AddSphereAsBody(matrixFrame.origin + Vec3.Up * 0.3f, 0.15f, BodyFlags.None);
							flag = false;
							string text = "campaign_flag";
							if (this._cachedBannerComponent.Item1 == bannerCode + text)
							{
								this._cachedBannerComponent.Item2.GetFirstMetaMesh().Frame = matrixFrame;
								this.StrategicEntity.AddComponent(this._cachedBannerComponent.Item2);
								goto IL_03FB;
							}
							MetaMesh bannerOfCharacter = MobilePartyVisual.GetBannerOfCharacter(new Banner(bannerCode), text);
							bannerOfCharacter.Frame = matrixFrame;
							int componentCount = this.StrategicEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator);
							this.StrategicEntity.AddMultiMesh(bannerOfCharacter, true);
							if (this.StrategicEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > componentCount)
							{
								this._cachedBannerComponent.Item1 = bannerCode + text;
								this._cachedBannerComponent.Item2 = this.StrategicEntity.GetComponentAtIndex(componentCount, GameEntity.ComponentType.ClothSimulator);
								goto IL_03FB;
							}
							goto IL_03FB;
						}
					}
					this.StrategicEntity.RemovePhysics(false);
				}
				else if (base.MapEntity.MobileParty != null && (base.MapEntity.MobileParty.IsCurrentlyAtSea || base.MapEntity.MobileParty.IsTransitionInProgress))
				{
					this.RemoveVisualFromVisualsOfEntities();
					if (base.MapEntity.MobileParty.IsTransitionInProgress)
					{
						if (base.MapEntity.MobileParty.Army == null || base.MapEntity.MobileParty.Army.LeaderParty == base.MapEntity.MobileParty || base.MapEntity.MobileParty.AttachedTo == null)
						{
							this.AddMobileIconComponents(base.MapEntity, ref flag, ref flag2);
						}
						if (!this._isInTransitionProgressCached)
						{
							this.AddVisualToVisualsOfEntities();
							this.OnTransitionStarted();
						}
					}
					if (base.MapEntity.MobileParty.IsTransitionInProgress != this._isInTransitionProgressCached)
					{
						if (this._isInTransitionProgressCached)
						{
							this.OnTransitionEnded();
						}
						else
						{
							this.OnTransitionStarted();
						}
					}
				}
				else
				{
					this.AddVisualToVisualsOfEntities();
					this.InitializePartyCollider(base.MapEntity);
					this.AddMobileIconComponents(base.MapEntity, ref flag, ref flag2);
				}
				IL_03FB:
				if (flag)
				{
					this._cachedBannerComponent = new ValueTuple<string, GameEntityComponent>(null, null);
				}
				if (flag2)
				{
					this._cachedBannerEntity = new ValueTuple<string, GameEntity>(null, null);
				}
				this.StrategicEntity.CheckResources(true, false);
				if (this.IsMobileEntity)
				{
					this._isInTransitionProgressCached = base.MapEntity.MobileParty.IsTransitionInProgress;
				}
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001FA18 File Offset: 0x0001DC18
		private void AddMobileIconComponents(PartyBase party, ref bool clearBannerComponentCache, ref bool clearBannerEntityCache)
		{
			uint num = (FactionManager.IsAtWarAgainstFaction(party.MapFaction, Hero.MainHero.MapFaction) ? 4294905856U : 4278206719U);
			if (this.IsPartOfBesiegerCamp(party))
			{
				this.AddTentEntityForParty(this.StrategicEntity, party, ref clearBannerComponentCache);
				return;
			}
			if (PartyBaseHelper.GetVisualPartyLeader(party) != null)
			{
				string text = null;
				Hero leaderHero = party.LeaderHero;
				if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
				{
					text = party.LeaderHero.ClanBanner.BannerCode;
				}
				ActionIndexCache act_none = ActionIndexCache.act_none;
				ActionIndexCache act_none2 = ActionIndexCache.act_none;
				MapEvent mapEvent = ((party.MobileParty.Army != null && party.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(party.MobileParty)) ? party.MobileParty.Army.LeaderParty.MapEvent : party.MapEvent);
				int num2;
				this.GetMeleeWeaponToWield(party, out num2);
				if (mapEvent != null && (mapEvent.EventType == MapEvent.BattleTypes.FieldBattle || mapEvent.EventType == MapEvent.BattleTypes.Raid || mapEvent.EventType == MapEvent.BattleTypes.SiegeOutside || mapEvent.EventType == MapEvent.BattleTypes.SallyOut))
				{
					MobilePartyVisual.GetPartyBattleAnimation(party, num2, out act_none, out act_none2);
				}
				IFaction mapFaction = party.MapFaction;
				uint num3 = ((mapFaction != null) ? mapFaction.Color : 4291609515U);
				IFaction mapFaction2 = party.MapFaction;
				uint num4 = ((mapFaction2 != null) ? mapFaction2.Color2 : 4291609515U);
				this.AddCharacterToPartyIcon(party, PartyBaseHelper.GetVisualPartyLeader(party), num, text, num2, num3, num4, in act_none, in act_none2, MBRandom.NondeterministicRandomFloat * 0.7f, ref clearBannerEntityCache);
				if (party.IsMobile)
				{
					string text2;
					string text3;
					this.GetMountAndHarnessVisualIdsForPartyIcon(out text2, out text3);
					if (!string.IsNullOrEmpty(text2))
					{
						this.AddMountToPartyIcon(new Vec3(0.3f, -0.25f, 0f, -1f), text2, text3, num, PartyBaseHelper.GetVisualPartyLeader(party));
					}
				}
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		private void AddMountToPartyIcon(Vec3 positionOffset, string mountItemId, string harnessItemId, uint contourColor, CharacterObject character)
		{
			ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(mountItemId);
			Monster monster = @object.HorseComponent.Monster;
			ItemObject itemObject = null;
			if (!string.IsNullOrEmpty(harnessItemId))
			{
				itemObject = Game.Current.ObjectManager.GetObject<ItemObject>(harnessItemId);
			}
			Equipment equipment = new Equipment();
			equipment[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(@object, null, null, false);
			equipment[EquipmentIndex.HorseHarness] = new EquipmentElement(itemObject, null, null, false);
			AgentVisualsData agentVisualsData = new AgentVisualsData().Equipment(equipment).Scale(@object.ScaleFactor * 0.3f);
			Mat3 identity = Mat3.Identity;
			AgentVisualsData agentVisualsData2 = agentVisualsData.Frame(new MatrixFrame(in identity, in positionOffset)).ActionSet(MBGlobals.GetActionSet(monster.ActionSetCode + "_map")).Scene(this.MapScene)
				.Monster(monster)
				.PrepareImmediately(false)
				.UseScaledWeapons(true)
				.HasClippingPlane(true)
				.MountCreationKey(MountCreationKey.GetRandomMountKeyString(@object, character.GetMountKeySeed()));
			this.CaravanMountAgentVisuals = AgentVisuals.Create(agentVisualsData2, "PartyIcon " + mountItemId, false, false, false);
			this.CaravanMountAgentVisuals.GetEntity().SetContourColor(new uint?(contourColor), false);
			MatrixFrame matrixFrame = this.CaravanMountAgentVisuals.GetFrame();
			matrixFrame.rotation.ApplyScaleLocal(this.CaravanMountAgentVisuals.GetScale());
			matrixFrame = this.StrategicEntity.GetFrame().TransformToParent(in matrixFrame);
			this.CaravanMountAgentVisuals.GetEntity().SetFrame(ref matrixFrame, true);
			float num = MathF.Min(0.325f * this._speed / 0.3f, 20f);
			this.CaravanMountAgentVisuals.Tick(null, 0.0001f, this.IsEntityMovingVisually(), num);
			this.CaravanMountAgentVisuals.GetEntity().Skeleton.ForceUpdateBoneFrames();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001FD80 File Offset: 0x0001DF80
		private void AddCharacterToPartyIcon(PartyBase party, CharacterObject characterObject, uint contourColor, string bannerKey, int wieldedItemIndex, uint teamColor1, uint teamColor2, in ActionIndexCache leaderAction, in ActionIndexCache mountAction, float animationStartDuration, ref bool clearBannerEntityCache)
		{
			Equipment equipment = characterObject.Equipment.Clone(false);
			bool flag = !string.IsNullOrEmpty(bannerKey) && (((characterObject.IsPlayerCharacter || characterObject.HeroObject.Clan == Clan.PlayerClan) && Clan.PlayerClan.Tier >= Campaign.Current.Models.ClanTierModel.BannerEligibleTier) || (!characterObject.IsPlayerCharacter && (!characterObject.IsHero || (characterObject.IsHero && characterObject.HeroObject.Clan != Clan.PlayerClan))));
			int num = 4;
			if (flag)
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>("campaign_banner_small");
				equipment[EquipmentIndex.ExtraWeaponSlot] = new EquipmentElement(@object, null, null, false);
			}
			Monster baseMonsterFromRace = TaleWorlds.Core.FaceGen.GetBaseMonsterFromRace(characterObject.Race);
			MBActionSet actionSetWithSuffix = MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, characterObject.IsFemale, flag ? "_map_with_banner" : "_map");
			AgentVisualsData agentVisualsData = new AgentVisualsData().UseMorphAnims(true).Equipment(equipment).BodyProperties(characterObject.GetBodyProperties(characterObject.Equipment, -1))
				.SkeletonType(characterObject.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.Scale(0.3f)
				.Frame(this.StrategicEntity.GetFrame())
				.ActionSet(actionSetWithSuffix)
				.Scene(this.MapScene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(false)
				.RightWieldedItemIndex(wieldedItemIndex)
				.HasClippingPlane(true)
				.UseScaledWeapons(true)
				.ClothColor1(teamColor1)
				.ClothColor2(teamColor2)
				.CharacterObjectStringId(characterObject.StringId)
				.AddColorRandomness(!characterObject.IsHero)
				.Race(characterObject.Race);
			if (flag)
			{
				Banner banner = new Banner(bannerKey);
				agentVisualsData.Banner(banner).LeftWieldedItemIndex(num);
				if (this._cachedBannerEntity.Item1 == bannerKey + "campaign_banner_small")
				{
					agentVisualsData.CachedWeaponEntity(EquipmentIndex.ExtraWeaponSlot, this._cachedBannerEntity.Item2);
				}
			}
			if (!party.MobileParty.IsCurrentlyAtSea || party.MobileParty.IsTransitionInProgress)
			{
				this.HumanAgentVisuals = AgentVisuals.Create(agentVisualsData, "PartyIcon " + characterObject.Name, false, false, false);
			}
			if (this.HumanAgentVisuals != null)
			{
				if (flag)
				{
					GameEntity entity = this.HumanAgentVisuals.GetEntity();
					GameEntity child = entity.GetChild(entity.ChildCount - 1);
					if (child.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > 0)
					{
						clearBannerEntityCache = false;
						this._cachedBannerEntity = new ValueTuple<string, GameEntity>(bannerKey + "campaign_banner_small", child);
					}
				}
				if (leaderAction != ActionIndexCache.act_none)
				{
					float actionAnimationDuration = MBActionSet.GetActionAnimationDuration(actionSetWithSuffix, in leaderAction);
					if (actionAnimationDuration < 1f)
					{
						this.HumanAgentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, in leaderAction, animationStartDuration, -0.2f, true, 0f);
					}
					else
					{
						this.HumanAgentVisuals.GetVisuals().GetSkeleton().SetAgentActionChannel(0, in leaderAction, animationStartDuration / actionAnimationDuration, -0.2f, true, 0f);
					}
				}
			}
			if (characterObject.HasMount() && (!party.MobileParty.IsCurrentlyAtSea || party.MobileParty.IsTransitionInProgress))
			{
				Monster monster = characterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item.HorseComponent.Monster;
				MBActionSet actionSet = MBGlobals.GetActionSet(monster.ActionSetCode + "_map");
				AgentVisualsData agentVisualsData2 = new AgentVisualsData().Equipment(characterObject.Equipment).Scale(characterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item.ScaleFactor * 0.3f).Frame(MatrixFrame.Identity)
					.ActionSet(actionSet)
					.Scene(this.MapScene)
					.Monster(monster)
					.PrepareImmediately(false)
					.UseScaledWeapons(true)
					.HasClippingPlane(true)
					.MountCreationKey(MountCreationKey.GetRandomMountKeyString(characterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, characterObject.GetMountKeySeed()));
				this.MountAgentVisuals = AgentVisuals.Create(agentVisualsData2, "PartyIcon " + characterObject.Name + " mount", false, false, false);
				if (mountAction != ActionIndexCache.act_none)
				{
					float actionAnimationDuration2 = MBActionSet.GetActionAnimationDuration(actionSet, in mountAction);
					if (actionAnimationDuration2 < 1f)
					{
						this.MountAgentVisuals.GetWeakEntity().Skeleton.SetAgentActionChannel(0, in mountAction, animationStartDuration, -0.2f, true, 0f);
					}
					else
					{
						this.MountAgentVisuals.GetWeakEntity().Skeleton.SetAgentActionChannel(0, in mountAction, animationStartDuration / actionAnimationDuration2, -0.2f, true, 0f);
					}
				}
				this.MountAgentVisuals.GetWeakEntity().SetContourColor(new uint?(contourColor), false);
				MatrixFrame frame = this.StrategicEntity.GetFrame();
				frame.rotation.ApplyScaleLocal(agentVisualsData2.ScaleData);
				this.MountAgentVisuals.GetWeakEntity().SetFrame(ref frame, true);
			}
			float num2 = ((this.MountAgentVisuals != null) ? 1.3f : 1f);
			float num3 = MathF.Min(0.25f * num2 * this._speed / 0.3f, 20f);
			if (this.MountAgentVisuals != null)
			{
				this.MountAgentVisuals.Tick(null, 0.0001f, this.IsEntityMovingVisually(), num3);
				this.MountAgentVisuals.GetWeakEntity().Skeleton.ForceUpdateBoneFrames();
			}
			if (this.HumanAgentVisuals != null)
			{
				WeakGameEntity weakEntity = this.HumanAgentVisuals.GetWeakEntity();
				weakEntity.SetContourColor(new uint?(contourColor), false);
				MatrixFrame frame2 = this.StrategicEntity.GetFrame();
				frame2.rotation.ApplyScaleLocal(agentVisualsData.ScaleData);
				weakEntity.SetFrame(ref frame2, true);
				this.HumanAgentVisuals.Tick(this.MountAgentVisuals, 0.0001f, this.IsEntityMovingVisually(), num3);
				weakEntity.Skeleton.ForceUpdateBoneFrames();
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00020344 File Offset: 0x0001E544
		private bool IsEntityMovingVisually()
		{
			if (base.MapEntity.IsMobile && base.MapEntity.MapEvent != null)
			{
				this._isEntityMovingCache = false;
			}
			else
			{
				if (Campaign.Current.CampaignDt <= 0f)
				{
					MobileParty mobileParty = base.MapEntity.MobileParty;
					if (mobileParty == null || !mobileParty.IsMainParty || !Campaign.Current.IsMainPartyWaiting)
					{
						goto IL_00AF;
					}
				}
				this._isEntityMovingCache = false;
				MobileParty mobileParty2 = base.MapEntity.MobileParty;
				if (mobileParty2 != null && !mobileParty2.VisualPosition2DWithoutError.NearlyEquals(this._lastFrameVisualPositionWithoutError, 1E-05f))
				{
					this._lastFrameVisualPositionWithoutError = base.MapEntity.MobileParty.VisualPosition2DWithoutError;
					this._isEntityMovingCache = true;
				}
			}
			IL_00AF:
			if (this._isInTransitionProgressCached)
			{
				this._isEntityMovingCache = true;
			}
			return this._isEntityMovingCache;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00020418 File Offset: 0x0001E618
		public static MetaMesh GetBannerOfCharacter(Banner banner, string bannerMeshName)
		{
			MetaMesh copy = MetaMesh.GetCopy(bannerMeshName, true, false);
			for (int i = 0; i < copy.MeshCount; i++)
			{
				Mesh meshAtIndex = copy.GetMeshAtIndex(i);
				if (!meshAtIndex.HasTag("dont_use_tableau"))
				{
					Material material = meshAtIndex.GetMaterial();
					Material tableauMaterial = null;
					Tuple<Material, Banner> tuple = new Tuple<Material, Banner>(material, banner);
					if (MapScreen.Instance.CharacterBannerMaterialCache.ContainsKey(tuple))
					{
						tableauMaterial = MapScreen.Instance.CharacterBannerMaterialCache[tuple];
					}
					else
					{
						tableauMaterial = material.CreateCopy();
						Action<Texture> action = delegate(Texture tex)
						{
							tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
							uint num = (uint)tableauMaterial.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
							ulong shaderFlags = tableauMaterial.GetShaderFlags();
							tableauMaterial.SetShaderFlags(shaderFlags | (ulong)num);
						};
						BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual("MobilePartyVisual");
						banner.GetTableauTextureLarge(in bannerDebugInfo, action);
						MapScreen.Instance.CharacterBannerMaterialCache[tuple] = tableauMaterial;
					}
					meshAtIndex.SetMaterial(tableauMaterial);
				}
			}
			return copy;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00020500 File Offset: 0x0001E700
		public void AddTentEntityForParty(GameEntity strategicEntity, PartyBase party, ref bool clearBannerComponentCache)
		{
			GameEntity gameEntity = GameEntity.CreateEmpty(strategicEntity.Scene, true, true, true);
			gameEntity.AddMultiMesh(MetaMesh.GetCopy("map_icon_siege_camp_tent", true, false), true);
			MatrixFrame identity = MatrixFrame.Identity;
			identity.rotation.ApplyScaleLocal(1.2f);
			gameEntity.SetFrame(ref identity, true);
			string text = null;
			Hero leaderHero = party.LeaderHero;
			if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
			{
				text = party.LeaderHero.ClanBanner.BannerCode;
			}
			bool flag = party.MobileParty.Army != null && party.MobileParty.Army.LeaderParty == party.MobileParty;
			MatrixFrame identity2 = MatrixFrame.Identity;
			identity2.origin.z = identity2.origin.z + (flag ? 0.2f : 0.15f);
			float num = MBMath.Map(party.CalculateCurrentStrength() / 500f * ((party.MobileParty.Army != null && flag) ? 1f : 0.8f), 0f, 1f, 0.15f, 0.5f);
			identity2.rotation.ApplyScaleLocal(num);
			if (!string.IsNullOrEmpty(text))
			{
				clearBannerComponentCache = false;
				string text2 = "campaign_flag";
				if (this._cachedBannerComponent.Item1 == text + text2)
				{
					this._cachedBannerComponent.Item2.GetFirstMetaMesh().Frame = identity2;
					gameEntity.AddComponent(this._cachedBannerComponent.Item2);
				}
				else
				{
					MetaMesh bannerOfCharacter = MobilePartyVisual.GetBannerOfCharacter(new Banner(text), text2);
					bannerOfCharacter.Frame = identity2;
					int componentCount = gameEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator);
					gameEntity.AddMultiMesh(bannerOfCharacter, true);
					if (gameEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > componentCount)
					{
						this._cachedBannerComponent.Item1 = text + text2;
						this._cachedBannerComponent.Item2 = gameEntity.GetComponentAtIndex(componentCount, GameEntity.ComponentType.ClothSimulator);
					}
				}
			}
			strategicEntity.AddChild(gameEntity, false);
			gameEntity.SetVisibilityExcludeParents(true);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000206DD File Offset: 0x0001E8DD
		internal void ClearVisualMemory()
		{
			this.ResetPartyIcon();
			base.MapEntity.SetVisualAsDirty();
			this._cachedBannerEntity = new ValueTuple<string, GameEntity>(null, null);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00020700 File Offset: 0x0001E900
		private void GetMeleeWeaponToWield(PartyBase party, out int wieldedItemIndex)
		{
			wieldedItemIndex = -1;
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(party);
			if (visualPartyLeader != null)
			{
				for (int i = 0; i < 5; i++)
				{
					if (visualPartyLeader.Equipment[i].Item != null && visualPartyLeader.Equipment[i].Item.PrimaryWeapon.IsMeleeWeapon)
					{
						wieldedItemIndex = i;
						return;
					}
				}
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00020760 File Offset: 0x0001E960
		private static void GetPartyBattleAnimation(PartyBase party, int wieldedItemIndex, out ActionIndexCache leaderAction, out ActionIndexCache mountAction)
		{
			leaderAction = ActionIndexCache.act_none;
			mountAction = ActionIndexCache.act_none;
			if (party.MobileParty.Army == null || !party.MobileParty.Army.DoesLeaderPartyAndAttachedPartiesContain(party.MobileParty))
			{
				MapEvent mapEvent = party.MapEvent;
			}
			else
			{
				MapEvent mapEvent2 = party.MobileParty.Army.LeaderParty.MapEvent;
			}
			CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(party);
			MapEvent mapEvent3 = party.MapEvent;
			if (((mapEvent3 != null) ? mapEvent3.MapEventSettlement : null) != null && visualPartyLeader != null && !visualPartyLeader.HasMount())
			{
				leaderAction = ActionIndexCache.act_map_raid;
				return;
			}
			if (wieldedItemIndex > -1 && ((visualPartyLeader != null) ? visualPartyLeader.Equipment[wieldedItemIndex].Item : null) != null)
			{
				WeaponComponent weaponComponent = visualPartyLeader.Equipment[wieldedItemIndex].Item.WeaponComponent;
				if (weaponComponent != null && weaponComponent.PrimaryWeapon.IsMeleeWeapon)
				{
					if (visualPartyLeader.HasMount())
					{
						if (visualPartyLeader.Equipment[10].Item.HorseComponent.Monster.MonsterUsage == "camel")
						{
							if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.OneHandedWeapon || weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.TwoHandedWeapon)
							{
								leaderAction = ActionIndexCache.act_map_rider_camel_attack_1h;
								mountAction = ActionIndexCache.act_map_mount_attack_1h;
							}
							else if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.Polearm)
							{
								if (weaponComponent.PrimaryWeapon.SwingDamageType == DamageTypes.Invalid)
								{
									leaderAction = ActionIndexCache.act_map_rider_camel_attack_1h_spear;
									mountAction = ActionIndexCache.act_map_mount_attack_spear;
								}
								else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm)
								{
									leaderAction = ActionIndexCache.act_map_rider_camel_attack_1h_swing;
									mountAction = ActionIndexCache.act_map_mount_attack_swing;
								}
								else
								{
									leaderAction = ActionIndexCache.act_map_rider_camel_attack_2h_swing;
									mountAction = ActionIndexCache.act_map_mount_attack_swing;
								}
							}
						}
						else if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.OneHandedWeapon || weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.TwoHandedWeapon)
						{
							leaderAction = ActionIndexCache.act_map_rider_horse_attack_1h;
							mountAction = ActionIndexCache.act_map_mount_attack_1h;
						}
						else if (weaponComponent.GetItemType() == ItemObject.ItemTypeEnum.Polearm)
						{
							if (weaponComponent.PrimaryWeapon.SwingDamageType == DamageTypes.Invalid)
							{
								leaderAction = ActionIndexCache.act_map_rider_horse_attack_1h_spear;
								mountAction = ActionIndexCache.act_map_mount_attack_spear;
							}
							else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm)
							{
								leaderAction = ActionIndexCache.act_map_rider_horse_attack_1h_swing;
								mountAction = ActionIndexCache.act_map_mount_attack_swing;
							}
							else
							{
								leaderAction = ActionIndexCache.act_map_rider_horse_attack_2h_swing;
								mountAction = ActionIndexCache.act_map_mount_attack_swing;
							}
						}
					}
					else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.Mace || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedSword)
					{
						leaderAction = ActionIndexCache.act_map_attack_1h;
					}
					else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedMace || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedSword)
					{
						leaderAction = ActionIndexCache.act_map_attack_2h;
					}
					else if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedPolearm || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm)
					{
						leaderAction = ActionIndexCache.act_map_attack_spear_1h_or_2h;
					}
				}
			}
			if (leaderAction == ActionIndexCache.act_none)
			{
				if (visualPartyLeader.HasMount())
				{
					HorseComponent horseComponent = visualPartyLeader.Equipment[10].Item.HorseComponent;
					leaderAction = ((horseComponent.Monster.MonsterUsage == "camel") ? ActionIndexCache.act_map_rider_camel_attack_unarmed : ActionIndexCache.act_map_rider_horse_attack_unarmed);
					mountAction = ActionIndexCache.act_map_mount_attack_unarmed;
					return;
				}
				leaderAction = ActionIndexCache.act_map_attack_unarmed;
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00020AE7 File Offset: 0x0001ECE7
		private void GetMountAndHarnessVisualIdsForPartyIcon(out string mountStringId, out string harnessStringId)
		{
			mountStringId = "";
			harnessStringId = "";
			if (base.MapEntity.IsMobile)
			{
				PartyComponent partyComponent = base.MapEntity.MobileParty.PartyComponent;
				if (partyComponent == null)
				{
					return;
				}
				partyComponent.GetMountAndHarnessVisualIdsForPartyIcon(base.MapEntity, out mountStringId, out harnessStringId);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00020B28 File Offset: 0x0001ED28
		private void InitializePartyCollider(PartyBase party)
		{
			if (this.StrategicEntity != null && party.IsMobile)
			{
				this.StrategicEntity.AddSphereAsBody(new Vec3(0f, 0f, 0f, -1f), 0.5f, BodyFlags.Moveable | BodyFlags.OnlyCollideWithRaycast);
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00020B7C File Offset: 0x0001ED7C
		private void ResetPartyIcon()
		{
			if (this.HumanAgentVisuals != null)
			{
				this.HumanAgentVisuals.Reset();
				this.HumanAgentVisuals = null;
			}
			if (this.MountAgentVisuals != null)
			{
				this.MountAgentVisuals.Reset();
				this.MountAgentVisuals = null;
			}
			if (this.CaravanMountAgentVisuals != null)
			{
				this.CaravanMountAgentVisuals.Reset();
				this.CaravanMountAgentVisuals = null;
			}
			if (this.StrategicEntity != null)
			{
				if ((this.StrategicEntity.EntityFlags & EntityFlags.Ignore) != (EntityFlags)0U)
				{
					this.StrategicEntity.RemoveFromPredisplayEntity();
				}
				this.StrategicEntity.ClearComponents();
			}
			this._bearingRotation = base.MapEntity.MobileParty.Bearing.RotationInRadians;
			MobilePartyVisualManager.Current.UnRegisterFadingVisual(this);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00020C38 File Offset: 0x0001EE38
		private float GetTransitionProgress()
		{
			if (this.IsMobileEntity && base.MapEntity.MobileParty.IsTransitionInProgress && base.MapEntity.MobileParty.NavigationTransitionDuration != CampaignTime.Zero)
			{
				float num = (float)base.MapEntity.MobileParty.NavigationTransitionDuration.ToHours;
				Army army = base.MapEntity.MobileParty.Army;
				if (((army != null) ? army.LeaderParty : null) == base.MapEntity.MobileParty && base.MapEntity.MobileParty.AttachedParties.Count > 0)
				{
					float num2 = base.MapEntity.MobileParty.AttachedParties.MaxQ<MobileParty>((MobileParty x) => (float)x.NavigationTransitionDuration.ToHours);
					num = Math.Max(num, num2);
				}
				return MBMath.ClampFloat(base.MapEntity.MobileParty.NavigationTransitionStartTime.ElapsedHoursUntilNow / num, 0f, 1f);
			}
			return 1f;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00020D4C File Offset: 0x0001EF4C
		private void OnTransitionStarted()
		{
			MobilePartyVisualManager.Current.RegisterFadingVisual(this);
			this._transitionStartRotation = (base.MapEntity.MobileParty.EndPositionForNavigationTransition.ToVec2() - base.MapEntity.Position.ToVec2()).RotationInRadians;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00020DA2 File Offset: 0x0001EFA2
		private void OnTransitionEnded()
		{
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00020DA4 File Offset: 0x0001EFA4
		private float GetVisualRotation()
		{
			if (base.MapEntity.IsMobile && base.MapEntity.MapEvent != null && base.MapEntity.MapEvent.IsFieldBattle)
			{
				return this.GetMapEventVisualRotation();
			}
			if (base.MapEntity.IsMobile && base.MapEntity.MobileParty.IsTransitionInProgress)
			{
				return this._transitionStartRotation;
			}
			return this._bearingRotation;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00020E10 File Offset: 0x0001F010
		private float GetMapEventVisualRotation()
		{
			if (base.MapEntity.MapEventSide.OtherSide.LeaderParty != null && base.MapEntity.MapEventSide.OtherSide.LeaderParty.IsMobile && base.MapEntity.MapEventSide.OtherSide.LeaderParty.IsMobile)
			{
				return (base.MapEntity.MapEventSide.OtherSide.LeaderParty.MobileParty.VisualPosition2DWithoutError - base.MapEntity.MobileParty.VisualPosition2DWithoutError).Normalized().RotationInRadians;
			}
			return this._bearingRotation;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00020EB7 File Offset: 0x0001F0B7
		private void AddVisualToVisualsOfEntities()
		{
			if (!MapScreen.VisualsOfEntities.ContainsKey(this.StrategicEntity.Pointer))
			{
				MapScreen.VisualsOfEntities.Add(this.StrategicEntity.Pointer, this);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00020EE8 File Offset: 0x0001F0E8
		private void RemoveVisualFromVisualsOfEntities()
		{
			MapScreen.VisualsOfEntities.Remove(this.StrategicEntity.Pointer);
			foreach (GameEntity gameEntity in this.StrategicEntity.GetChildren())
			{
				MapScreen.VisualsOfEntities.Remove(gameEntity.Pointer);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00020F5C File Offset: 0x0001F15C
		private bool IsPartOfBesiegerCamp(PartyBase party)
		{
			Settlement besiegedSettlement = party.MobileParty.BesiegedSettlement;
			return ((besiegedSettlement != null) ? besiegedSettlement.SiegeEvent : null) != null && party.MobileParty.BesiegedSettlement.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(party, MapEvent.BattleTypes.Siege);
		}

		// Token: 0x040001F0 RID: 496
		private const float PartyScale = 0.3f;

		// Token: 0x040001F1 RID: 497
		private const float HorseAnimationSpeedFactor = 1.3f;

		// Token: 0x040001F2 RID: 498
		private float _speed;

		// Token: 0x040001F3 RID: 499
		private float _entityAlpha;

		// Token: 0x040001F4 RID: 500
		private float _transitionStartRotation;

		// Token: 0x040001F5 RID: 501
		private Vec2 _lastFrameVisualPositionWithoutError;

		// Token: 0x040001F6 RID: 502
		private bool _isEntityMovingCache;

		// Token: 0x040001F7 RID: 503
		private bool _isInTransitionProgressCached;

		// Token: 0x040001F8 RID: 504
		private float _bearingRotation;

		// Token: 0x040001F9 RID: 505
		private ValueTuple<string, GameEntityComponent> _cachedBannerComponent;

		// Token: 0x040001FA RID: 506
		private ValueTuple<string, GameEntity> _cachedBannerEntity;

		// Token: 0x040001FB RID: 507
		private Scene _mapScene;
	}
}
