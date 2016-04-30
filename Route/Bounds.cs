using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route
{
    public class Bounds
    {
        private float minLatitude;
        private float maxLatitude;
        private float minLongitude;
        private float maxLongitude;

        public float MinLatitude
        {
            get
            {
                return minLatitude;
            }

            set
            {
                minLatitude = value;
            }
        }

        public float MaxLatitude
        {
            get
            {
                return maxLatitude;
            }

            set
            {
                maxLatitude = value;
            }
        }

        public float MinLongitude
        {
            get
            {
                return minLongitude;
            }

            set
            {
                minLongitude = value;
            }
        }

        public float MaxLongitude
        {
            get
            {
                return maxLongitude;
            }

            set
            {
                maxLongitude = value;
            }
        }

        public override string ToString()
        {
            return string.Format("minLatitude:{0},minLogitude:{1},maxLatitude:{2}, MaxLongitude:{3}", MinLatitude, MinLongitude, MaxLatitude, MaxLongitude);
        }
    }
}
