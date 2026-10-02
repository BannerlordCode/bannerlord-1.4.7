using System;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x02000056 RID: 86
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GameStateScreen : Attribute
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00011F60 File Offset: 0x00010160
		// (set) Token: 0x060002BE RID: 702 RVA: 0x00011F68 File Offset: 0x00010168
		public Type GameStateType { get; private set; }

		// Token: 0x060002BF RID: 703 RVA: 0x00011F71 File Offset: 0x00010171
		public GameStateScreen(Type gameStateType)
		{
			this.GameStateType = gameStateType;
		}
	}
}
