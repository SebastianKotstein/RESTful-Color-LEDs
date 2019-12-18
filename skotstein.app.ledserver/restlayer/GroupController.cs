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
        /// <group>Group</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/groups</url>
        /// <param name="id" cref="Int32" in="query">Returns only the groups having the passed ID.</param>
        /// <param name="name" cref="string" in="query">Returns all groups whose name contains the passed value (case invariant).</param>
        /// <param name="ledId" cref="string" in="query">Returns all groupss where the LED having the passed ID is a member.</param>
        /// <response code="200">
        ///     <see cref="Groups"/>
        ///     successful response
        ///     <example name="Groups">
        ///         <value>
        ///             $EXAMPLE_6Groups
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
        /// Creates a new group
        /// </summary>
        /// <group>Group</group>
        /// <verb>POST</verb>
        /// <url>pseudo://localhost/api/v1/groups</url>
        /// <param name="payload" in="body" required="false">
        ///     <see cref="Group"/>
        ///     The name of the group can be specified optionally. If the payload is empty, the name of the created group is empty. 
        ///     <example name="Group name">
        ///         <value>
        ///             $EXAMPLE_7GroupName
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="201">
        ///     <header name="Location" cref="string">
        ///         <description>Contains the URL pointing to the resource representing the group which has been successfully created</description>
        ///     </header>
        ///     <see cref="Group"/>
        ///     successful response
        ///     <example name="Group">
        ///         <value>
        ///             $EXAMPLE_9Group
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The payload is invalid
        ///     <example name="Invalid payload">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidPayload
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/groups",HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
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

            Group result = _groupHandler.GetGroup(id);
            string jsonResult = JsonSerializer.SerializeJson(result);

            context.Response.Headers.Set("Location", ApiBase.API_V1 + "/groups/" + id);
            context.Response.Payload.Write(jsonResult);
            context.Response.Status = HttpStatus.Created;
        }

        /// <summary>
        /// Returns the group having the specified ID
        /// </summary>
        /// <group>Group</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/groups/{groupId}</url>
        /// <param name="groupId" cref="int" in="path">The ID of the group</param>
        /// <response code="200">
        ///     <see cref="Group"/>
        ///     successful response
        ///     <example name="Group">
        ///         <value>
        ///             $EXAMPLE_9Group
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
        ///     The group having the passed ID does not exist
        ///     <example name="Group not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgGroupNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/groups/{groupId}", HttpMethod.GET)]
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
        /// Changes the name of the group having the specified ID
        /// </summary>
        /// <group>Group</group>
        /// <verb>PUT</verb>
        /// <url>pseudo://localhost/api/v1/groups/{groupId}</url>
        /// <param name="groupId" cref="int" in="path">The ID of the group</param>
        /// <param name="payload" in="body" required="false">
        ///     <see cref="Group"/>
        ///     The name of the group
        ///     <example name="Group name">
        ///         <value>
        ///             $EXAMPLE_7GroupName
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="200">
        ///     <see cref="Group"/>
        ///     The response contains the updated group
        ///     <example name="Group">
        ///         <value>
        ///             $EXAMPLE_9Group
        ///         </value>
        ///     </example>
        /// </response>
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
        ///     The group having the passed ID does not exist
        ///     <example name="Group not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgGroupNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/groups/{groupId}", HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void SetGroup(HttpContext context, string groupId)
        {       
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                Group group = JsonSerializer.DeserializeJson<Group>(json);

                int id = ApiBase.ParseId(groupId);
                if (group == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                else
                {
                    _groupHandler.ChangeName(id, group.Name);

                    Group response = _groupHandler.GetGroup(id);
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
        /// Deletes the group having the specified ID
        /// </summary>
        /// <group>Group</group>
        /// <verb>DELETE</verb>
        /// <url>pseudo://localhost/api/v1/groups/{groupId}</url>
        /// <param name="groupId" cref="int" in="path">The ID of the group</param>
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
        [Path(ApiBase.API_V1 + "/groups/{groupId}",HttpMethod.DELETE)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void DeleteGroup(HttpContext context, string groupId)
        {
            int id = ApiBase.ParseId(groupId);
            _groupHandler.DeleteGroup(id);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Returns the list of LEDs which are member of the group having the specified ID
        /// </summary>
        /// <group>Group</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/groups/{groupId}/leds</url>
        /// <param name="groupId" cref="int" in="path">The ID of the group</param>
        /// <response code="200">
        ///     <see cref="GroupLeds"/>
        ///     successful response
        ///     <example name="Group LEDs">
        ///         <value>
        ///             $EXAMPLE_8GroupLeds
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
        ///     The group having the passed ID does not exist
        ///     <example name="Group not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgGroupNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/groups/{groupId}/leds",HttpMethod.GET)]
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
        /// Note that the specified LEDs must exist, otherwise LEDs, which do not exist, will not be added.
        /// </summary>
        /// <group>Group</group>
        /// <verb>PUT</verb>
        /// <url>pseudo://localhost/api/v1/groups/{groupId}/leds</url>
        /// <param name="groupId" cref="int" in="path">The ID of the group</param>
        /// <param name="payload" in="body" required="true">
        ///     <see cref="GroupLeds"/>
        ///     List of LEDs
        ///     <example name="List of LEDs">
        ///         <value>
        ///             $EXAMPLE_10GroupLEDsModification
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="200">
        ///     <see cref="GroupLeds"/>
        ///     The response contains the updated list of LEDs
        ///     <example name="Group LEDs">
        ///         <value>
        ///             $EXAMPLE_8GroupLeds
        ///         </value>
        ///     </example>
        /// </response>
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
        ///     The group having the passed ID does not exist
        ///     <example name="Group not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgGroupNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/groups/{groupId}/leds", HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.GROUP_WRITE)]
        public void SetLedsOfGroup(HttpContext context, string groupId)
        {
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                GroupLeds groupLeds = JsonSerializer.DeserializeJson<GroupLeds>(json);

                int id = ApiBase.ParseId(groupId);
                if (groupLeds == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                _groupHandler.SetLedsOfGroup(id, groupLeds);

                GroupLeds response = _groupHandler.GetGroupLeds(id);
                string jsonRespone = JsonSerializer.SerializeJson(groupLeds);
                context.Response.Payload.Write(jsonRespone);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_PAYLOAD_EXPECTED);
            }

        }

        /// <summary>
        /// Sets the color of all LEDs being member of the group having the specified ID
        /// </summary>
        /// <group>Group</group>
        /// <verb>POST</verb>
        /// <url>pseudo://localhost/api/v1/groups/{groupId}/leds</url>
        /// <param name="groupId" cref="int" in="path">The ID of the group</param>
        /// <param name="payload" in="body" required="true">
        ///     <see cref="RgbValue"/>
        ///     The prefered color of the LEDs
        ///     <example name="RGB value">
        ///         <value>
        ///             $EXAMPLE_2RgbValue
        ///         </value>
        ///     </example>
        /// </param>
        ///  <response code="200">successful</response>
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
        ///     The group having the passed ID does not exist
        ///     <example name="Group not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgGroupNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/groups/{groupId}/leds", HttpMethod.POST)]
        [AuthorizationScope(Scopes.LED_WRITE)]
        public void SetColorOfGroup(HttpContext context, string groupId)
        {
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                RgbValue rgbValue = JsonSerializer.DeserializeJson<RgbValue>(json);

                int id = ApiBase.ParseId(groupId);
                if (rgbValue == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                _groupHandler.SetColorOfGroup(id, rgbValue.Rgb);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_PAYLOAD_EXPECTED);
            }
        }
    }
}
