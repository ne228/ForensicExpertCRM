namespace ForensicExpertCRM_Web.Services
{
    public static class Extenshions
    {
        public static List<Data.Domain.TypeExpertise> Shuffle(this List<Data.Domain.TypeExpertise> list)
        {
            Random rand = new Random();

            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                var tmp = list[j];
                list[j] = list[i];
                list[i] = tmp;
            }
            return list;
        }
    }


}
