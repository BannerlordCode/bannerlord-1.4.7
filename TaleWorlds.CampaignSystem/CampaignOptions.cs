using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005E RID: 94
	public class CampaignOptions
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x000285A7 File Offset: 0x000267A7
		private static CampaignOptions _current
		{
			get
			{
				Campaign campaign = Campaign.Current;
				if (campaign == null)
				{
					return null;
				}
				return campaign.Options;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x000285B9 File Offset: 0x000267B9
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x000285CB File Offset: 0x000267CB
		public static bool IsLifeDeathCycleDisabled
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._isLifeDeathCycleDisabled;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._isLifeDeathCycleDisabled = value;
				}
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x000285DF File Offset: 0x000267DF
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x000285F1 File Offset: 0x000267F1
		public static bool AutoAllocateClanMemberPerks
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._autoAllocateClanMemberPerks;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._autoAllocateClanMemberPerks = value;
				}
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00028605 File Offset: 0x00026805
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x00028617 File Offset: 0x00026817
		public static bool IsIronmanMode
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				return current != null && current._isIronmanMode;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._isIronmanMode = value;
				}
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0002862B File Offset: 0x0002682B
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x0002863D File Offset: 0x0002683D
		public static CampaignOptions.Difficulty PlayerTroopsReceivedDamage
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._playerTroopsReceivedDamage;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._playerTroopsReceivedDamage = value;
				}
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x00028651 File Offset: 0x00026851
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x00028663 File Offset: 0x00026863
		public static CampaignOptions.Difficulty RecruitmentDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._recruitmentDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._recruitmentDifficulty = value;
				}
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00028677 File Offset: 0x00026877
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x00028689 File Offset: 0x00026889
		public static CampaignOptions.Difficulty PlayerMapMovementSpeed
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._playerMapMovementSpeed;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._playerMapMovementSpeed = value;
				}
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0002869D File Offset: 0x0002689D
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x000286AF File Offset: 0x000268AF
		public static CampaignOptions.Difficulty StealthAndDisguiseDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._stealthAndDisguiseDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._stealthAndDisguiseDifficulty = value;
				}
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x000286C3 File Offset: 0x000268C3
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x000286D5 File Offset: 0x000268D5
		public static CampaignOptions.Difficulty CombatAIDifficulty
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._combatAIDifficulty;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._combatAIDifficulty = value;
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x000286E9 File Offset: 0x000268E9
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x000286FB File Offset: 0x000268FB
		public static CampaignOptions.Difficulty PersuasionSuccessChance
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._persuasionSuccessChance;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._persuasionSuccessChance = value;
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0002870F File Offset: 0x0002690F
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x00028721 File Offset: 0x00026921
		public static CampaignOptions.Difficulty ClanMemberDeathChance
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._clanMemberDeathChance;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._clanMemberDeathChance = value;
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00028735 File Offset: 0x00026935
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00028747 File Offset: 0x00026947
		public static CampaignOptions.Difficulty BattleDeath
		{
			get
			{
				CampaignOptions current = CampaignOptions._current;
				if (current == null)
				{
					return CampaignOptions.Difficulty.Realistic;
				}
				return current._battleDeath;
			}
			set
			{
				if (CampaignOptions._current != null)
				{
					CampaignOptions._current._battleDeath = value;
				}
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002875C File Offset: 0x0002695C
		public CampaignOptions()
		{
			this._playerTroopsReceivedDamage = CampaignOptions.Difficulty.VeryEasy;
			this._recruitmentDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._playerMapMovementSpeed = CampaignOptions.Difficulty.VeryEasy;
			this._combatAIDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._persuasionSuccessChance = CampaignOptions.Difficulty.VeryEasy;
			this._clanMemberDeathChance = CampaignOptions.Difficulty.VeryEasy;
			this._battleDeath = CampaignOptions.Difficulty.VeryEasy;
			this._stealthAndDisguiseDifficulty = CampaignOptions.Difficulty.VeryEasy;
			this._isLifeDeathCycleDisabled = false;
			this._autoAllocateClanMemberPerks = false;
			this._isIronmanMode = false;
			this.AccelerationMode = GameAccelerationMode.Default;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x000287C3 File Offset: 0x000269C3
		internal static void AutoGeneratedStaticCollectObjectsCampaignOptions(object o, List<object> collectedObjects)
		{
			((CampaignOptions)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000287D1 File Offset: 0x000269D1
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000287D3 File Offset: 0x000269D3
		internal static object AutoGeneratedGetMemberValueAccelerationMode(object o)
		{
			return ((CampaignOptions)o).AccelerationMode;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000287E5 File Offset: 0x000269E5
		internal static object AutoGeneratedGetMemberValue_autoAllocateClanMemberPerks(object o)
		{
			return ((CampaignOptions)o)._autoAllocateClanMemberPerks;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x000287F7 File Offset: 0x000269F7
		internal static object AutoGeneratedGetMemberValue_playerTroopsReceivedDamage(object o)
		{
			return ((CampaignOptions)o)._playerTroopsReceivedDamage;
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00028809 File Offset: 0x00026A09
		internal static object AutoGeneratedGetMemberValue_recruitmentDifficulty(object o)
		{
			return ((CampaignOptions)o)._recruitmentDifficulty;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002881B File Offset: 0x00026A1B
		internal static object AutoGeneratedGetMemberValue_playerMapMovementSpeed(object o)
		{
			return ((CampaignOptions)o)._playerMapMovementSpeed;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002882D File Offset: 0x00026A2D
		internal static object AutoGeneratedGetMemberValue_stealthAndDisguiseDifficulty(object o)
		{
			return ((CampaignOptions)o)._stealthAndDisguiseDifficulty;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0002883F File Offset: 0x00026A3F
		internal static object AutoGeneratedGetMemberValue_combatAIDifficulty(object o)
		{
			return ((CampaignOptions)o)._combatAIDifficulty;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00028851 File Offset: 0x00026A51
		internal static object AutoGeneratedGetMemberValue_isLifeDeathCycleDisabled(object o)
		{
			return ((CampaignOptions)o)._isLifeDeathCycleDisabled;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00028863 File Offset: 0x00026A63
		internal static object AutoGeneratedGetMemberValue_persuasionSuccessChance(object o)
		{
			return ((CampaignOptions)o)._persuasionSuccessChance;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00028875 File Offset: 0x00026A75
		internal static object AutoGeneratedGetMemberValue_clanMemberDeathChance(object o)
		{
			return ((CampaignOptions)o)._clanMemberDeathChance;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00028887 File Offset: 0x00026A87
		internal static object AutoGeneratedGetMemberValue_isIronmanMode(object o)
		{
			return ((CampaignOptions)o)._isIronmanMode;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00028899 File Offset: 0x00026A99
		internal static object AutoGeneratedGetMemberValue_battleDeath(object o)
		{
			return ((CampaignOptions)o)._battleDeath;
		}

		// Token: 0x040002E4 RID: 740
		[SaveableField(4)]
		private bool _autoAllocateClanMemberPerks;

		// Token: 0x040002E5 RID: 741
		[SaveableField(5)]
		private CampaignOptions.Difficulty _playerTroopsReceivedDamage;

		// Token: 0x040002E6 RID: 742
		[SaveableField(8)]
		private CampaignOptions.Difficulty _recruitmentDifficulty;

		// Token: 0x040002E7 RID: 743
		[SaveableField(9)]
		private CampaignOptions.Difficulty _playerMapMovementSpeed;

		// Token: 0x040002E8 RID: 744
		[SaveableField(18)]
		private CampaignOptions.Difficulty _stealthAndDisguiseDifficulty;

		// Token: 0x040002E9 RID: 745
		[SaveableField(11)]
		private CampaignOptions.Difficulty _combatAIDifficulty;

		// Token: 0x040002EA RID: 746
		[SaveableField(12)]
		private bool _isLifeDeathCycleDisabled;

		// Token: 0x040002EB RID: 747
		[SaveableField(13)]
		private CampaignOptions.Difficulty _persuasionSuccessChance;

		// Token: 0x040002EC RID: 748
		[SaveableField(14)]
		private CampaignOptions.Difficulty _clanMemberDeathChance;

		// Token: 0x040002ED RID: 749
		[SaveableField(15)]
		private bool _isIronmanMode;

		// Token: 0x040002EE RID: 750
		[SaveableField(17)]
		private CampaignOptions.Difficulty _battleDeath;

		// Token: 0x040002EF RID: 751
		[SaveableField(19)]
		public GameAccelerationMode AccelerationMode;

		// Token: 0x0200051A RID: 1306
		public enum Difficulty : short
		{
			// Token: 0x040015ED RID: 5613
			VeryEasy,
			// Token: 0x040015EE RID: 5614
			Easy,
			// Token: 0x040015EF RID: 5615
			Realistic
		}
	}
}
