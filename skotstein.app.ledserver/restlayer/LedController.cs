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
        /// Returns the list of all LEDs
        /// </summary>
        /// <group>LED</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/leds</url>
        /// <param name="id" cref="string" in="query">Returns only the LEDs having the passed ID.</param>
        /// <param name="controllerId" cref="int" in="query">Returns all LEDs which are controlled by the controller having the passed controller ID.</param>
        /// <param name="rgb" cref="string" in="query">Returns all LEDs which currently have the passed color (RGB value, case invariant, with or without leading '#', e.g. '#FFAA00').</param>
        /// <param name="greater" cref="int" in="query">Returns all LEDs whose LED number is greater than the passed value.</param>
        /// <param name="greater_equals" cref="int" in="query">Returns all LEDs whose LED number is greater than or equals the passed value.</param>
        /// <param name="less" cref="int" in="query">Returns all LEDs whose LED number is less than the passed value.</param>
        /// <param name="less_equals" cref="int" in="query">Returns all LEDs whose LED number is less than or equals the passed value.</param>
        /// <param name="equals" cref="int" in="query">Returns all LEDs whose LED number is equals the passed value.</param>
        /// <response code="200">
        ///     <see cref="Leds"/>
        ///     successful response
        ///     <example name="LEDs">
        ///         <value>
        ///             $EXAMPLE_11Leds
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The Controller ID must be an integer or the value of a query parameter is invalid
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
        ///         </value>
        ///     </example>
        ///     <example name="Invalid query value">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidQueryParameter
        ///         </value>
        ///     </example>
        /// </response>
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
        /// Returns the data of the LED having the specified ID
        /// </summary>
        /// <group>LED</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/leds/{ledId}</url>
        /// <param name="ledId" cref="string" in="path">The ID of the LED</param>
        /// <response code="200">
        ///     <see cref="Led"/>
        ///     successful response
        ///     <example name="LED">
        ///         <value>
        ///             $EXAMPLE_12Led
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The ID must have the format [int]:[int]
        ///     <example name="Invalid LED ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidLedId
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The LED having the passed ID does not exist
        ///     <example name="LED not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgLedNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1+"/leds/{ledId}",SKotstein.Net.Http.Context.HttpMethod.GET)]
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
        /// Sets the color of the LED having the specified ID
        /// </summary>
        /// <group>LED</group>
        /// <verb>PUT</verb>
        /// <url>pseudo://localhost/api/v1/leds/{ledId}</url>
        /// <param name="ledId" cref="string" in="path">The ID of the LED</param>
        /// <param name="payload" in="body" required="true">
        ///     <see cref="RgbValue"/>
        ///     The prefered color of the LEDs
        ///     <example name="RGB value">
        ///         <value>
        ///             $EXAMPLE_2RgbValue
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="200">
        ///     <see cref="Led"/>
        ///     The response contains the updated LED
        ///     <example name="LED">
        ///         <value>
        ///             $EXAMPLE_12Led
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The payload is empty, invalid or the  ID must have the format [int]:[int]
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidLedId
        ///         </value>
        ///     </example>
        ///     <example name="Invalid payload">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidPayload
        ///         </value>
        ///     </example>
        ///     <example name="Payload expected">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgPayloadExpected
        ///         </value>
        ///     </example>
        /// </response> 
        ///  <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The LED having the passed ID does not exist
        ///     <example name="LED not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgLedNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1+"/leds/{ledId}",SKotstein.Net.Http.Context.HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.LED_WRITE)]
        public void SetLed(HttpContext context, string ledId)
        {
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                RgbValue rgbValue = JsonSerializer.DeserializeJson<RgbValue>(json);
                if(rgbValue == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                _ledHandler.SetColor(ledId, rgbValue.Rgb);

                Led response = _ledHandler.GetLed(ledId);
                string jsonResponse = JsonSerializer.SerializeJson(response);
                context.Response.Payload.Write(jsonResponse);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_PAYLOAD_EXPECTED);
            }
           
        }
    }
}
