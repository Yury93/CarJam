using UnityEngine; 
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.Services
{ 
    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Container => _instance ?? (_instance = new ServiceLocator());

        public void RegisterSingle<TService>(TService service) where TService : IService
        {
            RegisterServices<TService>.ServiceInstance = service; 
        }
        public TService Single<TService>() where TService : IService
        {
            TService service = RegisterServices<TService>.ServiceInstance; 
            return service;
        }

        private static class RegisterServices<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }

    public interface IService
    {

    }
}