using skotstein.app.ledserver.businesslayer;
using skotstein.app.ledserver.exceptions;
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
    /// <summary>
    /// This class implements API endpoints which allow an HTTP client to query <see cref="Led"/> resources (see <see cref="GetLeds(HttpContext)"/> and <see cref="GetLed(HttpContext, string)"/>)
    /// and to set their color (see <see cref="SetLed(HttpContext, string)"/>).
    /// Incoming HTTP requests are mapped and forwared to the methods implemented in this class automatically.
    /// </summary>
    public class LedController : HttpController
    {
        private LedHandler _ledHandler;

        /// <summary>
        /// Initializes an instance of this class
        /// </summary>
        /// <param name="handler">Reference to the business logic for processing incoming requests</param>
        public LedController(LedHandler handler)
        {
            _ledHandler = handler;
        }

        /// <summary>
        /// Returns the list of LEDs.
        /// </summary>
        /// <param name="context"></param>
        [Path(ApiBase.API_V1+"/leds",SKotstein.Net.Http.Context.HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.LED_READ)]
        public void GetLeds(HttpContext context)
        {
            Leds leds = _ledHandler.GetLeds(context.Request.ParsedQuery);
            string json = JsonSerializer.SerializeJson(leds);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Returns the data of the LED having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ledId"></param>
        [Path(ApiBase.API_V1+"/leds/{id}",SKotstein.Net.Http.Context.HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.LED_READ)]
        public void GetLed(HttpContext context, string ledId)
        {
            Led led = _ledHandler.GetLed(ledId);
            string json = JsonSerializer.SerializeJson(led);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Sets the color of the LED having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ledId"></param>
        [Path(ApiBase.API_V1+"/leds/{id}",SKotstein.Net.Http.Context.HttpMethod.PUT)]
        [AuthorizationScope(Scopes.LED_WRITE)]
        public void SetLed(HttpContext context, string ledId)
        {
            string json = context.Request.Payload.ReadAll();
            RgbValue rgbValue = JsonSerializer.DeserializeJson<RgbValue>(json);
            _ledHandler.SetColor(ledId, rgbValue.Rgb);
            context.Response.Status = HttpStatus.OK;
        }
    }
}
