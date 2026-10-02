using System;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000061 RID: 97
	public abstract class MapEntityVisual<T> : MapEntityVisual
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001DFC5 File Offset: 0x0001C1C5
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0001DFCD File Offset: 0x0001C1CD
		public T MapEntity { get; private set; }

		// Token: 0x060003DC RID: 988 RVA: 0x0001DFD6 File Offset: 0x0001C1D6
		public MapEntityVisual(T entity)
		{
			this.MapEntity = entity;
		}
	}
}
