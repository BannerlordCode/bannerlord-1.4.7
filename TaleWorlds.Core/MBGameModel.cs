using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000AD RID: 173
	public abstract class MBGameModel<T> : GameModel where T : GameModel
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0001E0D6 File Offset: 0x0001C2D6
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0001E0DE File Offset: 0x0001C2DE
		private protected T BaseModel { protected get; private set; }

		// Token: 0x06000928 RID: 2344 RVA: 0x0001E0E7 File Offset: 0x0001C2E7
		public void Initialize(T baseModel)
		{
			this.BaseModel = baseModel;
		}
	}
}
