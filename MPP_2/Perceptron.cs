namespace MPP_2
{
    public class Perceptron
    {
        List<double> Wages = new List<double>();
        public double Bias { get; set; }
        public double TeachingValue { get; set;}

        List<TrainItem> trainItems { get; set; }
        List<TestItem> testItems { get; set; }

        Dictionary<String, int> labelsKeys = new Dictionary<String, int>();

        Random random = new Random();

        public Perceptron(double alfa) 
        {
            TeachingValue = alfa;
            
            Bias = random.Next(-5,5);
            
        }
        
        public void LearnTrainSet(List<TrainItem> InTrainItems)
        {
            trainItems = InTrainItems;
            if(Wages.Count == 0) { 
                for (int i = 0; i < trainItems[0].attributes.Count; i++)
                {
                    Wages.Add(random.Next(-50,50)/10.0);
                }
            }

            List<String> Labels = new List<String>();
            
            foreach (TrainItem item in trainItems)
            {
                Labels.Add(item.Label);
            }

            Labels = Labels.Distinct().ToList();

            labelsKeys.Add(Labels[0], 0);
            labelsKeys.Add(Labels[1], 1);

            Random rng = new Random();

            List<TrainItem> shuffledTrainItems = trainItems.OrderBy(x => rng.Next()).ToList();


            foreach (TrainItem item in shuffledTrainItems)
            {

                double Net = 0;
                int Answer;
                int RightAnswer = 0;

                for (int i = 0; i < Wages.Count;i++)
                {
                    Net += Wages.ElementAt(i) * item.attributes.ElementAt(i);
                }
                
                Net -= Bias;

                if (Net < 0)
                {
                    Answer = 0;
                }else
                {
                    Answer = 1;
                }

                if (item.Label.Equals(labelsKeys.ElementAt(Answer).Key))
                {
                    RightAnswer = Answer;
                }
                else if (!item.Label.Equals(labelsKeys.ElementAt(Answer).Key) & Answer == 0)
                {
                    RightAnswer = 1;
                }
                else if (!item.Label.Equals(labelsKeys.ElementAt(Answer).Key) & Answer == 1)
                {
                    RightAnswer = 0;
                }

                for (int i = 0;i < Wages.Count; i++)
                {
                    Wages[i] = Math.Round(Wages.ElementAt(i) + (RightAnswer - Answer) * TeachingValue * item.attributes.ElementAt(i),1);
                }
                Bias = Bias - (RightAnswer - Answer) * TeachingValue;
            }
        }

        public void ClassifyTestSet(List<TestItem> InTestItems)
        {
            testItems = InTestItems;

            double accurateClassification = 0;
            double accurateClassificationForKeyOne = 0;
            double accurateClassificationForKeyTwo = 0;

            foreach (TestItem item in testItems)
            {
                double Net = 0;
                int Answer;
                for (int i = 0; i < Wages.Count; i++)
                {
                    Net += Wages.ElementAt(i) * item.attributes.ElementAt(i);
                }

                Net -= Bias;

                if (Net < 0)
                {
                    Answer = 0;
                }
                else
                {
                    Answer = 1;
                }

                if (item.Label.Equals(labelsKeys.ElementAt(Answer).Key))
                {
                    accurateClassification++;

                    if(Answer == 0)
                    {
                        accurateClassificationForKeyOne++;
                    }
                    else
                    {
                        accurateClassificationForKeyTwo++;
                    }
                }
            }
            Console.WriteLine("Accuracy: " +  accurateClassification + " : " + testItems.Count + " gives us " + Math.Round((accurateClassification / testItems.Count) * 100,1) + "%");
            Console.WriteLine("Accuracy for first object: " +  accurateClassificationForKeyOne + " : " + testItems.Count/2 + " gives us " + Math.Round((accurateClassificationForKeyOne / (testItems.Count/2)) * 100, 1) + "%");
            Console.WriteLine("Accuracy for second object: " +  accurateClassificationForKeyTwo + " : " + testItems.Count/2 + " gives us " + Math.Round((accurateClassificationForKeyTwo / (testItems.Count/2)) * 100, 1) + "%");
        }

        public string ClassifyInput(InputItem item)
        {
            double Net = 0;
            int Answer;
            for (int i = 0; i < Wages.Count; i++)
            {
                Net += Wages.ElementAt(i) * item.attributes.ElementAt(i);
            }

            Net -= Bias;

            if (Net < 0)
            {
                Answer = 0;
            }
            else
            {
                Answer = 1;
            }



            return "Perceptron classified object with vector: ["+ String.Join(";",item.attributes) + "] as: " + labelsKeys.ElementAt(Answer).Key + " [" + Answer + "]";
        }





    }
}
