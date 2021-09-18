using System;

namespace CustomerApi.Config
{
    public interface IMqConfig
    {
        
        public string UserName{get;set;}
        public string Password{get;set;}
        public string HostName{get;set;}
    }

    public class MqConfig: IMqConfig
    {
        
        public string UserName{get;set;}
        public string Password{get;set;}
        public string HostName{get;set;}
    }

}