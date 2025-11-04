using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Models
{
    public class CameraPlacementModel
    {
        public string CameraNo { get; set; }
        public string LocationDescription { get; set; }
        public string IndoorOutdoor { get; set; }
        public string Type { get; set; }

        public List<string> IndoorOutdoorOptions { get; set; } = new() { "Indoor", "Outdoor" };
        public List<string> TypeOptions { get; set; } = new() { "Dome", "Bullet", "PTZ", "Other" };
    }
}
