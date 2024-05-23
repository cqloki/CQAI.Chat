// See https://aka.ms/new-console-template for more information

using CQAI.Chat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoonshotDotnet;

//以下两个配置必须填写正确

//公开的服务地址,月之暗面：https://api.moonshot.cn
MoonshotClientDemo.Host = "https://api.moonshot.cn";
//您在平台上创建的 API Key
MoonshotClientDemo.ApiKey = "sk-";


var history = new List<MessagesItem>();
var services = new ServiceCollection();
services.AddHttpClient();
var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<MoonshotClientDemo>>();
var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
var moonshotClient = new MoonshotClientDemo(logger, httpClientFactory);

// ListModels 
var modelsResp = await moonshotClient.ListModels();
var modelId = modelsResp.data[0]?.id;
Console.WriteLine($"模型：{modelId} 创建成功\r\n");

await history.Init();
Console.WriteLine($"我是 Kimi～很高兴遇见你！有什么可以帮助你的吗？\r\n");
while (true)
{
    Console.WriteLine("");
    var input = Console.ReadLine();
    Console.WriteLine("");

    MessagesItem? message = await history.Chat(moonshotClient, modelId, input);
    message?.content.Output();

}
