using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Source.Objects
{
	// Token: 0x020003CB RID: 971
	public class NavigationMeshDeactivator : ScriptComponentBehavior
	{
		// Token: 0x06003627 RID: 13863 RVA: 0x000DF8BF File Offset: 0x000DDABF
		public void DisableAssignedFaces(Scene scene)
		{
			scene.SetAbilityOfFacesWithId(this.DisableFaceWithId, false);
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000DF8CF File Offset: 0x000DDACF
		public void EnableAssignedFaces(Scene scene)
		{
			scene.SetAbilityOfFacesWithId(this.DisableFaceWithId, true);
		}

		// Token: 0x04001739 RID: 5945
		public int DisableFaceWithId = -1;

		// Token: 0x0400173A RID: 5946
		public int DisableFaceWithIdForAnimals = -1;
	}
}
