using k8s;

namespace K8sController
{
    public class Config
    {
        public static Config Instance => _instance ??= new Config();
        private static Config _instance;

        public bool InCluster { get; }
        
        private Config()
        {
            InCluster = DotNetEnv.Env.GetBool("InCluster",true);
        }

        public Kubernetes GetCli()
        {
            var config = Instance.InCluster
                ? KubernetesClientConfiguration.InClusterConfig()
                : new KubernetesClientConfiguration {Host = "http://127.0.0.1:8001"};
            return new Kubernetes(config);
        }
    }
}