using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000102 RID: 258
	public class CustomBattleAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x000178BF File Offset: 0x00015ABF
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x000178C7 File Offset: 0x00015AC7
		public CustomBattleCombatant CustomBattleCombatant { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x000178D0 File Offset: 0x00015AD0
		IBattleCombatant IAgentOriginBase.BattleCombatant
		{
			get
			{
				return this.CustomBattleCombatant;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x000178D8 File Offset: 0x00015AD8
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x000178E0 File Offset: 0x00015AE0
		public BasicCharacterObject Troop { get; private set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x000178E9 File Offset: 0x00015AE9
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x000178F1 File Offset: 0x00015AF1
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x000178F9 File Offset: 0x00015AF9
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00017901 File Offset: 0x00015B01
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x00017909 File Offset: 0x00015B09
		// (set) Token: 0x06000D1D RID: 3357 RVA: 0x00017911 File Offset: 0x00015B11
		public int Rank { get; private set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0001791A File Offset: 0x00015B1A
		public Banner Banner
		{
			get
			{
				return this.CustomBattleCombatant.Banner;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00017927 File Offset: 0x00015B27
		public bool IsUnderPlayersCommand
		{
			get
			{
				return this._isPlayerSide;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x0001792F File Offset: 0x00015B2F
		public bool IsInSameArmyAsPlayer
		{
			get
			{
				return this._isPlayerSide;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00017937 File Offset: 0x00015B37
		public uint FactionColor
		{
			get
			{
				return this.CustomBattleCombatant.BasicCulture.Color;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000D22 RID: 3362 RVA: 0x00017949 File Offset: 0x00015B49
		public uint FactionColor2
		{
			get
			{
				return this.CustomBattleCombatant.BasicCulture.Color2;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0001795B File Offset: 0x00015B5B
		public int Seed
		{
			get
			{
				return this.Troop.GetDefaultFaceSeed(this.Rank);
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00017970 File Offset: 0x00015B70
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0001798C File Offset: 0x00015B8C
		public CustomBattleAgentOrigin(CustomBattleCombatant customBattleCombatant, BasicCharacterObject characterObject, CustomBattleTroopSupplier troopSupplier, bool isPlayerSide, int rank = -1, UniqueTroopDescriptor uniqueNo = default(UniqueTroopDescriptor))
		{
			this.CustomBattleCombatant = customBattleCombatant;
			this.Troop = characterObject;
			this._descriptor = ((!uniqueNo.IsValid) ? new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed) : uniqueNo);
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._troopSupplier = troopSupplier;
			this._isPlayerSide = isPlayerSide;
			AgentOriginUtilities.GetDefaultTroopTraits(this.Troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00017A1A File Offset: 0x00015C1A
		public void SetWounded()
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopWounded();
				this._isRemoved = true;
			}
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00017A36 File Offset: 0x00015C36
		public void SetKilled()
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopKilled();
				this._isRemoved = true;
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00017A52 File Offset: 0x00015C52
		public void SetRouted(bool isOrderRetreat)
		{
			if (!this._isRemoved)
			{
				this._troopSupplier.OnTroopRouted();
				this._isRemoved = true;
			}
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00017A6E File Offset: 0x00015C6E
		public void OnAgentRemoved(float agentHealth)
		{
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00017A70 File Offset: 0x00015C70
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00017A72 File Offset: 0x00015C72
		public void SetBanner(Banner banner)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00017A79 File Offset: 0x00015C79
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x040002D0 RID: 720
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x040002D1 RID: 721
		private readonly bool _isPlayerSide;

		// Token: 0x040002D2 RID: 722
		private CustomBattleTroopSupplier _troopSupplier;

		// Token: 0x040002D3 RID: 723
		private bool _isRemoved;

		// Token: 0x040002D4 RID: 724
		private bool _hasThrownWeapon;

		// Token: 0x040002D5 RID: 725
		private bool _hasHeavyArmor;

		// Token: 0x040002D6 RID: 726
		private bool _hasShield;

		// Token: 0x040002D7 RID: 727
		private bool _hasSpear;
	}
}
