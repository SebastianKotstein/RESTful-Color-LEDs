using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Manipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.tools
{
    public class OptionsPayloadInjector : HttpManipulator<RoutedContext>
    {
        public OptionsPayloadInjector(string name)
        {
            this.Name = name;
        }

        public override void Manipulate(RoutedContext context)
        {
            /*
            if(context.Context.Request.Method == HttpMethod.OPTIONS)
            {
                context.Context.Response.Payload.Write("");
                context.Context.Response.Headers.Set("Content-Type", MimeType.APPLICATION_JSON);
            }
            */
            
        }
    }
}
