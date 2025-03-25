using Server.Models.Device;

namespace Server.Models.Room
{
    public class RoomModel
    {
        public Guid Id { get; set; }
        public required string RoomCode { get; set; }
        public required string Name { get; set; }
        public required string RoomType { get; set; }
        public required string Status { get; set; }

        //Navigation property
        public required List<DeviceModel> Devices { get; set; }
    }
}