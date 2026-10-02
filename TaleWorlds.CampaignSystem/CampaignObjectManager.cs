using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200005D RID: 93
	public class CampaignObjectManager
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x0002789F File Offset: 0x00025A9F
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x000278A7 File Offset: 0x00025AA7
		[SaveableProperty(80)]
		public MBReadOnlyList<Settlement> Settlements { get; private set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x000278B0 File Offset: 0x00025AB0
		public MBReadOnlyList<MobileParty> MobileParties
		{
			get
			{
				return this._mobileParties;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x000278B8 File Offset: 0x00025AB8
		public MBReadOnlyList<MobileParty> CaravanParties
		{
			get
			{
				return this._caravanParties;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x000278C0 File Offset: 0x00025AC0
		public MBReadOnlyList<MobileParty> PatrolParties
		{
			get
			{
				return this._patrolParties;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x000278C8 File Offset: 0x00025AC8
		public MBReadOnlyList<MobileParty> MilitiaParties
		{
			get
			{
				return this._militiaParties;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x000278D0 File Offset: 0x00025AD0
		public MBReadOnlyList<MobileParty> GarrisonParties
		{
			get
			{
				return this._garrisonParties;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x000278D8 File Offset: 0x00025AD8
		public MBReadOnlyList<MobileParty> BanditParties
		{
			get
			{
				return this._banditParties;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x000278E0 File Offset: 0x00025AE0
		public MBReadOnlyList<MobileParty> VillagerParties
		{
			get
			{
				return this._villagerParties;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x000278E8 File Offset: 0x00025AE8
		public MBReadOnlyList<MobileParty> LordParties
		{
			get
			{
				return this._lordParties;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x000278F0 File Offset: 0x00025AF0
		public MBReadOnlyList<MobileParty> CustomParties
		{
			get
			{
				return this._customParties;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000278F8 File Offset: 0x00025AF8
		public MBReadOnlyList<MobileParty> PartiesWithoutPartyComponent
		{
			get
			{
				return this._partiesWithoutPartyComponent;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x00027900 File Offset: 0x00025B00
		public MBReadOnlyList<Hero> AliveHeroes
		{
			get
			{
				return this._aliveHeroes;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00027908 File Offset: 0x00025B08
		public MBReadOnlyList<Hero> DeadOrDisabledHeroes
		{
			get
			{
				return this._deadOrDisabledHeroes;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x00027910 File Offset: 0x00025B10
		public MBReadOnlyList<Clan> Clans
		{
			get
			{
				return this._clans;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00027918 File Offset: 0x00025B18
		public MBReadOnlyList<Kingdom> Kingdoms
		{
			get
			{
				return this._kingdoms;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x00027920 File Offset: 0x00025B20
		public MBReadOnlyList<IFaction> Factions
		{
			get
			{
				return this._factions;
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00027928 File Offset: 0x00025B28
		public CampaignObjectManager()
		{
			this._objects = new CampaignObjectManager.ICampaignObjectType[5];
			this._mobileParties = new MBList<MobileParty>();
			this._caravanParties = new MBList<MobileParty>();
			this._patrolParties = new MBList<MobileParty>();
			this._militiaParties = new MBList<MobileParty>();
			this._garrisonParties = new MBList<MobileParty>();
			this._customParties = new MBList<MobileParty>();
			this._banditParties = new MBList<MobileParty>();
			this._villagerParties = new MBList<MobileParty>();
			this._lordParties = new MBList<MobileParty>();
			this._partiesWithoutPartyComponent = new MBList<MobileParty>();
			this._deadOrDisabledHeroes = new MBList<Hero>();
			this._aliveHeroes = new MBList<Hero>();
			this._clans = new MBList<Clan>();
			this._kingdoms = new MBList<Kingdom>();
			this._factions = new MBList<IFaction>();
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000279EC File Offset: 0x00025BEC
		private void InitializeManagerObjectLists()
		{
			this._objects[4] = new CampaignObjectManager.CampaignObjectType<MobileParty>(this._mobileParties);
			this._objects[0] = new CampaignObjectManager.CampaignObjectType<Hero>(this._deadOrDisabledHeroes);
			this._objects[1] = new CampaignObjectManager.CampaignObjectType<Hero>(this._aliveHeroes);
			this._objects[2] = new CampaignObjectManager.CampaignObjectType<Clan>(this._clans);
			this._objects[3] = new CampaignObjectManager.CampaignObjectType<Kingdom>(this._kingdoms);
			this._objectTypesAndNextIds = new Dictionary<Type, uint>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				uint maxObjectSubId = campaignObjectType.GetMaxObjectSubId();
				uint num;
				if (this._objectTypesAndNextIds.TryGetValue(campaignObjectType.ObjectClass, out num))
				{
					if (num <= maxObjectSubId)
					{
						this._objectTypesAndNextIds[campaignObjectType.ObjectClass] = maxObjectSubId + 1U;
					}
				}
				else
				{
					this._objectTypesAndNextIds.Add(campaignObjectType.ObjectClass, maxObjectSubId + 1U);
				}
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00027AC8 File Offset: 0x00025CC8
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData, ObjectLoadData objectLoadData)
		{
			this._objects = new CampaignObjectManager.ICampaignObjectType[5];
			this._factions = new MBList<IFaction>();
			this._caravanParties = new MBList<MobileParty>();
			this._patrolParties = new MBList<MobileParty>();
			this._militiaParties = new MBList<MobileParty>();
			this._garrisonParties = new MBList<MobileParty>();
			this._customParties = new MBList<MobileParty>();
			this._banditParties = new MBList<MobileParty>();
			this._villagerParties = new MBList<MobileParty>();
			this._lordParties = new MBList<MobileParty>();
			this._partiesWithoutPartyComponent = new MBList<MobileParty>();
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00027B50 File Offset: 0x00025D50
		internal void PreAfterLoad()
		{
			CampaignObjectManager.ICampaignObjectType[] objects = this._objects;
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].PreAfterLoad();
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00027B7C File Offset: 0x00025D7C
		internal void AfterLoad()
		{
			CampaignObjectManager.ICampaignObjectType[] objects = this._objects;
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].AfterLoad();
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00027BA8 File Offset: 0x00025DA8
		internal void InitializeOnLoad()
		{
			this.Settlements = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			foreach (Clan clan in this._clans)
			{
				if (!this._factions.Contains(clan))
				{
					this._factions.Add(clan);
				}
			}
			foreach (Kingdom kingdom in this._kingdoms)
			{
				if (!this._factions.Contains(kingdom))
				{
					this._factions.Add(kingdom);
				}
			}
			foreach (MobileParty mobileParty in this._mobileParties)
			{
				mobileParty.UpdatePartyComponentFlags();
				this.AddPartyToAppropriateList(mobileParty);
			}
			this.InitializeManagerObjectLists();
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00027CC8 File Offset: 0x00025EC8
		internal void InitializeOnNewGame()
		{
			List<Hero> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<Hero>();
			MBReadOnlyList<MobileParty> objectTypeList2 = MBObjectManager.Instance.GetObjectTypeList<MobileParty>();
			MBReadOnlyList<Clan> objectTypeList3 = MBObjectManager.Instance.GetObjectTypeList<Clan>();
			MBReadOnlyList<Kingdom> objectTypeList4 = MBObjectManager.Instance.GetObjectTypeList<Kingdom>();
			this.Settlements = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			foreach (Hero hero in objectTypeList)
			{
				if (hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled)
				{
					if (!this._deadOrDisabledHeroes.Contains(hero))
					{
						this._deadOrDisabledHeroes.Add(hero);
					}
				}
				else if (!this._aliveHeroes.Contains(hero))
				{
					this._aliveHeroes.Add(hero);
				}
			}
			foreach (Clan clan in objectTypeList3)
			{
				if (!this._clans.Contains(clan))
				{
					this._clans.Add(clan);
				}
				if (!this._factions.Contains(clan))
				{
					this._factions.Add(clan);
				}
			}
			foreach (Kingdom kingdom in objectTypeList4)
			{
				if (!this._kingdoms.Contains(kingdom))
				{
					this._kingdoms.Add(kingdom);
				}
				if (!this._factions.Contains(kingdom))
				{
					this._factions.Add(kingdom);
				}
			}
			foreach (MobileParty mobileParty in objectTypeList2)
			{
				this._mobileParties.Add(mobileParty);
				this.AddPartyToAppropriateList(mobileParty);
			}
			this.InitializeManagerObjectLists();
			this.InitializeCachedData();
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00027ED4 File Offset: 0x000260D4
		private void InitializeCachedData()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsVillage)
				{
					settlement.OwnerClan.OnBoundVillageAdded(settlement.Village);
				}
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00027F38 File Offset: 0x00026138
		internal void AddMobileParty(MobileParty party)
		{
			party.Id = new MBGUID(14U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<MobileParty>());
			this._mobileParties.Add(party);
			this.OnItemAdded<MobileParty>(CampaignObjectManager.CampaignObjects.MobileParty, party);
			this.AddPartyToAppropriateList(party);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00027F71 File Offset: 0x00026171
		internal void RemoveMobileParty(MobileParty party)
		{
			this._mobileParties.Remove(party);
			this.OnItemRemoved<MobileParty>(CampaignObjectManager.CampaignObjects.MobileParty, party);
			this.RemovePartyFromAppropriateList(party);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00027F8F File Offset: 0x0002618F
		internal void BeforePartyComponentChanged(MobileParty party)
		{
			this.RemovePartyFromAppropriateList(party);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00027F98 File Offset: 0x00026198
		internal void AfterPartyComponentChanged(MobileParty party)
		{
			this.AddPartyToAppropriateList(party);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00027FA1 File Offset: 0x000261A1
		internal void AddHero(Hero hero)
		{
			hero.Id = new MBGUID(32U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Hero>());
			this.OnHeroAdded(hero);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00027FC6 File Offset: 0x000261C6
		internal void UnregisterDeadHero(Hero hero)
		{
			this._deadOrDisabledHeroes.Remove(hero);
			this.OnItemRemoved<Hero>(CampaignObjectManager.CampaignObjects.DeadOrDisabledHeroes, hero);
			CampaignEventDispatcher.Instance.OnHeroUnregistered(hero);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00027FE8 File Offset: 0x000261E8
		private void OnHeroAdded(Hero hero)
		{
			if (hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled)
			{
				this._deadOrDisabledHeroes.Add(hero);
				this.OnItemAdded<Hero>(CampaignObjectManager.CampaignObjects.DeadOrDisabledHeroes, hero);
				return;
			}
			this._aliveHeroes.Add(hero);
			this.OnItemAdded<Hero>(CampaignObjectManager.CampaignObjects.AliveHeroes, hero);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00028028 File Offset: 0x00026228
		internal void HeroStateChanged(Hero hero, Hero.CharacterStates oldState)
		{
			bool flag = oldState == Hero.CharacterStates.Dead || oldState == Hero.CharacterStates.Disabled;
			bool flag2 = hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled;
			if (flag != flag2)
			{
				if (flag2)
				{
					if (this._aliveHeroes.Contains(hero))
					{
						this._aliveHeroes.Remove(hero);
					}
				}
				else if (this._deadOrDisabledHeroes.Contains(hero))
				{
					this._deadOrDisabledHeroes.Remove(hero);
				}
				this.OnHeroAdded(hero);
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002809B File Offset: 0x0002629B
		internal void AddClan(Clan clan)
		{
			clan.Id = new MBGUID(18U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Clan>());
			this._clans.Add(clan);
			this.OnItemAdded<Clan>(CampaignObjectManager.CampaignObjects.Clans, clan);
			this._factions.Add(clan);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000280D9 File Offset: 0x000262D9
		internal void RemoveClan(Clan clan)
		{
			if (this._clans.Contains(clan))
			{
				this._clans.Remove(clan);
				this.OnItemRemoved<Clan>(CampaignObjectManager.CampaignObjects.Clans, clan);
			}
			if (this._factions.Contains(clan))
			{
				this._factions.Remove(clan);
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00028119 File Offset: 0x00026319
		internal void AddKingdom(Kingdom kingdom)
		{
			kingdom.Id = new MBGUID(20U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Kingdom>());
			this._kingdoms.Add(kingdom);
			this.OnItemAdded<Kingdom>(CampaignObjectManager.CampaignObjects.Kingdoms, kingdom);
			this._factions.Add(kingdom);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00028158 File Offset: 0x00026358
		private void AddPartyToAppropriateList(MobileParty party)
		{
			if (party.IsBandit)
			{
				this._banditParties.Add(party);
				return;
			}
			if (party.IsCaravan)
			{
				this._caravanParties.Add(party);
				return;
			}
			if (party.IsPatrolParty)
			{
				this._patrolParties.Add(party);
				return;
			}
			if (party.IsLordParty)
			{
				this._lordParties.Add(party);
				return;
			}
			if (party.IsMilitia)
			{
				this._militiaParties.Add(party);
				return;
			}
			if (party.IsVillager)
			{
				this._villagerParties.Add(party);
				return;
			}
			if (party.IsCustomParty)
			{
				this._customParties.Add(party);
				return;
			}
			if (party.IsGarrison)
			{
				this._garrisonParties.Add(party);
				return;
			}
			this._partiesWithoutPartyComponent.Add(party);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002821C File Offset: 0x0002641C
		private void RemovePartyFromAppropriateList(MobileParty party)
		{
			if (party.IsBandit)
			{
				this._banditParties.Remove(party);
				return;
			}
			if (party.IsCaravan)
			{
				this._caravanParties.Remove(party);
				return;
			}
			if (party.IsPatrolParty)
			{
				this._patrolParties.Remove(party);
				return;
			}
			if (party.IsLordParty)
			{
				this._lordParties.Remove(party);
				return;
			}
			if (party.IsMilitia)
			{
				this._militiaParties.Remove(party);
				return;
			}
			if (party.IsVillager)
			{
				this._villagerParties.Remove(party);
				return;
			}
			if (party.IsCustomParty)
			{
				this._customParties.Remove(party);
				return;
			}
			if (party.IsGarrison)
			{
				this._garrisonParties.Remove(party);
				return;
			}
			this._partiesWithoutPartyComponent.Remove(party);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000282E8 File Offset: 0x000264E8
		private void OnItemAdded<T>(CampaignObjectManager.CampaignObjects targetList, T obj) where T : MBObjectBase
		{
			CampaignObjectManager.CampaignObjectType<T> campaignObjectType = (CampaignObjectManager.CampaignObjectType<T>)this._objects[(int)targetList];
			if (campaignObjectType != null)
			{
				campaignObjectType.OnItemAdded(obj);
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00028310 File Offset: 0x00026510
		private void OnItemRemoved<T>(CampaignObjectManager.CampaignObjects targetList, T obj) where T : MBObjectBase
		{
			CampaignObjectManager.CampaignObjectType<T> campaignObjectType = (CampaignObjectManager.CampaignObjectType<T>)this._objects[(int)targetList];
			if (campaignObjectType != null)
			{
				campaignObjectType.UnregisterItem(obj);
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00028338 File Offset: 0x00026538
		public T FindFirst<T>(Predicate<T> predicate) where T : MBObjectBase
		{
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (typeof(T) == campaignObjectType.ObjectClass)
				{
					T t = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).FindFirst(predicate);
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00028398 File Offset: 0x00026598
		public MBReadOnlyList<T> FindAll<T>(Predicate<T> predicate) where T : MBObjectBase
		{
			MBList<T> mblist = new MBList<T>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (typeof(T) == campaignObjectType.ObjectClass)
				{
					MBReadOnlyList<T> mbreadOnlyList = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).FindAll(predicate);
					if (mbreadOnlyList != null)
					{
						mblist.AddRange(mbreadOnlyList);
					}
				}
			}
			return mblist;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000283F8 File Offset: 0x000265F8
		private uint GetNextUniqueObjectIdOfType<T>() where T : MBObjectBase
		{
			uint num;
			if (this._objectTypesAndNextIds.TryGetValue(typeof(T), out num))
			{
				this._objectTypesAndNextIds[typeof(T)] = num + 1U;
			}
			return num;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00028438 File Offset: 0x00026638
		public T Find<T>(string id) where T : MBObjectBase
		{
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (campaignObjectType != null && typeof(T) == campaignObjectType.ObjectClass)
				{
					T t = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).Find(id);
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002849C File Offset: 0x0002669C
		public string FindNextUniqueStringId<T>(string id) where T : MBObjectBase
		{
			List<CampaignObjectManager.CampaignObjectType<T>> list = new List<CampaignObjectManager.CampaignObjectType<T>>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (campaignObjectType != null && typeof(T) == campaignObjectType.ObjectClass)
				{
					list.Add(campaignObjectType as CampaignObjectManager.CampaignObjectType<T>);
				}
			}
			return CampaignObjectManager.CampaignObjectType<T>.FindNextUniqueStringId(list, id);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000284F5 File Offset: 0x000266F5
		internal static void AutoGeneratedStaticCollectObjectsCampaignObjectManager(object o, List<object> collectedObjects)
		{
			((CampaignObjectManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00028504 File Offset: 0x00026704
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._deadOrDisabledHeroes);
			collectedObjects.Add(this._aliveHeroes);
			collectedObjects.Add(this._clans);
			collectedObjects.Add(this._kingdoms);
			collectedObjects.Add(this._mobileParties);
			collectedObjects.Add(this.Settlements);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00028559 File Offset: 0x00026759
		internal static object AutoGeneratedGetMemberValueSettlements(object o)
		{
			return ((CampaignObjectManager)o).Settlements;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00028566 File Offset: 0x00026766
		internal static object AutoGeneratedGetMemberValue_deadOrDisabledHeroes(object o)
		{
			return ((CampaignObjectManager)o)._deadOrDisabledHeroes;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00028573 File Offset: 0x00026773
		internal static object AutoGeneratedGetMemberValue_aliveHeroes(object o)
		{
			return ((CampaignObjectManager)o)._aliveHeroes;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00028580 File Offset: 0x00026780
		internal static object AutoGeneratedGetMemberValue_clans(object o)
		{
			return ((CampaignObjectManager)o)._clans;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002858D File Offset: 0x0002678D
		internal static object AutoGeneratedGetMemberValue_kingdoms(object o)
		{
			return ((CampaignObjectManager)o)._kingdoms;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002859A File Offset: 0x0002679A
		internal static object AutoGeneratedGetMemberValue_mobileParties(object o)
		{
			return ((CampaignObjectManager)o)._mobileParties;
		}

		// Token: 0x040002CE RID: 718
		internal const uint HeroObjectManagerTypeID = 32U;

		// Token: 0x040002CF RID: 719
		internal const uint MobilePartyObjectManagerTypeID = 14U;

		// Token: 0x040002D0 RID: 720
		internal const uint ClanObjectManagerTypeID = 18U;

		// Token: 0x040002D1 RID: 721
		internal const uint KingdomObjectManagerTypeID = 20U;

		// Token: 0x040002D2 RID: 722
		private CampaignObjectManager.ICampaignObjectType[] _objects;

		// Token: 0x040002D3 RID: 723
		private Dictionary<Type, uint> _objectTypesAndNextIds;

		// Token: 0x040002D4 RID: 724
		[SaveableField(20)]
		private readonly MBList<Hero> _deadOrDisabledHeroes;

		// Token: 0x040002D5 RID: 725
		[SaveableField(30)]
		private readonly MBList<Hero> _aliveHeroes;

		// Token: 0x040002D6 RID: 726
		[SaveableField(40)]
		private readonly MBList<Clan> _clans;

		// Token: 0x040002D7 RID: 727
		[SaveableField(50)]
		private readonly MBList<Kingdom> _kingdoms;

		// Token: 0x040002D8 RID: 728
		private MBList<IFaction> _factions;

		// Token: 0x040002D9 RID: 729
		[SaveableField(71)]
		private MBList<MobileParty> _mobileParties;

		// Token: 0x040002DA RID: 730
		private MBList<MobileParty> _caravanParties;

		// Token: 0x040002DB RID: 731
		private MBList<MobileParty> _patrolParties;

		// Token: 0x040002DC RID: 732
		private MBList<MobileParty> _militiaParties;

		// Token: 0x040002DD RID: 733
		private MBList<MobileParty> _garrisonParties;

		// Token: 0x040002DE RID: 734
		private MBList<MobileParty> _banditParties;

		// Token: 0x040002DF RID: 735
		private MBList<MobileParty> _villagerParties;

		// Token: 0x040002E0 RID: 736
		private MBList<MobileParty> _customParties;

		// Token: 0x040002E1 RID: 737
		private MBList<MobileParty> _lordParties;

		// Token: 0x040002E2 RID: 738
		private MBList<MobileParty> _partiesWithoutPartyComponent;

		// Token: 0x02000517 RID: 1303
		private interface ICampaignObjectType : IEnumerable
		{
			// Token: 0x17000EE6 RID: 3814
			// (get) Token: 0x06004C54 RID: 19540
			Type ObjectClass { get; }

			// Token: 0x06004C55 RID: 19541
			void PreAfterLoad();

			// Token: 0x06004C56 RID: 19542
			void AfterLoad();

			// Token: 0x06004C57 RID: 19543
			uint GetMaxObjectSubId();
		}

		// Token: 0x02000518 RID: 1304
		private class CampaignObjectType<T> : CampaignObjectManager.ICampaignObjectType, IEnumerable, IEnumerable<T> where T : MBObjectBase
		{
			// Token: 0x17000EE7 RID: 3815
			// (get) Token: 0x06004C58 RID: 19544 RVA: 0x0017DE86 File Offset: 0x0017C086
			// (set) Token: 0x06004C59 RID: 19545 RVA: 0x0017DE8E File Offset: 0x0017C08E
			public uint MaxCreatedPostfixIndex { get; private set; }

			// Token: 0x06004C5A RID: 19546 RVA: 0x0017DE98 File Offset: 0x0017C098
			public CampaignObjectType(IEnumerable<T> registeredObjects)
			{
				this._registeredObjects = registeredObjects;
				foreach (T t in this._registeredObjects)
				{
					ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(t.StringId);
					if (idParts.Item2 > this.MaxCreatedPostfixIndex)
					{
						this.MaxCreatedPostfixIndex = idParts.Item2;
					}
				}
			}

			// Token: 0x17000EE8 RID: 3816
			// (get) Token: 0x06004C5B RID: 19547 RVA: 0x0017DF14 File Offset: 0x0017C114
			Type CampaignObjectManager.ICampaignObjectType.ObjectClass
			{
				get
				{
					return typeof(T);
				}
			}

			// Token: 0x06004C5C RID: 19548 RVA: 0x0017DF20 File Offset: 0x0017C120
			public void PreAfterLoad()
			{
				foreach (T t in this._registeredObjects.ToList<T>())
				{
					t.PreAfterLoadInternal();
				}
			}

			// Token: 0x06004C5D RID: 19549 RVA: 0x0017DF7C File Offset: 0x0017C17C
			public void AfterLoad()
			{
				foreach (T t in this._registeredObjects.ToList<T>())
				{
					t.IsReady = true;
					t.AfterLoadInternal();
				}
			}

			// Token: 0x06004C5E RID: 19550 RVA: 0x0017DFE4 File Offset: 0x0017C1E4
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				return this._registeredObjects.GetEnumerator();
			}

			// Token: 0x06004C5F RID: 19551 RVA: 0x0017DFF1 File Offset: 0x0017C1F1
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._registeredObjects.GetEnumerator();
			}

			// Token: 0x06004C60 RID: 19552 RVA: 0x0017E000 File Offset: 0x0017C200
			public uint GetMaxObjectSubId()
			{
				uint num = 0U;
				foreach (T t in this._registeredObjects)
				{
					if (t.Id.SubId > num)
					{
						num = t.Id.SubId;
					}
				}
				return num;
			}

			// Token: 0x06004C61 RID: 19553 RVA: 0x0017E074 File Offset: 0x0017C274
			public void OnItemAdded(T item)
			{
				ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(item.StringId);
				if (idParts.Item2 > this.MaxCreatedPostfixIndex)
				{
					this.MaxCreatedPostfixIndex = idParts.Item2;
				}
				this.RegisterItem(item);
			}

			// Token: 0x06004C62 RID: 19554 RVA: 0x0017E0B3 File Offset: 0x0017C2B3
			private void RegisterItem(T item)
			{
				item.IsReady = true;
			}

			// Token: 0x06004C63 RID: 19555 RVA: 0x0017E0C1 File Offset: 0x0017C2C1
			public void UnregisterItem(T item)
			{
				item.IsReady = false;
			}

			// Token: 0x06004C64 RID: 19556 RVA: 0x0017E0D0 File Offset: 0x0017C2D0
			public T Find(string id)
			{
				foreach (T t in this._registeredObjects)
				{
					if (t.StringId == id)
					{
						return t;
					}
				}
				return default(T);
			}

			// Token: 0x06004C65 RID: 19557 RVA: 0x0017E138 File Offset: 0x0017C338
			public T FindFirst(Predicate<T> predicate)
			{
				foreach (T t in this._registeredObjects)
				{
					if (predicate(t))
					{
						return t;
					}
				}
				return default(T);
			}

			// Token: 0x06004C66 RID: 19558 RVA: 0x0017E198 File Offset: 0x0017C398
			public MBReadOnlyList<T> FindAll(Predicate<T> predicate)
			{
				MBList<T> mblist = new MBList<T>();
				foreach (T t in this._registeredObjects)
				{
					if (predicate == null || predicate(t))
					{
						mblist.Add(t);
					}
				}
				return mblist;
			}

			// Token: 0x06004C67 RID: 19559 RVA: 0x0017E1F8 File Offset: 0x0017C3F8
			public static string FindNextUniqueStringId(List<CampaignObjectManager.CampaignObjectType<T>> lists, string id)
			{
				if (!CampaignObjectManager.CampaignObjectType<T>.Exist(lists, id))
				{
					return id;
				}
				ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(id);
				string item = idParts.Item1;
				uint num = idParts.Item2;
				num = MathF.Max(num, lists.Max<CampaignObjectManager.CampaignObjectType<T>, uint>((CampaignObjectManager.CampaignObjectType<T> x) => x.MaxCreatedPostfixIndex));
				num += 1U;
				return item + num;
			}

			// Token: 0x06004C68 RID: 19560 RVA: 0x0017E260 File Offset: 0x0017C460
			[return: TupleElementNames(new string[] { "str", "number" })]
			private static ValueTuple<string, uint> GetIdParts(string stringId)
			{
				int num = stringId.Length - 1;
				while (num > 0 && char.IsDigit(stringId[num]))
				{
					num--;
				}
				string text = stringId.Substring(0, num + 1);
				uint num2 = 0U;
				if (num < stringId.Length - 1)
				{
					uint.TryParse(stringId.Substring(num + 1, stringId.Length - num - 1), out num2);
				}
				return new ValueTuple<string, uint>(text, num2);
			}

			// Token: 0x06004C69 RID: 19561 RVA: 0x0017E2C8 File Offset: 0x0017C4C8
			private static bool Exist(List<CampaignObjectManager.CampaignObjectType<T>> lists, string id)
			{
				using (List<CampaignObjectManager.CampaignObjectType<T>>.Enumerator enumerator = lists.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Find(id) != null)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x040015E3 RID: 5603
			private readonly IEnumerable<T> _registeredObjects;
		}

		// Token: 0x02000519 RID: 1305
		private enum CampaignObjects
		{
			// Token: 0x040015E6 RID: 5606
			DeadOrDisabledHeroes,
			// Token: 0x040015E7 RID: 5607
			AliveHeroes,
			// Token: 0x040015E8 RID: 5608
			Clans,
			// Token: 0x040015E9 RID: 5609
			Kingdoms,
			// Token: 0x040015EA RID: 5610
			MobileParty,
			// Token: 0x040015EB RID: 5611
			ObjectCount
		}
	}
}
