using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Student
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Email { get; set; }
    public int OgrenciNo { get; set; }
}

class Program
{
    private const string DataFile = "ogrenciler.json";
    private static List<Student> students = new List<Student>();

    static void Main()
    {
        YukleogrenciListesi();
        
        while (true)
        {
            Console.WriteLine("\n========== Öğrenci Yönetim Sistemi ==========");
            Console.WriteLine("1. Öğrenci Ekle");
            Console.WriteLine("2. Öğrenci Listele");
            Console.WriteLine("3. Öğrenci Sil");
            Console.WriteLine("4. Öğrenci Güncelle");
            Console.WriteLine("5. Çıkış");
            Console.Write("Seçiminiz: ");
            
            string secim = Console.ReadLine();
            
            switch (secim)
            {
                case "1":
                    OgrenciEkle();
                    break;
                case "2":
                    OgrenciListele();
                    break;
                case "3":
                    OgrenciSil();
                    break;
                case "4":
                    OgrenciGuncelle();
                    break;
                case "5":
                    Console.WriteLine("Programdan çıkılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim!");
                    break;
            }
        }
    }

    static void OgrenciEkle()
    {
        Console.WriteLine("\n--- Yeni Öğrenci Ekle ---");
        
        Console.Write("Ad: ");
        string ad = Console.ReadLine();
        
        Console.Write("Soyadı: ");
        string soyad = Console.ReadLine();
        
        Console.Write("Email: ");
        string email = Console.ReadLine();
        
        Console.Write("Öğrenci No: ");
        if (!int.TryParse(Console.ReadLine(), out int ogrenciNo))
        {
            Console.WriteLine("Geçersiz öğrenci numarası!");
            return;
        }

        Student yeniOgrenci = new Student
        {
            Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1,
            Ad = ad,
            Soyad = soyad,
            Email = email,
            OgrenciNo = ogrenciNo
        };

        students.Add(yeniOgrenci);
        KaydetOgrenciListesi();
        Console.WriteLine("Öğrenci başarıyla eklendi!");
    }

    static void OgrenciListele()
    {
        Console.WriteLine("\n--- Öğrenci Listesi ---");
        
        if (students.Count == 0)
        {
            Console.WriteLine("Kayıtlı öğrenci yok.");
            return;
        }

        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"{"ID",-5} {"Ad",-15} {"Soyadı",-15} {"Email",-25} {"Öğrenci No",-10}");
        Console.WriteLine(new string('-', 80));
        
        foreach (var ogrenci in students)
        {
            Console.WriteLine($"{ogrenci.Id,-5} {ogrenci.Ad,-15} {ogrenci.Soyad,-15} {ogrenci.Email,-25} {ogrenci.OgrenciNo,-10}");
        }
        
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Toplam öğrenci: {students.Count}");
    }

    static void OgrenciSil()
    {
        Console.WriteLine("\n--- Öğrenci Sil ---");
        
        OgrenciListele();
        
        Console.Write("Silinecek öğrencinin ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID!");
            return;
        }

        Student silinecek = students.FirstOrDefault(s => s.Id == id);
        if (silinecek != null)
        {
            students.Remove(silinecek);
            KaydetOgrenciListesi();
            Console.WriteLine("Öğrenci başarıyla silindi!");
        }
        else
        {
            Console.WriteLine("Öğrenci bulunamadı!");
        }
    }

    static void OgrenciGuncelle()
    {
        Console.WriteLine("\n--- Öğrenci Güncelle ---");
        
        OgrenciListele();
        
        Console.Write("Güncellenecek öğrencinin ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID!");
            return;
        }

        Student ogrenci = students.FirstOrDefault(s => s.Id == id);
        if (ogrenci == null)
        {
            Console.WriteLine("Öğrenci bulunamadı!");
            return;
        }

        Console.Write("Yeni Ad (Şimdiki: " + ogrenci.Ad + "): ");
        string yeniAd = Console.ReadLine();
        if (!string.IsNullOrEmpty(yeniAd))
            ogrenci.Ad = yeniAd;

        Console.Write("Yeni Soyadı (Şimdiki: " + ogrenci.Soyad + "): ");
        string yeniSoyad = Console.ReadLine();
        if (!string.IsNullOrEmpty(yeniSoyad))
            ogrenci.Soyad = yeniSoyad;

        Console.Write("Yeni Email (Şimdiki: " + ogrenci.Email + "): ");
        string yeniEmail = Console.ReadLine();
        if (!string.IsNullOrEmpty(yeniEmail))
            ogrenci.Email = yeniEmail;

        Console.Write("Yeni Öğrenci No (Şimdiki: " + ogrenci.OgrenciNo + "): ");
        string noInput = Console.ReadLine();
        if (int.TryParse(noInput, out int yeniNo))
            ogrenci.OgrenciNo = yeniNo;

        KaydetOgrenciListesi();
        Console.WriteLine("Öğrenci başarıyla güncellendi!");
    }

    static void YukleogrenciListesi()
    {
        if (File.Exists(DataFile))
        {
            try
            {
                string json = File.ReadAllText(DataFile);
                students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            }
            catch
            {
                Console.WriteLine("Dosya yüklenirken hata oluştu. Yeni liste oluşturulacak.");
                students = new List<Student>();
            }
        }
    }

    static void KaydetOgrenciListesi()
    {
        try
        {
            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DataFile, json);
        }
        catch
        {
            Console.WriteLine("Dosya kaydedilirken hata oluştu!");
        }
    }
}
