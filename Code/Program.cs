using System.Globalization;
using System.Runtime.CompilerServices;

namespace FileEncryptionUsingXOR
{
    public class Program
    {
        private static string dataDirectory = "C:\\Bank";
        private static string dataFileName = "Bank.txt";
        private static readonly string encryptionKey = "encryptionKey1234";
        private static string data = "";
        private static int moneyAmount;
        private static int balance;
        private static bool closeBank;

        static void Main(string[] args)
        {
            while(closeBank != true)
            {
                Program program = new Program();
                string bankBalance;
                string filePath = Path.Combine(dataDirectory, dataFileName);
                if(File.Exists(filePath))
                {
                    balance = int.Parse(ReadDataFromFile(), CultureInfo.InvariantCulture.NumberFormat);
                }
                else
                {
                    balance = 0;
                }
            
                if(balance > 0)
                {
                    bankBalance = ReadDataFromFile();
                }
                else
                {
                    bankBalance = "0";
                }
                Console.WriteLine("Balance: $" + bankBalance);
                moneyAmount = 0;
                Console.Write("Add money to bank: $");
                bool validInput = false;
                while(validInput != true)
                {
                    var userDataInput = Console.ReadLine();
                    if(int.TryParse(userDataInput, out moneyAmount))
                    {
                        balance += moneyAmount;
                        data = balance.ToString();
                        validInput = true;
                        break;
                    }
                    else
                    {
                        Console.Write("Input is invalid.\nTry again.\n$");
                    }
                }
                validInput = false;
                string dataCollected = ReadDataFromFile();
                Console.WriteLine("Money added: $" + moneyAmount + "\nPress any key to close this window.");
                program.WriteDataFile();
                Console.Write("Deposit more or close window (Depsoit/Close): ");
                while(validInput != true)
                {
                    var userInput = Console.ReadLine();
                    if(userInput == "Deposit")
                    {
                        break;
                    }
                    else if(userInput == "Close")
                    {
                        closeBank = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Input is invalid. Check your spelling");
                    }
                }
            }
            Console.WriteLine("Press any key to close this window");
            Console.ReadKey();
        }

        public void WriteDataFile()
        {
            string filePath = Path.Combine(dataDirectory, dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                data = EncryptedData(data);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(data);
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        public static string ReadDataFromFile()
        {
            string filePath = Path.Combine(dataDirectory, dataFileName);
            string loadedData = "";
            if(File.Exists(filePath))
            {
                try
                {
                    string serializedData = "";
                    using(FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        using(StreamReader streamReader = new StreamReader(fileStream))
                        {
                            serializedData = streamReader.ReadToEnd();
                        }
                    }
                    serializedData = EncryptedData(serializedData);
                    loadedData = serializedData;
                }
                catch(Exception error)
                {
                    Console.WriteLine(error);
                }
            }
            return loadedData;
        }

        public static string EncryptedData(string data)
        {
            string encryptedData = "";
            for(int i = 0; i < data.Length; i++)
            {
                encryptedData += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
            }
            return encryptedData;
        }
    }
}