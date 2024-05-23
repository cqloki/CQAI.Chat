using MoonshotClient;
using MoonshotDotnet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CQAI.Chat
{
    public static class ChatDemo
    {
        public static async Task<MessagesItem?> Chat(this List<MessagesItem> history, MoonshotClientDemo moonshotClient, string? modelId, string? input)
        {
            var userInput = new MessagesItem
            {
                role = "user",
                content = input
            };
            history.Add(userInput);

            var chatRep = new ChatReq
            {
                max_tokens = 2048,
                temperature = 0.5,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0,
                model = modelId,
                messages = history
            };

            //Console.WriteLine($"{chatRep.messages.Find(s => s.role == "user")?.content}");

            var chatResp = await moonshotClient.Chat(chatRep);
            var chatRespBody = await chatResp.Content.ReadAsStringAsync();
            var chatRespModel = JsonConvert.DeserializeObject<MoonshotRes>(chatRespBody);
            var message = chatRespModel?.choices[0].message;
            history.Add(message);
            return message;
        }
    

        public static async Task Output(this string msg)
        {
            foreach (char c in msg)
            {
                Console.Write(c); // 逐字输出
                Thread.Sleep(50); 
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// 初始化模型角色
        /// </summary>
        /// <param name="history"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task Init(this List<MessagesItem> history, string content = null)
        {
            if (content == null)
            {
                content = "你是 Kimi，由 Moonshot AI 提供的人工智能助手，你更擅长中文对话。你会为用户提供安全，有帮助，准确的回答。";
            }

            history.Add(new MessagesItem
            {
                role = "system",
                content = content
            });
        }

    
    }
}
