using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000031 RID: 49
	[ApplicationInterfaceBase]
	internal interface ITableauView
	{
		// Token: 0x06000538 RID: 1336
		[EngineMethod("create_tableau_view", false, null, false)]
		TableauView CreateTableauView(string viewName);

		// Token: 0x06000539 RID: 1337
		[EngineMethod("set_sort_meshes", false, null, false)]
		void SetSortingEnabled(UIntPtr pointer, bool value);

		// Token: 0x0600053A RID: 1338
		[EngineMethod("set_continous_rendering", false, null, false)]
		void SetContinousRendering(UIntPtr pointer, bool value);

		// Token: 0x0600053B RID: 1339
		[EngineMethod("set_do_not_render_this_frame", false, null, false)]
		void SetDoNotRenderThisFrame(UIntPtr pointer, bool value);

		// Token: 0x0600053C RID: 1340
		[EngineMethod("set_delete_after_rendering", false, null, false)]
		void SetDeleteAfterRendering(UIntPtr pointer, bool value);
	}
}
