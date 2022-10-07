using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module11.VoiceTexterBot.Configuration;
using Module11.VoiceTexterBot.Controllers;
using Module11.VoiceTexterBot.Services;
using System.Text;
using Telegram.Bot;

namespace Module11.VoiceTexterBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Сервис запущен!");
            Console.ResetColor();

            // Запускаем сервис
            await host.RunAsync();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Сервис остановлен!");
            Console.ResetColor();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileHandler, AudioFileHandler>();

            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IStorage, MemoryStorage>();

            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5620147017:AAGPyUeDUG2jYo2tY5Ah8Smsq5vVnu9Uh48",
                DownloadsFolder = "D:\\vs\\Module11.VoiceTexterBot\\Files",
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
                OutputAudioFormat = "wav",
                InputAudioBitrate = 48000,
            };
        }
    }
}