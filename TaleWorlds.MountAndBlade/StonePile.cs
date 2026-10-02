using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037A RID: 890
	public class StonePile : UsableMachine, IDetachment
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06003292 RID: 12946 RVA: 0x000CEE2A File Offset: 0x000CD02A
		// (set) Token: 0x06003293 RID: 12947 RVA: 0x000CEE32 File Offset: 0x000CD032
		public int AmmoCount { get; protected set; }

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003294 RID: 12948 RVA: 0x000CEE3C File Offset: 0x000CD03C
		public bool HasThrowingPointUsed
		{
			get
			{
				foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
				{
					if (throwingPoint.StandingPoint.HasUser || throwingPoint.StandingPoint.HasAIMovingTo || (throwingPoint.WaitingPoint != null && (throwingPoint.WaitingPoint.HasUser || throwingPoint.WaitingPoint.HasAIMovingTo)))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003295 RID: 12949 RVA: 0x000CEECC File Offset: 0x000CD0CC
		public virtual BattleSideEnum Side
		{
			get
			{
				return BattleSideEnum.Defender;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06003296 RID: 12950 RVA: 0x000CEECF File Offset: 0x000CD0CF
		public override int MaxUserCount
		{
			get
			{
				return this._throwingPoints.Count;
			}
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000CEEDC File Offset: 0x000CD0DC
		protected StonePile()
		{
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x000CEF04 File Offset: 0x000CD104
		protected void ConsumeAmmo()
		{
			int ammoCount = this.AmmoCount;
			this.AmmoCount = ammoCount - 1;
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SetStonePileAmmo(base.Id, this.AmmoCount));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.UpdateAmmoMesh();
			this.CheckAmmo();
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x000CEF57 File Offset: 0x000CD157
		public void SetAmmo(int ammoLeft)
		{
			if (this.AmmoCount != ammoLeft)
			{
				this.AmmoCount = ammoLeft;
				this.UpdateAmmoMesh();
				this.CheckAmmo();
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x000CEF78 File Offset: 0x000CD178
		protected virtual void CheckAmmo()
		{
			if (this.AmmoCount <= 0)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.IsDeactivated = true;
				}
			}
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x000CEFD4 File Offset: 0x000CD1D4
		protected internal override void OnInit()
		{
			base.OnInit();
			this._tickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.5f + MBRandom.RandomFloat * 0.5f, true);
			this._givenItem = Game.Current.ObjectManager.GetObject<ItemObject>(this.GivenItemID);
			MBList<VolumeBox> mblist = base.GameEntity.CollectScriptComponentsIncludingChildrenRecursive<VolumeBox>();
			this._throwingPoints = new List<StonePile.ThrowingPoint>();
			this._volumeBoxTimerPairs = new List<StonePile.VolumeBoxTimerPair>();
			foreach (StandingPointWithWeaponRequirement standingPointWithWeaponRequirement in base.StandingPoints.OfType<StandingPointWithWeaponRequirement>())
			{
				if (standingPointWithWeaponRequirement.GameEntity.HasTag(this.AmmoPickUpTag))
				{
					standingPointWithWeaponRequirement.InitGivenWeapon(this._givenItem);
					standingPointWithWeaponRequirement.SetHasAlternative(true);
					standingPointWithWeaponRequirement.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
				}
				else if (standingPointWithWeaponRequirement.GameEntity.HasTag("throwing"))
				{
					standingPointWithWeaponRequirement.InitRequiredWeapon(this._givenItem);
					StonePile.ThrowingPoint throwingPoint = new StonePile.ThrowingPoint();
					throwingPoint.StandingPoint = standingPointWithWeaponRequirement as StandingPointWithVolumeBox;
					throwingPoint.AmmoPickUpPoint = null;
					throwingPoint.AttackEntity = null;
					throwingPoint.AttackEntityNearbyAgentsCheckRadius = 0f;
					List<StandingPointWithWeaponRequirement> list = standingPointWithWeaponRequirement.GameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<StandingPointWithWeaponRequirement>("wait_to_throw");
					if (list != null && list.Count > 0)
					{
						throwingPoint.WaitingPoint = list[0];
						throwingPoint.WaitingPoint.InitRequiredWeapon(this._givenItem);
					}
					else
					{
						throwingPoint.WaitingPoint = null;
					}
					bool flag = false;
					int num = 0;
					while (num < this._volumeBoxTimerPairs.Count && !flag)
					{
						if (this._volumeBoxTimerPairs[num].VolumeBox.GameEntity.HasTag(throwingPoint.StandingPoint.VolumeBoxTag))
						{
							throwingPoint.EnemyInRangeTimer = this._volumeBoxTimerPairs[num].Timer;
							flag = true;
						}
						num++;
					}
					if (!flag)
					{
						VolumeBox volumeBox = mblist.FirstOrDefault<VolumeBox>((VolumeBox vb) => vb.GameEntity.HasTag(throwingPoint.StandingPoint.VolumeBoxTag));
						StonePile.VolumeBoxTimerPair volumeBoxTimerPair = default(StonePile.VolumeBoxTimerPair);
						volumeBoxTimerPair.VolumeBox = volumeBox;
						volumeBoxTimerPair.Timer = new Timer(-3.5f, 0.5f, false);
						throwingPoint.EnemyInRangeTimer = volumeBoxTimerPair.Timer;
						this._volumeBoxTimerPairs.Add(volumeBoxTimerPair);
					}
					this._throwingPoints.Add(throwingPoint);
				}
			}
			this.EnemyRangeToStopUsing = 5f;
			this.AmmoCount = this.StartingAmmoCount;
			this.UpdateAmmoMesh();
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this._throwingTargets = base.Scene.FindEntitiesWithTag("throwing_target").ToList<GameEntity>();
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x000CF2D4 File Offset: 0x000CD4D4
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			this.AmmoCount = this.StartingAmmoCount;
			this.UpdateAmmoMesh();
			foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
			{
				standingPoint.IsDeactivated = false;
			}
			foreach (StonePile.VolumeBoxTimerPair volumeBoxTimerPair in this._volumeBoxTimerPairs)
			{
				volumeBoxTimerPair.Timer.Reset(-3.5f);
			}
			foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
			{
				throwingPoint.AmmoPickUpPoint = null;
			}
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x000CF3C8 File Offset: 0x000CD5C8
		public override void AfterMissionStart()
		{
			if (base.AmmoPickUpPoints != null)
			{
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					standingPoint.LockUserFrames = true;
				}
			}
			if (this._throwingPoints != null)
			{
				foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
				{
					throwingPoint.StandingPoint.IsDisabledForPlayers = true;
					throwingPoint.StandingPoint.LockUserFrames = false;
					throwingPoint.StandingPoint.LockUserPositions = true;
				}
			}
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x000CF488 File Offset: 0x000CD688
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			if (usableGameObject.GameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=jfcceEoE}{PILE_TYPE} Pile", null);
				textObject.SetTextVariable("PILE_TYPE", new TextObject("{=1CPdu9K0}Stone", null));
				return textObject;
			}
			return null;
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x000CF4D0 File Offset: 0x000CD6D0
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (gameEntity.IsValid && gameEntity.HasTag(this.AmmoPickUpTag))
			{
				TextObject textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
				textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
				return textObject;
			}
			return null;
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x000CF524 File Offset: 0x000CD724
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new StonePileAI(this);
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x000CF52C File Offset: 0x000CD72C
		public override bool IsInRangeToCheckAlternativePoints(Agent agent)
		{
			float num = ((base.StandingPoints.Count > 0) ? (agent.GetInteractionDistanceToUsable(base.StandingPoints[0]) + 2f) : 2f);
			return base.GameEntity.GlobalPosition.DistanceSquared(agent.Position) < num * num;
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x000CF588 File Offset: 0x000CD788
		public override StandingPoint GetBestPointAlternativeTo(StandingPoint standingPoint, Agent agent)
		{
			if (base.AmmoPickUpPoints.Contains(standingPoint))
			{
				float num = standingPoint.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
				StandingPoint standingPoint2 = standingPoint;
				foreach (StandingPoint standingPoint3 in base.AmmoPickUpPoints)
				{
					float num2 = standingPoint3.GameEntity.GlobalPosition.DistanceSquared(agent.Position);
					if (num2 < num && ((!standingPoint3.HasUser && !standingPoint3.HasAIMovingTo) || standingPoint3.IsInstantUse) && !standingPoint3.IsDeactivated && !standingPoint3.IsDisabledForAgent(agent))
					{
						num = num2;
						standingPoint2 = standingPoint3;
					}
				}
				return standingPoint2;
			}
			return standingPoint;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x000CF664 File Offset: 0x000CD864
		private void TickOccasionally()
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.AmmoCount <= 0 && !this.HasThrowingPointUsed)
				{
					this.ReleaseAllUserAgentsAndFormations(BattleSideEnum.None, true);
					return;
				}
				if (this.IsDisabledForBattleSideAI(this.Side))
				{
					this.ReleaseAllUserAgentsAndFormations(this.Side, false);
					return;
				}
				bool flag = this._volumeBoxTimerPairs.Count == 0;
				foreach (StonePile.VolumeBoxTimerPair volumeBoxTimerPair in this._volumeBoxTimerPairs)
				{
					if (volumeBoxTimerPair.VolumeBox.HasAgentsInAttackerSide())
					{
						flag = true;
						if (volumeBoxTimerPair.Timer.ElapsedTime() > 3.5f)
						{
							volumeBoxTimerPair.Timer.Reset(Mission.Current.CurrentTime);
						}
						else
						{
							volumeBoxTimerPair.Timer.Reset(Mission.Current.CurrentTime - 0.5f);
						}
					}
				}
				MBReadOnlyList<Formation> userFormations = base.UserFormations;
				if (flag && userFormations.CountQ<Formation>((Formation f) => f.Team.Side == this.Side) == 0)
				{
					float minDistanceSquared = float.MaxValue;
					Formation bestFormation = null;
					foreach (Team team in Mission.Current.Teams)
					{
						if (team.Side == this.Side)
						{
							using (List<Formation>.Enumerator enumerator3 = team.FormationsIncludingEmpty.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									Formation formation = enumerator3.Current;
									if (formation.CountOfUnits > 0 && formation.CountOfUnitsWithoutLooseDetachedOnes >= this.MaxUserCount && formation.CountOfUnitsWithoutLooseDetachedOnes > 0)
									{
										formation.ApplyActionOnEachUnit(delegate(Agent agent)
										{
											float num = agent.Position.DistanceSquared(this.GameEntity.GlobalPosition);
											if (minDistanceSquared > num)
											{
												minDistanceSquared = num;
												bestFormation = formation;
											}
										}, null);
									}
								}
							}
						}
					}
					Formation bestFormation2 = bestFormation;
					if (bestFormation2 == null)
					{
						return;
					}
					bestFormation2.StartUsingMachine(this, false);
					return;
				}
				else if (!flag)
				{
					if (userFormations.Count > 0)
					{
						this.ReleaseAllUserAgentsAndFormations(BattleSideEnum.None, true);
						return;
					}
				}
				else
				{
					if (userFormations.All<Formation>((Formation f) => f.Team.Side == this.Side && f.UnitsWithoutLooseDetachedOnes.Count == 0))
					{
						if (base.StandingPoints.Count<StandingPoint>((StandingPoint sp) => sp.HasUser || sp.HasAIMovingTo) == 0)
						{
							this.ReleaseAllUserAgentsAndFormations(BattleSideEnum.None, true);
							return;
						}
					}
					this.UpdateThrowingPointAttackEntities();
				}
			}
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x000CF90C File Offset: 0x000CDB0C
		private void ReleaseAllUserAgentsAndFormations(BattleSideEnum sideFilterForAIControlledAgents, bool disableForNonAIControlledAgents)
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				Agent agent = (standingPoint.HasUser ? standingPoint.UserAgent : (standingPoint.HasAIMovingTo ? standingPoint.MovingAgent : null));
				if (agent != null)
				{
					if (agent.IsAIControlled)
					{
						if (sideFilterForAIControlledAgents == BattleSideEnum.None)
						{
							goto IL_006E;
						}
						Team team = agent.Team;
						if (team != null && team.Side == sideFilterForAIControlledAgents)
						{
							goto IL_006E;
						}
					}
					if (agent.IsAIControlled || !disableForNonAIControlledAgents)
					{
						continue;
					}
					IL_006E:
					if (agent.GetPrimaryWieldedItemIndex() == EquipmentIndex.ExtraWeaponSlot && agent.Equipment[EquipmentIndex.ExtraWeaponSlot].Item == this._givenItem)
					{
						agent.DropItem(EquipmentIndex.ExtraWeaponSlot, WeaponClass.Undefined);
					}
					base.Ai.StopUsingStandingPoint(standingPoint);
				}
			}
			MBReadOnlyList<Formation> userFormations = base.UserFormations;
			for (int i = userFormations.Count - 1; i >= 0; i--)
			{
				Formation formation = userFormations[i];
				if (formation.Team.Side == this.Side)
				{
					formation.StopUsingMachine(this, false);
				}
			}
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x000CFA34 File Offset: 0x000CDC34
		private void UpdateThrowingPointAttackEntities()
		{
			bool flag = false;
			List<WeakGameEntity> list = null;
			foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
			{
				if (throwingPoint.StandingPoint.HasAIUser)
				{
					if (!flag)
					{
						list = this.GetEnemySiegeWeapons();
						flag = true;
						if (list == null)
						{
							foreach (StonePile.ThrowingPoint throwingPoint2 in this._throwingPoints)
							{
								throwingPoint2.AttackEntity = null;
								throwingPoint2.AttackEntityNearbyAgentsCheckRadius = 0f;
							}
							if (this._throwingTargets.Count == 0)
							{
								break;
							}
						}
					}
					Agent userAgent = throwingPoint.StandingPoint.UserAgent;
					GameEntity attackEntity = throwingPoint.AttackEntity;
					if (attackEntity != null)
					{
						bool flag2 = false;
						if (!this.CanShootAtEntity(userAgent, attackEntity.WeakEntity, false))
						{
							flag2 = true;
						}
						else if (this._throwingTargets.Contains(attackEntity))
						{
							flag2 = !throwingPoint.CanUseAttackEntity();
						}
						else if (!list.Contains(attackEntity.WeakEntity))
						{
							flag2 = true;
						}
						if (flag2)
						{
							throwingPoint.AttackEntity = null;
							throwingPoint.AttackEntityNearbyAgentsCheckRadius = 0f;
						}
					}
					if (!(throwingPoint.AttackEntity == null))
					{
						continue;
					}
					bool flag3 = false;
					if (this._throwingTargets.Count > 0)
					{
						foreach (GameEntity gameEntity in this._throwingTargets)
						{
							if (attackEntity != gameEntity && this.CanShootAtEntity(userAgent, gameEntity.WeakEntity, true))
							{
								throwingPoint.AttackEntity = gameEntity;
								throwingPoint.AttackEntityNearbyAgentsCheckRadius = 1.31f;
								flag3 = true;
								break;
							}
						}
					}
					if (flag3 || list == null)
					{
						continue;
					}
					using (List<WeakGameEntity>.Enumerator enumerator4 = list.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							WeakGameEntity weakGameEntity = enumerator4.Current;
							if (attackEntity != weakGameEntity && this.CanShootAtEntity(userAgent, weakGameEntity, false))
							{
								throwingPoint.AttackEntity = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
								throwingPoint.AttackEntityNearbyAgentsCheckRadius = 0f;
								break;
							}
						}
						continue;
					}
				}
				throwingPoint.AttackEntity = null;
			}
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x000CFCC0 File Offset: 0x000CDEC0
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x000CFCCC File Offset: 0x000CDECC
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this._tickOccasionallyTimer.Check(Mission.Current.CurrentTime))
				{
					this.TickOccasionally();
				}
				StandingPoint.StackArray8StandingPoint stackArray8StandingPoint = default(StandingPoint.StackArray8StandingPoint);
				int num = 0;
				Agent.StackArray8Agent stackArray8Agent = default(Agent.StackArray8Agent);
				int num2 = 0;
				foreach (StandingPoint standingPoint in base.AmmoPickUpPoints)
				{
					if (standingPoint.HasUser)
					{
						ActionIndexCache currentAction = standingPoint.UserAgent.GetCurrentAction(1);
						if (!(currentAction == ActionIndexCache.act_pickup_boulder_begin))
						{
							if (currentAction == ActionIndexCache.act_pickup_boulder_end)
							{
								MissionWeapon missionWeapon = new MissionWeapon(this._givenItem, null, null, 1);
								Agent userAgent = standingPoint.UserAgent;
								userAgent.EquipWeaponToExtraSlotAndWield(ref missionWeapon);
								base.Ai.StopUsingStandingPoint(standingPoint);
								this.ConsumeAmmo();
								if (userAgent.IsAIControlled)
								{
									stackArray8Agent[num2++] = userAgent;
								}
							}
							else if (!standingPoint.UserAgent.SetActionChannel(1, in ActionIndexCache.act_pickup_boulder_begin, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
							{
								base.Ai.StopUsingStandingPoint(standingPoint);
							}
						}
					}
					if (standingPoint.HasAIUser || standingPoint.HasAIMovingTo)
					{
						stackArray8StandingPoint[num++] = standingPoint;
					}
				}
				StonePile.ThrowingPoint.StackArray8ThrowingPoint stackArray8ThrowingPoint = default(StonePile.ThrowingPoint.StackArray8ThrowingPoint);
				int num3 = 0;
				foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
				{
					throwingPoint.AmmoPickUpPoint = null;
					if (throwingPoint.AttackEntity != null || (throwingPoint.EnemyInRangeTimer.Check(Mission.Current.CurrentTime) && throwingPoint.EnemyInRangeTimer.ElapsedTime() < 3.5f))
					{
						if (!this.UpdateThrowingPointIfHasAnyInteractingAgent(throwingPoint))
						{
							stackArray8ThrowingPoint[num3++] = throwingPoint;
						}
					}
					else
					{
						throwingPoint.StandingPoint.IsDeactivated = true;
						if (throwingPoint.WaitingPoint != null)
						{
							throwingPoint.WaitingPoint.IsDeactivated = true;
						}
					}
				}
				for (int i = 0; i < num; i++)
				{
					if (num3 > i)
					{
						StandingPointWithWeaponRequirement standingPointWithWeaponRequirement = stackArray8StandingPoint[i] as StandingPointWithWeaponRequirement;
						stackArray8ThrowingPoint[i].AmmoPickUpPoint = standingPointWithWeaponRequirement;
					}
					else if (stackArray8StandingPoint[i].HasUser || stackArray8StandingPoint[i].HasAIMovingTo)
					{
						base.Ai.StopUsingStandingPoint(stackArray8StandingPoint[i]);
					}
				}
				for (int j = 0; j < num2; j++)
				{
					Agent agent = stackArray8Agent[j];
					StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(this.Side, agent, null, null);
					this.AssignAgentToStandingPoint(suitableStandingPointFor, agent);
				}
			}
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x000CFFDC File Offset: 0x000CE1DC
		private bool ShouldStandAtWaitingPoint(StonePile.ThrowingPoint throwingPoint)
		{
			bool flag = false;
			if (throwingPoint.WaitingPoint != null)
			{
				flag = true;
				Vec2 asVec = throwingPoint.StandingPoint.GameEntity.GlobalPosition.AsVec2;
				if (AgentProximityMap.CanSearchRadius(this._givenItemRange))
				{
					AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, asVec, this._givenItemRange, false);
					while (proximityMapSearchStruct.LastFoundAgent != null)
					{
						if (proximityMapSearchStruct.LastFoundAgent.State == AgentState.Active && proximityMapSearchStruct.LastFoundAgent.Team != null && proximityMapSearchStruct.LastFoundAgent.Team.Side == BattleSideEnum.Attacker)
						{
							flag = false;
							break;
						}
						AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
					}
				}
				else
				{
					float num = this._givenItemRange * this._givenItemRange;
					if (Mission.Current.AttackerTeam != null)
					{
						MBReadOnlyList<Agent> activeAgents = Mission.Current.AttackerTeam.ActiveAgents;
						int count = activeAgents.Count;
						for (int i = 0; i < count; i++)
						{
							if (activeAgents[i].Position.AsVec2.DistanceSquared(asVec) <= num)
							{
								flag = false;
								break;
							}
						}
					}
					if (Mission.Current.AttackerAllyTeam != null)
					{
						MBReadOnlyList<Agent> activeAgents2 = Mission.Current.AttackerAllyTeam.ActiveAgents;
						int count2 = activeAgents2.Count;
						for (int j = 0; j < count2; j++)
						{
							if (activeAgents2[j].Position.AsVec2.DistanceSquared(asVec) <= num)
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x000D0158 File Offset: 0x000CE358
		private bool UpdateThrowingPointIfHasAnyInteractingAgent(StonePile.ThrowingPoint throwingPoint)
		{
			Agent agent = null;
			StandingPoint standingPoint = null;
			throwingPoint.StandingPoint.IsDeactivated = false;
			if (throwingPoint.StandingPoint.HasAIMovingTo)
			{
				agent = throwingPoint.StandingPoint.MovingAgent;
				standingPoint = throwingPoint.StandingPoint;
			}
			else if (throwingPoint.StandingPoint.HasUser)
			{
				agent = throwingPoint.StandingPoint.UserAgent;
				standingPoint = throwingPoint.StandingPoint;
			}
			if (throwingPoint.WaitingPoint != null)
			{
				throwingPoint.WaitingPoint.IsDeactivated = false;
				if (throwingPoint.WaitingPoint.HasAIMovingTo)
				{
					agent = throwingPoint.WaitingPoint.MovingAgent;
					standingPoint = throwingPoint.WaitingPoint;
				}
				else if (throwingPoint.WaitingPoint.HasUser)
				{
					agent = throwingPoint.WaitingPoint.UserAgent;
					standingPoint = throwingPoint.WaitingPoint;
				}
			}
			bool flag = agent != null;
			if (flag && agent.Controller == AgentControllerType.AI)
			{
				EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
				if (primaryWieldedItemIndex == EquipmentIndex.None || agent.Equipment[primaryWieldedItemIndex].Item != this._givenItem)
				{
					base.Ai.StopUsingStandingPoint(standingPoint);
					throwingPoint.AttackEntity = null;
					return flag;
				}
				if (standingPoint == throwingPoint.WaitingPoint)
				{
					if (!this.ShouldStandAtWaitingPoint(throwingPoint))
					{
						base.Ai.StopUsingStandingPoint(standingPoint);
						this.AssignAgentToStandingPoint(throwingPoint.StandingPoint, agent);
						return flag;
					}
				}
				else if (agent.IsUsingGameObject && throwingPoint.AttackEntity != null)
				{
					if (throwingPoint.CanUseAttackEntity())
					{
						agent.SetScriptedTargetEntity(throwingPoint.AttackEntity.WeakEntity, Agent.AISpecialCombatModeFlags.None, true);
						return flag;
					}
					agent.DisableScriptedCombatMovement();
					throwingPoint.AttackEntity = null;
					return flag;
				}
				else if (this.ShouldStandAtWaitingPoint(throwingPoint))
				{
					base.Ai.StopUsingStandingPoint(standingPoint);
					this.AssignAgentToStandingPoint(throwingPoint.WaitingPoint, agent);
				}
			}
			return flag;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x000D02EE File Offset: 0x000CE4EE
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteIntToPacket(this.AmmoCount, CompressionMission.RangedSiegeWeaponAmmoCompressionInfo);
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x000D0308 File Offset: 0x000CE508
		float? IDetachment.GetWeightOfAgentAtNextSlot(List<ValueTuple<Agent, float>> candidates, out Agent match)
		{
			BattleSideEnum side = candidates[0].Item1.Team.Side;
			StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(side, null, null, candidates);
			if (suitableStandingPointFor == null)
			{
				match = null;
				return null;
			}
			float? weightOfNextSlot = ((IDetachment)this).GetWeightOfNextSlot(side);
			match = StonePileAI.GetSuitableAgentForStandingPoint(this, suitableStandingPointFor, candidates, new List<Agent>(), weightOfNextSlot.Value);
			if (match == null)
			{
				return null;
			}
			float num = 1f;
			float? num2 = weightOfNextSlot;
			float num3 = num;
			if (num2 == null)
			{
				return null;
			}
			return new float?(num2.GetValueOrDefault() * num3);
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x000D03A0 File Offset: 0x000CE5A0
		float? IDetachment.GetWeightOfAgentAtNextSlot(List<Agent> candidates, out Agent match)
		{
			BattleSideEnum side = candidates[0].Team.Side;
			StandingPoint suitableStandingPointFor = this.GetSuitableStandingPointFor(side, null, candidates, null);
			if (suitableStandingPointFor == null)
			{
				match = null;
				return null;
			}
			match = StonePileAI.GetSuitableAgentForStandingPoint(this, suitableStandingPointFor, candidates, new List<Agent>());
			if (match == null)
			{
				return null;
			}
			float? weightOfNextSlot = ((IDetachment)this).GetWeightOfNextSlot(side);
			float num = 1f;
			float? num2 = weightOfNextSlot;
			float num3 = num;
			if (num2 == null)
			{
				return null;
			}
			return new float?(num2.GetValueOrDefault() * num3);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x000D042C File Offset: 0x000CE62C
		protected override StandingPoint GetSuitableStandingPointFor(BattleSideEnum side, Agent agent = null, List<Agent> agents = null, List<ValueTuple<Agent, float>> agentValuePairs = null)
		{
			List<Agent> list = new List<Agent>();
			if (agents == null)
			{
				if (agent != null)
				{
					list.Add(agent);
					goto IL_005A;
				}
				if (agentValuePairs == null)
				{
					goto IL_005A;
				}
				using (List<ValueTuple<Agent, float>>.Enumerator enumerator = agentValuePairs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<Agent, float> valueTuple = enumerator.Current;
						list.Add(valueTuple.Item1);
					}
					goto IL_005A;
				}
			}
			list.AddRange(agents);
			IL_005A:
			bool flag = false;
			bool flag2 = false;
			StandingPoint standingPoint = null;
			int num = 0;
			while (num < this._throwingPoints.Count && (standingPoint == null || flag2))
			{
				StonePile.ThrowingPoint throwingPoint = this._throwingPoints[num];
				if (this.IsThrowingPointAssignable(throwingPoint))
				{
					StandingPoint standingPoint2 = throwingPoint.StandingPoint;
					bool flag3 = this.ShouldStandAtWaitingPoint(throwingPoint);
					if (flag3)
					{
						standingPoint2 = throwingPoint.WaitingPoint;
					}
					bool flag4 = false;
					int num2 = 0;
					while (!flag4 && num2 < list.Count)
					{
						flag4 = !standingPoint2.IsDisabledForAgent(list[num2]);
						num2++;
					}
					if (flag4)
					{
						flag2 = flag3;
						standingPoint = standingPoint2;
					}
					else
					{
						flag = true;
					}
				}
				num++;
			}
			int num3 = 0;
			while (num3 < base.StandingPoints.Count && standingPoint == null)
			{
				StandingPoint standingPoint3 = base.StandingPoints[num3];
				if (!standingPoint3.IsDeactivated && (standingPoint3.IsInstantUse || (!standingPoint3.HasUser && !standingPoint3.HasAIMovingTo)) && !standingPoint3.GameEntity.HasTag("throwing") && !standingPoint3.GameEntity.HasTag("wait_to_throw") && (flag || !standingPoint3.GameEntity.HasTag(this.AmmoPickUpTag)))
				{
					int num4 = 0;
					while (num4 < list.Count && standingPoint == null)
					{
						if (!standingPoint3.IsDisabledForAgent(list[num4]))
						{
							standingPoint = standingPoint3;
						}
						num4++;
					}
					if (list.Count == 0)
					{
						standingPoint = standingPoint3;
					}
				}
				num3++;
			}
			return standingPoint;
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000D0628 File Offset: 0x000CE828
		protected override float GetDetachmentWeightAux(BattleSideEnum side)
		{
			if (this.IsDisabledForBattleSideAI(side))
			{
				return float.MinValue;
			}
			this.UsableStandingPoints.Clear();
			int num = 0;
			foreach (StonePile.ThrowingPoint throwingPoint in this._throwingPoints)
			{
				if (this.IsThrowingPointAssignable(throwingPoint))
				{
					num++;
				}
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint = base.StandingPoints[i];
				if (standingPoint.GameEntity.HasTag(this.AmmoPickUpTag) && num > 0)
				{
					num--;
					if (standingPoint.IsUsableBySide(side))
					{
						if (!standingPoint.HasAIMovingTo)
						{
							if (!flag2)
							{
								this.UsableStandingPoints.Clear();
							}
							flag2 = true;
						}
						else if (flag2 || standingPoint.MovingAgent.Formation.Team.Side != side)
						{
							goto IL_00EC;
						}
						flag = true;
						this.UsableStandingPoints.Add(new ValueTuple<int, StandingPoint>(i, standingPoint));
					}
				}
				IL_00EC:;
			}
			this.AreUsableStandingPointsVacant = flag2;
			if (!flag)
			{
				return float.MinValue;
			}
			if (flag2)
			{
				return 1f;
			}
			if (!base.IsDetachmentRecentlyEvaluated)
			{
				return 0.1f;
			}
			return 0.01f;
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000D0778 File Offset: 0x000CE978
		protected virtual void UpdateAmmoMesh()
		{
			int num = 20 - this.AmmoCount;
			if (base.GameEntity.IsValid)
			{
				for (int i = 0; i < base.GameEntity.MultiMeshComponentCount; i++)
				{
					MetaMesh metaMesh = base.GameEntity.GetMetaMesh(i);
					for (int j = 0; j < metaMesh.MeshCount; j++)
					{
						metaMesh.GetMeshAtIndex(j).SetVectorArgument(0f, (float)num, 0f, 0f);
					}
				}
			}
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x000D07FC File Offset: 0x000CE9FC
		private bool CanShootAtEntity(Agent agent, WeakGameEntity entity, bool canShootEvenIfRayCastHitsNothing = false)
		{
			bool flag = false;
			Vec3 eyeGlobalPosition = agent.GetEyeGlobalPosition();
			Vec3 globalPosition = entity.GlobalPosition;
			Vec3 vec = eyeGlobalPosition - globalPosition;
			float num = vec.Normalize();
			if (num > 1E-05f && MathF.Abs(vec.z) < MathF.Cos(0.2617994f) && num < this._givenItemRange)
			{
				float num2;
				WeakGameEntity parent;
				if (base.Scene.RayCastForClosestEntityOrTerrain(agent.GetEyeGlobalPosition(), entity.GlobalPosition, out num2, out parent, 0.01f, BodyFlags.CommonFocusRayCastExcludeFlags))
				{
					while (parent.IsValid)
					{
						if (parent == entity)
						{
							flag = true;
							break;
						}
						parent = parent.Parent;
					}
				}
				else
				{
					flag = canShootEvenIfRayCastHitsNothing;
				}
			}
			return flag;
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x000D08A0 File Offset: 0x000CEAA0
		private List<WeakGameEntity> GetEnemySiegeWeapons()
		{
			List<WeakGameEntity> list = null;
			if (Mission.Current.Teams.Attacker.TeamAI is TeamAISiegeComponent)
			{
				using (List<IPrimarySiegeWeapon>.Enumerator enumerator = ((TeamAISiegeComponent)Mission.Current.Teams.Attacker.TeamAI).PrimarySiegeWeapons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SiegeWeapon siegeWeapon;
						if ((siegeWeapon = enumerator.Current as SiegeWeapon) != null && siegeWeapon.GameEntity.GetFirstScriptOfType<DestructableComponent>() != null && siegeWeapon.IsUsed)
						{
							if (list == null)
							{
								list = new List<WeakGameEntity>();
							}
							list.Add(siegeWeapon.GameEntity);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x000D0958 File Offset: 0x000CEB58
		private bool IsThrowingPointAssignable(StonePile.ThrowingPoint throwingPoint)
		{
			return throwingPoint.AmmoPickUpPoint == null && !throwingPoint.StandingPoint.IsDeactivated && !throwingPoint.StandingPoint.HasUser && !throwingPoint.StandingPoint.HasAIMovingTo && (throwingPoint.WaitingPoint == null || (!throwingPoint.WaitingPoint.IsDeactivated && !throwingPoint.WaitingPoint.HasUser && !throwingPoint.WaitingPoint.HasAIMovingTo));
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x000D09CC File Offset: 0x000CEBCC
		private bool AssignAgentToStandingPoint(StandingPoint standingPoint, Agent agent)
		{
			if (standingPoint == null || agent == null || !StonePileAI.IsAgentAssignable(agent))
			{
				return false;
			}
			int num = base.StandingPoints.IndexOf(standingPoint);
			if (num >= 0)
			{
				((IDetachment)this).AddAgent(agent, num, Agent.AIScriptedFrameFlags.None);
				if (agent.Formation != null)
				{
					agent.Formation.DetachUnit(agent, ((IDetachment)this).IsLoose);
					agent.Detachment = this;
					agent.SetDetachmentWeight(this.GetWeightOfStandingPoint(standingPoint));
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001577 RID: 5495
		private const string ThrowingTargetTag = "throwing_target";

		// Token: 0x04001578 RID: 5496
		private const string ThrowingPointTag = "throwing";

		// Token: 0x04001579 RID: 5497
		private const string WaitingPointTag = "wait_to_throw";

		// Token: 0x0400157A RID: 5498
		private const float EnemyInRangeTimerDuration = 0.5f;

		// Token: 0x0400157B RID: 5499
		private const float EnemyWaitTimeLimit = 3f;

		// Token: 0x0400157C RID: 5500
		private const float ThrowingTargetRadius = 1.31f;

		// Token: 0x0400157D RID: 5501
		public int StartingAmmoCount = 12;

		// Token: 0x0400157E RID: 5502
		public string GivenItemID = "boulder";

		// Token: 0x0400157F RID: 5503
		[EditableScriptComponentVariable(true, "")]
		private float _givenItemRange = 15f;

		// Token: 0x04001580 RID: 5504
		private ItemObject _givenItem;

		// Token: 0x04001581 RID: 5505
		private List<GameEntity> _throwingTargets;

		// Token: 0x04001582 RID: 5506
		private List<StonePile.ThrowingPoint> _throwingPoints;

		// Token: 0x04001583 RID: 5507
		private List<StonePile.VolumeBoxTimerPair> _volumeBoxTimerPairs;

		// Token: 0x04001584 RID: 5508
		private Timer _tickOccasionallyTimer;

		// Token: 0x0200064B RID: 1611
		[DefineSynchedMissionObjectType(typeof(StonePile))]
		public struct StonePileRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000AC0 RID: 2752
			// (get) Token: 0x06004032 RID: 16434 RVA: 0x000F84CA File Offset: 0x000F66CA
			// (set) Token: 0x06004033 RID: 16435 RVA: 0x000F84D2 File Offset: 0x000F66D2
			public int ReadAmmoCount { get; private set; }

			// Token: 0x06004034 RID: 16436 RVA: 0x000F84DB File Offset: 0x000F66DB
			public StonePileRecord(int readAmmoCount)
			{
				this.ReadAmmoCount = readAmmoCount;
			}

			// Token: 0x06004035 RID: 16437 RVA: 0x000F84E4 File Offset: 0x000F66E4
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.ReadAmmoCount = GameNetworkMessage.ReadIntFromPacket(CompressionMission.RangedSiegeWeaponAmmoCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x0200064C RID: 1612
		private class ThrowingPoint
		{
			// Token: 0x06004036 RID: 16438 RVA: 0x000F84FC File Offset: 0x000F66FC
			public bool CanUseAttackEntity()
			{
				bool flag = true;
				if (this.AttackEntityNearbyAgentsCheckRadius > 0f)
				{
					float currentTime = Mission.Current.CurrentTime;
					if (currentTime >= this._cachedCanUseAttackEntityExpireTime)
					{
						this._cachedCanUseAttackEntity = Mission.Current.HasAnyAgentsOfSideInRange(this.AttackEntity.GlobalPosition, this.AttackEntityNearbyAgentsCheckRadius, BattleSideEnum.Attacker);
						this._cachedCanUseAttackEntityExpireTime = currentTime + 1f;
					}
					flag = this._cachedCanUseAttackEntity;
				}
				return flag;
			}

			// Token: 0x04002144 RID: 8516
			private const float CachedCanUseAttackEntityUpdateInterval = 1f;

			// Token: 0x04002145 RID: 8517
			public StandingPointWithVolumeBox StandingPoint;

			// Token: 0x04002146 RID: 8518
			public StandingPointWithWeaponRequirement AmmoPickUpPoint;

			// Token: 0x04002147 RID: 8519
			public StandingPointWithWeaponRequirement WaitingPoint;

			// Token: 0x04002148 RID: 8520
			public Timer EnemyInRangeTimer;

			// Token: 0x04002149 RID: 8521
			public GameEntity AttackEntity;

			// Token: 0x0400214A RID: 8522
			public float AttackEntityNearbyAgentsCheckRadius;

			// Token: 0x0400214B RID: 8523
			private float _cachedCanUseAttackEntityExpireTime;

			// Token: 0x0400214C RID: 8524
			private bool _cachedCanUseAttackEntity;

			// Token: 0x020006C6 RID: 1734
			public struct StackArray8ThrowingPoint
			{
				// Token: 0x17000B10 RID: 2832
				public StonePile.ThrowingPoint this[int index]
				{
					get
					{
						switch (index)
						{
						case 0:
							return this._element0;
						case 1:
							return this._element1;
						case 2:
							return this._element2;
						case 3:
							return this._element3;
						case 4:
							return this._element4;
						case 5:
							return this._element5;
						case 6:
							return this._element6;
						case 7:
							return this._element7;
						default:
							return null;
						}
					}
					set
					{
						switch (index)
						{
						case 0:
							this._element0 = value;
							return;
						case 1:
							this._element1 = value;
							return;
						case 2:
							this._element2 = value;
							return;
						case 3:
							this._element3 = value;
							return;
						case 4:
							this._element4 = value;
							return;
						case 5:
							this._element5 = value;
							return;
						case 6:
							this._element6 = value;
							return;
						case 7:
							this._element7 = value;
							return;
						default:
							return;
						}
					}
				}

				// Token: 0x04002351 RID: 9041
				private StonePile.ThrowingPoint _element0;

				// Token: 0x04002352 RID: 9042
				private StonePile.ThrowingPoint _element1;

				// Token: 0x04002353 RID: 9043
				private StonePile.ThrowingPoint _element2;

				// Token: 0x04002354 RID: 9044
				private StonePile.ThrowingPoint _element3;

				// Token: 0x04002355 RID: 9045
				private StonePile.ThrowingPoint _element4;

				// Token: 0x04002356 RID: 9046
				private StonePile.ThrowingPoint _element5;

				// Token: 0x04002357 RID: 9047
				private StonePile.ThrowingPoint _element6;

				// Token: 0x04002358 RID: 9048
				private StonePile.ThrowingPoint _element7;

				// Token: 0x04002359 RID: 9049
				public const int Length = 8;
			}
		}

		// Token: 0x0200064D RID: 1613
		private struct VolumeBoxTimerPair
		{
			// Token: 0x0400214D RID: 8525
			public VolumeBox VolumeBox;

			// Token: 0x0400214E RID: 8526
			public Timer Timer;
		}
	}
}
