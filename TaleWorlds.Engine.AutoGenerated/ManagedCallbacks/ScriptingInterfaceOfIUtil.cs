using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000030 RID: 48
	internal class ScriptingInterfaceOfIUtil : IUtil
	{
		// Token: 0x06000639 RID: 1593 RVA: 0x0001A094 File Offset: 0x00018294
		public void AddCommandLineFunction(string concatName)
		{
			byte[] array = null;
			if (concatName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(concatName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(concatName, 0, concatName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_AddCommandLineFunctionDelegate(array);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001A0F0 File Offset: 0x000182F0
		public void AddMainThreadPerformanceQuery(string parent, string name, float seconds)
		{
			byte[] array = null;
			if (parent != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(parent);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(parent, 0, parent.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (name != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(name);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(name, 0, name.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_AddMainThreadPerformanceQueryDelegate(array, array2, seconds);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001A190 File Offset: 0x00018390
		public void AddPerformanceReportToken(string performance_type, string name, float loading_time)
		{
			byte[] array = null;
			if (performance_type != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(performance_type);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(performance_type, 0, performance_type.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (name != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(name);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(name, 0, name.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_AddPerformanceReportTokenDelegate(array, array2, loading_time);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001A230 File Offset: 0x00018430
		public void AddSceneObjectReport(string scene_name, string report_name, float report_value)
		{
			byte[] array = null;
			if (scene_name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(scene_name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(scene_name, 0, scene_name.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (report_name != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(report_name);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(report_name, 0, report_name.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_AddSceneObjectReportDelegate(array, array2, report_value);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001A2CE File Offset: 0x000184CE
		public void CheckIfAssetsAndSourcesAreSame()
		{
			ScriptingInterfaceOfIUtil.call_CheckIfAssetsAndSourcesAreSameDelegate();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001A2DA File Offset: 0x000184DA
		public bool CheckIfTerrainShaderHeaderGenerationFinished()
		{
			return ScriptingInterfaceOfIUtil.call_CheckIfTerrainShaderHeaderGenerationFinishedDelegate();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001A2E6 File Offset: 0x000184E6
		public void CheckResourceModifications()
		{
			ScriptingInterfaceOfIUtil.call_CheckResourceModificationsDelegate();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001A2F4 File Offset: 0x000184F4
		public void CheckSceneForProblems(string path)
		{
			byte[] array = null;
			if (path != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(path);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(path, 0, path.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_CheckSceneForProblemsDelegate(array);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001A34E File Offset: 0x0001854E
		public bool CheckShaderCompilation()
		{
			return ScriptingInterfaceOfIUtil.call_CheckShaderCompilationDelegate();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001A35A File Offset: 0x0001855A
		public void clear_decal_atlas(DecalAtlasGroup atlasGroup)
		{
			ScriptingInterfaceOfIUtil.call_clear_decal_atlasDelegate(atlasGroup);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001A367 File Offset: 0x00018567
		public void ClearOldResourcesAndObjects()
		{
			ScriptingInterfaceOfIUtil.call_ClearOldResourcesAndObjectsDelegate();
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001A373 File Offset: 0x00018573
		public void ClearShaderMemory()
		{
			ScriptingInterfaceOfIUtil.call_ClearShaderMemoryDelegate();
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001A380 File Offset: 0x00018580
		public bool CommandLineArgumentExists(string str)
		{
			byte[] array = null;
			if (str != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(str);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(str, 0, str.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIUtil.call_CommandLineArgumentExistsDelegate(array);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001A3DC File Offset: 0x000185DC
		public void CompileAllShaders(string targetPlatform)
		{
			byte[] array = null;
			if (targetPlatform != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(targetPlatform);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(targetPlatform, 0, targetPlatform.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_CompileAllShadersDelegate(array);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001A438 File Offset: 0x00018638
		public void CompileTerrainShadersDist(string targetPlatform, string targetConfig, string output_path)
		{
			byte[] array = null;
			if (targetPlatform != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(targetPlatform);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(targetPlatform, 0, targetPlatform.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (targetConfig != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(targetConfig);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(targetConfig, 0, targetConfig.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (output_path != null)
			{
				int byteCount3 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(output_path);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(output_path, 0, output_path.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			ScriptingInterfaceOfIUtil.call_CompileTerrainShadersDistDelegate(array, array2, array3);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001A520 File Offset: 0x00018720
		public void CreateSelectionInEditor(UIntPtr[] gameEntities, int entityCount, string name)
		{
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(gameEntities, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_CreateSelectionInEditorDelegate(pointer, entityCount, array);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001A594 File Offset: 0x00018794
		public void DebugSetGlobalLoadingWindowState(bool s)
		{
			ScriptingInterfaceOfIUtil.call_DebugSetGlobalLoadingWindowStateDelegate(s);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001A5A4 File Offset: 0x000187A4
		public void DeleteEntitiesInEditorScene(UIntPtr[] gameEntities, int entityCount)
		{
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(gameEntities, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIUtil.call_DeleteEntitiesInEditorSceneDelegate(pointer, entityCount);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001A5D5 File Offset: 0x000187D5
		public void DetachWatchdog()
		{
			ScriptingInterfaceOfIUtil.call_DetachWatchdogDelegate();
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001A5E1 File Offset: 0x000187E1
		public bool DidAutomatedGIBakeFinished()
		{
			return ScriptingInterfaceOfIUtil.call_DidAutomatedGIBakeFinishedDelegate();
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001A5ED File Offset: 0x000187ED
		public void DisableCoreGame()
		{
			ScriptingInterfaceOfIUtil.call_DisableCoreGameDelegate();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001A5F9 File Offset: 0x000187F9
		public void DisableGlobalEditDataCacher()
		{
			ScriptingInterfaceOfIUtil.call_DisableGlobalEditDataCacherDelegate();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001A605 File Offset: 0x00018805
		public void DisableGlobalLoadingWindow()
		{
			ScriptingInterfaceOfIUtil.call_DisableGlobalLoadingWindowDelegate();
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001A611 File Offset: 0x00018811
		public void DoDelayedexit(int returnCode)
		{
			ScriptingInterfaceOfIUtil.call_DoDelayedexitDelegate(returnCode);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001A620 File Offset: 0x00018820
		public void DoFullBakeAllLevelsAutomated(string module, string sceneName)
		{
			byte[] array = null;
			if (module != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(module);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(module, 0, module.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (sceneName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(sceneName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(sceneName, 0, sceneName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_DoFullBakeAllLevelsAutomatedDelegate(array, array2);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001A6C0 File Offset: 0x000188C0
		public void DoFullBakeSingleLevelAutomated(string module, string sceneName)
		{
			byte[] array = null;
			if (module != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(module);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(module, 0, module.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (sceneName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(sceneName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(sceneName, 0, sceneName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_DoFullBakeSingleLevelAutomatedDelegate(array, array2);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001A760 File Offset: 0x00018960
		public void DoLightOnlyBakeAllLevelsAutomated(string module, string sceneName)
		{
			byte[] array = null;
			if (module != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(module);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(module, 0, module.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (sceneName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(sceneName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(sceneName, 0, sceneName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_DoLightOnlyBakeAllLevelsAutomatedDelegate(array, array2);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001A800 File Offset: 0x00018A00
		public void DoLightOnlyBakeSingleLevelAutomated(string module, string sceneName)
		{
			byte[] array = null;
			if (module != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(module);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(module, 0, module.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (sceneName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(sceneName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(sceneName, 0, sceneName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_DoLightOnlyBakeSingleLevelAutomatedDelegate(array, array2);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001A8A0 File Offset: 0x00018AA0
		public void DumpGPUMemoryStatistics(string filePath)
		{
			byte[] array = null;
			if (filePath != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(filePath);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(filePath, 0, filePath.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_DumpGPUMemoryStatisticsDelegate(array);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001A8FA File Offset: 0x00018AFA
		public void EnableGlobalEditDataCacher()
		{
			ScriptingInterfaceOfIUtil.call_EnableGlobalEditDataCacherDelegate();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001A906 File Offset: 0x00018B06
		public void EnableGlobalLoadingWindow()
		{
			ScriptingInterfaceOfIUtil.call_EnableGlobalLoadingWindowDelegate();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0001A912 File Offset: 0x00018B12
		public void EnableSingleGPUQueryPerFrame()
		{
			ScriptingInterfaceOfIUtil.call_EnableSingleGPUQueryPerFrameDelegate();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001A91E File Offset: 0x00018B1E
		public void EndLoadingStuckCheckState()
		{
			ScriptingInterfaceOfIUtil.call_EndLoadingStuckCheckStateDelegate();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001A92C File Offset: 0x00018B2C
		public string ExecuteCommandLineCommand(string command)
		{
			byte[] array = null;
			if (command != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(command);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(command, 0, command.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_ExecuteCommandLineCommandDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001A990 File Offset: 0x00018B90
		public void ExitProcess(int exitCode)
		{
			ScriptingInterfaceOfIUtil.call_ExitProcessDelegate(exitCode);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001A9A0 File Offset: 0x00018BA0
		public string ExportNavMeshFaceMarks(string file_name)
		{
			byte[] array = null;
			if (file_name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(file_name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(file_name, 0, file_name.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_ExportNavMeshFaceMarksDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001AA04 File Offset: 0x00018C04
		public void FindMeshesWithoutLods(string module_name)
		{
			byte[] array = null;
			if (module_name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(module_name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(module_name, 0, module_name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_FindMeshesWithoutLodsDelegate(array);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001AA5E File Offset: 0x00018C5E
		public void FlushManagedObjectsMemory()
		{
			ScriptingInterfaceOfIUtil.call_FlushManagedObjectsMemoryDelegate();
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001AA6C File Offset: 0x00018C6C
		public void GatherCoreGameReferences(string scene_names)
		{
			byte[] array = null;
			if (scene_names != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(scene_names);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(scene_names, 0, scene_names.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_GatherCoreGameReferencesDelegate(array);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001AAC8 File Offset: 0x00018CC8
		public void GenerateTerrainShaderHeaders(string targetPlatform, string targetConfig, string output_path)
		{
			byte[] array = null;
			if (targetPlatform != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(targetPlatform);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(targetPlatform, 0, targetPlatform.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (targetConfig != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(targetConfig);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(targetConfig, 0, targetConfig.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (output_path != null)
			{
				int byteCount3 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(output_path);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(output_path, 0, output_path.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			ScriptingInterfaceOfIUtil.call_GenerateTerrainShaderHeadersDelegate(array, array2, array3);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001ABB0 File Offset: 0x00018DB0
		public float GetApplicationMemory()
		{
			return ScriptingInterfaceOfIUtil.call_GetApplicationMemoryDelegate();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001ABBC File Offset: 0x00018DBC
		public string GetApplicationMemoryStatistics()
		{
			if (ScriptingInterfaceOfIUtil.call_GetApplicationMemoryStatisticsDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001ABD2 File Offset: 0x00018DD2
		public string GetApplicationName()
		{
			if (ScriptingInterfaceOfIUtil.call_GetApplicationNameDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		public string GetAttachmentsPath()
		{
			if (ScriptingInterfaceOfIUtil.call_GetAttachmentsPathDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001ABFE File Offset: 0x00018DFE
		public string GetBaseDirectory()
		{
			if (ScriptingInterfaceOfIUtil.call_GetBaseDirectoryDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001AC14 File Offset: 0x00018E14
		public int GetBenchmarkStatus()
		{
			return ScriptingInterfaceOfIUtil.call_GetBenchmarkStatusDelegate();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001AC20 File Offset: 0x00018E20
		public int GetBuildNumber()
		{
			return ScriptingInterfaceOfIUtil.call_GetBuildNumberDelegate();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001AC2C File Offset: 0x00018E2C
		public string GetConsoleHostMachine()
		{
			if (ScriptingInterfaceOfIUtil.call_GetConsoleHostMachineDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001AC42 File Offset: 0x00018E42
		public int GetCoreGameState()
		{
			return ScriptingInterfaceOfIUtil.call_GetCoreGameStateDelegate();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001AC4E File Offset: 0x00018E4E
		public ulong GetCurrentCpuMemoryUsage()
		{
			return ScriptingInterfaceOfIUtil.call_GetCurrentCpuMemoryUsageDelegate();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001AC5A File Offset: 0x00018E5A
		public int GetCurrentEstimatedGPUMemoryCostMB()
		{
			return ScriptingInterfaceOfIUtil.call_GetCurrentEstimatedGPUMemoryCostMBDelegate();
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001AC66 File Offset: 0x00018E66
		public uint GetCurrentProcessID()
		{
			return ScriptingInterfaceOfIUtil.call_GetCurrentProcessIDDelegate();
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001AC72 File Offset: 0x00018E72
		public ulong GetCurrentThreadId()
		{
			return ScriptingInterfaceOfIUtil.call_GetCurrentThreadIdDelegate();
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001AC7E File Offset: 0x00018E7E
		public float GetDeltaTime(int timerId)
		{
			return ScriptingInterfaceOfIUtil.call_GetDeltaTimeDelegate(timerId);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001AC8B File Offset: 0x00018E8B
		public void GetDetailedGPUBufferMemoryStats(ref int totalMemoryAllocated, ref int totalMemoryUsed, ref int emptyChunkCount)
		{
			ScriptingInterfaceOfIUtil.call_GetDetailedGPUBufferMemoryStatsDelegate(ref totalMemoryAllocated, ref totalMemoryUsed, ref emptyChunkCount);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001AC9A File Offset: 0x00018E9A
		public string GetDetailedXBOXMemoryInfo()
		{
			if (ScriptingInterfaceOfIUtil.call_GetDetailedXBOXMemoryInfoDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001ACB0 File Offset: 0x00018EB0
		public void GetEditorSelectedEntities(UIntPtr[] gameEntitiesTemp)
		{
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(gameEntitiesTemp, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIUtil.call_GetEditorSelectedEntitiesDelegate(pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001ACE0 File Offset: 0x00018EE0
		public int GetEditorSelectedEntityCount()
		{
			return ScriptingInterfaceOfIUtil.call_GetEditorSelectedEntityCountDelegate();
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001ACEC File Offset: 0x00018EEC
		public int GetEngineFrameNo()
		{
			return ScriptingInterfaceOfIUtil.call_GetEngineFrameNoDelegate();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001ACF8 File Offset: 0x00018EF8
		public void GetEntitiesOfSelectionSet(string name, UIntPtr[] gameEntitiesTemp)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(gameEntitiesTemp, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIUtil.call_GetEntitiesOfSelectionSetDelegate(array, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001AD6C File Offset: 0x00018F6C
		public int GetEntityCountOfSelectionSet(string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIUtil.call_GetEntityCountOfSelectionSetDelegate(array);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001ADC6 File Offset: 0x00018FC6
		public string GetExecutableWorkingDirectory()
		{
			if (ScriptingInterfaceOfIUtil.call_GetExecutableWorkingDirectoryDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001ADDC File Offset: 0x00018FDC
		public float GetFps()
		{
			return ScriptingInterfaceOfIUtil.call_GetFpsDelegate();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001ADE8 File Offset: 0x00018FE8
		public bool GetFrameLimiterWithSleep()
		{
			return ScriptingInterfaceOfIUtil.call_GetFrameLimiterWithSleepDelegate();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001ADF4 File Offset: 0x00018FF4
		public string GetFullCommandLineString()
		{
			if (ScriptingInterfaceOfIUtil.call_GetFullCommandLineStringDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001AE0C File Offset: 0x0001900C
		public string GetFullFilePathOfScene(string sceneName)
		{
			byte[] array = null;
			if (sceneName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(sceneName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(sceneName, 0, sceneName.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_GetFullFilePathOfSceneDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001AE70 File Offset: 0x00019070
		public string GetFullModulePath(string moduleName)
		{
			byte[] array = null;
			if (moduleName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(moduleName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(moduleName, 0, moduleName.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_GetFullModulePathDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001AED4 File Offset: 0x000190D4
		public string GetFullModulePaths()
		{
			if (ScriptingInterfaceOfIUtil.call_GetFullModulePathsDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001AEEA File Offset: 0x000190EA
		public int GetGPUMemoryMB()
		{
			return ScriptingInterfaceOfIUtil.call_GetGPUMemoryMBDelegate();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001AEF8 File Offset: 0x000190F8
		public ulong GetGpuMemoryOfAllocationGroup(string allocationName)
		{
			byte[] array = null;
			if (allocationName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(allocationName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(allocationName, 0, allocationName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIUtil.call_GetGpuMemoryOfAllocationGroupDelegate(array);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001AF52 File Offset: 0x00019152
		public void GetGPUMemoryStats(ref float totalMemory, ref float renderTargetMemory, ref float depthTargetMemory, ref float srvMemory, ref float bufferMemory)
		{
			ScriptingInterfaceOfIUtil.call_GetGPUMemoryStatsDelegate(ref totalMemory, ref renderTargetMemory, ref depthTargetMemory, ref srvMemory, ref bufferMemory);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001AF65 File Offset: 0x00019165
		public string GetLocalOutputPath()
		{
			if (ScriptingInterfaceOfIUtil.call_GetLocalOutputPathDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001AF7B File Offset: 0x0001917B
		public float GetMainFps()
		{
			return ScriptingInterfaceOfIUtil.call_GetMainFpsDelegate();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001AF87 File Offset: 0x00019187
		public ulong GetMainThreadId()
		{
			return ScriptingInterfaceOfIUtil.call_GetMainThreadIdDelegate();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001AF93 File Offset: 0x00019193
		public int GetMemoryUsageOfCategory(int index)
		{
			return ScriptingInterfaceOfIUtil.call_GetMemoryUsageOfCategoryDelegate(index);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001AFA0 File Offset: 0x000191A0
		public string GetModulesCode()
		{
			if (ScriptingInterfaceOfIUtil.call_GetModulesCodeDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001AFB6 File Offset: 0x000191B6
		public string GetNativeMemoryStatistics()
		{
			if (ScriptingInterfaceOfIUtil.call_GetNativeMemoryStatisticsDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001AFCC File Offset: 0x000191CC
		public int GetNumberOfShaderCompilationsInProgress()
		{
			return ScriptingInterfaceOfIUtil.call_GetNumberOfShaderCompilationsInProgressDelegate();
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001AFD8 File Offset: 0x000191D8
		public string GetPCInfo()
		{
			if (ScriptingInterfaceOfIUtil.call_GetPCInfoDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001AFEE File Offset: 0x000191EE
		public string GetPlatformModulePaths()
		{
			if (ScriptingInterfaceOfIUtil.call_GetPlatformModulePathsDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001B004 File Offset: 0x00019204
		public string GetPossibleCommandLineStartingWith(string command, int index)
		{
			byte[] array = null;
			if (command != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(command);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(command, 0, command.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_GetPossibleCommandLineStartingWithDelegate(array, index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001B069 File Offset: 0x00019269
		public float GetRendererFps()
		{
			return ScriptingInterfaceOfIUtil.call_GetRendererFpsDelegate();
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001B075 File Offset: 0x00019275
		public int GetReturnCode()
		{
			return ScriptingInterfaceOfIUtil.call_GetReturnCodeDelegate();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001B084 File Offset: 0x00019284
		public string GetSingleModuleScenesOfModule(string moduleName)
		{
			byte[] array = null;
			if (moduleName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(moduleName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(moduleName, 0, moduleName.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_GetSingleModuleScenesOfModuleDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001B0E8 File Offset: 0x000192E8
		public int GetSteamAppId()
		{
			return ScriptingInterfaceOfIUtil.call_GetSteamAppIdDelegate();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001B0F4 File Offset: 0x000192F4
		public string GetSystemLanguage()
		{
			if (ScriptingInterfaceOfIUtil.call_GetSystemLanguageDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001B10A File Offset: 0x0001930A
		public int GetVertexBufferChunkSystemMemoryUsage()
		{
			return ScriptingInterfaceOfIUtil.call_GetVertexBufferChunkSystemMemoryUsageDelegate();
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001B116 File Offset: 0x00019316
		public string GetVisualTestsTestFilesPath()
		{
			if (ScriptingInterfaceOfIUtil.call_GetVisualTestsTestFilesPathDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001B12C File Offset: 0x0001932C
		public string GetVisualTestsValidatePath()
		{
			if (ScriptingInterfaceOfIUtil.call_GetVisualTestsValidatePathDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001B142 File Offset: 0x00019342
		public bool IsAsyncPhysicsThread()
		{
			return ScriptingInterfaceOfIUtil.call_IsAsyncPhysicsThreadDelegate();
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001B14E File Offset: 0x0001934E
		public bool IsBenchmarkQuited()
		{
			return ScriptingInterfaceOfIUtil.call_IsBenchmarkQuitedDelegate();
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001B15A File Offset: 0x0001935A
		public int IsDetailedSoundLogOn()
		{
			return ScriptingInterfaceOfIUtil.call_IsDetailedSoundLogOnDelegate();
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001B166 File Offset: 0x00019366
		public bool IsDevkit()
		{
			return ScriptingInterfaceOfIUtil.call_IsDevkitDelegate();
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001B172 File Offset: 0x00019372
		public bool IsEditModeEnabled()
		{
			return ScriptingInterfaceOfIUtil.call_IsEditModeEnabledDelegate();
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001B17E File Offset: 0x0001937E
		public bool IsLockhartPlatform()
		{
			return ScriptingInterfaceOfIUtil.call_IsLockhartPlatformDelegate();
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001B18A File Offset: 0x0001938A
		public bool IsSceneReportFinished()
		{
			return ScriptingInterfaceOfIUtil.call_IsSceneReportFinishedDelegate();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001B196 File Offset: 0x00019396
		public void LoadSkyBoxes()
		{
			ScriptingInterfaceOfIUtil.call_LoadSkyBoxesDelegate();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001B1A4 File Offset: 0x000193A4
		public void LoadVirtualTextureTileset(string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_LoadVirtualTextureTilesetDelegate(array);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001B1FE File Offset: 0x000193FE
		public void ManagedParallelFor(int fromInclusive, int toExclusive, long curKey, int grainSize)
		{
			ScriptingInterfaceOfIUtil.call_ManagedParallelForDelegate(fromInclusive, toExclusive, curKey, grainSize);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001B20F File Offset: 0x0001940F
		public void ManagedParallelForWithDt(int fromInclusive, int toExclusive, long curKey, int grainSize)
		{
			ScriptingInterfaceOfIUtil.call_ManagedParallelForWithDtDelegate(fromInclusive, toExclusive, curKey, grainSize);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001B220 File Offset: 0x00019420
		public void ManagedParallelForWithoutRenderThread(int fromInclusive, int toExclusive, long curKey, int grainSize)
		{
			ScriptingInterfaceOfIUtil.call_ManagedParallelForWithoutRenderThreadDelegate(fromInclusive, toExclusive, curKey, grainSize);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001B231 File Offset: 0x00019431
		public void ManagedParallelForWithoutRenderThreadDt(int fromInclusive, int toExclusive, long curKey, int grainSize)
		{
			ScriptingInterfaceOfIUtil.call_ManagedParallelForWithoutRenderThreadDtDelegate(fromInclusive, toExclusive, curKey, grainSize);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001B242 File Offset: 0x00019442
		public void OnLoadingWindowDisabled()
		{
			ScriptingInterfaceOfIUtil.call_OnLoadingWindowDisabledDelegate();
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001B24E File Offset: 0x0001944E
		public void OnLoadingWindowEnabled()
		{
			ScriptingInterfaceOfIUtil.call_OnLoadingWindowEnabledDelegate();
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001B25C File Offset: 0x0001945C
		public void OpenConsoleStorePage(string productId)
		{
			byte[] array = null;
			if (productId != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(productId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(productId, 0, productId.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_OpenConsoleStorePageDelegate(array);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001B2B8 File Offset: 0x000194B8
		public void OpenOnscreenKeyboard(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum)
		{
			byte[] array = null;
			if (initialText != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(initialText);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(initialText, 0, initialText.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (descriptionText != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(descriptionText);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(descriptionText, 0, descriptionText.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_OpenOnscreenKeyboardDelegate(array, array2, maxLength, keyboardTypeEnum);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001B358 File Offset: 0x00019558
		public void OutputBenchmarkValuesToPerformanceReporter()
		{
			ScriptingInterfaceOfIUtil.call_OutputBenchmarkValuesToPerformanceReporterDelegate();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001B364 File Offset: 0x00019564
		public void OutputPerformanceReports()
		{
			ScriptingInterfaceOfIUtil.call_OutputPerformanceReportsDelegate();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001B370 File Offset: 0x00019570
		public void PairSceneNameToModuleName(string sceneName, string moduleName)
		{
			byte[] array = null;
			if (sceneName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(sceneName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(sceneName, 0, sceneName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (moduleName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(moduleName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(moduleName, 0, moduleName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIUtil.call_PairSceneNameToModuleNameDelegate(array, array2);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001B410 File Offset: 0x00019610
		public string ProcessWindowTitle(string title)
		{
			byte[] array = null;
			if (title != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(title);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(title, 0, title.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_ProcessWindowTitleDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001B474 File Offset: 0x00019674
		public void QuitGame()
		{
			ScriptingInterfaceOfIUtil.call_QuitGameDelegate();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001B480 File Offset: 0x00019680
		public int RegisterGPUAllocationGroup(string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIUtil.call_RegisterGPUAllocationGroupDelegate(array);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001B4DC File Offset: 0x000196DC
		public void RegisterMeshForGPUMorph(string metaMeshName)
		{
			byte[] array = null;
			if (metaMeshName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(metaMeshName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(metaMeshName, 0, metaMeshName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_RegisterMeshForGPUMorphDelegate(array);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001B538 File Offset: 0x00019738
		public int SaveDataAsTexture(string path, int width, int height, float[] data)
		{
			byte[] array = null;
			if (path != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(path);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(path, 0, path.Length, array, 0);
				array[byteCount] = 0;
			}
			PinnedArrayData<float> pinnedArrayData = new PinnedArrayData<float>(data, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			int num = ScriptingInterfaceOfIUtil.call_SaveDataAsTextureDelegate(array, width, height, pointer);
			pinnedArrayData.Dispose();
			return num;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001B5B0 File Offset: 0x000197B0
		public void SelectEntities(UIntPtr[] gameEntities, int entityCount)
		{
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(gameEntities, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIUtil.call_SelectEntitiesDelegate(pointer, entityCount);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001B5E1 File Offset: 0x000197E1
		public void SetAllocationAlwaysValidScene(UIntPtr scene)
		{
			ScriptingInterfaceOfIUtil.call_SetAllocationAlwaysValidSceneDelegate(scene);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001B5EE File Offset: 0x000197EE
		public void SetAssertionAtShaderCompile(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetAssertionAtShaderCompileDelegate(value);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001B5FB File Offset: 0x000197FB
		public void SetAssertionsAndWarningsSetExitCode(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetAssertionsAndWarningsSetExitCodeDelegate(value);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001B608 File Offset: 0x00019808
		public void SetBenchmarkStatus(int status, string def)
		{
			byte[] array = null;
			if (def != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(def);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(def, 0, def.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_SetBenchmarkStatusDelegate(status, array);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001B663 File Offset: 0x00019863
		public void SetCanLoadModules(bool canLoadModules)
		{
			ScriptingInterfaceOfIUtil.call_SetCanLoadModulesDelegate(canLoadModules);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001B670 File Offset: 0x00019870
		public void SetCoreGameState(int state)
		{
			ScriptingInterfaceOfIUtil.call_SetCoreGameStateDelegate(state);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001B67D File Offset: 0x0001987D
		public void SetCrashOnAsserts(bool val)
		{
			ScriptingInterfaceOfIUtil.call_SetCrashOnAssertsDelegate(val);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001B68A File Offset: 0x0001988A
		public void SetCrashOnWarnings(bool val)
		{
			ScriptingInterfaceOfIUtil.call_SetCrashOnWarningsDelegate(val);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001B698 File Offset: 0x00019898
		public void SetCrashReportCustomStack(string customStack)
		{
			byte[] array = null;
			if (customStack != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(customStack);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(customStack, 0, customStack.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_SetCrashReportCustomStackDelegate(array);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001B6F4 File Offset: 0x000198F4
		public void SetCrashReportCustomString(string customString)
		{
			byte[] array = null;
			if (customString != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(customString);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(customString, 0, customString.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_SetCrashReportCustomStringDelegate(array);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001B74E File Offset: 0x0001994E
		public void SetCreateDumpOnWarnings(bool val)
		{
			ScriptingInterfaceOfIUtil.call_SetCreateDumpOnWarningsDelegate(val);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001B75B File Offset: 0x0001995B
		public void SetDisableDumpGeneration(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetDisableDumpGenerationDelegate(value);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001B768 File Offset: 0x00019968
		public void SetDumpFolderPath(string path)
		{
			byte[] array = null;
			if (path != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(path);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(path, 0, path.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_SetDumpFolderPathDelegate(array);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001B7C2 File Offset: 0x000199C2
		public void SetFixedDt(bool enabled, float dt)
		{
			ScriptingInterfaceOfIUtil.call_SetFixedDtDelegate(enabled, dt);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001B7D0 File Offset: 0x000199D0
		public void SetForceDrawEntityID(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetForceDrawEntityIDDelegate(value);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001B7DD File Offset: 0x000199DD
		public void SetForceVsync(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetForceVsyncDelegate(value);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001B7EA File Offset: 0x000199EA
		public void SetFrameLimiterWithSleep(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetFrameLimiterWithSleepDelegate(value);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001B7F7 File Offset: 0x000199F7
		public void SetGraphicsPreset(int preset)
		{
			ScriptingInterfaceOfIUtil.call_SetGraphicsPresetDelegate(preset);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001B804 File Offset: 0x00019A04
		public void SetLoadingScreenPercentage(float value)
		{
			ScriptingInterfaceOfIUtil.call_SetLoadingScreenPercentageDelegate(value);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001B811 File Offset: 0x00019A11
		public void SetMessageLineRenderingState(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetMessageLineRenderingStateDelegate(value);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001B81E File Offset: 0x00019A1E
		public void SetPrintCallstackAtCrahses(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetPrintCallstackAtCrahsesDelegate(value);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001B82B File Offset: 0x00019A2B
		public void SetRenderAgents(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetRenderAgentsDelegate(value);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001B838 File Offset: 0x00019A38
		public void SetRenderMode(int mode)
		{
			ScriptingInterfaceOfIUtil.call_SetRenderModeDelegate(mode);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001B845 File Offset: 0x00019A45
		public void SetReportMode(bool reportMode)
		{
			ScriptingInterfaceOfIUtil.call_SetReportModeDelegate(reportMode);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001B852 File Offset: 0x00019A52
		public void SetScreenTextRenderingState(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetScreenTextRenderingStateDelegate(value);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001B85F File Offset: 0x00019A5F
		public void SetWatchdogAutoreport(bool value)
		{
			ScriptingInterfaceOfIUtil.call_SetWatchdogAutoreportDelegate(value);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001B86C File Offset: 0x00019A6C
		public void SetWatchdogValue(string fileName, string groupName, string key, string value)
		{
			byte[] array = null;
			if (fileName != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(fileName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(fileName, 0, fileName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (groupName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(groupName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(groupName, 0, groupName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			byte[] array3 = null;
			if (key != null)
			{
				int byteCount3 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(key);
				array3 = ((byteCount3 < 1024) ? CallbackStringBufferManager.StringBuffer2 : new byte[byteCount3 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(key, 0, key.Length, array3, 0);
				array3[byteCount3] = 0;
			}
			byte[] array4 = null;
			if (value != null)
			{
				int byteCount4 = ScriptingInterfaceOfIUtil._utf8.GetByteCount(value);
				array4 = ((byteCount4 < 1024) ? CallbackStringBufferManager.StringBuffer3 : new byte[byteCount4 + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(value, 0, value.Length, array4, 0);
				array4[byteCount4] = 0;
			}
			ScriptingInterfaceOfIUtil.call_SetWatchdogValueDelegate(array, array2, array3, array4);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001B9A4 File Offset: 0x00019BA4
		public void SetWindowTitle(string title)
		{
			byte[] array = null;
			if (title != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(title);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(title, 0, title.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_SetWindowTitleDelegate(array);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001B9FE File Offset: 0x00019BFE
		public void StartLoadingStuckCheckState(float seconds)
		{
			ScriptingInterfaceOfIUtil.call_StartLoadingStuckCheckStateDelegate(seconds);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001BA0C File Offset: 0x00019C0C
		public void StartScenePerformanceReport(string folderPath)
		{
			byte[] array = null;
			if (folderPath != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(folderPath);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(folderPath, 0, folderPath.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_StartScenePerformanceReportDelegate(array);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001BA66 File Offset: 0x00019C66
		public void TakeScreenshotFromPlatformPath(PlatformFilePath path)
		{
			ScriptingInterfaceOfIUtil.call_TakeScreenshotFromPlatformPathDelegate(path);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001BA74 File Offset: 0x00019C74
		public void TakeScreenshotFromStringPath(string path)
		{
			byte[] array = null;
			if (path != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(path);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(path, 0, path.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIUtil.call_TakeScreenshotFromStringPathDelegate(array);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001BAD0 File Offset: 0x00019CD0
		public string TakeSSFromTop(string file_name)
		{
			byte[] array = null;
			if (file_name != null)
			{
				int byteCount = ScriptingInterfaceOfIUtil._utf8.GetByteCount(file_name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIUtil._utf8.GetBytes(file_name, 0, file_name.Length, array, 0);
				array[byteCount] = 0;
			}
			if (ScriptingInterfaceOfIUtil.call_TakeSSFromTopDelegate(array) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001BB34 File Offset: 0x00019D34
		public void ToggleRender()
		{
			ScriptingInterfaceOfIUtil.call_ToggleRenderDelegate();
		}

		// Token: 0x04000586 RID: 1414
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000587 RID: 1415
		public static ScriptingInterfaceOfIUtil.AddCommandLineFunctionDelegate call_AddCommandLineFunctionDelegate;

		// Token: 0x04000588 RID: 1416
		public static ScriptingInterfaceOfIUtil.AddMainThreadPerformanceQueryDelegate call_AddMainThreadPerformanceQueryDelegate;

		// Token: 0x04000589 RID: 1417
		public static ScriptingInterfaceOfIUtil.AddPerformanceReportTokenDelegate call_AddPerformanceReportTokenDelegate;

		// Token: 0x0400058A RID: 1418
		public static ScriptingInterfaceOfIUtil.AddSceneObjectReportDelegate call_AddSceneObjectReportDelegate;

		// Token: 0x0400058B RID: 1419
		public static ScriptingInterfaceOfIUtil.CheckIfAssetsAndSourcesAreSameDelegate call_CheckIfAssetsAndSourcesAreSameDelegate;

		// Token: 0x0400058C RID: 1420
		public static ScriptingInterfaceOfIUtil.CheckIfTerrainShaderHeaderGenerationFinishedDelegate call_CheckIfTerrainShaderHeaderGenerationFinishedDelegate;

		// Token: 0x0400058D RID: 1421
		public static ScriptingInterfaceOfIUtil.CheckResourceModificationsDelegate call_CheckResourceModificationsDelegate;

		// Token: 0x0400058E RID: 1422
		public static ScriptingInterfaceOfIUtil.CheckSceneForProblemsDelegate call_CheckSceneForProblemsDelegate;

		// Token: 0x0400058F RID: 1423
		public static ScriptingInterfaceOfIUtil.CheckShaderCompilationDelegate call_CheckShaderCompilationDelegate;

		// Token: 0x04000590 RID: 1424
		public static ScriptingInterfaceOfIUtil.clear_decal_atlasDelegate call_clear_decal_atlasDelegate;

		// Token: 0x04000591 RID: 1425
		public static ScriptingInterfaceOfIUtil.ClearOldResourcesAndObjectsDelegate call_ClearOldResourcesAndObjectsDelegate;

		// Token: 0x04000592 RID: 1426
		public static ScriptingInterfaceOfIUtil.ClearShaderMemoryDelegate call_ClearShaderMemoryDelegate;

		// Token: 0x04000593 RID: 1427
		public static ScriptingInterfaceOfIUtil.CommandLineArgumentExistsDelegate call_CommandLineArgumentExistsDelegate;

		// Token: 0x04000594 RID: 1428
		public static ScriptingInterfaceOfIUtil.CompileAllShadersDelegate call_CompileAllShadersDelegate;

		// Token: 0x04000595 RID: 1429
		public static ScriptingInterfaceOfIUtil.CompileTerrainShadersDistDelegate call_CompileTerrainShadersDistDelegate;

		// Token: 0x04000596 RID: 1430
		public static ScriptingInterfaceOfIUtil.CreateSelectionInEditorDelegate call_CreateSelectionInEditorDelegate;

		// Token: 0x04000597 RID: 1431
		public static ScriptingInterfaceOfIUtil.DebugSetGlobalLoadingWindowStateDelegate call_DebugSetGlobalLoadingWindowStateDelegate;

		// Token: 0x04000598 RID: 1432
		public static ScriptingInterfaceOfIUtil.DeleteEntitiesInEditorSceneDelegate call_DeleteEntitiesInEditorSceneDelegate;

		// Token: 0x04000599 RID: 1433
		public static ScriptingInterfaceOfIUtil.DetachWatchdogDelegate call_DetachWatchdogDelegate;

		// Token: 0x0400059A RID: 1434
		public static ScriptingInterfaceOfIUtil.DidAutomatedGIBakeFinishedDelegate call_DidAutomatedGIBakeFinishedDelegate;

		// Token: 0x0400059B RID: 1435
		public static ScriptingInterfaceOfIUtil.DisableCoreGameDelegate call_DisableCoreGameDelegate;

		// Token: 0x0400059C RID: 1436
		public static ScriptingInterfaceOfIUtil.DisableGlobalEditDataCacherDelegate call_DisableGlobalEditDataCacherDelegate;

		// Token: 0x0400059D RID: 1437
		public static ScriptingInterfaceOfIUtil.DisableGlobalLoadingWindowDelegate call_DisableGlobalLoadingWindowDelegate;

		// Token: 0x0400059E RID: 1438
		public static ScriptingInterfaceOfIUtil.DoDelayedexitDelegate call_DoDelayedexitDelegate;

		// Token: 0x0400059F RID: 1439
		public static ScriptingInterfaceOfIUtil.DoFullBakeAllLevelsAutomatedDelegate call_DoFullBakeAllLevelsAutomatedDelegate;

		// Token: 0x040005A0 RID: 1440
		public static ScriptingInterfaceOfIUtil.DoFullBakeSingleLevelAutomatedDelegate call_DoFullBakeSingleLevelAutomatedDelegate;

		// Token: 0x040005A1 RID: 1441
		public static ScriptingInterfaceOfIUtil.DoLightOnlyBakeAllLevelsAutomatedDelegate call_DoLightOnlyBakeAllLevelsAutomatedDelegate;

		// Token: 0x040005A2 RID: 1442
		public static ScriptingInterfaceOfIUtil.DoLightOnlyBakeSingleLevelAutomatedDelegate call_DoLightOnlyBakeSingleLevelAutomatedDelegate;

		// Token: 0x040005A3 RID: 1443
		public static ScriptingInterfaceOfIUtil.DumpGPUMemoryStatisticsDelegate call_DumpGPUMemoryStatisticsDelegate;

		// Token: 0x040005A4 RID: 1444
		public static ScriptingInterfaceOfIUtil.EnableGlobalEditDataCacherDelegate call_EnableGlobalEditDataCacherDelegate;

		// Token: 0x040005A5 RID: 1445
		public static ScriptingInterfaceOfIUtil.EnableGlobalLoadingWindowDelegate call_EnableGlobalLoadingWindowDelegate;

		// Token: 0x040005A6 RID: 1446
		public static ScriptingInterfaceOfIUtil.EnableSingleGPUQueryPerFrameDelegate call_EnableSingleGPUQueryPerFrameDelegate;

		// Token: 0x040005A7 RID: 1447
		public static ScriptingInterfaceOfIUtil.EndLoadingStuckCheckStateDelegate call_EndLoadingStuckCheckStateDelegate;

		// Token: 0x040005A8 RID: 1448
		public static ScriptingInterfaceOfIUtil.ExecuteCommandLineCommandDelegate call_ExecuteCommandLineCommandDelegate;

		// Token: 0x040005A9 RID: 1449
		public static ScriptingInterfaceOfIUtil.ExitProcessDelegate call_ExitProcessDelegate;

		// Token: 0x040005AA RID: 1450
		public static ScriptingInterfaceOfIUtil.ExportNavMeshFaceMarksDelegate call_ExportNavMeshFaceMarksDelegate;

		// Token: 0x040005AB RID: 1451
		public static ScriptingInterfaceOfIUtil.FindMeshesWithoutLodsDelegate call_FindMeshesWithoutLodsDelegate;

		// Token: 0x040005AC RID: 1452
		public static ScriptingInterfaceOfIUtil.FlushManagedObjectsMemoryDelegate call_FlushManagedObjectsMemoryDelegate;

		// Token: 0x040005AD RID: 1453
		public static ScriptingInterfaceOfIUtil.GatherCoreGameReferencesDelegate call_GatherCoreGameReferencesDelegate;

		// Token: 0x040005AE RID: 1454
		public static ScriptingInterfaceOfIUtil.GenerateTerrainShaderHeadersDelegate call_GenerateTerrainShaderHeadersDelegate;

		// Token: 0x040005AF RID: 1455
		public static ScriptingInterfaceOfIUtil.GetApplicationMemoryDelegate call_GetApplicationMemoryDelegate;

		// Token: 0x040005B0 RID: 1456
		public static ScriptingInterfaceOfIUtil.GetApplicationMemoryStatisticsDelegate call_GetApplicationMemoryStatisticsDelegate;

		// Token: 0x040005B1 RID: 1457
		public static ScriptingInterfaceOfIUtil.GetApplicationNameDelegate call_GetApplicationNameDelegate;

		// Token: 0x040005B2 RID: 1458
		public static ScriptingInterfaceOfIUtil.GetAttachmentsPathDelegate call_GetAttachmentsPathDelegate;

		// Token: 0x040005B3 RID: 1459
		public static ScriptingInterfaceOfIUtil.GetBaseDirectoryDelegate call_GetBaseDirectoryDelegate;

		// Token: 0x040005B4 RID: 1460
		public static ScriptingInterfaceOfIUtil.GetBenchmarkStatusDelegate call_GetBenchmarkStatusDelegate;

		// Token: 0x040005B5 RID: 1461
		public static ScriptingInterfaceOfIUtil.GetBuildNumberDelegate call_GetBuildNumberDelegate;

		// Token: 0x040005B6 RID: 1462
		public static ScriptingInterfaceOfIUtil.GetConsoleHostMachineDelegate call_GetConsoleHostMachineDelegate;

		// Token: 0x040005B7 RID: 1463
		public static ScriptingInterfaceOfIUtil.GetCoreGameStateDelegate call_GetCoreGameStateDelegate;

		// Token: 0x040005B8 RID: 1464
		public static ScriptingInterfaceOfIUtil.GetCurrentCpuMemoryUsageDelegate call_GetCurrentCpuMemoryUsageDelegate;

		// Token: 0x040005B9 RID: 1465
		public static ScriptingInterfaceOfIUtil.GetCurrentEstimatedGPUMemoryCostMBDelegate call_GetCurrentEstimatedGPUMemoryCostMBDelegate;

		// Token: 0x040005BA RID: 1466
		public static ScriptingInterfaceOfIUtil.GetCurrentProcessIDDelegate call_GetCurrentProcessIDDelegate;

		// Token: 0x040005BB RID: 1467
		public static ScriptingInterfaceOfIUtil.GetCurrentThreadIdDelegate call_GetCurrentThreadIdDelegate;

		// Token: 0x040005BC RID: 1468
		public static ScriptingInterfaceOfIUtil.GetDeltaTimeDelegate call_GetDeltaTimeDelegate;

		// Token: 0x040005BD RID: 1469
		public static ScriptingInterfaceOfIUtil.GetDetailedGPUBufferMemoryStatsDelegate call_GetDetailedGPUBufferMemoryStatsDelegate;

		// Token: 0x040005BE RID: 1470
		public static ScriptingInterfaceOfIUtil.GetDetailedXBOXMemoryInfoDelegate call_GetDetailedXBOXMemoryInfoDelegate;

		// Token: 0x040005BF RID: 1471
		public static ScriptingInterfaceOfIUtil.GetEditorSelectedEntitiesDelegate call_GetEditorSelectedEntitiesDelegate;

		// Token: 0x040005C0 RID: 1472
		public static ScriptingInterfaceOfIUtil.GetEditorSelectedEntityCountDelegate call_GetEditorSelectedEntityCountDelegate;

		// Token: 0x040005C1 RID: 1473
		public static ScriptingInterfaceOfIUtil.GetEngineFrameNoDelegate call_GetEngineFrameNoDelegate;

		// Token: 0x040005C2 RID: 1474
		public static ScriptingInterfaceOfIUtil.GetEntitiesOfSelectionSetDelegate call_GetEntitiesOfSelectionSetDelegate;

		// Token: 0x040005C3 RID: 1475
		public static ScriptingInterfaceOfIUtil.GetEntityCountOfSelectionSetDelegate call_GetEntityCountOfSelectionSetDelegate;

		// Token: 0x040005C4 RID: 1476
		public static ScriptingInterfaceOfIUtil.GetExecutableWorkingDirectoryDelegate call_GetExecutableWorkingDirectoryDelegate;

		// Token: 0x040005C5 RID: 1477
		public static ScriptingInterfaceOfIUtil.GetFpsDelegate call_GetFpsDelegate;

		// Token: 0x040005C6 RID: 1478
		public static ScriptingInterfaceOfIUtil.GetFrameLimiterWithSleepDelegate call_GetFrameLimiterWithSleepDelegate;

		// Token: 0x040005C7 RID: 1479
		public static ScriptingInterfaceOfIUtil.GetFullCommandLineStringDelegate call_GetFullCommandLineStringDelegate;

		// Token: 0x040005C8 RID: 1480
		public static ScriptingInterfaceOfIUtil.GetFullFilePathOfSceneDelegate call_GetFullFilePathOfSceneDelegate;

		// Token: 0x040005C9 RID: 1481
		public static ScriptingInterfaceOfIUtil.GetFullModulePathDelegate call_GetFullModulePathDelegate;

		// Token: 0x040005CA RID: 1482
		public static ScriptingInterfaceOfIUtil.GetFullModulePathsDelegate call_GetFullModulePathsDelegate;

		// Token: 0x040005CB RID: 1483
		public static ScriptingInterfaceOfIUtil.GetGPUMemoryMBDelegate call_GetGPUMemoryMBDelegate;

		// Token: 0x040005CC RID: 1484
		public static ScriptingInterfaceOfIUtil.GetGpuMemoryOfAllocationGroupDelegate call_GetGpuMemoryOfAllocationGroupDelegate;

		// Token: 0x040005CD RID: 1485
		public static ScriptingInterfaceOfIUtil.GetGPUMemoryStatsDelegate call_GetGPUMemoryStatsDelegate;

		// Token: 0x040005CE RID: 1486
		public static ScriptingInterfaceOfIUtil.GetLocalOutputPathDelegate call_GetLocalOutputPathDelegate;

		// Token: 0x040005CF RID: 1487
		public static ScriptingInterfaceOfIUtil.GetMainFpsDelegate call_GetMainFpsDelegate;

		// Token: 0x040005D0 RID: 1488
		public static ScriptingInterfaceOfIUtil.GetMainThreadIdDelegate call_GetMainThreadIdDelegate;

		// Token: 0x040005D1 RID: 1489
		public static ScriptingInterfaceOfIUtil.GetMemoryUsageOfCategoryDelegate call_GetMemoryUsageOfCategoryDelegate;

		// Token: 0x040005D2 RID: 1490
		public static ScriptingInterfaceOfIUtil.GetModulesCodeDelegate call_GetModulesCodeDelegate;

		// Token: 0x040005D3 RID: 1491
		public static ScriptingInterfaceOfIUtil.GetNativeMemoryStatisticsDelegate call_GetNativeMemoryStatisticsDelegate;

		// Token: 0x040005D4 RID: 1492
		public static ScriptingInterfaceOfIUtil.GetNumberOfShaderCompilationsInProgressDelegate call_GetNumberOfShaderCompilationsInProgressDelegate;

		// Token: 0x040005D5 RID: 1493
		public static ScriptingInterfaceOfIUtil.GetPCInfoDelegate call_GetPCInfoDelegate;

		// Token: 0x040005D6 RID: 1494
		public static ScriptingInterfaceOfIUtil.GetPlatformModulePathsDelegate call_GetPlatformModulePathsDelegate;

		// Token: 0x040005D7 RID: 1495
		public static ScriptingInterfaceOfIUtil.GetPossibleCommandLineStartingWithDelegate call_GetPossibleCommandLineStartingWithDelegate;

		// Token: 0x040005D8 RID: 1496
		public static ScriptingInterfaceOfIUtil.GetRendererFpsDelegate call_GetRendererFpsDelegate;

		// Token: 0x040005D9 RID: 1497
		public static ScriptingInterfaceOfIUtil.GetReturnCodeDelegate call_GetReturnCodeDelegate;

		// Token: 0x040005DA RID: 1498
		public static ScriptingInterfaceOfIUtil.GetSingleModuleScenesOfModuleDelegate call_GetSingleModuleScenesOfModuleDelegate;

		// Token: 0x040005DB RID: 1499
		public static ScriptingInterfaceOfIUtil.GetSteamAppIdDelegate call_GetSteamAppIdDelegate;

		// Token: 0x040005DC RID: 1500
		public static ScriptingInterfaceOfIUtil.GetSystemLanguageDelegate call_GetSystemLanguageDelegate;

		// Token: 0x040005DD RID: 1501
		public static ScriptingInterfaceOfIUtil.GetVertexBufferChunkSystemMemoryUsageDelegate call_GetVertexBufferChunkSystemMemoryUsageDelegate;

		// Token: 0x040005DE RID: 1502
		public static ScriptingInterfaceOfIUtil.GetVisualTestsTestFilesPathDelegate call_GetVisualTestsTestFilesPathDelegate;

		// Token: 0x040005DF RID: 1503
		public static ScriptingInterfaceOfIUtil.GetVisualTestsValidatePathDelegate call_GetVisualTestsValidatePathDelegate;

		// Token: 0x040005E0 RID: 1504
		public static ScriptingInterfaceOfIUtil.IsAsyncPhysicsThreadDelegate call_IsAsyncPhysicsThreadDelegate;

		// Token: 0x040005E1 RID: 1505
		public static ScriptingInterfaceOfIUtil.IsBenchmarkQuitedDelegate call_IsBenchmarkQuitedDelegate;

		// Token: 0x040005E2 RID: 1506
		public static ScriptingInterfaceOfIUtil.IsDetailedSoundLogOnDelegate call_IsDetailedSoundLogOnDelegate;

		// Token: 0x040005E3 RID: 1507
		public static ScriptingInterfaceOfIUtil.IsDevkitDelegate call_IsDevkitDelegate;

		// Token: 0x040005E4 RID: 1508
		public static ScriptingInterfaceOfIUtil.IsEditModeEnabledDelegate call_IsEditModeEnabledDelegate;

		// Token: 0x040005E5 RID: 1509
		public static ScriptingInterfaceOfIUtil.IsLockhartPlatformDelegate call_IsLockhartPlatformDelegate;

		// Token: 0x040005E6 RID: 1510
		public static ScriptingInterfaceOfIUtil.IsSceneReportFinishedDelegate call_IsSceneReportFinishedDelegate;

		// Token: 0x040005E7 RID: 1511
		public static ScriptingInterfaceOfIUtil.LoadSkyBoxesDelegate call_LoadSkyBoxesDelegate;

		// Token: 0x040005E8 RID: 1512
		public static ScriptingInterfaceOfIUtil.LoadVirtualTextureTilesetDelegate call_LoadVirtualTextureTilesetDelegate;

		// Token: 0x040005E9 RID: 1513
		public static ScriptingInterfaceOfIUtil.ManagedParallelForDelegate call_ManagedParallelForDelegate;

		// Token: 0x040005EA RID: 1514
		public static ScriptingInterfaceOfIUtil.ManagedParallelForWithDtDelegate call_ManagedParallelForWithDtDelegate;

		// Token: 0x040005EB RID: 1515
		public static ScriptingInterfaceOfIUtil.ManagedParallelForWithoutRenderThreadDelegate call_ManagedParallelForWithoutRenderThreadDelegate;

		// Token: 0x040005EC RID: 1516
		public static ScriptingInterfaceOfIUtil.ManagedParallelForWithoutRenderThreadDtDelegate call_ManagedParallelForWithoutRenderThreadDtDelegate;

		// Token: 0x040005ED RID: 1517
		public static ScriptingInterfaceOfIUtil.OnLoadingWindowDisabledDelegate call_OnLoadingWindowDisabledDelegate;

		// Token: 0x040005EE RID: 1518
		public static ScriptingInterfaceOfIUtil.OnLoadingWindowEnabledDelegate call_OnLoadingWindowEnabledDelegate;

		// Token: 0x040005EF RID: 1519
		public static ScriptingInterfaceOfIUtil.OpenConsoleStorePageDelegate call_OpenConsoleStorePageDelegate;

		// Token: 0x040005F0 RID: 1520
		public static ScriptingInterfaceOfIUtil.OpenOnscreenKeyboardDelegate call_OpenOnscreenKeyboardDelegate;

		// Token: 0x040005F1 RID: 1521
		public static ScriptingInterfaceOfIUtil.OutputBenchmarkValuesToPerformanceReporterDelegate call_OutputBenchmarkValuesToPerformanceReporterDelegate;

		// Token: 0x040005F2 RID: 1522
		public static ScriptingInterfaceOfIUtil.OutputPerformanceReportsDelegate call_OutputPerformanceReportsDelegate;

		// Token: 0x040005F3 RID: 1523
		public static ScriptingInterfaceOfIUtil.PairSceneNameToModuleNameDelegate call_PairSceneNameToModuleNameDelegate;

		// Token: 0x040005F4 RID: 1524
		public static ScriptingInterfaceOfIUtil.ProcessWindowTitleDelegate call_ProcessWindowTitleDelegate;

		// Token: 0x040005F5 RID: 1525
		public static ScriptingInterfaceOfIUtil.QuitGameDelegate call_QuitGameDelegate;

		// Token: 0x040005F6 RID: 1526
		public static ScriptingInterfaceOfIUtil.RegisterGPUAllocationGroupDelegate call_RegisterGPUAllocationGroupDelegate;

		// Token: 0x040005F7 RID: 1527
		public static ScriptingInterfaceOfIUtil.RegisterMeshForGPUMorphDelegate call_RegisterMeshForGPUMorphDelegate;

		// Token: 0x040005F8 RID: 1528
		public static ScriptingInterfaceOfIUtil.SaveDataAsTextureDelegate call_SaveDataAsTextureDelegate;

		// Token: 0x040005F9 RID: 1529
		public static ScriptingInterfaceOfIUtil.SelectEntitiesDelegate call_SelectEntitiesDelegate;

		// Token: 0x040005FA RID: 1530
		public static ScriptingInterfaceOfIUtil.SetAllocationAlwaysValidSceneDelegate call_SetAllocationAlwaysValidSceneDelegate;

		// Token: 0x040005FB RID: 1531
		public static ScriptingInterfaceOfIUtil.SetAssertionAtShaderCompileDelegate call_SetAssertionAtShaderCompileDelegate;

		// Token: 0x040005FC RID: 1532
		public static ScriptingInterfaceOfIUtil.SetAssertionsAndWarningsSetExitCodeDelegate call_SetAssertionsAndWarningsSetExitCodeDelegate;

		// Token: 0x040005FD RID: 1533
		public static ScriptingInterfaceOfIUtil.SetBenchmarkStatusDelegate call_SetBenchmarkStatusDelegate;

		// Token: 0x040005FE RID: 1534
		public static ScriptingInterfaceOfIUtil.SetCanLoadModulesDelegate call_SetCanLoadModulesDelegate;

		// Token: 0x040005FF RID: 1535
		public static ScriptingInterfaceOfIUtil.SetCoreGameStateDelegate call_SetCoreGameStateDelegate;

		// Token: 0x04000600 RID: 1536
		public static ScriptingInterfaceOfIUtil.SetCrashOnAssertsDelegate call_SetCrashOnAssertsDelegate;

		// Token: 0x04000601 RID: 1537
		public static ScriptingInterfaceOfIUtil.SetCrashOnWarningsDelegate call_SetCrashOnWarningsDelegate;

		// Token: 0x04000602 RID: 1538
		public static ScriptingInterfaceOfIUtil.SetCrashReportCustomStackDelegate call_SetCrashReportCustomStackDelegate;

		// Token: 0x04000603 RID: 1539
		public static ScriptingInterfaceOfIUtil.SetCrashReportCustomStringDelegate call_SetCrashReportCustomStringDelegate;

		// Token: 0x04000604 RID: 1540
		public static ScriptingInterfaceOfIUtil.SetCreateDumpOnWarningsDelegate call_SetCreateDumpOnWarningsDelegate;

		// Token: 0x04000605 RID: 1541
		public static ScriptingInterfaceOfIUtil.SetDisableDumpGenerationDelegate call_SetDisableDumpGenerationDelegate;

		// Token: 0x04000606 RID: 1542
		public static ScriptingInterfaceOfIUtil.SetDumpFolderPathDelegate call_SetDumpFolderPathDelegate;

		// Token: 0x04000607 RID: 1543
		public static ScriptingInterfaceOfIUtil.SetFixedDtDelegate call_SetFixedDtDelegate;

		// Token: 0x04000608 RID: 1544
		public static ScriptingInterfaceOfIUtil.SetForceDrawEntityIDDelegate call_SetForceDrawEntityIDDelegate;

		// Token: 0x04000609 RID: 1545
		public static ScriptingInterfaceOfIUtil.SetForceVsyncDelegate call_SetForceVsyncDelegate;

		// Token: 0x0400060A RID: 1546
		public static ScriptingInterfaceOfIUtil.SetFrameLimiterWithSleepDelegate call_SetFrameLimiterWithSleepDelegate;

		// Token: 0x0400060B RID: 1547
		public static ScriptingInterfaceOfIUtil.SetGraphicsPresetDelegate call_SetGraphicsPresetDelegate;

		// Token: 0x0400060C RID: 1548
		public static ScriptingInterfaceOfIUtil.SetLoadingScreenPercentageDelegate call_SetLoadingScreenPercentageDelegate;

		// Token: 0x0400060D RID: 1549
		public static ScriptingInterfaceOfIUtil.SetMessageLineRenderingStateDelegate call_SetMessageLineRenderingStateDelegate;

		// Token: 0x0400060E RID: 1550
		public static ScriptingInterfaceOfIUtil.SetPrintCallstackAtCrahsesDelegate call_SetPrintCallstackAtCrahsesDelegate;

		// Token: 0x0400060F RID: 1551
		public static ScriptingInterfaceOfIUtil.SetRenderAgentsDelegate call_SetRenderAgentsDelegate;

		// Token: 0x04000610 RID: 1552
		public static ScriptingInterfaceOfIUtil.SetRenderModeDelegate call_SetRenderModeDelegate;

		// Token: 0x04000611 RID: 1553
		public static ScriptingInterfaceOfIUtil.SetReportModeDelegate call_SetReportModeDelegate;

		// Token: 0x04000612 RID: 1554
		public static ScriptingInterfaceOfIUtil.SetScreenTextRenderingStateDelegate call_SetScreenTextRenderingStateDelegate;

		// Token: 0x04000613 RID: 1555
		public static ScriptingInterfaceOfIUtil.SetWatchdogAutoreportDelegate call_SetWatchdogAutoreportDelegate;

		// Token: 0x04000614 RID: 1556
		public static ScriptingInterfaceOfIUtil.SetWatchdogValueDelegate call_SetWatchdogValueDelegate;

		// Token: 0x04000615 RID: 1557
		public static ScriptingInterfaceOfIUtil.SetWindowTitleDelegate call_SetWindowTitleDelegate;

		// Token: 0x04000616 RID: 1558
		public static ScriptingInterfaceOfIUtil.StartLoadingStuckCheckStateDelegate call_StartLoadingStuckCheckStateDelegate;

		// Token: 0x04000617 RID: 1559
		public static ScriptingInterfaceOfIUtil.StartScenePerformanceReportDelegate call_StartScenePerformanceReportDelegate;

		// Token: 0x04000618 RID: 1560
		public static ScriptingInterfaceOfIUtil.TakeScreenshotFromPlatformPathDelegate call_TakeScreenshotFromPlatformPathDelegate;

		// Token: 0x04000619 RID: 1561
		public static ScriptingInterfaceOfIUtil.TakeScreenshotFromStringPathDelegate call_TakeScreenshotFromStringPathDelegate;

		// Token: 0x0400061A RID: 1562
		public static ScriptingInterfaceOfIUtil.TakeSSFromTopDelegate call_TakeSSFromTopDelegate;

		// Token: 0x0400061B RID: 1563
		public static ScriptingInterfaceOfIUtil.ToggleRenderDelegate call_ToggleRenderDelegate;

		// Token: 0x020005E5 RID: 1509
		// (Invoke) Token: 0x06001DAD RID: 7597
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddCommandLineFunctionDelegate(byte[] concatName);

		// Token: 0x020005E6 RID: 1510
		// (Invoke) Token: 0x06001DB1 RID: 7601
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMainThreadPerformanceQueryDelegate(byte[] parent, byte[] name, float seconds);

		// Token: 0x020005E7 RID: 1511
		// (Invoke) Token: 0x06001DB5 RID: 7605
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPerformanceReportTokenDelegate(byte[] performance_type, byte[] name, float loading_time);

		// Token: 0x020005E8 RID: 1512
		// (Invoke) Token: 0x06001DB9 RID: 7609
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSceneObjectReportDelegate(byte[] scene_name, byte[] report_name, float report_value);

		// Token: 0x020005E9 RID: 1513
		// (Invoke) Token: 0x06001DBD RID: 7613
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CheckIfAssetsAndSourcesAreSameDelegate();

		// Token: 0x020005EA RID: 1514
		// (Invoke) Token: 0x06001DC1 RID: 7617
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckIfTerrainShaderHeaderGenerationFinishedDelegate();

		// Token: 0x020005EB RID: 1515
		// (Invoke) Token: 0x06001DC5 RID: 7621
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CheckResourceModificationsDelegate();

		// Token: 0x020005EC RID: 1516
		// (Invoke) Token: 0x06001DC9 RID: 7625
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CheckSceneForProblemsDelegate(byte[] path);

		// Token: 0x020005ED RID: 1517
		// (Invoke) Token: 0x06001DCD RID: 7629
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckShaderCompilationDelegate();

		// Token: 0x020005EE RID: 1518
		// (Invoke) Token: 0x06001DD1 RID: 7633
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void clear_decal_atlasDelegate(DecalAtlasGroup atlasGroup);

		// Token: 0x020005EF RID: 1519
		// (Invoke) Token: 0x06001DD5 RID: 7637
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearOldResourcesAndObjectsDelegate();

		// Token: 0x020005F0 RID: 1520
		// (Invoke) Token: 0x06001DD9 RID: 7641
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearShaderMemoryDelegate();

		// Token: 0x020005F1 RID: 1521
		// (Invoke) Token: 0x06001DDD RID: 7645
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CommandLineArgumentExistsDelegate(byte[] str);

		// Token: 0x020005F2 RID: 1522
		// (Invoke) Token: 0x06001DE1 RID: 7649
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CompileAllShadersDelegate(byte[] targetPlatform);

		// Token: 0x020005F3 RID: 1523
		// (Invoke) Token: 0x06001DE5 RID: 7653
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CompileTerrainShadersDistDelegate(byte[] targetPlatform, byte[] targetConfig, byte[] output_path);

		// Token: 0x020005F4 RID: 1524
		// (Invoke) Token: 0x06001DE9 RID: 7657
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CreateSelectionInEditorDelegate(IntPtr gameEntities, int entityCount, byte[] name);

		// Token: 0x020005F5 RID: 1525
		// (Invoke) Token: 0x06001DED RID: 7661
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DebugSetGlobalLoadingWindowStateDelegate([MarshalAs(UnmanagedType.U1)] bool s);

		// Token: 0x020005F6 RID: 1526
		// (Invoke) Token: 0x06001DF1 RID: 7665
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeleteEntitiesInEditorSceneDelegate(IntPtr gameEntities, int entityCount);

		// Token: 0x020005F7 RID: 1527
		// (Invoke) Token: 0x06001DF5 RID: 7669
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DetachWatchdogDelegate();

		// Token: 0x020005F8 RID: 1528
		// (Invoke) Token: 0x06001DF9 RID: 7673
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool DidAutomatedGIBakeFinishedDelegate();

		// Token: 0x020005F9 RID: 1529
		// (Invoke) Token: 0x06001DFD RID: 7677
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisableCoreGameDelegate();

		// Token: 0x020005FA RID: 1530
		// (Invoke) Token: 0x06001E01 RID: 7681
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisableGlobalEditDataCacherDelegate();

		// Token: 0x020005FB RID: 1531
		// (Invoke) Token: 0x06001E05 RID: 7685
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DisableGlobalLoadingWindowDelegate();

		// Token: 0x020005FC RID: 1532
		// (Invoke) Token: 0x06001E09 RID: 7689
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DoDelayedexitDelegate(int returnCode);

		// Token: 0x020005FD RID: 1533
		// (Invoke) Token: 0x06001E0D RID: 7693
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DoFullBakeAllLevelsAutomatedDelegate(byte[] module, byte[] sceneName);

		// Token: 0x020005FE RID: 1534
		// (Invoke) Token: 0x06001E11 RID: 7697
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DoFullBakeSingleLevelAutomatedDelegate(byte[] module, byte[] sceneName);

		// Token: 0x020005FF RID: 1535
		// (Invoke) Token: 0x06001E15 RID: 7701
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DoLightOnlyBakeAllLevelsAutomatedDelegate(byte[] module, byte[] sceneName);

		// Token: 0x02000600 RID: 1536
		// (Invoke) Token: 0x06001E19 RID: 7705
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DoLightOnlyBakeSingleLevelAutomatedDelegate(byte[] module, byte[] sceneName);

		// Token: 0x02000601 RID: 1537
		// (Invoke) Token: 0x06001E1D RID: 7709
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DumpGPUMemoryStatisticsDelegate(byte[] filePath);

		// Token: 0x02000602 RID: 1538
		// (Invoke) Token: 0x06001E21 RID: 7713
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnableGlobalEditDataCacherDelegate();

		// Token: 0x02000603 RID: 1539
		// (Invoke) Token: 0x06001E25 RID: 7717
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnableGlobalLoadingWindowDelegate();

		// Token: 0x02000604 RID: 1540
		// (Invoke) Token: 0x06001E29 RID: 7721
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnableSingleGPUQueryPerFrameDelegate();

		// Token: 0x02000605 RID: 1541
		// (Invoke) Token: 0x06001E2D RID: 7725
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EndLoadingStuckCheckStateDelegate();

		// Token: 0x02000606 RID: 1542
		// (Invoke) Token: 0x06001E31 RID: 7729
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int ExecuteCommandLineCommandDelegate(byte[] command);

		// Token: 0x02000607 RID: 1543
		// (Invoke) Token: 0x06001E35 RID: 7733
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ExitProcessDelegate(int exitCode);

		// Token: 0x02000608 RID: 1544
		// (Invoke) Token: 0x06001E39 RID: 7737
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int ExportNavMeshFaceMarksDelegate(byte[] file_name);

		// Token: 0x02000609 RID: 1545
		// (Invoke) Token: 0x06001E3D RID: 7741
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FindMeshesWithoutLodsDelegate(byte[] module_name);

		// Token: 0x0200060A RID: 1546
		// (Invoke) Token: 0x06001E41 RID: 7745
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FlushManagedObjectsMemoryDelegate();

		// Token: 0x0200060B RID: 1547
		// (Invoke) Token: 0x06001E45 RID: 7749
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GatherCoreGameReferencesDelegate(byte[] scene_names);

		// Token: 0x0200060C RID: 1548
		// (Invoke) Token: 0x06001E49 RID: 7753
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GenerateTerrainShaderHeadersDelegate(byte[] targetPlatform, byte[] targetConfig, byte[] output_path);

		// Token: 0x0200060D RID: 1549
		// (Invoke) Token: 0x06001E4D RID: 7757
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetApplicationMemoryDelegate();

		// Token: 0x0200060E RID: 1550
		// (Invoke) Token: 0x06001E51 RID: 7761
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetApplicationMemoryStatisticsDelegate();

		// Token: 0x0200060F RID: 1551
		// (Invoke) Token: 0x06001E55 RID: 7765
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetApplicationNameDelegate();

		// Token: 0x02000610 RID: 1552
		// (Invoke) Token: 0x06001E59 RID: 7769
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAttachmentsPathDelegate();

		// Token: 0x02000611 RID: 1553
		// (Invoke) Token: 0x06001E5D RID: 7773
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetBaseDirectoryDelegate();

		// Token: 0x02000612 RID: 1554
		// (Invoke) Token: 0x06001E61 RID: 7777
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetBenchmarkStatusDelegate();

		// Token: 0x02000613 RID: 1555
		// (Invoke) Token: 0x06001E65 RID: 7781
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetBuildNumberDelegate();

		// Token: 0x02000614 RID: 1556
		// (Invoke) Token: 0x06001E69 RID: 7785
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetConsoleHostMachineDelegate();

		// Token: 0x02000615 RID: 1557
		// (Invoke) Token: 0x06001E6D RID: 7789
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetCoreGameStateDelegate();

		// Token: 0x02000616 RID: 1558
		// (Invoke) Token: 0x06001E71 RID: 7793
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ulong GetCurrentCpuMemoryUsageDelegate();

		// Token: 0x02000617 RID: 1559
		// (Invoke) Token: 0x06001E75 RID: 7797
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetCurrentEstimatedGPUMemoryCostMBDelegate();

		// Token: 0x02000618 RID: 1560
		// (Invoke) Token: 0x06001E79 RID: 7801
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetCurrentProcessIDDelegate();

		// Token: 0x02000619 RID: 1561
		// (Invoke) Token: 0x06001E7D RID: 7805
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ulong GetCurrentThreadIdDelegate();

		// Token: 0x0200061A RID: 1562
		// (Invoke) Token: 0x06001E81 RID: 7809
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetDeltaTimeDelegate(int timerId);

		// Token: 0x0200061B RID: 1563
		// (Invoke) Token: 0x06001E85 RID: 7813
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetDetailedGPUBufferMemoryStatsDelegate(ref int totalMemoryAllocated, ref int totalMemoryUsed, ref int emptyChunkCount);

		// Token: 0x0200061C RID: 1564
		// (Invoke) Token: 0x06001E89 RID: 7817
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetDetailedXBOXMemoryInfoDelegate();

		// Token: 0x0200061D RID: 1565
		// (Invoke) Token: 0x06001E8D RID: 7821
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetEditorSelectedEntitiesDelegate(IntPtr gameEntitiesTemp);

		// Token: 0x0200061E RID: 1566
		// (Invoke) Token: 0x06001E91 RID: 7825
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetEditorSelectedEntityCountDelegate();

		// Token: 0x0200061F RID: 1567
		// (Invoke) Token: 0x06001E95 RID: 7829
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetEngineFrameNoDelegate();

		// Token: 0x02000620 RID: 1568
		// (Invoke) Token: 0x06001E99 RID: 7833
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetEntitiesOfSelectionSetDelegate(byte[] name, IntPtr gameEntitiesTemp);

		// Token: 0x02000621 RID: 1569
		// (Invoke) Token: 0x06001E9D RID: 7837
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetEntityCountOfSelectionSetDelegate(byte[] name);

		// Token: 0x02000622 RID: 1570
		// (Invoke) Token: 0x06001EA1 RID: 7841
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetExecutableWorkingDirectoryDelegate();

		// Token: 0x02000623 RID: 1571
		// (Invoke) Token: 0x06001EA5 RID: 7845
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetFpsDelegate();

		// Token: 0x02000624 RID: 1572
		// (Invoke) Token: 0x06001EA9 RID: 7849
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetFrameLimiterWithSleepDelegate();

		// Token: 0x02000625 RID: 1573
		// (Invoke) Token: 0x06001EAD RID: 7853
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFullCommandLineStringDelegate();

		// Token: 0x02000626 RID: 1574
		// (Invoke) Token: 0x06001EB1 RID: 7857
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFullFilePathOfSceneDelegate(byte[] sceneName);

		// Token: 0x02000627 RID: 1575
		// (Invoke) Token: 0x06001EB5 RID: 7861
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFullModulePathDelegate(byte[] moduleName);

		// Token: 0x02000628 RID: 1576
		// (Invoke) Token: 0x06001EB9 RID: 7865
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFullModulePathsDelegate();

		// Token: 0x02000629 RID: 1577
		// (Invoke) Token: 0x06001EBD RID: 7869
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetGPUMemoryMBDelegate();

		// Token: 0x0200062A RID: 1578
		// (Invoke) Token: 0x06001EC1 RID: 7873
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ulong GetGpuMemoryOfAllocationGroupDelegate(byte[] allocationName);

		// Token: 0x0200062B RID: 1579
		// (Invoke) Token: 0x06001EC5 RID: 7877
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetGPUMemoryStatsDelegate(ref float totalMemory, ref float renderTargetMemory, ref float depthTargetMemory, ref float srvMemory, ref float bufferMemory);

		// Token: 0x0200062C RID: 1580
		// (Invoke) Token: 0x06001EC9 RID: 7881
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetLocalOutputPathDelegate();

		// Token: 0x0200062D RID: 1581
		// (Invoke) Token: 0x06001ECD RID: 7885
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetMainFpsDelegate();

		// Token: 0x0200062E RID: 1582
		// (Invoke) Token: 0x06001ED1 RID: 7889
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ulong GetMainThreadIdDelegate();

		// Token: 0x0200062F RID: 1583
		// (Invoke) Token: 0x06001ED5 RID: 7893
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMemoryUsageOfCategoryDelegate(int index);

		// Token: 0x02000630 RID: 1584
		// (Invoke) Token: 0x06001ED9 RID: 7897
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetModulesCodeDelegate();

		// Token: 0x02000631 RID: 1585
		// (Invoke) Token: 0x06001EDD RID: 7901
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNativeMemoryStatisticsDelegate();

		// Token: 0x02000632 RID: 1586
		// (Invoke) Token: 0x06001EE1 RID: 7905
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumberOfShaderCompilationsInProgressDelegate();

		// Token: 0x02000633 RID: 1587
		// (Invoke) Token: 0x06001EE5 RID: 7909
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetPCInfoDelegate();

		// Token: 0x02000634 RID: 1588
		// (Invoke) Token: 0x06001EE9 RID: 7913
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetPlatformModulePathsDelegate();

		// Token: 0x02000635 RID: 1589
		// (Invoke) Token: 0x06001EED RID: 7917
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetPossibleCommandLineStartingWithDelegate(byte[] command, int index);

		// Token: 0x02000636 RID: 1590
		// (Invoke) Token: 0x06001EF1 RID: 7921
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRendererFpsDelegate();

		// Token: 0x02000637 RID: 1591
		// (Invoke) Token: 0x06001EF5 RID: 7925
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetReturnCodeDelegate();

		// Token: 0x02000638 RID: 1592
		// (Invoke) Token: 0x06001EF9 RID: 7929
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSingleModuleScenesOfModuleDelegate(byte[] moduleName);

		// Token: 0x02000639 RID: 1593
		// (Invoke) Token: 0x06001EFD RID: 7933
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSteamAppIdDelegate();

		// Token: 0x0200063A RID: 1594
		// (Invoke) Token: 0x06001F01 RID: 7937
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSystemLanguageDelegate();

		// Token: 0x0200063B RID: 1595
		// (Invoke) Token: 0x06001F05 RID: 7941
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVertexBufferChunkSystemMemoryUsageDelegate();

		// Token: 0x0200063C RID: 1596
		// (Invoke) Token: 0x06001F09 RID: 7945
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVisualTestsTestFilesPathDelegate();

		// Token: 0x0200063D RID: 1597
		// (Invoke) Token: 0x06001F0D RID: 7949
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVisualTestsValidatePathDelegate();

		// Token: 0x0200063E RID: 1598
		// (Invoke) Token: 0x06001F11 RID: 7953
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsAsyncPhysicsThreadDelegate();

		// Token: 0x0200063F RID: 1599
		// (Invoke) Token: 0x06001F15 RID: 7957
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsBenchmarkQuitedDelegate();

		// Token: 0x02000640 RID: 1600
		// (Invoke) Token: 0x06001F19 RID: 7961
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int IsDetailedSoundLogOnDelegate();

		// Token: 0x02000641 RID: 1601
		// (Invoke) Token: 0x06001F1D RID: 7965
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsDevkitDelegate();

		// Token: 0x02000642 RID: 1602
		// (Invoke) Token: 0x06001F21 RID: 7969
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEditModeEnabledDelegate();

		// Token: 0x02000643 RID: 1603
		// (Invoke) Token: 0x06001F25 RID: 7973
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsLockhartPlatformDelegate();

		// Token: 0x02000644 RID: 1604
		// (Invoke) Token: 0x06001F29 RID: 7977
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsSceneReportFinishedDelegate();

		// Token: 0x02000645 RID: 1605
		// (Invoke) Token: 0x06001F2D RID: 7981
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LoadSkyBoxesDelegate();

		// Token: 0x02000646 RID: 1606
		// (Invoke) Token: 0x06001F31 RID: 7985
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LoadVirtualTextureTilesetDelegate(byte[] name);

		// Token: 0x02000647 RID: 1607
		// (Invoke) Token: 0x06001F35 RID: 7989
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ManagedParallelForDelegate(int fromInclusive, int toExclusive, long curKey, int grainSize);

		// Token: 0x02000648 RID: 1608
		// (Invoke) Token: 0x06001F39 RID: 7993
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ManagedParallelForWithDtDelegate(int fromInclusive, int toExclusive, long curKey, int grainSize);

		// Token: 0x02000649 RID: 1609
		// (Invoke) Token: 0x06001F3D RID: 7997
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ManagedParallelForWithoutRenderThreadDelegate(int fromInclusive, int toExclusive, long curKey, int grainSize);

		// Token: 0x0200064A RID: 1610
		// (Invoke) Token: 0x06001F41 RID: 8001
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ManagedParallelForWithoutRenderThreadDtDelegate(int fromInclusive, int toExclusive, long curKey, int grainSize);

		// Token: 0x0200064B RID: 1611
		// (Invoke) Token: 0x06001F45 RID: 8005
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnLoadingWindowDisabledDelegate();

		// Token: 0x0200064C RID: 1612
		// (Invoke) Token: 0x06001F49 RID: 8009
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnLoadingWindowEnabledDelegate();

		// Token: 0x0200064D RID: 1613
		// (Invoke) Token: 0x06001F4D RID: 8013
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OpenConsoleStorePageDelegate(byte[] productId);

		// Token: 0x0200064E RID: 1614
		// (Invoke) Token: 0x06001F51 RID: 8017
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OpenOnscreenKeyboardDelegate(byte[] initialText, byte[] descriptionText, int maxLength, int keyboardTypeEnum);

		// Token: 0x0200064F RID: 1615
		// (Invoke) Token: 0x06001F55 RID: 8021
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OutputBenchmarkValuesToPerformanceReporterDelegate();

		// Token: 0x02000650 RID: 1616
		// (Invoke) Token: 0x06001F59 RID: 8025
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OutputPerformanceReportsDelegate();

		// Token: 0x02000651 RID: 1617
		// (Invoke) Token: 0x06001F5D RID: 8029
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PairSceneNameToModuleNameDelegate(byte[] sceneName, byte[] moduleName);

		// Token: 0x02000652 RID: 1618
		// (Invoke) Token: 0x06001F61 RID: 8033
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int ProcessWindowTitleDelegate(byte[] title);

		// Token: 0x02000653 RID: 1619
		// (Invoke) Token: 0x06001F65 RID: 8037
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void QuitGameDelegate();

		// Token: 0x02000654 RID: 1620
		// (Invoke) Token: 0x06001F69 RID: 8041
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int RegisterGPUAllocationGroupDelegate(byte[] name);

		// Token: 0x02000655 RID: 1621
		// (Invoke) Token: 0x06001F6D RID: 8045
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RegisterMeshForGPUMorphDelegate(byte[] metaMeshName);

		// Token: 0x02000656 RID: 1622
		// (Invoke) Token: 0x06001F71 RID: 8049
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int SaveDataAsTextureDelegate(byte[] path, int width, int height, IntPtr data);

		// Token: 0x02000657 RID: 1623
		// (Invoke) Token: 0x06001F75 RID: 8053
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SelectEntitiesDelegate(IntPtr gameEntities, int entityCount);

		// Token: 0x02000658 RID: 1624
		// (Invoke) Token: 0x06001F79 RID: 8057
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAllocationAlwaysValidSceneDelegate(UIntPtr scene);

		// Token: 0x02000659 RID: 1625
		// (Invoke) Token: 0x06001F7D RID: 8061
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAssertionAtShaderCompileDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200065A RID: 1626
		// (Invoke) Token: 0x06001F81 RID: 8065
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAssertionsAndWarningsSetExitCodeDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200065B RID: 1627
		// (Invoke) Token: 0x06001F85 RID: 8069
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBenchmarkStatusDelegate(int status, byte[] def);

		// Token: 0x0200065C RID: 1628
		// (Invoke) Token: 0x06001F89 RID: 8073
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCanLoadModulesDelegate([MarshalAs(UnmanagedType.U1)] bool canLoadModules);

		// Token: 0x0200065D RID: 1629
		// (Invoke) Token: 0x06001F8D RID: 8077
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCoreGameStateDelegate(int state);

		// Token: 0x0200065E RID: 1630
		// (Invoke) Token: 0x06001F91 RID: 8081
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCrashOnAssertsDelegate([MarshalAs(UnmanagedType.U1)] bool val);

		// Token: 0x0200065F RID: 1631
		// (Invoke) Token: 0x06001F95 RID: 8085
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCrashOnWarningsDelegate([MarshalAs(UnmanagedType.U1)] bool val);

		// Token: 0x02000660 RID: 1632
		// (Invoke) Token: 0x06001F99 RID: 8089
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCrashReportCustomStackDelegate(byte[] customStack);

		// Token: 0x02000661 RID: 1633
		// (Invoke) Token: 0x06001F9D RID: 8093
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCrashReportCustomStringDelegate(byte[] customString);

		// Token: 0x02000662 RID: 1634
		// (Invoke) Token: 0x06001FA1 RID: 8097
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCreateDumpOnWarningsDelegate([MarshalAs(UnmanagedType.U1)] bool val);

		// Token: 0x02000663 RID: 1635
		// (Invoke) Token: 0x06001FA5 RID: 8101
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDisableDumpGenerationDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000664 RID: 1636
		// (Invoke) Token: 0x06001FA9 RID: 8105
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDumpFolderPathDelegate(byte[] path);

		// Token: 0x02000665 RID: 1637
		// (Invoke) Token: 0x06001FAD RID: 8109
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFixedDtDelegate([MarshalAs(UnmanagedType.U1)] bool enabled, float dt);

		// Token: 0x02000666 RID: 1638
		// (Invoke) Token: 0x06001FB1 RID: 8113
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetForceDrawEntityIDDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000667 RID: 1639
		// (Invoke) Token: 0x06001FB5 RID: 8117
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetForceVsyncDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000668 RID: 1640
		// (Invoke) Token: 0x06001FB9 RID: 8121
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameLimiterWithSleepDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000669 RID: 1641
		// (Invoke) Token: 0x06001FBD RID: 8125
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGraphicsPresetDelegate(int preset);

		// Token: 0x0200066A RID: 1642
		// (Invoke) Token: 0x06001FC1 RID: 8129
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLoadingScreenPercentageDelegate(float value);

		// Token: 0x0200066B RID: 1643
		// (Invoke) Token: 0x06001FC5 RID: 8133
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMessageLineRenderingStateDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200066C RID: 1644
		// (Invoke) Token: 0x06001FC9 RID: 8137
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPrintCallstackAtCrahsesDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200066D RID: 1645
		// (Invoke) Token: 0x06001FCD RID: 8141
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderAgentsDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200066E RID: 1646
		// (Invoke) Token: 0x06001FD1 RID: 8145
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderModeDelegate(int mode);

		// Token: 0x0200066F RID: 1647
		// (Invoke) Token: 0x06001FD5 RID: 8149
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetReportModeDelegate([MarshalAs(UnmanagedType.U1)] bool reportMode);

		// Token: 0x02000670 RID: 1648
		// (Invoke) Token: 0x06001FD9 RID: 8153
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetScreenTextRenderingStateDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000671 RID: 1649
		// (Invoke) Token: 0x06001FDD RID: 8157
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetWatchdogAutoreportDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000672 RID: 1650
		// (Invoke) Token: 0x06001FE1 RID: 8161
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetWatchdogValueDelegate(byte[] fileName, byte[] groupName, byte[] key, byte[] value);

		// Token: 0x02000673 RID: 1651
		// (Invoke) Token: 0x06001FE5 RID: 8165
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetWindowTitleDelegate(byte[] title);

		// Token: 0x02000674 RID: 1652
		// (Invoke) Token: 0x06001FE9 RID: 8169
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StartLoadingStuckCheckStateDelegate(float seconds);

		// Token: 0x02000675 RID: 1653
		// (Invoke) Token: 0x06001FED RID: 8173
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StartScenePerformanceReportDelegate(byte[] folderPath);

		// Token: 0x02000676 RID: 1654
		// (Invoke) Token: 0x06001FF1 RID: 8177
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TakeScreenshotFromPlatformPathDelegate(PlatformFilePath path);

		// Token: 0x02000677 RID: 1655
		// (Invoke) Token: 0x06001FF5 RID: 8181
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TakeScreenshotFromStringPathDelegate(byte[] path);

		// Token: 0x02000678 RID: 1656
		// (Invoke) Token: 0x06001FF9 RID: 8185
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int TakeSSFromTopDelegate(byte[] file_name);

		// Token: 0x02000679 RID: 1657
		// (Invoke) Token: 0x06001FFD RID: 8189
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ToggleRenderDelegate();
	}
}
