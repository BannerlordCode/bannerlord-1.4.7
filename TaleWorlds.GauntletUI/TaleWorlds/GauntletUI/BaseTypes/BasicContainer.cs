using System;
using System.Numerics;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000051 RID: 81
	public class BasicContainer : Container
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x000170FE File Offset: 0x000152FE
		public BasicContainer(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00017107 File Offset: 0x00015307
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0001710F File Offset: 0x0001530F
		public override Predicate<Widget> AcceptDropPredicate { get; set; }

		// Token: 0x0600057A RID: 1402 RVA: 0x00017118 File Offset: 0x00015318
		public override Vector2 GetDropGizmoPosition(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001711F File Offset: 0x0001531F
		public override int GetIndexForDrop(Vector2 draggedWidgetPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00017126 File Offset: 0x00015326
		public override bool IsDragHovering { get; }

		// Token: 0x0600057D RID: 1405 RVA: 0x00017130 File Offset: 0x00015330
		public override void OnChildSelected(Widget widget)
		{
			int num = -1;
			for (int i = 0; i < base.ChildCount; i++)
			{
				if (widget == base.GetChild(i))
				{
					num = i;
				}
			}
			base.IntValue = num;
		}
	}
}
