using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.AgentOrigins
{
	// Token: 0x0200048F RID: 1167
	public class SimpleAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060049EF RID: 18927 RVA: 0x00176140 File Offset: 0x00174340
		public BasicCharacterObject Troop
		{
			get
			{
				return this._troop;
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060049F0 RID: 18928 RVA: 0x00176148 File Offset: 0x00174348
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060049F1 RID: 18929 RVA: 0x00176150 File Offset: 0x00174350
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060049F2 RID: 18930 RVA: 0x00176158 File Offset: 0x00174358
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060049F3 RID: 18931 RVA: 0x00176160 File Offset: 0x00174360
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x00176168 File Offset: 0x00174368
		public bool IsUnderPlayersCommand
		{
			get
			{
				PartyBase party = this.Party;
				return party != null && (party == PartyBase.MainParty || party.Owner == Hero.MainHero || party.MapFaction.Leader == Hero.MainHero);
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060049F5 RID: 18933 RVA: 0x001761AC File Offset: 0x001743AC
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

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x060049F6 RID: 18934 RVA: 0x0017621E File Offset: 0x0017441E
		public uint FactionColor
		{
			get
			{
				if (this.Party != null)
				{
					return this.Party.MapFaction.Color;
				}
				if (this._troop.IsHero)
				{
					return this._troop.HeroObject.MapFaction.Color;
				}
				return 0U;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x060049F7 RID: 18935 RVA: 0x0017625D File Offset: 0x0017445D
		public uint FactionColor2
		{
			get
			{
				if (this.Party != null)
				{
					return this.Party.MapFaction.Color2;
				}
				if (this._troop.IsHero)
				{
					return this._troop.HeroObject.MapFaction.Color2;
				}
				return 0U;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x060049F8 RID: 18936 RVA: 0x0017629C File Offset: 0x0017449C
		public int Seed
		{
			get
			{
				if (this.Party != null)
				{
					return CharacterHelper.GetPartyMemberFaceSeed(this.Party, this._troop, this.Rank);
				}
				return CharacterHelper.GetDefaultFaceSeed(this._troop, this.Rank);
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x060049F9 RID: 18937 RVA: 0x001762CF File Offset: 0x001744CF
		public PartyBase Party
		{
			get
			{
				if (!this._troop.IsHero || this._troop.HeroObject.PartyBelongedTo == null)
				{
					return null;
				}
				return this._troop.HeroObject.PartyBelongedTo.Party;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x060049FA RID: 18938 RVA: 0x00176307 File Offset: 0x00174507
		public IBattleCombatant BattleCombatant
		{
			get
			{
				return this.Party;
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x060049FB RID: 18939 RVA: 0x0017630F File Offset: 0x0017450F
		public Banner Banner
		{
			get
			{
				return this._banner;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x00176317 File Offset: 0x00174517
		// (set) Token: 0x060049FD RID: 18941 RVA: 0x0017631F File Offset: 0x0017451F
		public int Rank { get; private set; }

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x00176328 File Offset: 0x00174528
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x00176338 File Offset: 0x00174538
		public SimpleAgentOrigin(BasicCharacterObject troop, int rank = -1, Banner banner = null, UniqueTroopDescriptor descriptor = default(UniqueTroopDescriptor))
		{
			this._troop = (CharacterObject)troop;
			this._descriptor = descriptor;
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._banner = banner;
			AgentOriginUtilities.GetDefaultTroopTraits(this._troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x001763A0 File Offset: 0x001745A0
		public void SetWounded()
		{
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x001763A2 File Offset: 0x001745A2
		public void SetKilled()
		{
			if (this._troop.IsHero)
			{
				KillCharacterAction.ApplyByBattle(this._troop.HeroObject, null, true);
			}
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x001763C3 File Offset: 0x001745C3
		public void SetRouted(bool isOrderRetreat)
		{
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x001763C5 File Offset: 0x001745C5
		public void OnAgentRemoved(float agentHealth)
		{
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x001763C8 File Offset: 0x001745C8
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject formationCaptain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
			if (isTeamKill)
			{
				CharacterObject troop = this._troop;
				ExplainedNumber xpFromHit = Campaign.Current.Models.CombatXpModel.GetXpFromHit(troop, (CharacterObject)formationCaptain, (CharacterObject)victim, this.Party, damage, isFatal, CombatXpModel.MissionTypeEnum.Battle);
				if (troop.IsHero && attackerWeapon != null)
				{
					SkillObject skillForWeapon = Campaign.Current.Models.CombatXpModel.GetSkillForWeapon(attackerWeapon, false);
					troop.HeroObject.AddSkillXp(skillForWeapon, (float)xpFromHit.RoundedResultNumber);
				}
			}
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x00176444 File Offset: 0x00174644
		public void SetBanner(Banner banner)
		{
			this._banner = banner;
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x0017644D File Offset: 0x0017464D
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x0400146A RID: 5226
		private CharacterObject _troop;

		// Token: 0x0400146B RID: 5227
		private bool _hasThrownWeapon;

		// Token: 0x0400146C RID: 5228
		private bool _hasHeavyArmor;

		// Token: 0x0400146D RID: 5229
		private bool _hasShield;

		// Token: 0x0400146E RID: 5230
		private bool _hasSpear;

		// Token: 0x0400146F RID: 5231
		private Banner _banner;

		// Token: 0x04001471 RID: 5233
		private UniqueTroopDescriptor _descriptor;
	}
}
