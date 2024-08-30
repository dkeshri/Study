using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTConcept.Influxdb.Logic
{
    public class InfluxDbConfigurationOptions
    {
        public const string InfluxDb = "InfluxDb";

        public string InfluxUrl {  get; set; } = "http://localhost:8086";
        public string UserName {  get; set; } = "admin";
        public string Password {  get; set; } = "admin@123";
        public string BucketName { get; set; } = "dkeshri-bucket";
        public string Org {  get; set; } = "DeepakKeshri_org";
        public string Token {  get; set; } = "";
        public int RetentionPeriodDays { get; set; } = 10;

    }
}
