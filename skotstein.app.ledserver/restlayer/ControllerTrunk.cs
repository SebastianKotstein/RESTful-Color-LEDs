using skotstein.app.ledserver.businesslayer;
using skotstein.app.ledserver.exceptions;
using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.restlayer
{
    public class ControllerTrunk : HttpController
    {
        private TrunkEndpoint _endpoint;

        public ControllerTrunk(TrunkEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        [Path("/backbone/v1/trunk",HttpMethod.POST)]
        [ContentType("RAW")]
        public void Notify(HttpContext context)
        {
            try
            {
                if (!context.Request.Headers.Has("Authorization"))
                {
                    throw new UnauthorizedAccessException();
                }

                if(context.Request.Payload.Length > 0)
                {
                    byte[] reply = _endpoint.ProcessMessage(context.Request.Payload.ReadAll(), context.Request.Headers.Get("Authorization"));
                    context.Response.Payload.WriteBytes(reply);
                    context.Response.Status = HttpStatus.OK;
                }
                else
                {
                    throw new BadRequestException("");
                }
            }
            catch(BadRequestException bre)
            {
                context.Response.Status = HttpStatus.BadRequest;
            }
            catch(UnauthorizedAccessException uae)
            {
                context.Response.Status = HttpStatus.Unauthorized;
            }

            
        }
    }
}
