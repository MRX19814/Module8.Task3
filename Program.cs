using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var folderPath = string.Empty;

        if (args.Length > 0)
        {
            folderPath = args[0];
        }
        else
        {
            Console.Write("Введите путь до папки: ");
            folderPath = Console.ReadLine();
        }

        try
        {
            /// Проверка валидности пути
            if (!IsValidFolderPath(folderPath))
            {
                Console.WriteLine("Неверный путь к папке.");
                return;
            }

            /// Получение размера папки до очистки
            long initialFolderSize = CalculateFolderSize(folderPath);
            Console.WriteLine("Размер папки до очистки: " + initialFolderSize + " байт");

            /// Выполнение очистки папки
            int filesDeleted = CleanFolder(folderPath);
            Console.WriteLine("Удалено файлов: " + filesDeleted);

            /// Получение размера папки после очистки
            long finalFolderSize = CalculateFolderSize(folderPath);
            Console.WriteLine("Размер папки после очистки: " + finalFolderSize + " байт");

            /// Вычисление освобожденного места
            long spaceFreed = initialFolderSize - finalFolderSize;
            Console.WriteLine("Освобождено места: " + spaceFreed + " байт");

            Console.WriteLine("Операция выполнена успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка:");
            Console.WriteLine(ex.Message);
        }

        Console.ReadLine();
    }

    static bool IsValidFolderPath(string folderPath)
    {
        return Directory.Exists(folderPath);
    }

    static long CalculateFolderSize(string folderPath)
    {
        if (!IsValidFolderPath(folderPath))
        {
            throw new DirectoryNotFoundException("Указанный путь не существует.");
        }

        try
        {
            long size = 0;
            var directoryInfo = new DirectoryInfo(folderPath);

            foreach (var file in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                size += file.Length;
            }

            return size;
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при подсчете размера папки: " + ex.Message);
        }
    }

    static int CleanFolder(string folderPath)
    {
        if (!IsValidFolderPath(folderPath))
        {
            throw new DirectoryNotFoundException("Указанный путь не существует.");
        }

        try
        {
            int filesDeleted = 0;
            var directory = new DirectoryInfo(folderPath);

            foreach (var file in directory.GetFiles())
            {
                if (file.LastWriteTime < DateTime.Now.Subtract(TimeSpan.FromMinutes(30)))
                {
                    file.Delete();
                    filesDeleted++;
                }
            }

            return filesDeleted;
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при очистке папки: " + ex.Message);
        }
    }
}