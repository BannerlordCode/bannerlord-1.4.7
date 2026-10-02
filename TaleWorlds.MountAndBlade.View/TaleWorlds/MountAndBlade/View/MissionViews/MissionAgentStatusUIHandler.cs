using System;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200006C RID: 108
	public class MissionAgentStatusUIHandler : MissionBattleUIBaseView, IInteractionInterfaceHandler
	{
		// Token: 0x0600042C RID: 1068 RVA: 0x0001F599 File Offset: 0x0001D799
		public virtual void AddInteractionMessage(MissionInteractionItemBaseVM message)
		{
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001F59B File Offset: 0x0001D79B
		public virtual void RemoveInteractionMessage(MissionInteractionItemBaseVM message)
		{
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001F59D File Offset: 0x0001D79D
		public virtual bool HasInteractionMessage(MissionInteractionItemBaseVM message)
		{
			return false;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001F5A0 File Offset: 0x0001D7A0
		protected override void OnCreateView()
		{
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001F5A2 File Offset: 0x0001D7A2
		protected override void OnDestroyView()
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001F5A4 File Offset: 0x0001D7A4
		protected override void OnSuspendView()
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001F5A6 File Offset: 0x0001D7A6
		protected override void OnResumeView()
		{
		}
	}
}
