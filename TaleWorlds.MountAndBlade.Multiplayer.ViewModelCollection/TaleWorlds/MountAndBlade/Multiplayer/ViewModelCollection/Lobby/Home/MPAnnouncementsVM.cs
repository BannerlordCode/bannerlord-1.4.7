using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Home
{
	// Token: 0x0200004D RID: 77
	public class MPAnnouncementsVM : ViewModel
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x00015C1A File Offset: 0x00013E1A
		public MPAnnouncementsVM(float? announcementUpdateIntervalInSeconds)
		{
			this._updateTimer = null;
			this._announcementUpdateIntervalInSeconds = announcementUpdateIntervalInSeconds;
			this.AnnouncementList = new MBBindingList<MPAnnouncementItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00015C46 File Offset: 0x00013E46
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=lQ0T2pbY}Events & Announcements", null).ToString();
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00015C64 File Offset: 0x00013E64
		public void OnTick(float dt)
		{
			if (!NetworkMain.GameClient.AtLobby)
			{
				this._updateTimer = null;
				return;
			}
			if (this._announcementUpdateIntervalInSeconds != null)
			{
				float? num = this._announcementUpdateIntervalInSeconds;
				float num2 = 0f;
				if (((num.GetValueOrDefault() > num2) & (num != null)) && !this._isRefreshingAnnouncements)
				{
					if (this._updateTimer != null)
					{
						num = this._updateTimer;
						float? announcementUpdateIntervalInSeconds = this._announcementUpdateIntervalInSeconds;
						if (!((num.GetValueOrDefault() > announcementUpdateIntervalInSeconds.GetValueOrDefault()) & ((num != null) & (announcementUpdateIntervalInSeconds != null))))
						{
							this._updateTimer += dt;
							return;
						}
					}
					this.RefreshAnnouncements();
					this._updateTimer = new float?(0f);
					return;
				}
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00015D4E File Offset: 0x00013F4E
		private void RefreshAnnouncements()
		{
			this._isRefreshingAnnouncements = true;
			this.UpdateAnnouncements();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00015D60 File Offset: 0x00013F60
		public async void UpdateAnnouncements()
		{
			PublishedLobbyNewsArticle[] array = await NetworkMain.GameClient.GetLobbyNews();
			this.AnnouncementList.Clear();
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					MPAnnouncementItemVM mpannouncementItemVM = new MPAnnouncementItemVM(array[i]);
					this.AnnouncementList.Add(mpannouncementItemVM);
				}
			}
			this.HasValidAnnouncements = this.AnnouncementList.Count > 0 && ApplicationPlatform.IsPlatformWindows() && ApplicationPlatform.CurrentPlatform != Platform.GDKDesktop;
			this._isRefreshingAnnouncements = false;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00015D99 File Offset: 0x00013F99
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x00015DA1 File Offset: 0x00013FA1
		[DataSourceProperty]
		public bool HasValidAnnouncements
		{
			get
			{
				return this._hasValidAnnouncements;
			}
			set
			{
				if (value != this._hasValidAnnouncements)
				{
					this._hasValidAnnouncements = value;
					base.OnPropertyChangedWithValue(value, "HasValidAnnouncements");
				}
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00015DBF File Offset: 0x00013FBF
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x00015DC7 File Offset: 0x00013FC7
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00015DEA File Offset: 0x00013FEA
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x00015DF2 File Offset: 0x00013FF2
		[DataSourceProperty]
		public MBBindingList<MPAnnouncementItemVM> AnnouncementList
		{
			get
			{
				return this._announcementList;
			}
			set
			{
				if (value != this._announcementList)
				{
					this._announcementList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPAnnouncementItemVM>>(value, "AnnouncementList");
				}
			}
		}

		// Token: 0x04000328 RID: 808
		private readonly float? _announcementUpdateIntervalInSeconds;

		// Token: 0x04000329 RID: 809
		private float? _updateTimer;

		// Token: 0x0400032A RID: 810
		private bool _isRefreshingAnnouncements;

		// Token: 0x0400032B RID: 811
		private bool _hasValidAnnouncements;

		// Token: 0x0400032C RID: 812
		private string _titleText;

		// Token: 0x0400032D RID: 813
		private MBBindingList<MPAnnouncementItemVM> _announcementList;
	}
}
