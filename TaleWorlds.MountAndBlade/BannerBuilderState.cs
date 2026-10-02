using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023A RID: 570
	public class BannerBuilderState : GameState
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x0007498D File Offset: 0x00072B8D
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x00074990 File Offset: 0x00072B90
		public string DefaultBannerKey { get; }

		// Token: 0x06002119 RID: 8473 RVA: 0x00074998 File Offset: 0x00072B98
		public BannerBuilderState()
		{
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000749A0 File Offset: 0x00072BA0
		public BannerBuilderState(string defaultBannerKey)
		{
			this.DefaultBannerKey = defaultBannerKey;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000749AF File Offset: 0x00072BAF
		protected override void OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000749B7 File Offset: 0x00072BB7
		protected override void OnFinalize()
		{
			base.OnFinalize();
		}
	}
}
