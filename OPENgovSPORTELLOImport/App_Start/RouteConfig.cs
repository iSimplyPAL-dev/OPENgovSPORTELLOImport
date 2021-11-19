using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OPENgovSPORTELLOImport
{
    /// <summary>
    /// 
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MyToken {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MyToken));
        public string UserImport = "t7I4aRn56O18m2-3";
        public string UserConvert = "540E62i_csR7o18V";
        public string UserAvgTimes = "gt1eI9a-6Vm54203";
        public string ReasonImport = "IMPORT";
        public string ReasonConvert = "CONVERT";
        public string ReasonAvgTimes = "AVGTIM";

        public string GenerateToken(string reason,string IdEnte, string PathFile)
        {
            //byte[] _time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] _IdEnte = Guid.Parse(IdEnte).ToByteArray();
            byte[] _PathFile = Encoding.ASCII.GetBytes(PathFile);
            byte[] _reason = Encoding.ASCII.GetBytes(reason);
            byte[] data = new byte[/*_time.Length + */_IdEnte.Length + _reason.Length + _PathFile.Length];

            /*System.Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            System.Buffer.BlockCopy(_key, 0, data, _time.Length, _key.Length);
            System.Buffer.BlockCopy(_reason, 0, data, _time.Length + _key.Length, _reason.Length);
            System.Buffer.BlockCopy(_PathFile, 0, data, _time.Length + _key.Length + _reason.Length, _PathFile.Length);*/

            return Convert.ToBase64String(data.ToArray());
        }
        public TokenValidation ValidateToken(string token, string reason, string user, out string IdEnte, out string Fornitore, out string PathFile)
        {
            var result = new TokenValidation();
            IdEnte = string.Empty; Fornitore = string.Empty; PathFile = string.Empty;
                DateTime paramWhen = DateTime.UtcNow.AddYears(-1);
            string paramUser, paramReason, paramIdEnte, paramFornitore, paramPathFile;
            try
            {
                paramUser = paramReason = paramIdEnte = paramFornitore = paramPathFile = string.Empty;

                byte[] data = Convert.FromBase64String(token);
                string[] ListParam = System.Text.Encoding.UTF8.GetString(data.ToArray()).Split(char.Parse("|"));
                if (ListParam.Count()!=6) {
                    Log.Debug("MyToke.ValidateToken.errore.split non con 6 posizioni");
                    result.Errors.Add(TokenValidationStatus.WrongToken);
                }
                else
                {
                    paramWhen = DateTime.Parse(ListParam[0]);
                    paramUser = ListParam[1];
                    paramIdEnte = ListParam[2];
                    paramReason = ListParam[3];
                    paramFornitore = ListParam[4];
                    paramPathFile = ListParam[5];
                }

                if (paramWhen < DateTime.UtcNow.AddHours(-24))
                {
                    Log.Debug("MyToke.ValidateToken.Error.Expired->" + paramWhen + " - to->" + DateTime.UtcNow.AddHours(-24).ToString());
                    result.Errors.Add(TokenValidationStatus.Expired);
                }
                if (user != paramUser)
                {
                    Log.Debug("MyToke.ValidateToken.Error.WrongGuid->" + paramUser + " - to->" + user);
                    result.Errors.Add(TokenValidationStatus.WrongGuid);
                }
                if (reason != paramReason)
                {
                    Log.Debug("MyToke.ValidateToken.Error.WrongPurpose->" + paramReason + " - to->" + reason);
                    result.Errors.Add(TokenValidationStatus.WrongPurpose);
                }
                IdEnte = paramIdEnte;
                Fornitore = paramFornitore;
                PathFile = paramPathFile;
            }
            catch (Exception ex)
            {
                Log.Debug("MyToke.ValidateToken.errore.", ex);
                result.Errors.Add(TokenValidationStatus.WrongToken);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public class TokenValidation
        {
            public bool Validated { get { return Errors.Count == 0; } }
            public readonly List<TokenValidationStatus> Errors = new List<TokenValidationStatus>();
        }
        /// <summary>
        /// 
        /// </summary>
        public enum TokenValidationStatus
        {
            Expired,
            WrongUser,
            WrongPurpose,
            WrongGuid,
            WrongToken
        }
    }
}
