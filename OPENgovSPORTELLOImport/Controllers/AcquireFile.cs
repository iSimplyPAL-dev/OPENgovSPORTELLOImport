using ImportInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using log4net;
using System.IO;

namespace OPENgovSPORTELLOImport
{
    /// <summary>
    /// 
    /// </summary>
    public class AcquireFileController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LineParser));
        private TributiModel myResult;
        HttpContext httpContext = HttpContext.Current;
        /// <summary>
        /// 
        /// </summary>
        public AcquireFileController()
        {
            this.myResult = new TributiModel();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TributiModel Get()
        {
            try
            {
                Log.Debug("AcquireFile.Get.chiamato");
                if (this.httpContext.Request.Headers["Token"] != null)
                {
                    string CFPIVA = HttpContext.Current.Request.QueryString["CFPIVA"];

                    string TokenHeader = this.httpContext.Request.Headers["Token"];
                    string IdEnte, TypeFornitore, PathFile;//letti da validation token
                    IdEnte = TypeFornitore = PathFile = "";
                    
                    var validation = new MyToken().ValidateToken(TokenHeader, new MyToken().ReasonImport, new MyToken().UserImport, out IdEnte, out TypeFornitore, out PathFile);
                    if (validation.Validated)
                    {
                        Log.Debug("AcquireFile.Get.parametri->IdEnte=" + IdEnte + ",PathFile=" + PathFile + ",CFPIVA=" + CFPIVA);
                        return GetDati(IdEnte, PathFile, CFPIVA);
                    }
                    else
                    {
                        Log.Debug("AcquireFile.Get.NoValid." + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(TokenHeader).ToArray()));
                        foreach(MyToken.TokenValidationStatus myErr in  validation.Errors)
                        {
                            Log.Debug("AcquireFile.Get.NoValid.status->" + myErr.ToString());
                        }
                        return new TributiModel { Stato = "203 Non-Authoritative Information" };
                    }
                }
                else
                {
                    Log.Debug("AcquireFile.Get.202 Method o Token errati");
                    return new TributiModel { Stato = "202 Method o Token errati" };
                }
            }
            catch 
            {
                Log.Debug("AcquireFile.Get.catch");
                return new TributiModel { Stato = "202 Method o Token errati" };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CFPIVA"></param>
        /// <returns></returns>
        private TributiModel GetDati(string IdEnte, string PathFile, string CFPIVA)
        {
            var ctx = HttpContext.Current;
            TributiModel myItem = new TributiModel { Stato = "200 OK" };
            if (ctx != null)
            {
                myItem.Anagrafica = new LineParser().LoadAnagrafica(IdEnte, PathFile, CFPIVA);
                if (myItem.Anagrafica != null)
                {
                    myItem.Dich8852 = new LineParser().LoadDich8852(IdEnte, PathFile, myItem.Anagrafica.CodIndividuale);
                    Log.Debug("SortelloIMPORT::AcquireFile::LoadDich8852_fatta");

                    myItem.Pag8852 = new LineParser().LoadPag8852(IdEnte, PathFile, myItem.Anagrafica.CodIndividuale);
                    Log.Debug("SortelloIMPORT::AcquireFile::LoadPag8852_fatta");
                    List<string> ListKeyUI = new List<string>();
                    myItem.Dich0434 = new LineParser().LoadDich0434(IdEnte, PathFile, myItem.Anagrafica.CodIndividuale, ref ListKeyUI);
                    Log.Debug("SortelloIMPORT::AcquireFile::LoadDich0434_fatta");
                    myItem.RidEse0434 = new LineParser().LoadDichRid0434(IdEnte, PathFile, ListKeyUI);
                    Log.Debug("SortelloIMPORT::AcquireFile::LoadDichRid0434_fatta");
                    myItem.Avvisi0434 = new LineParser().LoadAvvisi0434(IdEnte, PathFile, myItem.Anagrafica.CodIndividuale);
                    Log.Debug("SortelloIMPORT::AcquireFile:LoadAvvisi0434_fatta");
                    myItem.Rate0434 = new LineParser().LoadRate0434(IdEnte, PathFile, myItem.Anagrafica.CodIndividuale);
                    Log.Debug("SortelloIMPORT::AcquireFile::LoadRate0434_fatta");
                    myItem.Pag0434 = new LineParser().LoadPag0434(IdEnte, PathFile, myItem.Anagrafica.CodIndividuale);
                    Log.Debug("SortelloIMPORT::AcquireFile::LoadPag0434_fatta");
                }
                else
                {
                    myItem.Anagrafica = new LineParser().LoadDemografico(IdEnte, PathFile, CFPIVA);
                }
            }
            return myItem;
        }
    }
}