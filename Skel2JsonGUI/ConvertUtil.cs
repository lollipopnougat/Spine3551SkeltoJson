using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpineBin2Json35;

namespace Skel2Json
{
    class ConvertUtil
    {

        public static string Skel2Json(string inputFilePath, string outputPath)
        {
            string filePath = inputFilePath.Replace(".skel", "");
            string atlasPath = filePath + ".atlas";
            string skelPath = filePath + ".skel";
            using (var fs = File.OpenRead(atlasPath))
            using (var tr = new StreamReader(fs))
            using (var input = File.OpenRead(skelPath))
            using (var warningOut = File.OpenWrite(outputPath + ".txt"))
            using (var warning = new StreamWriter(warningOut))
            {
                var atlasReader = new AtlasReader(tr);
                var reader = new SpineBin2Json35.BinaryReader(input, atlasReader, warning);
                var obj = reader.Convert();

                using (var output = File.OpenWrite(outputPath))
                using (var sw = new StreamWriter(output)) sw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                string message = "";
                if (input.Length == input.Position)
                {
                    message += string.Format("文件长度: {0}\n读取长度: {1}\n 请您检查输出的txt文件来查看龙骨不支持的特性", input.Length, input.Position);
                }
                else
                {
                    message += message += string.Format("文件长度: {0}\n读取长度: {1}\n 好像哪里不太对劲", input.Length, input.Position);
                }
                return message;

                
            }

        }
    }
}
