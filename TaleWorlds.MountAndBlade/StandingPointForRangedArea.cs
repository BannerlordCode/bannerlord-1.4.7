using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000358 RID: 856
	public class StandingPointForRangedArea : StandingPoint
	{
		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x000C7D26 File Offset: 0x000C5F26
		public override Agent.AIScriptedFrameFlags DisableScriptedFrameFlags
		{
			get
			{
				return Agent.AIScriptedFrameFlags.NoAttack | Agent.AIScriptedFrameFlags.ConsiderRotation;
			}
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x000C7D29 File Offset: 0x000C5F29
		protected internal override void OnInit()
		{
			base.OnInit();
			this.AutoSheathWeapons = false;
			this.LockUserFrames = false;
			this.LockUserPositions = true;
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000C7D54 File Offset: 0x000C5F54
		public override bool IsDisabledForAgent(Agent agent)
		{
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			if (primaryWieldedItemIndex == EquipmentIndex.None)
			{
				return true;
			}
			WeaponComponentData currentUsageItem = agent.Equipment[primaryWieldedItemIndex].CurrentUsageItem;
			if (currentUsageItem == null || !currentUsageItem.IsRangedWeapon)
			{
				return true;
			}
			if (primaryWieldedItemIndex == EquipmentIndex.ExtraWeaponSlot)
			{
				return this.ThrowingValueMultiplier <= 0f || base.IsDisabledForAgent(agent);
			}
			return this.RangedWeaponValueMultiplier <= 0f || base.IsDisabledForAgent(agent);
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000C7DC4 File Offset: 0x000C5FC4
		public override float GetUsageScoreForAgent(Agent agent)
		{
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			float num = 0f;
			if (primaryWieldedItemIndex != EquipmentIndex.None && agent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon)
			{
				num = ((primaryWieldedItemIndex == EquipmentIndex.ExtraWeaponSlot) ? this.ThrowingValueMultiplier : this.RangedWeaponValueMultiplier);
			}
			return base.GetUsageScoreForAgent(agent) + num;
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000C7E19 File Offset: 0x000C6019
		public override bool HasAlternative()
		{
			return true;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000C7E1C File Offset: 0x000C601C
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.HasUser)
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.TickParallel2;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x000C7E35 File Offset: 0x000C6035
		protected internal override void OnTickParallel2(float dt)
		{
			base.OnTickParallel2(dt);
			if (base.HasUser && this.IsDisabledForAgent(base.UserAgent))
			{
				base.UserAgent.StopUsingGameObjectMT(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
		}

		// Token: 0x040014A2 RID: 5282
		public float ThrowingValueMultiplier = 5f;

		// Token: 0x040014A3 RID: 5283
		public float RangedWeaponValueMultiplier = 2f;
	}
}
