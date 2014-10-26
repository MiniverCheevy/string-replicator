using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Voodoo;
using Voodoo.Messages;
using Voodoo.Operations;

namespace StringReplicator.Core.Operations
{
    /// <summary>
    ///     This is a slightly modified version of the Format function in string.cs from the mono project.
    ///     https://github.com/mono/mono/blob/effa4c07ba850bedbe1ff54b2a5df281c058ebcb/mcs/class/corlib/System/String.cs
    /// </summary>
    public class LineFormatingOperation : Executor<LineFormattingRequest, TextResponse>
    {
        private StringBuilder output;

        public LineFormatingOperation(LineFormattingRequest request) : base(request)
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

            var arg = args[argumentNumber];
            var str = arg.To<string>();
            object obj = null;
            if (attemptToCoerceStringToOtherType(str, out obj))
            {
                var formattable = obj as IFormattable;
                if (formattable != null)
                    str = formattable.ToString(argFormat, CultureInfo.CurrentUICulture);
            }

            var friendly = argFormat == "!";
            if (friendly)
                str = str.ToFriendlyString();

            if (width > str.Length)
            {
                paddAndAppend(width, str, leftAlign);
            }
            else
            {
                output.Append(str);
            }
            return position;
        }

        private bool attemptToCoerceStringToOtherType(string s, out object convertedObject)
        {
            convertedObject = null;
            var unWantedCodes = new[] {TypeCode.Empty, TypeCode.Object, TypeCode.String};
            var codes =
                Enum.GetValues(typeof (TypeCode))
                .ToArray<TypeCode>()
                .Where(c => !unWantedCodes.Contains(c)).ToArray();

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

        private void paddAndAppend(int width, string str, bool leftAlign)
        {
            const char padchar = ' ';
            var padlen = width - str.Length;

            if (leftAlign)
            {
                output.Append(str);
                output.Append(padchar, padlen);
            }
            else
            {
                output.Append(padchar, padlen);
                output.Append(str);
            }
        }

        private void parseFormatSpecifier(string str, ref int position, out int argumentNumber, out int width,
            out bool leftAlign, out string format)
        {
            var max = str.Length;

            // parses format specifier of form:
            //   N,[\ +[-]M][:F]}
            //
            // where:
            // N = argument number (non-negative integer)

            argumentNumber = parseDecimal(str, ref position);
            if (argumentNumber < 0)
                throw new FormatException(Messages.FormatError);

            if (position < max && str[position] == ',')
            {
                // White space between ',' and number or sign.
                ++position;
                while (position < max && Char.IsWhiteSpace(str[position]))
                    ++position;
                var start = position;

                format = str.Substring(start, position - start);

                leftAlign = (position < max && str[position] == '-');
                if (leftAlign)
                    ++position;

                width = parseDecimal(str, ref position);
                if (width < 0)
                    throw new FormatException(Messages.FormatError);
            }
            else
            {
                width = 0;
                leftAlign = false;
                format = String.Empty;
            }

            if (position < max && str[position] == ':')
            {
                var start = ++position;
                while (position < max && str[position] != '}')
                    ++position;

                format += str.Substring(start, position - start);
            }
            else
                format = null;

            if ((position >= max) || str[position++] != '}')
                throw new FormatException(Messages.FormatError);
        }

        private int parseDecimal(string str, ref int position)
        {
            var p = position;
            var number = 0;
            var max = str.Length;

            while (p < max)
            {
                var c = str[p];
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