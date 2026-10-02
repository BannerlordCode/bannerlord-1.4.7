using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003B RID: 59
	internal class EmptyWidget : Widget
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x0000FEB0 File Offset: 0x0000E0B0
		public EmptyWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000FEB9 File Offset: 0x0000E0B9
		protected override void OnUpdate(float dt)
		{
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000FEBB File Offset: 0x0000E0BB
		protected override void OnParallelUpdate(float dt)
		{
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000FEBD File Offset: 0x0000E0BD
		protected override void OnLateUpdate(float dt)
		{
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000FEBF File Offset: 0x0000E0BF
		public override void UpdateBrushes(float dt)
		{
		}
	}
}
