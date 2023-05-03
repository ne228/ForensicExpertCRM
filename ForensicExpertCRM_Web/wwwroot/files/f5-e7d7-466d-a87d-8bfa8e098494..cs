using Microsoft.Office.Interop.Word;
using Task4.Logging;
using Task4.Data;

namespace Task4
{
    public class WordApp
    {

        private string Path { get; }
        private ILogger Logger { get; }

        public Document Document { get; }
        private Application Word_App { get; set; }
        public WordApp(string path, ILogger logger)
        {
            Path = path;
            Logger = logger;

            Word_App = new();
            Logger.WriteLine("Word App запущен!");

            Word_App.Visible = false;

            // Откройте книгу.
            Document = Word_App.Documents.Open(
                Path,
               ReadOnly: true);

            Logger.WriteLine($"Word запущен по пути: {Path}");

        }

        public Ref Start()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            var res = new Ref();        

            // Парсинг, если документ - таблица
            Microsoft.Office.Interop.Word.Range range = Document.Content;
            var allText = Extension.Reduction(range.Text.Replace("\r", "").Replace("\u0001", "")).Split('\a').ToList();

            // Парсинг, если документы - параграфы
            if (allText.Count == 1)
            {
                allText = new List<string>();

                foreach (Paragraph para in Document.Paragraphs)
                {
                    string text = Extension.Reduction(para.Range.Text.Replace("\r", "").Replace("\u0001", ""));
                    // Обработка текста параграфа
                    allText.Add(text);
                }

            }

            // Очистка массивов
            allText.RemoveAll(x => String.IsNullOrEmpty(x));
            allText.RemoveAll(x => x == " ");
            allText.RemoveAll(x => x == "  ");



            // Парсинг общей информации
            var kartoteki = allText.Split("картотека").ToList();
            var person = new Person(kartoteki[1].Normilize());
            res.FirstName = person.Фамилия;
            res.MidlleName = person.Имя;
            res.LastName = person.Отчество;
            res.DateBirdth = person.ДатаРождения;
            res.PlaceBirdth = person.МестоРожд;


            // Парсинг федеральных БД

            var federalKartoteki = kartoteki[2].Split("Дактилоформула");

            List<ParseFederal> parseFederals = new();
            for (int i = 1; i < federalKartoteki.Count; i++)
            {
                federalKartoteki[i] = federalKartoteki[i].Normilize();
                parseFederals.Add(new ParseFederal(federalKartoteki[i]));
            }



            // Парсинг региональныъ БД
            kartoteki.RemoveRange(0, 3);


            var regionalKartoteki = new List<List<string>>();
            for (int i = 0; i < kartoteki.Count; i++)
            {
                kartoteki[i].RemoveAt(0);

                if (i % 2 == 1)
                    regionalKartoteki.Add(kartoteki[i]);
                else
                {
                    foreach (var item in kartoteki[i])
                        if (item.ToLower().Contains("регион"))
                            res.Places.Add($"ИЦ МВД России по {item.Split(':')[1]},");

                }
            }

            // Start parsing regional
            List<ParseRegional> parseRegional = new();
            foreach (var regionalKartoteka in regionalKartoteki)
            {
                var currentKarts = regionalKartoteka.Split("пресечение");
                currentKarts.RemoveAll(x => x.Count == 0);

                for (int i = 1; i < currentKarts.Count; i += 2)
                {
                    currentKarts[i].AddRange(currentKarts[i - 1]);
                    currentKarts[i] = currentKarts[i].Normilize();
                    parseRegional.Add(new ParseRegional(currentKarts[i]));
                }
            }

            // Преобразование в нужные типы
            res.ToConduct(parseFederals);
            res.ToConduct(parseRegional);
            res.ToPreventConduct(parseRegional);

            return res;
        }

        public void Clear()
        {

            Document.Close();
            Word_App.Quit();

            Logger.WriteLine("Word App закончил работу");
        }
    }
}
