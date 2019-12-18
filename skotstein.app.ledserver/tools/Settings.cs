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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.tools
{

    /// <summary>
    /// An instance of <see cref="Settings"/> holds all general application specific parameters, e.g. the specified <see cref="Culture"/> which is relevant for the UI language
    /// or the last loaded project path. For storing these settings persistently, an instance of this class can be serialized in/deserialized from an appropriate JSON structure stored within a file.
    /// Use <see cref="Load"/> and <see cref="Save"/> for loading and storing the settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Contains the version of the application.
        /// </summary>
        public const string SETTINGS_FILE_PATH = "settings.json";

        private string _wifiSsid;
        private string _wifiPwd;
        private string _targetUrl;
        private int _clientTimeout;

        private int _serverPort;
        private bool _multithreaded;

        private string _firmwarePath;
        private string _controllerPath;
        private string _groupPath;

        private bool _oauth;
        private string _clientPath;
        private string _refreshTokenPath;
        private string _userPath;

        /// <summary>
        /// Enables or disables HATEOAS. If HATEOAS is enabled (recommended), the response of an API call contains hyperlinks pointing to related resources 
        /// and available actions. Disabling HATEOAS is not recommended as it vialotes the corresponding REST-constraint and the underlying principles for a hyper-driven API design.
        /// Hence, this flag should only be used for debugging or by using an additional HATEOAS wrapper. 
        /// </summary>
        public const bool ENABLE_HATEOAS = true;


        [JsonProperty("wifiSsid")]
        public string WifiSsid
        {
            get
            {
                return _wifiSsid;
            }

            set
            {
                _wifiSsid = value;
            }
        }

        [JsonProperty("wifiPwd")]
        public string WifiPwd
        {
            get
            {
                return _wifiPwd;
            }

            set
            {
                _wifiPwd = value;
            }
        }

        [JsonProperty("targetUrl")]
        public string TargetUrl
        {
            get
            {
                return _targetUrl;
            }

            set
            {
                _targetUrl = value;
            }
        }

        [JsonProperty("client_timeout")]
        public int ClientTimeout
        {
            get
            {
                return _clientTimeout;
            }

            set
            {
                _clientTimeout = value;
            }
        }

        [JsonProperty("server_port")]
        public int ServerPort
        {
            get
            {
                return _serverPort;
            }

            set
            {
                _serverPort = value;
            }
        }

        [JsonProperty("multithreaded")]
        public bool Multithreaded
        {
            get
            {
                return _multithreaded;
            }
            set
            {
                _multithreaded = value;
            }
        }

        [JsonProperty("path_firmware")]
        public string FirmwarePath
        {
            get
            {
                return _firmwarePath;
            }

            set
            {
                _firmwarePath = value;
            }
        }

        [JsonProperty("path_controller")]
        public string ControllerPath
        {
            get
            {
                return _controllerPath;
            }

            set
            {
                _controllerPath = value;
            }
        }

        [JsonProperty("path_group")]
        public string GroupPath
        {
            get
            {
                return _groupPath;
            }

            set
            {
                _groupPath = value;
            }
        }

        [JsonProperty("client_path")]
        public string ClientPath
        {
            get
            {
                return _clientPath;
            }

            set
            {
                _clientPath = value;
            }
        }

        [JsonProperty("refresh_token_path")]
        public string RefreshTokenPath
        {
            get
            {
                return _refreshTokenPath;
            }

            set
            {
                _refreshTokenPath = value;
            }
        }

        [JsonProperty("user_path")]
        public string UserPath
        {
            get
            {
                return _userPath;
            }

            set
            {
                _userPath = value;
            }
        }

        [JsonProperty("oauth")]
        public bool Oauth
        {
            get
            {
                return _oauth;
            }

            set
            {
                _oauth = value;
            }
        }

        public Settings()
        {
        }

        /// <summary>
        /// Saves this instance to file. The default path is specified in <see cref="SETTINGS_FILE_PATH"/>.
        /// </summary>
        public void Save()
        {
            string json = JsonSerializer.SerializeJson(this);
            File.WriteAllText(Settings.SETTINGS_FILE_PATH, json);
        }

        /// <summary>
        /// Loads an instance of <see cref="Settings"/> from file. The default path is specified in <see cref="SETTINGS_FILE_PATH"/>.
        /// </summary>
        /// <returns></returns>
        public static Settings Load()
        {
            try
            {
                string json = File.ReadAllText(Settings.SETTINGS_FILE_PATH);
                return JsonSerializer.DeserializeJson<Settings>(json);
            }
            catch (Exception e)
            {
                return new Settings();
            }
        }

    }
}
