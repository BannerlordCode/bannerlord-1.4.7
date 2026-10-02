using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TaleWorlds.Library;
using TaleWorlds.Localization.TextProcessor;
using TaleWorlds.Localization.TextProcessor.LanguageProcessors;

namespace TaleWorlds.Localization
{
	// Token: 0x02000009 RID: 9
	public static class MBTextManager
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003B6A File Offset: 0x00001D6A
		public static string ActiveTextLanguage
		{
			get
			{
				return MBTextManager._activeTextLanguageId;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003B71 File Offset: 0x00001D71
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003B78 File Offset: 0x00001D78
		public static bool LocalizationDebugMode { get; set; }

		// Token: 0x06000072 RID: 114 RVA: 0x00003B80 File Offset: 0x00001D80
		public static bool LanguageExistsInCurrentConfiguration(string language, bool developmentMode)
		{
			return LocalizedTextManager.GetLanguageIds(developmentMode).Any<string>((string l) => l == language);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003BB4 File Offset: 0x00001DB4
		public static bool ChangeLanguage(string language)
		{
			if (LocalizedTextManager.GetLanguageIds(true).Any<string>((string l) => l == language))
			{
				MBTextManager._languageProcessor = LocalizedTextManager.CreateTextProcessorForLanguage(language);
				MBTextManager._activeTextLanguageId = language;
				MBTextManager._activeTextLanguageIndex = LocalizedTextManager.GetLanguageIndex(MBTextManager._activeTextLanguageId);
				LocalizedTextManager.LoadLanguage(MBTextManager._activeTextLanguageId);
				return true;
			}
			Debug.FailedAssert("Invalid language", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\MBTextManager.cs", "ChangeLanguage", 141);
			return false;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003C37 File Offset: 0x00001E37
		public static int GetActiveTextLanguageIndex()
		{
			return MBTextManager._activeTextLanguageIndex;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003C40 File Offset: 0x00001E40
		public static bool TryChangeVoiceLanguage(string language)
		{
			if (LocalizedVoiceManager.GetVoiceLanguageIds().Any<string>((string l) => l == language))
			{
				MBTextManager._activeVoiceLanguageId = language;
				LocalizedVoiceManager.LoadLanguage(MBTextManager._activeVoiceLanguageId);
				return true;
			}
			return false;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003C8A File Offset: 0x00001E8A
		private static TextObject ProcessNumber(object integer)
		{
			return new TextObject(integer.ToString(), null);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003C98 File Offset: 0x00001E98
		internal static string ProcessTextToString(TextObject to, bool shouldClear)
		{
			if (to == null)
			{
				return null;
			}
			if (TextObject.IsNullOrEmpty(to))
			{
				return "";
			}
			string localizedText = MBTextManager.GetLocalizedText(to.Value);
			string text;
			if (!string.IsNullOrEmpty(to.Value))
			{
				text = MBTextManager.Process(localizedText, to);
				text = MBTextManager._languageProcessor.Process(text);
				if (shouldClear)
				{
					MBTextManager._languageProcessor.ClearTemporaryData();
				}
			}
			else
			{
				text = "";
			}
			if (MBTextManager.LocalizationDebugMode)
			{
				string text2 = to.GetID();
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "!";
				}
				return "(" + text2 + ") " + text;
			}
			return text;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003D30 File Offset: 0x00001F30
		internal static string ProcessWithoutLanguageProcessor(TextObject to)
		{
			if (to == null)
			{
				return null;
			}
			if (TextObject.IsNullOrEmpty(to))
			{
				return "";
			}
			string localizedText = MBTextManager.GetLocalizedText(to.Value);
			string text;
			if (!string.IsNullOrEmpty(to.Value))
			{
				text = MBTextManager.Process(localizedText, to);
			}
			else
			{
				text = "";
			}
			return text;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003D80 File Offset: 0x00001F80
		private static string Process(string query, TextObject parent = null)
		{
			List<MBTextToken> list = null;
			if (parent != null)
			{
				list = parent.GetCachedTokens();
			}
			if (list == null)
			{
				list = MBTextManager.Tokenizer.Tokenize(query);
			}
			return TextGrammarProcessor.Process(MBTextParser.Parse(list), MBTextManager.TextContext, parent);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003DBF File Offset: 0x00001FBF
		public static void ClearAll()
		{
			MBTextManager.TextContext.ClearAll();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003DCB File Offset: 0x00001FCB
		public static void SetTextVariable(string variableName, string text, bool sendClients = false)
		{
			if (text == null)
			{
				return;
			}
			MBTextManager.TextContext.SetTextVariable(variableName, new TextObject(text, null));
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003DE3 File Offset: 0x00001FE3
		public static void SetTextVariable(string variableName, TextObject text, bool sendClients = false)
		{
			if (text == null)
			{
				return;
			}
			MBTextManager.TextContext.SetTextVariable(variableName, text);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003DFC File Offset: 0x00001FFC
		public static void SetTextVariable(string variableName, int content)
		{
			TextObject textObject = MBTextManager.ProcessNumber(content);
			MBTextManager.SetTextVariable(variableName, textObject, false);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003E20 File Offset: 0x00002020
		public static void SetTextVariable(string variableName, float content, int decimalDigits = 2)
		{
			TextObject textObject = MBTextManager.ProcessNumber(MathF.Round(content, decimalDigits));
			MBTextManager.SetTextVariable(variableName, textObject, false);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003E48 File Offset: 0x00002048
		public static void SetTextVariable(string variableName, object content)
		{
			if (content == null)
			{
				return;
			}
			TextObject textObject = new TextObject(content.ToString(), null);
			MBTextManager.SetTextVariable(variableName, textObject, false);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003E70 File Offset: 0x00002070
		public static void SetTextVariable(string variableName, int arrayIndex, object content)
		{
			if (content == null)
			{
				return;
			}
			string text = content.ToString();
			MBTextManager.SetTextVariable(variableName + ":" + arrayIndex, text, false);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003EA0 File Offset: 0x000020A0
		public static void SetFunction(string funcName, string functionBody)
		{
			MBTextModel mbtextModel = MBTextParser.Parse(MBTextManager.Tokenizer.Tokenize(functionBody));
			MBTextManager.TextContext.SetFunction(funcName, mbtextModel);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003ECA File Offset: 0x000020CA
		public static void ResetFunctions()
		{
			MBTextManager.TextContext.ResetFunctions();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003ED6 File Offset: 0x000020D6
		public static void ThrowLocalizationError(string message)
		{
			Debug.FailedAssert(message, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Localization\\MBTextManager.cs", "ThrowLocalizationError", 342);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003EF0 File Offset: 0x000020F0
		internal static string GetLocalizedText(string text)
		{
			if (text != null && text.Length > 2 && text[0] == '{' && text[1] == '=')
			{
				if (MBTextManager._idStringBuilder == null)
				{
					MBTextManager._idStringBuilder = new StringBuilder(8);
				}
				else
				{
					MBTextManager._idStringBuilder.Clear();
				}
				if (MBTextManager._targetStringBuilder == null)
				{
					MBTextManager._targetStringBuilder = new StringBuilder(100);
				}
				else
				{
					MBTextManager._targetStringBuilder.Clear();
				}
				int i = 2;
				while (i < text.Length)
				{
					if (text[i] != '}')
					{
						MBTextManager._idStringBuilder.Append(text[i]);
						i++;
					}
					else
					{
						for (i++; i < text.Length; i++)
						{
							MBTextManager._targetStringBuilder.Append(text[i]);
						}
						string text2 = "";
						if (MBTextManager._activeTextLanguageId == "English")
						{
							text2 = MBTextManager._targetStringBuilder.ToString();
							return MBTextManager.RemoveComments(text2);
						}
						if ((MBTextManager._idStringBuilder.Length != 1 || MBTextManager._idStringBuilder[0] != '*') && (MBTextManager._idStringBuilder.Length != 1 || MBTextManager._idStringBuilder[0] != '!'))
						{
							if (MBTextManager._activeTextLanguageId != "English")
							{
								text2 = LocalizedTextManager.GetTranslatedText(MBTextManager._activeTextLanguageId, MBTextManager._idStringBuilder.ToString());
							}
							if (text2 != null)
							{
								return MBTextManager.RemoveComments(text2);
							}
						}
						IL_0164:
						return MBTextManager._targetStringBuilder.ToString();
					}
				}
				goto IL_0164;
			}
			return text;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000406C File Offset: 0x0000226C
		private static string RemoveComments(string localizedText)
		{
			foreach (object obj in MBTextManager.CommentRemoverRegex.Matches(localizedText))
			{
				Match match = (Match)obj;
				localizedText = localizedText.Replace(match.Value, "");
			}
			return localizedText;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000040D8 File Offset: 0x000022D8
		public static string DiscardAnimationTagsAndCheckAnimationTagPositions(string text)
		{
			return MBTextManager.DiscardAnimationTags(text);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000040E0 File Offset: 0x000022E0
		public static string DiscardAnimationTags(string text)
		{
			string text2 = "";
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '[')
				{
					flag = true;
				}
				if (!flag)
				{
					text2 += text[i].ToString();
				}
				if (text[i] == ']')
				{
					flag = false;
				}
			}
			return text2;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000413C File Offset: 0x0000233C
		private static bool CheckAnimationTagPositions(string text)
		{
			string text2 = "";
			Match match = MBTextManager.AnimationTagRemoverRegex.Match(text);
			if (match.Success)
			{
				text2 = MBTextManager.DiscardAnimationTags(match.Value);
			}
			return string.IsNullOrEmpty(text2.Replace(" ", ""));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004184 File Offset: 0x00002384
		public static string[] GetConversationAnimations(TextObject to)
		{
			string text = to.CopyTextObject().ToString();
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = new string[4];
			bool flag = false;
			int num = 0;
			if (!string.IsNullOrEmpty(text))
			{
				for (int i = 0; i < text.Length; i++)
				{
					if (text[i] == '[')
					{
						flag = true;
					}
					else if (flag)
					{
						if (text[i] == ',' || text[i] == ']')
						{
							to.Value.Contains("{=!}");
							array[num] = stringBuilder.ToString();
							stringBuilder.Clear();
							if (text[i] == ']')
							{
								flag = false;
							}
						}
						else if (text[i] == ':')
						{
							string text2 = stringBuilder.ToString();
							stringBuilder.Clear();
							if (text2 == "ib")
							{
								num = 0;
							}
							else if (text2 == "if")
							{
								num = 1;
							}
							else if (text2 == "rb")
							{
								num = 2;
							}
							else if (text2 == "rf")
							{
								num = 3;
							}
						}
						else if (text[i] != ' ')
						{
							stringBuilder.Append(text[i]);
						}
					}
				}
			}
			return array;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000042C1 File Offset: 0x000024C1
		public static bool TryGetVoiceObject(TextObject to, out VoiceObject vo, out string vocalizationId)
		{
			if (!TextObject.IsNullOrEmpty(to))
			{
				vo = MBTextManager.ProcessTextForVocalization(to, out vocalizationId);
				return true;
			}
			vo = null;
			vocalizationId = null;
			return false;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000042E0 File Offset: 0x000024E0
		private static VoiceObject ProcessTextForVocalization(TextObject to, out string vocalizationId)
		{
			vocalizationId = null;
			if (TextObject.IsNullOrEmpty(to))
			{
				return null;
			}
			string localizationId = MBTextManager.GetLocalizationId(to);
			if (localizationId != "!")
			{
				vocalizationId = localizationId;
				return LocalizedVoiceManager.GetLocalizedVoice(localizationId);
			}
			List<MBTextToken> list = to.GetCachedTokens();
			if (list == null)
			{
				list = MBTextManager.Tokenizer.Tokenize(to.Value);
			}
			foreach (MBTextToken mbtextToken in list)
			{
				if (mbtextToken.TokenType == TokenType.Identifier)
				{
					VoiceObject voiceObject = MBTextManager.ProcessTextForVocalization(MBTextManager.TextContext.GetRawTextVariable(mbtextToken.Value, to), out vocalizationId);
					if (voiceObject != null)
					{
						return voiceObject;
					}
				}
			}
			return null;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000043A0 File Offset: 0x000025A0
		private static string GetLocalizationId(TextObject to)
		{
			if (TextObject.IsNullOrEmpty(to))
			{
				return string.Empty;
			}
			string value = to.Value;
			if (value != null && value.Length > 2 && value[0] == '{' && value[1] == '=')
			{
				int num = 2;
				for (int i = num; i < value.Length; i++)
				{
					if (value[i] == '}')
					{
						IL_005D:
						return value.Substring(num, i - num);
					}
				}
				goto IL_005D;
			}
			return string.Empty;
		}

		// Token: 0x04000017 RID: 23
		public const string LinkAttribute = "LINK";

		// Token: 0x04000018 RID: 24
		internal const string LinkTag = ".link";

		// Token: 0x04000019 RID: 25
		internal const int LinkTagLength = 7;

		// Token: 0x0400001A RID: 26
		internal const string LinkEnding = "</b></a>";

		// Token: 0x0400001B RID: 27
		internal const int LinkEndingLength = 8;

		// Token: 0x0400001C RID: 28
		internal const string LinkStarter = "<a style=\"Link.";

		// Token: 0x0400001D RID: 29
		private const string CommentRegexPattern = "{%.+?}";

		// Token: 0x0400001E RID: 30
		private const string AnimationTagsRegexPattern = "\\[.+\\]";

		// Token: 0x0400001F RID: 31
		private static readonly TextProcessingContext TextContext = new TextProcessingContext();

		// Token: 0x04000020 RID: 32
		private static LanguageSpecificTextProcessor _languageProcessor = new EnglishTextProcessor();

		// Token: 0x04000021 RID: 33
		private static string _activeVoiceLanguageId = "English";

		// Token: 0x04000022 RID: 34
		private static string _activeTextLanguageId = "English";

		// Token: 0x04000023 RID: 35
		private static int _activeTextLanguageIndex = 0;

		// Token: 0x04000025 RID: 37
		[ThreadStatic]
		private static StringBuilder _idStringBuilder;

		// Token: 0x04000026 RID: 38
		[ThreadStatic]
		private static StringBuilder _targetStringBuilder;

		// Token: 0x04000027 RID: 39
		private static readonly Regex CommentRemoverRegex = new Regex("{%.+?}");

		// Token: 0x04000028 RID: 40
		private static readonly Regex AnimationTagRemoverRegex = new Regex("\\[.+\\]");

		// Token: 0x04000029 RID: 41
		internal static readonly Tokenizer Tokenizer = new Tokenizer();
	}
}
