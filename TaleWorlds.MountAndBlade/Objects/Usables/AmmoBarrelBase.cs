using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003A5 RID: 933
	public abstract class AmmoBarrelBase : UsableMachine
	{
		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06003504 RID: 13572 RVA: 0x000D9FC5 File Offset: 0x000D81C5
		private int PickupSoundFromBarrelCache
		{
			get
			{
				if (this._pickupSoundFromBarrel == -1)
				{
					this._pickupSoundFromBarrel = this.GetSoundEvent();
				}
				return this._pickupSoundFromBarrel;
			}
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000D9FE2 File Offset: 0x000D81E2
		public AmmoBarrelBase()
		{
			this._requiredWeaponClasses = this.GetRequiredWeaponClasses();
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000DA004 File Offset: 0x000D8204
		protected internal override void OnInit()
		{
			base.OnInit();
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				(standingPoint as StandingPointWithWeaponRequirement).InitRequiredWeaponClasses(this._requiredWeaponClasses);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.MakeVisibilityCheck = false;
			this._isVisible = base.GameEntity.IsVisibleIncludeParents();
		}

		// Token: 0x06003507 RID: 13575
		protected abstract WeaponClass[] GetRequiredWeaponClasses();

		// Token: 0x06003508 RID: 13576 RVA: 0x000DA08C File Offset: 0x000D828C
		public override void OnDeploymentFinished()
		{
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.LockUserFrames = false;
				}
			}
		}

		// Token: 0x06003509 RID: 13577
		protected abstract int GetSoundEvent();

		// Token: 0x0600350A RID: 13578 RVA: 0x000DA0E8 File Offset: 0x000D82E8
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x0600350B RID: 13579
		public abstract override TextObject GetDescriptionText(WeakGameEntity gameEntity);

		// Token: 0x0600350C RID: 13580 RVA: 0x000DA117 File Offset: 0x000D8317
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (GameNetwork.IsClientOrReplay)
			{
				return base.GetTickRequirement();
			}
			return ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel | base.GetTickRequirement();
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000DA12F File Offset: 0x000D832F
		protected internal override void OnTickParallel(float dt)
		{
			this.TickAux(true);
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000DA138 File Offset: 0x000D8338
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._needsSingleThreadTickOnce)
			{
				this._needsSingleThreadTickOnce = false;
				this.TickAux(false);
			}
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000DA158 File Offset: 0x000D8358
		private void TickAux(bool isParallel)
		{
			if (this._isVisible && !GameNetwork.IsClientOrReplay)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.HasUser)
					{
						Agent userAgent = standingPoint.UserAgent;
						ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
						ActionIndexCache currentAction2 = userAgent.GetCurrentAction(1);
						if (!(currentAction2 == ActionIndexCache.act_none) || (!(currentAction == ActionIndexCache.act_pickup_down_begin) && !(currentAction == ActionIndexCache.act_pickup_down_begin_left_stance)))
						{
							if (currentAction2 == ActionIndexCache.act_none && (currentAction == ActionIndexCache.act_pickup_down_end || currentAction == ActionIndexCache.act_pickup_down_end_left_stance))
							{
								if (isParallel)
								{
									this._needsSingleThreadTickOnce = true;
								}
								else
								{
									for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
									{
										if (!userAgent.Equipment[equipmentIndex].IsEmpty && this._requiredWeaponClasses.Contains(userAgent.Equipment[equipmentIndex].CurrentUsageItem.WeaponClass) && userAgent.Equipment[equipmentIndex].Amount < userAgent.Equipment[equipmentIndex].ModifiedMaxAmount)
										{
											userAgent.SetWeaponAmountInSlot(equipmentIndex, userAgent.Equipment[equipmentIndex].ModifiedMaxAmount, true);
											Mission.Current.MakeSoundOnlyOnRelatedPeer(this.PickupSoundFromBarrelCache, userAgent.Position, userAgent.Index);
										}
									}
									userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
								}
							}
							else
							{
								if (!(currentAction2 != ActionIndexCache.act_none))
								{
									Agent agent = userAgent;
									int num = 0;
									ActionIndexCache actionIndexCache = (userAgent.GetIsLeftStance() ? ActionIndexCache.act_pickup_down_begin_left_stance : ActionIndexCache.act_pickup_down_begin);
									if (agent.SetActionChannel(num, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
									{
										continue;
									}
								}
								if (isParallel)
								{
									this._needsSingleThreadTickOnce = true;
								}
								else
								{
									userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000DA38C File Offset: 0x000D858C
		public override OrderType GetOrder(BattleSideEnum side)
		{
			return OrderType.None;
		}

		// Token: 0x04001683 RID: 5763
		private readonly WeaponClass[] _requiredWeaponClasses;

		// Token: 0x04001684 RID: 5764
		private int _pickupSoundFromBarrel = -1;

		// Token: 0x04001685 RID: 5765
		private bool _isVisible = true;

		// Token: 0x04001686 RID: 5766
		private bool _needsSingleThreadTickOnce;
	}
}
