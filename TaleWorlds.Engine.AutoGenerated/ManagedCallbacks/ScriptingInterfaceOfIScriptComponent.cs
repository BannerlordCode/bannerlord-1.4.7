using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000025 RID: 37
	internal class ScriptingInterfaceOfIScriptComponent : IScriptComponent
	{
		// Token: 0x0600056A RID: 1386 RVA: 0x00018006 File Offset: 0x00016206
		public string GetName(UIntPtr pointer)
		{
			if (ScriptingInterfaceOfIScriptComponent.call_GetNameDelegate(pointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001801D File Offset: 0x0001621D
		public ScriptComponentBehavior GetScriptComponentBehavior(UIntPtr pointer)
		{
			return DotNetObject.GetManagedObjectWithId(ScriptingInterfaceOfIScriptComponent.call_GetScriptComponentBehaviorDelegate(pointer)) as ScriptComponentBehavior;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00018034 File Offset: 0x00016234
		public void SetVariableEditorWidgetStatus(UIntPtr pointer, string field, bool enabled)
		{
			byte[] array = null;
			if (field != null)
			{
				int byteCount = ScriptingInterfaceOfIScriptComponent._utf8.GetByteCount(field);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIScriptComponent._utf8.GetBytes(field, 0, field.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIScriptComponent.call_SetVariableEditorWidgetStatusDelegate(pointer, array, enabled);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00018090 File Offset: 0x00016290
		public void SetVariableEditorWidgetValue(UIntPtr pointer, string field, RglScriptFieldType fieldType, double value)
		{
			byte[] array = null;
			if (field != null)
			{
				int byteCount = ScriptingInterfaceOfIScriptComponent._utf8.GetByteCount(field);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIScriptComponent._utf8.GetBytes(field, 0, field.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIScriptComponent.call_SetVariableEditorWidgetValueDelegate(pointer, array, fieldType, value);
		}

		// Token: 0x040004C2 RID: 1218
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004C3 RID: 1219
		public static ScriptingInterfaceOfIScriptComponent.GetNameDelegate call_GetNameDelegate;

		// Token: 0x040004C4 RID: 1220
		public static ScriptingInterfaceOfIScriptComponent.GetScriptComponentBehaviorDelegate call_GetScriptComponentBehaviorDelegate;

		// Token: 0x040004C5 RID: 1221
		public static ScriptingInterfaceOfIScriptComponent.SetVariableEditorWidgetStatusDelegate call_SetVariableEditorWidgetStatusDelegate;

		// Token: 0x040004C6 RID: 1222
		public static ScriptingInterfaceOfIScriptComponent.SetVariableEditorWidgetValueDelegate call_SetVariableEditorWidgetValueDelegate;

		// Token: 0x0200052C RID: 1324
		// (Invoke) Token: 0x06001AC9 RID: 6857
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr pointer);

		// Token: 0x0200052D RID: 1325
		// (Invoke) Token: 0x06001ACD RID: 6861
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetScriptComponentBehaviorDelegate(UIntPtr pointer);

		// Token: 0x0200052E RID: 1326
		// (Invoke) Token: 0x06001AD1 RID: 6865
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVariableEditorWidgetStatusDelegate(UIntPtr pointer, byte[] field, [MarshalAs(UnmanagedType.U1)] bool enabled);

		// Token: 0x0200052F RID: 1327
		// (Invoke) Token: 0x06001AD5 RID: 6869
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVariableEditorWidgetValueDelegate(UIntPtr pointer, byte[] field, RglScriptFieldType fieldType, double value);
	}
}
