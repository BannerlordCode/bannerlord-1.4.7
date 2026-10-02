using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000027 RID: 39
	public static class User32
	{
		// Token: 0x0600011F RID: 287
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetAsyncKeyState(int vkey);

		// Token: 0x06000120 RID: 288
		[DllImport("user32.dll")]
		public static extern bool DestroyWindow(IntPtr hWnd);

		// Token: 0x06000121 RID: 289
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		// Token: 0x06000122 RID: 290
		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

		// Token: 0x06000123 RID: 291
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		// Token: 0x06000124 RID: 292
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

		// Token: 0x06000125 RID: 293
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPos(out Point lpPoint);

		// Token: 0x06000126 RID: 294
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReleaseCapture();

		// Token: 0x06000127 RID: 295
		[DllImport("user32.dll")]
		public static extern IntPtr SetCapture(IntPtr hWnd);

		// Token: 0x06000128 RID: 296
		[DllImport("user32.dll")]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		// Token: 0x06000129 RID: 297
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x0600012A RID: 298
		[DllImport("user32.dll")]
		public static extern IntPtr CreateWindowEx(int dwExStyle, [MarshalAs(UnmanagedType.LPTStr)] string lpClassName, string lpWindowName, WindowStyle dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		// Token: 0x0600012B RID: 299
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

		// Token: 0x0600012C RID: 300
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsIconic(IntPtr hWnd);

		// Token: 0x0600012D RID: 301
		[DllImport("user32.dll")]
		public static extern bool CloseWindow(IntPtr hWnd);

		// Token: 0x0600012E RID: 302
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PeekMessage(out NativeMessage lpMsg, [In] IntPtr hWnd, [In] uint wMsgFilterMin, [In] uint wMsgFilterMax, [In] uint wRemoveMsg);

		// Token: 0x0600012F RID: 303
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool TranslateMessage([In] ref NativeMessage lpMsg);

		// Token: 0x06000130 RID: 304
		[DllImport("user32.dll")]
		public static extern IntPtr DispatchMessage([In] ref NativeMessage lpMsg);

		// Token: 0x06000131 RID: 305
		[DllImport("user32.dll")]
		public static extern ushort RegisterClass([In] ref WindowClass lpWndClass);

		// Token: 0x06000132 RID: 306
		[DllImport("user32.dll")]
		public static extern bool UnregisterClass([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, IntPtr hInstance);

		// Token: 0x06000133 RID: 307
		[DllImport("user32.dll")]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000134 RID: 308
		[DllImport("user32.dll")]
		public static extern IntPtr LoadCursorFromFile(string lpFileName);

		// Token: 0x06000135 RID: 309
		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		// Token: 0x06000136 RID: 310
		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

		// Token: 0x06000137 RID: 311
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

		// Token: 0x06000138 RID: 312
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		// Token: 0x06000139 RID: 313
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		// Token: 0x0600013A RID: 314
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UpdateWindow(IntPtr hWnd);

		// Token: 0x0600013B RID: 315
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

		// Token: 0x0600013C RID: 316
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BlendFunction pblend, int dwFlags);

		// Token: 0x0600013D RID: 317
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

		// Token: 0x0600013E RID: 318
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600013F RID: 319
		[DllImport("user32.dll")]
		public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

		// Token: 0x06000140 RID: 320
		[DllImport("user32.dll")]
		public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, User32.MonitorEnumDelegate lpfnEnum, IntPtr dwData);

		// Token: 0x06000141 RID: 321
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(IntPtr hMonitor, ref User32.MONITORINFOEX lpmi);

		// Token: 0x0200004C RID: 76
		public struct RECT
		{
			// Token: 0x04000300 RID: 768
			public int left;

			// Token: 0x04000301 RID: 769
			public int top;

			// Token: 0x04000302 RID: 770
			public int right;

			// Token: 0x04000303 RID: 771
			public int bottom;
		}

		// Token: 0x0200004D RID: 77
		// (Invoke) Token: 0x060001AC RID: 428
		public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref User32.RECT lprcMonitor, IntPtr lParam);

		// Token: 0x0200004E RID: 78
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct MONITORINFOEX
		{
			// Token: 0x04000304 RID: 772
			public int cbSize;

			// Token: 0x04000305 RID: 773
			public User32.RECT rcMonitor;

			// Token: 0x04000306 RID: 774
			public User32.RECT rcWork;

			// Token: 0x04000307 RID: 775
			public uint dwFlags;

			// Token: 0x04000308 RID: 776
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szDevice;
		}
	}
}
