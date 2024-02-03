namespace multi.lib.common
{
    public class Tenant
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string DbConnectionString { get; set; }
        public string DbName { get; set; }
    }
}