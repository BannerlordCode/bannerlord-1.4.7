using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment
{
	// Token: 0x020000B3 RID: 179
	public class RecruitVolunteerVM : ViewModel
	{
		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600119C RID: 4508 RVA: 0x00046593 File Offset: 0x00044793
		// (set) Token: 0x0600119D RID: 4509 RVA: 0x0004659B File Offset: 0x0004479B
		public Hero OwnerHero { get; private set; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600119E RID: 4510 RVA: 0x000465A4 File Offset: 0x000447A4
		// (set) Token: 0x0600119F RID: 4511 RVA: 0x000465AC File Offset: 0x000447AC
		public List<CharacterObject> VolunteerTroops { get; private set; }

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x000465B5 File Offset: 0x000447B5
		public int GoldCost { get; }

		// Token: 0x060011A1 RID: 4513 RVA: 0x000465C0 File Offset: 0x000447C0
		public RecruitVolunteerVM(Hero owner, List<CharacterObject> troops, Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> onRecruit, Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> onRemoveFromCart)
		{
			this.OwnerHero = owner;
			this.VolunteerTroops = troops;
			this._onRecruit = onRecruit;
			this._onRemoveFromCart = onRemoveFromCart;
			this.Owner = new RecruitVolunteerOwnerVM(owner, (int)owner.GetRelationWithPlayer());
			this.Troops = new MBBindingList<RecruitVolunteerTroopVM>();
			int num = 0;
			foreach (CharacterObject characterObject in troops)
			{
				RecruitVolunteerTroopVM recruitVolunteerTroopVM = new RecruitVolunteerTroopVM(this, characterObject, num, new Action<RecruitVolunteerTroopVM>(this.ExecuteRecruit), new Action<RecruitVolunteerTroopVM>(this.ExecuteRemoveFromCart));
				recruitVolunteerTroopVM.CanBeRecruited = false;
				recruitVolunteerTroopVM.PlayerHasEnoughRelation = false;
				if (HeroHelper.HeroCanRecruitFromHero(Hero.MainHero, this.OwnerHero, num))
				{
					recruitVolunteerTroopVM.PlayerHasEnoughRelation = true;
					if (characterObject != null)
					{
						recruitVolunteerTroopVM.CanBeRecruited = true;
					}
				}
				num++;
				this.Troops.Add(recruitVolunteerTroopVM);
			}
			this.RecruitHint = new HintViewModel();
			this.RefreshProperties();
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000466C0 File Offset: 0x000448C0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshProperties();
			RecruitVolunteerOwnerVM owner = this.Owner;
			if (owner != null)
			{
				owner.RefreshValues();
			}
			this.Troops.ApplyActionOnAllItems(delegate(RecruitVolunteerTroopVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00046714 File Offset: 0x00044914
		public void ExecuteRecruit(RecruitVolunteerTroopVM troop)
		{
			this._onRecruit(this, troop);
			this.RefreshProperties();
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00046729 File Offset: 0x00044929
		public void ExecuteRemoveFromCart(RecruitVolunteerTroopVM troop)
		{
			this._onRemoveFromCart(this, troop);
			this.RefreshProperties();
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00046740 File Offset: 0x00044940
		private void RefreshProperties()
		{
			this.RecruitText = this.GoldCost.ToString();
			if (this.RecruitableNumber == 0)
			{
				this.QuantityText = GameTexts.FindText("str_none", null).ToString();
				return;
			}
			GameTexts.SetVariable("QUANTITY", this.RecruitableNumber.ToString());
			this.QuantityText = GameTexts.FindText("str_x_quantity", null).ToString();
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x000467AC File Offset: 0x000449AC
		public void OnRecruitMoveToCart(RecruitVolunteerTroopVM troop)
		{
			MBInformationManager.HideInformations();
			this.Troops.RemoveAt(troop.Index);
			RecruitVolunteerTroopVM recruitVolunteerTroopVM = new RecruitVolunteerTroopVM(this, null, troop.Index, new Action<RecruitVolunteerTroopVM>(this.ExecuteRecruit), new Action<RecruitVolunteerTroopVM>(this.ExecuteRemoveFromCart));
			recruitVolunteerTroopVM.IsTroopEmpty = true;
			recruitVolunteerTroopVM.PlayerHasEnoughRelation = true;
			this.Troops.Insert(troop.Index, recruitVolunteerTroopVM);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00046815 File Offset: 0x00044A15
		public void OnRecruitRemovedFromCart(RecruitVolunteerTroopVM troop)
		{
			this.Troops.RemoveAt(troop.Index);
			this.Troops.Insert(troop.Index, troop);
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060011A8 RID: 4520 RVA: 0x0004683A File Offset: 0x00044A3A
		// (set) Token: 0x060011A9 RID: 4521 RVA: 0x00046842 File Offset: 0x00044A42
		[DataSourceProperty]
		public MBBindingList<RecruitVolunteerTroopVM> Troops
		{
			get
			{
				return this._troops;
			}
			set
			{
				if (value != this._troops)
				{
					this._troops = value;
					base.OnPropertyChangedWithValue<MBBindingList<RecruitVolunteerTroopVM>>(value, "Troops");
				}
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060011AA RID: 4522 RVA: 0x00046860 File Offset: 0x00044A60
		// (set) Token: 0x060011AB RID: 4523 RVA: 0x00046868 File Offset: 0x00044A68
		[DataSourceProperty]
		public RecruitVolunteerOwnerVM Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (value != this._owner)
				{
					this._owner = value;
					base.OnPropertyChangedWithValue<RecruitVolunteerOwnerVM>(value, "Owner");
				}
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00046886 File Offset: 0x00044A86
		// (set) Token: 0x060011AD RID: 4525 RVA: 0x0004688E File Offset: 0x00044A8E
		[DataSourceProperty]
		public bool CanRecruit
		{
			get
			{
				return this._canRecruit;
			}
			set
			{
				if (value != this._canRecruit)
				{
					this._canRecruit = value;
					base.OnPropertyChangedWithValue(value, "CanRecruit");
				}
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x000468AC File Offset: 0x00044AAC
		// (set) Token: 0x060011AF RID: 4527 RVA: 0x000468B4 File Offset: 0x00044AB4
		[DataSourceProperty]
		public bool ButtonIsVisible
		{
			get
			{
				return this._buttonIsVisible;
			}
			set
			{
				if (value != this._buttonIsVisible)
				{
					this._buttonIsVisible = value;
					base.OnPropertyChangedWithValue(value, "ButtonIsVisible");
				}
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x000468D2 File Offset: 0x00044AD2
		// (set) Token: 0x060011B1 RID: 4529 RVA: 0x000468DA File Offset: 0x00044ADA
		[DataSourceProperty]
		public string QuantityText
		{
			get
			{
				return this._quantityText;
			}
			set
			{
				if (value != this._quantityText)
				{
					this._quantityText = value;
					base.OnPropertyChangedWithValue<string>(value, "QuantityText");
				}
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x000468FD File Offset: 0x00044AFD
		// (set) Token: 0x060011B3 RID: 4531 RVA: 0x00046905 File Offset: 0x00044B05
		[DataSourceProperty]
		public string RecruitText
		{
			get
			{
				return this._recruitText;
			}
			set
			{
				if (value != this._recruitText)
				{
					this._recruitText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecruitText");
				}
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00046928 File Offset: 0x00044B28
		// (set) Token: 0x060011B5 RID: 4533 RVA: 0x00046930 File Offset: 0x00044B30
		[DataSourceProperty]
		public HintViewModel RecruitHint
		{
			get
			{
				return this._recruitHint;
			}
			set
			{
				if (value != this._recruitHint)
				{
					this._recruitHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RecruitHint");
				}
			}
		}

		// Token: 0x0400080F RID: 2063
		public int RecruitableNumber;

		// Token: 0x04000810 RID: 2064
		private readonly Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> _onRecruit;

		// Token: 0x04000811 RID: 2065
		private readonly Action<RecruitVolunteerVM, RecruitVolunteerTroopVM> _onRemoveFromCart;

		// Token: 0x04000812 RID: 2066
		private string _quantityText;

		// Token: 0x04000813 RID: 2067
		private string _recruitText;

		// Token: 0x04000814 RID: 2068
		private bool _canRecruit;

		// Token: 0x04000815 RID: 2069
		private bool _buttonIsVisible;

		// Token: 0x04000816 RID: 2070
		private HintViewModel _recruitHint;

		// Token: 0x04000817 RID: 2071
		private RecruitVolunteerOwnerVM _owner;

		// Token: 0x04000818 RID: 2072
		private MBBindingList<RecruitVolunteerTroopVM> _troops;
	}
}
