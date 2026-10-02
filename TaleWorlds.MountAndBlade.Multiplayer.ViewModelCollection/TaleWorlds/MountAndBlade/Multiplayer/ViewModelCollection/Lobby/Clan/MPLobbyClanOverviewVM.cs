using System;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000071 RID: 113
	public class MPLobbyClanOverviewVM : ViewModel
	{
		// Token: 0x06000AF1 RID: 2801 RVA: 0x00021558 File Offset: 0x0001F758
		public MPLobbyClanOverviewVM(Action openInviteClanMemberPopup)
		{
			this._openInviteClanMemberPopup = openInviteClanMemberPopup;
			this.AnnouncementsList = new MBBindingList<MPLobbyClanAnnouncementVM>();
			this.ChangeSigilPopup = new MPLobbyClanChangeSigilPopupVM();
			this.ChangeFactionPopup = new MPLobbyClanChangeFactionPopupVM();
			this.SendAnnouncementPopup = new MPLobbyClanSendPostPopupVM(MPLobbyClanSendPostPopupVM.PostPopupMode.Announcement);
			this.SetClanInformationPopup = new MPLobbyClanSendPostPopupVM(MPLobbyClanSendPostPopupVM.PostPopupMode.Information);
			this.AreActionButtonsEnabled = true;
			this.RefreshValues();
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000215B8 File Offset: 0x0001F7B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ChangeSigilText = new TextObject("{=7R0i82Nw}Change Sigil", null).ToString();
			this.ChangeFactionText = new TextObject("{=aGGq9lJT}Change Culture", null).ToString();
			this.LeaveText = new TextObject("{=3sRdGQou}Leave", null).ToString();
			this.DisbandText = new TextObject("{=xXSFaGW8}Disband", null).ToString();
			this.InformationText = new TextObject("{=SyklU5aP}Information", null).ToString();
			this.AnnouncementsText = new TextObject("{=JY2pBVHQ}Announcements", null).ToString();
			this.NoAnnouncementsText = new TextObject("{=0af2iQvw}Clan doesn't have any announcements", null).ToString();
			this.NoDescriptionText = new TextObject("{=NwiYsUwm}Clan doesn't have a description", null).ToString();
			this.TitleText = new TextObject("{=r223yChR}Overview", null).ToString();
			this.CantLeaveHint = new HintViewModel(new TextObject("{=76HlhP7r}You have to give leadership to another member to leave", null), null);
			this.InviteMembersHint = new HintViewModel(new TextObject("{=tSMckUw3}Invite Members", null), null);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x000216C0 File Offset: 0x0001F8C0
		public async Task RefreshClanInformation(ClanHomeInfo info)
		{
			if (info == null || info.ClanInfo == null)
			{
				this.CloseAllPopups();
			}
			else
			{
				ClanInfo clanInfo = info.ClanInfo;
				GameTexts.SetVariable("STR", clanInfo.Tag);
				string clanTagInBrackets = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
				string text = await PlatformServices.FilterString(clanInfo.Name, new TextObject("{=wNUcqcJP}Clan Name", null).ToString());
				GameTexts.SetVariable("STR1", text);
				GameTexts.SetVariable("STR2", clanTagInBrackets);
				this.NameText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
				GameTexts.SetVariable("LEFT", new TextObject("{=lBn2pSBL}Members", null).ToString());
				GameTexts.SetVariable("RIGHT", clanInfo.Players.Length);
				this.MembersText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
				this.SigilImage = new BannerImageIdentifierVM(new Banner(clanInfo.Sigil), true);
				this.FactionCultureID = clanInfo.Faction;
				BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(this.FactionCultureID);
				this.CultureColor1 = Color.FromUint((@object != null) ? @object.Color : 0U);
				this.CultureColor2 = Color.FromUint((@object != null) ? @object.Color2 : 0U);
				if (NetworkMain.GameClient != null)
				{
					this.IsLeader = NetworkMain.GameClient.IsClanLeader;
					this.IsPrivilegedMember = this.IsLeader || NetworkMain.GameClient.IsClanOfficer;
				}
				else
				{
					Debug.FailedAssert("Game client is destroyed while updating clan home info", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Clan\\MPLobbyClanOverviewVM.cs", "RefreshClanInformation", 89);
					Debug.Print("Game client is destroyed while updating clan home info", 0, Debug.DebugColor.White, 17592186044416UL);
					this.IsLeader = false;
					this.IsPrivilegedMember = false;
				}
				this.FactionBanner = new BannerImageIdentifierVM(@object.Banner, true);
				this.ClanDescriptionText = clanInfo.InformationText;
				this.DoesHaveDescription = true;
				if (string.IsNullOrEmpty(clanInfo.InformationText))
				{
					this.DoesHaveDescription = false;
				}
				this.AnnouncementsList.Clear();
				ClanAnnouncement[] announcements = clanInfo.Announcements;
				foreach (ClanAnnouncement clanAnnouncement in announcements)
				{
					this.AnnouncementsList.Add(new MPLobbyClanAnnouncementVM(clanAnnouncement.AuthorId, clanAnnouncement.Announcement, clanAnnouncement.CreationTime, clanAnnouncement.Id, this.IsPrivilegedMember));
				}
				this.DoesHaveAnnouncements = true;
				if (announcements.IsEmpty<ClanAnnouncement>())
				{
					this.DoesHaveAnnouncements = false;
				}
				clanInfo = null;
				clanTagInBrackets = null;
			}
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00021710 File Offset: 0x0001F910
		private void ExecuteDisbandClan()
		{
			string text = new TextObject("{=oFWcihyW}Disband Clan", null).ToString();
			string text2 = new TextObject("{=vW1VgmaP}Are you sure want to disband your clan?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.DisbandClan), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00021787 File Offset: 0x0001F987
		private void DisbandClan()
		{
			NetworkMain.GameClient.DestroyClan();
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00021794 File Offset: 0x0001F994
		private void ExecuteLeaveClan()
		{
			string text = new TextObject("{=4ZE6i9nW}Leave Clan", null).ToString();
			string text2 = new TextObject("{=67hsZZor}Are you sure want to leave your clan?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.LeaveClan), null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002180B File Offset: 0x0001FA0B
		private void LeaveClan()
		{
			NetworkMain.GameClient.KickFromClan(NetworkMain.GameClient.PlayerID);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00021821 File Offset: 0x0001FA21
		private void ExecuteOpenChangeSigilPopup()
		{
			this.ChangeSigilPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002182E File Offset: 0x0001FA2E
		private void ExecuteCloseChangeSigilPopup()
		{
			this.ChangeSigilPopup.ExecuteClosePopup();
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002183B File Offset: 0x0001FA3B
		private void ExecuteOpenChangeFactionPopup()
		{
			this.ChangeFactionPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00021848 File Offset: 0x0001FA48
		private void ExecuteCloseChangeFactionPopup()
		{
			this.ChangeFactionPopup.ExecuteClosePopup();
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00021855 File Offset: 0x0001FA55
		private void ExecuteOpenSendAnnouncementPopup()
		{
			this.SendAnnouncementPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00021862 File Offset: 0x0001FA62
		private void ExecuteCloseSendAnnouncementPopup()
		{
			this.SendAnnouncementPopup.ExecuteClosePopup();
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002186F File Offset: 0x0001FA6F
		private void ExecuteOpenSetClanInformationPopup()
		{
			this.SetClanInformationPopup.ExecuteOpenPopup();
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002187C File Offset: 0x0001FA7C
		private void ExecuteCloseSetClanInformationPopup()
		{
			this.SetClanInformationPopup.ExecuteClosePopup();
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00021889 File Offset: 0x0001FA89
		private void ExecuteOpenInviteClanMemberPopup()
		{
			Action openInviteClanMemberPopup = this._openInviteClanMemberPopup;
			if (openInviteClanMemberPopup == null)
			{
				return;
			}
			openInviteClanMemberPopup();
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002189B File Offset: 0x0001FA9B
		private void CloseAllPopups()
		{
			this.ChangeSigilPopup.ExecuteClosePopup();
			this.ChangeFactionPopup.ExecuteClosePopup();
			this.SendAnnouncementPopup.ExecuteClosePopup();
			this.SetClanInformationPopup.ExecuteClosePopup();
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x000218C9 File Offset: 0x0001FAC9
		// (set) Token: 0x06000B03 RID: 2819 RVA: 0x000218D1 File Offset: 0x0001FAD1
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x000218EE File Offset: 0x0001FAEE
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x000218F6 File Offset: 0x0001FAF6
		[DataSourceProperty]
		public bool IsLeader
		{
			get
			{
				return this._isLeader;
			}
			set
			{
				if (value != this._isLeader)
				{
					this._isLeader = value;
					base.OnPropertyChanged("IsLeader");
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00021913 File Offset: 0x0001FB13
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x0002191B File Offset: 0x0001FB1B
		[DataSourceProperty]
		public bool IsPrivilegedMember
		{
			get
			{
				return this._isPrivilegedMember;
			}
			set
			{
				if (value != this._isPrivilegedMember)
				{
					this._isPrivilegedMember = value;
					base.OnPropertyChanged("IsPrivilegedMember");
				}
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00021938 File Offset: 0x0001FB38
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x00021940 File Offset: 0x0001FB40
		[DataSourceProperty]
		public bool AreActionButtonsEnabled
		{
			get
			{
				return this._areActionButtonsEnabled;
			}
			set
			{
				if (value != this._areActionButtonsEnabled)
				{
					this._areActionButtonsEnabled = value;
					base.OnPropertyChanged("AreActionButtonsEnabled");
				}
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0002195D File Offset: 0x0001FB5D
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x00021965 File Offset: 0x0001FB65
		[DataSourceProperty]
		public bool DoesHaveDescription
		{
			get
			{
				return this._doesHaveDescription;
			}
			set
			{
				if (value != this._doesHaveDescription)
				{
					this._doesHaveDescription = value;
					base.OnPropertyChanged("DoesHaveDescription");
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00021982 File Offset: 0x0001FB82
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x0002198A File Offset: 0x0001FB8A
		[DataSourceProperty]
		public bool DoesHaveAnnouncements
		{
			get
			{
				return this._doesHaveAnnouncements;
			}
			set
			{
				if (value != this._doesHaveAnnouncements)
				{
					this._doesHaveAnnouncements = value;
					base.OnPropertyChanged("DoesHaveAnnouncements");
				}
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x000219A7 File Offset: 0x0001FBA7
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x000219AF File Offset: 0x0001FBAF
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChanged("NameText");
				}
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x000219D1 File Offset: 0x0001FBD1
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x000219D9 File Offset: 0x0001FBD9
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChanged("MembersText");
				}
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x000219FB File Offset: 0x0001FBFB
		// (set) Token: 0x06000B13 RID: 2835 RVA: 0x00021A03 File Offset: 0x0001FC03
		[DataSourceProperty]
		public string ChangeSigilText
		{
			get
			{
				return this._changeSigilText;
			}
			set
			{
				if (value != this._changeSigilText)
				{
					this._changeSigilText = value;
					base.OnPropertyChanged("ChangeSigilText");
				}
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00021A25 File Offset: 0x0001FC25
		// (set) Token: 0x06000B15 RID: 2837 RVA: 0x00021A2D File Offset: 0x0001FC2D
		[DataSourceProperty]
		public string ChangeFactionText
		{
			get
			{
				return this._changeFactionText;
			}
			set
			{
				if (value != this._changeFactionText)
				{
					this._changeFactionText = value;
					base.OnPropertyChanged("ChangeFactionText");
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00021A4F File Offset: 0x0001FC4F
		// (set) Token: 0x06000B17 RID: 2839 RVA: 0x00021A57 File Offset: 0x0001FC57
		[DataSourceProperty]
		public string LeaveText
		{
			get
			{
				return this._leaveText;
			}
			set
			{
				if (value != this._leaveText)
				{
					this._leaveText = value;
					base.OnPropertyChanged("LeaveText");
				}
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00021A79 File Offset: 0x0001FC79
		// (set) Token: 0x06000B19 RID: 2841 RVA: 0x00021A81 File Offset: 0x0001FC81
		[DataSourceProperty]
		public string DisbandText
		{
			get
			{
				return this._disbandText;
			}
			set
			{
				if (value != this._disbandText)
				{
					this._disbandText = value;
					base.OnPropertyChanged("DisbandText");
				}
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00021AA3 File Offset: 0x0001FCA3
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x00021AAB File Offset: 0x0001FCAB
		[DataSourceProperty]
		public string FactionCultureID
		{
			get
			{
				return this._factionCultureID;
			}
			set
			{
				if (value != this._factionCultureID)
				{
					this._factionCultureID = value;
					base.OnPropertyChanged("FactionCultureID");
				}
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00021ACD File Offset: 0x0001FCCD
		// (set) Token: 0x06000B1D RID: 2845 RVA: 0x00021AD5 File Offset: 0x0001FCD5
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00021AF8 File Offset: 0x0001FCF8
		// (set) Token: 0x06000B1F RID: 2847 RVA: 0x00021B00 File Offset: 0x0001FD00
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00021B23 File Offset: 0x0001FD23
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x00021B2B File Offset: 0x0001FD2B
		[DataSourceProperty]
		public string InformationText
		{
			get
			{
				return this._informationText;
			}
			set
			{
				if (value != this._informationText)
				{
					this._informationText = value;
					base.OnPropertyChanged("InformationText");
				}
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00021B4D File Offset: 0x0001FD4D
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x00021B55 File Offset: 0x0001FD55
		[DataSourceProperty]
		public string AnnouncementsText
		{
			get
			{
				return this._announcementsText;
			}
			set
			{
				if (value != this._announcementsText)
				{
					this._announcementsText = value;
					base.OnPropertyChanged("AnnouncementsText");
				}
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00021B77 File Offset: 0x0001FD77
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x00021B7F File Offset: 0x0001FD7F
		[DataSourceProperty]
		public string ClanDescriptionText
		{
			get
			{
				return this._clanDescriptionText;
			}
			set
			{
				if (value != this._clanDescriptionText)
				{
					this._clanDescriptionText = value;
					base.OnPropertyChanged("ClanDescriptionText");
				}
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00021BA1 File Offset: 0x0001FDA1
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x00021BA9 File Offset: 0x0001FDA9
		[DataSourceProperty]
		public string NoDescriptionText
		{
			get
			{
				return this._noDescriptionText;
			}
			set
			{
				if (value != this._noDescriptionText)
				{
					this._noDescriptionText = value;
					base.OnPropertyChanged("NoDescriptionText");
				}
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00021BCB File Offset: 0x0001FDCB
		// (set) Token: 0x06000B29 RID: 2857 RVA: 0x00021BD3 File Offset: 0x0001FDD3
		[DataSourceProperty]
		public string NoAnnouncementsText
		{
			get
			{
				return this._noAnnouncementsText;
			}
			set
			{
				if (value != this._noAnnouncementsText)
				{
					this._noAnnouncementsText = value;
					base.OnPropertyChanged("NoAnnouncementsText");
				}
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00021BF5 File Offset: 0x0001FDF5
		// (set) Token: 0x06000B2B RID: 2859 RVA: 0x00021BFD File Offset: 0x0001FDFD
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
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00021C1F File Offset: 0x0001FE1F
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x00021C27 File Offset: 0x0001FE27
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
					base.OnPropertyChanged("SigilImage");
				}
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00021C44 File Offset: 0x0001FE44
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x00021C4C File Offset: 0x0001FE4C
		[DataSourceProperty]
		public BannerImageIdentifierVM FactionBanner
		{
			get
			{
				return this._factionBanner;
			}
			set
			{
				if (value != this._factionBanner)
				{
					this._factionBanner = value;
					base.OnPropertyChanged("FactionBanner");
				}
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00021C69 File Offset: 0x0001FE69
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x00021C71 File Offset: 0x0001FE71
		[DataSourceProperty]
		public MPLobbyClanChangeSigilPopupVM ChangeSigilPopup
		{
			get
			{
				return this._changeSigilPopup;
			}
			set
			{
				if (value != this._changeSigilPopup)
				{
					this._changeSigilPopup = value;
					base.OnPropertyChanged("ChangeSigilPopup");
				}
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00021C8E File Offset: 0x0001FE8E
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x00021C96 File Offset: 0x0001FE96
		[DataSourceProperty]
		public MPLobbyClanChangeFactionPopupVM ChangeFactionPopup
		{
			get
			{
				return this._changeFactionPopup;
			}
			set
			{
				if (value != this._changeFactionPopup)
				{
					this._changeFactionPopup = value;
					base.OnPropertyChanged("ChangeFactionPopup");
				}
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00021CB3 File Offset: 0x0001FEB3
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x00021CBB File Offset: 0x0001FEBB
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanAnnouncementVM> AnnouncementsList
		{
			get
			{
				return this._announcementsList;
			}
			set
			{
				if (value != this._announcementsList)
				{
					this._announcementsList = value;
					base.OnPropertyChanged("AnnouncementsList");
				}
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00021CD8 File Offset: 0x0001FED8
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x00021CE0 File Offset: 0x0001FEE0
		[DataSourceProperty]
		public MPLobbyClanSendPostPopupVM SendAnnouncementPopup
		{
			get
			{
				return this._sendAnnouncementPopup;
			}
			set
			{
				if (value != this._sendAnnouncementPopup)
				{
					this._sendAnnouncementPopup = value;
					base.OnPropertyChanged("SendAnnouncementPopup");
				}
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00021CFD File Offset: 0x0001FEFD
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x00021D05 File Offset: 0x0001FF05
		[DataSourceProperty]
		public MPLobbyClanSendPostPopupVM SetClanInformationPopup
		{
			get
			{
				return this._setClanInformationPopup;
			}
			set
			{
				if (value != this._setClanInformationPopup)
				{
					this._setClanInformationPopup = value;
					base.OnPropertyChanged("SetClanInformationPopup");
				}
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00021D22 File Offset: 0x0001FF22
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x00021D2A File Offset: 0x0001FF2A
		[DataSourceProperty]
		public HintViewModel CantLeaveHint
		{
			get
			{
				return this._cantLeaveHint;
			}
			set
			{
				if (value != this._cantLeaveHint)
				{
					this._cantLeaveHint = value;
					base.OnPropertyChanged("CantLeaveHint");
				}
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00021D47 File Offset: 0x0001FF47
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00021D4F File Offset: 0x0001FF4F
		[DataSourceProperty]
		public HintViewModel InviteMembersHint
		{
			get
			{
				return this._inviteMembersHint;
			}
			set
			{
				if (value != this._inviteMembersHint)
				{
					this._inviteMembersHint = value;
					base.OnPropertyChanged("InviteMembersHint");
				}
			}
		}

		// Token: 0x04000500 RID: 1280
		private readonly Action _openInviteClanMemberPopup;

		// Token: 0x04000501 RID: 1281
		private bool _isSelected;

		// Token: 0x04000502 RID: 1282
		private bool _isLeader;

		// Token: 0x04000503 RID: 1283
		private bool _isPrivilegedMember;

		// Token: 0x04000504 RID: 1284
		private bool _areActionButtonsEnabled;

		// Token: 0x04000505 RID: 1285
		private bool _doesHaveDescription;

		// Token: 0x04000506 RID: 1286
		private bool _doesHaveAnnouncements;

		// Token: 0x04000507 RID: 1287
		private string _nameText;

		// Token: 0x04000508 RID: 1288
		private string _membersText;

		// Token: 0x04000509 RID: 1289
		private string _changeSigilText;

		// Token: 0x0400050A RID: 1290
		private string _changeFactionText;

		// Token: 0x0400050B RID: 1291
		private string _leaveText;

		// Token: 0x0400050C RID: 1292
		private string _disbandText;

		// Token: 0x0400050D RID: 1293
		private string _factionCultureID;

		// Token: 0x0400050E RID: 1294
		private string _informationText;

		// Token: 0x0400050F RID: 1295
		private string _announcementsText;

		// Token: 0x04000510 RID: 1296
		private string _clanDescriptionText;

		// Token: 0x04000511 RID: 1297
		private string _noDescriptionText;

		// Token: 0x04000512 RID: 1298
		private string _noAnnouncementsText;

		// Token: 0x04000513 RID: 1299
		private string _titleText;

		// Token: 0x04000514 RID: 1300
		private Color _cultureColor1;

		// Token: 0x04000515 RID: 1301
		private Color _cultureColor2;

		// Token: 0x04000516 RID: 1302
		private BannerImageIdentifierVM _sigilImage;

		// Token: 0x04000517 RID: 1303
		private BannerImageIdentifierVM _factionBanner;

		// Token: 0x04000518 RID: 1304
		private MPLobbyClanChangeSigilPopupVM _changeSigilPopup;

		// Token: 0x04000519 RID: 1305
		private MPLobbyClanChangeFactionPopupVM _changeFactionPopup;

		// Token: 0x0400051A RID: 1306
		private MBBindingList<MPLobbyClanAnnouncementVM> _announcementsList;

		// Token: 0x0400051B RID: 1307
		private MPLobbyClanSendPostPopupVM _sendAnnouncementPopup;

		// Token: 0x0400051C RID: 1308
		private MPLobbyClanSendPostPopupVM _setClanInformationPopup;

		// Token: 0x0400051D RID: 1309
		private HintViewModel _cantLeaveHint;

		// Token: 0x0400051E RID: 1310
		private HintViewModel _inviteMembersHint;
	}
}
