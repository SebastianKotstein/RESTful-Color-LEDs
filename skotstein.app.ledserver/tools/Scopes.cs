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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.tools
{
    public class Scopes
    {
        public const string LED_READ = "led-read";
        public const string LED_WRITE = "led-write";
        public const string GROUP_READ = "group-read";
        public const string GROUP_WRITE = "group-write";
        public const string CONTROLLER_READ = "controller-read";
        public const string CONTROLLER_WRITE = "controller-write";
        public const string FIRMWARE_READ = "firmware-read"; //handle with care as this scope grants access to the wifi credentials!!
        public const string FIRMWARE_WRITE = "firmware-write";
        public const string AUTH_CLIENT_READ = "auth-client-read";
        public const string AUTH_CLIENT_WRITE = "auth-client-write";
        public const string AUTH_SCOPE_READ = "auth-scope-read";
        public const string AUTH_USER_READ = "auth-user-read";
        public const string AUTH_USER_WRITE = "auth-user-write";
    }
}
