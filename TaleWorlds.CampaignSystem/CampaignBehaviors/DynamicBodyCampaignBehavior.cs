using System;
using Helpers;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003EB RID: 1003
	public class DynamicBodyCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06003E2F RID: 15919 RVA: 0x0011084C File Offset: 0x0010EA4C
		// (set) Token: 0x06003E30 RID: 15920 RVA: 0x0011086B File Offset: 0x0010EA6B
		private CampaignTime LastSettlementVisitTime
		{
			get
			{
				if (Hero.MainHero.CurrentSettlement != null)
				{
					this._lastSettlementVisitTime = CampaignTime.Now;
				}
				return this._lastSettlementVisitTime;
			}
			set
			{
				this._lastSettlementVisitTime = value;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x00110874 File Offset: 0x0010EA74
		private float MaxPlayerWeight
		{
			get
			{
				return MathF.Min(1f, this._unmodifiedWeight * 1.3f);
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06003E32 RID: 15922 RVA: 0x0011088C File Offset: 0x0010EA8C
		private float MinPlayerWeight
		{
			get
			{
				return MathF.Max(0f, this._unmodifiedWeight * 0.7f);
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06003E33 RID: 15923 RVA: 0x001108A4 File Offset: 0x0010EAA4
		private float MaxPlayerBuild
		{
			get
			{
				return MathF.Min(1f, this._unmodifiedBuild * 1.3f);
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06003E34 RID: 15924 RVA: 0x001108BC File Offset: 0x0010EABC
		private float MinPlayerBuild
		{
			get
			{
				return MathF.Max(0f, this._unmodifiedBuild * 0.7f);
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x001108D4 File Offset: 0x0010EAD4
		private void DailyTick()
		{
			bool flag = this.LastSettlementVisitTime.ElapsedDaysUntilNow < 1f;
			bool flag2 = Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Party.IsStarving;
			float num = ((Hero.MainHero.CurrentSettlement == null && flag2) ? (-0.1f) : (flag ? 0.025f : (-0.025f)));
			Hero.MainHero.Weight = MBMath.ClampFloat(Hero.MainHero.Weight + num, this.MinPlayerWeight, this.MaxPlayerWeight);
			float num2 = ((MapEvent.PlayerMapEvent != null || PlayerSiege.PlayerSiegeEvent != null || this._lastEncounterTime.ElapsedDaysUntilNow < 2f) ? 0.025f : (-0.015f));
			Hero.MainHero.Build = MBMath.ClampFloat(Hero.MainHero.Build + num2, this.MinPlayerBuild, this.MaxPlayerBuild);
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x001109C4 File Offset: 0x0010EBC4
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party != null && party.IsMainParty)
			{
				this.LastSettlementVisitTime = CampaignTime.Now;
			}
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x001109DC File Offset: 0x0010EBDC
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent)
			{
				this._lastEncounterTime = CampaignTime.Now;
			}
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x001109F1 File Offset: 0x0010EBF1
		private void OnPlayerBodyPropertiesChanged()
		{
			this._unmodifiedBuild = Hero.MainHero.Build;
			this._unmodifiedWeight = Hero.MainHero.Weight;
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00110A13 File Offset: 0x0010EC13
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
			this._unmodifiedBuild = newPlayer.Build;
			this._unmodifiedWeight = newPlayer.Weight;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x00110A30 File Offset: 0x0010EC30
		private void OnHeroCreated(Hero hero, bool bornNaturally)
		{
			if (!bornNaturally)
			{
				DynamicBodyProperties dynamicBodyPropertiesBetweenMinMaxRange = CharacterHelper.GetDynamicBodyPropertiesBetweenMinMaxRange(hero.CharacterObject);
				hero.Weight = dynamicBodyPropertiesBetweenMinMaxRange.Weight;
				hero.Build = dynamicBodyPropertiesBetweenMinMaxRange.Build;
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00110A64 File Offset: 0x0010EC64
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			this._lastSettlementVisitTime = CampaignTime.Now;
			this._lastEncounterTime = CampaignTime.Now;
			this.OnPlayerBodyPropertiesChanged();
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x00110A84 File Offset: 0x0010EC84
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.OnPlayerBodyPropertiesChangedEvent.AddNonSerializedListener(this, new Action(this.OnPlayerBodyPropertiesChanged));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action(this.OnPlayerBodyPropertiesChanged));
			CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x00110B4C File Offset: 0x0010ED4C
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_lastSettlementVisitTime", ref this._lastSettlementVisitTime);
			dataStore.SyncData<CampaignTime>("_lastEncounterTime", ref this._lastEncounterTime);
			dataStore.SyncData<float>("_unmodifiedWeight", ref this._unmodifiedWeight);
			dataStore.SyncData<float>("_unmodifiedBuild", ref this._unmodifiedBuild);
		}

		// Token: 0x040012C9 RID: 4809
		private const float DailyBuildDecrease = -0.015f;

		// Token: 0x040012CA RID: 4810
		private const float DailyBuildIncrease = 0.025f;

		// Token: 0x040012CB RID: 4811
		private const float DailyWeightDecreaseWhenStarving = -0.1f;

		// Token: 0x040012CC RID: 4812
		private const float DailyWeightDecreaseWhenNotStarving = -0.025f;

		// Token: 0x040012CD RID: 4813
		private const float DailyWeightIncrease = 0.025f;

		// Token: 0x040012CE RID: 4814
		private CampaignTime _lastSettlementVisitTime;

		// Token: 0x040012CF RID: 4815
		private CampaignTime _lastEncounterTime;

		// Token: 0x040012D0 RID: 4816
		private float _unmodifiedWeight = -1f;

		// Token: 0x040012D1 RID: 4817
		private float _unmodifiedBuild = -1f;
	}
}
