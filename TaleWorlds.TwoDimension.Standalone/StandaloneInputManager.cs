using System;
using System.Drawing;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000E RID: 14
	public class StandaloneInputManager : IInputManager
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00004EDD File Offset: 0x000030DD
		public StandaloneInputManager(GraphicsForm graphicsForm)
		{
			this._graphicsForm = graphicsForm;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004EEC File Offset: 0x000030EC
		float IInputManager.GetMousePositionX()
		{
			return this._graphicsForm.MousePosition().X / (float)this._graphicsForm.Width;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004F0B File Offset: 0x0000310B
		float IInputManager.GetMousePositionY()
		{
			return this._graphicsForm.MousePosition().Y / (float)this._graphicsForm.Height;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004F2A File Offset: 0x0000312A
		float IInputManager.GetMouseScrollValue()
		{
			return 0f;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004F31 File Offset: 0x00003131
		bool IInputManager.IsMouseActive()
		{
			return true;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004F34 File Offset: 0x00003134
		bool IInputManager.IsAnyTouchActive()
		{
			return false;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004F37 File Offset: 0x00003137
		bool IInputManager.IsControllerConnected()
		{
			return false;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004F3A File Offset: 0x0000313A
		void IInputManager.PressKey(InputKey key)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004F3C File Offset: 0x0000313C
		void IInputManager.ClearKeys()
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004F3E File Offset: 0x0000313E
		int IInputManager.GetVirtualKeyCode(InputKey key)
		{
			return -1;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004F41 File Offset: 0x00003141
		void IInputManager.SetClipboardText(string text)
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004F43 File Offset: 0x00003143
		string IInputManager.GetClipboardText()
		{
			return "";
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004F4A File Offset: 0x0000314A
		float IInputManager.GetMouseMoveX()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004F51 File Offset: 0x00003151
		float IInputManager.GetMouseMoveY()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004F58 File Offset: 0x00003158
		float IInputManager.GetNormalizedMouseMoveX()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004F5F File Offset: 0x0000315F
		float IInputManager.GetNormalizedMouseMoveY()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004F66 File Offset: 0x00003166
		float IInputManager.GetGyroX()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004F6D File Offset: 0x0000316D
		float IInputManager.GetGyroY()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004F74 File Offset: 0x00003174
		float IInputManager.GetGyroZ()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004F7B File Offset: 0x0000317B
		float IInputManager.GetMouseSensitivity()
		{
			return 1f;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004F82 File Offset: 0x00003182
		float IInputManager.GetMouseDeltaZ()
		{
			return this._graphicsForm.GetMouseDeltaZ();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004F8F File Offset: 0x0000318F
		void IInputManager.UpdateKeyData(byte[] keyData)
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004F91 File Offset: 0x00003191
		Vec2 IInputManager.GetKeyState(InputKey key)
		{
			if (!this._graphicsForm.GetKey(key))
			{
				return new Vec2(0f, 0f);
			}
			return new Vec2(1f, 0f);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004FC0 File Offset: 0x000031C0
		bool IInputManager.IsKeyPressed(InputKey key)
		{
			return this._graphicsForm.GetKeyDown(key);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004FCE File Offset: 0x000031CE
		bool IInputManager.IsKeyDown(InputKey key)
		{
			return this._graphicsForm.GetKey(key);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004FDC File Offset: 0x000031DC
		bool IInputManager.IsKeyDownImmediate(InputKey key)
		{
			return this._graphicsForm.GetKey(key);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004FEA File Offset: 0x000031EA
		bool IInputManager.IsKeyReleased(InputKey key)
		{
			return this._graphicsForm.GetKeyUp(key);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004FF8 File Offset: 0x000031F8
		Vec2 IInputManager.GetResolution()
		{
			return new Vec2((float)this._graphicsForm.Width, (float)this._graphicsForm.Height);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005018 File Offset: 0x00003218
		Vec2 IInputManager.GetDesktopResolution()
		{
			Rectangle rectangle;
			User32.GetClientRect(User32.GetDesktopWindow(), out rectangle);
			return new Vec2((float)rectangle.Width, (float)rectangle.Height);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005047 File Offset: 0x00003247
		void IInputManager.SetCursorPosition(int x, int y)
		{
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005049 File Offset: 0x00003249
		void IInputManager.SetCursorFriction(float frictionValue)
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000504B File Offset: 0x0000324B
		InputKey[] IInputManager.GetClickKeys()
		{
			return new InputKey[]
			{
				InputKey.LeftMouseButton,
				InputKey.ControllerRDown
			};
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005063 File Offset: 0x00003263
		public void SetRumbleEffect(float[] lowFrequencyLevels, float[] lowFrequencyDurations, int numLowFrequencyElements, float[] highFrequencyLevels, float[] highFrequencyDurations, int numHighFrequencyElements)
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005065 File Offset: 0x00003265
		public void SetTriggerFeedback(byte leftTriggerPosition, byte leftTriggerStrength, byte rightTriggerPosition, byte rightTriggerStrength)
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005067 File Offset: 0x00003267
		public void SetTriggerWeaponEffect(byte leftStartPosition, byte leftEnd_position, byte leftStrength, byte rightStartPosition, byte rightEndPosition, byte rightStrength)
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005069 File Offset: 0x00003269
		public void SetTriggerVibration(float[] leftTriggerAmplitudes, float[] leftTriggerFrequencies, float[] leftTriggerDurations, int numLeftTriggerElements, float[] rightTriggerAmplitudes, float[] rightTriggerFrequencies, float[] rightTriggerDurations, int numRightTriggerElements)
		{
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000506B File Offset: 0x0000326B
		public void SetLightbarColor(float red, float green, float blue)
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000506D File Offset: 0x0000326D
		Input.ControllerTypes IInputManager.GetControllerType()
		{
			return Input.ControllerTypes.Xbox;
		}

		// Token: 0x04000043 RID: 67
		private GraphicsForm _graphicsForm;
	}
}
