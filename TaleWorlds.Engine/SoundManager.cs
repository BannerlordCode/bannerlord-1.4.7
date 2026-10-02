using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008F RID: 143
	public static class SoundManager
	{
		// Token: 0x06000CBD RID: 3261 RVA: 0x0000E365 File Offset: 0x0000C565
		public static void SetListenerFrame(MatrixFrame frame)
		{
			EngineApplicationInterface.ISoundManager.SetListenerFrame(ref frame, ref frame.origin);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0000E37A File Offset: 0x0000C57A
		public static void SetListenerFrame(MatrixFrame frame, Vec3 attenuationPosition)
		{
			EngineApplicationInterface.ISoundManager.SetListenerFrame(ref frame, ref attenuationPosition);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0000E38C File Offset: 0x0000C58C
		public static MatrixFrame GetListenerFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.ISoundManager.GetListenerFrame(out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		public static Vec3 GetAttenuationPosition()
		{
			Vec3 vec;
			EngineApplicationInterface.ISoundManager.GetAttenuationPosition(out vec);
			return vec;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0000E3C2 File Offset: 0x0000C5C2
		public static void Reset()
		{
			EngineApplicationInterface.ISoundManager.Reset();
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0000E3CE File Offset: 0x0000C5CE
		public static bool StartOneShotEvent(string eventFullName, in Vec3 position, string paramName, float paramValue)
		{
			return EngineApplicationInterface.ISoundManager.StartOneShotEventWithParam(eventFullName, position, paramName, paramValue);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0000E3E3 File Offset: 0x0000C5E3
		public static bool StartOneShotEvent(string eventFullName, in Vec3 position)
		{
			return EngineApplicationInterface.ISoundManager.StartOneShotEvent(eventFullName, position);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0000E3F6 File Offset: 0x0000C5F6
		public static bool StartOneShotEventWithIndex(int index, in Vec3 position)
		{
			return EngineApplicationInterface.ISoundManager.StartOneShotEventWithIndex(index, position);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0000E409 File Offset: 0x0000C609
		public static void SetState(string stateGroup, string state)
		{
			EngineApplicationInterface.ISoundManager.SetState(stateGroup, state);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0000E417 File Offset: 0x0000C617
		public static SoundEvent CreateEvent(string eventFullName, Scene scene)
		{
			return SoundEvent.CreateEventFromString(eventFullName, scene);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0000E420 File Offset: 0x0000C620
		public static void LoadEventFileAux(string soundBank, bool decompressSamples)
		{
			if (!SoundManager._loaded)
			{
				EngineApplicationInterface.ISoundManager.LoadEventFileAux(soundBank, decompressSamples);
				SoundManager._loaded = true;
			}
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0000E43B File Offset: 0x0000C63B
		public static void AddSoundClientWithId(ulong clientId)
		{
			EngineApplicationInterface.ISoundManager.AddSoundClientWithId(clientId);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0000E448 File Offset: 0x0000C648
		public static void DeleteSoundClientWithId(ulong clientId)
		{
			EngineApplicationInterface.ISoundManager.DeleteSoundClientWithId(clientId);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0000E455 File Offset: 0x0000C655
		public static void SetGlobalParameter(string parameterName, float value)
		{
			EngineApplicationInterface.ISoundManager.SetGlobalParameter(parameterName, value);
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0000E463 File Offset: 0x0000C663
		public static int GetEventGlobalIndex(string eventFullName)
		{
			if (string.IsNullOrEmpty(eventFullName))
			{
				return -1;
			}
			return EngineApplicationInterface.ISoundManager.GetGlobalIndexOfEvent(eventFullName);
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0000E47A File Offset: 0x0000C67A
		public static void PauseBus(string busName)
		{
			EngineApplicationInterface.ISoundManager.PauseBus(busName);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0000E487 File Offset: 0x0000C687
		public static void UnpauseBus(string busName)
		{
			EngineApplicationInterface.ISoundManager.UnpauseBus(busName);
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0000E494 File Offset: 0x0000C694
		public static void InitializeVoicePlayEvent()
		{
			EngineApplicationInterface.ISoundManager.InitializeVoicePlayEvent();
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
		public static void CreateVoiceEvent()
		{
			EngineApplicationInterface.ISoundManager.CreateVoiceEvent();
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0000E4AC File Offset: 0x0000C6AC
		public static void DestroyVoiceEvent(int id)
		{
			EngineApplicationInterface.ISoundManager.DestroyVoiceEvent(id);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0000E4B9 File Offset: 0x0000C6B9
		public static void FinalizeVoicePlayEvent()
		{
			EngineApplicationInterface.ISoundManager.FinalizeVoicePlayEvent();
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0000E4C5 File Offset: 0x0000C6C5
		public static void StartVoiceRecording()
		{
			EngineApplicationInterface.ISoundManager.StartVoiceRecord();
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0000E4D1 File Offset: 0x0000C6D1
		public static void StopVoiceRecording()
		{
			EngineApplicationInterface.ISoundManager.StopVoiceRecord();
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0000E4DD File Offset: 0x0000C6DD
		public static void GetVoiceData(byte[] voiceBuffer, int chunkSize, out int readBytesLength)
		{
			readBytesLength = 0;
			EngineApplicationInterface.ISoundManager.GetVoiceData(voiceBuffer, chunkSize, ref readBytesLength);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0000E4EF File Offset: 0x0000C6EF
		public static void UpdateVoiceToPlay(byte[] voiceBuffer, int length, int index)
		{
			EngineApplicationInterface.ISoundManager.UpdateVoiceToPlay(voiceBuffer, length, index);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0000E4FE File Offset: 0x0000C6FE
		public static void AddXBOXRemoteUser(ulong XUID, ulong deviceID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			EngineApplicationInterface.ISoundManager.AddXBOXRemoteUser(XUID, deviceID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0000E514 File Offset: 0x0000C714
		public static void InitializeXBOXSoundManager()
		{
			EngineApplicationInterface.ISoundManager.InitializeXBOXSoundManager();
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0000E520 File Offset: 0x0000C720
		public static void ApplyPushToTalk(bool pushed)
		{
			EngineApplicationInterface.ISoundManager.ApplyPushToTalk(pushed);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0000E52D File Offset: 0x0000C72D
		public static void ClearXBOXSoundManager()
		{
			EngineApplicationInterface.ISoundManager.ClearXBOXSoundManager();
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0000E539 File Offset: 0x0000C739
		public static void UpdateXBOXLocalUser()
		{
			EngineApplicationInterface.ISoundManager.UpdateXBOXLocalUser();
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0000E545 File Offset: 0x0000C745
		public static void UpdateXBOXChatCommunicationFlags(ulong XUID, bool canSendMicSound, bool canSendTextSound, bool canSendText, bool canReceiveSound, bool canReceiveText)
		{
			EngineApplicationInterface.ISoundManager.UpdateXBOXChatCommunicationFlags(XUID, canSendMicSound, canSendTextSound, canSendText, canReceiveSound, canReceiveText);
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0000E559 File Offset: 0x0000C759
		public static void RemoveXBOXRemoteUser(ulong XUID)
		{
			EngineApplicationInterface.ISoundManager.RemoveXBOXRemoteUser(XUID);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0000E566 File Offset: 0x0000C766
		public static void ProcessDataToBeReceived(ulong senderDeviceID, byte[] data, uint dataSize)
		{
			EngineApplicationInterface.ISoundManager.ProcessDataToBeReceived(senderDeviceID, data, dataSize);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0000E575 File Offset: 0x0000C775
		public static void ProcessDataToBeSent(ref int numData)
		{
			EngineApplicationInterface.ISoundManager.ProcessDataToBeSent(ref numData);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0000E582 File Offset: 0x0000C782
		public static void HandleStateChanges()
		{
			EngineApplicationInterface.ISoundManager.HandleStateChanges();
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0000E58E File Offset: 0x0000C78E
		public static void GetSizeOfDataToBeSentAt(int index, ref uint byteCount, ref uint numReceivers)
		{
			EngineApplicationInterface.ISoundManager.GetSizeOfDataToBeSentAt(index, ref byteCount, ref numReceivers);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0000E59D File Offset: 0x0000C79D
		public static bool GetDataToBeSentAt(int index, byte[] buffer, ulong[] receivers, ref bool transportGuaranteed)
		{
			return EngineApplicationInterface.ISoundManager.GetDataToBeSentAt(index, buffer, receivers, ref transportGuaranteed);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0000E5AD File Offset: 0x0000C7AD
		public static void ClearDataToBeSent()
		{
			EngineApplicationInterface.ISoundManager.ClearDataToBeSent();
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0000E5B9 File Offset: 0x0000C7B9
		public static void CompressData(int clientID, byte[] buffer, int length, byte[] compressedBuffer, out int compressedBufferLength)
		{
			compressedBufferLength = 0;
			EngineApplicationInterface.ISoundManager.CompressData((ulong)((long)clientID), buffer, length, compressedBuffer, ref compressedBufferLength);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0000E5D0 File Offset: 0x0000C7D0
		public static void DecompressData(int clientID, byte[] compressedBuffer, int compressedBufferLength, byte[] decompressedBuffer, out int decompressedBufferLength)
		{
			decompressedBufferLength = 0;
			EngineApplicationInterface.ISoundManager.DecompressData((ulong)((long)clientID), compressedBuffer, compressedBufferLength, decompressedBuffer, ref decompressedBufferLength);
		}

		// Token: 0x040001CA RID: 458
		private static bool _loaded;
	}
}
