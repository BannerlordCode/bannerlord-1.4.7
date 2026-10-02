using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.TroopSuppliers;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.AgentOrigins
{
	// Token: 0x0200048E RID: 1166
	public class PartyGroupAgentOrigin : IAgentOriginBase
	{
		// Token: 0x060049D6 RID: 18902 RVA: 0x00175E94 File Offset: 0x00174094
		internal PartyGroupAgentOrigin(PartyGroupTroopSupplier supplier, UniqueTroopDescriptor descriptor, int rank)
		{
			this._supplier = supplier;
			this._descriptor = descriptor;
			this._rank = rank;
			AgentOriginUtilities.GetDefaultTroopTraits(this.Troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060049D7 RID: 18903 RVA: 0x00175ED4 File Offset: 0x001740D4
		public PartyBase Party
		{
			get
			{
				return this._supplier.GetParty(this._descriptor);
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060049D8 RID: 18904 RVA: 0x00175EE7 File Offset: 0x001740E7
		public IBattleCombatant BattleCombatant
		{
			get
			{
				return this.Party;
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060049D9 RID: 18905 RVA: 0x00175EEF File Offset: 0x001740EF
		public Banner Banner
		{
			get
			{
				if (this.Party.LeaderHero == null)
				{
					return this.Party.MapFaction.Banner;
				}
				return this.Party.LeaderHero.ClanBanner;
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060049DA RID: 18906 RVA: 0x00175F20 File Offset: 0x00174120
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060049DB RID: 18907 RVA: 0x00175F3B File Offset: 0x0017413B
		public CharacterObject Troop
		{
			get
			{
				return this._supplier.GetTroop(this._descriptor);
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060049DC RID: 18908 RVA: 0x00175F4E File Offset: 0x0017414E
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060049DD RID: 18909 RVA: 0x00175F56 File Offset: 0x00174156
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x00175F5E File Offset: 0x0017415E
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x00175F66 File Offset: 0x00174166
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x060049E0 RID: 18912 RVA: 0x00175F6E File Offset: 0x0017416E
		BasicCharacterObject IAgentOriginBase.Troop
		{
			get
			{
				return this.Troop;
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x060049E1 RID: 18913 RVA: 0x00175F76 File Offset: 0x00174176
		public UniqueTroopDescriptor TroopDesc
		{
			get
			{
				return this._descriptor;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060049E2 RID: 18914 RVA: 0x00175F7E File Offset: 0x0017417E
		public int Rank
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x00175F86 File Offset: 0x00174186
		public bool IsUnderPlayersCommand
		{
			get
			{
				return this.Troop == Hero.MainHero.CharacterObject || PartyBase.IsPartyUnderPlayerCommand(this.Party);
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x00175FA8 File Offset: 0x001741A8
		public bool IsInSameArmyAsPlayer
		{
			get
			{
				PartyBase party = this.Party;
				MobileParty mobileParty;
				Army army;
				return party != null && (mobileParty = party.MobileParty) != null && (army = mobileParty.Army) != null && army == MobileParty.MainParty.Army && (army.LeaderParty == mobileParty || mobileParty.AttachedTo == army.LeaderParty) && (army.LeaderParty == MobileParty.MainParty || MobileParty.MainParty.AttachedTo == army.LeaderParty);
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060049E5 RID: 18917 RVA: 0x0017601A File Offset: 0x0017421A
		public uint FactionColor
		{
			get
			{
				return this.Party.MapFaction.Color;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x0017602C File Offset: 0x0017422C
		public uint FactionColor2
		{
			get
			{
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060049E7 RID: 18919 RVA: 0x0017603E File Offset: 0x0017423E
		public int Seed
		{
			get
			{
				return CharacterHelper.GetPartyMemberFaceSeed(this.Party, this.Troop, this.Rank);
			}
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x00176057 File Offset: 0x00174257
		public void SetWounded()
		{
			if (!this._isRemoved)
			{
				this._supplier.OnTroopWounded(this._descriptor);
				this._isRemoved = true;
			}
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x0017607C File Offset: 0x0017427C
		public void SetKilled()
		{
			if (!this._isRemoved)
			{
				this._supplier.OnTroopKilled(this._descriptor);
				if (this.Troop.IsHero)
				{
					KillCharacterAction.ApplyByBattle(this.Troop.HeroObject, null, true);
				}
				this._isRemoved = true;
			}
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x001760C8 File Offset: 0x001742C8
		public void SetRouted(bool isOrderRetreat)
		{
			if (!this._isRemoved)
			{
				this._supplier.OnTroopRouted(this._descriptor, isOrderRetreat);
				this._isRemoved = true;
			}
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x001760EB File Offset: 0x001742EB
		public void OnAgentRemoved(float agentHealth)
		{
			if (this.Troop.IsHero)
			{
				this.Troop.HeroObject.HitPoints = MathF.Max(1, MathF.Round(agentHealth));
			}
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x00176116 File Offset: 0x00174316
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
			this._supplier.OnTroopScoreHit(this._descriptor, victim, damage, isFatal, isTeamKill, attackerWeapon);
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00176131 File Offset: 0x00174331
		public void SetBanner(Banner banner)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00176138 File Offset: 0x00174338
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x04001462 RID: 5218
		private readonly PartyGroupTroopSupplier _supplier;

		// Token: 0x04001463 RID: 5219
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x04001464 RID: 5220
		private readonly int _rank;

		// Token: 0x04001465 RID: 5221
		private bool _isRemoved;

		// Token: 0x04001466 RID: 5222
		private bool _hasThrownWeapon;

		// Token: 0x04001467 RID: 5223
		private bool _hasHeavyArmor;

		// Token: 0x04001468 RID: 5224
		private bool _hasShield;

		// Token: 0x04001469 RID: 5225
		private bool _hasSpear;
	}
}
