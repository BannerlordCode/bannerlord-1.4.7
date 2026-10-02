using System;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace TaleWorlds.CampaignSystem.FastMode
{
	// Token: 0x02000003 RID: 3
	public class FastModeSubModule : MBSubModuleBase
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002064 File Offset: 0x00000264
		protected override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
		{
			Campaign campaign = game.GameType as Campaign;
			if (campaign != null && campaign.CampaignGameLoadingType == Campaign.GameLoadingType.NewCampaign)
			{
				campaign.Options.AccelerationMode = GameAccelerationMode.Fast;
				CampaignGameStarter campaignGameStarter;
				if ((campaignGameStarter = gameStarterObject as CampaignGameStarter) != null)
				{
					DefaultCharacterDevelopmentModel model = campaignGameStarter.GetModel<DefaultCharacterDevelopmentModel>();
					if (model == null)
					{
						return;
					}
					model.InitializeXpRequiredForSkillLevel();
				}
			}
		}
	}
}
