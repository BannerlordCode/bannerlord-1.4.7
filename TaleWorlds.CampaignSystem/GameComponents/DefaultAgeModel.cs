using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000EE RID: 238
	public class DefaultAgeModel : AgeModel
	{
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x00063B9D File Offset: 0x00061D9D
		public override int BecomeInfantAge
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x00063BA0 File Offset: 0x00061DA0
		public override int BecomeChildAge
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060015F6 RID: 5622 RVA: 0x00063BA3 File Offset: 0x00061DA3
		public override int BecomeTeenagerAge
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00063BA7 File Offset: 0x00061DA7
		public override int HeroComesOfAge
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x00063BAB File Offset: 0x00061DAB
		public override int MiddleAdultHoodAge
		{
			get
			{
				return 35;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060015F9 RID: 5625 RVA: 0x00063BAF File Offset: 0x00061DAF
		public override int BecomeOldAge
		{
			get
			{
				return 55;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x00063BB3 File Offset: 0x00061DB3
		public override int MaxAge
		{
			get
			{
				return 128;
			}
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00063BBC File Offset: 0x00061DBC
		public override void GetAgeLimitForLocation(CharacterObject character, out int minimumAge, out int maximumAge, string additionalTags = "")
		{
			if (character.Occupation == Occupation.TavernWench)
			{
				minimumAge = 20;
				maximumAge = 28;
				return;
			}
			if (character.Occupation == Occupation.Townsfolk)
			{
				if (additionalTags == "TavernVisitor")
				{
					minimumAge = 20;
					maximumAge = 60;
					return;
				}
				if (additionalTags == "TavernDrinker")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "SlowTownsman")
				{
					minimumAge = 50;
					maximumAge = 70;
					return;
				}
				if (additionalTags == "TownsfolkCarryingStuff")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "BroomsWoman")
				{
					minimumAge = 30;
					maximumAge = 45;
					return;
				}
				if (additionalTags == "Dancer")
				{
					minimumAge = 20;
					maximumAge = 28;
					return;
				}
				if (additionalTags == "Beggar")
				{
					minimumAge = 60;
					maximumAge = 90;
					return;
				}
				if (additionalTags == "Child")
				{
					minimumAge = this.BecomeChildAge;
					maximumAge = this.BecomeTeenagerAge;
					return;
				}
				if (additionalTags == "Teenager")
				{
					minimumAge = this.BecomeTeenagerAge;
					maximumAge = this.HeroComesOfAge;
					return;
				}
				if (additionalTags == "Infant")
				{
					minimumAge = this.BecomeInfantAge;
					maximumAge = this.BecomeChildAge;
					return;
				}
				if (additionalTags == "Notary" || additionalTags == "Barber")
				{
					minimumAge = 30;
					maximumAge = 80;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = 70;
				return;
			}
			else if (character.Occupation == Occupation.Villager)
			{
				if (additionalTags == "TownsfolkCarryingStuff")
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (additionalTags == "Child")
				{
					minimumAge = this.BecomeChildAge;
					maximumAge = this.BecomeTeenagerAge;
					return;
				}
				if (additionalTags == "Teenager")
				{
					minimumAge = this.BecomeTeenagerAge;
					maximumAge = this.HeroComesOfAge;
					return;
				}
				if (additionalTags == "Infant")
				{
					minimumAge = this.BecomeInfantAge;
					maximumAge = this.BecomeChildAge;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = 70;
				return;
			}
			else
			{
				if (character.Occupation == Occupation.TavernGameHost)
				{
					minimumAge = 30;
					maximumAge = 40;
					return;
				}
				if (character.Occupation == Occupation.Musician)
				{
					minimumAge = 20;
					maximumAge = 40;
					return;
				}
				if (character.Occupation == Occupation.ArenaMaster)
				{
					minimumAge = 30;
					maximumAge = 60;
					return;
				}
				if (character.Occupation == Occupation.ShopWorker)
				{
					minimumAge = 18;
					maximumAge = 50;
					return;
				}
				if (character.Occupation == Occupation.Tavernkeeper)
				{
					minimumAge = 40;
					maximumAge = 80;
					return;
				}
				if (character.Occupation == Occupation.RansomBroker)
				{
					minimumAge = 30;
					maximumAge = 60;
					return;
				}
				if (character.Occupation == Occupation.Blacksmith || character.Occupation == Occupation.GoodsTrader || character.Occupation == Occupation.HorseTrader || character.Occupation == Occupation.Armorer || character.Occupation == Occupation.Weaponsmith)
				{
					minimumAge = 30;
					maximumAge = 80;
					return;
				}
				if (additionalTags == "AlleyGangMember")
				{
					minimumAge = 30;
					maximumAge = 40;
					return;
				}
				minimumAge = this.HeroComesOfAge;
				maximumAge = this.MaxAge;
				return;
			}
		}

		// Token: 0x04000740 RID: 1856
		public const string TavernVisitorTag = "TavernVisitor";

		// Token: 0x04000741 RID: 1857
		public const string TavernDrinkerTag = "TavernDrinker";

		// Token: 0x04000742 RID: 1858
		public const string SlowTownsmanTag = "SlowTownsman";

		// Token: 0x04000743 RID: 1859
		public const string TownsfolkCarryingStuffTag = "TownsfolkCarryingStuff";

		// Token: 0x04000744 RID: 1860
		public const string BroomsWomanTag = "BroomsWoman";

		// Token: 0x04000745 RID: 1861
		public const string DancerTag = "Dancer";

		// Token: 0x04000746 RID: 1862
		public const string BeggarTag = "Beggar";

		// Token: 0x04000747 RID: 1863
		public const string ChildTag = "Child";

		// Token: 0x04000748 RID: 1864
		public const string TeenagerTag = "Teenager";

		// Token: 0x04000749 RID: 1865
		public const string InfantTag = "Infant";

		// Token: 0x0400074A RID: 1866
		public const string NotaryTag = "Notary";

		// Token: 0x0400074B RID: 1867
		public const string BarberTag = "Barber";

		// Token: 0x0400074C RID: 1868
		public const string AlleyGangMemberTag = "AlleyGangMember";
	}
}
