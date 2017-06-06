using System;
using System.Text;

namespace BaiduFanyi
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var api = new TransApi("", "");
            var ret = api.GetTransResult2("Hello c#. I love you.", "en", "zh");
            if (string.IsNullOrEmpty(ret.error_code))
            {
                foreach (var item in ret.trans_result)
                {
                    Console.WriteLine(item.src);
                    Console.WriteLine(item.dst);
                }
            }
            else
            {
                Console.WriteLine(ret.error_msg);
            }
        }
    }
}
