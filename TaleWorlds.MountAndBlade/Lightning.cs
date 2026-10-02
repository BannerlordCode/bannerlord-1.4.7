using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000334 RID: 820
	public class Lightning : ScriptComponentBehavior
	{
		// Token: 0x06002E14 RID: 11796 RVA: 0x000B1D52 File Offset: 0x000AFF52
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000B1D5C File Offset: 0x000AFF5C
		protected internal override void OnInit()
		{
			base.OnInit();
			this._lightningTimer = MBRandom.RandomFloat * this.LightningRate;
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000B1D82 File Offset: 0x000AFF82
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000B1D85 File Offset: 0x000AFF85
		protected internal override void OnEditorTick(float dt)
		{
			this.TickAux(dt);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000B1D8E File Offset: 0x000AFF8E
		protected internal override void OnTick(float dt)
		{
			this.TickAux(dt);
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000B1D98 File Offset: 0x000AFF98
		private void SpawnBolt()
		{
			this._boltIntensity = 1f;
			Random random = new Random();
			float num = (random.NextFloat() * 2f - 1f) * this.LightTravelRadius;
			float num2 = (random.NextFloat() * 2f - 1f) * this.LightTravelRadius;
			Vec3 vec = new Vec3(num, num2, 0f, -1f);
			base.GameEntity.SetLocalPosition(vec);
			string text = "event:/map/ambient/node/thunder";
			Vec3 globalPosition = base.GameEntity.GlobalPosition;
			SoundManager.StartOneShotEvent(text, in globalPosition);
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000B1E28 File Offset: 0x000B0028
		private void SetLightIntensity(float intensity)
		{
			Light light = base.GameEntity.GetComponentAtIndex(0, TaleWorlds.Engine.GameEntity.ComponentType.Light) as Light;
			Light light2 = base.GameEntity.GetComponentAtIndex(1, TaleWorlds.Engine.GameEntity.ComponentType.Light) as Light;
			float timeOfDay = base.Scene.TimeOfDay;
			if (light != null && light2 != null)
			{
				if (timeOfDay < 3.5f || timeOfDay > 21f)
				{
					float num = ((timeOfDay > 20f) ? MBMath.Map(timeOfDay, 20f, 24f, 4f, 1f) : MBMath.Map(timeOfDay, 0f, 6f, 1f, 4f));
					light.Intensity = this.LightIntensity * intensity * num;
					light2.Intensity = this.LightIntensity * intensity * num / 4f;
					return;
				}
				float num2 = ((timeOfDay < 12f) ? MBMath.Map(timeOfDay, 6f, 12f, 100f, 200f) : MBMath.Map(timeOfDay, 12f, 20f, 200f, 100f));
				light.Intensity = this.LightIntensity * intensity * num2;
				light2.Intensity = this.LightIntensity * intensity * num2 / 2f;
			}
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000B1F66 File Offset: 0x000B0166
		private float CalculateBoltIntensity(float dt)
		{
			this._boltIntensity -= dt * this.BoltSpeed;
			if (this._boltIntensity < 0f)
			{
				this._boltIntensity = 0f;
			}
			return this._boltIntensity;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000B1F9C File Offset: 0x000B019C
		private void TickAux(float dt)
		{
			this._lightningTimer += dt;
			if (this.IsBoltEnabled)
			{
				base.GameEntity.ResumeParticleSystem(true);
			}
			if (this._lightningTimer > this.LightningRate)
			{
				this._lightningTimer = 0f;
				this._spawnLightning = true;
			}
			if (this._spawnLightning)
			{
				this._boltTimer += dt * this.BoltSpeed;
				this.SetLightIntensity(this.CalculateBoltIntensity(dt));
				if (this._boltTimer > this.BoltLife)
				{
					if (this.IsBoltEnabled)
					{
						base.GameEntity.PauseParticleSystem(false);
					}
					this.SpawnBolt();
					if (this.IsBoltEnabled)
					{
						base.GameEntity.BurstEntityParticle(false);
					}
					this.SetLightIntensity(0f);
					this._currentNumberOfLightning++;
					if (this._currentNumberOfLightning == 2)
					{
						this._spawnLightning = false;
						this._currentNumberOfLightning = 0;
					}
					this._boltTimer = 0f;
				}
			}
		}

		// Token: 0x0400123B RID: 4667
		public float LightIntensity = 1000f;

		// Token: 0x0400123C RID: 4668
		public float LightningRate = 5f;

		// Token: 0x0400123D RID: 4669
		public float BoltSpeed = 1f;

		// Token: 0x0400123E RID: 4670
		public float LightTravelRadius = 5f;

		// Token: 0x0400123F RID: 4671
		public float BoltLife = 1f;

		// Token: 0x04001240 RID: 4672
		public bool IsBoltEnabled = true;

		// Token: 0x04001241 RID: 4673
		private float _boltTimer;

		// Token: 0x04001242 RID: 4674
		private float _lightningTimer;

		// Token: 0x04001243 RID: 4675
		private bool _spawnLightning;

		// Token: 0x04001244 RID: 4676
		private float _boltIntensity = 1f;

		// Token: 0x04001245 RID: 4677
		private int _currentNumberOfLightning;
	}
}
