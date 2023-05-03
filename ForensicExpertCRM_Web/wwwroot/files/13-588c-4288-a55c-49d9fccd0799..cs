// See https://aka.ms/new-console-template for more information
using OverWork.Excel;
using Task4;
using Task4.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var logger = new Logger();
            var path = args.Length > 0 ? args[0] : null;

            if (path == null)
            {
                logger.WriteLine($"Введите путь до файла:\n");
                path = Console.ReadLine();
            }


            if (!File.Exists(path))
            {
                logger.WriteLine($"{path} файла не существует");
                return;
            }

            logger.WriteLine($"Запущен по пути: {path}");

            var word = new WordApp(path, logger);
            var res = word.Start();
            word.Clear();


            var excel = new ExcelApp(Path.GetFileNameWithoutExtension(path), logger);


            excel.SetVisible(false);
            excel.CreateNewExcel();
            excel.WriteToExcel(res);

            excel.Exit();
            Console.WriteLine("Нажмите любую кнопку чтобый выйти...");
            Console.ReadKey();
        }
        catch (Exception exс)
        {
            Console.WriteLine(exс.Message);
        }

    }
}