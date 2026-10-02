using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x0200013D RID: 317
	public class ClanMembersVM : ViewModel
	{
		// Token: 0x06001DBE RID: 7614 RVA: 0x0006DDB4 File Offset: 0x0006BFB4
		public ClanMembersVM(Action onRefresh, Action<Hero> showHeroOnMap)
		{
			this._onRefresh = onRefresh;
			this._faction = Hero.MainHero.Clan;
			this._showHeroOnMap = showHeroOnMap;
			this._teleportationBehavior = Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>();
			this.Family = new MBBindingList<ClanLordItemVM>();
			this.Companions = new MBBindingList<ClanLordItemVM>();
			MBBindingList<MBBindingList<ClanLordItemVM>> mbbindingList = new MBBindingList<MBBindingList<ClanLordItemVM>> { this.Family, this.Companions };
			this.SortController = new ClanMembersSortControllerVM(mbbindingList);
			this.RefreshMembersList();
			this.RefreshValues();
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0006DE44 File Offset: 0x0006C044
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TraitsText = GameTexts.FindText("str_traits_group", null).ToString();
			this.SkillsText = GameTexts.FindText("str_skills", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.Family.ApplyActionOnAllItems(delegate(ClanLordItemVM x)
			{
				x.RefreshValues();
			});
			this.Companions.ApplyActionOnAllItems(delegate(ClanLordItemVM x)
			{
				x.RefreshValues();
			});
			this.SortController.RefreshValues();
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0006DF10 File Offset: 0x0006C110
		public void RefreshMembersList()
		{
			this.Family.Clear();
			this.Companions.Clear();
			this.SortController.ResetAllStates();
			List<Hero> list = new List<Hero>();
			foreach (Hero hero in this._faction.AliveLords)
			{
				if (!hero.IsDisabled)
				{
					if (hero == Hero.MainHero)
					{
						list.Insert(0, hero);
					}
					else
					{
						list.Add(hero);
					}
				}
			}
			IEnumerable<Hero> enumerable = this._faction.Companions.Where<Hero>((Hero m) => m.IsPlayerCompanion);
			foreach (Hero hero2 in list)
			{
				this.Family.Add(new ClanLordItemVM(hero2, this._teleportationBehavior, this._showHeroOnMap, new Action<ClanLordItemVM>(this.OnMemberSelection), new Action(this.OnRequestRecall), new Action(this.OnTalkWithMember)));
			}
			foreach (Hero hero3 in enumerable)
			{
				this.Companions.Add(new ClanLordItemVM(hero3, this._teleportationBehavior, this._showHeroOnMap, new Action<ClanLordItemVM>(this.OnMemberSelection), new Action(this.OnRequestRecall), new Action(this.OnTalkWithMember)));
			}
			GameTexts.SetVariable("RANK", GameTexts.FindText("str_family_group", null));
			GameTexts.SetVariable("NUMBER", this.Family.Count);
			this.FamilyText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_companions_group", null));
			GameTexts.SetVariable("LEFT", this._faction.Companions.Count);
			GameTexts.SetVariable("RIGHT", this._faction.CompanionLimit);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null));
			this.CompanionsText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.OnMemberSelection(this.GetDefaultMember());
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0006E188 File Offset: 0x0006C388
		private ClanLordItemVM GetDefaultMember()
		{
			if (this.Family.Count > 0)
			{
				return this.Family[0];
			}
			if (this.Companions.Count <= 0)
			{
				return null;
			}
			return this.Companions[0];
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0006E1C4 File Offset: 0x0006C3C4
		public void SelectMember(Hero hero)
		{
			bool flag = false;
			foreach (ClanLordItemVM clanLordItemVM in this.Family)
			{
				if (clanLordItemVM.GetHero() == hero)
				{
					this.OnMemberSelection(clanLordItemVM);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (ClanLordItemVM clanLordItemVM2 in this.Companions)
				{
					if (clanLordItemVM2.GetHero() == hero)
					{
						this.OnMemberSelection(clanLordItemVM2);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				foreach (ClanLordItemVM clanLordItemVM3 in this.Family)
				{
					if (clanLordItemVM3.GetHero() == Hero.MainHero)
					{
						this.OnMemberSelection(clanLordItemVM3);
						flag = true;
						break;
					}
				}
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0006E2C0 File Offset: 0x0006C4C0
		private void OnMemberSelection(ClanLordItemVM member)
		{
			if (this.CurrentSelectedMember != null)
			{
				this.CurrentSelectedMember.IsSelected = false;
			}
			this.CurrentSelectedMember = member;
			if (member != null)
			{
				member.IsSelected = true;
			}
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0006E2E8 File Offset: 0x0006C4E8
		private void OnRequestRecall()
		{
			ClanLordItemVM currentSelectedMember = this.CurrentSelectedMember;
			Hero hero = ((currentSelectedMember != null) ? currentSelectedMember.GetHero() : null);
			if (hero != null)
			{
				int num = (int)Math.Ceiling((double)Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, PartyBase.MainParty).ResultNumber);
				MBTextManager.SetTextVariable("TRAVEL_DURATION", CampaignUIHelper.GetHoursAndDaysTextFromHourValue(num).ToString(), false);
				MBTextManager.SetTextVariable("HERO_NAME", hero.Name.ToString(), false);
				object obj = GameTexts.FindText("str_recall_member", null);
				TextObject textObject = GameTexts.FindText("str_recall_clan_member_inquiry", null);
				InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.OnConfirmRecall), null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0006E3CE File Offset: 0x0006C5CE
		private void OnConfirmRecall()
		{
			TeleportHeroAction.ApplyDelayedTeleportToParty(this.CurrentSelectedMember.GetHero(), MobileParty.MainParty);
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0006E3F5 File Offset: 0x0006C5F5
		private void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0006E408 File Offset: 0x0006C608
		private void OnTalkWithMember()
		{
			ClanLordItemVM currentSelectedMember = this.CurrentSelectedMember;
			bool flag;
			if (currentSelectedMember == null)
			{
				flag = null != null;
			}
			else
			{
				Hero hero = currentSelectedMember.GetHero();
				flag = ((hero != null) ? hero.CharacterObject : null) != null;
			}
			if (flag)
			{
				CharacterObject characterObject = this.CurrentSelectedMember.GetHero().CharacterObject;
				LocationComplex locationComplex = LocationComplex.Current;
				if (((locationComplex != null) ? locationComplex.GetLocationOfCharacter(LocationComplex.Current.GetFirstLocationCharacterOfCharacter(characterObject)) : null) == null)
				{
					CampaignMission.OpenConversationMission(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(characterObject, PartyBase.MainParty, false, false, false, false, false, false), "", "", false);
					return;
				}
				Game.Current.GameStateManager.PopState(0);
				CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(characterObject, PartyBase.MainParty, false, false, false, false, false, false));
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0006E4DC File Offset: 0x0006C6DC
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Family.ApplyActionOnAllItems(delegate(ClanLordItemVM f)
			{
				f.OnFinalize();
			});
			this.Companions.ApplyActionOnAllItems(delegate(ClanLordItemVM f)
			{
				f.OnFinalize();
			});
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x0006E543 File Offset: 0x0006C743
		// (set) Token: 0x06001DCA RID: 7626 RVA: 0x0006E54B File Offset: 0x0006C74B
		[DataSourceProperty]
		public bool IsAnyValidMemberSelected
		{
			get
			{
				return this._isAnyValidMemberSelected;
			}
			set
			{
				if (value != this._isAnyValidMemberSelected)
				{
					this._isAnyValidMemberSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidMemberSelected");
				}
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0006E569 File Offset: 0x0006C769
		// (set) Token: 0x06001DCC RID: 7628 RVA: 0x0006E571 File Offset: 0x0006C771
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
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0006E58F File Offset: 0x0006C78F
		// (set) Token: 0x06001DCE RID: 7630 RVA: 0x0006E597 File Offset: 0x0006C797
		[DataSourceProperty]
		public string FamilyText
		{
			get
			{
				return this._familyText;
			}
			set
			{
				if (value != this._familyText)
				{
					this._familyText = value;
					base.OnPropertyChangedWithValue<string>(value, "FamilyText");
				}
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06001DCF RID: 7631 RVA: 0x0006E5BA File Offset: 0x0006C7BA
		// (set) Token: 0x06001DD0 RID: 7632 RVA: 0x0006E5C2 File Offset: 0x0006C7C2
		[DataSourceProperty]
		public string TraitsText
		{
			get
			{
				return this._traitsText;
			}
			set
			{
				if (value != this._traitsText)
				{
					this._traitsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TraitsText");
				}
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x0006E5E5 File Offset: 0x0006C7E5
		// (set) Token: 0x06001DD2 RID: 7634 RVA: 0x0006E5ED File Offset: 0x0006C7ED
		[DataSourceProperty]
		public string SkillsText
		{
			get
			{
				return this._skillsText;
			}
			set
			{
				if (value != this._skillsText)
				{
					this._skillsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillsText");
				}
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x0006E610 File Offset: 0x0006C810
		// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x0006E618 File Offset: 0x0006C818
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
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x0006E63B File Offset: 0x0006C83B
		// (set) Token: 0x06001DD6 RID: 7638 RVA: 0x0006E643 File Offset: 0x0006C843
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x0006E666 File Offset: 0x0006C866
		// (set) Token: 0x06001DD8 RID: 7640 RVA: 0x0006E66E File Offset: 0x0006C86E
		[DataSourceProperty]
		public string CompanionsText
		{
			get
			{
				return this._companionsText;
			}
			set
			{
				if (value != this._companionsText)
				{
					this._companionsText = value;
					base.OnPropertyChangedWithValue<string>(value, "CompanionsText");
				}
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x0006E691 File Offset: 0x0006C891
		// (set) Token: 0x06001DDA RID: 7642 RVA: 0x0006E699 File Offset: 0x0006C899
		[DataSourceProperty]
		public MBBindingList<ClanLordItemVM> Companions
		{
			get
			{
				return this._companions;
			}
			set
			{
				if (value != this._companions)
				{
					this._companions = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanLordItemVM>>(value, "Companions");
				}
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06001DDB RID: 7643 RVA: 0x0006E6B7 File Offset: 0x0006C8B7
		// (set) Token: 0x06001DDC RID: 7644 RVA: 0x0006E6BF File Offset: 0x0006C8BF
		[DataSourceProperty]
		public MBBindingList<ClanLordItemVM> Family
		{
			get
			{
				return this._family;
			}
			set
			{
				if (value != this._family)
				{
					this._family = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanLordItemVM>>(value, "Family");
				}
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06001DDD RID: 7645 RVA: 0x0006E6DD File Offset: 0x0006C8DD
		// (set) Token: 0x06001DDE RID: 7646 RVA: 0x0006E6E5 File Offset: 0x0006C8E5
		[DataSourceProperty]
		public ClanLordItemVM CurrentSelectedMember
		{
			get
			{
				return this._currentSelectedMember;
			}
			set
			{
				if (value != this._currentSelectedMember)
				{
					this._currentSelectedMember = value;
					base.OnPropertyChangedWithValue<ClanLordItemVM>(value, "CurrentSelectedMember");
					this.IsAnyValidMemberSelected = value != null;
				}
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x0006E70D File Offset: 0x0006C90D
		// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x0006E715 File Offset: 0x0006C915
		[DataSourceProperty]
		public ClanMembersSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<ClanMembersSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000DE4 RID: 3556
		private readonly Clan _faction;

		// Token: 0x04000DE5 RID: 3557
		private readonly Action _onRefresh;

		// Token: 0x04000DE6 RID: 3558
		private readonly Action<Hero> _showHeroOnMap;

		// Token: 0x04000DE7 RID: 3559
		private readonly ITeleportationCampaignBehavior _teleportationBehavior;

		// Token: 0x04000DE8 RID: 3560
		private bool _isSelected;

		// Token: 0x04000DE9 RID: 3561
		private MBBindingList<ClanLordItemVM> _companions;

		// Token: 0x04000DEA RID: 3562
		private MBBindingList<ClanLordItemVM> _family;

		// Token: 0x04000DEB RID: 3563
		private ClanLordItemVM _currentSelectedMember;

		// Token: 0x04000DEC RID: 3564
		private string _familyText;

		// Token: 0x04000DED RID: 3565
		private string _traitsText;

		// Token: 0x04000DEE RID: 3566
		private string _companionsText;

		// Token: 0x04000DEF RID: 3567
		private string _skillsText;

		// Token: 0x04000DF0 RID: 3568
		private string _nameText;

		// Token: 0x04000DF1 RID: 3569
		private string _locationText;

		// Token: 0x04000DF2 RID: 3570
		private bool _isAnyValidMemberSelected;

		// Token: 0x04000DF3 RID: 3571
		private ClanMembersSortControllerVM _sortController;
	}
}
