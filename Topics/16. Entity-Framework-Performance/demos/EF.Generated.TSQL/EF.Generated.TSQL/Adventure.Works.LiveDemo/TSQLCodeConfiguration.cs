using System.Data.Entity;

namespace Adventure.Works.LiveDemo
{
    public class TSQLCodeConfiguration : DbConfiguration
    {
        public TSQLCodeConfiguration()
        {
            this.AddInterceptor(new TSQLSyntaxLogger());
        }
    }
}
