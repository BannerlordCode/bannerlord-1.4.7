using System;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x0200000E RID: 14
	public class MouseWidget : Widget
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x0000577B File Offset: 0x0000397B
		public MouseWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005784 File Offset: 0x00003984
		protected override void OnUpdate(float dt)
		{
			if (base.IsVisible)
			{
				this.UpdatePressedKeys();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005794 File Offset: 0x00003994
		public void UpdatePressedKeys()
		{
			Color color = new Color(1f, 0f, 0f, 1f);
			this.LeftMouseButton.Color = Color.White;
			this.RightMouseButton.Color = Color.White;
			this.MiddleMouseButton.Color = Color.White;
			this.MouseX1Button.Color = Color.White;
			this.MouseX2Button.Color = Color.White;
			this.MouseScrollUp.IsVisible = false;
			this.MouseScrollDown.IsVisible = false;
			this.KeyboardKeys.Text = "";
			if (Input.IsKeyDown(InputKey.LeftMouseButton))
			{
				this.LeftMouseButton.Color = color;
			}
			if (Input.IsKeyDown(InputKey.RightMouseButton))
			{
				this.RightMouseButton.Color = color;
			}
			if (Input.IsKeyDown(InputKey.MiddleMouseButton))
			{
				this.MiddleMouseButton.Color = color;
			}
			if (Input.IsKeyDown(InputKey.X1MouseButton))
			{
				this.MouseX1Button.Color = color;
			}
			if (Input.IsKeyDown(InputKey.X2MouseButton))
			{
				this.MouseX2Button.Color = color;
			}
			if (Input.IsKeyDown(InputKey.MouseScrollUp))
			{
				this.MouseScrollUp.IsVisible = true;
			}
			if (Input.IsKeyDown(InputKey.MouseScrollDown))
			{
				this.MouseScrollDown.IsVisible = true;
			}
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "UpdatePressedKeys");
			for (int i = 0; i < 256; i++)
			{
				if (Key.GetInputType((InputKey)i) == Key.InputType.Keyboard && Input.IsKeyDown((InputKey)i))
				{
					InputKey inputKey = (InputKey)i;
					mbstringBuilder.Append<string>(inputKey.ToString());
					mbstringBuilder.Append<string>(", ");
				}
			}
			this.KeyboardKeys.Text = mbstringBuilder.ToStringAndRelease().TrimEnd(MouseWidget._trimChars);
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00005952 File Offset: 0x00003B52
		// (set) Token: 0x060000CD RID: 205 RVA: 0x0000595A File Offset: 0x00003B5A
		public Widget LeftMouseButton { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00005963 File Offset: 0x00003B63
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000596B File Offset: 0x00003B6B
		public Widget RightMouseButton { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005974 File Offset: 0x00003B74
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x0000597C File Offset: 0x00003B7C
		public Widget MiddleMouseButton { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005985 File Offset: 0x00003B85
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x0000598D File Offset: 0x00003B8D
		public Widget MouseX1Button { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005996 File Offset: 0x00003B96
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000599E File Offset: 0x00003B9E
		public Widget MouseX2Button { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000059A7 File Offset: 0x00003BA7
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x000059AF File Offset: 0x00003BAF
		public Widget MouseScrollUp { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000059B8 File Offset: 0x00003BB8
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x000059C0 File Offset: 0x00003BC0
		public Widget MouseScrollDown { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000059C9 File Offset: 0x00003BC9
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000059D1 File Offset: 0x00003BD1
		public TextWidget KeyboardKeys { get; set; }

		// Token: 0x04000058 RID: 88
		private static readonly char[] _trimChars = new char[] { ' ', ',' };
	}
}
