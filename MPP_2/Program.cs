using MPP_2;
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

NumberFormatInfo provider = new NumberFormatInfo();
provider.NumberDecimalSeparator = ".";
provider.NumberGroupSeparator = ",";

List<TrainItem> trainItems = new List<TrainItem>();

string[] trainLines = File.ReadAllLines(TrainingSetPath);

foreach (string line in trainLines)
{
    var data = line.Split(';');
    int dataLength = data.Length;
    TrainItem trainItem = new TrainItem();
    trainItem.Label = data[data.Length - 1];
    for (int i = 0; i < dataLength - 1; i++)
    {
        trainItem.attributes.Add(Convert.ToDouble(data[i], provider));
    }
    trainItems.Add(trainItem);
}

List<TestItem> testItems = new List<TestItem>();

string[] testLines = File.ReadAllLines(TestSetPath);

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

double alfa = double.Parse(args[0], provider);
Perceptron perceptron;

void UserInputForClassification()
{
    List<InputItem> inputItems = new List<InputItem>();
    bool stop = false;
    while (stop != true)
    {
        Console.WriteLine("Aby wyjść proszę wpisać 'exit' ");
        Console.WriteLine("Prosze podać 4 wartości w formie '1.1;2.2;3.3;4.4' ");

        string input = Console.ReadLine();

        if (input == "exit")
        {
            break;
        }
        else
        {
            var data = input.Split(";");
            int dataLength = data.Length;
            if (dataLength != 4)
            {
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
        Console.WriteLine(perceptron.ClassifyInput(inputItem));
    }



}

perceptron = new Perceptron(alfa);

perceptron.LearnTrainSet(trainItems);

perceptron.ClassifyTestSet(testItems);

UserInputForClassification();
