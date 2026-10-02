using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200024E RID: 590
	internal class OnSessionInvitationAcceptedJob : Job
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x00075A54 File Offset: 0x00073C54
		public OnSessionInvitationAcceptedJob(SessionInvitationType sessionInvitationType)
		{
			this._sessionInvitationType = sessionInvitationType;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x00075A64 File Offset: 0x00073C64
		public override void DoJob(float dt)
		{
			base.DoJob(dt);
			if (MBGameManager.Current != null)
			{
				MBGameManager.Current.OnSessionInvitationAccepted(this._sessionInvitationType);
			}
			else if (GameStateManager.Current != null && GameStateManager.Current.ActiveState != null)
			{
				GameStateManager.Current.CleanStates(0);
			}
			base.Finished = true;
		}

		// Token: 0x04000CE9 RID: 3305
		private readonly SessionInvitationType _sessionInvitationType;
	}
}
