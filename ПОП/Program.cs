class Program
{
    int numberOfThreads = 2;
    int step = 2;
    CalculatorData[] allData;

    static void Main(string[] args)
    {
        Program program = new Program();
        program.Start();
    }

    void Start()
    {
        allData = new CalculatorData[numberOfThreads];

        for (int i = 0; i < numberOfThreads; i++)
        {
            CalculatorData data = new CalculatorData(i + 1, step);
            allData[i] = data;

            Thread calcThread = new Thread(() => Calculator(data));
            data.Thread = calcThread;
            calcThread.Start();
        }

        Thread stopperThread = new Thread(() => stopper(allData));
        stopperThread.Start();
    }

    class CalculatorData
    {
        public int Id { get; }
        public int Step { get; }
        public bool CanStop = false;
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
        long count = 0;
        int current = 0;

        while (!data.CanStop)
        {
            sum += current;
            current += data.Step;
            count++;
        }

        Console.WriteLine($"Потiк #{data.Id}: Сума = {sum}, Кiлькiсть доданкiв = {count}");
    }

    void stopper(CalculatorData[] allData)
    {
        for (int i = 0; i < allData.Length; i++)
        {
            int delayInSeconds = (i + 1) * 3;
            Thread.Sleep(delayInSeconds * 1000);
            allData[i].CanStop = true;
        }
    }
}
}
