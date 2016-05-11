using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metinyaslama
{
    class Program
    {
        static void Main(string[] args)
        {
            int satirUzunlugu = 15; 
            int greedyMidinamikMi = 0;

            Console.WriteLine("Greedy ile çözülmesini istiyorsanız 0  ");
            Console.WriteLine("Dinamik programlama ile çözülmesini istiyorsanız 1 ");
            Console.Write("Yazınız -> ");
            greedyMidinamikMi = Int32.Parse(Console.ReadLine());

            Console.Write("Satir uzunlugunu giriniz : ");
            satirUzunlugu = Int32.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.Write("Dosya yolunu yaziniz : ");
            string dy = Console.ReadLine();
            string dosyaYolu = @dy;
            FileStream fs = new FileStream(dosyaYolu, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs, Encoding.GetEncoding("iso-8859-9"));

            //Dosyadan verileri diziye alma işlemi 
            List<string> metin = new List<string>();
            string yazi = sw.ReadLine();
            while (yazi != null)
            {

                string[] dizi = yazi.Split(' ');
                for (int i = 0; i < dizi.Length; i++)
                {
                    metin.Add(dizi[i]);
                }
                yazi = sw.ReadLine();

            }
            sw.Close();
            fs.Close();

            //diziler üzerinde işlem yapacağımızdan listdeki veriyi bir diziye alıyoruz
            string[] girdiMetin = new string[metin.Count];
            int index = 0;
            foreach (string eleman in metin)
            {
                if (index != metin.Count)
                    girdiMetin[index++] = eleman;
            }

            //girdi metnimizizn kelimelerini kaç karakterse ekrana basıyoruz
            foreach (string eleman in girdiMetin)
            {
                Console.Write(eleman);
                Console.WriteLine("---" + eleman.Length);
            }
            

            //çözüm için greedy yöntemi seçilmiş ise buradan devam ediyoruz
            if (greedyMidinamikMi == 0)
            {
                List<string> ciktimetnim = new List<string>();
                string str;
                int indis = 0;
                for (int i = 0; i < girdiMetin.Length; i++)
                {

                    str = "";
                    //hangi kelimeye kadar gidilecek
                    for (int j = i; j < girdiMetin.Length; j++)
                    {
                        //elimizdeki kelimeler + bosluk ve sonraki kelime satırdan taşıyormu kontrol ediyoruz
                        int _uzunluk = str.Length + girdiMetin[j].Length;
                        if (_uzunluk <= satirUzunlugu)
                        {
                            str += girdiMetin[j] + " ";
                            indis = j + 1; //kaldığımız kelimeden sonraki kelimeyi satır başı kelimesi olması için indis atıyoruz
                        }
                        else
                        {
                            break;
                        }

                    }

                    str = "";
                    //yazılacak kelime sayısını ve boşluk için str yi buluyoruz
                    int kelimesayisi = 0;
                    for (int k = i; k < indis; k++)
                    {
                        str += girdiMetin[k];
                        kelimesayisi++;
                    }
                    int boslukSayisi = satirUzunlugu - str.Length;

                    if (kelimesayisi == 1)
                    {
                        str = girdiMetin[i];
                        for (int t = 0; t < boslukSayisi; t++)
                        {
                            str += " ";
                        }
                    }
                    else if (kelimesayisi == 2)
                    {
                        str = girdiMetin[i];
                        for (int t = 0; t < boslukSayisi; t++)
                        {
                            str += " ";
                        }
                        str += girdiMetin[i + 1];
                    }
                    else
                    {
                        //elimizde en az 3 kelimemiz var 
                        //ortalama boşluk sayısını buluyoruz
                        int ortalamaBoslukSayisi = boslukSayisi / (kelimesayisi - 1);
                        int ekstraBosluk = boslukSayisi % (kelimesayisi - 1); //ekstra için boşluk aralığına göre mod alıyoruz
                        str = girdiMetin[i];
                        for (int t = 1; t < kelimesayisi; t++)
                        {
                            for (int k = 0; k < ortalamaBoslukSayisi; k++)
                            {
                                str += " ";
                            }
                            if (ekstraBosluk > 0)
                            {
                                str += " ";
                                ekstraBosluk--;
                            }
                            str += girdiMetin[i + t];

                        }

                    }

                    ciktimetnim.Add(str);
                    i = indis - 1;
                }

                //greedynin çözüm metnini yazdırıyoruz
                foreach (string eleman in ciktimetnim)
                {
                    Console.WriteLine(eleman);
                }

            }  //Dinamik programlama çözümü seçilmiş ise buradan devam ediyoruz 
            else if (greedyMidinamikMi == 1)
            {
                int boyut = girdiMetin.Length;
                int[,] matris = new int[boyut, boyut];
                //matrisi oluşturduk ilk değer olarak heryere -1 atadık
                for (int i = 0; i < girdiMetin.Length; i++)
                {
                    for (int j = 0; j < girdiMetin.Length; j++)
                    {
                        matris[i, j] = -1;

                    }
                }

                //matrisin içini dolduruyoruz
                for (int i = 0; i < girdiMetin.Length; i++)
                {
                    int oncekiUzunluk = 0;
                    for (int j = 0; j < girdiMetin.Length; j++)
                    {
                        if (i > j)
                        {
                            //kelimenin önündeki kelimeler olduğundan onları hesaba katmıyoruz
                            matris[i, j] = -2;
                        }
                        else if (j >= i)
                        {

                            int uzunluk = satirUzunlugu - (girdiMetin[j].Length + oncekiUzunluk);

                            if (uzunluk <= satirUzunlugu && uzunluk >= 0)
                            {
                                matris[i, j] = uzunluk * uzunluk;
                                oncekiUzunluk += girdiMetin[j].Length + 1;

                            }
                            else
                            {
                                break;
                            }


                        }

                    }
                    Console.WriteLine();
                }

                //matrisi bulmasonu

                //matris yazdırma
                for (int i = 0; i < girdiMetin.Length; i++)
                {
                    for (int j = 0; j < girdiMetin.Length; j++)
                    {
                        Console.Write("     " + matris[i, j]);

                    }
                    Console.WriteLine();
                }
                //matris yazdırma sonu

                //çözüm ve indis dizisi bulma 
                int[] cozumDizisi = new int[girdiMetin.Length + 1];
                int[] indisDizisi = new int[girdiMetin.Length];

                for (int i = girdiMetin.Length - 1; i >= 0; i--)
                {
                    int enkucuk = Int32.MaxValue;

                    for (int j = girdiMetin.Length - 1; j >= 0; j--)
                    {
                        if (i > j)
                        {
                            break;
                        }
                        else if (matris[i, j] == -1)
                        {
                            continue;
                        }
                        else
                        {
                            if (cozumDizisi[j + 1] + matris[i, j] < enkucuk)
                            {
                                enkucuk = cozumDizisi[j + 1] + matris[i, j];
                                cozumDizisi[i] = enkucuk;
                                indisDizisi[i] = j + 1;
                            }
                        }


                    }
                }
                Console.Write("Çözüm dizisi : ");
                foreach (int eleman in cozumDizisi)
                {
                    Console.Write(eleman + "-");
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("indis dizisi : ");
                foreach (int eleman in indisDizisi)
                {
                    Console.Write(eleman + "-");
                }
                Console.WriteLine();

                // çözüm ve indis dizisi bulma sonu


                //metni oluşturma
                Console.WriteLine();
                Console.WriteLine("Cıktı metni");
                Console.WriteLine();
                for (int j = 0; j < satirUzunlugu; j++)
                {
                    Console.Write(j % 10);
                }
                Console.WriteLine();
                
                List<string> ciktimetnim2 = new List<string>();
                for (int i = 0; i < girdiMetin.Length; i++)
                {
                    string str = "";
                    int kelimesayisi = 0;

                    //bosluk ve kelime sayısını bulduruyoruz
                    for (int j = i; j < indisDizisi[i]; j++)
                    {
                        str += girdiMetin[j];
                        kelimesayisi++;
                    }
                    int boslukSayisi = satirUzunlugu - str.Length;

                    if (kelimesayisi == 1)
                    {
                        str = girdiMetin[i];
                        for (int t = 0; t < boslukSayisi; t++)
                        {
                            str += " ";
                        }
                    }
                    else if (kelimesayisi == 2)
                    {
                        str = girdiMetin[i];
                        for (int t = 0; t < boslukSayisi; t++)
                        {
                            str += " ";
                        }
                        str += girdiMetin[i + 1];
                    }
                    else
                    {
                        int ortalamaBoslukSayisi = boslukSayisi / (kelimesayisi - 1);
                        int ekstraBosluk = boslukSayisi % (kelimesayisi - 1); //ekstra için bosluk aralığına göre mod alıyoruz
                        str = girdiMetin[i];
                        for (int t = 1; t < kelimesayisi; t++)
                        {
                            for (int k = 0; k < ortalamaBoslukSayisi; k++)
                            {
                                str += " ";
                            }
                            if (ekstraBosluk > 0)
                            {
                                str += " ";
                                ekstraBosluk--;
                            }
                            str += girdiMetin[i + t];

                        }

                    }

                    ciktimetnim2.Add(str);

                    i = indisDizisi[i] - 1;
                }


                //son çıktımızı yazdırıyoruzz
                foreach (string eleman in ciktimetnim2)
                {
                    Console.WriteLine(eleman);
                }

            }//dinamik sonu
            else
            {
                Console.WriteLine("Çözüm seçilmedi");
            }

            Console.ReadKey();
        }
    }
}