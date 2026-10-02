using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037B RID: 891
	public class UsableGameObjectGroup : ScriptComponentBehavior, IVisible
	{
		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060032B6 RID: 12982 RVA: 0x000D0A70 File Offset: 0x000CEC70
		// (set) Token: 0x060032B7 RID: 12983 RVA: 0x000D0A8C File Offset: 0x000CEC8C
		public bool IsVisible
		{
			get
			{
				return base.GameEntity.IsVisibleIncludeParents();
			}
			set
			{
				base.GameEntity.SetVisibilityExcludeParents(value);
			}
		}
	}
}
