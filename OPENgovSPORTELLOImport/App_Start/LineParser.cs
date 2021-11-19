using ImportInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using log4net;

namespace OPENgovSPORTELLOImport
{
    /// <summary>
    /// 
    /// </summary>
    public class LineParser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LineParser));
        /// <summary>
        /// ANAGRAFE.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CFPIVA"></param>
        /// <returns></returns>
        public Anagrafica LoadAnagrafica(string IdEnte, string PathFile, string CFPIVA)
        {
            string line;
            int MaxLength = 624;
            Anagrafica myItem = null;
            string LineRead = string.Empty;

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileAnagrafica);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace(((Char)65533), char.Parse(" "));
                        if (line.Trim() == string.Empty)
                            continue;
                        if (line.Length == MaxLength)
                        {
                            if (line.Substring(6, 16).Trim().ToUpper() == CFPIVA.ToUpper() || line.Substring(22, 11).Trim().ToUpper() == CFPIVA.ToUpper())
                            {
                                myItem = new Anagrafica();

                                int myStart, myLength;
                                myStart = myLength = 0;

                                myItem.CodEnte = IdEnte;

                                myLength = 6;
                                myItem.CodIndividuale = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 16;
                                myItem.CodiceFiscale = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 11;
                                myItem.PartitaIva = line.Substring(myStart, myLength).Trim();
                                myItem.PartitaIva = (myItem.PartitaIva == "00000000000" ? string.Empty : myItem.PartitaIva);
                                myStart += myLength;
                                myLength = 100;
                                myItem.Cognome = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                myItem.Nome = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 1;
                                myItem.Sesso = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 8;
                                myItem.DataNascita = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 8;
                                myItem.DataMorte = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myItem.ComuneNascita = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                myItem.ProvinciaNascita = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 12;
                                myItem.CodViaResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myItem.ViaResidenza = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                myItem.FrazioneResidenza = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                myItem.CivicoResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 3;
                                myItem.EsponenteCivicoResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 3;
                                myItem.InternoCivicoResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 5;
                                myItem.CapResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myItem.ComuneResidenza = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                myItem.ProvinciaResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                List<IndirizziSpedizione> list = new List<IndirizziSpedizione>();
                                IndirizziSpedizione sped = new IndirizziSpedizione();
                                sped.CognomeInvio = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                sped.ViaRCP = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                sped.CivicoRCP = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 5;
                                sped.CapRCP = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                sped.ComuneRCP = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                sped.ProvinciaRCP = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                if (sped.CognomeInvio != string.Empty)
                                {
                                    list.Add(sped);
                                    myItem.listSpedizioni = list;
                                }
                                myLength = 20;
                                if (line.Substring(myStart, myLength).Trim() != string.Empty)
                                {
                                    myItem.Contatti = new List<Contatti>();
                                    Contatti mio = new Contatti { IdTipo = "3", Descrizione = line.Substring(myStart, myLength).Trim() };
                                    myItem.Contatti.Add(mio);
                                    myStart += myLength;
                                }
                                LineRead = line;
                                break;
                            }
                        }
                        else
                        {
                            Log.Debug("LoadAnagrafica.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di "+line.Length.ToString());
                        }
                    }
                }
                else
                {
                    Log.Debug("LoadAnagrafica.Errore.Mancanza presenza flusso->"+ PathFile + TypeFile.FileAnagrafica);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("ImportAnagraficaTrib.errore::", ex);
            }
            return myItem;
        }
         /// <summary>
        ///  DEMOGRAFICI.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CFPIVA"></param>
        /// <returns></returns>
        public Anagrafica LoadDemografico(string IdEnte, string PathFile, string CFPIVA)
        {
            string line;
            int MaxLength = 356;
            Anagrafica myItem = null;
            string LineRead = string.Empty;

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileDemografico);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace(((Char)65533), char.Parse(" "));
                        if (line.Trim() == string.Empty)
                            continue;
                        if (line.Length == MaxLength)
                        {
                            if (line.Substring(306, 16).Trim() == CFPIVA)
                            {
                                myItem = new Anagrafica();

                                int myStart, myLength;
                                myStart = myLength = 0;

                                myItem.CodEnte = IdEnte;

                                myLength = 7;
                                myItem.CodIndividuale = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myItem.Nome = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 100;
                                myItem.Cognome = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 8;
                                myItem.DataNascita = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 2;
                                myItem.ProvinciaNascita = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myItem.ComuneNascita = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 20;                                //ToponimoViaResidenza
                                myStart += myLength;
                                myLength = 12;
                                myItem.CodViaResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myItem.ViaResidenza = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 4;
                                myItem.CivicoResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 3;
                                myItem.EsponenteCivicoResidenza = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 16;
                                myItem.CodiceFiscale = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 1;
                                myItem.Sesso = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 20;
                                myItem.NazionalitaNascita = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 3;                                //IdParentela 
                                myStart += myLength;
                                myLength = 9;
                                myItem.CodFamiglia = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 1;                                //TipoScheda 
                                myStart += myLength;
                            }
                        }
                        else
                        {
                            Log.Debug("LoadDemografico.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di " + line.Length.ToString());
                        }

                    }
                }
            }
            catch 
            {
            }
            return myItem;
        }
        /// <summary>
        /// DICH_8852.TXT               controllato, ok come da tracciato v3
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CodSoggetto"></param>
        /// <returns></returns>
        public List<Dich8852> LoadDich8852(string IdEnte, string PathFile, string CodSoggetto)
        {
            string line;
            int MaxLength = 536;
            List<Dich8852> myList = new List<Dich8852>();
            List<Dich8852> myListRet = new List<Dich8852>();
            List<string> LineRead = new List<string>();
            DateTime myData = DateTime.MinValue;
            int myInt = 0;
            decimal myDecimal = 0;

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileDich8852);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace(((Char)65533), char.Parse(" "));
                        if (line.Trim() == string.Empty)
                            continue;
                        if (line.Length == MaxLength)
                        {
                            if (line.Substring(68, 6).Trim() == CodSoggetto)
                            {
                                Dich8852 myItem = new Dich8852();
                                Oggetti8852 myOggetti = new Oggetti8852();
                                Dettaglio8852 myDettaglio = new Dettaglio8852();

                                int myStart, myLength;
                                myStart = myLength = 0;

                                myItem.Ente = IdEnte;

                                myLength = 50;
                                myItem.NumeroProtocollo = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 8;
                                myData = DateTime.MinValue;
                                if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                {
                                    try
                                    {
                                        myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                            "yyyyMMdd",
                                                            System.Globalization.CultureInfo.InvariantCulture,
                                                            System.Globalization.DateTimeStyles.None);
                                    }
                                    catch
                                    { }
                                }
                                if (myData == DateTime.MinValue)
                                    myData = DateTime.Parse("1900-01-01");
                                myItem.DataProtocollo = myData;
                                myItem.AnnoDichiarazione = myItem.DataProtocollo.Year.ToString();
                                myStart += myLength;
                                myLength = 10;
                                myOggetti.CodUI = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 6;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.IdSoggetto = myInt;
                                myStart += myLength;
                                myLength = 12;
                                myOggetti.CodVia = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myOggetti.Via = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myOggetti.NumeroCivico = myInt;
                                myStart += myLength;
                                myLength = 2;
                                myOggetti.Scala = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 5;
                                myOggetti.Piano = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 3;
                                myOggetti.Interno = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 4;
                                myOggetti.Foglio = line.Substring(myStart, myLength).Trim();
                                myOggetti.Foglio = TrimRifCat(myOggetti.Foglio);
                                myStart += myLength;
                                myLength = 5;
                                myOggetti.Numero = line.Substring(myStart, myLength).Trim();
                                myOggetti.Numero = TrimRifCat(myOggetti.Numero);
                                myStart += myLength;
                                myLength = 4;
                                //prendo solo i caratteri numerici
                                myInt = int.Parse(new string(line.Substring(myStart, myLength).Trim().Where(x => char.IsDigit(x)).ToArray()));
                                myOggetti.Subalterno = myInt;
                                myStart += myLength;
                                myLength = 8;
                                myData = DateTime.MinValue;
                                if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                {
                                    try
                                    {
                                        myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                            "yyyyMMdd",
                                                            System.Globalization.CultureInfo.InvariantCulture,
                                                            System.Globalization.DateTimeStyles.None);
                                    }
                                    catch
                                    { }
                                }
                                if (myData == DateTime.MinValue)
                                    myData = DateTime.Parse("1900-01-01");
                                myOggetti.DataInizio = myData;
                                myStart += myLength;
                                myLength = 8;
                                myData = DateTime.MinValue;
                                if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                {
                                    try
                                    {
                                        myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                            "yyyyMMdd",
                                                            System.Globalization.CultureInfo.InvariantCulture,
                                                            System.Globalization.DateTimeStyles.None);
                                    }
                                    catch
                                    { }
                                }
                                if (myData == DateTime.MinValue)
                                    myData = DateTime.MaxValue;
                                if (myData.Year >= DateTime.Now.Year)
                                    myData = DateTime.MaxValue;
                                myOggetti.DataFine = myData;
                                myStart += myLength;
                                myLength = 2;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.MesiPossesso = myInt;
                                myStart += myLength;
                                myLength = 2;
                                myDettaglio.TipoPossesso = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 2; //tipoutilizzo
                                myDettaglio.TipoUtilizzo = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 5;
                                decimal.TryParse(line.Substring(myStart, myLength).Trim(), out myDecimal);
                                myDettaglio.PercPossesso = myDecimal / 100;
                                myStart += myLength;
                                myLength = 1;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.AbitazionePrincipaleAttuale = myInt;
                                myStart += myLength;
                                myLength = 1;//flag pertinenza?
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myOggetti.IdImmobilePertinente = myInt;
                                  myStart += myLength;
                                myLength = 2;
                                int IdCaratteristica = 3;
                                myOggetti.CodRendita = "RE";
                                if (line.Substring(myStart, myLength).Trim() == "TA" || line.Substring(myStart, myLength).Trim() == "05")
                                { IdCaratteristica = 1; myOggetti.CodRendita = "TA"; }
                                else if (line.Substring(myStart, myLength).Trim() == "AF" || line.Substring(myStart, myLength).Trim() == "04")
                                { IdCaratteristica = 2; myOggetti.CodRendita = "AF"; }
                                //inserita gestione libri contabili, scelto 4
                                else if (line.Substring(myStart, myLength).Trim() == "LC")
                                { IdCaratteristica = 4; myOggetti.CodRendita = "LC"; }
                                 myOggetti.Caratteristica = IdCaratteristica;
                                myStart += myLength;
                                myLength = 10;
                                myOggetti.Zona = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 10;
                                //categoria
                                myOggetti.CodCategoriaCatastale = (line.Substring(myStart, myLength).Trim() != "" ? line.Substring(myStart, myLength).Trim() : "NR");
                                myStart += myLength;
                                myLength = 10;
                                myOggetti.CodClasse = (line.Substring(myStart, myLength).Trim() != "" ? line.Substring(myStart, myLength).Trim() : "0");
                                myStart += myLength;
                                myLength = 16;
                                decimal.TryParse(line.Substring(myStart, myLength).Trim(), out myDecimal);
                                myOggetti.Rendita = myDecimal / 100;
                                myStart += myLength;
                                myLength = 16;
                                decimal.TryParse(line.Substring(myStart, myLength).Trim(), out myDecimal);
                                myOggetti.ValoreImmobile = myDecimal / 100;
                                myStart += myLength;
                                myLength = 8;
                                decimal.TryParse(line.Substring(myStart, myLength).Trim(), out myDecimal);
                                myOggetti.Consistenza = myDecimal / 100;
                                myStart += myLength;
                                myLength = 1;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.EsclusioneEsenzione = myInt; //verificare se gestito al contrario
                                myStart += myLength;
                                myLength = 2;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.MesiEsclusioneEsenzione = myInt;
                                myStart += myLength;
                                myLength = 1;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.Riduzione = myInt;//verificare se gestito al contrario
                                myStart += myLength;
                                myLength = 2;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.MesiRiduzione = myInt;
                                myStart += myLength;
                                myLength = 2;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.NumeroFigli = myInt;
                                myStart += myLength;
                                myLength = 2;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myDettaglio.NumeroUtilizzatori = myInt;
                                myStart += myLength;
                                myLength = 1;
                                myDettaglio.ColtivatoreDiretto = (line.Substring(myStart, myLength).Trim() == "1" ? true : false);
                                myStart += myLength;
                                myLength = 255;
                                myOggetti.NoteIci = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;

                                myOggetti.Dettaglio = new Dettaglio8852();
                                myOggetti.Dettaglio = myDettaglio;
                                myItem.Immobili = new List<Oggetti8852>();
                                myItem.Immobili.Add(myOggetti);
                                myList.Add(myItem);

                                LineRead.Add(line);
                            }
                        }
                        else
                        {
                            Log.Debug("LoadDich8852.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di " + line.Length.ToString());
                        }

                    }
                }
                Dich8852 myPrec = new Dich8852();
                foreach (Dich8852 myDich in myList)
                {
                    if (myPrec.AnnoDichiarazione == string.Empty || myPrec.AnnoDichiarazione == null)
                        myListRet.Add(myDich);
                    else
                    {
                        if (myDich.AnnoDichiarazione == myPrec.AnnoDichiarazione && myDich.DataProtocollo == myPrec.DataProtocollo && myDich.NumeroProtocollo == myPrec.NumeroProtocollo)
                        {
                            myListRet[myListRet.Count - 1].Immobili.Add(myDich.Immobili[0]);
                        }
                        else
                            myListRet.Add(myDich);
                    }
                    myPrec = myDich;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("ImportAnagraficaTrib.errore::", ex);
            }
            return myListRet;
        }
       /// <summary>
        /// VERSAMENTI_8852.TXT         controllato, ok come da tracciato v3
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CodSoggetto"></param>
        /// <returns></returns>
        public List<Pag8852> LoadPag8852(string IdEnte, string PathFile, string CodSoggetto)
        {
            {
                string line;
                int MaxLength = 70;
                List<Pag8852> myList = new List<Pag8852>();
                List<string> LineRead = new List<string>();
                DateTime myData = DateTime.MinValue;
                int myInt = 0;
                decimal myDecimal = 0;

                try
                {
                    byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FilePag8852);
                    if (PostedFile != null)
                    {
                        MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                        StreamReader sr = new StreamReader(ms);
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Replace(((Char)65533), char.Parse(" "));
                            if (line.Trim() == string.Empty)
                                continue;
                            if (line.Length == MaxLength)
                            {
                                if (line.Substring(13, 6).Trim() == CodSoggetto)
                                {
                                    Pag8852 myItem = new Pag8852();
                                    Oggetti8852 myOggetti = new Oggetti8852();
                                    Dettaglio8852 myDettaglio = new Dettaglio8852();

                                    int myStart, myLength;
                                    myStart = myLength = 0;

                                    myItem.Ente = IdEnte;
                                    myLength = 9;
                                    int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                    myItem.ID = myInt;
                                    myStart += myLength;
                                    myLength = 4;
                                    myItem.AnnoRiferimento = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 6;
                                    int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                    myItem.IdAnagrafico = myInt;
                                    myStart += myLength;
                                    myLength = 10;
                                    myItem.Provenienza = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                    myStart += myLength;
                                    myLength = 8;
                                    myData = DateTime.MinValue;
                                    if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                    {
                                        try
                                        {
                                            myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                "yyyyMMdd",
                                                                System.Globalization.CultureInfo.InvariantCulture,
                                                                System.Globalization.DateTimeStyles.None);
                                        }
                                        catch
                                        { }
                                    }
                                    if (myData == DateTime.MinValue)
                                        myData = DateTime.Parse("1900-01-01");
                                    myItem.DataRiversamento = myData;
                                    myStart += myLength;
                                    myLength = 8;
                                    myData = DateTime.MinValue;
                                    if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                    {
                                        try
                                        {
                                            myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                "yyyyMMdd",
                                                                System.Globalization.CultureInfo.InvariantCulture,
                                                                System.Globalization.DateTimeStyles.None);
                                        }
                                        catch
                                        { }
                                    }
                                    if (myData == DateTime.MinValue)
                                        myData = DateTime.Parse("1900-01-01");
                                    myItem.DataPagamento = myData;
                                    myStart += myLength;
                                    myLength = 2;
                                    int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                    myItem.NumeroFabbricatiPosseduti = myInt;
                                    myStart += myLength;
                                    myLength = 1;
                                    myItem.Acconto = (line.Substring(myStart, myLength).Trim() == "1" ? true : false);
                                    myStart += myLength;
                                    myLength = 1;
                                    myItem.Saldo = (line.Substring(myStart, myLength).Trim() == "1" ? true : false);
                                    myStart += myLength;
                                    myLength = 1;
                                    myItem.RavvedimentoOperoso = (line.Substring(myStart, myLength).Trim() == "1" ? true : false);
                                    myStart += myLength;
                                    myLength = 4;
                                    myItem.CodTributo = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 16;
                                    decimal.TryParse(line.Substring(myStart, myLength).Trim(), out myDecimal);
                                    myItem.ImportoPagato = myDecimal / 100;
                                    myItem.ImportoImposta = myDecimal / 100;
                                    myList.Add(myItem);

                                    LineRead.Add(line);
                                }
                            }
                            else
                            {
                                Log.Debug("LoadPag8852.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di " + line.Length.ToString());
                            }
                        }
                    }
                }
                catch 
                {
                }
                return myList;
            }
        }
         /// <summary>
        /// DICH_0434.TXT               controllato, ok come da tracciato v3
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CodSoggetto"></param>
        /// <param name="ListKeyUI"></param>
        /// <returns></returns>
        public List<Dich0434> LoadDich0434(string IdEnte, string PathFile, string CodSoggetto, ref List<string> ListKeyUI)
        {
            string line;
            int MaxLength = 510;
            List<Dich0434> myList = new List<Dich0434>();
            List<Dich0434> myListRet = new List<Dich0434>();
            List<string> LineRead = new List<string>();
            DateTime myData = DateTime.MinValue;
            int myInt = 0;
            double myDouble = 0;
            ListKeyUI = new List<string>();

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileDich0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace(((Char)65533), char.Parse(" "));
                        if (line.Trim() == string.Empty)
                            continue;
                        if (line.Length == MaxLength)
                        {      
                            if (line.Substring(53, 6).Trim() == CodSoggetto)
                            {
                                Dich0434 myItem = new Dich0434();
                                Dettaglio0434 myDettaglio = new Dettaglio0434();
                                Oggetto0434 myOggetti = new Oggetto0434();

                                int myStart, myLength;
                                myStart = myLength = 0;

                                myItem.sEnte = IdEnte;
                                myLength = 30;
                                myItem.sNDichiarazione = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 8;
                                myData = DateTime.MinValue;
                                if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                {
                                    try
                                    {
                                        myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                            "yyyyMMdd",
                                                            System.Globalization.CultureInfo.InvariantCulture,
                                                            System.Globalization.DateTimeStyles.None);
                                    }
                                    catch
                                    { }
                                }
                                if (myData == DateTime.MinValue)
                                    myData = DateTime.Parse("1900-01-01");
                                myItem.tDataDichiarazione = myData;
                                myStart += myLength;
                                myLength = 10;
                                myDettaglio.CodImmobile = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 5;
                                myDettaglio.ProgImmobile = int.Parse(line.Substring(myStart, myLength).Trim());
                                ListKeyUI.Add(myItem.sNDichiarazione + "|" + myDettaglio.CodImmobile + "|" + line.Substring(myStart, myLength).Trim());
                                myStart += myLength;
                                myLength = 6;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myItem.IdContribuente = myInt;
                                myStart += myLength;
                                myLength = 12;
                                myDettaglio.sCodVia = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 50;
                                myDettaglio.sVia = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                myDettaglio.sCivico = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 2;
                                myDettaglio.sScala = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 5; //piano
                                myStart += myLength;
                                myLength = 3;
                                myDettaglio.sInterno = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 4;
                                myDettaglio.sFoglio = line.Substring(myStart, myLength).Trim();
                                myDettaglio.sFoglio = TrimRifCat(myDettaglio.sFoglio);
                                myStart += myLength;
                                myLength = 5;
                                myDettaglio.sNumero = line.Substring(myStart, myLength).Trim();
                                myDettaglio.sNumero = TrimRifCat(myDettaglio.sNumero);
                                myStart += myLength;
                                myLength = 4;
                                myDettaglio.sSubalterno = line.Substring(myStart, myLength).Trim();
                                myDettaglio.sSubalterno = TrimRifCat(myDettaglio.sSubalterno);
                                myStart += myLength;
                                myLength = 8;
                                myData = DateTime.MinValue;
                                if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                {
                                    try
                                    {
                                        myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                            "yyyyMMdd",
                                                            System.Globalization.CultureInfo.InvariantCulture,
                                                            System.Globalization.DateTimeStyles.None);
                                    }
                                    catch 
                                    {}
                                }
                                if (myData == DateTime.MinValue)
                                    myData = DateTime.Parse("1900-01-01");
                                myDettaglio.tDataInizio = myData;
                                myStart += myLength;
                                myLength = 8;
                                myData = DateTime.MinValue;
                                if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                {
                                    try
                                    {
                                        myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                            "yyyyMMdd",
                                                            System.Globalization.CultureInfo.InvariantCulture,
                                                            System.Globalization.DateTimeStyles.None);
                                    }
                                    catch
                                    { }
                                }
                                if (myData == DateTime.MinValue)
                                    myData = DateTime.MaxValue;
                                myDettaglio.tDataFine = myData;
                                myStart += myLength;
                                myLength = 8;
                                double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                myDettaglio.nMQCatasto = myDouble / 100;
                                myStart += myLength;
                                myLength = 5;
                                myOggetti.IdTipoVano = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 8;
                                double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                myOggetti.nMq = myDouble / 100;
                                myStart += myLength;
                                myLength = 50;
                                myOggetti.IdTipoVano = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 4;
                                myOggetti.IdCategoria = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 5;
                                myOggetti.IdCatTARES = line.Substring(myStart, myLength).Trim();
                                myStart += myLength;
                                myLength = 2;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myOggetti.nNC = myInt;
                                 myStart += myLength;
                                myLength = 2;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                myOggetti.nNCPV = myInt;
                                myStart += myLength;
                                myLength = 1;
                                myOggetti.bIsEsente = (line.Substring(myStart, myLength).Trim() == "1" ? true : false);
                                myStart += myLength;
                                myLength = 255;
                                myDettaglio.sNoteUI = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                myStart += myLength;

                                if (myDettaglio.Oggetti == null)
                                    myDettaglio.Oggetti = new List<Oggetto0434>();
                                myDettaglio.Oggetti.Add(myOggetti);
                                if (myItem.Immobili == null)
                                    myItem.Immobili = new List<Dettaglio0434>();
                                myItem.Immobili.Add(myDettaglio);
                                myList.Add(myItem);

                                LineRead.Add(line);
                            }
                        }
                        else
                        {
                            string failure = "La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri.";
                        }
                    }
                    Dich0434 myPrec = new Dich0434();
                    foreach (Dich0434 myDich in myList)
                    {
                        if (myPrec.IdContribuente > 0 && myDich.sNDichiarazione == myPrec.sNDichiarazione && myDich.tDataDichiarazione == myPrec.tDataDichiarazione)
                        {
                            myPrec.Immobili.Add(myDich.Immobili[0]);
                        }
                        else
                        {
                            if (myPrec.IdContribuente > 0)
                            {
                                myListRet.Add(myPrec);
                            }
                            myPrec = myDich;
                        }
                    }
                    myListRet.Add(myPrec);
                }
            }
            catch
            {
                
            }
            return myListRet;
        }
        /// <summary>
        /// RID_DICH_0434.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="ListKeyUI"></param>
        /// <returns></returns>
        public List<RidEse0434> LoadDichRid0434(string IdEnte, string PathFile, List<string> ListKeyUI)
        {
            {
                string line;
                int MaxLength = 58;
                List<RidEse0434> myListRet = new List<RidEse0434>();
                DateTime myData = DateTime.MinValue;

                try
                {
                    byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileRidEse0434);
                    if (PostedFile != null)
                    {
                        if (ListKeyUI.Count > 0)
                        {
                            MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                            StreamReader sr = new StreamReader(ms);
                            while ((line = sr.ReadLine()) != null)
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line.Trim() == string.Empty)
                                    continue;
                                if (line.Length == MaxLength)
                                {
                                    string myItemKey = string.Empty;
                                    int myStart, myLength;
                                    myStart = myLength = 0;

                                    myLength = 30;
                                    myItemKey = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 10;
                                    myItemKey += "|" + line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 5;
                                    myItemKey += "|" + line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;

                                    foreach (string myKey in ListKeyUI)
                                    {
                                        if (myItemKey == myKey)
                                        {
                                            RidEse0434 myItem = new RidEse0434();

                                            myStart = myLength = 0;

                                            myLength = 30;
                                            myItem.sNDichiarazione = line.Substring(myStart, myLength).Trim();
                                            myStart += myLength;
                                            myLength = 10;
                                            myItem.CodImmobile = line.Substring(myStart, myLength).Trim();
                                            myStart += myLength;
                                            myLength = 5;
                                            myItem.ProgImmobile = int.Parse(line.Substring(myStart, myLength).Trim());
                                            myStart += myLength;
                                            myLength = 8;
                                            myData = DateTime.MinValue;
                                            if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                            {
                                                try
                                                {
                                                    myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                        "yyyyMMdd",
                                                                        System.Globalization.CultureInfo.InvariantCulture,
                                                                        System.Globalization.DateTimeStyles.None);
                                                }
                                                catch
                                                { }
                                            }
                                            if (myData == DateTime.MinValue)
                                                myData = DateTime.Parse("1900-01-01");
                                            myItem.tDataInizio = myData;
                                            myStart += myLength;
                                            myLength = 5;
                                            myItem.Codice = line.Substring(myStart, myLength).Trim();
                                            myStart += myLength;
                                            myListRet.Add(myItem);
                                        }
                                    }
                                }
                                else
                                {
                                    Log.Debug("LoadDichRid0434.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di " + line.Length.ToString());
                                }

                            }
                        }
                    }
                }
                catch 
                {
                }
                return myListRet;
            }
        }
        /// <summary>
        /// AVVISI_0434.TXT  
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CodSoggetto"></param>
        /// <returns></returns>
        public List<Avviso0434> LoadAvvisi0434(string IdEnte, string PathFile, string CodSoggetto)
        {
            {
                string line;
                int MaxLength = 189;
                List<Avviso0434> myList = new List<Avviso0434>();
                List<string> LineRead = new List<string>();
                DateTime myData = DateTime.MinValue;
                int myInt = 0;
                double myDouble = 0;

                try
                {
                    byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileAvvisi0434);
                    if (PostedFile != null)
                    {
                        MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                        StreamReader sr = new StreamReader(ms);
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Replace(((Char)65533), char.Parse(" "));
                            if (line.Trim() == string.Empty)
                                continue;
                            if (line.Length == MaxLength || line.Length==(MaxLength-16))
                            {
                                if (line.Substring(0, 6).Trim() == CodSoggetto)
                                {
                                    Avviso0434 myItem = new Avviso0434();
                                    DetVociAvviso0434 myVoce = new DetVociAvviso0434();
                                    myItem.DetVoci = new List<DetVociAvviso0434>();

                                    int myStart, myLength;
                                    myStart = myLength = 0;

                                    myItem.IdEnte = IdEnte;
                                    myLength = 6;
                                    int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                    myItem.IdContribuente = myInt;
                                    myStart += myLength;
                                    myLength = 4;
                                    myItem.sAnnoRiferimento = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 18;
                                    myItem.sCodiceCartella = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 8;
                                    myData = DateTime.MinValue;
                                    if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                    {
                                        try
                                        {
                                            myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                "yyyyMMdd",
                                                                System.Globalization.CultureInfo.InvariantCulture,
                                                                System.Globalization.DateTimeStyles.None);
                                        }
                                        catch
                                        { }
                                    }
                                    if (myData == DateTime.MinValue)
                                        myData = DateTime.MaxValue;
                                    myItem.tDataEmissione = myData;
                                    myStart += myLength;
                                    myLength = 16;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "0000";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 1;//filler Tipo Avviso {O=avviso Originale, S=sgravio, A=aggravio}
                                    myStart += myLength;
                                    myLength = 15;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "9986";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 16;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "9987";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 16;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "9994";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 16;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "3955";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 16;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "0096";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 1;
                                    string Segno = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 7;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "9999";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    if (Segno == "-")
                                        myVoce.impDettaglio = myVoce.impDettaglio * -1;
                                    myItem.impArrotondamento = myVoce.impDettaglio;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 16;
                                    myVoce = new DetVociAvviso0434();
                                    myVoce.sCapitolo = "SPNO";
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myVoce.impDettaglio = myDouble / 100;
                                    myItem.DetVoci.Add(myVoce);
                                    myStart += myLength;
                                    myLength = 1;
                                    Segno = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 16;
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myItem.impCarico = myDouble / 100;
                                    if (Segno == "-")
                                        myItem.impCarico = myItem.impCarico * -1;
                                    myStart += myLength;
                                    if (line.Length == MaxLength)
                                    {
                                        myLength = 16;
                                        double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                        myItem.impDovuto = myDouble / 100;
                                        myStart += myLength;
                                    }
                                    else {
                                        myItem.impDovuto = myItem.impCarico;
                                    }
                                    myList.Add(myItem);

                                    LineRead.Add(line);
                                }
                            }
                            else
                            {
                                Log.Debug("LoadAvvisi0434.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di " + line.Length.ToString());
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("LoadAvvisi0434.errore::", ex);
                }
                return myList;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CodSoggetto"></param>
        /// <returns></returns>
        public List<Rate0434> LoadRate0434(string IdEnte, string PathFile, string CodSoggetto)
        {
            {
                string line;
                int MaxLength = 66;
                List<Rate0434> myList = new List<Rate0434>();
                List<Rate0434> myListRet = new List<Rate0434>();
                List<string> LineRead = new List<string>();
                DateTime myData = DateTime.MinValue;
                int myInt = 0;
                decimal myDecimal = 0;

                try
                {
                    byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileRate0434);
                    if (PostedFile != null)
                    {
                        MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                        StreamReader sr = new StreamReader(ms);
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Replace(((Char)65533), char.Parse(" "));
                            if (line.Trim() == string.Empty)
                                continue;
                            if (line.Length == MaxLength)
                            {
                                if (line.Substring(0, 6).Trim() == CodSoggetto)
                                {
                                    Rate0434 myItem = new Rate0434();
                                    DetRata0434 myDet = new DetRata0434();

                                    int myStart, myLength;
                                    myStart = myLength = 0;

                                    myItem.IdEnte = IdEnte;
                                    myLength = 6;
                                    int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                    myItem.IdContribuente = myInt;
                                    myStart += myLength;
                                    myLength = 18;
                                    myItem.sCodiceCartella = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 4;
                                    string AnnoRiferimento = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 8;
                                    myData = DateTime.MinValue;
                                    if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                    {
                                        try
                                        {
                                            myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                "yyyyMMdd",
                                                                System.Globalization.CultureInfo.InvariantCulture,
                                                                System.Globalization.DateTimeStyles.None);
                                        }
                                        catch
                                        { }
                                    }
                                    if (myData == DateTime.MinValue)
                                        myData = DateTime.MaxValue;
                                    myItem.tDataEmissione = myData;
                                    myStart += myLength;
                                    myLength = 2;
                                    myItem.sNRata = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 8;
                                    myData = DateTime.MinValue;
                                    if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                    {
                                        try
                                        {
                                            myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                "yyyyMMdd",
                                                                System.Globalization.CultureInfo.InvariantCulture,
                                                                System.Globalization.DateTimeStyles.None);
                                        }
                                        catch
                                        { }
                                    }
                                    if (myData == DateTime.MinValue)
                                        myData = DateTime.MaxValue;
                                    myItem.tDataScadenza = myData;
                                    myStart += myLength;
                                    myLength = 4;
                                    myDet.Tributo = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 16;
                                    decimal.TryParse(line.Substring(myStart, myLength).Trim(), out myDecimal);
                                    myDet.Importo = myDecimal / 100;
                                    myItem.DetRata = new List<DetRata0434>();
                                    myItem.DetRata.Add(myDet);
                                    myList.Add(myItem);

                                    LineRead.Add(line);
                                }
                            }
                            else
                            {
                                Log.Debug("LoadRate0434.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di " + line.Length.ToString());
                            }

                        }
                        Rate0434 myPrec = new Rate0434();
                        foreach (Rate0434 myRata in myList)
                        {
                            if (myPrec.IdContribuente > 0 && myRata.sCodiceCartella == myPrec.sCodiceCartella && myRata.tDataEmissione == myPrec.tDataEmissione && myRata.sNRata == myPrec.sNRata)
                            {
                                myPrec.DetRata.Add(myRata.DetRata[0]);
                            }
                            else
                            {
                                if (myPrec.IdContribuente > 0)
                                    myListRet.Add(myPrec);
                            }
                            myPrec = myRata;
                        }
                        myListRet.Add(myPrec);
                    }
                }
                catch
                {
                }
                return myListRet;
            }
        }
        /// <summary>
        /// PAGAMENTI_0434.TXT      
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="CodSoggetto"></param>
        /// <returns></returns>
        public List<Pag0434> LoadPag0434(string IdEnte, string PathFile, string CodSoggetto)
        {
            {
                string line;
                int MaxLength = 79;
                List<Pag0434> myList = new List<Pag0434>();
                List<string> LineRead = new List<string>();
                DateTime myData = DateTime.MinValue;
                int myInt = 0;
                double myDouble = 0;

                try
                {
                    byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FilePag0434);
                    if (PostedFile != null)
                    {
                        MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                        StreamReader sr = new StreamReader(ms);
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Replace(((Char)65533), char.Parse(" "));
                            if (line.Trim() == string.Empty)
                                continue;
                            if (line.Length == MaxLength)
                            {
                                if (line.Substring(0, 6).Trim() == CodSoggetto)
                                {
                                    Pag0434 myItem = new Pag0434();

                                    int myStart, myLength;
                                    myStart = myLength = 0;

                                    myItem.IdEnte = IdEnte;
                                    myLength = 6;
                                    int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                                    myItem.IdContribuente = myInt;
                                    myStart += myLength;
                                    myLength = 18;
                                    myItem.sNumeroAvviso = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 4;
                                    myItem.sAnno = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 8;
                                    myItem.sAnno = line.Substring(myStart, 4).Trim();//prendo l'anno dalla data di avviso
                                    myStart += myLength;
                                    myLength = 10;
                                    myItem.sProvenienza = line.Substring(myStart, myLength).Trim().Replace("'", "’");
                                    myStart += myLength;
                                    myLength = 8;
                                    myData = DateTime.MinValue;
                                    if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                    {
                                        try
                                        {
                                            myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                "yyyyMMdd",
                                                                System.Globalization.CultureInfo.InvariantCulture,
                                                                System.Globalization.DateTimeStyles.None);
                                        }
                                        catch
                                        { }
                                    }
                                    if (myData == DateTime.MinValue)
                                        myData = DateTime.MaxValue;
                                    myItem.tDataAccredito = myData;
                                    myStart += myLength;
                                    myLength = 8;
                                    myData = DateTime.MinValue;
                                    if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                                    {
                                        try
                                        {
                                            myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                                "yyyyMMdd",
                                                                System.Globalization.CultureInfo.InvariantCulture,
                                                                System.Globalization.DateTimeStyles.None);
                                        }
                                        catch
                                        { }
                                    }
                                    if (myData == DateTime.MinValue)
                                        myData = DateTime.MaxValue;
                                    myItem.tDataPagamento = myData;
                                    myStart += myLength;
                                    myLength = 1;
                                    myItem.sSegno = line.Substring(myStart, myLength).Trim();
                                    myStart += myLength;
                                    myLength = 16;
                                    double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                                    myItem.dImportoPagamento = myDouble / 100;
                                    myList.Add(myItem);

                                    LineRead.Add(line);
                                }
                            }
                            else
                            {
                                Log.Debug("LoadPag0434.Errore.La lunghezza della linea non è di " + MaxLength.ToString() + " caratteri ma di " + line.Length.ToString());
                            }

                        }
                    }
                }
                catch 
                {
                }
                return myList;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RifCat"></param>
        /// <returns></returns>
        private string TrimRifCat(string RifCat)
        {
            string myRet = string.Empty;
            try
            {
                int nPos = 0;
                foreach (char myChar in RifCat)
                {
                    if (myChar != char.Parse("0"))
                    {
                        myRet = RifCat.Substring(nPos);
                        break;
                    }
                    nPos += 1;
                }
            }
            catch 
            {
                myRet = string.Empty;
            }
            return myRet;
        }

        /// <summary>
        /// funzione per il calcolo delle rate totali
        /// </summary>
        /// <param name="myListScadenze"></param>
        /// <param name="myDataEmissione"></param>
        /// <returns></returns>
        public int calcoloRateTot(List<Scadenze> myListScadenze, DateTime myDataEmissione)
        {
            try
            {
                int rateTot = 0;
                foreach (Scadenze myScadenza in myListScadenze)
                {
                    //se non coincidono esco 
                    if (myScadenza.DataEmissione == myDataEmissione)
                    {
                        //se il numero rata è U (unica rata) e le rate tot è 0
                        if (myScadenza.NumeroRata.ToUpper() == "U" && rateTot == 0) //.toupper() risolve il problema se maiucola o minuscola
                        {
                            rateTot = 1;
                        }
                        else
                        {
                            if (myScadenza.NumeroRata.ToUpper() != "U")
                            {
                                rateTot = int.Parse(myScadenza.NumeroRata);
                            }
                        }
                    }
                }
                return rateTot;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.LineParser.calcoloRateTot::errore::", ex);
                return new int();
            }
        }
        /// <summary>
        /// funzione che genera il numero rate, serve solo per 0434 perchè nel file che ci viene passato manca il numero rata
        /// </summary>
        /// <param name="myList"></param>
        /// <param name="myScadenzeList"></param>
        /// <returns></returns>
        public List<Pag0434> assegnaNumeroRate0434(List<Pag0434> myList, List<Scadenze> myScadenzeList)
        {
            try
            {
                Pag0434 myPagPrec = new Pag0434();
                int rateTot = 0;

                //per ogni pagamento effettuato (già ordinato)
                foreach (Pag0434 myTmp in myList)
                {
                    //se la data di emissione è uguale alla precedente
                    if (myTmp.DataEmissione == myPagPrec.DataEmissione)
                    {
                        //se il numero di avviso è uguale al precedente
                        if (myTmp.sNumeroAvviso == myPagPrec.sNumeroAvviso)
                        {
                            //se il numero della rata è uguale al precedente (conservato in un campo string)
                            if (myPagPrec.NumeroRata == rateTot.ToString())
                            {
                                //allora il numero rata è uguale al precedente
                                myTmp.NumeroRata = myPagPrec.NumeroRata;
                            }
                            else
                            {
                                //si somma uno al numero rata già presente
                                myTmp.NumeroRata = (int.Parse(myPagPrec.NumeroRata) + 1).ToString();
                            }
                        }
                        else
                        {
                            myPagPrec.NumeroRata = "0";
                            //si somma uno al numero rata già presente
                            myTmp.NumeroRata = (int.Parse(myPagPrec.NumeroRata) + 1).ToString();
                        }
                    }
                    else {
                        //richiamo la funzione per ottenere il numero di rate totali passando la lista delle scadenze e la data di emissione
                        rateTot = calcoloRateTot(myScadenzeList, myTmp.DataEmissione);
                        //imposto numero rate
                        myPagPrec.NumeroRata = "0";
                        //si somma uno al numero rata già presente
                        myTmp.NumeroRata = (int.Parse(myPagPrec.NumeroRata) + 1).ToString();
                    }
                    //imposto come data prec la data di emisione attuale?
                    myPagPrec.DataEmissione = myTmp.DataEmissione;
                    //numero rata prec diventa il numero rata di mytmp
                    myPagPrec.NumeroRata = myTmp.NumeroRata;
                }
                //restituisco la myList con l aggiunta del numero rata
                return myList;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.LineParser.assegnaNumeroRate::errore::", ex);
                return new List<Pag0434>();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public List<Scadenze> LoadRate0434TempiPagamenti(string IdEnte, string PathFile)
        {
            try
            {
                string line;
                int MaxLength = 66;
                List<Scadenze> myList = new List<Scadenze>();
                DateTime myData = DateTime.MinValue;
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FileRate0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace(((Char)65533), char.Parse(" "));
                        if (line.Trim() == string.Empty)
                            continue;
                        if (line.Length == MaxLength)
                        {
                            //leggo il file contenente le rate
                            Scadenze myItem = new Scadenze();

                            int myStart, myLength;
                            myStart = myLength = 0;

                            myLength = 6;
                            myStart += myLength;
                            myLength = 18;
                            myStart += myLength;
                            myLength = 4;
                            myStart += myLength;
                            myLength = 8;
                            myData = DateTime.MinValue;
                            if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                            {
                                try
                                {
                                    myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                        "yyyyMMdd",
                                                        System.Globalization.CultureInfo.InvariantCulture,
                                                        System.Globalization.DateTimeStyles.None);
                                }
                                catch
                                { }
                            }
                            if (myData == DateTime.MinValue)
                                myData = DateTime.MaxValue;
                            //prendo la data di emissione
                            myItem.DataEmissione = myData;
                            myStart += myLength;
                            myLength = 2;
                            //prendo il numero della rata
                            myItem.NumeroRata = line.Substring(myStart, myLength).Trim();
                            myStart += myLength;
                            myLength = 8;
                            myData = DateTime.MinValue;
                            if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                            {
                                try
                                {
                                    myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                        "yyyyMMdd",
                                                        System.Globalization.CultureInfo.InvariantCulture,
                                                        System.Globalization.DateTimeStyles.None);
                                }
                                catch
                                { }
                            }
                            if (myData == DateTime.MinValue)
                                myData = DateTime.MaxValue;
                            //prendo la data di scadenza
                            myItem.DataScadenza = myData;
                            myStart += myLength;
                            myLength = 4;
                            myStart += myLength;
                            myLength = 16;

                            if (myList.Count == 0)
                            {
                                myList.Add(myItem);
                            }
                            int conta = 0;
                            foreach (Scadenze myScad in myList)
                            {
                                //nelle rate non possono essercene 2 con: stessa data emissione e stesso numero rata MA con data scadenza diversa, dovrebbe esserci un numero rata differente in questo caso
                                if ((myScad.DataEmissione == myItem.DataEmissione && myScad.DataScadenza == myItem.DataScadenza && myScad.NumeroRata == myItem.NumeroRata) || (myScad.DataEmissione == myItem.DataEmissione && myScad.NumeroRata == myItem.NumeroRata))
                                {
                                    //segno se presente uno di questi casi ed esco
                                    conta++;
                                    break;
                                }
                            }
                            if (conta == 0)
                            {
                                //nel caso in cui non siano presenti doppioni allora vado ad aggiungere la riga
                                myList.Add(myItem);
                            }
                        }
                    }
                }
                return myList;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.LineParser.LoadRate0434TempiPagamenti::errore::", ex);
                return new List<Scadenze>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Scadenze> LoadRate8852TempiPagamenti()
        {
            try
            {
                List<Scadenze> myList = new List<Scadenze>();
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Scadenze myScadenzaTmp = new Scadenze();
                        myScadenzaTmp.DataEmissione = DateTime.Parse((DateTime.Now.Year - i).ToString() + "-01-01");
                        if (j == 0)
                        {
                            myScadenzaTmp.DataScadenza = DateTime.Parse((DateTime.Now.Year - i).ToString() + "-06-16");
                            myScadenzaTmp.NumeroRata = "01";
                        }
                        else if (j == 1)
                        {
                            myScadenzaTmp.DataScadenza = DateTime.Parse((DateTime.Now.Year - i).ToString() + "-12-16");
                            myScadenzaTmp.NumeroRata = "02";
                        }
                        else if (j == 2)
                        {
                            myScadenzaTmp.DataScadenza = DateTime.Parse((DateTime.Now.Year - i).ToString() + "-06-16");
                            myScadenzaTmp.NumeroRata = "0U";
                        }
                        myScadenzaTmp.DataEmissione = DateTime.Parse((DateTime.Now.Year - i).ToString() + "-01-01");
                        myList.Add(myScadenzaTmp);
                    }
                }
                return myList;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.LineParser.LoadRate8852TempiPagamenti::errore::", ex);
                return new List<Scadenze>();
            }
        }
        /// <summary>
        /// restituisce la lista dei pagamenti
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="myScadenzeList"></param>
        /// <returns></returns>
        public List<Pag0434> LoadPag0434TempiPagamenti(string IdEnte, string PathFile, List<Scadenze> myScadenzeList)
        {
            try
            {
                string line;
                int MaxLength = 79;
                //dichiaro la lista temporanea
                List<Pag0434> myList = new List<Pag0434>();
                DateTime myData = DateTime.MinValue;
                double myDouble = 0;
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FilePag0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace(((Char)65533), char.Parse(" "));
                        if (line.Trim() == string.Empty)
                            continue;
                        if (line.Length == MaxLength)
                        {
                            //leggo il file dei pagamenti
                            Pag0434 myItem = new Pag0434();

                            int myStart, myLength;
                            myStart = myLength = 0;

                            myLength = 6;
                            myStart += myLength;
                            myLength = 18;
                            myItem.sNumeroAvviso = line.Substring(myStart, myLength).Trim();
                            myStart += myLength;
                            myLength = 4;
                            myStart += myLength;
                            myLength = 8;
                            if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                            {
                                try
                                {
                                    myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                        "yyyyMMdd",
                                                        System.Globalization.CultureInfo.InvariantCulture,
                                                        System.Globalization.DateTimeStyles.None);
                                }
                                catch
                                { }
                            }
                            if (myData == DateTime.MinValue)
                                myData = DateTime.MaxValue;
                            //prendo la data di emissione
                            myItem.DataEmissione = myData;
                            myStart += myLength;
                            myLength = 10;
                            myStart += myLength;
                            myLength = 8;
                            myStart += myLength;
                            myLength = 8;
                            myData = DateTime.MinValue;
                            if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                            {
                                try
                                {
                                    myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                        "yyyyMMdd",
                                                        System.Globalization.CultureInfo.InvariantCulture,
                                                        System.Globalization.DateTimeStyles.None);
                                }
                                catch
                                { }
                            }
                            if (myData == DateTime.MinValue)
                                myData = DateTime.MaxValue;
                            //prendo la data di pagamento
                            myItem.tDataPagamento = myData;
                            myStart += myLength;
                            myLength = 1;
                            myStart += myLength;
                            myLength = 16;
                            double.TryParse(line.Substring(myStart, myLength).Trim(), out myDouble);
                            myItem.dImportoPagamento = myDouble / 100;

                            myList.Add(myItem);
                        }
                    }//chiudo il while

                    //ordino la lista appena ottenuta con i pagamenti effettuati
                    myList = myList.OrderBy(item => item.DataEmissione).ThenBy(item => item.tDataPagamento).ThenBy(item => item.sNumeroAvviso).ToList();
                    //passo la lista con i pagamenti e la lista con le rate nella segunte funzione per abbinarli
                    List<Pag0434> myLystPagamentiConRate = assegnaNumeroRate0434(myList, myScadenzeList);
                }
                return myList;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.LineParser.LoadPag0434TempiPagamenti::errore::", ex);
                return new List<Pag0434>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <param name="myScadenzeList"></param>
        /// <returns></returns>
        public List<Pag8852> LoadPag8852TempiPagamenti(string IdEnte, string PathFile, List<Scadenze> myScadenzeList)
        {
            try
            {
                string line;
                int MaxLength = 70;
                List<Pag8852> myList = new List<Pag8852>();
                DateTime myData = DateTime.MinValue;
                int myInt = 0;
                decimal myDecimal = 0;

                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + TypeFile.FilePag8852);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace(((Char)65533), char.Parse(" "));

                        if (line.Trim() == string.Empty)
                            continue;
                        if (line.Length == MaxLength)
                        {
                            Pag8852 myItem = new Pag8852();
                            int myStart, myLength;
                            myStart = myLength = 0;
                            string annoTmp;

                            myLength = 9;
                            int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                            myStart += myLength;
                            myLength = 4;
                            annoTmp = line.Substring(myStart, myLength).Trim(); //DEVO COSTRUIRE LA DATA COMPLETA AL 01/01 DI OGNI ANNO
                            myItem.AnnoRiferimento = (annoTmp + "-01-01");
                            myStart += myLength;
                            myLength = 6;
                            int.TryParse(line.Substring(myStart, myLength).Trim(), out myInt);
                            myItem.IdAnagrafico = myInt; // CODICE CONTRIBUENTE
                            myStart += myLength;
                            myLength = 10;
                            myStart += myLength;
                            myLength = 8;
                            myData = DateTime.MinValue;
                            if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                            {
                                try
                                {
                                    myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                        "yyyyMMdd",
                                                        System.Globalization.CultureInfo.InvariantCulture,
                                                        System.Globalization.DateTimeStyles.None);
                                }
                                catch
                                { }
                            }
                            if (myData == DateTime.MinValue)
                                myData = DateTime.Parse("1900-01-01");
                            myStart += myLength;
                            myLength = 8;
                            myData = DateTime.MinValue;
                            if (line.Substring(myStart, myLength).Trim() != "00000000" && line.Substring(myStart, myLength).Trim() != "")
                            {
                                try
                                {
                                    myData = DateTime.ParseExact(line.Substring(myStart, myLength).Trim(),
                                                        "yyyyMMdd",
                                                        System.Globalization.CultureInfo.InvariantCulture,
                                                        System.Globalization.DateTimeStyles.None);
                                }
                                catch
                                { }
                            }
                            if (myData == DateTime.MinValue)
                                myData = DateTime.Parse("1900-01-01");
                            myItem.DataPagamento = myData; //DATA PAGAMENTO
                            myStart += myLength;
                            myLength = 2;
                            myStart += myLength;
                            myLength = 1;
                            myItem.Acconto = (line.Substring(myStart, myLength).Trim() == "1" ? true : false); // RATA 1 
                            myStart += myLength;
                            myLength = 1;
                            myItem.Saldo = (line.Substring(myStart, myLength).Trim() == "1" ? true : false); // RATA 2 -- SE ACCONTO=1 E SALDO=1 ALLORA PAGAMENTO UNICO
                            myStart += myLength;
                            myLength = 1;
                            myStart += myLength;
                            myLength = 4;
                            myStart += myLength;
                            myLength = 16;
                            decimal.TryParse(line.Substring(myStart, myLength).Trim(), out myDecimal);
                            myItem.ImportoPagato = myDecimal / 100;
                            //aggiungo riga
                            myList.Add(myItem);
                        }
                    }//chiudo il while
                    //ordino la lista appena ottenuta con i pagamenti effettuati
                    myList = myList.OrderBy(item => item.AnnoRiferimento).ThenBy(item => item.DataPagamento).ThenBy(item => item.IdAnagrafico).ToList();
                }
                return myList;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.LineParser.LoadPag8852TempiPagamenti::errore::", ex);
                return new List<Pag8852>();
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class CalcoloTempi
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CalcoloTempi));
        public List<TempiMediPagamento> LoadTempiMedi0434(string IdEnte, List<Scadenze> myScadenzeList, List<Pag0434> myDatePagamentoList)
        {
            try
            {
                List<TempiMediPagamento> ListTempiMediPagamento = new List<TempiMediPagamento>();

                foreach (Scadenze myScadenzeTmp in myScadenzeList)
                {
                    //per 10 volte perche devo avere 10 righe per ogni elemento in scadenza, una riga per ogni TipoPeriodo
                    for (int i = 1; i < 11; i++)
                    {
                        TempiMediPagamento TempiMediPagamentoTmp = new TempiMediPagamento();
                        //carico oggetto TempiMediPagamentoTmp
                        TempiMediPagamentoTmp.IdEnte = IdEnte;
                        TempiMediPagamentoTmp.DataEmissione = myScadenzeTmp.DataEmissione;
                        TempiMediPagamentoTmp.NumeroRata = myScadenzeTmp.NumeroRata;
                        TempiMediPagamentoTmp.DataScadenza = myScadenzeTmp.DataScadenza;
                        //imposto a 0 il contatore avvisi e la sommatoria pagato
                        TempiMediPagamentoTmp.ContatoreAvvisi = 0;
                        TempiMediPagamentoTmp.SommatoriaPagato = 0;
                        //da 1 a 10
                        TempiMediPagamentoTmp.TipoPeriodo = i;
                        //aggiungo alla lista
                        ListTempiMediPagamento.Add(TempiMediPagamentoTmp);
                    }
                }
                //per ogni elemento in ListTempiMediPagamento...
                foreach (TempiMediPagamento myTempiMediTmp in ListTempiMediPagamento)
                {
                    string numeroAvvisoPrec = "";
                    //per ogni data di pagamento nella relativa lista...
                    foreach (Pag0434 myDatePagamentoTmp in myDatePagamentoList)
                    {
                        myDatePagamentoTmp.NumeroRata = myDatePagamentoTmp.NumeroRata.PadLeft(2).Replace(' ', '0');
                        myTempiMediTmp.NumeroRata = myTempiMediTmp.NumeroRata.PadLeft(2).Replace(' ', '0');

                        //se coincidono vado a lavorizzare il TipoPeriodo
                        if (myDatePagamentoTmp.DataEmissione == myTempiMediTmp.DataEmissione && myDatePagamentoTmp.NumeroRata == myTempiMediTmp.NumeroRata)
                        {
                            //da verificare corretto funzionamento .Days prende un valore int
                            int giorni = (myDatePagamentoTmp.tDataPagamento - myTempiMediTmp.DataScadenza).Days;

                            if ((giorni <= -30) && myTempiMediTmp.TipoPeriodo == TypePeriodo._30_ggPrima)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._30_ggPrima.ToString();
                            }
                            else if ((giorni > -30 && giorni <= -15) && myTempiMediTmp.TipoPeriodo == TypePeriodo._30_15_ggPrima)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._30_15_ggPrima.ToString();
                            }
                            else if ((giorni > -15 && giorni <= 0) && myTempiMediTmp.TipoPeriodo == TypePeriodo._15_0_ggPrima)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._15_0_ggPrima.ToString();
                            }
                            else if ((giorni > 0 && giorni <= 15) && myTempiMediTmp.TipoPeriodo == TypePeriodo._0_15_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._0_15_ggDopo.ToString();
                            }
                            else if ((giorni > 15 && giorni <= 30) && myTempiMediTmp.TipoPeriodo == TypePeriodo._15_30_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._15_30_ggDopo.ToString();
                            }
                            else if ((giorni > 30 && giorni <= 60) && myTempiMediTmp.TipoPeriodo == TypePeriodo._30_60_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._30_60_ggDopo.ToString();
                            }
                            else if ((giorni > 60 && giorni <= 90) && myTempiMediTmp.TipoPeriodo == TypePeriodo._60_90_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._60_90_ggDopo.ToString();
                            }
                            else if ((giorni > 90 && giorni <= 180) && myTempiMediTmp.TipoPeriodo == TypePeriodo._90_180_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._90_180_ggDopo.ToString();
                            }
                            else if ((giorni > 180 && giorni <= 365) && myTempiMediTmp.TipoPeriodo == TypePeriodo._entro_1_Anno)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._entro_1_Anno.ToString();
                            }
                            else if ((giorni > 365) && myTempiMediTmp.TipoPeriodo == TypePeriodo._oltre_1_Anno)
                            {
                                myTempiMediTmp.SommatoriaPagato += myDatePagamentoTmp.dImportoPagamento;
                                if (myDatePagamentoTmp.sNumeroAvviso != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.sProvenienza = TypePeriodo._oltre_1_Anno.ToString();
                            }
                            //memorizzo il numero avviso precedente
                            numeroAvvisoPrec = myDatePagamentoTmp.sNumeroAvviso;
                        }
                    }
                    //inserisco il codice triburo fisso per identificarlo in tabella
                    myTempiMediTmp.CodTributo = "0434";
                }
                return ListTempiMediPagamento;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.CalcoloTempi.LoadTempiMedi::errore::", ex);
                return new List<TempiMediPagamento>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="myScadenzeList"></param>
        /// <param name="myDatePagamentoList"></param>
        /// <returns></returns>
        public List<TempiMediPagamento> LoadTempiMedi8852(string IdEnte, List<Scadenze> myScadenzeList, List<Pag8852> myDatePagamentoList)
        {
            try
            {
                List<TempiMediPagamento> ListTempiMediPagamento = new List<TempiMediPagamento>();
                //per ogni elemento in scadenza...
                foreach (Scadenze myScadenzeTmp in myScadenzeList)
                {
                    //per 10 volte perche devo avere 10 righe per ogni elemnto in scadenza, una riga per ogni TipoPeriodo
                    for (int i = 1; i < 11; i++)
                    {
                        TempiMediPagamento TempiMediPagamentoTmp = new TempiMediPagamento();
                        //carico oggetto TempiMediPagamentoTmp
                        TempiMediPagamentoTmp.IdEnte = IdEnte;
                        TempiMediPagamentoTmp.DataEmissione = myScadenzeTmp.DataEmissione;
                        TempiMediPagamentoTmp.NumeroRata = myScadenzeTmp.NumeroRata;
                        TempiMediPagamentoTmp.DataScadenza = myScadenzeTmp.DataScadenza;
                        //imposto a 0 il contatore avvisi e la sommatoria pagato
                        TempiMediPagamentoTmp.ContatoreAvvisi = 0;
                        TempiMediPagamentoTmp.SommatoriaPagato = 0;
                        //da 1 a 10
                        TempiMediPagamentoTmp.TipoPeriodo = i;
                        //aggiungo alla lista
                        ListTempiMediPagamento.Add(TempiMediPagamentoTmp);
                    }
                }
                //per ogni elemento in ListTempiMediPagamento...
                foreach (TempiMediPagamento myTempiMediTmp in ListTempiMediPagamento)
                {
                    int numeroAvvisoPrec = 0;
                    //DateTime myDate = new DateTime();
                    //per ogni data di pagamento nella relativa lista...
                    foreach (Pag8852 myDatePagamentoTmp in myDatePagamentoList)
                    {
                        //non serve più perche inserite da codice in quanto fisse
                        //myDatePagamentoTmp.NumeroRata = myDatePagamentoTmp.NumeroRata.PadLeft(2).Replace(' ', '0');
                        myTempiMediTmp.NumeroRata = myTempiMediTmp.NumeroRata.PadLeft(2).Replace(' ', '0');

                        //devo convertire la data in formato string yyyymmdd in una datetime corretta per il confronto
                        DateTime myDate = DateTime.Parse(myDatePagamentoTmp.AnnoRiferimento);
                        string myNumeroRata = "";

                        //devo convertire da bool a string per sapere il ocrretto numero rata
                        if (myDatePagamentoTmp.Acconto == true && myDatePagamentoTmp.Saldo == false)
                            myNumeroRata = "01";
                        else if (myDatePagamentoTmp.Acconto == false && myDatePagamentoTmp.Saldo == true)
                            myNumeroRata = "02";
                        else if (myDatePagamentoTmp.Acconto == true && myDatePagamentoTmp.Saldo == true)
                            myNumeroRata = "0U";
                        else if (myDatePagamentoTmp.Acconto == false && myDatePagamentoTmp.Saldo == false)
                            myNumeroRata = "0U";

                        //se coincidono vado a valorizzare il TipoPeriodo
                        if (myDate == myTempiMediTmp.DataEmissione && myNumeroRata == myTempiMediTmp.NumeroRata)
                        {
                            //calcolo la differenza in giorni
                            int giorni = (myDate - myTempiMediTmp.DataScadenza).Days;

                            if ((giorni <= -30) && myTempiMediTmp.TipoPeriodo == TypePeriodo._30_ggPrima)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.IdAnagrafico != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._30_ggPrima.ToString();
                            }
                            else if ((giorni > -30 && giorni <= -15) && myTempiMediTmp.TipoPeriodo == TypePeriodo._30_15_ggPrima)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._30_15_ggPrima.ToString();
                            }
                            else if ((giorni > -15 && giorni <= 0) && myTempiMediTmp.TipoPeriodo == TypePeriodo._15_0_ggPrima)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._15_0_ggPrima.ToString();
                            }
                            else if ((giorni > 0 && giorni <= 15) && myTempiMediTmp.TipoPeriodo == TypePeriodo._0_15_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._0_15_ggDopo.ToString();
                            }
                            else if ((giorni > 15 && giorni <= 30) && myTempiMediTmp.TipoPeriodo == TypePeriodo._15_30_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._15_30_ggDopo.ToString();
                            }
                            else if ((giorni > 30 && giorni <= 60) && myTempiMediTmp.TipoPeriodo == TypePeriodo._30_60_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._30_60_ggDopo.ToString();
                            }
                            else if ((giorni > 60 && giorni <= 90) && myTempiMediTmp.TipoPeriodo == TypePeriodo._60_90_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._60_90_ggDopo.ToString();
                            }
                            else if ((giorni > 90 && giorni <= 180) && myTempiMediTmp.TipoPeriodo == TypePeriodo._90_180_ggDopo)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._90_180_ggDopo.ToString();
                            }
                            else if ((giorni > 180 && giorni <= 365) && myTempiMediTmp.TipoPeriodo == TypePeriodo._entro_1_Anno)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._entro_1_Anno.ToString();
                            }
                            else if ((giorni > 365) && myTempiMediTmp.TipoPeriodo == TypePeriodo._oltre_1_Anno)
                            {
                                myTempiMediTmp.SommatoriaPagato = myTempiMediTmp.SommatoriaPagato + double.Parse(myDatePagamentoTmp.ImportoPagato.ToString());
                                if (myDatePagamentoTmp.ID != numeroAvvisoPrec)
                                {
                                    myTempiMediTmp.ContatoreAvvisi++;
                                }
                                myDatePagamentoTmp.Provenienza = TypePeriodo._oltre_1_Anno.ToString();
                            }
                            //memorizzo il numero avviso precedente
                            numeroAvvisoPrec = myDatePagamentoTmp.ID;
                        }
                    }
                    myTempiMediTmp.CodTributo = "8852";
                }
                return ListTempiMediPagamento;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLOImport.CalcoloTempi.LoadTempiMedi8852::errore::", ex);
                return new List<TempiMediPagamento>();
            }
        }
    }
     /// <summary>
    /// classe originale, converte i flussi forniti da halley 
    /// </summary>
    public class ConvertFromHalley
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConvertFromHalley));

        public string Anagrafica(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileAnagrafica);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + IdEnte + "\\" + TypeFile.FileAnagrafica);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 11;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 100;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;//Codice VIA – lunghezza 5 anziché 12
                                NewLine += (IdEnte + line.Substring(myStart, myLength)).PadLeft(12, char.Parse("0"));
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 20;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Dich8852(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileDich8852);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + IdEnte + "\\" + TypeFile.FileDich8852);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;//campo PROGRESSIVO IMMOBILE non gestito
                                myStart += myLength;
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 9;//campo TIPOIMMOBILE {1=Terreni,2=Aree,3=Fabbricati,4=Cat.D}
                                int TipoImmobile = 3;
                                int.TryParse(line.Substring(myStart, myLength), out TipoImmobile);
                                myStart += myLength;
                                myLength = 5;//Codice VIA – lunghezza 5 anziché 12
                                NewLine += (IdEnte + line.Substring(myStart, myLength)).PadLeft(12, char.Parse("0"));
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                NewLine += "1 ";//non gestiscono il TIPO POSSESSO quindi fisso 01 - Proprietà
                                NewLine += (TipoImmobile == 1 ? "1" : (TipoImmobile == 2 ? "2" : (line.Substring(myStart + 6, 1) == "1" ? "4" : "3"))).PadLeft(2, char.Parse("0"));//non gestiscono campo TIPO UTILIZZO sarebbe da dedurre da TIPOIMMOBILE+CATEGORIAIMU ma per ora gestiamo solo questi {1=Terreno, 2=Area, 3=Altro Fab, 4=Abitazione principale}
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;//campo PERCENTUALEDETRAZIONE non gestito
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 1;//non gestiscono campo TIPO RENDITA come lo intendiamo noi quindi da dedurre da TIPOIMMOBILE
                                myStart += myLength;
                                NewLine += (TipoImmobile == 1 ? "TA" : (TipoImmobile == 2 ? "AF" : "RE"));
                                NewLine += string.Empty.PadRight(10, char.Parse(" "));//non gestiscono ZONA
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                int TipoConsistenza = 0;
                                int.TryParse(line.Substring(myStart, myLength).Trim(), out TipoConsistenza);
                                myStart += myLength;
                                myLength = 8;
                                decimal Consistenza = 0;
                                decimal.TryParse(line.Substring(myStart, myLength).Trim(), out Consistenza);
                                NewLine += (TipoConsistenza == 1 ? (Consistenza * 10).ToString() : (Consistenza * 100).ToString()).PadLeft(8, char.Parse("0"));
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                NewLine += string.Empty.PadLeft(2, char.Parse("0"));//non gestiscono campo NUTILIZZATORI
                                myLength = 9;//non gestito DETRAZIONEFIGLI
                                myStart += myLength;
                                myLength = 6;//non gestito CATEGORIAIMU
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 255;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Pag8852(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FilePag8852);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + IdEnte + "\\" + TypeFile.FilePag8852);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).PadLeft(9, char.Parse("0"));
                                myStart += myLength;
                                myLength = 6;//il campo ANNO deve essere alla seconda posizione anziché la quarta
                                string CodContribuente = line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 10;
                                string Provenienza = line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength) + CodContribuente + Provenienza;
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Dich0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileDich0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + IdEnte + "\\" + TypeFile.FileDich0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                myLength = 30;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;//Codice VIA – lunghezza 5 anziché 12
                                NewLine += (IdEnte + line.Substring(myStart, myLength)).PadLeft(12, char.Parse("0"));
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength).Substring(2, 6).PadRight(8, char.Parse("0"));//campo MQ non gestiscono i 2 decimali
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 6; //Categoria TARI – lunghezza = 6 anziché 5
                                NewLine += line.Substring(myStart, myLength).Substring(1, 5);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 255;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Avvisi0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileAvvisi0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + IdEnte + "\\" + TypeFile.FileAvvisi0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                 myLength = 18;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 17;//Tutti i campi importi lunghezza 17 anziché 16
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Rate0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileRate0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + IdEnte + "\\" + TypeFile.FileRate0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 18;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 4;//non valorizzano il codice tributo
                                NewLine += "3944";
                                myStart += myLength;
                                myLength = 17;//Importo rata lunghezza 17 anziché 16
                                NewLine += line.Substring(myStart, myLength).Substring(1, 16);
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
    }
   /// <summary>
    /// converte i flussi forniti da StudioK
        /// Per StudioK occorre:
        ///  - formattare tutti i campi numerici forniti a destra aggiungendo gli 0 necessari a sinistra
        ///  - formattare alla giusta lunghezza i campi note
    /// </summary>
    public class ConvertFromStudioK
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(ConvertFromHalley));

         /// <summary>
        /// converte il file ANAGRAFE.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Anagrafica(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileAnagrafica);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileAnagrafica);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;
                                //codice anagrafico
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength).Replace(" ","").PadLeft(6, '0');
                                myStart += myLength;
                                //codice fiscale
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //partita iva
                                myLength = 11;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //cognome
                                myLength = 100;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //nome
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //sesso
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //data nascita
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //data morte
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //comune di nascita
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //provincia di nascita
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice strada
                                myLength = 12;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(12, '0');
                                myStart += myLength;
                                //via residenza
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //frazione residenza
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //civico residenza
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(10, '0');
                                myStart += myLength;
                                //lettera residenza
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //interno residenza
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //cap residenza
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //città residenza
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //provincia residenza
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //c/o civico
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //via invio
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //civico invio
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(10, '0');
                                myStart += myLength;
                                //cap invio
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;
                                //città invio
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //procincia invio
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //telefono
                                myLength = 20;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
         /// <summary>
        /// converte il file DICH_8852.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Dich8852(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileDich8852);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileDich8852);
                    string flag;
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                flag = "";

                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;
                                //numero dichiarazone
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //data dichiarazione
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice immobile
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice proprietario
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(6, '0');
                                myStart += myLength;
                                //codice strada
                                myLength = 12;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(12, '0');
                                myStart += myLength;
                                //strada
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //civico
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //scala
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //piano
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //interno
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //foglio
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //numero
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //subalterno
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;

                                //proprietario dal
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //proprietario fino al
                                myLength = 8;
                                string dataTemp = line.Substring(myStart, myLength);
                                if (dataTemp == "20991231")
                                {
                                    dataTemp = "99991231";
                                }
                                NewLine += dataTemp;
                                myStart += myLength;
                                //mesi di possesso
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                //tipo di possesso
                                myLength = 2;
                                string tipoPossessoTMP = line.Substring(myStart, myLength);
                                if (tipoPossessoTMP=="  ")
                                {
                                    NewLine += "1 ";
                                }
                                else
                                {
                                    NewLine += line.Substring(myStart, myLength);
                                }
                                myStart += myLength;

                                //tipo utilizzo
                                myLength = 2;
                                string tipoUtilizzoTMP = line.Substring(myStart, myLength);
                                if (tipoUtilizzoTMP == "  ")
                                {
                                    NewLine += "2 ";
                                }
                                else
                                {
                                    NewLine += line.Substring(myStart, myLength);
                                } 
                                myStart += myLength;
                                //quota possesso
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;
                                //flag principale
                                myLength = 1;
                                flag= line.Substring(myStart, myLength);
                                if (flag == "1")
                                    flag = "1";
                                else
                                    flag = "0";
                                NewLine += flag;
                                myStart += myLength;
                                //flag pertinenza
                                myLength = 1;
                                flag = line.Substring(myStart, myLength);
                                if (flag == "1")
                                    flag = "1";
                                else
                                    flag = "0";
                                NewLine += flag;
                                myStart += myLength;
                                //tipo rendita
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //zona censuria/zona prg
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //categoria
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength).Replace("/", "").PadLeft(10);
                                myStart += myLength;
                                //classe
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //rendita
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //valore
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //consistenza
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(8, '0');
                                myStart += myLength;
                                //flag esente
                                myLength = 1;
                                flag = line.Substring(myStart, myLength);
                                if (flag == "1")
                                    flag = "1";
                                else
                                    flag = "0";
                                NewLine += flag;
                                myStart += myLength;
                                //mesi esenzione
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                //flag riduzione
                                myLength = 1;
                                flag = line.Substring(myStart, myLength);
                                if (flag == "1")
                                    flag = "1";
                                else
                                    flag = "0";
                                NewLine += flag;
                                myStart += myLength;
                                //mesi riduzione
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                //numero figli fino a 26 anni
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                //numero utilizzatori
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                //flag coltivatore diretto
                                myLength = 1;
                                flag = line.Substring(myStart, myLength);
                                if (flag == "1")
                                    flag = "1";
                                else
                                    flag = "0";
                                NewLine += flag;
                                myStart += myLength;
                                //note
                                myLength = 255;
                                string notetmp = " ";
                                notetmp = notetmp.PadLeft(255, ' ');
                                NewLine += notetmp;
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
         /// <summary>
        /// converte il file VERSAMENTI_8852.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Pag8852(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FilePag8852);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FilePag8852);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            string flag;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;
                                flag = "";

                                //id pagamento
                                myLength = 9;
                                NewLine += line.Substring(myStart, myLength).PadLeft(9, char.Parse("0"));
                                myStart += myLength;
                                //anno riferimento
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice proprietario
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //provenienza
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //data accredito
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //data pagamento
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //numero fabbricati posseduti
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //flag acconto
                                myLength = 1;
                                    flag = line.Substring(myStart, myLength);
                                    if (flag == "1")
                                        flag = "1";
                                    else
                                        flag = "0";
                                    NewLine += flag;
                                myStart += myLength;
                                //flag saldo
                                myLength = 1;
                                    flag = line.Substring(myStart, myLength);
                                    if (flag == "1")
                                        flag = "1";
                                    else
                                        flag = "0";
                                    NewLine += flag;
                                myStart += myLength;
                                //flag ravvedimento operoso
                                myLength = 1;
                                    flag = line.Substring(myStart, myLength);
                                    if (flag == "1")
                                        flag = "1";
                                    else
                                        flag = "0";
                                    NewLine += flag;
                                myStart += myLength;
                                //codice tributo
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //importo
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
     /// <summary>
        /// converte il file DICH_0434.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Dich0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileDich0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileDich0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            string flag;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;
                                flag = "";
                                //numero dichiarazione
                                myLength = 30;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //data dichiarazione
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;

                                //innesto per casistica data "occupato dal" vuota 
                                string annoDichiarazione = line.Substring(30, 4);
                                int annoDichiarazioneNumerico = int.Parse(annoDichiarazione);
                                annoDichiarazioneNumerico = annoDichiarazioneNumerico - 1;

                                //codice immobile
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //profgressivo immobili
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;
                                //codice occupante
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(6, '0');
                                myStart += myLength;
                                //codice strada
                                myLength = 12;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(12, '0');
                                myStart += myLength;
                                //strada
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                //civico
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //scala
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //piano
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //interno
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //foglio
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //numero
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //subalterno
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //occupato dal
                                myLength = 8;
                                string dataTemp = line.Substring(myStart, myLength).Replace(" ", "");
                                if (dataTemp!="")
                                {
                                    NewLine += line.Substring(myStart, myLength);
                                }
                                else
                                {
                                    NewLine += annoDichiarazioneNumerico.ToString() + "0101";
                                }
                                
                                myStart += myLength;

                                //occupato fino al
                                myLength = 8;
                                dataTemp = line.Substring(myStart, myLength).Replace(" ", "");
                                if (dataTemp != "")
                                {
                                    NewLine += line.Substring(myStart, myLength);
                                }
                                else
                                {
                                    NewLine += "00010101";
                                }
                                myStart += myLength;
                                
                                //MQ catastali
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(8, '0');
                                myStart += myLength;
                                //codice vano
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;
                                //MQ
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(8, '0');
                                myStart += myLength;
                                //tipo vano
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //categoria tarsu
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(4, '0');
                                myStart += myLength;
                                
                                //categoria tares/tari
                                myLength = 5;
                                string catTemp = line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                //domestica casalbordino 069015
                                //domestica pollutri 069068
                                //domestica pollutri 069068
                                if (catTemp == "00300" || catTemp == "00001" || catTemp == "00024" || catTemp == "00100")
                                {
                                    NewLine += "DOM  ";
                                }
                                else
                                {
                                     NewLine += catTemp;
                                }
                                myStart += myLength;

                                //num componenti pf
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                //num componenti pv
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                // flag esente
                                myLength = 1;
                                    flag = line.Substring(myStart, myLength);
                                    if (flag == "1")
                                        flag = "1";
                                    else
                                        flag = "0";
                                    NewLine += flag;
                                myStart += myLength;
                                //note //nel file mancano le note! commento e aggiungo 255 fissi
                                myLength = 255;
                                string notetmp = " ";
                                notetmp = notetmp.PadLeft(255, ' ');
                                NewLine += notetmp;
                                myStart += myLength;
                            }
                            catch (Exception ex)
                            {
                                Log.Debug("OPENgovSPORTELLOimport.LineParser.Dich0434::errore::", ex);
                            }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
          /// <summary>
         /// converte il file AVVISI_0434.TXT
         /// </summary>
         /// <param name="IdEnte"></param>
         /// <param name="PathFile"></param>
         /// <returns></returns>
        public string Avvisi0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileAvvisi0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileAvvisi0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                //codice occupante
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(6, '0');
                                myStart += myLength;
                                //anno imposta
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(4, '0');
                                myStart += myLength;
                                //numero avviso
                                myLength = 18;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //data avviso
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //imposta
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //eca
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //meca
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //provicniale
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //maggiorazione
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //iva
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //arrotondamento
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(8, '0');
                                myStart += myLength;
                                //spese di notifica
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                                //totale avviso
                                myLength = 17;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(17, '0');
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
         /// <summary>
         /// converte il file RATE_0434.TXT
         /// </summary>
         /// <param name="IdEnte"></param>
         /// <param name="PathFile"></param>
         /// <returns></returns>
        public string Rate0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileRate0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileRate0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                //codice occupante
                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(6, '0');
                                myStart += myLength;
                                //numero avviso
                                myLength = 18;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //anno d imposta
                                myLength = 4;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(4, '0');
                                myStart += myLength;
                                //data avviso
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //numero rata
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(2, '0');
                                myStart += myLength;
                                //data scadenza
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice tributo
                                myLength = 4;//non valorizzano il codice tributo
                                NewLine += "3944";
                                myStart += myLength;
                                //importo rata
                                myLength = 16;//Importo rata lunghezza 17 anziché 16
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(16, '0');
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
         /// <summary>
         /// converte il file ESE_DICH_0434.TXT
         /// </summary>
         /// <param name="IdEnte"></param>
         /// <param name="PathFile"></param>
         /// <returns></returns>
        public string EseDich_0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileEseDich0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileEseDich0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                //numero dichiarazione
                                myLength = 30;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice immobile
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //progressivo immobile
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;
                                //occupato dal
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice esenzione
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;

                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
     /// <summary>
        /// converte il file RID_DICH_0434.TXT
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string RidDich_0434(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileRidDich0434);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileRidDich0434);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                //numero dichiarazione
                                myLength = 30;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice immobile
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //progressivo immobile
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;
                                //occupato dal
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                //codice riduzione
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength).Replace(" ", "").PadLeft(5, '0');
                                myStart += myLength;

                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
    }

    /// <summary>
    /// converte i flussi forniti da ApSystem
    /// </summary>
    public class ConvertFromApSystem
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConvertFromHalley));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string Anagrafica(string IdEnte, string PathFile)
        {
            string line, NewLine;
            string myItem = "200 OK";

            try
            {
                byte[] PostedFile = System.IO.File.ReadAllBytes(PathFile + IdEnte + "\\" + TypeFile.FileAnagrafica);
                if (PostedFile != null)
                {
                    MemoryStream ms = new MemoryStream(PostedFile, 0, PostedFile.Length);
                    StreamReader sr = new StreamReader(ms);
                    var writer = new StreamWriter(PathFile + "\\" + TypeFile.FileAnagrafica);
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            NewLine = string.Empty;
                            try
                            {
                                line = line.Replace(((Char)65533), char.Parse(" "));
                                if (line == string.Empty)
                                    continue;
                                int myStart, myLength;
                                myStart = myLength = 0;

                                myLength = 6;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 16;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 11;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 100;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 1;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 8;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;//Codice VIA – lunghezza 5 anziché 12
                                NewLine += (IdEnte + line.Substring(myStart, myLength)).PadLeft(12, char.Parse("0"));
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 3;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 10;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 5;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 50;
                                NewLine += line.Substring(myStart, myLength).Replace("'", "’");
                                myStart += myLength;
                                myLength = 2;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                                myLength = 20;
                                NewLine += line.Substring(myStart, myLength);
                                myStart += myLength;
                            }
                            catch (Exception)
                            { }
                            writer.WriteLine(NewLine);
                        }
                    }
                    catch (Exception)
                    { }
                    finally
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                myItem = ex.Message;
            }
            return myItem;
        }
    }
}
