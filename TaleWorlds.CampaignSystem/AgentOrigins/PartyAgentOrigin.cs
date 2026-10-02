using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.AgentOrigins
{
	// Token: 0x0200048D RID: 1165
	public class PartyAgentOrigin : IAgentOriginBase
	{
		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x00175A58 File Offset: 0x00173C58
		// (set) Token: 0x060049BE RID: 18878 RVA: 0x00175AB9 File Offset: 0x00173CB9
		public PartyBase Party
		{
			get
			{
				PartyBase partyBase = this._party;
				if (this._troop.IsHero && this._troop.HeroObject.PartyBelongedTo != null && this._troop.HeroObject.PartyBelongedTo.Party != null)
				{
					partyBase = this._troop.HeroObject.PartyBelongedTo.Party;
				}
				return partyBase;
			}
			set
			{
				this._party = value;
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x060049BF RID: 18879 RVA: 0x00175AC2 File Offset: 0x00173CC2
		public IBattleCombatant BattleCombatant
		{
			get
			{
				return this.Party;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x060049C0 RID: 18880 RVA: 0x00175ACC File Offset: 0x00173CCC
		public Banner Banner
		{
			get
			{
				Banner banner;
				if ((banner = this._banner) == null)
				{
					if (this.Party == null)
					{
						if (!this._troop.IsHero)
						{
							return null;
						}
						return this._troop.HeroObject.MapFaction.Banner;
					}
					else
					{
						if (this.Party.LeaderHero == null)
						{
							return this.Party.MapFaction.Banner;
						}
						banner = this.Party.LeaderHero.ClanBanner;
					}
				}
				return banner;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x00175B3E File Offset: 0x00173D3E
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x060049C2 RID: 18882 RVA: 0x00175B46 File Offset: 0x00173D46
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x00175B4E File Offset: 0x00173D4E
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x00175B56 File Offset: 0x00173D56
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060049C5 RID: 18885 RVA: 0x00175B5E File Offset: 0x00173D5E
		public BasicCharacterObject Troop
		{
			get
			{
				return this._troop;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x00175B66 File Offset: 0x00173D66
		// (set) Token: 0x060049C7 RID: 18887 RVA: 0x00175B6E File Offset: 0x00173D6E
		public int Rank { get; private set; }

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x00175B78 File Offset: 0x00173D78
		public bool IsUnderPlayersCommand
		{
			get
			{
				PartyBase party = this.Party;
				return (party != null && party == PartyBase.MainParty) || party.Owner == Hero.MainHero || party.MapFaction.Leader == Hero.MainHero;
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060049C9 RID: 18889 RVA: 0x00175BB8 File Offset: 0x00173DB8
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

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x00175C2A File Offset: 0x00173E2A
		public uint FactionColor
		{
			get
			{
				if (this.Party == null)
				{
					return this._troop.HeroObject.MapFaction.Color;
				}
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060049CB RID: 18891 RVA: 0x00175C5A File Offset: 0x00173E5A
		public uint FactionColor2
		{
			get
			{
				if (this.Party == null)
				{
					return this._troop.HeroObject.MapFaction.Color2;
				}
				return this.Party.MapFaction.Color2;
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060049CC RID: 18892 RVA: 0x00175C8A File Offset: 0x00173E8A
		public int Seed
		{
			get
			{
				if (this.Party == null)
				{
					return 0;
				}
				return CharacterHelper.GetPartyMemberFaceSeed(this.Party, this._troop, this.Rank);
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x00175CB0 File Offset: 0x00173EB0
		public int UniqueSeed
		{
			get
			{
				return this._descriptor.UniqueSeed;
			}
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x00175CCC File Offset: 0x00173ECC
		public PartyAgentOrigin(PartyBase partyBase, CharacterObject characterObject, int rank = -1, UniqueTroopDescriptor uniqueNo = default(UniqueTroopDescriptor), bool alwaysWounded = false, bool isInvincible = false)
		{
			this.Party = partyBase;
			this._troop = characterObject;
			this._descriptor = ((!uniqueNo.IsValid) ? new UniqueTroopDescriptor(Game.Current.NextUniqueTroopSeed) : uniqueNo);
			this.Rank = ((rank == -1) ? MBRandom.RandomInt(10000) : rank);
			this._alwaysWounded = alwaysWounded;
			this._isInvincible = isInvincible;
			AgentOriginUtilities.GetDefaultTroopTraits(this.Troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x00175D5C File Offset: 0x00173F5C
		public void SetWounded()
		{
			if (!this._isInvincible)
			{
				if (this._troop.IsHero)
				{
					this._troop.HeroObject.MakeWounded(null, KillCharacterAction.KillCharacterActionDetail.None);
				}
				if (this.Party != null)
				{
					this.Party.MemberRoster.AddToCounts(this._troop, 0, false, 1, 0, true, -1);
				}
			}
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x00175DB8 File Offset: 0x00173FB8
		public void SetKilled()
		{
			if (!this._isInvincible)
			{
				if (this._alwaysWounded)
				{
					this.SetWounded();
					return;
				}
				if (this._troop.IsHero)
				{
					KillCharacterAction.ApplyByBattle(this._troop.HeroObject, null, true);
					return;
				}
				if (!this._troop.IsHero)
				{
					PartyBase party = this.Party;
					if (party == null)
					{
						return;
					}
					party.MemberRoster.AddToCounts(this._troop, -1, false, 0, 0, true, -1);
				}
			}
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x00175E2B File Offset: 0x0017402B
		public void SetRouted(bool isOrderRetreat)
		{
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00175E30 File Offset: 0x00174030
		public void OnAgentRemoved(float agentHealth)
		{
			if (this._troop.IsHero && this._troop.HeroObject.HeroState != Hero.CharacterStates.Dead && !this._isInvincible)
			{
				this._troop.HeroObject.HitPoints = MathF.Max(1, MathF.Round(agentHealth));
			}
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x00175E81 File Offset: 0x00174081
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x00175E83 File Offset: 0x00174083
		public void SetBanner(Banner banner)
		{
			this._banner = banner;
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x00175E8C File Offset: 0x0017408C
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x04001457 RID: 5207
		private PartyBase _party;

		// Token: 0x04001458 RID: 5208
		private Banner _banner;

		// Token: 0x04001459 RID: 5209
		private CharacterObject _troop;

		// Token: 0x0400145A RID: 5210
		private bool _hasThrownWeapon;

		// Token: 0x0400145B RID: 5211
		private bool _hasHeavyArmor;

		// Token: 0x0400145C RID: 5212
		private bool _hasShield;

		// Token: 0x0400145D RID: 5213
		private bool _hasSpear;

		// Token: 0x0400145F RID: 5215
		private readonly UniqueTroopDescriptor _descriptor;

		// Token: 0x04001460 RID: 5216
		private readonly bool _alwaysWounded;

		// Token: 0x04001461 RID: 5217
		private readonly bool _isInvincible;
	}
}
