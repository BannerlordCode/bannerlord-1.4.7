using System;
using SandBox.View.Map.Visuals;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000072 RID: 114
	public abstract class EntityVisualManagerBase<TEntity> : EntityVisualManagerBase
	{
		// Token: 0x060004E8 RID: 1256
		public abstract MapEntityVisual<TEntity> GetVisualOfEntity(TEntity entity);

		// Token: 0x060004E9 RID: 1257 RVA: 0x00025AAA File Offset: 0x00023CAA
		public static EntityVisualManagerBase<TEntity> GetEntityVisualManagerBase()
		{
			return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<EntityVisualManagerBase<TEntity>>();
		}
	}
}
