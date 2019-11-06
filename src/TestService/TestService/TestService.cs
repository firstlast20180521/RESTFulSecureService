using System;

namespace TestService
{
    class TestService : ITestService
    {
        public MessageData PostMsg(MessageData msg)
        {
            Console.WriteLine(string.Format(null, "受信 : {0}", msg.ToString()));

            return new MessageData()
            {
                Name = msg.Name,
                Gender = msg.Gender,
                Age = msg.Age + 1
            };
        }

    }
}
