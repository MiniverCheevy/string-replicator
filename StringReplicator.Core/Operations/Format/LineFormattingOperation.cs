using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Voodoo;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations.Format
{
    /// <summary>
    ///     This is a slightly modified version of the Format function in string.cs from the mono project.
    ///     https://github.com/mono/mono/blob/effa4c07ba850bedbe1ff54b2a5df281c058ebcb/mcs/class/corlib/System/String.cs
    /// </summary>
    public class LineFormattingOperation : Executor<LineFormattingRequest, TextResponse>
    {
        private StringBuilder output;
        private static CultureInfo culture;

        static LineFormattingOperation()
        {
            culture = CultureInfo.CurrentUICulture;
        }
        public LineFormattingOperation(LineFormattingRequest request) : base(request)
        {
            this.output = new StringBuilder();
        }

        protected override TextResponse ProcessRequest()
        {
            response.Text = format(request.FormatString, request.Arguments).ToString();
            return response;
        }

        protected override void CustomErrorBehavior(Exception ex)
        {
            base.CustomErrorBehavior(ex);
            response.Text = ex.Message;
        }

        private StringBuilder format(string format, IList<object> args)
        {
            var position = 0;
            var start = position;

            while (position < format.Length)
            {
                var c = format[position++];

                if (c == '{')
                {
                    output.Append(format, start, position - start - 1);
                    if (format[position] == '{')
                    {
                        start = position++;
                        continue;
                    }

                    position = doActualFormatting(format, args, position);

                    start = position;
                }
                else if (c == '}' && position < format.Length && format[position] == '}')
                {
                    output.Append(format, start, position - start - 1);
                    start = position++;
                }
                else if (c == '}')
                {
                    throw new FormatException(Messages.FormatError);
                }
            }

            if (start < format.Length)
                output.Append(format, start, format.Length - start);

            return output;
        }

        private int doActualFormatting(string format, IList<object> args, int position)
        {
            int argumentNumber, width;
            bool leftAlign;
            string argFormat;

            parseFormatSpecifier(format, ref position, out argumentNumber, out width, out leftAlign, out argFormat);
            if (argumentNumber >= args.Count)
                throw new FormatException(Messages.WrongNumberOfArguments);

            var argument = args[argumentNumber];
            var @string = argument.To<string>();
            object obj = null;
            if (attemptToCoerceStringToOtherType(@string, out obj))
            {
                var formattable = obj as IFormattable;
                if (formattable != null)
                    @string = formattable.ToString(argFormat, culture);
            }

            if (argFormat == "!")
                @string = @string.ToFriendlyString();

            if (argFormat == "^")
                @string = culture.TextInfo.ToTitleCase(@string.ToLower());

            if (width > @string.Length)
            {
                padAndAppend(width, @string, leftAlign);
            }
            else
            {
                output.Append(@string);
            }
            return position;
        }

        private bool attemptToCoerceStringToOtherType(string s, out object convertedObject)
        {
            convertedObject = null;

            if (!s.Any(char.IsDigit))
                return false;
                        
            var codes = new TypeCode[]{TypeCode.DateTime, TypeCode.Decimal, TypeCode.Int32};

            var provider = CultureInfo.CurrentCulture;
            foreach (var code in codes)
            {
                try
                {
                    var result = Convert.ChangeType(s, code, provider);
                    convertedObject = result;
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        private void padAndAppend(int width, string @string, bool leftAlign)
        {
            const char padCharacter = ' ';
            var padLength = width - @string.Length;

            if (leftAlign)
            {
                output.Append(@string);
                output.Append(padCharacter, padLength);
            }
            else
            {
                output.Append(padCharacter, padLength);
                output.Append(@string);
            }
        }

        private void parseFormatSpecifier(string @string, ref int position, out int argumentNumber, out int width,
            out bool leftAlign, out string format)
        {
            var max = @string.Length;

            // parses format specifier of form:
            //   N,[\ +[-]M][:F]}
            //
            // where:
            // N = argument number (non-negative integer)

            argumentNumber = parseDecimal(@string, ref position);
            if (argumentNumber < 0)
                throw new FormatException(Messages.FormatError);

            if (position < max && @string[position] == ',')
            {
                // White space between ',' and number or sign.
                ++position;
                while (position < max && Char.IsWhiteSpace(@string[position]))
                    ++position;
                var start = position;

                format = @string.Substring(start, position - start);

                leftAlign = (position < max && @string[position] == '-');
                if (leftAlign)
                    ++position;

                width = parseDecimal(@string, ref position);
                if (width < 0)
                    throw new FormatException(Messages.FormatError);
            }
            else
            {
                width = 0;
                leftAlign = false;
                format = String.Empty;
            }

            if (position < max && @string[position] == ':')
            {
                var start = ++position;
                while (position < max && @string[position] != '}')
                    ++position;

                format += @string.Substring(start, position - start);
            }
            else
                format = null;

            if ((position >= max) || @string[position++] != '}')
                throw new FormatException(Messages.FormatError);
        }

        private int parseDecimal(string @string, ref int position)
        {
            var p = position;
            var number = 0;
            var max = @string.Length;

            while (p < max)
            {
                var c = @string[p];
                if (c < '0' || '9' < c)
                    break;

                number = number*10 + c - '0';
                ++p;
            }

            if (p == position || p == max)
                return -1;

            position = p;
            return number;
        }
    }
}