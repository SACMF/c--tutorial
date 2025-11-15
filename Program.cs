using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// Öğrenci sınıfı
public class Student
{
    public int Id { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? Email { get; set; }
    public int OgrenciNo { get; set; }
}

// Öğrenci işlemleri
public class StudentManager
{
    private const string DataFile = "ogrenciler.json";
    private List<Student> students = new List<Student>();

    // Verileri dosyadan yükle
    public void Yukle()
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
                Console.WriteLine("Dosya yüklenirken hata oluştu.");
                students = new List<Student>();
            }
        }
    }

    // Verileri dosyaya kaydet
    public void Kaydet()
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

    // Öğrenci ekle
    public void EkleOgrenci(string? ad, string? soyad, string? email, int ogrenciNo)
    {
        Student yeniOgrenci = new Student
        {
            Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1,
            Ad = ad,
            Soyad = soyad,
            Email = email,
            OgrenciNo = ogrenciNo
        };

        students.Add(yeniOgrenci);
        Kaydet();
        Console.WriteLine("Öğrenci başarıyla eklendi!");
    }

    // Öğrenci listesini göster
    public void ListeleOgrenciler()
    {
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

    // Öğrenci sil
    public void SilOgrenci(int id)
    {
        Student? silinecek = students.FirstOrDefault(s => s.Id == id);
        if (silinecek != null)
        {
            students.Remove(silinecek);
            Kaydet();
            Console.WriteLine("Öğrenci başarıyla silindi!");
        }
        else
        {
            Console.WriteLine("Öğrenci bulunamadı!");
        }
    }

    // Öğrenci güncelle
    public void GuncelleOgrenci(int id, string? ad, string? soyad, string? email, int ogrenciNo)
    {
        Student? ogrenci = students.FirstOrDefault(s => s.Id == id);
        if (ogrenci == null)
        {
            Console.WriteLine("Öğrenci bulunamadı!");
            return;
        }

        if (!string.IsNullOrEmpty(ad))
            ogrenci.Ad = ad;
        if (!string.IsNullOrEmpty(soyad))
            ogrenci.Soyad = soyad;
        if (!string.IsNullOrEmpty(email))
            ogrenci.Email = email;
        if (ogrenciNo > 0)
            ogrenci.OgrenciNo = ogrenciNo;

        Kaydet();
        Console.WriteLine("Öğrenci başarıyla güncellendi!");
    }

    // ID'ye göre öğrenci bul
    public Student? BulOgrenci(int id)
    {
        return students.FirstOrDefault(s => s.Id == id);
    }
}

class Program
{
    static void Main()
    {
        StudentManager manager = new StudentManager();
        manager.Yukle();

        while (true)
        {
            Console.WriteLine("\n========== Öğrenci Yönetim Sistemi ==========");
            Console.WriteLine("1. Öğrenci Ekle");
            Console.WriteLine("2. Öğrenci Listele");
            Console.WriteLine("3. Öğrenci Sil");
            Console.WriteLine("4. Öğrenci Güncelle");
            Console.WriteLine("5. Çıkış");
            Console.Write("Seçiminiz: ");

            string? secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    OgrenciEkleMenu(manager);
                    break;
                case "2":
                    Console.WriteLine("\n--- Öğrenci Listesi ---");
                    manager.ListeleOgrenciler();
                    break;
                case "3":
                    OgrenciSilMenu(manager);
                    break;
                case "4":
                    OgrenciGuncelleMenu(manager);
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

    static void OgrenciEkleMenu(StudentManager manager)
    {
        Console.WriteLine("\n--- Yeni Öğrenci Ekle ---");

        Console.Write("Ad: ");
        string? ad = Console.ReadLine();

        Console.Write("Soyadı: ");
        string? soyad = Console.ReadLine();

        Console.Write("Email: ");
        string? email = Console.ReadLine();

        Console.Write("Öğrenci No: ");
        if (!int.TryParse(Console.ReadLine(), out int ogrenciNo))
        {
            Console.WriteLine("Geçersiz öğrenci numarası!");
            return;
        }

        manager.EkleOgrenci(ad, soyad, email, ogrenciNo);
    }

    static void OgrenciSilMenu(StudentManager manager)
    {
        Console.WriteLine("\n--- Öğrenci Sil ---");

        manager.ListeleOgrenciler();

        Console.Write("\nSilinecek öğrencinin ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID!");
            return;
        }

        manager.SilOgrenci(id);
    }

    static void OgrenciGuncelleMenu(StudentManager manager)
    {
        Console.WriteLine("\n--- Öğrenci Güncelle ---");

        manager.ListeleOgrenciler();

        Console.Write("\nGüncellenecek öğrencinin ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID!");
            return;
        }

        Student? ogrenci = manager.BulOgrenci(id);
        if (ogrenci == null)
        {
            Console.WriteLine("Öğrenci bulunamadı!");
            return;
        }

        Console.Write("Yeni Ad (Enter ile geç): ");
        string? yeniAd = Console.ReadLine();

        Console.Write("Yeni Soyadı (Enter ile geç): ");
        string? yeniSoyad = Console.ReadLine();

        Console.Write("Yeni Email (Enter ile geç): ");
        string? yeniEmail = Console.ReadLine();

        Console.Write("Yeni Öğrenci No (Enter ile geç): ");
        int yeniNo = 0;
        if (int.TryParse(Console.ReadLine(), out int temp))
            yeniNo = temp;

        manager.GuncelleOgrenci(id, yeniAd, yeniSoyad, yeniEmail, yeniNo);
    }
}