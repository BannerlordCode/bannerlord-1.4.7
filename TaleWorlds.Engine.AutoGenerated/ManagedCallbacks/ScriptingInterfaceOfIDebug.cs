using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200000E RID: 14
	internal class ScriptingInterfaceOfIDebug : IDebug
	{
		// Token: 0x060000EC RID: 236 RVA: 0x0000F64F File Offset: 0x0000D84F
		public void AbortGame(int ExitCode)
		{
			ScriptingInterfaceOfIDebug.call_AbortGameDelegate(ExitCode);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000F65C File Offset: 0x0000D85C
		public void AssertMemoryUsage(int memoryMB)
		{
			ScriptingInterfaceOfIDebug.call_AssertMemoryUsageDelegate(memoryMB);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000F669 File Offset: 0x0000D869
		public void ClearAllDebugRenderObjects()
		{
			ScriptingInterfaceOfIDebug.call_ClearAllDebugRenderObjectsDelegate();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000F678 File Offset: 0x0000D878
		public bool ContentWarning(string MessageString)
		{
			byte[] array = null;
			if (MessageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(MessageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(MessageString, 0, MessageString.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_ContentWarningDelegate(array);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000F6D4 File Offset: 0x0000D8D4
		public void EchoCommandWindow(string content)
		{
			byte[] array = null;
			if (content != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(content);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(content, 0, content.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_EchoCommandWindowDelegate(array);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000F730 File Offset: 0x0000D930
		public bool Error(string MessageString)
		{
			byte[] array = null;
			if (MessageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(MessageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(MessageString, 0, MessageString.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_ErrorDelegate(array);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000F78C File Offset: 0x0000D98C
		public bool FailedAssert(string messageString, string callerFile, string callerMethod, int callerLine)
		{
			byte[] array = null;
			if (messageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(messageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(messageString, 0, messageString.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (callerFile != null)
			{
				int byteCount2 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerFile);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerFile, 0, callerFile.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (callerMethod != null)
			{
				int byteCount3 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerMethod);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerMethod, 0, callerMethod.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_FailedAssertDelegate(array, array2, array3, callerLine);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000F876 File Offset: 0x0000DA76
		public Vec3 GetDebugVector()
		{
			return ScriptingInterfaceOfIDebug.call_GetDebugVectorDelegate();
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000F882 File Offset: 0x0000DA82
		public int GetShowDebugInfo()
		{
			return ScriptingInterfaceOfIDebug.call_GetShowDebugInfoDelegate();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000F88E File Offset: 0x0000DA8E
		public bool IsErrorReportModeActive()
		{
			return ScriptingInterfaceOfIDebug.call_IsErrorReportModeActiveDelegate();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000F89A File Offset: 0x0000DA9A
		public bool IsErrorReportModePauseMission()
		{
			return ScriptingInterfaceOfIDebug.call_IsErrorReportModePauseMissionDelegate();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000F8A6 File Offset: 0x0000DAA6
		public bool IsTestMode()
		{
			return ScriptingInterfaceOfIDebug.call_IsTestModeDelegate();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
		public int MessageBox(string lpText, string lpCaption, uint uType)
		{
			byte[] array = null;
			if (lpText != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(lpText);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(lpText, 0, lpText.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (lpCaption != null)
			{
				int byteCount2 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(lpCaption);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(lpCaption, 0, lpCaption.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_MessageBoxDelegate(array, array2, uType);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000F954 File Offset: 0x0000DB54
		public void PostWarningLine(string line)
		{
			byte[] array = null;
			if (line != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(line);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(line, 0, line.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_PostWarningLineDelegate(array);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000F9AE File Offset: 0x0000DBAE
		public void RenderDebugBoxObject(Vec3 min, Vec3 max, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugBoxObjectDelegate(min, max, color, depthCheck, time);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000F9C1 File Offset: 0x0000DBC1
		public void RenderDebugBoxObjectWithFrame(Vec3 min, Vec3 max, ref MatrixFrame frame, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugBoxObjectWithFrameDelegate(min, max, ref frame, color, depthCheck, time);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000F9D6 File Offset: 0x0000DBD6
		public void RenderDebugCapsule(Vec3 p0, Vec3 p1, float radius, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugCapsuleDelegate(p0, p1, radius, color, depthCheck, time);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000F9EB File Offset: 0x0000DBEB
		public void RenderDebugDirectionArrow(Vec3 position, Vec3 direction, uint color, bool depthCheck)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugDirectionArrowDelegate(position, direction, color, depthCheck);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000F9FC File Offset: 0x0000DBFC
		public void RenderDebugFrame(ref MatrixFrame frame, float lineLength, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugFrameDelegate(ref frame, lineLength, time);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000FA0B File Offset: 0x0000DC0B
		public void RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugLineDelegate(position, direction, color, depthCheck, time);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000FA1E File Offset: 0x0000DC1E
		public void RenderDebugRect(float left, float bottom, float right, float top)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugRectDelegate(left, bottom, right, top);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000FA2F File Offset: 0x0000DC2F
		public void RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugRectWithColorDelegate(left, bottom, right, top, color);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000FA42 File Offset: 0x0000DC42
		public void RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time)
		{
			ScriptingInterfaceOfIDebug.call_RenderDebugSphereDelegate(position, radius, color, depthCheck, time);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000FA58 File Offset: 0x0000DC58
		public void RenderDebugText(float screenX, float screenY, string str, uint color, float time)
		{
			byte[] array = null;
			if (str != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(str);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(str, 0, str.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_RenderDebugTextDelegate(screenX, screenY, array, color, time);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000FAB8 File Offset: 0x0000DCB8
		public void RenderDebugText3d(Vec3 worldPosition, string str, uint color, int screenPosOffsetX, int screenPosOffsetY, float time)
		{
			byte[] array = null;
			if (str != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(str);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(str, 0, str.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_RenderDebugText3dDelegate(worldPosition, array, color, screenPosOffsetX, screenPosOffsetY, time);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000FB1A File Offset: 0x0000DD1A
		public void SetDebugVector(Vec3 debugVector)
		{
			ScriptingInterfaceOfIDebug.call_SetDebugVectorDelegate(debugVector);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000FB27 File Offset: 0x0000DD27
		public void SetDumpGenerationDisabled(bool Disabled)
		{
			ScriptingInterfaceOfIDebug.call_SetDumpGenerationDisabledDelegate(Disabled);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000FB34 File Offset: 0x0000DD34
		public void SetErrorReportScene(UIntPtr scenePointer)
		{
			ScriptingInterfaceOfIDebug.call_SetErrorReportSceneDelegate(scenePointer);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000FB41 File Offset: 0x0000DD41
		public void SetShowDebugInfo(int value)
		{
			ScriptingInterfaceOfIDebug.call_SetShowDebugInfoDelegate(value);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000FB50 File Offset: 0x0000DD50
		public bool SilentAssert(string messageString, string callerFile, string callerMethod, int callerLine, bool getDump)
		{
			byte[] array = null;
			if (messageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(messageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(messageString, 0, messageString.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (callerFile != null)
			{
				int byteCount2 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerFile);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerFile, 0, callerFile.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (callerMethod != null)
			{
				int byteCount3 = ScriptingInterfaceOfIDebug._utf8.GetByteCount(callerMethod);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(callerMethod, 0, callerMethod.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_SilentAssertDelegate(array, array2, array3, callerLine, getDump);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000FC3C File Offset: 0x0000DE3C
		public bool Warning(string MessageString)
		{
			byte[] array = null;
			if (MessageString != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(MessageString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(MessageString, 0, MessageString.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIDebug.call_WarningDelegate(array);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000FC98 File Offset: 0x0000DE98
		public void WriteDebugLineOnScreen(string line)
		{
			byte[] array = null;
			if (line != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(line);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(line, 0, line.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_WriteDebugLineOnScreenDelegate(array);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		public void WriteLine(int logLevel, string line, int color, ulong filter)
		{
			byte[] array = null;
			if (line != null)
			{
				int byteCount = ScriptingInterfaceOfIDebug._utf8.GetByteCount(line);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIDebug._utf8.GetBytes(line, 0, line.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIDebug.call_WriteLineDelegate(logLevel, array, color, filter);
		}

		// Token: 0x0400007F RID: 127
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000080 RID: 128
		public static ScriptingInterfaceOfIDebug.AbortGameDelegate call_AbortGameDelegate;

		// Token: 0x04000081 RID: 129
		public static ScriptingInterfaceOfIDebug.AssertMemoryUsageDelegate call_AssertMemoryUsageDelegate;

		// Token: 0x04000082 RID: 130
		public static ScriptingInterfaceOfIDebug.ClearAllDebugRenderObjectsDelegate call_ClearAllDebugRenderObjectsDelegate;

		// Token: 0x04000083 RID: 131
		public static ScriptingInterfaceOfIDebug.ContentWarningDelegate call_ContentWarningDelegate;

		// Token: 0x04000084 RID: 132
		public static ScriptingInterfaceOfIDebug.EchoCommandWindowDelegate call_EchoCommandWindowDelegate;

		// Token: 0x04000085 RID: 133
		public static ScriptingInterfaceOfIDebug.ErrorDelegate call_ErrorDelegate;

		// Token: 0x04000086 RID: 134
		public static ScriptingInterfaceOfIDebug.FailedAssertDelegate call_FailedAssertDelegate;

		// Token: 0x04000087 RID: 135
		public static ScriptingInterfaceOfIDebug.GetDebugVectorDelegate call_GetDebugVectorDelegate;

		// Token: 0x04000088 RID: 136
		public static ScriptingInterfaceOfIDebug.GetShowDebugInfoDelegate call_GetShowDebugInfoDelegate;

		// Token: 0x04000089 RID: 137
		public static ScriptingInterfaceOfIDebug.IsErrorReportModeActiveDelegate call_IsErrorReportModeActiveDelegate;

		// Token: 0x0400008A RID: 138
		public static ScriptingInterfaceOfIDebug.IsErrorReportModePauseMissionDelegate call_IsErrorReportModePauseMissionDelegate;

		// Token: 0x0400008B RID: 139
		public static ScriptingInterfaceOfIDebug.IsTestModeDelegate call_IsTestModeDelegate;

		// Token: 0x0400008C RID: 140
		public static ScriptingInterfaceOfIDebug.MessageBoxDelegate call_MessageBoxDelegate;

		// Token: 0x0400008D RID: 141
		public static ScriptingInterfaceOfIDebug.PostWarningLineDelegate call_PostWarningLineDelegate;

		// Token: 0x0400008E RID: 142
		public static ScriptingInterfaceOfIDebug.RenderDebugBoxObjectDelegate call_RenderDebugBoxObjectDelegate;

		// Token: 0x0400008F RID: 143
		public static ScriptingInterfaceOfIDebug.RenderDebugBoxObjectWithFrameDelegate call_RenderDebugBoxObjectWithFrameDelegate;

		// Token: 0x04000090 RID: 144
		public static ScriptingInterfaceOfIDebug.RenderDebugCapsuleDelegate call_RenderDebugCapsuleDelegate;

		// Token: 0x04000091 RID: 145
		public static ScriptingInterfaceOfIDebug.RenderDebugDirectionArrowDelegate call_RenderDebugDirectionArrowDelegate;

		// Token: 0x04000092 RID: 146
		public static ScriptingInterfaceOfIDebug.RenderDebugFrameDelegate call_RenderDebugFrameDelegate;

		// Token: 0x04000093 RID: 147
		public static ScriptingInterfaceOfIDebug.RenderDebugLineDelegate call_RenderDebugLineDelegate;

		// Token: 0x04000094 RID: 148
		public static ScriptingInterfaceOfIDebug.RenderDebugRectDelegate call_RenderDebugRectDelegate;

		// Token: 0x04000095 RID: 149
		public static ScriptingInterfaceOfIDebug.RenderDebugRectWithColorDelegate call_RenderDebugRectWithColorDelegate;

		// Token: 0x04000096 RID: 150
		public static ScriptingInterfaceOfIDebug.RenderDebugSphereDelegate call_RenderDebugSphereDelegate;

		// Token: 0x04000097 RID: 151
		public static ScriptingInterfaceOfIDebug.RenderDebugTextDelegate call_RenderDebugTextDelegate;

		// Token: 0x04000098 RID: 152
		public static ScriptingInterfaceOfIDebug.RenderDebugText3dDelegate call_RenderDebugText3dDelegate;

		// Token: 0x04000099 RID: 153
		public static ScriptingInterfaceOfIDebug.SetDebugVectorDelegate call_SetDebugVectorDelegate;

		// Token: 0x0400009A RID: 154
		public static ScriptingInterfaceOfIDebug.SetDumpGenerationDisabledDelegate call_SetDumpGenerationDisabledDelegate;

		// Token: 0x0400009B RID: 155
		public static ScriptingInterfaceOfIDebug.SetErrorReportSceneDelegate call_SetErrorReportSceneDelegate;

		// Token: 0x0400009C RID: 156
		public static ScriptingInterfaceOfIDebug.SetShowDebugInfoDelegate call_SetShowDebugInfoDelegate;

		// Token: 0x0400009D RID: 157
		public static ScriptingInterfaceOfIDebug.SilentAssertDelegate call_SilentAssertDelegate;

		// Token: 0x0400009E RID: 158
		public static ScriptingInterfaceOfIDebug.WarningDelegate call_WarningDelegate;

		// Token: 0x0400009F RID: 159
		public static ScriptingInterfaceOfIDebug.WriteDebugLineOnScreenDelegate call_WriteDebugLineOnScreenDelegate;

		// Token: 0x040000A0 RID: 160
		public static ScriptingInterfaceOfIDebug.WriteLineDelegate call_WriteLineDelegate;

		// Token: 0x02000100 RID: 256
		// (Invoke) Token: 0x06000A19 RID: 2585
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AbortGameDelegate(int ExitCode);

		// Token: 0x02000101 RID: 257
		// (Invoke) Token: 0x06000A1D RID: 2589
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AssertMemoryUsageDelegate(int memoryMB);

		// Token: 0x02000102 RID: 258
		// (Invoke) Token: 0x06000A21 RID: 2593
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearAllDebugRenderObjectsDelegate();

		// Token: 0x02000103 RID: 259
		// (Invoke) Token: 0x06000A25 RID: 2597
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ContentWarningDelegate(byte[] MessageString);

		// Token: 0x02000104 RID: 260
		// (Invoke) Token: 0x06000A29 RID: 2601
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EchoCommandWindowDelegate(byte[] content);

		// Token: 0x02000105 RID: 261
		// (Invoke) Token: 0x06000A2D RID: 2605
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool ErrorDelegate(byte[] MessageString);

		// Token: 0x02000106 RID: 262
		// (Invoke) Token: 0x06000A31 RID: 2609
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool FailedAssertDelegate(byte[] messageString, byte[] callerFile, byte[] callerMethod, int callerLine);

		// Token: 0x02000107 RID: 263
		// (Invoke) Token: 0x06000A35 RID: 2613
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetDebugVectorDelegate();

		// Token: 0x02000108 RID: 264
		// (Invoke) Token: 0x06000A39 RID: 2617
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetShowDebugInfoDelegate();

		// Token: 0x02000109 RID: 265
		// (Invoke) Token: 0x06000A3D RID: 2621
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsErrorReportModeActiveDelegate();

		// Token: 0x0200010A RID: 266
		// (Invoke) Token: 0x06000A41 RID: 2625
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsErrorReportModePauseMissionDelegate();

		// Token: 0x0200010B RID: 267
		// (Invoke) Token: 0x06000A45 RID: 2629
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsTestModeDelegate();

		// Token: 0x0200010C RID: 268
		// (Invoke) Token: 0x06000A49 RID: 2633
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int MessageBoxDelegate(byte[] lpText, byte[] lpCaption, uint uType);

		// Token: 0x0200010D RID: 269
		// (Invoke) Token: 0x06000A4D RID: 2637
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PostWarningLineDelegate(byte[] line);

		// Token: 0x0200010E RID: 270
		// (Invoke) Token: 0x06000A51 RID: 2641
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugBoxObjectDelegate(Vec3 min, Vec3 max, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x0200010F RID: 271
		// (Invoke) Token: 0x06000A55 RID: 2645
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugBoxObjectWithFrameDelegate(Vec3 min, Vec3 max, ref MatrixFrame frame, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000110 RID: 272
		// (Invoke) Token: 0x06000A59 RID: 2649
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugCapsuleDelegate(Vec3 p0, Vec3 p1, float radius, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000111 RID: 273
		// (Invoke) Token: 0x06000A5D RID: 2653
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugDirectionArrowDelegate(Vec3 position, Vec3 direction, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck);

		// Token: 0x02000112 RID: 274
		// (Invoke) Token: 0x06000A61 RID: 2657
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugFrameDelegate(ref MatrixFrame frame, float lineLength, float time);

		// Token: 0x02000113 RID: 275
		// (Invoke) Token: 0x06000A65 RID: 2661
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugLineDelegate(Vec3 position, Vec3 direction, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000114 RID: 276
		// (Invoke) Token: 0x06000A69 RID: 2665
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugRectDelegate(float left, float bottom, float right, float top);

		// Token: 0x02000115 RID: 277
		// (Invoke) Token: 0x06000A6D RID: 2669
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugRectWithColorDelegate(float left, float bottom, float right, float top, uint color);

		// Token: 0x02000116 RID: 278
		// (Invoke) Token: 0x06000A71 RID: 2673
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugSphereDelegate(Vec3 position, float radius, uint color, [MarshalAs(UnmanagedType.U1)] bool depthCheck, float time);

		// Token: 0x02000117 RID: 279
		// (Invoke) Token: 0x06000A75 RID: 2677
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugTextDelegate(float screenX, float screenY, byte[] str, uint color, float time);

		// Token: 0x02000118 RID: 280
		// (Invoke) Token: 0x06000A79 RID: 2681
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderDebugText3dDelegate(Vec3 worldPosition, byte[] str, uint color, int screenPosOffsetX, int screenPosOffsetY, float time);

		// Token: 0x02000119 RID: 281
		// (Invoke) Token: 0x06000A7D RID: 2685
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDebugVectorDelegate(Vec3 debugVector);

		// Token: 0x0200011A RID: 282
		// (Invoke) Token: 0x06000A81 RID: 2689
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDumpGenerationDisabledDelegate([MarshalAs(UnmanagedType.U1)] bool Disabled);

		// Token: 0x0200011B RID: 283
		// (Invoke) Token: 0x06000A85 RID: 2693
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetErrorReportSceneDelegate(UIntPtr scenePointer);

		// Token: 0x0200011C RID: 284
		// (Invoke) Token: 0x06000A89 RID: 2697
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetShowDebugInfoDelegate(int value);

		// Token: 0x0200011D RID: 285
		// (Invoke) Token: 0x06000A8D RID: 2701
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool SilentAssertDelegate(byte[] messageString, byte[] callerFile, byte[] callerMethod, int callerLine, [MarshalAs(UnmanagedType.U1)] bool getDump);

		// Token: 0x0200011E RID: 286
		// (Invoke) Token: 0x06000A91 RID: 2705
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool WarningDelegate(byte[] MessageString);

		// Token: 0x0200011F RID: 287
		// (Invoke) Token: 0x06000A95 RID: 2709
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteDebugLineOnScreenDelegate(byte[] line);

		// Token: 0x02000120 RID: 288
		// (Invoke) Token: 0x06000A99 RID: 2713
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WriteLineDelegate(int logLevel, byte[] line, int color, ulong filter);
	}
}
