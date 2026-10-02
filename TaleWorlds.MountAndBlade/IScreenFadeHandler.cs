using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000388 RID: 904
	public interface IScreenFadeHandler
	{
		// Token: 0x060033E2 RID: 13282
		void BeginFadeOutAndIn(float fadeOutDuration = 0.5f, float blackOutDuration = 0.5f, float fadeInDuration = 0.5f);

		// Token: 0x060033E3 RID: 13283
		void BeginFadeOut(float fadeOutDuration = 0.5f);

		// Token: 0x060033E4 RID: 13284
		void BeginFadeIn(float fadeInDuration = 0.5f);

		// Token: 0x060033E5 RID: 13285
		ScreenFadeController.ScreenFadeState GetScreenFadeState();
	}
}
