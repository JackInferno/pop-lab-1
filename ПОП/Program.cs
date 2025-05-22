class Program
{
    static void Main(string[] args)
    {
        Program program = new Program();
        program.Start();
    }

    int numberOfThreads = 2;
    int step = 2; 

    void Start()
    {
        for (int i = 0; i < numberOfThreads; i++)
        {
            CalculatorData data = new CalculatorData(i + 1, step);
            Thread calcThread = new Thread(() => Calculator(data));
            data.Thread = calcThread;
            calcThread.Start();

            int delayInSeconds = (i + 1) * 5;
            new Thread(() => Stopper(data, delayInSeconds)).Start();
        }
    }

    class CalculatorData
    {
        public int Id { get; }
        public int Step { get; }
        public bool CanStop { get; set; } = false;
        public Thread Thread { get; set; }

        public CalculatorData(int id, int step)
        {
            Id = id;
            Step = step;
        }
    }

    void Calculator(CalculatorData data)
    {
        long sum = 0;
        int count = 0;
        int current = 0;

        while (!data.CanStop)
        {
            sum += current;
            current += data.Step;
            count++;
        }

        Console.WriteLine($"Потiк #{data.Id}: Сума = {sum}, Кiлькiсть доданкiв = {count}");
    }

    void Stopper(CalculatorData data, int delayInSeconds)
    {
        Thread.Sleep(delayInSeconds * 100);
        data.CanStop = true;
    }
}
