using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000257 RID: 599
	public sealed class MissionGameModels : GameModelsManager
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060021F5 RID: 8693 RVA: 0x000779AB File Offset: 0x00075BAB
		// (set) Token: 0x060021F6 RID: 8694 RVA: 0x000779B2 File Offset: 0x00075BB2
		public static MissionGameModels Current { get; private set; }

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060021F7 RID: 8695 RVA: 0x000779BA File Offset: 0x00075BBA
		// (set) Token: 0x060021F8 RID: 8696 RVA: 0x000779C2 File Offset: 0x00075BC2
		public AgentStatCalculateModel AgentStatCalculateModel { get; private set; }

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x000779CB File Offset: 0x00075BCB
		// (set) Token: 0x060021FA RID: 8698 RVA: 0x000779D3 File Offset: 0x00075BD3
		public ApplyWeatherEffectsModel ApplyWeatherEffectsModel { get; private set; }

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x000779DC File Offset: 0x00075BDC
		// (set) Token: 0x060021FC RID: 8700 RVA: 0x000779E4 File Offset: 0x00075BE4
		public StrikeMagnitudeCalculationModel StrikeMagnitudeModel { get; private set; }

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x000779ED File Offset: 0x00075BED
		// (set) Token: 0x060021FE RID: 8702 RVA: 0x000779F5 File Offset: 0x00075BF5
		public AgentApplyDamageModel AgentApplyDamageModel { get; private set; }

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x000779FE File Offset: 0x00075BFE
		// (set) Token: 0x06002200 RID: 8704 RVA: 0x00077A06 File Offset: 0x00075C06
		public AgentDecideKilledOrUnconsciousModel AgentDecideKilledOrUnconsciousModel { get; private set; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x00077A0F File Offset: 0x00075C0F
		// (set) Token: 0x06002202 RID: 8706 RVA: 0x00077A17 File Offset: 0x00075C17
		public MissionDifficultyModel MissionDifficultyModel { get; private set; }

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x00077A20 File Offset: 0x00075C20
		// (set) Token: 0x06002204 RID: 8708 RVA: 0x00077A28 File Offset: 0x00075C28
		public BattleMoraleModel BattleMoraleModel { get; private set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x00077A31 File Offset: 0x00075C31
		// (set) Token: 0x06002206 RID: 8710 RVA: 0x00077A39 File Offset: 0x00075C39
		public BattleInitializationModel BattleInitializationModel { get; private set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002207 RID: 8711 RVA: 0x00077A42 File Offset: 0x00075C42
		// (set) Token: 0x06002208 RID: 8712 RVA: 0x00077A4A File Offset: 0x00075C4A
		public BattleSpawnModel BattleSpawnModel { get; private set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x00077A53 File Offset: 0x00075C53
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x00077A5B File Offset: 0x00075C5B
		public BattleBannerBearersModel BattleBannerBearersModel { get; private set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x00077A64 File Offset: 0x00075C64
		// (set) Token: 0x0600220C RID: 8716 RVA: 0x00077A6C File Offset: 0x00075C6C
		public FormationArrangementModel FormationArrangementsModel { get; private set; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x00077A75 File Offset: 0x00075C75
		// (set) Token: 0x0600220E RID: 8718 RVA: 0x00077A7D File Offset: 0x00075C7D
		public AutoBlockModel AutoBlockModel { get; private set; }

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x00077A86 File Offset: 0x00075C86
		// (set) Token: 0x06002210 RID: 8720 RVA: 0x00077A8E File Offset: 0x00075C8E
		public DamageParticleModel DamageParticleModel { get; private set; }

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x00077A97 File Offset: 0x00075C97
		// (set) Token: 0x06002212 RID: 8722 RVA: 0x00077A9F File Offset: 0x00075C9F
		public ItemPickupModel ItemPickupModel { get; private set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x00077AA8 File Offset: 0x00075CA8
		// (set) Token: 0x06002214 RID: 8724 RVA: 0x00077AB0 File Offset: 0x00075CB0
		public MissionShipParametersModel MissionShipParametersModel { get; private set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x00077AB9 File Offset: 0x00075CB9
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x00077AC1 File Offset: 0x00075CC1
		public MissionSiegeEngineCalculationModel MissionSiegeEngineCalculationModel { get; private set; }

		// Token: 0x06002217 RID: 8727 RVA: 0x00077ACC File Offset: 0x00075CCC
		private void GetSpecificGameBehaviors()
		{
			this.AgentStatCalculateModel = base.GetGameModel<AgentStatCalculateModel>();
			this.ApplyWeatherEffectsModel = base.GetGameModel<ApplyWeatherEffectsModel>();
			this.StrikeMagnitudeModel = base.GetGameModel<StrikeMagnitudeCalculationModel>();
			this.AgentApplyDamageModel = base.GetGameModel<AgentApplyDamageModel>();
			this.AgentDecideKilledOrUnconsciousModel = base.GetGameModel<AgentDecideKilledOrUnconsciousModel>();
			this.MissionDifficultyModel = base.GetGameModel<MissionDifficultyModel>();
			this.BattleMoraleModel = base.GetGameModel<BattleMoraleModel>();
			this.BattleInitializationModel = base.GetGameModel<BattleInitializationModel>();
			this.BattleSpawnModel = base.GetGameModel<BattleSpawnModel>();
			this.BattleBannerBearersModel = base.GetGameModel<BattleBannerBearersModel>();
			this.FormationArrangementsModel = base.GetGameModel<FormationArrangementModel>();
			this.AutoBlockModel = base.GetGameModel<AutoBlockModel>();
			this.DamageParticleModel = base.GetGameModel<DamageParticleModel>();
			this.ItemPickupModel = base.GetGameModel<ItemPickupModel>();
			this.MissionShipParametersModel = base.GetGameModel<MissionShipParametersModel>();
			this.MissionSiegeEngineCalculationModel = base.GetGameModel<MissionSiegeEngineCalculationModel>();
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x00077B99 File Offset: 0x00075D99
		private void MakeGameComponentBindings()
		{
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00077B9B File Offset: 0x00075D9B
		public MissionGameModels(IEnumerable<GameModel> inputComponents)
			: base(inputComponents)
		{
			MissionGameModels.Current = this;
			this.GetSpecificGameBehaviors();
			this.MakeGameComponentBindings();
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00077BB6 File Offset: 0x00075DB6
		public static void Clear()
		{
			MissionGameModels.Current = null;
		}
	}
}
