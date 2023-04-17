using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForensicExpertCRM
{
    public static class Functions
    {

        public static double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        private static double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }



        public static void Train()
        {
            var rnd = new Random(DateTime.Today.Microsecond);

            List<Input> inputs = new List<Input>();
            for (int i = 0; i <= 100; i++)
            {
                var input = new Input()
                {
                    CountEmploee = rnd.Next(0, 100) ,
                    CountExpert = rnd.Next(0, 100),
                    Destination = 100 / (double)rnd.Next(0, 500) *100.0,
                    Speed = rnd.NextDouble() * 100.0,
                    PercentTerm = rnd.NextDouble() * 100.0,
                    Rating = rnd.NextDouble() * 100.0,
                };
                input.Calculate();


                if (Double.IsInfinity(input.Destination) || Double.IsInfinity(input.Result))
                    continue;


                input.ExpDefEmp *= 100;
                input.Result *= 100;
                inputs.Add(input);
            }


            var json = JsonConvert.SerializeObject(inputs);
            File.WriteAllText(@"C:\Users\smallaxe\Desktop\Test.txt", json);


        }

    }


    public class Input
    {

        [JsonIgnore]
        public int CountExpert { get; set; }
        [JsonIgnore]
        public int CountEmploee { get; set; }

        public double ExpDefEmp { get; set; }
        public double Speed { get; set; }


        public double PercentTerm { get; set; }
        [JsonIgnore]
        public double Destination { get; set; }
        [JsonIgnore]
        public double Rating { get; set; }

        public double Result { get; set; }


        public double Calculate()
        {
            double temp = ((double)CountExpert / (double)CountEmploee);
            ExpDefEmp = 1.0 - temp;

            if (ExpDefEmp < 0.0) { ExpDefEmp = 0.0; }
            double res = 0.3 * ExpDefEmp + 0.2 * Speed + 0.3 * PercentTerm + 0.1 * Destination + 0.2 * Rating;

            Result = res;

            Console.WriteLine($"cExp={CountExpert} cEmp={CountEmploee} | ExpDefEmp={this.ExpDefEmp} " +
                $"speed={Speed} dest={Destination} r={Rating}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t\t| res = " + res);
            Console.ForegroundColor = ConsoleColor.White;

            return res;
        }
    }


}
