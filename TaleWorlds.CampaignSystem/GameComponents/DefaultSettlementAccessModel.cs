using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000149 RID: 329
	public class DefaultSettlementAccessModel : SettlementAccessModel
	{
		// Token: 0x060019C9 RID: 6601 RVA: 0x00081FB4 File Offset: 0x000801B4
		public override void CanMainHeroEnterSettlement(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			if (settlement.IsFortification && Hero.MainHero.MapFaction == settlement.MapFaction && (settlement.Town.GarrisonParty == null || settlement.Town.GarrisonParty.Party.NumberOfAllMembers == 0))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				return;
			}
			if (settlement.IsTown)
			{
				this.CanMainHeroEnterTown(settlement, out accessDetails);
				return;
			}
			if (settlement.IsCastle)
			{
				this.CanMainHeroEnterCastle(settlement, out accessDetails);
				return;
			}
			if (settlement.IsVillage)
			{
				this.CanMainHeroEnterVillage(settlement, out accessDetails);
				return;
			}
			Debug.FailedAssert("Invalid type of settlement", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSettlementAccessModel.cs", "CanMainHeroEnterSettlement", 42);
			accessDetails = new SettlementAccessModel.AccessDetails
			{
				AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
				AccessMethod = SettlementAccessModel.AccessMethod.Direct
			};
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x00082087 File Offset: 0x00080287
		public override void CanMainHeroEnterDungeon(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			accessDetails = default(SettlementAccessModel.AccessDetails);
			this.CanMainHeroEnterKeepInternal(settlement, out accessDetails);
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x00082098 File Offset: 0x00080298
		public override void CanMainHeroEnterLordsHall(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			accessDetails = default(SettlementAccessModel.AccessDetails);
			this.CanMainHeroEnterKeepInternal(settlement, out accessDetails);
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000820AC File Offset: 0x000802AC
		private void CanMainHeroEnterKeepInternal(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			accessDetails = default(SettlementAccessModel.AccessDetails);
			Hero mainHero = Hero.MainHero;
			if (settlement.OwnerClan == mainHero.Clan)
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
			}
			else if (DiplomacyHelper.IsSameFactionAndNotEliminated(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
			}
			else if (FactionManager.IsNeutralWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				if (Campaign.Current.IsMainHeroDisguised)
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
						LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise,
						AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.Disguised
					};
				}
				else if (Campaign.Current.Models.CrimeModel.DoesPlayerHaveAnyCrimeRating(settlement.MapFaction))
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
						LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Bribe,
						AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.CrimeRating
					};
				}
				else if (mainHero.Clan.Tier < 3)
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
						LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Bribe,
						AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.ClanTier
					};
				}
				else
				{
					accessDetails = new SettlementAccessModel.AccessDetails
					{
						AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
						AccessMethod = SettlementAccessModel.AccessMethod.Direct
					};
				}
			}
			else if (FactionManager.IsAtWarAgainstFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
					LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise,
					AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.Disguised
				};
			}
			if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && (accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe || accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise) && settlement.LocationComplex.GetListOfCharactersInLocation("lordshall").IsEmpty<LocationCharacter>() && settlement.LocationComplex.GetListOfCharactersInLocation("prison").IsEmpty<LocationCharacter>())
			{
				accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
				accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.LocationEmpty;
			}
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000822B0 File Offset: 0x000804B0
		public override bool CanMainHeroAccessLocation(Settlement settlement, string locationId, out bool disableOption, out TextObject disabledText)
		{
			disabledText = null;
			disableOption = false;
			bool flag = true;
			if (locationId == "center")
			{
				flag = this.CanMainHeroWalkAroundTownCenter(settlement, out disableOption, out disabledText);
			}
			else if (locationId == "arena")
			{
				flag = this.CanMainHeroGoToArena(settlement, out disableOption, out disabledText);
			}
			else if (locationId == "tavern")
			{
				flag = this.CanMainHeroGoToTavern(settlement, out disableOption, out disabledText);
			}
			else if (locationId == "lordshall")
			{
				SettlementAccessModel.AccessDetails accessDetails;
				this.CanMainHeroEnterLordsHall(settlement, out accessDetails);
				if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe)
				{
					flag = Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterLordsHall(settlement) == 0;
				}
				else
				{
					flag = accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess;
				}
			}
			else if (locationId == "prison")
			{
				SettlementAccessModel.AccessDetails accessDetails2;
				this.CanMainHeroEnterDungeon(settlement, out accessDetails2);
				if (accessDetails2.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails2.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Bribe)
				{
					flag = Campaign.Current.Models.BribeCalculationModel.GetBribeToEnterDungeon(settlement) == 0;
				}
				else
				{
					flag = accessDetails2.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess;
				}
			}
			else if (locationId == "house_1" || locationId == "house_2" || locationId == "house_3")
			{
				Location locationWithId = settlement.LocationComplex.GetLocationWithId(locationId);
				flag = locationWithId.IsReserved && (locationWithId.SpecialItems.Count > 0 || locationWithId.GetCharacterList().Any<LocationCharacter>());
			}
			else if (locationId == "port")
			{
				disableOption = true;
				disabledText = new TextObject("{=ILnr9eCQ}Door is locked!", null);
				flag = false;
			}
			else
			{
				Debug.FailedAssert("invalid location which is not supported by DefaultSettlementAccessModel", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSettlementAccessModel.cs", "CanMainHeroAccessLocation", 207);
			}
			return flag;
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00082464 File Offset: 0x00080664
		public override bool IsRequestMeetingOptionAvailable(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			bool flag = true;
			disableOption = false;
			disabledText = null;
			SettlementAccessModel.AccessDetails accessDetails;
			this.CanMainHeroEnterSettlement(settlement, out accessDetails);
			if (settlement.OwnerClan == Clan.PlayerClan)
			{
				flag = false;
			}
			else if (DiplomacyHelper.IsSameFactionAndNotEliminated(settlement.MapFaction, Clan.PlayerClan.MapFaction) && accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
			{
				flag = TownHelpers.IsThereAnyoneToMeetInTown(settlement);
			}
			else if (settlement.IsTown && FactionManager.IsNeutralWithFaction(Hero.MainHero.MapFaction, settlement.MapFaction) && Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingMild(settlement.MapFaction))
			{
				flag = false;
			}
			else if (Clan.PlayerClan.Tier < 3)
			{
				disableOption = true;
				disabledText = new TextObject("{=bdzZUVxf}Your clan tier is not high enough to request a meeting.", null);
				flag = true;
			}
			else if (TownHelpers.IsThereAnyoneToMeetInTown(settlement))
			{
				flag = true;
			}
			else
			{
				disableOption = true;
				disabledText = new TextObject("{=196tGVIm}There are no nobles to meet.", null);
			}
			return flag;
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x0008253C File Offset: 0x0008073C
		public override bool CanMainHeroDoSettlementAction(Settlement settlement, SettlementAccessModel.SettlementAction settlementAction, out bool disableOption, out TextObject disabledText)
		{
			switch (settlementAction)
			{
			case SettlementAccessModel.SettlementAction.RecruitTroops:
				return this.CanMainHeroRecruitTroops(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.Craft:
				return this.CanMainHeroCraft(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.WalkAroundTheArena:
				return this.CanMainHeroEnterArena(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.JoinTournament:
				return this.CanMainHeroJoinTournament(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.WatchTournament:
				return this.CanMainHeroWatchTournament(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.Trade:
				return this.CanMainHeroTrade(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.WaitInSettlement:
				return this.CanMainHeroWaitInSettlement(settlement, out disableOption, out disabledText);
			case SettlementAccessModel.SettlementAction.ManageTown:
				return this.CanMainHeroManageTown(settlement, out disableOption, out disabledText);
			default:
				Debug.FailedAssert("Invalid Settlement Action", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultSettlementAccessModel.cs", "CanMainHeroDoSettlementAction", 276);
				disableOption = false;
				disabledText = null;
				return true;
			}
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x000825EC File Offset: 0x000807EC
		private bool CanMainHeroGoToArena(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			if (Campaign.Current.IsMainHeroDisguised)
			{
				disabledText = new TextObject("{=brzz79Je}You cannot enter arena while in disguise.", null);
				disableOption = true;
				return false;
			}
			if (Campaign.Current.IsDay)
			{
				disabledText = null;
				disableOption = false;
				return true;
			}
			disabledText = new TextObject("{=wsbkjJhz}Arena is closed at night.", null);
			disableOption = true;
			return false;
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0008263C File Offset: 0x0008083C
		private bool CanMainHeroGoToTavern(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = null;
			disableOption = false;
			return true;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00082645 File Offset: 0x00080845
		private bool CanMainHeroEnterArena(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = null;
			return true;
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00082650 File Offset: 0x00080850
		private void CanMainHeroEnterVillage(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			Hero mainHero = Hero.MainHero;
			accessDetails = new SettlementAccessModel.AccessDetails
			{
				AccessLevel = SettlementAccessModel.AccessLevel.NoAccess,
				AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.None,
				PreliminaryActionObligation = SettlementAccessModel.PreliminaryActionObligation.None,
				PreliminaryActionType = SettlementAccessModel.PreliminaryActionType.None
			};
			MobileParty partyBelongedTo = mainHero.PartyBelongedTo;
			if (partyBelongedTo != null && (partyBelongedTo.Army == null || partyBelongedTo.Army.LeaderParty == partyBelongedTo))
			{
				accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.FullAccess;
				accessDetails.AccessMethod = SettlementAccessModel.AccessMethod.Direct;
			}
			if (settlement.Village.VillageState == Village.VillageStates.Looted)
			{
				accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
				accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.VillageIsLooted;
			}
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x000826DA File Offset: 0x000808DA
		private bool CanMainHeroManageTown(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = null;
			disableOption = false;
			return settlement.IsTown && settlement.OwnerClan.Leader == Hero.MainHero;
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00082700 File Offset: 0x00080900
		private void CanMainHeroEnterCastle(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			Hero mainHero = Hero.MainHero;
			accessDetails = default(SettlementAccessModel.AccessDetails);
			if (settlement.OwnerClan == mainHero.Clan)
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				return;
			}
			if (DiplomacyHelper.IsSameFactionAndNotEliminated(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.ByRequest
				};
				if (!settlement.Town.IsOwnerUnassigned && settlement.OwnerClan.Leader.GetRelationWithPlayer() < -4f && Hero.MainHero.MapFaction.Leader != Hero.MainHero)
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.RelationshipWithOwner;
					return;
				}
			}
			else if (FactionManager.IsNeutralWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.ByRequest
				};
				if (Campaign.Current.Models.CrimeModel.DoesPlayerHaveAnyCrimeRating(settlement.MapFaction))
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.CrimeRating;
					return;
				}
				if (settlement.OwnerClan.Leader.GetRelationWithPlayer() < 0f)
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.NoAccess;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.RelationshipWithOwner;
					return;
				}
			}
			else if (FactionManager.IsAtWarAgainstFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.NoAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.ByRequest,
					AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.HostileFaction
				};
			}
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0008288C File Offset: 0x00080A8C
		private void CanMainHeroEnterTown(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails)
		{
			Hero mainHero = Hero.MainHero;
			accessDetails = default(SettlementAccessModel.AccessDetails);
			if (settlement.OwnerClan == mainHero.Clan)
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				return;
			}
			if (DiplomacyHelper.IsSameFactionAndNotEliminated(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingModerate(settlement.MapFaction) || Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(settlement.MapFaction))
				{
					accessDetails.PreliminaryActionType = SettlementAccessModel.PreliminaryActionType.FaceCharges;
					accessDetails.PreliminaryActionObligation = SettlementAccessModel.PreliminaryActionObligation.Optional;
					return;
				}
			}
			else if (FactionManager.IsNeutralWithFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.FullAccess,
					AccessMethod = SettlementAccessModel.AccessMethod.Direct
				};
				if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingModerate(settlement.MapFaction) || Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(settlement.MapFaction))
				{
					accessDetails.AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess;
					accessDetails.AccessMethod = SettlementAccessModel.AccessMethod.None;
					accessDetails.LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise;
					accessDetails.AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.CrimeRating;
					return;
				}
			}
			else if (FactionManager.IsAtWarAgainstFaction(mainHero.MapFaction, settlement.MapFaction))
			{
				accessDetails = new SettlementAccessModel.AccessDetails
				{
					AccessLevel = SettlementAccessModel.AccessLevel.LimitedAccess,
					LimitedAccessSolution = SettlementAccessModel.LimitedAccessSolution.Disguise,
					AccessLimitationReason = SettlementAccessModel.AccessLimitationReason.HostileFaction
				};
			}
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00082A11 File Offset: 0x00080C11
		private bool CanMainHeroWalkAroundTownCenter(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = null;
			disableOption = false;
			return settlement.IsTown || settlement.IsCastle;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00082A2C File Offset: 0x00080C2C
		private bool CanMainHeroRecruitTroops(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disabledText = null;
			disableOption = false;
			if (settlement.IsVillage && settlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				disabledText = new TextObject("{=nHlqkdUC}You cannot recruit troops from a hostile village.", null);
				disableOption = true;
				return false;
			}
			return !Settlement.CurrentSettlement.IsVillage || Settlement.CurrentSettlement.Village.VillageState == Village.VillageStates.Normal;
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x00082A90 File Offset: 0x00080C90
		private bool CanMainHeroCraft(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = null;
			return Campaign.Current.IsCraftingEnabled;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00082AA4 File Offset: 0x00080CA4
		private bool CanMainHeroJoinTournament(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			bool flag = settlement.Town.HasTournament && Campaign.Current.IsDay;
			disableOption = false;
			disabledText = null;
			if (!flag)
			{
				return false;
			}
			if (Campaign.Current.IsMainHeroDisguised)
			{
				disableOption = true;
				disabledText = new TextObject("{=mu6Xl4RS}You cannot enter the tournament while disguised.", null);
				return false;
			}
			if (Hero.MainHero.IsWounded)
			{
				disableOption = true;
				disabledText = new TextObject("{=68rmPu7Z}Your health is too low to fight.", null);
				return false;
			}
			return true;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00082B12 File Offset: 0x00080D12
		private bool CanMainHeroWatchTournament(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = null;
			return settlement.Town.HasTournament && Campaign.Current.IsDay;
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00082B34 File Offset: 0x00080D34
		private bool CanMainHeroTrade(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			if (settlement.IsVillage)
			{
				Village village = Settlement.CurrentSettlement.Village;
				if (village.VillageState == Village.VillageStates.BeingRaided)
				{
					disabledText = null;
					disableOption = false;
					return false;
				}
				bool flag = settlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction);
				TextObject textObject = new TextObject("{=vYHVlydb}You cannot trade with a hostile village.", null);
				if (village.VillageState == Village.VillageStates.Normal && village.Owner.ItemRoster.Count > 0)
				{
					disableOption = flag;
					disabledText = (disableOption ? textObject : null);
					return !disableOption;
				}
				if (village.Gold > 0)
				{
					disableOption = flag;
					disabledText = (disableOption ? textObject : new TextObject("{=FbowXAC0}There are no available products right now.", null));
					return false;
				}
				disableOption = flag;
				disabledText = (disableOption ? textObject : new TextObject("{=bmfo7CaO}Village shop is not available right now.", null));
				return false;
			}
			else
			{
				if (Campaign.Current.IsMainHeroDisguised && !Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.SmugglerConnections))
				{
					disableOption = true;
					TextObject textObject2 = new TextObject("{=HVUHHuVA}{PERK_NAME} perk required to trade while in disguise.", null);
					textObject2.SetTextVariable("PERK_NAME", DefaultPerks.Roguery.SmugglerConnections.Name);
					disabledText = textObject2;
					return false;
				}
				disableOption = false;
				disabledText = null;
				return true;
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00082C44 File Offset: 0x00080E44
		private bool CanMainHeroWaitInSettlement(Settlement settlement, out bool disableOption, out TextObject disabledText)
		{
			disableOption = false;
			disabledText = null;
			if (Campaign.Current.IsMainHeroDisguised)
			{
				disableOption = true;
				disabledText = new TextObject("{=dN5Qc9vN}You cannot wait in town while in disguise.", null);
				return false;
			}
			if (settlement.IsVillage)
			{
				if (settlement.Party.MapEvent != null)
				{
					disableOption = true;
					disabledText = new TextObject("{=dN5Qc7vN}You cannot wait in village while it is being raided.", null);
					return false;
				}
				if (settlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					disableOption = true;
					disabledText = new TextObject("{=s7aas7L4}You cannot wait in a hostile village.", null);
					return false;
				}
			}
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				disableOption = true;
				disabledText = new TextObject("{=tjsld7WT}You cannot wait in town while you're a member of an army.", null);
				return false;
			}
			return true;
		}
	}
}
