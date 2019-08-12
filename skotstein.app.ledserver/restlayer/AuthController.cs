using skotstein.app.ledserver.model;
using skotstein.app.ledserver.tools;
using skotstein.net.http.oauth;
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
    public class AuthController : HttpController
    {
        private OAuth2 _oauth;

        public AuthController(OAuth2 oauth)
        {
            _oauth = oauth;
        }

        [Path(ApiBase.API_V1+"/auth",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetAuthBase(HttpContext context)
        {
            context.Response.Payload.Write(JsonSerializer.SerializeJson(new AuthBase()));
            context.Response.Status = HttpStatus.OK;
        }

        [Path(ApiBase.API_V1+"/auth/token",SKotstein.Net.Http.Context.HttpMethod.POST)]
        //Content-Type is set by _oAuth.Authorize(...)
        public void Authorize(HttpContext context)
        {
            if (context.Request.Headers.Has("Content-Type"))
            {
                if (context.Request.Headers.Has("Content-Type")&& context.Request.Headers.Get("Content-Type").CompareTo(MimeType.APPLICATION_X_WWW_FORM_URLENCODED) == 0)
                {
                    IDictionary<string, string> query = new Dictionary<string, string>();
                    string payload = context.Request.Payload.ReadAll();
                    payload = payload.Replace("\n","").Replace("\r","");

                    foreach(string tuple in payload.Split('&'))
                    {
                        if(tuple.Split('=').Length == 2)
                        {
                            string key = HttpRequest.DecodeUrl(tuple.Split('=')[0],"ASCII");
                            string value = HttpRequest.DecodeUrl(tuple.Split('=')[1], "ASCII");
                            if (!query.ContainsKey(key))
                            {
                                query.Add(key, value);
                            }
                        }
                    }

                    _oauth.Authorize(context, query);
                }
            }
            else
            {
                _oauth.Authorize(context, context.Request.ParsedQuery);
            }
        }


    }
}
