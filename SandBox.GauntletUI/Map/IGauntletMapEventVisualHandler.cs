using System;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000036 RID: 54
	public interface IGauntletMapEventVisualHandler
	{
		// Token: 0x06000291 RID: 657
		void OnNewEventStarted(GauntletMapEventVisual newEvent);

		// Token: 0x06000292 RID: 658
		void OnInitialized(GauntletMapEventVisual newEvent);

		// Token: 0x06000293 RID: 659
		void OnEventEnded(GauntletMapEventVisual newEvent);

		// Token: 0x06000294 RID: 660
		void OnEventVisibilityChanged(GauntletMapEventVisual visibilityChangedEvent);
	}
}
