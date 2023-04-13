namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Train? train = null;
            uint num = 22;
            uint passengers = 200;
            try
            {
                train = new Train(1, TrainType.Passenger, "Kuiv  - Odessa",  DateTime.Now.AddHours(-4), DateTime.Now.AddHours(3),
                                      new Carriage(5, CType.Reserved, 50, 48),
                                      new Carriage(12, CType.Reserved, 50, 50),
                                      new Carriage(3, CType.Сompartment, 25, 24),
                                      new Carriage(8, CType.Сompartment, 25, 20));

                while (passengers != 0)
                {
                    train?.PassBoarding(ref passengers);
                    if (passengers != 0) train?.AddCarriage(new Carriage(num++, CType.Reserved, 50));
                }
            }
            catch (Exception exc) { Console.WriteLine(exc.Message); }
           
            Console.WriteLine(train?.ToString() ?? " No train data....");
        }
    }
}