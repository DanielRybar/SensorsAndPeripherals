using SensorsAndPeripherals.Models;

namespace SensorsAndPeripherals.Interfaces
{
    public interface ISensorListService
    {
        public List<SensorInfo> GetAllSensors();
    }
}