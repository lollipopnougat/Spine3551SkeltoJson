using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Spine;
using System.IO;
using Newtonsoft.Json;

namespace Skel2Json
{
    public partial class Form1 : Form
    {
        private SkeletonData skeletonData;
        private string fileName;
        private string[] fileNameArray;
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenFileBtn_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = "skel",
                Filter = "Spine二进制模型文件|*.skel",
                CheckFileExists = true,
                Title = "选择文件",
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] tmp = ofd.FileName.Replace(".skel", "").Split('\\');
                fileName = ofd.FileName;
                fileNameArray = ofd.FileName.Split('\\');
                if (ofd.FileName.IndexOf(":") < 0) return;
                Label1.Text = $"选择了: {fileName}";
                Label2.Text = "正在加载文件信息";
            }
            else
            {
                MessageBox.Show("未选择文件，退出修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                TextureLoader textureLoader = new Empty();
                Atlas atlas = new Atlas(fileName.Replace(".skel", "") + ".atlas", textureLoader);
                AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(atlas);
                SkeletonBinary skeletonBinary = new SkeletonBinary(attachmentLoader);
                skeletonData = skeletonBinary.ReadSkeletonData(fileName);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Label2.Text = $"模型名: {skeletonData.Name}, \n版本: {skeletonData.Version}, \n宽: {skeletonData.Width}, \n高: {skeletonData.Height}\n骨骼数: {skeletonData.Bones.Count}\n动画数: {skeletonData.Animations.Count}";
            ExportBtn.Enabled = true;
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {

            try
            {
                
                SaveFileDialog sfd = new SaveFileDialog()
                {

                    DefaultExt = "json",
                    Filter = "Spinejson模型文件|*.json",
                    Title = "保存Json文件",
                    InitialDirectory = fileName.Replace(".skel", ""),
                    FileName = fileNameArray[fileNameArray.Length - 1].Replace(".skel", ""),
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FileName.IndexOf(":") < 0) return;
                    string message = ConvertUtil.Skel2Json(fileName, sfd.FileName);
                    Label1.Text = $"导出了: {sfd.FileName}";
                    MessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("未选择文件，退出修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }


        }

        private void AboutBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本程序的诞生离不开 https://github.com/huix-oldcat/SpineBin2Json35 （原项目为GPL V3.0 协议分发授权, 作者为huix-oldcat, B站ID: 老猫不发威你当我病危）\n 以及 Spine-csharp 3.5.51 官方运行库的支持\n程序版本2.2 by lnp(B站ID: 封掣)", "关于");
        }
    }
}
