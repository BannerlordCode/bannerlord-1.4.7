using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000080 RID: 128
	public class PolicyDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002DD40 File Offset: 0x0002BF40
		public KingdomPolicyDecision PolicyDecision
		{
			get
			{
				KingdomPolicyDecision kingdomPolicyDecision;
				if ((kingdomPolicyDecision = this._policyDecision) == null)
				{
					kingdomPolicyDecision = (this._policyDecision = this._decision as KingdomPolicyDecision);
				}
				return kingdomPolicyDecision;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0002DD6B File Offset: 0x0002BF6B
		public PolicyObject Policy
		{
			get
			{
				return this.PolicyDecision.Policy;
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0002DD78 File Offset: 0x0002BF78
		public PolicyDecisionItemVM(KingdomPolicyDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			base.DecisionType = 3;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002DD8C File Offset: 0x0002BF8C
		protected override void InitValues()
		{
			base.InitValues();
			base.DecisionType = 3;
			this.NameText = this.Policy.Name.ToString();
			this.PolicyDescriptionText = this.Policy.Description.ToString();
			this.PolicyEffectList = new MBBindingList<StringItemWithHintVM>();
			foreach (string text in this.Policy.SecondaryEffects.ToString().Split(new char[] { '\n' }))
			{
				this.PolicyEffectList.Add(new StringItemWithHintVM(text, TextObject.GetEmpty()));
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0002DE26 File Offset: 0x0002C026
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x0002DE2E File Offset: 0x0002C02E
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

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0002DE51 File Offset: 0x0002C051
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x0002DE59 File Offset: 0x0002C059
		[DataSourceProperty]
		public string PolicyDescriptionText
		{
			get
			{
				return this._policyDescriptionText;
			}
			set
			{
				if (value != this._policyDescriptionText)
				{
					this._policyDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PolicyDescriptionText");
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0002DE7C File Offset: 0x0002C07C
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x0002DE84 File Offset: 0x0002C084
		[DataSourceProperty]
		public MBBindingList<StringItemWithHintVM> PolicyEffectList
		{
			get
			{
				return this._policyEffectList;
			}
			set
			{
				if (value != this._policyEffectList)
				{
					this._policyEffectList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringItemWithHintVM>>(value, "PolicyEffectList");
				}
			}
		}

		// Token: 0x040004B5 RID: 1205
		private KingdomPolicyDecision _policyDecision;

		// Token: 0x040004B6 RID: 1206
		private MBBindingList<StringItemWithHintVM> _policyEffectList;

		// Token: 0x040004B7 RID: 1207
		private string _nameText;

		// Token: 0x040004B8 RID: 1208
		private string _policyDescriptionText;
	}
}
