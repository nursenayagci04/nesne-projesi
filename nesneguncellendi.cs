using System;
using System.Collections.Generic;

namespace CalisanTakip
{
    public class Calisan
    {
        private string ad;
        private string soyad;
        private int yas;
        private string departman;
        private string pozisyon;
        private int izinGunleri;
        private string tcKimlikNo;
        private string sifre;
        private bool yoneticiMi;

        public string Ad => ad;
        public string Soyad => soyad;
        public int Yas => yas;
        public string Departman => departman;
        public string Pozisyon => pozisyon;
        public int KalanIzinGunleri => izinGunleri;
        public string TcKimlikNo => tcKimlikNo;
        public bool YoneticiMi => yoneticiMi;

        public Calisan(string ad, string soyad, int yas, string departman, string pozisyon, string tcKimlikNo, string sifre, int izinGunleri = 20, bool yoneticiMi = false)
        {
            if (yas <= 0)
            {
                Console.WriteLine("Hata: Yaş 0 veya negatif olamaz.");
                return;
            }
            this.ad = ad;
            this.soyad = soyad;
            this.yas = yas;
            this.departman = departman;
            this.pozisyon = pozisyon;
            this.tcKimlikNo = tcKimlikNo;
            this.sifre = sifre;
            this.izinGunleri = izinGunleri;
            this.yoneticiMi = yoneticiMi;
        }

        public void IzinGunleriEkle(int gun)
        {
            if (gun < 0)
            {
                Console.WriteLine("Hata: Gün sayısı negatif olamaz.");
                return;
            }
            izinGunleri += gun;
        }

        public bool IzinKullan(int gun)
        {
            if (gun <= 0)
            {
                Console.WriteLine("Hata: İzin günü 0 veya negatif olamaz.");
                return false;
            }
            if (gun > izinGunleri)
            {
                Console.WriteLine("Hata: Yeterli izin gününüz yok. Kalan izin günleriniz: " + izinGunleri);
                return false;
            }
            izinGunleri -= gun;
            Console.WriteLine("İzin talebiniz onaylandı. Kalan izin günleriniz: " + izinGunleri);
            Console.WriteLine("İyi günler!");
            CizKalp(); // İzin onaylandıktan sonra ikinci kalp çizimi
            return true;
        }

        public bool SifreDogrula(string sifre)
        {
            return this.sifre == sifre;
        }

        public override string ToString()
        {
            return "Ad: " + Ad + " " + Soyad + ", Yaş: " + Yas + ", Departman: " + Departman + ", Pozisyon: " + Pozisyon + ", Yönetici Mi: " + (YoneticiMi ? "Evet" : "Hayır");
        }

        public static void CizKalp()
        {
            Console.WriteLine("   ****     ****   ");
            Console.WriteLine("  ******   ******  ");
            Console.WriteLine(" ******** ******** ");
            Console.WriteLine(" ***************** ");
            Console.WriteLine("  ***************  ");
            Console.WriteLine("   *************   ");
            Console.WriteLine("    ***********    ");
            Console.WriteLine("     *********     ");
            Console.WriteLine("      *******      ");
            Console.WriteLine("       *****       ");
            Console.WriteLine("        ***        ");
            Console.WriteLine("         *         ");
        }
    }

    public class CalisanYonetici
    {
        private List<Calisan> calisanlar = new List<Calisan>();

        public void CalisanEkle(Calisan calisan)
        {
            if (calisan == null)
            {
                Console.WriteLine("Hata: Geçersiz çalışan nesnesi.");
                return;
            }
            calisanlar.Add(calisan);
        }

