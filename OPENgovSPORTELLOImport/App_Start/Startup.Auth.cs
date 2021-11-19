using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using OPENgovSPORTELLOImport.Providers;
using OPENgovSPORTELLOImport.Models;
using System.Text;

namespace OPENgovSPORTELLOImport
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

         /// <summary>
        /// Per ulteriori informazioni sulla configurazione dell'autenticazione, visitare http://go.microsoft.com/fwlink/?LinkId=301864
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configurare il contesto di database e la gestione utenti per l'utilizzo di un'unica istanza per richiesta
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Consentire all'applicazione di utilizzare un cookie per memorizzare informazioni relative all'utente connesso
            // e per memorizzare temporaneamente le informazioni relative a un utente che accede tramite un provider di accesso di terze parti
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configurare l'applicazione per il flusso basato su OAuth
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In modalità di produzione impostare AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Consentire all'applicazione di utilizzare token di connessione per autenticare gli utenti
            app.UseOAuthBearerTokens(OAuthOptions);

            // Rimuovere il commento dalle seguenti righe per abilitare l'accesso con provider di accesso di terze parti
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomeutente"></param>
        /// <returns></returns>
        public string autenticazione(string nomeutente)
        {
            try
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                nomeutente = encoding.GetString(Convert.FromBase64String(nomeutente));

                int separator = nomeutente.IndexOf(':');
                string name = nomeutente.Substring(0, separator);
                string password = nomeutente.Substring(separator + 1);

                var dbContext = new Microsoft.AspNet.Identity.EntityFramework.IdentityDbContext("SportelloContext");
                var userStore = new Microsoft.AspNet.Identity.EntityFramework.UserStore<Microsoft.AspNet.Identity.EntityFramework.IdentityUser>(dbContext);
                var userManager = new UserManager<Microsoft.AspNet.Identity.EntityFramework.IdentityUser>(userStore);
                Microsoft.AspNet.Identity.EntityFramework.IdentityUser myU = userManager.FindByName(name);
                if (myU == null)
                {
                    return "201 Utente non presente";
                }
                if (userManager.CheckPassword<Microsoft.AspNet.Identity.EntityFramework.IdentityUser, string>(myU, password))
                {
                    return "200 OK";
                }
                else
                {
                    return "202 Password errata";
                }
            }
            catch 
            {
                return "203 KO";
            }
        }
    }
}
