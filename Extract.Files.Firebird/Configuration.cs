namespace Extract.Files.Firebird
{
    public class Configuration
    {
        public string? Database { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public string? DataSource { get; set; }
        public int Port { get; set; }
        public string? Charset { get; set; }
        public string? Query { get; set; }
        public string? PathOutput { get; set; }
        public string? Extension { get; set; }

    }
}
