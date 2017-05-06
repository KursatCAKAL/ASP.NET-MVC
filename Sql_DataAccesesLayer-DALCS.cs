using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALKullanimi
{
    public class DAL
    {
        public DAL()//BU CTOR'U OLU�TURMAMIZIN SEBEB� DAL class'�n� ba�ka bir yerde normal bir nesne gibi olu�turmak istedi�imde sorun��kmamas� i�in.
        {}
        //DAL orndal=new DAL(); �eklinde olu�turabilmem i�in e�er bu ctor'u yapmasayd�k sadece alttaki ctor olsayd� ben parametre girmek zorunda kalacakt�m.
        public DAL(string gelenBaglantiAdresi)
        {
            karsilayanBaglanti_Adresi = gelenBaglantiAdresi;
        }//Olu�turdugum nesneme mecburi olarak ba�lant� adresi parametresi gir diyorum

        public string karsilayanBaglanti_Adresi { get; set; }//Bu prop sayesinde gelen ba�lant�y� tutuyorum.

        SqlConnection conn;

        public enum BaglantiIslem//Bu enum sayesinde ba�lant�n�n a��kl�k kapal�l�k durumunu kontrol etmem soncunda i�lemler yapabiliyorum.
        {
            BaglantiyiAc,
            Baglant�y�Kapat
        }

        public delegate void HATA(string HataMesaji);//Ana bir delegate tipi olu�turdum buna ba�l� alt hata �e�itleri olu�turabilirim art�k bu tipi kullanan.
        public event HATA BaglantiHatasi;
        public event HATA SorguHatasi;

        public bool BaglantiDurum(BaglantiIslem islem)//Bu fonksiyonum(--->>>method de�il method geriye de�er d�nd�rmez---<<<) sayesinde ba�lant�n�n durumunu kontrol edip ona g�re a��p kapatacam
        {
            bool Durum = true;//buna default bir value vermekten ziyade a�a��da bir ba�ar�s�zl�k durumunda false yapt�m ya e�er ba�ar�s�zl�k olmaz ise ba�lant� open d�nd�r�r gibi d���n.

            try
            {
                conn = new SqlConnection(karsilayanBaglanti_Adresi);//connection'�mla ilgili bir i�lem yapabilmem i�in connection'�m�n tan�ml� olmas� laz�m tan�ml�yorum ki a�a��da onunla ilgili i�lemler yapabiliyim.

                switch (islem)
                {
                    case BaglantiIslem.BaglantiyiAc:
                        if (conn.State==System.Data.ConnectionState.Closed)//ba�lant� kapal� m� kontrol edip kapal�ysa a��yorum.
                        {
                            conn.Open();
                        }
                        break;
                    case BaglantiIslem.Baglant�y�Kapat:
                        if (conn.State == System.Data.ConnectionState.Open)//ba�lant� a��k m� kontrol edip a��ksa kapat�yorum.
                        {
                            conn.Close();
                        }
                        break;
                }

            }
            catch(Exception ex)//ba�lant�yla ilgili bir hata olmas� durumda �al��acak �rne�in ba�lant� adresi yanl�� ya da sql server eri�ime kapat�ld� authentication iznin kesildi vs.
            {
                try { BaglantiHatasi(ex.Message.ToString()); }//event'�m �al���yor
                catch { }
                Durum = false;
            }
            return Durum;

        }

        public SqlDataReader GetDataReader(string Sorgu,bool IsProcedure)
        {
            SqlDataReader dr = null;

            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))//Ba�lant�m a��k ise e�er ve ayn� zamanda SqlDataReader tipindeki GetDateReader fonksiyonuma gerekli sorguyu atam�� isem bana gerekli i�lemi yapacak.
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)//Bu k�s�m procedure ile i�lem yap�lma olay�n� kontrol ediyor e�er procedure true ise ben sorgu k�sm�na procedure girmeliyim ve girdi�imde procedure kulland���ma y�nelik devam edicek i�lemlerim.
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    dr = cmd.ExecuteReader();//komutumdan gelen bilgi ak���n� �zerinde tutuyor
                }
            }
            catch (Exception ex)//Sorgumda s�k�nt� var ise bu �al���cak.
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally  //Ben her durumda ba�lant�m� kapatmak zorunday�m , e�er 86 daki scope dan hemen sonra kapatsam olmaz ��nk� diyelim ki sorguyu yanl�� yazd�n bi s�k�nt� oldu ba�lant� kapant��� i�in 
                    //catch'e d���p hatay� g�steremez,, 
                   //catch'de kapatsam olmaz ��nk� diyelim ki sorguyu falan do�ru yazd�m bu sefer catch'e d��mez ise ba�lant�m a��k kalm�� olur.
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);
            }

            return dr;//85 te tuttu�um bilgi ak���n� d�nd�r�yorum.
        }//Procedure kullanm�caksam false yazar sorgumu girerek DataReader'�mla ger�ekle�tireceklerimi ger�ekle�tiririm.

        public SqlDataReader GetDataReader(string Sorgu,bool IsProcedure, params SqlParameter[] parametreler)
        {
            SqlDataReader dr = null;

            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }
                                                 
                                                //Di�er yorumlar ayn� fark buradaki.
                    if (parametreler.Length>0)//bu ko�ul sa�lan�yorsa parametre var demektik ben bu parametleri tek tek ekliyorum cmd'imin parametresine.
                    {
                        foreach (SqlParameter item in parametreler)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    dr = cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);
            }

            return dr;
        }//Parametreli olma durumu vard�r.

        public DataTable GetDataTable(string Sorgu, bool IsProcedure)
        {
            DataTable dt = new DataTable();

            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);//DataTable'gibi bir nesneyi �zerine verdi�imiz komut sayesinde doldurabilen veri ak���n� sa�layan bir yap�d�r.
                                                                //Verdi�im komutlar ve parametreler sayesindede bunu sa�l�yor.

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);//Son olarak ba�lant�y� kapat�yor.
            }

            return dt;//Ve direk DataTable' bilgisi olarak d�nd�r�yor bana i�i.
        }

        public DataTable GetDataTable(string Sorgu, bool IsProcedure, params SqlParameter[] parametreler)//Yukar�daki datatable'�m�n sadece parametreli modudur bu da.
        {
            DataTable dt = new DataTable();

            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    if (parametreler.Length > 0)
                    {
                        foreach (SqlParameter item in parametreler)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);
            }

            return dt;
        }

        public DataSet GetDataSet(string Sorgu, bool IsProcedure)
        {
            DataSet ds = new DataSet();

            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);
            }

            return ds;
        }

        public DataSet GetDataSet (string Sorgu, bool IsProcedure, params SqlParameter[] parametreler)
        {
            DataSet ds = new DataSet();

            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    if (parametreler.Length > 0)
                    {
                        foreach (SqlParameter item in parametreler)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);
            }

            return ds;
        }

        public string ReturnSQL(string TabloAdi, params string[] Kolonlar)
        {
            string Rtn = "SELECT ";

            foreach (string item in Kolonlar)//Mesele Kolonlar params'�na sadece * koyar ge�ersem Rtn adl� sorgum SELECT * FROM + TABLOAD� olur.
            {
                Rtn += item + ",";//SORGUNUN SONUNDA HER HALUKARDA ',' kal�r bu y�zden onu gideriyorum.line 301 'de 
            }

            Rtn = Rtn.TrimEnd(',');//

            Rtn += " FROM " + TabloAdi;

            return Rtn;
        }

        public string ReturnSQL(string TabloAdi, List<TBLFilitre> filitre, params string[] Kolonlar)//Bir �stteki tablomun filtreleme yapabildi�im modu.
        {
            string Rtn = "SELECT ";

            foreach (string item in Kolonlar)
            {
                Rtn += item + ",";
            }

            Rtn = Rtn.TrimEnd(',');

            Rtn += " FROM " + TabloAdi;

            if (filitre.Count>0)//filtre listemde eleman varsa yani filtre giri�i yapm�� isem buras� �al���yor ve filtrelerim ekleniyor.
            {
                Rtn += " WHERE ";

                foreach (TBLFilitre item in filitre)//filte olarak g�nderdi�imiz List parametre i�inden tek tek TBLFilitre class'� tipinde item d�n�yoruz
                {
                    Rtn += item.RtnSTR();//ve bunu Rtn sorgumuza ekliyoruz.
                }

                Rtn = Rtn.Remove(Rtn.Length - 4, 4);//Sorgumun sonunda AND veya OR her t�rl� bir tane fazladan kal�yor bunu silmek i�in b�yle bir �ey yap�yorum.
            }

            return Rtn;
        }

        public void INSERT(string TabloAdi, List<KolonToValue> veriler)//Herhangi bir de�er geriye d�nd�r�p onu kullanmaks�z�n INSERT olay�m� direk i�ine parametre vererek �a��r�yorum.
        {
            string SQL = "INSERT INTO " + TabloAdi + " (";


            foreach (KolonToValue item in veriler)
            {
                SQL += item.KolonAdi + ",";
            }

            SQL = SQL.TrimEnd(',');

            SQL += ") VALUES (";

            foreach (KolonToValue item in veriler)
            {
                SQL += "'" + item.ValueDegeri + "',";//SQL �NJECT�ON A�I�INI ��FT TIRNAK ARASINA ALMAMIZ KAPATIYOR.HER NE KADAR PARAMETER KULLANMAK DAHA SA�LIKLI OLSADA BU DA DO�RU B�R KULLANIM
            }

            SQL = SQL.TrimEnd(',');

            SQL += ")";

            KodCalistir(SQL, false);//ATLAMA BURAYI-->> Ve kod �al��t�r methodum sayesinde insert i�lemi ger�ekle�iyor.(((connection command gibi de�eleri devreye sokuyor Kod�al��tir methodum.)))
        }

        public void UPDATE(string TabloAdi, List<KolonToValue> veriler, List<TBLFilitre> filitre)//Herhangi bir de�er geriye d�nd�r�p onu kullanmaks�z�n UPDATE olay�m� direk i�ine parametre vererek �a��r�yorum.
        {
            string SQL = "UPDATE " + TabloAdi + " SET ";

            foreach (KolonToValue item in veriler)
            {
                SQL += item.KolonAdi + "='" + item.ValueDegeri + "',";//SQL �NJECT�ON A�I�INI ��FT TIRNAK ARASINA ALMAMIZ KAPATIYOR.HER NE KADAR PARAMETER KULLANMAK DAHA SA�LIKLI OLSADA BU DA DO�RU B�R KULLANIM
            }

            SQL = SQL.TrimEnd(',');

            if (filitre.Count > 0)
            {
                SQL += " WHERE ";

                foreach (TBLFilitre item in filitre)
                {
                    SQL += item.RtnSTR();//UPDATE METHODUMA g�nderdi�im filtre listesi  �zerinde d�nerken bu item �zerinden RtnSTR methodu sayesinde filtre �artlar�n� ekliyorum main sorgum olan SQL string'ine.
                }

                SQL = SQL.Remove(SQL.Length - 4, 4);
            }

            KodCalistir(SQL, false);

        }

        public void DELETE(string TabloAdi, List<TBLFilitre> filitre)//Herhangi bir de�er geriye d�nd�r�p onu kullanmaks�z�n DELETE olay�m� direk i�ine parametre vererek �a��r�yorum.
        {
            string SQL = "DELETE " + TabloAdi;

            if (filitre.Count > 0)
            {
                SQL += " WHERE ";

                foreach (TBLFilitre item in filitre)
                {
                    SQL += item.RtnSTR();//UPDATE METHODUMA g�nderdi�im filtre listesi  �zerinde d�nerken bu item �zerinden RtnSTR methodu sayesinde filtre �artlar�n� ekliyorum main sorgum olan SQL string'ine.
                }

                SQL = SQL.Remove(SQL.Length - 4, 4);
            }

            KodCalistir(SQL, false);
        }

        public void KodCalistir(string Sorgu, bool IsProcedure)//��ine g�nderdi�imiz sorguyu komut olarak �al��t�r�yor.Bu methodu �a��rd�ktan sonra i�indekiler �al��m�� oluyor.Alt�na devam scope'dan sonra edebilirsin.
        //Connection,command gibi de�eleri devreye sokuyor ba�lant� veya sorguda hata varsa eventlar� �al��t�r�yor kodlar�m� �al��t�rm�� oluyor.
        {
            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);
            }
        }

        public void KodCalistir(string Sorgu, bool IsProcedure, params SqlParameter[] parametreler)//��ine g�nderdi�imiz sorguyu komut olarak �al��t�r�yor.Bu methodu �a��rd�ktan sonra i�indekiler �al��m�� oluyor.Alt�na devam scope'dan sonra edebilirsin.
                                                                                                 //Connection,command gibi de�eleri devreye sokuyor ba�lant� veya sorguda hata varsa eventlar� �al��t�r�yor kodlar�m� �al��t�rm�� oluyor.
        {
            try
            {
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    if (parametreler.Length > 0)
                    {
                        foreach (SqlParameter item in parametreler)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally
            {
                BaglantiDurum(BaglantiIslem.Baglant�y�Kapat);
            }
        }
    }

    public class TBLFilitre
    {
        public enum SartTipi
        {
            E�ittir,
            E�itDe�ildir,
            K���kt�r,
            B�y�kt�r,
            K���kE�ittir,
            B�y�kE�ittir,
            Ba��nda,
            Sonunda,
            ��eri�inde
        }

        public enum AND_OR
        {
            AND,
            OR
        }


        public string _KolonAdi { get; set; }
        public SartTipi _Sartdegeri { get; set; }
        public string _ValueDegeri { get; set; }
        public AND_OR _AndOR { get; set; }

        public TBLFilitre(string KolonAdi,SartTipi SartDegeri,string ValueDegeri,AND_OR ANDOR)
        {
            _KolonAdi = KolonAdi;
            _Sartdegeri = SartDegeri;
            _ValueDegeri = ValueDegeri;
            _AndOR = ANDOR;
        }

        public string RtnSTR()
        {
            string Rtn = _KolonAdi;

            switch (_Sartdegeri)
            {
                case SartTipi.E�ittir:
                    Rtn += " = '" + _ValueDegeri + "'";
                    break;
                case SartTipi.E�itDe�ildir:
                    Rtn += " <> '" + _ValueDegeri + "'";
                    break;
                case SartTipi.K���kt�r:
                    Rtn += " < '" + _ValueDegeri + "'";
                    break;
                case SartTipi.B�y�kt�r:
                    Rtn += " > '" + _ValueDegeri + "'";
                    break;
                case SartTipi.K���kE�ittir:
                    Rtn += " <= '" + _ValueDegeri + "'";
                    break;
                case SartTipi.B�y�kE�ittir:
                    Rtn += " >= '" + _ValueDegeri + "'";
                    break;
                case SartTipi.Ba��nda:
                    Rtn += " Like '" + _ValueDegeri + "%'";
                    break;
                case SartTipi.Sonunda:
                    Rtn += " Like '%" + _ValueDegeri + "'";
                    break;
                case SartTipi.��eri�inde:
                    Rtn += " Like '%" + _ValueDegeri + "%'";
                    break;
            }

            switch (_AndOR)
            {
                case AND_OR.AND:
                    Rtn += " AND ";
                    break;
                case AND_OR.OR:
                    Rtn += " OR ";
                    break;
            }

            return Rtn;
        }


    }
    public class JOINLER
    {
        public enum JOINTipleri
        {
            LEFT,
            RIGHT,
            INNER,
            CROSS
        }
        //JOINLER tipinde bir List<> olu�turdugumda bu de�eri giricem ve bu parametreleri kullan�cam.
        public JOINTipleri _JOINTip { get; set; }
        public string _AnaTablo { get; set; }
        public string _AnaKolon { get; set; }
        public string _EklenecekTablo { get; set; }
        public string _EklenecekKolon { get; set; }

        public JOINLER(JOINTipleri JOINTip, string AnaTablo, string AnaKolon, string EklenecekTablo, string EklenecekKolon)
        {
            _JOINTip = JOINTip;
            _AnaKolon = AnaKolon;
            _AnaTablo = AnaTablo;
            _EklenecekTablo = EklenecekTablo;
            _EklenecekKolon = EklenecekKolon;
        }


        public string RtnSTR()//Bu fonksiyonumu �a��rd�g�mda bana ald��� JOINTip'ler do�rultusunda sorgumu olu�turucak.
        {
            string Rtn = "";

            switch (_JOINTip)
            {
                case JOINTipleri.LEFT:
                    Rtn = " LEFT JOIN " + _EklenecekTablo + " ON " + _EklenecekTablo + "." + _EklenecekKolon + "=" + _AnaTablo + "." + _AnaKolon;
                    break;
                case JOINTipleri.RIGHT:
                    Rtn = " RIGHT JOIN " + _EklenecekTablo + " ON " + _EklenecekTablo + "." + _EklenecekKolon + "=" + _AnaTablo + "." + _AnaKolon;
                    break;
                case JOINTipleri.INNER:
                    Rtn = " INNER JOIN " + _EklenecekTablo + " ON " + _EklenecekTablo + "." + _EklenecekKolon + "=" + _AnaTablo + "." + _AnaKolon;
                    break;
                case JOINTipleri.CROSS:
                    Rtn = " CROSS JOIN " + _EklenecekTablo;
                    break;
            }

            return Rtn;
        }
    }

    public class KolonToValue//INSERT AND UPDATE KOMUTLARIMDA ,INSERT VE UPDATE KOMUTLARIM VERI GIRISLERI ISTEDIGI ICIN BU SEKILDE KOLON ADINA GORE VERILERIMI BU CLASS'IM UZZERINDEN AKTARIYORUM BIR LIST OLUSTURUP INSERT VE UPDATE ICINDE FOREACH ILE DONEREK CEKIP VERIYORUM INSERT UPDATE METHODLARIMIN ICINDE 
    {
        public string KolonAdi { get; set; }

        public string ValueDegeri { get; set; }
    }
}
