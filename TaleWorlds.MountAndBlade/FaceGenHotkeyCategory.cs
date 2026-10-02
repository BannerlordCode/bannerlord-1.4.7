using System;
using System.Linq;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000228 RID: 552
	public sealed class FaceGenHotkeyCategory : GameKeyContext
	{
		// Token: 0x060020B3 RID: 8371 RVA: 0x000731D3 File Offset: 0x000713D3
		public FaceGenHotkeyCategory()
			: base("FaceGenHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000731F8 File Offset: 0x000713F8
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("Ascend", "FaceGenHotkeyCategory", InputKey.MiddleMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Rotate", "FaceGenHotkeyCategory", InputKey.LeftMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Zoom", "FaceGenHotkeyCategory", InputKey.RightMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Copy", "FaceGenHotkeyCategory", InputKey.C, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Paste", "FaceGenHotkeyCategory", InputKey.V, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00073290 File Offset: 0x00071490
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(56, "ControllerZoomIn", "FaceGenHotkeyCategory", InputKey.Invalid, InputKey.ControllerRTrigger, ""), true);
			base.RegisterGameKey(new GameKey(57, "ControllerZoomOut", "FaceGenHotkeyCategory", InputKey.Invalid, InputKey.ControllerLTrigger, ""), true);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000732E4 File Offset: 0x000714E4
		private void RegisterGameAxisKeys()
		{
			GameAxisKey gameAxisKey = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisX"));
			GameAxisKey gameAxisKey2 = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisY"));
			base.RegisterGameAxisKey(gameAxisKey, true);
			base.RegisterGameAxisKey(gameAxisKey2, true);
		}

		// Token: 0x04000B7D RID: 2941
		public const string CategoryId = "FaceGenHotkeyCategory";

		// Token: 0x04000B7E RID: 2942
		public const string Zoom = "Zoom";

		// Token: 0x04000B7F RID: 2943
		public const string Rotate = "Rotate";

		// Token: 0x04000B80 RID: 2944
		public const string Ascend = "Ascend";

		// Token: 0x04000B81 RID: 2945
		public const string ControllerRotationAxis = "CameraAxisX";

		// Token: 0x04000B82 RID: 2946
		public const string ControllerCameraUpDownAxis = "CameraAxisY";

		// Token: 0x04000B83 RID: 2947
		public const string Copy = "Copy";

		// Token: 0x04000B84 RID: 2948
		public const string Paste = "Paste";

		// Token: 0x04000B85 RID: 2949
		public const int ControllerZoomIn = 56;

		// Token: 0x04000B86 RID: 2950
		public const int ControllerZoomOut = 57;
	}
}
