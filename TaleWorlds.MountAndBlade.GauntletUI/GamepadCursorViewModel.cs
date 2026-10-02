using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000011 RID: 17
	public class GamepadCursorViewModel : ViewModel
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004F44 File Offset: 0x00003144
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00004F4C File Offset: 0x0000314C
		[DataSourceProperty]
		public bool IsConsoleMouseVisible
		{
			get
			{
				return this._isConsoleMouseVisible;
			}
			set
			{
				if (this._isConsoleMouseVisible != value)
				{
					this._isConsoleMouseVisible = value;
					base.OnPropertyChangedWithValue(value, "IsConsoleMouseVisible");
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004F6A File Offset: 0x0000316A
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004F72 File Offset: 0x00003172
		[DataSourceProperty]
		public bool IsGamepadCursorVisible
		{
			get
			{
				return this._isGamepadCursorVisible;
			}
			set
			{
				if (this._isGamepadCursorVisible != value)
				{
					this._isGamepadCursorVisible = value;
					base.OnPropertyChangedWithValue(value, "IsGamepadCursorVisible");
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004F90 File Offset: 0x00003190
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004F98 File Offset: 0x00003198
		[DataSourceProperty]
		public float CursorPositionX
		{
			get
			{
				return this._cursorPositionX;
			}
			set
			{
				if (this._cursorPositionX != value)
				{
					this._cursorPositionX = value;
					base.OnPropertyChangedWithValue(value, "CursorPositionX");
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004FB6 File Offset: 0x000031B6
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004FBE File Offset: 0x000031BE
		[DataSourceProperty]
		public float CursorPositionY
		{
			get
			{
				return this._cursorPositionY;
			}
			set
			{
				if (this._cursorPositionY != value)
				{
					this._cursorPositionY = value;
					base.OnPropertyChangedWithValue(value, "CursorPositionY");
				}
			}
		}

		// Token: 0x0400005C RID: 92
		private float _cursorPositionX = 960f;

		// Token: 0x0400005D RID: 93
		private float _cursorPositionY = 540f;

		// Token: 0x0400005E RID: 94
		private bool _isConsoleMouseVisible;

		// Token: 0x0400005F RID: 95
		private bool _isGamepadCursorVisible;
	}
}
