using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000006 RID: 6
	internal static class EngineCallbacksGenerated
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020A9 File Offset: 0x000002A9
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000020B0 File Offset: 0x000002B0
		internal static Delegate[] Delegates { get; private set; }

		// Token: 0x06000010 RID: 16 RVA: 0x000020B8 File Offset: 0x000002B8
		public static void Initialize()
		{
			EngineCallbacksGenerated.Delegates = new Delegate[85];
			EngineCallbacksGenerated.Delegates[0] = new EngineCallbacksGenerated.CrashInformationCollector_CollectInformation_delegate(EngineCallbacksGenerated.CrashInformationCollector_CollectInformation);
			EngineCallbacksGenerated.Delegates[1] = new EngineCallbacksGenerated.EngineController_GetApplicationPlatformName_delegate(EngineCallbacksGenerated.EngineController_GetApplicationPlatformName);
			EngineCallbacksGenerated.Delegates[2] = new EngineCallbacksGenerated.EngineController_GetModulesVersionStr_delegate(EngineCallbacksGenerated.EngineController_GetModulesVersionStr);
			EngineCallbacksGenerated.Delegates[3] = new EngineCallbacksGenerated.EngineController_GetVersionStr_delegate(EngineCallbacksGenerated.EngineController_GetVersionStr);
			EngineCallbacksGenerated.Delegates[4] = new EngineCallbacksGenerated.EngineController_Initialize_delegate(EngineCallbacksGenerated.EngineController_Initialize);
			EngineCallbacksGenerated.Delegates[5] = new EngineCallbacksGenerated.EngineController_OnConfigChange_delegate(EngineCallbacksGenerated.EngineController_OnConfigChange);
			EngineCallbacksGenerated.Delegates[6] = new EngineCallbacksGenerated.EngineController_OnConstrainedStateChange_delegate(EngineCallbacksGenerated.EngineController_OnConstrainedStateChange);
			EngineCallbacksGenerated.Delegates[7] = new EngineCallbacksGenerated.EngineController_OnControllerDisconnection_delegate(EngineCallbacksGenerated.EngineController_OnControllerDisconnection);
			EngineCallbacksGenerated.Delegates[8] = new EngineCallbacksGenerated.EngineController_OnDLCInstalled_delegate(EngineCallbacksGenerated.EngineController_OnDLCInstalled);
			EngineCallbacksGenerated.Delegates[9] = new EngineCallbacksGenerated.EngineController_OnDLCLoaded_delegate(EngineCallbacksGenerated.EngineController_OnDLCLoaded);
			EngineCallbacksGenerated.Delegates[10] = new EngineCallbacksGenerated.EngineManaged_CheckSharedStructureSizes_delegate(EngineCallbacksGenerated.EngineManaged_CheckSharedStructureSizes);
			EngineCallbacksGenerated.Delegates[11] = new EngineCallbacksGenerated.EngineManaged_EngineApiMethodInterfaceInitializer_delegate(EngineCallbacksGenerated.EngineManaged_EngineApiMethodInterfaceInitializer);
			EngineCallbacksGenerated.Delegates[12] = new EngineCallbacksGenerated.EngineManaged_FillEngineApiPointers_delegate(EngineCallbacksGenerated.EngineManaged_FillEngineApiPointers);
			EngineCallbacksGenerated.Delegates[13] = new EngineCallbacksGenerated.EngineScreenManager_InitializeLastPressedKeys_delegate(EngineCallbacksGenerated.EngineScreenManager_InitializeLastPressedKeys);
			EngineCallbacksGenerated.Delegates[14] = new EngineCallbacksGenerated.EngineScreenManager_LateTick_delegate(EngineCallbacksGenerated.EngineScreenManager_LateTick);
			EngineCallbacksGenerated.Delegates[15] = new EngineCallbacksGenerated.EngineScreenManager_OnGameWindowFocusChange_delegate(EngineCallbacksGenerated.EngineScreenManager_OnGameWindowFocusChange);
			EngineCallbacksGenerated.Delegates[16] = new EngineCallbacksGenerated.EngineScreenManager_OnOnscreenKeyboardCanceled_delegate(EngineCallbacksGenerated.EngineScreenManager_OnOnscreenKeyboardCanceled);
			EngineCallbacksGenerated.Delegates[17] = new EngineCallbacksGenerated.EngineScreenManager_OnOnscreenKeyboardDone_delegate(EngineCallbacksGenerated.EngineScreenManager_OnOnscreenKeyboardDone);
			EngineCallbacksGenerated.Delegates[18] = new EngineCallbacksGenerated.EngineScreenManager_PreTick_delegate(EngineCallbacksGenerated.EngineScreenManager_PreTick);
			EngineCallbacksGenerated.Delegates[19] = new EngineCallbacksGenerated.EngineScreenManager_Tick_delegate(EngineCallbacksGenerated.EngineScreenManager_Tick);
			EngineCallbacksGenerated.Delegates[20] = new EngineCallbacksGenerated.EngineScreenManager_Update_delegate(EngineCallbacksGenerated.EngineScreenManager_Update);
			EngineCallbacksGenerated.Delegates[21] = new EngineCallbacksGenerated.ManagedExtensions_CollectCommandLineFunctions_delegate(EngineCallbacksGenerated.ManagedExtensions_CollectCommandLineFunctions);
			EngineCallbacksGenerated.Delegates[22] = new EngineCallbacksGenerated.ManagedExtensions_CopyObjectFieldsFrom_delegate(EngineCallbacksGenerated.ManagedExtensions_CopyObjectFieldsFrom);
			EngineCallbacksGenerated.Delegates[23] = new EngineCallbacksGenerated.ManagedExtensions_CreateScriptComponentInstance_delegate(EngineCallbacksGenerated.ManagedExtensions_CreateScriptComponentInstance);
			EngineCallbacksGenerated.Delegates[24] = new EngineCallbacksGenerated.ManagedExtensions_ForceGarbageCollect_delegate(EngineCallbacksGenerated.ManagedExtensions_ForceGarbageCollect);
			EngineCallbacksGenerated.Delegates[25] = new EngineCallbacksGenerated.ManagedExtensions_GetEditorVisibilityOfField_delegate(EngineCallbacksGenerated.ManagedExtensions_GetEditorVisibilityOfField);
			EngineCallbacksGenerated.Delegates[26] = new EngineCallbacksGenerated.ManagedExtensions_GetObjectField_delegate(EngineCallbacksGenerated.ManagedExtensions_GetObjectField);
			EngineCallbacksGenerated.Delegates[27] = new EngineCallbacksGenerated.ManagedExtensions_GetScriptComponentClassNames_delegate(EngineCallbacksGenerated.ManagedExtensions_GetScriptComponentClassNames);
			EngineCallbacksGenerated.Delegates[28] = new EngineCallbacksGenerated.ManagedExtensions_GetTypeOfField_delegate(EngineCallbacksGenerated.ManagedExtensions_GetTypeOfField);
			EngineCallbacksGenerated.Delegates[29] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldBool_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldBool);
			EngineCallbacksGenerated.Delegates[30] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldColor_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldColor);
			EngineCallbacksGenerated.Delegates[31] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldDouble_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldDouble);
			EngineCallbacksGenerated.Delegates[32] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldEntity_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldEntity);
			EngineCallbacksGenerated.Delegates[33] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldEnum_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldEnum);
			EngineCallbacksGenerated.Delegates[34] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldFloat_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldFloat);
			EngineCallbacksGenerated.Delegates[35] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldInt_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldInt);
			EngineCallbacksGenerated.Delegates[36] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMaterial_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMaterial);
			EngineCallbacksGenerated.Delegates[37] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMatrixFrame_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMatrixFrame);
			EngineCallbacksGenerated.Delegates[38] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMesh_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMesh);
			EngineCallbacksGenerated.Delegates[39] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldString_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldString);
			EngineCallbacksGenerated.Delegates[40] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldTexture_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldTexture);
			EngineCallbacksGenerated.Delegates[41] = new EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldVec3_delegate(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldVec3);
			EngineCallbacksGenerated.Delegates[42] = new EngineCallbacksGenerated.ManagedScriptHolder_CreateManagedScriptHolder_delegate(EngineCallbacksGenerated.ManagedScriptHolder_CreateManagedScriptHolder);
			EngineCallbacksGenerated.Delegates[43] = new EngineCallbacksGenerated.ManagedScriptHolder_FixedTickComponents_delegate(EngineCallbacksGenerated.ManagedScriptHolder_FixedTickComponents);
			EngineCallbacksGenerated.Delegates[44] = new EngineCallbacksGenerated.ManagedScriptHolder_GetNumberOfScripts_delegate(EngineCallbacksGenerated.ManagedScriptHolder_GetNumberOfScripts);
			EngineCallbacksGenerated.Delegates[45] = new EngineCallbacksGenerated.ManagedScriptHolder_RemoveScriptComponentFromAllTickLists_delegate(EngineCallbacksGenerated.ManagedScriptHolder_RemoveScriptComponentFromAllTickLists);
			EngineCallbacksGenerated.Delegates[46] = new EngineCallbacksGenerated.ManagedScriptHolder_SetScriptComponentHolder_delegate(EngineCallbacksGenerated.ManagedScriptHolder_SetScriptComponentHolder);
			EngineCallbacksGenerated.Delegates[47] = new EngineCallbacksGenerated.ManagedScriptHolder_TickComponents_delegate(EngineCallbacksGenerated.ManagedScriptHolder_TickComponents);
			EngineCallbacksGenerated.Delegates[48] = new EngineCallbacksGenerated.ManagedScriptHolder_TickComponentsEditor_delegate(EngineCallbacksGenerated.ManagedScriptHolder_TickComponentsEditor);
			EngineCallbacksGenerated.Delegates[49] = new EngineCallbacksGenerated.MessageManagerBase_PostMessageLine_delegate(EngineCallbacksGenerated.MessageManagerBase_PostMessageLine);
			EngineCallbacksGenerated.Delegates[50] = new EngineCallbacksGenerated.MessageManagerBase_PostMessageLineFormatted_delegate(EngineCallbacksGenerated.MessageManagerBase_PostMessageLineFormatted);
			EngineCallbacksGenerated.Delegates[51] = new EngineCallbacksGenerated.MessageManagerBase_PostSuccessLine_delegate(EngineCallbacksGenerated.MessageManagerBase_PostSuccessLine);
			EngineCallbacksGenerated.Delegates[52] = new EngineCallbacksGenerated.MessageManagerBase_PostWarningLine_delegate(EngineCallbacksGenerated.MessageManagerBase_PostWarningLine);
			EngineCallbacksGenerated.Delegates[53] = new EngineCallbacksGenerated.NativeParallelDriver_ParalelForLoopBodyCaller_delegate(EngineCallbacksGenerated.NativeParallelDriver_ParalelForLoopBodyCaller);
			EngineCallbacksGenerated.Delegates[54] = new EngineCallbacksGenerated.NativeParallelDriver_ParalelForLoopBodyWithDtCaller_delegate(EngineCallbacksGenerated.NativeParallelDriver_ParalelForLoopBodyWithDtCaller);
			EngineCallbacksGenerated.Delegates[55] = new EngineCallbacksGenerated.RenderTargetComponent_CreateRenderTargetComponent_delegate(EngineCallbacksGenerated.RenderTargetComponent_CreateRenderTargetComponent);
			EngineCallbacksGenerated.Delegates[56] = new EngineCallbacksGenerated.RenderTargetComponent_OnPaintNeeded_delegate(EngineCallbacksGenerated.RenderTargetComponent_OnPaintNeeded);
			EngineCallbacksGenerated.Delegates[57] = new EngineCallbacksGenerated.SceneProblemChecker_OnCheckForSceneProblems_delegate(EngineCallbacksGenerated.SceneProblemChecker_OnCheckForSceneProblems);
			EngineCallbacksGenerated.Delegates[58] = new EngineCallbacksGenerated.ScriptComponentBehavior_AddScriptComponentToTick_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_AddScriptComponentToTick);
			EngineCallbacksGenerated.Delegates[59] = new EngineCallbacksGenerated.ScriptComponentBehavior_DeregisterAsPrefabScriptComponent_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_DeregisterAsPrefabScriptComponent);
			EngineCallbacksGenerated.Delegates[60] = new EngineCallbacksGenerated.ScriptComponentBehavior_DeregisterAsUndoStackScriptComponent_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_DeregisterAsUndoStackScriptComponent);
			EngineCallbacksGenerated.Delegates[61] = new EngineCallbacksGenerated.ScriptComponentBehavior_DisablesOroCreation_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_DisablesOroCreation);
			EngineCallbacksGenerated.Delegates[62] = new EngineCallbacksGenerated.ScriptComponentBehavior_GetEditableFields_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_GetEditableFields);
			EngineCallbacksGenerated.Delegates[63] = new EngineCallbacksGenerated.ScriptComponentBehavior_HandleOnRemoved_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_HandleOnRemoved);
			EngineCallbacksGenerated.Delegates[64] = new EngineCallbacksGenerated.ScriptComponentBehavior_IsOnlyVisual_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_IsOnlyVisual);
			EngineCallbacksGenerated.Delegates[65] = new EngineCallbacksGenerated.ScriptComponentBehavior_MovesEntity_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_MovesEntity);
			EngineCallbacksGenerated.Delegates[66] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnBoundingBoxValidate_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnBoundingBoxValidate);
			EngineCallbacksGenerated.Delegates[67] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnCheckForProblems_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnCheckForProblems);
			EngineCallbacksGenerated.Delegates[68] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnDynamicNavmeshVertexUpdate_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnDynamicNavmeshVertexUpdate);
			EngineCallbacksGenerated.Delegates[69] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnEditModeVisibilityChanged_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditModeVisibilityChanged);
			EngineCallbacksGenerated.Delegates[70] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorInit_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorInit);
			EngineCallbacksGenerated.Delegates[71] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorTick_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorTick);
			EngineCallbacksGenerated.Delegates[72] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorValidate_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorValidate);
			EngineCallbacksGenerated.Delegates[73] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorVariableChanged_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorVariableChanged);
			EngineCallbacksGenerated.Delegates[74] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnInit_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnInit);
			EngineCallbacksGenerated.Delegates[75] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnPhysicsCollisionAux_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnPhysicsCollisionAux);
			EngineCallbacksGenerated.Delegates[76] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnPreInit_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnPreInit);
			EngineCallbacksGenerated.Delegates[77] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnSaveAsPrefab_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnSaveAsPrefab);
			EngineCallbacksGenerated.Delegates[78] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnSceneSave_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnSceneSave);
			EngineCallbacksGenerated.Delegates[79] = new EngineCallbacksGenerated.ScriptComponentBehavior_OnTerrainReload_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_OnTerrainReload);
			EngineCallbacksGenerated.Delegates[80] = new EngineCallbacksGenerated.ScriptComponentBehavior_RegisterAsPrefabScriptComponent_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_RegisterAsPrefabScriptComponent);
			EngineCallbacksGenerated.Delegates[81] = new EngineCallbacksGenerated.ScriptComponentBehavior_RegisterAsUndoStackScriptComponent_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_RegisterAsUndoStackScriptComponent);
			EngineCallbacksGenerated.Delegates[82] = new EngineCallbacksGenerated.ScriptComponentBehavior_SetScene_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_SetScene);
			EngineCallbacksGenerated.Delegates[83] = new EngineCallbacksGenerated.ScriptComponentBehavior_SkeletonPostIntegrateCallbackAux_delegate(EngineCallbacksGenerated.ScriptComponentBehavior_SkeletonPostIntegrateCallbackAux);
			EngineCallbacksGenerated.Delegates[84] = new EngineCallbacksGenerated.ThumbnailCreatorView_OnThumbnailRenderComplete_delegate(EngineCallbacksGenerated.ThumbnailCreatorView_OnThumbnailRenderComplete);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000276C File Offset: 0x0000096C
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.CrashInformationCollector_CollectInformation_delegate))]
		internal static UIntPtr CrashInformationCollector_CollectInformation()
		{
			string text = CrashInformationCollector.CollectInformation();
			UIntPtr threadLocalCachedRglVarString = NativeStringHelper.GetThreadLocalCachedRglVarString();
			NativeStringHelper.SetRglVarString(threadLocalCachedRglVarString, text);
			return threadLocalCachedRglVarString;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000278C File Offset: 0x0000098C
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_GetApplicationPlatformName_delegate))]
		internal static UIntPtr EngineController_GetApplicationPlatformName()
		{
			string applicationPlatformName = EngineController.GetApplicationPlatformName();
			UIntPtr threadLocalCachedRglVarString = NativeStringHelper.GetThreadLocalCachedRglVarString();
			NativeStringHelper.SetRglVarString(threadLocalCachedRglVarString, applicationPlatformName);
			return threadLocalCachedRglVarString;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000027AC File Offset: 0x000009AC
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_GetModulesVersionStr_delegate))]
		internal static UIntPtr EngineController_GetModulesVersionStr()
		{
			string modulesVersionStr = EngineController.GetModulesVersionStr();
			UIntPtr threadLocalCachedRglVarString = NativeStringHelper.GetThreadLocalCachedRglVarString();
			NativeStringHelper.SetRglVarString(threadLocalCachedRglVarString, modulesVersionStr);
			return threadLocalCachedRglVarString;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000027CC File Offset: 0x000009CC
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_GetVersionStr_delegate))]
		internal static UIntPtr EngineController_GetVersionStr()
		{
			string versionStr = EngineController.GetVersionStr();
			UIntPtr threadLocalCachedRglVarString = NativeStringHelper.GetThreadLocalCachedRglVarString();
			NativeStringHelper.SetRglVarString(threadLocalCachedRglVarString, versionStr);
			return threadLocalCachedRglVarString;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000027EB File Offset: 0x000009EB
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_Initialize_delegate))]
		internal static void EngineController_Initialize()
		{
			EngineController.Initialize();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000027F2 File Offset: 0x000009F2
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_OnConfigChange_delegate))]
		internal static void EngineController_OnConfigChange()
		{
			EngineController.OnConfigChange();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000027F9 File Offset: 0x000009F9
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_OnConstrainedStateChange_delegate))]
		internal static void EngineController_OnConstrainedStateChange(bool isConstrained)
		{
			EngineController.OnConstrainedStateChange(isConstrained);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002801 File Offset: 0x00000A01
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_OnControllerDisconnection_delegate))]
		internal static void EngineController_OnControllerDisconnection()
		{
			EngineController.OnControllerDisconnection();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002808 File Offset: 0x00000A08
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_OnDLCInstalled_delegate))]
		internal static void EngineController_OnDLCInstalled()
		{
			EngineController.OnDLCInstalled();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000280F File Offset: 0x00000A0F
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineController_OnDLCLoaded_delegate))]
		internal static void EngineController_OnDLCLoaded()
		{
			EngineController.OnDLCLoaded();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002816 File Offset: 0x00000A16
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineManaged_CheckSharedStructureSizes_delegate))]
		internal static void EngineManaged_CheckSharedStructureSizes()
		{
			EngineManaged.CheckSharedStructureSizes();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000281D File Offset: 0x00000A1D
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineManaged_EngineApiMethodInterfaceInitializer_delegate))]
		internal static void EngineManaged_EngineApiMethodInterfaceInitializer(int id, IntPtr pointer)
		{
			EngineManaged.EngineApiMethodInterfaceInitializer(id, pointer);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002826 File Offset: 0x00000A26
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineManaged_FillEngineApiPointers_delegate))]
		internal static void EngineManaged_FillEngineApiPointers()
		{
			EngineManaged.FillEngineApiPointers();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002830 File Offset: 0x00000A30
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_InitializeLastPressedKeys_delegate))]
		internal static void EngineScreenManager_InitializeLastPressedKeys(NativeObjectPointer lastKeysPressed)
		{
			NativeArray nativeArray = null;
			if (lastKeysPressed.Pointer != UIntPtr.Zero)
			{
				nativeArray = new NativeArray(lastKeysPressed.Pointer);
			}
			EngineScreenManager.InitializeLastPressedKeys(nativeArray);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002863 File Offset: 0x00000A63
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_LateTick_delegate))]
		internal static void EngineScreenManager_LateTick(float dt)
		{
			EngineScreenManager.LateTick(dt);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000286B File Offset: 0x00000A6B
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_OnGameWindowFocusChange_delegate))]
		internal static void EngineScreenManager_OnGameWindowFocusChange(bool focusGained)
		{
			EngineScreenManager.OnGameWindowFocusChange(focusGained);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002873 File Offset: 0x00000A73
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_OnOnscreenKeyboardCanceled_delegate))]
		internal static void EngineScreenManager_OnOnscreenKeyboardCanceled()
		{
			EngineScreenManager.OnOnscreenKeyboardCanceled();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000287A File Offset: 0x00000A7A
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_OnOnscreenKeyboardDone_delegate))]
		internal static void EngineScreenManager_OnOnscreenKeyboardDone(IntPtr inputText)
		{
			EngineScreenManager.OnOnscreenKeyboardDone(Marshal.PtrToStringAnsi(inputText));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002887 File Offset: 0x00000A87
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_PreTick_delegate))]
		internal static void EngineScreenManager_PreTick(float dt)
		{
			EngineScreenManager.PreTick(dt);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000288F File Offset: 0x00000A8F
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_Tick_delegate))]
		internal static void EngineScreenManager_Tick(float dt)
		{
			EngineScreenManager.Tick(dt);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002897 File Offset: 0x00000A97
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.EngineScreenManager_Update_delegate))]
		internal static void EngineScreenManager_Update()
		{
			EngineScreenManager.Update();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000289E File Offset: 0x00000A9E
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_CollectCommandLineFunctions_delegate))]
		internal static void ManagedExtensions_CollectCommandLineFunctions()
		{
			ManagedExtensions.CollectCommandLineFunctions();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028A8 File Offset: 0x00000AA8
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_CopyObjectFieldsFrom_delegate))]
		internal static void ManagedExtensions_CopyObjectFieldsFrom(int dst, int src, IntPtr className, int callFieldChangeEventAsInteger)
		{
			DotNetObject managedObjectWithId = DotNetObject.GetManagedObjectWithId(dst);
			DotNetObject managedObjectWithId2 = DotNetObject.GetManagedObjectWithId(src);
			string text = Marshal.PtrToStringAnsi(className);
			ManagedExtensions.CopyObjectFieldsFrom(managedObjectWithId, managedObjectWithId2, text, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028D4 File Offset: 0x00000AD4
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_CreateScriptComponentInstance_delegate))]
		internal static int ManagedExtensions_CreateScriptComponentInstance(IntPtr className, UIntPtr entityPtr, NativeObjectPointer managedScriptComponent)
		{
			string text = Marshal.PtrToStringAnsi(className);
			ManagedScriptComponent managedScriptComponent2 = null;
			if (managedScriptComponent.Pointer != UIntPtr.Zero)
			{
				managedScriptComponent2 = new ManagedScriptComponent(managedScriptComponent.Pointer);
			}
			return ManagedExtensions.CreateScriptComponentInstance(text, entityPtr, managedScriptComponent2).GetManagedId();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002913 File Offset: 0x00000B13
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_ForceGarbageCollect_delegate))]
		internal static void ManagedExtensions_ForceGarbageCollect()
		{
			ManagedExtensions.ForceGarbageCollect();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000291A File Offset: 0x00000B1A
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_GetEditorVisibilityOfField_delegate))]
		internal static bool ManagedExtensions_GetEditorVisibilityOfField(uint classNameHash, uint fieldNamehash)
		{
			return ManagedExtensions.GetEditorVisibilityOfField(classNameHash, fieldNamehash);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002923 File Offset: 0x00000B23
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_GetObjectField_delegate))]
		internal static void ManagedExtensions_GetObjectField(int managedObject, uint classNameHash, ref ScriptComponentFieldHolder scriptComponentFieldHolder, uint fieldNameHash, RglScriptFieldType type)
		{
			ManagedExtensions.GetObjectField(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, ref scriptComponentFieldHolder, fieldNameHash, type);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002938 File Offset: 0x00000B38
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_GetScriptComponentClassNames_delegate))]
		internal static UIntPtr ManagedExtensions_GetScriptComponentClassNames()
		{
			string scriptComponentClassNames = ManagedExtensions.GetScriptComponentClassNames();
			UIntPtr threadLocalCachedRglVarString = NativeStringHelper.GetThreadLocalCachedRglVarString();
			NativeStringHelper.SetRglVarString(threadLocalCachedRglVarString, scriptComponentClassNames);
			return threadLocalCachedRglVarString;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002957 File Offset: 0x00000B57
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_GetTypeOfField_delegate))]
		internal static RglScriptFieldType ManagedExtensions_GetTypeOfField(uint classNameHash, uint fieldNameHash)
		{
			return ManagedExtensions.GetTypeOfField(classNameHash, fieldNameHash);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002960 File Offset: 0x00000B60
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldBool_delegate))]
		internal static void ManagedExtensions_SetObjectFieldBool(int managedObject, uint classNameHash, uint fieldNameHash, bool value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldBool(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002972 File Offset: 0x00000B72
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldColor_delegate))]
		internal static void ManagedExtensions_SetObjectFieldColor(int managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldColor(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002984 File Offset: 0x00000B84
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldDouble_delegate))]
		internal static void ManagedExtensions_SetObjectFieldDouble(int managedObject, uint classNameHash, uint fieldNameHash, double value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldDouble(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002996 File Offset: 0x00000B96
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldEntity_delegate))]
		internal static void ManagedExtensions_SetObjectFieldEntity(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldEntity(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000029A8 File Offset: 0x00000BA8
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldEnum_delegate))]
		internal static void ManagedExtensions_SetObjectFieldEnum(int managedObject, uint classNameHash, uint fieldNameHash, IntPtr value, int callFieldChangeEventAsInteger)
		{
			DotNetObject managedObjectWithId = DotNetObject.GetManagedObjectWithId(managedObject);
			string text = Marshal.PtrToStringAnsi(value);
			ManagedExtensions.SetObjectFieldEnum(managedObjectWithId, classNameHash, fieldNameHash, text, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000029CC File Offset: 0x00000BCC
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldFloat_delegate))]
		internal static void ManagedExtensions_SetObjectFieldFloat(int managedObject, uint classNameHash, uint fieldNameHash, float value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldFloat(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000029DE File Offset: 0x00000BDE
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldInt_delegate))]
		internal static void ManagedExtensions_SetObjectFieldInt(int managedObject, uint classNameHash, uint fieldNameHash, int value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldInt(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029F0 File Offset: 0x00000BF0
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMaterial_delegate))]
		internal static void ManagedExtensions_SetObjectFieldMaterial(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldMaterial(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A02 File Offset: 0x00000C02
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMatrixFrame_delegate))]
		internal static void ManagedExtensions_SetObjectFieldMatrixFrame(int managedObject, uint classNameHash, uint fieldNameHash, MatrixFrame value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldMatrixFrame(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A14 File Offset: 0x00000C14
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldMesh_delegate))]
		internal static void ManagedExtensions_SetObjectFieldMesh(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldMesh(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A28 File Offset: 0x00000C28
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldString_delegate))]
		internal static void ManagedExtensions_SetObjectFieldString(int managedObject, uint classNameHash, uint fieldNameHash, IntPtr value, int callFieldChangeEventAsInteger)
		{
			DotNetObject managedObjectWithId = DotNetObject.GetManagedObjectWithId(managedObject);
			string text = Marshal.PtrToStringAnsi(value);
			ManagedExtensions.SetObjectFieldString(managedObjectWithId, classNameHash, fieldNameHash, text, callFieldChangeEventAsInteger);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A4C File Offset: 0x00000C4C
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldTexture_delegate))]
		internal static void ManagedExtensions_SetObjectFieldTexture(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldTexture(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A5E File Offset: 0x00000C5E
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedExtensions_SetObjectFieldVec3_delegate))]
		internal static void ManagedExtensions_SetObjectFieldVec3(int managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger)
		{
			ManagedExtensions.SetObjectFieldVec3(DotNetObject.GetManagedObjectWithId(managedObject), classNameHash, fieldNameHash, value, callFieldChangeEventAsInteger);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A70 File Offset: 0x00000C70
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedScriptHolder_CreateManagedScriptHolder_delegate))]
		internal static int ManagedScriptHolder_CreateManagedScriptHolder()
		{
			return ManagedScriptHolder.CreateManagedScriptHolder().GetManagedId();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002A7C File Offset: 0x00000C7C
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedScriptHolder_FixedTickComponents_delegate))]
		internal static void ManagedScriptHolder_FixedTickComponents(int thisPointer, float fixedDt)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ManagedScriptHolder).FixedTickComponents(fixedDt);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A8F File Offset: 0x00000C8F
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedScriptHolder_GetNumberOfScripts_delegate))]
		internal static int ManagedScriptHolder_GetNumberOfScripts(int thisPointer)
		{
			return (DotNetObject.GetManagedObjectWithId(thisPointer) as ManagedScriptHolder).GetNumberOfScripts();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002AA4 File Offset: 0x00000CA4
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedScriptHolder_RemoveScriptComponentFromAllTickLists_delegate))]
		internal static void ManagedScriptHolder_RemoveScriptComponentFromAllTickLists(int thisPointer, int sc)
		{
			ManagedScriptHolder managedScriptHolder = DotNetObject.GetManagedObjectWithId(thisPointer) as ManagedScriptHolder;
			ScriptComponentBehavior scriptComponentBehavior = DotNetObject.GetManagedObjectWithId(sc) as ScriptComponentBehavior;
			managedScriptHolder.RemoveScriptComponentFromAllTickLists(scriptComponentBehavior);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002AD0 File Offset: 0x00000CD0
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedScriptHolder_SetScriptComponentHolder_delegate))]
		internal static void ManagedScriptHolder_SetScriptComponentHolder(int thisPointer, int sc)
		{
			ManagedScriptHolder managedScriptHolder = DotNetObject.GetManagedObjectWithId(thisPointer) as ManagedScriptHolder;
			ScriptComponentBehavior scriptComponentBehavior = DotNetObject.GetManagedObjectWithId(sc) as ScriptComponentBehavior;
			managedScriptHolder.SetScriptComponentHolder(scriptComponentBehavior);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002AFA File Offset: 0x00000CFA
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedScriptHolder_TickComponents_delegate))]
		internal static void ManagedScriptHolder_TickComponents(int thisPointer, float dt)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ManagedScriptHolder).TickComponents(dt);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002B0D File Offset: 0x00000D0D
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ManagedScriptHolder_TickComponentsEditor_delegate))]
		internal static void ManagedScriptHolder_TickComponentsEditor(int thisPointer, float dt)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ManagedScriptHolder).TickComponentsEditor(dt);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B20 File Offset: 0x00000D20
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.MessageManagerBase_PostMessageLine_delegate))]
		internal static void MessageManagerBase_PostMessageLine(int thisPointer, IntPtr text, uint color)
		{
			MessageManagerBase messageManagerBase = DotNetObject.GetManagedObjectWithId(thisPointer) as MessageManagerBase;
			string text2 = Marshal.PtrToStringAnsi(text);
			messageManagerBase.PostMessageLine(text2, color);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B48 File Offset: 0x00000D48
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.MessageManagerBase_PostMessageLineFormatted_delegate))]
		internal static void MessageManagerBase_PostMessageLineFormatted(int thisPointer, IntPtr text, uint color)
		{
			MessageManagerBase messageManagerBase = DotNetObject.GetManagedObjectWithId(thisPointer) as MessageManagerBase;
			string text2 = Marshal.PtrToStringAnsi(text);
			messageManagerBase.PostMessageLineFormatted(text2, color);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B70 File Offset: 0x00000D70
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.MessageManagerBase_PostSuccessLine_delegate))]
		internal static void MessageManagerBase_PostSuccessLine(int thisPointer, IntPtr text)
		{
			MessageManagerBase messageManagerBase = DotNetObject.GetManagedObjectWithId(thisPointer) as MessageManagerBase;
			string text2 = Marshal.PtrToStringAnsi(text);
			messageManagerBase.PostSuccessLine(text2);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B98 File Offset: 0x00000D98
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.MessageManagerBase_PostWarningLine_delegate))]
		internal static void MessageManagerBase_PostWarningLine(int thisPointer, IntPtr text)
		{
			MessageManagerBase messageManagerBase = DotNetObject.GetManagedObjectWithId(thisPointer) as MessageManagerBase;
			string text2 = Marshal.PtrToStringAnsi(text);
			messageManagerBase.PostWarningLine(text2);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002BBD File Offset: 0x00000DBD
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.NativeParallelDriver_ParalelForLoopBodyCaller_delegate))]
		internal static void NativeParallelDriver_ParalelForLoopBodyCaller(long loopBodyKey, int localStartIndex, int localEndIndex)
		{
			NativeParallelDriver.ParalelForLoopBodyCaller(loopBodyKey, localStartIndex, localEndIndex);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002BC7 File Offset: 0x00000DC7
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.NativeParallelDriver_ParalelForLoopBodyWithDtCaller_delegate))]
		internal static void NativeParallelDriver_ParalelForLoopBodyWithDtCaller(long loopBodyKey, int localStartIndex, int localEndIndex)
		{
			NativeParallelDriver.ParalelForLoopBodyWithDtCaller(loopBodyKey, localStartIndex, localEndIndex);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002BD4 File Offset: 0x00000DD4
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.RenderTargetComponent_CreateRenderTargetComponent_delegate))]
		internal static int RenderTargetComponent_CreateRenderTargetComponent(NativeObjectPointer renderTarget)
		{
			Texture texture = null;
			if (renderTarget.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(renderTarget.Pointer);
			}
			return RenderTargetComponent.CreateRenderTargetComponent(texture).GetManagedId();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C0C File Offset: 0x00000E0C
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.RenderTargetComponent_OnPaintNeeded_delegate))]
		internal static void RenderTargetComponent_OnPaintNeeded(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as RenderTargetComponent).OnPaintNeeded();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C20 File Offset: 0x00000E20
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.SceneProblemChecker_OnCheckForSceneProblems_delegate))]
		internal static bool SceneProblemChecker_OnCheckForSceneProblems(NativeObjectPointer scene)
		{
			Scene scene2 = null;
			if (scene.Pointer != UIntPtr.Zero)
			{
				scene2 = new Scene(scene.Pointer);
			}
			return SceneProblemChecker.OnCheckForSceneProblems(scene2);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C53 File Offset: 0x00000E53
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_AddScriptComponentToTick_delegate))]
		internal static void ScriptComponentBehavior_AddScriptComponentToTick(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).AddScriptComponentToTick();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C65 File Offset: 0x00000E65
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_DeregisterAsPrefabScriptComponent_delegate))]
		internal static void ScriptComponentBehavior_DeregisterAsPrefabScriptComponent(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).DeregisterAsPrefabScriptComponent();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C77 File Offset: 0x00000E77
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_DeregisterAsUndoStackScriptComponent_delegate))]
		internal static void ScriptComponentBehavior_DeregisterAsUndoStackScriptComponent(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).DeregisterAsUndoStackScriptComponent();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C89 File Offset: 0x00000E89
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_DisablesOroCreation_delegate))]
		internal static bool ScriptComponentBehavior_DisablesOroCreation(int thisPointer)
		{
			return (DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).DisablesOroCreation();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002C9B File Offset: 0x00000E9B
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_GetEditableFields_delegate))]
		internal static int ScriptComponentBehavior_GetEditableFields(IntPtr className)
		{
			return Managed.AddCustomParameter<string[]>(ScriptComponentBehavior.GetEditableFields(Marshal.PtrToStringAnsi(className))).GetManagedId();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002CB2 File Offset: 0x00000EB2
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_HandleOnRemoved_delegate))]
		internal static void ScriptComponentBehavior_HandleOnRemoved(int thisPointer, int removeReason)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).HandleOnRemoved(removeReason);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002CC5 File Offset: 0x00000EC5
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_IsOnlyVisual_delegate))]
		internal static bool ScriptComponentBehavior_IsOnlyVisual(int thisPointer)
		{
			return (DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).IsOnlyVisual();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002CD7 File Offset: 0x00000ED7
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_MovesEntity_delegate))]
		internal static bool ScriptComponentBehavior_MovesEntity(int thisPointer)
		{
			return (DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).MovesEntity();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002CE9 File Offset: 0x00000EE9
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnBoundingBoxValidate_delegate))]
		internal static void ScriptComponentBehavior_OnBoundingBoxValidate(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnBoundingBoxValidate();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002CFB File Offset: 0x00000EFB
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnCheckForProblems_delegate))]
		internal static bool ScriptComponentBehavior_OnCheckForProblems(int thisPointer)
		{
			return (DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnCheckForProblems();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002D0D File Offset: 0x00000F0D
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnDynamicNavmeshVertexUpdate_delegate))]
		internal static void ScriptComponentBehavior_OnDynamicNavmeshVertexUpdate(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnDynamicNavmeshVertexUpdate();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002D1F File Offset: 0x00000F1F
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditModeVisibilityChanged_delegate))]
		internal static void ScriptComponentBehavior_OnEditModeVisibilityChanged(int thisPointer, bool currentVisibility)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnEditModeVisibilityChanged(currentVisibility);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002D32 File Offset: 0x00000F32
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorInit_delegate))]
		internal static void ScriptComponentBehavior_OnEditorInit(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnEditorInit();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002D44 File Offset: 0x00000F44
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorTick_delegate))]
		internal static void ScriptComponentBehavior_OnEditorTick(int thisPointer, float dt)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnEditorTick(dt);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D57 File Offset: 0x00000F57
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorValidate_delegate))]
		internal static void ScriptComponentBehavior_OnEditorValidate(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnEditorValidate();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002D6C File Offset: 0x00000F6C
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnEditorVariableChanged_delegate))]
		internal static void ScriptComponentBehavior_OnEditorVariableChanged(int thisPointer, IntPtr variableName)
		{
			ScriptComponentBehavior scriptComponentBehavior = DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior;
			string text = Marshal.PtrToStringAnsi(variableName);
			scriptComponentBehavior.OnEditorVariableChanged(text);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002D91 File Offset: 0x00000F91
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnInit_delegate))]
		internal static void ScriptComponentBehavior_OnInit(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnInit();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002DA3 File Offset: 0x00000FA3
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnPhysicsCollisionAux_delegate))]
		internal static void ScriptComponentBehavior_OnPhysicsCollisionAux(int thisPointer, ref PhysicsContact contact, UIntPtr entity0, UIntPtr entity1)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnPhysicsCollisionAux(ref contact, entity0, entity1);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002DB8 File Offset: 0x00000FB8
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnPreInit_delegate))]
		internal static void ScriptComponentBehavior_OnPreInit(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnPreInit();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002DCA File Offset: 0x00000FCA
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnSaveAsPrefab_delegate))]
		internal static void ScriptComponentBehavior_OnSaveAsPrefab(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnSaveAsPrefab();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002DDC File Offset: 0x00000FDC
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnSceneSave_delegate))]
		internal static void ScriptComponentBehavior_OnSceneSave(int thisPointer, IntPtr saveFolder)
		{
			ScriptComponentBehavior scriptComponentBehavior = DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior;
			string text = Marshal.PtrToStringAnsi(saveFolder);
			scriptComponentBehavior.OnSceneSave(text);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002E01 File Offset: 0x00001001
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_OnTerrainReload_delegate))]
		internal static void ScriptComponentBehavior_OnTerrainReload(int thisPointer, int step)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).OnTerrainReload(step);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002E14 File Offset: 0x00001014
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_RegisterAsPrefabScriptComponent_delegate))]
		internal static void ScriptComponentBehavior_RegisterAsPrefabScriptComponent(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).RegisterAsPrefabScriptComponent();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002E26 File Offset: 0x00001026
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_RegisterAsUndoStackScriptComponent_delegate))]
		internal static void ScriptComponentBehavior_RegisterAsUndoStackScriptComponent(int thisPointer)
		{
			(DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior).RegisterAsUndoStackScriptComponent();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002E38 File Offset: 0x00001038
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_SetScene_delegate))]
		internal static void ScriptComponentBehavior_SetScene(int thisPointer, NativeObjectPointer scene)
		{
			ScriptComponentBehavior scriptComponentBehavior = DotNetObject.GetManagedObjectWithId(thisPointer) as ScriptComponentBehavior;
			Scene scene2 = null;
			if (scene.Pointer != UIntPtr.Zero)
			{
				scene2 = new Scene(scene.Pointer);
			}
			scriptComponentBehavior.SetScene(scene2);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002E76 File Offset: 0x00001076
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ScriptComponentBehavior_SkeletonPostIntegrateCallbackAux_delegate))]
		internal static bool ScriptComponentBehavior_SkeletonPostIntegrateCallbackAux(int script, UIntPtr animResultPointer)
		{
			return ScriptComponentBehavior.SkeletonPostIntegrateCallbackAux(DotNetObject.GetManagedObjectWithId(script) as ScriptComponentBehavior, animResultPointer);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002E8C File Offset: 0x0000108C
		[MonoPInvokeCallback(typeof(EngineCallbacksGenerated.ThumbnailCreatorView_OnThumbnailRenderComplete_delegate))]
		internal static void ThumbnailCreatorView_OnThumbnailRenderComplete(IntPtr renderId, NativeObjectPointer renderTarget)
		{
			string text = Marshal.PtrToStringAnsi(renderId);
			Texture texture = null;
			if (renderTarget.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(renderTarget.Pointer);
			}
			ThumbnailCreatorView.OnThumbnailRenderComplete(text, texture);
		}

		// Token: 0x02000033 RID: 51
		// (Invoke) Token: 0x060006E9 RID: 1769
		internal delegate UIntPtr CrashInformationCollector_CollectInformation_delegate();

		// Token: 0x02000034 RID: 52
		// (Invoke) Token: 0x060006ED RID: 1773
		internal delegate UIntPtr EngineController_GetApplicationPlatformName_delegate();

		// Token: 0x02000035 RID: 53
		// (Invoke) Token: 0x060006F1 RID: 1777
		internal delegate UIntPtr EngineController_GetModulesVersionStr_delegate();

		// Token: 0x02000036 RID: 54
		// (Invoke) Token: 0x060006F5 RID: 1781
		internal delegate UIntPtr EngineController_GetVersionStr_delegate();

		// Token: 0x02000037 RID: 55
		// (Invoke) Token: 0x060006F9 RID: 1785
		internal delegate void EngineController_Initialize_delegate();

		// Token: 0x02000038 RID: 56
		// (Invoke) Token: 0x060006FD RID: 1789
		internal delegate void EngineController_OnConfigChange_delegate();

		// Token: 0x02000039 RID: 57
		// (Invoke) Token: 0x06000701 RID: 1793
		internal delegate void EngineController_OnConstrainedStateChange_delegate([MarshalAs(UnmanagedType.U1)] bool isConstrained);

		// Token: 0x0200003A RID: 58
		// (Invoke) Token: 0x06000705 RID: 1797
		internal delegate void EngineController_OnControllerDisconnection_delegate();

		// Token: 0x0200003B RID: 59
		// (Invoke) Token: 0x06000709 RID: 1801
		internal delegate void EngineController_OnDLCInstalled_delegate();

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x0600070D RID: 1805
		internal delegate void EngineController_OnDLCLoaded_delegate();

		// Token: 0x0200003D RID: 61
		// (Invoke) Token: 0x06000711 RID: 1809
		internal delegate void EngineManaged_CheckSharedStructureSizes_delegate();

		// Token: 0x0200003E RID: 62
		// (Invoke) Token: 0x06000715 RID: 1813
		internal delegate void EngineManaged_EngineApiMethodInterfaceInitializer_delegate(int id, IntPtr pointer);

		// Token: 0x0200003F RID: 63
		// (Invoke) Token: 0x06000719 RID: 1817
		internal delegate void EngineManaged_FillEngineApiPointers_delegate();

		// Token: 0x02000040 RID: 64
		// (Invoke) Token: 0x0600071D RID: 1821
		internal delegate void EngineScreenManager_InitializeLastPressedKeys_delegate(NativeObjectPointer lastKeysPressed);

		// Token: 0x02000041 RID: 65
		// (Invoke) Token: 0x06000721 RID: 1825
		internal delegate void EngineScreenManager_LateTick_delegate(float dt);

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x06000725 RID: 1829
		internal delegate void EngineScreenManager_OnGameWindowFocusChange_delegate([MarshalAs(UnmanagedType.U1)] bool focusGained);

		// Token: 0x02000043 RID: 67
		// (Invoke) Token: 0x06000729 RID: 1833
		internal delegate void EngineScreenManager_OnOnscreenKeyboardCanceled_delegate();

		// Token: 0x02000044 RID: 68
		// (Invoke) Token: 0x0600072D RID: 1837
		internal delegate void EngineScreenManager_OnOnscreenKeyboardDone_delegate(IntPtr inputText);

		// Token: 0x02000045 RID: 69
		// (Invoke) Token: 0x06000731 RID: 1841
		internal delegate void EngineScreenManager_PreTick_delegate(float dt);

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x06000735 RID: 1845
		internal delegate void EngineScreenManager_Tick_delegate(float dt);

		// Token: 0x02000047 RID: 71
		// (Invoke) Token: 0x06000739 RID: 1849
		internal delegate void EngineScreenManager_Update_delegate();

		// Token: 0x02000048 RID: 72
		// (Invoke) Token: 0x0600073D RID: 1853
		internal delegate void ManagedExtensions_CollectCommandLineFunctions_delegate();

		// Token: 0x02000049 RID: 73
		// (Invoke) Token: 0x06000741 RID: 1857
		internal delegate void ManagedExtensions_CopyObjectFieldsFrom_delegate(int dst, int src, IntPtr className, int callFieldChangeEventAsInteger);

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x06000745 RID: 1861
		internal delegate int ManagedExtensions_CreateScriptComponentInstance_delegate(IntPtr className, UIntPtr entityPtr, NativeObjectPointer managedScriptComponent);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x06000749 RID: 1865
		internal delegate void ManagedExtensions_ForceGarbageCollect_delegate();

		// Token: 0x0200004C RID: 76
		// (Invoke) Token: 0x0600074D RID: 1869
		[return: MarshalAs(UnmanagedType.U1)]
		internal delegate bool ManagedExtensions_GetEditorVisibilityOfField_delegate(uint classNameHash, uint fieldNamehash);

		// Token: 0x0200004D RID: 77
		// (Invoke) Token: 0x06000751 RID: 1873
		internal delegate void ManagedExtensions_GetObjectField_delegate(int managedObject, uint classNameHash, ref ScriptComponentFieldHolder scriptComponentFieldHolder, uint fieldNameHash, RglScriptFieldType type);

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x06000755 RID: 1877
		internal delegate UIntPtr ManagedExtensions_GetScriptComponentClassNames_delegate();

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x06000759 RID: 1881
		internal delegate RglScriptFieldType ManagedExtensions_GetTypeOfField_delegate(uint classNameHash, uint fieldNameHash);

		// Token: 0x02000050 RID: 80
		// (Invoke) Token: 0x0600075D RID: 1885
		internal delegate void ManagedExtensions_SetObjectFieldBool_delegate(int managedObject, uint classNameHash, uint fieldNameHash, [MarshalAs(UnmanagedType.U1)] bool value, int callFieldChangeEventAsInteger);

		// Token: 0x02000051 RID: 81
		// (Invoke) Token: 0x06000761 RID: 1889
		internal delegate void ManagedExtensions_SetObjectFieldColor_delegate(int managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger);

		// Token: 0x02000052 RID: 82
		// (Invoke) Token: 0x06000765 RID: 1893
		internal delegate void ManagedExtensions_SetObjectFieldDouble_delegate(int managedObject, uint classNameHash, uint fieldNameHash, double value, int callFieldChangeEventAsInteger);

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x06000769 RID: 1897
		internal delegate void ManagedExtensions_SetObjectFieldEntity_delegate(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger);

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x0600076D RID: 1901
		internal delegate void ManagedExtensions_SetObjectFieldEnum_delegate(int managedObject, uint classNameHash, uint fieldNameHash, IntPtr value, int callFieldChangeEventAsInteger);

		// Token: 0x02000055 RID: 85
		// (Invoke) Token: 0x06000771 RID: 1905
		internal delegate void ManagedExtensions_SetObjectFieldFloat_delegate(int managedObject, uint classNameHash, uint fieldNameHash, float value, int callFieldChangeEventAsInteger);

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x06000775 RID: 1909
		internal delegate void ManagedExtensions_SetObjectFieldInt_delegate(int managedObject, uint classNameHash, uint fieldNameHash, int value, int callFieldChangeEventAsInteger);

		// Token: 0x02000057 RID: 87
		// (Invoke) Token: 0x06000779 RID: 1913
		internal delegate void ManagedExtensions_SetObjectFieldMaterial_delegate(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger);

		// Token: 0x02000058 RID: 88
		// (Invoke) Token: 0x0600077D RID: 1917
		internal delegate void ManagedExtensions_SetObjectFieldMatrixFrame_delegate(int managedObject, uint classNameHash, uint fieldNameHash, MatrixFrame value, int callFieldChangeEventAsInteger);

		// Token: 0x02000059 RID: 89
		// (Invoke) Token: 0x06000781 RID: 1921
		internal delegate void ManagedExtensions_SetObjectFieldMesh_delegate(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger);

		// Token: 0x0200005A RID: 90
		// (Invoke) Token: 0x06000785 RID: 1925
		internal delegate void ManagedExtensions_SetObjectFieldString_delegate(int managedObject, uint classNameHash, uint fieldNameHash, IntPtr value, int callFieldChangeEventAsInteger);

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x06000789 RID: 1929
		internal delegate void ManagedExtensions_SetObjectFieldTexture_delegate(int managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger);

		// Token: 0x0200005C RID: 92
		// (Invoke) Token: 0x0600078D RID: 1933
		internal delegate void ManagedExtensions_SetObjectFieldVec3_delegate(int managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger);

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x06000791 RID: 1937
		internal delegate int ManagedScriptHolder_CreateManagedScriptHolder_delegate();

		// Token: 0x0200005E RID: 94
		// (Invoke) Token: 0x06000795 RID: 1941
		internal delegate void ManagedScriptHolder_FixedTickComponents_delegate(int thisPointer, float fixedDt);

		// Token: 0x0200005F RID: 95
		// (Invoke) Token: 0x06000799 RID: 1945
		internal delegate int ManagedScriptHolder_GetNumberOfScripts_delegate(int thisPointer);

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x0600079D RID: 1949
		internal delegate void ManagedScriptHolder_RemoveScriptComponentFromAllTickLists_delegate(int thisPointer, int sc);

		// Token: 0x02000061 RID: 97
		// (Invoke) Token: 0x060007A1 RID: 1953
		internal delegate void ManagedScriptHolder_SetScriptComponentHolder_delegate(int thisPointer, int sc);

		// Token: 0x02000062 RID: 98
		// (Invoke) Token: 0x060007A5 RID: 1957
		internal delegate void ManagedScriptHolder_TickComponents_delegate(int thisPointer, float dt);

		// Token: 0x02000063 RID: 99
		// (Invoke) Token: 0x060007A9 RID: 1961
		internal delegate void ManagedScriptHolder_TickComponentsEditor_delegate(int thisPointer, float dt);

		// Token: 0x02000064 RID: 100
		// (Invoke) Token: 0x060007AD RID: 1965
		internal delegate void MessageManagerBase_PostMessageLine_delegate(int thisPointer, IntPtr text, uint color);

		// Token: 0x02000065 RID: 101
		// (Invoke) Token: 0x060007B1 RID: 1969
		internal delegate void MessageManagerBase_PostMessageLineFormatted_delegate(int thisPointer, IntPtr text, uint color);

		// Token: 0x02000066 RID: 102
		// (Invoke) Token: 0x060007B5 RID: 1973
		internal delegate void MessageManagerBase_PostSuccessLine_delegate(int thisPointer, IntPtr text);

		// Token: 0x02000067 RID: 103
		// (Invoke) Token: 0x060007B9 RID: 1977
		internal delegate void MessageManagerBase_PostWarningLine_delegate(int thisPointer, IntPtr text);

		// Token: 0x02000068 RID: 104
		// (Invoke) Token: 0x060007BD RID: 1981
		internal delegate void NativeParallelDriver_ParalelForLoopBodyCaller_delegate(long loopBodyKey, int localStartIndex, int localEndIndex);

		// Token: 0x02000069 RID: 105
		// (Invoke) Token: 0x060007C1 RID: 1985
		internal delegate void NativeParallelDriver_ParalelForLoopBodyWithDtCaller_delegate(long loopBodyKey, int localStartIndex, int localEndIndex);

		// Token: 0x0200006A RID: 106
		// (Invoke) Token: 0x060007C5 RID: 1989
		internal delegate int RenderTargetComponent_CreateRenderTargetComponent_delegate(NativeObjectPointer renderTarget);

		// Token: 0x0200006B RID: 107
		// (Invoke) Token: 0x060007C9 RID: 1993
		internal delegate void RenderTargetComponent_OnPaintNeeded_delegate(int thisPointer);

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x060007CD RID: 1997
		[return: MarshalAs(UnmanagedType.U1)]
		internal delegate bool SceneProblemChecker_OnCheckForSceneProblems_delegate(NativeObjectPointer scene);

		// Token: 0x0200006D RID: 109
		// (Invoke) Token: 0x060007D1 RID: 2001
		internal delegate void ScriptComponentBehavior_AddScriptComponentToTick_delegate(int thisPointer);

		// Token: 0x0200006E RID: 110
		// (Invoke) Token: 0x060007D5 RID: 2005
		internal delegate void ScriptComponentBehavior_DeregisterAsPrefabScriptComponent_delegate(int thisPointer);

		// Token: 0x0200006F RID: 111
		// (Invoke) Token: 0x060007D9 RID: 2009
		internal delegate void ScriptComponentBehavior_DeregisterAsUndoStackScriptComponent_delegate(int thisPointer);

		// Token: 0x02000070 RID: 112
		// (Invoke) Token: 0x060007DD RID: 2013
		[return: MarshalAs(UnmanagedType.U1)]
		internal delegate bool ScriptComponentBehavior_DisablesOroCreation_delegate(int thisPointer);

		// Token: 0x02000071 RID: 113
		// (Invoke) Token: 0x060007E1 RID: 2017
		internal delegate int ScriptComponentBehavior_GetEditableFields_delegate(IntPtr className);

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x060007E5 RID: 2021
		internal delegate void ScriptComponentBehavior_HandleOnRemoved_delegate(int thisPointer, int removeReason);

		// Token: 0x02000073 RID: 115
		// (Invoke) Token: 0x060007E9 RID: 2025
		[return: MarshalAs(UnmanagedType.U1)]
		internal delegate bool ScriptComponentBehavior_IsOnlyVisual_delegate(int thisPointer);

		// Token: 0x02000074 RID: 116
		// (Invoke) Token: 0x060007ED RID: 2029
		[return: MarshalAs(UnmanagedType.U1)]
		internal delegate bool ScriptComponentBehavior_MovesEntity_delegate(int thisPointer);

		// Token: 0x02000075 RID: 117
		// (Invoke) Token: 0x060007F1 RID: 2033
		internal delegate void ScriptComponentBehavior_OnBoundingBoxValidate_delegate(int thisPointer);

		// Token: 0x02000076 RID: 118
		// (Invoke) Token: 0x060007F5 RID: 2037
		[return: MarshalAs(UnmanagedType.U1)]
		internal delegate bool ScriptComponentBehavior_OnCheckForProblems_delegate(int thisPointer);

		// Token: 0x02000077 RID: 119
		// (Invoke) Token: 0x060007F9 RID: 2041
		internal delegate void ScriptComponentBehavior_OnDynamicNavmeshVertexUpdate_delegate(int thisPointer);

		// Token: 0x02000078 RID: 120
		// (Invoke) Token: 0x060007FD RID: 2045
		internal delegate void ScriptComponentBehavior_OnEditModeVisibilityChanged_delegate(int thisPointer, [MarshalAs(UnmanagedType.U1)] bool currentVisibility);

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x06000801 RID: 2049
		internal delegate void ScriptComponentBehavior_OnEditorInit_delegate(int thisPointer);

		// Token: 0x0200007A RID: 122
		// (Invoke) Token: 0x06000805 RID: 2053
		internal delegate void ScriptComponentBehavior_OnEditorTick_delegate(int thisPointer, float dt);

		// Token: 0x0200007B RID: 123
		// (Invoke) Token: 0x06000809 RID: 2057
		internal delegate void ScriptComponentBehavior_OnEditorValidate_delegate(int thisPointer);

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x0600080D RID: 2061
		internal delegate void ScriptComponentBehavior_OnEditorVariableChanged_delegate(int thisPointer, IntPtr variableName);

		// Token: 0x0200007D RID: 125
		// (Invoke) Token: 0x06000811 RID: 2065
		internal delegate void ScriptComponentBehavior_OnInit_delegate(int thisPointer);

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x06000815 RID: 2069
		internal delegate void ScriptComponentBehavior_OnPhysicsCollisionAux_delegate(int thisPointer, ref PhysicsContact contact, UIntPtr entity0, UIntPtr entity1);

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x06000819 RID: 2073
		internal delegate void ScriptComponentBehavior_OnPreInit_delegate(int thisPointer);

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x0600081D RID: 2077
		internal delegate void ScriptComponentBehavior_OnSaveAsPrefab_delegate(int thisPointer);

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x06000821 RID: 2081
		internal delegate void ScriptComponentBehavior_OnSceneSave_delegate(int thisPointer, IntPtr saveFolder);

		// Token: 0x02000082 RID: 130
		// (Invoke) Token: 0x06000825 RID: 2085
		internal delegate void ScriptComponentBehavior_OnTerrainReload_delegate(int thisPointer, int step);

		// Token: 0x02000083 RID: 131
		// (Invoke) Token: 0x06000829 RID: 2089
		internal delegate void ScriptComponentBehavior_RegisterAsPrefabScriptComponent_delegate(int thisPointer);

		// Token: 0x02000084 RID: 132
		// (Invoke) Token: 0x0600082D RID: 2093
		internal delegate void ScriptComponentBehavior_RegisterAsUndoStackScriptComponent_delegate(int thisPointer);

		// Token: 0x02000085 RID: 133
		// (Invoke) Token: 0x06000831 RID: 2097
		internal delegate void ScriptComponentBehavior_SetScene_delegate(int thisPointer, NativeObjectPointer scene);

		// Token: 0x02000086 RID: 134
		// (Invoke) Token: 0x06000835 RID: 2101
		[return: MarshalAs(UnmanagedType.U1)]
		internal delegate bool ScriptComponentBehavior_SkeletonPostIntegrateCallbackAux_delegate(int script, UIntPtr animResultPointer);

		// Token: 0x02000087 RID: 135
		// (Invoke) Token: 0x06000839 RID: 2105
		internal delegate void ThumbnailCreatorView_OnThumbnailRenderComplete_delegate(IntPtr renderId, NativeObjectPointer renderTarget);
	}
}
