using System;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002F RID: 47
	public class SaveError
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000A361 File Offset: 0x00008561
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0000A369 File Offset: 0x00008569
		public string Message { get; private set; }

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A372 File Offset: 0x00008572
		internal SaveError(string message)
		{
			this.Message = message;
		}
	}
}
