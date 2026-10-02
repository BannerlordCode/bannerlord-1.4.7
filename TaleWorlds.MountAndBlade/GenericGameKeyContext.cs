using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022D RID: 557
	public sealed class GenericGameKeyContext : GameKeyContext
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060020C0 RID: 8384 RVA: 0x000735E8 File Offset: 0x000717E8
		// (set) Token: 0x060020C1 RID: 8385 RVA: 0x000735EF File Offset: 0x000717EF
		public static GenericGameKeyContext Current { get; private set; }

		// Token: 0x060020C2 RID: 8386 RVA: 0x000735F7 File Offset: 0x000717F7
		public GenericGameKeyContext()
			: base("Generic", 116, GameKeyContext.GameKeyContextType.Default)
		{
			GenericGameKeyContext.Current = this;
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x0007361F File Offset: 0x0007181F
		private void RegisterHotKeys()
		{
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00073624 File Offset: 0x00071824
		private void RegisterGameKeys()
		{
			GameKey gameKey = new GameKey(0, "Up", "Generic", InputKey.W, InputKey.ControllerLStickUp, GameKeyMainCategories.ActionCategory);
			GameKey gameKey2 = new GameKey(1, "Down", "Generic", InputKey.S, InputKey.ControllerLStickDown, GameKeyMainCategories.ActionCategory);
			GameKey gameKey3 = new GameKey(2, "Left", "Generic", InputKey.A, InputKey.ControllerLStickLeft, GameKeyMainCategories.ActionCategory);
			GameKey gameKey4 = new GameKey(3, "Right", "Generic", InputKey.D, InputKey.ControllerLStickRight, GameKeyMainCategories.ActionCategory);
			base.RegisterGameKey(gameKey, true);
			base.RegisterGameKey(gameKey2, true);
			base.RegisterGameKey(gameKey3, true);
			base.RegisterGameKey(gameKey4, true);
			base.RegisterGameKey(new GameKey(4, "Leave", "Generic", InputKey.Tab, InputKey.ControllerRRight, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(5, "ShowIndicators", "Generic", InputKey.LeftAlt, InputKey.ControllerLBumper, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameAxisKey(new GameAxisKey("MovementAxisX", InputKey.ControllerLStick, gameKey4, gameKey3, GameAxisKey.AxisType.X), true);
			base.RegisterGameAxisKey(new GameAxisKey("MovementAxisY", InputKey.ControllerLStick, gameKey, gameKey2, GameAxisKey.AxisType.Y), true);
			base.RegisterGameAxisKey(new GameAxisKey("CameraAxisX", InputKey.ControllerRStick, null, null, GameAxisKey.AxisType.X), true);
			base.RegisterGameAxisKey(new GameAxisKey("CameraAxisY", InputKey.ControllerRStick, null, null, GameAxisKey.AxisType.Y), true);
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x0007376F File Offset: 0x0007196F
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C13 RID: 3091
		public const string CategoryId = "Generic";

		// Token: 0x04000C14 RID: 3092
		public const int Up = 0;

		// Token: 0x04000C15 RID: 3093
		public const int Down = 1;

		// Token: 0x04000C16 RID: 3094
		public const int Right = 3;

		// Token: 0x04000C17 RID: 3095
		public const int Left = 2;

		// Token: 0x04000C18 RID: 3096
		public const string MovementAxisX = "MovementAxisX";

		// Token: 0x04000C19 RID: 3097
		public const string MovementAxisY = "MovementAxisY";

		// Token: 0x04000C1A RID: 3098
		public const string CameraAxisX = "CameraAxisX";

		// Token: 0x04000C1B RID: 3099
		public const string CameraAxisY = "CameraAxisY";

		// Token: 0x04000C1C RID: 3100
		public const int Leave = 4;

		// Token: 0x04000C1D RID: 3101
		public const int ShowIndicators = 5;
	}
}
