using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043C RID: 1084
	public class RecruitPrisonersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004590 RID: 17808 RVA: 0x00157A58 File Offset: 0x00155C58
		public override void RegisterEvents()
		{
			CampaignEvents.OnMainPartyPrisonerRecruitedEvent.AddNonSerializedListener(this, new Action<FlattenedTroopRoster>(this.OnMainPartyPrisonerRecruited));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickAIMobileParty));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTickMainParty));
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00157AAC File Offset: 0x00155CAC
		private void HourlyTickMainParty()
		{
			MobileParty mainParty = MobileParty.MainParty;
			TroopRoster memberRoster = mainParty.MemberRoster;
			TroopRoster prisonRoster = mainParty.PrisonRoster;
			if (memberRoster.Count != 0 && memberRoster.TotalManCount > 0 && prisonRoster.Count != 0 && prisonRoster.TotalRegulars > 0 && mainParty.MapEvent == null)
			{
				int num = MBRandom.RandomInt(0, prisonRoster.Count);
				bool flag = false;
				for (int i = num; i < prisonRoster.Count + num; i++)
				{
					int num2 = i % prisonRoster.Count;
					CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(num2);
					if (characterAtIndex.IsRegular)
					{
						CharacterObject characterObject = characterAtIndex;
						int elementNumber = mainParty.PrisonRoster.GetElementNumber(num2);
						int num3 = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(mainParty.Party, characterObject);
						if (!flag && num3 < elementNumber)
						{
							flag = this.GenerateConformityForTroop(mainParty, characterObject, 1);
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x00157B98 File Offset: 0x00155D98
		private void DailyTickAIMobileParty(MobileParty mobileParty)
		{
			if (!mobileParty.IsMainParty && mobileParty.IsLordParty && mobileParty.MapEvent == null)
			{
				TroopRoster prisonRoster = mobileParty.PrisonRoster;
				if (prisonRoster.Count != 0 && prisonRoster.TotalRegulars > 0)
				{
					int num = MBRandom.RandomInt(0, prisonRoster.Count);
					bool flag = false;
					for (int i = num; i < prisonRoster.Count + num; i++)
					{
						int num2 = i % prisonRoster.Count;
						CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(num2);
						if (characterAtIndex.IsRegular)
						{
							CharacterObject characterObject = characterAtIndex;
							int elementNumber = mobileParty.PrisonRoster.GetElementNumber(num2);
							int num3 = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(mobileParty.Party, characterObject);
							if (!flag && num3 < elementNumber)
							{
								flag = this.GenerateConformityForTroop(mobileParty, characterObject, CampaignTime.HoursInDay);
							}
							if (Campaign.Current.Models.PrisonerRecruitmentCalculationModel.ShouldPartyRecruitPrisoners(mobileParty.Party))
							{
								int num4;
								if (this.IsPrisonerRecruitable(mobileParty, characterObject, out num4))
								{
									int num5 = mobileParty.Party.PartySizeLimit - mobileParty.MemberRoster.TotalManCount;
									int num6 = MathF.Min((num5 > 0) ? ((num5 > num3) ? num3 : num5) : 0, prisonRoster.GetElementNumber(characterObject));
									int characterWage = Campaign.Current.Models.PartyWageModel.GetCharacterWage(characterObject);
									num6 = MathF.Min(num6, mobileParty.GetAvailableWageBudget() / characterWage);
									if (num6 > 0)
									{
										this.RecruitPrisonersAi(mobileParty, characterObject, num6, num4);
									}
								}
							}
							else if (flag)
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x00157D20 File Offset: 0x00155F20
		private bool GenerateConformityForTroop(MobileParty mobileParty, CharacterObject troop, int hours = 1)
		{
			int num = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityChangePerHour(mobileParty.Party, troop).RoundedResultNumber * hours;
			mobileParty.PrisonRoster.AddXpToTroop(troop, num);
			return true;
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x00157D64 File Offset: 0x00155F64
		private void ApplyPrisonerRecruitmentEffects(MobileParty mobileParty, CharacterObject troop, int num)
		{
			int prisonerRecruitmentMoraleEffect = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetPrisonerRecruitmentMoraleEffect(mobileParty.Party, troop, num);
			mobileParty.RecentEventsMorale += (float)prisonerRecruitmentMoraleEffect;
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x00157DA0 File Offset: 0x00155FA0
		private void RecruitPrisonersAi(MobileParty mobileParty, CharacterObject troop, int num, int conformityCost)
		{
			mobileParty.PrisonRoster.GetElementNumber(troop);
			mobileParty.PrisonRoster.GetElementXp(troop);
			mobileParty.PrisonRoster.AddToCounts(troop, -num, false, 0, -conformityCost * num, true, -1);
			mobileParty.MemberRoster.AddToCounts(troop, num, false, 0, 0, true, -1);
			CampaignEventDispatcher.Instance.OnTroopRecruited(mobileParty.LeaderHero, null, null, troop, num);
			this.ApplyPrisonerRecruitmentEffects(mobileParty, troop, num);
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x00157E0F File Offset: 0x0015600F
		private bool IsPrisonerRecruitable(MobileParty mobileParty, CharacterObject character, out int conformityNeeded)
		{
			return Campaign.Current.Models.PrisonerRecruitmentCalculationModel.IsPrisonerRecruitable(mobileParty.Party, character, out conformityNeeded);
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x00157E30 File Offset: 0x00156030
		private void OnMainPartyPrisonerRecruited(FlattenedTroopRoster flattenedTroopRosters)
		{
			foreach (CharacterObject characterObject in flattenedTroopRosters.Troops)
			{
				CampaignEventDispatcher.Instance.OnUnitRecruited(characterObject, 1);
				this.ApplyPrisonerRecruitmentEffects(MobileParty.MainParty, characterObject, 1);
			}
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x00157E90 File Offset: 0x00156090
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
