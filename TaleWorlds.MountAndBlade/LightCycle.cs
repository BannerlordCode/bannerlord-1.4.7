using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000333 RID: 819
	public class LightCycle : ScriptComponentBehavior
	{
		// Token: 0x06002E0F RID: 11791 RVA: 0x000B1C10 File Offset: 0x000AFE10
		private void SetVisibility()
		{
			Light light = base.GameEntity.GetLight();
			float timeOfDay = base.Scene.TimeOfDay;
			this.visibility = timeOfDay < 6f || timeOfDay > 20f || base.Scene.IsAtmosphereIndoor || this.alwaysBurn;
			if (light != null)
			{
				light.SetVisibility(this.visibility);
			}
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				weakGameEntity.SetVisibilityExcludeParents(this.visibility);
			}
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000B1CC8 File Offset: 0x000AFEC8
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetVisibility();
			if (!this.visibility)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					base.Scene.RemoveEntity(list[i], 0);
				}
				base.GameEntity.RemoveScriptComponent(base.ScriptComponent.Pointer, 0);
			}
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000B1D3F File Offset: 0x000AFF3F
		protected internal override void OnEditorTick(float dt)
		{
			this.SetVisibility();
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000B1D47 File Offset: 0x000AFF47
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x04001239 RID: 4665
		public bool alwaysBurn;

		// Token: 0x0400123A RID: 4666
		private bool visibility;
	}
}
