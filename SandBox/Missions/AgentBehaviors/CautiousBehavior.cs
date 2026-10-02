using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A5 RID: 165
	public class CautiousBehavior : AgentBehavior
	{
		// Token: 0x060006F0 RID: 1776 RVA: 0x0002EC8A File Offset: 0x0002CE8A
		public CautiousBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._waitTimer = new Timer(base.Mission.CurrentTime, 10f, true);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0002ECB0 File Offset: 0x0002CEB0
		public override void Tick(float dt, bool isSimulation)
		{
			bool flag = true;
			if (base.OwnerAgent.IsCautious())
			{
				if (base.OwnerAgent.IsAIAtMoveDestination())
				{
					base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_guard_cautious_look_around_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
				else
				{
					base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_jump, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				}
				if (base.OwnerAgent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
				{
					base.Mission.AddTickAction(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 0, 1);
				}
				EquipmentIndex offhandWieldedItemIndex = base.OwnerAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex != EquipmentIndex.None && offhandWieldedItemIndex != EquipmentIndex.ExtraWeaponSlot)
				{
					base.Mission.AddTickAction(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 1, 1);
				}
			}
			else if (base.OwnerAgent.IsPatrollingCautious())
			{
				bool flag2 = base.OwnerAgent.IsAIAtMoveDestination();
				base.OwnerAgent.SetWeaponGuard(Agent.UsageDirection.AttackRight);
				if (flag2 && base.OwnerAgent.GetAIMoveDestination().AsVec2.DistanceSquared(base.OwnerAgent.GetAILastSuspiciousPosition().AsVec2) < 1f)
				{
					flag = false;
					if (this._waitTimer.Check(base.Mission.CurrentTime))
					{
						this._waitTimer.Reset(base.Mission.CurrentTime, MBRandom.RandomFloat * 4f + 8f);
						base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_jump, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
						WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
						Vec2 asVec = worldPosition.AsVec2;
						Vec2 movementDirection = base.OwnerAgent.GetMovementDirection();
						movementDirection.RotateCCW(MBRandom.RandomFloat * 6.2831855f);
						worldPosition.SetVec2(worldPosition.AsVec2 + movementDirection * base.OwnerAgent.Monster.BodyCapsuleRadius * MBRandom.RandomFloatRanged(20f, 35f));
						bool flag3;
						worldPosition.SetVec2(base.OwnerAgent.FindLongestDirectMoveToPosition(worldPosition.AsVec2, true, false, out flag3));
						float num = worldPosition.AsVec2.DistanceSquared(asVec);
						if (num > base.OwnerAgent.Monster.BodyCapsuleRadius * base.OwnerAgent.Monster.BodyCapsuleRadius * 10f * 10f)
						{
							worldPosition.SetVec2(asVec + movementDirection * (MathF.Sqrt(num) - base.OwnerAgent.Monster.BodyCapsuleRadius * 10f));
							base.OwnerAgent.SetAILastSuspiciousPosition(worldPosition, false);
						}
					}
					else
					{
						base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_guard_patrolling_cautious_look_around_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					}
				}
			}
			if (flag)
			{
				this._waitTimer.Reset(base.Mission.CurrentTime);
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002EFE3 File Offset: 0x0002D1E3
		public override float GetAvailability(bool isSimulation)
		{
			if (!base.OwnerAgent.IsCautious() && !base.OwnerAgent.IsPatrollingCautious())
			{
				return 0f;
			}
			return 10f;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002F00C File Offset: 0x0002D20C
		protected override void OnDeactivate()
		{
			if (!base.OwnerAgent.IsAlarmed())
			{
				base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_none, false, AnimFlags.amf_priority_jump, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				if (base.OwnerAgent.GetPrimaryWieldedItemIndex() != EquipmentIndex.None)
				{
					base.Mission.AddTickActionMT(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 0, 0);
				}
				EquipmentIndex offhandWieldedItemIndex = base.OwnerAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex != EquipmentIndex.None && offhandWieldedItemIndex != EquipmentIndex.ExtraWeaponSlot)
				{
					base.Mission.AddTickActionMT(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 1, 0);
				}
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002F0A9 File Offset: 0x0002D2A9
		protected override void OnActivate()
		{
			this._waitTimer.Reset(base.Mission.CurrentTime);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002F0C1 File Offset: 0x0002D2C1
		public override string GetDebugInfo()
		{
			return string.Empty;
		}

		// Token: 0x040003B0 RID: 944
		private readonly Timer _waitTimer;
	}
}
