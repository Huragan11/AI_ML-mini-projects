using MPP_1;
using System.Globalization;

if (args.Length != 3)
{
    throw new ArgumentOutOfRangeException();
}

var TrainingSetPath = args[1];
var TestSetPath = args[2];

if (!File.Exists(TrainingSetPath) || !File.Exists(TestSetPath))
{
    throw new FileNotFoundException();
}

List<TrainItem> trainItems = new List<TrainItem>();

string[] trainLines = File.ReadAllLines(TrainingSetPath);

NumberFormatInfo provider = new NumberFormatInfo();
provider.NumberDecimalSeparator = ",";
provider.NumberGroupSeparator = ".";

foreach (string line in trainLines)
{
    var data = line.Split(';');
    int dataLength = data.Length;
    TrainItem trainItem = new TrainItem();
    trainItem.Label = data[data.Length - 1];
    for (int i = 0; i < dataLength - 1; i++)
    {
        trainItem.attributes.Add(Convert.ToDouble(data[i],provider));
    }
    trainItems.Add(trainItem);
}

string[] testLines = File.ReadAllLines(TestSetPath);

List<TestItem> testItems = new List<TestItem>();

foreach (string line in testLines)
{
    var data = line.Split(';');
    int dataLength = data.Length;
    TestItem testItem = new TestItem();
    testItem.Label = data[data.Length - 1];
    for (int i = 0; i < dataLength - 1; i++)
    {
        testItem.attributes.Add(Convert.ToDouble(data[i], provider));
    }
    testItems.Add(testItem);
}

var k = int.Parse(args[0]);
void k_nn(int k)
{
    Dictionary<TestItem, bool> testItemsAccuracy = new Dictionary<TestItem, bool>();

    foreach (TestItem testItem in testItems)
    {
        Dictionary<TrainItem, double> trainItemsAndDistances = new Dictionary<TrainItem, double>();

        foreach (TrainItem trainItem in trainItems)
        {
            List<double> tmpValues = new List<double>();
            for (int i = 0; i < testItem.attributes.Count; i++)
            {
                double tmpValue = Math.Pow(testItem.attributes[i] - trainItem.attributes[i], 2);
                tmpValues.Add(tmpValue);
            }

            double tmpSum = 0;

            foreach (double tmpValue in tmpValues)
            {
                tmpSum += tmpValue;
            }
            double distance = Math.Sqrt(tmpSum);

            trainItemsAndDistances.Add(trainItem, distance);
        }

        var sortedTrainItemsAndDistances = from item in trainItemsAndDistances orderby item.Value ascending select item;

        List<String> labels = new List<string>();

        for (int i = 0; i < k; i++)
        {
            labels.Add(sortedTrainItemsAndDistances.ElementAt(i).Key.Label);

        }
        testItem.assignedLabel = labels.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

        if (testItem.assignedLabel.Equals(testItem.Label))
        {
            testItemsAccuracy.Add(testItem, true);
        }
        else
        {
            testItemsAccuracy.Add(testItem, false);
        }
    }
    double accuracy = (testItemsAccuracy.Values.Count(value => value == true) / (double)testItemsAccuracy.Count);
}

void k_nnForAllK()
{
    Dictionary<int, double> outputAccuracy = new Dictionary<int, double>();
for (int rep = 1; rep <= 105; rep++) {

    Dictionary<TestItem, bool> testItemsAccuracy = new Dictionary<TestItem, bool>();

foreach (TestItem testItem in testItems)
{
    Dictionary<TrainItem, double> trainItemsAndDistances = new Dictionary<TrainItem, double>();

    foreach(TrainItem trainItem in trainItems)
    {
        List<double> tmpValues = new List<double>();
        for (int i = 0; i < testItem.attributes.Count; i++)
        {
            double tmpValue = Math.Pow(testItem.attributes[i] - trainItem.attributes[i],2);
            tmpValues .Add(tmpValue);
        }
        
        double tmpSum = 0;

        foreach(double tmpValue in tmpValues)
        {
            tmpSum += tmpValue;
        }
        double distance = Math.Sqrt(tmpSum);

        trainItemsAndDistances.Add(trainItem, distance);
    }

    var sortedTrainItemsAndDistances = from item in trainItemsAndDistances orderby item.Value ascending select item;

    List<String> labels = new List<string>();

    for ( int i = 0; i < rep; i++)
    {
        labels.Add(sortedTrainItemsAndDistances.ElementAt(i).Key.Label);
        
    }
            testItem.assignedLabel = labels.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

    if (testItem.assignedLabel.Equals(testItem.Label))
    {
        testItemsAccuracy.Add(testItem, true);
    }
    else
    {
        testItemsAccuracy.Add(testItem, false);
    }
}

double accuracy = (testItemsAccuracy.Values.Count(value => value == true) / (double)testItemsAccuracy.Count);

outputAccuracy.Add(rep, Math.Round(accuracy,3));

}
    File.WriteAllLines("Accuracy", outputAccuracy.Select(x => x.Key + " " + x.Value).ToArray());
} 

void k_nnForInputAndKFromArgs(int k)
{
    List<InputItem> inputItems = new List<InputItem>();
    bool stop = false;
    while (stop != true)
    {
        Console.WriteLine("Aby wyjść proszę wpisać 'exit' ");
        Console.WriteLine("Prosze podać 4 wartości w formie '1,1;2,2;3,3;4,4' " );

        string input = Console.ReadLine();

        if (input == "exit")
        {
            break;
        }
        else
        {
            var data = input.Split(";");
            int dataLength = data.Length;
            if (dataLength != 4) {
                Console.WriteLine("Podano złą ilość argumentów");
            }
            else
            {
                InputItem inputItem = new InputItem();
                for (int i = 0; i < dataLength; i++)
                {
                    inputItem.attributes.Add(Convert.ToDouble(data[i], provider));
                }
                inputItems.Add(inputItem);
            }
        }
    }
    
    foreach (InputItem inputItem in inputItems)
    {
        Dictionary<TrainItem, double> trainItemsAndDistances = new Dictionary<TrainItem, double>();

        foreach (TrainItem trainItem in trainItems)
        {
            List<double> tmpValues = new List<double>();
            for (int i = 0; i < inputItem.attributes.Count; i++)
            {
                double tmpValue = Math.Pow(inputItem.attributes[i] - trainItem.attributes[i], 2);
                tmpValues.Add(tmpValue);
            }

            double tmpSum = 0;

            foreach (double tmpValue in tmpValues)
            {
                tmpSum += tmpValue;
            }
            double distance = Math.Sqrt(tmpSum);

            trainItemsAndDistances.Add(trainItem, distance);
        }

        var sortedTrainItemsAndDistances = from item in trainItemsAndDistances orderby item.Value ascending select item;

        List<String> labels = new List<string>();

        for (int i = 0; i < k; i++)
        {
            labels.Add(sortedTrainItemsAndDistances.ElementAt(i).Key.Label);

        }
        
        inputItem.assignedLabel = labels.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
        
        foreach(double i in inputItem.attributes)
        {
            Console.Write(i + " ; ");
        }
        Console.WriteLine(inputItem.assignedLabel);
    }
    
}

k_nn(k);
k_nnForAllK();
k_nnForInputAndKFromArgs(k);