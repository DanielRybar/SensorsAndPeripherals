using Android.Content;
using Android.Hardware;
using SensorsAndPeripherals.Interfaces;
using SensorsAndPeripherals.Models;

namespace SensorsAndPeripherals.Platforms.Android.Services
{
    public class SensorListService : ISensorListService
    {
        public List<SensorInfo> GetAllSensors()
        {
            var sensorList = new List<SensorInfo>();
            var sensorManager = (SensorManager)Platform.AppContext.GetSystemService(Context.SensorService)!;
            if (sensorManager is not null)
            {
                var androidSensors = sensorManager.GetSensorList(SensorType.All);
                if (androidSensors is not null)
                {
                    foreach (var sensor in androidSensors)
                    {
                        sensorList.Add(new SensorInfo
                        {
                            Name = sensor.Name,
                            Vendor = sensor.Vendor,
                            Version = sensor.Version,
                            Power = sensor.Power,
                            StringType = sensor.StringType
                        });
                    }
                }
            }
            return sensorList;
        }
    }
}