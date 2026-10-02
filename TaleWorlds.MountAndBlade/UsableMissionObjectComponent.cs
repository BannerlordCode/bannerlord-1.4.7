using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036A RID: 874
	public abstract class UsableMissionObjectComponent
	{
		// Token: 0x06003232 RID: 12850 RVA: 0x000CD1B8 File Offset: 0x000CB3B8
		protected internal virtual void OnAdded(Scene scene)
		{
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x000CD1BA File Offset: 0x000CB3BA
		protected internal virtual void OnRemoved()
		{
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x000CD1BC File Offset: 0x000CB3BC
		protected internal virtual void OnFocusGain(Agent userAgent)
		{
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x000CD1BE File Offset: 0x000CB3BE
		protected internal virtual void OnFocusLose(Agent userAgent)
		{
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000CD1C0 File Offset: 0x000CB3C0
		public virtual bool IsOnTickRequired()
		{
			return false;
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x000CD1C3 File Offset: 0x000CB3C3
		protected internal virtual void OnTick(float dt)
		{
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x000CD1C5 File Offset: 0x000CB3C5
		protected internal virtual void OnEditorTick(float dt)
		{
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x000CD1C7 File Offset: 0x000CB3C7
		protected internal virtual void OnEditorValidate()
		{
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x000CD1C9 File Offset: 0x000CB3C9
		protected internal virtual void OnUse(Agent userAgent)
		{
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x000CD1CB File Offset: 0x000CB3CB
		protected internal virtual void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000CD1CD File Offset: 0x000CB3CD
		protected internal virtual void OnMissionReset()
		{
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000CD1CF File Offset: 0x000CB3CF
		protected internal virtual void OnMissionObjectDisabled()
		{
		}
	}
}
