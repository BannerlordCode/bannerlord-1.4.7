using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x02000086 RID: 134
	public class KingdomClanItemVM : KingdomItemVM
	{
		// Token: 0x06000B3B RID: 2875 RVA: 0x0002FA6C File Offset: 0x0002DC6C
		public KingdomClanItemVM(Clan clan, Action<KingdomClanItemVM> onSelect)
		{
			this.Clan = clan;
			this._onSelect = onSelect;
			this.Banner = new BannerImageIdentifierVM(clan.Banner, false);
			this.Banner_9 = new BannerImageIdentifierVM(clan.Banner, true);
			this.RefreshValues();
			this.Refresh();
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0002FAC4 File Offset: 0x0002DCC4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Clan.Name.ToString();
			GameTexts.SetVariable("TIER", this.Clan.Tier);
			this.TierText = GameTexts.FindText("str_clan_tier", null).ToString();
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002FB18 File Offset: 0x0002DD18
		public void Refresh()
		{
			this.Members = new MBBindingList<HeroVM>();
			this.ClanType = 0;
			if (this.Clan.IsUnderMercenaryService)
			{
				this.ClanType = 2;
			}
			else if (this.Clan.Kingdom.RulingClan == this.Clan)
			{
				this.ClanType = 1;
			}
			foreach (Hero hero in this.Clan.Heroes.Where<Hero>((Hero h) => !h.IsDisabled && !h.IsNotSpawned && h.IsAlive && !h.IsChild))
			{
				this.Members.Add(new HeroVM(hero, false));
			}
			this.NumOfMembers = this.Members.Count;
			this.Fiefs = new MBBindingList<KingdomClanFiefItemVM>();
			foreach (Settlement settlement in this.Clan.Settlements.Where<Settlement>((Settlement s) => s.IsTown || s.IsCastle))
			{
				this.Fiefs.Add(new KingdomClanFiefItemVM(settlement));
			}
			this.NumOfFiefs = this.Fiefs.Count;
			this.Influence = (int)this.Clan.Influence;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002FC90 File Offset: 0x0002DE90
		protected override void OnSelect()
		{
			base.OnSelect();
			this._onSelect(this);
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x0002FCA4 File Offset: 0x0002DEA4
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0002FCAC File Offset: 0x0002DEAC
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

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0002FCCF File Offset: 0x0002DECF
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0002FCD7 File Offset: 0x0002DED7
		[DataSourceProperty]
		public int ClanType
		{
			get
			{
				return this._clanType;
			}
			set
			{
				if (value != this._clanType)
				{
					this._clanType = value;
					base.OnPropertyChangedWithValue(value, "ClanType");
				}
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0002FCF5 File Offset: 0x0002DEF5
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0002FCFD File Offset: 0x0002DEFD
		[DataSourceProperty]
		public int NumOfMembers
		{
			get
			{
				return this._numOfMembers;
			}
			set
			{
				if (value != this._numOfMembers)
				{
					this._numOfMembers = value;
					base.OnPropertyChangedWithValue(value, "NumOfMembers");
				}
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0002FD1B File Offset: 0x0002DF1B
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0002FD23 File Offset: 0x0002DF23
		[DataSourceProperty]
		public int NumOfFiefs
		{
			get
			{
				return this._numOfFiefs;
			}
			set
			{
				if (value != this._numOfFiefs)
				{
					this._numOfFiefs = value;
					base.OnPropertyChangedWithValue(value, "NumOfFiefs");
				}
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0002FD41 File Offset: 0x0002DF41
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0002FD49 File Offset: 0x0002DF49
		[DataSourceProperty]
		public string TierText
		{
			get
			{
				return this._tierText;
			}
			set
			{
				if (value != this._tierText)
				{
					this._tierText = value;
					base.OnPropertyChangedWithValue<string>(value, "TierText");
				}
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0002FD6C File Offset: 0x0002DF6C
		// (set) Token: 0x06000B4A RID: 2890 RVA: 0x0002FD74 File Offset: 0x0002DF74
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner)
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0002FD92 File Offset: 0x0002DF92
		// (set) Token: 0x06000B4C RID: 2892 RVA: 0x0002FD9A File Offset: 0x0002DF9A
		[DataSourceProperty]
		public BannerImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				if (value != this._banner_9)
				{
					this._banner_9 = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner_9");
				}
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0002FDB8 File Offset: 0x0002DFB8
		// (set) Token: 0x06000B4E RID: 2894 RVA: 0x0002FDC0 File Offset: 0x0002DFC0
		[DataSourceProperty]
		public MBBindingList<HeroVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				if (value != this._members)
				{
					this._members = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Members");
				}
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0002FDDE File Offset: 0x0002DFDE
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0002FDE6 File Offset: 0x0002DFE6
		[DataSourceProperty]
		public MBBindingList<KingdomClanFiefItemVM> Fiefs
		{
			get
			{
				return this._fiefs;
			}
			set
			{
				if (value != this._fiefs)
				{
					this._fiefs = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomClanFiefItemVM>>(value, "Fiefs");
				}
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0002FE04 File Offset: 0x0002E004
		// (set) Token: 0x06000B52 RID: 2898 RVA: 0x0002FE0C File Offset: 0x0002E00C
		[DataSourceProperty]
		public int Influence
		{
			get
			{
				return this._influence;
			}
			set
			{
				if (value != this._influence)
				{
					this._influence = value;
					base.OnPropertyChangedWithValue(value, "Influence");
				}
			}
		}

		// Token: 0x040004FF RID: 1279
		private readonly Action<KingdomClanItemVM> _onSelect;

		// Token: 0x04000500 RID: 1280
		public readonly Clan Clan;

		// Token: 0x04000501 RID: 1281
		private string _name;

		// Token: 0x04000502 RID: 1282
		private BannerImageIdentifierVM _banner;

		// Token: 0x04000503 RID: 1283
		private BannerImageIdentifierVM _banner_9;

		// Token: 0x04000504 RID: 1284
		private MBBindingList<HeroVM> _members;

		// Token: 0x04000505 RID: 1285
		private MBBindingList<KingdomClanFiefItemVM> _fiefs;

		// Token: 0x04000506 RID: 1286
		private int _influence;

		// Token: 0x04000507 RID: 1287
		private int _numOfMembers;

		// Token: 0x04000508 RID: 1288
		private int _numOfFiefs;

		// Token: 0x04000509 RID: 1289
		private string _tierText;

		// Token: 0x0400050A RID: 1290
		private int _clanType = -1;

		// Token: 0x020001E6 RID: 486
		private enum ClanTypes
		{
			// Token: 0x0400115B RID: 4443
			Normal,
			// Token: 0x0400115C RID: 4444
			Leader,
			// Token: 0x0400115D RID: 4445
			Mercenary
		}
	}
}
