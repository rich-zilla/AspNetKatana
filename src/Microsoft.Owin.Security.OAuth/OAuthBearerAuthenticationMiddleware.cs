﻿// <copyright file="OAuthBearerAuthenticationMiddleware.cs" company="Microsoft Open Technologies, Inc.">
// Copyright 2011-2013 Microsoft Open Technologies, Inc. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;

namespace Microsoft.Owin.Security.OAuth
{
    public class OAuthBearerAuthenticationMiddleware : AuthenticationMiddleware<OAuthBearerAuthenticationOptions>
    {
        private readonly ILogger _logger;

        private readonly string _challenge;

        public OAuthBearerAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            OAuthBearerAuthenticationOptions options) : base(next, options)
        {
            _logger = app.CreateLogger<OAuthBearerAuthenticationMiddleware>();

            if (string.IsNullOrWhiteSpace(options.Realm))
            {
                _challenge = "Bearer";
            }
            else
            {
                _challenge = "Bearer realm=\"" + options.Realm + "\"";
            }

            if (options.AccessTokenHandler == null)
            {
                var dataProtecter = app.CreateDataProtector(
                    typeof(OAuthBearerAuthenticationMiddleware).Namespace,
                    "Access Token");
                options.AccessTokenHandler = new TicketDataHandler(dataProtecter);
            }
        }

        protected override AuthenticationHandler<OAuthBearerAuthenticationOptions> CreateHandler()
        {
            return new OAuthBearerAuthenticationHandler(_logger, _challenge);
        }
    }
}
