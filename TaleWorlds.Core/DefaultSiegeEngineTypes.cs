using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000054 RID: 84
	public class DefaultSiegeEngineTypes
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00017B54 File Offset: 0x00015D54
		private static DefaultSiegeEngineTypes Instance
		{
			get
			{
				return Game.Current.DefaultSiegeEngineTypes;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00017B60 File Offset: 0x00015D60
		public static SiegeEngineType Preparations
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypePreparations;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00017B6C File Offset: 0x00015D6C
		public static SiegeEngineType Ladder
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeLadder;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00017B78 File Offset: 0x00015D78
		public static SiegeEngineType Ballista
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeBallista;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00017B84 File Offset: 0x00015D84
		public static SiegeEngineType FireBallista
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireBallista;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00017B90 File Offset: 0x00015D90
		public static SiegeEngineType Ram
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeRam;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00017B9C File Offset: 0x00015D9C
		public static SiegeEngineType ImprovedRam
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeImprovedRam;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00017BA8 File Offset: 0x00015DA8
		public static SiegeEngineType SiegeTower
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeSiegeTower;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00017BB4 File Offset: 0x00015DB4
		public static SiegeEngineType HeavySiegeTower
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeHeavySiegeTower;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00017BC0 File Offset: 0x00015DC0
		public static SiegeEngineType Catapult
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeCatapult;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00017BCC File Offset: 0x00015DCC
		public static SiegeEngineType FireCatapult
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireCatapult;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00017BD8 File Offset: 0x00015DD8
		public static SiegeEngineType Onager
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeOnager;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00017BE4 File Offset: 0x00015DE4
		public static SiegeEngineType FireOnager
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireOnager;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00017BF0 File Offset: 0x00015DF0
		public static SiegeEngineType Bricole
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeBricole;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00017BFC File Offset: 0x00015DFC
		public static SiegeEngineType Trebuchet
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeTrebuchet;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00017C08 File Offset: 0x00015E08
		public static SiegeEngineType FireTrebuchet
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeTrebuchet;
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00017C14 File Offset: 0x00015E14
		public DefaultSiegeEngineTypes()
		{
			this.RegisterAll();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00017C24 File Offset: 0x00015E24
		private void RegisterAll()
		{
			Game.Current.ObjectManager.LoadXML("SiegeEngines", false);
			this._siegeEngineTypePreparations = this.GetSiegeEngine("preparations");
			this._siegeEngineTypeLadder = this.GetSiegeEngine("ladder");
			this._siegeEngineTypeSiegeTower = this.GetSiegeEngine("siege_tower_level1");
			this._siegeEngineTypeHeavySiegeTower = this.GetSiegeEngine("siege_tower_level2");
			this._siegeEngineTypeBallista = this.GetSiegeEngine("ballista");
			this._siegeEngineTypeFireBallista = this.GetSiegeEngine("fire_ballista");
			this._siegeEngineTypeCatapult = this.GetSiegeEngine("catapult");
			this._siegeEngineTypeFireCatapult = this.GetSiegeEngine("fire_catapult");
			this._siegeEngineTypeOnager = this.GetSiegeEngine("onager");
			this._siegeEngineTypeFireOnager = this.GetSiegeEngine("fire_onager");
			this._siegeEngineTypeBricole = this.GetSiegeEngine("bricole");
			this._siegeEngineTypeTrebuchet = this.GetSiegeEngine("trebuchet");
			this._siegeEngineTypeFireTrebuchet = this.GetSiegeEngine("fire_trebuchet");
			this._siegeEngineTypeRam = this.GetSiegeEngine("ram");
			this._siegeEngineTypeImprovedRam = this.GetSiegeEngine("improved_ram");
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00017D45 File Offset: 0x00015F45
		private SiegeEngineType GetSiegeEngine(string id)
		{
			return Game.Current.ObjectManager.GetObject<SiegeEngineType>(id);
		}

		// Token: 0x04000353 RID: 851
		private SiegeEngineType _siegeEngineTypePreparations;

		// Token: 0x04000354 RID: 852
		private SiegeEngineType _siegeEngineTypeLadder;

		// Token: 0x04000355 RID: 853
		private SiegeEngineType _siegeEngineTypeBallista;

		// Token: 0x04000356 RID: 854
		private SiegeEngineType _siegeEngineTypeFireBallista;

		// Token: 0x04000357 RID: 855
		private SiegeEngineType _siegeEngineTypeRam;

		// Token: 0x04000358 RID: 856
		private SiegeEngineType _siegeEngineTypeImprovedRam;

		// Token: 0x04000359 RID: 857
		private SiegeEngineType _siegeEngineTypeSiegeTower;

		// Token: 0x0400035A RID: 858
		private SiegeEngineType _siegeEngineTypeHeavySiegeTower;

		// Token: 0x0400035B RID: 859
		private SiegeEngineType _siegeEngineTypeCatapult;

		// Token: 0x0400035C RID: 860
		private SiegeEngineType _siegeEngineTypeFireCatapult;

		// Token: 0x0400035D RID: 861
		private SiegeEngineType _siegeEngineTypeOnager;

		// Token: 0x0400035E RID: 862
		private SiegeEngineType _siegeEngineTypeFireOnager;

		// Token: 0x0400035F RID: 863
		private SiegeEngineType _siegeEngineTypeBricole;

		// Token: 0x04000360 RID: 864
		private SiegeEngineType _siegeEngineTypeTrebuchet;

		// Token: 0x04000361 RID: 865
		private SiegeEngineType _siegeEngineTypeFireTrebuchet;
	}
}
