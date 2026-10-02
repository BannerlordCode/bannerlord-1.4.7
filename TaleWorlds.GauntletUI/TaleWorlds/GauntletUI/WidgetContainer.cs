using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200003A RID: 58
	internal class WidgetContainer
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000FD0D File Offset: 0x0000DF0D
		internal int Count
		{
			get
			{
				return this.GetActiveList().Count;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000FD1A File Offset: 0x0000DF1A
		internal WidgetContainer(int initialCapacity)
		{
			this._backList = new HashSet<Widget>();
			this._frontList = new MBList<Widget>(initialCapacity);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000FD3C File Offset: 0x0000DF3C
		internal void Add(Widget widget)
		{
			HashSet<Widget> backList = this._backList;
			lock (backList)
			{
				this._backList.Add(widget);
			}
			this._isFragmented = true;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		internal void Remove(Widget widget)
		{
			HashSet<Widget> backList = this._backList;
			lock (backList)
			{
				this._backList.Remove(widget);
			}
			this._isFragmented = true;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000FDDC File Offset: 0x0000DFDC
		public void Clear()
		{
			this._backList.Clear();
			this._frontList.Clear();
			this._backList = null;
			this._frontList = null;
			this._isFragmented = true;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000FE09 File Offset: 0x0000E009
		public MBReadOnlyList<Widget> GetActiveList()
		{
			return this._frontList;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000FE14 File Offset: 0x0000E014
		public void Defrag()
		{
			if (!this._isFragmented)
			{
				return;
			}
			this._frontList.Clear();
			HashSet<Widget> backList = this._backList;
			lock (backList)
			{
				foreach (Widget widget in this._backList)
				{
					this._frontList.Add(widget);
				}
			}
			this._isFragmented = false;
		}

		// Token: 0x040001EE RID: 494
		private HashSet<Widget> _backList;

		// Token: 0x040001EF RID: 495
		private MBList<Widget> _frontList;

		// Token: 0x040001F0 RID: 496
		private bool _isFragmented;

		// Token: 0x02000083 RID: 131
		internal enum ContainerType
		{
			// Token: 0x04000460 RID: 1120
			Update,
			// Token: 0x04000461 RID: 1121
			ParallelUpdate,
			// Token: 0x04000462 RID: 1122
			LateUpdate,
			// Token: 0x04000463 RID: 1123
			VisualDefinition,
			// Token: 0x04000464 RID: 1124
			UpdateBrushes
		}
	}
}
