using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000066 RID: 102
	public static class FormationClassExtensions
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x00019002 File Offset: 0x00017202
		public static string GetName(this FormationClass formationClass)
		{
			if (formationClass == FormationClass.NumberOfDefaultFormations)
			{
				return "Skirmisher";
			}
			if (formationClass == FormationClass.NumberOfRegularFormations)
			{
				return "General";
			}
			if (formationClass != FormationClass.NumberOfAllFormations)
			{
				return formationClass.ToString();
			}
			return "Unset";
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00019034 File Offset: 0x00017234
		public static TextObject GetLocalizedName(this FormationClass formationClass)
		{
			string text = "str_troop_group_name";
			int num = (int)formationClass;
			return GameTexts.FindText(text, num.ToString());
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00019054 File Offset: 0x00017254
		public static TroopUsageFlags GetTroopUsageFlags(this FormationClass troopClass)
		{
			switch (troopClass)
			{
			case FormationClass.Ranged:
				return TroopUsageFlags.OnFoot | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;
			case FormationClass.Cavalry:
				return TroopUsageFlags.Mounted | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;
			case FormationClass.HorseArcher:
				return TroopUsageFlags.Mounted | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;
			}
			return TroopUsageFlags.OnFoot | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00019088 File Offset: 0x00017288
		public static TroopType GetTroopTypeForRegularFormation(this FormationClass formationClass)
		{
			TroopType troopType = TroopType.Invalid;
			switch (formationClass)
			{
			case FormationClass.Infantry:
			case FormationClass.HeavyInfantry:
				troopType = TroopType.Infantry;
				break;
			case FormationClass.Ranged:
			case FormationClass.NumberOfDefaultFormations:
				troopType = TroopType.Ranged;
				break;
			case FormationClass.Cavalry:
			case FormationClass.HorseArcher:
			case FormationClass.LightCavalry:
			case FormationClass.HeavyCavalry:
				troopType = TroopType.Cavalry;
				break;
			default:
				Debug.FailedAssert(string.Format("Undefined formation class {0} for TroopType!", formationClass), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\FormationClass.cs", "GetTroopTypeForRegularFormation", 321);
				break;
			}
			return troopType;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000190F0 File Offset: 0x000172F0
		public static bool IsDefaultFormationClass(this FormationClass formationClass)
		{
			return formationClass >= FormationClass.Infantry && formationClass < FormationClass.NumberOfDefaultFormations;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001910C File Offset: 0x0001730C
		public static bool IsRegularFormationClass(this FormationClass formationClass)
		{
			return formationClass >= FormationClass.Infantry && formationClass < FormationClass.NumberOfRegularFormations;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00019125 File Offset: 0x00017325
		public static FormationClass FallbackClass(this FormationClass formationClass)
		{
			if (formationClass == FormationClass.Ranged || formationClass == FormationClass.NumberOfDefaultFormations)
			{
				return FormationClass.Ranged;
			}
			if (formationClass == FormationClass.Cavalry || formationClass == FormationClass.HeavyCavalry)
			{
				return FormationClass.Cavalry;
			}
			if (formationClass == FormationClass.HorseArcher || formationClass == FormationClass.LightCavalry)
			{
				return FormationClass.HorseArcher;
			}
			return FormationClass.Infantry;
		}

		// Token: 0x040003DC RID: 988
		public const TroopUsageFlags DefaultInfantryTroopUsageFlags = TroopUsageFlags.OnFoot | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;

		// Token: 0x040003DD RID: 989
		public const TroopUsageFlags DefaultRangedTroopUsageFlags = TroopUsageFlags.OnFoot | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;

		// Token: 0x040003DE RID: 990
		public const TroopUsageFlags DefaultCavalryTroopUsageFlags = TroopUsageFlags.Mounted | TroopUsageFlags.Melee | TroopUsageFlags.OneHandedUser | TroopUsageFlags.ShieldUser | TroopUsageFlags.TwoHandedUser | TroopUsageFlags.PolearmUser;

		// Token: 0x040003DF RID: 991
		public const TroopUsageFlags DefaultHorseArcherTroopUsageFlags = TroopUsageFlags.Mounted | TroopUsageFlags.Ranged | TroopUsageFlags.BowUser | TroopUsageFlags.ThrownUser | TroopUsageFlags.CrossbowUser;

		// Token: 0x040003E0 RID: 992
		public static FormationClass[] FormationClassValues = (FormationClass[])Enum.GetValues(typeof(FormationClass));
	}
}
