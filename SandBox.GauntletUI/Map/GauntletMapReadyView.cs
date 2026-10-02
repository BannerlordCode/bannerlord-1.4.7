using System;
using SandBox.View.Map;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003E RID: 62
	[OverrideView(typeof(MapReadyView))]
	public class GauntletMapReadyView : MapReadyView
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x00011568 File Offset: 0x0000F768
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new BoolItemWithActionVM(null, true, null);
			this._layerAsGauntletLayer = new GauntletLayer("MapReadyBlocker", 9999, false);
			this._layerAsGauntletLayer.LoadMovie("MapReadyBlocker", this._dataSource);
			base.Layer = this._layerAsGauntletLayer;
			base.MapScreen.AddLayer(base.Layer);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000115D3 File Offset: 0x0000F7D3
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._dataSource.OnFinalize();
			base.MapScreen.RemoveLayer(base.Layer);
			base.Layer = null;
			this._dataSource = null;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00011605 File Offset: 0x0000F805
		public override void SetIsMapSceneReady(bool isReady)
		{
			base.SetIsMapSceneReady(isReady);
			this._dataSource.IsActive = !isReady;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001161D File Offset: 0x0000F81D
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00011639 File Offset: 0x0000F839
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x0400011E RID: 286
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x0400011F RID: 287
		private BoolItemWithActionVM _dataSource;
	}
}
