using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000018 RID: 24
	public interface ISiegeDeploymentView
	{
		// Token: 0x0600009A RID: 154
		void OnEntitySelection(WeakGameEntity selectedEntity);

		// Token: 0x0600009B RID: 155
		void OnEntityHover(WeakGameEntity hoveredEntity);
	}
}
