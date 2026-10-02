using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000065 RID: 101
	public static class TroopClassExtensions
	{
		// Token: 0x06000736 RID: 1846 RVA: 0x00018E74 File Offset: 0x00017074
		public static bool IsRanged(this FormationClass troopClass)
		{
			FormationClass formationClass = troopClass.DefaultClass();
			return formationClass == FormationClass.Ranged || formationClass == FormationClass.HorseArcher;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00018E92 File Offset: 0x00017092
		public static bool IsMounted(this FormationClass troopClass)
		{
			return troopClass.DefaultClass() == FormationClass.Cavalry || troopClass == FormationClass.HorseArcher;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00018EA3 File Offset: 0x000170A3
		public static bool IsMeleeInfantry(this FormationClass troopClass)
		{
			return troopClass.DefaultClass() == FormationClass.Infantry;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00018EAE File Offset: 0x000170AE
		public static bool IsMeleeCavalry(this FormationClass troopClass)
		{
			return troopClass.DefaultClass() == FormationClass.Cavalry;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00018EBC File Offset: 0x000170BC
		public static FormationClass DefaultClass(this FormationClass troopClass)
		{
			if (troopClass.IsRegularFormationClass())
			{
				FormationClass formationClass = troopClass;
				switch (troopClass)
				{
				case FormationClass.NumberOfDefaultFormations:
					formationClass = FormationClass.Ranged;
					break;
				case FormationClass.HeavyInfantry:
					formationClass = FormationClass.Infantry;
					break;
				case FormationClass.LightCavalry:
					formationClass = FormationClass.HorseArcher;
					break;
				case FormationClass.HeavyCavalry:
					formationClass = FormationClass.Cavalry;
					break;
				}
				return formationClass;
			}
			return FormationClass.Infantry;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00018EFE File Offset: 0x000170FE
		public static FormationClass AlternativeClass(this FormationClass troopClass)
		{
			switch (troopClass)
			{
			case FormationClass.Infantry:
				return FormationClass.Ranged;
			case FormationClass.Ranged:
				return FormationClass.Infantry;
			case FormationClass.Cavalry:
				return FormationClass.HorseArcher;
			case FormationClass.HorseArcher:
				return FormationClass.Cavalry;
			case FormationClass.NumberOfDefaultFormations:
				return FormationClass.HeavyInfantry;
			case FormationClass.HeavyInfantry:
				return FormationClass.NumberOfDefaultFormations;
			case FormationClass.LightCavalry:
				return FormationClass.HeavyCavalry;
			case FormationClass.HeavyCavalry:
				return FormationClass.LightCavalry;
			default:
				return troopClass;
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00018F3C File Offset: 0x0001713C
		public static FormationClass DismountedClass(this FormationClass troopClass)
		{
			FormationClass formationClass = troopClass;
			switch (troopClass)
			{
			case FormationClass.Cavalry:
				formationClass = FormationClass.Infantry;
				break;
			case FormationClass.HorseArcher:
				formationClass = FormationClass.Ranged;
				break;
			case FormationClass.LightCavalry:
				formationClass = FormationClass.NumberOfDefaultFormations;
				break;
			case FormationClass.HeavyCavalry:
				formationClass = FormationClass.HeavyInfantry;
				break;
			}
			return formationClass;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00018F7C File Offset: 0x0001717C
		public static bool IsDefaultTroopClass(this FormationClass troopClass)
		{
			return troopClass >= FormationClass.Infantry && troopClass < FormationClass.NumberOfDefaultFormations;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00018F98 File Offset: 0x00017198
		public static bool IsRegularTroopClass(this FormationClass troopClass)
		{
			return troopClass >= FormationClass.Infantry && troopClass < FormationClass.NumberOfRegularFormations;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00018FB4 File Offset: 0x000171B4
		public static FormationClass GetNextSpawnPrioritizedClass(this FormationClass troopClass)
		{
			if (troopClass.IsRegularTroopClass())
			{
				switch (troopClass)
				{
				case FormationClass.Infantry:
					return FormationClass.HeavyInfantry;
				case FormationClass.Ranged:
					return FormationClass.LightCavalry;
				case FormationClass.Cavalry:
					return FormationClass.HeavyCavalry;
				case FormationClass.HorseArcher:
					return FormationClass.HorseArcher;
				case FormationClass.NumberOfDefaultFormations:
					return FormationClass.Ranged;
				case FormationClass.HeavyInfantry:
					return FormationClass.Cavalry;
				case FormationClass.LightCavalry:
					return FormationClass.HorseArcher;
				case FormationClass.HeavyCavalry:
					return FormationClass.HeavyCavalry;
				}
			}
			return troopClass;
		}
	}
}
