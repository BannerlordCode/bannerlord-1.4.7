using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup
{
	// Token: 0x0200003A RID: 58
	public class MarriageOfferPopupVM : ViewModel
	{
		// Token: 0x060005A7 RID: 1447 RVA: 0x0001E408 File Offset: 0x0001C608
		public MarriageOfferPopupVM(Hero suitor, Hero maiden, Action onClose)
		{
			this._marriageBehavior = Campaign.Current.GetCampaignBehavior<IMarriageOfferCampaignBehavior>();
			this._onClose = onClose;
			if (suitor.Clan == Clan.PlayerClan)
			{
				this.OffereeClanMember = new MarriageOfferPopupHeroVM(suitor);
				this.OffererClanMember = new MarriageOfferPopupHeroVM(maiden);
			}
			else
			{
				this.OffereeClanMember = new MarriageOfferPopupHeroVM(maiden);
				this.OffererClanMember = new MarriageOfferPopupHeroVM(suitor);
			}
			this.ConsequencesList = new MBBindingList<BindingListStringItem>();
			this.RefreshValues();
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001E482 File Offset: 0x0001C682
		public void Update()
		{
			MarriageOfferPopupHeroVM offereeClanMember = this.OffereeClanMember;
			if (offereeClanMember != null)
			{
				offereeClanMember.Update();
			}
			MarriageOfferPopupHeroVM offererClanMember = this.OffererClanMember;
			if (offererClanMember == null)
			{
				return;
			}
			offererClanMember.Update();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001E4A5 File Offset: 0x0001C6A5
		public void ExecuteAcceptOffer()
		{
			IMarriageOfferCampaignBehavior marriageBehavior = this._marriageBehavior;
			if (marriageBehavior != null)
			{
				marriageBehavior.OnMarriageOfferAcceptedOnPopUp();
			}
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
		public void ExecuteDeclineOffer()
		{
			IMarriageOfferCampaignBehavior marriageBehavior = this._marriageBehavior;
			if (marriageBehavior != null)
			{
				marriageBehavior.OnMarriageOfferDeclinedOnPopUp();
			}
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001E4EC File Offset: 0x0001C6EC
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject textObject = GameTexts.FindText("str_marriage_offer_from_clan", null);
			textObject.SetTextVariable("CLAN_NAME", this.OffererClanMember.Hero.Clan.Name);
			this.TitleText = textObject.ToString();
			this.ClanText = GameTexts.FindText("str_clan", null).ToString();
			this.AgeText = new TextObject("{=jaaQijQs}Age", null).ToString();
			this.OccupationText = new TextObject("{=GZxFIeiJ}Occupation", null).ToString();
			this.RelationText = new TextObject("{=BlidMNGT}Relation", null).ToString();
			this.ConsequencesText = new TextObject("{=Lm6Mkhru}Consequences", null).ToString();
			this.ButtonOkLabel = new TextObject("{=Y94H6XnK}Accept", null).ToString();
			this.ButtonCancelLabel = new TextObject("{=cOgmdp9e}Decline", null).ToString();
			this.ConsequencesList.Clear();
			IMarriageOfferCampaignBehavior marriageBehavior = this._marriageBehavior;
			foreach (TextObject textObject2 in (((marriageBehavior != null) ? marriageBehavior.GetMarriageAcceptedConsequences() : null) ?? new MBBindingList<TextObject>()))
			{
				this.ConsequencesList.Add(new BindingListStringItem("- " + textObject2.ToString()));
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001E64C File Offset: 0x0001C84C
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			MarriageOfferPopupHeroVM offereeClanMember = this.OffereeClanMember;
			if (offereeClanMember != null)
			{
				offereeClanMember.OnFinalize();
			}
			MarriageOfferPopupHeroVM offererClanMember = this.OffererClanMember;
			if (offererClanMember == null)
			{
				return;
			}
			offererClanMember.OnFinalize();
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001E6A2 File Offset: 0x0001C8A2
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001E6B4 File Offset: 0x0001C8B4
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x0001E6BC File Offset: 0x0001C8BC
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001E6DF File Offset: 0x0001C8DF
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0001E6E7 File Offset: 0x0001C8E7
		[DataSourceProperty]
		public string ClanText
		{
			get
			{
				return this._clanText;
			}
			set
			{
				if (value != this._clanText)
				{
					this._clanText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanText");
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0001E70A File Offset: 0x0001C90A
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0001E712 File Offset: 0x0001C912
		[DataSourceProperty]
		public string AgeText
		{
			get
			{
				return this._ageText;
			}
			set
			{
				if (value != this._ageText)
				{
					this._ageText = value;
					base.OnPropertyChangedWithValue<string>(value, "AgeText");
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001E735 File Offset: 0x0001C935
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0001E73D File Offset: 0x0001C93D
		[DataSourceProperty]
		public string OccupationText
		{
			get
			{
				return this._occupationText;
			}
			set
			{
				if (value != this._occupationText)
				{
					this._occupationText = value;
					base.OnPropertyChangedWithValue<string>(value, "OccupationText");
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001E760 File Offset: 0x0001C960
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0001E768 File Offset: 0x0001C968
		[DataSourceProperty]
		public string RelationText
		{
			get
			{
				return this._relationText;
			}
			set
			{
				if (value != this._relationText)
				{
					this._relationText = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationText");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001E78B File Offset: 0x0001C98B
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x0001E793 File Offset: 0x0001C993
		[DataSourceProperty]
		public string ConsequencesText
		{
			get
			{
				return this._consequencesText;
			}
			set
			{
				if (value != this._consequencesText)
				{
					this._consequencesText = value;
					base.OnPropertyChangedWithValue<string>(value, "ConsequencesText");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0001E7B6 File Offset: 0x0001C9B6
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x0001E7BE File Offset: 0x0001C9BE
		[DataSourceProperty]
		public MBBindingList<BindingListStringItem> ConsequencesList
		{
			get
			{
				return this._consequencesList;
			}
			set
			{
				if (value != this._consequencesList)
				{
					this._consequencesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BindingListStringItem>>(value, "ConsequencesList");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0001E7DC File Offset: 0x0001C9DC
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x0001E7E4 File Offset: 0x0001C9E4
		[DataSourceProperty]
		public string ButtonOkLabel
		{
			get
			{
				return this._buttonOkLabel;
			}
			set
			{
				if (value != this._buttonOkLabel)
				{
					this._buttonOkLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonOkLabel");
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001E807 File Offset: 0x0001CA07
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0001E80F File Offset: 0x0001CA0F
		[DataSourceProperty]
		public string ButtonCancelLabel
		{
			get
			{
				return this._buttonCancelLabel;
			}
			set
			{
				if (value != this._buttonCancelLabel)
				{
					this._buttonCancelLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ButtonCancelLabel");
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001E832 File Offset: 0x0001CA32
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0001E83A File Offset: 0x0001CA3A
		[DataSourceProperty]
		public bool IsEncyclopediaOpen
		{
			get
			{
				return this._isEncyclopediaOpen;
			}
			set
			{
				if (value != this._isEncyclopediaOpen)
				{
					this._isEncyclopediaOpen = value;
					base.OnPropertyChangedWithValue(value, "IsEncyclopediaOpen");
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001E858 File Offset: 0x0001CA58
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x0001E860 File Offset: 0x0001CA60
		[DataSourceProperty]
		public MarriageOfferPopupHeroVM OffereeClanMember
		{
			get
			{
				return this._offereeClanMember;
			}
			set
			{
				if (value != this._offereeClanMember)
				{
					this._offereeClanMember = value;
					base.OnPropertyChangedWithValue<MarriageOfferPopupHeroVM>(value, "OffereeClanMember");
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001E87E File Offset: 0x0001CA7E
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x0001E886 File Offset: 0x0001CA86
		[DataSourceProperty]
		public MarriageOfferPopupHeroVM OffererClanMember
		{
			get
			{
				return this._offererClanMember;
			}
			set
			{
				if (value != this._offererClanMember)
				{
					this._offererClanMember = value;
					base.OnPropertyChangedWithValue<MarriageOfferPopupHeroVM>(value, "OffererClanMember");
				}
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001E8A4 File Offset: 0x0001CAA4
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001E8B3 File Offset: 0x0001CAB3
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001E8C2 File Offset: 0x0001CAC2
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0001E8CA File Offset: 0x0001CACA
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001E8E8 File Offset: 0x0001CAE8
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0001E8F0 File Offset: 0x0001CAF0
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x0400026E RID: 622
		private readonly IMarriageOfferCampaignBehavior _marriageBehavior;

		// Token: 0x0400026F RID: 623
		private Action _onClose;

		// Token: 0x04000270 RID: 624
		private string _titleText;

		// Token: 0x04000271 RID: 625
		private string _clanText;

		// Token: 0x04000272 RID: 626
		private string _ageText;

		// Token: 0x04000273 RID: 627
		private string _occupationText;

		// Token: 0x04000274 RID: 628
		private string _relationText;

		// Token: 0x04000275 RID: 629
		private string _consequencesText;

		// Token: 0x04000276 RID: 630
		private MBBindingList<BindingListStringItem> _consequencesList;

		// Token: 0x04000277 RID: 631
		private string _buttonOkLabel;

		// Token: 0x04000278 RID: 632
		private string _buttonCancelLabel;

		// Token: 0x04000279 RID: 633
		private bool _isEncyclopediaOpen;

		// Token: 0x0400027A RID: 634
		private MarriageOfferPopupHeroVM _offereeClanMember;

		// Token: 0x0400027B RID: 635
		private MarriageOfferPopupHeroVM _offererClanMember;

		// Token: 0x0400027C RID: 636
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400027D RID: 637
		private InputKeyItemVM _doneInputKey;
	}
}
