namespace Surging.Hero.Auth.IApplication.Authorization.Dtos
{
    public class GetUserMenuOutput
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public bool AlwaysShow { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public string Icon { get; set; }

        public int Level { get; set; }

        public string FullName { get; set; }
    }
}
