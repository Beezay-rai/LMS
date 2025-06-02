namespace LMS.Background
{
    public class MyRabbitMQ : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;  
        }
    }
}
