using System;
using System.Drawing;
using System.Runtime.InteropServices;
using TaleWorlds.TwoDimension.Standalone.Native.OpenGL;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000009 RID: 9
	public class LayeredWindowController
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00004110 File Offset: 0x00002310
		public LayeredWindowController(IntPtr windowHandle, int width, int height)
		{
			this._windowHandle = windowHandle;
			User32.SetWindowLong(this._windowHandle, -20, 524288U);
			this._screenDC = User32.GetDC(IntPtr.Zero);
			this._memoryDC = Gdi32.CreateCompatibleDC(this._screenDC);
			this.SetSize(width, height);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004180 File Offset: 0x00002380
		private void CreateBitmapInfo()
		{
			BitmapInfoHeader bitmapInfoHeader;
			bitmapInfoHeader.biWidth = this._windowSize.Width;
			bitmapInfoHeader.biHeight = this._windowSize.Height;
			bitmapInfoHeader.biPlanes = 1;
			bitmapInfoHeader.biBitCount = 32;
			bitmapInfoHeader.biCompression = 0U;
			bitmapInfoHeader.biSizeImage = 0U;
			bitmapInfoHeader.biXPelsPerMeter = 0;
			bitmapInfoHeader.biYPelsPerMeter = 0;
			bitmapInfoHeader.biClrUsed = 0U;
			bitmapInfoHeader.biClrImportant = 0U;
			bitmapInfoHeader.biSize = (uint)Marshal.SizeOf(typeof(BitmapInfoHeader));
			this._bitmapInfo.bmiHeader = bitmapInfoHeader;
			this._bitmapInfo.r = 0;
			this._bitmapInfo.g = 0;
			this._bitmapInfo.b = 0;
			this._bitmapInfo.a = 0;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004244 File Offset: 0x00002444
		public void SetSize(int width, int height)
		{
			this._windowSize = new Size(width, height);
			if (this._windowSize.Width > 0 && this._windowSize.Height > 0)
			{
				this._pixelData = new byte[this._windowSize.Width * this._windowSize.Height * 4];
			}
			this.CreateBitmapInfo();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000042A4 File Offset: 0x000024A4
		public void PostRender()
		{
			if (this._windowSize.Width <= 0 || this._windowSize.Height <= 0)
			{
				return;
			}
			Opengl32.PixelStore(Target.PACK_ALIGNMENT, 1);
			Opengl32.ReadPixels(0, 0, this._windowSize.Width, this._windowSize.Height, PixelFormat.BGRA, DataType.UnsignedByte, this._pixelData);
			IntPtr intPtr = Gdi32.CreateCompatibleBitmap(this._screenDC, this._windowSize.Width, this._windowSize.Height);
			IntPtr intPtr2 = Gdi32.SelectObject(this._memoryDC, intPtr);
			Gdi32.StretchDIBits(this._memoryDC, 0, 0, this._windowSize.Width, this._windowSize.Height, 0, 0, this._windowSize.Width, this._windowSize.Height, this._pixelData, ref this._bitmapInfo, 0U, 13369376);
			Rectangle rectangle;
			User32.GetWindowRect(this._windowHandle, out rectangle);
			Point point = new Point(rectangle.Left, rectangle.Top);
			User32.UpdateLayeredWindow(this._windowHandle, this._screenDC, ref point, ref this._windowSize, this._memoryDC, ref this._localOriginPoint, 0, ref this._blendFunction, 2);
			if (intPtr != IntPtr.Zero)
			{
				Gdi32.SelectObject(this._memoryDC, intPtr2);
				Gdi32.DeleteObject(intPtr);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000043F5 File Offset: 0x000025F5
		public void OnFinalize()
		{
			User32.ReleaseDC(IntPtr.Zero, this._screenDC);
			Gdi32.DeleteDC(this._memoryDC);
		}

		// Token: 0x0400002F RID: 47
		private const int GwlExStyle = -20;

		// Token: 0x04000030 RID: 48
		private const uint WsExLayered = 524288U;

		// Token: 0x04000031 RID: 49
		private readonly IntPtr _windowHandle;

		// Token: 0x04000032 RID: 50
		private readonly IntPtr _screenDC;

		// Token: 0x04000033 RID: 51
		private readonly IntPtr _memoryDC;

		// Token: 0x04000034 RID: 52
		private Size _windowSize;

		// Token: 0x04000035 RID: 53
		private byte[] _pixelData;

		// Token: 0x04000036 RID: 54
		private BlendFunction _blendFunction = BlendFunction.Default;

		// Token: 0x04000037 RID: 55
		private Point _localOriginPoint = new Point(0, 0);

		// Token: 0x04000038 RID: 56
		private BitmapInfo _bitmapInfo;
	}
}
