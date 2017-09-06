using System;
using System.IO;
using System.Reflection;

namespace FdvRentalBike.Workflow.Helpers
{
    public class VersionHelper
    {
        private const int Major = 1;
        private const int Minor = 0;
        private const int Revision = 4;
        private const string AppVersionFormat = "{0}.{1}.{2}";
        private const string VersionDateTimeFormat = "yyyy.MM.dd.HHmmss";
        private const int HeaderOffset = 60;
        private const int LinkerTimestampOffset = 8;

        public static string Version { get; private set; }

        public static string BuildVersion { get; private set; }

        static VersionHelper()
        {
            var buildVersion = GetLinkerTime(Assembly.GetExecutingAssembly()).ToString(VersionDateTimeFormat);

            Version = string.Format(AppVersionFormat, Major, Minor, Revision);

            BuildVersion = buildVersion;
        }

        private static DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, HeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
    }
}