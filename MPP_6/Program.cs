string filePath = args[0];
try
{
    string fileContent = File.ReadAllText(filePath);

    string[] lines = fileContent.Split('\n');

    string[] variables = lines[0].Split(' ');

    int BagSize = int.Parse(variables[0]);
    int AmountOfItems = int.Parse(variables[1]);

    string[] tmpMassList = lines[1].Split(',');
    string[] tmpValueList = lines[2].Split(',');

    List<int> masses = new List<int>();
    List<int> values = new List<int>();

    foreach (string value in tmpMassList)
    {
        int intValue;
        if (int.TryParse(value, out intValue))
            masses.Add(intValue);
    }

    foreach (string value in tmpValueList)
    {
        int intValue;
        if (int.TryParse(value, out intValue))
            values.Add(intValue);
    }

    string result;
    int resultMass = 0;
    int resultValue = 0;

    for (int i = 1; i < Math.Pow(2,AmountOfItems) - 1; i++)
    {
        int iterationValue = i;
        int tmpMass = 0;
        int tmpValue = 0;

        int bitIndex = AmountOfItems - 1;
        while(iterationValue != 0)
        {
            if((iterationValue & 1) == 1)
            {

                tmpMass += masses[bitIndex];
                if(tmpMass > BagSize) 
                {
                    break;
                }
                tmpValue += values[bitIndex];
            }

            iterationValue >>= 1;
            bitIndex--;
        }
        if(resultValue < tmpValue)
        {
            resultMass = tmpMass; 
            resultValue = tmpValue;

            Console.WriteLine("Binary: " + Convert.ToString(i,2).PadLeft(AmountOfItems, '0'));
            Console.WriteLine(resultMass);
            Console.WriteLine(resultValue);
        }
    }   
}
catch (Exception e)
{
    Console.WriteLine("Error reading the file: " + e.Message);
}