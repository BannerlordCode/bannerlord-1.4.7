using System;
using SandBox.View.Map.Visuals;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map
{
	// Token: 0x0200003F RID: 63
	public class CampaignEntityVisualComponent : IEntityComponent
	{
		// Token: 0x06000206 RID: 518 RVA: 0x00013F73 File Offset: 0x00012173
		public virtual void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00013F75 File Offset: 0x00012175
		public virtual bool OnMouseClick(MapEntityVisual visualOfSelectedEntity, Vec3 intersectionPoint, PathFaceRecord mouseOverFaceIndex, bool isDoubleClick)
		{
			return false;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00013F78 File Offset: 0x00012178
		public virtual bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			return false;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00013F7B File Offset: 0x0001217B
		public virtual void OnFrameTick(float dt)
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00013F7D File Offset: 0x0001217D
		public virtual void OnGameLoadFinished()
		{
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00013F7F File Offset: 0x0001217F
		public virtual void OnTick(float realDt, float dt)
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00013F81 File Offset: 0x00012181
		public virtual void ClearVisualMemory()
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00013F83 File Offset: 0x00012183
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00013F8B File Offset: 0x0001218B
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00013F93 File Offset: 0x00012193
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00013F95 File Offset: 0x00012195
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00013F97 File Offset: 0x00012197
		public virtual int Priority
		{
			get
			{
				return 0;
			}
		}
	}
}
