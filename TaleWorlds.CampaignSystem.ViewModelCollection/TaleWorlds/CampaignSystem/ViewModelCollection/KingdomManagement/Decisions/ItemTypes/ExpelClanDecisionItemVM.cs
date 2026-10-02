using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x0200007C RID: 124
	public class ExpelClanDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0002CB9C File Offset: 0x0002AD9C
		public ExpelClanFromKingdomDecision ExpelDecision
		{
			get
			{
				ExpelClanFromKingdomDecision expelClanFromKingdomDecision;
				if ((expelClanFromKingdomDecision = this._expelDecision) == null)
				{
					expelClanFromKingdomDecision = (this._expelDecision = this._decision as ExpelClanFromKingdomDecision);
				}
				return expelClanFromKingdomDecision;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0002CBC7 File Offset: 0x0002ADC7
		public Clan Clan
		{
			get
			{
				return this.ExpelDecision.ClanToExpel;
			}
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002CBD4 File Offset: 0x0002ADD4
		public ExpelClanDecisionItemVM(ExpelClanFromKingdomDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			base.DecisionType = 2;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0002CBE8 File Offset: 0x0002ADE8
		protected override void InitValues()
		{
			base.InitValues();
			base.DecisionType = 2;
			this.Members = new MBBindingList<HeroVM>();
			this.Fiefs = new MBBindingList<EncyclopediaSettlementVM>();
			GameTexts.SetVariable("RENOWN", this.Clan.Renown);
			string text = "STR1";
			TextObject encyclopediaText = this.Clan.EncyclopediaText;
			GameTexts.SetVariable(text, (encyclopediaText != null) ? encyclopediaText.ToString() : null);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_encyclopedia_renown", null).ToString());
			this.InformationText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.Leader = new HeroVM(this.Clan.Leader, false);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.SettlementsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.NameText = this.Clan.Name.ToString();
			int num = 0;
			float num2 = 0f;
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
			foreach (Hero hero in this.Clan.Heroes)
			{
				if (hero.IsAlive && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && pageOf.IsValidEncyclopediaItem(hero))
				{
					if (hero != this.Leader.Hero)
					{
						this.Members.Add(new HeroVM(hero, false));
					}
					num += hero.Gold;
				}
			}
			foreach (Hero hero2 in this.Clan.Companions)
			{
				if (hero2.IsAlive && hero2.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && pageOf.IsValidEncyclopediaItem(hero2))
				{
					if (hero2 != this.Leader.Hero)
					{
						this.Members.Add(new HeroVM(hero2, false));
					}
					num += hero2.Gold;
				}
			}
			foreach (MobileParty mobileParty in MobileParty.AllLordParties)
			{
				if (mobileParty.ActualClan == this.Clan && !mobileParty.IsDisbanding)
				{
					num2 += mobileParty.Party.CalculateCurrentStrength();
				}
			}
			this.ProsperityText = num.ToString();
			this.ProsperityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetClanProsperityTooltip(this.Clan));
			this.StrengthText = num2.ToString();
			this.StrengthHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetClanStrengthTooltip(this.Clan));
			foreach (Town town in from s in this.Clan.Fiefs
				orderby s.IsCastle, s.IsTown
				select s)
			{
				if (town.Settlement.OwnerClan == this.Clan)
				{
					this.Fiefs.Add(new EncyclopediaSettlementVM(town.Settlement));
				}
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0002CFB0 File Offset: 0x0002B1B0
		// (set) Token: 0x06000A4C RID: 2636 RVA: 0x0002CFB8 File Offset: 0x0002B1B8
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

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0002CFD6 File Offset: 0x0002B1D6
		// (set) Token: 0x06000A4E RID: 2638 RVA: 0x0002CFDE File Offset: 0x0002B1DE
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementVM> Fiefs
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
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementVM>>(value, "Fiefs");
				}
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0002CFFC File Offset: 0x0002B1FC
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x0002D004 File Offset: 0x0002B204
		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Leader");
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0002D022 File Offset: 0x0002B222
		// (set) Token: 0x06000A52 RID: 2642 RVA: 0x0002D02A File Offset: 0x0002B22A
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

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0002D04D File Offset: 0x0002B24D
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x0002D055 File Offset: 0x0002B255
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
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0002D078 File Offset: 0x0002B278
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x0002D080 File Offset: 0x0002B280
		[DataSourceProperty]
		public string SettlementsText
		{
			get
			{
				return this._settlementsText;
			}
			set
			{
				if (value != this._settlementsText)
				{
					this._settlementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsText");
				}
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0002D0A3 File Offset: 0x0002B2A3
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x0002D0AB File Offset: 0x0002B2AB
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
					base.OnPropertyChangedWithValue<string>(value, "InformationText");
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0002D0CE File Offset: 0x0002B2CE
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x0002D0D6 File Offset: 0x0002B2D6
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderText");
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0002D0F9 File Offset: 0x0002B2F9
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0002D101 File Offset: 0x0002B301
		[DataSourceProperty]
		public string ProsperityText
		{
			get
			{
				return this._prosperityText;
			}
			set
			{
				if (value != this._prosperityText)
				{
					this._prosperityText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProsperityText");
				}
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0002D124 File Offset: 0x0002B324
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x0002D12C File Offset: 0x0002B32C
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._strengthText;
			}
			set
			{
				if (value != this._strengthText)
				{
					this._strengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthText");
				}
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0002D14F File Offset: 0x0002B34F
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x0002D157 File Offset: 0x0002B357
		[DataSourceProperty]
		public BasicTooltipViewModel ProsperityHint
		{
			get
			{
				return this._prosperityHint;
			}
			set
			{
				if (value != this._prosperityHint)
				{
					this._prosperityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ProsperityHint");
				}
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0002D175 File Offset: 0x0002B375
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0002D17D File Offset: 0x0002B37D
		[DataSourceProperty]
		public BasicTooltipViewModel StrengthHint
		{
			get
			{
				return this._strengthHint;
			}
			set
			{
				if (value != this._strengthHint)
				{
					this._strengthHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "StrengthHint");
				}
			}
		}

		// Token: 0x04000491 RID: 1169
		private ExpelClanFromKingdomDecision _expelDecision;

		// Token: 0x04000492 RID: 1170
		private MBBindingList<HeroVM> _members;

		// Token: 0x04000493 RID: 1171
		private MBBindingList<EncyclopediaSettlementVM> _fiefs;

		// Token: 0x04000494 RID: 1172
		private HeroVM _leader;

		// Token: 0x04000495 RID: 1173
		private string _nameText;

		// Token: 0x04000496 RID: 1174
		private string _membersText;

		// Token: 0x04000497 RID: 1175
		private string _settlementsText;

		// Token: 0x04000498 RID: 1176
		private string _leaderText;

		// Token: 0x04000499 RID: 1177
		private string _informationText;

		// Token: 0x0400049A RID: 1178
		private string _prosperityText;

		// Token: 0x0400049B RID: 1179
		private string _strengthText;

		// Token: 0x0400049C RID: 1180
		private BasicTooltipViewModel _prosperityHint;

		// Token: 0x0400049D RID: 1181
		private BasicTooltipViewModel _strengthHint;
	}
}
