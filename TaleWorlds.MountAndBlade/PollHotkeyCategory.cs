using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000235 RID: 565
	public sealed class PollHotkeyCategory : GameKeyContext
	{
		// Token: 0x060020E2 RID: 8418 RVA: 0x00074655 File Offset: 0x00072855
		public PollHotkeyCategory()
			: base("PollHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterGameKeys();
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x0007466C File Offset: 0x0007286C
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(108, "AcceptPoll", "PollHotkeyCategory", InputKey.F10, InputKey.ControllerLBumper, GameKeyMainCategories.PollCategory), true);
			base.RegisterGameKey(new GameKey(109, "DeclinePoll", "PollHotkeyCategory", InputKey.F11, InputKey.ControllerRBumper, GameKeyMainCategories.PollCategory), true);
		}

		// Token: 0x04000C8A RID: 3210
		public const string CategoryId = "PollHotkeyCategory";

		// Token: 0x04000C8B RID: 3211
		public const int AcceptPoll = 108;

		// Token: 0x04000C8C RID: 3212
		public const int DeclinePoll = 109;
	}
}
