using MPP_4;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

if (args.Length != 2)
{
    throw new ArgumentOutOfRangeException();
}

var TestSetPath = args[1];

if (!File.Exists(TestSetPath))
{
    throw new FileNotFoundException();
}

NumberFormatInfo provider = new NumberFormatInfo();
provider.NumberDecimalSeparator = ".";
provider.NumberGroupSeparator = ",";

List<TestItem> testItems = new List<TestItem>();

string[] testLines = File.ReadAllLines(TestSetPath);

foreach (string line in testLines)
{
    var data = line.Split(',');
    int dataLength = data.Length;
    TestItem testItem = new TestItem();
    for (int i = 0; i < dataLength; i++)
    {
        testItem.attributes.Add(Convert.ToDouble(data[i],provider));
    }
    testItems.Add(testItem);
}

double E = -1;

int index = 0;
int howManyGroups = int.Parse(args[0]);
int howManyItemsInGroup = testItems.Count/howManyGroups;
int groupIndex = 0;

int index2 = 0;
foreach (TestItem testItem in testItems)
{
    testItem.Group = index2;
    if (index2 == howManyGroups - 1)
    {
        index2 = 0;
    }
    else
    {
        index2++;
    }
}

bool go = true;

while (go)
{
    ShowGroupsStatus(testItems, howManyGroups);
    Console.WriteLine("Current E: " + E);

    int howManyAttributes = testItems[0].attributes.Count;
    List<List<double>> centroids = DetermineCentroids(testItems,howManyGroups, howManyAttributes);

    bool didSomethingChanged = false;
    List<TestItem> testItemsWithNewGroups = AssignNewGroupsBasedOnCentroids(testItems, centroids, ref didSomethingChanged);

    if (didSomethingChanged == true)
    {
        List<List<double>> newCentroids = DetermineCentroids(testItemsWithNewGroups, howManyGroups, howManyAttributes);

        double newE = DetermineNewE(testItemsWithNewGroups, newCentroids);
        Console.WriteLine("New E is equal to: " + newE);

        if (E == newE)
        {
            Console.WriteLine("Algorithm Finished, E didn't changed");
            go = false;
            break;
        }
        else
        {
            E = newE;
        }
    }
    else
    {
        Console.WriteLine("Algorithm Finished, None of the groups changed");
        go = false;
        break;
    }

    testItems = testItemsWithNewGroups;
    Console.WriteLine("=====New Groups=====");

    ShowGroupsStatus(testItems, howManyGroups);
    Console.WriteLine("===========");
}

List<List<double>> DetermineCentroids(List<TestItem> testItems,int howManyGroups, int howManyAttributes)
{
    int index = 0;
    List<List<double>> centroids = new List<List<double>>();

    for (int i = 0; i < howManyGroups; i++)
    {
        
        List<TestItem> itemsInGroup = new List<TestItem>();

        foreach(TestItem testItem in testItems)
        {
            if (testItem.Group == i)
            {
                itemsInGroup.Add(testItem);
            }
        }

        List<double> centroid = new List<double>();
        for (int j = 0; j < howManyAttributes; j++)
        {
            double outcome = 0;
            for (int z = 0; z < itemsInGroup.Count; z++)
            {
                outcome += itemsInGroup[z].attributes[j];
            }
            centroid.Add(outcome / itemsInGroup.Count);
        }
        centroids.Add(centroid);
    }
    return centroids;
}

List<TestItem> AssignNewGroupsBasedOnCentroids(List<TestItem> testItems, List<List<double>> centroids, ref bool didSomethingChanged)
{

    foreach (TestItem testItem in testItems)
    {
        List<double> distances = new List<double>();
        foreach (List<double> centroid in centroids)
        {
            distances.Add(DistanceFromItemToCentroid(testItem,centroid));
        }

        testItem.newGroup = distances.IndexOf(distances.Min());

        if (testItem.newGroup != testItem.Group)
        {
            didSomethingChanged = true;
        }

        testItem.Group = testItem.newGroup;
        
    }
    return testItems;
}
void ShowGroupsStatus(List<TestItem> testItems, int numberOfGroups)
{
    for (int i = 0;i < numberOfGroups; i++)
    {
        int quantity = 0;
        foreach (TestItem testItem in testItems)
        {
            if(testItem.Group == i)
            {
                quantity++;
            }
        }
        Console.WriteLine("There is " + quantity + " in group: " + i);
    }
}
double DistanceFromItemToItsCentroid(TestItem testItem, List<List<double>> centroids)
{
    List<double> centroid = centroids.ElementAt(testItem.Group);
    double distance = 0;
    for (int i = 0; i < testItem.attributes.Count; i++)
    {
        distance += Math.Pow(testItem.attributes[i] - centroid[i], 2);
    }
    return distance;
}
double DistanceFromItemToCentroid(TestItem testItem, List<double> centroidIn)
{
    if(centroidIn.Count > 0 & !centroidIn.Contains(double.NaN))
    {

        double distance = 0;
        for (int i = 0; i < testItem.attributes.Count; i++)
        {
            distance += Math.Pow(testItem.attributes[i] - centroidIn[i], 2);
        }
        return distance;
    }
    else
    {
        return double.MaxValue;
    }
    
}
double DetermineNewE(List<TestItem> testItems, List<List<double>> centroids)
{
    double distance = 0;
    foreach (TestItem testItem in testItems)
    {
        distance += DistanceFromItemToItsCentroid(testItem,centroids);
    }
    return distance;
}







