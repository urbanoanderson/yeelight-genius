using System;
using System.Threading.Tasks;
using System.Windows.Media;
using YeelightAPI;

namespace YeelightGenius
{
    public class YeelightManager
    {
        private Device lamp;

        public EventHandler<bool> OnConnectionAttemptEnd { get; set; }

        public bool IsConnected { get; private set; }

        public async Task Connect()
        {
            Progress<Device> progressReporter = new Progress<Device>(async device =>
            {
                if (this.lamp == null || this.lamp.Id != device.Id)
                {
                    this.lamp = device;
                    this.IsConnected = await this.lamp.Connect();
                    this.OnConnectionAttemptEnd?.Invoke(this, this.IsConnected);
                }
            });

            await DeviceLocator.DiscoverAsync(progressReporter);
        }

        public async Task SetRgbColor(int r, int g, int b)
        {
            if (this.IsConnected)
            {
                await this.lamp.SetRGBColor(r, g, b);
            }
        }

        public Task SetRgbColor(Color color)
        {
            return this.SetRgbColor(color.R, color.G, color.B);
        }
    }
}
