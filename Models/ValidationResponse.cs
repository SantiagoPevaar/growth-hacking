using EllipticCurve.Utils;
using Newtonsoft.Json;
using System;

namespace CustomEmailSender.Models
{
    public class ValidationResponse
    {
        public Result Result { get; set; }
    }

    public class Result
    {
        public string Email { get; set; }
        public string Verdict { get; set; }
        public double Score { get; set; }
        public string Local { get; set; }
        public string Host { get; set; }
        [JsonProperty("checks")]
        public Checks Checks { get; set; }
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }
    }

    public class Checks
    {
        [JsonProperty("domain")]
        public Domain Domain { get; set; }
        [JsonProperty("local_part")]
        public LocalPart LocalPart { get; set; }
        [JsonProperty("additional")]
        public Additional Additional { get; set; }
    }

    public class Domain
    {
        [JsonProperty("has_valid_address_syntax")]
        public bool HasValidAddressSyntax { get; set; }
        [JsonProperty("has_mx_or_a_record")]
        public bool HasMxOrARecord { get; set; }
        [JsonProperty("is_suspected_disposable_address")]
        public bool IsSuspectedDisposableAddress { get; set; }
    }

    public class LocalPart
    {
        [JsonProperty("is_suspected_role_address")]
        public bool IsSuspectedRoleAddress { get; set; }
    }

    public class Additional
    {
        [JsonProperty("has_known_bounces")]
        public bool HasKnownBounces { get; set; }
        [JsonProperty("has_suspected_bounces")]
        public bool HasSuspectedBounces { get; set; }
    }

}
