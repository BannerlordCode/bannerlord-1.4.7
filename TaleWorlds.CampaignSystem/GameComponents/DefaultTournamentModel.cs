using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200015D RID: 349
	public class DefaultTournamentModel : TournamentModel
	{
		// Token: 0x06001AD0 RID: 6864 RVA: 0x00089E87 File Offset: 0x00088087
		public override TournamentGame CreateTournament(Town town)
		{
			return new FightTournamentGame(town);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x00089E90 File Offset: 0x00088090
		public override float GetTournamentStartChance(Town town)
		{
			if (town.Settlement.SiegeEvent != null)
			{
				return 0f;
			}
			if (Math.Abs(town.StringId.GetHashCode() % 3) != CampaignTime.Now.GetWeekOfSeason)
			{
				return 0f;
			}
			return 0.1f * (float)(town.Settlement.Parties.Count<MobileParty>((MobileParty x) => x.IsLordParty) + town.Settlement.HeroesWithoutParty.Count<Hero>((Hero x) => this.SuitableForTournament(x))) - 0.2f;
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x00089F30 File Offset: 0x00088130
		public override int GetNumLeaderboardVictoriesAtGameStart()
		{
			return 500;
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00089F38 File Offset: 0x00088138
		public override float GetTournamentEndChance(TournamentGame tournament)
		{
			float elapsedDaysUntilNow = tournament.CreationTime.ElapsedDaysUntilNow;
			return MathF.Max(0f, (elapsedDaysUntilNow - 10f) * 0.05f);
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x00089F6B File Offset: 0x0008816B
		private bool SuitableForTournament(Hero hero)
		{
			return hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && MathF.Max(hero.GetSkillValue(DefaultSkills.OneHanded), hero.GetSkillValue(DefaultSkills.TwoHanded)) > 100;
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00089FAC File Offset: 0x000881AC
		public override float GetTournamentSimulationScore(CharacterObject character)
		{
			return (character.IsHero ? 1f : 0.4f) * (MathF.Max((float)character.GetSkillValue(DefaultSkills.OneHanded), (float)character.GetSkillValue(DefaultSkills.TwoHanded), (float)character.GetSkillValue(DefaultSkills.Polearm)) + (float)character.GetSkillValue(DefaultSkills.Athletics) + (float)character.GetSkillValue(DefaultSkills.Riding)) * 0.01f;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0008A018 File Offset: 0x00088218
		public override int GetRenownReward(Hero winner, Town town)
		{
			float num = 3f;
			if (winner.GetPerkValue(DefaultPerks.OneHanded.Duelist))
			{
				num *= DefaultPerks.OneHanded.Duelist.SecondaryBonus;
			}
			if (winner.GetPerkValue(DefaultPerks.Charm.SelfPromoter))
			{
				num += DefaultPerks.Charm.SelfPromoter.PrimaryBonus;
			}
			return MathF.Round(num);
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0008A065 File Offset: 0x00088265
		public override int GetInfluenceReward(Hero winner, Town town)
		{
			return 0;
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x0008A068 File Offset: 0x00088268
		[return: TupleElementNames(new string[] { "skill", "xp" })]
		public override ValueTuple<SkillObject, int> GetSkillXpGainFromTournament(Town town)
		{
			float randomFloat = MBRandom.RandomFloat;
			SkillObject skillObject = ((randomFloat < 0.2f) ? DefaultSkills.OneHanded : ((randomFloat < 0.4f) ? DefaultSkills.TwoHanded : ((randomFloat < 0.6f) ? DefaultSkills.Polearm : ((randomFloat < 0.8f) ? DefaultSkills.Riding : DefaultSkills.Athletics))));
			int num = 500;
			return new ValueTuple<SkillObject, int>(skillObject, num);
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x0008A0C8 File Offset: 0x000882C8
		public override Equipment GetParticipantArmor(CharacterObject participant)
		{
			if (CampaignMission.Current != null && CampaignMission.Current.Mode != MissionMode.Tournament && Settlement.CurrentSettlement != null)
			{
				return (Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_" + Settlement.CurrentSettlement.MapFaction.Culture.StringId) ?? Game.Current.ObjectManager.GetObject<CharacterObject>("gear_practice_dummy_empire")).RandomBattleEquipment;
			}
			return participant.RandomBattleEquipment;
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0008A144 File Offset: 0x00088344
		public override MBList<ItemObject> GetRegularRewardItems(Town town, int regularRewardMinValue, int regularRewardMaxValue)
		{
			MBList<ItemObject> mblist = new MBList<ItemObject>();
			MBList<ItemObject> mblist2 = new MBList<ItemObject>();
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.Value > regularRewardMinValue && itemObject.Value < regularRewardMaxValue && !itemObject.NotMerchandise && (itemObject.IsCraftedWeapon || itemObject.IsMountable || itemObject.ArmorComponent != null) && !itemObject.IsCraftedByPlayer)
				{
					if (itemObject.Culture == town.Culture)
					{
						mblist.Add(itemObject);
					}
					else
					{
						mblist2.Add(itemObject);
					}
				}
			}
			foreach (ItemObject itemObject2 in Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItems())
			{
				if (itemObject2.BannerComponent.BannerLevel == 1 || itemObject2.BannerComponent.BannerLevel == 2)
				{
					mblist.Add(itemObject2);
				}
			}
			if (mblist.IsEmpty<ItemObject>())
			{
				mblist.AddRange(mblist2);
			}
			return mblist;
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x0008A274 File Offset: 0x00088474
		public override MBList<ItemObject> GetEliteRewardItems(Town town, int regularRewardMinValue, int regularRewardMaxValue)
		{
			MBList<ItemObject> mblist = new MBList<ItemObject>();
			foreach (string text in new string[]
			{
				"winds_fury_sword_t3", "bone_crusher_mace_t3", "tyrhung_sword_t3", "pernach_mace_t3", "early_retirement_2hsword_t3", "black_heart_2haxe_t3", "knights_fall_mace_t3", "the_scalpel_sword_t3", "judgement_mace_t3", "dawnbreaker_sword_t3",
				"ambassador_sword_t3", "heavy_nasalhelm_over_imperial_mail", "sturgian_helmet_closed", "full_helm_over_laced_coif", "desert_mail_coif", "heavy_nasalhelm_over_imperial_mail", "plumed_nomad_helmet", "ridged_northernhelm", "noble_horse_southern", "noble_horse_imperial",
				"noble_horse_western", "noble_horse_eastern", "noble_horse_battania", "noble_horse_northern", "special_camel", "western_crowned_helmet", "northern_warlord_helmet", "battania_warlord_pauldrons", "aserai_armor_02_b", "white_coat_over_mail",
				"spiked_helmet_with_facemask"
			})
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(text);
				if (@object != null)
				{
					mblist.Add(@object);
				}
			}
			return mblist;
		}
	}
}
