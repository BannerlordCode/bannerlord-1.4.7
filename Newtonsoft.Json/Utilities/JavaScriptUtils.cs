using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005C RID: 92
	[NullableContext(1)]
	[Nullable(0)]
	internal static class JavaScriptUtils
	{
		// Token: 0x06000535 RID: 1333 RVA: 0x00015F4C File Offset: 0x0001414C
		static JavaScriptUtils()
		{
			IList<char> list = new List<char> { '\n', '\r', '\t', '\\', '\f', '\b' };
			for (int i = 0; i < 32; i++)
			{
				list.Add((char)i);
			}
			foreach (char c in list.Union<char>(new char[] { '\'' }))
			{
				JavaScriptUtils.SingleQuoteCharEscapeFlags[(int)c] = true;
			}
			foreach (char c2 in list.Union<char>(new char[] { '"' }))
			{
				JavaScriptUtils.DoubleQuoteCharEscapeFlags[(int)c2] = true;
			}
			foreach (char c3 in list.Union<char>(new char[] { '"', '\'', '<', '>', '&' }))
			{
				JavaScriptUtils.HtmlCharEscapeFlags[(int)c3] = true;
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x000160B8 File Offset: 0x000142B8
		public static bool[] GetCharEscapeFlags(StringEscapeHandling stringEscapeHandling, char quoteChar)
		{
			if (stringEscapeHandling == StringEscapeHandling.EscapeHtml)
			{
				return JavaScriptUtils.HtmlCharEscapeFlags;
			}
			if (quoteChar == '"')
			{
				return JavaScriptUtils.DoubleQuoteCharEscapeFlags;
			}
			return JavaScriptUtils.SingleQuoteCharEscapeFlags;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x000160D4 File Offset: 0x000142D4
		public static bool ShouldEscapeJavaScriptString([Nullable(2)] string s, bool[] charEscapeFlags)
		{
			if (s == null)
			{
				return false;
			}
			foreach (char c in s)
			{
				if ((int)c >= charEscapeFlags.Length || charEscapeFlags[(int)c])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00016110 File Offset: 0x00014310
		[NullableContext(2)]
		public static void WriteEscapedJavaScriptString([Nullable(1)] TextWriter writer, string s, char delimiter, bool appendDelimiters, [Nullable(1)] bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, IArrayPool<char> bufferPool, ref char[] writeBuffer)
		{
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
			if (!StringUtils.IsNullOrEmpty(s))
			{
				int num = JavaScriptUtils.FirstCharToEscape(s, charEscapeFlags, stringEscapeHandling);
				if (num == -1)
				{
					writer.Write(s);
				}
				else
				{
					if (num != 0)
					{
						if (writeBuffer == null || writeBuffer.Length < num)
						{
							writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, num, writeBuffer);
						}
						s.CopyTo(0, writeBuffer, 0, num);
						writer.Write(writeBuffer, 0, num);
					}
					int num2;
					for (int i = num; i < s.Length; i++)
					{
						char c = s[i];
						if ((int)c >= charEscapeFlags.Length || charEscapeFlags[(int)c])
						{
							string text;
							if (c <= '\\')
							{
								switch (c)
								{
								case '\b':
									text = "\\b";
									break;
								case '\t':
									text = "\\t";
									break;
								case '\n':
									text = "\\n";
									break;
								case '\v':
									goto IL_012A;
								case '\f':
									text = "\\f";
									break;
								case '\r':
									text = "\\r";
									break;
								default:
									if (c != '\\')
									{
										goto IL_012A;
									}
									text = "\\\\";
									break;
								}
							}
							else if (c != '\u0085')
							{
								if (c != '\u2028')
								{
									if (c != '\u2029')
									{
										goto IL_012A;
									}
									text = "\\u2029";
								}
								else
								{
									text = "\\u2028";
								}
							}
							else
							{
								text = "\\u0085";
							}
							IL_018C:
							if (text == null)
							{
								goto IL_022E;
							}
							bool flag = string.Equals(text, "!", StringComparison.Ordinal);
							if (i > num)
							{
								num2 = i - num + (flag ? 6 : 0);
								int num3 = (flag ? 6 : 0);
								if (writeBuffer == null || writeBuffer.Length < num2)
								{
									char[] array = BufferUtils.RentBuffer(bufferPool, num2);
									if (flag)
									{
										Array.Copy(writeBuffer, array, 6);
									}
									BufferUtils.ReturnBuffer(bufferPool, writeBuffer);
									writeBuffer = array;
								}
								s.CopyTo(num, writeBuffer, num3, num2 - num3);
								writer.Write(writeBuffer, num3, num2 - num3);
							}
							num = i + 1;
							if (!flag)
							{
								writer.Write(text);
								goto IL_022E;
							}
							writer.Write(writeBuffer, 0, 6);
							goto IL_022E;
							IL_012A:
							if ((int)c >= charEscapeFlags.Length && stringEscapeHandling != StringEscapeHandling.EscapeNonAscii)
							{
								text = null;
								goto IL_018C;
							}
							if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
							{
								text = "\\'";
								goto IL_018C;
							}
							if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
							{
								text = "\\\"";
								goto IL_018C;
							}
							if (writeBuffer == null || writeBuffer.Length < 6)
							{
								writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, 6, writeBuffer);
							}
							StringUtils.ToCharAsUnicode(c, writeBuffer);
							text = "!";
							goto IL_018C;
						}
						IL_022E:;
					}
					num2 = s.Length - num;
					if (num2 > 0)
					{
						if (writeBuffer == null || writeBuffer.Length < num2)
						{
							writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, num2, writeBuffer);
						}
						s.CopyTo(num, writeBuffer, 0, num2);
						writer.Write(writeBuffer, 0, num2);
					}
				}
			}
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000163A4 File Offset: 0x000145A4
		public static string ToEscapedJavaScriptString([Nullable(2)] string value, char delimiter, bool appendDelimiters, StringEscapeHandling stringEscapeHandling)
		{
			bool[] charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(stringEscapeHandling, delimiter);
			string text;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter((value != null) ? value.Length : 16))
			{
				char[] array = null;
				JavaScriptUtils.WriteEscapedJavaScriptString(stringWriter, value, delimiter, appendDelimiters, charEscapeFlags, stringEscapeHandling, null, ref array);
				text = stringWriter.ToString();
			}
			return text;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00016400 File Offset: 0x00014600
		private static int FirstCharToEscape(string s, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling)
		{
			for (int num = 0; num != s.Length; num++)
			{
				char c = s[num];
				if ((int)c < charEscapeFlags.Length)
				{
					if (charEscapeFlags[(int)c])
					{
						return num;
					}
				}
				else
				{
					if (stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
					{
						return num;
					}
					if (c == '\u0085' || c == '\u2028' || c == '\u2029')
					{
						return num;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00016454 File Offset: 0x00014654
		public static Task WriteEscapedJavaScriptStringAsync(TextWriter writer, string s, char delimiter, bool appendDelimiters, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			if (appendDelimiters)
			{
				return JavaScriptUtils.WriteEscapedJavaScriptStringWithDelimitersAsync(writer, s, delimiter, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
			}
			if (StringUtils.IsNullOrEmpty(s))
			{
				return cancellationToken.CancelIfRequestedAsync() ?? AsyncUtils.CompletedTask;
			}
			return JavaScriptUtils.WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000164B4 File Offset: 0x000146B4
		private static Task WriteEscapedJavaScriptStringWithDelimitersAsync(TextWriter writer, string s, char delimiter, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			Task task = writer.WriteAsync(delimiter, cancellationToken);
			if (!task.IsCompletedSucessfully())
			{
				return JavaScriptUtils.WriteEscapedJavaScriptStringWithDelimitersAsync(task, writer, s, delimiter, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
			}
			if (!StringUtils.IsNullOrEmpty(s))
			{
				task = JavaScriptUtils.WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
				if (task.IsCompletedSucessfully())
				{
					return writer.WriteAsync(delimiter, cancellationToken);
				}
			}
			return JavaScriptUtils.WriteCharAsync(task, writer, delimiter, cancellationToken);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001651C File Offset: 0x0001471C
		private static async Task WriteEscapedJavaScriptStringWithDelimitersAsync(Task task, TextWriter writer, string s, char delimiter, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			await task.ConfigureAwait(false);
			if (!StringUtils.IsNullOrEmpty(s))
			{
				await JavaScriptUtils.WriteEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken).ConfigureAwait(false);
			}
			await writer.WriteAsync(delimiter).ConfigureAwait(false);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000165A4 File Offset: 0x000147A4
		public static async Task WriteCharAsync(Task task, TextWriter writer, char c, CancellationToken cancellationToken)
		{
			await task.ConfigureAwait(false);
			await writer.WriteAsync(c, cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00016600 File Offset: 0x00014800
		private static Task WriteEscapedJavaScriptStringWithoutDelimitersAsync(TextWriter writer, string s, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			int num = JavaScriptUtils.FirstCharToEscape(s, charEscapeFlags, stringEscapeHandling);
			if (num != -1)
			{
				return JavaScriptUtils.WriteDefinitelyEscapedJavaScriptStringWithoutDelimitersAsync(writer, s, num, charEscapeFlags, stringEscapeHandling, client, writeBuffer, cancellationToken);
			}
			return writer.WriteAsync(s, cancellationToken);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00016634 File Offset: 0x00014834
		private static async Task WriteDefinitelyEscapedJavaScriptStringWithoutDelimitersAsync(TextWriter writer, string s, int lastWritePosition, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, JsonTextWriter client, char[] writeBuffer, CancellationToken cancellationToken)
		{
			if (writeBuffer == null || writeBuffer.Length < lastWritePosition)
			{
				writeBuffer = client.EnsureWriteBuffer(lastWritePosition, 6);
			}
			if (lastWritePosition != 0)
			{
				s.CopyTo(0, writeBuffer, 0, lastWritePosition);
				await writer.WriteAsync(writeBuffer, 0, lastWritePosition, cancellationToken).ConfigureAwait(false);
			}
			bool isEscapedUnicodeText = false;
			string escapedValue = null;
			int num;
			for (int i = lastWritePosition; i < s.Length; i++)
			{
				char c = s[i];
				if ((int)c >= charEscapeFlags.Length || charEscapeFlags[(int)c])
				{
					if (c <= '\\')
					{
						switch (c)
						{
						case '\b':
							escapedValue = "\\b";
							goto IL_02A7;
						case '\t':
							escapedValue = "\\t";
							goto IL_02A7;
						case '\n':
							escapedValue = "\\n";
							goto IL_02A7;
						case '\v':
							break;
						case '\f':
							escapedValue = "\\f";
							goto IL_02A7;
						case '\r':
							escapedValue = "\\r";
							goto IL_02A7;
						default:
							if (c == '\\')
							{
								escapedValue = "\\\\";
								goto IL_02A7;
							}
							break;
						}
					}
					else
					{
						if (c == '\u0085')
						{
							escapedValue = "\\u0085";
							goto IL_02A7;
						}
						if (c == '\u2028')
						{
							escapedValue = "\\u2028";
							goto IL_02A7;
						}
						if (c == '\u2029')
						{
							escapedValue = "\\u2029";
							goto IL_02A7;
						}
					}
					if ((int)c >= charEscapeFlags.Length && stringEscapeHandling != StringEscapeHandling.EscapeNonAscii)
					{
						goto IL_04A2;
					}
					if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
					{
						escapedValue = "\\'";
					}
					else if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
					{
						escapedValue = "\\\"";
					}
					else
					{
						if (writeBuffer.Length < 6)
						{
							writeBuffer = client.EnsureWriteBuffer(6, 0);
						}
						StringUtils.ToCharAsUnicode(c, writeBuffer);
						isEscapedUnicodeText = true;
					}
					IL_02A7:
					if (i > lastWritePosition)
					{
						num = i - lastWritePosition + (isEscapedUnicodeText ? 6 : 0);
						int num2 = (isEscapedUnicodeText ? 6 : 0);
						if (writeBuffer.Length < num)
						{
							writeBuffer = client.EnsureWriteBuffer(num, 6);
						}
						s.CopyTo(lastWritePosition, writeBuffer, num2, num - num2);
						await writer.WriteAsync(writeBuffer, num2, num - num2, cancellationToken).ConfigureAwait(false);
					}
					lastWritePosition = i + 1;
					if (!isEscapedUnicodeText)
					{
						await writer.WriteAsync(escapedValue, cancellationToken).ConfigureAwait(false);
					}
					else
					{
						await writer.WriteAsync(writeBuffer, 0, 6, cancellationToken).ConfigureAwait(false);
						isEscapedUnicodeText = false;
					}
				}
				IL_04A2:;
			}
			num = s.Length - lastWritePosition;
			if (num != 0)
			{
				if (writeBuffer.Length < num)
				{
					writeBuffer = client.EnsureWriteBuffer(num, 0);
				}
				s.CopyTo(lastWritePosition, writeBuffer, 0, num);
				await writer.WriteAsync(writeBuffer, 0, num, cancellationToken).ConfigureAwait(false);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000166B4 File Offset: 0x000148B4
		public static bool TryGetDateFromConstructorJson(JsonReader reader, out DateTime dateTime, [Nullable(2)] [NotNullWhen(false)] out string errorMessage)
		{
			dateTime = default(DateTime);
			errorMessage = null;
			long? num;
			if (!JavaScriptUtils.TryGetDateConstructorValue(reader, out num, out errorMessage) || num == null)
			{
				errorMessage = errorMessage ?? "Date constructor has no arguments.";
				return false;
			}
			long? num2;
			if (!JavaScriptUtils.TryGetDateConstructorValue(reader, out num2, out errorMessage))
			{
				return false;
			}
			if (num2 != null)
			{
				List<long> list = new List<long> { num.Value, num2.Value };
				long? num3;
				while (JavaScriptUtils.TryGetDateConstructorValue(reader, out num3, out errorMessage))
				{
					if (num3 != null)
					{
						list.Add(num3.Value);
					}
					else
					{
						if (list.Count > 7)
						{
							errorMessage = "Unexpected number of arguments when reading date constructor.";
							return false;
						}
						while (list.Count < 7)
						{
							list.Add(0L);
						}
						dateTime = new DateTime((int)list[0], (int)list[1] + 1, (list[2] == 0L) ? 1 : ((int)list[2]), (int)list[3], (int)list[4], (int)list[5], (int)list[6]);
						return true;
					}
				}
				return false;
			}
			dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(num.Value);
			return true;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000167D8 File Offset: 0x000149D8
		private static bool TryGetDateConstructorValue(JsonReader reader, out long? integer, [Nullable(2)] [NotNullWhen(false)] out string errorMessage)
		{
			integer = null;
			errorMessage = null;
			if (!reader.Read())
			{
				errorMessage = "Unexpected end when reading date constructor.";
				return false;
			}
			if (reader.TokenType == JsonToken.EndConstructor)
			{
				return true;
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				errorMessage = "Unexpected token when reading date constructor. Expected Integer, got " + reader.TokenType.ToString();
				return false;
			}
			integer = new long?((long)reader.Value);
			return true;
		}

		// Token: 0x040001F4 RID: 500
		internal static readonly bool[] SingleQuoteCharEscapeFlags = new bool[128];

		// Token: 0x040001F5 RID: 501
		internal static readonly bool[] DoubleQuoteCharEscapeFlags = new bool[128];

		// Token: 0x040001F6 RID: 502
		internal static readonly bool[] HtmlCharEscapeFlags = new bool[128];

		// Token: 0x040001F7 RID: 503
		private const int UnicodeTextLength = 6;

		// Token: 0x040001F8 RID: 504
		private const string EscapedUnicodeText = "!";
	}
}
