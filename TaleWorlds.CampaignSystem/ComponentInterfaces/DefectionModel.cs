using System;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019A RID: 410
	public abstract class DefectionModel : MBGameModel<DefaultDefectionModel>
	{
		// Token: 0x06001C79 RID: 7289
		public abstract bool CanHeroDefectToFaction(Hero hero, Kingdom kingdom);
	}
}
