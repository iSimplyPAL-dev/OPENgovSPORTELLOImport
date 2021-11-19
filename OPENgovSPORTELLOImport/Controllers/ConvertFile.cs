using ImportInterface;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OPENgovSPORTELLOImport
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertFileController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LineParser));
        private string myResult;
        HttpContext httpContext = HttpContext.Current;
        /// <summary>
        /// 
        /// </summary>
        public ConvertFileController()
        {
            this.myResult = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            if (this.httpContext.Request.Headers["Token"] != null)
            {
                string NameFile = HttpContext.Current.Request.QueryString["NameFile"];
                string TokenHeader = this.httpContext.Request.Headers["Token"];
                string IdEnte, TypeFornitore, PathFile;//letti da validation token
                IdEnte = TypeFornitore=PathFile = "";
                var validation = new MyToken().ValidateToken(TokenHeader, new MyToken().ReasonConvert, new MyToken().UserConvert, out IdEnte, out TypeFornitore, out PathFile);                
                if (validation.Validated)
                {
                    Log.Debug("ConvertFile.Get.parametri->IdEnte=" + IdEnte + ",PathFile=" + PathFile + ",TypeFornitore=" + TypeFornitore);
                    string controller = "";
                    string fileName = "";
                    string[] ListFiles = Directory.GetFiles(PathFile.ToString() + IdEnte + "\\");
                    foreach (string myItem in ListFiles)
                    {
                        fileName = myItem.Replace(PathFile + IdEnte + "\\", "");
                        switch (TypeFornitore.ToUpper())
                        {
                            case "HALLEY":
                                controller = GetConvertFromHalley(IdEnte, PathFile, fileName);
                                break;
                            case "STUDIOK":
                                controller = GetConvertFromStudiok(IdEnte, PathFile, fileName);
                                break;
                            case "APSYSTEM":
                                controller = GetConvertFromHalley(IdEnte, PathFile, fileName);
                                break;
                        }
                        if (controller != "200 OK")
                        {
                            break;
                        }
                    }
                    if (controller != "200 OK")
                    {
                        return "204 Writing failed";
                    }
                    else
                    {
                        return "200 OK";
                    }
                }
                else
                {
                    return "203 Non-Authoritative Information";
                }
            }
            else
            {
                return "202 Method o Token errati";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        private string GetConvertFromHalley(string IdEnte, string PathFile, string NameFile)
        {
            var ctx = HttpContext.Current;
            string myItem = "200 OK";
            if (ctx != null)
            {
                switch (NameFile.ToUpper())
                {
                    case TypeFile.FileAnagrafica:
                        myItem = new ConvertFromHalley().Anagrafica(IdEnte, PathFile);
                        break;
                    case TypeFile.FileDich8852:
                        myItem = new ConvertFromHalley().Dich8852(IdEnte, PathFile);
                        break;
                    case TypeFile.FilePag8852:
                        myItem = new ConvertFromHalley().Pag8852(IdEnte, PathFile);
                        break;
                    case TypeFile.FileDich0434:
                        myItem = new ConvertFromHalley().Dich0434(IdEnte, PathFile);
                        break;
                    case TypeFile.FileAvvisi0434:
                        myItem = new ConvertFromHalley().Avvisi0434(IdEnte, PathFile);
                        break;
                    case TypeFile.FileRate0434:
                        myItem = new ConvertFromHalley().Rate0434(IdEnte, PathFile);
                        break;
                    default:
                        break;
                }
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        private string GetConvertFromStudiok(string IdEnte, string PathFile, string NameFile)
        {
            var ctx = HttpContext.Current;
            string myItem = "200 OK";
            if (ctx != null)
            {
                switch (NameFile.ToUpper())
                {
                    case TypeFile.FileAnagrafica:
                        myItem = new ConvertFromStudioK().Anagrafica(IdEnte, PathFile);
                        break;
                    case TypeFile.Stradario:
                        break;
                    case TypeFile.FileDich8852:
                        myItem = new ConvertFromStudioK().Dich8852(IdEnte, PathFile);
                        break;
                    case TypeFile.FilePag8852:
                        myItem = new ConvertFromStudioK().Pag8852(IdEnte, PathFile);
                        break;
                    case TypeFile.FileDich0434:
                        myItem = new ConvertFromStudioK().Dich0434(IdEnte, PathFile);
                        break;
                    case TypeFile.FileAvvisi0434:
                        myItem = new ConvertFromStudioK().Avvisi0434(IdEnte, PathFile);
                        break;
                    case TypeFile.FileRate0434:
                        myItem = new ConvertFromStudioK().Rate0434(IdEnte, PathFile);
                        break;
                    default:
                        break;
                }
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        private string GetConvertFromApSystem(string IdEnte, string PathFile, string NameFile)
        {
            var ctx = HttpContext.Current;
            string myItem = "200 OK";
            if (ctx != null)
            {
                
            }
            return myItem;
        }
    }
}