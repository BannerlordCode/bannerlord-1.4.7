using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000012 RID: 18
	internal class ScriptingInterfaceOfIGameEntityComponent : IGameEntityComponent
	{
		// Token: 0x0600024D RID: 589 RVA: 0x00011D08 File Offset: 0x0000FF08
		public GameEntity GetEntity(GameEntityComponent entityComponent)
		{
			UIntPtr uintPtr = ((entityComponent != null) ? entityComponent.Pointer : UIntPtr.Zero);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntityComponent.call_GetEntityDelegate(uintPtr);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00011D69 File Offset: 0x0000FF69
		public UIntPtr GetEntityPointer(UIntPtr componentPointer)
		{
			return ScriptingInterfaceOfIGameEntityComponent.call_GetEntityPointerDelegate(componentPointer);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00011D78 File Offset: 0x0000FF78
		public MetaMesh GetFirstMetaMesh(GameEntityComponent entityComponent)
		{
			UIntPtr uintPtr = ((entityComponent != null) ? entityComponent.Pointer : UIntPtr.Zero);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntityComponent.call_GetFirstMetaMeshDelegate(uintPtr);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x040001CA RID: 458
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001CB RID: 459
		public static ScriptingInterfaceOfIGameEntityComponent.GetEntityDelegate call_GetEntityDelegate;

		// Token: 0x040001CC RID: 460
		public static ScriptingInterfaceOfIGameEntityComponent.GetEntityPointerDelegate call_GetEntityPointerDelegate;

		// Token: 0x040001CD RID: 461
		public static ScriptingInterfaceOfIGameEntityComponent.GetFirstMetaMeshDelegate call_GetFirstMetaMeshDelegate;

		// Token: 0x02000247 RID: 583
		// (Invoke) Token: 0x06000F35 RID: 3893
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetEntityDelegate(UIntPtr entityComponent);

		// Token: 0x02000248 RID: 584
		// (Invoke) Token: 0x06000F39 RID: 3897
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetEntityPointerDelegate(UIntPtr componentPointer);

		// Token: 0x02000249 RID: 585
		// (Invoke) Token: 0x06000F3D RID: 3901
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFirstMetaMeshDelegate(UIntPtr entityComponent);
	}
}
