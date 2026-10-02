using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Policies
{
	// Token: 0x0200006B RID: 107
	public class KingdomPolicyItemVM : KingdomItemVM
	{
		// Token: 0x060008A4 RID: 2212 RVA: 0x00026F8C File Offset: 0x0002518C
		public KingdomPolicyItemVM(PolicyObject policy, Action<KingdomPolicyItemVM> onSelect, Func<PolicyObject, bool> getIsPolicyActive)
		{
			this._onSelect = onSelect;
			this._policy = policy;
			this._getIsPolicyActive = getIsPolicyActive;
			this.Name = policy.Name.ToString();
			this.Explanation = policy.Description.ToString();
			this.LikelihoodHint = new HintViewModel();
			this.PolicyEffectList = new MBBindingList<StringItemWithHintVM>();
			foreach (string text in policy.SecondaryEffects.ToString().Split(new char[] { '\n' }))
			{
				this.PolicyEffectList.Add(new StringItemWithHintVM(text, TextObject.GetEmpty()));
			}
			this.RefreshValues();
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00027038 File Offset: 0x00025238
		public override void RefreshValues()
		{
			base.RefreshValues();
			Func<PolicyObject, bool> getIsPolicyActive = this._getIsPolicyActive;
			this.PolicyAcceptanceText = ((getIsPolicyActive != null && getIsPolicyActive(this.Policy)) ? GameTexts.FindText("str_policy_support_for_abolishing", null).ToString() : GameTexts.FindText("str_policy_support_for_enacting", null).ToString());
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002708D File Offset: 0x0002528D
		protected override void OnSelect()
		{
			base.OnSelect();
			this._onSelect(this);
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x000270A1 File Offset: 0x000252A1
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x000270A9 File Offset: 0x000252A9
		[DataSourceProperty]
		public string PolicyAcceptanceText
		{
			get
			{
				return this._policyAcceptanceText;
			}
			set
			{
				if (value != this._policyAcceptanceText)
				{
					this._policyAcceptanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "PolicyAcceptanceText");
				}
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x000270CC File Offset: 0x000252CC
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x000270D4 File Offset: 0x000252D4
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

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x000270F2 File Offset: 0x000252F2
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x000270FA File Offset: 0x000252FA
		[DataSourceProperty]
		public string PolicyLikelihoodText
		{
			get
			{
				return this._policyLikelihoodText;
			}
			set
			{
				if (value != this._policyLikelihoodText)
				{
					this._policyLikelihoodText = value;
					base.OnPropertyChangedWithValue<string>(value, "PolicyLikelihoodText");
				}
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0002711D File Offset: 0x0002531D
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x00027125 File Offset: 0x00025325
		[DataSourceProperty]
		public HintViewModel LikelihoodHint
		{
			get
			{
				return this._likelihoodHint;
			}
			set
			{
				if (value != this._likelihoodHint)
				{
					this._likelihoodHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LikelihoodHint");
				}
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00027143 File Offset: 0x00025343
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x0002714B File Offset: 0x0002534B
		[DataSourceProperty]
		public PolicyObject Policy
		{
			get
			{
				return this._policy;
			}
			set
			{
				if (value != this._policy)
				{
					this._policy = value;
					base.OnPropertyChangedWithValue<PolicyObject>(value, "Policy");
				}
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00027169 File Offset: 0x00025369
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00027171 File Offset: 0x00025371
		[DataSourceProperty]
		public int PolicyLikelihood
		{
			get
			{
				return this._policyLikelihood;
			}
			set
			{
				if (value != this._policyLikelihood)
				{
					this._policyLikelihood = value;
					base.OnPropertyChangedWithValue(value, "PolicyLikelihood");
				}
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0002718F File Offset: 0x0002538F
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00027197 File Offset: 0x00025397
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x000271BA File Offset: 0x000253BA
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x000271C2 File Offset: 0x000253C2
		[DataSourceProperty]
		public string Explanation
		{
			get
			{
				return this._explanation;
			}
			set
			{
				if (value != this._explanation)
				{
					this._explanation = value;
					base.OnPropertyChangedWithValue<string>(value, "Explanation");
				}
			}
		}

		// Token: 0x040003C4 RID: 964
		private readonly Action<KingdomPolicyItemVM> _onSelect;

		// Token: 0x040003C5 RID: 965
		private readonly Func<PolicyObject, bool> _getIsPolicyActive;

		// Token: 0x040003C6 RID: 966
		private string _name;

		// Token: 0x040003C7 RID: 967
		private string _explanation;

		// Token: 0x040003C8 RID: 968
		private string _policyAcceptanceText;

		// Token: 0x040003C9 RID: 969
		private PolicyObject _policy;

		// Token: 0x040003CA RID: 970
		private int _policyLikelihood;

		// Token: 0x040003CB RID: 971
		private string _policyLikelihoodText;

		// Token: 0x040003CC RID: 972
		private HintViewModel _likelihoodHint;

		// Token: 0x040003CD RID: 973
		private MBBindingList<StringItemWithHintVM> _policyEffectList;
	}
}
