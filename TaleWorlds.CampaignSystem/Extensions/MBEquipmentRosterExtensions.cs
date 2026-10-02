using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000171 RID: 369
	public static class MBEquipmentRosterExtensions
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x0008D56A File Offset: 0x0008B76A
		public static MBReadOnlyList<MBEquipmentRoster> All
		{
			get
			{
				return Campaign.Current.AllEquipmentRosters;
			}
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0008D576 File Offset: 0x0008B776
		public static IEnumerable<Equipment> GetCivilianEquipments(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.Where<Equipment>((Equipment x) => x.IsCivilian);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0008D5A2 File Offset: 0x0008B7A2
		public static IEnumerable<Equipment> GetStealthEquipments(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.Where<Equipment>((Equipment x) => x.IsStealth);
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0008D5CE File Offset: 0x0008B7CE
		public static IEnumerable<Equipment> GetBattleEquipments(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.Where<Equipment>((Equipment x) => x.IsBattle);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0008D5FA File Offset: 0x0008B7FA
		public static Equipment GetRandomCivilianEquipment(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.GetRandomElementWithPredicate<Equipment>((Equipment x) => x.IsCivilian);
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0008D626 File Offset: 0x0008B826
		public static Equipment GetRandomStealthEquipment(this MBEquipmentRoster instance)
		{
			return instance.AllEquipments.GetRandomElementWithPredicate<Equipment>((Equipment x) => x.IsStealth);
		}
	}
}
