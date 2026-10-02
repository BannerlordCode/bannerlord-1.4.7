using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000101 RID: 257
	public class BasicBattleAgentOrigin : IAgentOriginBase
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00017839 File Offset: 0x00015A39
		bool IAgentOriginBase.IsUnderPlayersCommand
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x0001783C File Offset: 0x00015A3C
		bool IAgentOriginBase.IsInSameArmyAsPlayer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0001783F File Offset: 0x00015A3F
		uint IAgentOriginBase.FactionColor
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00017842 File Offset: 0x00015A42
		uint IAgentOriginBase.FactionColor2
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00017845 File Offset: 0x00015A45
		IBattleCombatant IAgentOriginBase.BattleCombatant
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00017848 File Offset: 0x00015A48
		int IAgentOriginBase.UniqueSeed
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0001784B File Offset: 0x00015A4B
		int IAgentOriginBase.Seed
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0001784E File Offset: 0x00015A4E
		Banner IAgentOriginBase.Banner
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00017851 File Offset: 0x00015A51
		BasicCharacterObject IAgentOriginBase.Troop
		{
			get
			{
				return this._troop;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00017859 File Offset: 0x00015A59
		bool IAgentOriginBase.HasThrownWeapon
		{
			get
			{
				return this._hasThrownWeapon;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00017861 File Offset: 0x00015A61
		bool IAgentOriginBase.HasHeavyArmor
		{
			get
			{
				return this._hasHeavyArmor;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00017869 File Offset: 0x00015A69
		bool IAgentOriginBase.HasShield
		{
			get
			{
				return this._hasShield;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00017871 File Offset: 0x00015A71
		bool IAgentOriginBase.HasSpear
		{
			get
			{
				return this._hasSpear;
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00017879 File Offset: 0x00015A79
		public BasicBattleAgentOrigin(BasicCharacterObject troop)
		{
			this._troop = troop;
			AgentOriginUtilities.GetDefaultTroopTraits(this._troop, out this._hasThrownWeapon, out this._hasSpear, out this._hasShield, out this._hasHeavyArmor);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000178AB File Offset: 0x00015AAB
		void IAgentOriginBase.SetWounded()
		{
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000178AD File Offset: 0x00015AAD
		void IAgentOriginBase.SetKilled()
		{
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000178AF File Offset: 0x00015AAF
		void IAgentOriginBase.SetRouted(bool isOrderRetreat)
		{
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000178B1 File Offset: 0x00015AB1
		void IAgentOriginBase.OnAgentRemoved(float agentHealth)
		{
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x000178B3 File Offset: 0x00015AB3
		void IAgentOriginBase.OnScoreHit(BasicCharacterObject victim, BasicCharacterObject captain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon)
		{
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000178B5 File Offset: 0x00015AB5
		void IAgentOriginBase.SetBanner(Banner banner)
		{
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000178B7 File Offset: 0x00015AB7
		TroopTraitsMask IAgentOriginBase.GetTraitsMask()
		{
			return AgentOriginUtilities.GetDefaultTraitsMask(this);
		}

		// Token: 0x040002C8 RID: 712
		private BasicCharacterObject _troop;

		// Token: 0x040002C9 RID: 713
		private bool _hasThrownWeapon;

		// Token: 0x040002CA RID: 714
		private bool _hasHeavyArmor;

		// Token: 0x040002CB RID: 715
		private bool _hasShield;

		// Token: 0x040002CC RID: 716
		private bool _hasSpear;
	}
}
