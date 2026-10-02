using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TaleWorlds.Library;
using TaleWorlds.Localization.Expressions;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x0200002C RID: 44
	public class TextProcessingContext
	{
		// Token: 0x0600012F RID: 303 RVA: 0x000065C7 File Offset: 0x000047C7
		internal void SetTextVariable(string variableName, TextObject data)
		{
			this._variables[variableName] = data;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000065D8 File Offset: 0x000047D8
		internal TextObject GetRawTextVariable(string variableName, TextObject parent)
		{
			TextObject textObject = null;
			if (parent != null && parent.GetVariableValue(variableName, out textObject))
			{
				return textObject;
			}
			if (!this._variables.ContainsKey(variableName))
			{
				return TextObject.GetEmpty();
			}
			return this._variables[variableName];
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006620 File Offset: 0x00004820
		internal MultiStatement GetVariableValue(string variableName, TextObject parent)
		{
			TextObject textObject = null;
			MBTextModel mbtextModel = null;
			if (!(parent != null) || !parent.GetVariableValue(variableName, out textObject))
			{
				this._variables.TryGetValue(variableName, out textObject);
			}
			if (textObject != null)
			{
				mbtextModel = MBTextParser.Parse(MBTextManager.Tokenizer.Tokenize(textObject.ToStringWithoutClear()));
			}
			if (mbtextModel == null)
			{
				return null;
			}
			if (mbtextModel.RootExpressions.Count == 1 && mbtextModel.RootExpressions[0] is MultiStatement)
			{
				return new MultiStatement((mbtextModel.RootExpressions[0] as MultiStatement).SubStatements);
			}
			return new MultiStatement(mbtextModel.RootExpressions);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000066C0 File Offset: 0x000048C0
		internal ValueTuple<TextObject, bool> GetVariableValueAsTextObject(string variableName, TextObject parent)
		{
			TextObject textObject = null;
			if (!TextObject.IsNullOrEmpty(parent))
			{
				if (parent.GetVariableValue(variableName, out textObject))
				{
					return new ValueTuple<TextObject, bool>(textObject, true);
				}
				textObject = TextProcessingContext.FindNestedFieldValue(MBTextManager.GetLocalizedText(parent.Value), variableName, parent);
				if (textObject != null && textObject.Length > 0)
				{
					return new ValueTuple<TextObject, bool>(textObject, true);
				}
			}
			if (!(textObject == null) && textObject.Length != 0)
			{
				return new ValueTuple<TextObject, bool>(textObject, true);
			}
			return new ValueTuple<TextObject, bool>(new TextObject("{=!}ERROR: " + variableName + " variable has not been set before.", null), false);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006748 File Offset: 0x00004948
		internal MultiStatement GetArrayAccess(string variableName, int index)
		{
			string text = variableName + ":" + index;
			TextObject textObject;
			if (this._variables.TryGetValue(text, out textObject))
			{
				return new MultiStatement(MBTextParser.Parse(MBTextManager.Tokenizer.Tokenize(textObject.ToStringWithoutClear())).RootExpressions);
			}
			return null;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006798 File Offset: 0x00004998
		private int CountMarkerOccurancesInString(string searchedIdentifier, TextObject parent)
		{
			Regex regex = new Regex("{." + searchedIdentifier + "}");
			TextObject textObject = parent;
			if (parent.IsLink)
			{
				string key = parent.Attributes.First<KeyValuePair<string, object>>((KeyValuePair<string, object> x) => !x.Key.Equals("LINK")).Key;
				parent.GetVariableValue(key, out textObject);
			}
			string text = MBTextManager.ProcessWithoutLanguageProcessor(textObject);
			if (regex.IsMatch(text))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006814 File Offset: 0x00004A14
		internal string GetParameterWithMarkerOccurance(string token, TextObject parent)
		{
			int num = token.IndexOf('!');
			if (num == -1)
			{
				return "";
			}
			string text = token.Substring(0, num);
			string text2 = token.Substring(num + 2, token.Length - num - 2);
			TextObject functionParamWithoutEvaluate = this.GetFunctionParamWithoutEvaluate(text);
			TextObject textObject;
			if (((((parent != null) ? parent.Attributes : null) != null && parent.TryGetAttributesValue(functionParamWithoutEvaluate.ToStringWithoutClear(), out textObject)) || this._variables.TryGetValue(functionParamWithoutEvaluate.ToStringWithoutClear(), out textObject)) && textObject.Length > 0)
			{
				return this.CountMarkerOccurancesInString(text2, textObject).ToString();
			}
			return "";
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000068B0 File Offset: 0x00004AB0
		internal string GetParameterWithMarkerOccurances(string token, TextObject parent)
		{
			int num = token.IndexOf('!');
			if (num == -1)
			{
				return "";
			}
			string text = token.Substring(0, num);
			int num2 = token.IndexOf('[') + 1;
			int num3 = token.IndexOf(']');
			string[] array = token.Substring(num2, num3 - num2).Split(new char[] { ',' });
			TextObject functionParamWithoutEvaluate = this.GetFunctionParamWithoutEvaluate(text);
			TextObject textObject;
			if (((((parent != null) ? parent.Attributes : null) != null && parent.TryGetAttributesValue(functionParamWithoutEvaluate.ToStringWithoutClear(), out textObject)) || this._variables.TryGetValue(functionParamWithoutEvaluate.ToStringWithoutClear(), out textObject)) && textObject.Length > 0)
			{
				foreach (string text2 in array)
				{
					int num4 = this.CountMarkerOccurancesInString(text2, textObject);
					if (num4 > 0)
					{
						return num4.ToString();
					}
				}
				return "0";
			}
			return "";
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006993 File Offset: 0x00004B93
		internal static bool IsDeclaration(string token)
		{
			return token.Length > 1 && token[0] == '@';
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000069AB File Offset: 0x00004BAB
		internal static bool IsLinkToken(string token)
		{
			return token == ".link" || token == "LINK";
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000069C7 File Offset: 0x00004BC7
		internal static bool IsDeclarationFinalizer(string token)
		{
			return token.Length == 2 && (token[0] == '\\' || token[0] == '/') && token[1] == '@';
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000069F8 File Offset: 0x00004BF8
		private static TextObject FindNestedFieldValue(string text, string identifier, TextObject parent)
		{
			string[] array = identifier.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			return new TextObject(TextProcessingContext.GetFieldValue(text, array, parent), null);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006A28 File Offset: 0x00004C28
		[return: TupleElementNames(new string[] { "value", "doesValueExist" })]
		internal ValueTuple<TextObject, bool> GetQualifiedVariableValue(string token, TextObject parent)
		{
			int num = token.IndexOf('.');
			if (num == -1)
			{
				return this.GetVariableValueAsTextObject(token, parent);
			}
			string text = token.Substring(0, num);
			string text2 = token.Substring(num + 1, token.Length - (num + 1));
			TextObject textObject;
			if (((parent != null) ? parent.Attributes : null) != null && parent.TryGetAttributesValue(text, out textObject))
			{
				ValueTuple<TextObject, bool> qualifiedVariableValue = this.GetQualifiedVariableValue(text2, textObject);
				TextObject item = qualifiedVariableValue.Item1;
				bool item2 = qualifiedVariableValue.Item2;
				if (!item.IsEmpty())
				{
					return new ValueTuple<TextObject, bool>(item, item2);
				}
			}
			else
			{
				TextObject textObject2;
				if (this._variables.TryGetValue(text, out textObject2) && textObject2.Length > 0)
				{
					return new ValueTuple<TextObject, bool>(TextProcessingContext.FindNestedFieldValue(MBTextManager.GetLocalizedText(textObject2.Value), text2, textObject2), true);
				}
				foreach (KeyValuePair<string, TextObject> keyValuePair in this._variables)
				{
					if (keyValuePair.Key == text && keyValuePair.Value.Attributes != null)
					{
						foreach (KeyValuePair<string, object> keyValuePair2 in keyValuePair.Value.Attributes)
						{
							if (keyValuePair2.Key == text2)
							{
								return new ValueTuple<TextObject, bool>(TextObject.TryGetOrCreateFromObject(keyValuePair2.Value), true);
							}
						}
					}
				}
			}
			return new ValueTuple<TextObject, bool>(TextObject.GetEmpty(), false);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006BC0 File Offset: 0x00004DC0
		private static string GetFieldValue(string text, string[] fieldNames, TextObject parent)
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "GetFieldValue");
			bool flag = false;
			while (i < text.Length)
			{
				if (text[i] != '{')
				{
					if (num == fieldNames.Length && num2 == num)
					{
						mbstringBuilder.Append(text[i]);
					}
				}
				else
				{
					string text2 = TextProcessingContext.ReadFirstToken(text, ref i);
					if (TextProcessingContext.IsLinkToken(text2))
					{
						flag = true;
					}
					else if (TextProcessingContext.IsDeclarationFinalizer(text2))
					{
						num--;
						if (num2 > num)
						{
							num2 = num;
						}
					}
					else if (TextProcessingContext.IsDeclaration(text2))
					{
						string text3 = text2.Substring(1);
						bool flag2 = num2 == num && num < fieldNames.Length && string.Compare(fieldNames[num], text3, StringComparison.InvariantCultureIgnoreCase) == 0;
						num++;
						if (flag2)
						{
							num2 = num;
						}
					}
					else if (flag)
					{
						TextObject textObject;
						if (parent.Attributes != null && parent.TryGetAttributesValue(text2, out textObject))
						{
							return TextProcessingContext.GetFieldValuesFromLinks(fieldNames, textObject, ref mbstringBuilder);
						}
					}
					else if (num == fieldNames.Length && num2 == num)
					{
						mbstringBuilder.Append<string>("{" + text2 + "}");
					}
				}
				i++;
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006CE8 File Offset: 0x00004EE8
		private static string GetFieldValuesFromLinks(string[] fieldNames, TextObject value, ref MBStringBuilder targetString)
		{
			TextObject textObject;
			if (fieldNames.Length == 1 && value.TryGetAttributesValue(fieldNames[0], out textObject))
			{
				targetString.Append<TextObject>(textObject);
				return targetString.ToStringAndRelease();
			}
			targetString.Append<string>(TextProcessingContext.GetFieldValue(MBTextManager.GetLocalizedText(value.Value), fieldNames, null));
			return targetString.ToStringAndRelease();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006D38 File Offset: 0x00004F38
		internal static string ReadFirstToken(string text, ref int i)
		{
			int num = i;
			while (i < text.Length && text[i] != '}')
			{
				i++;
			}
			int num2 = i - num;
			return text.Substring(num + 1, num2 - 1);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006D78 File Offset: 0x00004F78
		internal TextObject CallFunction(string functionName, List<TextExpression> functionParams, TextObject parent)
		{
			TextObject[] array = new TextObject[functionParams.Count];
			TextObject[] array2 = new TextObject[functionParams.Count];
			for (int i = 0; i < functionParams.Count; i++)
			{
				array[i] = new TextObject(functionParams[i].EvaluateString(this, parent), null);
				array2[i] = new TextObject(functionParams[i].RawValue, null);
			}
			this._curParams.Push(array);
			this._curParamsWithoutEvaluate.Push(array2);
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			MBTextModel functionBody = this.GetFunctionBody(functionName);
			mbstringBuilder.Initialize(16, "CallFunction");
			if (functionBody != null)
			{
				using (List<TextExpression>.Enumerator enumerator = functionBody.RootExpressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TextExpression textExpression = enumerator.Current;
						mbstringBuilder.Append<string>(textExpression.EvaluateString(this, parent));
					}
					goto IL_00E7;
				}
			}
			if (array.Length != 0)
			{
				mbstringBuilder.Append<TextObject>(array[0]);
			}
			IL_00E7:
			string text = mbstringBuilder.ToStringAndRelease();
			this._curParams.Pop();
			this._curParamsWithoutEvaluate.Pop();
			return new TextObject(text, null);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006EA4 File Offset: 0x000050A4
		public void SetFunction(string functionName, MBTextModel functionBody)
		{
			this._functions[functionName] = functionBody;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006EB3 File Offset: 0x000050B3
		public void ResetFunctions()
		{
			this._functions.Clear();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006EC0 File Offset: 0x000050C0
		public MBTextModel GetFunctionBody(string functionName)
		{
			MBTextModel mbtextModel;
			this._functions.TryGetValue(functionName, out mbtextModel);
			return mbtextModel;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006EE0 File Offset: 0x000050E0
		public TextObject GetFunctionParam(string rawValue)
		{
			int num;
			if (!int.TryParse(rawValue.Substring(1), out num))
			{
				return TextObject.GetEmpty();
			}
			if (this._curParams.Count > 0 && this._curParams.Peek().Length > num)
			{
				return this._curParams.Peek()[num];
			}
			return new TextObject("Can't find parameter:" + rawValue, null);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006F40 File Offset: 0x00005140
		public TextObject GetFunctionParamWithoutEvaluate(string rawValue)
		{
			int num;
			if (!int.TryParse(rawValue.Substring(1), out num))
			{
				return TextObject.GetEmpty();
			}
			if (this._curParamsWithoutEvaluate.Count > 0 && this._curParamsWithoutEvaluate.Peek().Length > num)
			{
				return this._curParamsWithoutEvaluate.Peek()[num];
			}
			return new TextObject("Can't find parameter:" + rawValue, null);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006FA0 File Offset: 0x000051A0
		internal void ClearAll()
		{
			this._variables.Clear();
		}

		// Token: 0x04000066 RID: 102
		private readonly Dictionary<string, TextObject> _variables = new Dictionary<string, TextObject>(new CaseInsensitiveComparer());

		// Token: 0x04000067 RID: 103
		private readonly Dictionary<string, MBTextModel> _functions = new Dictionary<string, MBTextModel>();

		// Token: 0x04000068 RID: 104
		private readonly Stack<TextObject[]> _curParams = new Stack<TextObject[]>();

		// Token: 0x04000069 RID: 105
		private readonly Stack<TextObject[]> _curParamsWithoutEvaluate = new Stack<TextObject[]>();
	}
}
