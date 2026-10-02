using System;
using TaleWorlds.MountAndBlade.Missions.Hints;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems
{
	// Token: 0x02000042 RID: 66
	internal class MissionHintInteractionItemVM : MissionInteractionItemBaseVM
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x0001626D File Offset: 0x0001446D
		public MissionHintInteractionItemVM(MissionHint hint)
		{
			this.Hint = hint;
			this.RefreshValues();
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00016282 File Offset: 0x00014482
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Message = this.Hint.Description.ToString();
		}

		// Token: 0x0400029B RID: 667
		public readonly MissionHint Hint;
	}
}
