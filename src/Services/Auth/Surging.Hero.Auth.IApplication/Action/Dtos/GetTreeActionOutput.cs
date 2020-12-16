using System.Collections.Generic;

namespace Surging.Hero.Auth.IApplication.Action.Dtos
{
    public class GetTreeActionOutput
    {
        public long Value { get; set; }

        public string Label { get; set; }

        public IEnumerable<GetTreeActionOutput> Children { get; set; }
    }
}