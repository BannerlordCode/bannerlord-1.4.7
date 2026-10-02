using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GameKeyCategory
{
	// Token: 0x020003F2 RID: 1010
	public sealed class PhotoModeHotKeyCategory : GameKeyContext
	{
		// Token: 0x06003740 RID: 14144 RVA: 0x000E4A93 File Offset: 0x000E2C93
		public PhotoModeHotKeyCategory()
			: base("PhotoModeHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000E4AB8 File Offset: 0x000E2CB8
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftShift),
				new Key(InputKey.ControllerRTrigger)
			};
			base.RegisterHotKey(new HotKey("FasterCamera", "PhotoModeHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x000E4B04 File Offset: 0x000E2D04
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(92, "HideUI", "PhotoModeHotKeyCategory", InputKey.H, InputKey.ControllerRUp, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(93, "CameraRollLeft", "PhotoModeHotKeyCategory", InputKey.Q, InputKey.ControllerLBumper, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(94, "CameraRollRight", "PhotoModeHotKeyCategory", InputKey.E, InputKey.ControllerRBumper, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(97, "ToggleCameraFollowMode", "PhotoModeHotKeyCategory", InputKey.V, InputKey.ControllerRLeft, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(95, "TakePicture", "PhotoModeHotKeyCategory", InputKey.Enter, InputKey.ControllerRDown, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(96, "TakePictureWithAdditionalPasses", "PhotoModeHotKeyCategory", InputKey.BackSpace, InputKey.ControllerRBumper, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(98, "ToggleMouse", "PhotoModeHotKeyCategory", InputKey.C, InputKey.ControllerLThumb, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(99, "ToggleVignette", "PhotoModeHotKeyCategory", InputKey.X, InputKey.ControllerRThumb, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(100, "ToggleCharacters", "PhotoModeHotKeyCategory", InputKey.B, InputKey.ControllerRRight, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(107, "Reset", "PhotoModeHotKeyCategory", InputKey.T, InputKey.ControllerLOption, GameKeyMainCategories.PhotoModeCategory), true);
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000E4C79 File Offset: 0x000E2E79
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x040017BF RID: 6079
		public const string CategoryId = "PhotoModeHotKeyCategory";

		// Token: 0x040017C0 RID: 6080
		public const int HideUI = 92;

		// Token: 0x040017C1 RID: 6081
		public const int CameraRollLeft = 93;

		// Token: 0x040017C2 RID: 6082
		public const int CameraRollRight = 94;

		// Token: 0x040017C3 RID: 6083
		public const int ToggleCameraFollowMode = 97;

		// Token: 0x040017C4 RID: 6084
		public const int TakePicture = 95;

		// Token: 0x040017C5 RID: 6085
		public const int TakePictureWithAdditionalPasses = 96;

		// Token: 0x040017C6 RID: 6086
		public const int ToggleMouse = 98;

		// Token: 0x040017C7 RID: 6087
		public const int ToggleVignette = 99;

		// Token: 0x040017C8 RID: 6088
		public const int ToggleCharacters = 100;

		// Token: 0x040017C9 RID: 6089
		public const int Reset = 107;

		// Token: 0x040017CA RID: 6090
		public const string FasterCamera = "FasterCamera";
	}
}
