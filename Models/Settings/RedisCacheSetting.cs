﻿namespace LMS.Models.Settings
{
    public sealed class RedisCacheSetting
    {
        public bool Enable { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
    }
}
