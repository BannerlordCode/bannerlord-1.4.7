using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000058 RID: 88
	public interface IEntityComponent
	{
		// Token: 0x06000700 RID: 1792
		void OnInitialize();

		// Token: 0x06000701 RID: 1793
		void OnFinalize();
	}
}
