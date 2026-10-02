using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006C RID: 108
	public class MPLobbyClanItemVM : ViewModel
	{
		// Token: 0x06000A80 RID: 2688 RVA: 0x000206B8 File Offset: 0x0001E8B8
		public MPLobbyClanItemVM(string name, string tag, string sigilCode, int gamesWon, int gamesLost, int ranking, bool isOwnClan)
		{
			this._name = name;
			this._tag = tag;
			this._sigilCode = sigilCode;
			this.GamesWon = gamesWon;
			this.GamesLost = gamesLost;
			this.Ranking = ranking;
			this.IsOwnClan = isOwnClan;
			this.RefreshValues();
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00020708 File Offset: 0x0001E908
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SigilImage = new BannerImageIdentifierVM(new Banner(this._sigilCode), false);
			GameTexts.SetVariable("STR", this._tag);
			string text = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
			GameTexts.SetVariable("STR1", this._name);
			GameTexts.SetVariable("STR2", text);
			this.NameWithTag = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00020784 File Offset: 0x0001E984
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x0002078C File Offset: 0x0001E98C
		[DataSourceProperty]
		public string NameWithTag
		{
			get
			{
				return this._nameWithTag;
			}
			set
			{
				if (value != this._nameWithTag)
				{
					this._nameWithTag = value;
					base.OnPropertyChangedWithValue<string>(value, "NameWithTag");
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x000207AF File Offset: 0x0001E9AF
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x000207B7 File Offset: 0x0001E9B7
		[DataSourceProperty]
		public int MemberCount
		{
			get
			{
				return this._memberCount;
			}
			set
			{
				if (value != this._memberCount)
				{
					this._memberCount = value;
					base.OnPropertyChangedWithValue(value, "MemberCount");
				}
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x000207D5 File Offset: 0x0001E9D5
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x000207DD File Offset: 0x0001E9DD
		[DataSourceProperty]
		public int GamesWon
		{
			get
			{
				return this._gamesWon;
			}
			set
			{
				if (value != this._gamesWon)
				{
					this._gamesWon = value;
					base.OnPropertyChangedWithValue(value, "GamesWon");
				}
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x000207FB File Offset: 0x0001E9FB
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x00020803 File Offset: 0x0001EA03
		[DataSourceProperty]
		public int GamesLost
		{
			get
			{
				return this._gamesLost;
			}
			set
			{
				if (value != this._gamesLost)
				{
					this._gamesLost = value;
					base.OnPropertyChangedWithValue(value, "GamesLost");
				}
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00020821 File Offset: 0x0001EA21
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x00020829 File Offset: 0x0001EA29
		[DataSourceProperty]
		public int Ranking
		{
			get
			{
				return this._ranking;
			}
			set
			{
				if (value != this._ranking)
				{
					this._ranking = value;
					base.OnPropertyChangedWithValue(value, "Ranking");
				}
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x00020847 File Offset: 0x0001EA47
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x0002084F File Offset: 0x0001EA4F
		[DataSourceProperty]
		public bool IsOwnClan
		{
			get
			{
				return this._isOwnClan;
			}
			set
			{
				if (value != this._isOwnClan)
				{
					this._isOwnClan = value;
					base.OnPropertyChangedWithValue(value, "IsOwnClan");
				}
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002086D File Offset: 0x0001EA6D
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x00020875 File Offset: 0x0001EA75
		[DataSourceProperty]
		public BannerImageIdentifierVM SigilImage
		{
			get
			{
				return this._sigilImage;
			}
			set
			{
				if (value != this._sigilImage)
				{
					this._sigilImage = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "SigilImage");
				}
			}
		}

		// Token: 0x040004C7 RID: 1223
		private string _name;

		// Token: 0x040004C8 RID: 1224
		private string _tag;

		// Token: 0x040004C9 RID: 1225
		private string _sigilCode;

		// Token: 0x040004CA RID: 1226
		private string _nameWithTag;

		// Token: 0x040004CB RID: 1227
		private int _memberCount;

		// Token: 0x040004CC RID: 1228
		private int _gamesWon;

		// Token: 0x040004CD RID: 1229
		private int _gamesLost;

		// Token: 0x040004CE RID: 1230
		private int _ranking;

		// Token: 0x040004CF RID: 1231
		private bool _isOwnClan;

		// Token: 0x040004D0 RID: 1232
		private BannerImageIdentifierVM _sigilImage;
	}
}
