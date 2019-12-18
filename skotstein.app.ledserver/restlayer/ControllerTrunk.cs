// MIT License
//
// Copyright (c) 2019 Sebastian Kotstein
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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
