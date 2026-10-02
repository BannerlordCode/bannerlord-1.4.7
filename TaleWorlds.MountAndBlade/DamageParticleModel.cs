using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F9 RID: 505
	public abstract class DamageParticleModel : MBGameModel<DamageParticleModel>
	{
		// Token: 0x06001D9F RID: 7583
		public abstract void GetMeleeAttackBloodParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, out HitParticleResultData particleResultData);

		// Token: 0x06001DA0 RID: 7584
		public abstract void GetMeleeAttackSweatParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, out HitParticleResultData particleResultData);

		// Token: 0x06001DA1 RID: 7585
		public abstract int GetMissileAttackParticle(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData);
	}
}
