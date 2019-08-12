using skotstein.app.ledserver.businesslayer;
using skotstein.app.ledserver.persistent;
using skotstein.app.ledserver.restlayer;
using skotstein.app.ledserver.tools;
using skotstein.net.http.oauth;
using skotstein.net.http.oauth.filestorage;
using skotstein.net.http.oauth.webkit;
using SKotstein.Net.Http.Service;
using System;
using System.Collections.Generic;

namespace skotstein.app.ledserver
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

            controllerStorage.Initialize(settings.ControllerPath);
            firmwareStorage.Initialize(settings.FirmwarePath);
            ledStorage.Initialize();
            groupStorage.Initialize(settings.GroupPath);

            //prepare OAuth storage
            ClientAccountFileStorage clientStorage = new ClientAccountFileStorage();
            UserAccountFileStorage userStorage = new UserAccountFileStorage();
            RefreshTokenFileStorage refreshTokenStorage = new RefreshTokenFileStorage();
            clientStorage.Initialize(settings.ClientPath);
            refreshTokenStorage.Initialize(settings.RefreshTokenPath);
            userStorage.Initialize(settings.UserPath);

            //prepare handler
            ControllerHandler controllerHandler = new ControllerHandler(controllerStorage,groupStorage,ledStorage);
            FirmwareHandler firmwareHandler = new FirmwareHandler(firmwareStorage, settings);
            LedHandler ledHandler = new LedHandler(ledStorage, controllerStorage);
            GroupHandler groupHandler = new GroupHandler(groupStorage, ledStorage);
            TrunkEndpoint trunkEndpoint = new TrunkEndpoint(controllerStorage, ledStorage);

            //prepare endpoints
            BaseController baseController = new BaseController();
            ControllerController controllerController = new ControllerController(controllerHandler);
            ControllerTrunk controllerTrunk = new ControllerTrunk(trunkEndpoint);
            FirmwareController firmwareController = new FirmwareController(firmwareHandler);
            LedController ledController = new LedController(ledHandler);
            GroupController groupController = new GroupController(groupHandler);

            HttpService service = new DefaultHttpSysService(false, "+", settings.ServerPort);
            service.AddController(baseController);
            service.AddController(controllerController);
            service.AddController(controllerTrunk, "Trunk", true);
            service.AddController(firmwareController);
            service.AddController(ledController);
            service.AddController(groupController);

            service.GetProcessorPostManipulation(DefaultHttpSysService.INTERNAL_PROCESSING_GROUP).Add(new OptionsPayloadInjector("OptionsInjector"));

            //prepare OAuth
            OAuth2 oauth = new OAuth2("fwehnvd3432nfre7r834nfsfiu43kmvrew!");
            oauth.ClientCredentialsAuthorization = new ClientCredentialsAuthorization(oauth, clientStorage);
            oauth.PasswordAuthorization = new PasswordAuthorization(oauth, clientStorage, userStorage);
            oauth.RefreshTokenAuthorization = new RefreshTokenAuthorization(oauth, clientStorage, refreshTokenStorage);
            service.GetProcessorPreManipulation(false).Add(oauth.AccessTokenValidator);
          

            AuthController authorizationController = new AuthController(oauth);
            service.AddController(authorizationController);

            //prepare OAuth Webkit
            IScopeHandler scopeHandler = new ScopeHandler(service, oauth);
            IClientHandler clientHandler = new ClientHandler(clientStorage);
            IUserHandler userHandler = new UserHandler(clientStorage, userStorage);
            service.AddController(new ClientController(clientHandler));
            service.AddController(new ScopeController(scopeHandler));
            service.AddController(new UserController(userHandler));

            //add dynamic scopes
            ClientController.AssignScopesToEndpoints(skotstein.app.ledserver.tools.Scopes.AUTH_CLIENT_READ, skotstein.app.ledserver.tools.Scopes.AUTH_CLIENT_WRITE, oauth);
            ScopeController.AssignScopeToEndpoint(skotstein.app.ledserver.tools.Scopes.AUTH_SCOPE_READ, oauth);
            UserController.AssignScopesToEndpoints(skotstein.app.ledserver.tools.Scopes.AUTH_USER_READ, skotstein.app.ledserver.tools.Scopes.AUTH_USER_WRITE,oauth);

            //start server
            Console.WriteLine(service.Routes);
            service.Start();
            Console.WriteLine("Service is listening on port:"+settings.ServerPort);
            Console.WriteLine("Press key to terminate");
            Console.ReadKey();

          



            

        }
    }
}
