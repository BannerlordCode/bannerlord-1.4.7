using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000BA RID: 186
	public class MissionSiegeWeapon : IMissionSiegeWeapon
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00020B23 File Offset: 0x0001ED23
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x00020B2B File Offset: 0x0001ED2B
		public SiegeEngineType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x00020B33 File Offset: 0x0001ED33
		public float Health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x00020B3B File Offset: 0x0001ED3B
		public float InitialHealth
		{
			get
			{
				return this._initialHealth;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x00020B43 File Offset: 0x0001ED43
		public float MaxHealth
		{
			get
			{
				return this._maxHealth;
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00020B4B File Offset: 0x0001ED4B
		private MissionSiegeWeapon(int index, SiegeEngineType type, float health, float maxHealth)
		{
			this._index = index;
			this._type = type;
			this._initialHealth = health;
			this._health = this._initialHealth;
			this._maxHealth = maxHealth;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00020B7C File Offset: 0x0001ED7C
		public static MissionSiegeWeapon CreateDefaultWeapon(SiegeEngineType type)
		{
			return new MissionSiegeWeapon(-1, type, (float)type.BaseHitPoints, (float)type.BaseHitPoints);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00020B93 File Offset: 0x0001ED93
		public static MissionSiegeWeapon CreateCampaignWeapon(SiegeEngineType type, int index, float health, float maxHealth)
		{
			return new MissionSiegeWeapon(index, type, health, maxHealth);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00020B9E File Offset: 0x0001ED9E
		public void SetHealth(float health)
		{
			this._health = health;
		}

		// Token: 0x0400056D RID: 1389
		private float _health;

		// Token: 0x0400056E RID: 1390
		private readonly int _index;

		// Token: 0x0400056F RID: 1391
		private readonly SiegeEngineType _type;

		// Token: 0x04000570 RID: 1392
		private readonly float _initialHealth;

		// Token: 0x04000571 RID: 1393
		private readonly float _maxHealth;
	}
}
