using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000225 RID: 549
	public sealed class CombatHotKeyCategory : GameKeyContext
	{
		// Token: 0x060020A7 RID: 8359 RVA: 0x00072992 File Offset: 0x00070B92
		public CombatHotKeyCategory()
			: base("CombatHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000729B4 File Offset: 0x00070BB4
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("DeploymentCameraIsActive", "CombatHotKeyCategory", InputKey.MiddleMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ToggleZoom", "CombatHotKeyCategory", InputKey.ControllerRThumb, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon1", "CombatHotKeyCategory", InputKey.ControllerRRight, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon2", "CombatHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon3", "CombatHotKeyCategory", InputKey.ControllerRLeft, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropWeapon4", "CombatHotKeyCategory", InputKey.ControllerRDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerEquipDropExtraWeapon", "CombatHotKeyCategory", InputKey.ControllerRThumb, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkSelectFirstCategory", "CombatHotKeyCategory", new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRLeft)
			}, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkSelectSecondCategory", "CombatHotKeyCategory", new List<Key>
			{
				new Key(InputKey.RightMouseButton),
				new Key(InputKey.ControllerRRight)
			}, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkCloseMenu", "CombatHotKeyCategory", InputKey.ControllerRThumb, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem1", "CombatHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem2", "CombatHotKeyCategory", InputKey.ControllerRRight, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem3", "CombatHotKeyCategory", InputKey.ControllerRDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("CheerBarkItem4", "CombatHotKeyCategory", InputKey.ControllerRLeft, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ForfeitSpawn", "CombatHotKeyCategory", new List<Key>
			{
				new Key(InputKey.X),
				new Key(InputKey.ControllerRLeft)
			}, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControlModeToggle", "CombatHotKeyCategory", InputKey.ControllerLDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerToggleWalk", "CombatHotKeyCategory", InputKey.ControllerRUp, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ControllerToggleCrouch", "CombatHotKeyCategory", InputKey.ControllerRDown, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00072C28 File Offset: 0x00070E28
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(9, "Attack", "CombatHotKeyCategory", InputKey.LeftMouseButton, InputKey.ControllerRTrigger, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(10, "Defend", "CombatHotKeyCategory", InputKey.RightMouseButton, InputKey.ControllerLTrigger, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(11, "EquipPrimaryWeapon", "CombatHotKeyCategory", InputKey.MouseScrollUp, InputKey.Invalid, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(12, "EquipSecondaryWeapon", "CombatHotKeyCategory", InputKey.MouseScrollDown, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(13, "Action", "CombatHotKeyCategory", InputKey.F, InputKey.ControllerRUp, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(14, "Jump", "CombatHotKeyCategory", InputKey.Space, InputKey.ControllerRDown, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(15, "Crouch", "CombatHotKeyCategory", InputKey.Z, InputKey.ControllerLDown, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(16, "Kick", "CombatHotKeyCategory", InputKey.E, InputKey.ControllerRLeft, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(17, "ToggleWeaponMode", "CombatHotKeyCategory", InputKey.X, InputKey.Invalid, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(18, "EquipWeapon1", "CombatHotKeyCategory", InputKey.Numpad1, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(19, "EquipWeapon2", "CombatHotKeyCategory", InputKey.Numpad2, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(20, "EquipWeapon3", "CombatHotKeyCategory", InputKey.Numpad3, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(21, "EquipWeapon4", "CombatHotKeyCategory", InputKey.Numpad4, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(22, "DropWeapon", "CombatHotKeyCategory", InputKey.G, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(23, "SheathWeapon", "CombatHotKeyCategory", InputKey.BackSlash, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(24, "Zoom", "CombatHotKeyCategory", InputKey.LeftShift, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(25, "ViewCharacter", "CombatHotKeyCategory", InputKey.Tilde, InputKey.ControllerLLeft, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(26, "LockTarget", "CombatHotKeyCategory", InputKey.MiddleMouseButton, InputKey.ControllerRThumb, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(27, "CameraToggle", "CombatHotKeyCategory", InputKey.R, InputKey.ControllerLThumb, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(28, "MissionScreenHotkeyCameraZoomIn", "CombatHotKeyCategory", InputKey.NumpadPlus, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(29, "MissionScreenHotkeyCameraZoomOut", "CombatHotKeyCategory", InputKey.NumpadMinus, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(30, "ToggleWalkMode", "CombatHotKeyCategory", InputKey.CapsLock, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(31, "Cheer", "CombatHotKeyCategory", InputKey.O, InputKey.ControllerLUp, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(33, "PushToTalk", "CombatHotKeyCategory", InputKey.V, InputKey.ControllerLRight, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(34, "EquipmentSwitch", "CombatHotKeyCategory", InputKey.U, InputKey.ControllerRBumper, GameKeyMainCategories.ActionCategory), true);
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00072F89 File Offset: 0x00071189
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000B43 RID: 2883
		public const string CategoryId = "CombatHotKeyCategory";

		// Token: 0x04000B44 RID: 2884
		public const int MissionScreenHotkeyCameraZoomIn = 28;

		// Token: 0x04000B45 RID: 2885
		public const int MissionScreenHotkeyCameraZoomOut = 29;

		// Token: 0x04000B46 RID: 2886
		public const int Action = 13;

		// Token: 0x04000B47 RID: 2887
		public const int Jump = 14;

		// Token: 0x04000B48 RID: 2888
		public const int Crouch = 15;

		// Token: 0x04000B49 RID: 2889
		public const int Attack = 9;

		// Token: 0x04000B4A RID: 2890
		public const int Defend = 10;

		// Token: 0x04000B4B RID: 2891
		public const int Kick = 16;

		// Token: 0x04000B4C RID: 2892
		public const int ToggleWeaponMode = 17;

		// Token: 0x04000B4D RID: 2893
		public const int ToggleWalkMode = 30;

		// Token: 0x04000B4E RID: 2894
		public const int EquipWeapon1 = 18;

		// Token: 0x04000B4F RID: 2895
		public const int EquipWeapon2 = 19;

		// Token: 0x04000B50 RID: 2896
		public const int EquipWeapon3 = 20;

		// Token: 0x04000B51 RID: 2897
		public const int EquipWeapon4 = 21;

		// Token: 0x04000B52 RID: 2898
		public const int EquipPrimaryWeapon = 11;

		// Token: 0x04000B53 RID: 2899
		public const int EquipSecondaryWeapon = 12;

		// Token: 0x04000B54 RID: 2900
		public const int DropWeapon = 22;

		// Token: 0x04000B55 RID: 2901
		public const int SheathWeapon = 23;

		// Token: 0x04000B56 RID: 2902
		public const int Zoom = 24;

		// Token: 0x04000B57 RID: 2903
		public const int ViewCharacter = 25;

		// Token: 0x04000B58 RID: 2904
		public const int LockTarget = 26;

		// Token: 0x04000B59 RID: 2905
		public const int CameraToggle = 27;

		// Token: 0x04000B5A RID: 2906
		public const int Cheer = 31;

		// Token: 0x04000B5B RID: 2907
		public const int PushToTalk = 33;

		// Token: 0x04000B5C RID: 2908
		public const int EquipmentSwitch = 34;

		// Token: 0x04000B5D RID: 2909
		public const string DeploymentCameraIsActive = "DeploymentCameraIsActive";

		// Token: 0x04000B5E RID: 2910
		public const string ToggleZoom = "ToggleZoom";

		// Token: 0x04000B5F RID: 2911
		public const string ControllerEquipDropRRight = "ControllerEquipDropWeapon1";

		// Token: 0x04000B60 RID: 2912
		public const string ControllerEquipDropRUp = "ControllerEquipDropWeapon2";

		// Token: 0x04000B61 RID: 2913
		public const string ControllerEquipDropRLeft = "ControllerEquipDropWeapon3";

		// Token: 0x04000B62 RID: 2914
		public const string ControllerEquipDropRDown = "ControllerEquipDropWeapon4";

		// Token: 0x04000B63 RID: 2915
		public const string ControllerEquipDropRThumb = "ControllerEquipDropExtraWeapon";

		// Token: 0x04000B64 RID: 2916
		public const string CheerBarkSelectFirstCategory = "CheerBarkSelectFirstCategory";

		// Token: 0x04000B65 RID: 2917
		public const string CheerBarkSelectSecondCategory = "CheerBarkSelectSecondCategory";

		// Token: 0x04000B66 RID: 2918
		public const string CheerBarkCloseMenu = "CheerBarkCloseMenu";

		// Token: 0x04000B67 RID: 2919
		public const string CheerBarkItem1 = "CheerBarkItem1";

		// Token: 0x04000B68 RID: 2920
		public const string CheerBarkItem2 = "CheerBarkItem2";

		// Token: 0x04000B69 RID: 2921
		public const string CheerBarkItem3 = "CheerBarkItem3";

		// Token: 0x04000B6A RID: 2922
		public const string CheerBarkItem4 = "CheerBarkItem4";

		// Token: 0x04000B6B RID: 2923
		public const string ControlModeToggle = "ControlModeToggle";

		// Token: 0x04000B6C RID: 2924
		public const string ControllerToggleWalk = "ControllerToggleWalk";

		// Token: 0x04000B6D RID: 2925
		public const string ControllerToggleCrouch = "ControllerToggleCrouch";

		// Token: 0x04000B6E RID: 2926
		public const string ForfeitSpawn = "ForfeitSpawn";
	}
}
