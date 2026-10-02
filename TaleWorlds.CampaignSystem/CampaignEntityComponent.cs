using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000038 RID: 56
	public class CampaignEntityComponent : IEntityComponent
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x0001E7C7 File Offset: 0x0001C9C7
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001E7CF File Offset: 0x0001C9CF
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001E7D7 File Offset: 0x0001C9D7
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001E7D9 File Offset: 0x0001C9D9
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001E7DB File Offset: 0x0001C9DB
		public virtual void OnTick(float realDt, float dt)
		{
		}
	}
}
