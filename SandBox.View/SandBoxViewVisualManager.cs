using System;
using System.Collections.Generic;
using SandBox.View.Map;
using SandBox.View.Map.Visuals;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.View
{
	// Token: 0x0200000C RID: 12
	public class SandBoxViewVisualManager
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00003F2F File Offset: 0x0000212F
		public SandBoxViewVisualManager()
		{
			this._components = new EntitySystem<CampaignEntityVisualComponent>();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003F44 File Offset: 0x00002144
		public static void VisualTick(MapScreen screen, float realDt, float dt)
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnVisualTick(screen, realDt, dt);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003F9C File Offset: 0x0000219C
		public static void OnTick(float realDt, float dt)
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnTick(realDt, dt);
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003FF4 File Offset: 0x000021F4
		public static void ClearVisualMemory()
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.ClearVisualMemory();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004048 File Offset: 0x00002248
		public static void OnFrameTick(float dt)
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnFrameTick(dt);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000040A0 File Offset: 0x000022A0
		public static bool OnMouseClick(MapEntityVisual visualOfSelectedEntity, Vec3 intersectionPoint, PathFaceRecord mouseOverFaceIndex, bool isDoubleClick)
		{
			bool flag = false;
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				flag |= campaignEntityVisualComponent.OnMouseClick(visualOfSelectedEntity, intersectionPoint, mouseOverFaceIndex, isDoubleClick);
			}
			return flag;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004100 File Offset: 0x00002300
		public static void OnGameLoadFinished()
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnGameLoadFinished();
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004154 File Offset: 0x00002354
		public TComponent GetEntityComponent<TComponent>() where TComponent : CampaignEntityVisualComponent
		{
			EntitySystem<CampaignEntityVisualComponent> components = this._components;
			if (components == null)
			{
				return default(TComponent);
			}
			return components.GetComponent<TComponent>();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000417A File Offset: 0x0000237A
		public TComponent AddEntityComponent<TComponent>() where TComponent : CampaignEntityVisualComponent, new()
		{
			TComponent tcomponent = this._components.AddComponent<TComponent>();
			this.SortComponents();
			return tcomponent;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000418D File Offset: 0x0000238D
		public void RemoveEntityComponent<TComponent>() where TComponent : CampaignEntityVisualComponent
		{
			this._components.RemoveComponent<TComponent>();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000419A File Offset: 0x0000239A
		public void Finalize<TComponent>(TComponent component) where TComponent : CampaignEntityVisualComponent
		{
			this._components.Finalize(component);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000041AD File Offset: 0x000023AD
		public void RemoveEntityComponent<TComponent>(TComponent component) where TComponent : CampaignEntityVisualComponent
		{
			this._components.RemoveComponent(component);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000041C0 File Offset: 0x000023C0
		public List<TComponent> GetComponents<TComponent>() where TComponent : CampaignEntityVisualComponent
		{
			return this._components.GetComponents<TComponent>();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000041CD File Offset: 0x000023CD
		public MBList<CampaignEntityVisualComponent> GetComponents()
		{
			return this._components.GetComponents();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000041DA File Offset: 0x000023DA
		private void SortComponents()
		{
			this._components.SortComponents<CampaignEntityVisualComponent>(SandBoxViewVisualManager._comparisonDelegate);
		}

		// Token: 0x04000015 RID: 21
		private EntitySystem<CampaignEntityVisualComponent> _components;

		// Token: 0x04000016 RID: 22
		private static readonly Comparison<CampaignEntityVisualComponent> _comparisonDelegate = (CampaignEntityVisualComponent x, CampaignEntityVisualComponent y) => x.Priority.CompareTo(y.Priority);
	}
}
