using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002DC RID: 732
	public class DefaultSiegeStrategies
	{
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x000A8049 File Offset: 0x000A6249
		private static DefaultSiegeStrategies Instance
		{
			get
			{
				return Campaign.Current.DefaultSiegeStrategies;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x000A8055 File Offset: 0x000A6255
		public static SiegeStrategy PreserveStrength
		{
			get
			{
				return DefaultSiegeStrategies.Instance._preserveStrength;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x000A8061 File Offset: 0x000A6261
		public static SiegeStrategy PrepareAgainstAssault
		{
			get
			{
				return DefaultSiegeStrategies.Instance._prepareAgainstAssault;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x000A806D File Offset: 0x000A626D
		public static SiegeStrategy CounterBombardment
		{
			get
			{
				return DefaultSiegeStrategies.Instance._counterBombardment;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x000A8079 File Offset: 0x000A6279
		public static SiegeStrategy PrepareAssault
		{
			get
			{
				return DefaultSiegeStrategies.Instance._prepareAssault;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x000A8085 File Offset: 0x000A6285
		public static SiegeStrategy BreachWalls
		{
			get
			{
				return DefaultSiegeStrategies.Instance._breachWalls;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x000A8091 File Offset: 0x000A6291
		public static SiegeStrategy WearOutDefenders
		{
			get
			{
				return DefaultSiegeStrategies.Instance._wearOutDefenders;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600282C RID: 10284 RVA: 0x000A809D File Offset: 0x000A629D
		public static SiegeStrategy Custom
		{
			get
			{
				return DefaultSiegeStrategies.Instance._custom;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x000A80A9 File Offset: 0x000A62A9
		public static IEnumerable<SiegeStrategy> AllAttackerStrategies
		{
			get
			{
				yield return DefaultSiegeStrategies.PrepareAssault;
				yield return DefaultSiegeStrategies.BreachWalls;
				yield return DefaultSiegeStrategies.WearOutDefenders;
				yield return DefaultSiegeStrategies.PreserveStrength;
				yield return DefaultSiegeStrategies.Custom;
				yield break;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x000A80B2 File Offset: 0x000A62B2
		public static IEnumerable<SiegeStrategy> AllDefenderStrategies
		{
			get
			{
				yield return DefaultSiegeStrategies.PrepareAgainstAssault;
				yield return DefaultSiegeStrategies.CounterBombardment;
				yield return DefaultSiegeStrategies.PreserveStrength;
				yield return DefaultSiegeStrategies.Custom;
				yield break;
			}
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000A80BB File Offset: 0x000A62BB
		public DefaultSiegeStrategies()
		{
			this.RegisterAll();
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000A80CC File Offset: 0x000A62CC
		private void RegisterAll()
		{
			this._preserveStrength = this.Create("siege_strategy_preserve_strength");
			this._prepareAgainstAssault = this.Create("siege_strategy_prepare_against_assault");
			this._counterBombardment = this.Create("siege_strategy_counter_bombardment");
			this._prepareAssault = this.Create("siege_strategy_prepare_assault");
			this._breachWalls = this.Create("siege_strategy_breach_walls");
			this._wearOutDefenders = this.Create("siege_strategy_wear_out_defenders");
			this._custom = this.Create("siege_strategy_custom");
			this.InitializeAll();
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000A8156 File Offset: 0x000A6356
		private SiegeStrategy Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<SiegeStrategy>(new SiegeStrategy(stringId));
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000A8170 File Offset: 0x000A6370
		private void InitializeAll()
		{
			this._custom.Initialize(new TextObject("{=!}Custom", null), new TextObject("{=!}Custom strategy that can be managed entirely.", null));
			this._preserveStrength.Initialize(new TextObject("{=!}Preserve Strength", null), new TextObject("{=!}Priority is set to preserving our strength.", null));
			this._prepareAgainstAssault.Initialize(new TextObject("{=!}Prepare Against Assault", null), new TextObject("{=!}Priority is set to keep advantage when the enemies' assault starts.", null));
			this._counterBombardment.Initialize(new TextObject("{=!}Counter Bombardment", null), new TextObject("{=!}Priority is set to countering enemy bombardment.", null));
			this._prepareAssault.Initialize(new TextObject("{=!}Prepare Assault", null), new TextObject("{=!}Priority is set to assaulting the walls.", null));
			this._breachWalls.Initialize(new TextObject("{=!}Breach Walls", null), new TextObject("{=!}Priority is set to breaching the walls.", null));
			this._wearOutDefenders.Initialize(new TextObject("{=!}Wear out Defenders", null), new TextObject("{=!}Priority is set to destroying engines of the enemy.", null));
		}

		// Token: 0x04000BB8 RID: 3000
		private SiegeStrategy _preserveStrength;

		// Token: 0x04000BB9 RID: 3001
		private SiegeStrategy _prepareAgainstAssault;

		// Token: 0x04000BBA RID: 3002
		private SiegeStrategy _counterBombardment;

		// Token: 0x04000BBB RID: 3003
		private SiegeStrategy _prepareAssault;

		// Token: 0x04000BBC RID: 3004
		private SiegeStrategy _breachWalls;

		// Token: 0x04000BBD RID: 3005
		private SiegeStrategy _wearOutDefenders;

		// Token: 0x04000BBE RID: 3006
		private SiegeStrategy _custom;
	}
}
