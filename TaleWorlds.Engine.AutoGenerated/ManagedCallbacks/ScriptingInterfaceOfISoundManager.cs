using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000029 RID: 41
	internal class ScriptingInterfaceOfISoundManager : ISoundManager
	{
		// Token: 0x060005CE RID: 1486 RVA: 0x00018E2F File Offset: 0x0001702F
		public void AddSoundClientWithId(ulong client_id)
		{
			ScriptingInterfaceOfISoundManager.call_AddSoundClientWithIdDelegate(client_id);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00018E3C File Offset: 0x0001703C
		public void AddXBOXRemoteUser(ulong XUID, ulong deviceID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			ScriptingInterfaceOfISoundManager.call_AddXBOXRemoteUserDelegate(XUID, deviceID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00018E53 File Offset: 0x00017053
		public void ApplyPushToTalk(bool pushed)
		{
			ScriptingInterfaceOfISoundManager.call_ApplyPushToTalkDelegate(pushed);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00018E60 File Offset: 0x00017060
		public void ClearDataToBeSent()
		{
			ScriptingInterfaceOfISoundManager.call_ClearDataToBeSentDelegate();
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00018E6C File Offset: 0x0001706C
		public void ClearXBOXSoundManager()
		{
			ScriptingInterfaceOfISoundManager.call_ClearXBOXSoundManagerDelegate();
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00018E78 File Offset: 0x00017078
		public void CompressData(ulong clientID, byte[] buffer, int length, byte[] compressedBuffer, ref int compressedBufferLength)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(buffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (buffer != null) ? buffer.Length : 0);
			PinnedArrayData<byte> pinnedArrayData2 = new PinnedArrayData<byte>(compressedBuffer, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			ManagedArray managedArray2 = new ManagedArray(pointer2, (compressedBuffer != null) ? compressedBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_CompressDataDelegate(clientID, managedArray, length, managedArray2, ref compressedBufferLength);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00018EED File Offset: 0x000170ED
		public void CreateVoiceEvent()
		{
			ScriptingInterfaceOfISoundManager.call_CreateVoiceEventDelegate();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00018EFC File Offset: 0x000170FC
		public void DecompressData(ulong clientID, byte[] compressedBuffer, int compressedBufferLength, byte[] decompressedBuffer, ref int decompressedBufferLength)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(compressedBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (compressedBuffer != null) ? compressedBuffer.Length : 0);
			PinnedArrayData<byte> pinnedArrayData2 = new PinnedArrayData<byte>(decompressedBuffer, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			ManagedArray managedArray2 = new ManagedArray(pointer2, (decompressedBuffer != null) ? decompressedBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_DecompressDataDelegate(clientID, managedArray, compressedBufferLength, managedArray2, ref decompressedBufferLength);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00018F71 File Offset: 0x00017171
		public void DeleteSoundClientWithId(ulong client_id)
		{
			ScriptingInterfaceOfISoundManager.call_DeleteSoundClientWithIdDelegate(client_id);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00018F7E File Offset: 0x0001717E
		public void DestroyVoiceEvent(int id)
		{
			ScriptingInterfaceOfISoundManager.call_DestroyVoiceEventDelegate(id);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00018F8B File Offset: 0x0001718B
		public void FinalizeVoicePlayEvent()
		{
			ScriptingInterfaceOfISoundManager.call_FinalizeVoicePlayEventDelegate();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00018F97 File Offset: 0x00017197
		public void GetAttenuationPosition(out Vec3 result)
		{
			ScriptingInterfaceOfISoundManager.call_GetAttenuationPositionDelegate(out result);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00018FA4 File Offset: 0x000171A4
		public bool GetDataToBeSentAt(int index, byte[] buffer, ulong[] receivers, ref bool transportGuaranteed)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(buffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (buffer != null) ? buffer.Length : 0);
			PinnedArrayData<ulong> pinnedArrayData2 = new PinnedArrayData<ulong>(receivers, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			bool flag = ScriptingInterfaceOfISoundManager.call_GetDataToBeSentAtDelegate(index, managedArray, pointer2, ref transportGuaranteed);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
			return flag;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00019004 File Offset: 0x00017204
		public int GetGlobalIndexOfEvent(string eventFullName)
		{
			byte[] array = null;
			if (eventFullName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(eventFullName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(eventFullName, 0, eventFullName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISoundManager.call_GetGlobalIndexOfEventDelegate(array);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001905E File Offset: 0x0001725E
		public void GetListenerFrame(out MatrixFrame result)
		{
			ScriptingInterfaceOfISoundManager.call_GetListenerFrameDelegate(out result);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001906B File Offset: 0x0001726B
		public void GetSizeOfDataToBeSentAt(int index, ref uint byte_count, ref uint numReceivers)
		{
			ScriptingInterfaceOfISoundManager.call_GetSizeOfDataToBeSentAtDelegate(index, ref byte_count, ref numReceivers);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001907C File Offset: 0x0001727C
		public void GetVoiceData(byte[] voiceBuffer, int chunkSize, ref int readBytesLength)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(voiceBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (voiceBuffer != null) ? voiceBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_GetVoiceDataDelegate(managedArray, chunkSize, ref readBytesLength);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000190BF File Offset: 0x000172BF
		public void HandleStateChanges()
		{
			ScriptingInterfaceOfISoundManager.call_HandleStateChangesDelegate();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000190CB File Offset: 0x000172CB
		public void InitializeVoicePlayEvent()
		{
			ScriptingInterfaceOfISoundManager.call_InitializeVoicePlayEventDelegate();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000190D7 File Offset: 0x000172D7
		public void InitializeXBOXSoundManager()
		{
			ScriptingInterfaceOfISoundManager.call_InitializeXBOXSoundManagerDelegate();
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000190E4 File Offset: 0x000172E4
		public void LoadEventFileAux(string soundBankName, bool decompressSamples)
		{
			byte[] array = null;
			if (soundBankName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(soundBankName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(soundBankName, 0, soundBankName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_LoadEventFileAuxDelegate(array, decompressSamples);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00019140 File Offset: 0x00017340
		public void PauseBus(string busName)
		{
			byte[] array = null;
			if (busName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(busName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(busName, 0, busName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_PauseBusDelegate(array);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001919C File Offset: 0x0001739C
		public void ProcessDataToBeReceived(ulong senderDeviceID, byte[] data, uint dataSize)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(data, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (data != null) ? data.Length : 0);
			ScriptingInterfaceOfISoundManager.call_ProcessDataToBeReceivedDelegate(senderDeviceID, managedArray, dataSize);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000191DF File Offset: 0x000173DF
		public void ProcessDataToBeSent(ref int numData)
		{
			ScriptingInterfaceOfISoundManager.call_ProcessDataToBeSentDelegate(ref numData);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000191EC File Offset: 0x000173EC
		public void RemoveXBOXRemoteUser(ulong XUID)
		{
			ScriptingInterfaceOfISoundManager.call_RemoveXBOXRemoteUserDelegate(XUID);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000191F9 File Offset: 0x000173F9
		public void Reset()
		{
			ScriptingInterfaceOfISoundManager.call_ResetDelegate();
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00019208 File Offset: 0x00017408
		public void SetGlobalParameter(string parameterName, float value)
		{
			byte[] array = null;
			if (parameterName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(parameterName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(parameterName, 0, parameterName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_SetGlobalParameterDelegate(array, value);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00019263 File Offset: 0x00017463
		public void SetListenerFrame(ref MatrixFrame frame, ref Vec3 attenuationPosition)
		{
			ScriptingInterfaceOfISoundManager.call_SetListenerFrameDelegate(ref frame, ref attenuationPosition);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00019274 File Offset: 0x00017474
		public void SetState(string stateGroup, string state)
		{
			byte[] array = null;
			if (stateGroup != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(stateGroup);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(stateGroup, 0, stateGroup.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (state != null)
			{
				int byteCount2 = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(state);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(state, 0, state.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_SetStateDelegate(array, array2);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00019314 File Offset: 0x00017514
		public bool StartOneShotEvent(string eventFullName, Vec3 position)
		{
			byte[] array = null;
			if (eventFullName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(eventFullName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(eventFullName, 0, eventFullName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISoundManager.call_StartOneShotEventDelegate(array, position);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001936F File Offset: 0x0001756F
		public bool StartOneShotEventWithIndex(int index, Vec3 position)
		{
			return ScriptingInterfaceOfISoundManager.call_StartOneShotEventWithIndexDelegate(index, position);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00019380 File Offset: 0x00017580
		public bool StartOneShotEventWithParam(string eventFullName, Vec3 position, string paramName, float paramValue)
		{
			byte[] array = null;
			if (eventFullName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(eventFullName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(eventFullName, 0, eventFullName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (paramName != null)
			{
				int byteCount2 = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(paramName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(paramName, 0, paramName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfISoundManager.call_StartOneShotEventWithParamDelegate(array, position, array2, paramValue);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00019420 File Offset: 0x00017620
		public void StartVoiceRecord()
		{
			ScriptingInterfaceOfISoundManager.call_StartVoiceRecordDelegate();
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001942C File Offset: 0x0001762C
		public void StopVoiceRecord()
		{
			ScriptingInterfaceOfISoundManager.call_StopVoiceRecordDelegate();
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00019438 File Offset: 0x00017638
		public void UnpauseBus(string busName)
		{
			byte[] array = null;
			if (busName != null)
			{
				int byteCount = ScriptingInterfaceOfISoundManager._utf8.GetByteCount(busName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISoundManager._utf8.GetBytes(busName, 0, busName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISoundManager.call_UnpauseBusDelegate(array);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00019494 File Offset: 0x00017694
		public void UpdateVoiceToPlay(byte[] voiceBuffer, int length, int index)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(voiceBuffer, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (voiceBuffer != null) ? voiceBuffer.Length : 0);
			ScriptingInterfaceOfISoundManager.call_UpdateVoiceToPlayDelegate(managedArray, length, index);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000194D7 File Offset: 0x000176D7
		public void UpdateXBOXChatCommunicationFlags(ulong XUID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			ScriptingInterfaceOfISoundManager.call_UpdateXBOXChatCommunicationFlagsDelegate(XUID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000194EC File Offset: 0x000176EC
		public void UpdateXBOXLocalUser()
		{
			ScriptingInterfaceOfISoundManager.call_UpdateXBOXLocalUserDelegate();
		}

		// Token: 0x04000522 RID: 1314
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000523 RID: 1315
		public static ScriptingInterfaceOfISoundManager.AddSoundClientWithIdDelegate call_AddSoundClientWithIdDelegate;

		// Token: 0x04000524 RID: 1316
		public static ScriptingInterfaceOfISoundManager.AddXBOXRemoteUserDelegate call_AddXBOXRemoteUserDelegate;

		// Token: 0x04000525 RID: 1317
		public static ScriptingInterfaceOfISoundManager.ApplyPushToTalkDelegate call_ApplyPushToTalkDelegate;

		// Token: 0x04000526 RID: 1318
		public static ScriptingInterfaceOfISoundManager.ClearDataToBeSentDelegate call_ClearDataToBeSentDelegate;

		// Token: 0x04000527 RID: 1319
		public static ScriptingInterfaceOfISoundManager.ClearXBOXSoundManagerDelegate call_ClearXBOXSoundManagerDelegate;

		// Token: 0x04000528 RID: 1320
		public static ScriptingInterfaceOfISoundManager.CompressDataDelegate call_CompressDataDelegate;

		// Token: 0x04000529 RID: 1321
		public static ScriptingInterfaceOfISoundManager.CreateVoiceEventDelegate call_CreateVoiceEventDelegate;

		// Token: 0x0400052A RID: 1322
		public static ScriptingInterfaceOfISoundManager.DecompressDataDelegate call_DecompressDataDelegate;

		// Token: 0x0400052B RID: 1323
		public static ScriptingInterfaceOfISoundManager.DeleteSoundClientWithIdDelegate call_DeleteSoundClientWithIdDelegate;

		// Token: 0x0400052C RID: 1324
		public static ScriptingInterfaceOfISoundManager.DestroyVoiceEventDelegate call_DestroyVoiceEventDelegate;

		// Token: 0x0400052D RID: 1325
		public static ScriptingInterfaceOfISoundManager.FinalizeVoicePlayEventDelegate call_FinalizeVoicePlayEventDelegate;

		// Token: 0x0400052E RID: 1326
		public static ScriptingInterfaceOfISoundManager.GetAttenuationPositionDelegate call_GetAttenuationPositionDelegate;

		// Token: 0x0400052F RID: 1327
		public static ScriptingInterfaceOfISoundManager.GetDataToBeSentAtDelegate call_GetDataToBeSentAtDelegate;

		// Token: 0x04000530 RID: 1328
		public static ScriptingInterfaceOfISoundManager.GetGlobalIndexOfEventDelegate call_GetGlobalIndexOfEventDelegate;

		// Token: 0x04000531 RID: 1329
		public static ScriptingInterfaceOfISoundManager.GetListenerFrameDelegate call_GetListenerFrameDelegate;

		// Token: 0x04000532 RID: 1330
		public static ScriptingInterfaceOfISoundManager.GetSizeOfDataToBeSentAtDelegate call_GetSizeOfDataToBeSentAtDelegate;

		// Token: 0x04000533 RID: 1331
		public static ScriptingInterfaceOfISoundManager.GetVoiceDataDelegate call_GetVoiceDataDelegate;

		// Token: 0x04000534 RID: 1332
		public static ScriptingInterfaceOfISoundManager.HandleStateChangesDelegate call_HandleStateChangesDelegate;

		// Token: 0x04000535 RID: 1333
		public static ScriptingInterfaceOfISoundManager.InitializeVoicePlayEventDelegate call_InitializeVoicePlayEventDelegate;

		// Token: 0x04000536 RID: 1334
		public static ScriptingInterfaceOfISoundManager.InitializeXBOXSoundManagerDelegate call_InitializeXBOXSoundManagerDelegate;

		// Token: 0x04000537 RID: 1335
		public static ScriptingInterfaceOfISoundManager.LoadEventFileAuxDelegate call_LoadEventFileAuxDelegate;

		// Token: 0x04000538 RID: 1336
		public static ScriptingInterfaceOfISoundManager.PauseBusDelegate call_PauseBusDelegate;

		// Token: 0x04000539 RID: 1337
		public static ScriptingInterfaceOfISoundManager.ProcessDataToBeReceivedDelegate call_ProcessDataToBeReceivedDelegate;

		// Token: 0x0400053A RID: 1338
		public static ScriptingInterfaceOfISoundManager.ProcessDataToBeSentDelegate call_ProcessDataToBeSentDelegate;

		// Token: 0x0400053B RID: 1339
		public static ScriptingInterfaceOfISoundManager.RemoveXBOXRemoteUserDelegate call_RemoveXBOXRemoteUserDelegate;

		// Token: 0x0400053C RID: 1340
		public static ScriptingInterfaceOfISoundManager.ResetDelegate call_ResetDelegate;

		// Token: 0x0400053D RID: 1341
		public static ScriptingInterfaceOfISoundManager.SetGlobalParameterDelegate call_SetGlobalParameterDelegate;

		// Token: 0x0400053E RID: 1342
		public static ScriptingInterfaceOfISoundManager.SetListenerFrameDelegate call_SetListenerFrameDelegate;

		// Token: 0x0400053F RID: 1343
		public static ScriptingInterfaceOfISoundManager.SetStateDelegate call_SetStateDelegate;

		// Token: 0x04000540 RID: 1344
		public static ScriptingInterfaceOfISoundManager.StartOneShotEventDelegate call_StartOneShotEventDelegate;

		// Token: 0x04000541 RID: 1345
		public static ScriptingInterfaceOfISoundManager.StartOneShotEventWithIndexDelegate call_StartOneShotEventWithIndexDelegate;

		// Token: 0x04000542 RID: 1346
		public static ScriptingInterfaceOfISoundManager.StartOneShotEventWithParamDelegate call_StartOneShotEventWithParamDelegate;

		// Token: 0x04000543 RID: 1347
		public static ScriptingInterfaceOfISoundManager.StartVoiceRecordDelegate call_StartVoiceRecordDelegate;

		// Token: 0x04000544 RID: 1348
		public static ScriptingInterfaceOfISoundManager.StopVoiceRecordDelegate call_StopVoiceRecordDelegate;

		// Token: 0x04000545 RID: 1349
		public static ScriptingInterfaceOfISoundManager.UnpauseBusDelegate call_UnpauseBusDelegate;

		// Token: 0x04000546 RID: 1350
		public static ScriptingInterfaceOfISoundManager.UpdateVoiceToPlayDelegate call_UpdateVoiceToPlayDelegate;

		// Token: 0x04000547 RID: 1351
		public static ScriptingInterfaceOfISoundManager.UpdateXBOXChatCommunicationFlagsDelegate call_UpdateXBOXChatCommunicationFlagsDelegate;

		// Token: 0x04000548 RID: 1352
		public static ScriptingInterfaceOfISoundManager.UpdateXBOXLocalUserDelegate call_UpdateXBOXLocalUserDelegate;

		// Token: 0x02000588 RID: 1416
		// (Invoke) Token: 0x06001C39 RID: 7225
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSoundClientWithIdDelegate(ulong client_id);

		// Token: 0x02000589 RID: 1417
		// (Invoke) Token: 0x06001C3D RID: 7229
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddXBOXRemoteUserDelegate(ulong XUID, ulong deviceID, [MarshalAs(UnmanagedType.U1)] bool canSendMicSound, [MarshalAs(UnmanagedType.U1)] bool canSendTextSound, [MarshalAs(UnmanagedType.U1)] bool canSendText, [MarshalAs(UnmanagedType.U1)] bool canReceiveSound, [MarshalAs(UnmanagedType.U1)] bool canReceiveText);

		// Token: 0x0200058A RID: 1418
		// (Invoke) Token: 0x06001C41 RID: 7233
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ApplyPushToTalkDelegate([MarshalAs(UnmanagedType.U1)] bool pushed);

		// Token: 0x0200058B RID: 1419
		// (Invoke) Token: 0x06001C45 RID: 7237
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearDataToBeSentDelegate();

		// Token: 0x0200058C RID: 1420
		// (Invoke) Token: 0x06001C49 RID: 7241
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearXBOXSoundManagerDelegate();

		// Token: 0x0200058D RID: 1421
		// (Invoke) Token: 0x06001C4D RID: 7245
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CompressDataDelegate(ulong clientID, ManagedArray buffer, int length, ManagedArray compressedBuffer, ref int compressedBufferLength);

		// Token: 0x0200058E RID: 1422
		// (Invoke) Token: 0x06001C51 RID: 7249
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CreateVoiceEventDelegate();

		// Token: 0x0200058F RID: 1423
		// (Invoke) Token: 0x06001C55 RID: 7253
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DecompressDataDelegate(ulong clientID, ManagedArray compressedBuffer, int compressedBufferLength, ManagedArray decompressedBuffer, ref int decompressedBufferLength);

		// Token: 0x02000590 RID: 1424
		// (Invoke) Token: 0x06001C59 RID: 7257
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeleteSoundClientWithIdDelegate(ulong client_id);

		// Token: 0x02000591 RID: 1425
		// (Invoke) Token: 0x06001C5D RID: 7261
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DestroyVoiceEventDelegate(int id);

		// Token: 0x02000592 RID: 1426
		// (Invoke) Token: 0x06001C61 RID: 7265
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FinalizeVoicePlayEventDelegate();

		// Token: 0x02000593 RID: 1427
		// (Invoke) Token: 0x06001C65 RID: 7269
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetAttenuationPositionDelegate(out Vec3 result);

		// Token: 0x02000594 RID: 1428
		// (Invoke) Token: 0x06001C69 RID: 7273
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetDataToBeSentAtDelegate(int index, ManagedArray buffer, IntPtr receivers, [MarshalAs(UnmanagedType.U1)] ref bool transportGuaranteed);

		// Token: 0x02000595 RID: 1429
		// (Invoke) Token: 0x06001C6D RID: 7277
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetGlobalIndexOfEventDelegate(byte[] eventFullName);

		// Token: 0x02000596 RID: 1430
		// (Invoke) Token: 0x06001C71 RID: 7281
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetListenerFrameDelegate(out MatrixFrame result);

		// Token: 0x02000597 RID: 1431
		// (Invoke) Token: 0x06001C75 RID: 7285
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSizeOfDataToBeSentAtDelegate(int index, ref uint byte_count, ref uint numReceivers);

		// Token: 0x02000598 RID: 1432
		// (Invoke) Token: 0x06001C79 RID: 7289
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetVoiceDataDelegate(ManagedArray voiceBuffer, int chunkSize, ref int readBytesLength);

		// Token: 0x02000599 RID: 1433
		// (Invoke) Token: 0x06001C7D RID: 7293
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void HandleStateChangesDelegate();

		// Token: 0x0200059A RID: 1434
		// (Invoke) Token: 0x06001C81 RID: 7297
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeVoicePlayEventDelegate();

		// Token: 0x0200059B RID: 1435
		// (Invoke) Token: 0x06001C85 RID: 7301
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeXBOXSoundManagerDelegate();

		// Token: 0x0200059C RID: 1436
		// (Invoke) Token: 0x06001C89 RID: 7305
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LoadEventFileAuxDelegate(byte[] soundBankName, [MarshalAs(UnmanagedType.U1)] bool decompressSamples);

		// Token: 0x0200059D RID: 1437
		// (Invoke) Token: 0x06001C8D RID: 7309
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PauseBusDelegate(byte[] busName);

		// Token: 0x0200059E RID: 1438
		// (Invoke) Token: 0x06001C91 RID: 7313
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProcessDataToBeReceivedDelegate(ulong senderDeviceID, ManagedArray data, uint dataSize);

		// Token: 0x0200059F RID: 1439
		// (Invoke) Token: 0x06001C95 RID: 7317
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProcessDataToBeSentDelegate(ref int numData);

		// Token: 0x020005A0 RID: 1440
		// (Invoke) Token: 0x06001C99 RID: 7321
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveXBOXRemoteUserDelegate(ulong XUID);

		// Token: 0x020005A1 RID: 1441
		// (Invoke) Token: 0x06001C9D RID: 7325
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetDelegate();

		// Token: 0x020005A2 RID: 1442
		// (Invoke) Token: 0x06001CA1 RID: 7329
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGlobalParameterDelegate(byte[] parameterName, float value);

		// Token: 0x020005A3 RID: 1443
		// (Invoke) Token: 0x06001CA5 RID: 7333
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetListenerFrameDelegate(ref MatrixFrame frame, ref Vec3 attenuationPosition);

		// Token: 0x020005A4 RID: 1444
		// (Invoke) Token: 0x06001CA9 RID: 7337
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetStateDelegate(byte[] stateGroup, byte[] state);

		// Token: 0x020005A5 RID: 1445
		// (Invoke) Token: 0x06001CAD RID: 7341
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartOneShotEventDelegate(byte[] eventFullName, Vec3 position);

		// Token: 0x020005A6 RID: 1446
		// (Invoke) Token: 0x06001CB1 RID: 7345
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartOneShotEventWithIndexDelegate(int index, Vec3 position);

		// Token: 0x020005A7 RID: 1447
		// (Invoke) Token: 0x06001CB5 RID: 7349
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool StartOneShotEventWithParamDelegate(byte[] eventFullName, Vec3 position, byte[] paramName, float paramValue);

		// Token: 0x020005A8 RID: 1448
		// (Invoke) Token: 0x06001CB9 RID: 7353
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StartVoiceRecordDelegate();

		// Token: 0x020005A9 RID: 1449
		// (Invoke) Token: 0x06001CBD RID: 7357
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StopVoiceRecordDelegate();

		// Token: 0x020005AA RID: 1450
		// (Invoke) Token: 0x06001CC1 RID: 7361
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnpauseBusDelegate(byte[] busName);

		// Token: 0x020005AB RID: 1451
		// (Invoke) Token: 0x06001CC5 RID: 7365
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateVoiceToPlayDelegate(ManagedArray voiceBuffer, int length, int index);

		// Token: 0x020005AC RID: 1452
		// (Invoke) Token: 0x06001CC9 RID: 7369
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateXBOXChatCommunicationFlagsDelegate(ulong XUID, [MarshalAs(UnmanagedType.U1)] bool canSendMicSound, [MarshalAs(UnmanagedType.U1)] bool canSendTextSound, [MarshalAs(UnmanagedType.U1)] bool canSendText, [MarshalAs(UnmanagedType.U1)] bool canReceiveSound, [MarshalAs(UnmanagedType.U1)] bool canReceiveText);

		// Token: 0x020005AD RID: 1453
		// (Invoke) Token: 0x06001CCD RID: 7373
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateXBOXLocalUserDelegate();
	}
}
