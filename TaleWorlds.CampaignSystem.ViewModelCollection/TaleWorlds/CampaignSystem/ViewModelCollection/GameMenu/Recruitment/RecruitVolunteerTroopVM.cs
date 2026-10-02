using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment
{
	// Token: 0x020000B2 RID: 178
	public class RecruitVolunteerTroopVM : ViewModel
	{
		// Token: 0x0600117B RID: 4475 RVA: 0x00045F98 File Offset: 0x00044198
		public RecruitVolunteerTroopVM(RecruitVolunteerVM owner, CharacterObject character, int index, Action<RecruitVolunteerTroopVM> onClick, Action<RecruitVolunteerTroopVM> onRemoveFromCart)
		{
			if (character != null)
			{
				this.NameText = character.Name.ToString();
				this._character = character;
				GameTexts.SetVariable("LEVEL", character.Level);
				this.Level = GameTexts.FindText("str_level_with_value", null).ToString();
				this.Character = character;
				this.Wage = this.Character.TroopWage;
				this.Cost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this.Character, Hero.MainHero, false).RoundedResultNumber;
				this.IsTroopEmpty = false;
				CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(character, false);
				this.ImageIdentifier = new CharacterImageIdentifierVM(characterCode);
				this.TierIconData = CampaignUIHelper.GetCharacterTierData(character, false);
				this.TypeIconData = CampaignUIHelper.GetCharacterTypeData(character, false);
			}
			else
			{
				this.IsTroopEmpty = true;
			}
			this.Owner = owner;
			if (this.Owner != null)
			{
				this._currentRelation = Hero.MainHero.GetRelation(this.Owner.OwnerHero);
			}
			this._maximumIndexCanBeRecruit = Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(Hero.MainHero, this.Owner.OwnerHero, -101);
			for (int i = -100; i < 100; i++)
			{
				if (index < Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(Hero.MainHero, this.Owner.OwnerHero, i))
				{
					this._requiredRelation = i;
					break;
				}
			}
			this._onClick = onClick;
			this.Index = index;
			this._onRemoveFromCart = onRemoveFromCart;
			this.RefreshValues();
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00046128 File Offset: 0x00044328
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._character != null)
			{
				this.NameText = this._character.Name.ToString();
				GameTexts.SetVariable("LEVEL", this._character.Level);
				this.Level = GameTexts.FindText("str_level_with_value", null).ToString();
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00046184 File Offset: 0x00044384
		public void ExecuteRecruit()
		{
			if (this.CanBeRecruited)
			{
				this._onClick(this);
				return;
			}
			if (this.IsInCart)
			{
				this._onRemoveFromCart(this);
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000461AF File Offset: 0x000443AF
		public void ExecuteOpenEncyclopedia()
		{
			if (this.Character != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Character.EncyclopediaLink);
			}
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000461D3 File Offset: 0x000443D3
		public void ExecuteRemoveFromCart()
		{
			if (this.IsInCart)
			{
				this._onRemoveFromCart(this);
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000461EC File Offset: 0x000443EC
		public virtual void ExecuteBeginHint()
		{
			if (this._character != null)
			{
				if (this.PlayerHasEnoughRelation)
				{
					InformationManager.ShowTooltip(typeof(CharacterObject), new object[] { this._character });
					return;
				}
				List<TooltipProperty> list = new List<TooltipProperty>();
				string text = "";
				list.Add(new TooltipProperty(text, this._character.Name.ToString(), 1, false, TooltipProperty.TooltipPropertyFlags.None));
				list.Add(new TooltipProperty(text, text, -1, false, TooltipProperty.TooltipPropertyFlags.None));
				GameTexts.SetVariable("LEVEL", this._character.Level);
				GameTexts.SetVariable("newline", "\n");
				list.Add(new TooltipProperty(text, GameTexts.FindText("str_level_with_value", null).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				GameTexts.SetVariable("REL1", this._currentRelation);
				GameTexts.SetVariable("REL2", this._requiredRelation);
				list.Add(new TooltipProperty(text, GameTexts.FindText("str_recruit_volunteers_not_enough_relation", null).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { list });
				return;
			}
			else
			{
				if (this.PlayerHasEnoughRelation)
				{
					MBInformationManager.ShowHint(GameTexts.FindText("str_recruit_volunteers_new_troop", null).ToString());
					return;
				}
				GameTexts.SetVariable("newline", "\n");
				GameTexts.SetVariable("REL1", this._currentRelation);
				GameTexts.SetVariable("REL2", this._requiredRelation);
				GameTexts.SetVariable("STR1", GameTexts.FindText("str_recruit_volunteers_new_troop", null));
				GameTexts.SetVariable("STR2", GameTexts.FindText("str_recruit_volunteers_not_enough_relation", null));
				MBInformationManager.ShowHint(GameTexts.FindText("str_string_newline_string", null).ToString());
				return;
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0004638E File Offset: 0x0004458E
		public virtual void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00046395 File Offset: 0x00044595
		public void ExecuteFocus()
		{
			if (!this.IsTroopEmpty)
			{
				Action<RecruitVolunteerTroopVM> onFocused = RecruitVolunteerTroopVM.OnFocused;
				if (onFocused == null)
				{
					return;
				}
				onFocused(this);
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000463AF File Offset: 0x000445AF
		public void ExecuteUnfocus()
		{
			Action<RecruitVolunteerTroopVM> onFocused = RecruitVolunteerTroopVM.OnFocused;
			if (onFocused == null)
			{
				return;
			}
			onFocused(null);
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x000463C1 File Offset: 0x000445C1
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x000463C9 File Offset: 0x000445C9
		[DataSourceProperty]
		public string Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue<string>(value, "Level");
				}
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x000463EC File Offset: 0x000445EC
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x000463F4 File Offset: 0x000445F4
		[DataSourceProperty]
		public bool CanBeRecruited
		{
			get
			{
				return this._canBeRecruited;
			}
			set
			{
				if (value != this._canBeRecruited)
				{
					this._canBeRecruited = value;
					base.OnPropertyChangedWithValue(value, "CanBeRecruited");
				}
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x00046412 File Offset: 0x00044612
		// (set) Token: 0x06001189 RID: 4489 RVA: 0x0004641A File Offset: 0x0004461A
		[DataSourceProperty]
		public bool IsHiglightEnabled
		{
			get
			{
				return this._isHiglightEnabled;
			}
			set
			{
				if (value != this._isHiglightEnabled)
				{
					this._isHiglightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHiglightEnabled");
				}
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00046438 File Offset: 0x00044638
		// (set) Token: 0x0600118B RID: 4491 RVA: 0x00046440 File Offset: 0x00044640
		[DataSourceProperty]
		public int Wage
		{
			get
			{
				return this._wage;
			}
			set
			{
				if (value != this._wage)
				{
					this._wage = value;
					base.OnPropertyChangedWithValue(value, "Wage");
				}
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x0004645E File Offset: 0x0004465E
		// (set) Token: 0x0600118D RID: 4493 RVA: 0x00046466 File Offset: 0x00044666
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x00046484 File Offset: 0x00044684
		// (set) Token: 0x0600118F RID: 4495 RVA: 0x0004648C File Offset: 0x0004468C
		[DataSourceProperty]
		public bool IsInCart
		{
			get
			{
				return this._isInCart;
			}
			set
			{
				if (value != this._isInCart)
				{
					this._isInCart = value;
					base.OnPropertyChangedWithValue(value, "IsInCart");
				}
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x000464AA File Offset: 0x000446AA
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x000464B2 File Offset: 0x000446B2
		[DataSourceProperty]
		public bool IsTroopEmpty
		{
			get
			{
				return this._isTroopEmpty;
			}
			set
			{
				if (value != this._isTroopEmpty)
				{
					this._isTroopEmpty = value;
					base.OnPropertyChangedWithValue(value, "IsTroopEmpty");
				}
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x000464D0 File Offset: 0x000446D0
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x000464D8 File Offset: 0x000446D8
		[DataSourceProperty]
		public bool PlayerHasEnoughRelation
		{
			get
			{
				return this._playerHasEnoughRelation;
			}
			set
			{
				if (value != this._playerHasEnoughRelation)
				{
					this._playerHasEnoughRelation = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasEnoughRelation");
				}
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x000464F6 File Offset: 0x000446F6
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x000464FE File Offset: 0x000446FE
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0004651C File Offset: 0x0004471C
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x00046524 File Offset: 0x00044724
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

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00046547 File Offset: 0x00044747
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x0004654F File Offset: 0x0004474F
		[DataSourceProperty]
		public StringItemWithHintVM TierIconData
		{
			get
			{
				return this._tierIconData;
			}
			set
			{
				if (value != this._tierIconData)
				{
					this._tierIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TierIconData");
				}
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x0004656D File Offset: 0x0004476D
		// (set) Token: 0x0600119B RID: 4507 RVA: 0x00046575 File Offset: 0x00044775
		[DataSourceProperty]
		public StringItemWithHintVM TypeIconData
		{
			get
			{
				return this._typeIconData;
			}
			set
			{
				if (value != this._typeIconData)
				{
					this._typeIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TypeIconData");
				}
			}
		}

		// Token: 0x040007F6 RID: 2038
		public static Action<RecruitVolunteerTroopVM> OnFocused;

		// Token: 0x040007F7 RID: 2039
		private readonly Action<RecruitVolunteerTroopVM> _onClick;

		// Token: 0x040007F8 RID: 2040
		private readonly Action<RecruitVolunteerTroopVM> _onRemoveFromCart;

		// Token: 0x040007F9 RID: 2041
		private CharacterObject _character;

		// Token: 0x040007FA RID: 2042
		public CharacterObject Character;

		// Token: 0x040007FB RID: 2043
		public int Index;

		// Token: 0x040007FC RID: 2044
		private int _maximumIndexCanBeRecruit;

		// Token: 0x040007FD RID: 2045
		private int _requiredRelation;

		// Token: 0x040007FE RID: 2046
		public RecruitVolunteerVM Owner;

		// Token: 0x040007FF RID: 2047
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x04000800 RID: 2048
		private string _nameText;

		// Token: 0x04000801 RID: 2049
		private string _level;

		// Token: 0x04000802 RID: 2050
		private bool _canBeRecruited;

		// Token: 0x04000803 RID: 2051
		private bool _isInCart;

		// Token: 0x04000804 RID: 2052
		private int _wage;

		// Token: 0x04000805 RID: 2053
		private int _cost;

		// Token: 0x04000806 RID: 2054
		private bool _isTroopEmpty;

		// Token: 0x04000807 RID: 2055
		private bool _playerHasEnoughRelation;

		// Token: 0x04000808 RID: 2056
		private int _currentRelation;

		// Token: 0x04000809 RID: 2057
		private bool _isHiglightEnabled;

		// Token: 0x0400080A RID: 2058
		private StringItemWithHintVM _tierIconData;

		// Token: 0x0400080B RID: 2059
		private StringItemWithHintVM _typeIconData;
	}
}
