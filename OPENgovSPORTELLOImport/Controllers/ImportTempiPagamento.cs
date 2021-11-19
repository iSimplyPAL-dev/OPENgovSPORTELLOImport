using ImportInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OPENgovSPORTELLOImport
{
    /// <summary>
    /// 
    /// </summary>
    public class ImportTempiPagamentoController : ApiController
    {
        private TributiModel myResult;
        HttpContext httpContext = HttpContext.Current;
        /// <summary>
        /// 
        /// </summary>
        public ImportTempiPagamentoController()
        {
            this.myResult = new TributiModel();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TempiMediModel Get()
        {
            if (this.httpContext.Request.Headers["Token"] != null)
            {
                string IDTRIBUTO = HttpContext.Current.Request.QueryString["IDTRIBUTO"];

                string TokenHeader = this.httpContext.Request.Headers["Token"];
                string IdEnte, TypeFornitore, PathFile; //letti da validation token
                IdEnte = TypeFornitore = PathFile = "";

                var validation = new MyToken().ValidateToken(TokenHeader, new MyToken().ReasonAvgTimes, new MyToken().UserAvgTimes, out IdEnte, out TypeFornitore, out PathFile);

                if (validation.Validated)
                {
                    return GetDati(IdEnte, PathFile, IDTRIBUTO);
                }
                else
                {
                    return new TempiMediModel { Stato = "203 Non-Authoritative Information" };
                }
            }
            else
            {
                return new TempiMediModel { Stato = "202 Method o Token errati" };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="IDTRIBUTO"></param>
        /// <returns></returns>
        private TempiMediModel GetDati(string IdEnte, string PathFile, string IDTRIBUTO)
        {
            try
            {
                var ctx = HttpContext.Current;
                TempiMediModel myTempiMediModel = new TempiMediModel();
                List<Scadenze> myScadenzeList0434 = new List<Scadenze>();
                List<Scadenze> myScadenzeList8852 = new List<Scadenze>();
                List<Pag0434> myDatePagamentoList0434 = new List<Pag0434>();
                List<Pag8852> myDatePagamentoList8852 = new List<Pag8852>();
                List<TempiMediPagamento> myTempiMediPagamento0434 = new List<TempiMediPagamento>();
                List<TempiMediPagamento> myTempiMediPagamento8852 = new List<TempiMediPagamento>();

                if (ctx != null)
                {
                    //carico l'oggetto con le scadenze
                    myScadenzeList0434 = new LineParser().LoadRate0434TempiPagamenti(IdEnte, PathFile);
                    myScadenzeList8852 = new LineParser().LoadRate8852TempiPagamenti();

                    //carico l'oggetto con le date di pagamento
                    //devo passare l'oggetto lista scadenze per poter contare il numero rate
                    myDatePagamentoList0434 = new LineParser().LoadPag0434TempiPagamenti(IdEnte, PathFile, myScadenzeList0434);
                    myDatePagamentoList8852 = new LineParser().LoadPag8852TempiPagamenti(IdEnte, PathFile, myScadenzeList8852);

                    //ottengo la lista dei tempi di pagamento
                    myTempiMediPagamento0434 = new CalcoloTempi().LoadTempiMedi0434(IdEnte, myScadenzeList0434, myDatePagamentoList0434);
                    myTempiMediPagamento8852 = new CalcoloTempi().LoadTempiMedi8852(IdEnte, myScadenzeList8852, myDatePagamentoList8852);
                    
                    //restituisco il codice 200 OK al completamento delle richieste
                    myTempiMediModel.Stato = "200 OK";
                    
                    //unisco le liste
                    List<TempiMediPagamento> myListCompleta = new List<TempiMediPagamento>();
                    myListCompleta.AddRange(myTempiMediPagamento0434);
                    myListCompleta.AddRange(myTempiMediPagamento8852);

                    //assegno l alista completa per il return
                    myTempiMediModel.TempiMediPagamento = myListCompleta;
                }
                //ritorno l'oggetto che contiene tutto
                return myTempiMediModel;
            }
            catch 
            {
                return new TempiMediModel { Stato = "300 Lettura File Fallita" };
            }
        }
    }
}