        public void GirisYap(string ad, string soyad, string sifre)
        {
            foreach (var calisan in calisanlar)
            {
                if (calisan.Ad.Equals(ad, StringComparison.OrdinalIgnoreCase) &&
                    calisan.Soyad.Equals(soyad, StringComparison.OrdinalIgnoreCase))
                {
                    if (calisan.SifreDogrula(sifre))
                    {
                        Console.Clear();
                        Console.WriteLine("Giriş başarılı!");
                        Console.WriteLine("------------------------------------------------------");
                        Console.WriteLine(calisan);

                        // Eğer çalışan yönetici değilse, yöneticinin onayını istiyoruz
                        if (!calisan.YoneticiMi)
                        {
                            Console.WriteLine("------------------------------------------------------");
                            Console.WriteLine("İzin talebiniz için yönetici onayı gerekmektedir.");
                            // Çalışana izin kullanmak istediği gün sayısını soruyoruz
                            Console.Write("Kaç gün izin kullanmak istiyorsunuz?: ");
                            int izinGun;
                            if (int.TryParse(Console.ReadLine(), out izinGun) && izinGun > 0)
                            {
                                // Yöneticinin onayını alıyoruz
                                Console.WriteLine("------------------------------------------------------");
                                Console.WriteLine("İzin talebiniz yöneticinize sunulacaktır.");
                                Console.Write("Yönetici adı: ");
                                string yoneticiAd = Console.ReadLine();
                                Console.Write("Yönetici soyadı: ");
                                string yoneticiSoyad = Console.ReadLine();
                                Console.Write("Yönetici şifresi: ");
                                string yoneticiSifre = Program.SifreGizle();

                                // Yönetici kontrolünü yapıyoruz
                                Calisan yonetici = calisanlar.Find(c => c.YoneticiMi && c.Ad.Equals(yoneticiAd, StringComparison.OrdinalIgnoreCase) && c.Soyad.Equals(yoneticiSoyad, StringComparison.OrdinalIgnoreCase));
                                if (yonetici != null && yonetici.SifreDogrula(yoneticiSifre))
                                {
                                    Console.WriteLine("------------------------------------------------------");
                                    Console.WriteLine($"{yonetici.Ad} {yonetici.Soyad} onayı bekleniyor.");

                                    Console.Write("Yönetici izin onayını veriyor mu? (evet/hayır): ");
                                    string onay = Console.ReadLine().ToLower();

                                    if (onay == "evet")
                                    {
                                        // Yöneticiden onay alındı, izni onaylıyoruz
                                        calisan.IzinKullan(izinGun);
                                    }
                                    else
                                    {
                                        Console.WriteLine("İzin reddedildi.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Yönetici bulunamadı veya şifre yanlış.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Hatalı gün sayısı girdiniz.");
                            }
                        }
                        else
                        {
                            // Eğer çalışan bir yönetici ise, izin direkt onaylanabilir
                            Console.Write("Kaç gün izin kullanmak istiyorsunuz?: ");
                            int gun;
                            if (int.TryParse(Console.ReadLine(), out gun) && gun > 0)
                            {
                                calisan.IzinKullan(gun);
                            }
                            else
                            {
                                Console.WriteLine("Hatalı gün sayısı girdiniz.");
                            }
                        }
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Hata: Şifre yanlış.");
                        return;
                    }
                }
            }
            Console.WriteLine("Hata: Çalışan bulunamadı.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hoşgeldiniz!");
            Calisan.CizKalp();
            Console.WriteLine("------------------------------------------------------");

            CalisanYonetici yonetici = new CalisanYonetici();
            var calisan1 = new Calisan("Nehir", "Saygılı", 20, "Mühendislik", "Kıdemli Mühendis", "12345678901", "333", 60, true);
            var calisan2 = new Calisan("Nursena", "Yağcı", 20, "Pazarlama", "Yönetici", "98765432109", "777", 60, true);
            var calisan3 = new Calisan("Ekin", "Koç", 35, "Muhasebe", "Uzman", "11223344556", "1992", 30);
            var calisan4 = new Calisan("Boran", "Kuzum", 32, "İK", "İK Uzmanı", "99887766554", "1881", 18);
            var calisan5 = new Calisan("Burçin", "Terzioğlu", 44, "Bilişim", "Kıdemli Yazılım Geliştirici", "55667788990", "5678", 45);

            yonetici.CalisanEkle(calisan1);
            yonetici.CalisanEkle(calisan2);
            yonetici.CalisanEkle(calisan3);
            yonetici.CalisanEkle(calisan4);
            yonetici.CalisanEkle(calisan5);

            Console.WriteLine("Giriş yapmak için adınızı, soyadınızı ve şifrenizi giriniz:");
            Console.Write("Ad: ");
            string ad = Console.ReadLine();
            Console.Write("Soyad: ");
            string soyad = Console.ReadLine();
            Console.Write("Şifre: ");
            string sifre = SifreGizle();

            yonetici.GirisYap(ad, soyad, sifre);
        }

        public static string SifreGizle()
        {
            string sifre = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (sifre.Length > 0)
                    {
                        sifre = sifre.Substring(0, sifre.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    sifre += key.KeyChar;
                    Console.Write("*");
                }
            }
            return sifre;
        }
    }
}

