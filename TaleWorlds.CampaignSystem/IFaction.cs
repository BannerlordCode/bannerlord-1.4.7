using System;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000094 RID: 148
	[SaveableInterface(22001)]
	public interface IFaction
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x0600127A RID: 4730
		TextObject Name { get; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x0600127B RID: 4731
		string StringId { get; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x0600127C RID: 4732
		MBGUID Id { get; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600127D RID: 4733
		TextObject InformalName { get; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600127E RID: 4734
		string EncyclopediaLink { get; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600127F RID: 4735
		TextObject EncyclopediaLinkWithName { get; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001280 RID: 4736
		TextObject EncyclopediaText { get; }

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001281 RID: 4737
		CultureObject Culture { get; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001282 RID: 4738
		Settlement InitialHomeSettlement { get; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001283 RID: 4739
		uint Color { get; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001284 RID: 4740
		uint Color2 { get; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001285 RID: 4741
		CharacterObject BasicTroop { get; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001286 RID: 4742
		Hero Leader { get; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001287 RID: 4743
		Banner Banner { get; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001288 RID: 4744
		MBReadOnlyList<Settlement> Settlements { get; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001289 RID: 4745
		MBReadOnlyList<Town> Fiefs { get; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600128A RID: 4746
		MBReadOnlyList<Hero> AliveLords { get; }

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x0600128B RID: 4747
		MBReadOnlyList<Hero> DeadLords { get; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x0600128C RID: 4748
		MBReadOnlyList<Hero> Heroes { get; }

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x0600128D RID: 4749
		MBReadOnlyList<WarPartyComponent> WarPartyComponents { get; }

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x0600128E RID: 4750
		bool IsBanditFaction { get; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x0600128F RID: 4751
		bool IsMinorFaction { get; }

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001290 RID: 4752
		bool IsKingdomFaction { get; }

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001291 RID: 4753
		bool IsRebelClan { get; }

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001292 RID: 4754
		bool IsClan { get; }

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001293 RID: 4755
		bool IsOutlaw { get; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001294 RID: 4756
		bool IsMapFaction { get; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001295 RID: 4757
		bool HasNavalNavigationCapability { get; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001296 RID: 4758
		IFaction MapFaction { get; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001297 RID: 4759
		float CurrentTotalStrength { get; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001298 RID: 4760
		Settlement FactionMidSettlement { get; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001299 RID: 4761
		float DistanceToClosestNonAllyFortification { get; }

		// Token: 0x0600129A RID: 4762
		bool IsAtWarWith(IFaction other);

		// Token: 0x0600129B RID: 4763
		StanceLink GetStanceWith(IFaction other);

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x0600129C RID: 4764
		MBReadOnlyList<IFaction> FactionsAtWarWith { get; }

		// Token: 0x0600129D RID: 4765
		void UpdateFactionsAtWarWith();

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600129E RID: 4766
		// (set) Token: 0x0600129F RID: 4767
		int TributeWallet { get; set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060012A0 RID: 4768
		// (set) Token: 0x060012A1 RID: 4769
		float MainHeroCrimeRating { get; set; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060012A2 RID: 4770
		float DailyCrimeRatingChange { get; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060012A3 RID: 4771
		float Aggressiveness { get; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060012A4 RID: 4772
		bool IsEliminated { get; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060012A5 RID: 4773
		ExplainedNumber DailyCrimeRatingChangeExplained { get; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060012A6 RID: 4774
		// (set) Token: 0x060012A7 RID: 4775
		CampaignTime NotAttackableByPlayerUntilTime { get; set; }
	}
}
