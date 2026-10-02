using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x0200004C RID: 76
	public class MPAnnouncementItemVM : ViewModel
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060006A4 RID: 1700 RVA: 0x00015908 File Offset: 0x00013B08
		// (remove) Token: 0x060006A5 RID: 1701 RVA: 0x0001593C File Offset: 0x00013B3C
		public static event Action<MPAnnouncementItemVM> OnInspect;

		// Token: 0x060006A6 RID: 1702 RVA: 0x00015970 File Offset: 0x00013B70
		public MPAnnouncementItemVM(PublishedLobbyNewsArticle announcement)
		{
			this._announcement = announcement;
			this.IsPinned = announcement.Pinned;
			this.Type = announcement.Type;
			this.TypeName = this.GetAnnouncementTypeName(this.Type);
			this.Title = this.ParseMarkup(announcement.Title);
			this.Description = this.ParseMarkup(announcement.Description);
			this.UpdateDateText();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000159DE File Offset: 0x00013BDE
		public void ExecuteInspect()
		{
			Action<MPAnnouncementItemVM> onInspect = MPAnnouncementItemVM.OnInspect;
			if (onInspect == null)
			{
				return;
			}
			onInspect(this);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000159F0 File Offset: 0x00013BF0
		private string ParseMarkup(string markupText)
		{
			return markupText;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000159F4 File Offset: 0x00013BF4
		private void UpdateDateText()
		{
			this.IsSingleDate = string.IsNullOrEmpty(this._announcement.DateStart) || string.IsNullOrEmpty(this._announcement.DateEnd) || this._announcement.DateStart == this._announcement.DateEnd;
			string text = string.Empty;
			if (this.IsSingleDate)
			{
				string text2 = this._announcement.DateStart;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = this._announcement.DateEnd;
				}
				text = text2;
			}
			else
			{
				TextObject textObject = GameTexts.FindText("str_LEFT_dash_RIGHT", null);
				textObject.SetTextVariable("LEFT", this._announcement.DateStart);
				textObject.SetTextVariable("RIGHT", this._announcement.DateEnd);
				text = textObject.ToString();
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Replace('\\', '.');
				text = text.Replace('/', '.');
			}
			this.DateText = text;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00015ADF File Offset: 0x00013CDF
		private string GetAnnouncementTypeName(int announcementType)
		{
			if (announcementType == 1)
			{
				return "Event";
			}
			if (announcementType != 2)
			{
				return string.Empty;
			}
			return "Announcement";
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00015AFC File Offset: 0x00013CFC
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x00015B04 File Offset: 0x00013D04
		[DataSourceProperty]
		public bool IsSingleDate
		{
			get
			{
				return this._isSingleDate;
			}
			set
			{
				if (value != this._isSingleDate)
				{
					this._isSingleDate = value;
					base.OnPropertyChangedWithValue(value, "IsSingleDate");
				}
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00015B22 File Offset: 0x00013D22
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x00015B2A File Offset: 0x00013D2A
		[DataSourceProperty]
		public bool IsPinned
		{
			get
			{
				return this._isPinned;
			}
			set
			{
				if (value != this._isPinned)
				{
					this._isPinned = value;
					base.OnPropertyChangedWithValue(value, "IsPinned");
				}
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00015B48 File Offset: 0x00013D48
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x00015B50 File Offset: 0x00013D50
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00015B6E File Offset: 0x00013D6E
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x00015B76 File Offset: 0x00013D76
		[DataSourceProperty]
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
			set
			{
				if (value != this._typeName)
				{
					this._typeName = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeName");
				}
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00015B99 File Offset: 0x00013D99
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x00015BA1 File Offset: 0x00013DA1
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00015BC4 File Offset: 0x00013DC4
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x00015BCC File Offset: 0x00013DCC
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00015BEF File Offset: 0x00013DEF
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x00015BF7 File Offset: 0x00013DF7
		[DataSourceProperty]
		public string DateText
		{
			get
			{
				return this._dateText;
			}
			set
			{
				if (value != this._dateText)
				{
					this._dateText = value;
					base.OnPropertyChangedWithValue<string>(value, "DateText");
				}
			}
		}

		// Token: 0x04000320 RID: 800
		private readonly PublishedLobbyNewsArticle _announcement;

		// Token: 0x04000321 RID: 801
		private bool _isSingleDate;

		// Token: 0x04000322 RID: 802
		private bool _isPinned;

		// Token: 0x04000323 RID: 803
		private int _type;

		// Token: 0x04000324 RID: 804
		private string _typeName;

		// Token: 0x04000325 RID: 805
		private string _title;

		// Token: 0x04000326 RID: 806
		private string _description;

		// Token: 0x04000327 RID: 807
		private string _dateText;
	}
}
