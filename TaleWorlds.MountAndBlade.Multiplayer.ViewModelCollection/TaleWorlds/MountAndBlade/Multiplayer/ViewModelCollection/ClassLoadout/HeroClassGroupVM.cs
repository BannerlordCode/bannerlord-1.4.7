using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A2 RID: 162
	public class HeroClassGroupVM : ViewModel
	{
		// Token: 0x06000F71 RID: 3953 RVA: 0x0002FA74 File Offset: 0x0002DC74
		public HeroClassGroupVM(Action<HeroClassVM> onSelect, Action<HeroPerkVM, MPPerkVM> onPerkSelect, MultiplayerClassDivisions.MPHeroClassGroup heroClassGroup, MultiplayerBattleColors.MultiplayerCultureColorInfo colorInfo)
		{
			this.HeroClassGroup = heroClassGroup;
			this._onPerkSelect = onPerkSelect;
			this.IconType = heroClassGroup.StringId;
			this.SubClasses = new MBBindingList<HeroClassVM>();
			Team team = GameNetwork.MyPeer.GetComponent<MissionPeer>().Team;
			IEnumerable<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses(GameNetwork.MyPeer.GetComponent<MissionPeer>().Culture);
			Func<MultiplayerClassDivisions.MPHeroClass, bool> <>9__0;
			Func<MultiplayerClassDivisions.MPHeroClass, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (MultiplayerClassDivisions.MPHeroClass h) => h.ClassGroup.Equals(heroClassGroup));
			}
			foreach (MultiplayerClassDivisions.MPHeroClass mpheroClass in mpheroClasses.Where<MultiplayerClassDivisions.MPHeroClass>(func))
			{
				this.SubClasses.Add(new HeroClassVM(onSelect, this._onPerkSelect, mpheroClass, colorInfo));
			}
			this.RefreshValues();
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0002FB60 File Offset: 0x0002DD60
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.HeroClassGroup.Name.ToString();
			this.SubClasses.ApplyActionOnAllItems(delegate(HeroClassVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0002FBB3 File Offset: 0x0002DDB3
		public bool IsValid
		{
			get
			{
				return this.SubClasses.Count > 0;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0002FBC3 File Offset: 0x0002DDC3
		// (set) Token: 0x06000F75 RID: 3957 RVA: 0x0002FBCB File Offset: 0x0002DDCB
		[DataSourceProperty]
		public MBBindingList<HeroClassVM> SubClasses
		{
			get
			{
				return this._subClasses;
			}
			set
			{
				if (value != this._subClasses)
				{
					this._subClasses = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroClassVM>>(value, "SubClasses");
				}
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06000F76 RID: 3958 RVA: 0x0002FBE9 File Offset: 0x0002DDE9
		// (set) Token: 0x06000F77 RID: 3959 RVA: 0x0002FBF1 File Offset: 0x0002DDF1
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

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06000F78 RID: 3960 RVA: 0x0002FC14 File Offset: 0x0002DE14
		// (set) Token: 0x06000F79 RID: 3961 RVA: 0x0002FC1C File Offset: 0x0002DE1C
		[DataSourceProperty]
		public string IconType
		{
			get
			{
				return this._iconType;
			}
			set
			{
				if (value != this._iconType)
				{
					this._iconType = value;
					base.OnPropertyChangedWithValue<string>(value, "IconType");
					this.IconPath = "TroopBanners\\ClassType_" + value;
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0002FC50 File Offset: 0x0002DE50
		// (set) Token: 0x06000F7B RID: 3963 RVA: 0x0002FC58 File Offset: 0x0002DE58
		[DataSourceProperty]
		public string IconPath
		{
			get
			{
				return this._iconPath;
			}
			set
			{
				if (value != this._iconPath)
				{
					this._iconPath = value;
					base.OnPropertyChangedWithValue<string>(value, "IconPath");
				}
			}
		}

		// Token: 0x0400072B RID: 1835
		public readonly MultiplayerClassDivisions.MPHeroClassGroup HeroClassGroup;

		// Token: 0x0400072C RID: 1836
		private readonly Action<HeroPerkVM, MPPerkVM> _onPerkSelect;

		// Token: 0x0400072D RID: 1837
		private string _name;

		// Token: 0x0400072E RID: 1838
		private string _iconType;

		// Token: 0x0400072F RID: 1839
		private string _iconPath;

		// Token: 0x04000730 RID: 1840
		private MBBindingList<HeroClassVM> _subClasses;
	}
}
