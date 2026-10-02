using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000012 RID: 18
	internal class ScriptingInterfaceOfIMBEditor : IMBEditor
	{
		// Token: 0x0600020F RID: 527 RVA: 0x0000B168 File Offset: 0x00009368
		public void ActivateSceneEditorPresentation()
		{
			ScriptingInterfaceOfIMBEditor.call_ActivateSceneEditorPresentationDelegate();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000B174 File Offset: 0x00009374
		public void AddEditorWarning(string msg)
		{
			byte[] array = null;
			if (msg != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(msg);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(msg, 0, msg.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_AddEditorWarningDelegate(array);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000B1D0 File Offset: 0x000093D0
		public void AddEntityWarning(UIntPtr entityId, string msg)
		{
			byte[] array = null;
			if (msg != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(msg);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(msg, 0, msg.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_AddEntityWarningDelegate(entityId, array);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B22C File Offset: 0x0000942C
		public void AddNavMeshWarning(UIntPtr sceneId, in PathFaceRecord record, string msg)
		{
			byte[] array = null;
			if (msg != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(msg);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(msg, 0, msg.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_AddNavMeshWarningDelegate(sceneId, in record, array);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B288 File Offset: 0x00009488
		public void ApplyDeltaToEditorCamera(in Vec3 delta)
		{
			ScriptingInterfaceOfIMBEditor.call_ApplyDeltaToEditorCameraDelegate(in delta);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000B295 File Offset: 0x00009495
		public bool BorderHelpersEnabled()
		{
			return ScriptingInterfaceOfIMBEditor.call_BorderHelpersEnabledDelegate();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B2A1 File Offset: 0x000094A1
		public void DeactivateSceneEditorPresentation()
		{
			ScriptingInterfaceOfIMBEditor.call_DeactivateSceneEditorPresentationDelegate();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B2AD File Offset: 0x000094AD
		public void EnterEditMissionMode(UIntPtr missionPointer)
		{
			ScriptingInterfaceOfIMBEditor.call_EnterEditMissionModeDelegate(missionPointer);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B2BA File Offset: 0x000094BA
		public void EnterEditMode(UIntPtr sceneWidgetPointer, ref MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing)
		{
			ScriptingInterfaceOfIMBEditor.call_EnterEditModeDelegate(sceneWidgetPointer, ref initialCameraFrame, initialCameraElevation, initialCameraBearing);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000B2CB File Offset: 0x000094CB
		public void ExitEditMode()
		{
			ScriptingInterfaceOfIMBEditor.call_ExitEditModeDelegate();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000B2D8 File Offset: 0x000094D8
		public string GetAllPrefabsAndChildWithTag(string tag)
		{
			byte[] array = null;
			if (tag != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(tag);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(tag, 0, tag.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIMBEditor.call_GetAllPrefabsAndChildWithTagDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000B33C File Offset: 0x0000953C
		public SceneView GetEditorSceneView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBEditor.call_GetEditorSceneViewDelegate();
			SceneView sceneView = NativeObject.CreateNativeObjectWrapper<SceneView>(nativeObjectPointer);
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return sceneView;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B37C File Offset: 0x0000957C
		public bool HelpersEnabled()
		{
			return ScriptingInterfaceOfIMBEditor.call_HelpersEnabledDelegate();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B388 File Offset: 0x00009588
		public bool IsEditMode()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsEditModeDelegate();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000B394 File Offset: 0x00009594
		public bool IsEditModeEnabled()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsEditModeEnabledDelegate();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public bool IsEntitySelected(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIMBEditor.call_IsEntitySelectedDelegate(entityId);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B3AD File Offset: 0x000095AD
		public bool IsReplayManagerRecording()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsReplayManagerRecordingDelegate();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B3B9 File Offset: 0x000095B9
		public bool IsReplayManagerRendering()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsReplayManagerRenderingDelegate();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B3C5 File Offset: 0x000095C5
		public bool IsReplayManagerReplaying()
		{
			return ScriptingInterfaceOfIMBEditor.call_IsReplayManagerReplayingDelegate();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B3D1 File Offset: 0x000095D1
		public void LeaveEditMissionMode()
		{
			ScriptingInterfaceOfIMBEditor.call_LeaveEditMissionModeDelegate();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B3DD File Offset: 0x000095DD
		public void LeaveEditMode()
		{
			ScriptingInterfaceOfIMBEditor.call_LeaveEditModeDelegate();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000B3E9 File Offset: 0x000095E9
		public void RenderEditorMesh(UIntPtr metaMeshId, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIMBEditor.call_RenderEditorMeshDelegate(metaMeshId, ref frame);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B3F8 File Offset: 0x000095F8
		public void SetLevelVisibility(string cumulated_string)
		{
			byte[] array = null;
			if (cumulated_string != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(cumulated_string);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(cumulated_string, 0, cumulated_string.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_SetLevelVisibilityDelegate(array);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B454 File Offset: 0x00009654
		public void SetUpgradeLevelVisibility(string cumulated_string)
		{
			byte[] array = null;
			if (cumulated_string != null)
			{
				int byteCount = ScriptingInterfaceOfIMBEditor._utf8.GetByteCount(cumulated_string);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBEditor._utf8.GetBytes(cumulated_string, 0, cumulated_string.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBEditor.call_SetUpgradeLevelVisibilityDelegate(array);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B4AE File Offset: 0x000096AE
		public void TickEditMode(float dt)
		{
			ScriptingInterfaceOfIMBEditor.call_TickEditModeDelegate(dt);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000B4BB File Offset: 0x000096BB
		public void TickSceneEditorPresentation(float dt)
		{
			ScriptingInterfaceOfIMBEditor.call_TickSceneEditorPresentationDelegate(dt);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public void ToggleEnableEditorPhysics()
		{
			ScriptingInterfaceOfIMBEditor.call_ToggleEnableEditorPhysicsDelegate();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B4D4 File Offset: 0x000096D4
		public void UpdateSceneTree(bool do_next_frame)
		{
			ScriptingInterfaceOfIMBEditor.call_UpdateSceneTreeDelegate(do_next_frame);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B4E1 File Offset: 0x000096E1
		public void ZoomToPosition(Vec3 pos)
		{
			ScriptingInterfaceOfIMBEditor.call_ZoomToPositionDelegate(pos);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000B502 File Offset: 0x00009702
		void IMBEditor.ApplyDeltaToEditorCamera(in Vec3 delta)
		{
			this.ApplyDeltaToEditorCamera(in delta);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000B50B File Offset: 0x0000970B
		void IMBEditor.AddNavMeshWarning(UIntPtr sceneId, in PathFaceRecord record, string msg)
		{
			this.AddNavMeshWarning(sceneId, in record, msg);
		}

		// Token: 0x04000196 RID: 406
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000197 RID: 407
		public static ScriptingInterfaceOfIMBEditor.ActivateSceneEditorPresentationDelegate call_ActivateSceneEditorPresentationDelegate;

		// Token: 0x04000198 RID: 408
		public static ScriptingInterfaceOfIMBEditor.AddEditorWarningDelegate call_AddEditorWarningDelegate;

		// Token: 0x04000199 RID: 409
		public static ScriptingInterfaceOfIMBEditor.AddEntityWarningDelegate call_AddEntityWarningDelegate;

		// Token: 0x0400019A RID: 410
		public static ScriptingInterfaceOfIMBEditor.AddNavMeshWarningDelegate call_AddNavMeshWarningDelegate;

		// Token: 0x0400019B RID: 411
		public static ScriptingInterfaceOfIMBEditor.ApplyDeltaToEditorCameraDelegate call_ApplyDeltaToEditorCameraDelegate;

		// Token: 0x0400019C RID: 412
		public static ScriptingInterfaceOfIMBEditor.BorderHelpersEnabledDelegate call_BorderHelpersEnabledDelegate;

		// Token: 0x0400019D RID: 413
		public static ScriptingInterfaceOfIMBEditor.DeactivateSceneEditorPresentationDelegate call_DeactivateSceneEditorPresentationDelegate;

		// Token: 0x0400019E RID: 414
		public static ScriptingInterfaceOfIMBEditor.EnterEditMissionModeDelegate call_EnterEditMissionModeDelegate;

		// Token: 0x0400019F RID: 415
		public static ScriptingInterfaceOfIMBEditor.EnterEditModeDelegate call_EnterEditModeDelegate;

		// Token: 0x040001A0 RID: 416
		public static ScriptingInterfaceOfIMBEditor.ExitEditModeDelegate call_ExitEditModeDelegate;

		// Token: 0x040001A1 RID: 417
		public static ScriptingInterfaceOfIMBEditor.GetAllPrefabsAndChildWithTagDelegate call_GetAllPrefabsAndChildWithTagDelegate;

		// Token: 0x040001A2 RID: 418
		public static ScriptingInterfaceOfIMBEditor.GetEditorSceneViewDelegate call_GetEditorSceneViewDelegate;

		// Token: 0x040001A3 RID: 419
		public static ScriptingInterfaceOfIMBEditor.HelpersEnabledDelegate call_HelpersEnabledDelegate;

		// Token: 0x040001A4 RID: 420
		public static ScriptingInterfaceOfIMBEditor.IsEditModeDelegate call_IsEditModeDelegate;

		// Token: 0x040001A5 RID: 421
		public static ScriptingInterfaceOfIMBEditor.IsEditModeEnabledDelegate call_IsEditModeEnabledDelegate;

		// Token: 0x040001A6 RID: 422
		public static ScriptingInterfaceOfIMBEditor.IsEntitySelectedDelegate call_IsEntitySelectedDelegate;

		// Token: 0x040001A7 RID: 423
		public static ScriptingInterfaceOfIMBEditor.IsReplayManagerRecordingDelegate call_IsReplayManagerRecordingDelegate;

		// Token: 0x040001A8 RID: 424
		public static ScriptingInterfaceOfIMBEditor.IsReplayManagerRenderingDelegate call_IsReplayManagerRenderingDelegate;

		// Token: 0x040001A9 RID: 425
		public static ScriptingInterfaceOfIMBEditor.IsReplayManagerReplayingDelegate call_IsReplayManagerReplayingDelegate;

		// Token: 0x040001AA RID: 426
		public static ScriptingInterfaceOfIMBEditor.LeaveEditMissionModeDelegate call_LeaveEditMissionModeDelegate;

		// Token: 0x040001AB RID: 427
		public static ScriptingInterfaceOfIMBEditor.LeaveEditModeDelegate call_LeaveEditModeDelegate;

		// Token: 0x040001AC RID: 428
		public static ScriptingInterfaceOfIMBEditor.RenderEditorMeshDelegate call_RenderEditorMeshDelegate;

		// Token: 0x040001AD RID: 429
		public static ScriptingInterfaceOfIMBEditor.SetLevelVisibilityDelegate call_SetLevelVisibilityDelegate;

		// Token: 0x040001AE RID: 430
		public static ScriptingInterfaceOfIMBEditor.SetUpgradeLevelVisibilityDelegate call_SetUpgradeLevelVisibilityDelegate;

		// Token: 0x040001AF RID: 431
		public static ScriptingInterfaceOfIMBEditor.TickEditModeDelegate call_TickEditModeDelegate;

		// Token: 0x040001B0 RID: 432
		public static ScriptingInterfaceOfIMBEditor.TickSceneEditorPresentationDelegate call_TickSceneEditorPresentationDelegate;

		// Token: 0x040001B1 RID: 433
		public static ScriptingInterfaceOfIMBEditor.ToggleEnableEditorPhysicsDelegate call_ToggleEnableEditorPhysicsDelegate;

		// Token: 0x040001B2 RID: 434
		public static ScriptingInterfaceOfIMBEditor.UpdateSceneTreeDelegate call_UpdateSceneTreeDelegate;

		// Token: 0x040001B3 RID: 435
		public static ScriptingInterfaceOfIMBEditor.ZoomToPositionDelegate call_ZoomToPositionDelegate;

		// Token: 0x020001FE RID: 510
		// (Invoke) Token: 0x06000AE6 RID: 2790
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ActivateSceneEditorPresentationDelegate();

		// Token: 0x020001FF RID: 511
		// (Invoke) Token: 0x06000AEA RID: 2794
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddEditorWarningDelegate(byte[] msg);

		// Token: 0x02000200 RID: 512
		// (Invoke) Token: 0x06000AEE RID: 2798
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddEntityWarningDelegate(UIntPtr entityId, byte[] msg);

		// Token: 0x02000201 RID: 513
		// (Invoke) Token: 0x06000AF2 RID: 2802
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNavMeshWarningDelegate(UIntPtr sceneId, in PathFaceRecord record, byte[] msg);

		// Token: 0x02000202 RID: 514
		// (Invoke) Token: 0x06000AF6 RID: 2806
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyDeltaToEditorCameraDelegate(in Vec3 delta);

		// Token: 0x02000203 RID: 515
		// (Invoke) Token: 0x06000AFA RID: 2810
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool BorderHelpersEnabledDelegate();

		// Token: 0x02000204 RID: 516
		// (Invoke) Token: 0x06000AFE RID: 2814
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeactivateSceneEditorPresentationDelegate();

		// Token: 0x02000205 RID: 517
		// (Invoke) Token: 0x06000B02 RID: 2818
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnterEditMissionModeDelegate(UIntPtr missionPointer);

		// Token: 0x02000206 RID: 518
		// (Invoke) Token: 0x06000B06 RID: 2822
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnterEditModeDelegate(UIntPtr sceneWidgetPointer, ref MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing);

		// Token: 0x02000207 RID: 519
		// (Invoke) Token: 0x06000B0A RID: 2826
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ExitEditModeDelegate();

		// Token: 0x02000208 RID: 520
		// (Invoke) Token: 0x06000B0E RID: 2830
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAllPrefabsAndChildWithTagDelegate(byte[] tag);

		// Token: 0x02000209 RID: 521
		// (Invoke) Token: 0x06000B12 RID: 2834
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetEditorSceneViewDelegate();

		// Token: 0x0200020A RID: 522
		// (Invoke) Token: 0x06000B16 RID: 2838
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HelpersEnabledDelegate();

		// Token: 0x0200020B RID: 523
		// (Invoke) Token: 0x06000B1A RID: 2842
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEditModeDelegate();

		// Token: 0x0200020C RID: 524
		// (Invoke) Token: 0x06000B1E RID: 2846
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEditModeEnabledDelegate();

		// Token: 0x0200020D RID: 525
		// (Invoke) Token: 0x06000B22 RID: 2850
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEntitySelectedDelegate(UIntPtr entityId);

		// Token: 0x0200020E RID: 526
		// (Invoke) Token: 0x06000B26 RID: 2854
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsReplayManagerRecordingDelegate();

		// Token: 0x0200020F RID: 527
		// (Invoke) Token: 0x06000B2A RID: 2858
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsReplayManagerRenderingDelegate();

		// Token: 0x02000210 RID: 528
		// (Invoke) Token: 0x06000B2E RID: 2862
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsReplayManagerReplayingDelegate();

		// Token: 0x02000211 RID: 529
		// (Invoke) Token: 0x06000B32 RID: 2866
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LeaveEditMissionModeDelegate();

		// Token: 0x02000212 RID: 530
		// (Invoke) Token: 0x06000B36 RID: 2870
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LeaveEditModeDelegate();

		// Token: 0x02000213 RID: 531
		// (Invoke) Token: 0x06000B3A RID: 2874
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderEditorMeshDelegate(UIntPtr metaMeshId, ref MatrixFrame frame);

		// Token: 0x02000214 RID: 532
		// (Invoke) Token: 0x06000B3E RID: 2878
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLevelVisibilityDelegate(byte[] cumulated_string);

		// Token: 0x02000215 RID: 533
		// (Invoke) Token: 0x06000B42 RID: 2882
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUpgradeLevelVisibilityDelegate(byte[] cumulated_string);

		// Token: 0x02000216 RID: 534
		// (Invoke) Token: 0x06000B46 RID: 2886
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickEditModeDelegate(float dt);

		// Token: 0x02000217 RID: 535
		// (Invoke) Token: 0x06000B4A RID: 2890
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickSceneEditorPresentationDelegate(float dt);

		// Token: 0x02000218 RID: 536
		// (Invoke) Token: 0x06000B4E RID: 2894
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ToggleEnableEditorPhysicsDelegate();

		// Token: 0x02000219 RID: 537
		// (Invoke) Token: 0x06000B52 RID: 2898
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateSceneTreeDelegate([MarshalAs(UnmanagedType.U1)] bool do_next_frame);

		// Token: 0x0200021A RID: 538
		// (Invoke) Token: 0x06000B56 RID: 2902
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ZoomToPositionDelegate(Vec3 pos);
	}
}
