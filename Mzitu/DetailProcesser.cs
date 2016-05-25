using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Mzitu
{
    public class DetailProcesser
    {
        /// <summary>
        /// 详细页链接
        /// </summary>
        private string _detailUrl;
        /// <summary>
        /// 当前详细页链接变化时触发此事件
        /// </summary>
        public event EventHandler<StringEventArgs> OnUrlChanged;

        public event EventHandler<StringEventArgs> OnSavePathChanged;

        public class StringEventArgs : EventArgs
        {
            public StringEventArgs(string url)
            {
                StringArg = url;
            }

            public string StringArg;
        }

        private string _savaPicPath;
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePicPath
        {
            get { return _savaPicPath; }
            private set
            {
                if (_savaPicPath != value)
                {
                    if (OnSavePathChanged != null)
                    {
                        OnSavePathChanged(this, new StringEventArgs(value));
                    }
                    _savaPicPath = value;
                }
            }
        }


        public string SavePath { get; private set; }
        /// <summary>
        /// 获取详细页链接
        /// </summary>
        public string DetailUrl
        {
            get { return _detailUrl; }
            private set
            {
                if (_detailUrl != value)
                {
                    if (OnUrlChanged != null)
                    {
                        OnUrlChanged(this, new StringEventArgs(value));
                    }
                    _detailUrl = value;
                }

            }
        }

        private bool _stop = false;

        public DetailProcesser(string url, string savePath)
        {
            savePath = savePath.TrimEnd('\\');
            SavePath = savePath;
            DetailUrl = url;
        }

        /// <summary>
        /// 开始抓取
        /// </summary>
        public void Start()
        {
            new Thread(() =>
            {
                var pagehtml = HttpRequestHelper.Get_Data(DetailUrl, null, "utf-8", null);
                DetailUrl = GetNextPageUrl(pagehtml);
                do
                {
                    var picUrl = GetPicUrl(pagehtml);
                    DownloadOnePic(picUrl);
                    pagehtml = HttpRequestHelper.Get_Data(DetailUrl, null, "utf-8", null);
                    DetailUrl = GetNextPageUrl(pagehtml);
                    if (_stop)
                    {
                        _stop = false;
                        break;
                    }

                } while (!string.IsNullOrEmpty(DetailUrl));
            }).Start();

        }

        /// <summary>
        /// 停止抓取
        /// </summary>
        public void Stop()
        {
            _stop = true;
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="picUrl">图片url</param>
        private void DownloadOnePic(string picUrl)
        {
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
            SavePicPath = string.Format("{0}\\{1}", SavePath, picUrl.Replace("http://pic.mmfile.net/", "").Replace("/", "_"));
            var sp = SavePicPath;
            if (File.Exists(SavePicPath))
            {
                SavePicPath += " 已存在";
                return;
            }
            SavePicPath += " 下载中...";
            if (string.IsNullOrEmpty(picUrl))
                return;
            var time = 3;
        GOTO_RETRY:
            var stream = HttpRequestHelper.Get_ImgStream(picUrl, null);
            Image image = null;
            try
            {
                image = Image.FromStream(stream);
            }
            catch (Exception)
            {
                SavePicPath += " 失败";
            }
            if (image != null)
            {
                try
                {
                    image.Save(sp, image.RawFormat);
                    SavePicPath += " 完成";
                }
                catch (Exception)
                {
                    if (time == 0)
                    {
                        SavePicPath += " 失败";
                        return;
                    }
                    SavePicPath += " 重试";
                    time--;
                    goto GOTO_RETRY;
                }
            }
        }

        private string GetPicUrl(string pagehtml)
        {
            var res = "";

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pagehtml);
            var picNode = document.DocumentNode.SelectSingleNode("//div[@class='main-image']//img");
            if (picNode != null)
            {
                res = picNode.GetAttributeValue("src", "");
            }
            return res;
        }

        private string GetNextPageUrl(string html)
        {
            var res = "";

            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var picNode = document.DocumentNode.SelectSingleNode("//div[@class='pagenavi']/a[last()]");
                if (picNode != null)
                {
                    res = picNode.GetAttributeValue("href", "");
                }
            }

            return res;
        }
    }
}
