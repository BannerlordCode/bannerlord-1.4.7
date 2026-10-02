using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D8 RID: 216
	public static class TroopTraitsMaskExtensions
	{
		// Token: 0x06000B3D RID: 2877 RVA: 0x00024A66 File Offset: 0x00022C66
		public static bool HasMelee(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Melee) > TroopTraitsMask.None;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00024A6E File Offset: 0x00022C6E
		public static bool HasRanged(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Ranged) > TroopTraitsMask.None;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00024A76 File Offset: 0x00022C76
		public static bool HasMount(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Mount) > TroopTraitsMask.None;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00024A7E File Offset: 0x00022C7E
		public static bool HasArmor(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Armor) > TroopTraitsMask.None;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00024A86 File Offset: 0x00022C86
		public static bool HasThrown(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Thrown) > TroopTraitsMask.None;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00024A8F File Offset: 0x00022C8F
		public static bool HasSpear(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Spear) > TroopTraitsMask.None;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00024A98 File Offset: 0x00022C98
		public static bool HasShield(this TroopTraitsMask troopTraitsMask)
		{
			return (troopTraitsMask & TroopTraitsMask.Shield) > TroopTraitsMask.None;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00024AA1 File Offset: 0x00022CA1
		public static bool HasLowTier(this TroopTraitsMask troopFilterMask)
		{
			return (troopFilterMask & TroopTraitsMask.LowTier) > TroopTraitsMask.None;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00024AAD File Offset: 0x00022CAD
		public static bool HasHighTier(this TroopTraitsMask troopFilterMask)
		{
			return (troopFilterMask & TroopTraitsMask.HighTier) > TroopTraitsMask.None;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00024ABC File Offset: 0x00022CBC
		public static string GetTroopTraitsText(this TroopTraitsMask troopTraitsMask)
		{
			string text = "";
			if (troopTraitsMask.HasMelee())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Melee, ref text);
			}
			else if (troopTraitsMask.HasRanged())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Ranged, ref text);
			}
			if (troopTraitsMask.HasMount())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Mount, ref text);
			}
			if (troopTraitsMask.HasArmor())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Armor, ref text);
			}
			if (troopTraitsMask.HasThrown())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Thrown, ref text);
			}
			if (troopTraitsMask.HasSpear())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Spear, ref text);
			}
			if (troopTraitsMask.HasShield())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Shield, ref text);
			}
			return text;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00024B48 File Offset: 0x00022D48
		public static string GetTraitsFilterText(this TroopTraitsMask troopTraitsFilter)
		{
			string text = "";
			if (troopTraitsFilter.HasArmor())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Armor, ref text);
			}
			if (troopTraitsFilter.HasThrown())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Thrown, ref text);
			}
			if (troopTraitsFilter.HasSpear())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Spear, ref text);
			}
			if (troopTraitsFilter.HasShield())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Shield, ref text);
			}
			if (troopTraitsFilter.HasLowTier())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.LowTier, ref text);
			}
			else if (troopTraitsFilter.HasHighTier())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.HighTier, ref text);
			}
			return text;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00024BCC File Offset: 0x00022DCC
		public static string GetClassFilterText(this TroopTraitsMask troopTraitsFilter)
		{
			string text = "";
			if (troopTraitsFilter.HasMelee())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Melee, ref text);
			}
			if (troopTraitsFilter.HasRanged())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Ranged, ref text);
			}
			if (troopTraitsFilter.HasMount())
			{
				TroopTraitsMaskExtensions.AddFlagToText(TroopTraitsMask.Mount, ref text);
			}
			return text;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00024C10 File Offset: 0x00022E10
		private static void AddFlagToText(TroopTraitsMask flag, ref string text)
		{
			if (text.Length > 0)
			{
				text += "|";
			}
			string text2;
			if (flag <= TroopTraitsMask.Thrown)
			{
				switch (flag)
				{
				case TroopTraitsMask.Melee:
					text2 = "Melee";
					goto IL_00B7;
				case TroopTraitsMask.Ranged:
					text2 = "Ranged";
					goto IL_00B7;
				case TroopTraitsMask.Melee | TroopTraitsMask.Ranged:
					break;
				case TroopTraitsMask.Mount:
					text2 = "Mount";
					goto IL_00B7;
				default:
					if (flag == TroopTraitsMask.Armor)
					{
						text2 = "Armor";
						goto IL_00B7;
					}
					if (flag == TroopTraitsMask.Thrown)
					{
						text2 = "Thrown";
						goto IL_00B7;
					}
					break;
				}
			}
			else if (flag <= TroopTraitsMask.Shield)
			{
				if (flag == TroopTraitsMask.Spear)
				{
					text2 = "Spear";
					goto IL_00B7;
				}
				if (flag == TroopTraitsMask.Shield)
				{
					text2 = "Shield";
					goto IL_00B7;
				}
			}
			else
			{
				if (flag == TroopTraitsMask.LowTier)
				{
					text2 = "Low Tier";
					goto IL_00B7;
				}
				if (flag == TroopTraitsMask.HighTier)
				{
					text2 = "High Tier";
					goto IL_00B7;
				}
			}
			text2 = "";
			IL_00B7:
			text += text2;
		}
	}
}
