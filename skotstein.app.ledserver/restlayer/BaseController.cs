﻿// MIT License
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

using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using skotstein.app.ledserver.documentation;
using skotstein.app.ledserver.exceptions;
using skotstein.app.ledserver.model;
using skotstein.app.ledserver.tools;
using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.restlayer
{

    public class BaseController : HttpController
    {

        private bool _isOAuthEnabled;

        public BaseController(bool isOAuthEnabled)
        {
            _isOAuthEnabled = isOAuthEnabled;
        }

        /// <summary>
        /// Serves as a central entry point for this API.
        /// The method returns an instance of <see cref="Base"/> converted into JSON. 
        /// </summary>
        /// <param name="context"></param>
        [SKotstein.Net.Http.Attributes.Path(ApiBase.API_V1,SKotstein.Net.Http.Context.HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetBase(HttpContext context)
        {
            context.Response.Payload.Write(JsonSerializer.SerializeJson(new Base() { IsOauthEnabled = _isOAuthEnabled }));
            context.Response.Status = HttpStatus.OK;
        }

        [SKotstein.Net.Http.Attributes.Path(ApiBase.API_V1 + "/docs", SKotstein.Net.Http.Context.HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetDocs(HttpContext context)
        {
            IList<Hyperlink> hyperlinks = new List<Hyperlink>();

            Hyperlink hyperlinkSchema = new Hyperlink(ApiBase.API_V1 + "/docs/schemas");
            Hyperlink hyperlinkOpenApi = new Hyperlink(ApiBase.API_V1 + "/docs/openapi");
            hyperlinks.Add(hyperlinkSchema);
            hyperlinks.Add(hyperlinkOpenApi);

            string json = JsonSerializer.SerializeJson(hyperlinks);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [SKotstein.Net.Http.Attributes.Path(ApiBase.API_V1 + "/docs/schemas", SKotstein.Net.Http.Context.HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetSchemas(HttpContext context)
        {
            IList<Hyperlink> schemas = new List<Hyperlink>();
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + AuthBase.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Base.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Controller.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + ControllerFirmware.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + ControllerLeds.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Controllers.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + ErrorMessage.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Firmware.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + FirmwareCollection.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Group.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + GroupLeds.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Groups.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Hyperlink.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Led.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + Leds.SCHEMA_NAME));
            schemas.Add(new Hyperlink(ApiBase.API_V1 + "/docs/schemas/" + RgbValue.SCHEMA_NAME));


            string json = JsonSerializer.SerializeJson(schemas);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        [SKotstein.Net.Http.Attributes.Path(ApiBase.API_V1+"/docs/schemas/{name}",HttpMethod.GET)]
        [ContentType("application/schema+json")]
        public void GetSchema(HttpContext context, string name)
        {
            JSchemaGenerator jSchemaGenerator = new JSchemaGenerator();
            string s = null;

            switch (name.ToLower())
            {
                case AuthBase.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(AuthBase)).ToString();
                    break;
                case Base.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Base)).ToString();
                    break;
                case Controller.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Controller)).ToString();
                    break;
                case ControllerFirmware.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(ControllerFirmware)).ToString();
                    break;
                case ControllerLeds.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(ControllerLeds)).ToString();
                    break;
                case Controllers.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Controllers)).ToString();
                    break;
                case ErrorMessage.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(ErrorMessage)).ToString();
                    break;
                case Firmware.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Firmware)).ToString();
                    break;
                case FirmwareCollection.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(FirmwareCollection)).ToString();
                    break;
                case Group.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Group)).ToString();
                    break;
                case GroupLeds.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(GroupLeds)).ToString();
                    break;
                case Groups.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Groups)).ToString();
                    break;
                case Hyperlink.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Hyperlink)).ToString();
                    break;
                case Led.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Led)).ToString();
                    break;
                case Leds.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(Leds)).ToString();
                    break;
                case RgbValue.SCHEMA_NAME:
                    s = jSchemaGenerator.Generate(typeof(RgbValue)).ToString();
                    break;
                default:
                    throw new ResourceNotFoundException(ResourceNotFoundException.MSG_SCHEMA_NOT_FOUND);
            }
            context.Response.Payload.Write(s);
            context.Response.Status = HttpStatus.OK;
        }

        [SKotstein.Net.Http.Attributes.Path(ApiBase.API_V1+"/docs/openapi",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        public void GetOpenApiDocumentation(HttpContext context)
        {
            if (File.Exists(OpenApiDocumentBuilder.OPEN_API_DOC))
            {
                context.Response.Payload.Write(File.ReadAllText(OpenApiDocumentBuilder.OPEN_API_DOC));
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                throw new ResourceNotFoundException("Not found");
            }

            /*
            string accept = "";
            string path = "";
            string contentType = "";
            if (context.Request.Headers.Has("Accept"))
            {
                accept = context.Request.Headers.Get("Accept");
            }
            if (accept.ToLower().Contains("application/yaml"))
            {
                path = @".\OpenApi.yaml";
                contentType = "application/yaml";
            }
            else
            {
                path = @".\OpenApi.json";
                contentType = MimeType.APPLICATION_JSON;
            }
            if (File.Exists(path))
            {
                context.Response.Payload.Write(File.ReadAllText(path));
                context.Response.Headers.Set("Content-Type", contentType);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                throw new ResourceNotFoundException("Not found");
            }
            */
        } 
    }
}
