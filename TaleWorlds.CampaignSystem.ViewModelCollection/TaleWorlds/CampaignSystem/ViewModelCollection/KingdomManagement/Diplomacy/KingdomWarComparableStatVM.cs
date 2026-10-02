using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000071 RID: 113
	public class KingdomWarComparableStatVM : ViewModel
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x00029600 File Offset: 0x00027800
		public KingdomWarComparableStatVM(int faction1Stat, int faction2Stat, TextObject name, string faction1Color, string faction2Color, int defaultRange, BasicTooltipViewModel faction1Hint = null, BasicTooltipViewModel faction2Hint = null)
		{
			int num = MathF.Max(MathF.Max(faction1Stat, faction2Stat), defaultRange);
			if (num == 0)
			{
				num = 1;
			}
			this.Faction1Color = faction1Color;
			this.Faction2Color = faction2Color;
			this.Faction1Value = faction1Stat;
			this.Faction2Value = faction2Stat;
			this._defaultRange = defaultRange;
			this.Faction1Percentage = MathF.Round((float)faction1Stat / (float)num * 100f);
			this.Faction2Percentage = MathF.Round((float)faction2Stat / (float)num * 100f);
			this._nameObj = name;
			this.Faction1Hint = faction1Hint;
			this.Faction2Hint = faction2Hint;
			this.RefreshValues();
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00029696 File Offset: 0x00027896
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameObj.ToString();
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x000296AF File Offset: 0x000278AF
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x000296B7 File Offset: 0x000278B7
		[DataSourceProperty]
		public BasicTooltipViewModel Faction1Hint
		{
			get
			{
				return this._faction1Hint;
			}
			set
			{
				if (value != this._faction1Hint)
				{
					this._faction1Hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Faction1Hint");
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x000296D5 File Offset: 0x000278D5
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x000296DD File Offset: 0x000278DD
		[DataSourceProperty]
		public BasicTooltipViewModel Faction2Hint
		{
			get
			{
				return this._faction2Hint;
			}
			set
			{
				if (value != this._faction2Hint)
				{
					this._faction2Hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Faction2Hint");
				}
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x000296FB File Offset: 0x000278FB
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00029703 File Offset: 0x00027903
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00029726 File Offset: 0x00027926
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x0002972E File Offset: 0x0002792E
		[DataSourceProperty]
		public string Faction1Color
		{
			get
			{
				return this._faction1Color;
			}
			set
			{
				if (value != this._faction1Color)
				{
					this._faction1Color = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction1Color");
				}
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00029751 File Offset: 0x00027951
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x00029759 File Offset: 0x00027959
		[DataSourceProperty]
		public string Faction2Color
		{
			get
			{
				return this._faction2Color;
			}
			set
			{
				if (value != this._faction2Color)
				{
					this._faction2Color = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction2Color");
				}
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0002977C File Offset: 0x0002797C
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x00029784 File Offset: 0x00027984
		[DataSourceProperty]
		public int Faction1Percentage
		{
			get
			{
				return this._faction1Percentage;
			}
			set
			{
				if (value != this._faction1Percentage)
				{
					this._faction1Percentage = value;
					base.OnPropertyChangedWithValue(value, "Faction1Percentage");
				}
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x000297A2 File Offset: 0x000279A2
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x000297AA File Offset: 0x000279AA
		[DataSourceProperty]
		public int Faction1Value
		{
			get
			{
				return this._faction1Value;
			}
			set
			{
				if (value != this._faction1Value)
				{
					this._faction1Value = value;
					base.OnPropertyChangedWithValue(value, "Faction1Value");
				}
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x000297C8 File Offset: 0x000279C8
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x000297D0 File Offset: 0x000279D0
		[DataSourceProperty]
		public int Faction2Percentage
		{
			get
			{
				return this._faction2Percentage;
			}
			set
			{
				if (value != this._faction2Percentage)
				{
					this._faction2Percentage = value;
					base.OnPropertyChangedWithValue(value, "Faction2Percentage");
				}
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x000297EE File Offset: 0x000279EE
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x000297F6 File Offset: 0x000279F6
		[DataSourceProperty]
		public int Faction2Value
		{
			get
			{
				return this._faction2Value;
			}
			set
			{
				if (value != this._faction2Value)
				{
					this._faction2Value = value;
					base.OnPropertyChangedWithValue(value, "Faction2Value");
				}
			}
		}

		// Token: 0x0400040E RID: 1038
		private TextObject _nameObj;

		// Token: 0x0400040F RID: 1039
		private int _defaultRange;

		// Token: 0x04000410 RID: 1040
		private BasicTooltipViewModel _faction1Hint;

		// Token: 0x04000411 RID: 1041
		private BasicTooltipViewModel _faction2Hint;

		// Token: 0x04000412 RID: 1042
		private string _name;

		// Token: 0x04000413 RID: 1043
		private string _faction1Color;

		// Token: 0x04000414 RID: 1044
		private string _faction2Color;

		// Token: 0x04000415 RID: 1045
		private int _faction1Percentage;

		// Token: 0x04000416 RID: 1046
		private int _faction1Value;

		// Token: 0x04000417 RID: 1047
		private int _faction2Percentage;

		// Token: 0x04000418 RID: 1048
		private int _faction2Value;
	}
}
