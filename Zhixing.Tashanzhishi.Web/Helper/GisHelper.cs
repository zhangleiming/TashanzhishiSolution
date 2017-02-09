using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhixing.Tashanzhishi.Web.Helper
{
    public static class GisHelper
    {

        /// <summary>
        /// 计算经纬度的距离范围
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="distanceRange">距离（单位：千米）</param>
        /// <returns></returns>
        public static SquareRangeModel GetSquareRange(decimal longitude, decimal latitude, decimal distanceRange)
        {
            double range = 180 / Math.PI * (double)distanceRange / 6372.797;
            double lngR = range / Math.Cos((double)latitude * Math.PI / 180.0);

            SquareRangeModel rangeInfo = new SquareRangeModel()
            {
                MinLat = latitude - (decimal)range,
                MaxLat = latitude + (decimal)range,
                MinLng = longitude - (decimal)lngR,
                MaxLng = longitude + (decimal)lngR
            };

            return rangeInfo;
        }

        /// <summary>
        /// 两个经纬度之间的距离(单位：千米)
        /// </summary>
        /// <param name="long1"></param>
        /// <param name="lat1"></param>
        /// <param name="long2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double Distance(double long1, double lat1, double long2, double lat2)
        {
            double a, b, R;
            R = 6378137; //地球半径
            lat1 = lat1 * Math.PI / 180.0;
            lat2 = lat2 * Math.PI / 180.0;
            a = lat1 - lat2;
            b = (long1 - long2) * Math.PI / 180.0;
            double d;
            double sa2, sb2;
            sa2 = Math.Sin(a / 2.0);
            sb2 = Math.Sin(b / 2.0);
            d = 2 * R * Math.Asin(Math.Sqrt(sa2 * sa2 + Math.Cos(lat1) * Math.Cos(lat2) * sb2 * sb2));
            return d / 1000;
        }
    }

    /// <summary>
    /// 经纬度正方形区域模型
    /// </summary>
    public class SquareRangeModel
    {
        /// <summary>
        /// 最小经度
        /// </summary>
        public decimal MinLng { get; set; }

        /// <summary>
        /// 最大经度
        /// </summary>
        public decimal MaxLng { get; set; }

        /// <summary>
        /// 最小纬度
        /// </summary>
        public decimal MinLat { get; set; }

        /// <summary>
        /// 最大纬度
        /// </summary>
        public decimal MaxLat { get; set; }
    }


}
