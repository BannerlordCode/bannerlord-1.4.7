using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000104 RID: 260
	public class WeaponAttributeVM : ViewModel
	{
		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x0005A8DC File Offset: 0x00058ADC
		public DamageTypes DamageType { get; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0005A8E4 File Offset: 0x00058AE4
		public CraftingTemplate.CraftingStatTypes AttributeType { get; }

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x0005A8EC File Offset: 0x00058AEC
		public float AttributeValue { get; }

		// Token: 0x06001793 RID: 6035 RVA: 0x0005A8F4 File Offset: 0x00058AF4
		public WeaponAttributeVM(CraftingTemplate.CraftingStatTypes type, DamageTypes damageType, string attributeName, float attributeValue)
		{
			this.AttributeType = type;
			this.DamageType = damageType;
			this.AttributeValue = attributeValue;
			string text = ((this.AttributeValue > 100f) ? attributeValue.ToString("F0") : attributeValue.ToString("F1"));
			string text2 = "<span style=\"Value\">" + text + "</span>";
			TextObject textObject = new TextObject("{=!}{ATTR_NAME}{ATTR_VALUE_RTT}", null);
			textObject.SetTextVariable("ATTR_NAME", attributeName);
			textObject.SetTextVariable("ATTR_VALUE_RTT", text2);
			this.AttributeFieldText = textObject.ToString();
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x0005A988 File Offset: 0x00058B88
		// (set) Token: 0x06001795 RID: 6037 RVA: 0x0005A990 File Offset: 0x00058B90
		[DataSourceProperty]
		public string AttributeFieldText
		{
			get
			{
				return this._attributeFieldText;
			}
			set
			{
				if (value != this._attributeFieldText)
				{
					this._attributeFieldText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttributeFieldText");
				}
			}
		}

		// Token: 0x04000ACA RID: 2762
		private string _attributeFieldText;
	}
}
