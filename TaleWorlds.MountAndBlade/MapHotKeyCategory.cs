using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000230 RID: 560
	public sealed class MapHotKeyCategory : GameKeyContext
	{
		// Token: 0x060020D0 RID: 8400 RVA: 0x00073B1B File Offset: 0x00071D1B
		public MapHotKeyCategory()
			: base("MapHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00073B40 File Offset: 0x00071D40
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			base.RegisterHotKey(new HotKey("MapClick", "MapHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.ControllerLOptionTap)
			};
			base.RegisterHotKey(new HotKey("MapTouchpadClick", "MapHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.LeftAlt),
				new Key(InputKey.ControllerLBumper)
			};
			base.RegisterHotKey(new HotKey("MapFollowModifier", "MapHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.ControllerRRight)
			};
			base.RegisterHotKey(new HotKey("MapChangeCursorMode", "MapHotKeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x00073C28 File Offset: 0x00071E28
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(50, "PartyMoveUp", "MapHotKeyCategory", InputKey.Up, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(51, "PartyMoveDown", "MapHotKeyCategory", InputKey.Down, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(52, "PartyMoveLeft", "MapHotKeyCategory", InputKey.Left, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(53, "PartyMoveRight", "MapHotKeyCategory", InputKey.Right, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(54, "QuickSave", "MapHotKeyCategory", InputKey.F5, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(55, "MapFastMove", "MapHotKeyCategory", InputKey.LeftShift, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(56, "MapZoomIn", "MapHotKeyCategory", InputKey.MouseScrollUp, InputKey.ControllerRTrigger, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(57, "MapZoomOut", "MapHotKeyCategory", InputKey.MouseScrollDown, InputKey.ControllerLTrigger, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(58, "MapRotateLeft", "MapHotKeyCategory", InputKey.Q, InputKey.Invalid, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(59, "MapRotateRight", "MapHotKeyCategory", InputKey.E, InputKey.Invalid, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(60, "MapTimeStop", "MapHotKeyCategory", InputKey.D1, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(61, "MapTimeNormal", "MapHotKeyCategory", InputKey.D2, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(62, "MapTimeFastForward", "MapHotKeyCategory", InputKey.D3, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(63, "MapTimeTogglePause", "MapHotKeyCategory", InputKey.Space, InputKey.ControllerRLeft, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(64, "MapCameraFollowMode", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerLThumb, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(65, "MapToggleFastForward", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerRBumper, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(66, "MapTrackSettlement", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerRThumb, GameKeyMainCategories.CampaignMapCategory), true);
			base.RegisterGameKey(new GameKey(67, "MapGoToEncylopedia", "MapHotKeyCategory", InputKey.Invalid, InputKey.ControllerLOption, GameKeyMainCategories.CampaignMapCategory), true);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00073E94 File Offset: 0x00072094
		private void RegisterGameAxisKeys()
		{
			GameKey gameKey = new GameKey(46, "MapMoveUp", "MapHotKeyCategory", InputKey.W, GameKeyMainCategories.CampaignMapCategory);
			GameKey gameKey2 = new GameKey(47, "MapMoveDown", "MapHotKeyCategory", InputKey.S, GameKeyMainCategories.CampaignMapCategory);
			GameKey gameKey3 = new GameKey(48, "MapMoveLeft", "MapHotKeyCategory", InputKey.A, GameKeyMainCategories.CampaignMapCategory);
			GameKey gameKey4 = new GameKey(49, "MapMoveRight", "MapHotKeyCategory", InputKey.D, GameKeyMainCategories.CampaignMapCategory);
			base.RegisterGameKey(gameKey, true);
			base.RegisterGameKey(gameKey2, true);
			base.RegisterGameKey(gameKey3, true);
			base.RegisterGameKey(gameKey4, true);
			base.RegisterGameAxisKey(new GameAxisKey("MapMovementAxisX", InputKey.ControllerLStick, gameKey4, gameKey3, GameAxisKey.AxisType.X), true);
			base.RegisterGameAxisKey(new GameAxisKey("MapMovementAxisY", InputKey.ControllerLStick, gameKey, gameKey2, GameAxisKey.AxisType.Y), true);
		}

		// Token: 0x04000C30 RID: 3120
		public const string CategoryId = "MapHotKeyCategory";

		// Token: 0x04000C31 RID: 3121
		public const int QuickSave = 54;

		// Token: 0x04000C32 RID: 3122
		public const int PartyMoveUp = 50;

		// Token: 0x04000C33 RID: 3123
		public const int PartyMoveLeft = 52;

		// Token: 0x04000C34 RID: 3124
		public const int PartyMoveDown = 51;

		// Token: 0x04000C35 RID: 3125
		public const int PartyMoveRight = 53;

		// Token: 0x04000C36 RID: 3126
		public const int MapMoveUp = 46;

		// Token: 0x04000C37 RID: 3127
		public const int MapMoveDown = 47;

		// Token: 0x04000C38 RID: 3128
		public const int MapMoveLeft = 48;

		// Token: 0x04000C39 RID: 3129
		public const int MapMoveRight = 49;

		// Token: 0x04000C3A RID: 3130
		public const string MovementAxisX = "MapMovementAxisX";

		// Token: 0x04000C3B RID: 3131
		public const string MovementAxisY = "MapMovementAxisY";

		// Token: 0x04000C3C RID: 3132
		public const int MapFastMove = 55;

		// Token: 0x04000C3D RID: 3133
		public const int MapZoomIn = 56;

		// Token: 0x04000C3E RID: 3134
		public const int MapZoomOut = 57;

		// Token: 0x04000C3F RID: 3135
		public const int MapRotateLeft = 58;

		// Token: 0x04000C40 RID: 3136
		public const int MapRotateRight = 59;

		// Token: 0x04000C41 RID: 3137
		public const int MapCameraFollowMode = 64;

		// Token: 0x04000C42 RID: 3138
		public const int MapToggleFastForward = 65;

		// Token: 0x04000C43 RID: 3139
		public const int MapTrackSettlement = 66;

		// Token: 0x04000C44 RID: 3140
		public const int MapGoToEncylopedia = 67;

		// Token: 0x04000C45 RID: 3141
		public const string MapClick = "MapClick";

		// Token: 0x04000C46 RID: 3142
		public const string MapTouchpadClick = "MapTouchpadClick";

		// Token: 0x04000C47 RID: 3143
		public const string MapFollowModifier = "MapFollowModifier";

		// Token: 0x04000C48 RID: 3144
		public const string MapChangeCursorMode = "MapChangeCursorMode";

		// Token: 0x04000C49 RID: 3145
		public const int MapTimeStop = 60;

		// Token: 0x04000C4A RID: 3146
		public const int MapTimeNormal = 61;

		// Token: 0x04000C4B RID: 3147
		public const int MapTimeFastForward = 62;

		// Token: 0x04000C4C RID: 3148
		public const int MapTimeTogglePause = 63;
	}
}
