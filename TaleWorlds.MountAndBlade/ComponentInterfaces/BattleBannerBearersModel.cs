using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003FE RID: 1022
	public abstract class BattleBannerBearersModel : MBGameModel<BattleBannerBearersModel>
	{
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x0600379F RID: 14239 RVA: 0x000E4EE3 File Offset: 0x000E30E3
		protected BannerBearerLogic BannerBearerLogic
		{
			get
			{
				return this._bannerBearerLogic;
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000E4EEB File Offset: 0x000E30EB
		public void InitializeModel(BannerBearerLogic bannerBearerLogic)
		{
			this._bannerBearerLogic = bannerBearerLogic;
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x000E4EF4 File Offset: 0x000E30F4
		public void FinalizeModel()
		{
			this._bannerBearerLogic = null;
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x000E4F00 File Offset: 0x000E3100
		public bool IsFormationBanner(Formation formation, SpawnedItemEntity item)
		{
			if (formation == null)
			{
				return false;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return bannerBearerLogic != null && bannerBearerLogic.IsFormationBanner(formation, item);
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x000E4F28 File Offset: 0x000E3128
		public bool IsBannerSearchingAgent(Agent agent)
		{
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return bannerBearerLogic != null && bannerBearerLogic.IsBannerSearchingAgent(agent);
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x000E4F48 File Offset: 0x000E3148
		public bool IsInteractableFormationBanner(SpawnedItemEntity item, Agent interactingAgent)
		{
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			Formation formation = ((bannerBearerLogic != null) ? bannerBearerLogic.GetFormationFromBanner(item) : null);
			return formation == null || formation.Captain == interactingAgent || interactingAgent.Formation == formation || (interactingAgent.IsPlayerControlled && interactingAgent.Team == formation.Team);
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x000E4F9A File Offset: 0x000E319A
		public bool HasFormationBanner(Formation formation)
		{
			if (formation == null)
			{
				return false;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return ((bannerBearerLogic != null) ? bannerBearerLogic.GetFormationBanner(formation) : null) != null;
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000E4FB8 File Offset: 0x000E31B8
		public bool HasBannerOnGround(Formation formation)
		{
			if (formation == null)
			{
				return false;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			return bannerBearerLogic != null && bannerBearerLogic.HasBannerOnGround(formation);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000E4FDD File Offset: 0x000E31DD
		public ItemObject GetFormationBanner(Formation formation)
		{
			if (formation == null)
			{
				return null;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			if (bannerBearerLogic == null)
			{
				return null;
			}
			return bannerBearerLogic.GetFormationBanner(formation);
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000E4FF8 File Offset: 0x000E31F8
		public List<Agent> GetFormationBannerBearers(Formation formation)
		{
			if (formation == null)
			{
				return new List<Agent>();
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			if (bannerBearerLogic != null)
			{
				return bannerBearerLogic.GetFormationBannerBearers(formation);
			}
			return new List<Agent>();
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000E5025 File Offset: 0x000E3225
		public BannerComponent GetActiveBanner(Formation formation)
		{
			if (formation == null)
			{
				return null;
			}
			BannerBearerLogic bannerBearerLogic = this.BannerBearerLogic;
			if (bannerBearerLogic == null)
			{
				return null;
			}
			return bannerBearerLogic.GetActiveBanner(formation);
		}

		// Token: 0x060037AA RID: 14250
		public abstract int GetMinimumFormationTroopCountToBearBanners();

		// Token: 0x060037AB RID: 14251
		public abstract float GetBannerInteractionDistance(Agent interactingAgent);

		// Token: 0x060037AC RID: 14252
		public abstract bool CanBannerBearerProvideEffectToFormation(Agent agent, Formation formation);

		// Token: 0x060037AD RID: 14253
		public abstract bool CanAgentPickUpAnyBanner(Agent agent);

		// Token: 0x060037AE RID: 14254
		public abstract bool CanAgentBecomeBannerBearer(Agent agent);

		// Token: 0x060037AF RID: 14255
		public abstract int GetAgentBannerBearingPriority(Agent agent);

		// Token: 0x060037B0 RID: 14256
		public abstract bool CanFormationDeployBannerBearers(Formation formation);

		// Token: 0x060037B1 RID: 14257
		public abstract int GetDesiredNumberOfBannerBearersForFormation(Formation formation);

		// Token: 0x060037B2 RID: 14258
		public abstract ItemObject GetBannerBearerReplacementWeapon(BasicCharacterObject agentCharacter);

		// Token: 0x040017D9 RID: 6105
		public const float DefaultDetachmentCostMultiplier = 10f;

		// Token: 0x040017DA RID: 6106
		private BannerBearerLogic _bannerBearerLogic;
	}
}
