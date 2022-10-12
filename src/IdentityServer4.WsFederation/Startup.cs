using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace IdentityServer4.WsFederation
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var cert = new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "idsrvtest.pfx"), "idsrv3test");
            
            services.AddIdentityServer()
                .AddSigningCredential(cert)
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(TestUsers.Users)
                .AddWsFederation();

            services.AddAuthentication()
                .AddGoogle("Google", "Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = "708996912208-9m4dkjb5hscn7cjrn5u0r4tbgkbj1fko.apps.googleusercontent.com";
                    options.ClientSecret = "wdfPY6t8H8cecgjlxud__4Gh";
                })
                .AddOpenIdConnect("ACM-IDM", "ACM-IDM", options =>
                {
                    options.Authority = "https://authenticatie-ti.vlaanderen.be/op";
                    options.CallbackPath = "/auth/login/callback";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.TokenValidationParameters.SaveSigninToken = true;
                    options.RequireHttpsMetadata = false;
                    options.ProtocolValidator.RequireNonce = true;
                    
                    options.Scope.Add("dv_oauthdemo_rola");
                    options.Scope.Add("vo");
                    options.Scope.Add("offline_access");

                    options.SignedOutCallbackPath = "/auth/logout/callback";
                    options.SignedOutRedirectUri = "/";
                    options.UseTokenLifetime = true;
                    
                    options.MetadataAddress = "https://authenticatie-ti.vlaanderen.be/op/.well-known/openid-configuration";
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = "3a1aad1c-7c93-4483-8ee5-e37bfcf7b3d6";
                    options.ClientSecret = "B4O2xRysdGX9BopZMWDMBatZN0XkoWAJLw4HnlC59-fX6aoqTraMhYmyqj-6oAisPUJFLrzL8peh4fT_avQS_hwunoNrV_V9";
                    
                    options.ClaimActions.Add(new RoleClaimAction());
                });

           // services.AddTransient<IClaimsTransformation, RoleClaimAugmentation>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            
            app.UseStaticFiles();

            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}