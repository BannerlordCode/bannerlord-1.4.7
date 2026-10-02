using System;
using System.Linq;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000227 RID: 551
	public sealed class CraftingHotkeyCategory : GameKeyContext
	{
		// Token: 0x060020AF RID: 8367 RVA: 0x00073049 File Offset: 0x00071249
		public CraftingHotkeyCategory()
			: base("CraftingHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x0007306C File Offset: 0x0007126C
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("Ascend", "CraftingHotkeyCategory", InputKey.MiddleMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Rotate", "CraftingHotkeyCategory", InputKey.LeftMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Zoom", "CraftingHotkeyCategory", InputKey.RightMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Copy", "CraftingHotkeyCategory", InputKey.C, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Paste", "CraftingHotkeyCategory", InputKey.V, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00073104 File Offset: 0x00071304
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(56, "ControllerZoomIn", "CraftingHotkeyCategory", InputKey.Invalid, InputKey.ControllerRTrigger, ""), true);
			base.RegisterGameKey(new GameKey(57, "ControllerZoomOut", "CraftingHotkeyCategory", InputKey.Invalid, InputKey.ControllerLTrigger, ""), true);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00073158 File Offset: 0x00071358
		private void RegisterGameAxisKeys()
		{
			GameAxisKey gameAxisKey = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisX"));
			GameAxisKey gameAxisKey2 = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisY"));
			base.RegisterGameAxisKey(gameAxisKey, true);
			base.RegisterGameAxisKey(gameAxisKey2, true);
		}

		// Token: 0x04000B72 RID: 2930
		public const string CategoryId = "CraftingHotkeyCategory";

		// Token: 0x04000B73 RID: 2931
		public const string Zoom = "Zoom";

		// Token: 0x04000B74 RID: 2932
		public const string Rotate = "Rotate";

		// Token: 0x04000B75 RID: 2933
		public const string Ascend = "Ascend";

		// Token: 0x04000B76 RID: 2934
		public const string ResetCamera = "ResetCamera";

		// Token: 0x04000B77 RID: 2935
		public const string Copy = "Copy";

		// Token: 0x04000B78 RID: 2936
		public const string Paste = "Paste";

		// Token: 0x04000B79 RID: 2937
		public const string ControllerRotationAxisX = "CameraAxisX";

		// Token: 0x04000B7A RID: 2938
		public const string ControllerRotationAxisY = "CameraAxisY";

		// Token: 0x04000B7B RID: 2939
		public const int ControllerZoomIn = 56;

		// Token: 0x04000B7C RID: 2940
		public const int ControllerZoomOut = 57;
	}
}
