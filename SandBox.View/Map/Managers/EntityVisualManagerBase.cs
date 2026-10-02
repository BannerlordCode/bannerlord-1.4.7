using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000071 RID: 113
	public abstract class EntityVisualManagerBase : CampaignEntityVisualComponent
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00025A54 File Offset: 0x00023C54
		public Scene MapScene
		{
			get
			{
				if (this._mapScene == null && Campaign.Current != null && Campaign.Current.MapSceneWrapper != null)
				{
					this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
				}
				return this._mapScene;
			}
		}

		// Token: 0x04000225 RID: 549
		private Scene _mapScene;
	}
}
