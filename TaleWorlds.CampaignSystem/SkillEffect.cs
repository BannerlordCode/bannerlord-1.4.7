using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000AB RID: 171
	public sealed class SkillEffect : PropertyObject
	{
		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x0005AA20 File Offset: 0x00058C20
		public static MBReadOnlyList<SkillEffect> All
		{
			get
			{
				return Campaign.Current.AllSkillEffects;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0005AA2C File Offset: 0x00058C2C
		// (set) Token: 0x06001368 RID: 4968 RVA: 0x0005AA34 File Offset: 0x00058C34
		public float Bonus { get; private set; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x0005AA3D File Offset: 0x00058C3D
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x0005AA45 File Offset: 0x00058C45
		public float BaseValue { get; private set; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0005AA4E File Offset: 0x00058C4E
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0005AA56 File Offset: 0x00058C56
		public float LimitMin { get; private set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0005AA5F File Offset: 0x00058C5F
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x0005AA67 File Offset: 0x00058C67
		public float LimitMax { get; private set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x0005AA70 File Offset: 0x00058C70
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x0005AA78 File Offset: 0x00058C78
		public PartyRole Role { get; private set; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0005AA81 File Offset: 0x00058C81
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x0005AA89 File Offset: 0x00058C89
		public EffectIncrementType IncrementType { get; private set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0005AA92 File Offset: 0x00058C92
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x0005AA9A File Offset: 0x00058C9A
		public SkillObject EffectedSkill { get; private set; }

		// Token: 0x06001375 RID: 4981 RVA: 0x0005AAA3 File Offset: 0x00058CA3
		public SkillEffect(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0005AAAC File Offset: 0x00058CAC
		public void Initialize(TextObject description, SkillObject effectedSkill, PartyRole role, float bonus, EffectIncrementType incrementType, float baseValue = 0f, float limitMin = -3.4028235E+38f, float limitMax = 3.4028235E+38f)
		{
			base.Initialize(TextObject.GetEmpty(), description);
			this.Role = role;
			this.Bonus = bonus;
			this.IncrementType = incrementType;
			this.EffectedSkill = effectedSkill;
			this.BaseValue = baseValue;
			this.LimitMin = limitMin;
			this.LimitMax = limitMax;
			base.AfterInitialized();
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0005AB01 File Offset: 0x00058D01
		public float GetSkillEffectValue(int skillLevel)
		{
			return MathF.Clamp(this.BaseValue + this.Bonus * (float)skillLevel, this.LimitMin, this.LimitMax);
		}
	}
}
