using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000CF RID: 207
	public class HeroAgeComparer : IComparer<HeroVM>
	{
		// Token: 0x060013C6 RID: 5062 RVA: 0x0004F865 File Offset: 0x0004DA65
		public HeroAgeComparer(bool isAscending)
		{
			this._isAscending = isAscending;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0004F874 File Offset: 0x0004DA74
		int IComparer<HeroVM>.Compare(HeroVM x, HeroVM y)
		{
			int num = x.Hero.Age.CompareTo(y.Hero.Age) * (this._isAscending ? 1 : (-1));
			if (num == 0)
			{
				num = x.NameText.CompareTo(y.NameText);
			}
			return num;
		}

		// Token: 0x0400090D RID: 2317
		private readonly bool _isAscending;
	}
}
