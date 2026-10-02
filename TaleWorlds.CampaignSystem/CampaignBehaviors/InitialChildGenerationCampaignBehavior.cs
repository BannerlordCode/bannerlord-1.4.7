using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000406 RID: 1030
	public class InitialChildGenerationCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060040ED RID: 16621 RVA: 0x001308FC File Offset: 0x0012EAFC
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x00130918 File Offset: 0x0012EB18
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int index)
		{
			if (index == 0)
			{
				using (List<Clan>.Enumerator enumerator = Clan.All.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Clan clan = enumerator.Current;
						if (!clan.IsBanditFaction && !clan.IsMinorFaction && !clan.IsEliminated && clan != Clan.PlayerClan)
						{
							List<Hero> list = new List<Hero>();
							MBList<Hero> mblist = new MBList<Hero>();
							MBList<Hero> mblist2 = new MBList<Hero>();
							foreach (Hero hero in clan.AliveLords)
							{
								if (hero.IsChild)
								{
									list.Add(hero);
								}
								else if (hero.IsFemale)
								{
									mblist.Add(hero);
								}
								else
								{
									mblist2.Add(hero);
								}
							}
							int num = MathF.Ceiling((float)(mblist2.Count + mblist.Count) / 2f) - list.Count;
							float num2 = 0.49f;
							if (mblist2.Count == 0)
							{
								num2 = -1f;
							}
							Func<Clan, bool> <>9__0;
							for (int i = 0; i < num; i++)
							{
								bool isFemale = MBRandom.RandomFloat <= num2;
								Hero hero2 = (isFemale ? mblist.GetRandomElement<Hero>() : mblist2.GetRandomElement<Hero>());
								if (hero2 == null)
								{
									IEnumerable<Clan> nonBanditFactions = Clan.NonBanditFactions;
									Func<Clan, bool> func;
									if ((func = <>9__0) == null)
									{
										func = (<>9__0 = (Clan t) => t != clan && t.Culture == clan.Culture);
									}
									MBList<Clan> mblist3 = nonBanditFactions.Where<Clan>(func).ToMBList<Clan>();
									Func<Hero, bool> <>9__1;
									for (int j = 0; j < 10; j++)
									{
										IEnumerable<Hero> aliveLords = mblist3.GetRandomElement<Clan>().AliveLords;
										Func<Hero, bool> func2;
										if ((func2 = <>9__1) == null)
										{
											func2 = (<>9__1 = (Hero t) => t.IsFemale == isFemale);
										}
										hero2 = aliveLords.Where<Hero>(func2).ToMBList<Hero>().GetRandomElement<Hero>();
										if (hero2 != null)
										{
											break;
										}
									}
								}
								if (hero2 != null)
								{
									int num3 = MBRandom.RandomInt(2, 18);
									Hero hero3 = HeroCreator.CreateChild(hero2.CharacterObject, clan.HomeSettlement, clan, num3);
									hero3.UpdateHomeSettlement();
									hero3.HeroDeveloper.InitializeHeroDeveloper();
									Equipment equipmentForInitialChildrenGeneration = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForInitialChildrenGeneration(hero3);
									EquipmentHelper.AssignHeroEquipmentFromEquipment(hero3, equipmentForInitialChildrenGeneration);
									Equipment equipment = new Equipment(Equipment.EquipmentType.Battle);
									equipment.FillFrom(equipmentForInitialChildrenGeneration, false);
									EquipmentHelper.AssignHeroEquipmentFromEquipment(hero3, equipment);
								}
								if (num2 <= 0f)
								{
									num2 = 0.49f;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x00130BF8 File Offset: 0x0012EDF8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0400130C RID: 4876
		private const float FemaleChildrenChance = 0.49f;
	}
}
