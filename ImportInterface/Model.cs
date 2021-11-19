using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportInterface
{/// <summary>
/// 
/// </summary>
    public class TypeFile
    {
        public const string FileAnagrafica = "ANAGRAFE.TXT";
        public const string FileDemografico = "DEMOGRAFICI.TXT";
        public const string FileDich8852 = "DICH_8852.TXT";
        public const string FilePag8852 = "VERSAMENTI_8852.TXT";
        public const string FileDich0434 = "DICH_0434.TXT";
        public const string FileRidEse0434 = "RID_DICH_0434.TXT";
        public const string FileAvvisi0434 = "AVVISI_0434.TXT";
        public const string FileRate0434 = "RATE_0434.TXT";
        public const string FilePag0434 = "PAGAMENTI_0434.TXT";
        public const string Stradario = "STRADARIO.TXT";
        public const string FileEseDich0434 = "ESE_DICH_0434.TXT";
        public const string FileRidDich0434 = "RID_DICH_0434.TXT";
    }
    /// <summary>
    /// 
    /// </summary>
    public class TributiModel
    {
        public string Stato { get; set; }
        public Anagrafica Anagrafica { get; set; }
        public List<Dich8852> Dich8852 { get; set; }
        public List<Pag8852> Pag8852 { get; set; }
        public List<Dich0434> Dich0434 { get; set; }
        public List<RidEse0434> RidEse0434 { get; set; }
        public List<Avviso0434> Avvisi0434 { get; set; }
        public List<Rate0434> Rate0434 { get; set; }
        public List<Pag0434> Pag0434 { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Anagrafica
    {
        public string CodiceFiscale { get; set; }
        public string PartitaIva { get; set; }
        public string CodEnte { get; set; }
        public string CodIndividuale { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Sesso { get; set; }
        public string DataNascita { get; set; }
        public string DataMorte { get; set; }
        public string ComuneNascita { get; set; }
        public string ProvinciaNascita { get; set; }
        public string CodViaResidenza { get; set; }
        public string ViaResidenza { get; set; }
        public string FrazioneResidenza { get; set; }
        public string CivicoResidenza { get; set; }
        public string EsponenteCivicoResidenza { get; set; }
        public string InternoCivicoResidenza { get; set; }
        public string CapResidenza { get; set; }
        public string ComuneResidenza { get; set; }
        public string ProvinciaResidenza { get; set; }
        public string NazionalitaNascita { get; set; }
        public string CodFamiglia { get; set; }
        public List<IndirizziSpedizione> listSpedizioni { get; set; }
        public List<Contatti> Contatti { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IndirizziSpedizione
    {
        public string CognomeInvio { get; set; }
        public string ViaRCP { get; set; }
        public string CivicoRCP { get; set; }
        public string CapRCP { get; set; }
        public string ComuneRCP { get; set; }
        public string ProvinciaRCP { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Contatti
    {
        public string IdTipo { get; set; }
        public string Descrizione { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Dich8852
    {
        public string Ente { get; set; }
        public string NumeroProtocollo { get; set; }
        public DateTime DataProtocollo { get; set; }
        public string AnnoDichiarazione { get; set; }
        public List<Oggetti8852> Immobili { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Oggetti8852
    {
        public string CodUI { get; set; }
        public string CodVia { get; set; }
        public string Via { get; set; }
        public int NumeroCivico { get; set; }
        public string Scala { get; set; }
        public string Piano { get; set; }
        public string Interno { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public int Subalterno { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public int Caratteristica { get; set; }
        public string CodRendita { get; set; }
        public string Zona { get; set; }
        public string CodCategoriaCatastale { get; set; }
        public string CodClasse { get; set; }
        public decimal Rendita { get; set; }
        public decimal ValoreImmobile { get; set; }
        public decimal Consistenza { get; set; }
        public string NoteIci { get; set; }
        public int IdImmobilePertinente { get; set; }
        public Dettaglio8852 Dettaglio { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Dettaglio8852
    {
        public int IdSoggetto { get; set; }
        public decimal PercPossesso { get; set; }
        public int MesiPossesso { get; set; }
        public string TipoUtilizzo { get; set; }
        public string TipoPossesso { get; set; }
        public int AbitazionePrincipaleAttuale { get; set; }
        public int EsclusioneEsenzione { get; set; }
        public int MesiEsclusioneEsenzione { get; set; }
        public int Riduzione { get; set; }
        public int MesiRiduzione { get; set; }
        public int NumeroFigli { get; set; }
        public int NumeroUtilizzatori { get; set; }
        public bool ColtivatoreDiretto { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Pag8852
    {
        public string Ente { get; set; }
        public int ID { get; set; }
        public string Provenienza { get; set; }
        public string AnnoRiferimento { get; set; }
        public int IdAnagrafico { get; set; }
        public decimal ImportoPagato { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataRiversamento { get; set; }
        public int NumeroFabbricatiPosseduti { get; set; }
        public bool Acconto { get; set; }
        public bool Saldo { get; set; }
        public bool RavvedimentoOperoso { get; set; }
        public string CodTributo { get; set; }
        public decimal ImportoImposta { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Dich0434
    {
        public string sEnte { get; set; }
        public int IdContribuente { get; set; }
        public string sNDichiarazione { get; set; }
        public DateTime tDataDichiarazione { get; set; }
        public List<Dettaglio0434> Immobili { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Dettaglio0434
    {
        public string CodImmobile { get; set; }
        public int ProgImmobile { get; set; }
        public string sCodVia { get; set; }
        public string sVia { get; set; }
        public string sCivico { get; set; }
        public string sScala { get; set; }
        public string sInterno { get; set; }
        public string sFoglio { get; set; }
        public string sNumero { get; set; }
        public string sSubalterno { get; set; }
        public DateTime tDataFine { get; set; }
        public DateTime tDataInizio { get; set; }
        public double nMQCatasto { get; set; }
        public string sNoteUI { get; set; }
        public List<Oggetto0434> Oggetti { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class Oggetto0434
    {
        public string IdTipoVano { get; set; }
        public double nMq { get; set; }
        public string IdCategoria { get; set; }
        public string IdCatTARES { get; set; }
        public int nNC { get; set; }
        public int nNCPV { get; set; }
        public bool bIsEsente { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RidEse0434
    {
        public string sNDichiarazione { get; set; }
        public string CodImmobile { get; set; }
        public int ProgImmobile { get; set; }
        public DateTime tDataInizio { get; set; }
        public string Codice { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Avviso0434
    {
        public string IdEnte { get; set; }
        public int IdContribuente { get; set; }
        public string sAnnoRiferimento { get; set; }
        public string sCodiceCartella { get; set; }
        public DateTime tDataEmissione { get; set; }
        public double impArrotondamento { get; set; }
        public double impCarico { get; set; }
        public double impDovuto { get; set; }
        public List<DetVociAvviso0434> DetVoci { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DetVociAvviso0434
    {
        public string sCapitolo { get; set; }
        public double impDettaglio { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Rate0434
    {
        public string IdEnte { get; set; }
        public int IdContribuente { get; set; }
        public string sCodiceCartella { get; set; }
        public DateTime tDataEmissione { get; set; }
        public string sNRata { get; set; }
        public DateTime tDataScadenza { get; set; }
        public List<DetRata0434> DetRata { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DetRata0434
    {
        public string Tributo { get; set; }
        public decimal Importo { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Pag0434
    {
        public string IdEnte { get; set; }
        public int IdContribuente { get; set; }
        public string sNumeroAvviso { get; set; }
        public string sAnno { get; set; }
        public string sProvenienza { get; set; }
        public DateTime tDataAccredito { get; set; }
        public DateTime tDataPagamento { get; set; }
        public string sSegno { get; set; }
        public double dImportoPagamento { get; set; }
        public DateTime DataEmissione { get; set; }
        public string NumeroRata { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TypePeriodo
    {
        public const int _30_ggPrima = 1;
        public const int _30_15_ggPrima = 2;
        public const int _15_0_ggPrima = 3;
        public const int _0_15_ggDopo = 4;
        public const int _15_30_ggDopo = 5;
        public const int _30_60_ggDopo = 6;
        public const int _60_90_ggDopo = 7;
        public const int _90_180_ggDopo = 8;
        public const int _entro_1_Anno = 9;
        public const int _oltre_1_Anno = 10;
    }
    /// <summary>
    /// 
    /// </summary>
    public class TempiMediModel
    {
        public string Stato { get; set; }
        public List<TempiMediPagamento> TempiMediPagamento { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TempiMediPagamento
    {
        public string IdEnte { get; set; }
        public string NumeroRata { get; set; }
        public DateTime DataEmissione { get; set; }
        public DateTime DataScadenza { get; set; }
        public int TipoPeriodo { get; set; }
        public int ContatoreAvvisi { get; set; }
        public double SommatoriaPagato { get; set; }
        public string CodTributo { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Scadenze
    {
        public DateTime DataEmissione { get; set; }
        public string NumeroRata { get; set; }
        public DateTime DataScadenza { get; set; }
    }
}
