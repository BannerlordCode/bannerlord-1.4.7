using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000051 RID: 81
	public interface ILoadingWindowManager
	{
		// Token: 0x06000872 RID: 2162
		void EnableLoadingWindow();

		// Token: 0x06000873 RID: 2163
		void DisableLoadingWindow();

		// Token: 0x06000874 RID: 2164
		void SetCurrentModeIsMultiplayer(bool isMultiplayer);

		// Token: 0x06000875 RID: 2165
		void Initialize();

		// Token: 0x06000876 RID: 2166
		void Destroy();
	}
}
