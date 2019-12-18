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
using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration;
using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration.Models;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using skotstein.app.ledserver.businesslayer;
using skotstein.app.ledserver.documentation;
using skotstein.app.ledserver.persistent;
using skotstein.app.ledserver.restlayer;
using skotstein.app.ledserver.tools;
using skotstein.net.http.oauth;
using skotstein.net.http.oauth.filestorage;
using skotstein.net.http.oauth.webkit;
using SKotstein.Net.Http.Core;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace skotstein.app.ledserver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //load settings
            Settings settings = Settings.Load();
            if (settings == null)
            {
                Console.WriteLine("Invalid settings - check the Settings.json file");
                Console.ReadKey();
                return;
            }
            settings.Save();

            //prepare storage
            FileBasedControllerStorage controllerStorage = new FileBasedControllerStorage();
            FileBasedFirmwareStorage firmwareStorage = new FileBasedFirmwareStorage();
            MemoryBasedLedStorage ledStorage = new MemoryBasedLedStorage();
            FileBasedGroupStorage groupStorage = new FileBasedGroupStorage();

            //initialize storage
            controllerStorage.Initialize(settings.ControllerPath);
            firmwareStorage.Initialize(settings.FirmwarePath);
            ledStorage.Initialize();
            groupStorage.Initialize(settings.GroupPath);

            //initialize business logic (handlers)
            ControllerHandler controllerHandler = new ControllerHandler(controllerStorage, groupStorage, ledStorage);
            FirmwareHandler firmwareHandler = new FirmwareHandler(firmwareStorage, settings);
            LedHandler ledHandler = new LedHandler(ledStorage, controllerStorage);
            GroupHandler groupHandler = new GroupHandler(groupStorage, ledStorage);
            TrunkEndpoint trunkEndpoint = new TrunkEndpoint(controllerStorage, ledStorage);

            //initialize API endpoints (controllers)
            BaseController baseController = new BaseController(settings.Oauth);
            ControllerController controllerController = new ControllerController(controllerHandler);
            ControllerTrunk controllerTrunk = new ControllerTrunk(trunkEndpoint);
            FirmwareController firmwareController = new FirmwareController(firmwareHandler);
            LedController ledController = new LedController(ledHandler);
            GroupController groupController = new GroupController(groupHandler);

            //initialize web server
            HttpService service = new DefaultHttpSysService(false, "+", settings.ServerPort);
            service.AddController(baseController, settings.Multithreaded);
            service.AddController(controllerController, settings.Multithreaded);
            service.AddController(controllerTrunk, "Trunk", true);
            service.AddController(firmwareController, settings.Multithreaded);
            service.AddController(ledController, settings.Multithreaded);
            service.AddController(groupController, settings.Multithreaded);
            //service.GetProcessorPostManipulation(DefaultHttpSysService.INTERNAL_PROCESSING_GROUP).Add(new OptionsPayloadInjector("OptionsInjector"));

            //if OAuth is enabled...
            if (settings.Oauth)
            {
                //prepare OAuth storage
                ClientAccountFileStorage clientStorage = new ClientAccountFileStorage();
                UserAccountFileStorage userStorage = new UserAccountFileStorage();
                RefreshTokenFileStorage refreshTokenStorage = new RefreshTokenFileStorage();

                //initialize oauth storage
                clientStorage.Initialize(settings.ClientPath);
                refreshTokenStorage.Initialize(settings.RefreshTokenPath);
                userStorage.Initialize(settings.UserPath);

                //prepare OAuth core
                OAuth2 oauth = new OAuth2("fwehnvd3432nfre7r834nfsfiu43kmvrew!");
                oauth.ClientCredentialsAuthorization = new ClientCredentialsAuthorization(oauth, clientStorage);
                oauth.PasswordAuthorization = new PasswordAuthorization(oauth, clientStorage, userStorage);
                oauth.RefreshTokenAuthorization = new RefreshTokenAuthorization(oauth, clientStorage,userStorage, refreshTokenStorage);
                service.GetProcessorPreManipulation(false).Add(new CustomizedAccessTokenValidator(oauth.AccessTokenValidator));

                //prepare OAuth Webkit
                IScopeHandler scopeHandler = new ScopeHandler(service, oauth);
                IClientHandler clientHandler = new ClientHandler(clientStorage);
                IUserHandler userHandler = new UserHandler(clientStorage, userStorage);

                //initialize API endpoints (controllers) for OAuth + Webkit
                service.AddController(new AuthController(oauth));
                service.AddController(new ClientController(clientHandler));
                service.AddController(new ScopeController(scopeHandler));
                service.AddController(new UserController(userHandler));

                //assign scopes to OAuth Webkit API endpoints
                ClientController.AssignScopesToEndpoints(skotstein.app.ledserver.tools.Scopes.AUTH_CLIENT_READ, skotstein.app.ledserver.tools.Scopes.AUTH_CLIENT_WRITE, oauth);
                ScopeController.AssignScopeToEndpoint(skotstein.app.ledserver.tools.Scopes.AUTH_SCOPE_READ, oauth);
                UserController.AssignScopesToEndpoints(skotstein.app.ledserver.tools.Scopes.AUTH_USER_READ, skotstein.app.ledserver.tools.Scopes.AUTH_USER_WRITE, oauth);
            }



            //generate YAML file
            OpenApiDocumentBuilder documentBuilder = new OpenApiDocumentBuilder();
            OpenApiDocument document = documentBuilder.Build("V1");
            if (settings.Oauth)
            {
                document = documentBuilder.AddSecuritySchema(document, new List<HttpController> { baseController, controllerController, groupController, ledController, firmwareController },/*@"http://localhost:4000/api/v1/auth/token",*/ @"http://localhost:4000/api/v1/auth/token");
            }
            string json = documentBuilder.AsJson(document);
            json = documentBuilder.Replace(json, new ExampleValues());


            json = documentBuilder.Replace(json, new HostValue("http://localhost:"+settings.ServerPort));


            File.WriteAllText(@".\OpenApi.json",json);

            //start server
            Console.WriteLine(service.Routes);
            service.Start();
            Console.WriteLine("Service is listening on port:"+settings.ServerPort);
            if (settings.Oauth)
            {
                Console.WriteLine("OAuth 2.0 is enabled");
                Console.WriteLine("Access Token URL: POST " + restlayer.ApiBase.API_V1 + "/auth/token");
            }
            else
            {
                Console.WriteLine("OAuth 2.0 is disabled");
            }
            Console.WriteLine("Press key to terminate");
            Console.ReadKey();
        }
    }
}
