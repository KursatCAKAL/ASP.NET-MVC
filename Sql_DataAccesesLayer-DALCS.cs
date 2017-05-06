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
        public DAL()//BU CTOR'U OLUÞTURMAMIZIN SEBEBÝ DAL class'ýný baþka bir yerde normal bir nesne gibi oluþturmak istediðimde sorunçýkmamasý için.
        {}
        //DAL orndal=new DAL(); þeklinde oluþturabilmem için eðer bu ctor'u yapmasaydýk sadece alttaki ctor olsaydý ben parametre girmek zorunda kalacaktým.
        public DAL(string gelenBaglantiAdresi)
        {
            karsilayanBaglanti_Adresi = gelenBaglantiAdresi;
        }//Oluþturdugum nesneme mecburi olarak baðlantý adresi parametresi gir diyorum

        public string karsilayanBaglanti_Adresi { get; set; }//Bu prop sayesinde gelen baðlantýyý tutuyorum.

        SqlConnection conn;

        public enum BaglantiIslem//Bu enum sayesinde baðlantýnýn açýklýk kapalýlýk durumunu kontrol etmem soncunda iþlemler yapabiliyorum.
        {
            BaglantiyiAc,
            BaglantýyýKapat
        }

        public delegate void HATA(string HataMesaji);//Ana bir delegate tipi oluþturdum buna baðlý alt hata çeþitleri oluþturabilirim artýk bu tipi kullanan.
        public event HATA BaglantiHatasi;
        public event HATA SorguHatasi;

        public bool BaglantiDurum(BaglantiIslem islem)//Bu fonksiyonum(--->>>method deðil method geriye deðer döndürmez---<<<) sayesinde baðlantýnýn durumunu kontrol edip ona göre açýp kapatacam
        {
            bool Durum = true;//buna default bir value vermekten ziyade aþaðýda bir baþarýsýzlýk durumunda false yaptým ya eðer baþarýsýzlýk olmaz ise baðlantý open döndürür gibi düþün.

            try
            {
                conn = new SqlConnection(karsilayanBaglanti_Adresi);//connection'ýmla ilgili bir iþlem yapabilmem için connection'ýmýn tanýmlý olmasý lazým tanýmlýyorum ki aþaðýda onunla ilgili iþlemler yapabiliyim.

                switch (islem)
                {
                    case BaglantiIslem.BaglantiyiAc:
                        if (conn.State==System.Data.ConnectionState.Closed)//baðlantý kapalý mý kontrol edip kapalýysa açýyorum.
                        {
                            conn.Open();
                        }
                        break;
                    case BaglantiIslem.BaglantýyýKapat:
                        if (conn.State == System.Data.ConnectionState.Open)//baðlantý açýk mý kontrol edip açýksa kapatýyorum.
                        {
                            conn.Close();
                        }
                        break;
                }

            }
            catch(Exception ex)//baðlantýyla ilgili bir hata olmasý durumda çalýþacak örneðin baðlantý adresi yanlýþ ya da sql server eriþime kapatýldý authentication iznin kesildi vs.
            {
                try { BaglantiHatasi(ex.Message.ToString()); }//event'ým çalýþýyor
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
                if (BaglantiDurum(BaglantiIslem.BaglantiyiAc))//Baðlantým açýk ise eðer ve ayný zamanda SqlDataReader tipindeki GetDateReader fonksiyonuma gerekli sorguyu atamýþ isem bana gerekli iþlemi yapacak.
                {
                    SqlCommand cmd = new SqlCommand(Sorgu, conn);

                    if (IsProcedure)//Bu kýsým procedure ile iþlem yapýlma olayýný kontrol ediyor eðer procedure true ise ben sorgu kýsmýna procedure girmeliyim ve girdiðimde procedure kullandýðýma yönelik devam edicek iþlemlerim.
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    dr = cmd.ExecuteReader();//komutumdan gelen bilgi akýþýný üzerinde tutuyor
                }
            }
            catch (Exception ex)//Sorgumda sýkýntý var ise bu çalýþýcak.
            {
                try { SorguHatasi(ex.Message.ToString()); }
                catch { }
            }
            finally  //Ben her durumda baðlantýmý kapatmak zorundayým , eðer 86 daki scope dan hemen sonra kapatsam olmaz çünkü diyelim ki sorguyu yanlýþ yazdýn bi sýkýntý oldu baðlantý kapantýðý için 
                    //catch'e düþüp hatayý gösteremez,, 
                   //catch'de kapatsam olmaz çünkü diyelim ki sorguyu falan doðru yazdým bu sefer catch'e düþmez ise baðlantým açýk kalmýþ olur.
            {
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);
            }

            return dr;//85 te tuttuðum bilgi akýþýný döndürüyorum.
        }//Procedure kullanmýcaksam false yazar sorgumu girerek DataReader'ýmla gerçekleþtireceklerimi gerçekleþtiririm.

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
                                                 
                                                //Diðer yorumlar ayný fark buradaki.
                    if (parametreler.Length>0)//bu koþul saðlanýyorsa parametre var demektik ben bu parametleri tek tek ekliyorum cmd'imin parametresine.
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
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);
            }

            return dr;
        }//Parametreli olma durumu vardýr.

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

                    SqlDataAdapter da = new SqlDataAdapter(cmd);//DataTable'gibi bir nesneyi üzerine verdiðimiz komut sayesinde doldurabilen veri akýþýný saðlayan bir yapýdýr.
                                                                //Verdiðim komutlar ve parametreler sayesindede bunu saðlýyor.

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
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);//Son olarak baðlantýyý kapatýyor.
            }

            return dt;//Ve direk DataTable' bilgisi olarak döndürüyor bana iþi.
        }

        public DataTable GetDataTable(string Sorgu, bool IsProcedure, params SqlParameter[] parametreler)//Yukarýdaki datatable'ýmýn sadece parametreli modudur bu da.
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
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);
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
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);
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
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);
            }

            return ds;
        }

        public string ReturnSQL(string TabloAdi, params string[] Kolonlar)
        {
            string Rtn = "SELECT ";

            foreach (string item in Kolonlar)//Mesele Kolonlar params'ýna sadece * koyar geçersem Rtn adlý sorgum SELECT * FROM + TABLOADÝ olur.
            {
                Rtn += item + ",";//SORGUNUN SONUNDA HER HALUKARDA ',' kalýr bu yüzden onu gideriyorum.line 301 'de 
            }

            Rtn = Rtn.TrimEnd(',');//

            Rtn += " FROM " + TabloAdi;

            return Rtn;
        }

        public string ReturnSQL(string TabloAdi, List<TBLFilitre> filitre, params string[] Kolonlar)//Bir üstteki tablomun filtreleme yapabildiðim modu.
        {
            string Rtn = "SELECT ";

            foreach (string item in Kolonlar)
            {
                Rtn += item + ",";
            }

            Rtn = Rtn.TrimEnd(',');

            Rtn += " FROM " + TabloAdi;

            if (filitre.Count>0)//filtre listemde eleman varsa yani filtre giriþi yapmýþ isem burasý çalýþýyor ve filtrelerim ekleniyor.
            {
                Rtn += " WHERE ";

                foreach (TBLFilitre item in filitre)//filte olarak gönderdiðimiz List parametre içinden tek tek TBLFilitre class'ý tipinde item dönüyoruz
                {
                    Rtn += item.RtnSTR();//ve bunu Rtn sorgumuza ekliyoruz.
                }

                Rtn = Rtn.Remove(Rtn.Length - 4, 4);//Sorgumun sonunda AND veya OR her türlü bir tane fazladan kalýyor bunu silmek için böyle bir þey yapýyorum.
            }

            return Rtn;
        }

        public void INSERT(string TabloAdi, List<KolonToValue> veriler)//Herhangi bir deðer geriye döndürüp onu kullanmaksýzýn INSERT olayýmý direk içine parametre vererek çaðýrýyorum.
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
                SQL += "'" + item.ValueDegeri + "',";//SQL ÝNJECTÝON AÇIÐINI ÇÝFT TIRNAK ARASINA ALMAMIZ KAPATIYOR.HER NE KADAR PARAMETER KULLANMAK DAHA SAÐLIKLI OLSADA BU DA DOÐRU BÝR KULLANIM
            }

            SQL = SQL.TrimEnd(',');

            SQL += ")";

            KodCalistir(SQL, false);//ATLAMA BURAYI-->> Ve kod çalýþtýr methodum sayesinde insert iþlemi gerçekleþiyor.(((connection command gibi deðeleri devreye sokuyor KodÇalýþtir methodum.)))
        }

        public void UPDATE(string TabloAdi, List<KolonToValue> veriler, List<TBLFilitre> filitre)//Herhangi bir deðer geriye döndürüp onu kullanmaksýzýn UPDATE olayýmý direk içine parametre vererek çaðýrýyorum.
        {
            string SQL = "UPDATE " + TabloAdi + " SET ";

            foreach (KolonToValue item in veriler)
            {
                SQL += item.KolonAdi + "='" + item.ValueDegeri + "',";//SQL ÝNJECTÝON AÇIÐINI ÇÝFT TIRNAK ARASINA ALMAMIZ KAPATIYOR.HER NE KADAR PARAMETER KULLANMAK DAHA SAÐLIKLI OLSADA BU DA DOÐRU BÝR KULLANIM
            }

            SQL = SQL.TrimEnd(',');

            if (filitre.Count > 0)
            {
                SQL += " WHERE ";

                foreach (TBLFilitre item in filitre)
                {
                    SQL += item.RtnSTR();//UPDATE METHODUMA gönderdiðim filtre listesi  üzerinde dönerken bu item üzerinden RtnSTR methodu sayesinde filtre þartlarýný ekliyorum main sorgum olan SQL string'ine.
                }

                SQL = SQL.Remove(SQL.Length - 4, 4);
            }

            KodCalistir(SQL, false);

        }

        public void DELETE(string TabloAdi, List<TBLFilitre> filitre)//Herhangi bir deðer geriye döndürüp onu kullanmaksýzýn DELETE olayýmý direk içine parametre vererek çaðýrýyorum.
        {
            string SQL = "DELETE " + TabloAdi;

            if (filitre.Count > 0)
            {
                SQL += " WHERE ";

                foreach (TBLFilitre item in filitre)
                {
                    SQL += item.RtnSTR();//UPDATE METHODUMA gönderdiðim filtre listesi  üzerinde dönerken bu item üzerinden RtnSTR methodu sayesinde filtre þartlarýný ekliyorum main sorgum olan SQL string'ine.
                }

                SQL = SQL.Remove(SQL.Length - 4, 4);
            }

            KodCalistir(SQL, false);
        }

        public void KodCalistir(string Sorgu, bool IsProcedure)//Ýçine gönderdiðimiz sorguyu komut olarak çalýþtýrýyor.Bu methodu çaðýrdýktan sonra içindekiler çalýþmýþ oluyor.Altýna devam scope'dan sonra edebilirsin.
        //Connection,command gibi deðeleri devreye sokuyor baðlantý veya sorguda hata varsa eventlarý çalýþtýrýyor kodlarýmý çalýþtýrmýþ oluyor.
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
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);
            }
        }

        public void KodCalistir(string Sorgu, bool IsProcedure, params SqlParameter[] parametreler)//Ýçine gönderdiðimiz sorguyu komut olarak çalýþtýrýyor.Bu methodu çaðýrdýktan sonra içindekiler çalýþmýþ oluyor.Altýna devam scope'dan sonra edebilirsin.
                                                                                                 //Connection,command gibi deðeleri devreye sokuyor baðlantý veya sorguda hata varsa eventlarý çalýþtýrýyor kodlarýmý çalýþtýrmýþ oluyor.
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
                BaglantiDurum(BaglantiIslem.BaglantýyýKapat);
            }
        }
    }

    public class TBLFilitre
    {
        public enum SartTipi
        {
            Eþittir,
            EþitDeðildir,
            Küçüktür,
            Büyüktür,
            KüçükEþittir,
            BüyükEþittir,
            Baþýnda,
            Sonunda,
            Ýçeriðinde
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
                case SartTipi.Eþittir:
                    Rtn += " = '" + _ValueDegeri + "'";
                    break;
                case SartTipi.EþitDeðildir:
                    Rtn += " <> '" + _ValueDegeri + "'";
                    break;
                case SartTipi.Küçüktür:
                    Rtn += " < '" + _ValueDegeri + "'";
                    break;
                case SartTipi.Büyüktür:
                    Rtn += " > '" + _ValueDegeri + "'";
                    break;
                case SartTipi.KüçükEþittir:
                    Rtn += " <= '" + _ValueDegeri + "'";
                    break;
                case SartTipi.BüyükEþittir:
                    Rtn += " >= '" + _ValueDegeri + "'";
                    break;
                case SartTipi.Baþýnda:
                    Rtn += " Like '" + _ValueDegeri + "%'";
                    break;
                case SartTipi.Sonunda:
                    Rtn += " Like '%" + _ValueDegeri + "'";
                    break;
                case SartTipi.Ýçeriðinde:
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
        //JOINLER tipinde bir List<> oluþturdugumda bu deðeri giricem ve bu parametreleri kullanýcam.
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


        public string RtnSTR()//Bu fonksiyonumu çaðýrdýgýmda bana aldýðý JOINTip'ler doðrultusunda sorgumu oluþturucak.
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
