using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuadPay.InstallmentsService.Interfaces;
using QuadPay.InstallmentsService.Models;
using QuadPay.InstallmentsService.Services;

namespace QuadPay.InstallmentsService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, app) =>
                {
                    app.SetBasePath(Environment.CurrentDirectory);
                    app.AddJsonFile("appsettings.json", false, true);
                    app.AddJsonFile($"appsettings.{environmentName}.json", true, true);
                    app.AddEnvironmentVariables();
                    app.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IPaymentPlanService, PaymentPlanService>();
                })
                .Build();
            
            var paymentPlanService = host.Services.GetRequiredService<IPaymentPlanService>();

            var paymentPlan = paymentPlanService.CreatePaymentPlan((decimal)525.57);

            OutputResults((decimal)525.57, paymentPlan);
        }

        private static void OutputResults(decimal amountRequested, PaymentPlan paymentPlan)
        {
            var totalAmountPaid = Math.Round(paymentPlan.Installments.Sum(x => x.Amount), 2);
            
            var output = new StringBuilder();

            output.AppendLine("\n===============================");
            output.AppendLine($"Amount request:\t\t{amountRequested:$0.00}");
            output.AppendLine("\nInstallments:");
            
            paymentPlan.Installments.ToList().ForEach(x => output.AppendLine($"\t{x.DueDate:MM/dd/yyyy}:\t{x.Amount:$0.00}"));

            output.AppendLine($"\nTotal amount paid:\t{totalAmountPaid:$0.00}");
            output.AppendLine("===============================");
            output.AppendLine("Completed!");
            
            Console.WriteLine(output.ToString());
        }
    }
}