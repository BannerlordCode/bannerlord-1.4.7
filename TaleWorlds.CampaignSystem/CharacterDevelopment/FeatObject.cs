using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003AB RID: 939
	public sealed class FeatObject : PropertyObject
	{
		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x0600367E RID: 13950 RVA: 0x000E46A6 File Offset: 0x000E28A6
		public static MBReadOnlyList<FeatObject> All
		{
			get
			{
				return Campaign.Current.AllFeats;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x000E46B2 File Offset: 0x000E28B2
		// (set) Token: 0x06003680 RID: 13952 RVA: 0x000E46BA File Offset: 0x000E28BA
		public float EffectBonus { get; private set; }

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06003681 RID: 13953 RVA: 0x000E46C3 File Offset: 0x000E28C3
		// (set) Token: 0x06003682 RID: 13954 RVA: 0x000E46CB File Offset: 0x000E28CB
		public FeatObject.AdditionType IncrementType { get; private set; }

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003683 RID: 13955 RVA: 0x000E46D4 File Offset: 0x000E28D4
		// (set) Token: 0x06003684 RID: 13956 RVA: 0x000E46DC File Offset: 0x000E28DC
		public bool IsPositive { get; private set; }

		// Token: 0x06003685 RID: 13957 RVA: 0x000E46E5 File Offset: 0x000E28E5
		public FeatObject(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x000E46EE File Offset: 0x000E28EE
		public void Initialize(string name, string description, float effectBonus, bool isPositiveEffect, FeatObject.AdditionType incrementType)
		{
			base.Initialize(new TextObject(name, null), new TextObject(description, null));
			this.EffectBonus = effectBonus;
			this.IncrementType = incrementType;
			this.IsPositive = isPositiveEffect;
			base.AfterInitialized();
		}

		// Token: 0x02000782 RID: 1922
		public enum AdditionType
		{
			// Token: 0x04001EA4 RID: 7844
			Add,
			// Token: 0x04001EA5 RID: 7845
			AddFactor
		}
	}
}
