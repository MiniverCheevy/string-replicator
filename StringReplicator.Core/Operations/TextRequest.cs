using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Voodoo;
using Voodoo.Messages;

namespace StringReplicator.Core.Operations
{    
    public class TextRequest
    {
	    private string _text;

	    public string Text
	    {
			set { _text = clean( value); }
			get { return _text; }
	    }

	    private string clean(string inputString)
	    {
			var response = new TextResponse() {Text  = inputString.To<string>()};
		    Voodoo.ActionHandler.Execute(() =>
		    {
			    response.Text = Encoding.ASCII.GetString(
				    Encoding.Convert(
					    Encoding.UTF8,
					    Encoding.GetEncoding(
						    Encoding.ASCII.EncodingName,
						    new EncoderReplacementFallback(string.Empty),
						    new DecoderExceptionFallback()
						    ),
					    Encoding.UTF8.GetBytes(inputString)
					    )
				    );

			    return response;
		    });
		    return response.Text;
	    }
    }
}
