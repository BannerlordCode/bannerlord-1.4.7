using System;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x0200020C RID: 524
	public abstract class CharacterCreationStageBase
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x00090DAD File Offset: 0x0008EFAD
		// (set) Token: 0x0600200D RID: 8205 RVA: 0x00090DB5 File Offset: 0x0008EFB5
		public ICharacterCreationStageListener Listener { get; set; }

		// Token: 0x0600200E RID: 8206 RVA: 0x00090DBE File Offset: 0x0008EFBE
		protected internal virtual void OnFinalize()
		{
			ICharacterCreationStageListener listener = this.Listener;
			if (listener == null)
			{
				return;
			}
			listener.OnStageFinalize();
		}
	}
}
