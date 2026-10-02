using System;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction
{
	// Token: 0x0200003F RID: 63
	public interface IInteractionInterfaceHandler
	{
		// Token: 0x06000595 RID: 1429
		void AddInteractionMessage(MissionInteractionItemBaseVM message);

		// Token: 0x06000596 RID: 1430
		void RemoveInteractionMessage(MissionInteractionItemBaseVM message);

		// Token: 0x06000597 RID: 1431
		bool HasInteractionMessage(MissionInteractionItemBaseVM message);
	}
}
