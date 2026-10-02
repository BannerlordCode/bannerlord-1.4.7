using System;
using SandBox.View.Map;
using SandBox.View.Map.Navigation;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200002C RID: 44
	[OverrideView(typeof(MapBarView))]
	public class GauntletMapBarView : MapView
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000DA2C File Offset: 0x0000BC2C
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			this._mapBarGlobalLayer.OnMapConversationStarted();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000DA3F File Offset: 0x0000BC3F
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			this._mapBarGlobalLayer.OnMapConversationOver();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000DA52 File Offset: 0x0000BC52
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._mapBarGlobalLayer = new GauntletMapBarGlobalLayer(base.MapScreen, new MapNavigationHandler(), 8.5f);
			this._mapBarGlobalLayer.Initialize(new MapBarVM());
			ScreenManager.AddGlobalLayer(this._mapBarGlobalLayer, true);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000DA91 File Offset: 0x0000BC91
		protected override void OnFinalize()
		{
			this._mapBarGlobalLayer.OnFinalize();
			ScreenManager.RemoveGlobalLayer(this._mapBarGlobalLayer);
			base.OnFinalize();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000DAAF File Offset: 0x0000BCAF
		protected override void OnResume()
		{
			base.OnResume();
			this._mapBarGlobalLayer.Refresh();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000DAC2 File Offset: 0x0000BCC2
		protected override bool IsEscaped()
		{
			return this._mapBarGlobalLayer.IsEscaped();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000DACF File Offset: 0x0000BCCF
		protected override TutorialContexts GetTutorialContext()
		{
			if (this._mapBarGlobalLayer.IsInArmyManagement)
			{
				return TutorialContexts.ArmyManagement;
			}
			return base.GetTutorialContext();
		}

		// Token: 0x040000BD RID: 189
		protected GauntletMapBarGlobalLayer _mapBarGlobalLayer;
	}
}
