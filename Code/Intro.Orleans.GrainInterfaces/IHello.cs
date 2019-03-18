using System.Threading.Tasks;

namespace Intro.OrleansBasics.GrainInterfaces
{
    public interface IHello : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}
