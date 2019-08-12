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
    /// This class implements the API endpoints for interacting with <see cref="Group"/> resources
    /// </summary>
    public class GroupController : HttpController
    {
        private GroupHandler _groupHandler;

        /// <summary>
        /// Initializes an instance of this class
        /// </summary>
        /// <param name="handler">Reference to the business logic for processing incoming requests</param>
        public GroupController(GroupHandler handler)
        {
            _groupHandler = handler;
        }

        /// <summary>
        /// Returns the list of groups
        /// </summary>
        /// <param name="context"></param>
        [Path(ApiBase.API_V1 + "/groups",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.GROUP_READ)]
        public void GetGroups(HttpContext context)
        {
            Groups groups = _groupHandler.GetGroups(context.Request.ParsedQuery);
            string json = JsonSerializer.SerializeJson(groups);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Creates a new group. The name of the group can be specified in payload.
        /// </summary>
        /// <param name="context"></param>
        [Path(ApiBase.API_V1 + "/groups",HttpMethod.POST)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void AddGroup(HttpContext context)
        {
            int id = 0;
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                Group group = JsonSerializer.DeserializeJson<Group>(json);
                if(group == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                //create group
                id = _groupHandler.CreateGroup(group.Name);
            }
            else
            {
                //create group with name = ""
                id = _groupHandler.CreateGroup("");
            }
            context.Response.Headers.Set("Location", ApiBase.API_V1 + "/groups/" + id);
            context.Response.Status = HttpStatus.Created;
        }

        /// <summary>
        /// Returns the data of the group having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupId"></param>
        [Path(ApiBase.API_V1 + "/groups/{Id}", HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void GetGroup(HttpContext context, string groupId)
        {
            int id = ApiBase.ParseId(groupId);
            Group group = _groupHandler.GetGroup(id);
            string json = JsonSerializer.SerializeJson(group);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Changes the name of the group having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupId"></param>
        [Path(ApiBase.API_V1 + "/groups/{Id}", HttpMethod.PUT)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void SetGroup(HttpContext context, string groupId)
        {
            string json = context.Request.Payload.ReadAll();
            Group group = JsonSerializer.DeserializeJson<Group>(json);

            int id = ApiBase.ParseId(groupId);
            if(group == null)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
            }
            else
            {
                _groupHandler.ChangeName(id, group.Name);
                context.Response.Status = HttpStatus.OK;
            }
        }
      
        /// <summary>
        /// Deletes the group having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupId"></param>
        [Path(ApiBase.API_V1 + "/groups/{Id}",HttpMethod.DELETE)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void DeleteGroup(HttpContext context, string groupId)
        {
            int id = ApiBase.ParseId(groupId);
            _groupHandler.DeleteGroup(id);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Returns the list of LEDs which are member of the group having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupId"></param>
        [Path(ApiBase.API_V1 + "/groups/{Id}/leds",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.GROUP_READ)]
        public void GetLedsOfGroup(HttpContext context, string groupId)
        {
            int id = ApiBase.ParseId(groupId);
            GroupLeds groupLeds = _groupHandler.GetGroupLeds(id);
            string json = JsonSerializer.SerializeJson(groupLeds);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Overwrites the list of LEDs of the group having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupId"></param>
        [Path(ApiBase.API_V1 + "/groups/{Id}/leds", HttpMethod.PUT)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void SetLedsOfGroup(HttpContext context, string groupId)
        {
            string json = context.Request.Payload.ReadAll();
            GroupLeds groupLeds = JsonSerializer.DeserializeJson<GroupLeds>(json);

            int id = ApiBase.ParseId(groupId);
            if(groupLeds == null)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
            }
            _groupHandler.SetLedsOfGroup(id, groupLeds);
            context.Response.Status = HttpStatus.OK;
        }

        /*
        [Path(ApiBase.API_V1 + "/groups/{Id}/leds", HttpMethod.DELETE)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void RemoveLedsFromGroup(HttpContext context, string groupId)
        {
            try
            {
                int iid = 0;
                if (!Int32.TryParse(groupId, out iid))
                {
                    throw new BadRequestException("");
                }
                if (context.Request.Payload.Length > 0)
                {
                    string json = context.Request.Payload.ReadAll();
                    GroupLeds groupLeds = JsonSerializer.DeserializeJson<GroupLeds>(json);
                    if (groupLeds != null)
                    {
                        IList<string> ids = new List<string>();
                        foreach (Led led in groupLeds.Leds)
                        {
                            ids.Add(led.ControllerId + ":" + led.LedNumber);
                        }
                        _groupHandler.RemoveLedsFromGroup(iid, ids);
                        context.Response.Status = HttpStatus.OK;
                    }
                    else
                    {
                        throw new BadRequestException("");
                    }
                }
                else
                {
                    throw new BadRequestException("");
                }
            }
            catch (BadRequestException bre)
            {
                context.Response.Status = HttpStatus.BadRequest;
            }
            catch (ResourceNotFoundException)
            {
                context.Response.Status = HttpStatus.NotFound;
            }
            //IO Exception
            catch (Exception e)
            {
                context.Response.Status = HttpStatus.InternalServerError;
                context.Response.Payload.Write(e.Message);
            }
        }
        */

        /// <summary>
        /// Sets the color of all LEDs being member of the group having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupId"></param>
        [Path(ApiBase.API_V1 + "/groups/{Id}/leds", HttpMethod.POST)]
        [AuthorizationScope(Scopes.LED_WRITE)]
        public void SetColorOfGroup(HttpContext context, string groupId)
        {
            string json = context.Request.Payload.ReadAll();
            RgbValue rgbValue = JsonSerializer.DeserializeJson<RgbValue>(json);

            int id = ApiBase.ParseId(groupId);
            if(rgbValue == null)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
            }
            _groupHandler.SetColorOfGroup(id, rgbValue.Rgb);
            context.Response.Status = HttpStatus.OK;
        }

    }
}
