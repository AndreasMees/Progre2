using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<Result<List<Vehicle>>> List();
        Task<Result> Save(Vehicle vehicle);
        Task<Result> Delete(int id);
    }
}