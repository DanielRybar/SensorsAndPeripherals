using SensorsAndPeripherals.Models;

namespace SensorsAndPeripherals.Interfaces
{
    public interface ISensorListService
    {
        List<SensorInfo> GetAllSensors();
    }
}