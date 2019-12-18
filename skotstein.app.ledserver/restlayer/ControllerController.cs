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
using SKotstein.Net.Http.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.restlayer
{ 
    /// <summary>
    /// This class implements the API endpoints for interacting with <see cref="Controller"/> resources
    /// </summary>
    public class ControllerController : HttpController
    { 
        private ControllerHandler _controllerHandler;

        /// <summary>
        /// Initializes an instance of this class
        /// </summary>
        /// <param name="handler">Reference to the business logic for processing incoming requests</param>
        public ControllerController(ControllerHandler handler)
        {
            _controllerHandler = handler;
        }

        /// <summary>
        /// Returns the list of registered controllers
        /// </summary>
        /// <group>Controller</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/controllers</url>
        /// <param name="id" cref="Int32" in="query">Returns only the controller having the passed ID.</param>
        /// <param name="name" cref="string" in="query">Returns all controllers whose name contains the passed value (case invariant).</param>
        /// <param name="device_name" cref="string" in="query">Returns all controllers whose device name contains the passed value (case invariant). Note that the device name is only visible if the respective controller is online.</param>
        /// <param name="firmware_id" cref="string" in="query">Returns all controllers whose firmware has the passed firmware ID. Note that the firmware ID is only visible if the respective controller is online.</param>
        /// <param name="network_state" cref="NetworkState" in="query">Returns all controllers whose network state has the passed value (case invariant).</param>
        /// <response code="200">
        ///     <see cref="Controllers"/>
        ///     successful response
        ///     <example name="List of registered controllers">
        ///         <value>
        ///             $EXAMPLE_0Controllers
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The ID must be an integer
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
        ///         </value>
        ///     </example>
        /// </response>
       

        [Path(ApiBase.API_V1 + "/controllers",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.CONTROLLER_READ)]
        public void GetControllers(HttpContext context)
        {
            Controllers controllers = _controllerHandler.GetControllers(context.Request.ParsedQuery);
            string json = JsonSerializer.SerializeJson(controllers);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Registers a new controller
        /// </summary>
        /// <group>Controller</group>
        /// <verb>POST</verb>
        /// <url>pseudo://localhost/api/v1/controllers</url>
        /// <param name="payload" in="body" required="false" type="application/json">
        ///     <see cref="Controller"/>
        ///     If present, the name and the number of LEDs can be specified
        ///     <example name="Name and LED count">
        ///         <value>
        ///             $EXAMPLE_3ControllerChange
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="201">
        ///     <header name="Location" cref="string"><description>Contains the URL pointing to the resource representing the controller which has been successfully registered</description></header>
        ///     <see cref="Controller"/>
        ///     successful response
        ///     <example name="Controller">
        ///         <value>
        ///             $EXAMPLE_1Controller
        ///         </value>
        ///     </example>    
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The payload is invalid or the LED count cannot be negative or greater than 256
        ///     <example name="Invalid LED count">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvaldidLedCount
        ///         </value>
        ///     </example>
        ///     <example name="Invalid payload">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidPayload
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/controllers", HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.CONTROLLER_WRITE)]
        public void AddController(HttpContext context)
        {
            int id = 0;
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                Controller controller = JsonSerializer.DeserializeJson<Controller>(json);
                if(controller == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                id = _controllerHandler.CreateController(controller.Name, controller.LedCount);
            }
            else
            {
                //create controller with name = "" and Led Count = 0
                id = _controllerHandler.CreateController("", 0);
            }

            Controller result = _controllerHandler.GetController(id);
            string jsonResult = JsonSerializer.SerializeJson(result);

            context.Response.Headers.Set("Location", ApiBase.API_V1 + "/controllers/" + id);
            context.Response.Payload.Write(jsonResult);
            context.Response.Status = HttpStatus.Created;
        }

        /// <summary>
        /// Returns the controller having the specified ID
        /// </summary>
        /// <group>Controller</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/controllers/{controllerId}</url>
        /// <param name="controllerId" cref="int" in="path">The ID of the controller</param>
        /// <response code="200">
        ///     <see cref="Controller"/>
        ///     successful response
        ///     <example name="Controller">
        ///         <value>
        ///             $EXAMPLE_1Controller
        ///         </value>
        ///     </example>    
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The ID must be an integer
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The controller having the passed ID does not exist
        ///     <example name="Controller not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgControllerNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.CONTROLLER_READ)]
        public void GetController(HttpContext context, string controllerId)
        {
            int id = ApiBase.ParseId(controllerId);
            string json = JsonSerializer.SerializeJson(_controllerHandler.GetController(id));
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Changes the parameters of the controller having the specified ID
        /// </summary>
        /// <group>Controller</group>
        /// <verb>PUT</verb>
        /// <url>pseudo://localhost/api/v1/controllers/{controllerId}</url>
        /// <param name="controllerId" cref="int" in="path">The ID of the controller</param>
        /// <param name="payload" in="body" required="false" type="application/json">
        ///     <see cref="Controller"/>
        ///     Contains the parameters which should be changed
        ///     <example name="Name and LED count">
        ///         <value>
        ///             $EXAMPLE_3ControllerChange
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="200"> 
        ///     <see cref="Controller"/>
        ///     The response contains the updated controller
        ///     <example name="Controller">
        ///         <value>
        ///             $EXAMPLE_1Controller
        ///         </value>
        ///     </example>    
        ///</response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The payload is empty, invalid or the ID must be an integer
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
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
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The controller having the passed ID does not exist
        ///     <example name="Controller not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgControllerNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}", HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.CONTROLLER_WRITE)]
        public void SetController(HttpContext context, string controllerId)
        {
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                Controller controller = JsonSerializer.DeserializeJson<Controller>(json);

                int id = ApiBase.ParseId(controllerId);
                if (controller == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                else
                {
                    _controllerHandler.ChangeController(id, controller.Name, controller.LedCount);

                    Controller response = _controllerHandler.GetController(id);
                    string jsonResponse = JsonSerializer.SerializeJson(response);
                    context.Response.Payload.Write(jsonResponse);
                    context.Response.Status = HttpStatus.OK;
                }
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_PAYLOAD_EXPECTED);
            }
        }

        /// <summary>
        /// Unregisters the controller having the specified ID
        /// </summary>
        /// <group>Controller</group>
        /// <verb>DELETE</verb>
        /// <url>pseudo://localhost/api/v1/controllers/{controllerId}</url>
        /// <param name="controllerId" cref="int" in="path">The ID of the controller</param>
        /// <response code="200">successful</response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The ID must be an integer
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}", HttpMethod.DELETE)]
        [AuthorizationScope(Scopes.CONTROLLER_WRITE)]
        public void DeleteController(HttpContext context, string controllerId)
        {
            int id = ApiBase.ParseId(controllerId);
            _controllerHandler.DeleteController(id);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Returns the meta information of the firmware which is installed on the controller having the specified ID
        /// </summary>
        /// <group>Controller</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/controllers/{controllerId}/firmware</url>
        /// <param name="controllerId" cref="int" in="path">The ID of the controller</param>
        /// <response code="200">
        ///     <see cref="ControllerFirmware"/>
        ///     successful response
        ///     <example name="Firmware">
        ///         <value>
        ///             $EXAMPLE_4ControllerFirmware
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The ID must be an integer
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The controller having the passed ID does not exist
        ///     <example name="Controller not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgControllerNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}/firmware", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.FIRMWARE_READ)]
        public void GetFirmware(HttpContext context, string controllerId)
        {
            int id = ApiBase.ParseId(controllerId);
            ControllerFirmware firmware = _controllerHandler.GetFirmware(id);
            string json = JsonSerializer.SerializeJson(firmware);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Returns a list of LEDs which are controlled by the controller having the specified ID
        /// </summary>
        /// <group>Controller</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/controllers/{controllerId}/leds</url>
        /// <param name="controllerId" cref="int" in="path">The ID of the controller</param>
        /// <response code="200">
        ///     <see cref="ControllerLeds"/>
        ///     successful response
        ///     <example name="Controller LEDs">
        ///         <value>
        ///             $EXAMPLE_5ControllerLeds
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The ID must be an integer
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The controller having the passed ID does not exist
        ///     <example name="Controller not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgControllerNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}/leds", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.CONTROLLER_READ)]
        public void GetLeds(HttpContext context, string controllerId)
        {
            int id = ApiBase.ParseId(controllerId);
            ControllerLeds controllerLeds = _controllerHandler.GetLedsOfController(id);
            string json = JsonSerializer.SerializeJson(controllerLeds);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;            
        }

        /// <summary>
        /// Sets the color of all LEDs which are controlled by the controller having the specified ID
        /// </summary>
        /// <group>Controller</group>
        /// <verb>POST</verb>
        /// <url>pseudo://localhost/api/v1/controllers/{controllerId}/leds</url>
        /// <param name="controllerId" cref="int" in="path">The ID of the controller</param>
        /// <param name="payload" in="body" required="true">
        ///     <see cref="RgbValue"/>
        ///     The prefered color of the LEDs
        ///     <example name="RGB value">
        ///         <value>
        ///             $EXAMPLE_2RgbValue
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="200">successful</response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The payload is empty, invalid or the ID must be an integer
        ///     <example name="Invalid ID">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidId
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
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The controller having the passed ID does not exist
        ///     <example name="Controller not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgControllerNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}/leds", HttpMethod.POST)]
        [AuthorizationScope(Scopes.LED_WRITE)]
        public void SetLeds(HttpContext context, string controllerId)
        {
            if (context.Request.Payload.Length > 0)
            {
                int id = ApiBase.ParseId(controllerId);
                string json = context.Request.Payload.ReadAll();
                RgbValue rgbValue = JsonSerializer.DeserializeJson<RgbValue>(json);
                if(rgbValue != null)
                {
                    _controllerHandler.SetColor(id, rgbValue.Rgb);
                    context.Response.Status = HttpStatus.OK;
                }
                else
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_PAYLOAD_EXPECTED);
            }
        }

    }
}
