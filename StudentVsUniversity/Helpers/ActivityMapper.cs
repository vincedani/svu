using System;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp;
using StudentVsUniversity.Models;

namespace StudentVsUniversity.Helpers
{
    public class ActivityMapper
    {
        public static MappedActivity MapActivityFromDB(Activity activity)
        {
            MappedActivity mapped = new MappedActivity()
            {
                ID = activity.ID,
                CreatedTime = activity.CreatedTime,
                Name = activity.Name,
                WorkTime = activity.WorkTime,
                RestTime = activity.RestTime,
                EllapsedWorkTime = activity.EllapsedWorkTime,
                EllapsedRestTime = activity.EllapsedRestTime,
                EllapsedMinutes = activity.EllapsedMinutes,
                Color =  GetSolidColorBrush(activity.Color).Color
            };
            return mapped;
        }

        public static Activity MapActivityToDB(MappedActivity mActivity)
        {
            Activity activity = new Activity()
            {
                ID = mActivity.ID,
                CreatedTime = mActivity.CreatedTime,
                Name = mActivity.Name,
                WorkTime = mActivity.WorkTime,
                RestTime = mActivity.RestTime,
                EllapsedWorkTime = mActivity.EllapsedWorkTime,
                EllapsedRestTime = mActivity.EllapsedRestTime,
                EllapsedMinutes = mActivity.EllapsedMinutes,
                Color = mActivity.Color.ToHex()
            };
            return activity;
        }

        private static SolidColorBrush GetSolidColorBrush(string hex)
        {
            SolidColorBrush brush;
            try
            {
                hex = hex.Replace("#", string.Empty);
                byte a = (byte) (Convert.ToUInt32(hex.Substring(0, 2), 16));
                byte r = (byte) (Convert.ToUInt32(hex.Substring(2, 2), 16));
                byte g = (byte) (Convert.ToUInt32(hex.Substring(4, 2), 16));
                byte b = (byte) (Convert.ToUInt32(hex.Substring(6, 2), 16));

                brush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            }
            catch (Exception)
            {
                brush = null;
            }

            return brush;
        }
    }
}
