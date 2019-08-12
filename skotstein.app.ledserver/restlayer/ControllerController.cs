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
        /// <param name="context"></param>
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
        /// Creates a new controller
        /// </summary>
        /// <param name="context"></param>
        [Path(ApiBase.API_V1 + "/controllers", HttpMethod.POST)]
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
            context.Response.Headers.Set("Location", ApiBase.API_V1 + "/controllers/" + id);
            context.Response.Status = HttpStatus.Created;
        }

        /// <summary>
        /// Returns the data of the controller having the specified ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
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
        /// <param name="context"></param>
        /// <param name="id"></param>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}", HttpMethod.PUT)]
        [AuthorizationScope(Scopes.CONTROLLER_WRITE)]
        public void SetController(HttpContext context, string controllerId)
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
                context.Response.Status = HttpStatus.OK;
            }
        }

        /// <summary>
        /// Unregister the controller having the specified ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
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
        /// <param name="context"></param>
        /// <param name="controllerId"></param>
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
        /// <param name="context"></param>
        /// <param name="controllerId"></param>
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
        /// <param name="context"></param>
        /// <param name="controllerId"></param>
        [Path(ApiBase.API_V1 + "/controllers/{controllerId}/leds", HttpMethod.POST)]
        [AuthorizationScope(Scopes.LED_WRITE)]
        public void SetLeds(HttpContext context, string controllerId)
        {
            int id = ApiBase.ParseId(controllerId);
            if (context.Request.Payload.Length > 0)
            {
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
