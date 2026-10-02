using System;
using TaleWorlds.ScreenSystem;

namespace SandBox.View
{
	// Token: 0x02000008 RID: 8
	public abstract class SandboxView
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002A1E File Offset: 0x00000C1E
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002A26 File Offset: 0x00000C26
		public bool IsFinalized { get; protected set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002A2F File Offset: 0x00000C2F
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002A37 File Offset: 0x00000C37
		public ScreenLayer Layer { get; protected set; }

		// Token: 0x0600001F RID: 31 RVA: 0x00002A40 File Offset: 0x00000C40
		protected internal virtual void OnActivate()
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A42 File Offset: 0x00000C42
		protected internal virtual void OnDeactivate()
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002A44 File Offset: 0x00000C44
		protected internal virtual void OnInitialize()
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002A46 File Offset: 0x00000C46
		protected internal virtual void OnFinalize()
		{
			this.IsFinalized = true;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A4F File Offset: 0x00000C4F
		protected internal virtual void OnFrameTick(float dt)
		{
		}
	}
}
