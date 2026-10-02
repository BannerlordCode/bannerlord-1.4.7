using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x02000020 RID: 32
	public static class UIColors
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000128AE File Offset: 0x00010AAE
		public static Color PositiveIndicator
		{
			get
			{
				return UIColors._positiveIndicator;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000128B5 File Offset: 0x00010AB5
		public static Color NegativeIndicator
		{
			get
			{
				return UIColors._negativeIndicator;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000128BC File Offset: 0x00010ABC
		public static Color Gold
		{
			get
			{
				return UIColors._gold;
			}
		}

		// Token: 0x040000DF RID: 223
		private static Color _positiveIndicator = Color.FromUint(4285250886U);

		// Token: 0x040000E0 RID: 224
		private static Color _negativeIndicator = Color.FromUint(4290070086U);

		// Token: 0x040000E1 RID: 225
		private static Color _gold = Color.FromUint(4294957447U);
	}
}
