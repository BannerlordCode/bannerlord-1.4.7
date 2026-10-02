using System;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000146 RID: 326
	public class FocusAddedByPlayerEvent : EventBase
	{
		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x00073846 File Offset: 0x00071A46
		// (set) Token: 0x06001F6F RID: 8047 RVA: 0x0007384E File Offset: 0x00071A4E
		public Hero AddedPlayer { get; private set; }

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x00073857 File Offset: 0x00071A57
		// (set) Token: 0x06001F71 RID: 8049 RVA: 0x0007385F File Offset: 0x00071A5F
		public SkillObject AddedSkill { get; private set; }

		// Token: 0x06001F72 RID: 8050 RVA: 0x00073868 File Offset: 0x00071A68
		public FocusAddedByPlayerEvent(Hero addedPlayer, SkillObject addedSkill)
		{
			this.AddedPlayer = addedPlayer;
			this.AddedSkill = addedSkill;
		}
	}
}
