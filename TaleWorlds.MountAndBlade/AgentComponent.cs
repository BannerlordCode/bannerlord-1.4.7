using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020000FD RID: 253
	public abstract class AgentComponent
	{
		// Token: 0x06000C1C RID: 3100 RVA: 0x00016F6E File Offset: 0x0001516E
		protected AgentComponent(Agent agent)
		{
			this.Agent = agent;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00016F7D File Offset: 0x0001517D
		public virtual void Initialize()
		{
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00016F7F File Offset: 0x0001517F
		public virtual void OnTick(float dt)
		{
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00016F81 File Offset: 0x00015181
		public virtual void OnTickParallel(float dt)
		{
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00016F83 File Offset: 0x00015183
		public virtual float GetMoraleAddition()
		{
			return 0f;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00016F8A File Offset: 0x0001518A
		public virtual float GetMoraleDecreaseConstant()
		{
			return 1f;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00016F91 File Offset: 0x00015191
		public virtual void OnItemPickup(SpawnedItemEntity item)
		{
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00016F93 File Offset: 0x00015193
		public virtual void OnWeaponDrop(MissionWeapon droppedWeapon)
		{
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00016F95 File Offset: 0x00015195
		public virtual void OnStopUsingGameObject()
		{
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00016F97 File Offset: 0x00015197
		public virtual void OnWeaponHPChanged(ItemObject item, int hitPoints)
		{
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00016F99 File Offset: 0x00015199
		public virtual void OnRetreating()
		{
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00016F9B File Offset: 0x0001519B
		public virtual void OnMount(Agent mount)
		{
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00016F9D File Offset: 0x0001519D
		public virtual void OnDismount(Agent mount)
		{
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00016F9F File Offset: 0x0001519F
		public virtual void OnHit(Agent affectorAgent, int damage, in MissionWeapon affectorWeapon, in Blow b, in AttackCollisionData collisionData)
		{
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00016FA1 File Offset: 0x000151A1
		public virtual void OnDisciplineChanged()
		{
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00016FA3 File Offset: 0x000151A3
		public virtual void OnAgentRemoved()
		{
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00016FA5 File Offset: 0x000151A5
		public virtual void OnAgentTeleported()
		{
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00016FA7 File Offset: 0x000151A7
		public virtual void OnAIInputSet(ref Agent.EventControlFlag eventFlag, ref Agent.MovementControlFlag movementFlag, ref Vec2 inputVector)
		{
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00016FA9 File Offset: 0x000151A9
		public virtual void OnComponentRemoved()
		{
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00016FAB File Offset: 0x000151AB
		public virtual void OnFormationSet()
		{
		}

		// Token: 0x040002C0 RID: 704
		protected readonly Agent Agent;
	}
}
