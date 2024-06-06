using Grpc.Net.Client;

namespace GrpcClient
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Klient gRPC: wprowadź dane a zadnie zostanie obliczone w serwisie!:");
        start:
            int a, b;
            string operation;
            try
            {
                Console.WriteLine("Podaj pierwszą liczbę:");
                a = int.Parse(Console.ReadLine()!);
                Console.WriteLine("Podaj działanie (znak +, -, * lub / :");
                operation = Console.ReadLine()!;
                Console.WriteLine("Podaj drugą liczbę:");
                b = int.Parse(Console.ReadLine()!);

                if (b == 0 && operation == "/") 
                    throw new DivideByZeroException();
            }
            catch
            {
                Console.WriteLine("Ups! Od nowa :)");
                goto start;
            }


            var channel = GrpcChannel.ForAddress("https://localhost:7264");
            var client = new Calculator.CalculatorClient(channel);
            var response = client.Work(new WorkRequest { A = a, B = b, Operation = operation });

            if (response.ResponseCase == WorkResponse.ResponseOneofCase.Result)
                Console.WriteLine($"Wynik działania: {response.Result}");
            else if (response.ResponseCase == WorkResponse.ResponseOneofCase.Error)
                Console.WriteLine($"Wystąpił błąd!: {response.Error}\n\n");


            goto start;
        }
    }
}
