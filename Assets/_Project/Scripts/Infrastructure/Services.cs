using UnityEngine; 
using System.Collections.Generic; 
public class Services
{
    private static Services _instance;
    public static Services Container => _instance ?? (_instance = new Services());

    public void RegisterSingle<IService>(IService service)  
    {
        RegisterServices<IService>.ServiceInstance = service;
        Debug.Log($"Регистрация {service}");
    }
    public IService Single<IService>()  
    {
        IService service = RegisterServices<IService>.ServiceInstance;
        UnityEngine.Debug.Log(service);
        return service;
    }

    private static class RegisterServices<IService>  
    {
        public static IService ServiceInstance;
    }
}
 
public interface IService
{

}
