using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000421 RID: 1057
	public class OrderOfBattleCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004380 RID: 17280 RVA: 0x0014793C File Offset: 0x00145B3C
		public OrderOfBattleCampaignBehavior()
		{
			this._siegeFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			this._siegeArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			this._fieldBattleFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			this._fieldBattleArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x00147970 File Offset: 0x00145B70
		public override void RegisterEvents()
		{
			CampaignEvents.OnHeroUnregisteredEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroUnregistered));
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0014798C File Offset: 0x00145B8C
		public override void SyncData(IDataStore dataStore)
		{
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_siegeFormationInfos", ref this._siegeFormationInfos) && this._siegeFormationInfos == null)
			{
				this._siegeFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_siegeArmyFormationInfos", ref this._siegeArmyFormationInfos) && this._siegeArmyFormationInfos == null)
			{
				this._siegeArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_formationInfos", ref this._fieldBattleFormationInfos) && this._fieldBattleFormationInfos == null)
			{
				this._fieldBattleFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_fieldBattleArmyFormationInfos", ref this._fieldBattleArmyFormationInfos) && this._fieldBattleArmyFormationInfos == null)
			{
				this._fieldBattleArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x00147A34 File Offset: 0x00145C34
		public OrderOfBattleCampaignBehavior.OrderOfBattleFormationData GetFormationDataAtIndex(int formationIndex, bool isSiegeBattle, bool isInArmy)
		{
			if (isSiegeBattle)
			{
				if (isInArmy)
				{
					if (this._siegeArmyFormationInfos.Count > formationIndex)
					{
						return this._siegeArmyFormationInfos[formationIndex];
					}
					return null;
				}
				else
				{
					if (this._siegeFormationInfos.Count > formationIndex)
					{
						return this._siegeFormationInfos[formationIndex];
					}
					return null;
				}
			}
			else if (isInArmy)
			{
				if (this._fieldBattleArmyFormationInfos.Count > formationIndex)
				{
					return this._fieldBattleArmyFormationInfos[formationIndex];
				}
				return null;
			}
			else
			{
				if (this._fieldBattleFormationInfos.Count > formationIndex)
				{
					return this._fieldBattleFormationInfos[formationIndex];
				}
				return null;
			}
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x00147ABD File Offset: 0x00145CBD
		public void SetFormationInfos(List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> formationInfos, bool isSiegeBattle, bool isInArmy)
		{
			if (isSiegeBattle)
			{
				if (isInArmy)
				{
					this._siegeArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
					return;
				}
				this._siegeFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
				return;
			}
			else
			{
				if (isInArmy)
				{
					this._fieldBattleArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
					return;
				}
				this._fieldBattleFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
				return;
			}
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x00147AFC File Offset: 0x00145CFC
		private void OnHeroUnregistered(Hero hero)
		{
			int i = this._siegeFormationInfos.Count - 1;
			Func<Hero, bool> <>9__0;
			while (i >= 0)
			{
				OrderOfBattleCampaignBehavior.OrderOfBattleFormationData orderOfBattleFormationData = this._siegeFormationInfos[i];
				if (orderOfBattleFormationData.Captain == hero)
				{
					goto IL_0055;
				}
				Hero[] heroTroops = orderOfBattleFormationData.HeroTroops;
				if (heroTroops != null && heroTroops.Contains(hero))
				{
					goto IL_0055;
				}
				IL_00D3:
				i--;
				continue;
				IL_0055:
				Hero[] heroTroops2 = orderOfBattleFormationData.HeroTroops;
				Hero[] array;
				if (heroTroops2 == null)
				{
					array = null;
				}
				else
				{
					Func<Hero, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (Hero t) => t != hero);
					}
					array = heroTroops2.Where<Hero>(func).ToArray<Hero>();
				}
				Hero[] array2 = array;
				Hero hero2 = ((orderOfBattleFormationData.Captain == hero) ? null : orderOfBattleFormationData.Captain);
				this._siegeFormationInfos[i] = new OrderOfBattleCampaignBehavior.OrderOfBattleFormationData(hero2, array2, orderOfBattleFormationData.FormationClass, orderOfBattleFormationData.PrimaryClassWeight, orderOfBattleFormationData.SecondaryClassWeight, orderOfBattleFormationData.Filters);
				goto IL_00D3;
			}
			int j = this._fieldBattleFormationInfos.Count - 1;
			Func<Hero, bool> <>9__1;
			while (j >= 0)
			{
				OrderOfBattleCampaignBehavior.OrderOfBattleFormationData orderOfBattleFormationData2 = this._fieldBattleFormationInfos[j];
				if (orderOfBattleFormationData2.Captain == hero)
				{
					goto IL_012E;
				}
				Hero[] heroTroops3 = orderOfBattleFormationData2.HeroTroops;
				if (heroTroops3 != null && heroTroops3.Contains(hero))
				{
					goto IL_012E;
				}
				IL_01B6:
				j--;
				continue;
				IL_012E:
				Hero[] heroTroops4 = orderOfBattleFormationData2.HeroTroops;
				Hero[] array3;
				if (heroTroops4 == null)
				{
					array3 = null;
				}
				else
				{
					Func<Hero, bool> func2;
					if ((func2 = <>9__1) == null)
					{
						func2 = (<>9__1 = (Hero t) => t != hero);
					}
					array3 = heroTroops4.Where<Hero>(func2).ToArray<Hero>();
				}
				Hero[] array4 = array3;
				Hero hero3 = ((orderOfBattleFormationData2.Captain == hero) ? null : orderOfBattleFormationData2.Captain);
				this._fieldBattleFormationInfos[j] = new OrderOfBattleCampaignBehavior.OrderOfBattleFormationData(hero3, array4, orderOfBattleFormationData2.FormationClass, orderOfBattleFormationData2.PrimaryClassWeight, orderOfBattleFormationData2.SecondaryClassWeight, orderOfBattleFormationData2.Filters);
				goto IL_01B6;
			}
		}

		// Token: 0x04001349 RID: 4937
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _siegeFormationInfos;

		// Token: 0x0400134A RID: 4938
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _siegeArmyFormationInfos;

		// Token: 0x0400134B RID: 4939
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _fieldBattleFormationInfos;

		// Token: 0x0400134C RID: 4940
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _fieldBattleArmyFormationInfos;

		// Token: 0x0200082C RID: 2092
		public class OrderOfBattleFormationData
		{
			// Token: 0x06006748 RID: 26440 RVA: 0x001C9060 File Offset: 0x001C7260
			public OrderOfBattleFormationData(Hero captain, Hero[] heroTroops, DeploymentFormationClass formationClass, int primaryWeight, int secondaryWeight, Dictionary<FormationFilterType, bool> filters)
			{
				this.Captain = captain;
				this.HeroTroops = heroTroops;
				this.FormationClass = formationClass;
				this.PrimaryClassWeight = primaryWeight;
				this.SecondaryClassWeight = secondaryWeight;
				this.Filters = new Dictionary<FormationFilterType, bool>();
				foreach (FormationFilterType formationFilterType in filters.Keys)
				{
					this.Filters.Add(formationFilterType, filters[formationFilterType]);
				}
			}

			// Token: 0x06006749 RID: 26441 RVA: 0x001C90F8 File Offset: 0x001C72F8
			internal static void AutoGeneratedStaticCollectObjectsOrderOfBattleFormationData(object o, List<object> collectedObjects)
			{
				((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600674A RID: 26442 RVA: 0x001C9106 File Offset: 0x001C7306
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Captain);
				collectedObjects.Add(this.Filters);
				collectedObjects.Add(this.HeroTroops);
			}

			// Token: 0x0600674B RID: 26443 RVA: 0x001C912C File Offset: 0x001C732C
			internal static object AutoGeneratedGetMemberValueCaptain(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).Captain;
			}

			// Token: 0x0600674C RID: 26444 RVA: 0x001C9139 File Offset: 0x001C7339
			internal static object AutoGeneratedGetMemberValueFormationClass(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).FormationClass;
			}

			// Token: 0x0600674D RID: 26445 RVA: 0x001C914B File Offset: 0x001C734B
			internal static object AutoGeneratedGetMemberValuePrimaryClassWeight(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).PrimaryClassWeight;
			}

			// Token: 0x0600674E RID: 26446 RVA: 0x001C915D File Offset: 0x001C735D
			internal static object AutoGeneratedGetMemberValueSecondaryClassWeight(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).SecondaryClassWeight;
			}

			// Token: 0x0600674F RID: 26447 RVA: 0x001C916F File Offset: 0x001C736F
			internal static object AutoGeneratedGetMemberValueFilters(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).Filters;
			}

			// Token: 0x06006750 RID: 26448 RVA: 0x001C917C File Offset: 0x001C737C
			internal static object AutoGeneratedGetMemberValueHeroTroops(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).HeroTroops;
			}

			// Token: 0x04002316 RID: 8982
			[SaveableField(1)]
			public readonly Hero Captain;

			// Token: 0x04002317 RID: 8983
			[SaveableField(2)]
			public readonly DeploymentFormationClass FormationClass;

			// Token: 0x04002318 RID: 8984
			[SaveableField(3)]
			public readonly int PrimaryClassWeight;

			// Token: 0x04002319 RID: 8985
			[SaveableField(4)]
			public readonly int SecondaryClassWeight;

			// Token: 0x0400231A RID: 8986
			[SaveableField(5)]
			public readonly Dictionary<FormationFilterType, bool> Filters;

			// Token: 0x0400231B RID: 8987
			[SaveableField(6)]
			public readonly Hero[] HeroTroops;
		}
	}
}
