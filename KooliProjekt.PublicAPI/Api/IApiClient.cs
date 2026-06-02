using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.PublicAPI
{
    public interface IApiClient
    {
        Task<Result<List<Vehicle>>> List();
        Task<Result> Save(Vehicle vehicle);
        Task<Result> Delete(int id);
    }
}