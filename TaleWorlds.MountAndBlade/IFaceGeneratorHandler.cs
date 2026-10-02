using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000248 RID: 584
	public interface IFaceGeneratorHandler
	{
		// Token: 0x06002186 RID: 8582
		void ChangeToBodyCamera();

		// Token: 0x06002187 RID: 8583
		void ChangeToEyeCamera();

		// Token: 0x06002188 RID: 8584
		void ChangeToNoseCamera();

		// Token: 0x06002189 RID: 8585
		void ChangeToMouthCamera();

		// Token: 0x0600218A RID: 8586
		void ChangeToFaceCamera();

		// Token: 0x0600218B RID: 8587
		void ChangeToHairCamera();

		// Token: 0x0600218C RID: 8588
		void RefreshCharacterEntity();

		// Token: 0x0600218D RID: 8589
		void MakeVoice();

		// Token: 0x0600218E RID: 8590
		void MakeVoiceDelayed();

		// Token: 0x0600218F RID: 8591
		void SetFacialAnimation(string faceAnimation, bool loop);

		// Token: 0x06002190 RID: 8592
		void Done();

		// Token: 0x06002191 RID: 8593
		void Cancel();

		// Token: 0x06002192 RID: 8594
		void UndressCharacterEntity();

		// Token: 0x06002193 RID: 8595
		void DressCharacterEntity();

		// Token: 0x06002194 RID: 8596
		void DefaultFace();
	}
}
