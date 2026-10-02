using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation
{
	// Token: 0x02000187 RID: 391
	public class CharacterCreationNarrativeStageScreenWidget : Widget
	{
		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0003723F File Offset: 0x0003543F
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x00037247 File Offset: 0x00035447
		public ButtonWidget NextButton { get; set; }

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00037250 File Offset: 0x00035450
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x00037258 File Offset: 0x00035458
		public ButtonWidget PreviousButton { get; set; }

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x00037261 File Offset: 0x00035461
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x00037269 File Offset: 0x00035469
		public ListPanel ItemList { get; set; }

		// Token: 0x06001444 RID: 5188 RVA: 0x00037272 File Offset: 0x00035472
		public CharacterCreationNarrativeStageScreenWidget(UIContext context)
			: base(context)
		{
		}
	}
}
