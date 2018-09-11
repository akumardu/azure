using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(Shared.Constants.IotHubConnectionString);
            AddDeviceAsync(Shared.Constants.DeviceId1).Wait();
            AddDeviceAsync(Shared.Constants.DeviceId2).Wait();
            Console.ReadLine();
        }

        private static async Task<string> AddDeviceAsync(string deviceId)
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);

            return device.Authentication.SymmetricKey.PrimaryKey;
        }
    }
}
