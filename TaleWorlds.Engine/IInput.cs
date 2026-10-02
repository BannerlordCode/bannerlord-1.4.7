using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003B RID: 59
	[ApplicationInterfaceBase]
	internal interface IInput
	{
		// Token: 0x0600061C RID: 1564
		[EngineMethod("clear_keys", false, null, false)]
		void ClearKeys();

		// Token: 0x0600061D RID: 1565
		[EngineMethod("get_mouse_sensitivity", false, null, false)]
		float GetMouseSensitivity();

		// Token: 0x0600061E RID: 1566
		[EngineMethod("get_mouse_delta_z", false, null, false)]
		float GetMouseDeltaZ();

		// Token: 0x0600061F RID: 1567
		[EngineMethod("is_mouse_active", false, null, false)]
		bool IsMouseActive();

		// Token: 0x06000620 RID: 1568
		[EngineMethod("is_controller_connected", false, null, false)]
		bool IsControllerConnected();

		// Token: 0x06000621 RID: 1569
		[EngineMethod("set_rumble_effect", false, null, false)]
		void SetRumbleEffect(float[] lowFrequencyLevels, float[] lowFrequencyDurations, int numLowFrequencyElements, float[] highFrequencyLevels, float[] highFrequencyDurations, int numHighFrequencyElements);

		// Token: 0x06000622 RID: 1570
		[EngineMethod("set_trigger_feedback", false, null, false)]
		void SetTriggerFeedback(byte leftTriggerPosition, byte leftTriggerStrength, byte rightTriggerPosition, byte rightTriggerStrength);

		// Token: 0x06000623 RID: 1571
		[EngineMethod("set_trigger_weapon_effect", false, null, false)]
		void SetTriggerWeaponEffect(byte leftStartPosition, byte leftEnd_position, byte leftStrength, byte rightStartPosition, byte rightEndPosition, byte rightStrength);

		// Token: 0x06000624 RID: 1572
		[EngineMethod("set_trigger_vibration", false, null, false)]
		void SetTriggerVibration(float[] leftTriggerAmplitudes, float[] leftTriggerFrequencies, float[] leftTriggerDurations, int numLeftTriggerElements, float[] rightTriggerAmplitudes, float[] rightTriggerFrequencies, float[] rightTriggerDurations, int numRightTriggerElements);

		// Token: 0x06000625 RID: 1573
		[EngineMethod("set_lightbar_color", false, null, false)]
		void SetLightbarColor(float red, float green, float blue);

		// Token: 0x06000626 RID: 1574
		[EngineMethod("press_key", false, null, false)]
		void PressKey(InputKey key);

		// Token: 0x06000627 RID: 1575
		[EngineMethod("get_virtual_key_code", false, null, false)]
		int GetVirtualKeyCode(InputKey key);

		// Token: 0x06000628 RID: 1576
		[EngineMethod("get_controller_type", false, null, false)]
		int GetControllerType();

		// Token: 0x06000629 RID: 1577
		[EngineMethod("set_clipboard_text", false, null, false)]
		void SetClipboardText(string text);

		// Token: 0x0600062A RID: 1578
		[EngineMethod("get_clipboard_text", false, null, false)]
		string GetClipboardText();

		// Token: 0x0600062B RID: 1579
		[EngineMethod("update_key_data", false, null, false)]
		void UpdateKeyData(byte[] keyData);

		// Token: 0x0600062C RID: 1580
		[EngineMethod("get_mouse_move_x", false, null, false)]
		float GetMouseMoveX();

		// Token: 0x0600062D RID: 1581
		[EngineMethod("get_mouse_move_y", false, null, false)]
		float GetMouseMoveY();

		// Token: 0x0600062E RID: 1582
		[EngineMethod("get_gyro_x", false, null, false)]
		float GetGyroX();

		// Token: 0x0600062F RID: 1583
		[EngineMethod("get_gyro_y", false, null, false)]
		float GetGyroY();

		// Token: 0x06000630 RID: 1584
		[EngineMethod("get_gyro_z", false, null, false)]
		float GetGyroZ();

		// Token: 0x06000631 RID: 1585
		[EngineMethod("get_mouse_position_x", false, null, false)]
		float GetMousePositionX();

		// Token: 0x06000632 RID: 1586
		[EngineMethod("get_mouse_position_y", false, null, false)]
		float GetMousePositionY();

		// Token: 0x06000633 RID: 1587
		[EngineMethod("get_mouse_scroll_value", false, null, false)]
		float GetMouseScrollValue();

		// Token: 0x06000634 RID: 1588
		[EngineMethod("get_key_state", false, null, false)]
		Vec2 GetKeyState(InputKey key);

		// Token: 0x06000635 RID: 1589
		[EngineMethod("is_key_down", false, null, true)]
		bool IsKeyDown(InputKey key);

		// Token: 0x06000636 RID: 1590
		[EngineMethod("is_key_down_immediate", false, null, false)]
		bool IsKeyDownImmediate(InputKey key);

		// Token: 0x06000637 RID: 1591
		[EngineMethod("is_key_pressed", false, null, true)]
		bool IsKeyPressed(InputKey key);

		// Token: 0x06000638 RID: 1592
		[EngineMethod("is_key_released", false, null, false)]
		bool IsKeyReleased(InputKey key);

		// Token: 0x06000639 RID: 1593
		[EngineMethod("is_any_touch_active", false, null, false)]
		bool IsAnyTouchActive();

		// Token: 0x0600063A RID: 1594
		[EngineMethod("set_cursor_position", false, null, false)]
		void SetCursorPosition(int x, int y);

		// Token: 0x0600063B RID: 1595
		[EngineMethod("set_cursor_friction_value", false, null, false)]
		void SetCursorFrictionValue(float frictionValue);
	}
}
