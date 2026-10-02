using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000006 RID: 6
	public class GraphicsForm : IMessageCommunicator
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003275 File Offset: 0x00001475
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000327D File Offset: 0x0000147D
		public GraphicsContext GraphicsContext { get; private set; }

		// Token: 0x06000033 RID: 51 RVA: 0x00003288 File Offset: 0x00001488
		public GraphicsForm(int width, int height, ResourceDepot resourceDepot, bool borderlessWindow = false, bool enableWindowBlur = false, bool layeredWindow = false, string name = null)
		{
			DXGI.RECT rect = this.DecideWindowPosition();
			int num = rect.right - rect.left;
			int num2 = rect.bottom - rect.top;
			int num3 = rect.left + (num - width) / 2;
			int num4 = rect.top + (num2 - height) / 2;
			this._windowsForm = new WindowsForm(num3, num4, width, height, resourceDepot, borderlessWindow, enableWindowBlur, name);
			this.Initalize(layeredWindow);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000330C File Offset: 0x0000150C
		public GraphicsForm(int x, int y, int width, int height, ResourceDepot resourceDepot, bool borderlessWindow = false, bool enableWindowBlur = false, bool layeredWindow = false, string name = null)
		{
			this._windowsForm = new WindowsForm(x, y, width, height, resourceDepot, borderlessWindow, enableWindowBlur, name);
			this.Initalize(layeredWindow);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003351 File Offset: 0x00001551
		public GraphicsForm(WindowsForm windowsForm)
		{
			this._windowsForm = windowsForm;
			this.Initalize(false);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000337C File Offset: 0x0000157C
		private void Initalize(bool layeredWindow)
		{
			this._currentInputData = new InputData();
			this._oldInputData = new InputData();
			this._messageLoopInputData = new InputData();
			this._windowsForm.AddMessageHandler(new WindowsFormMessageHandler(this.MessageHandler));
			this._windowsForm.Show();
			this.GraphicsContext = new GraphicsContext();
			if (layeredWindow)
			{
				this._layeredWindowController = new LayeredWindowController(this._windowsForm.Handle, this._windowsForm.Width, this._windowsForm.Height);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003408 File Offset: 0x00001608
		public bool CompareRecrangles(DXGI.RECT Rect1, DXGI.RECT Rect2)
		{
			int num = Rect1.right - Rect1.left;
			int num2 = Rect1.bottom - Rect1.top;
			int num3 = Rect2.right - Rect2.left;
			int num4 = Rect2.bottom - Rect2.top;
			return num > num3 && num2 > num4;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003458 File Offset: 0x00001658
		public DXGI.RECT DecideWindowPosition()
		{
			Rectangle rectangle;
			User32.GetClientRect(User32.GetDesktopWindow(), out rectangle);
			DXGI.RECT rect = new DXGI.RECT
			{
				left = rectangle.Left,
				right = rectangle.Right,
				top = rectangle.Top,
				bottom = rectangle.Bottom
			};
			DXGI.RECT rect2 = rect;
			IntPtr intPtr;
			DXGI.CreateDXGIFactory(ref DXGI.IID_IDXGIFactory, out intPtr);
			DXGI.IDXGIFactory idxgifactory = (DXGI.IDXGIFactory)Marshal.GetObjectForIUnknown(intPtr);
			MBList<Tuple<uint, DXGI.DXGI_ADAPTER_DESC>> mblist = new MBList<Tuple<uint, DXGI.DXGI_ADAPTER_DESC>>();
			uint num = 0U;
			DXGI.IDXGIAdapter idxgiadapter;
			while (idxgifactory.EnumAdapters(num, out idxgiadapter) == 0)
			{
				DXGI.DXGI_ADAPTER_DESC dxgi_ADAPTER_DESC;
				idxgiadapter.GetDesc(out dxgi_ADAPTER_DESC);
				if ((ulong)dxgi_ADAPTER_DESC.DedicatedVideoMemory > 0UL)
				{
					mblist.Add(new Tuple<uint, DXGI.DXGI_ADAPTER_DESC>(num, dxgi_ADAPTER_DESC));
				}
				num += 1U;
			}
			if (mblist.Count == 0)
			{
				Marshal.FinalReleaseComObject(idxgifactory);
				return rect2;
			}
			mblist.Sort(delegate(Tuple<uint, DXGI.DXGI_ADAPTER_DESC> x, Tuple<uint, DXGI.DXGI_ADAPTER_DESC> y)
			{
				if ((ulong)x.Item2.DedicatedVideoMemory <= (ulong)y.Item2.DedicatedVideoMemory)
				{
					return 1;
				}
				return -1;
			});
			foreach (Tuple<uint, DXGI.DXGI_ADAPTER_DESC> tuple in mblist)
			{
				DXGI.IDXGIAdapter idxgiadapter2;
				idxgifactory.EnumAdapters(tuple.Item1, out idxgiadapter2);
				uint num2 = 0U;
				DXGI.IDXGIOutput idxgioutput;
				while (idxgiadapter2.EnumOutputs(num2, out idxgioutput) == 0)
				{
					DXGI.DXGI_OUTPUT_DESC dxgi_OUTPUT_DESC;
					idxgioutput.GetDesc(out dxgi_OUTPUT_DESC);
					if (dxgi_OUTPUT_DESC.AttachedToDesktop && rect2 == dxgi_OUTPUT_DESC.DesktopCoordinates)
					{
						Marshal.FinalReleaseComObject(idxgifactory);
						return rect2;
					}
					num2 += 1U;
				}
			}
			foreach (Tuple<uint, DXGI.DXGI_ADAPTER_DESC> tuple2 in mblist)
			{
				DXGI.IDXGIAdapter idxgiadapter3;
				idxgifactory.EnumAdapters(tuple2.Item1, out idxgiadapter3);
				uint num3 = 0U;
				DXGI.IDXGIOutput idxgioutput2;
				while (idxgiadapter3.EnumOutputs(num3, out idxgioutput2) == 0)
				{
					DXGI.DXGI_OUTPUT_DESC dxgi_OUTPUT_DESC2;
					idxgioutput2.GetDesc(out dxgi_OUTPUT_DESC2);
					if (dxgi_OUTPUT_DESC2.AttachedToDesktop)
					{
						Marshal.FinalReleaseComObject(idxgifactory);
						return dxgi_OUTPUT_DESC2.DesktopCoordinates;
					}
					num3 += 1U;
				}
			}
			Marshal.FinalReleaseComObject(idxgifactory);
			return rect2;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003678 File Offset: 0x00001878
		public void Destroy()
		{
			this.GraphicsContext.DestroyContext();
			this._windowsForm.Destroy();
			LayeredWindowController layeredWindowController = this._layeredWindowController;
			if (layeredWindowController == null)
			{
				return;
			}
			layeredWindowController.OnFinalize();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000036A0 File Offset: 0x000018A0
		public void MinimizeWindow()
		{
			User32.ShowWindow(this._windowsForm.Handle, WindowShowStyle.Minimize);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000036B4 File Offset: 0x000018B4
		public void InitializeGraphicsContext(ResourceDepot resourceDepot)
		{
			this.GraphicsContext.Control = this._windowsForm;
			this.GraphicsContext.CreateContext(resourceDepot);
			this.GraphicsContext.ProjectionMatrix = MatrixExtensions.CreateOrthographicOffCenter(0f, (float)this._windowsForm.Width, (float)this._windowsForm.Height, 0f, 0f, 2f);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000371C File Offset: 0x0000191C
		public void BeginFrame()
		{
			if (this.GraphicsContext != null)
			{
				int num = this._windowsForm.Width;
				int num2 = this._windowsForm.Height;
				Rectangle rectangle;
				if (User32.GetClientRect(this._windowsForm.Handle, out rectangle))
				{
					int width = rectangle.Width;
					int height = rectangle.Height;
					if (width > 0 && height > 0)
					{
						num = width;
						num2 = height;
					}
				}
				this.GraphicsContext.BeginFrame(num, num2);
				this.GraphicsContext.Resize(num, num2);
				this.GraphicsContext.ProjectionMatrix = MatrixExtensions.CreateOrthographicOffCenter(0f, (float)num, (float)num2, 0f, 0f, 2f);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000037C4 File Offset: 0x000019C4
		public void Update()
		{
			if (!this._isDragging && this._mouseOverDragArea && this._currentInputData.LeftMouse && !this._oldInputData.LeftMouse)
			{
				this._isDragging = true;
				this.MessageHandler(WindowMessage.LeftButtonUp, 0L, 0L);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003814 File Offset: 0x00001A14
		public void MessageLoop()
		{
			if (this._isDragging)
			{
				User32.ReleaseCapture();
				User32.SendMessage(this._windowsForm.Handle, 161U, new IntPtr(2), IntPtr.Zero);
				this._isDragging = false;
				User32.SetCapture(this._windowsForm.Handle);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003868 File Offset: 0x00001A68
		public void UpdateInput(bool mouseOverDragArea = false)
		{
			this._mouseOverDragArea = mouseOverDragArea;
			InputData oldInputData = this._oldInputData;
			this._oldInputData = this._currentInputData;
			this._currentInputData = oldInputData;
			object inputDataLocker = this._inputDataLocker;
			lock (inputDataLocker)
			{
				this._currentInputData.FillFrom(this._messageLoopInputData);
				this._messageLoopInputData.Reset();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000038E0 File Offset: 0x00001AE0
		public void PostRender()
		{
			if (this._layeredWindowController != null)
			{
				this._layeredWindowController.PostRender();
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000038F8 File Offset: 0x00001AF8
		public bool GetKeyDown(InputKey keyCode)
		{
			if (keyCode == InputKey.LeftMouseButton)
			{
				return this.LeftMouseDown();
			}
			if (keyCode == InputKey.RightMouseButton)
			{
				return this.RightMouseDown();
			}
			return this._currentInputData.KeyData[(int)keyCode] && !this._oldInputData.KeyData[(int)keyCode];
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003944 File Offset: 0x00001B44
		public bool GetKey(InputKey keyCode)
		{
			if (keyCode == InputKey.LeftMouseButton)
			{
				return this.LeftMouse();
			}
			if (keyCode == InputKey.RightMouseButton)
			{
				return this.RightMouse();
			}
			return this._currentInputData.KeyData[(int)keyCode];
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003971 File Offset: 0x00001B71
		public bool GetKeyUp(InputKey keyCode)
		{
			if (keyCode == InputKey.LeftMouseButton)
			{
				return this.LeftMouseUp();
			}
			if (keyCode == InputKey.RightMouseButton)
			{
				return this.RightMouseUp();
			}
			return !this._currentInputData.KeyData[(int)keyCode] && this._oldInputData.KeyData[(int)keyCode];
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000039AF File Offset: 0x00001BAF
		public float GetMouseDeltaZ()
		{
			return this._currentInputData.MouseScrollDelta;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000039BC File Offset: 0x00001BBC
		public bool LeftMouse()
		{
			return this._currentInputData.LeftMouse;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000039C9 File Offset: 0x00001BC9
		public bool LeftMouseDown()
		{
			return this._currentInputData.LeftMouse && !this._oldInputData.LeftMouse;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000039E8 File Offset: 0x00001BE8
		public bool LeftMouseUp()
		{
			return !this._currentInputData.LeftMouse && this._oldInputData.LeftMouse;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003A04 File Offset: 0x00001C04
		public bool RightMouse()
		{
			return this._currentInputData.RightMouse;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003A11 File Offset: 0x00001C11
		public bool RightMouseDown()
		{
			return this._currentInputData.RightMouse && !this._oldInputData.RightMouse;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003A30 File Offset: 0x00001C30
		public bool RightMouseUp()
		{
			return !this._currentInputData.RightMouse && this._oldInputData.RightMouse;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003A4C File Offset: 0x00001C4C
		public Vector2 MousePosition()
		{
			return new Vector2((float)this._currentInputData.CursorX, (float)this._currentInputData.CursorY);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003A6B File Offset: 0x00001C6B
		public bool MouseMove()
		{
			return this._currentInputData.MouseMove;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003A78 File Offset: 0x00001C78
		public void FillInputDataFromCurrent(InputData inputData)
		{
			inputData.FillFrom(this._currentInputData);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003A88 File Offset: 0x00001C88
		private void MessageHandler(WindowMessage message, long wParam, long lParam)
		{
			object obj;
			if (message <= WindowMessage.KeyDown)
			{
				if (message <= WindowMessage.Close)
				{
					switch (message)
					{
					case WindowMessage.Size:
					{
						int num = (int)lParam % 65536;
						int num2 = (int)(lParam / 65536L);
						if (num <= 0 || num2 <= 0)
						{
							return;
						}
						LayeredWindowController layeredWindowController = this._layeredWindowController;
						if (layeredWindowController != null)
						{
							layeredWindowController.SetSize(num, num2);
						}
						if (this.GraphicsContext != null)
						{
							this.GraphicsContext.Resize(num, num2);
							return;
						}
						return;
					}
					case (WindowMessage)6U:
						return;
					case WindowMessage.SetFocus:
						goto IL_044D;
					case WindowMessage.KillFocus:
						goto IL_03F0;
					default:
						if (message != WindowMessage.Close)
						{
							return;
						}
						this.Destroy();
						Environment.Exit(0);
						return;
					}
				}
				else if (message != WindowMessage.DisplayChange)
				{
					if (message != WindowMessage.KeyDown)
					{
						return;
					}
					obj = this._inputDataLocker;
					lock (obj)
					{
						this._messageLoopInputData.KeyData[(int)(checked((IntPtr)wParam))] = true;
						return;
					}
					goto IL_01A2;
				}
			}
			else if (message <= WindowMessage.MouseWheel)
			{
				if (message == WindowMessage.KeyUp)
				{
					goto IL_01A2;
				}
				switch (message)
				{
				case WindowMessage.MouseMove:
					goto IL_0356;
				case WindowMessage.LeftButtonDown:
					goto IL_02F6;
				case WindowMessage.LeftButtonUp:
					goto IL_0296;
				case (WindowMessage)515U:
				case (WindowMessage)518U:
				case (WindowMessage)519U:
				case (WindowMessage)520U:
				case (WindowMessage)521U:
					return;
				case WindowMessage.RightButtonDown:
					goto IL_0236;
				case WindowMessage.RightButtonUp:
					goto IL_01D6;
				case WindowMessage.MouseWheel:
					goto IL_03B6;
				default:
					return;
				}
			}
			else if (message != WindowMessage.DeviceChange && message != WindowMessage.DpiChanged)
			{
				return;
			}
			if (this.GraphicsContext != null)
			{
				this.GraphicsContext.RequestContextReactivation();
			}
			Rectangle rectangle;
			if (!User32.GetClientRect(this._windowsForm.Handle, out rectangle))
			{
				return;
			}
			int width = rectangle.Width;
			int height = rectangle.Height;
			if (width <= 0 || height <= 0)
			{
				return;
			}
			LayeredWindowController layeredWindowController2 = this._layeredWindowController;
			if (layeredWindowController2 != null)
			{
				layeredWindowController2.SetSize(width, height);
			}
			if (this.GraphicsContext != null)
			{
				this.GraphicsContext.Resize(width, height);
				return;
			}
			return;
			IL_01A2:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.KeyData[(int)(checked((IntPtr)wParam))] = false;
				return;
			}
			IL_01D6:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.RightMouse = false;
				int num3 = (int)lParam % 65536;
				int num4 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num3;
				this._messageLoopInputData.CursorY = num4;
				return;
			}
			IL_0236:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.RightMouse = true;
				int num5 = (int)lParam % 65536;
				int num6 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num5;
				this._messageLoopInputData.CursorY = num6;
				return;
			}
			IL_0296:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.LeftMouse = false;
				int num7 = (int)lParam % 65536;
				int num8 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num7;
				this._messageLoopInputData.CursorY = num8;
				return;
			}
			IL_02F6:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.LeftMouse = true;
				int num9 = (int)lParam % 65536;
				int num10 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num9;
				this._messageLoopInputData.CursorY = num10;
				return;
			}
			IL_0356:
			obj = this._inputDataLocker;
			lock (obj)
			{
				this._messageLoopInputData.MouseMove = true;
				int num11 = (int)lParam % 65536;
				int num12 = (int)(lParam / 65536L);
				this._messageLoopInputData.CursorX = num11;
				this._messageLoopInputData.CursorY = num12;
				return;
			}
			IL_03B6:
			obj = this._inputDataLocker;
			lock (obj)
			{
				short num13 = (short)(wParam >> 16);
				this._messageLoopInputData.MouseScrollDelta = (float)num13;
				return;
			}
			IL_03F0:
			obj = this._inputDataLocker;
			lock (obj)
			{
				for (int i = 0; i < 256; i++)
				{
					this._messageLoopInputData.KeyData[i] = false;
					this._messageLoopInputData.RightMouse = false;
					this._messageLoopInputData.LeftMouse = false;
				}
				return;
			}
			IL_044D:
			obj = this._inputDataLocker;
			lock (obj)
			{
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003F80 File Offset: 0x00002180
		public int Width
		{
			get
			{
				return this._windowsForm.Width;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003F8D File Offset: 0x0000218D
		public int Height
		{
			get
			{
				return this._windowsForm.Height;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003F9A File Offset: 0x0000219A
		public bool IsMinimized
		{
			get
			{
				return this._windowsForm.IsMinimized;
			}
		}

		// Token: 0x0400001D RID: 29
		public const int WM_NCLBUTTONDOWN = 161;

		// Token: 0x0400001E RID: 30
		public const int HT_CAPTION = 2;

		// Token: 0x0400001F RID: 31
		private WindowsForm _windowsForm;

		// Token: 0x04000021 RID: 33
		private InputData _currentInputData;

		// Token: 0x04000022 RID: 34
		private InputData _oldInputData;

		// Token: 0x04000023 RID: 35
		private InputData _messageLoopInputData;

		// Token: 0x04000024 RID: 36
		private object _inputDataLocker = new object();

		// Token: 0x04000025 RID: 37
		private bool _mouseOverDragArea = true;

		// Token: 0x04000026 RID: 38
		private bool _isDragging;

		// Token: 0x04000027 RID: 39
		private LayeredWindowController _layeredWindowController;
	}
}
