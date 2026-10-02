using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002A RID: 42
	public interface IDropContainer
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000338 RID: 824
		// (set) Token: 0x06000339 RID: 825
		Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x0600033A RID: 826
		Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition);
	}
}
