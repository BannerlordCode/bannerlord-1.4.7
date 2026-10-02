using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200000C RID: 12
	internal class ScriptingInterfaceOfIMBAnimation : IMBAnimation
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000ACAE File Offset: 0x00008EAE
		public int AnimationIndexOfActionCode(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_AnimationIndexOfActionCodeDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000ACBC File Offset: 0x00008EBC
		public bool CheckAnimationClipExists(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_CheckAnimationClipExistsDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000ACCA File Offset: 0x00008ECA
		public float GetActionAnimationDuration(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetActionAnimationDurationDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000ACD8 File Offset: 0x00008ED8
		public float GetActionBlendOutStartProgress(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetActionBlendOutStartProgressDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		public int GetActionCodeWithName(string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIMBAnimation._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBAnimation._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBAnimation.call_GetActionCodeWithNameDelegate(array);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000AD42 File Offset: 0x00008F42
		public string GetActionNameWithCode(int index)
		{
			if (ScriptingInterfaceOfIMBAnimation.call_GetActionNameWithCodeDelegate(index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000AD59 File Offset: 0x00008F59
		public Agent.ActionCodeType GetActionType(int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetActionTypeDelegate(actionIndex);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000AD66 File Offset: 0x00008F66
		public float GetAnimationBlendInPeriod(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationBlendInPeriodDelegate(animationIndex);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000AD73 File Offset: 0x00008F73
		public int GetAnimationBlendsWithActionIndex(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationBlendsWithActionIndexDelegate(animationIndex);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000AD80 File Offset: 0x00008F80
		public int GetAnimationContinueToAction(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationContinueToActionDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000AD8E File Offset: 0x00008F8E
		public Vec3 GetAnimationDisplacementAtProgress(int animationIndex, float progress)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationDisplacementAtProgressDelegate(animationIndex, progress);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000AD9C File Offset: 0x00008F9C
		public float GetAnimationDuration(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationDurationDelegate(animationIndex);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000ADA9 File Offset: 0x00008FA9
		public AnimFlags GetAnimationFlags(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationFlagsDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000ADB7 File Offset: 0x00008FB7
		public string GetAnimationName(int actionSetNo, int actionIndex)
		{
			if (ScriptingInterfaceOfIMBAnimation.call_GetAnimationNameDelegate(actionSetNo, actionIndex) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000ADCF File Offset: 0x00008FCF
		public float GetAnimationParameter1(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter1Delegate(animationIndex);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000ADDC File Offset: 0x00008FDC
		public float GetAnimationParameter2(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter2Delegate(animationIndex);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000ADE9 File Offset: 0x00008FE9
		public float GetAnimationParameter3(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter3Delegate(animationIndex);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000ADF6 File Offset: 0x00008FF6
		public Vec3 GetDisplacementVector(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetDisplacementVectorDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000AE04 File Offset: 0x00009004
		public string GetIDWithIndex(int index)
		{
			if (ScriptingInterfaceOfIMBAnimation.call_GetIDWithIndexDelegate(index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000AE1C File Offset: 0x0000901C
		public int GetIndexWithID(string id)
		{
			byte[] array = null;
			if (id != null)
			{
				int byteCount = ScriptingInterfaceOfIMBAnimation._utf8.GetByteCount(id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBAnimation._utf8.GetBytes(id, 0, id.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBAnimation.call_GetIndexWithIDDelegate(array);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000AE76 File Offset: 0x00009076
		public int GetNumActionCodes()
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetNumActionCodesDelegate();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000AE82 File Offset: 0x00009082
		public int GetNumAnimations()
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetNumAnimationsDelegate();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000AE8E File Offset: 0x0000908E
		public bool IsAnyAnimationLoadingFromDisk()
		{
			return ScriptingInterfaceOfIMBAnimation.call_IsAnyAnimationLoadingFromDiskDelegate();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000AE9A File Offset: 0x0000909A
		public void PrefetchAnimationClip(int actionSetNo, int actionIndex)
		{
			ScriptingInterfaceOfIMBAnimation.call_PrefetchAnimationClipDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x0400016D RID: 365
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400016E RID: 366
		public static ScriptingInterfaceOfIMBAnimation.AnimationIndexOfActionCodeDelegate call_AnimationIndexOfActionCodeDelegate;

		// Token: 0x0400016F RID: 367
		public static ScriptingInterfaceOfIMBAnimation.CheckAnimationClipExistsDelegate call_CheckAnimationClipExistsDelegate;

		// Token: 0x04000170 RID: 368
		public static ScriptingInterfaceOfIMBAnimation.GetActionAnimationDurationDelegate call_GetActionAnimationDurationDelegate;

		// Token: 0x04000171 RID: 369
		public static ScriptingInterfaceOfIMBAnimation.GetActionBlendOutStartProgressDelegate call_GetActionBlendOutStartProgressDelegate;

		// Token: 0x04000172 RID: 370
		public static ScriptingInterfaceOfIMBAnimation.GetActionCodeWithNameDelegate call_GetActionCodeWithNameDelegate;

		// Token: 0x04000173 RID: 371
		public static ScriptingInterfaceOfIMBAnimation.GetActionNameWithCodeDelegate call_GetActionNameWithCodeDelegate;

		// Token: 0x04000174 RID: 372
		public static ScriptingInterfaceOfIMBAnimation.GetActionTypeDelegate call_GetActionTypeDelegate;

		// Token: 0x04000175 RID: 373
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationBlendInPeriodDelegate call_GetAnimationBlendInPeriodDelegate;

		// Token: 0x04000176 RID: 374
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationBlendsWithActionIndexDelegate call_GetAnimationBlendsWithActionIndexDelegate;

		// Token: 0x04000177 RID: 375
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationContinueToActionDelegate call_GetAnimationContinueToActionDelegate;

		// Token: 0x04000178 RID: 376
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationDisplacementAtProgressDelegate call_GetAnimationDisplacementAtProgressDelegate;

		// Token: 0x04000179 RID: 377
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationDurationDelegate call_GetAnimationDurationDelegate;

		// Token: 0x0400017A RID: 378
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationFlagsDelegate call_GetAnimationFlagsDelegate;

		// Token: 0x0400017B RID: 379
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationNameDelegate call_GetAnimationNameDelegate;

		// Token: 0x0400017C RID: 380
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationParameter1Delegate call_GetAnimationParameter1Delegate;

		// Token: 0x0400017D RID: 381
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationParameter2Delegate call_GetAnimationParameter2Delegate;

		// Token: 0x0400017E RID: 382
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationParameter3Delegate call_GetAnimationParameter3Delegate;

		// Token: 0x0400017F RID: 383
		public static ScriptingInterfaceOfIMBAnimation.GetDisplacementVectorDelegate call_GetDisplacementVectorDelegate;

		// Token: 0x04000180 RID: 384
		public static ScriptingInterfaceOfIMBAnimation.GetIDWithIndexDelegate call_GetIDWithIndexDelegate;

		// Token: 0x04000181 RID: 385
		public static ScriptingInterfaceOfIMBAnimation.GetIndexWithIDDelegate call_GetIndexWithIDDelegate;

		// Token: 0x04000182 RID: 386
		public static ScriptingInterfaceOfIMBAnimation.GetNumActionCodesDelegate call_GetNumActionCodesDelegate;

		// Token: 0x04000183 RID: 387
		public static ScriptingInterfaceOfIMBAnimation.GetNumAnimationsDelegate call_GetNumAnimationsDelegate;

		// Token: 0x04000184 RID: 388
		public static ScriptingInterfaceOfIMBAnimation.IsAnyAnimationLoadingFromDiskDelegate call_IsAnyAnimationLoadingFromDiskDelegate;

		// Token: 0x04000185 RID: 389
		public static ScriptingInterfaceOfIMBAnimation.PrefetchAnimationClipDelegate call_PrefetchAnimationClipDelegate;

		// Token: 0x020001DB RID: 475
		// (Invoke) Token: 0x06000A5A RID: 2650
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AnimationIndexOfActionCodeDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DC RID: 476
		// (Invoke) Token: 0x06000A5E RID: 2654
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckAnimationClipExistsDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DD RID: 477
		// (Invoke) Token: 0x06000A62 RID: 2658
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetActionAnimationDurationDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DE RID: 478
		// (Invoke) Token: 0x06000A66 RID: 2662
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetActionBlendOutStartProgressDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DF RID: 479
		// (Invoke) Token: 0x06000A6A RID: 2666
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetActionCodeWithNameDelegate(byte[] name);

		// Token: 0x020001E0 RID: 480
		// (Invoke) Token: 0x06000A6E RID: 2670
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetActionNameWithCodeDelegate(int index);

		// Token: 0x020001E1 RID: 481
		// (Invoke) Token: 0x06000A72 RID: 2674
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Agent.ActionCodeType GetActionTypeDelegate(int actionIndex);

		// Token: 0x020001E2 RID: 482
		// (Invoke) Token: 0x06000A76 RID: 2678
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationBlendInPeriodDelegate(int animationIndex);

		// Token: 0x020001E3 RID: 483
		// (Invoke) Token: 0x06000A7A RID: 2682
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationBlendsWithActionIndexDelegate(int animationIndex);

		// Token: 0x020001E4 RID: 484
		// (Invoke) Token: 0x06000A7E RID: 2686
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationContinueToActionDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001E5 RID: 485
		// (Invoke) Token: 0x06000A82 RID: 2690
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetAnimationDisplacementAtProgressDelegate(int animationIndex, float progress);

		// Token: 0x020001E6 RID: 486
		// (Invoke) Token: 0x06000A86 RID: 2694
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationDurationDelegate(int animationIndex);

		// Token: 0x020001E7 RID: 487
		// (Invoke) Token: 0x06000A8A RID: 2698
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate AnimFlags GetAnimationFlagsDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001E8 RID: 488
		// (Invoke) Token: 0x06000A8E RID: 2702
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationNameDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001E9 RID: 489
		// (Invoke) Token: 0x06000A92 RID: 2706
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationParameter1Delegate(int animationIndex);

		// Token: 0x020001EA RID: 490
		// (Invoke) Token: 0x06000A96 RID: 2710
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationParameter2Delegate(int animationIndex);

		// Token: 0x020001EB RID: 491
		// (Invoke) Token: 0x06000A9A RID: 2714
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationParameter3Delegate(int animationIndex);

		// Token: 0x020001EC RID: 492
		// (Invoke) Token: 0x06000A9E RID: 2718
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetDisplacementVectorDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001ED RID: 493
		// (Invoke) Token: 0x06000AA2 RID: 2722
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetIDWithIndexDelegate(int index);

		// Token: 0x020001EE RID: 494
		// (Invoke) Token: 0x06000AA6 RID: 2726
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetIndexWithIDDelegate(byte[] id);

		// Token: 0x020001EF RID: 495
		// (Invoke) Token: 0x06000AAA RID: 2730
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumActionCodesDelegate();

		// Token: 0x020001F0 RID: 496
		// (Invoke) Token: 0x06000AAE RID: 2734
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumAnimationsDelegate();

		// Token: 0x020001F1 RID: 497
		// (Invoke) Token: 0x06000AB2 RID: 2738
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsAnyAnimationLoadingFromDiskDelegate();

		// Token: 0x020001F2 RID: 498
		// (Invoke) Token: 0x06000AB6 RID: 2742
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PrefetchAnimationClipDelegate(int actionSetNo, int actionIndex);
	}
}
