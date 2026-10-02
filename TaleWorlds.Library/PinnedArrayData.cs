using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.Library
{
	// Token: 0x0200007C RID: 124
	public struct PinnedArrayData<T>
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000F7E7 File Offset: 0x0000D9E7
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x0000F7EF File Offset: 0x0000D9EF
		public bool Pinned { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x0000F800 File Offset: 0x0000DA00
		public IntPtr Pointer { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0000F809 File Offset: 0x0000DA09
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x0000F811 File Offset: 0x0000DA11
		public T[] Array { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0000F81A File Offset: 0x0000DA1A
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0000F822 File Offset: 0x0000DA22
		public T[,] Array2D { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0000F82B File Offset: 0x0000DA2B
		public GCHandle Handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000F834 File Offset: 0x0000DA34
		public PinnedArrayData(T[] array, bool manualPinning = false)
		{
			this.Array = array;
			this.Array2D = null;
			this.Pinned = false;
			this.Pointer = IntPtr.Zero;
			if (array != null)
			{
				if (!manualPinning)
				{
					try
					{
						this._handle = GCHandleFactory.GetHandle();
						this._handle.Target = array;
						this.Pointer = this.Handle.AddrOfPinnedObject();
						this.Pinned = true;
					}
					catch (ArgumentException)
					{
						manualPinning = true;
					}
				}
				if (manualPinning)
				{
					this.Pinned = false;
					int num = Marshal.SizeOf<T>();
					for (int i = 0; i < array.Length; i++)
					{
						Marshal.StructureToPtr<T>(array[i], PinnedArrayData<T>._unmanagedCache + num * i, false);
					}
					this.Pointer = PinnedArrayData<T>._unmanagedCache;
				}
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
		public PinnedArrayData(T[,] array, bool manualPinning = false)
		{
			this.Array = null;
			this.Array2D = array;
			this.Pinned = false;
			this.Pointer = IntPtr.Zero;
			if (array != null)
			{
				if (!manualPinning)
				{
					try
					{
						this._handle = GCHandleFactory.GetHandle();
						this._handle.Target = array;
						this.Pointer = this.Handle.AddrOfPinnedObject();
						this.Pinned = true;
					}
					catch (ArgumentException)
					{
						manualPinning = true;
					}
				}
				if (manualPinning)
				{
					this.Pinned = false;
					int num = Marshal.SizeOf<T>();
					for (int i = 0; i < array.GetLength(0); i++)
					{
						for (int j = 0; j < array.GetLength(1); j++)
						{
							Marshal.StructureToPtr<T>(array[i, j], PinnedArrayData<T>._unmanagedCache + num * (i * array.GetLength(1) + j), false);
						}
					}
					this.Pointer = PinnedArrayData<T>._unmanagedCache;
				}
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000F9DC File Offset: 0x0000DBDC
		public static bool CheckIfTypeRequiresManualPinning(Type type)
		{
			bool flag = false;
			Array array = global::System.Array.CreateInstance(type, 10);
			GCHandle gchandle;
			try
			{
				gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				gchandle.AddrOfPinnedObject();
			}
			catch (ArgumentException)
			{
				flag = true;
			}
			if (gchandle.IsAllocated)
			{
				gchandle.Free();
			}
			return flag;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000FA2C File Offset: 0x0000DC2C
		public void Dispose()
		{
			if (this.Pinned)
			{
				if (this.Array != null)
				{
					this._handle.Target = null;
					GCHandleFactory.ReturnHandle(this._handle);
					this.Array = null;
					this.Pointer = IntPtr.Zero;
					return;
				}
				if (this.Array2D != null)
				{
					this._handle.Target = null;
					GCHandleFactory.ReturnHandle(this._handle);
					this.Array2D = null;
					this.Pointer = IntPtr.Zero;
				}
			}
		}

		// Token: 0x0400015D RID: 349
		private static IntPtr _unmanagedCache = Marshal.AllocHGlobal(16384);

		// Token: 0x04000162 RID: 354
		private GCHandle _handle;
	}
}
