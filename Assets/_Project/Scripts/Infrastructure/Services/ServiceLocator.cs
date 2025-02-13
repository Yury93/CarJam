using UnityEngine; 
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.Services
{ 
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Container => _instance ?? (_instance = new ServiceLocator());

        public void RegisterSingle<IService>(IService service)
        {
            RegisterServices<IService>.ServiceInstance = service;
            Debug.Log($"Регистрация {service}");
        }
        public IService Single<IService>()
        {
            IService service = RegisterServices<IService>.ServiceInstance; 
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
}