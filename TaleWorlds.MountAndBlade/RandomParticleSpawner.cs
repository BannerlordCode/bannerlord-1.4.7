using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000339 RID: 825
	public class RandomParticleSpawner : ScriptComponentBehavior
	{
		// Token: 0x06002E3D RID: 11837 RVA: 0x000B2A0A File Offset: 0x000B0C0A
		private void InitScript()
		{
			this._timeUntilNextParticleSpawn = this.spawnInterval;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000B2A18 File Offset: 0x000B0C18
		private void CheckSpawnParticle(float dt)
		{
			this._timeUntilNextParticleSpawn -= dt;
			if (this._timeUntilNextParticleSpawn <= 0f)
			{
				int childCount = base.GameEntity.ChildCount;
				if (childCount > 0)
				{
					int num = MBRandom.RandomInt(childCount);
					WeakGameEntity child = base.GameEntity.GetChild(num);
					int componentCount = child.GetComponentCount(TaleWorlds.Engine.GameEntity.ComponentType.ParticleSystemInstanced);
					for (int i = 0; i < componentCount; i++)
					{
						((ParticleSystem)child.GetComponentAtIndex(i, TaleWorlds.Engine.GameEntity.ComponentType.ParticleSystemInstanced)).Restart();
					}
				}
				this._timeUntilNextParticleSpawn += this.spawnInterval;
			}
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000B2AAC File Offset: 0x000B0CAC
		protected internal override void OnInit()
		{
			base.OnInit();
			this.InitScript();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000B2AC6 File Offset: 0x000B0CC6
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.OnInit();
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000B2AD4 File Offset: 0x000B0CD4
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000B2ADE File Offset: 0x000B0CDE
		protected internal override void OnTick(float dt)
		{
			this.CheckSpawnParticle(dt);
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000B2AE7 File Offset: 0x000B0CE7
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.CheckSpawnParticle(dt);
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000B2AF7 File Offset: 0x000B0CF7
		protected internal override bool MovesEntity()
		{
			return true;
		}

		// Token: 0x0400125B RID: 4699
		private float _timeUntilNextParticleSpawn;

		// Token: 0x0400125C RID: 4700
		public float spawnInterval = 3f;
	}
}
