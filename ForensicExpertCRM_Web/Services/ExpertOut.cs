using Newtonsoft.Json;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow;
using Tensorflow.NumPy;
using System.Xml.Linq;


namespace ForensicExpertCRM_Web.Services
{
    public class ExpertOut
    {
        public ExpertOut(ExpertInput expertInput)
        {
            Id = expertInput.Id;
            Name = expertInput.Name;
            Rating = GetRating(expertInput);
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public double Rating { get; set; }


        // TODO NEURO
        private double GetRating(ExpertInput expertInput)
        {
            var json = JsonConvert.SerializeObject(expertInput, Formatting.Indented);

            var result = 0.5 * expertInput.Def + 0.2 * expertInput.Speed + 0.3 * expertInput.PercentTerm + 0.1 * expertInput.Destination + 0.2 * expertInput.Raiting;
            Console.WriteLine(json);
            Console.WriteLine(result);
            result = Math.Round(result, 5);
            Neuro(expertInput);
            return result;
        }

        private void Neuro(ExpertInput expertInput)
        {
            try
            {

                //var array = new float[] {(float)1.1, (float)1.1, (float)1.1, (float)1.1, (float)1.1 };

                //Tensor x = new Tensor(array, new Shape(5));
                ////var x_test = np.array(array.ToArray());

                //var model = keras.models.load_model("wwwroot\\model");
                //var predictions = model.predict(array);

            }
            catch (Exception exc)
            {

                Console.WriteLine(exc);
            }


        }
    }
}


