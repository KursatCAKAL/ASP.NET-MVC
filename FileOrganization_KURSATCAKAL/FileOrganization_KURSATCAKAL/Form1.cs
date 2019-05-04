using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace FileOrganization_KURSATCAKAL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static int islem_say = 1; static Random temp = new Random(); string lischPerformans, rischPerformans, eischPerformans, beischPerformans;
        private ListView sirasiz = new ListView(); private ListView lstLISCH = new ListView();
        private ListView lstRISCH = new ListView(); private ListView lstEISCH = new ListView(); private ListView lstBEISCH = new ListView();private ListView lstEICH = new ListView();
        public Eleman[] elemanUretilen;
        public int[] listeBas = new int[10]; int total_area;
        public int randomElemanSayisi; public int packingFactor; public int modValue; public int secondaryListCount; public int min_value; public int max_value;
        public ComboBox process_Choose;
        Stopwatch st_lish = new Stopwatch(); Stopwatch st_rlish = new Stopwatch(); Stopwatch st_eish = new Stopwatch(); Stopwatch st_beish = new Stopwatch();
        private void Form1_Load(object sender, EventArgs e)
        {
            randomElemanSayisi = temp.Next(15, 20);
            min_value = 10;
            max_value = 150;
            packingFactor = temp.Next(93, 96);
            secondaryListCount = (randomElemanSayisi * 100) / packingFactor +1;
            modValue = 10;

            #region Yöntem_Combobox
            process_Choose = cmbChoose;
            process_Choose.DisplayMember = "process";
            process_Choose.ValueMember = "process_id";
            process_Choose.Items.Add(new { process = "HEPSİ", process_id = "1" });
            process_Choose.Items.Add(new { process = "EISCH", process_id = "2" });
            process_Choose.Items.Add(new { process = "BEISCH", process_id = "3" });
            process_Choose.Items.Add(new { process = "RLISCH", process_id = "4" });
            process_Choose.Items.Add(new { process = "EICH", process_id = "5" });
            process_Choose.Items.Add(new { process = "LISCH", process_id = "6" });
            process_Choose.SelectedIndex = 0;
            #endregion

            sirasiz = lstView_Unsorted;

            elemanUretilen = new Eleman[randomElemanSayisi];
            elemanUretilen = item_producer(elemanUretilen, randomElemanSayisi);//baslangic dizisi
            addItems_To_ListView_HomeAdres(sirasiz, randomElemanSayisi, elemanUretilen);
            
        }

        #region Methodlar
        public int[] find_random_indis(Eleman[] elemanDolas_gelen)
        {
            int bos_say = 0;
            int[] boslar = new int[elemanDolas_gelen.Length];
            for (int i = 0; i < elemanDolas_gelen.Length; i++)
            {
                if (elemanDolas_gelen[i].Deger == -1)
                {
                    boslar[i] = i;
                    bos_say++;
                }
                else//dolu ise orayı -2 yap
                {
                    boslar[i] = -2;
                }
            }

            return boslar;
        }
        private Eleman[] item_producer(Eleman[] uretilecek, int elemanSayisi)
        {
            uretilecek = new Eleman[elemanSayisi];
            for (int i = 0; i < randomElemanSayisi; i++)
            {
                Eleman elemanEklenecek = new Eleman();
                try
                {
                    elemanEklenecek.Deger = temp.Next(min_value, max_value);
                }
                catch (Exception)
                {

                    MessageBox.Show("Max degerini min degerinden küçük girdiniz lütfen değerleri geçerli giriniz.");
                    this.Close();
                }
                

                elemanEklenecek.LinkAddress = "x";
                elemanEklenecek.HomeAddress = elemanEklenecek.Deger % modValue;

                uretilecek[i] = elemanEklenecek;
            }
            return uretilecek;
        }
        private ListView addItems_To_ListView(ListView gelenParametre, int elemanSayisi, Eleman[] eklenecekElemanListesi)
        {
            for (int i = 0; i < elemanSayisi; i++)
            {

                string[] subElemanlar = new string[] { i.ToString(), eklenecekElemanListesi[i].Deger.ToString(), eklenecekElemanListesi[i].LinkAddress.ToString(),eklenecekElemanListesi[i].PropKaydedilen.ToString() };

                ListViewItem sirasizElemanlar = new ListViewItem(subElemanlar);

                gelenParametre.Items.Add(sirasizElemanlar);

            }
            return gelenParametre;
        }
        private ListView addItems_To_ListView_EICH(ListView gelenParametre, int elemanSayisi, Eleman[] eklenecekElemanListesi,int primary,int overflow)
        {
            string[] subElemanlar;
            for (int i = 0; i < elemanSayisi; i++)
            {

                if (i<primary)
                {
                subElemanlar = new string[] { i.ToString(), eklenecekElemanListesi[i].Deger.ToString(), eklenecekElemanListesi[i].LinkAddress.ToString(), eklenecekElemanListesi[i].PropKaydedilen.ToString() };
                }
                else if (i==primary)
                {
                    subElemanlar = new string[] { "over", "flow", "start", "overflowstart"};
                }
                else 
                {
                    subElemanlar = new string[] { i.ToString(), eklenecekElemanListesi[i].Deger.ToString(), "x", eklenecekElemanListesi[i].PropKaydedilen.ToString() };
                }

                ListViewItem sirasizElemanlar = new ListViewItem(subElemanlar);

                gelenParametre.Items.Add(sirasizElemanlar);

            }
            return gelenParametre;
        }
        private ListView addItems_To_ListView_HomeAdres(ListView gelenParametre, int elemanSayisi, Eleman[] eklenecekElemanListesi)
        {
            for (int i = 0; i < elemanSayisi; i++)
            {

                string[] subElemanlar = new string[] { i.ToString(), eklenecekElemanListesi[i].Deger.ToString(), eklenecekElemanListesi[i].HomeAddress.ToString() };

                ListViewItem sirasizElemanlar = new ListViewItem(subElemanlar);

                gelenParametre.Items.Add(sirasizElemanlar);

            }
            return gelenParametre;
        }
        public Eleman[] algorithm_LISCH(Eleman[] baslangicDizi, Eleman[] elemanSunulacak)
        {
            #region EklemeBasarili
            elemanSunulacak = new Eleman[secondaryListCount];
            for (int i = 0; i < secondaryListCount; i++)
            {
                Eleman aktariciEleman = new Eleman();
                aktariciEleman.Deger = -1;
                aktariciEleman.HomeAddress = i % modValue;
                aktariciEleman.LinkAddress = "x";

                elemanSunulacak[i] = aktariciEleman;
            }
            #endregion
            
            for (int i = 0; i < baslangicDizi.Length; i++)
            {
                int probeEleman = 1;//probe değerini daha yerleştirirken buluyoruz.
                for (int k = 0; k < secondaryListCount; k++)
                {
                    int current_homeIndex = baslangicDizi[i].HomeAddress;
                    try
                    {
                        
                        if (elemanSunulacak[current_homeIndex].Deger == -1)//orada eleman yok ise
                        {
                            elemanSunulacak[current_homeIndex].Deger = baslangicDizi[i].Deger;
                            elemanSunulacak[current_homeIndex].PropKaydedilen = probeEleman;
                            k = secondaryListCount;
                        }

                        else if (elemanSunulacak[current_homeIndex].Deger != -1)//orada eleman var ise
                        {
                            bool yerbulundu_mu = false;
                            probeEleman++;

                            int yer_index = elemanSunulacak.Length - 1;

                            while (yerbulundu_mu == false)
                            {
                                if (elemanSunulacak[yer_index].Deger == -1)
                                {
                                    elemanSunulacak[yer_index].Deger = baslangicDizi[i].Deger;
                                    elemanSunulacak[yer_index].PropKaydedilen = probeEleman;

                                    #region Yerleştirme
                                    int link_adres_kontrol = baslangicDizi[i].HomeAddress;//belli home adresteki zincirin en altındaki verinin adres bilgisini aldım
                                                                                          //artık en son yeni bir eleman yerleştirildi ya 2 satır üstte , heh işte bu elemanı point edecek değer biraz önce adresini bulduğumuz eleman
                                    if (elemanSunulacak[link_adres_kontrol].LinkAddress == "x")//boş ise önce doldur
                                    {
                                        elemanSunulacak[current_homeIndex].LinkAddress = Convert.ToString(yer_index);//yer_index olacak
                                        elemanSunulacak[current_homeIndex].PropKaydedilen = probeEleman;
                                    }
                                    else if (elemanSunulacak[link_adres_kontrol].LinkAddress != "x")
                                    {
                                        while (elemanSunulacak[link_adres_kontrol].LinkAddress != "x")
                                        {
                                            link_adres_kontrol = Convert.ToInt32(elemanSunulacak[link_adres_kontrol].LinkAddress);
                                            probeEleman++;
                                        }

                                        elemanSunulacak[link_adres_kontrol].LinkAddress = Convert.ToString(yer_index);
                                        elemanSunulacak[link_adres_kontrol].PropKaydedilen = probeEleman;
                                    }
                                    #endregion

                                    yerbulundu_mu = true;
                                    k = secondaryListCount;

                                }
                                else if (elemanSunulacak[yer_index].Deger != -1)
                                {
                                    yer_index--;
                                }
                            }

                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lütfen mod değerini mod sonucu toplam eleman sayisinden fazla çıkacak şekilde girmeyiniz.");
                        this.Close();
                        
                    }
                    
                  
                }
            }

            return elemanSunulacak;

        }
        public Eleman[] algorithm_RLISCH(Eleman[] baslangicDizi, Eleman[] elemanSunulacak)
        {
            #region EklemeBasarili
            elemanSunulacak = new Eleman[secondaryListCount];
            for (int i = 0; i < secondaryListCount; i++)
            {
                Eleman aktariciEleman = new Eleman();
                aktariciEleman.Deger = -1;
                aktariciEleman.HomeAddress = i % modValue;
                aktariciEleman.LinkAddress = "x";

                elemanSunulacak[i] = aktariciEleman;
            }
            #endregion
            int probeEleman = 1;
            for (int i = 0; i < baslangicDizi.Length; i++)//Üretilen her bir elemanım için işlemleri yap.
            {
                for (int k = 0; k < secondaryListCount; k++)//Yapacağın ana işlem yerleştirilecek koşul tablonun tamamını baz alıp yer aramak.
                {
                    int current_homeIndex = baslangicDizi[i].HomeAddress;//O andaki Home adresteki değerler için işlem yapılacak.
                    try
                    { 
                    if (elemanSunulacak[current_homeIndex].Deger == -1)//orada eleman yok ise
                    {

                        elemanSunulacak[current_homeIndex].Deger = baslangicDizi[i].Deger;
                            elemanSunulacak[current_homeIndex].PropKaydedilen = probeEleman;
                        k = secondaryListCount;//yerleştirilme yapıldı. Yerleştirilecek diğer elemanı al.
                    }//Home adres boş olabilir.
                    else if (elemanSunulacak[current_homeIndex].Deger != -1)//orada eleman var ise
                    {
                        bool yerbulundu_mu = false;
                        probeEleman++;
                        int yer_index = elemanSunulacak.Length - 1;

                        Random rnd = new Random();
                        int random_yer = rnd.Next(0, yer_index);
                        int bos_adres = -2;
                        while (yerbulundu_mu == false)
                        {

                            if (bos_adres != -2)
                            {
                                random_yer = bos_adres;
                                probeEleman++;
                            }
                            if (elemanSunulacak[random_yer].Deger == -1)
                            {
                                elemanSunulacak[random_yer] = baslangicDizi[i];
                                    elemanSunulacak[random_yer].PropKaydedilen = probeEleman;
                               
                                #region Yerleştirme_RISCH
                                bool link_buldunmu = false;
                                int gidilecek_link = baslangicDizi[i].HomeAddress;
                                while (!link_buldunmu)
                                {
                                    if (elemanSunulacak[gidilecek_link].LinkAddress == "x")
                                    {
                                        elemanSunulacak[gidilecek_link].LinkAddress = random_yer.ToString();//Eğer 2. sıradaki eleman ise.
                                        link_buldunmu = true;
                                    }
                                    else
                                    {
                                        gidilecek_link = Convert.ToInt32(elemanSunulacak[gidilecek_link].LinkAddress);
                                    }
                                }
                                #endregion
                                yerbulundu_mu = true;
                                k = secondaryListCount;
                            }
                            else
                            {
                                int[] random_indisler = find_random_indis(elemanSunulacak);

                                rnd = new Random();
                                random_yer = rnd.Next(0, random_indisler.Length);

                                bos_adres = random_indisler[random_yer];
                                probeEleman++;
                            }
                        }


                    }//Home adres dolu olabilir.
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lütfen mod değerini mod sonucu toplam eleman sayisinden fazla çıkacak şekilde girmeyiniz.");
                        this.Close();
                    }

                }
            }

            return elemanSunulacak;

        }
        public Eleman[] algorithm_EISCH(Eleman[] baslangicDizi, Eleman[] elemanSunulacak)
        {
            #region EklemeBasarili
            elemanSunulacak = new Eleman[secondaryListCount];
            for (int i = 0; i < secondaryListCount; i++)
            {
                Eleman aktariciEleman = new Eleman();
                aktariciEleman.Deger = -1;
                aktariciEleman.HomeAddress = i % modValue;
                aktariciEleman.LinkAddress = "x";

                elemanSunulacak[i] = aktariciEleman;
            }
            #endregion
            
            for (int i = 0; i < baslangicDizi.Length; i++)
            {
                int probeEleman = 1;
                for (int k = 0; k < secondaryListCount; k++)
                {
                    int current_homeIndex = baslangicDizi[i].HomeAddress;
                    try
                    {
                    if (elemanSunulacak[current_homeIndex].Deger == -1 && current_homeIndex<=secondaryListCount)//orada eleman yok ise
                    {
                        
                            elemanSunulacak[current_homeIndex].Deger = baslangicDizi[i].Deger;
                            elemanSunulacak[current_homeIndex].PropKaydedilen = probeEleman;
                            k = secondaryListCount;
                            
                    }
                    else if (elemanSunulacak[current_homeIndex].Deger != -1)//orada eleman var ise
                    {
                        bool yerbulundu_mu = false;
                        probeEleman++;

                        int yer_index = elemanSunulacak.Length - 1;

                        while (yerbulundu_mu == false)
                        {
                            if (elemanSunulacak[yer_index].Deger == -1)
                            {
                                elemanSunulacak[yer_index].Deger = baslangicDizi[i].Deger;
                                    elemanSunulacak[yer_index].PropKaydedilen = probeEleman;
                                #region Yerleştirme_EISCH
                                bool link_buldunmu = false;
                                int gidilecek_link = baslangicDizi[i].HomeAddress;
                                while (!link_buldunmu)
                                {
                                    if (elemanSunulacak[gidilecek_link].LinkAddress == "x")
                                    {
                                        elemanSunulacak[gidilecek_link].LinkAddress = yer_index.ToString();//Eğer 2. sıradaki eleman ise.
                                        link_buldunmu = true;
                                    }
                                    else
                                    {
                                        elemanSunulacak[yer_index].LinkAddress = elemanSunulacak[baslangicDizi[i].HomeAddress].LinkAddress;
                                        elemanSunulacak[baslangicDizi[i].HomeAddress].LinkAddress = yer_index.ToString();
                                        link_buldunmu = true;
                                    }
                                }
                                #endregion


                                yerbulundu_mu = true;
                                k = secondaryListCount;

                            }
                            else if (elemanSunulacak[yer_index].Deger != -1)
                            {
                                yer_index--;
                                probeEleman++;

                            }
                        }

                    }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lütfen mod değerini mod sonucu toplam eleman sayisinden fazla çıkacak şekilde girmeyiniz.");
                        this.Close();

                    }
                }
            }

            return elemanSunulacak;

        }
        public Eleman[] algorithm_BEISCH(Eleman[] baslangicDizi, Eleman[] elemanSunulacak)
        {
            #region EklemeBasarili
            elemanSunulacak = new Eleman[secondaryListCount];
            for (int i = 0; i < secondaryListCount; i++)
            {
                Eleman aktariciEleman = new Eleman();
                aktariciEleman.Deger = -1;
                aktariciEleman.HomeAddress = i % modValue;
                aktariciEleman.LinkAddress = "x";

                elemanSunulacak[i] = aktariciEleman;
            }
            #endregion

            bool isBidirectional = false;
            int bidirectionalCount = 2;
            for (int i = 0; i < baslangicDizi.Length; i++)
            {
                int probeEleman = 1;
                for (int k = 0; k < secondaryListCount; k++)
                {
                    int current_homeIndex = baslangicDizi[i].HomeAddress;
                    try
                    {
                        if (elemanSunulacak[current_homeIndex].Deger == -1 && current_homeIndex <= secondaryListCount)//orada eleman yok ise
                        {
                            elemanSunulacak[current_homeIndex].Deger = baslangicDizi[i].Deger;
                            elemanSunulacak[current_homeIndex].PropKaydedilen = probeEleman;
                            k = secondaryListCount;
                        }
                        else if (elemanSunulacak[current_homeIndex].Deger != -1)//orada eleman var ise
                        {
                            bool yerbulundu_mu = false;
                            probeEleman++;

                            int yer_index = elemanSunulacak.Length - 1;

                            int yer_index_ust = 0;



                            while (yerbulundu_mu == false)
                            {
                                if (elemanSunulacak[yer_index].Deger == -1)//yer bulduysan yerleştir.
                                {
                                    elemanSunulacak[yer_index].Deger = baslangicDizi[i].Deger;
                                    elemanSunulacak[yer_index].PropKaydedilen = probeEleman;


                                    #region Yerleştirme_EISCH
                                    bool link_buldunmu = false;
                                    int counter_temizle = 0;
                                    int gidilecek_link = baslangicDizi[i].HomeAddress;
                                    while (!link_buldunmu)
                                    {
                                        if (elemanSunulacak[gidilecek_link].LinkAddress == "x")
                                        {
                                            elemanSunulacak[gidilecek_link].LinkAddress = yer_index.ToString();//Eğer 2. sıradaki eleman ise.
                                            link_buldunmu = true;
                                        }
                                        else
                                        {
                                            if (counter_temizle>=3)//aslında zincirin en altına düşecek olan değerin link'ini temizlemiş oluyorum çünkü eğer bunu yapmazsam 2.collusion olduğundaki linklediği adres üzerinde kalıyor
                                            {
                                                string temp = elemanSunulacak[baslangicDizi[i].HomeAddress].LinkAddress;
                                                elemanSunulacak[Convert.ToInt32(elemanSunulacak[baslangicDizi[i].HomeAddress].LinkAddress)].LinkAddress = "x";
                                                elemanSunulacak[yer_index].LinkAddress = temp;
                                                elemanSunulacak[baslangicDizi[i].HomeAddress].LinkAddress = yer_index.ToString();
                                                link_buldunmu = true;
                                            }
                                            else
                                            {
                                                string temp = elemanSunulacak[baslangicDizi[i].HomeAddress].LinkAddress;
                                                elemanSunulacak[yer_index].LinkAddress = temp;
                                                elemanSunulacak[baslangicDizi[i].HomeAddress].LinkAddress = yer_index.ToString();
                                                link_buldunmu = true;
                                            }
                                            
                                        }
                                        counter_temizle++;
                                    }
                                    #endregion


                                    yerbulundu_mu = true;
                                    k = secondaryListCount;
                                    bidirectionalCount++;
                                    if (bidirectionalCount % 2 == 1)
                                    {
                                        isBidirectional = true;
                                    }
                                    else if (bidirectionalCount % 2 == 0)
                                    {
                                        isBidirectional = false;
                                    }


                                }

                                else if (elemanSunulacak[yer_index].Deger != -1 && !isBidirectional)//yer bulamadıysan bidirectional'ın ilk aşaması olan alttan yer aramayı yap değiştirilen yeni yer için ilk koşul blogu çalışsın.
                                {
                                    yer_index--;
                                    probeEleman++;
                                }

                                else if (elemanSunulacak[yer_index].Deger != -1 && isBidirectional)////yer bulamadıysan bidirectional'ın ikinci aşaması olan üstten yer aramayı yap değiştirilen yeni yer için ilk koşul blogu çalışsın.
                                {
                                    yer_index = yer_index_ust-1;
                                    yer_index++;
                                    yer_index_ust++;
                                    probeEleman++;
                                }
                            }

                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lütfen mod değerini mod sonucu toplam eleman sayisinden fazla çıkacak şekilde girmeyiniz.");
                        this.Close();

                    }
                }
            }

            return elemanSunulacak;

        }
        public Eleman[] algorithm_EICH(Eleman[] baslangicDizi,Eleman[] elemanSunulacak,int degisenAlan)
        {
            int collision_count = 0;
            total_area = enyakin_Asal(modValue);
            if (degisenAlan!=0)
            {
                total_area = degisenAlan;
            }
            int primary_value = modValue;
            int overflow_value = total_area - modValue;
            
            Eleman[] primary = new Eleman[primary_value];
            Eleman[] overflow = new Eleman[overflow_value];

            primary = eleman_baslat(primary,primary_value);
            overflow = eleman_baslat(overflow, overflow_value);

            for (int i = 0; i < baslangicDizi.Length; i++)
            {
                int probeEleman = 1;
                for (int k = 0; k < total_area; k++)
                {
                    int current_homeIndex = baslangicDizi[i].HomeAddress;
                    try
                    {
                        if (primary[current_homeIndex].Deger == -1 && current_homeIndex <= primary_value)//orada eleman yok ise
                        {
                            primary[current_homeIndex].Deger = baslangicDizi[i].Deger;
                            primary[current_homeIndex].PropKaydedilen = probeEleman;
                            k = secondaryListCount;
                        }
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Lütfen mod değerini mod sonucu toplam eleman sayisinden fazla çıkacak şekilde girmeyiniz.");
                        this.Close();
                    }
                    
                }
            }

            return primary;
        }
        public Eleman[] eleman_baslat(Eleman[] baslatilacak, int count)
        {
            #region EklemeBasarili

            baslatilacak = new Eleman[count];
            for (int i = 0; i < count; i++)
            {
                Eleman aktariciEleman = new Eleman();
                aktariciEleman.Deger = -1;
                aktariciEleman.HomeAddress = i % modValue;
                aktariciEleman.LinkAddress = "x";

                baslatilacak[i] = aktariciEleman;
            }
            return baslatilacak;
            #endregion
        }
        #endregion
        public int enyakin_Asal(int gelenDeger)
        {
            int bak, kontrol_et, son = 0, asal_mı;
            for (bak = gelenDeger + 1; bak > gelenDeger; bak++)
            {
                asal_mı = 1;
                for (kontrol_et = 2; kontrol_et < bak; kontrol_et++)
                {

                    if (bak % kontrol_et == 0)
                    {
                        asal_mı = 0;

                        break;
                    }
                }

                if (asal_mı == 1)
                {
                    son = bak;
                    bak = 0;
                }
            }
            return son;
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            btnAra.Enabled = true;
            txtAra.Enabled = true;
            txtElemanSayisi.Enabled = true;
            txtMax.Enabled = true;
            txtMin.Enabled = true;
            txtMod.Enabled = true;
            rischPerformans = "";
            eischPerformans = "";
            lischPerformans = "";
            beischPerformans = "";
            
            if (islem_say >= 0)
            {
                if (txtMin.Text!=""||txtMax.Text!="")
                {
                    try
                    {
                        if (txtMin.Text!="")
                        {
                            min_value = Convert.ToInt32(txtMin.Text.ToString());
                        }
                        if (txtMax.Text != "")
                        {
                            max_value = Convert.ToInt32(txtMax.Text.ToString());
                        }
                       
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Lütfen Max ve Min değerlerini sayi olarak giriniz.");
                    }
                }
                if (txtMod.Text!="")
                {
                    try
                    {
                        modValue = Convert.ToInt32(txtMod.Text.ToString());
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Lütfen Mod değerini sayi olarak giriniz.");
                    }
                }
                if (txtElemanSayisi.Text!="")
                {
                    try
                    {
                        randomElemanSayisi = Convert.ToInt32(txtElemanSayisi.Text.ToString());
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Lütfen Eleman Sayisi değerini sayi olarak giriniz.");
                    }
                }
                else if (txtElemanSayisi.Text=="")
                {
                randomElemanSayisi = temp.Next(15, 20);
                }
                packingFactor = temp.Next(93, 96);
                secondaryListCount = (randomElemanSayisi * 100) / packingFactor+1;
                


                lstView_Unsorted.Items.Clear();
                listViewSirali.Items.Clear();
                lstViewEISCH.Items.Clear();
                lstViewBEISCH.Items.Clear();
                lstViewRISCH.Items.Clear();
                lstViewEich.Items.Clear();
                elemanUretilen = new Eleman[randomElemanSayisi];
                elemanUretilen = item_producer(elemanUretilen, randomElemanSayisi);

                sirasiz = lstView_Unsorted;
                lstLISCH = listViewSirali;
                lstEISCH = lstViewEISCH;
                lstRISCH = lstViewRISCH;
                lstBEISCH = lstViewBEISCH;
                lstEICH = lstViewEich;

                
                addItems_To_ListView_HomeAdres(sirasiz, randomElemanSayisi, elemanUretilen);

                Eleman[] eleman_to_sirali;

                switch (cmbChoose.Text)
                {
                    case "LISCH":
                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_lish.Start();
                        eleman_to_sirali = algorithm_LISCH(elemanUretilen, eleman_to_sirali);
                        st_lish.Stop();
                        lischPerformans = st_lish.ElapsedMilliseconds.ToString();
                        addItems_To_ListView(lstLISCH, secondaryListCount, eleman_to_sirali);//LISCH
                        break;
                    case "RLISCH":
                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_rlish.Start();
                        eleman_to_sirali = algorithm_RLISCH(elemanUretilen, eleman_to_sirali);
                        st_rlish.Stop();
                        rischPerformans = st_rlish.ElapsedMilliseconds.ToString();
                        addItems_To_ListView(lstRISCH, secondaryListCount, eleman_to_sirali);//RISCH
                        break;
                    case "EISCH":
                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_eish.Start();
                        eleman_to_sirali = algorithm_EISCH(elemanUretilen, eleman_to_sirali);
                        st_eish.Stop();
                        eischPerformans = st_eish.ElapsedMilliseconds.ToString();
                        addItems_To_ListView(lstEISCH, secondaryListCount, eleman_to_sirali);//EISCH
                        break;
                    case "BEISCH":
                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_beish.Start();
                        eleman_to_sirali = algorithm_BEISCH(elemanUretilen, eleman_to_sirali);
                        st_beish.Stop();
                        beischPerformans =st_beish.ElapsedMilliseconds.ToString();
                        addItems_To_ListView(lstBEISCH, secondaryListCount, eleman_to_sirali);//BEISCH
                        break;
                    case "EICH":
                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_eish.Start();
                        eleman_to_sirali = algorithm_EISCH(elemanUretilen, eleman_to_sirali);
                        st_eish.Stop();
                        eischPerformans = st_eish.ElapsedMilliseconds.ToString();
                        addItems_To_ListView_EICH(lstEICH, secondaryListCount, eleman_to_sirali, modValue, enyakin_Asal(modValue) - modValue);
                        break;
                    case "HEPSİ":
                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_lish.Start();
                        eleman_to_sirali = algorithm_LISCH(elemanUretilen, eleman_to_sirali);
                        st_lish.Stop();
                        lischPerformans = st_lish.Elapsed.ToString();
                        addItems_To_ListView(lstLISCH, secondaryListCount, eleman_to_sirali);//LISCH

                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_eish.Start();
                        eleman_to_sirali = algorithm_EISCH(elemanUretilen, eleman_to_sirali);
                        st_eish.Stop();
                        eischPerformans = st_eish.Elapsed.ToString();
                        addItems_To_ListView(lstEISCH, secondaryListCount, eleman_to_sirali);//EISCH

                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_beish.Start();
                        eleman_to_sirali = algorithm_BEISCH(elemanUretilen, eleman_to_sirali);
                        st_beish.Stop();
                        beischPerformans = st_beish.Elapsed.ToString();
                        addItems_To_ListView(lstBEISCH, secondaryListCount, eleman_to_sirali);//BEISCH

                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_rlish.Start();
                        eleman_to_sirali = algorithm_RLISCH(elemanUretilen, eleman_to_sirali);
                        st_rlish.Stop();
                        rischPerformans = st_rlish.Elapsed.ToString();
                        addItems_To_ListView(lstRISCH, secondaryListCount, eleman_to_sirali);//RISCH

                        eleman_to_sirali = new Eleman[secondaryListCount];
                        st_eish.Start();
                        eleman_to_sirali = algorithm_EISCH(elemanUretilen, eleman_to_sirali);
                        st_eish.Stop();
                        eischPerformans = st_eish.Elapsed.ToString();
                        addItems_To_ListView_EICH(lstEICH, secondaryListCount, eleman_to_sirali, modValue, enyakin_Asal(modValue) - modValue);

                        break;
                    default:
                        break;
                }

            }

          


            lblPackingFactorValue.Text = packingFactor.ToString();

            
            islem_say++;
            
        }
       
        #region Arama_İşlemleri
        private void btnAra_Click(object sender, EventArgs e)
        {
            try
            {
                int aranacakDeger = Convert.ToInt32(txtAra.Text.ToString());
                try
                {
                    int probeReturnLisch = probeBul(aranacakDeger, aranacakDeger % modValue, lstLISCH);
                    int probeReturnEisch = probeBul(aranacakDeger, aranacakDeger % modValue, lstEISCH);
                    int probeReturnBeisch = probeBul(aranacakDeger, aranacakDeger % modValue, lstBEISCH);
                    int probeReturnRisch = probeBul(aranacakDeger, aranacakDeger % modValue, lstRISCH);
                    MessageBox.Show("Probe LISCH:  " + probeReturnLisch.ToString() + "\n" + "Probe EISCH:  " + probeReturnEisch.ToString() + "\n" + "Probe BEISCH:  " + probeReturnBeisch.ToString() + "\n" + "Probe RISCH:  " + probeReturnRisch.ToString());
                }
                catch (Exception)
                {
                    MessageBox.Show("Değer bulunamadı.");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Lütfen aranacak değer giriniz.");
            }
        }
        public int probeBul(int aranacakEleman,int aranacakHomeAddress,ListView aranacakList)
        {
            bool bulundu_mu = false;
            bool sona_geldimi = false;
            int i = aranacakHomeAddress;
            int k = 1;
            int probe = 0;
            while (!bulundu_mu&&!sona_geldimi)
            {
                if (i <aranacakList.Items.Count)
                {
                    if (aranacakEleman == Convert.ToInt32(aranacakList.Items[i].SubItems[k].Text.ToString()))
                    {
                        bulundu_mu = true;
                    }
                    else
                    {
                        i = Convert.ToInt32(aranacakList.Items[i].SubItems[k + 1].Text.ToString());
                    }
                    probe++;
                }
                else if(i == aranacakList.Items.Count)
                {
                    sona_geldimi = true;//islem basarisiz.
                }
            }
            return probe;
        }
        public int probeBul(Eleman[] elemanDizisi, Eleman aranacakEleman)
        {
            int counter = 1;
            int link_Degeri = aranacakEleman.HomeAddress;
            bool buldunmu = false;
            int i = 0;

            while (!buldunmu && elemanDizisi.Length != i)
            {
                if (aranacakEleman.Deger == elemanDizisi[link_Degeri].Deger)
                {
                    buldunmu = true;
                }
                else
                {
                    link_Degeri = Convert.ToInt32(elemanDizisi[link_Degeri].LinkAddress);
                    counter++;

                }
            }

            return counter;

        }

        public double fullProbeBul(ListView aranacakList)
        {
            double fullProbeValue = 0;
            int k = 1;
            int[] dizi = new int[aranacakList.Items.Count];

            for (int i = 0; i < aranacakList.Items.Count; i++)
            {
                if (Convert.ToInt32(aranacakList.Items[i].SubItems[k].Text.ToString())!=-1)
                {
                    int siradakiAranan = Convert.ToInt32(aranacakList.Items[i].SubItems[k].Text.ToString());
                    //dizi[i] = probeBul(siradakiAranan, siradakiAranan % modValue, lstRISCH);
                    int temp= probeBul(siradakiAranan, siradakiAranan % modValue, lstRISCH);
                    fullProbeValue += temp;
                }
            }
            return fullProbeValue/aranacakList.Items.Count;
        }

        #endregion
        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPerformans_Click(object sender, EventArgs e)
        {
            rischPerformans = "";
            eischPerformans = "";
            lischPerformans = "";
            beischPerformans = "";
            btnSort_Click(sender, e);
            if (lstLISCH.Items.Count!=0&&lstBEISCH.Items.Count != 0 && lstRISCH.Items.Count != 0 && lstEISCH.Items.Count != 0)
            {
                lblLischAverage.Text = lischPerformans.ToString();
                lblBeischAverage.Text = beischPerformans.ToString();
                lblEischAverage.Text = eischPerformans.ToString();
                lblRischAverage.Text = rischPerformans.ToString();
            }
            else
            {
                MessageBox.Show("Tüm yöntemler için sıralama yaptıktan sonra performans değerleri için ölçüm yapabilirsiniz.");
            }

        }
    }
}
