using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.BarterSystem.Barterables
{
	// Token: 0x02000480 RID: 1152
	public class DeclareWarBarterable : Barterable
	{
		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x0600492B RID: 18731 RVA: 0x00173586 File Offset: 0x00171786
		public override string StringID
		{
			get
			{
				return "declare_war_barterable";
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x0600492C RID: 18732 RVA: 0x0017358D File Offset: 0x0017178D
		// (set) Token: 0x0600492D RID: 18733 RVA: 0x00173595 File Offset: 0x00171795
		public IFaction DeclaringFaction { get; private set; }

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x0600492E RID: 18734 RVA: 0x0017359E File Offset: 0x0017179E
		// (set) Token: 0x0600492F RID: 18735 RVA: 0x001735A6 File Offset: 0x001717A6
		public IFaction OtherFaction { get; private set; }

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06004930 RID: 18736 RVA: 0x001735AF File Offset: 0x001717AF
		public override TextObject Name
		{
			get
			{
				TextObject textObject = new TextObject("{=GZwNgIon}Declare war against {OTHER_FACTION}", null);
				textObject.SetTextVariable("OTHER_FACTION", this.OtherFaction.Name);
				return textObject;
			}
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x001735D3 File Offset: 0x001717D3
		public DeclareWarBarterable(IFaction declaringFaction, IFaction otherFaction)
			: base(declaringFaction.Leader, null)
		{
			this.DeclaringFaction = declaringFaction;
			this.OtherFaction = otherFaction;
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x001735F0 File Offset: 0x001717F0
		public override void Apply()
		{
			DeclareWarAction.ApplyByDefault(base.OriginalOwner.MapFaction, this.OtherFaction.MapFaction);
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00173610 File Offset: 0x00171810
		public override int GetUnitValueForFaction(IFaction faction)
		{
			int num = 0;
			Clan clan = ((faction is Clan) ? ((Clan)faction) : ((Kingdom)faction).RulingClan);
			if (faction.MapFaction == base.OriginalOwner.MapFaction)
			{
				TextObject textObject;
				num = (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(base.OriginalOwner.MapFaction, this.OtherFaction.MapFaction, clan, out textObject, false);
			}
			else if (faction.MapFaction == this.OtherFaction.MapFaction)
			{
				TextObject textObject;
				num = (int)Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(this.OtherFaction.MapFaction, base.OriginalOwner.MapFaction, clan, out textObject, false);
			}
			return num;
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x001736C4 File Offset: 0x001718C4
		public override ImageIdentifier GetVisualIdentifier()
		{
			return null;
		}
	}
}
