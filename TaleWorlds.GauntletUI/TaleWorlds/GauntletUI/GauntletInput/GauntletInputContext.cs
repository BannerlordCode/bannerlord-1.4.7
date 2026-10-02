using System;
using System.Numerics;
using TaleWorlds.InputSystem;

namespace TaleWorlds.GauntletUI.GauntletInput
{
	// Token: 0x0200004A RID: 74
	public class GauntletInputContext : IReadonlyInputContext
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x00011DCF File Offset: 0x0000FFCF
		public GauntletInputContext(IInputContext inputContext)
		{
			this._inputContext = inputContext;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00011DDE File Offset: 0x0000FFDE
		public bool GetIsMouseActive()
		{
			return this._inputContext.GetIsMouseActive();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00011DEB File Offset: 0x0000FFEB
		public Vector2 GetMousePosition()
		{
			if (this._isMousePositionOverridden)
			{
				return this._overrideMousePosition;
			}
			return this._inputContext.GetPointerPosition();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011E07 File Offset: 0x00010007
		public Vector2 GetMouseMovement()
		{
			return new Vector2(this._inputContext.GetMouseMoveX(), this._inputContext.GetMouseMoveY());
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011E24 File Offset: 0x00010024
		public InputKey[] GetClickKeys()
		{
			return Input.GetClickKeys();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011E2B File Offset: 0x0001002B
		public InputKey[] GetAlternateClickKeys()
		{
			return new InputKey[] { InputKey.RightMouseButton };
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00011E3B File Offset: 0x0001003B
		public float GetMouseScrollDelta()
		{
			return this._inputContext.GetDeltaMouseScroll();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00011E48 File Offset: 0x00010048
		public Vector2 GetControllerLeftStickState()
		{
			return (Vector2)this._inputContext.GetControllerLeftStickState();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00011E5A File Offset: 0x0001005A
		public Vector2 GetControllerRightStickState()
		{
			return (Vector2)this._inputContext.GetControllerRightStickState();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00011E6C File Offset: 0x0001006C
		public void SetMousePositionOverride(Vector2 mousePosition)
		{
			this._isMousePositionOverridden = true;
			this._overrideMousePosition = mousePosition;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00011E7C File Offset: 0x0001007C
		public void ResetMousePositionOverride()
		{
			this._isMousePositionOverridden = false;
		}

		// Token: 0x04000228 RID: 552
		private readonly IInputContext _inputContext;

		// Token: 0x04000229 RID: 553
		private bool _isMousePositionOverridden;

		// Token: 0x0400022A RID: 554
		private Vector2 _overrideMousePosition;
	}
}
